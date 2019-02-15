using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using RimWorld;
using Verse;

namespace Rimhammer40k
{
    public class GenStep_Blackstone : GenStep
    {
        public TerrainDef terrainDef;

        public override int SeedPart
        {
            get
            {
                return 1370184742;
            }
        }

        public override void Generate(Map map, GenStepParams parms)
        {
            if(map.Biome == BiomeDefOf.GaussDesertBlue || map.Biome == BiomeDefOf.GaussDesertGreen || map.Biome == BiomeDefOf.GaussDesertOrange)
            {
                ReplaceUglyRock(map);

                ReplaceStonyTerrain(map);

                ReplaceChunks(map);

                ReplaceSteamGeyser(map);
            }
        }

        public void ReplaceUglyRock(Map map)
        {

        }

        public void ReplaceStoneTerrain(Map map)
        {
            // Replace any terrain not matching gauss desert.
            IEnumerable<Terrain> enumerable = from def in map.AllCells
                                                where map.terrainGrid.TerrainAt()
                                                select def;
            foreach (HediffDef current in enumerable)
            {
                pawn.health.hediffSet.hediffs.Remove(pawn.health.hediffSet.GetFirstHediffOfDef(current));
            }
        }

        public void ReplaceChunks(Map map)
        {

        }

        public void ReplaceSteamGeyser(Map map)
        {

        }
    }
}
