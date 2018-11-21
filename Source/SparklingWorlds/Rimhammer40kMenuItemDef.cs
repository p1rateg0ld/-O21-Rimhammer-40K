using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

namespace Rimhammer40k
{
    class Rimhammer40kMenuItemDef : Def
    {
        private string MenuItemImageURL = "";
        private string MenuItemIconURL = "";
        public string MenuItemLabel = "";
        public string MenuItemDescription = "";

        public virtual Texture2D MenuItemImage
        {
            get
            {
                Texture2D texture2D = ContentFinder<Texture2D>.Get(this.MenuItemImageURL, false);
                if (!texture2D)
                {
                    return this.MenuItemIcon;
                }
                return texture2D;
            }
        }

        public virtual Texture2D MenuItemIcon
        {
            get
            {
                Texture2D texture2D = ContentFinder<Texture2D>.Get(this.MenuItemIconURL, false);
                if (!texture2D)
                {
                    return ContentFinder<Texture2D>.Get("UI/Menu/NoImage", false);
                }
                return texture2D;
            }
        }

        public virtual bool IsAvailable
        {

        }
    }
}
