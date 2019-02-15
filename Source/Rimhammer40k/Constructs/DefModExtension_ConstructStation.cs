using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Rimhammer40k.Constructs
{
    public class DefModExtension_ConstructStation : DefModExtension
    {
        public int maxNumConstructs;
        public bool displayDormantConstructs;
        public List<WorkTypeDef> workTypes;
    }
}