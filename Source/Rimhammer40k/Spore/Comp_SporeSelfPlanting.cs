using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using RimWorld;
using Verse;

namespace Rimhammer40k.Spore
{
    public class Comp_SporeSelfPlanting : ThingComp
    {
        public CompProperties_SporeSelfPlanting Props
        {
            get
            {
                return (CompProperties_SporeSelfPlanting)this.props;
            }
        }
    }
}
