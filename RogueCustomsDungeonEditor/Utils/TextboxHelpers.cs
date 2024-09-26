using RogueCustomsGameEngine.Utils.JsonImports;
using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.Utils
{
    public static class TextboxHelpers
    {
        public static void ToggleEntryInLocaleWarning(this TextBox textBox, DungeonInfo activeDungeon, Control warningControl)
        {
            if (warningControl == null) return;

            warningControl.Visible = textBox.Text.IsPartOfLocale(activeDungeon);
        }

        private static bool IsPartOfLocale(this string text, DungeonInfo activeDungeon)
        {
            foreach (var locale in activeDungeon.Locales)
            {
                foreach (var entry in locale.LocaleStrings)
                {
                    if (entry.Key.Equals(text))
                        return true;
                }
            }

            return false;
        }

        public static void ToggleConcatenatedEntryInLocaleWarning(this TextBox textBox, string prefix, string suffix, DungeonInfo activeDungeon, Control warningControl)
        {
            if (warningControl == null) return;

            var textToCheck = $"{prefix}{textBox.Text}{suffix}";

            warningControl.Visible = textToCheck.IsPartOfLocale(activeDungeon);
        }
    }
}
