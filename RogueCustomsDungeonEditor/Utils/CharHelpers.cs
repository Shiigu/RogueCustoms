using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsDungeonEditor.Utils
{
    public static class CharHelpers
    {
        public static List<char> GetIBM437PrintableCharacters()
        {
            const string IBM437mapset = "☺☻♥♦♣♠•◘○◙♂♀♪♫☼►◄↕‼¶§▬↨↑↓→←∟↔▲▼!\"#\\$%&'\\(\\)\\*\\+,-\\.\\/0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ\\[\\\\\\]\\^_`abcdefghijklmnopqrstuvwxyz{|}~\u007f¡¢£¥ª«¬°±²µ·º»¼½¿ÄÅÆÇÉÑÖÜßàáâäåæçèéêëìíîïñòóôö÷ùúûüÿƒΓΘΣΦΩαδεπστφⁿ₧∙√∞∩≈≡≤≥⌐⌠⌡─│┌┐└┘├┤┬┴┼═║╒╓╔╕╖╗╘╙╚╛╜╝╞╟╠╡╢╣╤╥╦╧╨╩╪╫╬▀▄█▌▐░▒▓■";
            var printableCharacters = new List<char>();

            foreach (var ibm437char in IBM437mapset)
            {
                printableCharacters.Add(ibm437char);
            }

            return printableCharacters;
        }
    }
}
