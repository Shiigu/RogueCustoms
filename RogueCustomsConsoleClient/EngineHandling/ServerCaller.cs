using Newtonsoft.Json;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsConsoleClient.EngineHandling
{
    public sealed class ServerCaller
    {
        private readonly HttpClient client;
        public string Address;

        public ServerCaller(string address)
        {
            client = new()
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            Address = address;
        }
        public async Task<List<DungeonListDto>> GetPickableDungeonList(string locale)
        {
            var json = await client.GetStringAsync($"{Address}/api/GetPickableDungeonList/{locale}");
            return JsonConvert.DeserializeObject<List<DungeonListDto>>(json);
        }

        public async Task<int> CreateDungeon(string dungeonName, string locale)
        {
            var response = await client.PostAsync($"{Address}/api/CreateDungeon/{dungeonName}/{locale}", null);
            var contents = await response.Content.ReadAsStringAsync();
            return int.Parse(contents);
        }

        public async Task<string> GetDungeonWelcomeMessage(int dungeonId)
        {
            var response = await client.GetStringAsync($"{Address}/api/GetDungeonWelcomeMessage/{dungeonId}");
            return response;
        }
        public async Task<string> GetDungeonEndingMessage(int dungeonId)
        {
            var response = await client.GetStringAsync($"{Address}/api/GetDungeonEndingMessage/{dungeonId}");
            return response;
        }

        public async Task<DungeonDto> GetDungeonStatus(int dungeonId)
        {
            var json = await client.GetStringAsync($"{Address}/api/GetDungeonStatus/{dungeonId}");
            return JsonConvert.DeserializeObject<DungeonDto>(json);
        }

        public async Task MovePlayer(int dungeonId, CoordinateInput input)
        {
            var content = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
            await client.PostAsync($"{Address}/api/MovePlayer/{dungeonId}", content);
        }

        public async Task PlayerUseItemInFloor(int dungeonId)
        {
            await client.PostAsync($"{Address}/api/PlayerUseItemInFloor/{dungeonId}", null);
        }
        public async Task PlayerPickUpItemInFloor(int dungeonId)
        {
            await client.PostAsync($"{Address}/api/PlayerPickUpItemInFloor/{dungeonId}", null);
        }
        public async Task PlayerUseItemFromInventory(int dungeonId, int itemId)
        {
            await client.PostAsync($"{Address}/api/PlayerUseItemFromInventory/{dungeonId}/{itemId}", null);
        }
        public async Task PlayerDropItemFromInventory(int dungeonId, int itemId)
        {
            await client.PostAsync($"{Address}/api/PlayerDropItemFromInventory/{dungeonId}/{itemId}", null);
        }
        public async Task PlayerSwapFloorItemWithInventoryItem(int dungeonId, int itemId)
        {
            await client.PostAsync($"{Address}/api/PlayerSwapFloorItemWithInventoryItem/{dungeonId}/{itemId}", null);
        }

        public async Task PlayerSkipTurn(int dungeonId)
        {
            await client.PostAsync($"{Address}/api/PlayerSkipTurn/{dungeonId}", null);
        }

        public async Task PlayerTakeStairs(int dungeonId)
        {
            await client.PostAsync($"{Address}/api/PlayerTakeStairs/{dungeonId}", null);
        }

        public async Task<ActionListDto> GetPlayerAttackActions(int dungeonId, int x, int y)
        {
            var json = await client.GetStringAsync($"{Address}/api/GetPlayerAttackActions/{dungeonId}/{x}/{y}");
            return JsonConvert.DeserializeObject<ActionListDto>(json);
        }
        public async Task<InventoryDto> GetPlayerInventory(int dungeonId)
        {
            var json = await client.GetStringAsync($"{Address}/api/GetPlayerInventory/{dungeonId}");
            return JsonConvert.DeserializeObject<InventoryDto>(json);
        }
        public async Task<PlayerInfoDto> GetPlayerDetailInfo(int dungeonId)
        {
            var json = await client.GetStringAsync($"{Address}/api/GetPlayerDetailInfo/{dungeonId}");
            return JsonConvert.DeserializeObject<PlayerInfoDto>(json);
        }

        public async Task PlayerAttackTargetWith(int dungeonId, AttackInput input)
        {
            var content = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
            await client.PostAsync($"{Address}/api/PlayerAttackTargetWith/{dungeonId}", content);
        }
    }
}
