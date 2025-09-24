using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO.Compression;
using RogueCustomsGameEngine.Utils.Representation;
using RogueCustomsGameEngine.Game.Entities;
using System.Drawing;
using System.Reflection;
using RogueCustomsGameEngine.Game.Interaction;
using System.Threading.Tasks;
using System.Xml.Linq;

#pragma warning disable SYSLIB0011 // El tipo o el miembro están obsoletos
namespace RogueCustomsGameEngine.Management
{
    [Serializable]
    public class DungeonManager
    {
        private Dungeon ActiveDungeon;
        public readonly Dictionary<string, DungeonInfo> AvailableDungeonInfos;
        private readonly DungeonListDto DungeonListForDisplay;

        public DungeonManager()
        {
            AvailableDungeonInfos = new Dictionary<string, DungeonInfo>();
            DungeonListForDisplay = new DungeonListDto(EngineConstants.CurrentDungeonJsonVersion);
        }

        private void GetDungeonList(string locale)
        {
            var dungeonsSinceLastRetrieval = AvailableDungeonInfos.Select(adi => adi.Key).ToList();
            var dungeonsInNewRetrieval = new List<string>();
            foreach (var file in Directory.GetFiles("./JSON/", "*.json"))
            {
                var dungeonInfo = GetDungeonInfoFromFile(file);
                if (dungeonInfo == null) continue;
                var fileName = Path.GetFileNameWithoutExtension(file);
                dungeonsInNewRetrieval.Add(fileName);
                AvailableDungeonInfos[fileName] = dungeonInfo;
                DungeonListForDisplay.AddDungeonToList(fileName, dungeonInfo, locale);
            }
            foreach(var removedDungeon in dungeonsSinceLastRetrieval.Except(dungeonsInNewRetrieval))
            {
                AvailableDungeonInfos.Remove(removedDungeon);
            }
        }

        public bool AddDungeonIfPossible(string filePath, string dungeonContents)
        {
            try
            {
                var parsedDungeon = JsonSerializer.Deserialize<DungeonInfo>(dungeonContents, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });
                File.Copy(filePath, $"./JSON/{Path.GetFileName(filePath)}", true);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SetPromptInvoker(IPromptInvoker promptInvoker)
        {
            ActiveDungeon.PromptInvoker = promptInvoker;
        }

        private static DungeonInfo? GetDungeonInfoFromFile(string path)
        {
            var jsonString = File.ReadAllText(path);
            return JsonSerializer.Deserialize<DungeonInfo>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
        }

        public DungeonListDto GetPickableDungeonList(string locale)
        {
            DungeonListForDisplay.Dungeons.Clear();
            GetDungeonList(locale);
            return DungeonListForDisplay;
        }

        public void CreateDungeon(string dungeonName, string locale, bool isHardcoreMode)
        {
            var dungeonInfo = AvailableDungeonInfos[dungeonName];

            ActiveDungeon = new Dungeon(dungeonInfo, locale, isHardcoreMode)
            {
                FileName = dungeonName,
                LastAccessTime = DateTime.UtcNow
            };
        }

        public DungeonSaveGameDto SaveDungeon()
        {
            using var memoryStream = new MemoryStream();
            using var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress, true);
            var formatter = new BinaryFormatter()
            {
                Binder = new CustomSerializationBinder()
            };
            AppContext.SetSwitch("System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization", true);
            formatter.Serialize(gzipStream, ActiveDungeon);
            return new DungeonSaveGameDto
            {
                FileName = ActiveDungeon.FileName,
                DungeonData = memoryStream.ToArray(),
                DungeonVersion = ActiveDungeon.Version
            };
        }

        public void LoadSavedDungeon(DungeonSaveGameDto dungeonSaveGame)
        {
            using var memoryStream = new MemoryStream(dungeonSaveGame.DungeonData);
            using var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress);
            var formatter = new BinaryFormatter()
            {
                Binder = new CustomSerializationBinder()
            };
            AppContext.SetSwitch("System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization", true);
            var restoredDungeon = formatter.Deserialize(gzipStream) as Dungeon;
            if (!restoredDungeon.Version.Equals(EngineConstants.CurrentDungeonJsonVersion))
                throw new InvalidDataException($"Deserialized Dungeon is at version {restoredDungeon.Version}. Required version is {EngineConstants.CurrentDungeonJsonVersion}.");
            var rngSeed = restoredDungeon.CurrentFloor.Rng.Seed;
            restoredDungeon.CurrentFloor.LoadRngState(rngSeed);
            restoredDungeon.CurrentFloor.SetActionParams();
            foreach (var tileType in restoredDungeon.TileTypes)
            {
                if (tileType.Id.Equals("Empty"))
                    TileType.Empty = tileType;
                else if (tileType.Id.Equals("Floor"))
                    TileType.Floor = tileType;
                else if (tileType.Id.Equals("Wall"))
                    TileType.Wall = tileType;
                else if (tileType.Id.Equals("Hallway"))
                    TileType.Hallway = tileType;
                else if (tileType.Id.Equals("Stairs"))
                    TileType.Stairs = tileType;
            }
            restoredDungeon.CurrentFloor.TileSet.TileTypeSets.ForEach(tts => tts.TileType.TileTypeSet = tts);
            restoredDungeon.CurrentFloor.DisplayEvents = new();
            restoredDungeon.CurrentFloor.DisplayEvents.Add(("ClearMessageLog", new()
                                {
                                    new() {
                                        DisplayEventType = DisplayEventType.ClearLogMessages,
                                        Params = new() { }
                                    }
                                }
            ));
            if (restoredDungeon.CurrentFloor.Messages != null)
            {
                foreach (var message in restoredDungeon.CurrentFloor.Messages)
                {
                    restoredDungeon.CurrentFloor.DisplayEvents.Add(("AppendMessage", new()
                {
                    new() {
                        DisplayEventType = DisplayEventType.AddLogMessage,
                        Params = new() { message }
                    }
                }
                    ));
                }
            }
            if (restoredDungeon.PlayerCharacter.ExistenceStatus == EntityExistenceStatus.Dead)
            {
                restoredDungeon.DungeonStatus = DungeonStatus.GameOver;
                restoredDungeon.CurrentFloor.DisplayEvents.Add(($"Player {restoredDungeon.PlayerName} is really dead", [new()
                    {
                        DisplayEventType = DisplayEventType.SetDungeonStatus,
                    Params = new() { DungeonStatus.GameOver }
                    }]));
            }
            else
            {
                restoredDungeon.CurrentFloor.DisplayEvents.Add(($"Player {restoredDungeon.PlayerName} may be on stairs", [new()
                    {
                        DisplayEventType = DisplayEventType.SetOnStairs,
                    Params = new() { restoredDungeon.CurrentFloor.Player.ContainingTile.Type == TileType.Stairs }
                    }]));
            }
            restoredDungeon.CurrentFloor.Snapshot.Read = false;
            restoredDungeon.CurrentFloor.Snapshot.JustLoaded = true;
            ConsoleRepresentation.EmptyTile = restoredDungeon.CurrentFloor.TileSet.Empty;
            ActiveDungeon = restoredDungeon;
        }

        public PlayerClassSelectionOutput GetPlayerClassSelection()
        {
            return new PlayerClassSelectionOutput(ActiveDungeon);
        }

        public void SetPlayerClassSelection(PlayerClassSelectionInput input)
        {
            ActiveDungeon.SetPlayerClass(input.ClassId);
            ActiveDungeon.SetPlayerName(input.Name);
        }

        public string GetDungeonWelcomeMessage()
        {
            return ActiveDungeon.WelcomeMessage;
        }

        public string GetDungeonEndingMessage()
        {
            return ActiveDungeon.EndingMessage;
        }

        public Task<DungeonDto> GetDungeonStatus()
        {
            return ActiveDungeon.GetStatus();
        }

        public Task MovePlayer(CoordinateInput input)
        {
            return ActiveDungeon.MovePlayer(input.X, input.Y);
        }

        public Task PlayerSkipTurn()
        {
            return ActiveDungeon.MovePlayer(0, 0);
        }

        public Task PlayerUseItemInFloor()
        {
            return ActiveDungeon.PlayerUseItemInFloor();
        }
        public Task PlayerPickUpItemInFloor()
        {
            return ActiveDungeon.PlayerPickUpItemInFloor();
        }
        public Task PlayerUseItemFromInventory(int itemId)
        {
            return ActiveDungeon.PlayerUseItemFromInventory(itemId);
        }
        public Task PlayerDropItemFromInventory(int itemId)
        {
            return ActiveDungeon.PlayerDropItemFromInventory(itemId);
        }
        public Task PlayerSwapFloorItemWithInventoryItem(int itemId)
        {
            return ActiveDungeon.PlayerSwapFloorItemWithInventoryItem(itemId);
        }

        public PlayerInfoDto GetPlayerDetailInfo()
        {
            return ActiveDungeon.GetPlayerDetailInfo();
        }

        public InventoryDto GetPlayerInventory()
        {
            return ActiveDungeon.GetPlayerInventory();
        }

        public ActionListDto GetPlayerAttackActions(int x, int y)
        {
            return ActiveDungeon.GetPlayerAttackActions(x, y);
        }
        public EntityDetailDto GetDetailsOfEntity(int x, int y)
        {
            return ActiveDungeon.GetDetailsOfEntity(x, y);
        }
        public Task PlayerAttackTargetWith(AttackInput input)
        {
            return ActiveDungeon.PlayerAttackTargetWith(input.SelectionId, input.X, input.Y, input.SourceType);
        }
        public Task PlayerTakeStairs()
        {
            return ActiveDungeon.PlayerTakeStairs();
        }
        public void RefreshDisplay(bool refreshWholeMap)
        {
            ActiveDungeon.RefreshDisplay(refreshWholeMap);
        }
    }

    public class CustomSerializationBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            return Type.GetType($"{typeName}, {assemblyName}");
        }
    }
}
#pragma warning restore SYSLIB0011 // El tipo o el miembro están obsoletos
