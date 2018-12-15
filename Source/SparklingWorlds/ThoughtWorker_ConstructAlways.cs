using RimWorld;
using Verse;
using Rimhammer40k.Necrons.Extensions;

namespace Rimhammer40k
{
    public class ThoughtWorker_ConstructAlways : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            bool flag = p.def.race.FleshType == PawnExt.necronFlesh;
            ThoughtState result;
            if (flag)
            {
                result = ThoughtState.ActiveAtStage(0);
            }
            else
            {
                result = ThoughtState.Inactive;
            }
            return result;
        }
    }
}
