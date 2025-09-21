using System;
using System.Collections.Generic;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsGameEngine.Utils.JsonImports
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public class FloorInfo
    {
        public int MinFloorLevel { get; set; }
        public int MaxFloorLevel { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public bool GenerateStairsOnStart { get; set; }                   // If false, some entity must have a GenerateStairs effect.

        public string TileSetId { get; set; }

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
        public int MonsterHouseOdds { get; set; }

        public decimal HungerDegeneration { get; set; }
        public List<FloorLayoutGenerationInfo> PossibleLayouts { get; set; }
        public List<SpecialTileInFloorInfo> PossibleSpecialTiles { get; set; }
        public KeyGenerationInfo PossibleKeys { get; set; }
        public ActionWithEffectsInfo OnFloorStart { get; set; }
    }

    [Serializable]
    public class ClassInFloorInfo
    {
        public string ClassId { get; set; }

        #region Monster-only
        public int OverallMaxForKindInFloor { get; set; }
        public bool CanSpawnOnFirstTurn { get; set; }
        public bool CanSpawnAfterFirstTurn { get; set; }
        #endregion

        #region Not for Traps        
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
        #endregion

        public int MinimumInFirstTurn { get; set; }
        public int SimultaneousMaxForKindInFloor { get; set; }
        public int ChanceToPick { get; set; }
        public string SpawnCondition { get; set; }
    }

    [Serializable]
    public class SpecialTileInFloorInfo
    {
        public string TileTypeId { get; set; }
        public SpecialTileGenerationAlgorithm? GeneratorType { get; set; }
        public int MinSpecialTileGenerations { get; set; }
        public int MaxSpecialTileGenerations { get; set; }
    }

    [Serializable]
    public class FloorLayoutGenerationInfo
    {
        public string Name { get; set; }
        public ProceduralGeneratorInfo ProceduralGenerator { get; set; }
        public StaticGeneratorInfo StaticGenerator { get; set; }
    }

    [Serializable]
    public class ProceduralGeneratorInfo
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public string RoomDisposition { get; set; }
        public RoomDimensionsInfo MinRoomSize { get; set; }
        public RoomDimensionsInfo MaxRoomSize { get; set; }
    }

    [Serializable]
    public class RoomDimensionsInfo
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }

    [Serializable]
    public class StaticGeneratorInfo
    {
        public string FloorGeometry { get; set; }
        public List<SpecialSpawnInfo> SpecialSpawns { get; set; }
        public bool GenerateNPCsAfterFirstTurn { get; set; }
        public bool GenerateItemsOnFirstTurn { get; set; }
        public bool GenerateTrapsOnFirstTurn { get; set; }
    }

    [Serializable]
    public class SpecialSpawnInfo
    {
        public string SpawnId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Level { get; set; }
        public string QualityLevel { get; set; }
        public string WaypointId { get; set; }
    }

    [Serializable]
    public class KeyGenerationInfo
    {
        public int LockedRoomOdds { get; set; }
        public int KeySpawnInEnemyInventoryOdds { get; set; }
        public int MaxPercentageOfLockedCandidateRooms { get; set; }
        public List<KeyTypeInfo> KeyTypes { get; set; }
    }

    [Serializable]
    public class KeyTypeInfo
    {
        public string KeyTypeName { get; set; }
        public bool CanLockStairs { get; set; }
        public bool CanLockItems { get; set; }
        public ConsoleRepresentation KeyConsoleRepresentation { get; set; }
        public ConsoleRepresentation DoorConsoleRepresentation { get; set; }
    }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
