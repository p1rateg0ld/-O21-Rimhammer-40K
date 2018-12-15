using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace Rimhammer40k.Necrons
{
    class CompSpawnNecron : ThingComp
    {
        public CompProperties_SpawnNecron Spawnprops
        {
            get
            {
                return this.props as CompProperties_SpawnNecron;
            }
        }

        public override void CompTick()
        {
            this.CheckShouldSpawn();
        }

        private void CheckShouldSpawn()
        {
            if (true)
            {
                this.SpawnNecron();
                this.parent.Destroy();
            }
        }

        public void SpawnNecron()
        {
            PawnGenerationRequest request = new PawnGenerationRequest(this.Spawnprops.PawnKind, Faction.OfPlayer, PawnGenerationContext.NonPlayer);
            Pawn pawn = PawnGenerator.GeneratePawn(request);

            GenSpawn.Spawn(pawn, parent.Position, parent.Map);
        }
    }
}
