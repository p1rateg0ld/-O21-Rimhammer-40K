using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Rimhammer40k.Settings
{
    public class Rimhammer40kModSettings : ModSettings
    {
        public Rimhammer40kModSettings()
        {
            Rimhammer40kModSettings.Instance = this;
        }

        public override void ExposeData()
        {
            Scribe_Values.Look<bool>(ref this.FirstStartUp, "FirstStartUp", true, true);
            Scribe_Values.Look<bool>(ref this.UseCustomBackground, "UseCustomBackground", true, true);
            Scribe_Values.Look<bool>(ref this.NecronsTeleportOnDeath, "NecronsTeleportOnDeath", true, true);
            Scribe_Values.Look<bool>(ref this.AllowOrksHumanClothes, "AllowOrksHumanClothes", true, true);
        }

        public void ResetToDefault()
        {
            UseCustomBackground = true;
            FirstStartUp = true;
            NecronsTeleportOnDeath = true;
            AllowOrksHumanClothes = false;
        }

        public static Rimhammer40kModSettings Instance;

        public bool UseCustomBackground = true;

        public bool AllowOrksHumanClothes = false;

        public bool NecronsTeleportOnDeath = true;

        public bool FirstStartUp = true;

        public void SetBool(ref bool b, bool set)
        {
            b = set;
        }

        public void SetValue(ref int i, int set)
        {
            i = set;
        }
    }
}
