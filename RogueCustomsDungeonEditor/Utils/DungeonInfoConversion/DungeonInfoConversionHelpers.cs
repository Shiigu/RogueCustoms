using RogueCustomsDungeonEditor.Controls;
using RogueCustomsDungeonEditor.EffectInfos;
using RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.DungeonInfoPatches;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

#pragma warning disable CA1416 // Validar la compatibilidad de la plataforma
namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion
{
    public static class DungeonInfoConversionHelpers
    {
        private static readonly Dictionary<string, Action<JsonObject>> PatchMap = new()
        {
            { string.Empty, V10ToV11Patch.Apply },
            { "1.0", V10ToV11Patch.Apply },
            { "1.1", V11ToV12Patch.Apply },
            { "1.2", V12ToV13Patch.Apply },
            { "1.3", V13ToV14Patch.Apply },
            { "1.4", V14ToV15Patch.Apply },
            { "1.5", V15ToV16Patch.Apply }
        };

        private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static DungeonInfo ConvertDungeonInfoIfNeeded(this string dungeonJson, LocaleInfo localeTemplate, List<string> mandatoryLocaleKeys)
        {
            var jsonRoot = (JsonNode.Parse(dungeonJson)?.AsObject()) ?? throw new ArgumentException("Invalid dungeon JSON.");

            string? currentVersion = jsonRoot["Version"]?.ToString();

            while (currentVersion != EngineConstants.CurrentDungeonJsonVersion)
            {
                if (!PatchMap.TryGetValue(currentVersion ?? "", out var patch))
                    throw new ArgumentException($"There's no conversion method for Dungeon Version {currentVersion}");

                patch(jsonRoot);

                currentVersion = jsonRoot["Version"]?.ToString();
            }

            var finalDungeon = jsonRoot.Deserialize<DungeonInfo>(JsonSerializerOptions);
            var localesWereAdded = false;

            foreach (var localeInfo in finalDungeon.Locales)
            {
                localesWereAdded = localesWereAdded || localeInfo.AddMissingMandatoryLocalesIfNeeded(localeTemplate, mandatoryLocaleKeys);
            }

            if(localesWereAdded)
            {
                MessageBox.Show($"Dungeon has been found to lack certain required Locale entries.\n\nThey have been added for every Locale with a default value.\n\nRemember to update them accordingly, and save the Dungeon afterwards.", "Open Dungeon", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return finalDungeon!;
        }
    }
}
#pragma warning restore CA1416 // Validar la compatibilidad de la plataforma
