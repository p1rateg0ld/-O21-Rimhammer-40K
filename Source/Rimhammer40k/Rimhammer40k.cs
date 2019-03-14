using Harmony;
using System.Reflection;
using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using RimWorld.BaseGen;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace Rimhammer40k
{

    [StaticConstructorOnStartup]
    public class Rimhammer40kMod : Mod
    {
        public Rimhammer40kMod(ModContentPack content) : base(content)
        {
            base.GetSettings<Settings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            Settings.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "Rimhammer 40k";
        }

        public static int maxOrkPopulation;

        public static int maxGrotPopulation;

        public static bool necronsTeleportOnDeath;
    }
}