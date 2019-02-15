using RimWorld;
using Verse;

namespace Rimhammer40k.Constructs
{
    [StaticConstructorOnStartup]
    public static class ConstructBackstories
    {
        public static Backstory childhood;
        public static Backstory adulthood;

        static ConstructBackstories()
        {
            LongEventHandler.ExecuteWhenFinished(() =>
            {
                childhood = new Backstory()
                {
                    title = "Artificial Construct",
                    titleShort = "Construct",
                    identifier = "ConstructBackstoryC",
                    workDisables = WorkTags.Social,
                    slot = BackstorySlot.Childhood,
                    baseDesc = ""
                };
                BackstoryDatabase.AddBackstory(childhood);
                adulthood = new Backstory()
                {
                    title = "Artificial Construct",
                    titleShort = "Construct",
                    identifier = "ConstructBackstoryA",
                    workDisables = WorkTags.Social,
                    slot = BackstorySlot.Adulthood,
                    baseDesc = ""
                };
                BackstoryDatabase.AddBackstory(adulthood);
            });
        }
    }
}