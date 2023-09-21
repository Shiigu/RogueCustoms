namespace RogueCustomsDungeonEditor.Utils
{
    public static class IntHelpers
    {
        public static bool Between(this int num, int min, int max)
        {
            return num >= min && num <= max;
        }

        public static bool DoIntervalsIntersect(int start1, int end1, int start2, int end2)
        {
            return end1 >= start2 && end2 >= start1;
        }
    }
}
