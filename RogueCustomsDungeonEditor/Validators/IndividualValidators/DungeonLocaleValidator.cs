using RogueCustomsDungeonEditor.Utils;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
{
    public class DungeonLocaleValidator
    {
        public static DungeonValidationMessages ValidateDefaultLocale(DungeonInfo dungeonJson)
        {
            var messages = new DungeonValidationMessages();

            if (!dungeonJson.Locales.Any(l => l.Language.Equals(dungeonJson.DefaultLocale)))
                messages.AddError($"Dungeon's Default Locale, {dungeonJson.DefaultLocale}, isn't present in the Locale field.");

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
        public static DungeonValidationMessages ValidateLocaleStrings(DungeonInfo dungeonJson, string localeLanguage, List<string> requiredLocales, List<string> optionalLocales)
        {
            var messages = new DungeonValidationMessages();

            var locale = dungeonJson.Locales.Find(l => l.Language.Equals(localeLanguage));

            foreach (var requiredLocale in requiredLocales)
            {
                if (!locale.LocaleStrings.Any(ls => ls.Key.Equals(requiredLocale)))
                    messages.AddError($"Locale lacks the in-engine Locale Key {requiredLocale}.");
            }

            foreach (var optionalLocale in optionalLocales)
            {
                if (!locale.LocaleStrings.Any(ls => ls.Key.Equals(optionalLocale)))
                    messages.AddWarning($"Locale lacks the dungeon-specific Locale Key {optionalLocale}. Consider adding it.");
            }

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}
