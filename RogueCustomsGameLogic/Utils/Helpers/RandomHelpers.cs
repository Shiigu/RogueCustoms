using System;

namespace RogueCustomsGameEngine.Utils.Helpers
{
    public static class RandomHelpers
    {
        public static int NextInclusive(this Random rng, int maxValue)
        {
            return rng.NextInclusive(0, maxValue);
        }
        public static int NextInclusive(this Random rng, int minValue, int maxValue)
        {
            return rng.Next(minValue, maxValue + 1);
        }
    }
}
