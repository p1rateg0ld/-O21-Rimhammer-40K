using Verse;

namespace Rimhammer40k
{
    public class RemovableHediff : Hediff
    {
        public override bool ShouldRemove
        {
            get
            {
                return true;
            }
        }

        public RemovableHediff()
        {
        }
    }
}
