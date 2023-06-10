using System;
using System.Collections.Generic;

namespace RogueCustomsGameEngine.Utils.JsonImports
{
    [Serializable]
    public class FloorInfo
    {
        public int MinFloorLevel { get; set; }
        public int MaxFloorLevel { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public bool GenerateStairsOnStart { get; set; }                   // If false, some entity must have a GenerateStairs effect.

        public List<ClassInFloorInfo> PossibleMonsters { get; set; }
        public int SimultaneousMinMonstersAtStart { get; set; }
        public int SimultaneousMaxMonstersInFloor { get; set; }
        public int TurnsPerMonsterGeneration { get; set; }

        public List<ClassInFloorInfo> PossibleItems { get; set; }
        public int MinItemsInFloor { get; set; }
        public int MaxItemsInFloor { get; set; }

        public List<ClassInFloorInfo> PossibleTraps { get; set; }
        public int MinTrapsInFloor { get; set; }
        public int MaxTrapsInFloor { get; set; }

        public int MaxConnectionsBetweenRooms { get; set; }
        public int OddsForExtraConnections { get; set; }
        public int RoomFusionOdds { get; set; }

        public List<GeneratorAlgorithmInfo> PossibleGeneratorAlgorithms { get; set; }
        public List<ActionWithEffectsInfo> OnFloorStartActions { get; set; } = new List<ActionWithEffectsInfo>();
    }

    [Serializable]
    public class ClassInFloorInfo
    {
        public string ClassId { get; set; }

        #region Monster-only
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
        public int OverallMaxForKindInFloor { get; set; }
        public bool CanSpawnOnFirstTurn { get; set; }
        public bool CanSpawnAfterFirstTurn { get; set; }
        #endregion
        public int SimultaneousMaxForKindInFloor { get; set; }
        public int ChanceToPick { get; set; }

    }

    [Serializable]
    public class GeneratorAlgorithmInfo
    {
        public string Name { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
    }
}
