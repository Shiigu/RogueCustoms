using SadConsole;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace RoguelikeConsoleClient.Helpers
{
    public static class StringHelpers
    {
        private static readonly Encoding ExtendedASCII = Encoding.GetEncoding("IBM437");
        public static ColoredGlyph[] ToGlyphString(this string text)
        {
            return ExtendedASCII.GetBytes(text).Select(b => new ColoredGlyph() { Glyph = b }).ToArray();
        }

        public static string Wrap(this string singleLineString, int columns)
            => string.Join(Environment.NewLine, singleLineString.Split(columns));

        public static IEnumerable<string> Split(this string str, int chunkSize)
        {
            if (str == null)
                yield return str;
            if (chunkSize < 1)
                throw new ArgumentException("'chunkSize' must be greater than 0.");

            for (int i = 0; i < str.Length; i += chunkSize)
                yield return str.Substring(i, Math.Min(chunkSize, str.Length - i));
        }
        public static string Format(this string input, object p)
        {
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(p))
                input = input.Replace("{" + prop.Name + "}", (prop.GetValue(p) ?? "(null)").ToString());

            return input;
        }
    }
}
