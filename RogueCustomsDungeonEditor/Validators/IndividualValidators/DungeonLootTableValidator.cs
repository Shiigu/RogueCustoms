using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Utils;
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
                        && !dungeonJson.ItemTypeInfos.Any(it => it.Id.Equals(entry.PickId, StringComparison.InvariantCultureIgnoreCase))
                        && !EngineConstants.SPECIAL_LOOT_ENTRIES.Any(lte => lte.Equals(entry.PickId, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        messages.AddError($"This Loot Table has an Entry {entry.PickId} but it is not a known Item, Item Type or Loot Table.");
                    }
                }

                if(lootTableJson.QualityLevelOdds == null)
                {
                    messages.AddError("This Loot Table has not set a Quality Level Odds table, not even a default, empty one.");
                }
                else
                {
                    foreach (var odd in lootTableJson.QualityLevelOdds)
                    {
                        var correspondingQualityLevel = dungeonJson.QualityLevelInfos.Find(qli => qli.Id.Equals(odd.Id));
                        if (correspondingQualityLevel == null)
                        {
                            messages.AddError("This Loot Table has odds for an Maximum Quality Level.");
                        }
                        else
                        {
                            if (odd.ChanceToPick < 0)
                                messages.AddError($"This Loot Table has an invalid Weight for Quality Level {odd.Id}. It must be a non-negative integer.");
                        }
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
