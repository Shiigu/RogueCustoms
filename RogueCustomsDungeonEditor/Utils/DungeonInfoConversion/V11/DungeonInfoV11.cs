﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V11
{
    [Serializable]
    public class DungeonInfoV11
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
        public List<FloorInfoV11> FloorInfos { get; set; }
        public List<FactionInfoV11> FactionInfos { get; set; }
        public List<PlayerClassInfoV11> PlayerClasses { get; set; }
        public List<NPCInfoV11> NPCs { get; set; }
        public List<ItemInfoV11> Items { get; set; }
        public List<TrapInfoV11> Traps { get; set; }
        public List<AlteredStatusInfoV11> AlteredStatuses { get; set; }

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
}
