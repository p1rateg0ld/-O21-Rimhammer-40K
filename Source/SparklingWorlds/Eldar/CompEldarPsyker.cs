using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using AbilityUser;

using RimWorld;

using UnityEngine;

using Verse;
using Verse.Sound;

namespace Rimhammer40k.Eldar
{
    public class CompEldarPsyker : CompAbilityUser
    {
        #region Variables

        private PsykerData psykerData = null;

        public PsykerData PsykerData
        {
            get
            {
                if(this.psykerData == null && this.IsEldarPsyker)
                {
                    this.psykerData = new PsykerData(this);
                }

                return this.psykerData;
            }
        }

        #endregion Variables

        #region Levels

        public int EldarPsykerLevel
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

        public int EldarPsykerXP
        {
            get
            {
                return this.PsykerData.XP;
            }
        }

        public float XPLastLevel
        {
            get
            {
                float result = 0f;
                if (this.EldarPsykerLevel > 0) result = this.EldarPsykerLevel * 600;
                return result;
            }
        }

        public float XPTillNextLevelPercent
        {
            get
            {
                return (float)(this.EldarPsykerXP - this.XPLastLevel) / (float)(this.EldarPsykerXPTillNextLevel - this.XPLastLevel);
            }
        }

        public int EldarPsykerXPTillNextLevel
        {
            get
            {
                return (this.EldarPsykerLevel + 1) * 600;
            }
        }

        public int EldarSkillLevel(string skillName)
        {
            int result = 0;
            PsykerSkill skillCheck = this.PsykerData.Skills.FirstOrDefault((PsykerSkill x) => x.label == skillName);
            if (skillCheck != null)
            {
                result = skillCheck.level;
            }

            return result;
        }

        #endregion Levels
    }
}