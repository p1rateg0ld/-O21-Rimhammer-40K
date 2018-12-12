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
    public class Need_GaussEnergy : Need
    {
        public Need_GaussEnergy(Pawn pawn) : base(pawn)
        {
            this.threshPercents = new List<float>();
            this.threshPercents.Add(0.28f);
            this.threshPercents.Add(0.14f);
        }

        public GEnergyCategory CurCategory
        {
            get
            {
                if(this.CurLevel < 0.01f)
                {
                    return GEnergyCategory.Drained;
                }
                if(this.CurLevel < 0.14f)
                {
                    return GEnergyCategory.LowEnergy;
                }
                if(this.CurLevel < 0.28f)
                {
                    return GEnergyCategory.MedEnergy;
                }
                return GEnergyCategory.HighEnergy;
            }
        }

        public float GEnergyFallPerTick
        {
            get
            {
                switch (this.CurCategory)
                {
                    case GEnergyCategory.HighEnergy:
                        return 1.58333332E-05f * this.GEnergyFallFactor;
                    case GEnergyCategory.MedEnergy:
                        return 1.58333332E-05f * this.GEnergyFallFactor * 0.7f;
                    case GEnergyCategory.LowEnergy:
                        return 1.58333332E-05f * this.GEnergyFallFactor * 0.3f;
                    case GEnergyCategory.Drained:
                        return 1.58333332E-05f * this.GEnergyFallFactor * 0.6f;
                    default:
                        return 999f;
                }
            }
        }

        private float GEnergyFallFactor
        {
            get
            {
                return this.pawn.health.hediffSet.RestFallFactor;
            }
        }

        public override int GUIChangeArrow
        {
            get
            {
                if (this.Recharging)
                {
                    return 1;
                }
                return -1;
            }
        }

        public int TicksAtZero
        {
            get
            {
                return this.ticksAtZero;
            }
        }

        private bool Recharging
        {
            get
            {
                return Find.TickManager.TicksGame < this.lastChargeTick + 2;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref this.ticksAtZero, "ticksAtZero", 0, false);
        }

        public override void NeedInterval()
        {
            if (!base.IsFrozen)
            {
                if (this.Recharging)
                {
                    float num = this.lastChargeEffectiveness;
                    num *= this.pawn.GetStatValue(StatDefOf.RestRateMultiplier, true);
                    if(num > 0f)
                    {
                        this.CurLevel += 0.00571428565f * num;
                    }
                }
                else
                {
                    this.CurLevel -= this.GEnergyFallPerTick * 150f;
                }
            }
            if(this.CurLevel < 0.0001f)
            {
                this.ticksAtZero += 150;
            }
            else
            {
                this.ticksAtZero = 0;
            }
            if(this.ticksAtZero > 1000 && this.pawn.Spawned)
            {
                float mtb;
                if (this.ticksAtZero < 15000)
                {
                    mtb = 0.25f;
                }
                else if (this.ticksAtZero < 30000)
                {
                    mtb = 0.125f;
                }
                else if (this.ticksAtZero < 45000)
                {
                    mtb = 0.0833333358f;
                }
                else
                {
                    mtb = 0.0625f;
                }
                if (Rand.MTBEventOccurs(mtb, 60000f, 150f) && (this.pawn.CurJob == null || this.pawn.CurJob.def != JobDefOf.LayDown))
                {
                    this.pawn.jobs.StartJob(new Job(JobDefOf.LayDown, this.pawn.Position), JobCondition.InterruptForced, null, false, true, null, new JobTag?(JobTag.SatisfyingNeeds), false);
                    if (this.pawn.InMentalState)
                    {
                        this.pawn.mindState.mentalStateHandler.CurState.RecoverFromState();
                    }
                    if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
                    {
                        Messages.Message("MessageInvoluntarySleep".Translate(this.pawn.LabelShort, this.pawn), this.pawn, MessageTypeDefOf.NegativeEvent, true);
                    }
                    TaleRecorder.RecordTale(TaleDefOf.Drained, new object[]
                    {
                        this.pawn
                    });
                }
            }
        }

        public void TickCharging(float chargeEffectiveness)
        {
            if (chargeEffectiveness <= 0f)
            {
                return;
            }
            this.lastChargeTick = Find.TickManager.TicksGame;
            this.lastChargeEffectiveness = chargeEffectiveness;
        }

        private int lastChargeTick = -999;
        
        private float lastChargeEffectiveness = 1f;
        
        private int ticksAtZero;
        
        private const float FullChargeHours = 2.5f;
        
        public const float BaseChargeGainPerTick = 3.8095237E-05f;
        
        private const float BaseChargeFallPerTick = 1.58333332E-05f;
        
        public const float ThreshMedEnergy = 0.28f;
        
        public const float ThreshLowEnergy = 0.14f;
        
        public const float DefaultGoChargeMaxLevel = 0.75f;
        
        public const float DefaultNaturalWakeThreshold = 1f;
        
        public const float CanWakeThreshold = 0.2f;
        
        private const float BaseInvoluntaryChargeMTBDays = 0.25f;
    }
}
