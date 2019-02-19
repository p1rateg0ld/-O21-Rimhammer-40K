using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using RimWorld;
using Verse;

namespace Rimhammer40k.Biomes
{
    [DefOf]
    public static class TerrainDefOf
    {
        static TerrainDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(TerrainDefOf));
        }

        public static TerrainDef NecronBlackSand;
    }
}
