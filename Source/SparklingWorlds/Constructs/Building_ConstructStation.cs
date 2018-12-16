using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Rimhammer40k.Constructs
{
    [StaticConstructorOnStartup]
    public abstract class Building_ConstructStation : Building
    {
        public static readonly Texture2D Cancel = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);
        protected bool lockdown;
        protected DefModExtension_ConstructStation extension;
        protected List<Pawn_Construct> spawnedConstructs = new List<Pawn_Construct>();

        public abstract int ConstructsLeft { get; }
        public abstract void Notify_ConstructLost();
        public abstract void Notify_ConstructGained();

        public override void PostMake()
        {
            base.PostMake();
            extension = def.GetModExtension<DefModExtension_ConstructStation>();
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            extension = def.GetModExtension<DefModExtension_ConstructStation>();
        }

        public override void Draw()
        {
            base.Draw();
            if (extension.displayDormantConstructs)
            {
                DrawDormantConstructs();
            }
        }

        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            base.DeSpawn(mode);
            List<Pawn_Construct> constructs = spawnedConstructs.ToList();
            for(int i = 0; i < constructs.Count; i++)
            {
                constructs[i].Destroy();
            }
        }

        public virtual void DrawDormantConstructs()
        {
            foreach(IntVec3 cell in GenAdj.CellsOccupiedBy(this).Take(ConstructsLeft))
            {
                ConstructDefOf.ConstructScarab.graphic.DrawFromDef(cell.ToVector3ShiftedWithAltitude(AltitudeLayer.LayingPawn), default(Rot4), ConstructDefOf.ConstructScarab);
            }
        }

        public override void DrawGUIOverlay()
        {
            base.DrawGUIOverlay();
            if (lockdown)
            {
                Map.overlayDrawer.DrawOverlay(this, OverlayTypes.ForbiddenBig);
            }
        }

        public abstract Job TryGiveJob();

        public override void Tick()
        {
            base.Tick();
            if(ConstructsLeft > 0 && !lockdown && this.IsHashIntervalTick(60) && GetComp<CompPowerTrader>()?.PowerOn != false)
            {
                Job job = TryGiveJob();
                if(job != null)
                {
                    job.playerForced = true;
                    job.expiryInterval = -1;
                    Pawn_Construct construct = MakeConstruct();
                    GenSpawn.Spawn(construct, Position, Map);
                    construct.jobs.StartJob(job);
                }
            }
        }

        public void Notify_ConstructMayBeLost(Pawn_Construct construct)
        {
            if (spawnedConstructs.Contains(construct))
            {
                spawnedConstructs.Remove(construct);
                Notify_ConstructLost();
            }
        }

        public override string GetInspectString()
        {
            StringBuilder builder = new StringBuilder();
            string str = base.GetInspectString();
            if (!string.IsNullOrEmpty(str))
            {
                builder.AppendLine(str);
            }
            builder.Append("Number of Constructs: " + ConstructsLeft);
            return builder.ToString();
        }

        public virtual Pawn_Construct MakeConstruct()
        {
            Pawn_Construct construct = (Pawn_Construct)PawnGenerator.GeneratePawn(ConstructDefOf.CanoptekScarab, Faction);
            construct.station = this;
            spawnedConstructs.Add(construct);
            return construct;
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo g in base.GetGizmos())
                yield return g;
            yield return new Command_Toggle()
            {
                defaultLabel = "ConstructStationLockdown".Translate(),
                defaultDesc = "ConstructStationLockdownDesc".Translate(),
                toggleAction = () =>
                {
                    lockdown = !lockdown;
                    if (lockdown)
                    {
                        foreach (Pawn_Construct construct in spawnedConstructs.ToList())
                        {
                            construct.jobs.StartJob(new Job(ConstructDefOf.Construct_ReturnToStation, this), JobCondition.InterruptForced);
                        }
                    }
                },
                isActive = () => lockdown,
                icon = Cancel
            };
        }
    }
}
