using System;
using Verse;
using RimWorld;

namespace Rimhammer40k.Necrons
{
    [DefOf]
    public static class BodyPartTagDefOf
    {
        static BodyPartTagDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(BodyPartTagDefOf));
        }

        public static BodyPartTagDef CPSource;

        public static BodyPartTagDef HVSource;

        public static BodyPartTagDef GEKidney;

        public static BodyPartTagDef GELiver;
    }
}
