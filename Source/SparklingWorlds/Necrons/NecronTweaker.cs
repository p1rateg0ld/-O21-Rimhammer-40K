using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

namespace Rimhammer40k.Necrons
{
    class NecronTweaker : DefModExtension
    {
        //remove corpse rotting
        public bool tweakCorpseRot = true;

        //add necron resurrection
        public bool tweakNecronResurrect = true;

        //add necron repair ability
        public bool tweakNecronRepair = true;

        //products butchering gives
        public RecipeDef recipeDef;

        //tweaks corpse butchering products by importing costs from recipe
        public bool tweakCorpseButcherProducts = true;

        //percentage of products returned when butchering
        public float corpseButcherProductsRatio = 1.0f;

        //if true, product percentage will round up, to prevent zero from being returned.
        public bool corpseButcherRoundUp = false;

        //stop deterioration of skills over time
        public bool noSkillLoss = true;

        //stop pawn from being able to socialize
        public bool canSocialize = false;
    }

    [StaticConstructorOnStartup]
    public static class PostInitializationTweaker
    {
        static PostInitializationTweaker()
        {
            //Start tweaking.
            //IEnumerable<ThingDef> corpseDefs = DefDatabase<ThingDef>.AllDefs.Where(thingDef => thingDef.defName.EndsWith("_Corpse"));

            foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
            {
                //If the Def got a NecronTweaker do stuff, otherwise do not bother.
                NecronTweaker tweaker = thingDef.GetModExtension<NecronTweaker>();
                if (tweaker != null)
                {
                    ThingDef corpseDef = thingDef?.race?.corpseDef;
                    if (corpseDef != null)
                    {
                        //Removes corpse rotting.
                        if (tweaker.tweakCorpseRot)
                        {
                            corpseDef.comps.RemoveAll(compProperties => compProperties is CompProperties_Rottable);
                            corpseDef.comps.RemoveAll(compProperties => compProperties is CompProperties_SpawnerFilth);
                        }

                        if (tweaker.tweakNecronResurrect)
                        {
                            CompProperties_NecronResurrection compProperties_NecronResurrection = new CompProperties_NecronResurrection();
                            corpseDef.comps.Add(compProperties_NecronResurrection);
                        }

                        //Modifies the butchering products by importing the costs from a recipe.
                        RecipeDef recipeDef = tweaker.recipeDef;
                        if (tweaker.tweakCorpseButcherProducts && recipeDef != null)
                        {
                            corpseDef.butcherProducts.Clear();

                            foreach (IngredientCount ingredient in recipeDef.ingredients)
                            {
                                float finalCount = 0f;
                                ThingDef ingredientThingDef = ingredient.filter.AnyAllowedDef;
                                int requiredCount = ingredient.CountRequiredOfFor(ingredientThingDef, recipeDef);

                                if (tweaker.corpseButcherRoundUp)
                                {
                                    finalCount = (float)Math.Ceiling((float)requiredCount * tweaker.corpseButcherProductsRatio);
                                }
                                else
                                {
                                    finalCount = (float)Math.Floor((float)requiredCount * tweaker.corpseButcherProductsRatio);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
