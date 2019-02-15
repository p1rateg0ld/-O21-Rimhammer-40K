using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace Rimhammer40k
{
    [DefOf]
    public static class RCDefsOf
    {
        public static JobDef UseRequisitionRelay;

        //public static SoundDef FreeReinforcementsOneShotRH;

        //public static SoundDef MediumReinforcementsOneShotRH;

        //public static SoundDef StrongReinforcementsOneShotRH;

        //public static SoundDef AmbientRequisitionRelayRH;

        public static SoundDef PickUpRequisitionRelayRH;
    }

    public class Building_RequisitionRelay : Building
    {
        private CompPowerTrader powerComp;

        public bool CanUseCommsNow
        {
            get
            {
                if (base.Spawned && base.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.SolarFlare))
                {
                    return false;
                }
                return powerComp.PowerOn;
            }
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            powerComp = base.GetComp<CompPowerTrader>();
            LessonAutoActivator.TeachOpportunity(ConceptDefOf.OpeningComms, OpportunityType.GoodToKnow);
        }

        private FloatMenuOption GetFailureReason(Pawn myPawn)
        {
            FloatMenuOption result;
            if (!myPawn.CanReach(this, PathEndMode.InteractionCell, Danger.Some, false, TraverseMode.ByPawn))
            {
                result = new FloatMenuOption("CannotUseNoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
            }
            else if (base.Spawned && base.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.SolarFlare))
            {
                result = new FloatMenuOption("CannotUseSolarFlare".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
            }
            else if (!this.powerComp.PowerOn)
            {
                result = new FloatMenuOption("CannotUseNoPower".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
            }
            else if (!myPawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking))
            {
                result = new FloatMenuOption("CannotUseReason".Translate("IncapableOfCapacity".Translate(PawnCapacityDefOf.Talking.label)), null, MenuOptionPriority.Default, null, null, 0f, null, null);
            }
            else if (myPawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled)
            {
                result = new FloatMenuOption("CannotPrioritizeWorkTypeDisabled".Translate(SkillDefOf.Social.LabelCap), null, MenuOptionPriority.Default, null, null, 0f, null, null);
            }
            else if (!this.CanUseCommsNow)
            {
                Log.Error(myPawn + " could not use comm console for unknown reason.", false);
                result = new FloatMenuOption("Cannot use now", null, MenuOptionPriority.Default, null, null, 0f, null, null);
            }
            else
            {
                result = null;
            }
            return result;
        }

        public IEnumerable<Faction> GetCommTargets(Pawn myPawn)
        {
            IEnumerable<Faction> PreselectList = (from f in Find.FactionManager.AllFactionsVisibleInViewOrder
                                                   where !f.IsPlayer
                                                   select f);
            IEnumerable<Faction> commTargetList = (from f in PreselectList
                                                   where (f.PlayerRelationKind == FactionRelationKind.Neutral && f.PlayerGoodwill > 0) || f.PlayerRelationKind == FactionRelationKind.Ally && !f.IsPlayer
                                                   select f);
            return commTargetList;
        }

        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
        {
            FloatMenuOption failureReason = this.GetFailureReason(myPawn);
            if (failureReason != null)
            {
                yield return failureReason;
            }
            else
            {
                IEnumerable<Faction> factionsToCall = this.GetCommTargets(myPawn);
                if(factionsToCall == null)
                {
                    yield return new FloatMenuOption("NoFriendlyFactionRH".Translate(), null);
                }
                else if(factionsToCall.Count() < 1)
                {
                    yield return new FloatMenuOption("NoFriendlyFactionRH".Translate(), null);
                }
                foreach (Faction commTarget in factionsToCall)
                {
                    FloatMenuOption option = CommFloatMenuOption(commTarget, myPawn);
                    if (option != null)
                    {
                        yield return option;
                    }
                }
            }
        }

        public FloatMenuOption CommFloatMenuOption(Faction commTarget, Pawn negotiator)
        {
            FloatMenuOption result;
            if (commTarget.IsPlayer)
            {
                result = null;
            }
            else
            {
                string text = "CallOnRadio".Translate(commTarget.GetCallLabel());
                string text2 = text;
                text = string.Concat(new string[]
                {
                    text2,
                    " (",
                    commTarget.PlayerRelationKind.GetLabel(),
                    ", ",
                    commTarget.PlayerGoodwill.ToStringWithSign(),
                    ")"
                });
                if (!this.LeaderIsAvailableToTalk(commTarget))
                {
                    string str;
                    if (commTarget.leader != null)
                    {
                        str = "LeaderUnavailable".Translate(commTarget.leader.LabelShort, commTarget.leader);
                    }
                    else
                    {
                        str = "LeaderUnavailableNoLeader".Translate();
                    }
                    result = new FloatMenuOption(text + " (" + str + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
                }
                else
                {
                    result = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, delegate ()
                    {
                        this.GiveUseCommsJob(negotiator, commTarget);
                    }, MenuOptionPriority.InitiateSocial, null, null, 0f, null, null), negotiator, this, "ReservedBy");
                }
            }
            return result;
        }

        public bool LeaderIsAvailableToTalk(Faction faction)
        {
            bool result;
            if (faction.leader == null)
            {
                result = false;
            }
            else
            {
                if (faction.leader.Spawned)
                {
                    if (faction.leader.Downed || faction.leader.IsPrisoner || !faction.leader.Awake() || faction.leader.InMentalState)
                    {
                        return false;
                    }
                }
                result = true;
            }
            return result;
        }

        public void GiveUseCommsJob(Pawn negotiator, Faction target)
        {
            Job job = new Job(RCDefsOf.UseRequisitionRelay, this)
            {
                commTarget = target
            };
            negotiator.jobs.TryTakeOrderedJob(job, JobTag.Misc);
            PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.OpeningComms, KnowledgeAmount.Total);
        }

        /** public IEnumerable<IntVec3> TradeableCells
        {
            get
            {
                return Building_OrbitalTradeBeacon.TradeableCellsAround(base.Position, base.Map);
            }
        }
        
        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo g in base.GetGizmos())
            {
                yield return g;
            }
            if (DesignatorUtility.FindAllowedDesignator<Designator_ZoneAddStockpile_Resources>() != null)
            {
                yield return new Command_Action
                {
                    action = new Action(this.MakeMatchingStockpile),
                    hotKey = KeyBindingDefOf.Misc1,
                    defaultDesc = "CommandMakeBeaconStockpileDesc".Translate(),
                    icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Stockpile", true),
                    defaultLabel = "CommandMakeBeaconStockpileLabel".Translate()
                };
            }
            yield break;
        }
        
        private void MakeMatchingStockpile()
        {
            Designator des = DesignatorUtility.FindAllowedDesignator<Designator_ZoneAddStockpile_Resources>();
            des.DesignateMultiCell(from c in this.TradeableCells
                                   where des.CanDesignateCell(c).Accepted
                                   select c);
        }
        
        public static List<IntVec3> TradeableCellsAround(IntVec3 pos, Map map)
        {
            Building_RequisitionRelay.tradeableCells.Clear();
            if (!pos.InBounds(map))
            {
                return Building_RequisitionRelay.tradeableCells;
            }
            Region region = pos.GetRegion(map, RegionType.Set_Passable);
            if (region == null)
            {
                return Building_RequisitionRelay.tradeableCells;
            }
            RegionTraverser.BreadthFirstTraverse(region, (Region from, Region r) => r.door == null, delegate (Region r)
            {
                foreach (IntVec3 item in r.Cells)
                {
                    if (item.InHorDistOf(pos, 7.9f))
                    {
                        Building_RequisitionRelay.tradeableCells.Add(item);
                    }
                }
                return false;
            }, 13, RegionType.Set_Passable);
            return Building_RequisitionRelay.tradeableCells;
        }
        
        public static IEnumerable<Building_RequisitionRelay> AllPowered(Map map)
        {
            foreach (Building_RequisitionRelay b in map.listerBuildings.AllBuildingsColonistOfClass<Building_RequisitionRelay>())
            {
                CompPowerTrader power = b.GetComp<CompPowerTrader>();
                if (power == null || power.PowerOn)
                {
                    yield return b;
                }
            }
            yield break;
        }
        
        private const float TradeRadius = 7.9f;
        
        private static List<IntVec3> tradeableCells = new List<IntVec3>(); **/
    }



    public class JobDriver_UseRequisitionRelay : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            Pawn pawn = this.pawn;
            LocalTargetInfo targetA = this.job.targetA;
            Job job = this.job;
            return pawn.Reserve(targetA, job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedOrNull(TargetIndex.A);
            yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.InteractionCell).FailOn(delegate (Toil to)
            {
                Building_RequisitionRelay building_CommsConsole = (Building_RequisitionRelay)to.actor.jobs.curJob.GetTarget(TargetIndex.A).Thing;
                return !building_CommsConsole.CanUseCommsNow;
            });
            Toil openComms = new Toil();
            openComms.initAction = delegate ()
            {
                Pawn actor = openComms.actor;
                Building_RequisitionRelay building_CommsConsole = (Building_RequisitionRelay)actor.jobs.curJob.GetTarget(TargetIndex.A).Thing;
                if (building_CommsConsole.CanUseCommsNow)
                {
                    OpenCommsWith(actor, actor.jobs.curJob.commTarget as Faction);
                }
            };
            yield return openComms;
            yield break;
        }

        public void OpenCommsWith(Pawn negotiator, Faction faction)
        {
            Dialog_Negotiation dialog_Negotiation = new Dialog_Negotiation(negotiator, faction, ReinforcementDialogMakerRC.FactionDialogFor(negotiator, faction), true);
            //dialog_Negotiation.soundAmbient = RCDefsOf.AmbientRequisitionRelayRH;
            SoundStarter.PlayOneShot(RCDefsOf.PickUpRequisitionRelayRH, SoundInfo.OnCamera());
            Find.WindowStack.Add(dialog_Negotiation);
        }
    }

    public static class ReinforcementDialogMakerRC
    {
        public static DiaNode FactionDialogFor(Pawn negotiator, Faction faction)
        {
            Map map = negotiator.Map;
            Pawn p;
            string text;
            if (faction.leader != null)
            {
                p = faction.leader;
                text = faction.leader.Name.ToStringFull;
            }
            else
            {
                Log.Error("Faction " + faction + " has no leader.", false);
                p = negotiator;
                text = faction.Name;
            }
            DiaNode diaNode;
            if (faction.PlayerRelationKind == FactionRelationKind.Hostile)
            {
                string key;
                if (!faction.def.permanentEnemy && "FactionGreetingHostileAppreciative".CanTranslate())
                {
                    key = "FactionGreetingHostileAppreciative";
                }
                else
                {
                    key = "FactionGreetingHostile";
                }
                diaNode = new DiaNode(key.Translate(text).AdjustedFor(p, "PAWN"));
            }
            else if (faction.PlayerRelationKind == FactionRelationKind.Neutral)
            {
                diaNode = new DiaNode("FactionGreetingWary".Translate(text, negotiator.LabelShort, negotiator.Named("NEGOTIATOR"), p.Named("LEADER")).AdjustedFor(p, "PAWN"));
            }
            else
            {
                diaNode = new DiaNode("FactionGreetingWarm".Translate(text, negotiator.LabelShort, negotiator.Named("NEGOTIATOR"), p.Named("LEADER")).AdjustedFor(p, "PAWN"));
            }
            if (map != null && map.IsPlayerHome)
            {
                diaNode.options.Add(ReinforcementDialogMakerRC.RequestFreeReinforcements(map, faction, negotiator));
                //If the faction is of a tech level that is industrial or higher...
                if (!(faction.def.techLevel < TechLevel.Spacer))
                {
                    diaNode.options.Add(ReinforcementDialogMakerRC.RequestQuickReactionReinforcements(map, faction, negotiator));
                    diaNode.options.Add(ReinforcementDialogMakerRC.RequestStrongQuickReactionReinforcements(map, faction, negotiator));
                }
            }
            DiaOption diaOption = new DiaOption("(" + "Disconnect".Translate() + ")")
            {
                resolveTree = true
            };
            diaNode.options.Add(diaOption);
            return diaNode;
        }

        public static DiaOption RequestFreeReinforcements(Map map, Faction faction, Pawn negotiator)
        {
            string text = "RequestFreeMilitaryAidRH".Translate();
            DiaOption result;
            if (faction.PlayerRelationKind != FactionRelationKind.Ally)
            {
                DiaOption diaOption = new DiaOption(text);
                diaOption.Disable("MustBeAlly".Translate());
                result = diaOption;
            }
            else if (!faction.def.allowedArrivalTemperatureRange.ExpandedBy(-4f).Includes(map.mapTemperature.SeasonalTemp))
            {
                DiaOption diaOption2 = new DiaOption(text);
                diaOption2.Disable("BadTemperature".Translate());
                result = diaOption2;
            }
            else
            {
                int num = faction.lastMilitaryAidRequestTick + 30000 - Find.TickManager.TicksGame;
                if (num > 0)
                {
                    DiaOption diaOption3 = new DiaOption(text);
                    diaOption3.Disable("WaitTime".Translate(num.ToStringTicksToPeriod()));
                    result = diaOption3;
                }
                else if (NeutralGroupIncidentUtility.AnyBlockingHostileLord(map, faction))
                {
                    DiaOption diaOption4 = new DiaOption(text);
                    diaOption4.Disable("HostileVisitorsPresent".Translate());
                    result = diaOption4;
                }
                else
                {
                    DiaOption diaOption5 = new DiaOption(text);
                    IEnumerable<Faction> source = (from x in map.attackTargetsCache.TargetsHostileToColony
                                                   where GenHostility.IsActiveThreatToPlayer(x)
                                                   select ((Thing)x).Faction into x
                                                   where x != null && !x.HostileTo(faction)
                                                   select x).Distinct<Faction>();
                    if (source.Any<Faction>())
                    {
                        DiaNode diaNode = new DiaNode("MilitaryAidConfirmMutualEnemy".Translate(faction.Name, (from fa in source
                                                                                                               select fa.Name).ToCommaList(true)));
                        DiaOption diaOption6 = new DiaOption("CallConfirm".Translate())
                        {
                            action = delegate ()
                            {
                                ReinforcementDialogMakerRC.CallForSmallAid(map, faction);
                            },
                            resolveTree = true
                        };
                        DiaOption diaOption7 = new DiaOption("CallCancel".Translate())
                        {
                            linkLateBind = ReinforcementDialogMakerRC.ResetToRoot(faction, negotiator)
                        };
                        diaNode.options.Add(diaOption6);
                        diaNode.options.Add(diaOption7);
                        diaOption5.link = diaNode;
                    }
                    else
                    {
                        diaOption5.action = delegate ()
                        {
                            ReinforcementDialogMakerRC.CallForSmallAid(map, faction);
                        };
                        diaOption5.resolveTree = true;
                    }
                    result = diaOption5;
                }
            }
            return result;
        }

        public static DiaOption RequestQuickReactionReinforcements(Map map, Faction faction, Pawn negotiator)
        {
            int silverToPay = 500;
            int goodWillNeeded = 25;
            string text = "RequestPaidMilitaryAidRH".Translate(silverToPay);
            DiaOption result;
            if (faction.PlayerRelationKind != FactionRelationKind.Ally && faction.PlayerGoodwill < goodWillNeeded)
            {
                DiaOption diaOption = new DiaOption(text);
                diaOption.Disable("NeedGoodwill".Translate(goodWillNeeded.ToString("F0")));
                result = diaOption;
            }
            else if (ReinforcementDialogMakerRC.AmountSendableSilver(map) < silverToPay)
            {
                DiaOption diaOption3 = new DiaOption(text);
                diaOption3.Disable("NeedSilverLaunchable".Translate(silverToPay.ToString()));
                result = diaOption3;
            }
            else if (!faction.def.allowedArrivalTemperatureRange.ExpandedBy(-4f).Includes(map.mapTemperature.SeasonalTemp))
            {
                DiaOption diaOption2 = new DiaOption(text);
                diaOption2.Disable("BadTemperature".Translate());
                result = diaOption2;
            }
            else
            {
                int num = faction.lastMilitaryAidRequestTick + 60000 - Find.TickManager.TicksGame;
                if (num > 0)
                {
                    DiaOption diaOption3 = new DiaOption(text);
                    diaOption3.Disable("WaitTime".Translate(num.ToStringTicksToPeriod()));
                    result = diaOption3;
                }
                else if (NeutralGroupIncidentUtility.AnyBlockingHostileLord(map, faction))
                {
                    DiaOption diaOption4 = new DiaOption(text);
                    diaOption4.Disable("HostileVisitorsPresent".Translate());
                    result = diaOption4;
                }
                else
                {
                    DiaOption diaOption5 = new DiaOption(text);
                    IEnumerable<Faction> source = (from x in map.attackTargetsCache.TargetsHostileToColony
                                                   where GenHostility.IsActiveThreatToPlayer(x)
                                                   select ((Thing)x).Faction into x
                                                   where x != null && !x.HostileTo(faction)
                                                   select x).Distinct<Faction>();
                    if (source.Any<Faction>())
                    {
                        DiaNode diaNode = new DiaNode("MilitaryAidConfirmMutualEnemy".Translate(faction.Name, (from fa in source
                                                                                                               select fa.Name).ToCommaList(true)));
                        DiaOption diaOption6 = new DiaOption("CallConfirm".Translate())
                        {
                            action = delegate ()
                            {
                                ReinforcementDialogMakerRC.CallForMediumAid(map, faction);
                            },
                            resolveTree = true
                        };
                        DiaOption diaOption7 = new DiaOption("CallCancel".Translate())
                        {
                            linkLateBind = ReinforcementDialogMakerRC.ResetToRoot(faction, negotiator)
                        };
                        diaNode.options.Add(diaOption6);
                        diaNode.options.Add(diaOption7);
                        diaOption5.link = diaNode;
                    }
                    else
                    {
                        diaOption5.action = delegate ()
                        {
                            TradeUtility.LaunchThingsOfType(ThingDefOf.Silver, silverToPay, map, null);
                            ReinforcementDialogMakerRC.CallForMediumAid(map, faction);
                        };
                        diaOption5.resolveTree = true;
                    }
                    result = diaOption5;
                }
            }
            return result;
        }

        public static DiaOption RequestStrongQuickReactionReinforcements(Map map, Faction faction, Pawn negotiator)
        {
            int silverToPay = 900;
            int goodWillNeeded = 25;
            string text = "RequestHeavyPaidMilitaryAidRH".Translate(silverToPay);
            DiaOption result;
            if (faction.PlayerRelationKind != FactionRelationKind.Ally && faction.PlayerGoodwill < goodWillNeeded)
            {
                DiaOption diaOption = new DiaOption(text);
                diaOption.Disable("NeedGoodwill".Translate(goodWillNeeded.ToString("F0")));
                result = diaOption;
            }
            else if (ReinforcementDialogMakerRC.AmountSendableSilver(map) < silverToPay)
            {
                DiaOption diaOption3 = new DiaOption(text);
                diaOption3.Disable("NeedSilverLaunchable".Translate(silverToPay.ToString()));
                result = diaOption3;
            }
            else if (!faction.def.allowedArrivalTemperatureRange.ExpandedBy(-4f).Includes(map.mapTemperature.SeasonalTemp))
            {
                DiaOption diaOption2 = new DiaOption(text);
                diaOption2.Disable("BadTemperature".Translate());
                result = diaOption2;
            }
            else
            {
                int num = faction.lastMilitaryAidRequestTick + 90000 - Find.TickManager.TicksGame;
                if (num > 0)
                {
                    DiaOption diaOption3 = new DiaOption(text);
                    diaOption3.Disable("WaitTime".Translate(num.ToStringTicksToPeriod()));
                    result = diaOption3;
                }
                else if (NeutralGroupIncidentUtility.AnyBlockingHostileLord(map, faction))
                {
                    DiaOption diaOption4 = new DiaOption(text);
                    diaOption4.Disable("HostileVisitorsPresent".Translate());
                    result = diaOption4;
                }
                else
                {
                    DiaOption diaOption5 = new DiaOption(text);
                    IEnumerable<Faction> source = (from x in map.attackTargetsCache.TargetsHostileToColony
                                                   where GenHostility.IsActiveThreatToPlayer(x)
                                                   select ((Thing)x).Faction into x
                                                   where x != null && !x.HostileTo(faction)
                                                   select x).Distinct<Faction>();
                    if (source.Any<Faction>())
                    {
                        DiaNode diaNode = new DiaNode("MilitaryAidConfirmMutualEnemy".Translate(faction.Name, (from fa in source
                                                                                                               select fa.Name).ToCommaList(true)));
                        DiaOption diaOption6 = new DiaOption("CallConfirm".Translate())
                        {
                            action = delegate ()
                            {
                                ReinforcementDialogMakerRC.CallForStrongAid(map, faction);
                            },
                            resolveTree = true
                        };
                        DiaOption diaOption7 = new DiaOption("CallCancel".Translate())
                        {
                            linkLateBind = ReinforcementDialogMakerRC.ResetToRoot(faction, negotiator)
                        };
                        diaNode.options.Add(diaOption6);
                        diaNode.options.Add(diaOption7);
                        diaOption5.link = diaNode;
                    }
                    else
                    {
                        diaOption5.action = delegate ()
                        {
                            TradeUtility.LaunchThingsOfType(ThingDefOf.Silver, silverToPay, map, null);
                            ReinforcementDialogMakerRC.CallForStrongAid(map, faction);
                        };
                        diaOption5.resolveTree = true;
                    }
                    result = diaOption5;
                }
            }
            return result;
        }

        private static int AmountSendableSilver(Map map)
        {
            return (from t in TradeUtility.AllLaunchableThingsForTrade(map)
                    where t.def == ThingDefOf.Silver
                    select t).Sum((Thing t) => t.stackCount);
        }

        private static void CallForSmallAid(Map map, Faction faction)
        {
            IncidentParms incidentParms = new IncidentParms
            {
                target = map,
                faction = faction,
                points = Rand.RangeInclusive(400, 800)
            };
            faction.lastMilitaryAidRequestTick = Find.TickManager.TicksGame;
            IncidentDefOf.RaidFriendly.Worker.TryExecute(incidentParms);
            //SoundStarter.PlayOneShotOnCamera(RCDefsOf.FreeReinforcementsOneShotRH);
        }

        private static void CallForMediumAid(Map map, Faction faction)
        {
            IncidentParms incidentParms = new IncidentParms
            {
                target = map,
                faction = faction,
                points = Rand.RangeInclusive(800, 3500)
            };
            //incidentParms.raidArrivalMode = PawnsArrivalModeDefOf.CenterDrop;
            faction.lastMilitaryAidRequestTick = Find.TickManager.TicksGame;
            IncidentDefOf.RaidFriendly.Worker.TryExecute(incidentParms);
            //SoundStarter.PlayOneShotOnCamera(RCDefsOf.MediumReinforcementsOneShotRH);
        }

        private static void CallForStrongAid(Map map, Faction faction)
        {
            IncidentParms incidentParms = new IncidentParms
            {
                target = map,
                faction = faction,
                points = Rand.RangeInclusive(3500, 5000)
            };
            //incidentParms.raidArrivalMode = PawnsArrivalModeDefOf.CenterDrop;
            faction.lastMilitaryAidRequestTick = Find.TickManager.TicksGame;
            IncidentDefOf.RaidFriendly.Worker.TryExecute(incidentParms);
            //SoundStarter.PlayOneShotOnCamera(RCDefsOf.StrongReinforcementsOneShotRH);
        }

        private static Func<DiaNode> ResetToRoot(Faction faction, Pawn negotiator)
        {
            return () => ReinforcementDialogMakerRC.FactionDialogFor(negotiator, faction);
        }

    }


}
