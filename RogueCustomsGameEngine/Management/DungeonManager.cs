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

namespace RogueCustomsGameEngine.Management
{
    [Serializable]
    public class DungeonManager
    {
        private int CurrentDungeonId;
        private readonly List<Dungeon> Dungeons;
        private readonly Dictionary<string, DungeonInfo> AvailableDungeonInfos;
        private readonly DungeonListDto DungeonListForDisplay;

        public DungeonManager()
        {
            CurrentDungeonId = 1;
            Dungeons = new List<Dungeon>();
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

        public int CreateDungeon(string dungeonName, string locale)
        {
            var dungeonInfo = AvailableDungeonInfos[dungeonName];

            var dungeon = new Dungeon(CurrentDungeonId, dungeonInfo, locale)
            {
                LastAccessTime = DateTime.UtcNow
            };
            Dungeons.Add(dungeon);
            CurrentDungeonId++;
            return dungeon.Id;
        }

        private Dungeon GetDungeonById(int id)
        {
            var dungeon = Dungeons.Find(d => d.Id == id);
            return dungeon ?? throw new ArgumentException("Dungeon does not exist");
        }

        public void UpdateAccessTimeAndCleanupUnusedDungeons(int dungeonId)
        {
            var dungeon = GetDungeonById(dungeonId);
            if (dungeon != null)
                dungeon.LastAccessTime = DateTime.UtcNow;
            var twoHoursAgo = DateTime.UtcNow.AddHours(-1 * EngineConstants.HOURS_BEFORE_DUNGEON_CACHE_DELETION);
            Dungeons.RemoveAll(dungeon => dungeon.Id != dungeonId && dungeon.LastAccessTime < twoHoursAgo);
        }

        public DungeonSaveGameDto SaveDungeon(int dungeonId)
        {
            var dungeon = GetDungeonById(dungeonId);

            using var memoryStream = new MemoryStream();
            using var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress);
            var formatter = new BinaryFormatter();
            formatter.Serialize(gzipStream, dungeon);
            return new DungeonSaveGameDto
            {
                DungeonData = memoryStream.ToArray()
            };
        }

        public int LoadSavedDungeon(DungeonSaveGameDto dungeonSaveGame)
        {
            using var memoryStream = new MemoryStream(dungeonSaveGame.DungeonData);
            using var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress);
            IFormatter formatter = new BinaryFormatter()
            {
                Binder = new CustomSerializationBinder()
            };
            var restoredDungeon = formatter.Deserialize(gzipStream) as Dungeon;
            if (!restoredDungeon.Version.Equals(EngineConstants.CurrentDungeonJsonVersion))
                throw new InvalidDataException($"Deserialized Dungeon is at version {restoredDungeon.Version}. Required version is {EngineConstants.CurrentDungeonJsonVersion}.");
            restoredDungeon.Id = CurrentDungeonId;
            var rngSeed = restoredDungeon.CurrentFloor.Rng.Seed;
            restoredDungeon.CurrentFloor.LoadRngState(rngSeed);
            restoredDungeon.CurrentFloor.SetActionParams();
            ConsoleRepresentation.EmptyTile = restoredDungeon.CurrentFloor.TileSet.Empty;
            Dungeons.Add(restoredDungeon);
            CurrentDungeonId++;
            return restoredDungeon.Id;
        }

        public PlayerClassSelectionOutput GetPlayerClassSelection(int dungeonId)
        {
            var dungeon = GetDungeonById(dungeonId);
            return new PlayerClassSelectionOutput(dungeon);
        }

        public void SetPlayerClassSelection(int dungeonId, PlayerClassSelectionInput input)
        {
            var dungeon = GetDungeonById(dungeonId);
            dungeon.SetPlayerClass(input.ClassId);
            dungeon.SetPlayerName(input.Name);
        }

        public string GetDungeonWelcomeMessage(int dungeonId)
        {
            var dungeon = GetDungeonById(dungeonId);
            return dungeon.WelcomeMessage;
        }

        public string GetDungeonEndingMessage(int dungeonId)
        {
            var dungeon = GetDungeonById(dungeonId);
            // Remove a completed dungeon from memory to clear space
            if (dungeon.DungeonStatus == DungeonStatus.Completed)
                Dungeons.Remove(dungeon);
            return dungeon.EndingMessage;
        }

        public DungeonDto GetDungeonStatus(int dungeonId)
        {
            var dungeon = GetDungeonById(dungeonId);

            var dungeonStatus = dungeon.GetStatus();

            // A Dungeon's Message Boxes don't have to be sent more than once
            dungeon.MessageBoxes.Clear();

            return dungeonStatus;
        }

        public void MovePlayer(int dungeonId, CoordinateInput input)
        {
            var dungeon = GetDungeonById(dungeonId);
            dungeon.MovePlayer(input.X, input.Y);
        }

        public void PlayerSkipTurn(int dungeonId)
        {
            var dungeon = GetDungeonById(dungeonId);
            dungeon.MovePlayer(0, 0);
        }

        public void PlayerUseItemInFloor(int dungeonId)
        {
            var dungeon = GetDungeonById(dungeonId);
            dungeon.PlayerUseItemInFloor();
        }
        public void PlayerPickUpItemInFloor(int dungeonId)
        {
            var dungeon = GetDungeonById(dungeonId);
            dungeon.PlayerPickUpItemInFloor();
        }
        public void PlayerUseItemFromInventory(int dungeonId, int itemId)
        {
            var dungeon = GetDungeonById(dungeonId);
            dungeon.PlayerUseItemFromInventory(itemId);
        }
        public void PlayerDropItemFromInventory(int dungeonId, int itemId)
        {
            var dungeon = GetDungeonById(dungeonId);
            dungeon.PlayerDropItemFromInventory(itemId);
        }
        public void PlayerSwapFloorItemWithInventoryItem(int dungeonId, int itemId)
        {
            var dungeon = GetDungeonById(dungeonId);
            dungeon.PlayerSwapFloorItemWithInventoryItem(itemId);
        }

        public PlayerInfoDto GetPlayerDetailInfo(int dungeonId)
        {
            var dungeon = GetDungeonById(dungeonId);
            return dungeon.GetPlayerDetailInfo();
        }

        public InventoryDto GetPlayerInventory(int dungeonId)
        {
            var dungeon = GetDungeonById(dungeonId);
            return dungeon.GetPlayerInventory();
        }

        public ActionListDto GetPlayerAttackActions(int dungeonId, int x, int y)
        {
            var dungeon = GetDungeonById(dungeonId);
            return dungeon.GetPlayerAttackActions(x, y);
        }
        public EntityDetailDto GetDetailsOfEntity(int dungeonId, int x, int y)
        {
            var dungeon = GetDungeonById(dungeonId);
            return dungeon.GetDetailsOfEntity(x, y);
        }
        public void PlayerAttackTargetWith(int dungeonId, AttackInput input)
        {
            var dungeon = GetDungeonById(dungeonId);
            dungeon.PlayerAttackTargetWith(input.SelectionId, input.X, input.Y);
        }
        public void PlayerTakeStairs(int dungeonId)
        {
            var dungeon = GetDungeonById(dungeonId);
            dungeon.PlayerTakeStairs();
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
