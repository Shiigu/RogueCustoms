using RogueCustomsGameEngine.Management;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsConsoleClient.UI.Consoles.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RogueCustomsConsoleClient.EngineHandling
{
    public class BackendHandler
    {
        private readonly ServerCaller ServerHandler;
        private readonly DungeonManager LocalHandler;

        public bool IsLocal;
        private string serverAddress;
        private readonly string LogFilePath;

        private int DungeonId;

        public static BackendHandler Instance { get; private set; }
        public string ServerAddress { 
            get { return serverAddress; }
            set {
                serverAddress = value;
                ServerHandler.Address = value;
            }
        }

        private BackendHandler(bool isLocal, string serverAddress = "")
        {
            this.serverAddress = serverAddress;
            ServerHandler = new ServerCaller(ServerAddress);
            LocalHandler = new DungeonManager();
            IsLocal = isLocal || string.IsNullOrWhiteSpace(ServerAddress);
            if (!Directory.Exists(Settings.Default.LocalLogFolder))
                Directory.CreateDirectory(Settings.Default.LocalLogFolder);
            LogFilePath = $"{Settings.Default.LocalLogFolder}/{DateTime.Now.ToString("s").Replace(":", "-")}.txt";
        }

        public static void CreateInstance(bool isLocal, string serverAddress = "")
        {
            Instance = new BackendHandler(isLocal, serverAddress);
        }

        private void WriteLog(Exception ex)
        {
            using var fs = File.Open(LogFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            using var sw = new StreamWriter(fs);
            sw.WriteLine($"AT {DateTime.Now:s}:");
            sw.WriteLine();
            sw.WriteLine(ex.GetType().Name);
            sw.WriteLine(ex.Message);
            sw.WriteLine("-------------");
            sw.WriteLine("Inner exception:");
            sw.WriteLine(ex.InnerException?.Message);
            sw.WriteLine(ex.InnerException?.StackTrace);
            sw.WriteLine("-------------");
            sw.WriteLine(ex.StackTrace);
            sw.WriteLine();
            sw.WriteLine("------------------------------------");
        }

        public List<DungeonListDto> GetPickableDungeonList(string locale)
        {
            try
            {
                List<DungeonListDto> DungeonList = null;
                if (IsLocal)
                    DungeonList = LocalHandler.GetPickableDungeonList(locale);
                else
                    Task.Run(async () => DungeonList = await ServerHandler.GetPickableDungeonList(locale)).Wait();
                return DungeonList;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                throw;
            }
        }

        public void CreateDungeon(string dungeonInternalName, string locale)
        {
            try
            {
                if (IsLocal)
                    DungeonId = LocalHandler.CreateDungeon(dungeonInternalName, locale);
                else
                    Task.Run(async () => DungeonId = await ServerHandler.CreateDungeon(dungeonInternalName, locale)).Wait();
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                throw;
            }
        }

        public string GetDungeonWelcomeMessage()
        {
            try
            {
                string welcomeMessage = null;
                if (IsLocal)
                    welcomeMessage = LocalHandler.GetDungeonWelcomeMessage(DungeonId);
                else
                    Task.Run(async () => welcomeMessage = await ServerHandler.GetDungeonWelcomeMessage(DungeonId)).Wait();
                return welcomeMessage;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                throw;
            }
        }
        public string GetDungeonEndingMessage()
        {
            try
            {
                string endingMessage = null;
                if (IsLocal)
                    endingMessage = LocalHandler.GetDungeonEndingMessage(DungeonId);
                else
                    Task.Run(async () => endingMessage = await ServerHandler.GetDungeonEndingMessage(DungeonId)).Wait();
                return endingMessage;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                throw;
            }
        }

        public DungeonDto GetDungeonStatus()
        {
            try
            {
                DungeonDto dungeonStatus = null;
                if (IsLocal)
                    dungeonStatus = LocalHandler.GetDungeonStatus(DungeonId);
                else
                    Task.Run(async () => dungeonStatus = await ServerHandler.GetDungeonStatus(DungeonId)).Wait();
                return dungeonStatus;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                throw;
            }
        }

        public void MovePlayer(CoordinateInput input)
        {
            try
            {
                if (IsLocal)
                    LocalHandler.MovePlayer(DungeonId, input);
                else
                    Task.Run(async () => await ServerHandler.MovePlayer(DungeonId, input)).Wait();
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                throw;
            }
        }

        public void PlayerUseItemInFloor()
        {
            try
            {
                if (IsLocal)
                    LocalHandler.PlayerUseItemInFloor(DungeonId);
                else
                    Task.Run(async () => await ServerHandler.PlayerUseItemInFloor(DungeonId)).Wait();
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                throw;
            }
        }

        public void PlayerPickUpItemInFloor()
        {
            try
            {
                if (IsLocal)
                    LocalHandler.PlayerPickUpItemInFloor(DungeonId);
                else
                    Task.Run(async () => await ServerHandler.PlayerPickUpItemInFloor(DungeonId)).Wait();
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                throw;
            }
        }

        public void PlayerUseItemFromInventory(int itemId)
        {
            try
            {
                if (IsLocal)
                    LocalHandler.PlayerUseItemFromInventory(DungeonId, itemId);
                else
                    Task.Run(async () => await ServerHandler.PlayerUseItemFromInventory(DungeonId, itemId)).Wait();
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                throw;
            }
        }
        public void PlayerDropItemFromInventory(int itemId)
        {
            try
            {
                if (IsLocal)
                    LocalHandler.PlayerDropItemFromInventory(DungeonId, itemId);
                else
                    Task.Run(async () => await ServerHandler.PlayerDropItemFromInventory(DungeonId, itemId)).Wait();
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                throw;
            }
        }
        public void PlayerSwapFloorItemWithInventoryItem(int itemId)
        {
            try
            {
                if (IsLocal)
                    LocalHandler.PlayerSwapFloorItemWithInventoryItem(DungeonId, itemId);
                else
                    Task.Run(async () => await ServerHandler.PlayerSwapFloorItemWithInventoryItem(DungeonId, itemId)).Wait();
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                throw;
            }
        }

        public void PlayerSkipTurn()
        {
            try
            {
                if (IsLocal)
                    LocalHandler.PlayerSkipTurn(DungeonId);
                else
                    Task.Run(async () => await ServerHandler.PlayerSkipTurn(DungeonId)).Wait();
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                throw;
            }
        }

        public void PlayerTakeStairs()
        {
            try
            {
                if (IsLocal)
                    LocalHandler.PlayerTakeStairs(DungeonId);
                else
                    Task.Run(async () => await ServerHandler.PlayerTakeStairs(DungeonId)).Wait();
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                throw;
            }
        }

        public ActionListDto GetPlayerAttackActions(int x, int y)
        {
            try
            {
                ActionListDto actions = null;
                if (IsLocal)
                    actions = LocalHandler.GetPlayerAttackActions(DungeonId, x, y);
                else
                    Task.Run(async () => actions = await ServerHandler.GetPlayerAttackActions(DungeonId, x, y)).Wait();
                return actions;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                throw;
            }
        }
        public PlayerInfoDto GetPlayerDetailInfo()
        {
            try
            {
                PlayerInfoDto playerInfo = null;
                if (IsLocal)
                    playerInfo = LocalHandler.GetPlayerDetailInfo(DungeonId);
                else
                    Task.Run(async () => playerInfo = await ServerHandler.GetPlayerDetailInfo(DungeonId)).Wait();
                return playerInfo;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                throw;
            }
        }


        public InventoryDto GetPlayerInventory()
        {
            try
            {
                InventoryDto inventory = null;
                if (IsLocal)
                    inventory = LocalHandler.GetPlayerInventory(DungeonId);
                else
                    Task.Run(async () => inventory = await ServerHandler.GetPlayerInventory(DungeonId)).Wait();
                return inventory;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                throw;
            }
        }

        public void PlayerAttackTargetWith(AttackInput input)
        {
            try
            {
                if (IsLocal)
                    LocalHandler.PlayerAttackTargetWith(DungeonId, input);
                else
                    Task.Run(async () => await ServerHandler.PlayerAttackTargetWith(DungeonId, input)).Wait();
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                throw;
            }
        }
    }
}
