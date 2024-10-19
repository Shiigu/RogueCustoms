using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils;
using RogueCustomsGameEngine.Utils.Effects;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.Helpers;
using GamePoint = RogueCustomsGameEngine.Utils.Representation.GamePoint;
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
using System.Text.Json;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using org.matheval.Functions;
using org.matheval.Node;
using RogueCustomsGameEngine.Utils.Exceptions;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using RogueCustomsGameEngine.Utils.Expressions;

namespace RogueCustomsGameEngine.Game.DungeonStructure
{
#pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
#pragma warning disable CS8601 // Posible asignación de referencia nula
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
#pragma warning disable CS8604 // Posible argumento de referencia nulo
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
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

        private FloorLayoutGenerator GeneratorToUse;

        private readonly FloorLayoutGenerator DefaultGeneratorToUse = new FloorLayoutGenerator
        {
            Columns = 1,
            Rows = 1,
            MinRoomSize = new() { Height = 5, Width = 5 },
            RoomDisposition = new RoomDispositionType[1, 1]
        };

        private RoomDispositionType[,] RoomDispositionToUse;
        public TileSet TileSet => FloorConfigurationToUse.TileSet;

        public string FloorName => Locale["FloorName"].Format(new {
                                                                DungeonName = Dungeon.Name.ToUpperInvariant(),
                                                                FloorLevel = FloorLevel.ToString()
                                                                });

        private int RoomCountRows => GeneratorToUse.Rows;
        private int RoomCountColumns => GeneratorToUse.Columns;
        public int MaxConnectionsBetweenRooms => FloorConfigurationToUse.MaxConnectionsBetweenRooms;
        public int OddsForExtraConnections => FloorConfigurationToUse.OddsForExtraConnections;
        public int RoomFusionOdds => FloorConfigurationToUse.RoomFusionOdds;
        public decimal HungerDegeneration => FloorConfigurationToUse.HungerDegeneration;

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
        public List<SpecialEffect> SpecialEffectsThatHappened { get; set; }
        public List<NonPlayableCharacter> AICharacters { get; set; }
        public List<Item> Items { get; set; }
        public List<Key> Keys { get; set; }
        public List<Tile> Doors => Tiles.Where(t => t.Type == TileType.Door);
        public List<Trap> Traps { get; set; }

        public List<AlteredStatus> PossibleStatuses { get; private set; }

        public RngHandler Rng { get; private set; }
        public (GamePoint TopLeftCorner, GamePoint BottomRightCorner)[,] RoomLimitsTable { get; set; }
        private List<(Room RoomA, Room RoomB, Tile ConnectorA, Tile ConnectorB, List<Tile> Tiles)> Hallways { get; set; }
        private List<(Room RoomA, Room RoomB, Room FusedRoom)> Fusions { get; set; }

        public Tile[,] Tiles { get; private set; }
        public Tile StairsTile => Tiles.Find(t => t.Type == TileType.Stairs);

        public List<Room> Rooms { get; private set; }

        public List<Flag> Flags { get; set; }

        public List<EntityClass> PossibleNPCClasses => Dungeon.Classes.Where(c => c.EntityType == EntityType.NPC).ToList();
        public List<EntityClass> PossibleItemClasses => Dungeon.Classes.Where(c => c.EntityType == EntityType.Weapon || c.EntityType == EntityType.Armor || c.EntityType == EntityType.Consumable).ToList();
        public List<EntityClass> PossibleTrapClasses => Dungeon.Classes.Where(c => c.EntityType == EntityType.Trap).ToList();

        public List<TileType> TileTypes { get; private set; }

        public List<TileType> DefaultTileTypes = new() { TileType.Empty, TileType.Floor, TileType.Hallway, TileType.Stairs, TileType.Wall, TileType.Door };

        #endregion

        public Map(Dungeon dungeon, int floorLevel, List<Flag> flags)
        {
            Dungeon = dungeon;
            FloorLevel = floorLevel;
            AICharacters = new List<NonPlayableCharacter>();
            Items = new List<Item>();
            Keys = new List<Key>();
            Traps = new List<Trap>();
            TileTypes = dungeon.TileTypes;
            FloorConfigurationToUse = Dungeon.FloorTypes.Find(ft => floorLevel.Between(ft.MinFloorLevel, ft.MaxFloorLevel));
            if (FloorConfigurationToUse == null)
                throw new InvalidDataException("There's no valid configuration for the current floor");
            TileSet.TileTypeSets.ForEach(tts => tts.TileType.TileTypeSet = tts);
            Width = FloorConfigurationToUse.Width;
            Height = FloorConfigurationToUse.Height;
            Rng = new RngHandler(Environment.TickCount);
            Flags = flags;
            if (!FloorConfigurationToUse.PossibleLayouts.Any())
                throw new InvalidDataException("There's no valid Floor Layout for the current floor");
            ConsoleRepresentation.EmptyTile = TileTypes.FirstOrDefault(tt => tt.Name.Equals("Empty"))?.TileTypeSet?.Central
                                                ?? new() { BackgroundColor = new(Color.Black), ForegroundColor = new (Color.Black), Character = ' ' };
            GeneratorToUse = FloorConfigurationToUse.PossibleLayouts.TakeRandomElement(Rng);
            DefaultGeneratorToUse.MaxRoomSize = new() { Width = Width, Height = Height };
            DefaultGeneratorToUse.RoomDisposition[0, 0] = RoomDispositionType.GuaranteedRoom;
            SpecialEffectsThatHappened = new();
            PossibleStatuses = new List<AlteredStatus>();
            Dungeon.Classes.Where(c => c.EntityType == EntityType.AlteredStatus).ForEach(alsc => PossibleStatuses.Add(new AlteredStatus(alsc, this)));
            SetActionParams();
        }

        public void SetActionParams()
        {
            AttackActions.SetActionParams(Rng, this);
            CharacterActions.SetActionParams(Rng, this);
            ItemActions.SetActionParams(Rng, this);
            GenericActions.SetActionParams(Rng, this);
            OnTileActions.SetActionParams(Rng, this);
            ExpressionParser.Setup(Rng, this);
        }

        public void LoadRngState(int seed)
        {
            Rng = new RngHandler(seed);
        }
        public void GenerateDebugMap()
        {
            _generationTries = 0;
            Width = 32;
            Height = 16;
            ResetAndCreateTiles();
            var room = new Room(this, new GamePoint(0, 0), 0, 0, 25, 10);
            Rooms = new List<Room> { room };
            room.CreateTiles();
            AddEntity(Dungeon.PlayerClass.Id);
            Player.Position = new GamePoint(5, 3);
            SetStairs(new GamePoint(19, 7));
            AppendMessage("This is a dummy map");
        }

        public void Generate()
        {
            _generationTries = 0;
            bool success;
            do
            {
                success = false;
                _generationTries++;

                if(RoomDispositionToUse == null || _generationTries % 100 == 0)
                {
                    var possibleRoomDisposition = RollRoomDistributionToUse();

                    if (!possibleRoomDisposition.IsFullyConnected(d => d != RoomDispositionType.NoRoom && d != RoomDispositionType.NoConnection && d != RoomDispositionType.ConnectionImpossible)) continue;

                    RoomDispositionToUse = possibleRoomDisposition;
                }

                Hallways = new();
                Fusions = new();

                ResetAndCreateTiles();
                CreateRooms();
                FuseRooms();
                Rooms = Rooms.Distinct().ToList();
                if (Rooms.Count(r => !r.IsDummy) > 1)
                    ConnectRooms();
                Parallel.ForEach(Rooms, r => r.CreateTiles());
                if ((Rooms.Count == 1 && !Rooms.Any(r => r.IsDummy)) || Rooms.Count(r => !r.IsDummy) > 1)
                {
                    success = Tiles.IsFullyConnected(t => t.IsWalkable);
                    if (success)
                    {
                        PlaceSpecialTiles();
                        success = Tiles.IsFullyConnected(t => t.IsWalkable);
                        if (success)
                        {
                            PlacePlayer();
                            success = Player.Position != null;
                            if (success)
                            {
                                if (FloorConfigurationToUse.GenerateStairsOnStart)
                                    SetStairs();
                                AppendMessage(Locale["FloorEnter"].Format(new { FloorLevel = FloorLevel.ToString() }), Color.Yellow);
                            }
                        }
                    }
                }
            }
            while (!success && _generationTries < EngineConstants.MaxGenerationTries);
            if (!success)
            {
                success = false;
                GeneratorToUse = DefaultGeneratorToUse;
                RoomDispositionToUse = GeneratorToUse.RoomDisposition;

                Hallways = new();
                Fusions = new();

                ResetAndCreateTiles();
                CreateRooms();
                FuseRooms();
                Parallel.ForEach(Rooms, r => r.CreateTiles());
                ConnectRooms();
                if (Rooms.Count == 1 || Rooms.Count(r => !r.IsDummy) != 1)
                {
                    success = Tiles.IsFullyConnected(t => t.IsWalkable);
                    if (success)
                    {
                        PlaceSpecialTiles();
                        success = Tiles.IsFullyConnected(t => t.IsWalkable);
                        if (success)
                        {
                            PlacePlayer();
                            success = Player.Position != null;
                            if (success)
                            {
                                if (FloorConfigurationToUse.GenerateStairsOnStart)
                                    SetStairs();
                                AppendMessage(Locale["FloorEnter"].Format(new { FloorLevel = FloorLevel.ToString() }), Color.Yellow);
                            }
                        }
                    }
                }
            }

            NewTurn();

            if(Rooms.Count(r => !r.IsDummy) > 1 && FloorConfigurationToUse.PossibleKeys?.KeyTypes != null && FloorConfigurationToUse.PossibleKeys.KeyTypes.Any())
            {
                do
                {
                    _generationTries++;
                    PlaceKeysAndDoors();
                    success = IsFullyConnectedWithKeys();
                    if (success)
                    {
                        foreach (var door in Doors)
                        {
                            try
                            {
                                var existingValue = GetFlagValue($"Doors_{door.DoorId}");
                                SetFlagValue($"Doors_{door.DoorId}", existingValue + 1);
                            }
                            catch (FlagNotFoundException)
                            {
                                CreateFlag($"Doors_{door.DoorId}", 1, true);
                            }
                        }
                    }
                }
                while (!success && _generationTries < EngineConstants.MaxGenerationTries);
                if (!success)
                {
                    foreach (var key in Keys)
                    {
                        key.ExistenceStatus = EntityExistenceStatus.Gone;
                        key.Owner = null;
                        key.Position = null;
                    }
                    Keys.Clear();
                    GetEntities().RemoveAll(e => e.EntityType == EntityType.Key);
                    Tiles.Where(t => t.Type == TileType.Door).ForEach(t =>
                    {
                        t.Type = TileType.Hallway;
                        t.DoorId = string.Empty;
                    });
                }
            }
        }

        public (bool MapGenerationSuccess, bool KeyGenerationSuccess) DebugGenerate()
        {
            _generationTries = 0;
            bool success;
            do
            {
                success = false;
                _generationTries++;

                if (RoomDispositionToUse == null || _generationTries % 100 == 0)
                {
                    var possibleRoomDisposition = RollRoomDistributionToUse();

                    if (!possibleRoomDisposition.IsFullyConnected(d => d != RoomDispositionType.NoRoom && d != RoomDispositionType.NoConnection && d != RoomDispositionType.ConnectionImpossible)) continue;

                    RoomDispositionToUse = possibleRoomDisposition;
                }

                Hallways = new();
                Fusions = new();

                ResetAndCreateTiles();
                CreateRooms();
                FuseRooms();
                Rooms = Rooms.Distinct().ToList();
                if (Rooms.Count(r => !r.IsDummy) > 1)
                    ConnectRooms();
                Parallel.ForEach(Rooms, r => r.CreateTiles());
                if ((Rooms.Count == 1 && !Rooms.Any(r => r.IsDummy)) || Rooms.Count(r => !r.IsDummy) > 1)
                {
                    success = Tiles.IsFullyConnected(t => t.IsWalkable);
                    if(success)
                    {
                        PlaceSpecialTiles();
                        success = Tiles.IsFullyConnected(t => t.IsWalkable);
                        if (success)
                        {
                            PlacePlayer();
                            success = Player.Position != null;
                        }
                    }
                }
            }
            while (!success && _generationTries < EngineConstants.MaxGenerationTries);
            if (!success)
            {
                success = false;
                GeneratorToUse = DefaultGeneratorToUse;
                RoomDispositionToUse = DefaultGeneratorToUse.RoomDisposition;

                Hallways = new();
                Fusions = new();

                ResetAndCreateTiles();
                CreateRooms();
                FuseRooms();
                Parallel.ForEach(Rooms, r => r.CreateTiles());
                ConnectRooms();
                if (Rooms.Count == 1 || Rooms.Count(r => !r.IsDummy) != 1)
                {
                    success = Tiles.IsFullyConnected(t => t.IsWalkable);
                    if (success)
                    {
                        PlaceSpecialTiles();
                        success = Tiles.IsFullyConnected(t => t.IsWalkable);
                        if (success)
                        {
                            PlacePlayer();
                            success = Player.Position != null;
                        }
                    }
                }
                success = false;
            }

            var mapGenerationSuccess = success;
            var keyGenerationSuccess = success;

            if (Rooms.Count(r => !r.IsDummy) > 1 && FloorConfigurationToUse.PossibleKeys?.KeyTypes != null && FloorConfigurationToUse.PossibleKeys.KeyTypes.Any())
            {
                do
                {
                    _generationTries++;
                    PlaceKeysAndDoors();
                    success = IsFullyConnectedWithKeys();
                    if (success)
                    {
                        foreach (var door in Doors)
                        {
                            try
                            {
                                var existingValue = GetFlagValue($"Doors_{door.DoorId}");
                                SetFlagValue($"Doors_{door.DoorId}", existingValue + 1);
                            }
                            catch (FlagNotFoundException)
                            {
                                CreateFlag($"Doors_{door.DoorId}", 1, true);
                            }
                        }
                    }
                }
                while (!success && _generationTries < EngineConstants.MaxGenerationTries);
                keyGenerationSuccess = success;
                if (!success)
                {
                    foreach (var key in Keys)
                    {
                        key.ExistenceStatus = EntityExistenceStatus.Gone;
                        key.Owner = null;
                        key.Position = null;
                    }
                    Keys.Clear();
                    GetEntities().RemoveAll(e => e.EntityType == EntityType.Key);
                    Tiles.Where(t => t.Type == TileType.Door).ForEach(t =>
                    {
                        t.Type = TileType.Hallway;
                        t.DoorId = string.Empty;
                    });
                }
            }
            return (mapGenerationSuccess, keyGenerationSuccess);
        }

        private RoomDispositionType[,] RollRoomDistributionToUse()
        {
            var rolledRoomDistribution = GeneratorToUse.RoomDisposition.Copy();

            var totalRandomRooms = rolledRoomDistribution.Where(rd => rd == RoomDispositionType.RandomRoom).Count;
            var randomRoomsRemoved = 0;
            var maximumRandomRoomsToRemove = Math.Max(1, totalRandomRooms / (3 + (_generationTries * 2) / EngineConstants.MaxGenerationTries));
            var totalRandomConnections = rolledRoomDistribution.Where(rd => rd == RoomDispositionType.RandomConnection).Count;
            var randomConnectionsRemoved = 0;
            var maximumRandomConnectionsToRemove = Math.Max(1, totalRandomConnections / (3 + (_generationTries * 2) / EngineConstants.MaxGenerationTries));

            for (var row = 0; row < rolledRoomDistribution.GetLength(0); row++)
            {
                for (var column = 0; column < rolledRoomDistribution.GetLength(1); column++)
                {
                    var generatorTile = rolledRoomDistribution[row, column];
                    var random = Rng.RollProbability();
                    if (generatorTile == RoomDispositionType.RandomRoom)
                    {
                        if (random <= 70)
                            rolledRoomDistribution[row, column] = RoomDispositionType.GuaranteedRoom;
                        else if (random <= 85)
                            rolledRoomDistribution[row, column] = RoomDispositionType.GuaranteedDummyRoom;
                        else if (randomRoomsRemoved < maximumRandomRoomsToRemove)
                        {
                            rolledRoomDistribution[row, column] = RoomDispositionType.NoRoom;
                            randomRoomsRemoved++;
                        }
                    }
                    else if (generatorTile == RoomDispositionType.RandomConnection)
                    {
                        if (randomConnectionsRemoved < maximumRandomConnectionsToRemove && random < 30)
                        {
                            rolledRoomDistribution[row, column] = RoomDispositionType.NoConnection;
                            randomConnectionsRemoved++;
                        }
                        else if(random < 100 - RoomFusionOdds)
                        {
                            rolledRoomDistribution[row, column] = RoomDispositionType.GuaranteedHallway;
                        }
                    }
                }
            }

            return rolledRoomDistribution;
        }

        private void CreateRooms()
        {
            var validRoomTileTypes = new List<RoomDispositionType>() { RoomDispositionType.GuaranteedRoom, RoomDispositionType.GuaranteedDummyRoom, RoomDispositionType.RandomRoom };

            GetPossibleRoomData();
            Rooms = new List<Room>();

            for (var row = 0; row < RoomDispositionToUse.GetLength(0); row++)
            {
                for (var column = 0; column < RoomDispositionToUse.GetLength(1); column++)
                {
                    var roomTile = RoomDispositionToUse[row, column];
                    if (!validRoomTileTypes.Contains(roomTile)) continue;
                    var roomRow = row / 2;
                    var roomColumn = column / 2;

                    var (MinX, MinY, MaxX, MaxY) = GetPossibleCoordinatesForRoom(roomRow, roomColumn);

                    var actualMaxX = MaxX - GeneratorToUse.MinRoomSize.Width;
                    var actualMaxY = MaxY - GeneratorToUse.MinRoomSize.Height;

                    if (roomTile == RoomDispositionType.GuaranteedRoom)
                    {
                        // Adjust room width and height ensuring they meet the min size requirements
                        var rngX1 = Rng.NextInclusive(MinX, actualMaxX);
                        var rngX2 = Rng.NextInclusive(rngX1 + GeneratorToUse.MinRoomSize.Width, MaxX);
                        var roomWidth = Math.Min(GeneratorToUse.MaxRoomSize.Width, rngX2 - rngX1 + 1);

                        var rngY1 = Rng.NextInclusive(MinY, actualMaxY);
                        var rngY2 = Rng.NextInclusive(rngY1 + GeneratorToUse.MinRoomSize.Height, MaxY);
                        var roomHeight = Math.Min(GeneratorToUse.MaxRoomSize.Height, rngY2 - rngY1 + 1);

                        Rooms.Add(new Room(this, new GamePoint(rngX1, rngY1), roomRow, roomColumn, roomWidth, roomHeight));                        
                    }
                    else if (roomTile == RoomDispositionType.GuaranteedDummyRoom)
                    {
                        // Dummy rooms are 1x1
                        var rngX = Rng.NextInclusive(MinX + 1, MaxX - 1);
                        var rngY = Rng.NextInclusive(MinY + 1, MaxY - 1);
                        Rooms.Add(new Room(this, new GamePoint(rngX, rngY), roomRow, roomColumn, 1, 1));
                    }
                }
            }
        }

        private void FuseRooms()
        {
            var validFusionTileTypes = new List<RoomDispositionType>() { RoomDispositionType.GuaranteedFusion, RoomDispositionType.RandomConnection };

            for (var row = 0; row < RoomDispositionToUse.GetLength(0); row++)
            {
                for (var column = 0; column < RoomDispositionToUse.GetLength(1); column++)
                {
                    var connectionTile = RoomDispositionToUse[row, column];
                    if (!validFusionTileTypes.Contains(connectionTile)) continue;
                    var (RoomRoow, RoomColumn) = (row / 2, column / 2);
                    var leftRoom = GetRoomByRowAndColumn(row / 2, (column - 1) / 2);
                    var rightRoom = GetRoomByRowAndColumn(row / 2, (column + 1) / 2);
                    var upRoom = GetRoomByRowAndColumn((row - 1) / 2, column / 2);
                    var downRoom = GetRoomByRowAndColumn((row + 1) / 2, column / 2);
                    var isVerticalConnection = column % 2 == 0 && row % 2 != 0;
                    var isHorizontalConnection = column % 2 != 0 && row % 2 == 0;
                    var isHorizontalConnectionValid = isHorizontalConnection && leftRoom != null && !leftRoom.IsDummy && !leftRoom.IsFused && rightRoom != null && !rightRoom.IsDummy && !rightRoom.IsFused;
                    var isVerticalConnectionValid = isVerticalConnection && upRoom != null && !upRoom.IsDummy && !upRoom.IsFused && downRoom != null && !downRoom.IsDummy && !downRoom.IsFused;
                    if (connectionTile == RoomDispositionType.RandomConnection)
                    {
                        if (Rng.RollProbability() < RoomFusionOdds && (isHorizontalConnectionValid || isVerticalConnectionValid))
                            connectionTile = RoomDispositionType.GuaranteedFusion;
                        else
                            connectionTile = RoomDispositionType.GuaranteedHallway;
                    }
                    if (connectionTile == RoomDispositionType.GuaranteedFusion)
                    {
                        Room? fusedRoom;
                        if (isHorizontalConnectionValid)
                        {
                            fusedRoom = FuseRooms(leftRoom, rightRoom);
                            if (fusedRoom != null)
                            {
                                Fusions.Add((leftRoom, rightRoom, fusedRoom));
                                Rooms[Rooms.IndexOf(leftRoom)] = Rooms[Rooms.IndexOf(rightRoom)] = fusedRoom;
                                leftRoom = rightRoom = fusedRoom;
                            }
                        }
                        else if (isVerticalConnectionValid)
                        {
                            fusedRoom = FuseRooms(downRoom, upRoom);
                            if (fusedRoom != null)
                            {
                                Fusions.Add((upRoom, downRoom, fusedRoom));
                                Rooms[Rooms.IndexOf(upRoom)] = Rooms[Rooms.IndexOf(downRoom)] = fusedRoom;
                                downRoom = upRoom = fusedRoom;
                            }
                        }
                        else
                            connectionTile = validFusionTileTypes.TakeRandomElement(Rng);
                    }
                }
            }
        }
        private void ConnectRooms()
        {
            var validHallwayTileTypes = new List<RoomDispositionType>() { RoomDispositionType.GuaranteedFusion, RoomDispositionType.GuaranteedHallway, RoomDispositionType.RandomConnection };

            for (var row = 0; row < RoomDispositionToUse.GetLength(0); row++)
            {
                for (var column = 0; column < RoomDispositionToUse.GetLength(1); column++)
                {
                    var connectionTile = RoomDispositionToUse[row, column];
                    if (!validHallwayTileTypes.Contains(connectionTile)) continue;
                    var leftRoom = GetRoomOrFusionByRowAndColumn(row / 2, (column - 1) / 2);
                    var rightRoom = GetRoomOrFusionByRowAndColumn(row / 2, (column + 1) / 2);
                    var upRoom = GetRoomOrFusionByRowAndColumn((row - 1) / 2, column / 2);
                    var downRoom = GetRoomOrFusionByRowAndColumn((row + 1) / 2, column / 2);
                    var isVerticalConnection = column % 2 == 0 && row % 2 != 0;
                    var isHorizontalConnection = column % 2 != 0 && row % 2 == 0;
                    if (connectionTile == RoomDispositionType.RandomConnection)
                        connectionTile = RoomDispositionType.GuaranteedHallway;
                    if (connectionTile == RoomDispositionType.GuaranteedHallway)
                    {
                        var maxConnections = MaxConnectionsBetweenRooms > 1 && Rng.RollProbability() < OddsForExtraConnections
                                             ? Rng.NextInclusive(2, MaxConnectionsBetweenRooms)
                                             : 1;
                        for (int i = 0; i < maxConnections; i++)
                        {
                            if (isHorizontalConnection && leftRoom != null && rightRoom != null)
                                CreateHallway((leftRoom, rightRoom, RoomConnectionType.Horizontal));
                            else if (isVerticalConnection && downRoom != null && upRoom != null)
                                CreateHallway((upRoom, downRoom, RoomConnectionType.Vertical));
                        }
                    }
                }
            }
        }

        private void PlaceSpecialTiles()
        {
            if (FloorConfigurationToUse.PossibleSpecialTiles == null || !FloorConfigurationToUse.PossibleSpecialTiles.Any()) return;
            foreach (var specialTileGenerator in FloorConfigurationToUse.PossibleSpecialTiles)
            {
                if(specialTileGenerator.GeneratorType == SpecialTileGenerationAlgorithm.Lake)
                {
                    var roomCellsToUse = RoomLimitsTable.TakeNDifferentRandomElements(Rng.NextInclusive(specialTileGenerator.MinSpecialTileGenerations, specialTileGenerator.MaxSpecialTileGenerations), Rng);
                    foreach (var roomCell in roomCellsToUse)
                    {
                        CreateLake((roomCell.TopLeftCorner, roomCell.BottomRightCorner), specialTileGenerator.TileType);
                    }
                }
                else
                {
                    for (int i = 0; i < Rng.NextInclusive(specialTileGenerator.MinSpecialTileGenerations, specialTileGenerator.MaxSpecialTileGenerations); i++)
                    {
                        var roomCellsToUse = RoomLimitsTable.TakeNDifferentRandomElements(2, Rng);
                        var cellA = roomCellsToUse[0];
                        var cellB = roomCellsToUse[1];
                        CreateRiver((cellA.TopLeftCorner, cellA.BottomRightCorner), (cellB.TopLeftCorner, cellB.BottomRightCorner), specialTileGenerator.TileType);
                    }
                }
            }
        }

        private void PlaceKeysAndDoors()
        {
            var keyGenerationData = FloorConfigurationToUse.PossibleKeys;
            if (!keyGenerationData.KeyTypes.Any() || keyGenerationData.MaxPercentageOfLockedCandidateRooms < 1) return;
            foreach (var doorTile in Tiles.Where(t => t.Type == TileType.Door))
            {
                doorTile.Type = TileType.Hallway;
                doorTile.DoorId = string.Empty;
            }
            var nonDummyRooms = Rooms.Where(r => !r.IsDummy).ToList().Shuffle(Rng);
            if (nonDummyRooms.Count == 1) return;
            var maximumLockableRooms = (int) Math.Round(nonDummyRooms.Count * ((decimal) keyGenerationData.MaxPercentageOfLockedCandidateRooms / 100), 0, MidpointRounding.AwayFromZero);
            var lockedRooms = 0;
            var usedKeyTypes = new List<KeyType>();
            Keys.Clear();
            foreach (var AICharacter in AICharacters)
            {
                var keysInInventory = AICharacter.Inventory.Where(i => i.EntityType == EntityType.Key);
                AICharacter.Inventory = AICharacter.Inventory.Except(keysInInventory).ToList();
                foreach (var key in keysInInventory)
                {
                    key.ExistenceStatus = EntityExistenceStatus.Gone;
                    key.Owner = null;
                    key.Position = null;
                }
            }
            foreach (var room in nonDummyRooms)
            {
                if (usedKeyTypes.Count >= keyGenerationData.KeyTypes.Count) break;
                if (lockedRooms >= maximumLockableRooms) break;
                if(!IsCandidateRoom(room)) continue;
                if (Rng.RollProbability() > keyGenerationData.LockedRoomOdds) continue;
                var exitTiles = room.GetTiles().Where(t => t.IsConnectorTile);
                var usableKeyTypes = keyGenerationData.KeyTypes.Where(kt => !usedKeyTypes.Contains(kt)
                && ((room.HasStairs && kt.CanLockStairs) || (room.HasItems && kt.CanLockItems)));
                if (!usableKeyTypes.Any()) continue;
                var keyTypeToUse = usableKeyTypes.TakeRandomElement(Rng);
                foreach (var tile in exitTiles)
                {
                    tile.Type = TileType.Door;
                    tile.DoorId = keyTypeToUse.KeyTypeName;
                }
                lockedRooms++;

                var islands = Tiles.GetIslands(t => t.IsWalkable || usedKeyTypes.Select(ukt => ukt.KeyTypeName).Contains(t.DoorId));
                var islandWithPlayer = islands.FirstOrDefault(i => i.Contains(Player.ContainingTile));

                if (AddEntity(keyTypeToUse.KeyClass) is Item keyEntity && Rng.RollProbability() <= keyGenerationData.KeySpawnInEnemyInventoryOdds)
                {
                    var enemiesInPlayerIsland = AICharacters.Where(c => !c.Inventory.Any(i => i.EntityType == EntityType.Key) && islandWithPlayer.Contains(c.ContainingTile) && c.Faction.EnemiesWith.Contains(Player.Faction) && c.Visible);
                    if (enemiesInPlayerIsland.Any())
                        enemiesInPlayerIsland.TakeRandomElement(Rng).PickItem(keyEntity, false);
                }

                usedKeyTypes.Add(keyTypeToUse);
            }
        }

        private bool IsCandidateRoom(Room room)
        {
            if (room.IsDummy) return false;
            if (room.HasStairs)
                return true;
            if (room.HasItems)
                return true;

            foreach (var (RoomA, RoomB, _, _ , _) in Hallways.Where(h => h.RoomA == room || h.RoomB == room))
            {
                if (RoomA == room)
                    if (RoomB.GetTiles().Any(t => t.Type == TileType.Door))
                        return true;
                if (RoomB == room)
                    if (RoomA.GetTiles().Any(t => t.Type == TileType.Door))
                        return true;
            }

            return false;
        }

        private bool IsFullyConnectedWithKeys()
        {
            var availableKeyTypes = Keys.Select(k => k.Name).ToList();
            var usedKeyTypes = new List<string>();
            var foundNewKeys = false;
            var islandCount = -1;
            do
            {
                var islands = Tiles.GetIslands(t => t.IsWalkable || usedKeyTypes.Contains(t.DoorId));
                islandCount = islands.Count;
                if (islandCount == 1) break;
                var islandWithPlayer = islands.FirstOrDefault(i => i.Contains(Player.ContainingTile));
                if (islandWithPlayer == null) return false;
                var newKeys = Keys.Where(k => (k.Position != null && islandWithPlayer.Contains(k.ContainingTile)) || (k.Owner != null && islandWithPlayer.Contains(k.Owner.ContainingTile))).ToList();
                foundNewKeys = newKeys.Any(k => !usedKeyTypes.Contains(k.ClassId.Replace("KeyType", "")));
                usedKeyTypes.AddRange(newKeys.Select(k => k.ClassId.Replace("KeyType", "")));
                usedKeyTypes = usedKeyTypes.Distinct().ToList();
            }
            while (foundNewKeys && islandCount != 1);
            return islandCount == 1;
        }

        private Room? GetRoomByRowAndColumn(int row, int column) => Rooms.Find(r => r.RoomRow == row && r.RoomColumn == column);

        private Room? GetRoomOrFusionByRowAndColumn(int row, int column)
        {
            var room = GetRoomByRowAndColumn(row, column);
            if (room == null)
            {
                var fusionWithRoom = Fusions.FirstOrDefault(f => (f.RoomA.RoomColumn == column && f.RoomA.RoomRow == row)
                                                        || f.RoomB.RoomColumn == column && f.RoomB.RoomRow == row);
                if (fusionWithRoom != default)
                    room = fusionWithRoom.FusedRoom;
            }
            return room;
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

        #region Floor room setup

        private void ResetAndCreateTiles()
        {
            CurrentEntityId = 1;
            TurnCount = 0;

            Tiles = new Tile[Height, Width];
            Parallel.For(0, Height, (Action<int>)(y =>
            {
                Parallel.For(0, Width, (Action<int>)(x =>
                {
                    var newTile = new Tile
                    {
                        Map = this,
                        Position = new GamePoint(x, y),
                        Type = TileType.Empty,
                        Discovered = false,
                        Visible = false
                    };
                    Tiles[y, x] = newTile;
                }));
            }));
        }

        #endregion

        #region Entities

        public void AddItemToInventory(Item item)
        {
            item.Id = CurrentEntityId;
            item.Map = this;
            Items.Add(item);
            CurrentEntityId++;
        }

        public Entity AddEntity(string classId, int level = 1, GamePoint predeterminatePosition = null)
        {
            var entityClass = Dungeon.Classes.Find(c => c.Id.Equals(classId))
                ?? throw new InvalidDataException("Class does not exist!");
            return AddEntity(entityClass, level, predeterminatePosition);
        }

        public Entity AddEntity(EntityClass entityClass, int level = 1, GamePoint predeterminatePosition = null)
        {
            Entity entity = null;
            switch (entityClass.EntityType)
            {
                case EntityType.Player:
                    entity = new PlayerCharacter(entityClass, level, this)
                    {
                        Id = CurrentEntityId,
                        Position = predeterminatePosition != null ? predeterminatePosition : PickEmptyPosition(TurnCount == 0)
                    };
                    if (Dungeon.PlayerClass.RequiresNamePrompt && !string.IsNullOrWhiteSpace(Dungeon.PlayerName))
                        entity.Name = Dungeon.PlayerName;
                    break;
                case EntityType.NPC:
                    entity = new NonPlayableCharacter(entityClass, level, this)
                    {
                        Id = CurrentEntityId,
                        Position = predeterminatePosition != null ? predeterminatePosition : PickEmptyPosition(TurnCount == 0)
                    };
                    break;
                case EntityType.Weapon:
                case EntityType.Armor:
                case EntityType.Consumable:
                    entity = new Item(entityClass, this)
                    {
                        Id = CurrentEntityId,
                        Position = predeterminatePosition != null ? predeterminatePosition : PickEmptyPosition(TurnCount == 0)
                    };
                    break;
                case EntityType.Trap:
                    entity = new Trap(entityClass, this)
                    {
                        Id = CurrentEntityId,
                        Position = predeterminatePosition != null ? predeterminatePosition : PickEmptyPosition(TurnCount == 0)
                    };
                    break;
                case EntityType.Key:
                    entity = new Key(entityClass, this)
                    {
                        Id = CurrentEntityId,
                        Position = predeterminatePosition != null ? predeterminatePosition : PickEmptyPosition(TurnCount == 0)
                    };
                    break;
                default:
                    throw new InvalidDataException("Entity lacks a valid type!");
            }
            CurrentEntityId++;
            if (entity.Position == null) return entity;
            if (entity is Item i)
            {
                Items.Add(i);
            }
            else if (entity is Key k)
            {
                Keys.Add(k);
            }
            else if (entity is Trap t)
            {
                Traps.Add(t);
            }
            else if (entity is Character c)
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
                    if (npc.OnSpawn?.ChecksCondition(npc, npc) == true)
                        npc.OnSpawn?.Do(npc, npc, false);
                }
            }
            return entity;
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
                    AddItemToInventory(Player.EquippedWeapon);
                if (Player.EquippedArmor?.ClassId.Equals(Dungeon.PlayerClass.StartingArmorId) == false)
                    AddItemToInventory(Player.EquippedArmor);
                Player.Inventory?.ForEach(i =>
                {
                    AddItemToInventory(i);
                });
            }
            Player.Position = PickEmptyPosition(false);
            Player.UpdateVisibility();
            if(Player.Hunger != null)
                Player.UsedStats.Where(s => s.RegenerationTarget == Player.Hunger).ForEach(s => s.Base = HungerDegeneration * -1);
        }

        private void NewTurn()
        {
            if (TurnCount == 0)
            {
                if (!HasFlag("TurnCount"))
                    CreateFlag("TurnCount", TurnCount, false);
                else
                    SetFlagValue("TurnCount", TurnCount);
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
                if (FloorConfigurationToUse.PossibleItems.Any())
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
                FloorConfigurationToUse.OnFloorStart?.Do(null, Player, false);

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
            Player.TookAction = false;
            Player.PerformOnTurnStart();
            Player.RemainingMovement = (int) Player.Movement.Current;
            LatestPlayerRemainingMovement = Player.RemainingMovement;
            AICharacters.Where(e => e != null).ForEach(e =>
            {
                e.RemainingMovement = (int) e.Movement.Current;
                e.TookAction = false;
                e.PerformOnTurnStart();
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
            var aiCharactersThatCanActAlongsidePlayer = AICharacters.Where(c => (c.RemainingMovement > 0 || c.Movement.Current == 0) && c.CanTakeAction && !c.TookAction && c.RemainingMovement >= minRequiredMovementToAct).OrderByDescending(c => c.RemainingMovement).ToList();
            while (aiCharactersThatCanActAlongsidePlayer.Any())
            {
                Parallel.ForEach(aiCharactersThatCanActAlongsidePlayer, aictca => aictca.PickTargetAndPath());
                aiCharactersThatCanActAlongsidePlayer.ForEach(aictca => aictca.AttackOrMove());
                aiCharactersThatCanActAlongsidePlayer = AICharacters.Where(c => (c.RemainingMovement > 0 || c.Movement.Current == 0) && c.CanTakeAction && !c.TookAction && c.RemainingMovement >= minRequiredMovementToAct).OrderByDescending(c => c.RemainingMovement).ToList();
            }
            if(GetCharacters().TrueForAll(c => (c.RemainingMovement == 0 && c.Movement.Current > 0) || !c.CanTakeAction || c.TookAction))
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
                    var bumps = false;
                    if (!targetTile.IsWalkable)
                        bumps = true;
                    else if (x != 0 && y != 0 && (!GetTileFromCoordinates(Player.Position.X + x, Player.Position.Y).IsWalkable
                            || !GetTileFromCoordinates(Player.Position.X, Player.Position.Y + y).IsWalkable))
                        bumps = true;
                    if(bumps)
                    {
                        if (targetTile.Type == TileType.Door)
                        {
                            SpecialEffectsThatHappened.Add(SpecialEffect.DoorClosed);
                            AppendMessage(Locale["CharacterBumpedDoor"].Format(new { CharacterName = Player.Name, DoorName = Locale[$"DoorType{targetTile.DoorId}"] }), Color.White);
                        }
                        else
                            SpecialEffectsThatHappened.Add(SpecialEffect.Bumped);
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
                if (character != Player) return false;
                if (characterInTargetTile.Movement.Current <= 0) return false;
                if (!characterInTargetTile.CanTakeAction) return false;
                if (!character.Visible && characterInTargetTile.Visible) return false;
                if (characterInTargetTile.Faction.EnemiesWith.Contains(character.Faction)) return false;
                // Swap positions with allies, neutrals or invisibles
                characterInTargetTile.Position = character.Position;
                if (characterInTargetTile.RemainingMovement > 0)
                    characterInTargetTile.RemainingMovement--;
                if (character == Player && !characterInTargetTile.Faction.EnemiesWith.Contains(character.Faction))
                    AppendMessage(Locale["CharacterSwitchedPlacesWithPlayer"].Format(new { CharacterName = characterInTargetTile.Name, PlayerName = Player.Name }));
            }
            if (character is NonPlayableCharacter npc)
                npc.LastPosition = character.Position;

            character.Position = targetTile.Position;

            if (targetTile.Key != null)
                character.TryToPickItem(targetTile.Key);
            targetTile.GetPickableObjects().Cast<IPickable>().ForEach(i => character.TryToPickItem(i));
            targetTile.StoodOn(character);
            targetTile.Trap?.Stepped(character);

            character.RemainingMovement--;

            return true;
        }

        public void PlayerUseStairs()
        {
            if (Player.ContainingTile != StairsTile)
                throw new ArgumentException($"Player is trying to use non-existent stairs at ({Player.ContainingTile.Position.X}, {Player.ContainingTile.Position.Y})");
            AppendMessage(Locale["FloorLeave"].Format(new { TurnCount = TurnCount.ToString() }), Color.Yellow);
            Dungeon.TakeStairs();
        }

        private void PlayerUseItem(Item item)
        {
            if (!item.IsEquippable)
            {
                item.Used(Player);
                if (item.OnUse?.FinishesTurnWhenUsed == true)
                    Player.TookAction = true;
            }
            else
            {
                AppendMessage(Locale["PlayerEquippedItem"].Format(new { CharacterName = Player.Name, ItemName = item.Name }), Color.Yellow);
                Player.EquipItem(item);
                Player.TookAction = true;
            }
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
                .Find(e => e != null && e.ExistenceStatus == EntityExistenceStatus.Alive);
            if (itemThatCanBePickedUp == null)
                return;
            if (Player.ItemCount == Player.InventorySize)
            {
                AppendMessage(Locale["InventoryIsFull"].Format(new { CharacterName = Player.Name, ItemName = itemThatCanBePickedUp.Name }));
            }
            else
            {
                Player.PickItem(itemThatCanBePickedUp, true);
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
            if (Player.ContainingTile.GetItems().Any())
            {
                AppendMessage(Locale["TileIsOccupied"].Format(new { CharacterName = Player.Name, ItemName = itemThatCanBeDropped.Name }));
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
            for (int i = 0; i < Player.KeySet.Count; i++)
            {
                inventory.InventoryItems.Add(new InventoryItemDto(Player.KeySet[i], Player, this));
            }
            for (int i = 0; i < itemsOnTile.Count; i++)
            {
                inventory.InventoryItems.Add(new InventoryItemDto(itemsOnTile[i], Player, this));
            }
            return inventory;
        }

        public void PlayerAttackTargetWith(string selectionId, int x, int y)
        {
            var characterInTile = GetTileFromCoordinates(x, y).LivingCharacter;

            var selectionIdParts = selectionId.Split('_');

            if (selectionIdParts.Length == 2)
            {
                var entity = selectionIdParts[0]; // "Player", "Target" or "Tile"
                var actionIdAsString = selectionIdParts[1]; // ActionId

                if(!int.TryParse(actionIdAsString, out int actionId))
                    throw new ArgumentException("Action SelectionId is not a valid number.");

                if (entity.Equals("Player"))
                {
                    var selectedAction = Player.OnAttack.Find(oaa => oaa.ActionId == actionId)
                        ?? throw new ArgumentException("Player attempted use a non-existent Attack action.");
                    if (selectedAction.TargetTypes.Contains(TargetType.Tile) || selectedAction.TargetTypes.Contains(TargetType.Room))
                        Player.InteractWithTile(GetTileFromCoordinates(x, y), selectedAction);
                    else if (characterInTile != null)
                        Player.AttackCharacter(characterInTile, selectedAction);
                    else
                        throw new ArgumentException("Player attempted use an action without a valid target.");
                }
                else if (entity.Equals("Target"))
                {
                    if(characterInTile is not NonPlayableCharacter npc)
                        throw new ArgumentException("Player attempted use an action without a valid target.");
                    var selectedAction = npc.OnInteracted.Find(oia => oia.ActionId == actionId)
                        ?? throw new ArgumentException("Player attempted use a non-existent Interact action from the Target.");
                    if (characterInTile != null)
                        Player.InteractWithCharacter(characterInTile, selectedAction);
                    else
                        throw new ArgumentException("Player attempted use an action without a valid target.");
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
            var tile = GetTileFromCoordinates(x, y);
            var characterInTile = tile.LivingCharacter;
            var targetName = string.Empty;
            if (characterInTile != null)
                targetName = characterInTile.Name;
            else if (tile.Type == TileType.Door)
                targetName = Locale[$"DoorType{tile.DoorId}"];
            else
                targetName = Locale[tile.Type.Name];
            var actionList = new ActionListDto(targetName);

            Player.OnAttack.ForEach(oaa => actionList.AddAction(oaa, Player, characterInTile, tile, this, true));

            if(characterInTile is NonPlayableCharacter npc)
                npc.OnInteracted?.ForEach(oia => actionList.AddAction(oia, Player, characterInTile, tile, this, false));

            if(actionList.Actions.DistinctBy(a => a.SelectionId).Count() != actionList.Actions.Count)
                throw new ArgumentException("Duplicate Actions discovered in selection.");

            return actionList;
        }

        public EntityDetailDto GetDetailsOfEntity(int x, int y)
        {
            var tile = GetTileFromCoordinates(x, y);
            var characterInTile = tile.LivingCharacter;
            var showTileDescription = !DefaultTileTypes.Contains(tile.Type);
            if (characterInTile?.Visible == true)
                return new EntityDetailDto(characterInTile, tile, showTileDescription);
            var itemInTile = tile.GetPickableObjects().FirstOrDefault();
            if(itemInTile?.Visible == true)
                return new EntityDetailDto(itemInTile, tile, showTileDescription);
            var trapInTile = tile.Trap;
            if (trapInTile?.Visible == true)
                return new EntityDetailDto(trapInTile, tile, showTileDescription);
            return showTileDescription ? new EntityDetailDto(null, tile, showTileDescription) : null;
        }

        private void SetStairs(GamePoint p)
        {
            var tile = GetTileFromCoordinates(p.X, p.Y);
            tile.Type = TileType.Stairs;
            StairsAreSet = true;
        }

        public void SetStairs()
        {
            if(StairsTile != null)
                StairsTile.Type = TileType.Floor;
            SetStairs(PickEmptyPosition(true));
        }

        public GamePoint PickEmptyPosition(bool allowPlayerRoom)
        {
            int rngX = -1, rngY = -1;
            var nonDummyRooms = Rooms.Where(r => r.Width > 1 && r.Height > 1).Distinct().ToList();
            var playerRoom = Player?.Position != null ? GetRoomInCoordinates(Player.Position.X, Player.Position.Y) : null;
            if(nonDummyRooms.Count > 1 && playerRoom != null && !allowPlayerRoom)
                nonDummyRooms.Remove(playerRoom);
            var roomIsValid = false;
            do
            {
                roomIsValid = false;
                var possibleNonDummyRoom = nonDummyRooms.TakeRandomElement(Rng);
                var validEmptyTiles = possibleNonDummyRoom.GetTiles().Where(t => CanBeConsideredEmpty(t));
                if (!validEmptyTiles.Any())
                {
                    nonDummyRooms.Remove(possibleNonDummyRoom);
                }
                else
                {
                    roomIsValid = true;
                    var aRandomTile = validEmptyTiles.TakeRandomElement(Rng);
                    rngX = aRandomTile.Position.X;
                    rngY = aRandomTile.Position.Y;
                }
            }
            while (nonDummyRooms.Any() && (!roomIsValid || !CanBeConsideredEmpty(GetTileFromCoordinates(rngX, rngY))));
            return rngX != -1 && rngY != -1 ? new GamePoint(rngX, rngY) : null;
        }

        private bool CanBeConsideredEmpty(Tile? t)
        {
            if (t == null) return false;
            var position = t.Position;

            if (!t.IsWalkable) return false;
            if (t.Type != TileType.Floor) return false;
            if (t.LivingCharacter != null) return false;
            if (t.GetItems().Any()) return false;
            if (t.Key != null) return false;
            if (t.Trap != null) return false;

            var hasLaterallyAdjacentHallways = GetTileFromCoordinates(position.X - 1, position.Y)?.Type == TileType.Hallway
                                            || GetTileFromCoordinates(position.X + 1, position.Y)?.Type == TileType.Hallway
                                            || GetTileFromCoordinates(position.X, position.Y - 1)?.Type == TileType.Hallway
                                            || GetTileFromCoordinates(position.X, position.Y + 1)?.Type == TileType.Hallway;
            if (hasLaterallyAdjacentHallways)
                return false;

            var hasLaterallyAdjacentDoors = GetTileFromCoordinates(position.X - 1, position.Y)?.Type == TileType.Door
                                            || GetTileFromCoordinates(position.X + 1, position.Y)?.Type == TileType.Door
                                            || GetTileFromCoordinates(position.X, position.Y - 1)?.Type == TileType.Door
                                            || GetTileFromCoordinates(position.X, position.Y + 1)?.Type == TileType.Door;
            if (hasLaterallyAdjacentDoors)
                return !hasLaterallyAdjacentDoors;

            return true;
        }

        #endregion

        #region Finding things in coordinates

        private void GetPossibleRoomData()
        {
            // Calculate MaxRoomWidth and MaxRoomHeight with adjusted constraints
            MaxRoomWidth = Math.Max(GeneratorToUse.MaxRoomSize.Width, Width / RoomCountColumns);
            MaxRoomHeight = Math.Max(GeneratorToUse.MaxRoomSize.Height, Height / RoomCountRows);

            // Ensure the room size is at least 5x5 and the min width/height is not less than that
            if (MaxRoomWidth < EngineConstants.MinRoomWidthOrHeight || MaxRoomHeight < EngineConstants.MinRoomWidthOrHeight)
                throw new InvalidDataException("Grid size or floor dimensions are too small to support minimum room size of 5x5");

            MinRoomWidth = Math.Max(MaxRoomWidth / 4, EngineConstants.MinRoomWidthOrHeight);
            MinRoomWidth = Math.Max(MinRoomWidth, GeneratorToUse.MinRoomSize.Width);

            MinRoomHeight = Math.Max(MaxRoomHeight / 4, EngineConstants.MinRoomWidthOrHeight);
            MinRoomHeight = Math.Max(MinRoomHeight, GeneratorToUse.MinRoomSize.Height);

            // Centering the grid in the floor
            var widthGap = (Width - (MaxRoomWidth * RoomCountColumns)) / 2;
            var heightGap = (Height - (MaxRoomHeight * RoomCountRows)) / 2;

            RoomLimitsTable = new (GamePoint topLeftCorner, GamePoint bottomRightCorner)[RoomCountRows, RoomCountColumns];

            // Calculating grid cell boundaries
            for (int i = 0; i < RoomCountRows; i++)
            {
                for (int j = 0; j < RoomCountColumns; j++)
                {
                    var topLeftCorner = new GamePoint
                    {
                        X = widthGap + (MaxRoomWidth * j),
                        Y = heightGap + (MaxRoomHeight * i)
                    };
                    var bottomRightCorner = new GamePoint
                    {
                        X = Math.Min(Width - widthGap, topLeftCorner.X + MaxRoomWidth - 1),
                        Y = Math.Min(Height - heightGap, topLeftCorner.Y + MaxRoomHeight - 1)
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

        public Tile GetTileFromCoordinates(GamePoint position)
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

        public List<Entity> GetEntitiesFromCoordinates(GamePoint GamePoint)
        {
            return GetEntitiesFromCoordinates(GamePoint.X, GamePoint.Y);
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
            if (tile.LivingCharacter != null)
            {
                if(tile.LivingCharacter.ExistenceStatus != EntityExistenceStatus.Alive && tile == StairsTile)
                        return tileBaseConsoleRepresentation;

                var characterBaseConsoleRepresentation = new ConsoleRepresentation
                {
                    ForegroundColor = tile.LivingCharacter.ConsoleRepresentation.ForegroundColor.Clone(),
                    BackgroundColor = tile.LivingCharacter.ConsoleRepresentation.BackgroundColor.Clone(),
                    Character = tile.LivingCharacter.ConsoleRepresentation.Character
                };
                if ((tile.LivingCharacter == Player || tile.LivingCharacter.Faction.AlliedWith.Contains(Player.Faction)) && !tile.LivingCharacter.Visible)
                {
                    // Invisible players or allies will get their colors reversed
                    characterBaseConsoleRepresentation.BackgroundColor = tile.LivingCharacter.ConsoleRepresentation.ForegroundColor.Clone();
                    characterBaseConsoleRepresentation.ForegroundColor = tile.LivingCharacter.ConsoleRepresentation.BackgroundColor.Clone();
                    return characterBaseConsoleRepresentation;
                }
                else if (tile.LivingCharacter.Visible)
                {
                    return characterBaseConsoleRepresentation;
                }
            }
            if (Player.ContainingTile == tile)
                return Player.ConsoleRepresentation;
            if (tile == StairsTile)
                return tileBaseConsoleRepresentation;
            var visibleItems = tile.GetPickableObjects().Where(i => i.CanBeSeenBy(Player));
            if (visibleItems.Any())
                return visibleItems.First().ConsoleRepresentation;
            if (tile.Key != null)
                return tile.Key.ConsoleRepresentation;
            if (tile.Trap?.CanBeSeenBy(Player) == true)
                return tile.Trap.ConsoleRepresentation;
            var deadEntityInCoordinates = GetEntitiesFromCoordinates(tile.Position).Find(e => e.Passable && e.ExistenceStatus == EntityExistenceStatus.Dead);
            if (deadEntityInCoordinates != null)
                return deadEntityInCoordinates.ConsoleRepresentation;
            return tileBaseConsoleRepresentation;
        }

        #endregion

        #region Room Connections

        private Room FuseRooms(Room thisRoom, Room adjacentRoom)
        {
            if (adjacentRoom.Width <= 1 || adjacentRoom.Height <= 1) return null;

            // Prevent non-square room fusions
            if (thisRoom.Width > MaxRoomWidth && thisRoom.RoomRow != adjacentRoom.RoomRow) return null;
            if (adjacentRoom.Width > MaxRoomWidth && thisRoom.RoomRow != adjacentRoom.RoomRow) return null;
            if (thisRoom.Height > MaxRoomHeight && thisRoom.RoomColumn != adjacentRoom.RoomColumn) return null;
            if (adjacentRoom.Height > MaxRoomHeight && thisRoom.RoomColumn != adjacentRoom.RoomColumn) return null;

            var minX = Math.Min(thisRoom.Position.X, adjacentRoom.Position.X);
            var maxX = Math.Max(thisRoom.Position.X + thisRoom.Width, adjacentRoom.Position.X + adjacentRoom.Width);
            var minY = Math.Min(thisRoom.Position.Y, adjacentRoom.Position.Y);
            var maxY = Math.Max(thisRoom.Position.Y + thisRoom.Height, adjacentRoom.Position.Y + adjacentRoom.Height);
            var width = Math.Max(maxX - minX, MinRoomWidth);
            var height = Math.Max(maxY - minY, MinRoomHeight);
            return new Room(this, new GamePoint(minX, minY), thisRoom.RoomRow, thisRoom.RoomColumn, width, height)
            {
                IsFused = true
            };
        }

        #endregion

        #region Graph methods

        public List<Tile> GetPathBetweenTiles(GamePoint sourcePosition, GamePoint targetPosition)
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

        public List<Tile> GetAdjacentTiles(GamePoint position, bool considerDiagonals)
        {
            return Tiles.GetAdjacentElements(GetTileFromCoordinates(position), considerDiagonals);
        }
        public List<Tile> GetAdjacentWalkableTiles(GamePoint position, bool considerDiagonals)
        {
            return Tiles.GetAdjacentElementsWhere(GetTileFromCoordinates(position), considerDiagonals, t => t.IsWalkable);
        }
        public List<Tile> GetFOVTilesWithinDistance(GamePoint source, int distance)
        {
            var tilesWithinDistance = Tiles.Where(t => t.Type.IsVisible && Math.Round(GamePoint.Distance(source, t.Position), 0, MidpointRounding.AwayFromZero) <= distance);
            var visibleTiles = new List<Tile>();
            foreach (var tile in tilesWithinDistance)
            {
                visibleTiles.AddRange(GetFOVLineBetweenTiles(GetTileFromCoordinates(source.X, source.Y), tile, distance));
            }
            return visibleTiles.Distinct().ToList();
        }
        public List<Tile> GetFOVLineBetweenTiles(Tile source, Tile target, int distance)
        {
            var componentPositions = new List<GamePoint>();
            var sourcePosition = source.Position;
            var targetPosition = target.Position;
            componentPositions.AddRange(MathAlgorithms.Raycast(sourcePosition, targetPosition, p => p.X, p => p.Y, (x, y) => new GamePoint(x, y), p => !GetTileFromCoordinates(p).IsSolid, p => !GetTileFromCoordinates(p).IsSolid && GetTileFromCoordinates(p).Room == null, distance, (p1, p2) => GamePoint.Distance(p1, p2)).ToList());
            if(!componentPositions.Contains(sourcePosition))
                componentPositions.Add(sourcePosition);

            if (!componentPositions.Exists(p => p.Equals(sourcePosition)))
                return new List<Tile>();

            return componentPositions.OrderBy(p => GamePoint.Distance(p, sourcePosition))
                                     .ToList()
                                     .ConvertAll(p => GetTileFromCoordinates(p))
                                     .TakeUntil(t => t.IsSolid)
                                     .ToList();
        }

        #endregion

        #region Tile Making
        private void CreateHallway((Room Source, Room Target, RoomConnectionType Tag) edge)
        {
            var roomA = edge.Source;
            var roomB = edge.Target;

            try
            {
                if (edge.Tag == RoomConnectionType.Horizontal)
                {
                    if (roomA.BottomRight.X < roomB.BottomLeft.X)
                        CreateHorizontalHallway(roomA, roomB);
                    else if (roomA.BottomRight.X > roomB.BottomLeft.X)
                        CreateHorizontalHallway(roomB, roomA);
                    else
                        return;
                }
                else if (edge.Tag == RoomConnectionType.Vertical)
                {
                    if (roomA.BottomLeft.Y < roomB.BottomRight.Y)
                        CreateVerticalHallway(roomA, roomB);
                    else if (roomA.BottomLeft.Y > roomB.BottomRight.Y)
                        CreateVerticalHallway(roomB, roomA);
                    else
                        return;
                }
            }
            catch
            {
                // If cannot build Hallway, pretend we never tried.
            }
        }

        private void CreateHorizontalHallway(Room leftRoom, Room rightRoom)
        {
            Tile? leftConnector = null, rightConnector = null, connectorA = null, connectorB = null;

            if (leftRoom.IsDummy)
                leftConnector = GetTileFromCoordinates(leftRoom.Position.X, leftRoom.Position.Y);
            if (rightRoom.IsDummy)
                rightConnector = GetTileFromCoordinates(rightRoom.Position.X, rightRoom.Position.Y);

            var leftRoomMinY = leftRoom.Position.Y + 1;
            var leftRoomMaxY = leftRoom.Position.Y + leftRoom.Height - 2;

            var rightRoomMinY = rightRoom.Position.Y + 1;
            var rightRoomMaxY = rightRoom.Position.Y + rightRoom.Height - 2;

            var largerMinY = leftRoomMinY > rightRoomMinY ? leftRoomMinY : rightRoomMinY;
            var lowerMaxY = leftRoomMaxY > rightRoomMaxY ? rightRoomMaxY : leftRoomMaxY;

            var minY = largerMinY > lowerMaxY ? lowerMaxY : largerMinY;
            var maxY = largerMinY > lowerMaxY ? largerMinY : lowerMaxY;

            if (leftConnector == null)
            {
                var x = leftRoom.Position.X + leftRoom.Width - 1;
                var y = Rng.NextInclusive(leftRoomMinY, leftRoomMaxY);
                leftConnector = GetTileFromCoordinates(x, y);
            }

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
                {
                    connectorA = GetTileFromCoordinates(ConnectorAPosition);
                    tilesToConvert.Add(connectorA);
                }
                if (ConnectorBPosition != null)
                {
                    connectorB = GetTileFromCoordinates(ConnectorBPosition);
                    tilesToConvert.Add(connectorB);
                }

                if (InBetweenConnectorPosition != null)
                {
                    // Horizontal line from Left Room to Hallway connection column
                    for (var i = ConnectorAPosition.X; i <= InBetweenConnectorPosition.X; i++)
                    {
                        tilesToConvert.Add(GetTileFromCoordinates(i, ConnectorAPosition.Y));
                    }

                    // Horizontal line from Right Room to Hallway connection column
                    for (var i = InBetweenConnectorPosition.X; i <= ConnectorBPosition.X; i++)
                    {
                        tilesToConvert.Add(GetTileFromCoordinates(i, ConnectorBPosition.Y));
                    }

                    // Draw a downwards line in case entryGamePoint from left is higher or equal to entryGamePoint from right
                    for (var i = rightConnector.Position.Y + 1; i < ConnectorAPosition.Y; i++)
                    {
                        tilesToConvert.Add(GetTileFromCoordinates(InBetweenConnectorPosition.X, i));
                    }
                    // Draw an upwards line in case entryGamePoint from left is lower than entryGamePoint from right
                    for (var i = ConnectorAPosition.Y + 1; i < ConnectorBPosition.Y; i++)
                    {
                        tilesToConvert.Add(GetTileFromCoordinates(InBetweenConnectorPosition.X, i));
                    }
                }

                tilesToConvert = tilesToConvert.Distinct().OrderByDescending(t => t.Position.Y).ThenBy(t => t.Position.X).ToList();

                isValidHallway = IsHallwayTileGroupValid(tilesToConvert, connectorA, connectorB);
            }
            while (!isValidHallway && hallwayGenerationTries < EngineConstants.MaxGenerationTriesForHallway);

            if (isValidHallway)
            {
                BuildHallwayTiles(tilesToConvert, connectorA, connectorB);
                Hallways.Add((leftRoom, rightRoom, connectorA, connectorB, tilesToConvert));
            }
        }

        private void CreateVerticalHallway(Room topRoom, Room downRoom)
        {
            Tile? topConnector = null, downConnector = null, connectorA = null, connectorB = null;

            if (topRoom.IsDummy)
                topConnector = GetTileFromCoordinates(topRoom.Position.X, topRoom.Position.Y);
            if (downRoom.IsDummy)
                downConnector = GetTileFromCoordinates(downRoom.Position.X, downRoom.Position.Y);

            var topRoomMinX = topRoom.Position.X + 1;
            var topRoomMaxX = topRoom.Position.X + topRoom.Width - 2;

            var downRoomMinX = downRoom.Position.X + 1;
            var downRoomMaxX = downRoom.Position.X + downRoom.Width - 2;

            var largerMinX = topRoomMinX > downRoomMinX ? topRoomMinX : downRoomMinX;
            var lowerMaxX = topRoomMinX > downRoomMinX ? downRoomMinX : topRoomMinX;

            var minX = largerMinX > lowerMaxX ? lowerMaxX : largerMinX;
            var maxX = largerMinX > lowerMaxX ? largerMinX : lowerMaxX;

            if (topConnector == null)
            {
                var x = Rng.NextInclusive(topRoomMinX, topRoomMaxX);
                var y = topRoom.Position.Y + topRoom.Height - 1;
                topConnector = GetTileFromCoordinates(x, y);
            }

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
                {
                    connectorA = GetTileFromCoordinates(ConnectorAPosition);
                    tilesToConvert.Add(connectorA);
                }
                if (ConnectorBPosition != null)
                {
                    connectorB = GetTileFromCoordinates(ConnectorBPosition);
                    tilesToConvert.Add(connectorB);
                }

                if (InBetweenConnectorPosition != null)
                {
                    // Vertical line from Up Room to Hallway connection row
                    for (var i = ConnectorAPosition.Y; i <= InBetweenConnectorPosition.Y; i++)
                    {
                        tilesToConvert.Add(GetTileFromCoordinates(ConnectorAPosition.X, i));
                    }

                    // Vertical line from Down Room to Hallway connection row
                    for (var i = InBetweenConnectorPosition.Y; i <= ConnectorBPosition.Y; i++)
                    {
                        tilesToConvert.Add(GetTileFromCoordinates(ConnectorBPosition.X, i));
                    }

                    // Draw a rightwards line in case entryGamePoint from up is more or equal to the right to entryGamePoint from below
                    for (var i = ConnectorBPosition.X + 1; i < ConnectorAPosition.X; i++)
                    {
                        tilesToConvert.Add(GetTileFromCoordinates(i, InBetweenConnectorPosition.Y));
                    }
                    // Draw a leftwards line in case entryGamePoint from up is more to the left than entryGamePoint from below
                    for (var i = ConnectorAPosition.X + 1; i < ConnectorBPosition.X; i++)
                    {
                        tilesToConvert.Add(GetTileFromCoordinates(i, InBetweenConnectorPosition.Y));
                    }
                }

                tilesToConvert = tilesToConvert.Distinct().OrderBy(t => t.Position.Y).ThenBy(t => t.Position.X).ToList();
                isValidHallway = IsHallwayTileGroupValid(tilesToConvert, topConnector, downConnector);
            }
            while (!isValidHallway && hallwayGenerationTries < EngineConstants.MaxGenerationTriesForHallway);

            if (isValidHallway)
            {
                BuildHallwayTiles(tilesToConvert, connectorA, connectorB);
                Hallways.Add((topRoom, downRoom, connectorA, connectorB, tilesToConvert));
            }
        }

        private (GamePoint? InBetweenConnectorPosition, GamePoint? ConnectorAPosition, GamePoint? ConnectorBPosition) CalculateConnectionPosition(Tile connectorA, Tile connectorB, RoomConnectionType connectionType)
        {
            int minX = 0, maxX = -1, minY = 0, maxY = -1;
            var connectorAPosition = new GamePoint(connectorA.Position.X, connectorA.Position.Y);
            var connectorBPosition = new GamePoint(connectorB.Position.X, connectorB.Position.Y);

            if (connectionType == RoomConnectionType.Horizontal)
            {
                if (connectorB.Position.X < connectorA.Position.X)
                {
                    (connectorB, connectorA) = (connectorA, connectorB);
                    (connectorBPosition, connectorAPosition) = (connectorAPosition, connectorBPosition);
                }
                if (Math.Abs(connectorA.Position.X - connectorB.Position.X) == 1)
                {
                    if (connectorA.Position.Y == connectorB.Position.Y)
                        return (null, connectorA.Position, connectorB.Position);
                    var leftYs = Enumerable.Range(connectorA.Room.Position.Y + 1, Math.Max(1, connectorA.Room.Height - 2)).ToList();
                    var rightYs = Enumerable.Range(connectorB.Room.Position.Y + 1, Math.Max(1, connectorB.Room.Height - 2)).ToList();
                    var sharedYs = leftYs.Intersect(rightYs);
                    if (!sharedYs.Any()) return (null, null, null);
                    var pickedY = sharedYs.TakeRandomElement(Rng);
                    connectorAPosition = new GamePoint(connectorA.Position.X, pickedY);
                    connectorBPosition = new GamePoint(connectorB.Position.X, pickedY);
                    return (null, connectorAPosition, connectorBPosition); // No need for a connection GamePoint in this case.
                }

                minX = connectorA.Position.X + 1;
                maxX = connectorB.Position.X - 1;
                minY = Math.Min(connectorA.Position.Y, connectorB.Position.Y);
                maxY = Math.Max(connectorA.Position.Y, connectorB.Position.Y);
            }
            else if (connectionType == RoomConnectionType.Vertical)
            {
                if(connectorB.Position.Y < connectorA.Position.Y)
                {
                    (connectorB, connectorA) = (connectorA, connectorB);
                    (connectorBPosition, connectorAPosition) = (connectorAPosition, connectorBPosition);
                }

                if (Math.Abs(connectorA.Position.Y - connectorB.Position.Y) == 1)
                {
                    if (connectorA.Position.X == connectorB.Position.X)
                        return (null, connectorA.Position, connectorB.Position);
                    var topXs = Enumerable.Range(connectorA.Room.Position.X + 1, Math.Max(1, connectorA.Room.Width - 2)).ToList();
                    var downXs = Enumerable.Range(connectorB.Room.Position.X + 1, Math.Max(1, connectorB.Room.Width - 2)).ToList();
                    var sharedXs = topXs.Intersect(downXs);
                    if (!sharedXs.Any()) return (null, null, null);
                    var pickedX = sharedXs.TakeRandomElement(Rng);
                    connectorAPosition = new GamePoint(pickedX, connectorA.Position.Y);
                    connectorBPosition = new GamePoint(pickedX, connectorB.Position.Y);
                    return (null, connectorAPosition, connectorBPosition); // No need for a connection GamePoint in this case.
                }

                minY = connectorA.Position.Y + 1;
                maxY = connectorB.Position.Y - 1;
                minX = Math.Min(connectorA.Position.X, connectorB.Position.X);
                maxX = Math.Max(connectorA.Position.X, connectorB.Position.X);
            }

            try
            {
                var connectionPosition = new GamePoint
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
                if (tile.Room != null)
                    return false;
            }
            return true;
        }

        private static void BuildHallwayTiles(List<Tile> tilesToConvert, Tile connectorA, Tile connectorB)
        {
            foreach (var tile in tilesToConvert)
            {
                tile.Type = TileType.Hallway;
                if (tile.Room != null)
                    tile.IsConnectorTile = true;
            }
        }

        private void CreateRiver((GamePoint TopLeftCorner, GamePoint BottomRightCorner) topLimits, (GamePoint TopLeftCorner, GamePoint BottomRightCorner) downLimits, TileType specialTileType)
        {
            Tile? topConnector = null, downConnector = null, connectorA = null, connectorB = null;

            var topLimitsMinX = topLimits.TopLeftCorner.X + 1;
            var topLimitsMaxX = topLimits.BottomRightCorner.X - 1;
            var topLimitsMinY = topLimits.TopLeftCorner.Y + 1;
            var topLimitsMaxY = topLimits.BottomRightCorner.Y - 1;

            var downLimitsMinX = downLimits.TopLeftCorner.X + 1;
            var downLimitsMaxX = downLimits.BottomRightCorner.X - 1;
            var downLimitsMinY = downLimits.TopLeftCorner.Y + 1;
            var downLimitsMaxY = downLimits.BottomRightCorner.Y - 1;

            if (topConnector == null)
            {
                var x = Rng.NextInclusive(topLimitsMinX, topLimitsMaxX);
                var y = Rng.NextInclusive(topLimitsMinY, topLimitsMaxY);
                topConnector = GetTileFromCoordinates(x, y);
            }

            if (downConnector == null)
            {
                var x = Rng.NextInclusive(downLimitsMinX, downLimitsMaxX);
                var y = Rng.NextInclusive(downLimitsMinY, downLimitsMaxY);
                downConnector = GetTileFromCoordinates(x, y);
            }

            var riverGenerationTries = 0;
            List<Tile> tilesToConvert;
            bool isValidRiver;

            do
            {
                riverGenerationTries++;
                tilesToConvert = new();

                var (InBetweenConnectorPosition, ConnectorAPosition, ConnectorBPosition) = CalculateConnectionPosition(topConnector, downConnector, RoomConnectionType.Vertical);

                if (ConnectorAPosition != null)
                {
                    connectorA = GetTileFromCoordinates(ConnectorAPosition);
                    tilesToConvert.Add(connectorA);
                }
                if (ConnectorBPosition != null)
                {
                    connectorB = GetTileFromCoordinates(ConnectorBPosition);
                    tilesToConvert.Add(connectorB);
                }

                if (InBetweenConnectorPosition != null)
                {
                    // Vertical line from Up Room to Hallway connection row
                    for (var i = ConnectorAPosition.Y; i <= InBetweenConnectorPosition.Y; i++)
                    {
                        tilesToConvert.Add(GetTileFromCoordinates(ConnectorAPosition.X, i));
                    }

                    // Vertical line from Down Room to Hallway connection row
                    for (var i = InBetweenConnectorPosition.Y; i <= ConnectorBPosition.Y; i++)
                    {
                        tilesToConvert.Add(GetTileFromCoordinates(ConnectorBPosition.X, i));
                    }

                    // Draw a rightwards line in case entryGamePoint from up is more or equal to the right to entryGamePoint from below
                    for (var i = ConnectorBPosition.X + 1; i < ConnectorAPosition.X; i++)
                    {
                        tilesToConvert.Add(GetTileFromCoordinates(i, InBetweenConnectorPosition.Y));
                    }
                    // Draw a leftwards line in case entryGamePoint from up is more to the left than entryGamePoint from below
                    for (var i = ConnectorAPosition.X + 1; i < ConnectorBPosition.X; i++)
                    {
                        tilesToConvert.Add(GetTileFromCoordinates(i, InBetweenConnectorPosition.Y));
                    }
                }

                tilesToConvert = tilesToConvert.Distinct().OrderBy(t => t.Position.Y).ThenBy(t => t.Position.X).ToList();
                isValidRiver = IsSpecialTileGroupValid(tilesToConvert);
            }
            while (!isValidRiver && riverGenerationTries < EngineConstants.MaxGenerationTriesForRiver);

            if (isValidRiver)
            {
                BuildSpecialTiles(tilesToConvert, specialTileType);
            }
        }

        private void CreateLake((GamePoint TopLeftCorner, GamePoint BottomRightCorner) limits, TileType specialTileType)
        {
            var minX = limits.TopLeftCorner.X + 1;
            var maxX = limits.BottomRightCorner.X - 1;
            var minY = limits.TopLeftCorner.Y + 1;
            var maxY = limits.BottomRightCorner.Y - 1;
            var lakeGenerationTries = 0;

            List<Tile> tilesToConvert;
            bool isValidLake;

            do
            {
                lakeGenerationTries++;
                tilesToConvert = new();

                var filteredTiles = Tiles.Where(tile =>
                    tile.Position.X >= minX && tile.Position.X <= maxX &&
                    tile.Position.Y >= minY && tile.Position.Y <= maxY);

                // We remove all the picked Tiles that lack a direct connection with one that is already walkable (to avoid producing unnecessary Islands)
                filteredTiles = filteredTiles.Where(tile => GetAdjacentWalkableTiles(tile.Position, false).Any()).ToList();

                var maximumElements = filteredTiles.Count / 5;
                tilesToConvert = filteredTiles.TakeNDifferentRandomElements(Rng.NextInclusive(maximumElements), Rng);

                isValidLake = IsSpecialTileGroupValid(tilesToConvert);
            }
            while (!isValidLake && lakeGenerationTries < EngineConstants.MaxGenerationTriesForRiver);

            if (isValidLake)
            {
                BuildSpecialTiles(tilesToConvert, specialTileType);
            }
        }

        private static bool IsSpecialTileGroupValid(List<Tile> tilesToConvert)
        {
            if (!tilesToConvert.Any()) return false;

            foreach (var tile in tilesToConvert)
            {
                if (tile.Type == TileType.Stairs || (tile.Room != null && tile.Room.IsDummy))
                    return false;
            }
            return true;
        }

        private static void BuildSpecialTiles(List<Tile> tilesToConvert, TileType tileType)
        {
            foreach (var tile in tilesToConvert)
            {
                tile.Type = tileType;
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
            var flag = Flags.Find(f => f.Key.Equals(key)) 
                ?? throw new FlagNotFoundException($"There's no flag with key {key} in {FloorName}", key);
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

        public Map Clone()
        {
            return JsonSerializer.Deserialize<Map>(JsonSerializer.Serialize(this));
        }

        public void AddSpecialEffectIfPossible(SpecialEffect specialEffect)
        {
            SpecialEffectsThatHappened.Add(specialEffect);
        }
        public void AddSpecialEffectIfPossible(SpecialEffect specialEffect, int position)
        {
            SpecialEffectsThatHappened.Insert(position, specialEffect);
        }
        public void MoveSpecialEffectToTheEndIfPossible(SpecialEffect specialEffect)
        {
            if (!SpecialEffectsThatHappened.Contains(specialEffect)) return;
            SpecialEffectsThatHappened.Remove(specialEffect);
            SpecialEffectsThatHappened.Insert(SpecialEffectsThatHappened.Count, specialEffect);
        }
        #endregion
    }
    #pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
    #pragma warning restore CS8601 // Posible asignación de referencia nula
    #pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
    #pragma warning restore CS8604 // Posible argumento de referencia nulo
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
