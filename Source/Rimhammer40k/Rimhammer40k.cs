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
    public static class Rimhammer40kSettings
    {
        public static Settings.Rimhammer40kModSettings settings;
    }

    [StaticConstructorOnStartup]
    public class Rimhammer40kMod : Mod
    {
        public Settings.Rimhammer40kModSettings settings;

        public Vector2 scrollPos = new Vector2();

        public Rimhammer40kMod(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<Settings.Rimhammer40kModSettings>();
            Rimhammer40kSettings.settings = this.settings;
        }

        public override string SettingsCategory() => "Rimhammer 40k";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            //TODO: fix spacing for all languages
            float yOff = 25f;
            Widgets.DrawLine(new Vector2(inRect.width / 2, inRect.y - 7f), new Vector2(inRect.width / 2, inRect.height + 75f), Color.gray, 1f);
            Rect winRect = new Rect(0f, yOff, inRect.width, inRect.height);
            Rect leftSide = new Rect(0f, winRect.y, winRect.width / 2, winRect.height);

            //GUI.BeginGroup(leftSide);
            MakeTitle(new Rect(5f, leftSide.y + 7f, leftSide.width, 17f), "SettingsGeneral".Translate());
            Rect rect1 = new Rect(0f, 20f, leftSide.width, 50).ContractedBy(5f);
            MakeNewCheckBox(rect1, "UseCustomBackgroundLabel".Translate(), ref this.settings.UseCustomBackground, out rect1, "UseCustomBackgroundDesc".Translate(), false, yOff);
            MakeNewCheckBox(rect1, "AllowOrksHumanClothesLabel".Translate(), ref this.settings.AllowOrksHumanClothes, out rect1, "AllowOrksHumanClothesDesc".Translate(), false, yOff);
            //GUI.EndGroup();

            if (Widgets.ButtonText(new Rect(0f, inRect.height + 35f, 125f, 45f), "Reset Default"))
            {
                Rimhammer40kSettings.settings.ResetToDefault();
            }
        }

        public void MakeNewCheckBox(Rect rect, string label, ref bool checkOn, out Rect newRect, string desc = null, bool disabled = false, float yOffset = 0f, float xOffset = 0f)
        {
            Rect rect2 = rect;
            rect2.y += yOffset;
            rect2.x += xOffset;
            Widgets.CheckboxLabeled(rect2.ContractedBy(10f), label, ref checkOn, disabled);
            if (desc != null)
            {
                TooltipHandler.TipRegion(rect2.ContractedBy(10f), desc);
            }
            newRect = rect2;
        }

        public void MakeTitle(Rect rect, string label)
        {
            Text.Font = GameFont.Tiny;
            Widgets.Label(rect, label);
            Widgets.DrawLine(new Vector2(rect.xMin, rect.yMax), new Vector2(rect.xMin + rect.width, rect.yMax), Color.gray, 1f);
            Text.Font = GameFont.Small;
        }
    }
}