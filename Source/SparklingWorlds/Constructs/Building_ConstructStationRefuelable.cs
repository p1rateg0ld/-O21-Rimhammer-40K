using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Rimhammer40k.Constructs
{
    public class Building_ConstructStationRefuelable : Building_WorkGiverConstructStation
    {
        protected CompRefuelable refeulableComp;

        public override void PostMake()
        {
            base.PostMake();
            refeulableComp = GetComp<CompRefuelable>();
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            refeulableComp = GetComp<CompRefuelable>();
        }

        public override int ConstructsLeft
        {
            get
            {
                return Mathf.RoundToInt(refeulableComp.Fuel) - spawnedConstructs.Count;
            }
        }

        public override void Notify_ConstructLost()
        {
            refeulableComp.ConsumeFuel(1);
        }

        public override void Notify_ConstructGained()
        {
            refeulableComp.Refuel(1);
        }
    }
}
