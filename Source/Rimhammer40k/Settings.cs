using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using UnityEngine;
using RimWorld;
using Verse;

using HugsLib;

namespace Rimhammer40k
{
    public class Settings : ModSettings
    {
        public Settings()
        {
            Settings.Instance = this;
        }

        public static Settings Instance;

        public static Dictionary<string, object> values = new Dictionary<string, object>();
        
        private static Vector2 _scrollposition = Vector2.zero;
        
        private static float _settingsHeight = 999f;
        
        private static float rowHeight = 24f;
        
        private static float rowMargin = 6f;

        public static void DoSettingsWindowContents(Rect canvas)
        {
            Widgets.BeginScrollView(canvas, ref Settings._scrollposition, new Rect(0f, 0f, canvas.width - 19f, Settings._settingsHeight), true);
            float num = 0f;
            Text.Font = GameFont.Medium;
            Text.Anchor = UnityEngine.TextAnchor.LowerLeft;
            Widgets.Label(new Rect(0f, num, canvas.width, Settings.rowHeight * 2f), "Population Limits");
            Text.Anchor = 0;
            Text.Font = GameFont.Small;
            num += Settings.rowHeight * 2f;
            Settings.DrawLabeledInput(ref num, canvas, "Max Ork Population", ref Rimhammer40kMod.maxOrkPopulation, "Maximum Orks that will spawn from spores. Default: 10");
            Settings.DrawLabeledInput(ref num, canvas, "Max Grot Population", ref Rimhammer40kMod.maxGrotPopulation, "Maximum Grots that will spawn from spores. Default: 5");
            Text.Anchor = 0;
            Text.Font = GameFont.Small;
            string text = "Maximum populations will only limit how many of each will spawn from spores, it does not affect Storyteller soft cap and does not take into account non Ork/Grot pawns!";
            Text.Font = 0;
            float num2 = Text.CalcHeight(text, canvas.width);
            Widgets.Label(new Rect(0f, num, canvas.width, num2), text);
            Text.Font = GameFont.Small;
            num += num2;
            Settings._settingsHeight = num;
            Widgets.EndScrollView();
        }

        public override void ExposeData()
        {
            Scribe_Values.Look<bool>(ref Rimhammer40kMod.necronsTeleportOnDeath, "NecronsTeleportOnDeath", true, true);
            Scribe_Values.Look<int>(ref Rimhammer40kMod.maxOrkPopulation, "MaxOrkPopulation", 10, true);
            Scribe_Values.Look<int>(ref Rimhammer40kMod.maxGrotPopulation, "MaxGrotPopulation", 5, true);
        }

        public static void DrawLabeledInput(ref float curY, Rect canvas, string label, ref float value, string tip = "")
        {
            Widgets.Label(new Rect(0f, curY, canvas.width / 3f * 2f, Settings.rowHeight), label);
            if (!Settings.values.ContainsKey(label))
            {
                Settings.values.Add(label, value);
            }
            GUI.SetNextControlName(label);
            Settings.values[label] = Widgets.TextField(new Rect(canvas.width / 3f * 2f, curY, canvas.width / 3f, Settings.rowHeight), Settings.values[label].ToString());
            if (tip != "")
            {
                TooltipHandler.TipRegion(new Rect(0f, curY, canvas.width, Settings.rowHeight), tip);
            }
            if (GUI.GetNameOfFocusedControl() != label && !float.TryParse(Settings.values[label].ToString(), out value))
            {
                Messages.Message("Invalid Float", MessageTypeDefOf.RejectInput, true);
                Settings.values[label] = value;
            }
            curY += Settings.rowHeight + Settings.rowMargin;
        }

        public static void DrawLabeledInput(ref float curY, Rect canvas, string label, ref int value, string tip = "")
        {
            Widgets.Label(new Rect(0f, curY, canvas.width / 3f * 2f, Settings.rowHeight), label);
            if (!Settings.values.ContainsKey(label))
            {
                Settings.values.Add(label, value);
            }
            GUI.SetNextControlName(label);
            Settings.values[label] = Widgets.TextField(new Rect(canvas.width / 3f * 2f, curY, canvas.width / 3f, Settings.rowHeight), Settings.values[label].ToString());
            if (tip != "")
            {
                TooltipHandler.TipRegion(new Rect(0f, curY, canvas.width, Settings.rowHeight), tip);
            }
            if (GUI.GetNameOfFocusedControl() != label && !int.TryParse(Settings.values[label].ToString(), out value))
            {
                Messages.Message("Invalid Integer", MessageTypeDefOf.RejectInput, true);
                Settings.values[label] = value;
            }
            curY += Settings.rowHeight + Settings.rowMargin;
        }

        public void SetBool(ref bool b, bool set)
        {
            b = set;
        }

        public void SetValue(ref int i, int set)
        {
            i = set;
        }
    }
}
