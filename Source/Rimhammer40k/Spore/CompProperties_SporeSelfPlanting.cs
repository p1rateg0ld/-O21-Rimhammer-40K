using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using RimWorld;
using Verse;

namespace Rimhammer40k.Spore
{
    public class CompProperties_SporeSelfPlanting : CompProperties
    {
        public CompProperties_SporeSelfPlanting()
        {
            this.compClass = typeof(Comp_SporeSelfPlanting);
        }

        public bool isOrkoid = true;

        public ThingDef spawnedThing = null;

        public int timeTillSpawn = 60000;
    }
}
