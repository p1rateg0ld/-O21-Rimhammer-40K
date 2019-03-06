using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using RimWorld;
using Verse;

using Rimhammer40k.Logic;

namespace Rimhammer40k.Eldar
{
    public class SpiritStone : ThingWithComps
    {
        // Relevant Information
        public string sourceName = null;
        public Gender gender = Gender.None;
        public List<ExposedTraitEntry> traits = new List<ExposedTraitEntry>();
        public Backstory backstoryChild;
        public Backstory backstoryAdult;
        public List<SkillRecord> skills = new List<SkillRecord>();
        public Faction faction;

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref sourceName, "sourceName");

            string childhoodIdentifier = (backstoryChild == null) ? null : backstoryChild.identifier;
            Scribe_Values.Look(ref childhoodIdentifier, "backstoryChild");
            if (Scribe.mode == LoadSaveMode.LoadingVars && !childhoodIdentifier.NullOrEmpty())
            {
                if (!BackstoryDatabase.TryGetWithIdentifier(childhoodIdentifier, out backstoryChild, true))
                {
                    Log.Error("Couldn't load child backstory with identifier " + childhoodIdentifier + ". Giving random.", false);
                    backstoryChild = BackstoryDatabase.RandomBackstory(BackstorySlot.Childhood);
                }
            }

            string adulthoodIdentifier = (backstoryAdult == null) ? null : backstoryAdult.identifier;
            Scribe_Values.Look(ref adulthoodIdentifier, "backStoryAdult");
            if (Scribe.mode == LoadSaveMode.LoadingVars && !adulthoodIdentifier.NullOrEmpty())
            {
                if (!BackstoryDatabase.TryGetWithIdentifier(adulthoodIdentifier, out backstoryAdult, true))
                {
                    Log.Error("Couldn't load adult backstory with identifier " + adulthoodIdentifier + ". Giving random.", false);
                    backstoryAdult = BackstoryDatabase.RandomBackstory(BackstorySlot.Adulthood);
                }
            }
            Scribe_Collections.Look(ref skills, "skills", LookMode.Deep);
        }
    }
}
