using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace Rimhammer40k.Constructs.AI
{
    public class JobDriver_SelfTerminate : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return new Toil()
            {
                initAction = () =>
                {
                    pawn.Destroy();
                },
                defaultCompleteMode = ToilCompleteMode.Instant
            };
        }
    }
}
