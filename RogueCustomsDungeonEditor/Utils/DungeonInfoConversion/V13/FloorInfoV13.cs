using System;
using System.Collections.Generic;

using RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V11;

using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V13
{
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.

    public class FloorInfoV13
    {
        public int MinFloorLevel { get; set; }
        public int MaxFloorLevel { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public bool GenerateStairsOnStart { get; set; }                   // If false, some entity must have a GenerateStairs effect.

        public string TileSetId { get; set; }

        public List<ClassInFloorInfoV13> PossibleMonsters { get; set; }
        public int SimultaneousMinMonstersAtStart { get; set; }
        public int SimultaneousMaxMonstersInFloor { get; set; }
        public int TurnsPerMonsterGeneration { get; set; }

        public List<ClassInFloorInfoV13> PossibleItems { get; set; }
        public int MinItemsInFloor { get; set; }
        public int MaxItemsInFloor { get; set; }

        public List<ClassInFloorInfoV13> PossibleTraps { get; set; }
        public int MinTrapsInFloor { get; set; }
        public int MaxTrapsInFloor { get; set; }

        public int MaxConnectionsBetweenRooms { get; set; }
        public int OddsForExtraConnections { get; set; }
        public int RoomFusionOdds { get; set; }

        public decimal HungerDegeneration { get; set; }

        public List<GeneratorAlgorithmInfoV13> PossibleGeneratorAlgorithms { get; set; }
        public ActionWithEffectsInfo OnFloorStart { get; set; } = new ActionWithEffectsInfo();
    }

    [Serializable]
    public class ClassInFloorInfoV13
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
    public class GeneratorAlgorithmInfoV13
    {
        public string Name { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
    }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
