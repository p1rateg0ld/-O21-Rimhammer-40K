using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using RimWorld;
using Verse;

namespace Rimhammer40k.Spore
{
    public class Spore : ThingWithComps
    {
        public int ticksTillSelfPlant;

        public override void PostMake()
        {
            base.PostMake();

            ticksTillSelfPlant = Current.Game.tickManager.TicksGame + 4000;
        }
        public override void Tick()
        {
            base.Tick();

            if(Current.Game.tickManager.TicksGame >= ticksTillSelfPlant)
            {
                if (this.Map.terrainGrid.TerrainAt(this.Position).fertility != null && this.Map.terrainGrid.TerrainAt(this.Position).fertility >= 0.7)
                {
                    if (this.Position.GetPlant(this.Map) != null)
                    {
                        this.Position.GetPlant(this.Map).Destroy(DestroyMode.Vanish);
                    }
                    GenSpawn.Spawn(ThingDef.Named("O21_Plant_OrkoidShroom"), this.Position, this.Map, 0);
                }
                this.Destroy(DestroyMode.Vanish);
            }
        }
    }
}
