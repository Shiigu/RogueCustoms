using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

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
            DungeonListForDisplay = new DungeonListDto(Constants.CurrentDungeonJsonVersion);
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
            var twoHoursAgo = DateTime.UtcNow.AddHours(-1 * Constants.HOURS_BEFORE_DUNGEON_CACHE_DELETION);
            Dungeons.RemoveAll(dungeon => dungeon.Id != dungeonId && dungeon.LastAccessTime < twoHoursAgo);
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
            return dungeon.EndingMessage;
        }

        public DungeonDto GetDungeonStatus(int dungeonId)
        {
            var dungeon = GetDungeonById(dungeonId);
            var dungeonStatus = dungeon.GetStatus();
            // A Dungeon's Message Boxes don't have to be sent more than once
            dungeon.MessageBoxes.Clear();
            // Remove a completed dungeon from memory to clear space
            if (dungeon.DungeonStatus == DungeonStatus.Completed)
                Dungeons.Remove(dungeon);
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
}
