using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using RimWorld;
using Verse;

namespace Rimhammer40k.Psyker
{
    class PsykerCardUtility
    {
        public static Vector2 PsykerCardSize = new Vector2(700f, 556f);

        public static float ButtonSize = 40f;
        public static float PsykerButtonSize = 46f;
        public static float PsykerButtonPointSize = 24f;
        public static float HeaderSize = 24f;

        public static float TextSize = 22f;
        public static float Padding = 3f;
        public static float SpacingOffset = 22f;
        public static float SectionOffset = 8f;

        public static float ColumnSize = 245f;
        public static float SkillsColumnHeight = 113f;
        public static float SkillsColumnDivider = 114f;
        public static float SkillsTextWidth = 138f;

        public static float SkillsBoxSize = 18f;
        public static float PowersColumnHeight = 195f;
        public static float PowersColumnWidth = 123f;

        public static void DrawPsykerCard(Rect rect, Pawn pawn)
        {
            GUI.BeginGroup(rect);

            Comp_AbilityUserPsyker compPsyker = pawn.GetComp<Comp_AbilityUserPsyker>();
            if(compPsyker != null)
            {
                float skillsTextSize = Text.CalcSize("O21_Psyker_Skills".Translate()).x;
                Rect rectSkillsLabel = new Rect((PsykerCardSize.x / 2) - skillsTextSize, rect.yMax + SectionOffset, rect.width, HeaderSize);
                Text.Font = GameFont.Medium;
                Widgets.Label(rectSkillsLabel, "O21_Psyker_Skills".Translate().CapitalizeFirst());
                Text.Font = GameFont.Small;

            }
        }
    }
}
