using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace Rimhammer40k.Necrons
{
    public class JobDriver_Hibernate : JobDriver
    {
        public Thing Target
        {
            get
            {
                return base.TargetA.Thing;
            }
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            throw new NotImplementedException();
        }

        public CompPowerTrader powerTrader;

        protected override IEnumerable<Toil> MakeNewToils()
        {
            throw new NotImplementedException();
        }
    }
}
