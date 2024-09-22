using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Godot;

namespace RogueCustomsGodotClient.Helpers
{
    public static class StringHelper
    {
        public static string ToStringWithoutBbcode(this string stringWithBbCode)
        {
            var stringWithoutBbCode = new string(stringWithBbCode);
            var bbTagsToRemove = new List<string>
            {
                "b", "i", "u", "s", "color", "size", "font", "highlight",
                "align", "url", "email", "img", "quote", "code", "list",
                "*", "spoiler", "center", "left", "right"
            };
            foreach (var tag in bbTagsToRemove)
            {
                // Remove opening tags
                stringWithoutBbCode = RemoveBbCodeTag(stringWithoutBbCode, tag);
                // Remove closing tags
                stringWithoutBbCode = RemoveBbCodeTag(stringWithoutBbCode, "/" + tag);
            }

            return stringWithoutBbCode;
        }

        private static string RemoveBbCodeTag(string input, string tag)
        {
            string openTag = "[" + tag;
            string closeTag = "]";
            int tagStart;

            while ((tagStart = input.IndexOf(openTag)) != -1)
            {
                int tagEnd = input.IndexOf(closeTag, tagStart);
                if (tagEnd != -1)
                {
                    input = input.Remove(tagStart, tagEnd - tagStart + closeTag.Length);
                }
                else
                {
                    break;
                }
            }

            return input;
        }

        public static Vector2 GetSizeToFitForDimensions(this string text, Font font, int maxSizeX, int maxSizeY)
        {
            var textLines = text.Split('\n');
            var longestLine = textLines.OrderByDescending(line => line.Length).FirstOrDefault().ToStringWithoutBbcode();
            var sizeForOneLine = font.GetStringSize(longestLine);
            var heightForMultiline = Math.Max((int)(sizeForOneLine.X / maxSizeX), 1) * sizeForOneLine.Y * textLines.Length;

            return new Vector2(Mathf.Min(maxSizeX, sizeForOneLine.X), Mathf.Min(maxSizeY, heightForMultiline));
        }

        public static Vector2 GetSizeToFitForDimensionsWithoutBbCode(this string text, Font font, int maxSizeX, int maxSizeY)
            => text.ToStringWithoutBbcode().GetSizeToFitForDimensions(font, maxSizeX, maxSizeY);
        public static string Format(this string input, object p)
        {
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(p))
                input = input.Replace("{" + prop.Name + "}", (prop.GetValue(p) ?? "(null)").ToString());

            return input;
        }

        public static string ToBbCodeAppropriateString(this string input)
        {
            var bbCodeString = new StringBuilder();
            var inputLines = input.Replace("\r", "").Split('\n');
            for (int i = 0; i < inputLines.Length; i++)
            {
                bbCodeString.Append("[p]");
                bbCodeString.Append(inputLines[i].Replace("\r", ""));
                if (string.IsNullOrEmpty(inputLines[i].Replace("\r", "")))
                    bbCodeString.Append(" ");
            }
            return bbCodeString.ToString();
        }
    }
}
