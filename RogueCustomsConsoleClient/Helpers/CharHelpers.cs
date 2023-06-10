using System.Linq;
using System.Text;

namespace RogueCustomsConsoleClient.Helpers
{
    public static class CharHelpers
    {
        private static readonly Encoding ExtendedASCII = Encoding.GetEncoding("IBM437");
        public static int ToGlyph(this char Character)
        {
            return ExtendedASCII.GetBytes(Character.ToString()).First();
        }
        public static string ToGlyphString(this char Character)
        {
            return ((char) Character.ToGlyph()).ToString();
        }
    }
}
