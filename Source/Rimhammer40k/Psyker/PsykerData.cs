using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using RimWorld;
using Verse;

namespace Rimhammer40k.Psyker
{
    public class PsykerData : IExposable
    {
        #region Variables
        private Pawn pawn;
        private int level = 0;
        private int xp = 1;
        private int abilityPoints = 0;
        private int ticksUntilXPGain = -1;
        private int ticksUntilMeditate = 0;

        private bool initialized = false;
        public bool psykerPowersInitialized = false;

        private List<PsykerSkill> skills;
        private List<PsykerPower> psykerPowersBiomancy;
        private List<PsykerPower> psykerPowersDivination;
        private List<PsykerPower> psykerPowersPyromancy;
        private List<PsykerPower> psykerPowersTelekinesis;
        private List<PsykerPower> psykerPowersTelepathy;

        public bool TabResolved = false;
        #endregion

        #region Properties
        public bool PsykerPowersInitialized { get => psykerPowersInitialized; set => psykerPowersInitialized = value; }

        public int Level { get => level; set => level = value; }
        public int XP { get => xp; set => xp = value; }
        public int AbilityPoints { get => abilityPoints; set => abilityPoints = value; }
        public int TicksUntilXPGain { get => ticksUntilXPGain; set => ticksUntilXPGain = value; }
        public int TicksUntilMeditate { get => ticksUntilMeditate; set => ticksUntilMeditate = value; }
        public Pawn Pawn => pawn;

        public List<PsykerSkill> Skills
        {
            get
            {
                if(skills == null)
                {
                    skills = new List<PsykerSkill>
                    {
                        new PsykerSkill("Psyker_OffensiveAbility", "Psyker_OffensiveAbility_Desc"),
                        new PsykerSkill("Psyker_DefensiveAbility", "Psyker_DefensiveAbility_Desc"),
                        new PsykerSkill("Psyker_EnergyPool", "Psyker_EnergyPool_Desc")
                    };
                }
                return skills;
            }
        }

        public List<PsykerPower> PsykerPowersBiomancy
        {
            get
            {
                return psykerPowersBiomancy;
            }
        }

        public List<PsykerPower> PsykerPowersDivination
        {
            get
            {
                return psykerPowersDivination;
            }
        }

        public List<PsykerPower> PsykerPowersPyromancy
        {
            get
            {
                return psykerPowersPyromancy;
            }
        }

        public List<PsykerPower> PsykerPowersTelekinesis
        {
            get
            {
                return psykerPowersTelekinesis;
            }
        }

        public List<PsykerPower> PsykerPowersTelepathy
        {
            get
            {
                return psykerPowersTelepathy;
            }
        }
        #endregion

        public PsykerData()
        {

        }

        public PsykerData(Comp_AbilityUserPsyker newUser)
        {
            this.pawn = newUser.AbilityUser;
        }

        public void ExposeData()
        {
            Scribe_References.Look<Pawn>(ref this.pawn, "psykerDataPawn");
            Scribe_Values.Look<int>(ref this.level, "psykerDataLevel", 0);
            Scribe_Values.Look<int>(ref this.xp, "psykerDataXp");
            Scribe_Values.Look<bool>(ref this.psykerPowersInitialized, "psykerDataPowersInitialized", true);
            Scribe_Values.Look<int>(ref this.abilityPoints, "psykerDataAbilityPoints", 0);
            Scribe_Values.Look<int>(ref this.ticksUntilMeditate, "psykerDataTicksUntilMeditate", 0);
            Scribe_Values.Look<int>(ref this.ticksUntilXPGain, "psykerDataTicksUntilXPGain", -1);
            Scribe_Collections.Look<PsykerSkill>(ref this.skills, "psykerDataSkills", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<PsykerPower>(ref this.psykerPowersBiomancy, "psykerDataPowersBiomancy", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<PsykerPower>(ref this.psykerPowersDivination, "psykerDataPowersDivination", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<PsykerPower>(ref this.psykerPowersPyromancy, "psykerDataPowersPyromancy", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<PsykerPower>(ref this.psykerPowersTelekinesis, "psykerDataPowersTelekinesis", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<PsykerPower>(ref this.psykerPowersTelepathy, "psykerDataPowersTelepathy", LookMode.Deep, new object[0]);
        }
    }
}
