using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Utils;
using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsGameEngine.Game.DungeonStructure
{
    [Serializable]
    public class LootTable
    {
        [JsonIgnore]
        private LootTableInfo _baseInfo;

        public string Id { get; set; }
        public List<LootTableEntry> Entries { get; set; }

        public LootTable(LootTableInfo info)
        {
            Id = info.Id;
            _baseInfo = info;
        }

        public void FillEntries(Dungeon dungeon)
        {
            Entries = [];
            foreach (var entry in _baseInfo.Entries)
            {
                Entries.Add(new(entry, dungeon));
            }
        }
    }

    [Serializable]
    public class LootTableEntry
    {
        public object Pick { get; set; }
        public int Weight { get; set; }

        public LootTableEntry(LootTableEntryInfo info, Dungeon dungeon)
        {
            Pick = null;

            if (EngineConstants.SPECIAL_LOOT_ENTRIES.Contains(info.PickId))
                Pick = info.PickId;

            if (Pick == null)
            {
                var pickAsItem = dungeon.ItemClasses.FirstOrDefault(i => i.Id.Equals(info.PickId, StringComparison.InvariantCultureIgnoreCase));
                if (pickAsItem != null) Pick = pickAsItem;
            }

            if (Pick == null)
            {
                var pickAsLootTable = dungeon.LootTables.FirstOrDefault(lt => lt.Id.Equals(info.PickId, StringComparison.InvariantCultureIgnoreCase));
                if (pickAsLootTable != null) Pick = pickAsLootTable;
            }

            if (Pick == null) throw new InvalidDataException($"Loot Table has an Entry with PickId {info.PickId}, that does not correspond to any ItemClass or LootTable in the dungeon.");

            Weight = info.Weight;
        }
    }
}
