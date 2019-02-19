using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using RimWorld;
using Verse;

namespace Rimhammer40k.Psyker
{
    public class ITab_Psyker : ITab
    {
        private Pawn PawnToShowInfoAbout
        {
            get
            {
                Pawn pawn = null;
                bool flag = base.SelPawn != null;
                if (flag)
                {
                    pawn = base.SelPawn;
                }
                else
                {
                    Corpse corpse = base.SelThing as Corpse;
                    bool flag2 = corpse != null;
                    if (flag2)
                    {
                        pawn = corpse.InnerPawn;
                    }
                }
                bool flag3 = pawn == null;
                Pawn result;
                if (flag3)
                {
                    Log.Error("Psyker tab found no selected pawn to display.");
                    result = null;
                }
                else
                {
                    result = pawn;
                }
                return result;
            }
        }

        public override bool IsVisible
        {
            get
            {
                bool flag = base.SelPawn.IsColonist &&
                    (base.SelPawn.health.hediffSet.HasHediff(HediffDef.Named("PsykerBiomancy")) ||
                    base.SelPawn.health.hediffSet.HasHediff(HediffDef.Named("PsykerDivination")) ||
                    base.SelPawn.health.hediffSet.HasHediff(HediffDef.Named("PsykerPyromancy")) ||
                    base.SelPawn.health.hediffSet.HasHediff(HediffDef.Named("PsykerTelekinesis")) ||
                    base.SelPawn.health.hediffSet.HasHediff(HediffDef.Named("PsykerTelepathy")));
                return flag;
            }
        }

        public ITab_Psyker()
        {
            this.size = PsykerCardUtility.psykerCardSize + new Vector2(17f, 17f) * 2f;
            this.labelKey = "O21_TabPsyker";
        }

        protected override void FillTab()
        {
            Rect rect = new Rect(17f, 17f, PsykerCardUtility.psykerCardSize.x, PsykerCardUtility.psykerCardSize.y);
            PsykerCardUtility.DrawPsykerCard(rect, this.PawnToShowInfoAbout);
        }
    }
}
