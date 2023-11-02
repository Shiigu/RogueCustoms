using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.Utils
{
    public static class TextboxHelpers
    {
        public static void ToggleEntryInLocaleWarning(this TextBox textBox, DungeonInfo activeDungeon, Control warningControl)
        {
            if (warningControl == null) return;

            var isPartOfLocale = false;

            foreach (var locale in activeDungeon.Locales)
            {
                if (isPartOfLocale) break;
                foreach (var entry in locale.LocaleStrings)
                {
                    if (isPartOfLocale) break;
                    if (entry.Key.Equals(textBox.Text))
                        isPartOfLocale = true;
                }
            }

            warningControl.Visible = isPartOfLocale;
        }
    }
}
