using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RimWorld;
using Verse;

namespace Rimhammer40k.Spaceship
{
    public abstract class Comp_OrbitalRelay : ThingComp
    {
        public CompProperties_OrbitalRelay Props
        {
            get
            {
                return (CompProperties_OrbitalRelay)this.props;
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
        }
    }
}
