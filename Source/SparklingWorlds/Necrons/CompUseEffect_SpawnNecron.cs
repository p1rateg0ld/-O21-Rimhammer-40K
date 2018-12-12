using System;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace Rimhammer40k.Necrons
{
    public class CompUseEffect_SpawnNecron : CompUseEffect
    {
        public override float OrderPriority
        {
            get
            {
                return 1000f;
            }
        }

        public CompProperties_SpawnPawn SpawnerProps
        {
            get
            {
                return this.props as CompProperties_SpawnPawn;
            }
        }

        public virtual void DoSpawn(Pawn usedBy)
        {
            Pawn pawn = PawnGenerator.GeneratePawn(this.SpawnerProps.pawnKind, Faction.OfPlayer);
            bool flag = pawn != null;
            if (flag)
            {
                GenPlace.TryPlaceThing(pawn, this.parent.Position, this.parent.Map, ThingPlaceMode.Near, null);
                bool sendMessage = this.SpawnerProps.sendMessage;
                if (sendMessage)
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    Messages.Message("NecronSpawned".Translate(new object[] { pawn.Name }), new GlobalTargetInfo(pawn), MessageTypeDefOf.NeutralEvent);
#pragma warning restore CS0618 // Type or member is obsolete
                }
            }
        }

        public override void DoEffect(Pawn usedBy)
        {
            base.DoEffect(usedBy);
            for(int i = 0; i < this.SpawnerProps.amount; i++)
            {
                this.DoSpawn(usedBy);
            }
        }
    }
}
