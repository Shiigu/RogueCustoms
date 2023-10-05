namespace RogueCustomsGameEngine.Utils.Helpers
{
    public static class NumberHelpers
    {
        public static bool Between(this int num, int min, int max)
        {
            return num >= min && num <= max;
        }
        public static bool Between(this double num, int min, int max)
        {
            return num >= min && num <= max;
        }
    }
}
