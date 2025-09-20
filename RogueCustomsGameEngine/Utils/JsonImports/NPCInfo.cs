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
        public bool ReappearsOnTheNextFloorIfAlliedToThePlayer { get; set; }
        public NPCLootTableDataInfo RegularLootTable { get; set; }
        public List<NPCModifierDataInfo> ModifierData { get; set; }
        public int OddsForModifier { get; set; }
        public bool RandomizesForecolorIfWithModifiers { get; set; }
        public decimal BaseHPMultiplierIfWithModifiers { get; set; }
        public decimal ExperienceYieldMultiplierIfWithModifiers { get; set; }
        public NPCLootTableDataInfo LootTableWithModifiers { get; set; }
    }

    [Serializable]
    public class NPCModifierDataInfo
    {
        public int Level { get; set; }
        public int ModifierAmount { get; set; }
    }

    [Serializable]
    public class NPCLootTableDataInfo
    {
        public string LootTableId { get; set; }
        public int DropPicks { get; set; }
    }
}