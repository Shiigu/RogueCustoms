using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
{
    public static class DungeonLootTableValidator
    {
        public static async Task<DungeonValidationMessages> Validate(LootTableInfo lootTableJson, DungeonInfo dungeonJson)
        {
            var messages = new DungeonValidationMessages();


            if (string.IsNullOrWhiteSpace(lootTableJson.Id))
            {
                messages.AddError($"This Loot Table does not have an Id.");
            }
            else
            {
                if(lootTableJson.Entries.Any(e => e.PickId.Equals(lootTableJson.Id,StringComparison.InvariantCultureIgnoreCase)))
                    messages.AddError($"This Loot Table causes a potential infinite loop because it has itself as an Entry.");
                var connectedLootTables = GetConnectedLootTables(lootTableJson, dungeonJson);
                if (connectedLootTables.Any(clt => clt.Equals(lootTableJson.Id)))
                    messages.AddError($"This Loot Table causes a potential infinite loop between its linked Loot Table Entries.");

                var duplicateEntries = lootTableJson.Entries
                    .GroupBy(e => e.PickId)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();

                if (duplicateEntries.Count > 0)
                {
                    messages.AddError($"This Loot Table has duplicate Entries.");
                }
                foreach (var entry in lootTableJson.Entries)
                {
                    if(entry.Weight <= 0)
                        messages.AddError($"This Loot Table has an Entry {entry.PickId} with a non-positive Weight.");
                    var match = Regex.Match(entry.PickId, EngineConstants.CurrencyRegexPattern);
                    if (match.Success)
                    {
                        var currencyPileId = match.Groups[1].Value;
                        if (!dungeonJson.CurrencyInfo.CurrencyPiles.Any(cp => cp.Id.Equals(currencyPileId, StringComparison.InvariantCultureIgnoreCase)))
                            messages.AddError($"This Loot Table has an Entry {entry.PickId} but {currencyPileId} is not a known Currency Pile Type.");
                    }
                    else if(!dungeonJson.Items.Any(i => i.Id.Equals(entry.PickId, StringComparison.InvariantCultureIgnoreCase))
                        && !dungeonJson.LootTableInfos.Any(lt => lt.Id.Equals(entry.PickId, StringComparison.InvariantCultureIgnoreCase))
                        && !EngineConstants.SPECIAL_LOOT_ENTRIES.Any(lte => lte.Equals(entry.PickId, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        messages.AddError($"This Loot Table has an Entry {entry.PickId} but it is not a known Item, Item Type or Loot Table.");
                    }
                }
            }

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }

        private static List<string> GetConnectedLootTables(LootTableInfo lootTableJson, DungeonInfo dungeonJson)
        {
            var connectedTables = new List<string>();
            foreach (var entry in lootTableJson.Entries)
            {
                if(dungeonJson.LootTableInfos.Any(lt => lt.Id.Equals(entry) && entry.Weight > 0))
                {
                    connectedTables.Add(entry.PickId);
                    var subTable = dungeonJson.LootTableInfos.First(lt => lt.Id.Equals(entry));
                    connectedTables.AddRange(GetConnectedLootTables(subTable, dungeonJson));
                }
            }
            return connectedTables;
        }
    }
}
