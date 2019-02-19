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
    public static class ThingDefOf
    {
        static ThingDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf));
        }

        public static ThingDef Blackstone;

        public static ThingDef Sandstone;

        public static ThingDef Granite;

        public static ThingDef Slate;

        public static ThingDef Limestone;

        public static ThingDef Marble;
    }
}
