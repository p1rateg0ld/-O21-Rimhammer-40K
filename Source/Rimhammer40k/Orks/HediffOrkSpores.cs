using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using RimWorld;
using Verse;

namespace Rimhammer40k.Orks
{
    public class HediffOrkSpores : HediffWithComps
    {
        public int ticksUntilNextSpore;

        public override void PostMake()
        {
            base.PostMake();
            this.SetNextSporeTick();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref this.ticksUntilNextSpore, "ticksUntilNextSpore", 0, false);
        }

        public override void Tick()
        {
            base.Tick();
            if (Current.Game.tickManager.TicksGame >= this.ticksUntilNextSpore)
            {
                this.TryDropSpore();
                this.SetNextSporeTick();
            }
        }

        public void SetNextSporeTick()
        {
            this.ticksUntilNextSpore = Current.Game.tickManager.TicksGame + UnityEngine.Random.Range(60000, 120000);
        }

        public void TryDropSpore()
        {
            GenSpawn.Spawn(ThingDef.Named("O21_OrkSpore"), pawn.Position, pawn.Map, 0);
        }
    }
}
