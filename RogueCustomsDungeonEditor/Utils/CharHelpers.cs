using System.Collections.Generic;

#pragma warning disable CA1416 // Validar la compatibilidad de la plataforma
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
#pragma warning restore CA1416 // Validar la compatibilidad de la plataforma
