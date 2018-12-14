using System;
using RimWorld;
using Verse;

namespace Rimhammer40k.Necrons
{
    public class CompProperties_SpawnPawn : CompProperties_UseEffect
    {
        public CompProperties_SpawnPawn()
        {
            this.compClass = typeof(CompUseEffect_SpawnNecron);
        }

        public PawnKindDef pawnKind;

        public int amount = 1;

        public FactionDef forcedFaction;

        public bool usePlayerFaction = true;

        public string pawnSpawnedStrignKey = "NecronSpawnedMessageText";

        public bool sendMessage = true;
    }
}
