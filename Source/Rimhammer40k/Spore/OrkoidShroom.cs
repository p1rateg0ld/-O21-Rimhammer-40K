using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using RimWorld;
using Verse;

using AlienRace;

namespace Rimhammer40k.Spore
{
    public class OrkoidShroom : Plant
    {
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
        }

        public override void PostMake()
        {
            base.PostMake();
        }

        public override void TickLong()
        {
            base.TickLong();
            if(this.TicksUntilFullyGrown == 0)
            {
                TrySpawn();
            }
        }

        public void TrySpawn()
        {
            int whichOrkoid = UnityEngine.Random.Range(0, 100);
            if (whichOrkoid < 66)
            {
                if (CanSpawnOrk())
                {
                    Pawn pawn = GenerateOrk();
                    GenSpawn.Spawn(pawn, this.Position, this.Map, 0);
                }
                else if (CanSpawnGrot())
                {
                    Pawn pawn = GenerateGrot();
                    GenSpawn.Spawn(pawn, this.Position, this.Map, 0);
                }
            }
            else
            {
                if (CanSpawnGrot())
                {
                    Pawn pawn = GenerateGrot();
                    GenSpawn.Spawn(pawn, this.Position, this.Map, 0);
                }
                else if (CanSpawnOrk())
                {
                    Pawn pawn = GenerateOrk();
                    GenSpawn.Spawn(pawn, this.Position, this.Map, 0);
                }
            }
            this.Destroy(DestroyMode.Vanish);
        }

        public Pawn GenerateOrk()
        {
            Log.Message("Spawning Ork...", false);
            PawnGenerationRequest request = new PawnGenerationRequest(
                PawnKindDef.Named("FeralOrk"),
            faction: Faction.OfPlayer,
            fixedBiologicalAge: 0,
            fixedChronologicalAge: 0,
            forceGenerateNewPawn: true,
            canGeneratePawnRelations: false,
            colonistRelationChanceFactor: 0f,
            allowFood: false);
            Pawn pawn = PawnGenerator.GeneratePawn(request);

            pawn?.equipment?.DestroyAllEquipment();
            pawn?.apparel?.DestroyAll();
            pawn?.inventory?.DestroyAll();

            return pawn;
        }

        public Pawn GenerateGrot()
        {
            Log.Message("Spawning Grot...", false);
            PawnGenerationRequest request = new PawnGenerationRequest(
                PawnKindDef.Named("FeralGrot"),
            faction: Faction.OfPlayer,
            fixedBiologicalAge: 0,
            fixedChronologicalAge: 0,
            forceGenerateNewPawn: true,
            canGeneratePawnRelations: false,
            colonistRelationChanceFactor: 0f,
            allowFood: false);
            Pawn pawn = PawnGenerator.GeneratePawn(request);

            pawn?.equipment?.DestroyAllEquipment();
            pawn?.apparel?.DestroyAll();
            pawn?.inventory?.DestroyAll();

            return pawn;
        }

        public bool CanSpawnOrk()
        {
            if (ColonistOrkCount > Rimhammer40kMod.maxOrkPopulation)
            {
                return false;
            }
            return true;
        }

        public int ColonistOrkCount
        {
            get
            {
                return this.Map.mapPawns.AllPawns.Count((Pawn x) => (x.IsColonist && x.def.defName == "Alien_Ork"));
            }
        }

        public bool CanSpawnGrot()
        {
            if (ColonistGrotCount > Rimhammer40kMod.maxOrkPopulation)
            {
                Log.Message("Current Grot Count: " + this.Map.mapPawns.AllPawns.Count((Pawn x) => (x.IsColonist && x.def.defName == "Alien_Grot")).ToString(), false);
                Log.Message("Max Grot Count: " + Rimhammer40kMod.maxGrotPopulation.ToString(), false);
                return false;
            }
            return true;
        }

        public int ColonistGrotCount
        {
            get
            {
                return this.Map.mapPawns.AllPawns.Count((Pawn x) => (x.IsColonist && x.def.defName == "Alien_Grot"));
            }
        }
    }
}
