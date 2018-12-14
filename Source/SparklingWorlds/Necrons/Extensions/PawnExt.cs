using System;
using Verse;
using RimWorld;

namespace Rimhammer40k.Necrons.Extensions
{
    [StaticConstructorOnStartup]
    static public class PawnExt
    {
        static readonly public FleshTypeDef necronFlesh;

        static PawnExt()
        {
            necronFlesh = DefDatabase<FleshTypeDef>.GetNamed("Necron");
        }

        static public bool IsNecron(this Pawn pawn)
        {
            return pawn.RaceProps.FleshType == necronFlesh;
        }

        static public bool IsNotNecron(this Pawn pawn)
        {
            return pawn.RaceProps.FleshType != necronFlesh;
        }
    }
}
