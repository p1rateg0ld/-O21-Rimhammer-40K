using System.Collections.Generic;
using Verse;
using RimWorld;

namespace Rimhammer40k.Tau
{
    public class TauDroneDef : Pawn
    {
        public ThingDef destroyedDef = null;

        public bool allowLearning = false;

        public class RobotSkills
        {
            public SkillDef skillDef;
            public int level = 0;
            public Passion passion = Passion.None;
        }
        public class RobotWorkTypes
        {
            public WorkTypeDef workTypeDef;
            public int priority = 1;
        }

        public List<RobotSkills> robotSkills = new List<RobotSkills>();
        public List<RobotWorkTypes> robotWorkTypes = new List<RobotWorkTypes>();

        WorkTags robotWorkTagsInt = WorkTags.None;
        public WorkTags RobotWorkTags
        {
            get
            {
                if (robotWorkTagsInt == WorkTags.None && robotWorkTypes.Count > 0)
                    InitWorkTagsFromWorkTypes();

                return robotWorkTagsInt;
            }
            set
            {
                robotWorkTagsInt = value;
            }
        }

        private WorkTags InitWorkTagsFromWorkTypes()
        {
            WorkTags workTags = WorkTags.None;
            foreach (RobotWorkTypes workTypes in this.robotWorkTypes)
                workTags = workTags | workTypes.workTypeDef.workTags;

            return workTags;
        }
    }
}
