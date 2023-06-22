using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RogueCustomsDungeonValidator.Utils
{
    public static class StringHelpers
    {
        public static bool CanBeEncodedToIBM437(this string s)
        {
            return Regex.IsMatch(s, @"^[\s☺☻♥♦♣♠•◘○◙♂♀♪♫☼►◄↕‼¶§▬↨↑↓→←∟↔▲▼!""#\$%&'\(\)\*\+,-\.\/0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ\[\\\]\^_`abcdefghijklmnopqrstuvwxyz{|}~¡¢£¥ª«¬°±²µ·º»¼½¿ÄÅÆÇÉÑÖÜßàáâäåæçèéêëìíîïñòóôö÷ùúûüÿƒΓΘΣΦΩαδεπστφⁿ₧∙√∞∩≈≡≤≥⌐⌠⌡─│┌┐└┘├┤┬┴┼═║╒╓╔╕╖╗╘╙╚╛╜╝╞╟╠╡╢╣╤╥╦╧╨╩╪╫╬▀▄█▌▐░▒▓■]+$");
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
    }
}
