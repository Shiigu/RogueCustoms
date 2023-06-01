namespace RoguelikeConsoleClient.Helpers
{
    public static class IntHelpers
    {
        public static bool Between(this int num, int min, int max)
        {
            return num >= min && num <= max;
        }
    }
}
