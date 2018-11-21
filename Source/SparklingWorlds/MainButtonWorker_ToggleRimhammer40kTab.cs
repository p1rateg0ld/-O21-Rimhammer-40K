using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace Rimhammer40k
{
    internal class MainButtonWorker_ToggleRimhammer40kTab : MainButtonWorker_ToggleTab
    {
        public override void DoButton(Rect rect)
        {
            Text.Font = GameFont.Small;
            string text = this.def.LabelCap;
            float num = this.def.LabelCapWidth;
            if (num > rect.width - 2f)
            {
                text = this.def.ShortenedLabelCap;
                num = this.def.ShortenedLabelCapWidth;
            }
            if ((!this.def.validWithoutMap || this.def == MainButtonDefOf.World) && Find.CurrentMap == null)
            {
                Widgets.DrawAtlas(rect, Widgets.ButtonSubtleAtlas);
                if(Event.current.type == EventType.MouseDown && Mouse.IsOver(rect))
                {
                    Event.current.Use();
                    return;
                }
            }
            else
            {
                bool flag = num > 0.85f * rect.width - 1f;
                Rect rect2 = rect;
                string label = text;
                float textLeftMargin = (!flag) ? -1f : 2f;
                if (Widgets.ButtonTextSubtle(rect2, label, 0f, textLeftMargin, SoundDefOf.Mouseover_Category, default(Vector2)))
                {
                    this.InterfaceTryActivate();
                }
                if (Find.MainTabsRoot.OpenTab != this.def && !Find.WindowStack.NonImmediateDialogWindowOpen)
                {
                    UIHighlighter.HighlightOpportunity(rect, this.def.cachedHighlightTagClosed);
                }
                if (!this.def.description.NullOrEmpty())
                {
                    TooltipHandler.TipRegion(rect, this.def.description);
                }
            }
        }
    }
}
