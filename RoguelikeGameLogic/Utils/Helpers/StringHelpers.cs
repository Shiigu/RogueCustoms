using RoguelikeGameEngine.Utils.Representation;
using System.Text.RegularExpressions;

namespace RoguelikeGameEngine.Utils.Helpers
{
    public static class StringHelpers
    {
        public static bool IsDiceNotation(this string s)
        {
            return Regex.Match(s, Constants.DiceNotationRegexPattern).Success;
        }

        public static GameColor ToGameColor(this string s)
        {
            var colorValues = s.Split(',').Select(s => byte.Parse(s)).ToArray();
            var r = colorValues[0];
            var g = colorValues[1];
            var b = colorValues[2];
            var a = colorValues[3];
            return new GameColor
            {
                R = r,
                G = g,
                B = b,
                A = a
            };
        }
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }
    }
}
