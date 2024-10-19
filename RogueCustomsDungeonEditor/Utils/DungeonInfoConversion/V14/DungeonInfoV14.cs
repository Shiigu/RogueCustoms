using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V14
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public class DungeonInfoV14
    {
        public string Version { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string WelcomeMessage { get; set; }
        public string EndingMessage { get; set; }
        public string DefaultLocale { get; set; }
        public List<LocaleInfo> Locales { get; set; }
        public int AmountOfFloors { get; set; }
        public List<TileSetInfoV14> TileSetInfos { get; set; }
        public List<FloorInfo> FloorInfos { get; set; }
        public List<FactionInfo> FactionInfos { get; set; }
        public List<PlayerClassInfoV14> PlayerClasses { get; set; }
        public List<NPCInfoV14> NPCs { get; set; }
        public List<ItemInfo> Items { get; set; }
        public List<TrapInfo> Traps { get; set; }
        public List<AlteredStatusInfo> AlteredStatuses { get; set; }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
