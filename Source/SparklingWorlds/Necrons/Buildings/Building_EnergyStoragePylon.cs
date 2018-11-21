using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;
using Rimhammer40k.Necrons.CSpyder;

namespace Rimhammer40k.Necrons.Buildings
{
    public class EnergyStoragePylonDef : GraphicSwitchableDef
    {
        public PawnKindDef canoptekSpyder;
        public float refineTime = 3;
    }

    public class Building_EnergyStoragePylon : Building_GraphicSwitchable
    {
        public new EnergyStoragePylonDef def;

        public List<CanoptekSpyder> canoptekSpyderList = new List<CanoptekSpyder>();

        public Comp_GNW NetworkComp;

        public int refineTicks;

        public override bool IsActivated => false;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look<EnergyStoragePylonDef>(ref def, "def");
            Scribe_Collections.Look<CanoptekSpyder>(ref canoptekSpyderList, "canoptekSpyderList", LookMode.Reference);
            Scribe_Values.Look<int>(ref refineTicks, "refineTicks");
        }

        public override void Tick()
        {
            base.Tick();
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            def = (EnergyStoragePylonDef)base.def;
            NetworkComp = this.GetComp<Comp_GNW>();
            if (!respawningAfterLoad)
            {
                CanoptekSpyder canoptekSpyder = SpawnCanoptekSpyder();
                canoptekSpyder.UpdateStoragePylonOrAddNewMain(null);

                UpdateOrAddMissingCanoptekSpyders();
            }
            this.refineTicks = GenTicks.SecondsToTicks(this.def.refineTime);
        }
    }
}
