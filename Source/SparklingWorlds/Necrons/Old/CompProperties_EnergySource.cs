using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Rimhammer40k.Necrons
{
    class CompProperties_EnergySource : CompProperties
    {
        public CompProperties_EnergySource()
        {
            this.compClass = typeof(Comp_EnergySource);
        }

        public bool isConsumable = false;

        public float energyWhenConsumed = 0f;

        public float passiveEnergyGeneration = 0f;

        public List<ThingOrderRequest> fuels = new List<ThingOrderRequest>();

        public float maxFuelAmount = 75f;

        public double fuelAmountUsedPerInterval = 0.001;

        public float activeEnergyGeneration = 0f;

        public JobDef refillJob;
    }
}
