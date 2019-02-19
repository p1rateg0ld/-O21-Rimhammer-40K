using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using RimWorld;
using Verse;

namespace Rimhammer40k.Psyker
{
    public class PsykerSkill : IExposable
    {
        public string label;
        public string desc;
        public int level;

        public PsykerSkill()
        {

        }

        public PsykerSkill(String newLabel, String newDesc)
        {
            label = newLabel;
            desc = newDesc;
            level = 0;
        }

        public void ExposeData()
        {
            Scribe_Values.Look<string>(ref label, "label", "default");
            Scribe_Values.Look<string>(ref desc, "desc", "default");
            Scribe_Values.Look<int>(ref level, "level", 0);

        }
    }
}
