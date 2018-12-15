using System;
using RimWorld;
using Verse;

namespace Rimhammer40k.Necrons
{
    public class CompProperties_SpawnNecron : CompProperties
    {
        public CompProperties_SpawnNecron()
        {
            this.compClass = typeof(CompSpawnNecron);
        }

        public PawnKindDef PawnKind;
    }
}
