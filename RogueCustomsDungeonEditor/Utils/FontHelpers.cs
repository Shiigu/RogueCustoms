using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.Utils
{
#pragma warning disable CA1416 // Validar la compatibilidad de la plataforma
    public static class FontHelpers
    {
        private readonly static PrivateFontCollection PrivateFontCollection = new();
        private readonly static HashSet<string> LoadedFonts = new(StringComparer.OrdinalIgnoreCase);

        public static bool LoadFont(string fontPath)
        {
            if (LoadedFonts.Contains(fontPath))
            {
                return true;
            }

            var previousFontCount = PrivateFontCollection.Families.Length;

            var fontData = File.ReadAllBytes(fontPath);
            var fontPtr = Marshal.AllocCoTaskMem(fontData.Length);
            Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            PrivateFontCollection.AddMemoryFont(fontPtr, fontData.Length);
            Marshal.FreeCoTaskMem(fontPtr);

            var success = PrivateFontCollection.Families.Length > previousFontCount;
            if (success)
            {
                var fontFamily = PrivateFontCollection.Families[^1];
                LoadedFonts.Add(fontPath);
            }

            return success;
        }

        public static FontFamily? GetFontByName(string fontName)
        {
            return PrivateFontCollection.Families.FirstOrDefault(f => f.Name.Equals(fontName));
        }

        public static FontFamily? GetFontByIndex(int index)
        {
            return PrivateFontCollection.Families.ElementAtOrDefault(index);
        }

        public static List<char> GetPrintableCharactersFromFont(FontFamily fontFamily)
        {
            const string possibleCharacters = " �\u0001\u0002\u0003\u0004\u0005\u0006\a\b\u000e\u000f!\"#$%&\'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~\u007f¡¢£¤¥¦§¨©ª«¬­®¯°±²³´µ¶·¸¹º»¼½¾¿ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþÿĀāĂăĄąĆćĈĉĊċČčĎďĐđĒēĔĕĖėĘęĚěĜĝĞğĠġĢģĤĥĦħĨĩĪīĬĭĮįİıĲĳĴĵĶķĸĹĺĻļĽľĿŀŁłŃńŅņŇňŉŊŋŌōŎŏŐőŒœŔŕŖŗŘřŚśŜŝŞşŠšŢţŤťŦŧŨũŪūŬŭŮůŰűŲųŴŵŶŷŸŹźŻżŽžſƒơƷǺǻǼǽǾǿȘșȚțɑɸˆˇˉ˘˙˚˛˜˝;΄΅Ά·ΈΉΊΌΎΏΐΑΒΓΔΕΖΗΘΙΚΛΜΝΞΟΠΡΣΤΥΦΧΨΩΪΫάέήίΰαβγδεζηθικλμνξοπρςστυφχψωϊϋόύώϐϴЀЁЂЃЄЅІЇЈЉЊЋЌЍЎЏАБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдежзийклмнопрстуфхцчшщъыьэюяѐёђѓєѕіїјљњћќѝўџҐґ־אבגדהוזחטיךכלםמןנסעףפץצקרשתװױײ׳״ᴛᴦᴨẀẁẂẃẄẅẟỲỳ‐‒–—―‗‘’‚‛“”„‟†‡•…‧‰′″‵‹›‼‾‿⁀⁄⁔⁴⁵⁶⁷⁸⁹⁺⁻ⁿ₁₂₃₄₅₆₇₈₉₊₋₣₤₧₪€℅ℓ№™Ω℮⅐⅑⅓⅔⅕⅖⅗⅘⅙⅚⅛⅜⅝⅞←↑→↓↔↕↨∂∅∆∈∏∑−∕∙√∞∟∩∫≈≠≡≤≥⊙⌀⌂⌐⌠⌡─│┌┐└┘├┤┬┴┼═║╒╓╔╕╖╗╘╙╚╛╜╝╞╟╠╡╢╣╤╥╦╧╨╩╪╫╬▀▁▄█▌▐░▒▓■□▪▫▬▲►▼◄◊○●◘◙◦☺☻☼♀♂♠♣♥♦♪♫✓ﬁﬂ";
            var printableCharacters = new List<char>();

            using (var font = new Font(fontFamily, 12))
            {
                if (!font.FontFamily.IsStyleAvailable(FontStyle.Regular))
                    return printableCharacters;
                foreach (var character in possibleCharacters)
                {
                    // A width higher than 0 means the char is rendered
                    if (TextRenderer.MeasureText(character.ToString(), font).Width > 0)
                    {
                        printableCharacters.Add(character);
                    }
                }
            }

            return printableCharacters;
        }
    }
#pragma warning restore CA1416 // Validar la compatibilidad de la plataforma
}
