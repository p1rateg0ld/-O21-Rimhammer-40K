using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Rimhammer40k
{
    [StaticConstructorOnStartup]
    public class MainTabWindow_Rimhammer40k : MainTabWindow
    {
        public Rimhammer40kMenuItemDef SelectedMenuItem;

        public MainTabWindow_Rimhammer40k()
        {
            this.soundAppear = null;
            this.soundClose = null;
            this.doCloseButton = false;
            this.doCloseX = false;
            this.preventCameraMotion = false;
            this.absorbInputAroundWindow = false;
            this.closeOnClickedOutside = true;
        }

        protected override float Margin
        {
            get
            {
                return 5f;
            }
        }

        public override Vector2 RequestedTabSize
        {
            get
            {
                return new Vector2(1180f, (float)UI.screenHeight * 0.7f);
            }
        }

        public override void PreOpen()
        {
            base.PreOpen();
            this.MenuItems = Rimhammer40kUtils.GetMenuItems();
        }

        public List<Rimhammer40kMenuItemDef> MenuItemsAvailable
        {
            get
            {
                return (from x in this.MenuItems.AllMenuItems
                        where x.PrerequisitesCompleted || (!x.Hidden && x.IsAvailable)
                        select x).ToList<Rimhammer40kMenuItemDef>();
            }
        }

        public override void DoWindowContents(Rect inRect)
        {
            if(this.SelectedMenuItem == null)
            {
                this.SelectedMenuItem = this.MenuItemsAvailable.FirstOrDefault((Rimhammer40kMenuItemDef x) => x.CanViewNow);
            }
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.MiddleLeft;
            Rect rect = inRect;
            rect.width = 300f;
            Rect rect2 = inRect;
            rect2.width -= 300f;
            rect2.x = rect.xMax;
            rect = rect.ContractedBy(10f);
            rect2 = rect2.ContractedBy(10f);
            Widgets.DrawMenuSection(rect);
            Widgets.DrawTextureFitted(new Rect(rect.x, rect.y, rect.width, 64f).ContractedBy(2f), ContentFinder<Texture2D>.Get("UI/Menu/ModLogo", false), 1f);
            rect.y += 64f;
            rect.height -= 64f;
            rect = rect.ContractedBy(4f);
            float num = 30f;
            float height = num * (float)this.MenuItemsAvailable.Count<Rimhammer40kMenuItemDef>();
            Rect viewRect = new Rect(0f, 0f, rect.width - 16f, height);
            GUI.BeginGroup(rect);
            Widgets.BeginScrollView(new Rect(0f, 0f, rect.width, rect.height), ref this.scrollPosLeft, viewRect, true);
            int num2 = 0;
            foreach(Rimhammer40kMenuItemDef rimhammer40KMenuItemDef in this.MenuItemsAvailable)
            {
                if(!rimhammer40KMenuItemDef.Hidden || !rimhammer40KMenuItemDef.IsAvailable)
                {
                    Rect rect3 = new Rect(0f, (float)num2, rect.width, num);
                    this.DoAreaRow(rect3, rimhammer40KMenuItemDef);
                    num2 += (int)num;
                }
            }
            Widgets.EndScrollView();
            GUI.EndGroup();
            GUI.BeginGroup(rect2);
            if(this.SelectedMenuItem != null)
            {
                this.DrawPanel(new Rect(0f, 0f, rect2.width, rect2.height));
            }
            GUI.EndGroup();
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;
        }
    }
}
