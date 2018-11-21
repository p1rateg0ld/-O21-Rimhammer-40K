using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using RimWorld;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace Rimhammer40k.AIRobot
{
    public class X2_MainTabWindow_Robots : MainTabWindow_PawnTable
    {
        private static PawnTableDef pawnTableDef = null;
        protected override PawnTableDef PawnTableDef
        {
            get
            {
                if (pawnTableDef == null)
                    pawnTableDef = DefDatabase<PawnTableDef>.GetNamed("Constructs");
                return pawnTableDef;
            }
        }

        protected override IEnumerable<Pawn> Pawns
        {
            get
            {
                List<Pawn> robots = new List<Pawn>();

                foreach (X2_Building_AIRobotRechargeStation station in Find.CurrentMap.listerBuildings.AllBuildingsColonistOfClass<X2_Building_AIRobotRechargeStation>())
                {
                    if (station == null || !station.Spawned || station.Destroyed)
                        continue;

                    if (station.GetRobot != null)
                        robots.Add(station.GetRobot);
                    
                }

                if (robots == null)
                    return null;

                try
                {
                    return robots.OrderBy(r => r.LabelShort);
                }
                catch
                { return robots; }
            }
        }
        
        public override void PostOpen()
        {
            base.PostOpen();
            Find.World.renderer.wantedMode = WorldRenderMode.None;
        }
    }
}
