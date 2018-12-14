using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;
using Verse;
using RimWorld;
using Rimhammer40k.Necrons.Extensions;

namespace Rimhammer40k.Necrons.Patches
{
    [HarmonyPatch(typeof(RimWorld.HealthCardUtility))]
    [HarmonyPatch(nameof(RimWorld.HealthCardUtility.DrawHediffListing))]
    static public class HealthCardUtility_DrawHediffListing
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo labelHelper = AccessTools.Method(typeof(HealthCardUtility_DrawHediffListing)
                                            , nameof(HealthCardUtility_DrawHediffListing.TransformToLeakingIfFemale));

            foreach (var code in instructions)
                if (code.opcode == OpCodes.Ldstr && (string)code.operand == "BleedingRate")
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_1);  //Pawn on stack
                    yield return new CodeInstruction(OpCodes.Call, labelHelper); //Consume 1, return string
                }
                else
                    yield return code;
        }

        static public string TransformToLeakingIfFemale(Pawn pawn)
        {
            if (pawn.IsNecron())
                return "AT_LeakingRate";
            return "BleedingRate";
        }
    }
}
