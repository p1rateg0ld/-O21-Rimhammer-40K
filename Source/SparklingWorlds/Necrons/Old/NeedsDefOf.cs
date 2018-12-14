using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;

namespace Rimhammer40k.Necrons
{
    [DefOf]
    public static class NeedsDefOf
    {
        static NeedsDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(NeedDefOf));
        }

        public static NeedDef GaussEnergy;
    }
}
