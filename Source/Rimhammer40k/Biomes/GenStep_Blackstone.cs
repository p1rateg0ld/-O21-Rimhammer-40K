using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using RimWorld;
using Verse;

namespace Rimhammer40k.Biomes
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

                ReplaceStoneTerrain(map);

                ReplaceChunks(map);
            }
        }

        public void ReplaceUglyRock(Map map)
        {
            foreach(IntVec3 c in map.AllCells)
            {
                Building edifice = c.GetEdifice(map);
                if(edifice != null)
                {
                    if(edifice.def == ThingDefOf.Sandstone || 
                        edifice.def == ThingDefOf.Granite || 
                        edifice.def == ThingDefOf.Slate || 
                        edifice.def == ThingDefOf.Limestone || 
                        edifice.def == ThingDefOf.Marble)
                    {
                        edifice.Destroy();
                        GenSpawn.Spawn(ThingDef.Named("Blackstone"), c, map);
                    }
                }
            }
        }

        public void ReplaceStoneTerrain(Map map)
        {
            foreach(IntVec3 c in map.AllCells)
            {
                TerrainDef terrain = c.GetTerrain(map);
                if (!terrain.IsRiver)
                {
                    map.terrainGrid.SetTerrain(c, TerrainDefOf.NecronBlackSand);
                }
            }
        }

        public void ReplaceChunks(Map map)
        {
            foreach(IntVec3 c in map.AllCells)
            {
                Thing chunk1 = c.GetFirstThing(map, ThingDef.Named("ChunkSandstone"));
                if(chunk1 != null)
                {
                    chunk1.Destroy();
                    GenSpawn.Spawn(ThingDef.Named("ChunkBlackstone"), c, map);
                }

                Thing chunk2 = c.GetFirstThing(map, ThingDef.Named("ChunkSlate"));
                if (chunk2 != null)
                {
                    chunk2.Destroy();
                    GenSpawn.Spawn(ThingDef.Named("ChunkBlackstone"), c, map);
                }

                Thing chunk3 = c.GetFirstThing(map, ThingDef.Named("ChunkGranite"));
                if (chunk3 != null)
                {
                    chunk3.Destroy();
                    GenSpawn.Spawn(ThingDef.Named("ChunkBlackstone"), c, map);
                }

                Thing chunk4 = c.GetFirstThing(map, ThingDef.Named("ChunkLimestone"));
                if (chunk4 != null)
                {
                    chunk4.Destroy();
                    GenSpawn.Spawn(ThingDef.Named("ChunkBlackstone"), c, map);
                }

                Thing chunk5 = c.GetFirstThing(map, ThingDef.Named("ChunkMarble"));
                if (chunk5 != null)
                {
                    chunk5.Destroy();
                    GenSpawn.Spawn(ThingDef.Named("ChunkBlackstone"), c, map);
                }
            }
        }
    }
}
