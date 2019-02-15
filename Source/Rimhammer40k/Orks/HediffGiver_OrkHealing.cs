using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Rimhammer40k.Orks
{
    public class HediffGiver_OrkHealing : HediffGiver
    {
        public override void OnIntervalPassed(Pawn pawn, Hediff cause)
        {
            if (base.TryApply(pawn, null))
            {

                return;
            }
        }
    }
}
