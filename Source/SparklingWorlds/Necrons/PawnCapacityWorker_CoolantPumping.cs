using System;
using System.Collections.Generic;
using Verse;

namespace Rimhammer40k.Necrons
{
    public class PawnCapacityWorker_CoolantPumping : PawnCapacityWorker
    {
        public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
        {
            BodyPartTagDef CPSource = BodyPartTagDefOf.CPSource;
            return PawnCapacityUtility.CalculateTagEfficiency(diffSet, CPSource, float.MaxValue, default(FloatRange), impactors, -1f);
        }

        public override bool CanHaveCapacity(BodyDef body)
        {
            return body.HasPartWithTag(BodyPartTagDefOf.CPSource);
        }
    }
}
