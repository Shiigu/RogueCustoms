﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Roguelike.Services;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Roguelike.Controllers
{
    [Route("api")]
    [ApiController]
    public class DungeonController : ControllerBase
    {
        public readonly DungeonService DungeonService;
        private readonly ILogger<DungeonController> Logger;
        private readonly IMemoryCache Cache;

        public DungeonController(IMemoryCache cache, ILogger<DungeonController> logger)
        {
            Cache = cache ?? throw new ArgumentNullException(nameof(cache));
            Logger = logger;
            DungeonService = new DungeonService(cache);
        }

        [HttpPost("ClearCache")]
        public void ClearCache()
        {
            Cache.Remove("DungeonManager");
        }

        [HttpGet("GetPickableDungeonList/{locale}")]
        public DungeonListDto GetPickableDungeonList(string locale)
        {
            try
            {
                return DungeonService.GetPickableDungeonList(locale);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpPost("CreateDungeon/{dungeonName}/{locale}")]
        public int CreateDungeon(string dungeonName, string locale)
        {
            try
            {
                return DungeonService.CreateDungeon(dungeonName, locale);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpPost("SaveDungeon/{dungeonId}")]
        public DungeonSaveGameDto SaveDungeon(int dungeonId)
        {
            try
            {
                return DungeonService.SaveDungeon(dungeonId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpPost("LoadSavedDungeon")]
        public int LoadSavedDungeon([FromBody] DungeonSaveGameDto input)
        {
            try
            {
                return DungeonService.LoadSavedDungeon(input);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpGet("GetPlayerClassSelection/{dungeonId}")]
        public PlayerClassSelectionOutput GetPlayerClassSelection(int dungeonId)
        {
            try
            {
                return DungeonService.GetPlayerClassSelection(dungeonId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpPost("SetPlayerClassSelection/{dungeonId}")]
        public void SetPlayerClassSelection(int dungeonId, [FromBody] PlayerClassSelectionInput input)
        {
            try
            {
                DungeonService.SetPlayerClassSelection(dungeonId, input);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpGet("GetDungeonWelcomeMessage/{dungeonId}")]
        public string GetDungeonWelcomeMessage(int dungeonId)
        {
            try
            {
                return DungeonService.GetDungeonWelcomeMessage(dungeonId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpGet("GetDungeonEndingMessage/{dungeonId}")]
        public string GetDungeonEndingMessage(int dungeonId)
        {
            try
            {
                return DungeonService.GetDungeonEndingMessage(dungeonId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpGet("GetDungeonStatus/{dungeonId}")]
        public DungeonDto GetDungeonStatus(int dungeonId)
        {
            try
            {
                return DungeonService.GetDungeonStatus(dungeonId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpPost("MovePlayer/{dungeonId}")]
        public void MovePlayer(int dungeonId, [FromBody] CoordinateInput input)
        {
            try
            {
                DungeonService.MovePlayer(dungeonId, input);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpPost("PlayerSkipTurn/{dungeonId}")]
        public void PlayerSkipTurn(int dungeonId)
        {
            try
            {
                DungeonService.PlayerSkipTurn(dungeonId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpPost("PlayerUseItemInFloor/{dungeonId}")]
        public void PlayerUseItemInFloor(int dungeonId)
        {
            try
            {
                DungeonService.PlayerUseItemInFloor(dungeonId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }
        [HttpPost("PlayerPickUpItemInFloor/{dungeonId}")]
        public void PlayerPickUpItemInFloor(int dungeonId)
        {
            try
            {
                DungeonService.PlayerPickUpItemInFloor(dungeonId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }
        [HttpPost("PlayerUseItemFromInventory/{dungeonId}/{itemId}")]
        public void PlayerUseItemFromInventory(int dungeonId, int itemId)
        {
            try
            {
                DungeonService.PlayerUseItemFromInventory(dungeonId, itemId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }
        [HttpPost("PlayerDropItemFromInventory/{dungeonId}/{itemId}")]
        public void PlayerDropItemFromInventory(int dungeonId, int itemId)
        {
            try
            {
                DungeonService.PlayerDropItemFromInventory(dungeonId, itemId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpPost("PlayerSwapFloorItemWithInventoryItem/{dungeonId}/{itemId}")]
        public void PlayerSwapFloorItemWithInventoryItem(int dungeonId, int itemId)
        {
            try
            {
                DungeonService.PlayerSwapFloorItemWithInventoryItem(dungeonId, itemId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpGet("GetPlayerDetailInfo/{dungeonId}")]
        public PlayerInfoDto GetPlayerDetailInfo(int dungeonId)
        {
            try
            {
                return DungeonService.GetPlayerDetailInfo(dungeonId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpGet("GetPlayerInventory/{dungeonId}")]
        public InventoryDto GetPlayerInventory(int dungeonId)
        {
            try
            {
                return DungeonService.GetPlayerInventory(dungeonId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpGet("GetPlayerAttackActions/{dungeonId}/{x}/{y}")]
        public ActionListDto GetPlayerAttackActions(int dungeonId, int x, int y)
        {
            try
            {
                return DungeonService.GetPlayerAttackActions(dungeonId, x, y);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpGet("GetDetailsOfEntity/{dungeonId}/{x}/{y}")]
        public EntityDetailDto GetDetailsOfEntity(int dungeonId, int x, int y)
        {
            try
            {
                return DungeonService.GetDetailsOfEntity(dungeonId, x, y);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpPost("PlayerAttackTargetWith/{dungeonId}")]
        public void PlayerAttackTargetWith(int dungeonId, [FromBody] AttackInput input)
        {
            try
            {
                DungeonService.PlayerAttackTargetWith(dungeonId, input);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpPost("PlayerTakeStairs/{dungeonId}")]
        public void PlayerTakeStairs(int dungeonId)
        {
            try
            {
                DungeonService.PlayerTakeStairs(dungeonId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpGet("GetDungeonInfoTemplate")]
        public DungeonInfo GetDungeonInfoTemplate()
        {
            return new DungeonInfo();
        }
    }
}
