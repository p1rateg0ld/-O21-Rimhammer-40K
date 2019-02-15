using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Rimhammer40k.Constructs
{
    class Building_ConstructStationSpecialGraphics : Building_ConstructStationRefuelable
    {
        public override void DrawDormantConstructs()
        {
            if(ConstructsLeft > 0)
            {
                ConstructDefOf.ConstructScarab.graphic.DrawFromDef(Position.ToVector3ShiftedWithAltitude(AltitudeLayer.LayingPawn), default(Rot4), ConstructDefOf.ConstructScarab);
            }
        }
    }
}
