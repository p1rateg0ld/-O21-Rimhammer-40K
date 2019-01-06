using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace Rimhammer40k
{
    public class BiomeWorker_GaussDesertGreen : BiomeWorker
    {
        public override float GetScore(Tile tile, int tileID)
        {
            if (tile.WaterCovered)
            {
                return -100f;
            }
            if (tile.rainfall >= 600f)
            {
                return 0f;
            }
            return tile.temperature + 0.0001f;
        }
    }
}
