using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
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
                var connectedLootTables = GetConnectedLootTables(lootTableJson, dungeonJson);
                if (connectedLootTables.Any(clt => clt.Equals(lootTableJson.Id)))
                    messages.AddError($"This Loot Table causes a potential infinite loop between its linked Loot Tables.");

                var duplicateEntries = lootTableJson.Entries
                    .GroupBy(e => e.PickId)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();

                if(duplicateEntries.Count > 0)
                    messages.AddError($"This Loot Table has duplicate entries.");
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
