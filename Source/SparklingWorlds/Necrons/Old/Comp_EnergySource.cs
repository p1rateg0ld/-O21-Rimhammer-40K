using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.AI;
using UnityEngine;

namespace Rimhammer40k.Necrons
{
    class Comp_EnergySource : ThingComp
    {
        public CompProperties_EnergySource EnergyProps
        {
            get
            {
                return this.props as CompProperties_EnergySource;
            }
        }
    }

    public virtual void RechargeEnergyNeed(Pawn targetPawn)
    {
        Need_GaussEnergy need_GaussEnergy = targetPawn.needs.TryGetNeed<Need_GaussEnergy>();
        bool flag = need_GaussEnergy != null;
        if (flag)
        {
            bool isConsumable = this.EnergyProps.isConsumable;
            if (isConsumable)
            {
                float num = (float)this.parent.stackCount * this.EnergyProps.energyWhenConsumed;
                need_GaussEnergy.CurLevel += num;
            }
            else
            {
                need_GaussEnergy.CurLevel += this.EnergyProps.passiveEnergyGeneration;
            }
        }
    }
}
