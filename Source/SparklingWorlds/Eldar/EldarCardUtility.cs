using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RimWorld;
using Verse;
using Harmony;

namespace Rimhammer40k.Eldar
{
    public class EldarCardUtility
    {
        public static Vector2 eldarCardSize = default(Vector2);
        public static Vector2 EldarCardSize
        {
            get
            {
                if(eldarCardSize == default(Vector2))
                {
                    eldarCardSize = new Vector2(395f, 536f);
                    if(LanguageDatabase.activeLanguage == LanguageDatabase.AllLoadedLanguages.FirstOrDefault(x => x.folderName == "German"))
                    {
                        eldarCardSize = new Vector2(470f, 536f);
                    }
                }
                return eldarCardSize;
            }
        }

        public static float ButtonSize = 40f;

        public static float EldarButtonSize = 46f;

        public static float EldarButtonPointSize = 24f;

        public static float HeaderSize = 32f;

        public static float TextSize = 22f;

        public static float Padding = 3f;

        public static float SpacingOffset = 15f;

        public static float SectionOffset = 8f;

        public static float ColumnSize = 245f;

        public static float SkillsColumnHeight = 113f;

        public static float SkillsColumnDivider = 114f;

        public static float SkillsTextWidth = 138f;

        public static float SkillsBoxSize = 18f;

        public static float AbilitiesColumnHeight = 195f;

        public static float AbilitiesColumnWidth = 123f;

        public static bool adjustedForLanguage = false;
        public static void AdjustForLanguage()
        {
            if (!adjustedForLanguage)
            {
                adjustedForLanguage = true;
                if(LanguageDatabase.activeLanguage == LanguageDatabase.AllLoadedLanguages.FirstOrDefault(x => x.folderName == "German"))
                {
                    SkillsColumnDivider = 170f;
                    SkillsTextWidth = 170f;
                }
            }
        }

        public static void DrawEldarCard(Rect rect, Pawn pawn)
        {
            AdjustForLanguage();

            GUI.BeginGroup(rect);

            CompEldarPsyker compEldar = pawn.GetComp<CompEldarPsyker>();
            if(compEldar != null)
            {
                if(compEldar.EldarPsykerLevel > 0)
                {
                    float pathTextSize = TextCalcSize("R40k_Path".Translate()).x;
                    Rect rect2 = new Rect(((rect.width / 2) - pathTextSize) + SpacingOffset, rect.y, rect.width, HeaderSize);
                    Text.Font = GameFont.Medium;
                    Widgets.Label(rect2, "R40k_Path".Translate().CapitalizeFirst());
                    Text.Font = GameFont.Small;
                }
            }
        }
    }
}
