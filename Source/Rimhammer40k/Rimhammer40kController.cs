using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using RimWorld;
using Verse;

using HugsLib;

namespace Rimhammer40k
{
    [EarlyInit]
    public class Rimhammer40kController : ModBase
    {
        public override string ModIdentifier
        {
            get { return "Rimhammer40k"; }
        }

        public override void EarlyInitalize()
        {
            Logger.Message(":: Rimhammer40k Active ::");
        }
    }
}
