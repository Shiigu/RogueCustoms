using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RogueCustomsGameEngine.Utils.JsonImports
{
    [Serializable]
    public class DungeonInfo
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string WelcomeMessage { get; set; }
        public string EndingMessage { get; set; }
        public string DefaultLocale { get; set; }
        public List<LocaleInfo> Locales { get; set; }
        public int AmountOfFloors { get; set; }
        public List<FloorInfo> FloorInfos { get; set; }
        public List<FactionInfo> FactionInfos { get; set; }
        public List<ClassInfo> Characters { get; set; }
        public List<ClassInfo> Items { get; set; }
        public List<ClassInfo> Traps { get; set; }
        public List<ClassInfo> AlteredStatuses { get; set; }

        public string GetLocalizedName(string localeLanguage)
        {
            var localeInfoToUse = Locales.Find(l => l.Language.Equals(localeLanguage))
                ?? Locales.Find(l => l.Language.Equals(DefaultLocale))
                ?? throw new Exception($"No locale data has been found for {localeLanguage}, and no default locale was defined.");
            var localeString = localeInfoToUse.LocaleStrings.Find(ls => ls.Key.Equals(Name));
            if (localeString != null)
                return localeString.Value;
            return Name;
        }
    }
}
