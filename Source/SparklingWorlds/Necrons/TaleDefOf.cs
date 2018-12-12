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
    public static class TaleDefOf
    {
        static TaleDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(TaleDefOf));
        }

        public static TaleDef Drained;
    }
}
