using MiscUtil.Xml.Linq.Extensions;

using RogueCustomsDungeonEditor.Validators.IndividualValidators;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.Validators
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public class DungeonValidator
    {
        private ToolStripStatusLabel ProgressLabel;
        private ToolStripProgressBar ProgressBar;
        private readonly DungeonInfo DungeonJson;

        public DungeonValidationMessages NameValidationMessages { get; private set; }
        public DungeonValidationMessages AuthorValidationMessages { get; private set; }
        public DungeonValidationMessages MessageValidationMessages { get; private set; }
        public DungeonValidationMessages IdValidationMessages { get; private set; }
        public DungeonValidationMessages FloorPlanValidationMessages { get; private set; }
        public List<(string Id, DungeonValidationMessages ValidationMessages)> TileTypeValidationMessages { get; private set; } = new List<(string Id, DungeonValidationMessages ValidationMessages)>();
        public List<(string Id, DungeonValidationMessages ValidationMessages)> TileSetValidationMessages { get; private set; } = new List<(string Id, DungeonValidationMessages ValidationMessages)>();
        public List<(int FloorMinimumLevel, int FloorMaximumLevel, DungeonValidationMessages ValidationMessages)> FloorGroupValidationMessages { get; private set; } = new List<(int FloorMinimumLevel, int FloorMaximumLevel, DungeonValidationMessages ValidationMessages)>();
        public List<(string Id, DungeonValidationMessages ValidationMessages)> FactionValidationMessages { get; private set; } = new List<(string Id, DungeonValidationMessages ValidationMessages)>();
        public List<(string Id, DungeonValidationMessages ValidationMessages)> StatValidationMessages { get; private set; } = new List<(string Id, DungeonValidationMessages ValidationMessages)>();
        public List<(string Id, DungeonValidationMessages ValidationMessages)> ElementValidationMessages { get; private set; } = new List<(string Id, DungeonValidationMessages ValidationMessages)>();
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

        public bool Validate(List<string> requiredLocaleStrings, ToolStripStatusLabel progressLabel, ToolStripProgressBar progressBar)
        {
            ProgressLabel = progressLabel;
            ProgressBar = progressBar;

            UpdateProgressLabel("Preparing Dungeon for Validation...", false);

            var dungeonSpecificLocaleStringsToExpect = new List<string>();

            foreach (var locale in DungeonJson.Locales)
            {
                dungeonSpecificLocaleStringsToExpect.AddRange(
                        locale.LocaleStrings
                            .Where(localeString => !requiredLocaleStrings.Contains(localeString.Key)
                                                && !dungeonSpecificLocaleStringsToExpect.Contains(localeString.Key))
                            .Select(localeString => localeString.Key));
            
            }

            var totalSteps = DungeonJson.Locales.Count + DungeonJson.TileTypeInfos.Count + DungeonJson.TileSetInfos.Count + DungeonJson.FloorInfos.Count +
                     DungeonJson.FactionInfos.Count + DungeonJson.CharacterStats.Count + DungeonJson.ElementInfos.Count + DungeonJson.PlayerClasses.Count + DungeonJson.NPCs.Count +
                     DungeonJson.Items.Count + DungeonJson.Traps.Count + DungeonJson.AlteredStatuses.Count;
            ProgressBar.Visible = true;
            ProgressBar.Value = 0;
            ProgressBar.Maximum = totalSteps;

            var sampleDungeon = new Dungeon(0, DungeonJson, DungeonJson.DefaultLocale);
            sampleDungeon.PlayerClass = sampleDungeon.Classes.Find(p => p.EntityType == EntityType.Player);
            sampleDungeon.NewMap();

            UpdateProgressLabel("Starting Validation...", false);

            UpdateProgressLabel("Running Name Validation...", false);
            NameValidationMessages = DungeonNameValidator.Validate(DungeonJson);
            UpdateProgressLabel("Name Validation complete!", true);

            UpdateProgressLabel("Running Author Validation...", false);
            AuthorValidationMessages = DungeonAuthorValidator.Validate(DungeonJson);
            UpdateProgressLabel("Author Validation complete!", true);

            UpdateProgressLabel("Running Briefing/End Validation...", false);
            MessageValidationMessages = DungeonMessageValidator.Validate(DungeonJson);
            UpdateProgressLabel("Briefing/End Validation complete!", true);

            UpdateProgressLabel("Running Default Locale Validation...", false);
            DefaultLocaleValidationMessages = DungeonLocaleValidator.ValidateDefaultLocale(DungeonJson);
            UpdateProgressLabel("Default Locale Validation complete!", true);

            foreach (var locale in DungeonJson.Locales)
            {
                UpdateProgressLabel($"Running Locale {locale.Language} Validation...", false);
                LocaleStringValidationMessages.Add((locale.Language, DungeonLocaleValidator.ValidateLocaleStrings(DungeonJson, locale.Language, requiredLocaleStrings, dungeonSpecificLocaleStringsToExpect)));
                UpdateProgressLabel($"Locale {locale.Language} Validation complete!", true);
            }

            UpdateProgressLabel("Running Id Validation...", false);
            IdValidationMessages = DungeonIdsValidator.Validate(DungeonJson);
            UpdateProgressLabel("Id Validation complete!", true);

            UpdateProgressLabel("Running Floor Plan Validation...", false);
            FloorPlanValidationMessages = DungeonFloorValidator.ValidateGeneralFloorPlan(DungeonJson);
            UpdateProgressLabel("Floor Plan Validation complete!", true);

            foreach (var tileTypeInfo in DungeonJson.TileTypeInfos)
            {
                UpdateProgressLabel($"Running Tile Type {tileTypeInfo.Id} Validation...", false);
                TileTypeValidationMessages.Add((tileTypeInfo.Id, DungeonTileTypeValidator.Validate(tileTypeInfo, DungeonJson, sampleDungeon)));
                UpdateProgressLabel($"Tile Type {tileTypeInfo.Id} Validation complete!", true);
            }

            foreach (var tileSetInfo in DungeonJson.TileSetInfos)
            {
                UpdateProgressLabel($"Running Tileset {tileSetInfo.Id} Validation...", false);
                TileSetValidationMessages.Add((tileSetInfo.Id, DungeonTilesetValidator.Validate(tileSetInfo, DungeonJson)));
                UpdateProgressLabel($"Tileset {tileSetInfo.Id} Validation complete!", true);
            }

            foreach (var floorInfo in DungeonJson.FloorInfos)
            {
                var floorInfoString = string.Empty;
                if (floorInfo.MinFloorLevel != floorInfo.MaxFloorLevel)
                    floorInfoString = $"Floors {floorInfo.MinFloorLevel}-{floorInfo.MaxFloorLevel}";
                else
                    floorInfoString = $"Floor {floorInfo.MinFloorLevel}";
                UpdateProgressLabel($"Running {floorInfoString} Validation...", false);
                FloorGroupValidationMessages.Add((floorInfo.MinFloorLevel, floorInfo.MaxFloorLevel, DungeonFloorValidator.ValidateFloorType(floorInfo, DungeonJson, sampleDungeon)));
                UpdateProgressLabel($"{floorInfoString} Validation complete!", true);
            }

            foreach (var factionInfo in DungeonJson.FactionInfos)
            {
                UpdateProgressLabel($"Running Faction {factionInfo.Id} Validation...", false);
                FactionValidationMessages.Add((factionInfo.Id, DungeonFactionValidator.Validate(factionInfo, DungeonJson)));
                UpdateProgressLabel($"Faction {factionInfo.Id} Validation complete!", true);
            }

            foreach (var statInfo in DungeonJson.CharacterStats)
            {
                UpdateProgressLabel($"Running Stat {statInfo.Id} Validation...", false);
                StatValidationMessages.Add((statInfo.Id, DungeonStatValidator.Validate(statInfo, DungeonJson)));
                UpdateProgressLabel($"Stat {statInfo.Id} Validation complete!", true);
            }

            foreach (var elementInfo in DungeonJson.ElementInfos)
            {
                UpdateProgressLabel($"Running Element {elementInfo.Id} Validation...", false);
                ElementValidationMessages.Add((elementInfo.Id, DungeonElementValidator.Validate(elementInfo, DungeonJson, sampleDungeon)));
                UpdateProgressLabel($"Element {elementInfo.Id} Validation complete!", true);
            }

            foreach (var playerInfo in DungeonJson.PlayerClasses)
            {
                UpdateProgressLabel($"Running Player Class {playerInfo.Id} Validation...", false);
                PlayerClassValidationMessages.Add((playerInfo.Id, DungeonPlayerClassValidator.Validate(playerInfo, DungeonJson, sampleDungeon)));
                UpdateProgressLabel($"Player Class {playerInfo.Id} Validation complete!", true);
            }
            foreach (var npcInfo in DungeonJson.NPCs)
            {
                UpdateProgressLabel($"Running NPC {npcInfo.Id} Validation...", false);
                NPCValidationMessages.Add((npcInfo.Id, DungeonNPCValidator.Validate(npcInfo, DungeonJson, sampleDungeon)));
                UpdateProgressLabel($"NPC {npcInfo.Id} Validation complete!", true);
            }
            foreach (var itemInfo in DungeonJson.Items)
            {
                UpdateProgressLabel($"Running Item {itemInfo.Id} Validation...", false);
                ItemValidationMessages.Add((itemInfo.Id, DungeonItemValidator.Validate(itemInfo, DungeonJson, sampleDungeon)));
                UpdateProgressLabel($"Item {itemInfo.Id} Validation complete!", true);
            }
            foreach (var trapInfo in DungeonJson.Traps)
            {
                UpdateProgressLabel($"Running Trap {trapInfo.Id} Validation...", false);
                TrapValidationMessages.Add((trapInfo.Id, DungeonTrapValidator.Validate(trapInfo, DungeonJson, sampleDungeon)));
                UpdateProgressLabel($"Trap {trapInfo.Id} Validation complete!", true);
            }
            foreach (var alteredStatusInfo in DungeonJson.AlteredStatuses)
            {
                UpdateProgressLabel($"Running Altered Status {alteredStatusInfo.Id} Validation...", false);
                AlteredStatusValidationMessages.Add((alteredStatusInfo.Id, DungeonAlteredStatusValidator.Validate(alteredStatusInfo, DungeonJson, sampleDungeon)));
                UpdateProgressLabel($"Altered Status {alteredStatusInfo.Id} Validation complete!", true);
            }

            UpdateProgressLabel($"All Validations complete!", true);
            ProgressBar.Visible = false;

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

        private void UpdateProgressLabel(string text, bool updateProgress)
        {
            try
            {
                ProgressLabel.AutoSize = false;
                ProgressBar.AutoSize = false;
                var currentWidth = ProgressLabel.Width;
                var preferredWidth = TextRenderer.MeasureText(text, ProgressLabel.Font).Width;
                ProgressLabel.Text = text;
                ProgressLabel.Width = Math.Max(preferredWidth, currentWidth);
                
                if (updateProgress)
                    ProgressBar.Value++;

                ProgressBar.Width -= (ProgressLabel.Width - currentWidth);

                Application.DoEvents();
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Error updating label: {ex.Message}");
            }
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
