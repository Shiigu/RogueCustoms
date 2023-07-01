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

namespace RogueCustomsGameEngine.Game.DungeonStructure
{
    public class Map
    {
        #region Fields

        private int _generationTries;

        private int CurrentEntityId;

        private bool _displayedTurnMessage;

        public PlayerCharacter Player => Dungeon.PlayerCharacter;

        public int TurnCount { get; set; }
        private int LastMonsterGenerationTurn;

        public int Id { get; }
        private int Seed { get; }

        private readonly Dungeon Dungeon;
        public Locale Locale => Dungeon.LocaleToUse;
        public DungeonStatus DungeonStatus
        {
            get { return Dungeon.DungeonStatus; }
            set { Dungeon.DungeonStatus = value; }
        }

        private readonly int FloorLevel;

        private readonly FloorType FloorConfigurationToUse;

        private readonly GeneratorAlgorithm GeneratorAlgorithmToUse;



        public string FloorName => Locale["FloorName"].Format(new {
                                                                DungeonName = Dungeon.Name.ToUpperInvariant(),
                                                                FloorLevel = FloorLevel.ToString()
                                                                });

        private int RoomCountRows => GeneratorAlgorithmToUse.Rows;
        private int RoomCountColumns => GeneratorAlgorithmToUse.Columns;
        public int MaxConnectionsBetweenRooms => FloorConfigurationToUse.MaxConnectionsBetweenRooms;
        public int OddsForExtraConnections => FloorConfigurationToUse.OddsForExtraConnections;
        public int RoomFusionOdds => FloorConfigurationToUse.RoomFusionOdds;

        public int Width => FloorConfigurationToUse.Width;
        public int Height => FloorConfigurationToUse.Height;

        private int TotalMonstersInFloor => AICharacters.Where(e => e.ExistenceStatus == EntityExistenceStatus.Alive).Count();
        private int TotalItemsInFloor => Items.Where(e => e.ExistenceStatus != EntityExistenceStatus.Gone).Count();
        private int TotalTrapsInFloor => Traps.Where(e => e.ExistenceStatus != EntityExistenceStatus.Gone).Count();
        private int MinRoomWidth { get; set; }
        private int MaxRoomWidth { get; set; }
        private int MinRoomHeight { get; set; }
        private int MaxRoomHeight { get; set; }
        public bool StairsAreSet { get; set; } = false;
        public List<Entity> Entities => Characters.Cast<Entity>().Union(Items.Cast<Entity>()).Union(Traps.Cast<Entity>()).Where(e => e != null).ToList();
        public List<Character> Characters => AICharacters.Cast<Character>().Append(Player).ToList();
        public List<NonPlayableCharacter> AICharacters { get; set; }
        public List<Item> Items { get; set; }
        public List<Item> Traps { get; set; }

        public readonly List<AlteredStatus> PossibleStatuses;

        public readonly Random Rng;
        private (Point TopLeftCorner, Point BottomRightCorner)[,] RoomLimitsTable { get; set; }

        public Tile[,] Tiles { get; private set; }

        public Room[,] Rooms { get; private set; }
        public int RoomCount => Rooms.GetLength(0) * Rooms.GetLength(1);
        private RoomConnectionType?[,] RoomAdjacencyMatrix;

        #endregion

        public Map(Dungeon dungeon, int floorLevel)
        {
            Dungeon = dungeon;
            FloorLevel = floorLevel;
            AICharacters = new List<NonPlayableCharacter>();
            Items = new List<Item>();
            Traps = new List<Item>();
            FloorConfigurationToUse = Dungeon.FloorTypes.Find(ft => floorLevel.Between(ft.MinFloorLevel, ft.MaxFloorLevel));
            if (FloorConfigurationToUse == null)
                throw new Exception("There's no valid configuration for the current floor");
            Seed = Environment.TickCount;
            Rng = new Random(Seed);
            if (!FloorConfigurationToUse.PossibleGeneratorAlgorithms.Any())
                throw new Exception("There's no valid generation algorithm for the current floor");

            GeneratorAlgorithmToUse = FloorConfigurationToUse.PossibleGeneratorAlgorithms[Rng.NextInclusive(FloorConfigurationToUse.PossibleGeneratorAlgorithms.Count - 1)];

            PossibleStatuses = new List<AlteredStatus>();
            Dungeon.Classes.Where(c => c.EntityType == EntityType.AlteredStatus).ForEach(alsc => PossibleStatuses.Add(new AlteredStatus(alsc, this)));

            AttackActions.Rng = Rng;
            AttackActions.Map = this;
            CharacterActions.Rng = Rng;
            CharacterActions.Map = this;
            ItemActions.Rng = Rng;
            ItemActions.Map = this;
            GenericActions.Rng = Rng;
            GenericActions.Map = this;

            ActionHelpers.Map = this;
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
            ResetAndCreateTiles();
            Rooms = new Room[1, 1];
            var room = new Room(this, new Point(0, 0), 0, 0, 25, 10);
            Rooms[0, 0] = room;
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
            (int Row, int Column) lastPossibleNormalRoomIndex = (RoomCountRows - 1, RoomCountColumns - 1);
            GetPossibleRoomData();
            do
            {
                Rooms = new Room[RoomCountRows, RoomCountColumns];
                for (var row = 0; row < RoomCountRows; row++)
                {
                    for (var column = 0; column < RoomCountColumns; column++)
                    {
                        var rngRoom = Rng.NextInclusive(1, 100);
                        // Force generation of a normal room if it's the last possible location and no normal Room was created yet
                        if (normalRoomsCount == 0 && row == lastPossibleNormalRoomIndex.Row && column == lastPossibleNormalRoomIndex.Column || !rngRoom.Between(71, 100))
                        {
                            var (MinX, MinY, MaxX, MaxY) = GetPossibleCoordinatesForRoom(row, column);
                            var rngX1 = Rng.NextInclusive(MinX, MaxX - MinRoomWidth);
                            var rngX2 = Rng.NextInclusive(rngX1 + MinRoomWidth, MaxX);
                            var roomWidth = rngX2 - rngX1 + 1;
                            var rngY1 = Rng.NextInclusive(MinY, MaxY - MinRoomHeight);
                            var rngY2 = Rng.NextInclusive(rngY1 + MinRoomHeight, MaxY);
                            var roomHeight = rngY2 - rngY1 + 1;
                            Rooms[row, column] = new Room(this, new Point(rngX1, rngY1), row, column, roomWidth, roomHeight); ;
                            normalRoomsCount++;
                        }
                        else if (rngRoom.Between(71, 85))
                        {
                            var (MinX, MinY, MaxX, MaxY) = GetPossibleCoordinatesForRoom(row, column);
                            var rngX = Rng.NextInclusive(MinX + 1, MaxX - 1);
                            var rngY = Rng.NextInclusive(MinY + 1, MaxY - 1);
                            Rooms[row, column] = new Room(this, new Point(rngX, rngY), row, column, 1, 1);
                        }
                    }
                }
            }
            while (!SetPossibleRoomConnections(false).IsFullyConnectedAdjacencyMatrix(r => r != null));
        }

        private void CreateRoomsWithSpecificLayout(List<(int Row, int Column)> normalRooms, List<(int Row, int Column)> dummyRooms)
        {
            if (normalRooms.Intersect(dummyRooms).Any()) throw new Exception("At least one room is set as Normal and Dummy");
            var normalRoomsCount = 0;
            GetPossibleRoomData();
            Rooms = new Room[RoomCountRows, RoomCountColumns];
            for (var row = 0; row < RoomCountRows; row++)
            {
                for (var column = 0; column < RoomCountColumns; column++)
                {
                    if (normalRooms.Any(nr => nr.Row == row && nr.Column == column))
                    {
                        var (MinX, MinY, MaxX, MaxY) = GetPossibleCoordinatesForRoom(row, column);
                        var rngX1 = Rng.NextInclusive(MinX, MaxX - MinRoomWidth);
                        var rngX2 = Rng.NextInclusive(rngX1 + MinRoomWidth, MaxX);
                        var roomWidth = rngX2 - rngX1 + 1;
                        var rngY1 = Rng.NextInclusive(MinY, MaxY - MinRoomHeight);
                        var rngY2 = Rng.NextInclusive(rngY1 + MinRoomHeight, MaxY);
                        var roomHeight = rngY2 - rngY1 + 1;
                        Rooms[row, column] = new Room(this, new Point(rngX1, rngY1), row, column, roomWidth, roomHeight);
                        normalRoomsCount++;
                    }
                    else if (dummyRooms.Any(fd => fd.Row == row && fd.Column == column))
                    {
                        var (MinX, MinY, MaxX, MaxY) = GetPossibleCoordinatesForRoom(row, column);
                        var rngX = Rng.NextInclusive(MinX + 1, MaxX - 1);
                        var rngY = Rng.NextInclusive(MinY + 1, MaxY - 1);
                        var room = new Room(this, new Point(rngX, rngY), row, column, 1, 1);
                        Rooms[row, column] = new Room(this, new Point(rngX, rngY), row, column, 1, 1);
                    }
                }
            }
        }

        private bool ConnectRoomsOrTurnIntoDefaultIfNotPossible()
        {
            RoomAdjacencyMatrix = SetPossibleRoomConnections(true);
            var success = RoomAdjacencyMatrix.IsFullyConnectedAdjacencyMatrix(r => r != null);
            if (success)
            {
                FuseRoomsIfNeeded();
                SetHallways();
                Parallel.For(0, RoomCountRows, y =>
                {
                    Parallel.For(0, RoomCountColumns, x =>
                    {
                        Rooms[y, x]?.CreateTiles();
                    });
                });
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
                Dungeon.PlayerCharacter = pc;
            else if (entity is NonPlayableCharacter npc)
                AICharacters.Add(npc);
            else if(entity is Item i)
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
                ?? throw new Exception("Class does not exist!");
            Entity entity = null;
            switch (entityClass.EntityType)
            {
                case EntityType.Player:
                    entity = new PlayerCharacter(entityClass, level, this)
                    {
                        Id = CurrentEntityId,
                        Position = PickEmptyPosition(TurnCount == 0)
                    };
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
                    throw new Exception("Entity lacks a valid type!");
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
                if (entity is PlayerCharacter p)
                    Dungeon.PlayerCharacter = p;
                else if (entity is NonPlayableCharacter npc)
                    AICharacters.Add(npc);
                var weaponEntityClass = Dungeon.Classes.Find(cl => cl.Id.Equals(c.StartingWeaponId))
                    ?? throw new Exception("Class does not have a valid starting weapon!");
                c.StartingWeapon = new Item(weaponEntityClass, this)
                {
                    Id = CurrentEntityId,
                    Owner = c
                };
                CurrentEntityId++;
                var armorEntityClass = Dungeon.Classes.Find(cl => cl.Id.Equals(c.StartingArmorId))
                    ?? throw new Exception("Class does not have a valid starting armor!");
                c.StartingArmor = new Item(armorEntityClass, this)
                {
                    Id = CurrentEntityId,
                    Owner = c
                };
                CurrentEntityId++;
            }
        }

        private void PlacePlayer()
        {
            var playerClass = Dungeon.Classes.Find(c => c.EntityType == EntityType.Player);
            if (playerClass == null)
                throw new Exception("There is no Player class!");
            if (Player == null)
            {
                AddEntity(playerClass.Id);
            }
            else
            {
                Player.Map = this;
                Player.Id = CurrentEntityId;
                CurrentEntityId++;
                if (Player.EquippedWeapon != null && !Player.EquippedWeapon.ClassId.Equals(playerClass.StartingWeaponId))
                {
                    Player.EquippedWeapon.Id = CurrentEntityId;
                    Player.EquippedWeapon.Map = this;
                    AddEntity(Player.EquippedWeapon, true);
                    CurrentEntityId++;
                }
                if (Player.EquippedArmor != null && !Player.EquippedArmor.ClassId.Equals(playerClass.StartingArmorId))
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
                int generationAttempts, generationsToTry;
                #region Generate Monsters
                if(FloorConfigurationToUse.PossibleMonsters.Any())
                {
                    var totalMonsterGeneratorChance = FloorConfigurationToUse.PossibleMonsters.Sum(mg => mg.ChanceToPick);
                    if (!totalMonsterGeneratorChance.Between(1, 100)) throw new Exception("Monster generation odds are not 1-100%");
                    List<ClassInFloor> usableMonsterGenerators = new();
                    FloorConfigurationToUse.PossibleMonsters.ForEach(pm =>
                    {
                        if (!pm.CanSpawnOnFirstTurn || pm.OverallMaxForKindInFloor > 0 && pm.TotalGeneratedInFloor >= pm.OverallMaxForKindInFloor) return;
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
                        usableMonsterGenerators.RemoveAll(mg => mg.SimultaneousMaxForKindInFloor > 0 && AICharacters.Count(e => e.ClassId.Equals(mg.Class.Id) && e.ExistenceStatus == EntityExistenceStatus.Alive) >= mg.SimultaneousMaxForKindInFloor || mg.OverallMaxForKindInFloor > 0 && mg.TotalGeneratedInFloor >= mg.OverallMaxForKindInFloor);
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
                    if (!totalItemGeneratorChance.Between(1, 100)) throw new Exception("Item generation odds are not 1-100%");
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
                    if (!totalTrapGeneratorChance.Between(1, 100)) throw new Exception("Trap generation odds are not 1-100%");
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
                FloorConfigurationToUse.OnFloorStartActions.ForEach(ofsa => ofsa.Do(Player, Player));

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
            _displayedTurnMessage = false;
            Player.RemainingMovement = Player.Movement;
            Player.PerformOnTurnStartActions();
            AICharacters.Where(e => e != null).ForEach(e =>
            {
                e.RemainingMovement = e.Movement;
                e.PerformOnTurnStartActions();
            });
        }

        private void AddEnemy()
        {
            if (!FloorConfigurationToUse.PossibleMonsters.Any() || TotalMonstersInFloor >= FloorConfigurationToUse.SimultaneousMaxMonstersInFloor) return;
            List<ClassInFloor> usableMonsterGenerators = new();
            FloorConfigurationToUse.PossibleMonsters.ForEach(pm =>
            {
                if (!pm.CanSpawnAfterFirstTurn || pm.OverallMaxForKindInFloor > 0 && pm.TotalGeneratedInFloor >= pm.OverallMaxForKindInFloor) return;
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
            Player.UpdateVisibility();
            if (Player.RemainingMovement > 0 && Player.CanTakeAction) return;
            var aiCharactersThatCanAct = AICharacters.Where(c => c.CanTakeAction);
            Parallel.ForEach(aiCharactersThatCanAct, aictca => aictca.PickTargetAndPath());
            aiCharactersThatCanAct.ForEach(aictca => aictca.AttackOrMove());
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
            if (Player.RemainingMovement != 0)
            {
                var currentTile = GetTileFromCoordinates(Player.Position)
                    ?? throw new Exception("PlayerEntity is on a nonexistent Tile");
                var targetTile = GetTileFromCoordinates(Player.Position.X + x, Player.Position.Y + y)
                    ?? throw new Exception("PlayerEntity is about to move to a nonexistent Tile");
                if (currentTile == targetTile) // This is only possible if the player chooses to Skip Turn.
                {
                    Player.RemainingMovement = 0;
                }
                else if (Tiles.GetAdjacentElements(currentTile, true).Contains(targetTile))
                {
                    if (!targetTile.IsWalkable) return;
                    if (x != 0 && y != 0)
                    {
                        if (!GetTileFromCoordinates(Player.Position.X + x, Player.Position.Y).IsWalkable
                            || !GetTileFromCoordinates(Player.Position.X, Player.Position.Y + y).IsWalkable)
                        {
                            return;
                        }
                    }
                    var characterInTargetTile = Characters.Find(c => c.ContainingTile == targetTile && c != Player && c.ExistenceStatus == EntityExistenceStatus.Alive);
                    if (characterInTargetTile != null)
                    {
                        if(!characterInTargetTile.Faction.EnemiesWith.Contains(Player.Faction))
                        {
                            // Swap positions with allies or neutrals
                            characterInTargetTile.Position = Player.Position;
                            characterInTargetTile.RemainingMovement--;
                        }
                        else
                        {
                            return;
                        }
                    }
                    TryMoveCharacter(Player, targetTile);
                }
            }
            if (Player.RemainingMovement == 0) ProcessTurn();
        }

        public bool TryMoveCharacter(Character character, Tile targetTile)
        {
            var solidEntityInTile = GetEntitiesFromCoordinates(targetTile.Position.X, targetTile.Position.Y).Find(e => !e.Passable);
            if (solidEntityInTile != null) return false;
            character.Position = targetTile.Position;
            var passableAliveEntities = GetEntitiesFromCoordinates(targetTile.Position.X, targetTile.Position.Y).Where(e => e.ExistenceStatus == EntityExistenceStatus.Alive && e.Passable);

            passableAliveEntities.ForEach(pae =>
            {
                if (pae is Item i)
                    character.TryToPickItem(i);
            });

            character.RemainingMovement--;

            return true;
        }

        public void PlayerUseStairs()
        {
            var stairsTile = Tiles.Find(t => t.Type == TileType.Stairs);
            if (Player.ContainingTile != stairsTile)
                throw new Exception($"Player is trying to use non-existent stairs at ({Player.ContainingTile.Position.X}, {Player.ContainingTile.Position.Y})");
            AppendMessage(Locale["FloorLeave"].Format(new { TurnCount = TurnCount.ToString() }), Color.Yellow);
            Dungeon.TakeStairs();
        }

        private void PlayerUseItem(Item item)
        {
            item.OnItemUseActions.ForEach(oiua => oiua.Do(item, Player));
            Player.RemainingMovement = 0;
            ProcessTurn();
        }

        public void PlayerUseItemInFloor()
        {
            if (GetEntitiesFromCoordinates(Player.Position.X, Player.Position.Y)
                .Find(e => (e.EntityType == EntityType.Consumable || e.EntityType == EntityType.Weapon || e.EntityType == EntityType.Armor)
                    && e.ExistenceStatus == EntityExistenceStatus.Alive && e.Passable) is not Item usableItem)
                return;
            PlayerUseItem(usableItem);
        }
        public void PlayerPickUpItemInFloor()
        {
            var itemThatCanBePickedUp = GetEntitiesFromCoordinates(Player.Position.X, Player.Position.Y)
                .ConvertAll(e => e as Item)
                .Find(e => e?.ExistenceStatus == EntityExistenceStatus.Alive && e?.CanBePickedUp == true);
            if (itemThatCanBePickedUp == null)
                return;
            if (Player.Inventory.Count == Player.InventorySize)
            {
                AppendMessage(Locale["InventoryIsFull"].Format(new { CharacterName = Player.Name, ItemName = itemThatCanBePickedUp.Name }));
            }
            else
            {
                Player.PickItem(itemThatCanBePickedUp);
                Player.RemainingMovement = 0;
                ProcessTurn();
            }
        }
        public void PlayerUseItemFromInventory(int itemId)
        {
            var item = Items.Find(i => i.Id == itemId)
                ?? throw new Exception("Player attempted to use an item that does not exist!");
            PlayerUseItem(item);
        }

        public void PlayerDropItemFromInventory(int itemId)
        {
            var itemThatCanBeDropped = Items.Find(i => i.Id == itemId)
                ?? throw new Exception("Player attempted to use an item that does not exist!");
            var entitiesInTile = GetEntitiesFromCoordinates(Player.Position);
            if (entitiesInTile.Any(e => e.Passable && e.ExistenceStatus != EntityExistenceStatus.Gone))
            {
                AppendMessage(Locale["TilesIsOccupied"].Format(new { CharacterName = Player.Name, ItemName = itemThatCanBeDropped.Name }));
            }
            else
            {
                Player.DropItem(itemThatCanBeDropped);
                Player.RemainingMovement = 0;
                ProcessTurn();
            }
        }

        public void PlayerSwapFloorItemWithInventoryItem(int itemId)
        {
            var itemInInventory = Items.Find(i => i.Id == itemId)
                ?? throw new Exception("Player attempted to use an item that does not exist!");
            var itemInTile = Items.Find(i => i.Position?.Equals(Player.Position) == true && i.ExistenceStatus != EntityExistenceStatus.Gone);
            if (itemInTile != null)
            {
                Player.DropItem(itemInInventory);
                Player.TryToPickItem(itemInTile);
                Player.RemainingMovement = 0;
                ProcessTurn();
            }
            else
            {
                throw new Exception("Player attempted to pick an item from a tile without item!");
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

        public void PlayerAttackTargetWith(string name, int x, int y)
        {
            var action = Player.OnAttackActions.Find(oaa => oaa.Name.Equals(name));

            if (action == null) throw new Exception("Player attempted use a non-existent action.");

            var characterInTile = GetTileFromCoordinates(x, y).Character;

            if (characterInTile != null)
                Player.AttackCharacter(characterInTile, action);
            else
                AddMessageBox(Dungeon.Name, Locale["NoTarget"], "Ooops!", new GameColor(Color.LightGoldenrodYellow));

            ProcessTurn();
        }

        public ActionListDto GetPlayerAttackActions(int x, int y)
        {
            var characterInTile = GetTileFromCoordinates(x, y).Character;
            var onAttackActionDtos = new List<ActionItemDto>();

            Player.OnAttackActions.ForEach(oaa => onAttackActionDtos.Add(new ActionItemDto(oaa, characterInTile, this)));

            return new ActionListDto
            {
                TargetName = (characterInTile != null) ? characterInTile.Name : "<?????>",
                Actions = onAttackActionDtos
            };
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

        private Point PickEmptyPosition(bool allowPlayerRoom)
        {
            int rngX, rngY;
            var nonDummyRooms = Rooms.Where(r => r.Width > 1 && r.Height > 1).Distinct().ToList();
            Tile tileToCheck;
            bool hasLaterallyAdjacentHallways;
            var playerRoom = Player?.Position != null ? GetRoomInCoordinates(Player.Position.X, Player.Position.Y) : null;
            if(nonDummyRooms.Count > 1 && !allowPlayerRoom)
                nonDummyRooms.Remove(playerRoom);
            do
            {
                var possibleNonDummyRoom = nonDummyRooms.TakeRandomElement(Rng);
                rngX = Rng.NextInclusive(possibleNonDummyRoom.Position.X + 1, possibleNonDummyRoom.Position.X + possibleNonDummyRoom.Width - 2);
                rngY = Rng.NextInclusive(possibleNonDummyRoom.Position.Y + 1, possibleNonDummyRoom.Position.Y + possibleNonDummyRoom.Height - 2);
                tileToCheck = GetTileFromCoordinates(rngX, rngY);
                hasLaterallyAdjacentHallways = GetTileFromCoordinates(rngX - 1, rngY).Type == TileType.Hallway
                                            || GetTileFromCoordinates(rngX + 1, rngY).Type == TileType.Hallway
                                            || GetTileFromCoordinates(rngX, rngY - 1).Type == TileType.Hallway
                                            || GetTileFromCoordinates(rngX, rngY + 1).Type == TileType.Hallway;
            }
            while (!tileToCheck.IsWalkable || tileToCheck.Character != null || hasLaterallyAdjacentHallways);
            return new Point(rngX, rngY);
        }

        #endregion

        #region Finding things in coordinates

        private void GetPossibleRoomData()
        {
            MaxRoomWidth = Width / RoomCountColumns;
            if (MaxRoomWidth < Constants.MinRoomWidthOrHeight)
                throw new Exception("Combination of floor Width and Columns prevents generating 5x5 rooms (the bare minimum)");
            MinRoomWidth = Math.Max(MaxRoomWidth / 4, Constants.MinRoomWidthOrHeight);
            MaxRoomHeight = Height / RoomCountRows;
            if (MaxRoomHeight < Constants.MinRoomWidthOrHeight)
                throw new Exception("Combination of floor Height and Rows prevents generating 5x5 rooms (the bare minimum)");
            MinRoomHeight = Math.Max(MaxRoomHeight / 4, Constants.MinRoomWidthOrHeight);
            var widthGap = (Width - MaxRoomWidth * RoomCountColumns) / 2;
            var heightGap = (Height - MaxRoomHeight * RoomCountRows) / 2;
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
                        X = minX + MaxRoomWidth * j,
                        Y = minY + MaxRoomHeight * i
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

        private List<Entity> GetEntitiesFromCoordinates(int x, int y)
        {
            return Entities.Where(t => t != null && t.Position != null && t.Position.X == x && t.Position.Y == y).ToList();
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
            var tile = GetTileFromCoordinates(x, y);
            if (tile == null) throw new Exception("Tile does not exist");
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
            var visibleEntitiesInCoordinates = GetEntitiesFromCoordinates(x, y).Where(e => Player.CanSee(e)).ToList();
            var solidEntityInCoordinates = visibleEntitiesInCoordinates.Find(e => !e.Passable);
            if (solidEntityInCoordinates?.ExistenceStatus == EntityExistenceStatus.Alive)
                return solidEntityInCoordinates.ConsoleRepresentation;
            if (tile.ConsoleRepresentation.Character == (char)TileType.Stairs)
                return tileBaseConsoleRepresentation;
            var passableEntityInCoordinates = visibleEntitiesInCoordinates.Find(e => e.Passable && e.ExistenceStatus == EntityExistenceStatus.Alive);
            if (passableEntityInCoordinates != null)
                return passableEntityInCoordinates.ConsoleRepresentation;
            var deadEntityInCoordinates = visibleEntitiesInCoordinates.Find(e => e.Passable && e.ExistenceStatus == EntityExistenceStatus.Dead);
            if (deadEntityInCoordinates != null)
                return deadEntityInCoordinates.ConsoleRepresentation;
            return tileBaseConsoleRepresentation;
        }

        #endregion

        #region Room Connections
        private RoomConnectionType?[,] SetPossibleRoomConnections(bool removeRandomEdges)
        {
            int normalRoomCount = 0, connectionCount = 0, minimumConnections;
            var temporaryRoomConnectionMatrix = new RoomConnectionType?[RoomCount, RoomCount];

            normalRoomCount = Rooms.Count(r => r.Width > 1 && r.Height > 1);
            minimumConnections = normalRoomCount - 1;

            // Create default graph of rooms where all adjacencies are connected

            var roomsAsList = Rooms.ToList();

            for (int i = 0; i < roomsAsList.Count; i++)
            {
                temporaryRoomConnectionMatrix[i, i] = null;
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
                var testMatrix = new RoomConnectionType?[RoomCount, RoomCount];
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
                        if (testMatrix[x, y] != null && Rng.NextInclusive(1, 100).Between(1, 30))
                        {
                            testMatrix[x, y] = null;
                            if (testMatrix.IsFullyConnectedAdjacencyMatrix(r => r != null))
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

                    Room fusedRoom = null;

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
                        for (int i = 0; i < Rooms.GetLength(0); i++)
                        {
                            for (int j = 0; j < Rooms.GetLength(1); j++)
                            {
                                if (roomsToFuseWithThis.Contains(Rooms[i, j]))
                                    Rooms[i, j] = fusedRoom;
                            }
                        }
                        Rooms[row, column] = fusedRoom;
                    }

                }
            }
        }

        #endregion

        #region Graph methods

        private List<(Room Room, RoomConnectionType ConnectionType)> GetConnectionsForRoom(Room room)
        {
            var adjacentRooms = new List<(Room Room, RoomConnectionType ConnectionType)>();
            var roomsAsList = Rooms.ToList();
            var indexOfRoom = roomsAsList.IndexOf(room);

            for (int i = 0; i < RoomCount; i++)
            {
                if (RoomAdjacencyMatrix[indexOfRoom, i] != null)
                    adjacentRooms.Add((roomsAsList[i], RoomAdjacencyMatrix[indexOfRoom, i].Value));
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
                return distance + 8;
            var entitiesInTile = GetEntitiesFromCoordinates(new Point(x2, y2));
            // Discourage but not prohibit walking on visible traps
            if (entitiesInTile.Any(e => e.EntityType == EntityType.Trap && e.Visible))
                return distance + 8;
            return distance;
        }

        public List<Tile> GetAdjacentTiles(Point position)
        {
            return Tiles.GetAdjacentElements(GetTileFromCoordinates(position), true);
        }
        public List<Tile> GetFOVTilesWithinDistance(Point source, int distance)
        {
            var tilesWithinDistance = Tiles.Where(t => t.Type != TileType.Empty && Math.Round(Point.Distance(source, t.Position), 0, MidpointRounding.AwayFromZero) <= distance);
            var visibleTiles = new List<Tile>();
            foreach (var tile in tilesWithinDistance)
            {
                visibleTiles.AddRange(GetFOVLineBetweenTiles(GetTileFromCoordinates(source.X, source.Y), tile));
            }
            return visibleTiles.Distinct().ToList();
        }
        public List<Tile> GetFOVLineBetweenTiles(Tile source, Tile target)
        {
            var componentPositions = new List<Point>();
            var sourcePosition = source.Position;
            var targetPosition = target.Position;
            componentPositions.AddRange(MathAlgorithms.BresenhamLine(sourcePosition, targetPosition, p => p.X, p => p.Y, (x, y) => new Point(x, y), p => GetTileFromCoordinates(p).IsWalkable).ToList());

            if (!componentPositions.Any(p => p.Equals(sourcePosition)))
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
                        var room = Rooms[x, y];
                        if (room == null) continue;
                        foreach (var roomConnection in GetConnectionsForRoom(room))
                        {
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

            if (edge.Tag == RoomConnectionType.Horizontal)
            {
                CreateHorizontalHallway(roomA, roomB);
            }
            else if (edge.Tag == RoomConnectionType.Vertical)
            {
                CreateVerticalHallway(roomA, roomB);
            }
        }

        private void CreateHorizontalHallway(Room roomA, Room roomB)
        {
            Room leftRoom, rightRoom;
            Tile? leftConnector = null, rightConnector = null;

            if (roomA.RoomColumn < roomB.RoomColumn)
            {
                leftRoom = roomA;
                rightRoom = roomB;
            }
            else if (roomA.RoomColumn > roomB.RoomColumn)
            {
                leftRoom = roomB;
                rightRoom = roomA;
            }
            else
            {
                return;
            }

            if (leftRoom.Width == 1 && leftRoom.Height == 1)
                leftConnector = GetTileFromCoordinates(leftRoom.Position.X, leftRoom.Position.Y);
            if (rightRoom.Width == 1 && rightRoom.Height == 1)
                rightConnector = GetTileFromCoordinates(rightRoom.Position.X, rightRoom.Position.Y);

            if (leftConnector == null)
            {
                var x = leftRoom.Position.X + leftRoom.Width - 1;
                var y = Rng.NextInclusive(leftRoom.Position.Y + 1, leftRoom.Position.Y + leftRoom.Height - 2);
                leftConnector = GetTileFromCoordinates(x, y);
            }

            if (rightConnector == null)
            {
                var x = rightRoom.Position.X;
                var y = Rng.NextInclusive(rightRoom.Position.Y + 1, rightRoom.Position.Y + rightRoom.Height - 2);
                rightConnector = GetTileFromCoordinates(x, y);
            }

            var hallwayGenerationTries = 0;
            List<Tile> tilesToConvert;
            bool isValidHallway;

            do
            {
                hallwayGenerationTries++;
                tilesToConvert = new();
                isValidHallway = true;

                Point connectionPosition;

                if (Math.Abs(leftConnector.Position.X - rightConnector.Position.X) > 1)
                {
                    var minX = leftConnector.Position.X + 1;
                    var maxX = rightConnector.Position.X - 1;
                    var minY = Math.Min(leftConnector.Position.Y, rightConnector.Position.Y);
                    var maxY = Math.Max(leftConnector.Position.Y, rightConnector.Position.Y);

                    try
                    {
                        connectionPosition = new Point
                        {
                            X = Rng.NextInclusive(minX, maxX),
                            Y = Rng.NextInclusive(minY, maxY)
                        };
                    }
                    catch { return; }

                    var connectorTile = GetTileFromCoordinates(connectionPosition.X, connectionPosition.Y);

                    // Horizontal line from Left Room to Hallway connection column
                    for (var i = leftConnector.Position.X; i <= connectorTile.Position.X; i++)
                    {
                        tilesToConvert.Add(GetTileFromCoordinates(i, leftConnector.Position.Y));
                    }

                    // Horizontal line from Right Room to Hallway connection column
                    for (var i = connectorTile.Position.X; i <= rightConnector.Position.X; i++)
                    {
                        tilesToConvert.Add(GetTileFromCoordinates(i, rightConnector.Position.Y));
                    }

                    // Draw a downwards line in case entrypoint from left is higher or equal to entrypoint from right
                    for (var i = rightConnector.Position.Y + 1; i < leftConnector.Position.Y; i++)
                    {
                        tilesToConvert.Add(GetTileFromCoordinates(connectorTile.Position.X, i));
                    }
                    // Draw an upwards line in case entrypoint from left is lower than entrypoint from right
                    for (var i = leftConnector.Position.Y + 1; i < rightConnector.Position.Y; i++)
                    {
                        tilesToConvert.Add(GetTileFromCoordinates(connectorTile.Position.X, i));
                    }
                }
                else
                {
                    var leftYs = Enumerable.Range(leftRoom.Position.Y + 1, Math.Max(1, leftRoom.Height - 2)).ToList();
                    var rightYs = Enumerable.Range(rightRoom.Position.Y + 1, Math.Max(1, rightRoom.Height - 2)).ToList();
                    var commonYs = leftYs.Intersect(rightYs);
                    if (!commonYs.Any()) return;
                    var pickedY = commonYs.TakeRandomElement(Rng);
                    leftConnector = GetTileFromCoordinates(leftConnector.Position.X, pickedY);
                    rightConnector = GetTileFromCoordinates(rightConnector.Position.X, pickedY);
                    tilesToConvert.Add(leftConnector);
                    tilesToConvert.Add(rightConnector);
                }
                foreach (var tile in tilesToConvert)
                {
                    if ((tile != leftConnector && tile != rightConnector && tile.Type == TileType.Wall)
                        || tile.Type == TileType.Floor || tile.Type == TileType.Stairs)
                    {
                        isValidHallway = false;
                        break;
                    }
                }
            }
            while (!isValidHallway && hallwayGenerationTries < Constants.MaxGenerationTriesForHallway);
            if (isValidHallway)
            {
                foreach (var tile in tilesToConvert)
                {
                    tile.Type = TileType.Hallway;
                    if (tile == leftConnector || tile == rightConnector)
                        tile.IsConnectorTile = true;
                }
            }
        }

        private void CreateVerticalHallway(Room roomA, Room roomB)
        {
            Room topRoom, downRoom;
            Tile? topConnector = null, downConnector = null;

            if (roomA.RoomRow < roomB.RoomRow)
            {
                topRoom = roomA;
                downRoom = roomB;
            }
            else if (roomA.RoomRow > roomB.RoomRow)
            {
                topRoom = roomB;
                downRoom = roomA;
            }
            else
            {
                return;
            }

            if (topRoom.Width == 1 && topRoom.Height == 1)
                topConnector = GetTileFromCoordinates(topRoom.Position.X, topRoom.Position.Y);
            if (downRoom.Width == 1 && downRoom.Height == 1)
                downConnector = GetTileFromCoordinates(downRoom.Position.X, downRoom.Position.Y);

            if (topConnector == null)
            {
                var x = Rng.NextInclusive(topRoom.Position.X + 1, topRoom.Position.X + topRoom.Width - 2);
                var y = topRoom.Position.Y + topRoom.Height - 1;
                topConnector = GetTileFromCoordinates(x, y);
            }

            if (downConnector == null)
            {
                var x = Rng.NextInclusive(downRoom.Position.X + 1, downRoom.Position.X + downRoom.Width - 2);
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
                isValidHallway = true;

                Point connectionPosition;

                if (Math.Abs(topConnector.Position.Y - downConnector.Position.Y) > 1)
                {
                    var minY = topConnector.Position.Y + 1;
                    var maxY = downConnector.Position.Y - 1;
                    var minX = Math.Min(topConnector.Position.X, downConnector.Position.X);
                    var maxX = Math.Max(topConnector.Position.X, downConnector.Position.X);

                    try
                    {
                        connectionPosition = new Point
                        {
                            X = Rng.NextInclusive(minX, maxX),
                            Y = Rng.NextInclusive(minY, maxY)
                        };
                    }
                    catch { return; }

                    var connectorTile = GetTileFromCoordinates(connectionPosition.X, connectionPosition.Y);

                    // Vertical line from Up Room to Hallway connection row
                    for (var i = topConnector.Position.Y; i <= connectorTile.Position.Y; i++)
                    {
                        tilesToConvert.Add(GetTileFromCoordinates(topConnector.Position.X, i));
                    }

                    // Vertical line from Down Room to Hallway connection row
                    for (var i = connectorTile.Position.Y; i <= downConnector.Position.Y; i++)
                    {
                        tilesToConvert.Add(GetTileFromCoordinates(downConnector.Position.X, i));
                    }

                    // Draw a rightwards line in case entrypoint from up is more or equal to the right to entrypoint from below
                    for (var i = downConnector.Position.X + 1; i < topConnector.Position.X; i++)
                    {
                        tilesToConvert.Add(GetTileFromCoordinates(i, connectorTile.Position.Y));
                    }
                    // Draw a leftwards line in case entrypoint from up is more to the left than entrypoint from below
                    for (var i = topConnector.Position.X + 1; i < downConnector.Position.X; i++)
                    {
                        tilesToConvert.Add(GetTileFromCoordinates(i, connectorTile.Position.Y));
                    }
                }
                else
                {
                    var topXs = Enumerable.Range(topRoom.Position.X + 1, Math.Max(1, topRoom.Width - 2)).ToList();
                    var downXs = Enumerable.Range(downRoom.Position.X + 1, Math.Max(1, downRoom.Width - 2)).ToList();
                    var commonXs = topXs.Intersect(downXs);
                    if (!commonXs.Any()) return;
                    var pickedX = commonXs.TakeRandomElement(Rng);
                    topConnector = GetTileFromCoordinates(pickedX, topConnector.Position.Y);
                    downConnector = GetTileFromCoordinates(pickedX, downConnector.Position.Y);
                    tilesToConvert.Add(topConnector);
                    tilesToConvert.Add(downConnector);
                }
                foreach (var tile in tilesToConvert)
                {
                    if (tile != topConnector && tile != downConnector && tile.Type == TileType.Wall
                        || tile.Type == TileType.Floor || tile.Type == TileType.Stairs)
                    {
                        isValidHallway = false;
                        break;
                    }
                }
            }
            while (!isValidHallway && hallwayGenerationTries < Constants.MaxGenerationTriesForHallway);
            if (isValidHallway)
            {
                foreach (var tile in tilesToConvert)
                {
                    tile.Type = TileType.Hallway;
                    if (tile == topConnector || tile == downConnector)
                        tile.IsConnectorTile = true;
                }
            }
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
}
