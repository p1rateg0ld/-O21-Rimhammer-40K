using System.Collections.Generic;
using Verse;

namespace Rimhammer40k.Necrons
{
    public class PawnCapacityWorker_HeatVenting : PawnCapacityWorker
    {
        public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
        {
            BodyPartTagDef GEKidney = BodyPartTagDefOf.HVSource;
            return PawnCapacityUtility.CalculateTagEfficiency(diffSet, GEKidney, float.MaxValue, default(FloatRange), impactors, -1f);
        }

        public override bool CanHaveCapacity(BodyDef body)
        {
            return body.HasPartWithTag(BodyPartTagDefOf.HVSource);
        }
    }
}
