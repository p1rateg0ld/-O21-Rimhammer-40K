using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Harmony;
using Rimhammer40k.Necrons.Extensions;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Rimhammer40k.Necrons
{
    class HarmonyPatches
    {
        public static bool Patch_PawnUtility_HumanFilthChancePerCell(float __result, ThingDef def)
        {
            bool flag = def.HasModExtension<NecronPawnProperties>();
            return !flag;
        }

        public static bool Patch_ThoughtWorker_NeedFood_CurrentStateInternal(ref ThoughtState __result, Pawn p)
        {
            bool flag = p.IsNecron();
            bool result;
            if (flag)
            {
                __result = ThoughtState.Inactive;
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }

        public static bool Patch_SkillRecord_Interval(SkillRecord __instance)
        {
            Pawn pawn = (Pawn)AccessTools.Field(typeof(SkillRecord), "pawn").GetValue(__instance);
            NecronPawnProperties modExtension;
            bool flag = (modExtension = pawn.def.GetModExtension<NecronPawnProperties>()) != null && modExtension.noSkillLoss;
            return !flag;
        }
    }
}
