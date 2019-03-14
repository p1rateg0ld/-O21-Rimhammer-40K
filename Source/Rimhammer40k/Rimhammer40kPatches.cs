using Harmony;
using System;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using RimWorld.BaseGen;
using Verse;
using Verse.AI.Group;
using UnityEngine;

namespace Rimhammer40k
{
    [StaticConstructorOnStartup]
    public static class Rimhammer40kPatches
    {
        static Rimhammer40kPatches()
        {
            HarmonyInstance Rimhammer40k = HarmonyInstance.Create("com.rimhammer40k.rimworld.mod");

            Rimhammer40k.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
