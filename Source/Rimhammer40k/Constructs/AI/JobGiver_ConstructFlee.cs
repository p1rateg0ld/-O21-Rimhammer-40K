﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace Rimhammer40k.Constructs.AI
{
    public class JobGiver_ConstructFlee : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            Job result;
            if (pawn.playerSettings != null && pawn.playerSettings.UsesConfigurableHostilityResponse)
            {
                result = null;
            }
            else if (ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(pawn))
            {
                result = null;
            }
            else
            {
                if (pawn.Faction == null)
                {
                    List<Thing> list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.AlwaysFlee);
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (pawn.Position.InHorDistOf(list[i].Position, 18f) && SelfDefenseUtility.ShouldFleeFrom(list[i], pawn, false, false))
                        {
                            return ReturnToStationJob((Pawn_Construct)pawn);
                        }
                    }
                    Job job2 = FleeLargeFireJob(pawn);
                    if (job2 != null)
                    {
                        return job2;
                    }
                }
                else if (pawn.GetLord() == null)
                {
                    List<IAttackTarget> potentialTargetsFor = pawn.Map.attackTargetsCache.GetPotentialTargetsFor(pawn);
                    for (int j = 0; j < potentialTargetsFor.Count; j++)
                    {
                        Thing thing = potentialTargetsFor[j].Thing;
                        if (pawn.Position.InHorDistOf(thing.Position, 18f) && SelfDefenseUtility.ShouldFleeFrom(thing, pawn, false, true))
                        {
                            return ReturnToStationJob((Pawn_Construct)pawn);
                        }
                    }
                }
                result = null;
            }
            return result;
        }

        private Job FleeLargeFireJob(Pawn pawn)
        {
            List<Thing> list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Fire);
            Job result;
            if (list.Count < 60)
            {
                result = null;
            }
            else
            {
                TraverseParms tp = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
                Fire closestFire = null;
                float closestDistSq = -1f;
                int firesCount = 0;
                RegionTraverser.BreadthFirstTraverse(pawn.Position, pawn.Map, (Region from, Region to) => to.Allows(tp, false), delegate (Region x)
                {
                    List<Thing> list2 = x.ListerThings.ThingsInGroup(ThingRequestGroup.Fire);
                    for (int i = 0; i < list2.Count; i++)
                    {
                        float num = (float)pawn.Position.DistanceToSquared(list2[i].Position);
                        if (num <= 400f)
                        {
                            if (closestFire == null || num < closestDistSq)
                            {
                                closestDistSq = num;
                                closestFire = (Fire)list2[i];
                            }
                            firesCount++;
                        }
                    }
                    return closestDistSq <= 100f && firesCount >= 60;
                }, 18, RegionType.Set_Passable);
                if (closestDistSq <= 100f && firesCount >= 60)
                {
                    Job job = ReturnToStationJob((Pawn_Construct)pawn);
                    if (job != null)
                    {
                        return job;
                    }
                }
                result = null;
            }
            return result;
        }

        public Job ReturnToStationJob(Pawn_Construct construct)
        {
            if (construct.station != null && construct.Map == construct.station.Map)
            {
                return new Job(ConstructDefOf.Construct_ReturnToStation, construct.station);
            }
            return null;
        }
    }
}
