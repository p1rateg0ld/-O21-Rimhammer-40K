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
    public class PsykerAbilityDef : AbilityUser.AbilityDef
    {
        public float psykerPoolCost = 0.0f;
        public int abilityPoints = 1;

        public string GetPointDesc()
        {
            string result = "";
            StringBuilder s = new StringBuilder();
            s.AppendLine()
        }
    }
}
