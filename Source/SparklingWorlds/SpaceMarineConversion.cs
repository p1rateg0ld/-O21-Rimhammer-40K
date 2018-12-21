using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;
using System.Diagnostics;
using RimWorld.Planet;
using RimWorld.BaseGen;
using Verse.AI;
using Verse.Sound;

namespace Rimhammer40k
{
    [DefOf]
    public static class RimhamDefOfRH
    {
        public static JobDef EnterSMCDRMH;

        public static ThingDef SpacemarineConversionDeviceRH;

        public static SoundDef AstartesOvenDone;

        //replace this with the spacemarie pawnkinddef you want to use to assign correct race
        public static PawnKindDef Astartes;
        public static PawnKindDef Terran;
        public static PawnKindDef Colonist;
        public static PawnKindDef Valhallan;
        public static PawnKindDef Krieg;
        public static PawnKindDef Guevesa;
    }

    public class SpacemarineConversionDeviceRH : Building_Casket
    {
        protected int IcookingTicking = 0;

        protected int IcookingTime = 12000; //60k is one day

        public CompPowerTrader powerComp;

        public CompGlower Glower;

        private ColorInt red = new ColorInt(252, 187, 113, 0);

        private ColorInt green = new ColorInt(100, 255, 100, 0);

        public ColorInt CurrentColour;

        public bool PowerOn => powerComp.PowerOn;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            Glower = this.GetComp<CompGlower>();
            powerComp = this.GetComp<CompPowerTrader>();
            ChangeColour(green);
            CurrentColour = green;
        }

        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
        {
            if (!powerComp.PowerOn)
            {
                FloatMenuOption item = new FloatMenuOption("CannotUseNoPower".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
                return new List<FloatMenuOption>
            {
                item
            };
            }
            if (!ReservationUtility.CanReserve(myPawn, this, 1))
            {
                FloatMenuOption item2 = new FloatMenuOption(Translator.Translate("CannotUseReserved"), (Action)null, MenuOptionPriority.Default, (Action)null, null, 0f, (Func<Rect, bool>)null, null);
                return new List<FloatMenuOption>
            {
                item2
            };
            }
            if (!ReachabilityUtility.CanReach(myPawn, this, PathEndMode.OnCell, Danger.Some, false))
            {
                FloatMenuOption item3 = new FloatMenuOption(Translator.Translate("CannotUseNoPath"), (Action)null, MenuOptionPriority.Default, (Action)null, null, 0f, (Func<Rect, bool>)null, null);
                return new List<FloatMenuOption>
            {
                item3
            };
            }
            if (!(Gender.Male == myPawn.gender))
            {
                FloatMenuOption item4 = new FloatMenuOption("CannotUseNotMaleRH".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
                return new List<FloatMenuOption>
            {
                item4
            };
            }
            if ((myPawn.kindDef.race != RimhamDefOfRH.Colonist.race) && (myPawn.kindDef.race != RimhamDefOfRH.Terran.race) && (myPawn.kindDef.race != RimhamDefOfRH.Valhallan.race) && (myPawn.kindDef.race != RimhamDefOfRH.Krieg.race) && (myPawn.kindDef.race != RimhamDefOfRH.Guevesa.race))
            {
                FloatMenuOption item5 = new FloatMenuOption("CannotUseNotHumanRH".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
                return new List<FloatMenuOption>
                    {
                        item5
                    };
            }
            if (!this.HasAnyContents)
            {
                FloatMenuOption item6 = new FloatMenuOption(Translator.Translate("EnterSMCDRMH"), (Action)delegate
                {
                    Job val2 = new Job(RimhamDefOfRH.EnterSMCDRMH, this);
                    ReservationUtility.Reserve(myPawn, this, val2);
                    myPawn.jobs.TryTakeOrderedJob(val2);
                }, MenuOptionPriority.Default, (Action)null, null, 0f, (Func<Rect, bool>)null, null);
                return new List<FloatMenuOption>
            {
                item6
            };
            }
            return null;
        }

        public void CookIt()
        {
            ThingOwner convertedContainer = new ThingOwner<Thing>();
            foreach (Thing item in this.innerContainer)
            {
                if (item is Pawn val)
                {
                    convertedContainer.TryAdd(ConvertToSpaceMarine(val));
                }
            }
            this.innerContainer.ClearAndDestroyContents();
            this.innerContainer = convertedContainer;
        }

        protected Pawn ConvertToSpaceMarine(Pawn pawnToConvert)
        {

            PawnGenerationRequest request = new PawnGenerationRequest(
                RimhamDefOfRH.Astartes,
                faction: Faction.OfPlayer,
                forceGenerateNewPawn: true,
                canGeneratePawnRelations: false,
                colonistRelationChanceFactor: 0f,
                fixedBiologicalAge: pawnToConvert.ageTracker.AgeBiologicalYearsFloat,
                fixedChronologicalAge: pawnToConvert.ageTracker.AgeChronologicalYearsFloat,
                fixedGender: pawnToConvert.gender,
                allowFood: false);
            Pawn pawn = PawnGenerator.GeneratePawn(request);

            //No pregenerated equipment.
            pawn?.equipment?.DestroyAllEquipment();
            pawn?.apparel?.DestroyAll();
            pawn?.inventory?.DestroyAll();

            //Transfer everything from old pawn to new pawn
            pawn.drugs = pawnToConvert.drugs;
            pawn.foodRestriction = pawnToConvert.foodRestriction;
            //pawn.guilt = pawnToConvert.guilt;
            //pawn.health = pawnToConvert.health;
            pawn.health.hediffSet = pawnToConvert.health.hediffSet;
            //pawn.needs = pawnToConvert.needs;
            pawn.records = pawnToConvert.records;
            pawn.skills = pawnToConvert.skills;
            pawn.story = pawnToConvert.story;
            pawn.timetable = pawnToConvert.timetable;
            pawn.workSettings = pawnToConvert.workSettings;
            pawn.Name = pawnToConvert.Name;

            //Change body to hulk
            pawn.story.bodyType = BodyTypeDefOf.Hulk;
            pawn.Drawer.renderer.graphics.ResolveAllGraphics();

            return pawn;
        }

        public override void Tick()
        {
            if (powerComp.PowerOn)
            {
                if (this.HasAnyContents)
                {
                    //Not the best way but this just ticks up until max is reached.
                    //It would be better to check the "endtime" with the Tickfinder
                    IcookingTicking++;
                    if (CurrentColour != red)
                    {
                        ChangeColour(red);
                    }
                    if (IcookingTicking >= IcookingTime)
                    {
                        CookIt();
                        this.EjectContents();
                        IcookingTicking = 0;
                    }
                }
                else if (CurrentColour != green)
                {
                    ChangeColour(green);
                }
            }
            else
            {
                if (CurrentColour != red)
                {
                    ChangeColour(red);
                }
                IcookingTicking = 0;
                if (this.HasAnyContents)
                {
                    this.EjectContents();
                }
            }
        }

        public override void EjectContents()
        {
            if (!this.Destroyed)
            {
                SoundStarter.PlayOneShot(RimhamDefOfRH.AstartesOvenDone, SoundInfo.OnCamera());
            }
            IcookingTicking = 0;
            this.innerContainer.TryDropAll(this.InteractionCell, base.Map, ThingPlaceMode.Near);
            this.contentsKnown = true;
        }

        public override void Draw()
        {
            base.Draw();
            DrawCookingBar();
        }

        public void DrawCookingBar()
        {
            //Replaced Drawhelper with vanilla drawer here
            GenDraw.FillableBarRequest fillableBarRequest = default(GenDraw.FillableBarRequest);
            fillableBarRequest.size = new Vector2(0.55f, 0.16f);
            fillableBarRequest.fillPercent = (float)IcookingTicking / (float)IcookingTime;
            fillableBarRequest.filledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.9f, 0.9f, 0.10f));
            fillableBarRequest.unfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.6f, 0.6f, 0.6f));
            Rot4 rotation = this.Rotation;
            rotation.Rotate(RotationDirection.Clockwise);
            fillableBarRequest.rotation = rotation;
            if (fillableBarRequest.rotation == Rot4.West)
            {
                fillableBarRequest.rotation = Rot4.West;
                fillableBarRequest.center = this.DrawPos + Vector3.up * 0.1f + Vector3.back * 0.45f;
            }
            if (fillableBarRequest.rotation == Rot4.North)
            {
                fillableBarRequest.rotation = Rot4.North;
                fillableBarRequest.center = this.DrawPos + Vector3.up * 0.1f + Vector3.left * 0.45f;
            }
            if (fillableBarRequest.rotation == Rot4.East)
            {
                fillableBarRequest.rotation = Rot4.East;
                fillableBarRequest.center = this.DrawPos + Vector3.up * 0.1f + Vector3.forward * 0.45f;
            }
            if (fillableBarRequest.rotation == Rot4.South)
            {
                fillableBarRequest.rotation = Rot4.South;
                fillableBarRequest.center = this.DrawPos + Vector3.up * 0.1f + Vector3.right * 0.45f;
            }
            GenDraw.DrawFillableBar(fillableBarRequest);
        }

        public void ChangeColour(ColorInt colour)
        {

            CurrentColour = colour;

            if (Glower != null)
            {
                float newGlowRadius = Glower.Props.glowRadius;
                this.Map.glowGrid.DeRegisterGlower(Glower);
                Glower.Initialize(new CompProperties_Glower
                {
                    compClass = typeof(CompGlower),
                    glowColor = colour,
                    glowRadius = newGlowRadius
                });
                this.Map.mapDrawer.MapMeshDirty(this.Position, MapMeshFlag.Things);
                this.Map.glowGrid.RegisterGlower(Glower);
            }
        }

        public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
        {
            if (base.TryAcceptThing(thing, allowSpecialEffects))
            {
                if (allowSpecialEffects)
                {
                    SoundDef.Named("CryptosleepCasketAccept").PlayOneShot(new TargetInfo(base.Position, base.Map, false));
                }
                return true;
            }
            return false;
        }

        public static SpacemarineConversionDeviceRH FindCryptosleepCasketFor(Pawn p, Pawn traveler, bool ignoreOtherReservations = false)
        {
            IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
                                               where typeof(SpacemarineConversionDeviceRH).IsAssignableFrom(def.thingClass)
                                               select def;
            foreach (ThingDef current in enumerable)
            {
                SpacemarineConversionDeviceRH building_SMCDRH = (SpacemarineConversionDeviceRH)GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForDef(current), PathEndMode.InteractionCell, TraverseParms.For(traveler, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, delegate (Thing x)
                {
                    bool arg_33_0;
                    if (!((SpacemarineConversionDeviceRH)x).HasAnyContents)
                    {
                        Pawn traveler2 = traveler;
                        LocalTargetInfo target = x;
                        bool ignoreOtherReservations2 = ignoreOtherReservations;
                        arg_33_0 = traveler2.CanReserve(target, 1, -1, null, ignoreOtherReservations2);
                    }
                    else
                    {
                        arg_33_0 = false;
                    }
                    return arg_33_0;
                }, null, 0, -1, false, RegionType.Set_Passable, false);
                if (building_SMCDRH != null)
                {
                    return building_SMCDRH;
                }
            }
            return null;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref this.IcookingTicking, "IcookingTicking", 0, false);
            Scribe_Values.Look<int>(ref this.IcookingTime, "IcookingTime", 12000, false);
        }
    }

    public class JobDriver_EnterSMCD_RH : JobDriver_EnterCryptosleepCasket
    {
        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedOrNull(TargetIndex.A);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
            Toil prepare = Toils_General.Wait(500, TargetIndex.None);
            prepare.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
            prepare.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
            yield return prepare;
            Toil enter = new Toil();
            enter.initAction = delegate
            {
                Pawn actor = enter.actor;
                SpacemarineConversionDeviceRH pod = (SpacemarineConversionDeviceRH)actor.CurJob.targetA.Thing;
                void action()
                {
                    actor.equipment.DropAllEquipment(actor.Position);
                    actor.apparel.DropAll(actor.Position);
                    actor.inventory.DropAllNearPawn(actor.Position);
                    actor.DeSpawn();
                    pod.TryAcceptThing(actor, true);
                }
                if (!pod.def.building.isPlayerEjectable)
                {
                    int freeColonistsSpawnedOrInPlayerEjectablePodsCount = this.Map.mapPawns.FreeColonistsSpawnedOrInPlayerEjectablePodsCount;
                    if (freeColonistsSpawnedOrInPlayerEjectablePodsCount <= 1)
                    {
                        Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("CasketWarning".Translate().AdjustedFor(actor), action, false, null));
                    }
                    else
                    {
                        action();
                    }
                }
                else
                {
                    action();
                }
            };
            enter.defaultCompleteMode = ToilCompleteMode.Instant;
            yield return enter;
        }
    }

}
