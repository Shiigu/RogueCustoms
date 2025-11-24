using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using org.matheval;

using RogueCustomsGameEngine.Utils;

namespace RogueCustomsDungeonEditor.Utils
{
    public static class StringHelpers
    {
        public static bool CanBeEncoded(this string s)
        {
            return Regex.IsMatch(s, @"^[\s �!""#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`abcdefghijklmnopqrstuvwxyz{|}~¡¢£¤¥¦§¨©ª«¬­®¯°±²³´µ¶·¸¹º»¼½¾¿ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþÿĀāĂăĄąĆćĈĉĊċČčĎďĐđĒēĔĕĖėĘęĚěĜĝĞğĠġĢģĤĥĦħĨĩĪīĬĭĮįİıĲĳĴĵĶķĸĹĺĻļĽľĿŀŁłŃńŅņŇňŉŊŋŌōŎŏŐőŒœŔŕŖŗŘřŚśŜŝŞşŠšŢţŤťŦŧŨũŪūŬŭŮůŰűŲųŴŵŶŷŸŹźŻżŽžſƒơƷǺǻǼǽǾǿȘșȚțɑɸˆˇˉ˘˙˚˛˜˝;΄΅Ά·ΈΉΊΌΎΏΐΑΒΓΔΕΖΗΘΙΚΛΜΝΞΟΠΡΣΤΥΦΧΨΩΪΫάέήίΰαβγδεζηθικλμνξοπρςστυφχψωϊϋόύώϐϴЀЁЂЃЄЅІЇЈЉЊЋЌЍЎЏАБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдежзийклмнопрстуфхцчшщъыьэюяѐёђѓєѕіїјљњћќѝўџҐґ־אבגדהוזחטיךכלםמןנסעףפץצקרשתװױײ׳״ᴛᴦᴨẀẁẂẃẄẅẟỲỳ‐‒–—―‗‘’‚‛“”„‟†‡•…‧‰′″‵‹›‼‾‿⁀⁄⁔⁴⁵⁶⁷⁸⁹⁺⁻ⁿ₁₂₃₄₅₆₇₈₉₊₋₣₤₧₪€℅ℓ№™Ω℮⅐⅑⅓⅔⅕⅖⅗⅘⅙⅚⅛⅜⅝⅞←↑→↓↔↕↨∂∅∆∈∏∑−∕∙√∞∟∩∫≈≠≡≤≥⊙⌀⌂⌐⌠⌡─│┌┐└┘├┤┬┴┼═║╒╓╔╕╖╗╘╙╚╛╜╝╞╟╠╡╢╣╤╥╦╧╨╩╪╫╬▀▁▄█▌▐░▒▓■□▪▫▬▲►▼◄◊○●◘◙◦☺☻☼♀♂♠♣♥♦♪♫✓ﬁﬂ]+$");
        }
        public static string JoinAnd<T>(this IEnumerable<T?> values,
        in string separator = ", ",
        in string lastSeparator = ", and ") => JoinAnd(values, new StringBuilder(), separator, lastSeparator).ToString();

        public static StringBuilder JoinAnd<T>(this IEnumerable<T?> values,
            StringBuilder sb,
            in string separator = ", ",
            in string lastSeparator = ", and ")
        {
            _ = values ?? throw new ArgumentNullException(nameof(values));
            _ = separator ?? throw new ArgumentNullException(nameof(separator));
            _ = lastSeparator ?? throw new ArgumentNullException(nameof(lastSeparator));

            using var enumerator = values.GetEnumerator();

            // add first item without separator
            if (enumerator.MoveNext())
            {
                sb.Append(enumerator.Current);
            }

            var nextItem = (hasValue: false, item: default(T?));
            // see if there is a next item
            if (enumerator.MoveNext())
            {
                nextItem = (true, enumerator.Current);
            }

            // while there is a next item, add separator and current item
            while (enumerator.MoveNext())
            {
                sb.Append(separator);
                sb.Append(nextItem.item);
                nextItem = (true, enumerator.Current);
            }

            // add last separator and last item
            if (nextItem.hasValue)
            {
                sb.Append(lastSeparator ?? separator);
                sb.Append(nextItem.item);
            }

            return sb;
        }

        public static string[] SplitStringWithPattern(this string input, string pattern)
        {
            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(input);

            var result = new List<string>();

            int previousMatchEndIndex = 0;

            foreach (Match match in matches)
            {
                // Add the portion before the match
                result.Add(input.Substring(previousMatchEndIndex, match.Index - previousMatchEndIndex));

                // Add the matched portion
                result.Add(match.Value);

                // Update previousMatchEndIndex for the next iteration
                previousMatchEndIndex = match.Index + match.Length;
            }

            // Add the portion after the last match
            string lastPart = input.Substring(previousMatchEndIndex);

            // Exclude the final split if it's empty
            if (!string.IsNullOrEmpty(lastPart))
            {
                result.Add(lastPart);
            }

            return result.ToArray();
        }
        public static bool IsDiceNotation(this string s)
        {
            return Regex.Match(s, EngineConstants.DiceNotationRegexPattern, RegexOptions.IgnoreCase).Success;
        }
        public static bool IsIntervalNotation(this string s)
        {
            return Regex.Match(s, EngineConstants.IntervalRegexPattern, RegexOptions.IgnoreCase).Success;
        }

        public static bool IsMathExpression(this string input)
        {
            try
            {
                new Expression(input).Eval<decimal>();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsBooleanExpression(this string input)
        {
            var pattern = @"[<>!=]=?|&&|\|\||\b(true|false)\b|[A-Za-z_][A-Za-z0-9_]*\(([^()]*|(?<open>\()|(?<-open>\)))+(?(open)(?!))\)";

            return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }
    }
}
