using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using RimWorld;
using Verse;
using Verse.Sound;
using AbilityUser;


namespace Rimhammer40k.Psyker
{
    public class Comp_AbilityUserPsyker : CompAbilityUser
    {
        #region Variables
        private PsykerData psykerData = null;
        public PsykerData PsykerData
        {
            get
            {
                if(this.psykerData == null && this.IsPsyker)
                {
                    this.psykerData = new PsykerData(this);
                }

                return this.psykerData;
            }
        }
        #endregion

        #region Levels
        public int PsykerLevel
        {
            get
            {
                return this.PsykerData.Level;
            }

            set
            {
                if(value > this.PsykerData.Level)
                {
                    this.PsykerData.AbilityPoints++;
                    if(this.PsykerData.XP < value * 600)
                    {
                        this.PsykerData.XP = value * 600;
                    }
                }

                this.PsykerData.Level = value;
            }
        }

        public int PsykerXP
        {
            get
            {
                return this.PsykerData.XP;
            }

            set
            {
                this.PsykerData.XP = value;
            }
        }

        public float XPLastLevel
        {
            get
            {
                float result = 0f;
                if (this.PsykerLevel > 0) result = this.PsykerLevel * 600;
                return result;
            }
        }

        public int PsykerXPTillNextLevel
        {
            get
            {
                return (this.PsykerLevel + 1) * 600;
            }
        }

        public float XPTillNextLevelPercent
        {
            get
            {
                return (float)(this.PsykerXP - this.XPLastLevel) / (float)(this.PsykerXPTillNextLevel - this.XPLastLevel);
            }
        }

        public int PsykerSkillLevel(string skillName)
        {
            int result = 0;
            PsykerSkill skillCheck = this.PsykerData.Skills.FirstOrDefault((PsykerSkill x) => x.label == skillName);
            if(skillCheck != null)
            {
                result = skillCheck.level;
            }
            return result;
        }

        public void LevelUp(bool hideNotification = false)
        {
            this.PsykerLevel += 1;
            if (!hideNotification)
            {
                Messages.Message("PsykerLevelUp".Translate(new object[] { this.parent.Label }), MessageTypeDefOf.PositiveEvent);
            }
            SoundDef.Named("AstartesOvenDone").PlayOneShotOnCamera();
        }

        public void ResetPowers()
        {
            foreach(PsykerSkill skill in this.PsykerData.Skills)
            {
                this.PsykerData.AbilityPoints += skill.level;
                skill.level = 0;
            }

            foreach(PsykerPower power in this.PsykerData.PsykerPowersBiomancy)
            {
                power.level = 0;
            }

            foreach (PsykerPower power in this.PsykerData.PsykerPowersDivination)
            {
                power.level = 0;
            }

            foreach (PsykerPower power in this.PsykerData.PsykerPowersPyromancy)
            {
                power.level = 0;
            }

            foreach (PsykerPower power in this.PsykerData.PsykerPowersTelekinesis)
            {
                power.level = 0;
            }

            foreach (PsykerPower power in this.PsykerData.PsykerPowersTelepathy)
            {
                power.level = 0;
            }

            List<PsykerAbility> tempList = new List<PsykerAbility>();
            foreach (PawnAbility ab in this.AbilityData.Powers)
            {
                tempList.Add(ab as PsykerAbility);
            }

            foreach(PsykerAbility ability in tempList)
            {
                this.RemovePawnAbility(ability.Def);
            }
        }
        #endregion
    }
}
