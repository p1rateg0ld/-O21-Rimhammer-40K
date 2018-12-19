using System;
using Verse;
using RimWorld;

namespace Rimhammer40k.Orks.Extensions
{
    [StaticConstructorOnStartup]
    static public class PawnExt
    {
        static readonly public FleshTypeDef orkFlesh;

        static PawnExt()
        {
            orkFlesh = DefDatabase<FleshTypeDef>.GetNamed("Ork");
        }

        static public bool IsOrk(this Pawn pawn)
        {
            return pawn.RaceProps.FleshType == orkFlesh;
        }

        static public bool IsNotOrk(this Pawn pawn)
        {
            return pawn.RaceProps.FleshType != orkFlesh;
        }
    }
}
