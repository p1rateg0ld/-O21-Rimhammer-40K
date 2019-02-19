using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using RimWorld;
using Verse;
using AbilityUser;

namespace Rimhammer40k.Psyker
{
    public class PsykerAbility : PawnAbility
    {
        public Comp_AbilityUserPsyker Psyker => PsykerUtility.GetPsyker(this.Pawn);
        public PsykerAbilityDef PsykerDef => Def as PsykerAbilityDef;
    }
}
