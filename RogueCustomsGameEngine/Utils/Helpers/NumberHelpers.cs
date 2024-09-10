namespace RogueCustomsGameEngine.Utils.Helpers
{
    public static class NumberHelpers
    {
        public static bool Between(this int num, int min, int max)
        {
            return num >= min && num <= max;
        }

        public static int Clamp(this int num, int min, int max)
        {
            if (num < min)
                return min;
            if (num > max)
                return max;
            return num;
        }

        public static bool Between(this double num, double min, double max)
        {
            return num >= min && num <= max;
        }
        public static double Clamp(this double num, double min, double max)
        {
            if (num < min)
                return min;
            if (num > max)
                return max;
            return num;
        }
    }
}
