using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace Rimhammer40k
{
    [DefOf]
    public static class MBDefsOf
    {
        public static JobDef UseMissionBoardRC;

        public static SoundDef AmbientMissionBoardRC;

        public static SoundDef OneShotMissionSelectedRC;
    }

    public class CompProperties_MissionBoardRC : CompProperties
    {
        public List<LabeledIncident> doableMissions = null;

        public CompProperties_MissionBoardRC()
        {
            base.compClass = typeof(CompMissionBoardRC);
        }

    }

    public class CompMissionBoardRC : ThingComp
    {
        public int tickAtLastMission = -1;

        public CompProperties_MissionBoardRC Props
        {
            get
            {
                return (CompProperties_MissionBoardRC)this.props;
            }
        }

        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn myPawn)
        {
            FloatMenuOption failureReason = this.GetFailureReason(myPawn);
            if (failureReason != null)
            {
                yield return failureReason;
            }
            else
            {
                FloatMenuOption option = IncidentFloatMenuOption(myPawn);
                if (option != null)
                {
                    yield return option;
                }
            }
        }

        public FloatMenuOption IncidentFloatMenuOption(Pawn negotiator)
        {
            FloatMenuOption result;
            string text = "SelectMissionBoardRC".Translate();
            result = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, delegate ()
            {
                this.GiveUseMissionBoardJob(negotiator);
            }), negotiator, this.parent, "ReservedBy");
            return result;
        }

        public void GiveUseMissionBoardJob(Pawn negotiator)
        {
            Job job = new Job(MBDefsOf.UseMissionBoardRC, this.parent);
            negotiator.jobs.TryTakeOrderedJob(job, JobTag.Misc);
        }

        private FloatMenuOption GetFailureReason(Pawn myPawn)
        {
            FloatMenuOption result;
            if (!myPawn.CanReach(this.parent, PathEndMode.InteractionCell, Danger.Some, false, TraverseMode.ByPawn))
            {
                result = new FloatMenuOption("CannotUseNoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
            }
            else
            {
                result = null;
            }
            return result;
        }

    }

    public class JobDriver_UseMissionBoardRC : JobDriver
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
            yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.InteractionCell);
            Toil openMissionSelect = new Toil();
            openMissionSelect.initAction = delegate ()
            {
                Pawn actor = openMissionSelect.actor;
                Building building_MissionBoard = (Building)actor.jobs.curJob.GetTarget(TargetIndex.A).Thing;
                StartMissionSelection(building_MissionBoard);
            };
            yield return openMissionSelect;
            yield break;
        }

        public void StartMissionSelection(Building missionBoard)
        {
            Dialog_NodeTree dialog_Negotiation = new Dialog_NodeTree(MissionBoardDialogMakerRC.MissionSelectWindow(missionBoard, this.pawn.Map), true);
            dialog_Negotiation.soundAmbient = MBDefsOf.AmbientMissionBoardRC;
            Find.WindowStack.Add(dialog_Negotiation);
        }
    }

    public static class MissionBoardDialogMakerRC
    {
        public static DiaNode MissionSelectWindow(Building missionBoard, Map map)
        {
            DiaNode diaNode;
            diaNode = new DiaNode("SelectMissionRC".Translate());
            if (map != null && map.IsPlayerHome && missionBoard.GetComp<CompMissionBoardRC>() != null)
            {
                foreach (LabeledIncident labeledIncident in missionBoard.GetComp<CompMissionBoardRC>().Props.doableMissions)
                {
                    diaNode.options.Add(MissionBoardDialogMakerRC.StartMissionOption(map, labeledIncident, missionBoard));
                }
            }
            DiaOption diaOption = new DiaOption("(" + "Close".Translate() + ")");
            diaOption.resolveTree = true;
            diaNode.options.Add(diaOption);
            return diaNode;
        }

        private static void StartMission(Map map, IncidentDef incident)
        {
            IncidentParms incidentParms = new IncidentParms();
            incidentParms.target = map;
            incidentParms.points = StorytellerUtility.DefaultThreatPointsNow(map);
            incident.Worker.TryExecute(incidentParms);
            SoundStarter.PlayOneShotOnCamera(MBDefsOf.OneShotMissionSelectedRC);
        }

        public static DiaOption StartMissionOption(Map map, LabeledIncident labeledIncident, Building missionBoard)
        {
            int silverToPay = 500;

            string text = labeledIncident.customLabel;
            DiaOption result;
            DiaOption diaOption5 = new DiaOption(text);

            int num = missionBoard.GetComp<CompMissionBoardRC>().tickAtLastMission + 60000 - Find.TickManager.TicksGame;
            if (num > 0)
            {
                DiaOption waitDialogOption = new DiaOption(text);
                waitDialogOption.Disable("WaitTime".Translate(num.ToStringTicksToPeriod()));
                result = waitDialogOption;
            }
            else if (MissionBoardDialogMakerRC.AmountSendableSilver(map) < silverToPay)
            {
                DiaOption needsSilverOption = new DiaOption(text);
                needsSilverOption.Disable("NeedSilverLaunchable".Translate(silverToPay.ToString()));
                result = needsSilverOption;
            }
            else
            {
                diaOption5.action = delegate ()
                {
                    MissionBoardDialogMakerRC.StartMission(map, labeledIncident.incidentDef);
                    TradeUtility.LaunchThingsOfType(ThingDefOf.Silver, silverToPay, map, null);
                    missionBoard.GetComp<CompMissionBoardRC>().tickAtLastMission = Find.TickManager.TicksGame;
                };
                diaOption5.resolveTree = true;
                result = diaOption5;
            }
            return result;
        }

        private static int AmountSendableSilver(Map map)
        {
            return (from t in TradeUtility.AllLaunchableThingsForTrade(map)
                    where t.def == ThingDefOf.Silver
                    select t).Sum((Thing t) => t.stackCount);
        }

    }

    public class LabeledIncident
    {
        public IncidentDef incidentDef;

        public string customLabel;

        public override string ToString()
        {
            return string.Concat(new string[]
            {
                "(",
                (this.incidentDef == null) ? "null" : this.incidentDef.ToString(),
                " w=",
                this.customLabel,
                ")"
            });
        }

        public void LoadDataFromXmlCustom(XmlNode xmlRoot)
        {
            DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "incidentDef", xmlRoot.Name);
            this.customLabel = xmlRoot.FirstChild.Value;
        }
    }
}
