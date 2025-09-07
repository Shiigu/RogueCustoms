using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Utils.JsonImports
{
    [Serializable]
    public class LootTableInfo
    {
        public string Id { get; set; }
        public List<LootTableEntryInfo> Entries { get; set; }
        public bool OverridesQualityLevelOddsOfItems { get; set; }
        public List<QualityLevelOddsInfo> QualityLevelOdds { get; set; }
    }

    [Serializable]
    public class LootTableEntryInfo
    {
        public string PickId { get; set; }
        public int Weight { get; set; }
    }
}
