using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace RogueCustomsGameEngine.Game.DungeonStructure
{
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
    [Serializable]
    public class FloorType
    {
        public readonly int MinFloorLevel;
        public readonly int MaxFloorLevel;

        public readonly int Width;
        public readonly int Height;
        public readonly bool GenerateStairsOnStart;                     // If false, some entity must have a GenerateStairs effect.

        public readonly string TileSetId;
        public TileSet TileSet { get; set; }

        private readonly List<ClassInFloorInfo> PossibleMonstersInfo;
        public List<ClassInFloor> PossibleMonsters { get; private set; }
        public readonly int SimultaneousMinMonstersAtStart;
        public readonly int SimultaneousMaxMonstersInFloor;
        public int TurnsPerMonsterGeneration { get; set; }
        private readonly List<ClassInFloorInfo> PossibleItemsInfo;
        public List<ClassInFloor> PossibleItems { get; private set; }
        public readonly int MinItemsInFloor;
        public readonly int MaxItemsInFloor;
        private readonly List<ClassInFloorInfo> PossibleTrapsInfo;
        public List<ClassInFloor> PossibleTraps { get; private set; }
        public readonly int MinTrapsInFloor;
        public readonly int MaxTrapsInFloor;

        public readonly int MaxConnectionsBetweenRooms;
        public readonly int OddsForExtraConnections;
        public readonly int RoomFusionOdds;
        public readonly int MonsterHouseOdds;

        public readonly decimal HungerDegeneration;

        public List<FloorLayoutGenerator> PossibleLayouts { get; private set; }
        public List<SpecialTileInFloor> PossibleSpecialTiles { get; private set; }
        public KeyDoorGenerator PossibleKeys { get; private set; }

        public readonly ActionWithEffects OnFloorStart;

        public FloorType(FloorInfo floorInfo, Locale locale, List<TileType> tileTypes, List<ActionSchool> actionSchools)
        {
            MinFloorLevel = floorInfo.MinFloorLevel;
            MaxFloorLevel = floorInfo.MaxFloorLevel;
            TileSetId = floorInfo.TileSetId;
            Width = floorInfo.Width;
            Height = floorInfo.Height;
            GenerateStairsOnStart = floorInfo.GenerateStairsOnStart;
            PossibleMonstersInfo = floorInfo.PossibleMonsters;
            SimultaneousMinMonstersAtStart = floorInfo.SimultaneousMinMonstersAtStart;
            SimultaneousMaxMonstersInFloor = floorInfo.SimultaneousMaxMonstersInFloor;
            TurnsPerMonsterGeneration = floorInfo.TurnsPerMonsterGeneration;
            PossibleItemsInfo = floorInfo.PossibleItems;
            MinItemsInFloor = floorInfo.MinItemsInFloor;
            MaxItemsInFloor = floorInfo.MaxItemsInFloor;
            PossibleTrapsInfo = floorInfo.PossibleTraps;
            MinTrapsInFloor = floorInfo.MinTrapsInFloor;
            MaxTrapsInFloor = floorInfo.MaxTrapsInFloor;
            MaxConnectionsBetweenRooms = floorInfo.MaxConnectionsBetweenRooms;
            OddsForExtraConnections = floorInfo.OddsForExtraConnections;
            RoomFusionOdds = floorInfo.RoomFusionOdds;
            MonsterHouseOdds = floorInfo.MonsterHouseOdds;
            HungerDegeneration = floorInfo.HungerDegeneration;
            PossibleLayouts = new List<FloorLayoutGenerator>();
            foreach (var layout in floorInfo.PossibleLayouts)
            {
                var matrixColumns = layout.Columns * 2 - 1;
                var matrixRows = layout.Rows * 2 - 1;
                var dispositionMatrix = new RoomDispositionType[matrixRows, matrixColumns];
                for (int i = 0; i < layout.RoomDisposition.Length; i++)
                {
                    var roomTile = layout.RoomDisposition[i];
                    (int X, int Y) = (i / matrixColumns, i % matrixColumns);
                    var isHallwayTile = (X % 2 != 0 && Y % 2 == 0) || (X % 2 == 0 && Y % 2 != 0);
                    dispositionMatrix[X, Y] = roomTile.ToRoomDispositionIndicator(isHallwayTile);
                }
                PossibleLayouts.Add(new FloorLayoutGenerator
                {
                    Rows = layout.Rows,
                    Columns = layout.Columns,
                    RoomDisposition = dispositionMatrix,
                    MinRoomSize = layout.MinRoomSize,
                    MaxRoomSize = layout.MaxRoomSize,
                });
            }
            OnFloorStart = ActionWithEffects.Create(floorInfo.OnFloorStart, actionSchools);
            PossibleKeys = new();
            if(floorInfo.PossibleKeys != null)
            {
                PossibleKeys.MaxPercentageOfLockedCandidateRooms = floorInfo.PossibleKeys.MaxPercentageOfLockedCandidateRooms;
                PossibleKeys.KeySpawnInEnemyInventoryOdds = floorInfo.PossibleKeys.KeySpawnInEnemyInventoryOdds;
                PossibleKeys.LockedRoomOdds = floorInfo.PossibleKeys.LockedRoomOdds;
                PossibleKeys.KeyTypes = new();
                foreach (var keyType in floorInfo.PossibleKeys.KeyTypes)
                {
                    var keyClassTemplate = new ItemInfo()
                    {
                        Id = $"KeyType{keyType.KeyTypeName}",
                        Name = $"KeyType{keyType.KeyTypeName}",
                        ConsoleRepresentation = keyType.KeyConsoleRepresentation,
                        Description = "KeyDescription",
                        EntityType = "Key",
                        StartsVisible = true,
                        Power = "0",
                        OnAttacked = new(),
                        OnDeath = new(),
                        OnTurnStart = new(),
                        OnUse = new(),
                        OnAttack = new() { Key.GetOpenDoorActionForKey(keyType.KeyTypeName) },
                    };
                    PossibleKeys.KeyTypes.Add(new()
                    {
                        KeyTypeName = keyType.KeyTypeName,
                        CanLockItems = keyType.CanLockItems,
                        CanLockStairs = keyType.CanLockStairs,
                        DoorConsoleRepresentation = keyType.DoorConsoleRepresentation,
                        KeyClass = new EntityClass(keyClassTemplate, locale, EntityType.Key, null, actionSchools, null)
                    });
                }
            }

            PossibleSpecialTiles = new();
            if(floorInfo.PossibleSpecialTiles != null && floorInfo.PossibleSpecialTiles.Any())
            {
                foreach (var specialTileGenerator in floorInfo.PossibleSpecialTiles)
                {
                    var tileType = tileTypes.Find(tt => tt.Id.Equals(specialTileGenerator.TileTypeId))
                        ?? throw new InvalidDataException($"There's a Special Tile generator algorithm with invalid Tile Type {specialTileGenerator.TileTypeId}");
                    if(specialTileGenerator.GeneratorType == null)
                        throw new InvalidDataException($"There's no Special Tile generator algorithm without a Generator Type");
                    PossibleSpecialTiles.Add(new SpecialTileInFloor
                    {
                        TileType = tileType,
                        MinSpecialTileGenerations = specialTileGenerator.MinSpecialTileGenerations,
                        MaxSpecialTileGenerations = specialTileGenerator.MaxSpecialTileGenerations,
                        GeneratorType = specialTileGenerator.GeneratorType.Value
                    });
                }
            }

            if (!PossibleLayouts.Any())
                throw new InvalidDataException("There's no valid generator algorithms for the current floor");
        }

        public void FillPossibleClassLists(List<EntityClass> classList)
        {
            PossibleMonsters = new List<ClassInFloor>();
            PossibleMonstersInfo.ForEach(pmi =>
            {
                var classForMonster = classList.Find(c => c.Id.Equals(pmi.ClassId) && c.EntityType == EntityType.NPC) ?? throw new InvalidDataException($"There's no class matching for {pmi.ClassId}!");
                PossibleMonsters.Add(new ClassInFloor
                {
                    Class = classForMonster,
                    MinLevel = pmi.MinLevel,
                    MaxLevel = pmi.MaxLevel,
                    OverallMaxForKindInFloor = pmi.OverallMaxForKindInFloor,
                    MinimumInFirstTurn = pmi.MinimumInFirstTurn,
                    SimultaneousMaxForKindInFloor = pmi.SimultaneousMaxForKindInFloor,
                    ChanceToPick = pmi.ChanceToPick,
                    CanSpawnOnFirstTurn = pmi.CanSpawnOnFirstTurn,
                    CanSpawnAfterFirstTurn = pmi.CanSpawnAfterFirstTurn,
                    SpawnCondition = pmi.SpawnCondition
                });
            });
            PossibleItems = new List<ClassInFloor>();
            PossibleItemsInfo.ForEach(pii =>
            {
                var classForItem = classList.Find(c => c.Id.Equals(pii.ClassId) && (c.EntityType == EntityType.Weapon || c.EntityType == EntityType.Armor || c.EntityType == EntityType.Consumable)) ?? throw new InvalidDataException($"There's no class matching for {pii.ClassId}!");
                PossibleItems.Add(new ClassInFloor
                {
                    Class = classForItem,
                    MinimumInFirstTurn = pii.MinimumInFirstTurn,
                    SimultaneousMaxForKindInFloor = pii.SimultaneousMaxForKindInFloor,
                    ChanceToPick = pii.ChanceToPick,
                    SpawnCondition = pii.SpawnCondition
                });
            });
            PossibleTraps = new List<ClassInFloor>();
            PossibleTrapsInfo.ForEach(pti =>
            {
                var classForTrap = classList.Find(c => c.Id.Equals(pti.ClassId) && c.EntityType == EntityType.Trap) ?? throw new InvalidDataException($"There's no class matching for {pti.ClassId}!");
                PossibleTraps.Add(new ClassInFloor
                {
                    Class = classForTrap,
                    MinimumInFirstTurn = pti.MinimumInFirstTurn,
                    SimultaneousMaxForKindInFloor = pti.SimultaneousMaxForKindInFloor,
                    ChanceToPick = pti.ChanceToPick,
                    SpawnCondition = pti.SpawnCondition
                });
            });
        }

        protected static void MapActions(List<ActionWithEffects> actionList, List<ActionWithEffectsInfo> actionInfoList, List<ActionSchool> actionSchools)
        {
            actionInfoList.ForEach(aa => actionList.Add(ActionWithEffects.Create(aa, actionSchools)));
        }
    }

    [Serializable]
    public class ClassInFloor
    {
        public EntityClass Class { get; set; }

        #region Monster-only
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
        public int OverallMaxForKindInFloor { get; set; }
        public bool CanSpawnOnFirstTurn { get; set; }
        public bool CanSpawnAfterFirstTurn { get; set; }
        #endregion
        public int TotalGeneratedInFloor { get; set; }
        public int MinimumInFirstTurn { get; set; }
        public int SimultaneousMaxForKindInFloor { get; set; }
        public int ChanceToPick { get; set; }
        public string SpawnCondition { get; set; }
    }

    [Serializable]
    public class FloorLayoutGenerator
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public RoomDispositionType[,] RoomDisposition { get; set; }
        public RoomDimensionsInfo MinRoomSize { get; set; }
        public RoomDimensionsInfo MaxRoomSize { get; set; }
    }

    [Serializable]
    public class SpecialTileInFloor
    {
        public TileType TileType { get; set; }
        public SpecialTileGenerationAlgorithm GeneratorType { get; set; }
        public int MinSpecialTileGenerations { get; set; }
        public int MaxSpecialTileGenerations { get; set; }
    }

    public enum SpecialTileGenerationAlgorithm
    {
        River,
        Lake
    }

    [Serializable]
    public class KeyDoorGenerator
    {
        public int LockedRoomOdds { get; set; }
        public int KeySpawnInEnemyInventoryOdds { get; set; }
        public int MaxPercentageOfLockedCandidateRooms { get; set; }
        public List<KeyType> KeyTypes { get; set; }
    }

    [Serializable]
    public class KeyType
    {
        public string KeyTypeName { get; set; }
        public EntityClass KeyClass { get; set; }
        public bool CanLockStairs { get; set; }
        public bool CanLockItems { get; set; }
        public ConsoleRepresentation DoorConsoleRepresentation { get; set; }
    }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}
