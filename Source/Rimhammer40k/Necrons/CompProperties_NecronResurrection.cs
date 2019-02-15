using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Rimhammer40k.Necrons
{

    public class CompProperties_NecronResurrection : CompProperties
    {
        public CompProperties_NecronResurrection()
        {
            this.compClass = typeof(CompNecronResurrection);
        }

        public CompProperties_NecronResurrection(float daysToResurrection)
        {
            this.daysToResurrection = daysToResurrection;
        }
        
        public int TicksToResurrection
        {
            get
            {
                return Mathf.RoundToInt(this.daysToResurrection * 60000f);
            }
        }

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            foreach (string e in base.ConfigErrors(parentDef))
            {
                yield return e;
            }
            if (parentDef.tickerType != TickerType.Normal && parentDef.tickerType != TickerType.Rare)
            {
                yield return string.Concat(new object[]
                {
                    "CompNecronResurrection needs tickerType ",
                    TickerType.Rare,
                    " or ",
                    TickerType.Normal,
                    ", has ",
                    parentDef.tickerType
                });
            }
            yield break;
        }

        public float daysToResurrection = 1f;
    }
}
