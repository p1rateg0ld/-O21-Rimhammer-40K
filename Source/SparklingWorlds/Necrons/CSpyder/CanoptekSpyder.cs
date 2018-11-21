using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;

namespace Rimhammer40k.Necrons.CSpyder
{
    public class CanoptekSpyderKindDef : PawnKindDef
    {
        public float maxStorage;

        public ThingDef destroyedThingDef;
    }

    [StaticConstructorOnStartup]
    public class CanoptekSpyder : MechanicalPawn
    {
        public new CanoptekSpyderKindDef kindDef;
        public Building_EnergyStoragePylon mainEnergyStoragePylon;
        public List<Building_EnergyStoragePylon> availableStoragePylons = new List<Building_EnergyStoragePylon>();

        private IntVec3 homePosition = IntVec3.Invalid;
    }
}
