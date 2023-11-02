using RogueCustomsDungeonEditor.Validators.IndividualValidators;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsDungeonEditor.Validators
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public class DungeonValidator
    {
        private readonly DungeonInfo DungeonJson;

        public DungeonValidationMessages NameValidationMessages { get; private set; }
        public DungeonValidationMessages AuthorValidationMessages { get; private set; }
        public DungeonValidationMessages MessageValidationMessages { get; private set; }
        public DungeonValidationMessages IdValidationMessages { get; private set; }
        public DungeonValidationMessages FloorPlanValidationMessages { get; private set; }
        public List<(string Id, DungeonValidationMessages ValidationMessages)> TileSetValidationMessages { get; private set; } = new List<(string Id, DungeonValidationMessages ValidationMessages)>();
        public List<(int FloorMinimumLevel, int FloorMaximumLevel, DungeonValidationMessages ValidationMessages)> FloorGroupValidationMessages { get; private set; } = new List<(int FloorMinimumLevel, int FloorMaximumLevel, DungeonValidationMessages ValidationMessages)>();
        public List<(string Id, DungeonValidationMessages ValidationMessages)> FactionValidationMessages { get; private set; } = new List<(string Id, DungeonValidationMessages ValidationMessages)>();
        public List<(string Id, DungeonValidationMessages ValidationMessages)> PlayerClassValidationMessages { get; private set; } = new List<(string Id, DungeonValidationMessages ValidationMessages)>();
        public List<(string Id, DungeonValidationMessages ValidationMessages)> NPCValidationMessages { get; private set; } = new List<(string Id, DungeonValidationMessages ValidationMessages)>();
        public List<(string Id, DungeonValidationMessages ValidationMessages)> ItemValidationMessages { get; private set; } = new List<(string Id, DungeonValidationMessages ValidationMessages)>();
        public List<(string Id, DungeonValidationMessages ValidationMessages)> TrapValidationMessages { get; private set; } = new List<(string Id, DungeonValidationMessages ValidationMessages)>();
        public List<(string Id, DungeonValidationMessages ValidationMessages)> AlteredStatusValidationMessages { get; private set; } = new List<(string Id, DungeonValidationMessages ValidationMessages)>();

        public DungeonValidationMessages DefaultLocaleValidationMessages { get; private set; }
        public List<(string Language, DungeonValidationMessages ValidationMessages)> LocaleStringValidationMessages { get; private set; } = new List<(string Language, DungeonValidationMessages ValidationMessages)>();

        public DungeonValidator(DungeonInfo dungeonJson)
        {
            DungeonJson = dungeonJson;
        }

        public bool Validate(List<string> requiredLocaleStrings)
        {
            var dungeonSpecificLocaleStringsToExpect = new List<string>();

            foreach (var locale in DungeonJson.Locales)
            {
                dungeonSpecificLocaleStringsToExpect.AddRange(
                        locale.LocaleStrings
                            .Where(localeString => !requiredLocaleStrings.Contains(localeString.Key)
                                                && !dungeonSpecificLocaleStringsToExpect.Contains(localeString.Key))
                            .Select(localeString => localeString.Key));
            }
            var sampleDungeon = new Dungeon(0, DungeonJson, DungeonJson.DefaultLocale);
            sampleDungeon.PlayerClass = sampleDungeon.Classes.Find(p => p.EntityType == EntityType.Player);
            sampleDungeon.NewMap();

            NameValidationMessages = DungeonNameValidator.Validate(DungeonJson);
            AuthorValidationMessages = DungeonAuthorValidator.Validate(DungeonJson);
            MessageValidationMessages = DungeonMessageValidator.Validate(DungeonJson);
            IdValidationMessages = DungeonIdsValidator.Validate(DungeonJson);
            FloorPlanValidationMessages = DungeonFloorValidator.ValidateGeneralFloorPlan(DungeonJson);

            foreach (var tileSetInfo in DungeonJson.TileSetInfos)
                TileSetValidationMessages.Add((tileSetInfo.Id, DungeonTilesetValidator.Validate(tileSetInfo, DungeonJson)));

            foreach (var floorInfo in DungeonJson.FloorInfos)
                FloorGroupValidationMessages.Add((floorInfo.MinFloorLevel, floorInfo.MaxFloorLevel, DungeonFloorValidator.ValidateFloorType(floorInfo, DungeonJson, sampleDungeon)));

            foreach (var factionInfo in DungeonJson.FactionInfos)
                FactionValidationMessages.Add((factionInfo.Id, DungeonFactionValidator.Validate(factionInfo, DungeonJson)));

            foreach (var playerInfo in DungeonJson.PlayerClasses)
                PlayerClassValidationMessages.Add((playerInfo.Id, DungeonPlayerClassValidator.Validate(playerInfo, DungeonJson, sampleDungeon)));
            foreach (var npcInfo in DungeonJson.NPCs)
                NPCValidationMessages.Add((npcInfo.Id, DungeonNPCValidator.Validate(npcInfo, DungeonJson, sampleDungeon)));
            foreach (var itemInfo in DungeonJson.Items)
                ItemValidationMessages.Add((itemInfo.Id, DungeonItemValidator.Validate(itemInfo, DungeonJson, sampleDungeon)));
            foreach (var trapInfo in DungeonJson.Traps)
                TrapValidationMessages.Add((trapInfo.Id, DungeonTrapValidator.Validate(trapInfo, DungeonJson, sampleDungeon)));
            foreach (var alteredStatusInfo in DungeonJson.AlteredStatuses)
                AlteredStatusValidationMessages.Add((alteredStatusInfo.Id, DungeonAlteredStatusValidator.Validate(alteredStatusInfo, DungeonJson, sampleDungeon)));

            DefaultLocaleValidationMessages = DungeonLocaleValidator.ValidateDefaultLocale(DungeonJson);
            LocaleStringValidationMessages.AddRange(DungeonJson.Locales.Select(locale => (locale.Language, DungeonLocaleValidator.ValidateLocaleStrings(DungeonJson, locale.Language, requiredLocaleStrings, dungeonSpecificLocaleStringsToExpect))));
            return !NameValidationMessages.HasErrors
                && !AuthorValidationMessages.HasErrors
                && !MessageValidationMessages.HasErrors
                && !IdValidationMessages.HasErrors
                && !FloorPlanValidationMessages.HasErrors
                && !FloorGroupValidationMessages.Exists(ftvm => ftvm.ValidationMessages.HasErrors)
                && !FactionValidationMessages.Exists(fvm => fvm.ValidationMessages.HasErrors)
                && !NPCValidationMessages.Exists(cvm => cvm.ValidationMessages.HasErrors)
                && !ItemValidationMessages.Exists(ivm => ivm.ValidationMessages.HasErrors)
                && !TrapValidationMessages.Exists(tvm => tvm.ValidationMessages.HasErrors)
                && !AlteredStatusValidationMessages.Exists(asvm => asvm.ValidationMessages.HasErrors)
                && !DefaultLocaleValidationMessages.HasErrors
                && !LocaleStringValidationMessages.Exists(lsvm => lsvm.ValidationMessages.HasErrors);
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
