using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse.AI;

namespace Rimhammer40k.Constructs.AI
{
    public class JobDriver_ReturnToStation : JobDriver_Goto
    {
        protected override IEnumerable<Toil> MakeNewToils()
        {
            foreach (Toil t in base.MakeNewToils())
                yield return t;
            yield return new Toil()
            {
                initAction = () =>
                {
                    pawn.Destroy();
                    ((Pawn_Construct)pawn).station.Notify_ConstructGained();
                },
                defaultCompleteMode = ToilCompleteMode.Instant
            };
        }
    }
}
