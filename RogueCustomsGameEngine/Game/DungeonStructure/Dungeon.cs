using RogueCustomsGameEngine.Utils.Representation;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Game.Entities;
using System.Drawing;
using RogueCustomsGameEngine.Utils.Enums;
using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using System.Collections.Immutable;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Collections;
using RogueCustomsGameEngine.Utils.Helpers;

namespace RogueCustomsGameEngine.Game.DungeonStructure
{
#pragma warning disable CS8604 // Posible argumento de referencia nulo
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
    [Serializable]
    public class Dungeon
    {
        public DateTime LastAccessTime { get; set; }
        public PlayerCharacter? PlayerCharacter { get; set; }
        public EntityClass? PlayerClass { get; set; }
        public string? PlayerName { get; set; }
        public string Version { get; set; }
        public string FileName { get; set; }
        public DungeonStatus DungeonStatus { get; set; }
        public Map CurrentFloor { get; private set; }


        public int CurrentEntityId { get; set; }
        private int CurrentFloorLevel;
        public readonly string WelcomeMessage;
        public readonly string EndingMessage;
        public bool IsDebugMode { get; set; }
        public bool IsHardcoreMode { get; set; }
        public List<ActionWithEffects> Scripts { get; private set; }

        #region From JSON
        public string Name { get; set; }
        public string Author { get; set; }
        public int AmountOfFloors { get; set; }
        public Locale LocaleToUse { get; set; }
        public List<TileSet> TileSets { get; set; }
        public List<FloorType> FloorTypes { get; set; }
        public List<Element> Elements { get; set; }
        public List<EntityClass> Classes { get; set; }

        public List<Faction> Factions { get; private set; }
        public List<TileType> TileTypes { get; private set; }
        #endregion

        public Dungeon(DungeonInfo dungeonInfo, string localeLanguage, bool isHardcoreMode)
        {
            CurrentEntityId = 1;
            Version = dungeonInfo.Version;
            Author = dungeonInfo.Author;
            AmountOfFloors = dungeonInfo.AmountOfFloors;
            Elements = new List<Element>();
            Classes = new List<EntityClass>();
            TileTypes = new List<TileType>();
            TileSets = new List<TileSet>();
            var localeInfoToUse = dungeonInfo.Locales.Find(l => l.Language.Equals(localeLanguage))
                ?? dungeonInfo.Locales.Find(l => l.Language.Equals(dungeonInfo.DefaultLocale))
                ?? throw new FormatException($"No locale data has been found for {localeLanguage}, and no default locale was defined.");
            LocaleToUse = new Locale(localeInfoToUse);
            Name = LocaleToUse[dungeonInfo.Name];
            WelcomeMessage = LocaleToUse[dungeonInfo.WelcomeMessage];
            EndingMessage = LocaleToUse[dungeonInfo.EndingMessage];
            dungeonInfo.TileTypeInfos.ForEach(tl => TileTypes.Add(new TileType(tl)));
            dungeonInfo.TileSetInfos.ForEach(ts => TileSets.Add(new TileSet(ts, this)));
            dungeonInfo.ElementInfos.ForEach(e => Elements.Add(new Element(e, LocaleToUse)));
            dungeonInfo.PlayerClasses.ForEach(ci => Classes.Add(new EntityClass(ci, LocaleToUse, EntityType.Player, dungeonInfo.CharacterStats)));
            dungeonInfo.NPCs.ForEach(ci => Classes.Add(new EntityClass(ci, LocaleToUse, EntityType.NPC, dungeonInfo.CharacterStats)));
            dungeonInfo.Items.ForEach(ci => Classes.Add(new EntityClass(ci, LocaleToUse, null, null)));
            dungeonInfo.Traps.ForEach(ci => Classes.Add(new EntityClass(ci, LocaleToUse, EntityType.Trap, null)));
            dungeonInfo.AlteredStatuses.ForEach(ci => Classes.Add(new EntityClass(ci, LocaleToUse, EntityType.AlteredStatus, null)));
            FloorTypes = new List<FloorType>();
            dungeonInfo.FloorInfos.ForEach(fi => FloorTypes.Add(new FloorType(fi, LocaleToUse, TileTypes)));
            FloorTypes.ForEach(ft => {
                ft.TileSet = TileSets.Find(ts => ts.Id.Equals(ft.TileSetId))
                    ?? throw new FormatException($"No TileSet with id {ft.TileSetId} was found.");
                ft.FillPossibleClassLists(Classes);
            });
            Factions = new List<Faction>();
            dungeonInfo.FactionInfos.ForEach(fi => Factions.Add(new Faction(fi, LocaleToUse)));
            Scripts = new();
            dungeonInfo.Scripts.ForEach(s => Scripts.Add(ActionWithEffects.Create(s)));
            IsHardcoreMode = isHardcoreMode;
            MapFactions();
            CurrentFloorLevel = 1;
            PlayerCharacter = null;
            DungeonStatus = DungeonStatus.Running;
        }

        private void MapFactions()
        {
            foreach (var faction in Factions)
            {
                faction.AlliedWithIds.ForEach(awi =>
                {
                    faction.AlliedWith.Add(Factions.Find(f => f.Id.Equals(awi))
                                ?? throw new InvalidDataException($"There's no faction with id {awi}"));
                });
                faction.NeutralWithIds.ForEach(nwi =>
                {
                    faction.NeutralWith.Add(Factions.Find(f => f.Id.Equals(nwi))
                                ?? throw new InvalidDataException($"There's no faction with id {nwi}"));
                });
                faction.EnemiesWithIds.ForEach(ewi =>
                {
                    faction.EnemiesWith.Add(Factions.Find(f => f.Id.Equals(ewi))
                                ?? throw new InvalidDataException($"There's no faction with id {ewi}"));
                });
            }
            foreach (var entityClass in Classes)
            {
                if (!string.IsNullOrWhiteSpace(entityClass.FactionId))
                {
                    entityClass.Faction = Factions.Find(f => f.Id.Equals(entityClass.FactionId))
                                ?? throw new InvalidDataException($"There's no faction with id {entityClass.FactionId}");
                }
            }
        }

        public void NewMap()
        {
            var flagList = new List<Flag>();
            if (CurrentFloorLevel > 1)
            {
                var statusNamesToRemove = new List<string>();
                foreach (var statusToRemove in PlayerCharacter.AlteredStatuses.Where(als => als.CleanseOnFloorChange))
                {
                    statusNamesToRemove.Add(statusToRemove.Name);
                    statusToRemove.OnRemove?.Do(statusToRemove, PlayerCharacter, false);
                }
                foreach (var modification in PlayerCharacter.StatModifications)
                {
                   modification.Modifications?.RemoveAll(a => statusNamesToRemove.Contains(a.Id));
                }

                PlayerCharacter.AlteredStatuses.RemoveAll(als => als.CleanseOnFloorChange);
                PlayerCharacter.KeySet.Clear();
                flagList = CurrentFloor.Flags.Where(f => !f.RemoveOnFloorChange).ToList();
            }
            CurrentFloor = new Map(this, CurrentFloorLevel, flagList);
            CurrentFloor.Generate();
        }

        public void GenerateDebugMap()
        {
            IsDebugMode = true;
            CurrentFloor = new Map(this, CurrentFloorLevel, []);
            CurrentFloor.GenerateDebugMap();
        }

        public void SetPlayerName(string name)
        {
            PlayerName = name;
        }

        public void SetPlayerClass(string classId)
        {
            var playerClass = Classes.Find(c => c.Id == classId);
            if (playerClass == null || playerClass.EntityType != EntityType.Player)
                throw new ArgumentException($"{classId} is not a Player Class");
            PlayerClass = playerClass;
        }

        public DungeonDto GetStatus()
        {
            if (CurrentFloor == null)
                NewMap();
            CurrentFloor.Snapshot.DisplayEvents = CurrentFloor.DisplayEvents;
            CurrentFloor.Snapshot.PickableItemPositions = CurrentFloor.Tiles.Where(t => t.GetItems().Any()).Select(t => t.Position).ToList();
            return CurrentFloor.Snapshot;
        }

        public void TakeStairs()
        {
            CurrentFloorLevel++;
            if (CurrentFloorLevel > AmountOfFloors)
            {
                CurrentFloor.TurnCount++;
                CurrentFloor.DisplayEvents.Add(("Finished", new()
                {
                    new() {
                        DisplayEventType = DisplayEventType.SetDungeonStatus,
                        Params = new() { DungeonStatus.Completed }
                    }
                }
                ));
                DungeonStatus = DungeonStatus.Completed;
                return;
            }
            NewMap();
        }

        public void MovePlayer(int x, int y)
        {
            CurrentFloor.PlayerMove(x, y);
        }

        public void PlayerUseItemInFloor()
        {
            CurrentFloor.PlayerUseItemInFloor();
        }
        public void PlayerPickUpItemInFloor()
        {
            CurrentFloor.PlayerPickUpItemInFloor();
        }
        public void PlayerUseItemFromInventory(int itemId)
        {
            CurrentFloor.PlayerUseItemFromInventory(itemId);
        }
        public void PlayerDropItemFromInventory(int itemId)
        {
            CurrentFloor.PlayerDropItemFromInventory(itemId);
        }
        public void PlayerSwapFloorItemWithInventoryItem(int itemId)
        {
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

        public EntityDetailDto GetDetailsOfEntity(int x, int y)
        {
            return CurrentFloor.GetDetailsOfEntity(x, y);
        }

        public InventoryDto GetPlayerInventory()
        {
            return CurrentFloor.GetPlayerInventory();
        }

        public void PlayerAttackTargetWith(string selectionId, int x, int y, ActionSourceType sourceType)
        {
            CurrentFloor.PlayerAttackTargetWith(selectionId, x, y, sourceType);
        }

        public void PlayerTakeStairs()
        {
            CurrentFloor.PlayerUseStairs();
        }
    }
#pragma warning restore CS8604 // Posible argumento de referencia nulo
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}
