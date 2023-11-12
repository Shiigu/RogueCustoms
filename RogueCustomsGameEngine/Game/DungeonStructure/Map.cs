using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils;
using RogueCustomsGameEngine.Utils.Effects;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.Helpers;
using Point = RogueCustomsGameEngine.Utils.Representation.Point;
using System.Drawing;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsGameEngine.Utils.Representation;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Immutable;

namespace RogueCustomsGameEngine.Game.DungeonStructure
{
    #pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
    #pragma warning disable CS8601 // Posible asignación de referencia nula
    #pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
    #pragma warning disable CS8604 // Posible argumento de referencia nulo
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public class Map
    {
        #region Fields

        private int _generationTries;

        private int CurrentEntityId;

        private bool _displayedTurnMessage;

        public PlayerCharacter Player => Dungeon.PlayerCharacter;

        public int TurnCount { get; set; }
        private int LastMonsterGenerationTurn;
        private int LatestPlayerRemainingMovement;

        public int Id { get; }

        private readonly Dungeon Dungeon;
        public Locale Locale => Dungeon.LocaleToUse;
        public DungeonStatus DungeonStatus
        {
            get { return Dungeon.DungeonStatus; }
            set { Dungeon.DungeonStatus = value; }
        }

        private readonly int FloorLevel;

        public readonly FloorType FloorConfigurationToUse;

        private readonly GeneratorAlgorithm GeneratorAlgorithmToUse;
        public TileSet TileSet => FloorConfigurationToUse.TileSet;

        public string FloorName => Locale["FloorName"].Format(new {
                                                                DungeonName = Dungeon.Name.ToUpperInvariant(),
                                                                FloorLevel = FloorLevel.ToString()
                                                                });

        private int RoomCountRows => GeneratorAlgorithmToUse.Rows;
        private int RoomCountColumns => GeneratorAlgorithmToUse.Columns;
        public int MaxConnectionsBetweenRooms => FloorConfigurationToUse.MaxConnectionsBetweenRooms;
        public int OddsForExtraConnections => FloorConfigurationToUse.OddsForExtraConnections;
        public int RoomFusionOdds => FloorConfigurationToUse.RoomFusionOdds;

        public int Width { get; private set; }
        public int Height { get; private set; }

        private int TotalMonstersInFloor => AICharacters.Count(e => e.ExistenceStatus == EntityExistenceStatus.Alive);
        private int TotalItemsInFloor => Items.Count(e => e.ExistenceStatus != EntityExistenceStatus.Gone);
        private int TotalTrapsInFloor => Traps.Count(e => e.ExistenceStatus != EntityExistenceStatus.Gone);
        private int MinRoomWidth { get; set; }
        private int MaxRoomWidth { get; set; }
        private int MinRoomHeight { get; set; }
        private int MaxRoomHeight { get; set; }
        public bool StairsAreSet { get; set; } = false;
        public bool PlayerTookDamage { get; set; } = false;
        public bool PlayerGotHealed { get; set; } = false;
        public bool PlayerGotMPBurned { get; set; } = false;
        public bool PlayerGotMPReplenished { get; set; } = false;
        public List<NonPlayableCharacter> AICharacters { get; set; }
        public List<Item> Items { get; set; }
        public List<Item> Traps { get; set; }

        public ImmutableList<AlteredStatus> PossibleStatuses { get; private set; }

        public readonly RngHandler Rng;
        private (Point TopLeftCorner, Point BottomRightCorner)[,] RoomLimitsTable { get; set; }

        public Tile[,] Tiles { get; private set; }

        public List<Room> Rooms { get; private set; }
        private RoomConnectionType[,] RoomAdjacencyMatrix;

        public List<Flag> Flags { get; set; }

        #endregion

        public Map(Dungeon dungeon, int floorLevel, List<Flag> flags)
        {
            Dungeon = dungeon;
            FloorLevel = floorLevel;
            AICharacters = new List<NonPlayableCharacter>();
            Items = new List<Item>();
            Traps = new List<Item>();
            FloorConfigurationToUse = Dungeon.FloorTypes.Find(ft => floorLevel.Between(ft.MinFloorLevel, ft.MaxFloorLevel));
            if (FloorConfigurationToUse == null)
                throw new InvalidDataException("There's no valid configuration for the current floor");
            Width = FloorConfigurationToUse.Width;
            Height = FloorConfigurationToUse.Height;
            Rng = new RngHandler(Environment.TickCount);
            Flags = flags;
            if (!FloorConfigurationToUse.PossibleGeneratorAlgorithms.Any())
                throw new InvalidDataException("There's no valid generation algorithm for the current floor");
            ConsoleRepresentation.EmptyTile = TileSet.Empty;
            GeneratorAlgorithmToUse = FloorConfigurationToUse.PossibleGeneratorAlgorithms[Rng.NextInclusive(FloorConfigurationToUse.PossibleGeneratorAlgorithms.Count - 1)];

            PossibleStatuses = ImmutableList.Create<AlteredStatus>();
            Dungeon.Classes.Where(c => c.EntityType == EntityType.AlteredStatus).ForEach(alsc => PossibleStatuses = PossibleStatuses.Add(new AlteredStatus(alsc, this)));

            AttackActions.SetActionParams(Rng, this);
            CharacterActions.SetActionParams(Rng, this);
            ItemActions.SetActionParams(Rng, this);
            GenericActions.SetActionParams(Rng, this);
            ActionHelpers.SetActionParams(Rng, this);
        }

        public void Generate()
        {
            _generationTries = 0;
            switch (GeneratorAlgorithmToUse.Type)
            {
                case GeneratorAlgorithmTypes.Standard:
                    GenerateStandard();
                    break;
                case GeneratorAlgorithmTypes.OuterDummyRing:
                    GenerateWithOuterDummyRing();
                    break;
                case GeneratorAlgorithmTypes.InnerDummyRing:
                    GenerateWithInnerDummyRing();
                    break;
                case GeneratorAlgorithmTypes.OneBigRoom:
                    GenerateOneBigRoom();
                    break;
            }
            NewTurn();
        }

        private Room? GetRoomByRowAndColumn(int row, int column) => Rooms.Find(r => r.RoomRow == row && r.RoomColumn == column);

        public void AppendMessage(string message) => AppendMessage(message, new GameColor(Color.White), new GameColor(Color.Transparent));
        public void AppendMessage(string message, Color foregroundColor) => AppendMessage(message, new GameColor(foregroundColor));
        public void AppendMessage(string message, GameColor foregroundColor) => AppendMessage(message, foregroundColor, new GameColor(Color.Transparent));
        public void AppendMessage(string message, Color foregroundColor, Color backgroundColor) => AppendMessage(message, new GameColor(foregroundColor), new GameColor(backgroundColor));
        public void AppendMessage(string message, GameColor foregroundColor, GameColor backgroundColor)
        {
            if (string.IsNullOrWhiteSpace(message)) return;
            if (!_displayedTurnMessage && TurnCount > 0)
            {
                Dungeon.Messages.Add(new MessageDto
                {
                    Message = Locale["NewTurn"].Format(new { TurnCount = TurnCount.ToString() }),
                    ForegroundColor = new GameColor(Color.Yellow)
                });
                _displayedTurnMessage = true;
            }
            Dungeon.Messages.Add(new MessageDto
            {
                Message = message,
                ForegroundColor = foregroundColor,
                BackgroundColor = backgroundColor
            });
        }
        public void AddMessageBox(string title, string message, string buttonCaption, GameColor windowColor)
        {
            Dungeon.AddMessageBox(title, message, buttonCaption, windowColor);
        }

        #region Floor algorithms

        public void GenerateDummyMap()
        {
            _generationTries = 0;
            Width = 32;
            Height = 16;
            ResetAndCreateTiles();
            var room = new Room(this, new Point(0, 0), 0, 0, 25, 10);
            Rooms = new List<Room> { room };
            room.CreateTiles();
            PlacePlayer();
            Player.Position = new Point(5, 3);
            SetStairs(new Point(19, 7));
            AppendMessage("This is a dummy map");
        }

        private void GenerateStandard()
        {
            bool success;
            do
            {
                _generationTries++;
                ResetAndCreateTiles();

                CreateRoomsWithRandomizedConfig();
                success = ConnectRoomsOrTurnIntoDefaultIfNotPossible();
            }
            while (!success && _generationTries < Constants.MaxGenerationTries);
            if (!success)
            {
                GeneratorAlgorithmToUse.Rows = 1;
                GeneratorAlgorithmToUse.Columns = 1;
                GenerateOneBigRoom();
            }
        }

        private void GenerateWithInnerDummyRing()
        {
            // Set room parameters

            var forcedNormals = new List<(int, int)>();
            var forcedDummies = new List<(int, int)>();

            for (int row = 0; row < RoomCountRows; row++)
            {
                for (int column = 0; column < RoomCountColumns; column++)
                {
                    if (row != 0 && row != RoomCountRows - 1
                        && column != 0 && column != RoomCountColumns - 1)
                    {
                        forcedDummies.Add((row, column));
                    }
                    else
                    {
                        forcedNormals.Add((row, column));
                    }
                }
            }

            bool success;
            do
            {
                _generationTries++;
                ResetAndCreateTiles();

                CreateRoomsWithSpecificLayout(forcedNormals, forcedDummies);
                success = ConnectRoomsOrTurnIntoDefaultIfNotPossible();
            }
            while (!success && _generationTries < Constants.MaxGenerationTries);
            if (!success)
            {
                GeneratorAlgorithmToUse.Rows = 1;
                GeneratorAlgorithmToUse.Columns = 1;
                GenerateOneBigRoom();
            }
        }

        private void GenerateWithOuterDummyRing()
        {
            // Set room parameters

            var forcedNormals = new List<(int, int)>();
            var forcedDummies = new List<(int, int)>();

            for (int row = 0; row < RoomCountRows; row++)
            {
                for (int column = 0; column < RoomCountColumns; column++)
                {
                    if (row != 0 && row != RoomCountRows - 1
                        && column != 0 && column != RoomCountColumns - 1)
                    {
                        forcedNormals.Add((row, column));
                    }
                    else
                    {
                        forcedDummies.Add((row, column));
                    }
                }
            }

            bool success;
            do
            {
                _generationTries++;
                ResetAndCreateTiles();

                CreateRoomsWithSpecificLayout(forcedNormals, forcedDummies);
                success = ConnectRoomsOrTurnIntoDefaultIfNotPossible();
            }
            while (!success && _generationTries < Constants.MaxGenerationTries);
            if (!success)
            {
                GeneratorAlgorithmToUse.Rows = 1;
                GeneratorAlgorithmToUse.Columns = 1;
                GenerateOneBigRoom();
            }
        }

        private void GenerateOneBigRoom()
        {
            ResetAndCreateTiles();

            CreateRoomsWithRandomizedConfig();
            ConnectRoomsOrTurnIntoDefaultIfNotPossible();
        }

        #endregion

        #region Floor room setup

        private void ResetAndCreateTiles()
        {
            CurrentEntityId = 1;
            TurnCount = 0;

            Tiles = new Tile[Height, Width];
            Parallel.For(0, Height, y =>
            {
                Parallel.For(0, Width, x =>
                {
                    var newTile = new Tile
                    {
                        Map = this,
                        Position = new Point(x, y),
                        Type = TileType.Empty,
                        Discovered = false,
                        Visible = false
                    };
                    Tiles[y, x] = newTile;
                });
            });
        }

        private void CreateRoomsWithRandomizedConfig()
        {
            var normalRoomsCount = 0;
            (int LastPossibleNormalRow, int LastPossibleNormalColumn) = (RoomCountRows - 1, RoomCountColumns - 1);
            GetPossibleRoomData();
            do
            {
                Rooms = new List<Room>();
                for (var row = 0; row < RoomCountRows; row++)
                {
                    for (var column = 0; column < RoomCountColumns; column++)
                    {
                        var rngRoom = Rng.NextInclusive(1, 100);
                        // Force generation of a normal room if it's the last possible location and no normal Room was created yet
                        if ((normalRoomsCount == 0 && row == LastPossibleNormalRow && column == LastPossibleNormalColumn) || !rngRoom.Between(71, 100))
                        {
                            var (MinX, MinY, MaxX, MaxY) = GetPossibleCoordinatesForRoom(row, column);
                            var rngX1 = Rng.NextInclusive(MinX, MaxX - MinRoomWidth);
                            var rngX2 = Rng.NextInclusive(rngX1 + MinRoomWidth, MaxX);
                            var roomWidth = rngX2 - rngX1 + 1;
                            var rngY1 = Rng.NextInclusive(MinY, MaxY - MinRoomHeight);
                            var rngY2 = Rng.NextInclusive(rngY1 + MinRoomHeight, MaxY);
                            var roomHeight = rngY2 - rngY1 + 1;
                            Rooms.Add(new Room(this, new Point(rngX1, rngY1), row, column, roomWidth, roomHeight));
                            normalRoomsCount++;
                        }
                        else if (rngRoom.Between(71, 85))
                        {
                            var (MinX, MinY, MaxX, MaxY) = GetPossibleCoordinatesForRoom(row, column);
                            var rngX = Rng.NextInclusive(MinX + 1, MaxX - 1);
                            var rngY = Rng.NextInclusive(MinY + 1, MaxY - 1);
                            Rooms.Add(new Room(this, new Point(rngX, rngY), row, column, 1, 1));
                        }
                    }
                }
            }
            while (!SetPossibleRoomConnections(false).IsFullyConnectedAdjacencyMatrix(r => r != RoomConnectionType.None));
        }

        private void CreateRoomsWithSpecificLayout(List<(int Row, int Column)> normalRooms, List<(int Row, int Column)> dummyRooms)
        {
            if (normalRooms.Intersect(dummyRooms).Any()) throw new InvalidDataException("At least one room is set as Normal and Dummy");
            var normalRoomsCount = 0;
            GetPossibleRoomData();
            Rooms = new List<Room>();
            for (var row = 0; row < RoomCountRows; row++)
            {
                for (var column = 0; column < RoomCountColumns; column++)
                {
                    if (normalRooms.Exists(nr => nr.Row == row && nr.Column == column))
                    {
                        var (MinX, MinY, MaxX, MaxY) = GetPossibleCoordinatesForRoom(row, column);
                        var rngX1 = Rng.NextInclusive(MinX, MaxX - MinRoomWidth);
                        var rngX2 = Rng.NextInclusive(rngX1 + MinRoomWidth, MaxX);
                        var roomWidth = rngX2 - rngX1 + 1;
                        var rngY1 = Rng.NextInclusive(MinY, MaxY - MinRoomHeight);
                        var rngY2 = Rng.NextInclusive(rngY1 + MinRoomHeight, MaxY);
                        var roomHeight = rngY2 - rngY1 + 1;
                        Rooms.Add(new Room(this, new Point(rngX1, rngY1), row, column, roomWidth, roomHeight));
                        normalRoomsCount++;
                    }
                    else if (dummyRooms.Exists(fd => fd.Row == row && fd.Column == column))
                    {
                        var (MinX, MinY, MaxX, MaxY) = GetPossibleCoordinatesForRoom(row, column);
                        var rngX = Rng.NextInclusive(MinX + 1, MaxX - 1);
                        var rngY = Rng.NextInclusive(MinY + 1, MaxY - 1);
                        Rooms.Add(new Room(this, new Point(rngX, rngY), row, column, 1, 1));
                    }
                }
            }
        }

        private bool ConnectRoomsOrTurnIntoDefaultIfNotPossible()
        {
            RoomAdjacencyMatrix = SetPossibleRoomConnections(true);
            var success = RoomAdjacencyMatrix.IsFullyConnectedAdjacencyMatrix(r => r != RoomConnectionType.None);
            if (success)
            {
                FuseRoomsIfNeeded();
                SetHallways();
                Parallel.ForEach(Rooms, r => r.CreateTiles());
                success = Tiles.IsFullyConnected(t => t.IsWalkable);
                if (success)
                {
                    PlacePlayer();
                    if (FloorConfigurationToUse.GenerateStairsOnStart)
                        SetStairs();
                    AppendMessage(Locale["FloorEnter"].Format(new { FloorLevel = FloorLevel.ToString() }), Color.Yellow);
                }
            }
            return success;
        }

        #endregion

        #region Entities

        public void AddEntity(Entity entity, bool isInInventory)
        {
            entity.Id = CurrentEntityId;
            entity.Map = this;
            if (!isInInventory)
                entity.Position = PickEmptyPosition(TurnCount == 0);
            if (entity is PlayerCharacter pc)
            {
                Dungeon.PlayerCharacter = pc;
            }
            else if (entity is NonPlayableCharacter npc)
            {
                AICharacters.Add(npc);
            }
            else if (entity is Item i)
            {
                if (i.EntityType != EntityType.Trap)
                    Items.Add(i);
                else
                    Traps.Add(i);
            }
            CurrentEntityId++;
        }

        public void AddEntity(string classId, int level = 1)
        {
            var entityClass = Dungeon.Classes.Find(c => c.Id.Equals(classId))
                ?? throw new InvalidDataException("Class does not exist!");
            Entity entity = null;
            switch (entityClass.EntityType)
            {
                case EntityType.Player:
                    entity = new PlayerCharacter(entityClass, level, this)
                    {
                        Id = CurrentEntityId,
                        Position = PickEmptyPosition(TurnCount == 0)
                    };
                    if (Dungeon.PlayerClass.RequiresNamePrompt && !string.IsNullOrWhiteSpace(Dungeon.PlayerName))
                        entity.Name = Dungeon.PlayerName;
                    break;
                case EntityType.NPC:
                    entity = new NonPlayableCharacter(entityClass, level, this)
                    {
                        Id = CurrentEntityId,
                        Position = PickEmptyPosition(TurnCount == 0)
                    };
                    break;
                case EntityType.Weapon:
                case EntityType.Armor:
                case EntityType.Consumable:
                case EntityType.Trap:
                    entity = new Item(entityClass, this)
                    {
                        Id = CurrentEntityId,
                        Position = PickEmptyPosition(TurnCount == 0)
                    };
                    break;
                default:
                    throw new InvalidDataException("Entity lacks a valid type!");
            }
            CurrentEntityId++;
            if (entity is Item i)
            {
                if (i.EntityType != EntityType.Trap)
                    Items.Add(i);
                else
                    Traps.Add(i);
            }
            if (entity is Character c)
            {
                var weaponEntityClass = Dungeon.Classes.Find(cl => cl.Id.Equals(c.StartingWeaponId))
                    ?? throw new InvalidDataException("Class does not have a valid starting weapon!");
                c.StartingWeapon = new Item(weaponEntityClass, this)
                {
                    Id = CurrentEntityId,
                    Owner = c
                };
                CurrentEntityId++;
                var armorEntityClass = Dungeon.Classes.Find(cl => cl.Id.Equals(c.StartingArmorId))
                    ?? throw new InvalidDataException("Class does not have a valid starting armor!");
                c.StartingArmor = new Item(armorEntityClass, this)
                {
                    Id = CurrentEntityId,
                    Owner = c
                };
                CurrentEntityId++;
                foreach (var itemId in entityClass.StartingInventoryIds)
                {
                    var itemEntityClass = Dungeon.Classes.Find(cl => cl.Id.Equals(itemId))
                        ?? throw new InvalidDataException("Class does has an invalid starting inventory item!");
                    var inventoryItem = new Item(itemEntityClass, this)
                    {
                        Id = CurrentEntityId,
                        Owner = c
                    };
                    Items.Add(inventoryItem);
                    c.Inventory.Add(inventoryItem);
                    CurrentEntityId++;
                }
                if (entity is PlayerCharacter p)
                    Dungeon.PlayerCharacter = p;
                else if (entity is NonPlayableCharacter npc)
                {
                    AICharacters.Add(npc);
                    if(npc.OnSpawn?.ChecksCondition(npc, npc) == true)
                        npc.OnSpawn?.Do(npc, npc, false);
                }
            }
        }

        private void PlacePlayer()
        {
            if (Player == null)
            {
                AddEntity(Dungeon.PlayerClass.Id);
            }
            else
            {
                Player.Map = this;
                Player.Id = CurrentEntityId;
                CurrentEntityId++;
                if (Player.EquippedWeapon?.ClassId.Equals(Dungeon.PlayerClass.StartingWeaponId) == false)
                {
                    Player.EquippedWeapon.Id = CurrentEntityId;
                    Player.EquippedWeapon.Map = this;
                    AddEntity(Player.EquippedWeapon, true);
                    CurrentEntityId++;
                }
                if (Player.EquippedArmor?.ClassId.Equals(Dungeon.PlayerClass.StartingArmorId) == false)
                {
                    Player.EquippedArmor.Id = CurrentEntityId;
                    Player.EquippedArmor.Map = this;
                    AddEntity(Player.EquippedArmor, true);
                    CurrentEntityId++;
                }
                Player.Inventory?.ForEach(i =>
                {
                    i.Id = CurrentEntityId;
                    i.Map = this;
                    AddEntity(i, true);
                    CurrentEntityId++;
                });
            }
            Player.Position = PickEmptyPosition(false);
            Player.UpdateVisibility();
        }

        private void NewTurn()
        {
            if (TurnCount == 0)
            {
                CreateFlag("TurnCount", TurnCount, false);
                int generationAttempts, generationsToTry;
                #region Generate Monsters
                if(FloorConfigurationToUse.PossibleMonsters.Any())
                {
                    var totalMonsterGeneratorChance = FloorConfigurationToUse.PossibleMonsters.Sum(mg => mg.ChanceToPick);
                    if (!totalMonsterGeneratorChance.Between(1, 100)) throw new InvalidDataException("Monster generation odds are not 1-100%");
                    List<ClassInFloor> usableMonsterGenerators = new();
                    FloorConfigurationToUse.PossibleMonsters.ForEach(pm =>
                    {
                        if (!pm.CanSpawnOnFirstTurn || (pm.OverallMaxForKindInFloor > 0 && pm.TotalGeneratedInFloor >= pm.OverallMaxForKindInFloor)) return;
                        var currentMonstersWithId = AICharacters.Where(e => e.ClassId.Equals(pm.Class.Id) && e.ExistenceStatus == EntityExistenceStatus.Alive);
                        if (currentMonstersWithId.Count() >= pm.SimultaneousMaxForKindInFloor) return;
                        usableMonsterGenerators.Add(pm);
                    });
                    totalMonsterGeneratorChance = usableMonsterGenerators.Sum(mg => mg.ChanceToPick);
                    generationAttempts = 0;
                    generationsToTry = Rng.NextInclusive(FloorConfigurationToUse.SimultaneousMinMonstersAtStart, FloorConfigurationToUse.SimultaneousMaxMonstersInFloor);
                    while (TotalMonstersInFloor < FloorConfigurationToUse.SimultaneousMaxMonstersInFloor && totalMonsterGeneratorChance > 0 && generationAttempts < generationsToTry)
                    {
                        AddEnemy(usableMonsterGenerators, totalMonsterGeneratorChance);
                        usableMonsterGenerators.RemoveAll(mg => (mg.SimultaneousMaxForKindInFloor > 0 && AICharacters.Count(e => e.ClassId.Equals(mg.Class.Id) && e.ExistenceStatus == EntityExistenceStatus.Alive) >= mg.SimultaneousMaxForKindInFloor) || (mg.OverallMaxForKindInFloor > 0 && mg.TotalGeneratedInFloor >= mg.OverallMaxForKindInFloor));
                        totalMonsterGeneratorChance = usableMonsterGenerators.Sum(mg => mg.ChanceToPick);
                        generationAttempts++;
                    }
                    LastMonsterGenerationTurn = 0;
                }

                #endregion

                #region Generate Items

                if(FloorConfigurationToUse.PossibleItems.Any())
                {
                    var totalItemGeneratorChance = FloorConfigurationToUse.PossibleItems.Sum(ig => ig.ChanceToPick);
                    if (!totalItemGeneratorChance.Between(1, 100)) throw new InvalidDataException("Item generation odds are not 1-100%");
                    List<ClassInFloor> usableItemGenerators = new();
                    FloorConfigurationToUse.PossibleItems.ForEach(pi =>
                    {
                        if (pi.SimultaneousMaxForKindInFloor > 0 && pi.TotalGeneratedInFloor >= pi.SimultaneousMaxForKindInFloor) return;
                        usableItemGenerators.Add(pi);
                    });
                    totalItemGeneratorChance = usableItemGenerators.Sum(ig => ig.ChanceToPick);
                    generationAttempts = 0;
                    generationsToTry = Rng.NextInclusive(FloorConfigurationToUse.MinItemsInFloor, FloorConfigurationToUse.MaxItemsInFloor);
                    while (TotalItemsInFloor < FloorConfigurationToUse.MaxItemsInFloor && totalItemGeneratorChance > 0 && generationAttempts < generationsToTry)
                    {
                        AddItem(usableItemGenerators, totalItemGeneratorChance);
                        usableItemGenerators.RemoveAll(ig => ig.SimultaneousMaxForKindInFloor > 0 && ig.TotalGeneratedInFloor >= ig.SimultaneousMaxForKindInFloor);
                        totalItemGeneratorChance = usableItemGenerators.Sum(ig => ig.ChanceToPick);
                        generationAttempts++;
                    }
                }

                #endregion

                #region Generate Traps

                if(FloorConfigurationToUse.PossibleTraps.Any())
                {
                    var totalTrapGeneratorChance = FloorConfigurationToUse.PossibleTraps.Sum(tg => tg.ChanceToPick);
                    if (!totalTrapGeneratorChance.Between(1, 100)) throw new InvalidDataException("Trap generation odds are not 1-100%");
                    List<ClassInFloor> usableTrapGenerators = new();
                    FloorConfigurationToUse.PossibleTraps.ForEach(pt =>
                    {
                        if (pt.SimultaneousMaxForKindInFloor > 0 && pt.TotalGeneratedInFloor >= pt.SimultaneousMaxForKindInFloor) return;
                        usableTrapGenerators.Add(pt);
                    });
                    totalTrapGeneratorChance = usableTrapGenerators.Sum(tg => tg.ChanceToPick);
                    generationAttempts = 0;
                    generationsToTry = Rng.NextInclusive(FloorConfigurationToUse.MinTrapsInFloor, FloorConfigurationToUse.MaxTrapsInFloor);
                    while (TotalTrapsInFloor < FloorConfigurationToUse.MaxTrapsInFloor && totalTrapGeneratorChance > 0 && generationAttempts < generationsToTry)
                    {
                        AddItem(usableTrapGenerators, totalTrapGeneratorChance);
                        usableTrapGenerators.RemoveAll(tg => tg.SimultaneousMaxForKindInFloor > 0 && tg.TotalGeneratedInFloor >= tg.SimultaneousMaxForKindInFloor);
                        totalTrapGeneratorChance = usableTrapGenerators.Sum(tg => tg.ChanceToPick);
                        generationAttempts++;
                    }
                }

                #endregion

                #region Perform On Floor Start Actions

                AddMessageBox(Dungeon.Name, Locale["FloorEnter"].Format(new { FloorLevel = FloorLevel.ToString() }), "OK", new GameColor(Color.Yellow));
                FloorConfigurationToUse.OnFloorStart?.Do(Player, Player, false);

                #endregion
            }
            else if (TurnCount - LastMonsterGenerationTurn >= FloorConfigurationToUse.TurnsPerMonsterGeneration)
            {
                #region Generate A monster

                var currentMonsters = AICharacters.Where(e => e.EntityType == EntityType.NPC && e.ExistenceStatus == EntityExistenceStatus.Alive);
                if (currentMonsters.Count() < FloorConfigurationToUse.SimultaneousMaxMonstersInFloor)
                {
                    AddEnemy();
                }

                #endregion
            }
            TurnCount++;
            SetFlagValue("TurnCount", TurnCount);
            _displayedTurnMessage = false;
            Player.RemainingMovement = Player.Movement;
            LatestPlayerRemainingMovement = Player.RemainingMovement;
            Player.TookAction = false;
            Player.PerformOnTurnStartActions();
            AICharacters.Where(e => e != null).ForEach(e =>
            {
                e.RemainingMovement = e.Movement;
                e.TookAction = false;
                e.PerformOnTurnStartActions();
            });
        }

        private void AddEnemy()
        {
            if (!FloorConfigurationToUse.PossibleMonsters.Any() || TotalMonstersInFloor >= FloorConfigurationToUse.SimultaneousMaxMonstersInFloor) return;
            List<ClassInFloor> usableMonsterGenerators = new();
            FloorConfigurationToUse.PossibleMonsters.ForEach(pm =>
            {
                if (!pm.CanSpawnAfterFirstTurn || (pm.OverallMaxForKindInFloor > 0 && pm.TotalGeneratedInFloor >= pm.OverallMaxForKindInFloor)) return;
                var currentMonstersWithId = AICharacters.Where(e => e.ClassId.Equals(pm.Class.Id) && e.ExistenceStatus == EntityExistenceStatus.Alive);
                if (currentMonstersWithId.Count() >= pm.SimultaneousMaxForKindInFloor) return;
                usableMonsterGenerators.Add(pm);
            });
            AddEnemy(usableMonsterGenerators, 100);
        }

        private void AddEnemy(List<ClassInFloor> usableMonsterGenerators, int odds)
        {
            if (TotalMonstersInFloor >= FloorConfigurationToUse.SimultaneousMaxMonstersInFloor) return;
            var pickedGenerator = usableMonsterGenerators.GetWithProbability(mg => mg.ChanceToPick, Rng, odds);
            if (pickedGenerator != null)
            {
                var level = Rng.NextInclusive(pickedGenerator.MinLevel, pickedGenerator.MaxLevel);
                AddEntity(pickedGenerator.Class.Id, level);
                pickedGenerator.TotalGeneratedInFloor++;
                LastMonsterGenerationTurn = TurnCount;
            }
        }

        private void AddItem(List<ClassInFloor> usableItemGenerators, int odds)
        {
            var pickedGenerator = usableItemGenerators.GetWithProbability(ig => ig.ChanceToPick, Rng, odds);
            if (pickedGenerator != null)
            {
                AddEntity(pickedGenerator.Class.Id);
                pickedGenerator.TotalGeneratedInFloor++;
            }
        }

        private void ProcessTurn()
        {
            if (LatestPlayerRemainingMovement == Player.RemainingMovement && Player.CanTakeAction && !Player.TookAction) return;
            Player.UpdateVisibility();
            var minRequiredMovementToAct = (Player.RemainingMovement == 0 || !Player.CanTakeAction || Player.TookAction) ? 0 : LatestPlayerRemainingMovement;
            var aiCharactersThatCanActAlongsidePlayer = AICharacters.Where(c => (c.RemainingMovement > 0 || c.Movement == 0) && c.CanTakeAction && !c.TookAction && c.RemainingMovement >= minRequiredMovementToAct).OrderByDescending(c => c.RemainingMovement).ToList();
            while (aiCharactersThatCanActAlongsidePlayer.Any())
            {
                Parallel.ForEach(aiCharactersThatCanActAlongsidePlayer, aictca => aictca.PickTargetAndPath());
                aiCharactersThatCanActAlongsidePlayer.ForEach(aictca => aictca.AttackOrMove());
                aiCharactersThatCanActAlongsidePlayer = AICharacters.Where(c => (c.RemainingMovement > 0 || c.Movement == 0) && c.CanTakeAction && !c.TookAction && c.RemainingMovement >= minRequiredMovementToAct).OrderByDescending(c => c.RemainingMovement).ToList();
            }
            if(GetCharacters().TrueForAll(c => (c.RemainingMovement == 0 && c.Movement > 0) || !c.CanTakeAction || c.TookAction))
                NewTurn();
        }

        public PlayerInfoDto GetPlayerDetailInfo()
        {
            return new PlayerInfoDto(Player, this);
        }

        public void PlayerMove(int x, int y)
        {
            if (DungeonStatus != DungeonStatus.Running) return;
            if (Player.ExistenceStatus != EntityExistenceStatus.Alive) return;
            if (x == 0 && y == 0) // This is only possible if the player chooses to Skip Turn.
            {
                Player.RemainingMovement = 0;
                Player.TookAction = true;
            }
            else if (Player.RemainingMovement != 0)
            {
                var currentTile = GetTileFromCoordinates(Player.Position)
                    ?? throw new ArgumentException("PlayerEntity is on a nonexistent Tile");
                var targetTile = GetTileFromCoordinates(Player.Position.X + x, Player.Position.Y + y)
                    ?? throw new ArgumentException("PlayerEntity is about to move to a nonexistent Tile");
                if (Tiles.GetAdjacentElements(currentTile, true).Contains(targetTile))
                {
                    if (!targetTile.IsWalkable) return;
                    if (x != 0 && y != 0 && (!GetTileFromCoordinates(Player.Position.X + x, Player.Position.Y).IsWalkable
                            || !GetTileFromCoordinates(Player.Position.X, Player.Position.Y + y).IsWalkable))
                    {
                        return;
                    }
                    TryMoveCharacter(Player, targetTile);
                }
            }
            ProcessTurn();
        }

        public bool TryMoveCharacter(Character character, Tile targetTile)
        {
            var characterInTargetTile = GetCharacters().Find(c => c.ContainingTile == targetTile && c != character && c.ExistenceStatus == EntityExistenceStatus.Alive);
            if (characterInTargetTile != null)
            {
                if (((characterInTargetTile.Visible && character == Player && !characterInTargetTile.Faction.EnemiesWith.Contains(character.Faction)) || !characterInTargetTile.Visible) && characterInTargetTile.CanTakeAction && characterInTargetTile.Movement > 0)
                {
                    // Swap positions with allies, neutrals or invisibles
                    characterInTargetTile.Position = character.Position;
                    if(characterInTargetTile.RemainingMovement > 0)
                        characterInTargetTile.RemainingMovement--;
                    if (character == Player && !characterInTargetTile.Faction.EnemiesWith.Contains(character.Faction))
                        AppendMessage(Locale["CharacterSwitchedPlacesWithPlayer"].Format(new { CharacterName = characterInTargetTile.Name, PlayerName = Player.Name }));
                }
                else
                {
                    return false;
                }
            }
            if (character is NonPlayableCharacter npc)
                npc.LastPosition = character.Position;

            character.Position = targetTile.Position;

            targetTile.GetItems().ForEach(i => character.TryToPickItem(i));
            targetTile.Trap?.Stepped(character);

            character.RemainingMovement--;

            return true;
        }

        public void PlayerUseStairs()
        {
            var stairsTile = Tiles.Find(t => t.Type == TileType.Stairs);
            if (Player.ContainingTile != stairsTile)
                throw new ArgumentException($"Player is trying to use non-existent stairs at ({Player.ContainingTile.Position.X}, {Player.ContainingTile.Position.Y})");
            AppendMessage(Locale["FloorLeave"].Format(new { TurnCount = TurnCount.ToString() }), Color.Yellow);
            Dungeon.TakeStairs();
        }

        private void PlayerUseItem(Item item)
        {
            item.Used(Player);
            if(item.OnUse?.FinishesTurnWhenUsed == true)
                Player.TookAction = true;
            Player.RemainingMovement = 0;
            ProcessTurn();
        }

        public void PlayerUseItemInFloor()
        {
            if (GetEntitiesFromCoordinates(Player.Position.X, Player.Position.Y)
                .Find(e => (e.EntityType == EntityType.Consumable || e.EntityType == EntityType.Weapon || e.EntityType == EntityType.Armor)
                    && e.ExistenceStatus == EntityExistenceStatus.Alive && e.Passable) is not Item usableItem)
            {
                return;
            }

            PlayerUseItem(usableItem);
        }
        public void PlayerPickUpItemInFloor()
        {
            var itemThatCanBePickedUp = GetEntitiesFromCoordinates(Player.Position.X, Player.Position.Y)
                .ConvertAll(e => e as Item)
                .Find(e => e != null && e.ExistenceStatus == EntityExistenceStatus.Alive && e.CanBePickedUp);
            if (itemThatCanBePickedUp == null)
                return;
            if (Player.Inventory.Count == Player.InventorySize)
            {
                AppendMessage(Locale["InventoryIsFull"].Format(new { CharacterName = Player.Name, ItemName = itemThatCanBePickedUp.Name }));
            }
            else
            {
                Player.PickItem(itemThatCanBePickedUp);
                Player.TookAction = true;
                Player.RemainingMovement = 0;
                ProcessTurn();
            }
        }
        public void PlayerUseItemFromInventory(int itemId)
        {
            var item = Items.Find(i => i.Id == itemId)
                ?? throw new ArgumentException("Player attempted to use an item that does not exist!");
            PlayerUseItem(item);
        }

        public void PlayerDropItemFromInventory(int itemId)
        {
            var itemThatCanBeDropped = Items.Find(i => i.Id == itemId)
                ?? throw new ArgumentException("Player attempted to use an item that does not exist!");
            var entitiesInTile = GetEntitiesFromCoordinates(Player.Position);
            if (entitiesInTile.Exists(e => e.Passable && e.ExistenceStatus != EntityExistenceStatus.Gone))
            {
                AppendMessage(Locale["TilesIsOccupied"].Format(new { CharacterName = Player.Name, ItemName = itemThatCanBeDropped.Name }));
            }
            else
            {
                Player.DropItem(itemThatCanBeDropped);
                Player.TookAction = true;
                Player.RemainingMovement = 0;
                ProcessTurn();
            }
        }

        public void PlayerSwapFloorItemWithInventoryItem(int itemId)
        {
            var itemInInventory = Items.Find(i => i.Id == itemId)
                ?? throw new ArgumentException("Player attempted to use an item that does not exist!");
            var itemInTile = Items.Find(i => i.Position?.Equals(Player.Position) == true && i.ExistenceStatus != EntityExistenceStatus.Gone);
            if (itemInTile != null)
            {
                Player.DropItem(itemInInventory);
                Player.TookAction = true;
                Player.TryToPickItem(itemInTile);
                Player.RemainingMovement = 0;
                ProcessTurn();
            }
            else
            {
                throw new InvalidOperationException("Player attempted to pick an item from a tile without item!");
            }
        }
        public InventoryDto GetPlayerInventory()
        {
            var inventory = new InventoryDto();
            var itemsOnTile = Items.Where(i => i.Position?.Equals(Player.Position) == true).ToList();
            inventory.TileIsOccupied = itemsOnTile.Any();
            if (Player.EquippedWeapon != null)
                inventory.InventoryItems.Add(new InventoryItemDto(Player.EquippedWeapon, Player, this));
            if (Player.EquippedArmor != null)
                inventory.InventoryItems.Add(new InventoryItemDto(Player.EquippedArmor, Player, this));
            for (int i = 0; i < Player.Inventory.Count; i++)
            {
                inventory.InventoryItems.Add(new InventoryItemDto(Player.Inventory[i], Player, this));
            }
            for (int i = 0; i < itemsOnTile.Count; i++)
            {
                inventory.InventoryItems.Add(new InventoryItemDto(itemsOnTile[i], Player, this));
            }
            return inventory;
        }

        public void PlayerAttackTargetWith(string selectionId, int x, int y)
        {
            var characterInTile = GetTileFromCoordinates(x, y).Character as NonPlayableCharacter
                ?? throw new ArgumentException("Player attempted use an action without a valid target.");

            var selectionIdParts = selectionId.Split('_');

            if (selectionIdParts.Length == 2)
            {
                var entity = selectionIdParts[0]; // "Player" or "Target"
                var actionIdAsString = selectionIdParts[1]; // ActionId

                if(!int.TryParse(actionIdAsString, out int actionId))
                    throw new ArgumentException("Action SelectionId is not a valid number.");

                if (entity.Equals("Player"))
                {
                    var selectedAction = Player.OnAttack.Find(oaa => oaa.ActionId == actionId)
                        ?? throw new ArgumentException("Player attempted use a non-existent Attack action.");
                    Player.AttackCharacter(characterInTile, selectedAction);
                }
                else if (entity.Equals("Target"))
                {
                    var selectedAction = characterInTile.OnInteracted.Find(oia => oia.ActionId == actionId)
                        ?? throw new ArgumentException("Player attempted use a non-existent Interact action from the Target.");
                    Player.InteractWithCharacter(characterInTile, selectedAction);
                }
                else
                {
                    throw new ArgumentException("Invalid entity in SelectionId.");
                }
            }
            else
            {
                throw new ArgumentException("Invalid SelectionId format.");
            }

            ProcessTurn();
        }

        public ActionListDto GetPlayerAttackActions(int x, int y)
        {
            var characterInTile = GetTileFromCoordinates(x, y).Character;
            var actionList = new ActionListDto((characterInTile != null) ? characterInTile.Name : "<?????>");

            Player.OnAttack.ForEach(oaa => actionList.AddAction(oaa, Player, characterInTile, this, true));

            if(characterInTile is NonPlayableCharacter npc)
                npc.OnInteracted?.ForEach(oia => actionList.AddAction(oia, Player, characterInTile, this, false));

            if(actionList.Actions.DistinctBy(a => a.SelectionId).Count() != actionList.Actions.Count)
                throw new ArgumentException("Duplicate Actions discovered in selection.");

            return actionList;
        }

        public EntityDetailDto GetDetailsOfEntity(int x, int y)
        {
            var tile = GetTileFromCoordinates(x, y);
            var characterInTile = tile.Character;
            if (characterInTile?.Visible == true)
                return new EntityDetailDto(characterInTile);
            var itemInTile = tile.GetItems().FirstOrDefault();
            if(itemInTile?.Visible == true)
                return new EntityDetailDto(itemInTile);
            var trapInTile = tile.Trap;
            if (trapInTile?.Visible == true)
                return new EntityDetailDto(trapInTile);
            return null;
        }

        private void SetStairs(Point p)
        {
            var tile = GetTileFromCoordinates(p.X, p.Y);
            tile.Type = TileType.Stairs;
            StairsAreSet = true;
        }

        public void SetStairs()
        {
            SetStairs(PickEmptyPosition(true));
        }

        public Point PickEmptyPosition(bool allowPlayerRoom)
        {
            int rngX, rngY;
            var nonDummyRooms = Rooms.Where(r => r.Width > 1 && r.Height > 1).Distinct().ToList();
            Tile tileToCheck;
            bool hasLaterallyAdjacentHallways;
            var playerRoom = Player?.Position != null ? GetRoomInCoordinates(Player.Position.X, Player.Position.Y) : null;
            if(nonDummyRooms.Count > 1 && playerRoom != null && !allowPlayerRoom)
                nonDummyRooms.Remove(playerRoom);
            do
            {
                var possibleNonDummyRoom = nonDummyRooms.TakeRandomElement(Rng);
                rngX = Rng.NextInclusive(possibleNonDummyRoom.Position.X + 1, possibleNonDummyRoom.Position.X + possibleNonDummyRoom.Width - 2);
                rngY = Rng.NextInclusive(possibleNonDummyRoom.Position.Y + 1, possibleNonDummyRoom.Position.Y + possibleNonDummyRoom.Height - 2);
                tileToCheck = GetTileFromCoordinates(rngX, rngY);
                hasLaterallyAdjacentHallways = GetTileFromCoordinates(rngX - 1, rngY)?.Type == TileType.Hallway
                                            || GetTileFromCoordinates(rngX + 1, rngY)?.Type == TileType.Hallway
                                            || GetTileFromCoordinates(rngX, rngY - 1)?.Type == TileType.Hallway
                                            || GetTileFromCoordinates(rngX, rngY + 1)?.Type == TileType.Hallway;
            }
            while (!tileToCheck.IsWalkable || tileToCheck.Type == TileType.Stairs || tileToCheck.Character != null || tileToCheck.GetItems().Any() || tileToCheck.Trap != null || hasLaterallyAdjacentHallways);
            return new Point(rngX, rngY);
        }

        #endregion

        #region Finding things in coordinates

        private void GetPossibleRoomData()
        {
            MaxRoomWidth = Width / RoomCountColumns;
            if (MaxRoomWidth < Constants.MinRoomWidthOrHeight)
                throw new InvalidDataException("Combination of floor Width and Columns prevents generating 5x5 rooms (the bare minimum)");
            MinRoomWidth = Math.Max(MaxRoomWidth / 4, Constants.MinRoomWidthOrHeight);
            MaxRoomHeight = Height / RoomCountRows;
            if (MaxRoomHeight < Constants.MinRoomWidthOrHeight)
                throw new InvalidDataException("Combination of floor Height and Rows prevents generating 5x5 rooms (the bare minimum)");
            MinRoomHeight = Math.Max(MaxRoomHeight / 4, Constants.MinRoomWidthOrHeight);
            var widthGap = (Width - (MaxRoomWidth * RoomCountColumns)) / 2;
            var heightGap = (Height - (MaxRoomHeight * RoomCountRows)) / 2;
            RoomLimitsTable = new (Point topLeftCorner, Point bottomRightCorner)[RoomCountRows, RoomCountColumns];
            var minX = widthGap;
            var maxX = Width - widthGap;
            var minY = heightGap;
            var maxY = Height - heightGap;
            for (int i = 0; i < RoomCountRows; i++)
            {
                for (int j = 0; j < RoomCountColumns; j++)
                {
                    var topLeftCorner = new Point
                    {
                        X = minX + (MaxRoomWidth * j),
                        Y = minY + (MaxRoomHeight * i)
                    };
                    var bottomRightCorner = new Point
                    {
                        X = Math.Min(maxX, topLeftCorner.X + MaxRoomWidth - 1),
                        Y = Math.Min(maxY, topLeftCorner.Y + MaxRoomHeight - 1)
                    };
                    RoomLimitsTable[i, j] = (topLeftCorner, bottomRightCorner);
                }
            }
        }

        private (int MinX, int MinY, int MaxX, int MaxY) GetPossibleCoordinatesForRoom(int row, int column)
        {
            var (TopLeftCorner, BottomRightCorner) = RoomLimitsTable[row, column];
            return (TopLeftCorner.X, TopLeftCorner.Y, BottomRightCorner.X, BottomRightCorner.Y);
        }

        public Tile GetTileFromCoordinates(Point position)
        {
            return GetTileFromCoordinates(position.X, position.Y);
        }

        public Tile GetTileFromCoordinates(int x, int y)
        {
            try
            {
                return Tiles[y, x];
            }
            catch { return null; }
        }

        public List<Entity> GetEntitiesFromCoordinates(Point point)
        {
            return GetEntitiesFromCoordinates(point.X, point.Y);
        }

        public List<Entity> GetEntities()
        {
            return GetCharacters().Cast<Entity>().Union(Items.Cast<Entity>()).Union(Traps.Cast<Entity>()).Where(e => e != null).ToList();
        }
        public List<Character> GetCharacters()
        {
            return AICharacters.Cast<Character>().Append(Player).ToList();
        }

        private List<Entity> GetEntitiesFromCoordinates(int x, int y)
        {
            return GetEntities().Where(t => t?.Position != null && t.Position.X == x && t.Position.Y == y).ToList();
        }

        public Room GetRoomInCoordinates(int x, int y)
        {
            return Rooms.Find(r => x.Between(r.Position.X, r.Position.X + r.Width - 1) && y.Between(r.Position.Y, r.Position.Y + r.Height - 1));
        }

        public List<Tile> GetTilesInRoom(Room room)
        {
            return Tiles.Where(t => t.Position.X.Between(room.Position.X, room.Position.X + room.Width - 1)
                                                 && t.Position.Y.Between(room.Position.Y, room.Position.Y + room.Height - 1)).ToList();
        }

        public ConsoleRepresentation GetConsoleRepresentationForCoordinates(int x, int y)
        {
            var tile = GetTileFromCoordinates(x, y) ?? throw new ArgumentException("Tile does not exist");
            if (!tile.Discovered)
                return ConsoleRepresentation.EmptyTile;
            var tileBaseConsoleRepresentation = new ConsoleRepresentation
            {
                ForegroundColor = tile.ConsoleRepresentation.ForegroundColor.Clone(),
                BackgroundColor = tile.ConsoleRepresentation.BackgroundColor.Clone(),
                Character = tile.ConsoleRepresentation.Character
            };
            if (tile.Discovered && !tile.Visible)
            {
                tileBaseConsoleRepresentation.BackgroundColor.A /= 2;
                tileBaseConsoleRepresentation.ForegroundColor.A /= 2;
                return tileBaseConsoleRepresentation;
            }
            if (tile.Character != null)
            {
                if(tile.Character.ExistenceStatus != EntityExistenceStatus.Alive && tile.Type == TileType.Stairs)
                        return tileBaseConsoleRepresentation;

                var characterBaseConsoleRepresentation = new ConsoleRepresentation
                {
                    ForegroundColor = tile.Character.ConsoleRepresentation.ForegroundColor.Clone(),
                    BackgroundColor = tile.Character.ConsoleRepresentation.BackgroundColor.Clone(),
                    Character = tile.Character.ConsoleRepresentation.Character
                };
                if ((tile.Character == Player || tile.Character.Faction.AlliedWith.Contains(Player.Faction)) && !tile.Character.Visible)
                {
                    characterBaseConsoleRepresentation.BackgroundColor.A /= 2;
                    characterBaseConsoleRepresentation.ForegroundColor.A /= 2;
                    return characterBaseConsoleRepresentation;
                }
                else if (tile.Character.Visible)
                {
                    return characterBaseConsoleRepresentation;
                }
            }
            if (tile.Type == TileType.Stairs)
                return tileBaseConsoleRepresentation;
            if (tile.GetItems().Exists(i => i.Visible))
                return tile.GetItems().Find(i => i.Visible).ConsoleRepresentation;
            if (tile.Trap?.Visible == true)
                return tile.Trap.ConsoleRepresentation;
            var deadEntityInCoordinates = GetEntitiesFromCoordinates(tile.Position).Find(e => e.Passable && e.ExistenceStatus == EntityExistenceStatus.Dead);
            if (deadEntityInCoordinates != null)
                return deadEntityInCoordinates.ConsoleRepresentation;
            return tileBaseConsoleRepresentation;
        }

        #endregion

        #region Room Connections
        private RoomConnectionType[,] SetPossibleRoomConnections(bool removeRandomEdges)
        {
            int normalRoomCount = 0, connectionCount = 0, minimumConnections;
            var temporaryRoomConnectionMatrix = new RoomConnectionType[Rooms.Count, Rooms.Count];

            normalRoomCount = Rooms.Count(r => r.Width > 1 && r.Height > 1);
            minimumConnections = normalRoomCount - 1;

            // Create default graph of rooms where all adjacencies are connected

            var roomsAsList = Rooms.ToList();

            for (int i = 0; i < roomsAsList.Count; i++)
            {
                temporaryRoomConnectionMatrix[i, i] = RoomConnectionType.None;
                if (roomsAsList[i] == null) continue;
                for (int j = 0; j < roomsAsList.Count; j++)
                {
                    if (i == j) continue;
                    if (roomsAsList[j] == null) continue;
                    if (roomsAsList[i].RoomRow == roomsAsList[j].RoomRow && Math.Abs(roomsAsList[i].RoomColumn - roomsAsList[j].RoomColumn) == 1)
                        temporaryRoomConnectionMatrix[i, j] = temporaryRoomConnectionMatrix[j, i] = RoomConnectionType.Horizontal;
                    if (roomsAsList[i].RoomColumn == roomsAsList[j].RoomColumn && Math.Abs(roomsAsList[i].RoomRow - roomsAsList[j].RoomRow) == 1)
                        temporaryRoomConnectionMatrix[i, j] = temporaryRoomConnectionMatrix[j, i] = RoomConnectionType.Vertical;
                }
            }

            if (removeRandomEdges)
            {
                var testMatrix = new RoomConnectionType[Rooms.Count, Rooms.Count];
                for (int y = 0; y < temporaryRoomConnectionMatrix.GetLength(1); y++)
                {
                    for (int x = 0; x < temporaryRoomConnectionMatrix.GetLength(0); x++)
                    {
                        testMatrix[x, y] = temporaryRoomConnectionMatrix[x, y];
                    }
                }

                // Remove a random amount of connections until all are evaluated (or the minimum amount of connections is obtained)

                for (int x = 0; x < temporaryRoomConnectionMatrix.GetLength(0); x++)
                {
                    for (int y = 0; y < temporaryRoomConnectionMatrix.GetLength(1); y++)
                    {
                        if (x == y) continue;
                        if (testMatrix[x, y] != RoomConnectionType.None && Rng.NextInclusive(1, 100).Between(1, 30))
                        {
                            testMatrix[x, y] = RoomConnectionType.None;
                            if (testMatrix.IsFullyConnectedAdjacencyMatrix(r => r != RoomConnectionType.None))
                            {
                                for (int i = 0; i < testMatrix.GetLength(0); i++)
                                {
                                    for (int j = 0; j < testMatrix.GetLength(1); j++)
                                    {
                                        temporaryRoomConnectionMatrix[i, j] = testMatrix[i, j];
                                    }
                                }
                                connectionCount--;
                            }
                        }
                        if (connectionCount <= minimumConnections) break;
                    }
                    if (connectionCount <= minimumConnections) break;
                }
            }

            return temporaryRoomConnectionMatrix;
        }

        private void FuseRoomsIfNeeded()
        {
            if (Rooms.Count(r => r.Width >= 1 && r.Height >= 1) < 2) return;
            for (var row = 0; row < RoomCountRows; row++)
            {
                for (var column = 0; column < RoomCountColumns; column++)
                {
                    var thisRoom = Rooms.Find(r => r.RoomRow == row && r.RoomColumn == column);
                    if (thisRoom == null || thisRoom.Width <= 1 || thisRoom.Height <= 1) continue;

                    if (Rng.NextInclusive(1, 100) > RoomFusionOdds) continue;

                    var adjacentRooms = GetConnectionsForRoom(thisRoom).Select(ar => ar.Room);

                    var roomsToFuseWithThis = new List<Room>();

                    Room? fusedRoom = null;

                    foreach (var adjacentRoom in adjacentRooms)
                    {
                        if (adjacentRoom.Width <= 1 || adjacentRoom.Height <= 1) continue;

                        // Prevent non-square room fusions
                        if (thisRoom.Width > MaxRoomWidth && thisRoom.RoomRow != adjacentRoom.RoomRow) continue;
                        if (adjacentRoom.Width > MaxRoomWidth && thisRoom.RoomRow != adjacentRoom.RoomRow) continue;
                        if (thisRoom.Height > MaxRoomHeight && thisRoom.RoomColumn != adjacentRoom.RoomColumn) continue;
                        if (adjacentRoom.Height > MaxRoomHeight && thisRoom.RoomColumn != adjacentRoom.RoomColumn) continue;

                        var minX = Math.Min(thisRoom.Position.X, adjacentRoom.Position.X);
                        var maxX = Math.Max(thisRoom.Position.X + thisRoom.Width, adjacentRoom.Position.X + adjacentRoom.Width);
                        var minY = Math.Min(thisRoom.Position.Y, adjacentRoom.Position.Y);
                        var maxY = Math.Max(thisRoom.Position.Y + thisRoom.Height, adjacentRoom.Position.Y + adjacentRoom.Height);
                        var width = Math.Max(maxX - minX, MinRoomWidth);
                        var height = Math.Max(maxY - minY, MinRoomHeight);
                        fusedRoom = new Room(this, new Point(minX, minY), thisRoom.RoomRow, thisRoom.RoomColumn, width, height);
                        roomsToFuseWithThis.AddRange(Rooms.Where(r => r.RoomRow == adjacentRoom.RoomRow && r.RoomColumn == adjacentRoom.RoomColumn));
                        break;
                    }

                    if (fusedRoom != null)
                    {
                        for (int i = 0; i < Rooms.Count; i++)
                        {
                            if (roomsToFuseWithThis.Contains(Rooms[i]) || thisRoom == Rooms[i])
                                Rooms[i] = fusedRoom;
                        }
                    }
                }
            }
        }

        #endregion

        #region Graph methods

        private List<(Room Room, RoomConnectionType ConnectionType)> GetConnectionsForRoom(Room room)
        {
            var adjacentRooms = new List<(Room Room, RoomConnectionType ConnectionType)>();
            var indexOfRoom = Rooms.IndexOf(room);

            for (int i = 0; i < Rooms.Count; i++)
            {
                if (RoomAdjacencyMatrix[indexOfRoom, i] != RoomConnectionType.None)
                    adjacentRooms.Add((Rooms[i], RoomAdjacencyMatrix[indexOfRoom, i]));
            }

            return adjacentRooms;
        }

        public List<Tile> GetPathBetweenTiles(Point sourcePosition, Point targetPosition)
        {
            return Tiles.GetShortestPathBetween((sourcePosition.X, sourcePosition.Y), (targetPosition.X, targetPosition.Y), true, t => t.Position.X, t => t.Position.Y, ArrayHelpers.GetSquaredEuclideanDistanceBetweenCells, GetTileConnectionWeight, t => t.IsWalkable);
        }

        private double GetTileConnectionWeight(int x1, int y1, int x2, int y2)
        {
            var distance = ArrayHelpers.GetSquaredEuclideanDistanceBetweenCells(x1, y1, x2, y2);
            if (distance == 0) return 0;
            // Discourage but not prohibit walking on occupied tiles
            var tile = GetTileFromCoordinates(x2, y2);
            if (tile.IsOccupied)
                return distance + 24;
            // Discourage but not prohibit walking on visible traps
            if (tile.Trap?.Visible == true && tile.Trap?.ExistenceStatus == EntityExistenceStatus.Alive)
                return distance + 8;
            return distance;
        }

        public List<Tile> GetAdjacentTiles(Point position)
        {
            return Tiles.GetAdjacentElements(GetTileFromCoordinates(position), true);
        }
        public List<Tile> GetAdjacentWalkableTiles(Point position)
        {
            return Tiles.GetAdjacentElementsWhere(GetTileFromCoordinates(position), true, t => t.IsWalkable);
        }
        public List<Tile> GetFOVTilesWithinDistance(Point source, int distance)
        {
            var tilesWithinDistance = Tiles.Where(t => t.Type != TileType.Empty && Math.Round(Point.Distance(source, t.Position), 0, MidpointRounding.AwayFromZero) <= distance);
            var visibleTiles = new List<Tile>();
            foreach (var tile in tilesWithinDistance)
            {
                visibleTiles.AddRange(GetFOVLineBetweenTiles(GetTileFromCoordinates(source.X, source.Y), tile, distance));
            }
            return visibleTiles.Distinct().ToList();
        }
        public List<Tile> GetFOVLineBetweenTiles(Tile source, Tile target, int distance)
        {
            var componentPositions = new List<Point>();
            var sourcePosition = source.Position;
            var targetPosition = target.Position;
            componentPositions.AddRange(MathAlgorithms.Raycast(sourcePosition, targetPosition, p => p.X, p => p.Y, (x, y) => new Point(x, y), p => GetTileFromCoordinates(p).IsWalkable, p => GetTileFromCoordinates(p).Type == TileType.Hallway && GetTileFromCoordinates(p).Room == null, distance, (p1, p2) => Point.Distance(p1, p2)).ToList());
            if(!componentPositions.Contains(sourcePosition))
                componentPositions.Add(sourcePosition);

            if (!componentPositions.Exists(p => p.Equals(sourcePosition)))
                return new List<Tile>();

            return componentPositions.OrderBy(p => Point.Distance(p, sourcePosition))
                                     .ToList()
                                     .ConvertAll(p => GetTileFromCoordinates(p))
                                     .TakeUntil(t => !t.IsWalkable)
                                     .ToList();
        }

        #endregion

        #region Tile Making

        private void SetHallways()
        {
            for (int n = 0; n < MaxConnectionsBetweenRooms; n++)
            {
                for (int x = 0; x < RoomCountRows; x++)
                {
                    for (int y = 0; y < RoomCountColumns; y++)
                    {
                        var room = GetRoomByRowAndColumn(x, y);
                        if (room == null) continue;
                        foreach (var roomConnection in GetConnectionsForRoom(room))
                        {
                            if (room.RoomRow == roomConnection.Room.RoomRow && room.RoomColumn == roomConnection.Room.RoomColumn)
                            {
                                var roomsAsList = Rooms.ToList();
                                var indexOfThisRoom = roomsAsList.IndexOf(room);
                                var indexOfOtherRoom = roomsAsList.IndexOf(roomConnection.Room);
                                RoomAdjacencyMatrix[indexOfThisRoom, indexOfOtherRoom] = RoomAdjacencyMatrix[indexOfOtherRoom, indexOfThisRoom] = RoomConnectionType.None;
                                continue;
                            }
                            CreateHallway((room, roomConnection.Room, roomConnection.ConnectionType));
                            if (Rng.Next(1, 100) > OddsForExtraConnections)
                            {
                                var roomsAsList = Rooms.ToList();
                                var indexOfThisRoom = roomsAsList.IndexOf(room);
                                var indexOfOtherRoom = roomsAsList.IndexOf(roomConnection.Room);
                                RoomAdjacencyMatrix[indexOfThisRoom, indexOfOtherRoom] = RoomAdjacencyMatrix[indexOfOtherRoom, indexOfThisRoom] = RoomConnectionType.None;
                            }
                        }
                    }
                }
            }
        }

        private void CreateHallway((Room Source, Room Target, RoomConnectionType Tag) edge)
        {
            var roomA = edge.Source;
            var roomB = edge.Target;

            try
            {
                if (edge.Tag == RoomConnectionType.Horizontal)
                {
                    if (roomA.RoomColumn < roomB.RoomColumn)
                        CreateHorizontalHallway(roomA, roomB);
                    else
                        CreateHorizontalHallway(roomB, roomA);
                }
                else if (edge.Tag == RoomConnectionType.Vertical)
                {
                    if (roomA.RoomRow < roomB.RoomRow)
                        CreateVerticalHallway(roomA, roomB);
                    else
                        CreateVerticalHallway(roomB, roomA);
                }
            }
            catch
            {
                // If cannot build Hallway, pretend we never tried.
            }
        }

        private void CreateHorizontalHallway(Room leftRoom, Room rightRoom)
        {
            Tile? leftConnector = null, rightConnector = null;

            if (leftRoom.Width == 1 && leftRoom.Height == 1)
                leftConnector = GetTileFromCoordinates(leftRoom.Position.X, leftRoom.Position.Y);
            if (rightRoom.Width == 1 && rightRoom.Height == 1)
                rightConnector = GetTileFromCoordinates(rightRoom.Position.X, rightRoom.Position.Y);

            var leftRoomMinY = leftRoom.Position.Y + 1;
            var leftRoomMaxY = leftRoom.Position.Y + leftRoom.Height - 2;

            if (leftConnector == null)
            {
                var x = leftRoom.Position.X + leftRoom.Width - 1;
                var y = Rng.NextInclusive(leftRoomMinY, leftRoomMaxY);
                leftConnector = GetTileFromCoordinates(x, y);
            }

            var rightRoomMinY = rightRoom.Position.Y + 1;
            var rightRoomMaxY = rightRoom.Position.Y + rightRoom.Height - 2;

            if (rightConnector == null)
            {
                var x = rightRoom.Position.X;
                var y = Rng.NextInclusive(rightRoomMinY, rightRoomMaxY);
                rightConnector = GetTileFromCoordinates(x, y);
            }

            var hallwayGenerationTries = 0;
            List<Tile> tilesToConvert;
            bool isValidHallway;

            do
            {
                hallwayGenerationTries++;
                tilesToConvert = new();

                var (InBetweenConnectorPosition, ConnectorAPosition, ConnectorBPosition) = CalculateConnectionPosition(leftConnector, rightConnector, RoomConnectionType.Horizontal);

                if (ConnectorAPosition != null)
                    tilesToConvert.Add(GetTileFromCoordinates(ConnectorAPosition));
                if (ConnectorBPosition != null)
                    tilesToConvert.Add(GetTileFromCoordinates(ConnectorBPosition));

                if (InBetweenConnectorPosition != null)
                {
                    // Horizontal line from Left Room to Hallway connection column
                    for (var i = leftConnector.Position.X; i <= InBetweenConnectorPosition.X; i++)
                    {
                        tilesToConvert.Add(GetTileFromCoordinates(i, leftConnector.Position.Y));
                    }

                    // Horizontal line from Right Room to Hallway connection column
                    for (var i = InBetweenConnectorPosition.X; i <= rightConnector.Position.X; i++)
                    {
                        tilesToConvert.Add(GetTileFromCoordinates(i, rightConnector.Position.Y));
                    }

                    // Draw a downwards line in case entrypoint from left is higher or equal to entrypoint from right
                    for (var i = rightConnector.Position.Y + 1; i < leftConnector.Position.Y; i++)
                    {
                        tilesToConvert.Add(GetTileFromCoordinates(InBetweenConnectorPosition.X, i));
                    }
                    // Draw an upwards line in case entrypoint from left is lower than entrypoint from right
                    for (var i = leftConnector.Position.Y + 1; i < rightConnector.Position.Y; i++)
                    {
                        tilesToConvert.Add(GetTileFromCoordinates(InBetweenConnectorPosition.X, i));
                    }
                }

                isValidHallway = IsHallwayTileGroupValid(tilesToConvert, leftConnector, rightConnector);
            }
            while (!isValidHallway && hallwayGenerationTries < Constants.MaxGenerationTriesForHallway);

            if (isValidHallway)
            {
                BuildHallwayTiles(tilesToConvert, leftConnector, rightConnector);
            }
        }

        private void CreateVerticalHallway(Room topRoom, Room downRoom)
        {
            Tile? topConnector = null, downConnector = null;

            if (topRoom.Width == 1 && topRoom.Height == 1)
                topConnector = GetTileFromCoordinates(topRoom.Position.X, topRoom.Position.Y);
            if (downRoom.Width == 1 && downRoom.Height == 1)
                downConnector = GetTileFromCoordinates(downRoom.Position.X, downRoom.Position.Y);

            var topRoomMinX = topRoom.Position.X + 1;
            var topRoomMaxX = topRoom.Position.X + topRoom.Width - 2;

            if (topConnector == null)
            {
                var x = Rng.NextInclusive(topRoomMinX, topRoomMaxX);
                var y = topRoom.Position.Y + topRoom.Height - 1;
                topConnector = GetTileFromCoordinates(x, y);
            }

            var downRoomMinX = downRoom.Position.X + 1;
            var downRoomMaxX = downRoom.Position.X + downRoom.Width - 2;

            if (downConnector == null)
            {
                var x = Rng.NextInclusive(downRoomMinX, downRoomMaxX);
                var y = downRoom.Position.Y;
                downConnector = GetTileFromCoordinates(x, y);
            }

            var hallwayGenerationTries = 0;
            List<Tile> tilesToConvert;
            bool isValidHallway;

            do
            {
                hallwayGenerationTries++;
                tilesToConvert = new();

                var (InBetweenConnectorPosition, ConnectorAPosition, ConnectorBPosition) = CalculateConnectionPosition(topConnector, downConnector, RoomConnectionType.Vertical);

                if (ConnectorAPosition != null)
                    tilesToConvert.Add(GetTileFromCoordinates(ConnectorAPosition));
                if (ConnectorBPosition != null)
                    tilesToConvert.Add(GetTileFromCoordinates(ConnectorBPosition));

                if (InBetweenConnectorPosition != null)
                {
                    // Vertical line from Up Room to Hallway connection row
                    for (var i = topConnector.Position.Y; i <= InBetweenConnectorPosition.Y; i++)
                    {
                        tilesToConvert.Add(GetTileFromCoordinates(topConnector.Position.X, i));
                    }

                    // Vertical line from Down Room to Hallway connection row
                    for (var i = InBetweenConnectorPosition.Y; i <= downConnector.Position.Y; i++)
                    {
                        tilesToConvert.Add(GetTileFromCoordinates(downConnector.Position.X, i));
                    }

                    // Draw a rightwards line in case entrypoint from up is more or equal to the right to entrypoint from below
                    for (var i = downConnector.Position.X + 1; i < topConnector.Position.X; i++)
                    {
                        tilesToConvert.Add(GetTileFromCoordinates(i, InBetweenConnectorPosition.Y));
                    }
                    // Draw a leftwards line in case entrypoint from up is more to the left than entrypoint from below
                    for (var i = topConnector.Position.X + 1; i < downConnector.Position.X; i++)
                    {
                        tilesToConvert.Add(GetTileFromCoordinates(i, InBetweenConnectorPosition.Y));
                    }
                }

                isValidHallway = IsHallwayTileGroupValid(tilesToConvert, topConnector, downConnector);
            }
            while (!isValidHallway && hallwayGenerationTries < Constants.MaxGenerationTriesForHallway);

            if (isValidHallway)
            {
                BuildHallwayTiles(tilesToConvert, topConnector, downConnector);
            }
        }

        private (Point? InBetweenConnectorPosition, Point? ConnectorAPosition, Point? ConnectorBPosition) CalculateConnectionPosition(Tile connectorA, Tile connectorB, RoomConnectionType connectionType)
        {
            int minX = 0, maxX = -1, minY = 0, maxY = -1;
            var connectorAPosition = new Point(connectorA.Position.X, connectorA.Position.Y);
            var connectorBPosition = new Point(connectorB.Position.X, connectorB.Position.Y);

            if (connectionType == RoomConnectionType.Horizontal)
            {
                if (Math.Abs(connectorA.Position.X - connectorB.Position.X) == 1)
                {
                    if (connectorA.Position.Y == connectorB.Position.Y)
                        return (null, connectorA.Position, connectorB.Position);
                    var leftYs = Enumerable.Range(connectorA.Room.Position.Y + 1, Math.Max(1, connectorA.Room.Height - 2)).ToList();
                    var rightYs = Enumerable.Range(connectorB.Room.Position.Y + 1, Math.Max(1, connectorB.Room.Height - 2)).ToList();
                    var sharedYs = leftYs.Intersect(rightYs);
                    if (!sharedYs.Any()) return (null, null, null);
                    var pickedY = sharedYs.TakeRandomElement(Rng);
                    connectorAPosition = new Point(connectorA.Position.X, pickedY);
                    connectorBPosition = new Point(connectorB.Position.X, pickedY);
                    return (null, connectorAPosition, connectorBPosition); // No need for a connection point in this case.
                }

                minX = connectorA.Position.X + 1;
                maxX = connectorB.Position.X - 1;
                minY = Math.Min(connectorA.Position.Y, connectorB.Position.Y);
                maxY = Math.Max(connectorA.Position.Y, connectorB.Position.Y);
            }
            else if (connectionType == RoomConnectionType.Vertical)
            {
                if (Math.Abs(connectorA.Position.Y - connectorB.Position.Y) == 1)
                {
                    if (connectorA.Position.X == connectorB.Position.X)
                        return (null, connectorA.Position, connectorB.Position);
                    var topXs = Enumerable.Range(connectorA.Room.Position.X + 1, Math.Max(1, connectorA.Room.Width - 2)).ToList();
                    var downXs = Enumerable.Range(connectorB.Room.Position.X + 1, Math.Max(1, connectorB.Room.Width - 2)).ToList();
                    var sharedXs = topXs.Intersect(downXs);
                    if (!sharedXs.Any()) return (null, null, null);
                    var pickedX = sharedXs.TakeRandomElement(Rng);
                    connectorAPosition = new Point(pickedX, connectorA.Position.Y);
                    connectorBPosition = new Point(pickedX, connectorB.Position.Y);
                    return (null, connectorAPosition, connectorBPosition); // No need for a connection point in this case.
                }

                minY = connectorA.Position.Y + 1;
                maxY = connectorB.Position.Y - 1;
                minX = Math.Min(connectorA.Position.X, connectorB.Position.X);
                maxX = Math.Max(connectorA.Position.X, connectorB.Position.X);
            }

            try
            {
                var connectionPosition = new Point
                {
                    X = Rng.NextInclusive(minX, maxX),
                    Y = Rng.NextInclusive(minY, maxY)
                };

                return (connectionPosition, connectorAPosition, connectorBPosition);
            }
            catch
            {
                return (null, null, null);
            }
        }

        private static bool IsHallwayTileGroupValid(List<Tile> tilesToConvert, Tile connectorA, Tile connectorB)
        {
            if (!tilesToConvert.Any()) return false;

            foreach (var tile in tilesToConvert)
            {
                if (tile == connectorA || tile == connectorB) continue;
                if (tile.Type == TileType.Wall || tile.Type == TileType.Floor || tile.Type == TileType.Stairs)
                    return false;
            }
            return true;
        }

        private static void BuildHallwayTiles(List<Tile> tilesToConvert, Tile connectorA, Tile connectorB)
        {
            foreach (var tile in tilesToConvert)
            {
                tile.Type = TileType.Hallway;
                if (tile == connectorA || tile == connectorB)
                    tile.IsConnectorTile = true;
            }
        }

        #endregion

        #region Flags

        public bool HasFlag(string key)
        {
            return Flags.Exists(f => f.Key.Equals(key));
        }

        public int GetFlagValue(string key)
        {
            var flag = Flags.Find(f => f.Key.Equals(key)) ?? throw new ArgumentException($"There's no flag with key {key} in {FloorName}");
            return flag.Value;
        }

        public void CreateFlag(string key, int value, bool removeOnFloorChange)
        {
            Flags.Add(new Flag(key, value, removeOnFloorChange));
        }

        public void SetFlagValue(string key, int value)
        {
            var flag = Flags.Find(f => f.Key.Equals(key)) ?? throw new ArgumentException($"There's no flag with key {key} in {FloorName}");
            flag.Value = value;
        }

        #endregion

        #region Utils
        private enum RoomConnectionType
        {
            Horizontal = 1,
            Vertical = 2,
            None = 0
        }
        #endregion
    }
    #pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
    #pragma warning restore CS8601 // Posible asignación de referencia nula
    #pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
    #pragma warning restore CS8604 // Posible argumento de referencia nulo
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
