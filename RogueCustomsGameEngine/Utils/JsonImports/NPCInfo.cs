using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;

namespace RogueCustomsGameEngine.Utils.JsonImports
{
    [Serializable]
    public class NPCInfo : CharacterInfo
    {
        public string AIType { get; set; }
        public bool KnowsAllCharacterPositions { get; set; }
        public bool PursuesOutOfSightCharacters { get; set; }
        public bool WandersIfWithoutTarget { get; set; }
        public ActionWithEffectsInfo OnSpawn { get; set; }
        public List<ActionWithEffectsInfo> OnInteracted { get; set; }
        public bool DropsEquipmentOnDeath { get; set; }
        public string LootTableId { get; set; }
        public int DropPicks { get; set; }
    }
}