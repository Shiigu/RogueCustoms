using Microsoft.Extensions.Caching.Memory;
using RogueCustomsGameEngine.Management;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using System.Collections.Generic;

namespace Roguelike.Services
{
    public class DungeonService
    {
        private readonly IMemoryCache Cache;
        private readonly DungeonManager DungeonManager;

        public DungeonService(IMemoryCache cache)
        {
            Cache = cache;
            if (!Cache.TryGetValue("DungeonManager", out DungeonManager))
            {
                DungeonManager = new DungeonManager();
                Cache.Set("DungeonManager", DungeonManager);
            }
        }
        public DungeonListDto GetPickableDungeonList(string locale)
        {
            return DungeonManager.GetPickableDungeonList(locale);
        }

        public int CreateDungeon(string dungeonName, string locale)
        {
            return DungeonManager.CreateDungeon(dungeonName, locale);
        }

        public PlayerClassSelectionOutput GetPlayerClassSelection(int dungeonId)
        {
            return DungeonManager.GetPlayerClassSelection(dungeonId);
        }

        public void SetPlayerClassSelection(int dungeonId, PlayerClassSelectionInput input)
        {
            DungeonManager.SetPlayerClassSelection(dungeonId, input);
        }

        public DungeonDto GetDungeonStatus(int dungeonId)
        {
            return DungeonManager.GetDungeonStatus(dungeonId);
        }
        public string GetDungeonWelcomeMessage(int dungeonId)
        {
            return DungeonManager.GetDungeonWelcomeMessage(dungeonId);
        }
        public string GetDungeonEndingMessage(int dungeonId)
        {
            return DungeonManager.GetDungeonEndingMessage(dungeonId);
        }

        public void MovePlayer(int dungeonId, CoordinateInput input)
        {
            DungeonManager.MovePlayer(dungeonId, input);
        }

        public void PlayerSkipTurn(int dungeonId)
        {
            DungeonManager.PlayerSkipTurn(dungeonId);
        }

        public void PlayerUseItemInFloor(int dungeonId)
        {
            DungeonManager.PlayerUseItemInFloor(dungeonId);
        }

        public void PlayerPickUpItemInFloor(int dungeonId)
        {
            DungeonManager.PlayerPickUpItemInFloor(dungeonId);
        }

        public void PlayerUseItemFromInventory(int dungeonId, int itemId)
        {
            DungeonManager.PlayerUseItemFromInventory(dungeonId, itemId);
        }

        public void PlayerDropItemFromInventory(int dungeonId, int itemId)
        {
            DungeonManager.PlayerDropItemFromInventory(dungeonId, itemId);
        }

        public void PlayerSwapFloorItemWithInventoryItem(int dungeonId, int itemId)
        {
            DungeonManager.PlayerSwapFloorItemWithInventoryItem(dungeonId, itemId);
        }

        public PlayerInfoDto GetPlayerDetailInfo(int dungeonId)
        {
            return DungeonManager.GetPlayerDetailInfo(dungeonId);
        }

        public InventoryDto GetPlayerInventory(int dungeonId)
        {
            return DungeonManager.GetPlayerInventory(dungeonId);
        }

        public ActionListDto GetPlayerAttackActions(int dungeonId, int x, int y)
        {
            return DungeonManager.GetPlayerAttackActions(dungeonId, x, y);
        }

        public EntityDetailDto GetDetailsOfEntity(int dungeonId, int x, int y)
        {
            return DungeonManager.GetDetailsOfEntity(dungeonId, x, y);
        }

        public void PlayerAttackTargetWith(int dungeonId, AttackInput input)
        {
            DungeonManager.PlayerAttackTargetWith(dungeonId, input);
        }

        public void PlayerTakeStairs(int dungeonId)
        {
            DungeonManager.PlayerTakeStairs(dungeonId);
        }
    }
}
