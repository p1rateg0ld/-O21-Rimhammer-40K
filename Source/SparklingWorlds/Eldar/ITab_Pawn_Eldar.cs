using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace Rimhammer40k
{
    class ITab_Pawn_Eldar : ITab
    {
        private Pawn PawnToShowInfoAbout
        {
            get
            {
                Pawn pawn = null;
                if(base.SelPawn != null)
                {
                    pawn = base.SelPawn;
                }
                else
                {
                    Corpse corpse = base.SelThing as Corpse;
                    if(corpse != null)
                    {
                        pawn = corpse.InnerPawn;
                    }
                }
                if(pawn == null)
                {
                    Log.Error("Character tab found no selected pawn to display.");
                    return null;
                }
                return pawn;
            }
        }

        /** public override bool IsVisible
        {
            get
            {
                if(base.SelPawn.story != null)
                {
                    return (base.SelPawn.story.traits.HasTrait(Rimhammer40kDefOf.R40kEldarTrait));
                }
                return false;
            }
        } **/

        public ITab_Pawn_Eldar()
        {
            this.size = EldarCardUtility.EldarCardSize + new Vector2(17f, 17f) * 2f;
            this.labelKey = "R40k_TabEldar";
        }

        protected override void FillTab()
        {
            Rect rect = new Rect(17f, 17f, EldarCardUtility.EldarCardSize.x, EldarCardUtility.EldarCardSize.y);
            EldarCardUtility.DrawEldarCard(rect, this.PawnToShowInfoAbout);
        }
    }
}
