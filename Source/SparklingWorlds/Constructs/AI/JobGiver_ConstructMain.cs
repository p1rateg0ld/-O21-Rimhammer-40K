using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.AI;

namespace Rimhammer40k.Constructs.AI
{
    public class JobGiver_ConstructMain : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            Pawn_Construct construct = (Pawn_Construct)pawn;
            if (construct.station != null)
            {
                if (construct.station.Spawned && construct.station.Map == pawn.Map)
                {
                    Job result;
                    if (construct.station is Building_WorkGiverConstructStation b)
                    {
                        pawn.workSettings = new Pawn_WorkSettings(pawn);
                        pawn.workSettings.EnableAndInitialize();
                        pawn.workSettings.DisableAll();
                        foreach (WorkTypeDef def in b.WorkTypes)
                        {
                            pawn.workSettings.SetPriority(def, 3);
                        }
                        // So the station finds the best job for the pawn
                        result = b.TryIssueJobPackageConstruct(construct, true).Job;
                        if (result == null)
                        {
                            result = b.TryIssueJobPackageConstruct(construct, false).Job;
                        }
                    }
                    else
                    {
                        result = construct.station.TryGiveJob();
                    }
                    if (result == null)
                    {
                        result = new Job(ConstructDefOf.Construct_ReturnToStation, construct.station);
                    }
                    return result;
                }
                return new Job(ConstructDefOf.Construct_SelfTerminate);
            }
            return null;
        }
    }
}
