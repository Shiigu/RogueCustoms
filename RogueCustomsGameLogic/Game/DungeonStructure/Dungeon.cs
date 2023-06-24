using RogueCustomsGameEngine.Utils.Representation;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Game.Entities;
using System.Drawing;
using RogueCustomsGameEngine.Utils.Enums;
using System.Collections.Generic;
using System;
using System.Linq;

namespace RogueCustomsGameEngine.Game.DungeonStructure
{
    public class Dungeon
    {
        public int Id { get; set; }
        public PlayerCharacter? PlayerCharacter { get; set; }
        public DungeonStatus DungeonStatus { get; set; }
        public Map CurrentFloor { get; private set; }

        private int CurrentFloorLevel;
        public readonly string WelcomeMessage;
        public readonly string EndingMessage;

        public List<string> Messages { get; set; }

        public List<MessageBoxDto> MessageBoxes { get; set; }

        #region From JSON
        public string Name { get; set; }
        public string Author { get; set; }
        public int AmountOfFloors { get; set; }
        public Locale LocaleToUse { get; set; }
        public List<FloorType> FloorTypes { get; set; }
        public List<EntityClass> Classes { get; set; }

        public readonly List<Faction> Factions;
        #endregion

        public Dungeon(int id, DungeonInfo dungeonInfo, string localeLanguage)
        {
            Id = id;
            Author = dungeonInfo.Author;
            AmountOfFloors = dungeonInfo.AmountOfFloors;
            Classes = new List<EntityClass>();
            var localeInfoToUse = dungeonInfo.Locales.Find(l => l.Language.Equals(localeLanguage))
                ?? dungeonInfo.Locales.Find(l => l.Language.Equals(dungeonInfo.DefaultLocale))
                ?? throw new Exception($"No locale data has been found for {localeLanguage}, and no default locale was defined.");
            LocaleToUse = new Locale(localeInfoToUse);
            Name = LocaleToUse[dungeonInfo.Name];
            WelcomeMessage = LocaleToUse[dungeonInfo.WelcomeMessage];
            EndingMessage = LocaleToUse[dungeonInfo.EndingMessage];
            dungeonInfo.Characters.ForEach(ci => Classes.Add(new EntityClass(ci, LocaleToUse)));
            dungeonInfo.Items.ForEach(ci => Classes.Add(new EntityClass(ci, LocaleToUse)));
            dungeonInfo.Traps.ForEach(ci => Classes.Add(new EntityClass(ci, LocaleToUse)));
            dungeonInfo.AlteredStatuses.ForEach(ci => Classes.Add(new EntityClass(ci, LocaleToUse)));
            FloorTypes = new List<FloorType>();
            dungeonInfo.FloorInfos.ForEach(fi => FloorTypes.Add(new FloorType(fi)));
            FloorTypes.ForEach(ft => ft.FillPossibleClassLists(Classes));
            Factions = new List<Faction>();
            dungeonInfo.FactionInfos.ForEach(fi => Factions.Add(new Faction(fi, LocaleToUse)));
            MapFactions();
            Messages = new List<string>();
            MessageBoxes = new List<MessageBoxDto>();
            CurrentFloorLevel = 1;
            PlayerCharacter = null;
            DungeonStatus = DungeonStatus.Running;
            NewMap();
        }

        private void MapFactions()
        {
            foreach (var faction in Factions)
            {
                faction.AlliedWithIds.ForEach(awi =>
                {
                    faction.AlliedWith.Add(Factions.Find(f => f.Id.Equals(awi))
                                ?? throw new Exception($"There's no faction with id {awi}"));
                });
                faction.NeutralWithIds.ForEach(nwi =>
                {
                    faction.NeutralWith.Add(Factions.Find(f => f.Id.Equals(nwi))
                                ?? throw new Exception($"There's no faction with id {nwi}"));
                });
                faction.EnemiesWithIds.ForEach(ewi =>
                {
                    faction.EnemiesWith.Add(Factions.Find(f => f.Id.Equals(ewi))
                                ?? throw new Exception($"There's no faction with id {ewi}"));
                });
            }
            foreach (var entityClass in Classes)
            {
                if (!string.IsNullOrWhiteSpace(entityClass.FactionId))
                    entityClass.Faction = Factions.Find(f => f.Id.Equals(entityClass.FactionId))
                                ?? throw new Exception($"There's no faction with id {entityClass.FactionId}");
            }
        }

        private void NewMap()
        {
            if (PlayerCharacter != null)
            {
                Messages.Clear();
                foreach (var status in PlayerCharacter.AlteredStatuses.Where(als => als.CleanseOnFloorChange))
                {
                    PlayerCharacter.MaxHPModifications?.RemoveAll(a => a.Id.Equals(status.Name));
                    PlayerCharacter.AttackModifications?.RemoveAll(a => a.Id.Equals(status.Name));
                    PlayerCharacter.DefenseModifications?.RemoveAll(a => a.Id.Equals(status.Name));
                    PlayerCharacter.MovementModifications?.RemoveAll(a => a.Id.Equals(status.Name));
                    PlayerCharacter.HPRegenerationModifications?.RemoveAll(a => a.Id.Equals(status.Name));
                }
                PlayerCharacter.AlteredStatuses.RemoveAll(als => als.CleanseOnFloorChange);
            }
            CurrentFloor = new Map(this, CurrentFloorLevel);
            CurrentFloor.Generate();
        }

        public void AddMessageBox(string title, string message, string buttonCaption, GameColor windowColor)
        {
            MessageBoxes.Add(new MessageBoxDto
            {
                Title = title,
                Message = message,
                ButtonCaption = buttonCaption,
                WindowColor = windowColor
            });
        }

        public void TakeStairs()
        {
            MessageBoxes.Clear();
            CurrentFloorLevel++;
            if (CurrentFloorLevel > AmountOfFloors)
            {
                CurrentFloor.TurnCount++;
                DungeonStatus = DungeonStatus.Completed;
                return;
            }
            NewMap();
        }

        public void MovePlayer(int x, int y)
        {
            MessageBoxes.Clear();
            CurrentFloor.PlayerMove(x, y);
        }

        public void PlayerUseItemInFloor()
        {
            MessageBoxes.Clear();
            CurrentFloor.PlayerUseItemInFloor();
        }
        public void PlayerPickUpItemInFloor()
        {
            MessageBoxes.Clear();
            CurrentFloor.PlayerPickUpItemInFloor();
        }
        public void PlayerUseItemFromInventory(int itemId)
        {
            MessageBoxes.Clear();
            CurrentFloor.PlayerUseItemFromInventory(itemId);
        }
        public void PlayerDropItemFromInventory(int itemId)
        {
            MessageBoxes.Clear();
            CurrentFloor.PlayerDropItemFromInventory(itemId);
        }
        public void PlayerSwapFloorItemWithInventoryItem(int itemId)
        {
            MessageBoxes.Clear();
            CurrentFloor.PlayerSwapFloorItemWithInventoryItem(itemId);
        }
        
        public PlayerInfoDto GetPlayerDetailInfo()
        {
            return CurrentFloor.GetPlayerDetailInfo();
        }

        public ActionListDto GetPlayerAttackActions(int x, int y)
        {
            return CurrentFloor.GetPlayerAttackActions(x, y);
        }
        public InventoryDto GetPlayerInventory()
        {
            return CurrentFloor.GetPlayerInventory();
        }
        public void PlayerAttackTargetWith(string name, int x, int y)
        {
            MessageBoxes.Clear();
            CurrentFloor.PlayerAttackTargetWith(name, x, y);
        }

        public void PlayerTakeStairs()
        {
            MessageBoxes.Clear();
            CurrentFloor.PlayerUseStairs();
        }
    }
}
