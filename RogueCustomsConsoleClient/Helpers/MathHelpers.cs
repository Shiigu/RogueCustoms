namespace RogueCustomsConsoleClient.Helpers
{
    public static class MathHelpers
    {
        public static bool Between(this int num, int min, int max)
        {
            return num >= min && num <= max;
        }
    }
}
