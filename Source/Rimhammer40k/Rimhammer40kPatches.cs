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

        [HarmonyPatch(typeof(UI_BackgroundMain)), HarmonyPatch("BackgroundOnGUI"), StaticConstructorOnStartup]
        internal static class Custom_UI_BackgroundMain
        {
            private static readonly Texture2D Custom_Background = ContentFinder<Texture2D>.Get("ui/heroart/RimhammerNecronBG", true);

            internal static readonly Vector2 MainBackgroundSize = new Vector2(2048f, 1280f);

            private static bool Prefix()
            {
                if (Rimhammer40kSettings.settings.UseCustomBackground)
                {
                    if (Custom_Background)
                    {
                        float width = (float)UI.screenWidth;
                        float num = (float)UI.screenWidth * (MainBackgroundSize.y / MainBackgroundSize.x);
                        GUI.DrawTexture(new Rect(0f, (float)UI.screenHeight / 2f - num / 2f, width, num), Custom_Background, ScaleMode.ScaleToFit, true);
                    }
                    return false;
                }
                return true;
            }
        }
    }
}
