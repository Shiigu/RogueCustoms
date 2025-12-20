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
        public PlayerCharacter? PlayerCharacter { get; set; }
        public EntityClass? PlayerClass { get; set; }
        public string? PlayerName { get; set; }
        public string Version { get; set; }
        public string FileName { get; set; }
        public DungeonStatus DungeonStatus { get; set; }
        public Map CurrentFloor { get; private set; }

        [NonSerialized]
        private IPromptInvoker _promptInvoker;

        public IPromptInvoker PromptInvoker
        {
            get => _promptInvoker;
            set => _promptInvoker = value;
        }
        public int CurrentEntityId;
        private int CurrentFloorLevel;
        public readonly string WelcomeMessage;
        public readonly string EndingMessage;
        public bool IsDebugMode { get; set; }
        public bool IsHardcoreMode { get; set; }

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
        public List<Learnset> Learnsets { get; set; }
        public List<Quest> Quests { get; set; }
        public List<ActionWithEffects> Scripts { get; private set; }
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
            Learnsets = new List<Learnset>();
            Quests = new List<Quest>();
            var localeInfoToUse = dungeonInfo.Locales.Find(l => l.Language.Equals(localeLanguage))
                ?? dungeonInfo.Locales.Find(l => l.Language.Equals(dungeonInfo.DefaultLocale))
                ?? throw new FormatException($"No locale data has been found for {localeLanguage}, and no default locale was defined.");
            LocaleToUse = new Locale(localeInfoToUse);
            Name = LocaleToUse[dungeonInfo.Name];
            WelcomeMessage = LocaleToUse[dungeonInfo.WelcomeMessage];
            EndingMessage = LocaleToUse[dungeonInfo.EndingMessage];
            Classes.Add(dungeonInfo.CurrencyInfo.Parse(this));
            dungeonInfo.LearnsetInfos.ForEach(li => Learnsets.Add(new Learnset(li)));
            dungeonInfo.QualityLevelInfos.ForEach(ql => QualityLevels.Add(new QualityLevel(ql, LocaleToUse)));
            dungeonInfo.ItemSlotInfos.ForEach(isi => ItemSlots.Add(new ItemSlot(isi, this)));
            dungeonInfo.ItemTypeInfos.ForEach(iti => ItemTypes.Add(new ItemType(iti, this)));
            dungeonInfo.CurrencyInfo.CurrencyPiles.ForEach(cp => CurrencyData.Add(new CurrencyPile(cp)));
            dungeonInfo.ActionSchoolInfos.ForEach(s => ActionSchools.Add(new ActionSchool(s, LocaleToUse)));
            dungeonInfo.ElementInfos.ForEach(e => Elements.Add(new Element(e, LocaleToUse, ActionSchools)));
            dungeonInfo.AffixInfos.ForEach(a => Affixes.Add(new Affix(a, LocaleToUse, ItemTypes, Elements, ActionSchools)));
            
            dungeonInfo.NPCModifierInfos.ForEach(nm => NPCModifiers.Add(new NPCModifier(nm, LocaleToUse, Elements, ActionSchools)));
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
            Learnsets.ForEach(ls => ls.MapScriptsToLearn(Scripts));
            MapFactions();
            dungeonInfo.QuestInfos.ForEach(q => Quests.Add(new Quest(q, this)));

            IsHardcoreMode = isHardcoreMode;
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

        public async Task NewMap(int floorLevel)
        {
            if (CurrentFloorLevel > 0 && PlayerCharacter != null && PlayerCharacter.HighestFloorReached < CurrentFloorLevel)
                PlayerCharacter.HighestFloorReached = CurrentFloorLevel;
            CurrentFloorLevel = floorLevel;
            var flagList = new List<Flag>();
            var npcsToKeep = new List<NonPlayableCharacter>();
            var questsToAbandon = new List<Quest>();
            if (CurrentFloorLevel > 1)
            {
                questsToAbandon = PlayerCharacter.Quests.Where(q => q.AbandonedOnFloorChange && q.Status == QuestStatus.InProgress).ToList();
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
            CurrentFloor.BindEverything();
            await CurrentFloor.Generate(false, []);
            if (CurrentFloorLevel > 1)
            {
                foreach (var questToAbandon in questsToAbandon)
                {
                    PlayerCharacter.AbandonQuest(questToAbandon);
                }
                await PlayerCharacter.UpdateQuests(QuestConditionType.ReachFloor);
            }
        }

        public Task ReturnToFloor1(int experiencePercentageToKeep, int equipmentPercentageToKeep, int inventoryPercentageToKeep, int learnedScriptsPercentageToKeep, int tagalongNPCsPercentageToKeep)
        {
            CurrentFloorLevel = 1;

            CurrentFloor.DisplayEvents = new();

            var tagalongNPCs = CurrentFloor.AICharacters.Where(c => c.ExistenceStatus == EntityExistenceStatus.Alive && c.ReappearsOnTheNextFloorIfAlliedToThePlayer && (c.Faction.IsAlliedWith(PlayerCharacter.Faction) || c.KnownCharacters.Any(kc => kc.TargetType == TargetType.Ally && kc.Character == PlayerCharacter))).ToList();
            var tagalongNPCCount = tagalongNPCs.Count;

            tagalongNPCs = tagalongNPCs.Shuffle(CurrentFloor.Rng).Take((int)(tagalongNPCs.Count * (tagalongNPCsPercentageToKeep / 100.0))).ToList();

            PlayerCharacter.Quests.Clear();

            var playerLostLevels = false;
            var minimumExperienceForCurrentLevel = PlayerCharacter.GetMinimumExperienceForLevel(PlayerCharacter.Level, true);

            PlayerCharacter.Experience = (int)(PlayerCharacter.Experience * (experiencePercentageToKeep / 100.0));

            while(PlayerCharacter.Experience < minimumExperienceForCurrentLevel && PlayerCharacter.Level > 1)
            {
                playerLostLevels = true;
                PlayerCharacter.Level--;
                minimumExperienceForCurrentLevel = PlayerCharacter.GetMinimumExperienceForLevel(PlayerCharacter.Level, true);
                PlayerCharacter.LastLevelUpExperience = minimumExperienceForCurrentLevel;
            }

            var playerLearnsetChanged = false;

            if (playerLostLevels || !PlayerCharacter.Learnset.Id.Equals(PlayerCharacter.BaseLearnset.Id))
            {
                PlayerCharacter.Learnset = PlayerCharacter.BaseLearnset;

                var oldLearnset = PlayerCharacter.OwnOnAttack.Where(ooa => ooa.IsFromLearnset).Select(ooa => ooa.Name).ToList();

                PlayerCharacter.OwnOnAttack.RemoveAll(ooa => ooa.IsFromLearnset);

                for (int i = 1; i <= PlayerCharacter.Level; i++)
                {
                    PlayerCharacter.UpdateScriptsFromLearnset(i, true);
                }
                var newLearnset = PlayerCharacter.OwnOnAttack.Where(ooa => ooa.IsFromLearnset).Select(ooa => ooa.Name).ToList();

                playerLearnsetChanged = !newLearnset.IsIdenticalTo(oldLearnset);
            }

            PlayerCharacter.AlteredStatuses.Clear();

            foreach (var stat in PlayerCharacter.UsedStats)
            {
                stat.ActiveModifications.Clear();
            }

            PlayerCharacter.SightRangeModification = null;

            PlayerCharacter.ExistenceStatus = EntityExistenceStatus.Alive;

            PlayerCharacter.HP.Current = PlayerCharacter.HP.BaseAfterLevelUp;
            if (PlayerCharacter.HPRegeneration != null)
                PlayerCharacter.HPRegeneration.CarriedRegeneration = 0;

            if(PlayerCharacter.MP != null)
                PlayerCharacter.MP.Current = PlayerCharacter.MP.BaseAfterLevelUp;
            if (PlayerCharacter.MPRegeneration != null)
                PlayerCharacter.MPRegeneration.CarriedRegeneration = 0;

            if (PlayerCharacter.Hunger != null)
                PlayerCharacter.Hunger.Current = PlayerCharacter.Hunger.BaseAfterLevelUp;
            if (PlayerCharacter.HungerDegeneration != null)
                PlayerCharacter.HungerDegeneration.CarriedRegeneration = 0;
            var scriptsCount = PlayerCharacter.OwnOnAttack.Count(ooa => ooa.IsFromLearnScript);
            var playerLostScripts = false;

            for (int i = 0; i < (int)(scriptsCount * (learnedScriptsPercentageToKeep / 100.0)); i++)
            {
                var aRandomScript = PlayerCharacter.OwnOnAttack.Where(ooa => ooa.IsFromLearnScript).ToList().Shuffle(CurrentFloor.Rng).Take(1);

                if (!aRandomScript.Any()) break;

                playerLostScripts = true;
                PlayerCharacter.OwnOnAttack.Remove(aRandomScript.First());
            }
            PlayerCharacter.KeySet.Clear();
            PlayerCharacter.Visible = true;
            PlayerCharacter.LostLives++;

            var flagList = CurrentFloor.Flags.Where(f => !f.RemoveOnFloorChange).ToList();
            CurrentFloor.TurnCount = 0;

            CurrentFloor = new Map(this, CurrentFloorLevel, flagList, tagalongNPCs);
            CurrentFloor.BindEverything();

            var events = new List<DisplayEventDto>();

            if (playerLostLevels)
                CurrentFloor.AppendMessage(CurrentFloor.Locale["CharacterHasDroppedLevels"].Format(new { CharacterName = PlayerCharacter.Name, Level = PlayerCharacter.Level.ToString() }), Color.OrangeRed, events);

            if (playerLearnsetChanged)
                CurrentFloor.AppendMessage(CurrentFloor.Locale["CharacterHasRegressedOnLearnset"].Format(new { CharacterName = PlayerCharacter.Name }), Color.OrangeRed, events);

            var playerEquipmentCount = PlayerCharacter.Equipment.Count;
            
            PlayerCharacter.Equipment = PlayerCharacter.Equipment.Where(i => i.RequiredPlayerLevel <= PlayerCharacter.Level).ToList();
            PlayerCharacter.Equipment = PlayerCharacter.Equipment.Shuffle(CurrentFloor.Rng).Take((int)(PlayerCharacter.Equipment.Count * (equipmentPercentageToKeep / 100.0))).ToList();

            if (playerEquipmentCount > PlayerCharacter.Equipment.Count)
                CurrentFloor.AppendMessage(CurrentFloor.Locale["CharacterHasLostEquipment"].Format(new { CharacterName = PlayerCharacter.Name }), Color.OrangeRed, events);

            var playerInventoryCount = PlayerCharacter.Inventory.Count;
            PlayerCharacter.Inventory = PlayerCharacter.Inventory.Shuffle(CurrentFloor.Rng).Take((int)(PlayerCharacter.Inventory.Count * (inventoryPercentageToKeep / 100.0))).ToList();

            if (playerInventoryCount > PlayerCharacter.Inventory.Count)
                CurrentFloor.AppendMessage(CurrentFloor.Locale["CharacterHasLostItems"].Format(new { CharacterName = PlayerCharacter.Name }), Color.OrangeRed, events);

            if (playerLostScripts)
                CurrentFloor.AppendMessage(CurrentFloor.Locale["CharacterHasLostScripts"].Format(new { CharacterName = PlayerCharacter.Name }), Color.OrangeRed, events);

            if (tagalongNPCCount > tagalongNPCs.Count)
                CurrentFloor.AppendMessage(CurrentFloor.Locale["CharacterHasLostAllies"].Format(new { CharacterName = PlayerCharacter.Name }), Color.OrangeRed, events);

            PlayerCharacter.InformRefreshedPlayerData(events);

            return CurrentFloor.Generate(false, [("Inform of Return to Floor 1", events)]);
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
                await NewMap(1);
            CurrentFloor.Snapshot.DisplayEvents = new(CurrentFloor.DisplayEvents.Where(de => !de.Events.Any(e => e.Read)));
            CurrentFloor.DisplayEvents.ForEach(de => de.Events.ForEach(e => e.Read = true));
            CurrentFloor.Snapshot.PickableItemPositions = CurrentFloor.Tiles.Where(t => t.GetItems().Count != 0).ConvertAll(t => t.Position);
            return CurrentFloor.Snapshot;
        }

        public async Task TakeStairs()
        {
            if (CurrentFloorLevel == AmountOfFloors)
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
            await NewMap(CurrentFloorLevel + 1);
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
        public void PlayerAbandonQuest(int questId)
        {
            CurrentFloor.PlayerAbandonQuest(questId);
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
        public List<QuestDto> GetPlayerQuests()
        {
            return CurrentFloor.GetPlayerQuests();
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
