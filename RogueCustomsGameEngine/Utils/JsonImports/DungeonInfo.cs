using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace RogueCustomsGameEngine.Utils.JsonImports
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public class DungeonInfo
    {
        public string Version { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string WelcomeMessage { get; set; }
        public string EndingMessage { get; set; }
        public string DefaultLocale { get; set; }
        public List<LocaleInfo> Locales { get; set; }
        public int AmountOfFloors { get; set; }
        public List<TileTypeInfo> TileTypeInfos { get; set; }
        public List<TileSetInfo> TileSetInfos { get; set; }
        public List<FloorInfo> FloorInfos { get; set; }
        public List<FactionInfo> FactionInfos { get; set; }
        public List<ElementInfo> ElementInfos { get; set; }
        public List<ActionSchoolInfo> ActionSchoolInfos { get; set; }
        public List<StatInfo> CharacterStats { get; set; }
        public List<LootTableInfo> LootTableInfos { get; set; }
        public List<PlayerClassInfo> PlayerClasses { get; set; }
        public List<NPCInfo> NPCs { get; set; }
        public List<ItemInfo> Items { get; set; }
        public List<TrapInfo> Traps { get; set; }
        public List<AlteredStatusInfo> AlteredStatuses { get; set; }
        public List<ActionWithEffectsInfo> Scripts { get; set; }

        public string GetLocalizedName(string localeLanguage)
        {
            var localeInfoToUse = Locales.Find(l => l.Language.Equals(localeLanguage))
                ?? Locales.Find(l => l.Language.Equals(DefaultLocale))
                ?? throw new InvalidDataException($"No locale data has been found for {localeLanguage}, and no default locale was defined.");
            var localeString = localeInfoToUse.LocaleStrings.Find(ls => ls.Key.Equals(Name));
            if (localeString != null)
                return localeString.Value;
            return Name;
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
