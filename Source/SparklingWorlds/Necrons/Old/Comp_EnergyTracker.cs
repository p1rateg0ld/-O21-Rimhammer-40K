using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;

namespace Rimhammer40k.Necrons
{
    public class Comp_EnergyTracker : ThingComp
    {
        public float gaussEnergy;

        private Pawn pawn;

        private Need_GaussEnergy gaussNeed;

        public CompProperties_EnergyTracker EnergyProperties
        {
            get
            {
                return this.props as CompProperties_EnergyTracker;
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            this.pawn = (this.parent as Pawn);
            bool flag = this.pawn != null;
            if (flag)
            {
                this.gaussNeed = this.pawn.needs.TryGetNeed<Need_GaussEnergy>();
            }
        }

        public override void CompTick()
        {
            bool flag = this.gaussNeed != null;
            if (flag)
            {
                this.gaussEnergy = this.gaussNeed.CurLevel;
            }
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look<float>(ref this.gaussEnergy, "gauss energy", 0f, false);
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            bool flag = Prefs.DevMode && DebugSettings.godMode;
            if (flag)
            {
                Command_Action gizmo = new Command_Action();
                gizmo.defaultLabel = "DEBUG: Set Gauss Energy to 100%";
                gizmo.action = delegate ()
                {
                    this.gaussNeed.CurLevelPercentage = 1f;
                };
                yield return gizmo;
                gizmo = null;
                Command_Action gizmo2 = new Command_Action();
                gizmo2.defaultLabel = "DEBUG: Set Gauss Energy to 50%";
                gizmo2.action = delegate ()
                {
                    this.gaussNeed.CurLevelPercentage = 0.5f;
                };
                yield return gizmo2;
                gizmo2 = null;
                Command_Action gizmo3 = new Command_Action();
                gizmo3.defaultLabel = "DEBUG: Set Gauss Energy to 20%";
                gizmo3.action = delegate ()
                {
                    this.gaussNeed.CurLevelPercentage = 0.2f;
                };
                yield return gizmo3;
                gizmo3 = null;
            }
            yield break;
        }
    }
}
