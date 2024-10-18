using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

using RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V11;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V12
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public class DungeonInfoV12
    {
        public string Version { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string WelcomeMessage { get; set; }
        public string EndingMessage { get; set; }
        public string DefaultLocale { get; set; }
        public List<LocaleInfoV11> Locales { get; set; }
        public int AmountOfFloors { get; set; }
        public List<TileSetInfoV11> TileSetInfos { get; set; }
        public List<FloorInfoV12> FloorInfos { get; set; }
        public List<FactionInfoV11> FactionInfos { get; set; }
        public List<PlayerClassInfoV12> PlayerClasses { get; set; }
        public List<NPCInfoV12> NPCs { get; set; }
        public List<ItemInfoV12> Items { get; set; }
        public List<TrapInfoV12> Traps { get; set; }
        public List<AlteredStatusInfoV12> AlteredStatuses { get; set; }

        public string GetLocalizedName(string localeLanguage)
        {
            var localeInfoV11ToUse = Locales.Find(l => l.Language.Equals(localeLanguage))
                ?? Locales.Find(l => l.Language.Equals(DefaultLocale))
                ?? throw new InvalidDataException($"No locale data has been found for {localeLanguage}, and no default locale was defined.");
            var localeString = localeInfoV11ToUse.LocaleStrings.Find(ls => ls.Key.Equals(Name));
            if (localeString != null)
                return localeString.Value;
            return Name;
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
