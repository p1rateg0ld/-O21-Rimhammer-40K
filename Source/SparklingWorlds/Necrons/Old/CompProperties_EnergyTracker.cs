using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
namespace Rimhammer40k.Necrons
{
    public class CompProperties_EnergyTracker : CompProperties
    {
        public CompProperties_EnergyTracker()
        {
            this.compClass = typeof(Comp_EnergyTracker);
        }

        public bool canHibernate = true;

        public JobDef hibernationJob;
    }
}
