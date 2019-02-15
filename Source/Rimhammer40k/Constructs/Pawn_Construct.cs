using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using Verse.AI;
using UnityEngine;

namespace Rimhammer40k.Constructs
{
    public class Pawn_Construct : Pawn
    {
        public Building_ConstructStation station;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            skills = new Pawn_SkillTracker(this);
            foreach(SkillRecord record in skills.skills)
            {
                record.levelInt = 20;
                record.passion = Passion.None;
            }
            story = new Pawn_StoryTracker(this)
            {
                bodyType = BodyTypeDefOf.Thin,
                crownType = CrownType.Average,
                childhood = ConstructBackstories.childhood,
                adulthood = ConstructBackstories.adulthood
            };
            drafter = new Pawn_DraftController(this);
            relations = new Pawn_RelationsTracker(this);
            Name = new NameSingle(ConstructDefOf.CanoptekScarab.label);
        }

        public override void Tick()
        {
            base.Tick();
            if (this.IsHashIntervalTick(250))
            {
                foreach(SkillRecord sr in skills.skills)
                {
                    sr.levelInt = 20;
                    if(sr.xpSinceLastLevel > 1f)
                    {
                        sr.xpSinceMidnight = 100f;
                        sr.xpSinceLastLevel = 100f;
                    }
                }
            }
            if (Downed)
            {
                Kill(null);
            }
        }

        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            base.DeSpawn(mode);
            if(station != null)
            {
                station.Notify_ConstructMayBeLost(this);
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look(ref station, "station");
        }
    }
}