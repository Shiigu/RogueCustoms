using RogueCustomsDungeonValidator.Utils;
using RogueCustomsDungeonValidator.Validators;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RogueCustomsDungeonValidator.Validators.IndividualValidators
{
    public static class StringValidator
    {
        public static DungeonValidationMessages ValidateString(this DungeonInfo dungeonJson, string localeStringKey, string fieldOwner, string fieldName, bool returnErrorIfMissing)
        {
            var validationMessages = new DungeonValidationMessages();

            if (string.IsNullOrWhiteSpace(localeStringKey))
            {
                if(returnErrorIfMissing)
                    validationMessages.AddError($"{fieldOwner} does not have a {fieldName}.");
                else
                    validationMessages.AddWarning($"{fieldOwner} does not have a {fieldName}.");
            }
            else
            {
                var localesWithKey = dungeonJson.Locales.Where(ls => ls.LocaleStrings.Any(ls => ls.Key.Equals(localeStringKey)));

                if (localesWithKey.Any())
                {
                    foreach (var locale in dungeonJson.Locales.Except(localesWithKey).ToList())
                    {
                        validationMessages.AddWarning($"{localeStringKey} isn't present in the Locale Dictionary for Language {locale.Language}. It won't be localized.");
                    }

                    foreach (var locale in localesWithKey)
                    {
                        var localeValue = locale.LocaleStrings.Find(ls => ls.Key.Equals(localeStringKey)).Value;
                        if (!localeValue.CanBeEncodedToIBM437())
                            validationMessages.AddWarning($"{localeStringKey}, when localized in Language {locale.Language}, cannot be properly encoded to IBM437. Console clients may display it incorrectly.");
                    }
                }
                else
                {
                    validationMessages.AddWarning($"{localeStringKey} isn't present in any Locale Dictionary. It will never be localized.");
                }
            }

            return validationMessages;
        }
    }
}
