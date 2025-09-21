using org.matheval.Node;

using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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

        public List<Generator> PossibleLayouts { get; private set; }
        public List<SpecialTileInFloor> PossibleSpecialTiles { get; private set; }
        public KeyDoorGenerator PossibleKeys { get; private set; }

        public readonly ActionWithEffects OnFloorStart;

        public FloorType(FloorInfo floorInfo, Dungeon dungeon)
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
            OnFloorStart = ActionWithEffects.Create(floorInfo.OnFloorStart, dungeon.ActionSchools);
            PossibleKeys = new();
            if(floorInfo.PossibleKeys != null)
            {
                PossibleKeys.MaxPercentageOfLockedCandidateRooms = floorInfo.PossibleKeys.MaxPercentageOfLockedCandidateRooms;
                PossibleKeys.KeySpawnInEnemyInventoryOdds = floorInfo.PossibleKeys.KeySpawnInEnemyInventoryOdds;
                PossibleKeys.LockedRoomOdds = floorInfo.PossibleKeys.LockedRoomOdds;
                PossibleKeys.KeyTypes = new();
                foreach (var keyType in floorInfo.PossibleKeys.KeyTypes)
                {
                    PossibleKeys.KeyTypes.Add(keyType.Parse(dungeon));
                }
            }

            PossibleSpecialTiles = new();
            if(floorInfo.PossibleSpecialTiles != null && floorInfo.PossibleSpecialTiles.Any())
            {
                foreach (var specialTileGenerator in floorInfo.PossibleSpecialTiles)
                {
                    var tileType = dungeon.TileTypes.Find(tt => tt.Id.Equals(specialTileGenerator.TileTypeId))
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


            PossibleLayouts = new List<Generator>();
            foreach (var layout in floorInfo.PossibleLayouts)
            {
                if (layout.ProceduralGenerator != null)
                {
                    var matrixColumns = layout.ProceduralGenerator.Columns * 2 - 1;
                    var matrixRows = layout.ProceduralGenerator.Rows * 2 - 1;
                    var dispositionMatrix = new RoomDispositionType[matrixRows, matrixColumns];
                    for (int i = 0; i < layout.ProceduralGenerator.RoomDisposition.Length; i++)
                    {
                        var roomTile = layout.ProceduralGenerator.RoomDisposition[i];
                        (int X, int Y) = (i / matrixColumns, i % matrixColumns);
                        var isHallwayTile = (X % 2 != 0 && Y % 2 == 0) || (X % 2 == 0 && Y % 2 != 0);
                        dispositionMatrix[X, Y] = roomTile.ToRoomDispositionIndicator(isHallwayTile);
                    }
                    PossibleLayouts.Add(new ProceduralGenerator
                    {
                        Name = layout.Name,
                        Rows = layout.ProceduralGenerator.Rows,
                        Columns = layout.ProceduralGenerator.Columns,
                        RoomDisposition = dispositionMatrix,
                        MinRoomSize = layout.ProceduralGenerator.MinRoomSize,
                        MaxRoomSize = layout.ProceduralGenerator.MaxRoomSize,
                    });
                }
                else if (layout.StaticGenerator != null)
                {
                    var floorGeometry = new TileType[floorInfo.Height, floorInfo.Width];
                    var specialSpawns = new List<SpecialSpawn>();
                    for (int i = 0; i < layout.StaticGenerator.FloorGeometry.Length; i++)
                    {
                        var tile = layout.StaticGenerator.FloorGeometry[i];
                        var tileTypeId = tile switch
                        {
                            '-' => "Empty",
                            '#' => "Wall",
                            '.' => "Floor",
                            '+' => "Hallway",
                            _ => "Empty"
                        };
                        var tileType = dungeon.TileTypes.First(tt => tt.Id.Equals(tileTypeId));
                        (int X, int Y) = (i % floorInfo.Width, i / floorInfo.Width);
                        floorGeometry[Y, X] = tileType;
                        var specialSpawnForTile = layout.StaticGenerator.SpecialSpawns?.FirstOrDefault(ss => ss.X == X + 1 && ss.Y == Y + 1);
                        if (specialSpawnForTile != null)
                        {
                            var adjustedX = specialSpawnForTile.X - 1;
                            var adjustedY = specialSpawnForTile.Y - 1;
                            var spawn = CreateSpecialSpawn(specialSpawnForTile, this, dungeon, adjustedX, adjustedY);
                            if (spawn != null)
                            {
                                specialSpawns.Add(spawn);
                            }
                        }
                    }
                    PossibleLayouts.Add(new StaticGenerator
                    {
                        Name = layout.Name,
                        FloorGeometry = floorGeometry,
                        SpecialSpawns = specialSpawns,
                        GenerateNPCsAfterFirstTurn = layout.StaticGenerator.GenerateNPCsAfterFirstTurn,
                        GenerateItemsOnFirstTurn = layout.StaticGenerator.GenerateItemsOnFirstTurn,
                        GenerateTrapsOnFirstTurn = layout.StaticGenerator.GenerateTrapsOnFirstTurn
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
                PossibleMonsters.Add(new ClassInFloor
                {
                    ClassId = pmi.ClassId,
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
                PossibleItems.Add(new ClassInFloor
                {
                    ClassId = pii.ClassId,
                    MinimumInFirstTurn = pii.MinimumInFirstTurn,
                    SimultaneousMaxForKindInFloor = pii.SimultaneousMaxForKindInFloor,
                    ChanceToPick = pii.ChanceToPick,
                    SpawnCondition = pii.SpawnCondition,
                    MinLevel = pii.MinLevel,
                    MaxLevel = pii.MaxLevel
                });
            });
            PossibleTraps = new List<ClassInFloor>();
            PossibleTrapsInfo.ForEach(pti =>
            {
                PossibleTraps.Add(new ClassInFloor
                {
                    ClassId = pti.ClassId,
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

        private static SpecialSpawn? CreateSpecialSpawn(SpecialSpawnInfo spawnTileInfo, FloorType floorType, Dungeon dungeon, int x, int y)
        {
            if (spawnTileInfo.SpawnId == "Stairs")
            {
                return new SpecialSpawn { X = x, Y = y, ObjectToSpawn = TileType.Stairs };
            }

            if (TryHandleTileType(spawnTileInfo, dungeon, x, y, out var special)) return special;
            if (TryHandleKey(spawnTileInfo, floorType, x, y, out special)) return special;
            if (TryHandleDoor(spawnTileInfo, floorType, x, y, out special)) return special;
            if (TryHandleCharacter(spawnTileInfo, dungeon, x, y, out special)) return special;
            if (TryHandleItem(spawnTileInfo, dungeon, x, y, out special)) return special;
            if (TryHandleTrap(spawnTileInfo, dungeon, x, y, out special)) return special;
            if (TryHandleCurrency(spawnTileInfo, dungeon, x, y, out special)) return special;
            if (TryHandleWaypoint(spawnTileInfo, x, y, out special)) return special;

            return null;
        }

        private static bool TryHandleTileType(SpecialSpawnInfo spawnTileInfo, Dungeon dungeon, int x, int y, out SpecialSpawn? result)
        {
            var specialTileType = dungeon.TileTypes.Find(tt => tt.Id.Equals(spawnTileInfo.SpawnId));
            if (specialTileType != null)
            {
                result = new SpecialSpawn { X = x, Y = y, ObjectToSpawn = specialTileType };
                return true;
            }
            result = null;
            return false;
        }

        private static bool TryHandleKey(SpecialSpawnInfo spawnTileInfo, FloorType floorType, int x, int y, out SpecialSpawn? result)
        {
            if (spawnTileInfo.SpawnId.Contains("Key"))
            {
                var keyTypeString = spawnTileInfo.SpawnId.Replace("Key", "").Trim();
                var keyType = floorType.PossibleKeys.KeyTypes.Find(kt => kt.KeyTypeName.Equals(keyTypeString));
                if (keyType != null)
                {
                    result = new SpecialSpawn { X = x, Y = y, ObjectToSpawn = keyType.KeyClass };
                    return true;
                }
            }
            result = null;
            return false;
        }

        private static bool TryHandleDoor(SpecialSpawnInfo spawnTileInfo, FloorType floorType, int x, int y, out SpecialSpawn? result)
        {
            if (spawnTileInfo.SpawnId.Contains("Door"))
            {
                var keyTypeString = spawnTileInfo.SpawnId.Replace("Door", "").Trim();
                var keyType = floorType.PossibleKeys.KeyTypes.Find(kt => kt.KeyTypeName.Equals(keyTypeString));
                if (keyType != null)
                {
                    result = new SpecialSpawn { X = x, Y = y, ObjectToSpawn = $"DoorType{keyType.KeyTypeName}" };
                    return true;
                }
            }
            result = null;
            return false;
        }

        private static bool TryHandleCharacter(SpecialSpawnInfo spawnTileInfo, Dungeon dungeon, int x, int y, out SpecialSpawn? result)
        {
            var characterClass = dungeon.CharacterClasses.Find(cc => cc.Id.Equals(spawnTileInfo.SpawnId));
            if (characterClass != null || spawnTileInfo.SpawnId == EngineConstants.SPAWN_ANY_CHARACTER ||
                spawnTileInfo.SpawnId == EngineConstants.SPAWN_ANY_ALLIED_CHARACTER_INCLUDING_PLAYER ||
                spawnTileInfo.SpawnId == EngineConstants.SPAWN_ANY_ALLIED_CHARACTER ||
                spawnTileInfo.SpawnId == EngineConstants.SPAWN_ANY_NEUTRAL_CHARACTER ||
                spawnTileInfo.SpawnId == EngineConstants.SPAWN_ANY_ENEMY_CHARACTER ||
                spawnTileInfo.SpawnId == EngineConstants.SPAWN_PLAYER_CHARACTER)
            {
                result = new SpecialSpawn
                {
                    X = x,
                    Y = y,
                    ObjectToSpawn = characterClass ?? (object)spawnTileInfo.SpawnId,
                    Level = spawnTileInfo.Level
                };
                return true;
            }
            result = null;
            return false;
        }

        private static bool TryHandleItem(SpecialSpawnInfo spawnTileInfo, Dungeon dungeon, int x, int y, out SpecialSpawn? result)
        {
            var itemClass = dungeon.ItemClasses.Find(ic => ic.Id.Equals(spawnTileInfo.SpawnId));
            if (itemClass != null || spawnTileInfo.SpawnId == EngineConstants.SPAWN_ANY_ITEM)
            {
                var qualityLevel = dungeon.QualityLevels.Find(ql => ql.Id.Equals(spawnTileInfo.QualityLevel))
                    ?? dungeon.QualityLevels.MinBy(ql => ql.MaximumAffixes);

                result = new SpecialSpawn
                {
                    X = x,
                    Y = y,
                    ObjectToSpawn = itemClass ?? (object)spawnTileInfo.SpawnId,
                    Level = spawnTileInfo.Level,
                    QualityLevel = qualityLevel
                };
                return true;
            }
            result = null;
            return false;
        }

        private static bool TryHandleTrap(SpecialSpawnInfo spawnTileInfo, Dungeon dungeon, int x, int y, out SpecialSpawn? result)
        {
            var trapClass = dungeon.TrapClasses.Find(tc => tc.Id.Equals(spawnTileInfo.SpawnId));
            if (trapClass != null || spawnTileInfo.SpawnId == EngineConstants.SPAWN_ANY_TRAP)
            {
                result = new SpecialSpawn { X = x, Y = y, ObjectToSpawn = trapClass ?? (object)spawnTileInfo.SpawnId };
                return true;
            }
            result = null;
            return false;
        }

        private static bool TryHandleCurrency(SpecialSpawnInfo spawnTileInfo, Dungeon dungeon, int x, int y, out SpecialSpawn? result)
        {
            var match = Regex.Match(spawnTileInfo.SpawnId, EngineConstants.CurrencyRegexPattern);
            if (match.Success)
            {
                var currencyPileId = match.Groups[1].Value;
                var currencyPile = dungeon.CurrencyData.Find(cp => cp.Id.Equals(currencyPileId));
                if (currencyPile != null)
                {
                    result = new SpecialSpawn { X = x, Y = y, ObjectToSpawn = spawnTileInfo.SpawnId };
                    return true;
                }
            }
            result = null;
            return false;
        }

        private static bool TryHandleWaypoint(SpecialSpawnInfo spawnTileInfo, int x, int y, out SpecialSpawn? result)
        {
            if (spawnTileInfo.SpawnId == EngineConstants.CREATE_WAYPOINT)
            {
                result = new SpecialSpawn { X = x, Y = y, ObjectToSpawn = EngineConstants.CREATE_WAYPOINT, WaypointId = spawnTileInfo.WaypointId };
                return true;
            }
            result = null;
            return false;
        }
    }

    [Serializable]
    public class ClassInFloor
    {
        public string ClassId { get; set; }

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
    public abstract class Generator
    {
        public string Name { get; set; }
    }

    [Serializable]
    public class ProceduralGenerator : Generator
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public RoomDispositionType[,] RoomDisposition { get; set; }
        public RoomDimensionsInfo MinRoomSize { get; set; }
        public RoomDimensionsInfo MaxRoomSize { get; set; }
    }

    [Serializable]
    public class StaticGenerator : Generator
    {
        public TileType[,] FloorGeometry { get; set; }
        public List<SpecialSpawn> SpecialSpawns { get; set; }
        public bool GenerateNPCsAfterFirstTurn { get; set; }
        public bool GenerateItemsOnFirstTurn { get; set; }
        public bool GenerateTrapsOnFirstTurn { get; set; }
    }

    [Serializable]
    public class SpecialSpawn
    {
        public object ObjectToSpawn { get; set; } // Can be TileType or EntityClass
        public int X { get; set; }
        public int Y { get; set; }
        public int Level { get; set; } // Only for NPCs and Items
        public QualityLevel QualityLevel { get; set; } // Only for Items
        public string WaypointId { get; set; } // Only for Waypoints
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
