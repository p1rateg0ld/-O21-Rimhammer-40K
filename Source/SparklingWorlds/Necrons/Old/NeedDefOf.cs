using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using UnityEngine;

namespace Rimhammer40k.Necrons
{
    [DefOf]
    public static class NeedsDefOf
    {
        static NeedsDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(NeedsDefOf));
        }

        public static NeedDef GaussEnergy;
    }
}
