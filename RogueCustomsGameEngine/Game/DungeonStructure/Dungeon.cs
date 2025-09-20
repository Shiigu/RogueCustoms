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
using RogueCustomsGameEngine.Game.Interaction;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

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

        // The CS0592 error occurs because the [NonSerialized] attribute is only valid for fields, not properties.  
        // To fix this, change the declaration of `PromptInvoker` from a property to a field.  

        [NonSerialized]
        private IPromptInvoker _promptInvoker;

        public IPromptInvoker PromptInvoker
        {
            get => _promptInvoker;
            set => _promptInvoker = value;
        }

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
        public List<ActionSchool> ActionSchools { get; set; }
        public List<LootTable> LootTables { get; set; }
        public List<CurrencyPile> CurrencyData { get; set; }
        public List<QualityLevel> QualityLevels { get; set; }
        public List<Affix> Affixes { get; set; }
        public List<NPCModifier> NPCModifiers { get; set; }
        public List<ItemSlot> ItemSlots { get; set; }
        public List<ItemType> ItemTypes { get; set; }
        public float SaleValuePercentage { get; set; }
        public List<EntityClass> Classes { get; set; }
        [JsonIgnore]
        public EntityClass CurrencyClass => Classes.FirstOrDefault(c => c.EntityType == EntityType.Currency);
        [JsonIgnore]
        public List<EntityClass> PlayerClasses => Classes.Where(c => c.EntityType == EntityType.Player).ToList();
        [JsonIgnore]
        public List<EntityClass> NPCClasses => Classes.Where(c => c.EntityType == EntityType.NPC).ToList();
        [JsonIgnore]
        public List<EntityClass> CharacterClasses => PlayerClasses.Union(NPCClasses).ToList();
        [JsonIgnore]
        public List<EntityClass> ItemClasses => Classes.Where(c => c.EntityType == EntityType.Item).ToList();
        [JsonIgnore]
        public List<EntityClass> TrapClasses => Classes.Where(c => c.EntityType == EntityType.Trap).ToList();
        [JsonIgnore]
        public List<EntityClass> AlteredStatusClasses => Classes.Where(c => c.EntityType == EntityType.AlteredStatus).ToList();

        [JsonIgnore]
        public List<EntityClass> UndroppableItemClasses => ItemClasses.Where(c => !c.CanDrop).ToList();

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
            ActionSchools = new List<ActionSchool>();
            LootTables = new List<LootTable>();
            Classes = new List<EntityClass>();
            TileTypes = new List<TileType>();
            TileSets = new List<TileSet>();
            CurrencyData = new List<CurrencyPile>();
            Affixes = new List<Affix>();
            NPCModifiers = new List<NPCModifier>();
            QualityLevels = new List<QualityLevel>();
            ItemSlots = new List<ItemSlot>();
            ItemTypes = new List<ItemType>();
            var localeInfoToUse = dungeonInfo.Locales.Find(l => l.Language.Equals(localeLanguage))
                ?? dungeonInfo.Locales.Find(l => l.Language.Equals(dungeonInfo.DefaultLocale))
                ?? throw new FormatException($"No locale data has been found for {localeLanguage}, and no default locale was defined.");
            LocaleToUse = new Locale(localeInfoToUse);
            Name = LocaleToUse[dungeonInfo.Name];
            WelcomeMessage = LocaleToUse[dungeonInfo.WelcomeMessage];
            EndingMessage = LocaleToUse[dungeonInfo.EndingMessage];
            Classes.Add(dungeonInfo.CurrencyInfo.Parse(this));
            dungeonInfo.ItemSlotInfos.ForEach(isi => ItemSlots.Add(new ItemSlot(isi, this)));
            dungeonInfo.ItemTypeInfos.ForEach(iti => ItemTypes.Add(new ItemType(iti, this)));
            dungeonInfo.CurrencyInfo.CurrencyPiles.ForEach(cp => CurrencyData.Add(new CurrencyPile(cp)));
            dungeonInfo.ActionSchoolInfos.ForEach(s => ActionSchools.Add(new ActionSchool(s, LocaleToUse)));
            dungeonInfo.ElementInfos.ForEach(e => Elements.Add(new Element(e, LocaleToUse, ActionSchools)));
            dungeonInfo.AffixInfos.ForEach(a => Affixes.Add(new Affix(a, LocaleToUse, ItemTypes, Elements, ActionSchools)));
            
            dungeonInfo.NPCModifierInfos.ForEach(nm => NPCModifiers.Add(new NPCModifier(nm, LocaleToUse, Elements, ActionSchools)));
            dungeonInfo.QualityLevelInfos.ForEach(ql => QualityLevels.Add(new QualityLevel(ql, LocaleToUse)));
            dungeonInfo.TileTypeInfos.ForEach(tl => TileTypes.Add(new TileType(tl, ActionSchools)));
            dungeonInfo.TileSetInfos.ForEach(ts => TileSets.Add(new TileSet(ts, this)));
            dungeonInfo.LootTableInfos.ForEach(lt => LootTables.Add(new LootTable(lt)));
            dungeonInfo.PlayerClasses.ForEach(ci => Classes.Add(new EntityClass(ci, this, dungeonInfo.CharacterStats)));
            dungeonInfo.NPCs.ForEach(ci => Classes.Add(new EntityClass(ci, this, dungeonInfo.CharacterStats)));
            dungeonInfo.Items.ForEach(ci => Classes.Add(new EntityClass(ci, this, null)));
            dungeonInfo.Traps.ForEach(ci => Classes.Add(new EntityClass(ci, this, null)));
            dungeonInfo.AlteredStatuses.ForEach(ci => Classes.Add(new EntityClass(ci, this, null)));
            LootTables.ForEach(lt => lt.FillEntries(this));
            FloorTypes = new List<FloorType>();
            dungeonInfo.FloorInfos.ForEach(fi => FloorTypes.Add(new FloorType(fi, this)));
            FloorTypes.ForEach(ft => {
                ft.TileSet = TileSets.Find(ts => ts.Id.Equals(ft.TileSetId))
                    ?? throw new FormatException($"No TileSet with id {ft.TileSetId} was found.");
                ft.FillPossibleClassLists(Classes);
            });
            Factions = new List<Faction>();
            dungeonInfo.FactionInfos.ForEach(fi => Factions.Add(new Faction(fi, LocaleToUse)));
            Scripts = new();
            dungeonInfo.Scripts.ForEach(s => Scripts.Add(ActionWithEffects.Create(s, ActionSchools)));
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

        public async Task NewMap()
        {
            var flagList = new List<Flag>();
            var npcsToKeep = new List<NonPlayableCharacter>();
            if (CurrentFloorLevel > 1)
            {
                npcsToKeep = CurrentFloor.AICharacters.Where(c => c.ExistenceStatus == EntityExistenceStatus.Alive && c.ReappearsOnTheNextFloorIfAlliedToThePlayer && (c.Faction.IsAlliedWith(PlayerCharacter.Faction) || c.KnownCharacters.Any(kc => kc.TargetType == TargetType.Ally && kc.Character == PlayerCharacter))).ToList();
                var statusNamesToRemove = new List<string>();
                foreach (var statusToRemove in PlayerCharacter.AlteredStatuses.Where(als => als.CleanseOnFloorChange))
                {
                    statusNamesToRemove.Add(statusToRemove.Name);
                    if(statusToRemove.OnRemove != null)
                        await statusToRemove.OnRemove.Do(statusToRemove, PlayerCharacter, false);
                }
                foreach (var modification in PlayerCharacter.StatModifications)
                {
                   modification.Modifications?.RemoveAll(a => statusNamesToRemove.Contains(a.Id));
                }

                PlayerCharacter.AlteredStatuses.RemoveAll(als => als.CleanseOnFloorChange);
                PlayerCharacter.KeySet.Clear();
                flagList = CurrentFloor.Flags.Where(f => !f.RemoveOnFloorChange).ToList();
            }
            CurrentFloor = new Map(this, CurrentFloorLevel, flagList, npcsToKeep);
            await CurrentFloor.Generate(false);
        }

        public Task GenerateDebugMap()
        {
            IsDebugMode = true;
            CurrentFloor = new Map(this, CurrentFloorLevel, [], []);
            return CurrentFloor.GenerateDebugMap();
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

        public async Task<DungeonDto> GetStatus()
        {
            if (CurrentFloor == null)
                await NewMap();
            CurrentFloor.Snapshot.DisplayEvents = CurrentFloor.DisplayEvents;
            CurrentFloor.Snapshot.PickableItemPositions = CurrentFloor.Tiles.Where(t => t.GetItems().Any()).Select(t => t.Position).ToList();
            return CurrentFloor.Snapshot;
        }

        public async Task TakeStairs()
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
            await NewMap();
        }

        public Task MovePlayer(int x, int y)
        {
            return CurrentFloor.PlayerMove(x, y);
        }

        public Task PlayerUseItemInFloor()
        {
            return CurrentFloor.PlayerUseItemInFloor();
        }
        public Task PlayerPickUpItemInFloor()
        {
            return CurrentFloor.PlayerPickUpItemInFloor();
        }
        public Task PlayerUseItemFromInventory(int itemId)
        {
            return CurrentFloor.PlayerUseItemFromInventory(itemId);
        }
        public Task PlayerDropItemFromInventory(int itemId)
        {
            return CurrentFloor.PlayerDropItemFromInventory(itemId);
        }
        public Task PlayerSwapFloorItemWithInventoryItem(int itemId)
        {
            return CurrentFloor.PlayerSwapFloorItemWithInventoryItem(itemId);
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

        public Task PlayerAttackTargetWith(string selectionId, int x, int y, ActionSourceType sourceType)
        {
            return CurrentFloor.PlayerAttackTargetWith(selectionId, x, y, sourceType);
        }

        public Task PlayerTakeStairs()
        {
            return CurrentFloor.PlayerUseStairs();
        }

        public void RefreshDisplay(bool refreshWholeMap)
        {
            CurrentFloor.RefreshDisplay(refreshWholeMap);
        }
    }
#pragma warning restore CS8604 // Posible argumento de referencia nulo
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}
