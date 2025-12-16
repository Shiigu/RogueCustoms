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
using System.Text.Json.Serialization;
using RogueCustomsGameEngine.Game.Interaction;
using RogueCustomsGameEngine.Utils.Effects.Utils;
using System.Text.RegularExpressions;
using RogueCustomsGameEngine.Game.DungeonStructure.FloorGenerators;
using D20Tek.Common.Models;
using RogueCustomsGameEngine.Game.DungeonStructure.FloorGenerators.Interfaces;
using System.Threading;

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

        public int GenerationTries { get; private set; }

        private bool _displayedTurnMessage;

        public PlayerCharacter Player => Dungeon.PlayerCharacter;

        public int TurnCount { get; set; }
        private int LastMonsterGenerationTurn;
        private int LatestPlayerRemainingMovement;

        public readonly Dungeon Dungeon;

        public List<ActionWithEffects> Scripts => Dungeon.Scripts;
        public List<Learnset> Learnsets => Dungeon.Learnsets;
        public List<Quest> Quests => Dungeon.Quests;

        public Locale Locale => Dungeon.LocaleToUse;

        private IPromptInvoker PromptInvoker => Dungeon.PromptInvoker;
        public DungeonStatus DungeonStatus
        {
            get { return Dungeon.DungeonStatus; }
            set { Dungeon.DungeonStatus = value; }
        }

        public readonly int FloorLevel;

        public readonly FloorType FloorConfigurationToUse;
        public bool AwaitingPromptInput { get; set; }
        public bool AwaitingQuestInput { get; set; }

        public Generator GeneratorToUse { get; set; }

        private readonly ProceduralGenerator DefaultGeneratorToUse = new ProceduralGenerator
        {
            Columns = 1,
            Rows = 1,
            MinRoomSize = new() { Height = 5, Width = 5 },
            RoomDisposition = new RoomDispositionType[1, 1]
        };
        public TileSet TileSet => FloorConfigurationToUse.TileSet;

        public string FloorName => Locale["FloorName"].Format(new {
                                                                DungeonName = Dungeon.Name.ToUpperInvariant(),
                                                                FloorLevel = FloorLevel.ToString()
                                                                });
        public decimal HungerDegeneration => FloorConfigurationToUse.HungerDegeneration;

        public int Width { get; private set; }
        public int Height { get; private set; }

        private int TotalMonstersInFloor => AICharacters.Count(e => !e.SpawnedViaMonsterHouse && e.ExistenceStatus == EntityExistenceStatus.Alive);
        private int TotalItemsInFloor => Items.Where(i => i.SpawnedInTheFloor).Count(e => e.ExistenceStatus != EntityExistenceStatus.Gone);
        private int TotalTrapsInFloor => Traps.Count(e => e.ExistenceStatus != EntityExistenceStatus.Gone);
        public bool IsHardcoreMode => Dungeon.IsHardcoreMode;
        public bool StairsAreSet { get; set; } = false;
        public List<NonPlayableCharacter> AICharacters { get; set; }
        public List<Element> Elements => Dungeon.Elements;
        public List<NPCModifier> NPCModifiers => Dungeon.NPCModifiers;
        public List<Affix> Affixes => Dungeon.Affixes;
        public List<Affix> Prefixes => Affixes.Where(a => a.Type == AffixType.Prefix).ToList();
        public List<Affix> Suffixes => Affixes.Where(a => a.Type == AffixType.Suffix).ToList();
        public List<QualityLevel> QualityLevels => Dungeon.QualityLevels;
        public List<CurrencyPile> CurrencyData => Dungeon.CurrencyData;
        public List<Currency> CurrencyPiles { get; set; }
        public List<ItemSlot> ItemSlots => Dungeon.ItemSlots;
        public List<Item> Items { get; set; }
        public List<Key> Keys { get; set; }
        public List<Tile> Doors => Tiles.Where(t => t.Type == TileType.Door);
        public List<Trap> Traps { get; set; }

        public List<LootTable> LootTables => Dungeon.LootTables;
        public RngHandler Rng { get; private set; }

        public Tile[,] Tiles { get; set; }
        public GamePoint StairsPosition { get; set; }
        public Tile StairsTile => Tiles.Find(t => t.Type == TileType.Stairs);

        public List<Room> Rooms { get; set; }

        public List<Flag> Flags { get; set; }

        public EntityClass CurrencyClass => Dungeon.CurrencyClass;
        public float SaleValuePercentage => Player.SaleValuePercentage;
        public List<EntityClass> PossibleClasses => Dungeon.Classes;
        public List<EntityClass> PossiblePlayerClasses => Dungeon.PlayerClasses;
        public List<EntityClass> PossibleNPCClasses => Dungeon.NPCClasses;
        public List<EntityClass> PossibleItemClasses => Dungeon.ItemClasses;
        public List<EntityClass> PossibleTrapClasses => Dungeon.TrapClasses;

        public List<AlteredStatus> PossibleStatuses { get; private set; }

        public List<EntityClass> UndroppableItemClasses => Dungeon.UndroppableItemClasses;

        public List<TileType> TileTypes { get; private set; }

        public List<TileType> DefaultTileTypes = new() { TileType.Empty, TileType.Floor, TileType.Hallway, TileType.Stairs, TileType.Wall, TileType.Door };
        public List<Entity> Entities
        {
            get
            {
                var list = new List<Entity>();
                list.Add(Player);
                return list.Union(AICharacters).Union(Items).Union(Traps).Union(CurrencyPiles).ToList();
            }
        }
        public List<MessageDto> Messages { get; private set; }
        [JsonIgnore]
        public bool IsDebugMode => Dungeon.IsDebugMode;

        [JsonIgnore]
        public DungeonDto Snapshot { get; set; }

        [JsonIgnore]
        // It's a list of lists because elements belonging to the same sub-list should be shown almost immediately (they are the result of the same act)
        // The Name is exclusively for debug purposes
        public List<(string Name, List<DisplayEventDto> Events)> DisplayEvents { get; set; }
        #endregion

        public Map(Dungeon dungeon, int floorLevel, List<Flag> flags, List<NonPlayableCharacter> npcsToKeep)
        {
            Dungeon = dungeon;
            FloorLevel = floorLevel;
            AICharacters = new List<NonPlayableCharacter>(npcsToKeep);
            CurrencyPiles = new List<Currency>();
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
            Messages = new();
            DisplayEvents = new();
            PossibleStatuses = new List<AlteredStatus>();
            Dungeon.AlteredStatusClasses.ForEach(alsc => PossibleStatuses.Add(new AlteredStatus(alsc, this)));
            SetActionParams();
            TurnCount = 0;
        }

        public void SetActionParams()
        {
            AttackActions.SetActionParams(Rng, this);
            CharacterActions.SetActionParams(Rng, this);
            ItemActions.SetActionParams(Rng, this);
            GenericActions.SetActionParams(Rng, this);
            OnTileActions.SetActionParams(Rng, this);
            ControlBlockActions.SetActionParams(Rng, this);
            PromptActions.SetActionParams(Rng, this);
            OnActionActions.SetActionParams(Rng, this);
            ExpressionParser.Setup(Rng, this);
            if (FloorConfigurationToUse.OnFloorStart != null)
                FloorConfigurationToUse.OnFloorStart.Map = this;
            foreach (var element in Dungeon.Elements)
            {
                if(element.OnAfterAttack != null)
                    element.OnAfterAttack.Map = this;
            }
            foreach (var tileType in Dungeon.TileTypes)
            {
                if (tileType.OnStood != null)
                    tileType.OnStood.Map = this;
            }
        }

        public void LoadRngState(int seed)
        {
            Rng = new RngHandler(seed);
        }
        public async Task GenerateDebugMap()
        {
            GenerationTries = 0;
            Width = 32;
            Height = 16;
            ResetAndCreateTilesIfNeeded();
            // Upper wall
            for (var i = 0; i < 25; i++)
            {
                var tile = GetTileFromCoordinates(i, 0);
                tile.Type = TileType.Wall;
            }
            // Lower wall
            for (var i = 0; i < 25; i++)
            {
                var tile = GetTileFromCoordinates(i, 10);
                tile.Type = TileType.Wall;
            }
            // Left wall
            for (var i = 0; i < 10; i++)
            {
                var tile = GetTileFromCoordinates(0, i);
                tile.Type = TileType.Wall;
            }
            // Right wall
            for (var i = 0; i < 10; i++)
            {
                var tile = GetTileFromCoordinates(25, i);
                tile.Type = TileType.Wall;
            }
            // Floor
            for (var i = 1; i < 24; i++)
            {
                for (var j = 1; j < 9 - 1; j++)
                {
                    var tile = GetTileFromCoordinates(i, j);
                    tile.Type = TileType.Floor;
                }
            }
            Rooms = ExtractRooms();
            await AddEntity(Dungeon.PlayerClass.Id);
            Player.Position = new GamePoint(5, 3);
            SetStairs(new GamePoint(19, 7));
            DisplayEvents = new() { };
        }

        public async Task<(bool MapGenerationSuccess, bool KeyGenerationSuccess)> Generate(bool isGeneratingForDebug, List<(string Name, List<DisplayEventDto> Events)> eventsToAppend)
        {
            var usingDefaultGenerator = false;
            var mapGenerationSuccess = false;
            var keyGenerationSuccess = true;    // Defaults to true just in case there were no keys to generate
            GenerationTries = 0;
            IFloorGenerator generator = GeneratorToUse is ProceduralGenerator ?
                            new ProceduralFloorGenerator(this, GeneratorToUse as ProceduralGenerator) :
                            new StaticFloorGenerator(this, GeneratorToUse as StaticGenerator);
            bool success;
            do
            {
                success = false;
                GenerationTries++;
                generator.CreateNormalTiles();

                if (generator.ReadyToFloodFill)
                {
                    Rooms = ExtractRooms();
                }

                if ((Rooms.Count == 1 && !Rooms.Any(r => r.Tiles?.Count <= 1)) || Rooms.Count(r => r.Tiles.Count > 1) > 1)
                {
                    success = generator.IsFloorSolvable();
                    if (success)
                    {
                        generator.CreateSpecialTiles();
                        success = generator.IsFloorSolvable();
                        if (success)
                        {
                            await generator.PlacePlayerAndKeptNPCs();
                            success = Player.Position != null;
                            if (success)
                            {
                                generator.PlaceStairs();
                                success = generator.ArePlayerAndStairsPositionsCorrect();
                                if (success)
                                {
                                    Player.ContainingRoom.WasVisited = true;
                                    if (!isGeneratingForDebug)
                                    {
                                        DisplayEvents = new();
                                        DisplayEvents.Add(("ClearMessageLog", new()
                                        {
                                            new() {
                                                DisplayEventType = DisplayEventType.ClearLogMessages,
                                                Params = new() { }
                                            }
                                        }
                                        ));
                                        DisplayEvents.AddRange(eventsToAppend);
                                        AppendMessage(Locale["FloorEnter"].Format(new { FloorLevel = FloorLevel.ToString() }), Color.Yellow);
                                    }
                                }
                            }
                        }
                    }
                }
                if (!success && GenerationTries == EngineConstants.MaxGenerationTries)
                {
                    usingDefaultGenerator = true;
                    generator = new ProceduralFloorGenerator(this, DefaultGeneratorToUse);
                }
            }
            while (!success);

            mapGenerationSuccess = success && !usingDefaultGenerator;

            if (success && Rooms.Count(r => r.Tiles.Count > 1) > 1 && FloorConfigurationToUse.PossibleKeys?.KeyTypes != null && FloorConfigurationToUse.PossibleKeys.KeyTypes.Any())
            {
                do
                {
                    GenerationTries++;
                    await generator.PlaceKeysAndDoors();
                    success = generator.IsFloorSolvableWithKeys();
                    if (success)
                    {
                        var events = new List<DisplayEventDto>();
                        if (!isGeneratingForDebug)
                        {
                            foreach (var key in Keys)
                            {
                                events.Add(new()
                                {
                                    DisplayEventType = DisplayEventType.UpdateTileRepresentation,
                                    Params = new() { key.Position, GetConsoleRepresentationForCoordinates(key.Position.X, key.Position.Y) }
                                }
                                );
                            }
                            foreach (var door in Doors)
                            {
                                try
                                {
                                    var existingValue = (int)GetFlagValue($"Doors_{door.DoorId}");
                                    events.Add(new()
                                    {
                                        DisplayEventType = DisplayEventType.UpdateTileRepresentation,
                                        Params = new() { door.Position, GetConsoleRepresentationForCoordinates(door.Position.X, door.Position.Y) }
                                    }
                                    );
                                    SetFlagValue($"Doors_{door.DoorId}", existingValue + 1);
                                }
                                catch (FlagNotFoundException)
                                {
                                    CreateFlag($"Doors_{door.DoorId}", 1, true);
                                }
                            }
                            DisplayEvents.Add(("Placing Keys", events));
                        }
                    }
                }
                while (!success && GenerationTries < EngineConstants.MaxGenerationTries);
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
                    Tiles.Where(t => t.DoorId != "").ForEach(t =>
                    {
                        t.Type = TileType.Hallway;
                        t.DoorId = string.Empty;
                    });
                }
                keyGenerationSuccess = success;
            }

            try
            {
                await generator.PlaceEntities();
                await NewTurn();
            }
            catch (Exception ex)
            {
                if (!IsDebugMode)
                    throw;
                else
                    mapGenerationSuccess = false;
            }

            if (!isGeneratingForDebug)
                Snapshot = new(Dungeon, this);
            return (mapGenerationSuccess, keyGenerationSuccess);
        }

        private bool IsTileValidForVisiting(Tile tile)
        {
            if (tile == null) return false;
            if (tile.Type == TileType.Empty) return false;
            if (tile.Type == TileType.Wall) return false;
            if (tile.Type == TileType.Hallway) return false;
            return true;
        }

        private bool IsStrictFloor(Tile tile)
        {
            if (tile == null) return false;
            return tile.Type == TileType.Floor || tile.Type == TileType.Stairs;
        }

        public List<Room> ExtractRooms()
        {
            int rows = Tiles.GetLength(0);
            int cols = Tiles.GetLength(1);

            bool[,] visited = new bool[rows, cols];
            List<Room> rooms = new();

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    Tile startTile = Tiles[y, x];

                    if (!visited[y, x] && IsTileValidForVisiting(startTile) && IsStrictFloor(startTile))
                    {
                        var roomTiles = FloodFillRoom(visited, startTile);

                        if (roomTiles.Count > 0)
                        {
                            var boundaries = AddAdjacentWalls(roomTiles);
                            if (boundaries.Count > 0)
                            {
                                roomTiles = boundaries.ToList();
                                Room room = new Room(this, roomTiles);
                                rooms.Add(room);
                            }
                        }
                    }
                }
            }

            return rooms;
        }

        private List<Tile> FloodFillRoom(bool[,] visited, Tile start)
        {
            int rows = Tiles.GetLength(0);
            int cols = Tiles.GetLength(1);

            List<Tile> roomTiles = new();
            Queue<Tile> queue = new();
            queue.Enqueue(start);
            bool isFailedRoom = false;

            while (queue.Count > 0)
            {
                Tile current = queue.Dequeue();
                int x = current.Position.X;
                int y = current.Position.Y;

                if (visited[y, x]) continue;

                if (!IsTileValidForVisiting(current)) continue;

                visited[y, x] = true;
                roomTiles.Add(current);

                var neighbors = new (int dx, int dy)[]
                {
                    (-1, 0), (1, 0), (0, -1), (0, 1),
                    (1, 1), (1, -1), (-1, -1), (-1, 1)
                };

                bool hasValidNeighbours = true;

                foreach (var (dx, dy) in neighbors)
                {
                    int nx = x + dx, ny = y + dy;
                    if (nx >= 0 && nx < cols && ny >= 0 && ny < rows && !visited[ny, nx])
                    {
                        var neighbor = Tiles[ny, nx];
                        if (IsTileValidForVisiting(neighbor))
                        {
                            queue.Enqueue(neighbor);
                        }
                        else if (neighbor.Type == TileType.Empty)
                        {
                            //if (IsStrictFloor(current))
                            //    isFailedRoom = true;

                            hasValidNeighbours = false;
                            break;
                        }
                    }
                }

                if (!hasValidNeighbours)
                    roomTiles.Remove(current);
            }

            return isFailedRoom ? new() : roomTiles;
        }

        private HashSet<Tile> AddAdjacentWalls(List<Tile> roomTiles)
        {
            HashSet<Tile> extendedTiles = new(roomTiles);

            foreach (var tile in roomTiles)
            {
                var neighbors = new (int dx, int dy)[]
                {
            (-1, 0), (1, 0), (0, -1), (0, 1),
            (-1, -1), (-1, 1), (1, -1), (1, 1)
                };

                foreach (var (dx, dy) in neighbors)
                {
                    int nx = tile.Position.X + dx;
                    int ny = tile.Position.Y + dy;
                    if (nx >= 0 && nx < Tiles.GetLength(1) && ny >= 0 && ny < Tiles.GetLength(0))
                    {
                        var neighborTile = Tiles[ny, nx];
                        if (neighborTile.Type != TileType.Empty)
                            extendedTiles.Add(neighborTile);
                    }
                }
            }

            return extendedTiles;
        }

        public void AppendMessage(string message) => AppendMessage(message, new GameColor(Color.White), new GameColor(Color.Transparent), null);
        public void AppendMessage(string message, List<DisplayEventDto>? eventList) => AppendMessage(message, new GameColor(Color.White), new GameColor(Color.Transparent), eventList);
        public void AppendMessage(string message, Color foregroundColor) => AppendMessage(message, new GameColor(foregroundColor), null);
        public void AppendMessage(string message, Color foregroundColor, List<DisplayEventDto>? eventList) => AppendMessage(message, new GameColor(foregroundColor), eventList);
        public void AppendMessage(string message, GameColor foregroundColor) => AppendMessage(message, foregroundColor, new GameColor(Color.Transparent), null);
        public void AppendMessage(string message, GameColor foregroundColor, List<DisplayEventDto>? eventList) => AppendMessage(message, foregroundColor, new GameColor(Color.Transparent), eventList);
        public void AppendMessage(string message, Color foregroundColor, Color backgroundColor) => AppendMessage(message, new GameColor(foregroundColor), new GameColor(backgroundColor), null);
        public void AppendMessage(string message, Color foregroundColor, Color backgroundColor, List<DisplayEventDto>? eventList) => AppendMessage(message, new GameColor(foregroundColor), new GameColor(backgroundColor), eventList);
        public void AppendMessage(string message, GameColor foregroundColor, GameColor backgroundColor, List<DisplayEventDto>? eventList)
        {
            if (string.IsNullOrWhiteSpace(message)) return;
            if (!_displayedTurnMessage && TurnCount > 0)
            {
                var newTurnMessage = new MessageDto
                {
                    Message = Locale["NewTurn"].Format(new { TurnCount = TurnCount.ToString() }),
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Yellow)
                };
                Messages.Add(newTurnMessage);
                DisplayEvents.Add(($"Turn {TurnCount}", new()
                    {
                        new() { 
                            DisplayEventType = DisplayEventType.AddLogMessage, 
                            Params = new() { newTurnMessage } 
                        }
                    }
                ));
                _displayedTurnMessage = true;
            }
            Messages.Add(new MessageDto
            {
                Message = message,
                ForegroundColor = foregroundColor,
                BackgroundColor = backgroundColor
            });

            if(eventList != null)
            {
                eventList.Add(
                    new()
                    {
                        DisplayEventType = DisplayEventType.AddLogMessage,
                        Params = new()
                        {
                            new MessageDto
                            {
                                Message = message,
                                ForegroundColor = foregroundColor,
                                BackgroundColor = backgroundColor
                            }
                        }
                    });
            }
            else
            {
                DisplayEvents.Add(("AppendMessage", new()
                {
                    new() {
                        DisplayEventType = DisplayEventType.AddLogMessage,
                        Params = new()
                        {
                            new MessageDto
                            {
                                Message = message,
                                ForegroundColor = foregroundColor,
                                BackgroundColor = backgroundColor
                            }
                        }
                    }
                }
                ));
            }
        }
        public void AddMessageBox(string title, string message, string buttonCaption, GameColor windowColor) => AddMessageBox(title, message, buttonCaption, windowColor, null);
        public void AddMessageBox(string title, string message, string buttonCaption, GameColor windowColor, List<DisplayEventDto>? eventList)
        {
            if (eventList != null)
            {
                eventList.Add(
                    new()
                    {
                        DisplayEventType = DisplayEventType.AddMessageBox,
                        Params = new()
                        {
                            new MessageBoxDto
                            {
                                Title = title,
                                Message = message,
                                ButtonCaption = buttonCaption,
                                WindowColor = windowColor
                            }
                        }
                    });
            }
            else
            {
                DisplayEvents.Add(("AddMessageBox", new()
                {
                    new() {
                        DisplayEventType = DisplayEventType.AddMessageBox,
                        Params = new()
                        {
                            new MessageBoxDto
                            {
                                Title = title,
                                Message = message,
                                ButtonCaption = buttonCaption,
                                WindowColor = windowColor
                            }
                        }
                    }
                }
                ));
            }
            AwaitingPromptInput = true;
        }

        #region Floor room setup

        public void ResetAndCreateTilesIfNeeded()
        {
            ResetEntityId();
            TurnCount = 0;

            if (Tiles == null || Tiles.GetLength(0) != Height || Tiles.GetLength(1) != Width)
            {
                Tiles = new Tile[Height, Width];
                Parallel.For(0, Height, y =>
                {
                    Parallel.For(0, Width, x =>
                    {
                        Tiles[y, x] = new Tile
                        {
                            Map = this,
                            Position = new GamePoint(x, y),
                            Type = TileType.Empty,
                            Discovered = false,
                            Visible = false
                        };
                    });
                });
            }
            else 
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        Tiles[y,x].Type = TileType.Empty;
                        Tiles[y,x].Discovered = false;
                        Tiles[y,x].Visible = false;
                    }
                }
            }
        }

        #endregion

        #region Entities

        public void RegisterItemFromInventory(Item item)
        {
            item.Id = GenerateEntityId();
            item.Map = this;
            Items.Add(item);
        }

        public Currency CreateCurrency(CurrencyPile pileType, GamePoint predeterminatePosition = null, bool mustPickPosition = false)
        {
            GamePoint? PositionToUse = null;
            if (predeterminatePosition != null)
            {
                PositionToUse = predeterminatePosition;
            }
            else if (mustPickPosition)
            {
                PositionToUse = PickEmptyPosition(TurnCount == 0, true);
            }

            var currencyPile = new Currency(CurrencyClass, this)
            {
                Id = GenerateEntityId(),
                Position = PositionToUse,
                Amount = Rng.NextInclusive(pileType.Minimum, pileType.Maximum)
            };

            var baseName = currencyPile.Name;
            currencyPile.Name = Locale["CurrencyDisplayName"].Format(new { Amount = currencyPile.Amount.ToString(), CurrencyName = baseName });

            CurrencyPiles.Add(currencyPile);

            return currencyPile;
        }

        public Currency CreateCurrency(int amount, GamePoint predeterminatePosition = null, bool mustPickPosition = false)
        {
            GamePoint? PositionToUse = null;
            if (predeterminatePosition != null)
            {
                PositionToUse = predeterminatePosition;
            }
            else if (mustPickPosition)
            {
                PositionToUse = PickEmptyPosition(TurnCount == 0, true);
            }

            var currencyPile = new Currency(CurrencyClass, this)
            {
                Id = GenerateEntityId(),
                Position = PositionToUse,
                Amount = amount
            };

            var baseName = currencyPile.Name;
            currencyPile.Name = Locale["CurrencyDisplayName"].Format(new { Amount = currencyPile.Amount.ToString(), CurrencyName = baseName });

            CurrencyPiles.Add(currencyPile);

            return currencyPile;
        }

        public async Task<Entity> AddEntity(string classId, int level = 1, GamePoint predeterminatePosition = null, bool mustPickPosition = true)
        {
            var classIdToUse = classId;

            var match = Regex.Match(classIdToUse, EngineConstants.CurrencyRegexPattern);

            if (match.Success)
            {
                var pileTypeId = match.Groups[1].Value;
                var correspondingPileType = Dungeon.CurrencyData.Find(pt => pt.Id.Equals(pileTypeId))
                    ?? throw new InvalidDataException($"Currency pile type {pileTypeId} does not exist!");
                return CreateCurrency(correspondingPileType, predeterminatePosition, mustPickPosition);
            }
            else
            {
                var entityClass = Dungeon.Classes.Find(c => c.Id.Equals(classIdToUse))
                    ?? throw new InvalidDataException("Class does not exist!");
                return await AddEntity(entityClass, level, predeterminatePosition, mustPickPosition);
            }
        }

        public async Task<Entity> AddEntity(EntityClass entityClass, int level = 1, GamePoint predeterminatePosition = null, bool mustPickPosition = true)
        {
            Entity entity = null;
            GamePoint? PositionToUse = null;
            if(predeterminatePosition != null)
            {
                PositionToUse = predeterminatePosition;
            }
            else if (mustPickPosition)
            {
                var isAPassableEntity = entityClass.EntityType == EntityType.Item || entityClass.EntityType == EntityType.Trap || entityClass.EntityType == EntityType.Key;
                PositionToUse = PickEmptyPosition(TurnCount == 0, isAPassableEntity);
            }
            switch (entityClass.EntityType)
            {
                case EntityType.Player:
                    entity = new PlayerCharacter(entityClass, level, this)
                    {
                        Id = GenerateEntityId(),
                        Position = PositionToUse,
                        HighestFloorReached = 1
                    };
                    if (Dungeon.PlayerClass.RequiresNamePrompt && !string.IsNullOrWhiteSpace(Dungeon.PlayerName))
                        entity.Name = Dungeon.PlayerName;
                    break;
                case EntityType.NPC:
                    entity = new NonPlayableCharacter(entityClass, level, this)
                    {
                        Id = GenerateEntityId(),
                        Position = PositionToUse
                    };
                    break;
                case EntityType.Item:
                    entity = new Item(entityClass, level, this)
                    {
                        Id = GenerateEntityId(),
                        Position = PositionToUse
                    };
                    (entity as Item).SpawnedInTheFloor = PositionToUse != null;
                    (entity as Item).SetQualityLevel();
                    break;
                case EntityType.Trap:
                    entity = new Trap(entityClass, this)
                    {
                        Id = GenerateEntityId(),
                        Position = PositionToUse
                    };
                    break;
                case EntityType.Key:
                    entity = new Key(entityClass, this)
                    {
                        Id = GenerateEntityId(),
                        Position = PositionToUse
                    };
                    break;
                default:
                    throw new InvalidDataException("Entity lacks a valid type!");
            }

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
                var mostBasicQualityLevel = QualityLevels.MinBy(q => q.MaximumAffixes);
                foreach (var itemId in entityClass.StartingInventoryIds)
                {
                    var itemEntityClass = Dungeon.Classes.Find(cl => cl.Id.Equals(itemId))
                        ?? throw new InvalidDataException("Class {c.ClassId} has an invalid starting inventory item!");
                    var inventoryItem = new Item(itemEntityClass, 1, this)
                    {
                        Id = GenerateEntityId(),
                        GotSpecificallyIdentified = true
                    };
                    inventoryItem.SetQualityLevel(mostBasicQualityLevel);
                    Items.Add(inventoryItem);
                    c.Inventory.Add(inventoryItem);
                }
                foreach (var initialEquipmentId in entityClass.InitialEquipmentIds)
                {
                    if (string.IsNullOrWhiteSpace(initialEquipmentId)) continue;
                    var itemClass = Dungeon.ItemClasses.Find(ic => ic.Id.Equals(initialEquipmentId))
                        ?? throw new InvalidDataException($"Class {c.ClassId} has invalid Initial Equipment {initialEquipmentId}!");
                    var equippedItem = new Item(itemClass, 1, this)
                    {
                        Id = GenerateEntityId(),
                        GotSpecificallyIdentified = true
                    };
                    equippedItem.SetQualityLevel(mostBasicQualityLevel);
                    c.Equipment.Add(equippedItem);
                    Items.Add(equippedItem);
                }
                if (entity is PlayerCharacter p)
                {
                    Dungeon.PlayerCharacter = p;
                }
                else if (entity is NonPlayableCharacter npc)
                {
                    AICharacters.Add(npc);
                    foreach (var onSpawn in npc.OnSpawn ?? [])
                    {
                        if (onSpawn != null && onSpawn.ChecksCondition(npc, npc))
                            await onSpawn.Do(npc, npc, false);
                    }
                }
            }
            return entity;
        }

        public async Task PlacePlayer(GamePoint position = null)
        {
            if (Player == null)
            {
                await AddEntity(Dungeon.PlayerClass.Id);
            }
            else
            {
                RegisterPreexistingCharacter(Player);
            }
            Player.Position = position ?? PickEmptyPosition(false, false);
            Player.UpdateVisibility();
            if(Player.Hunger != null && Player.HungerDegeneration != null)
                Player.HungerDegeneration.Base = HungerDegeneration * -1;
        }

        public void RegisterPreexistingCharacter(Character character)
        {
            character.Map = this;
            character.Id = GenerateEntityId();
            foreach (var item in character.Equipment)
            {
                RegisterItemFromInventory(item);
            }
            character.Inventory?.ForEach(RegisterItemFromInventory);
            if (character is NonPlayableCharacter npc)
                npc.ResetAIData();
        }

        private async Task NewTurn()
        {
            _displayedTurnMessage = false;
            if (TurnCount == 0)
            {
                if (!HasFlag("TurnCount"))
                    CreateFlag("TurnCount", TurnCount, false);
                else
                    SetFlagValue("TurnCount", TurnCount);

                var mustRollNPCs = GeneratorToUse is ProceduralGenerator;
                var mustRollItems = GeneratorToUse is ProceduralGenerator || (GeneratorToUse is StaticGenerator sg && sg.GenerateItemsOnFirstTurn);
                var mustRollTraps = GeneratorToUse is ProceduralGenerator || (GeneratorToUse is StaticGenerator sg2 && sg2.GenerateTrapsOnFirstTurn);

                if(mustRollNPCs)
                    await RollTurn0NPCs();

                if (mustRollItems)
                    await RollTurn0Items();

                if (mustRollTraps)
                    await RollTurn0Traps();

                #region Flag Monster House

                if(GeneratorToUse is ProceduralGenerator && Rooms.Count(r => r.Tiles.Count > 1) > 1 && Rng.RollProbability() <= FloorConfigurationToUse.MonsterHouseOdds)
                {
                    var roomPickList = new List<Room>();
                    var smallestRoom = Rooms.Where(r => r.Tiles.Count > 1).Min(r => r.Tiles.Count);
                    foreach (var room in Rooms.Where(r => r.Tiles.Count > 1 && r != Player.ContainingRoom))
                    {
                        // The idea is to prioritize bigger rooms when picking a Monster House candidate
                        var tilesDifference = room.Tiles.Count - smallestRoom;
                        for (int i = 0; i < 1 + tilesDifference; i++)
                        {
                            roomPickList.Add(room);
                        }
                    }
                    var selectedRoom = roomPickList.TakeRandomElement(Rng);
                    selectedRoom.MustSpawnMonsterHouse = true;
                    var acceptableTiles = selectedRoom.Tiles.Where(t => CanBeConsideredEmpty(t) && t.Type.AcceptsItems);
                    acceptableTiles = acceptableTiles.TakeNDifferentRandomElements(acceptableTiles.Count() / 2, Rng);
                    var tilesForItems = acceptableTiles.Take(acceptableTiles.Count() / 2).Take(FloorConfigurationToUse.MaxItemsInFloor);
                    var tilesForTraps = acceptableTiles.Except(tilesForItems).Take(FloorConfigurationToUse.MaxTrapsInFloor);

                    if (FloorConfigurationToUse.PossibleItems.Any())
                    {
                        foreach (var tile in tilesForItems)
                        {
                            if (Rng.RollProbability() > 80) continue;
                            var pickedItemId = FloorConfigurationToUse.PossibleItems.TakeRandomElement(Rng).ClassId;
                            await AddEntity(pickedItemId, 1, tile.Position);
                        }
                    }

                    if (FloorConfigurationToUse.PossibleTraps.Any())
                    {
                        foreach (var tile in tilesForTraps)
                        {
                            if (Rng.RollProbability() > 80) continue;
                            var pickedTrapId = FloorConfigurationToUse.PossibleTraps.TakeRandomElement(Rng).ClassId;
                            await AddEntity(pickedTrapId, 1, tile.Position);
                        }
                    }
                }

                #endregion

                #region Perform On Floor Start Actions

                var events = new List<DisplayEventDto>();

                if (FloorLevel > 1)
                {
                    events.Add(
                        new()
                        {
                            DisplayEventType = DisplayEventType.PlaySpecialEffect,
                            Params = new() { SpecialEffect.TakeStairs }
                        });
                }
                AddMessageBox(Dungeon.Name, Locale["FloorEnter"].Format(new { FloorLevel = FloorLevel.ToString() }), "OK", new GameColor(Color.Yellow), events);

                DisplayEvents.Add(($"Floor {FloorLevel} start", events));

                if (FloorConfigurationToUse.OnFloorStart != null)
                    await FloorConfigurationToUse.OnFloorStart.Do(Player, Player, false);

                TurnCount = 1;
                SetFlagValue("TurnCount", TurnCount);
                #endregion
            }
            else 
            {
                TurnCount++;
                SetFlagValue("TurnCount", TurnCount);
                Snapshot.TurnCount = TurnCount;

                #region Generate A monster
                if (TurnCount - LastMonsterGenerationTurn >= FloorConfigurationToUse.TurnsPerMonsterGeneration)
                {
                    var currentMonsters = AICharacters.Where(e => e.EntityType == EntityType.NPC && !e.SpawnedViaMonsterHouse && e.ExistenceStatus == EntityExistenceStatus.Alive);
                    if (currentMonsters.Count() < FloorConfigurationToUse.SimultaneousMaxMonstersInFloor)
                    {
                        await AddNPC();
                    }
                }
                #endregion
            }

            ClearIterationFlags();

            Player.TookAction = false;
            foreach (var tile in Tiles.Where(t => t.Type != t.BaseType && t.RemainingTransformationTurns > 0))
            {
                tile.RemainingTransformationTurns--;
                if (tile.RemainingTransformationTurns <= 0)
                {
                    tile.ResetType();
                }
            }

            foreach (var item in Items)
            {
                item.UpdateNameIfNeeded();
            }

            var livingCharacters = GetCharacters().Where(e => e != null && e.ExistenceStatus == EntityExistenceStatus.Alive);

            foreach (var character in livingCharacters)
            {
                character.RemainingMovement = (int)character.Movement.Current;
                character.TookAction = false;
                await character.PerformOnTurnStart();
                if (character.ContainingTile?.OnStood != null)
                {
                    await character.ContainingTile.OnStood.Do(character, character, true);
                }
            }
            LatestPlayerRemainingMovement = Player.RemainingMovement;

            if (Snapshot != null)
                while (!Snapshot.Read) await Task.Delay(10);

            var currentState = new List<DisplayEventDto>();
            currentState.Add(new()
            {
                DisplayEventType = DisplayEventType.SetDungeonStatus,
                Params = new() { Dungeon.DungeonStatus }
            });
            currentState.Add(new()
            {
                DisplayEventType = DisplayEventType.SetCanMove,
                Params = new() { Player.RemainingMovement > 0 }
            });
            currentState.Add(new()
            {
                DisplayEventType = DisplayEventType.SetCanAct,
                Params = new() { Player.CanTakeAction }
            });
            currentState.Add(new()
            {
                DisplayEventType = DisplayEventType.SetOnStairs,
                Params = new() { Player.ContainingTile.Type == TileType.Stairs }
            });
            DisplayEvents.Add(("Player Current State", currentState));
            Snapshot = new(Dungeon, this);
        }

        public async Task RollTurn0NPCs()
        {
            if (!FloorConfigurationToUse.PossibleMonsters.Any()) return;
            var totalGuaranteedNPCs = FloorConfigurationToUse.PossibleMonsters.Sum(mg => mg.MinimumInFirstTurn);
            foreach (var possibleNPC in FloorConfigurationToUse.PossibleMonsters)
            {
                for (int i = 0; i < possibleNPC.MinimumInFirstTurn; i++)
                {
                    var level = Rng.NextInclusive(possibleNPC.MinLevel, possibleNPC.MaxLevel);
                    var npc = await AddEntity(possibleNPC.ClassId, level) as NonPlayableCharacter;
                    npc.SpawnedViaMonsterHouse = false;
                    possibleNPC.TotalGeneratedInFloor++;
                    LastMonsterGenerationTurn = TurnCount;
                }
            }
            var minimumGenerations = Math.Max(0, FloorConfigurationToUse.SimultaneousMinMonstersAtStart - totalGuaranteedNPCs);
            var maximumGenerations = Math.Max(0, FloorConfigurationToUse.SimultaneousMaxMonstersInFloor - totalGuaranteedNPCs);
            var generationsToDo = Rng.NextInclusive(minimumGenerations, maximumGenerations);
            for (int i = 0; i < generationsToDo; i++)
            {
                await AddNPC();
            }
        }

        public async Task RollTurn0Items()
        {
            if (!FloorConfigurationToUse.PossibleItems.Any()) return;
            var totalGuaranteedItems = FloorConfigurationToUse.PossibleItems.Sum(ig => ig.MinimumInFirstTurn);
            foreach (var possibleItem in FloorConfigurationToUse.PossibleItems)
            {
                for (int i = 0; i < possibleItem.MinimumInFirstTurn; i++)
                {
                    await AddEntity(possibleItem.ClassId);
                    possibleItem.TotalGeneratedInFloor++;
                }
            }
            var minimumGenerations = Math.Max(0, FloorConfigurationToUse.MinItemsInFloor - totalGuaranteedItems);
            var maximumGenerations = Math.Max(0, FloorConfigurationToUse.MaxItemsInFloor - totalGuaranteedItems);
            var generationsToDo = Rng.NextInclusive(minimumGenerations, maximumGenerations);
            for (int i = 0; i < generationsToDo; i++)
            {
                await AddItem();
            }
        }

        public async Task RollTurn0Traps()
        {
            if (!FloorConfigurationToUse.PossibleTraps.Any()) return;
            var totalGuaranteedTraps = FloorConfigurationToUse.PossibleTraps.Sum(tg => tg.MinimumInFirstTurn);
            foreach (var possibleTrap in FloorConfigurationToUse.PossibleTraps)
            {
                for (int i = 0; i < possibleTrap.MinimumInFirstTurn; i++)
                {
                    await AddEntity(possibleTrap.ClassId);
                    possibleTrap.TotalGeneratedInFloor++;
                }
            }
            var minimumGenerations = Math.Max(0, FloorConfigurationToUse.MinTrapsInFloor - totalGuaranteedTraps);
            var maximumGenerations = Math.Max(0, FloorConfigurationToUse.MaxTrapsInFloor - totalGuaranteedTraps);
            var generationsToDo = Rng.NextInclusive(minimumGenerations, maximumGenerations);
            for (int i = 0; i < generationsToDo; i++)
            {
                await AddTrap();
            }
        }

        private async Task AddNPC()
        {
            if (!FloorConfigurationToUse.PossibleMonsters.Any() || TotalMonstersInFloor >= FloorConfigurationToUse.SimultaneousMaxMonstersInFloor) return;
            if (GeneratorToUse is StaticGenerator sg && !sg.GenerateNPCsAfterFirstTurn) return;
            List<ClassInFloor> usableNPCGenerators = new();
            FloorConfigurationToUse.PossibleMonsters.ForEach(pm =>
            {
                if ((TurnCount == 0 && !pm.CanSpawnOnFirstTurn) || (TurnCount > 0 && !pm.CanSpawnAfterFirstTurn) || (pm.OverallMaxForKindInFloor > 0 && pm.TotalGeneratedInFloor >= pm.OverallMaxForKindInFloor)) return;
                var currentMonstersWithId = AICharacters.Where(e => e.ClassId.Equals(pm.ClassId) && !e.SpawnedViaMonsterHouse && e.ExistenceStatus == EntityExistenceStatus.Alive);
                if (currentMonstersWithId.Count() >= pm.SimultaneousMaxForKindInFloor) return;
                if (!string.IsNullOrWhiteSpace(pm.SpawnCondition))
                {
                    var parsedCondition = ExpressionParser.ParseArgForExpression(pm.SpawnCondition, new EffectCallerParams {
                        This = Player,
                        Source = Player,
                        OriginalTarget = Player,
                        Target = Player
                    });
                    if (!ExpressionParser.CalculateBooleanExpression(parsedCondition)) return;
                }
                usableNPCGenerators.Add(pm);
            });
            var pickedGenerator = usableNPCGenerators.TakeRandomElementWithWeights(g => g.ChanceToPick, Rng);
            if (pickedGenerator == null) return;
            var level = Rng.NextInclusive(pickedGenerator.MinLevel, pickedGenerator.MaxLevel);
            var npc = await AddEntity(pickedGenerator.ClassId, level) as NonPlayableCharacter;
            npc.SpawnedViaMonsterHouse = false;
            pickedGenerator.TotalGeneratedInFloor++;
            LastMonsterGenerationTurn = TurnCount;
        }
        
        private async Task AddItem()
        {
            if (!FloorConfigurationToUse.PossibleItems.Any() || TotalItemsInFloor >= FloorConfigurationToUse.MaxItemsInFloor) return;
            List<ClassInFloor> usableItemGenerators = new();
            FloorConfigurationToUse.PossibleItems.ForEach(pi =>
            {
                if (pi.SimultaneousMaxForKindInFloor > 0 && pi.TotalGeneratedInFloor >= pi.SimultaneousMaxForKindInFloor) return;
                if (!string.IsNullOrWhiteSpace(pi.SpawnCondition))
                {
                    var parsedCondition = ExpressionParser.ParseArgForExpression(pi.SpawnCondition, new EffectCallerParams
                    {
                        This = Player,
                        Source = Player,
                        OriginalTarget = Player,
                        Target = Player
                    });
                    if (!ExpressionParser.CalculateBooleanExpression(parsedCondition)) return;
                }
                usableItemGenerators.Add(pi);
            });
            var pickedGenerator = usableItemGenerators.TakeRandomElementWithWeights(g => g.ChanceToPick, Rng);
            if (pickedGenerator == null) return;
            var itemLevel = Rng.NextInclusive(pickedGenerator.MinLevel, pickedGenerator.MaxLevel);
            await AddEntity(pickedGenerator.ClassId, itemLevel);
            pickedGenerator.TotalGeneratedInFloor++;
        }

        private async Task AddTrap()
        {
            if (!FloorConfigurationToUse.PossibleTraps.Any() || TotalTrapsInFloor >= FloorConfigurationToUse.MaxTrapsInFloor) return;
            List<ClassInFloor> usableTrapGenerators = new();
            FloorConfigurationToUse.PossibleTraps.ForEach(pt =>
            {
                if (pt.SimultaneousMaxForKindInFloor > 0 && pt.TotalGeneratedInFloor >= pt.SimultaneousMaxForKindInFloor) return;
                if (!string.IsNullOrWhiteSpace(pt.SpawnCondition))
                {
                    var parsedCondition = ExpressionParser.ParseArgForExpression(pt.SpawnCondition, new EffectCallerParams
                    {
                        This = Player,
                        Source = Player,
                        OriginalTarget = Player,
                        Target = Player
                    });
                    if (!ExpressionParser.CalculateBooleanExpression(parsedCondition)) return;
                }
                usableTrapGenerators.Add(pt);
            });
            var pickedGenerator = usableTrapGenerators.TakeRandomElementWithWeights(g => g.ChanceToPick, Rng);
            if (pickedGenerator == null) return;
            await AddEntity(pickedGenerator.ClassId);
            pickedGenerator.TotalGeneratedInFloor++;
        }

        private async Task ProcessTurn()
        {
            if (LatestPlayerRemainingMovement == Player.RemainingMovement && Player.CanTakeAction && !Player.TookAction) return;
            var minRequiredMovementToAct = (Player.RemainingMovement == 0 || !Player.CanTakeAction || Player.TookAction) ? 0 : LatestPlayerRemainingMovement;
            var aiCharactersThatCanActAlongsidePlayer = AICharacters.Where(c => c.ExistenceStatus == EntityExistenceStatus.Alive && ((c.RemainingMovement > 0 || c.Movement.Current == 0) && c.CanTakeAction && !c.TookAction && c.RemainingMovement >= minRequiredMovementToAct)).OrderByDescending(c => c.RemainingMovement).ToList();
            while (aiCharactersThatCanActAlongsidePlayer.Count > 0)
            {
                foreach (var aictca in aiCharactersThatCanActAlongsidePlayer)
                {
                    ClearIterationFlags();
                    await aictca.ProcessAI();
                }
                aiCharactersThatCanActAlongsidePlayer = AICharacters.Where(c => c.ExistenceStatus == EntityExistenceStatus.Alive && ((c.RemainingMovement > 0 || c.Movement.Current == 0) && c.CanTakeAction && !c.TookAction && c.RemainingMovement >= minRequiredMovementToAct)).OrderByDescending(c => c.RemainingMovement).ToList();
            }
            LatestPlayerRemainingMovement = Player.RemainingMovement;
            if (GetCharacters().TrueForAll(c => c.ExistenceStatus != EntityExistenceStatus.Alive || (c.RemainingMovement == 0 && c.Movement.Current > 0) || !c.CanTakeAction || c.TookAction))
            {
                while (!IsDebugMode && (AwaitingPromptInput || AwaitingQuestInput)) await Task.Delay(10);
                if (TurnCount == 0) return;
                await NewTurn();
            }
        }

        public PlayerInfoDto GetPlayerDetailInfo()
        {
            return new PlayerInfoDto(Player, this);
        }

        public async Task PlayerMove(int x, int y)
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
                        if (targetTile.DoorId != "")
                        {
                            var events = new List<DisplayEventDto>();
                            events.Add(new()
                            {
                                DisplayEventType = DisplayEventType.PlaySpecialEffect,
                                Params = new() { SpecialEffect.DoorClosed }
                            });
                            events.Add(new()
                            {
                                DisplayEventType = DisplayEventType.SetOnStairs,
                                Params = new() { currentTile.Type == TileType.Stairs }
                            });
                            AppendMessage(Locale["CharacterBumpedDoor"].Format(new { CharacterName = Player.Name, DoorName = Locale[$"DoorType{targetTile.DoorId}"] }), Color.White, events);
                            DisplayEvents.Add(("Bump door", events));
                        }
                        else
                        {
                            DisplayEvents.Add(("Normal bump", new()
                                {
                                    new() {
                                        DisplayEventType = DisplayEventType.PlaySpecialEffect,
                                        Params = new() { SpecialEffect.Bumped }
                                    },
                                    new()
                                    {
                                        DisplayEventType = DisplayEventType.SetOnStairs,
                                        Params = new() { currentTile.Type == TileType.Stairs }
                                    }
                                }
                            ));
                        }
                        return;
                    }
                    await TryMoveCharacter(Player, targetTile);
                }
            }
            await ProcessTurn();
        }

        public async Task<bool> TryMoveCharacter(Character character, Tile targetTile)
        {
            if(character == Player)
                Snapshot = new(Dungeon, this);
            if (character.ContainingTile == targetTile) return false;
            var events = new List<DisplayEventDto>();
            var initialTile = character.ContainingTile;
            var characterInTargetTile = GetCharacters().Find(c => c.ContainingTile == targetTile && c != character && c.ExistenceStatus == EntityExistenceStatus.Alive);
            if (characterInTargetTile != null)
            {
                if (character != Player) return false;
                if (characterInTargetTile.Movement.Current <= 0 || (characterInTargetTile is NonPlayableCharacter npc && npc.AIType == AIType.Null)) return false;
                if (!characterInTargetTile.CanTakeAction) return false;
                if (!character.Visible && characterInTargetTile.Visible) return false;
                if (characterInTargetTile.Visible && characterInTargetTile.Faction.IsEnemyWith(character.Faction)) return false;
                // Swap positions with allies, neutrals or invisibles
                characterInTargetTile.LatestPositions.AddButKeepingCapacity(characterInTargetTile.Position, 4);
                characterInTargetTile.Position = character.Position;
                characterInTargetTile.ContainingTile?.StoodOn(characterInTargetTile);
                if (characterInTargetTile.RemainingMovement > 0)
                    characterInTargetTile.RemainingMovement--;
                if (character == Player && !characterInTargetTile.Faction.IsEnemyWith(character.Faction))
                {
                    AppendMessage(Locale["CharacterSwitchedPlacesWithPlayer"].Format(new { CharacterName = characterInTargetTile.Name, PlayerName = Player.Name }), Color.DeepSkyBlue, events);
                }
            }

            character.LatestPositions.AddButKeepingCapacity(character.Position, 4);
            character.Position = targetTile.Position;


            var tilesToUpdate = new HashSet<GamePoint>();
            var oldFOV = character == Player && character.SightRange != EngineConstants.FullMapSightRange ? new List<Tile>(Player.FOVTiles) : null;

            if (!IsDebugMode)
            {
                if (character == Player || Player.FOVTiles.Contains(initialTile))
                    tilesToUpdate.Add(initialTile.Position);

                if (character == Player || Player.FOVTiles.Contains(targetTile))
                    tilesToUpdate.Add(targetTile.Position);
            }

            if (character == Player)
            {
                events.Add(new()
                {
                    DisplayEventType = DisplayEventType.UpdatePlayerPosition,
                    Params = new() { targetTile.Position }
                }
                );
                events.Add(new()
                {
                    DisplayEventType = DisplayEventType.SetOnStairs,
                    Params = new() { targetTile.Type == TileType.Stairs }
                }
                );
                Player.UpdateVisibility();

                if (oldFOV != null)
                {
                    foreach (var tile in oldFOV)
                        if (!tilesToUpdate.Contains(tile.Position))
                            tilesToUpdate.Add(tile.Position);

                    foreach (var tile in Player.FOVTiles)
                        if (!tilesToUpdate.Contains(tile.Position))
                            tilesToUpdate.Add(tile.Position);
                }
            }

            if (!IsDebugMode)
            {
                foreach (var pos in tilesToUpdate)
                {
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.UpdateTileRepresentation,
                        Params = new() { pos, GetConsoleRepresentationForCoordinates(pos.X, pos.Y) }
                    });
                }
            }

            if (character == Player)
            {
                if (targetTile.Room != null && !targetTile.Room.WasVisited)
                {
                    targetTile.Room.WasVisited = true;
                    if (targetTile.Room.MustSpawnMonsterHouse)
                        await InitiateMonsterHouse();
                }
            }

            DisplayEvents.Add(($"{character.Name} ({character.Id}) moves to ({targetTile.Position.X}, {targetTile.Position.Y})", events));

            if (targetTile.Key != null)
                character.TryToPickItem(targetTile.Key);
            targetTile.GetPickableObjects().Cast<IPickable>().ForEach(i => character.TryToPickItem(i));
            await targetTile.StoodOn(character);
            targetTile.Trap?.Stepped(character);

            character.RemainingMovement--;

            return true;
        }

        private async Task InitiateMonsterHouse()
        {
            var monsterHouseRoom = Rooms.Find(r => r.MustSpawnMonsterHouse);
            if (monsterHouseRoom == null) return;
            var events = new List<DisplayEventDto>();
            events.Add(
                    new()
                    {
                        DisplayEventType = DisplayEventType.PlaySpecialEffect,
                        Params = new() { SpecialEffect.MonsterHouseAlarm }
                    });
            AppendMessage(Locale["MonsterHouseWarningLogMessage"].Format(new { CharacterName = Player.Name }), Color.Red, events);
            AddMessageBox(Locale["MonsterHouseWarningHeader"], Locale["MonsterHouseWarningMessage"], "OK", new GameColor(Color.Red), events);
            var acceptableTiles = monsterHouseRoom.Tiles.Where(t => CanBeConsideredEmpty(t)).TakeNDifferentRandomElements(FloorConfigurationToUse.SimultaneousMaxMonstersInFloor, Rng);

            if (FloorConfigurationToUse.PossibleMonsters.Any())
            {
                foreach (var tile in acceptableTiles)
                {
                    var possibleMonsterClasses = PossibleNPCClasses.Where(pnc => FloorConfigurationToUse.PossibleMonsters.ConvertAll(pm => pm.ClassId).Contains(pnc.Id));
                    var pickedMonster = possibleMonsterClasses.Where(pm => pm.Faction.IsEnemyWith(Player.Faction) && pm.StartsVisible).TakeRandomElement(Rng);
                    var monsterSpawnData = FloorConfigurationToUse.PossibleMonsters.Find(pm => pm.ClassId.Equals(pickedMonster.Id));
                    if (pickedMonster == null || monsterSpawnData == null) return;
                    var monsterHouseEnemy = await AddEntity(pickedMonster.Id, Rng.NextInclusive(monsterSpawnData.MinLevel, monsterSpawnData.MaxLevel), tile.Position) as NonPlayableCharacter;
                    monsterHouseEnemy.SpawnedViaMonsterHouse = true;
                }
            }
            DisplayEvents.Add(("MONSTER HOUSE!", events));
        }

        public async Task PlayerUseStairs()
        {
            if (Player.ContainingTile != StairsTile)
                throw new ArgumentException($"Player is trying to use non-existent stairs at ({Player.ContainingTile.Position.X}, {Player.ContainingTile.Position.Y})");
            await Dungeon.TakeStairs();
        }
        public Task PlayerUseItemFromInventory(int itemId)
        {
            var item = Items.Find(i => i.Id == itemId);
            if(item == null)
                throw new ArgumentException("Player attempted to use an item that does not exist!");
            return PlayerUseItem(item);
        }

        private async Task PlayerUseItem(Item item)
        {
            ClearIterationFlags();
            DisplayEvents = new();
            Snapshot = new(Dungeon, this);
            if (!item.IsEquippable)
            {
                await item.Used(Player);
                await Player.UpdateQuests(QuestConditionType.UseItems, item.ItemType);
                await Player.UpdateQuests(QuestConditionType.UseItems, item);
                if (item.OnUse?.FinishesTurnWhenUsed == true)
                    Player.TookAction = true;
            }
            else
            {
                Player.EquipItem(item);
                Player.TookAction = true;
            }
            Player.RemainingMovement = 0;
            await ProcessTurn();
        }

        public async Task PlayerUseItemInFloor()
        {
            if (GetEntitiesFromCoordinates(Player.Position.X, Player.Position.Y)
                .Find(e => (e.EntityType == EntityType.Item)
                    && e.ExistenceStatus == EntityExistenceStatus.Alive && e.Passable) is not Item usableItem)
            {
                return;
            }
            await PlayerUseItem(usableItem);
        }
        public async Task PlayerPickUpItemInFloor()
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
                await ProcessTurn();
            }
        }

        public async Task PlayerDropItemFromInventory(int itemId)
        {
            var itemThatCanBeDropped = Items.Find(i => i.Id == itemId)
                ?? throw new ArgumentException("Player attempted to use an item that does not exist!");
            if (Player.ContainingTile.GetItems().Any())
            {
                AppendMessage(Locale["TileIsOccupied"].Format(new { CharacterName = Player.Name, ItemName = itemThatCanBeDropped.Name }), Color.Yellow);
            }
            else
            {
                Player.DropItem(itemThatCanBeDropped);
                Player.TookAction = true;
                Player.RemainingMovement = 0;
                await ProcessTurn();
            }
        }

        public async Task PlayerSwapFloorItemWithInventoryItem(int itemId)
        {
            var itemInInventory = Items.Find(i => i.Id == itemId)
                ?? throw new ArgumentException("Player attempted to swap with an item that does not exist!");
            var itemInTile = Items.Find(i => i.Position?.Equals(Player.Position) == true && i.ExistenceStatus != EntityExistenceStatus.Gone);
            if (itemInTile != null)
            {
                Player.PickItem(itemInTile, true);
                Player.DropItem(itemInInventory);
                Player.TookAction = true;
                Player.RemainingMovement = 0;
                await ProcessTurn();
            }
            else
            {
                throw new InvalidOperationException("Player attempted to pick an item from a tile without item!");
            }
        }

        public void PlayerAbandonQuest(int questId)
        {
            Player.AbandonQuest(questId);
        }

        public List<QuestDto> GetPlayerQuests()
        {
            var questDtos = new List<QuestDto>();
            foreach (var quest in Player.ActiveQuests)
            {
                questDtos.Add(new QuestDto(quest));
            }
            return questDtos;
        }

        public InventoryDto GetPlayerInventory()
        {
            var inventory = new InventoryDto();
            var itemsOnTile = Items.Where(i => i.Position?.Equals(Player.Position) == true).ToList();
            inventory.TileIsOccupied = itemsOnTile.Any();
            foreach (var item in Player.Equipment.OrderBy(item => Player.AvailableSlots.IndexOf(item.SlotsItOccupies[0])))
            {
                inventory.InventoryItems.Add(new InventoryItemDto(item, Player, this, false));
            }
            for (int i = 0; i < Player.Inventory.Count; i++)
            {
                inventory.InventoryItems.Add(new InventoryItemDto(Player.Inventory[i], Player, this, false));
            }
            for (int i = 0; i < Player.KeySet.Count; i++)
            {
                inventory.InventoryItems.Add(new InventoryItemDto(Player.KeySet[i], Player, this, false));
            }
            for (int i = 0; i < itemsOnTile.Count; i++)
            {
                inventory.InventoryItems.Add(new InventoryItemDto(itemsOnTile[i], Player, this, false));
            }
            inventory.CurrencyConsoleRepresentation = CurrencyClass.ConsoleRepresentation.Clone();
            return inventory;
        }

        public async Task PlayerAttackTargetWith(string selectionId, int x, int y, ActionSourceType sourceType)
        {
            ClearIterationFlags();

            var tile = GetTileFromCoordinates(x, y);
            var characterInTile = tile.LivingCharacter;

            var selectionIdParts = selectionId.Split('_');

            var correspondingEntity = sourceType == ActionSourceType.Player ? Player : characterInTile ?? throw new ArgumentException("Player attempted an Action belonging to a non-existent Entity.");

            ActionWithEffects selectedAction = null;

            if (correspondingEntity == Player)
            {
                selectedAction = Player.OnAttack.Find(oaa => oaa.SelectionId.Equals(selectionId))
                    ?? throw new ArgumentException("Player attempted a non-existent Action.");
            }
            else if (correspondingEntity == characterInTile && characterInTile is NonPlayableCharacter npc)
            {
                selectedAction = npc.OnInteracted.Find(oaa => oaa.SelectionId.Equals(selectionId))
                    ?? throw new ArgumentException("Player attempted a non-existent Action.");
            }

            if (sourceType == ActionSourceType.Player)
            {
                if (selectedAction.TargetTypes.Contains(TargetType.Tile) || selectedAction.TargetTypes.Contains(TargetType.Room))
                    await Player.InteractWithTile(tile, selectedAction);
                else if (characterInTile != null)
                    await Player.AttackCharacter(characterInTile, selectedAction);
                else
                    throw new ArgumentException("Player attempted use an action without a valid target.");
            }
            else if (sourceType == ActionSourceType.NPC)
            {
                await Player.InteractWithCharacter(characterInTile, selectedAction);
            }
            else
            {
                throw new ArgumentException("Player attempted a non-existent Action.");
            }

            if (selectedAction != null && selectedAction.User is Item i && i.ItemType.Usability == ItemUsability.Use)
            {
                await Player.UpdateQuests(QuestConditionType.UseItems, i.ItemType);
                await Player.UpdateQuests(QuestConditionType.UseItems, i);
            }

            await ProcessTurn();
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
            if (characterInTile != null && Player.CanSee(characterInTile))
                return new EntityDetailDto(characterInTile, tile, showTileDescription);
            var itemInTile = tile.GetPickableObjects().FirstOrDefault();
            if(itemInTile != null && Player.CanSee(itemInTile))
                return new EntityDetailDto(itemInTile, tile, showTileDescription);
            var trapInTile = tile.Trap;
            if (trapInTile != null && Player.CanSee(trapInTile))
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
            SetStairs(StairsPosition ?? PickEmptyPosition(true, false));
        }

        public GamePoint PickEmptyPosition(bool allowPlayerRoom, bool isItem)
        {
            int rngX = -1, rngY = -1;
            var nonDummyRooms = Rooms.Where(r => r.Tiles.Count > 1).Distinct().ToList();
            var playerRoom = Player?.Position != null ? GetRoomInCoordinates(Player.Position.X, Player.Position.Y) : null;
            if(nonDummyRooms.Count > 1 && playerRoom != null && !allowPlayerRoom)
                nonDummyRooms.Remove(playerRoom);
            var roomIsValid = false;
            do
            {
                roomIsValid = false;
                var possibleNonDummyRoom = nonDummyRooms.TakeRandomElement(Rng);
                var validEmptyTiles = possibleNonDummyRoom.Tiles.Where(t => (!isItem || t.Type.AcceptsItems) && CanBeConsideredEmpty(t));
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
            var entitiesList = new List<Entity>();

            foreach (var character in GetCharacters().Where(c => c != null))
            {
                entitiesList.Add(character);
            }

            entitiesList.AddRange(Items.Where(i => i != null));
            entitiesList.AddRange(Keys.Where(k => k != null));
            entitiesList.AddRange(Traps.Where(t => t != null));

            return entitiesList;
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
            return Rooms != null ? Rooms.Find(r => r.Tiles != null && r.Tiles.Any(t => t.Position.X == x && t.Position.Y == y)) : null;
        }

        public ConsoleRepresentation GetConsoleRepresentationForCoordinates(int x, int y)
        {
            var tile = GetTileFromCoordinates(x, y);
            if(tile == null)
                throw new ArgumentException("Tile does not exist");
            if (!tile.Discovered)
                return ConsoleRepresentation.EmptyTile;
            if (tile.Discovered && !tile.Visible)
            {
                if (tile.Trap != null && tile.Trap.Discovered && (!tile.Trap.ContainingTile.Type.CausesPartialInvisibility || tile.Trap.ContainingTile.Type == Player.ContainingTile.Type))
                {
                    return tile.Trap.ConsoleRepresentation.AsDarkened();
                }
                else
                {
                    return tile.ConsoleRepresentation.AsDarkened();
                }
            }
            if (tile.LivingCharacter != null)
            {
                if(tile.LivingCharacter.ExistenceStatus != EntityExistenceStatus.Alive && tile == StairsTile)
                        return tile.ConsoleRepresentation;

                var characterBaseConsoleRepresentation = new ConsoleRepresentation
                {
                    ForegroundColor = tile.LivingCharacter.ConsoleRepresentation.ForegroundColor.Clone(),
                    BackgroundColor = tile.LivingCharacter.ConsoleRepresentation.BackgroundColor.Clone(),
                    Character = tile.LivingCharacter.ConsoleRepresentation.Character
                };
                if ((tile.LivingCharacter == Player || tile.LivingCharacter.Faction.IsAlliedWith(Player.Faction)) && !tile.LivingCharacter.Visible)
                {
                    // Invisible players or allies will get their colors reversed
                    characterBaseConsoleRepresentation.BackgroundColor = tile.LivingCharacter.ConsoleRepresentation.ForegroundColor.Clone();
                    characterBaseConsoleRepresentation.ForegroundColor = tile.LivingCharacter.ConsoleRepresentation.BackgroundColor.Clone();
                    return characterBaseConsoleRepresentation;
                }
                else if (tile.LivingCharacter.CanBeSeenBy(Player))
                {
                    return characterBaseConsoleRepresentation;
                }
            }
            if (Player.ContainingTile == tile)
                return Player.ConsoleRepresentation;
            if (tile == StairsTile)
                return tile.ConsoleRepresentation;
            var visibleItems = tile.GetPickableObjects().Where(i => i.CanBeSeenBy(Player));
            if (visibleItems.Any())
                return visibleItems.First().ConsoleRepresentation;
            if (tile.Key != null)
                return tile.Key.ConsoleRepresentation;
            if (tile.Trap != null)
            {
                if (tile.Trap.CanBeSeenBy(Player) || (tile.Trap.Discovered && Player.CanSee(tile)))
                {
                    tile.Trap.Discovered = true;
                    return tile.Trap.ConsoleRepresentation;
                }
                else if (tile.Trap.Discovered && (!tile.Trap.ContainingTile.Type.CausesPartialInvisibility || tile.Trap.ContainingTile.Type == Player.ContainingTile.Type))
                {
                    return tile.Trap.ConsoleRepresentation.AsDarkened();
                }
            }
            if (tile.CurrencyPile != null)
                return tile.CurrencyPile.ConsoleRepresentation;
            var deadEntityInCoordinates = GetEntitiesFromCoordinates(tile.Position).Find(e => e.Passable && e.ExistenceStatus == EntityExistenceStatus.Dead);
            if (deadEntityInCoordinates != null && (!deadEntityInCoordinates.ContainingTile.Type.CausesPartialInvisibility || deadEntityInCoordinates.ContainingTile.Type == Player.ContainingTile.Type))
                return deadEntityInCoordinates.ConsoleRepresentation;
            return tile.ConsoleRepresentation;
        }

        #endregion

        #region Graph methods

        public List<Tile> GetPathBetweenTiles(GamePoint sourcePosition, GamePoint targetPosition)
        {
            return sourcePosition != null && targetPosition != null ? Tiles.GetShortestPathBetween((sourcePosition.X, sourcePosition.Y), (targetPosition.X, targetPosition.Y), true, t => t.Position.X, t => t.Position.Y, ArrayHelpers.GetSquaredEuclideanDistanceBetweenCells, GetTileConnectionWeight, t => t.IsWalkable) : new();
        }

        private double GetTileConnectionWeight(int x1, int y1, int x2, int y2)
        {
            var distance = ArrayHelpers.GetSquaredEuclideanDistanceBetweenCells(x1, y1, x2, y2);
            if (distance == 0) return 0;
            var tile1 = GetTileFromCoordinates(x1, y1);
            // Discourage but not prohibit walking on occupied tiles
            var tile2 = GetTileFromCoordinates(x2, y2);
            if (tile2.LivingCharacter != null)
                return distance + 8;
            // Discourage but not prohibit walking on visible traps or bad tile types
            var characterInTile1 = tile1.LivingCharacter;
            if (characterInTile1 != null && tile2.IsHarmfulFor(characterInTile1))
                return distance + 1;
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
        public List<Tile> GetTilesWithinCenteredSquare(GamePoint source, int distance, bool considerNonVisibles)
        {
            return Tiles.Where(t => (considerNonVisibles || t.Type.IsVisible) && Math.Round(GamePoint.ChebyshevDistance(source, t.Position), 0, MidpointRounding.AwayFromZero) <= distance).ToList();
        }
        public List<Tile> GetTilesWithinSquare(GamePoint source, int distance, bool considerNonVisibles)
        {
            return Tiles.Where(t => (considerNonVisibles || t.Type.IsVisible) && Math.Round(GamePoint.ManhattanDistance(source, t.Position), 0, MidpointRounding.AwayFromZero) <= distance).ToList();
        }
        public List<Tile> GetTilesWithinDistance(GamePoint source, int distance, bool considerNonVisibles)
        {
            return Tiles.Where(t => (considerNonVisibles || t.Type.IsVisible) && t.Type.IsVisible && Math.Round(GamePoint.Distance(source, t.Position), 0, MidpointRounding.AwayFromZero) <= distance).ToList();
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

        #region Prompts

        public Task<bool> OpenYesNoPrompt(string title, string message, string yesButtonText, string noButtonText, GameColor borderColor)
        {
            return PromptInvoker.OpenYesNoPrompt(title, message, yesButtonText, noButtonText, borderColor);
        }
        public Task<string> OpenSelectOption(string title, string message, SelectionItem[] choices, bool showCancelButton, GameColor borderColor)
        {
            return PromptInvoker.OpenSelectOption(title, message, choices, showCancelButton, borderColor);
        }
        public Task<ItemInput> OpenSelectItem(string title, InventoryDto choices, bool showCancelButton)
        {
            return PromptInvoker.OpenSelectItem(title, choices, showCancelButton);
        }
        public Task<string> OpenSelectAction(string title, ActionListDto choices, bool showCancelButton)
        {
            return PromptInvoker.OpenSelectAction(title, choices, showCancelButton);
        }
        public Task<int?> OpenBuyPrompt(string title, InventoryDto choices, bool showCancelButton)
        {
            return PromptInvoker.OpenBuyPrompt(title, choices, showCancelButton);
        }
        public Task<int?> OpenSellPrompt(string title, InventoryDto choices, bool showCancelButton)
        {
            return PromptInvoker.OpenSellPrompt(title, choices, showCancelButton);
        }
        public Task<string> OpenTextPrompt(string title, string message, string defaultText, GameColor borderColor)
        {
            return PromptInvoker.OpenTextPrompt(title, message, defaultText, borderColor);
        }

        #endregion

        #region Flags

        public bool HasFlag(string key)
        {
            return Flags.Exists(f => f.Key.Equals(key));
        }

        public object GetFlagValue(string key)
        {
            var flag = Flags.Find(f => f.Key.Equals(key)) 
                ?? throw new FlagNotFoundException($"There's no flag with key {key} in {FloorName}", key);
            return flag.Value;
        }

        public void CreateFlag(string key, object value, bool removeOnFloorChange)
        {
            Flags.Add(new Flag(key, value, removeOnFloorChange));
        }

        public void SetFlagValue(string key, object value)
        {
            var flag = Flags.Find(f => f.Key.Equals(key)) ?? throw new ArgumentException($"There's no flag with key {key} in {FloorName}");
            flag.Value = value;
        }

        #endregion

        #region Macrogame

        public Task ReturnToFloor1(int experiencePercentageToKeep, int equipmentPercentageToKeep, int inventoryPercentageToKeep, int learnedScriptsPercentageToKeep, int tagalongNPCsPercentageToKeep)
        {
            return Dungeon.ReturnToFloor1(experiencePercentageToKeep, equipmentPercentageToKeep, inventoryPercentageToKeep, learnedScriptsPercentageToKeep, tagalongNPCsPercentageToKeep);
        }

        #endregion

        #region Utils

        public Map Clone()
        {
            return JsonSerializer.Deserialize<Map>(JsonSerializer.Serialize(this));
        }

        public void RefreshDisplay(bool refreshWholeMap)
        {
            DisplayEvents = new();
            if (refreshWholeMap)
                Snapshot = new(Dungeon, this);
        }

        public void ClearIterationFlags()
        {
            foreach (var character in GetCharacters().Where(c => c.PickedForSwap))
            {
                character.PickedForSwap = false;
            }
            foreach (var tile in Tiles.Where(t => t.PickedForSwap))
            {
                tile.PickedForSwap = false;
            }
        }

        public void ResetEntityId()
        {
            Dungeon.CurrentEntityId = 1;
        }

        public int GenerateEntityId()
        {
            return Interlocked.Increment(ref Dungeon.CurrentEntityId);
        }

        #endregion
    }
    #pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
    #pragma warning restore CS8601 // Posible asignación de referencia nula
    #pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
    #pragma warning restore CS8604 // Posible argumento de referencia nulo
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
