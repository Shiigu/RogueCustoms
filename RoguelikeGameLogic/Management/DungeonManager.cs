using RoguelikeGameEngine.Game.DungeonStructure;
using RoguelikeGameEngine.Utils.InputsAndOutputs;
using RoguelikeGameEngine.Utils.JsonImports;
using System.Text.Json;

namespace RoguelikeGameEngine.Management
{
    public class DungeonManager
    {
        private int CurrentDungeonId;
        private readonly List<Dungeon> Dungeons;
        private readonly Dictionary<string, DungeonInfo> AvailableDungeonInfos;
        private readonly List<DungeonListDto> DungeonListForDisplay;

        public DungeonManager()
        {
            CurrentDungeonId = 1;
            Dungeons = new List<Dungeon>();
            AvailableDungeonInfos = new Dictionary<string, DungeonInfo>();
            DungeonListForDisplay = new List<DungeonListDto>();
        }

        private void GetDungeonList()
        {
            var dungeonsSinceLastRetrieval = AvailableDungeonInfos.Select(adi => adi.Key).ToList();
            var dungeonsInNewRetrieval = new List<string>();
            foreach (var file in Directory.GetFiles("./JSON/", "*.json"))
            {
                var dungeonInfo = GetDungeonInfoFromFile(file);
                var fileName = Path.GetFileNameWithoutExtension(file);
                dungeonsInNewRetrieval.Add(fileName);
                AvailableDungeonInfos[fileName] = dungeonInfo;
                DungeonListForDisplay.Add(new DungeonListDto(fileName, dungeonInfo));
            }
            foreach(var removedDungeon in dungeonsSinceLastRetrieval.Except(dungeonsInNewRetrieval))
            {
                AvailableDungeonInfos.Remove(removedDungeon);
            }
        }

        private DungeonInfo GetDungeonInfoFromFile(string path)
        {
            var jsonString = File.ReadAllText(path);
            return JsonSerializer.Deserialize<DungeonInfo>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public List<DungeonListDto> GetPickableDungeonList()
        {
            DungeonListForDisplay.Clear();
            GetDungeonList();
            return DungeonListForDisplay;
        }

        public int CreateDungeon(string dungeonName)
        {
            var dungeonInfo = AvailableDungeonInfos[dungeonName];

            var dungeon = new Dungeon(CurrentDungeonId, dungeonInfo);
            Dungeons.Add(dungeon);
            CurrentDungeonId++;
            return dungeon.Id;
        }

        private Dungeon GetDungeonById(int id)
        {
            var dungeon = Dungeons.Find(d => d.Id == id);
            return dungeon == null ? throw new Exception("Dungeon does not exist") : dungeon;
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
            return new DungeonDto(GetDungeonById(dungeonId));
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

        public List<ActionItemDto> GetPlayerAttackActions(int dungeonId, int x, int y)
        {
            var dungeon = GetDungeonById(dungeonId);
            return dungeon.GetPlayerAttackActions(x, y);
        }
        public void PlayerAttackTargetWith(int dungeonId, AttackInput input)
        {
            var dungeon = GetDungeonById(dungeonId);
            dungeon.PlayerAttackTargetWith(input.Name, input.X, input.Y);
        }
        public void PlayerTakeStairs(int dungeonId)
        {
            var dungeon = GetDungeonById(dungeonId);
            dungeon.PlayerTakeStairs();
        }
    }
}
