using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace Rimhammer40k.Necrons
{
    public class Need_GaussEnergy : Need
    {
        public static float rechargePercentage = 0.505f;

        public Need_GaussEnergy(Pawn pawn)
        {
            this.pawn = pawn;
        }

        public override float MaxLevel
        {
            get
            {
                return 1f;
            }
        }

        public override void SetInitialLevel()
        {
            this.CurLevel = 1f;
        }

        public override void DrawOnGUI(Rect rect, int maxThresholdMarkers = int.MaxValue, float customMargin = -1, bool drawArrows = true, bool doTooltip = true)
        {
            bool flag = this.threshPercents == null;
            if (flag)
            {
                this.threshPercents = new List<float>();
            }
            this.threshPercents.Clear();
            this.threshPercents.Add(0.5f);
            this.threshPercents.Add(0.2f);
            base.DrawOnGUI(rect, maxThresholdMarkers, customMargin, drawArrows, doTooltip);
        }

        public override void NeedInterval()
        {
            bool flag = this.pawn.def.HasModExtension<NecronPawnProperties>();
            if (flag)
            {
                foreach(Need need in this.pawn.needs.AllNeeds)
                {
                    bool flag2 = need.def != NeedsDefOf.GaussEnergy;
                    if (flag2)
                    {
                        need.CurLevelPercentage = 1f;
                    }
                }
            }
            float num = 1f;
            Comp_EnergyTracker energyTrackerComp;
            bool flag3 = !CaravanUtility.IsCaravanMember(this.pawn) && (energyTrackerComp = ThingCompUtility.TryGetComp<Comp_EnergyTracker>(this.pawn)) != null && energyTrackerComp.EnergyProperties.canHibernate && this.pawn.CurJobDef == energyTrackerComp.EnergyProperties.hibernationJob;
            if (flag3)
            {
                num = -0.1f;
            }
            bool flag4 = this.pawn.needs != null && !base.IsFrozen;
            if (flag4)
            {
                this.CurLevel -= num * 0.000833333354f;
                bool flag5 = this.pawn.needs.food != null && this.pawn.needs.food.CurLevelPercentage > 0f;
                if (flag5)
                {
                    this.CurLevel += 0.0133333337f;
                }
                bool flag6 = this.pawn.apparel != null;
                if (flag6)
                {
                    foreach (Apparel apparel in this.pawn.apparel.WornApparel)
                    {
                        Comp_EnergySource energySourceComp = ThingCompUtility.TryGetComp<Comp_EnergySource>(apparel);
                        bool flag7 = energySourceComp != null && !energySourceComp.EnergyProps.isConsumable;
                        if (flag7)
                        {
                            energySourceComp.RechargeEnergyNeed(this.pawn);
                        }
                    }
                }
                bool flag8 = CaravanUtility.IsCaravanMember(this.pawn) && base.CurLevelPercentage < Need_GaussEnergy.rechargePercentage;
                if (flag8)
                {
                    Caravan caravan = CaravanUtility.GetCaravan(this.pawn);
                    Thing thing3 = caravan.Goods.FirstOrDefault(delegate (Thing thing)
                    {
                        Comp_EnergyTracker energySourceComp4;
                        return (energySourceComp4 = ThingCompUtility.TryGetComp<Comp_EnergyTracker>(thing)) != null && energySourceComp4.EnergyProps.isConsumable;
                    });
                    bool flag9 = thing3 != null;
                    if (flag9)
                    {
                        Comp_EnergyTracker energySourceComp2 = ThingCompUtility.TryGetComp<Comp_EnergyTracker>(thing3);
                        int num2 = (int)Math.Ceiling((double)((this.MaxLevel - this.CurLevel) / energySourceComp2.EnergyProps.energyWhenConsumed));
                        num2 = Math.Min(num2, thing3.stackCount);
                        Thing thing2 = thing3.SplitOff(num2);
                        Comp_EnergyTracker energySourceComp3 = ThingCompUtility.TryGetComp<Comp_EnergyTracker>(thing2);
                        energySourceComp3.RechargeEnergyNeed(this.pawn);
                        thing2.Destroy(0);
                    }
                }
                bool flag10 = this.CurLevel < 0.2f;
                if (flag10)
                {
                    bool flag11 = !this.pawn.health.hediffSet.HasHediff(HediffDefOf.ChjPowerShortage, false);
                    if (flag11)
                    {
                        this.pawn.health.AddHediff(HediffDefOf.ChjPowerShortage, null, null, null);
                    }
                }
                else
                {
                    bool flag12 = this.pawn.health.hediffSet.HasHediff(HediffDefOf.ChjPowerShortage, false);
                    if (flag12)
                    {
                        this.pawn.health.RemoveHediff(this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ChjPowerShortage, false));
                    }
                }
                bool flag13 = this.CurLevel <= 0f;
                if (flag13)
                {
                    Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.ChjPowerFailure, this.pawn, null);
                    this.pawn.health.AddHediff(hediff, null, null, null);
                    this.pawn.Kill(null, hediff);
                }
            }
        }
    }
}
