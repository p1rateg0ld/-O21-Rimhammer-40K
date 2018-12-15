using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;

namespace Rimhammer40k.Necrons
{
    public class CompNecronResurrection : ThingComp
    {
        public CompProperties_NecronResurrection PropsNecRes
        {
            get
            {
                return (CompProperties_NecronResurrection)this.props;
            }
        }
        public override void CompTick()
        {
            this.Tick(1);
        }

        public override void CompTickRare()
        {
            this.Tick(250);
        }

        private void Tick(int interval)
        {

        }

        //Function to get random number
        private static readonly System.Random getrandom = new System.Random();

        private bool IsResurrectable;

        private int resurrectAfterTimestamp = -1;

        /**
         * Get a random number out of 100
         * If Necron is a colonist, resurrect with a 75% chance.
         * If Necron is not a colonist, resurrect with a 25% chance.
         * Otherwise, leave dead.
         **/
        public void AttemptResurrection()
        {
            System.Random rnd = new System.Random();
            int flag = rnd.Next(1, 100);
            if (flag < 75)
            {
                ResurrectNecron();
            }
            else
            {
                return;
            }
        }

        //Start Resurrection Loop
        public void ResurrectNecron()
        {
            IsResurrectable = true;
        }

        public void TickRare()
        {
            Corpse corpse = this.parent as Corpse;
            if (this.IsResurrectable == true)
            {
                this.resurrectAfterTimestamp = corpse.Age + 2000;
            }
            if (this.ShouldResurrect)
            {
                ResurrectionUtility.Resurrect(corpse.InnerPawn);
            }
        }

        private bool ShouldResurrect
        {
            get
            {
                Corpse corpse = this.parent as Corpse;
                return this.resurrectAfterTimestamp > 0 && corpse.Age >= this.resurrectAfterTimestamp;
            }
        }
    }
}
