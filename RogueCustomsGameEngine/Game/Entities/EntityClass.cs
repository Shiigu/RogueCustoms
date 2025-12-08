using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;

namespace RogueCustomsGameEngine.Game.Entities
{
    #pragma warning disable CS8601 // Posible asignación de referencia nula
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public class EntityClass
    {
        public readonly string Id;
        public readonly string Name;
        public readonly string Description;
        public readonly string FactionId;
        public Faction Faction { get; set; }
        public readonly ConsoleRepresentation ConsoleRepresentation;
        public EntityType EntityType { get; set; }
        public readonly bool Passable;
        public readonly bool StartsVisible;

        #region Character-only data

        public readonly List<ItemSlot> AvailableSlots;
        public readonly List<Stat> Stats;
        public readonly int BaseSightRange;
        public readonly int InventorySize;
        public List<string> InitialEquipmentIds { get; private set; }
        public List<string> StartingInventoryIds { get; private set; }
        public readonly int MaxLevel;
        public readonly bool CanGainExperience;
        public readonly string ExperiencePayoutFormula;
        public readonly string ExperienceToLevelUpFormula;
        public ActionWithEffects DefaultOnAttack { get; set; }

        public readonly Learnset BaseLearnset;

        #endregion

        #region Player Character-only data

        public readonly bool RequiresNamePrompt;
        public readonly float SaleValuePercentage;
        public readonly bool NeedsToIdentifyItems;

        #endregion

        #region NPC-only data

        public ActionWithEffects BeforeProcessAI { get; set; }
        public ActionWithEffects OnSpawn { get; set; }
        public List<ActionWithEffects> OnInteracted { get; set; }

        public readonly bool KnowsAllCharacterPositions;
        public readonly bool PursuesOutOfSightCharacters;
        public readonly bool WandersIfWithoutTarget;
        public readonly bool DropsEquipmentOnDeath;
        public readonly bool ReappearsOnTheNextFloorIfAlliedToThePlayer;
        public readonly AIType AIType;

        public readonly LootTable LootTable;
        public readonly int DropPicks;

        public readonly int OddsForModifier;
        public readonly List<(int Level, int Amount)> ModifierTable;
        public readonly bool RandomizesForecolorIfWithModifiers;
        public readonly decimal ExperienceYieldMultiplierIfWithModifiers;
        public readonly decimal BaseHPMultiplierIfWithModifiers;
        public readonly LootTable LootTableModifier;
        public readonly int DropPicksModifier;

        #endregion
        public readonly string Power;

        #region Item-only data
        public ItemType ItemType { get; set; }
        public ActionWithEffects OnUse { get; set; }
        public List<PassiveStatModifier> StatModifiers { get; set; }
        public int BaseValue { get; set; }
        public int RequiredPlayerLevel { get; set; }
        public QualityLevel MinimumQualityLevel { get; set; }
        public QualityLevel MaximumQualityLevel { get; set; }
        public List<QualityLevelOdds> QualityLevelOdds { get; set; }
        public bool CanDrop { get; set; }
        public bool CanBeUnequipped { get; set; }
       
        #endregion

        #region Trap-only data
        public ActionWithEffects OnStepped { get; set; }
        #endregion

        public ActionWithEffects OnTurnStart { get; set; }
        public List<ActionWithEffects> OnAttack { get; set; }
        public ActionWithEffects OnAttacked { get; set; }
        public ActionWithEffects OnDeath { get; set; }
        public ActionWithEffects OnLevelUp { get; set; }

        #region Status-only data
        public readonly bool CanStack;
        public readonly bool CanOverwrite;
        public readonly bool CleanseOnFloorChange;
        public readonly bool CleansedByCleanseActions;

        public ActionWithEffects BeforeAttack { get; set; }

        public ActionWithEffects OnApply { get; set; }

        public ActionWithEffects OnRemove { get; set; }
        #endregion
        public EntityClass(ClassInfo classInfo, Dungeon dungeon, List<StatInfo> statInfos)
        {
            Id = classInfo.Id;
            Name = dungeon.LocaleToUse[classInfo.Name];
            Description = (classInfo.Description != null) ? dungeon.LocaleToUse[classInfo.Description] : null;
            ConsoleRepresentation = classInfo.ConsoleRepresentation;

            if (classInfo is ItemInfo itemInfo)
            {
                EntityType = EntityType.Item;
                ItemType = dungeon.ItemTypes.Find(it => it.Id.Equals(itemInfo.ItemType, StringComparison.InvariantCultureIgnoreCase));
                Power = itemInfo.Power;
                StatModifiers = new();
                if(itemInfo.StatModifiers != null)
                {
                    foreach (var statModifier in itemInfo.StatModifiers)
                    {
                        StatModifiers.Add(new PassiveStatModifier
                        {
                            Id = statModifier.Id,
                            Amount = statModifier.Amount
                        });
                    }
                }
                StartsVisible = itemInfo.StartsVisible;
                Passable = true;
                OnTurnStart = ActionWithEffects.Create(itemInfo.OnTurnStart, dungeon.ActionSchools);
                OnAttack = new List<ActionWithEffects>();
                MapActions(OnAttack, itemInfo.OnAttack, dungeon.ActionSchools);
                OnAttacked = ActionWithEffects.Create(itemInfo.OnAttacked, dungeon.ActionSchools);
                OnDeath = ActionWithEffects.Create(itemInfo.OnDeath, dungeon.ActionSchools);
                OnUse = ActionWithEffects.Create(itemInfo.OnUse, dungeon.ActionSchools);
                RequiredPlayerLevel = itemInfo.RequiredPlayerLevel;
                BaseValue = itemInfo.BaseValue;
                MinimumQualityLevel = dungeon.QualityLevels.Find(ql => ql.Id.Equals(itemInfo.MinimumQualityLevel, StringComparison.InvariantCultureIgnoreCase));
                MaximumQualityLevel = dungeon.QualityLevels.Find(ql => ql.Id.Equals(itemInfo.MaximumQualityLevel, StringComparison.InvariantCultureIgnoreCase));
                QualityLevelOdds = [];
                foreach (var odds in itemInfo.QualityLevelOdds ?? [])
                {
                    QualityLevelOdds.Add(new(odds, dungeon));
                }
                CanDrop = itemInfo.CanDrop;
                CanBeUnequipped = itemInfo.CanBeUnequipped;
            }
            else if (classInfo is PlayerClassInfo playerClassInfo)
            {
                FactionId = playerClassInfo.Faction;
                CanGainExperience = playerClassInfo.CanGainExperience;
                MaxLevel = playerClassInfo.MaxLevel;
                ExperiencePayoutFormula = playerClassInfo.ExperiencePayoutFormula;
                ExperienceToLevelUpFormula = playerClassInfo.ExperienceToLevelUpFormula;
                Stats = new();
                foreach (var stat in playerClassInfo.Stats)
                {
                    Stats.Add(new()
                    {
                        Id = stat.StatId,
                        Base = stat.Base,
                        Current = stat.Base,
                        IncreasePerLevel = stat.IncreasePerLevel,
                        ActiveModifications = new(),
                        CarriedRegeneration = 0,
                        MinCap = stat.Minimum,
                        MaxCap = stat.Maximum
                    });
                }
                foreach (var stat in Stats)
                {
                    var correspondingStat = statInfos.Find(s => s.Id.Equals(stat.Id, StringComparison.InvariantCultureIgnoreCase));
                    if(correspondingStat == null)
                        throw new InvalidDataException($"EntityClass {playerClassInfo.Id} has a Stat of Id {stat.Id}, which isn't found in the Dungeon Data.");
                    stat.Name = dungeon.LocaleToUse[correspondingStat.Name];
                    stat.StatType = Enum.Parse<StatType>(correspondingStat.StatType);
                    stat.HasMax = correspondingStat.HasMax;
                    stat.RegenerationTargetId = correspondingStat.RegeneratesStatId;
                }
                BaseSightRange = 0;
                if (!string.IsNullOrWhiteSpace(playerClassInfo.BaseSightRange))
                {
                    switch (playerClassInfo.BaseSightRange.ToLower())
                    {
                        case "full map":
                        case "fullmap":
                        case "whole map":
                        case "wholemap":
                            BaseSightRange = EngineConstants.FullMapSightRange;
                            break;
                        case "full room":
                        case "fullroom":
                        case "whole room":
                        case "wholeroom":
                            BaseSightRange = EngineConstants.FullRoomSightRange;
                            break;
                        default:
                            if (!int.TryParse(playerClassInfo.BaseSightRange, out int sightRange) || sightRange < 0)
                                throw new InvalidDataException($"Sight Range of {playerClassInfo.BaseSightRange} is not valid.");
                            BaseSightRange = sightRange;
                            break;
                    }
                }
                InventorySize = playerClassInfo.InventorySize;
                OnTurnStart = ActionWithEffects.Create(playerClassInfo.OnTurnStart, dungeon.ActionSchools);
                OnAttack = new List<ActionWithEffects>();
                MapActions(OnAttack, playerClassInfo.OnAttack, dungeon.ActionSchools);
                OnAttacked = ActionWithEffects.Create(playerClassInfo.OnAttacked, dungeon.ActionSchools);
                OnDeath = ActionWithEffects.Create(playerClassInfo.OnDeath, dungeon.ActionSchools);
                OnLevelUp = ActionWithEffects.Create(playerClassInfo.OnLevelUp, dungeon.ActionSchools);
                EntityType = EntityType.Player;
                StartsVisible = playerClassInfo.StartsVisible;
                StartingInventoryIds = new List<string>(playerClassInfo.StartingInventory);
                Passable = false;
                RequiresNamePrompt = playerClassInfo.RequiresNamePrompt;
                InitialEquipmentIds = new List<string>(playerClassInfo.InitialEquipment);
                SaleValuePercentage = playerClassInfo.SaleValuePercentage / 100f;
                DefaultOnAttack = ActionWithEffects.Create(playerClassInfo.DefaultOnAttack, dungeon.ActionSchools);
                AvailableSlots = dungeon.ItemSlots.FindAll(islot => playerClassInfo.AvailableSlots.Contains(islot.Id));
                NeedsToIdentifyItems = playerClassInfo.NeedsToIdentifyItems;
                BaseLearnset = dungeon.Learnsets.Find(ls => ls.Id.Equals(playerClassInfo.Learnset, StringComparison.InvariantCultureIgnoreCase));
            }
            else if (classInfo is NPCInfo npcInfo)
            {
                FactionId = npcInfo.Faction;
                Stats = new();
                foreach (var stat in npcInfo.Stats)
                {
                    Stats.Add(new()
                    {
                        Id = stat.StatId,
                        Base = stat.Base,
                        Current = stat.Base,
                        IncreasePerLevel = stat.IncreasePerLevel,
                        ActiveModifications = new(),
                        CarriedRegeneration = 0,
                        MinCap = stat.Minimum,
                        MaxCap = stat.Maximum
                    });
                }
                foreach (var stat in Stats)
                {
                    var correspondingStat = statInfos.Find(s => s.Id.Equals(stat.Id, StringComparison.InvariantCultureIgnoreCase));
                    if (correspondingStat == null)
                        throw new InvalidDataException($"EntityClass {npcInfo.Id} has a Stat of Id {stat.Id}, which isn't found in the Dungeon Data.");
                    stat.Name = dungeon.LocaleToUse[correspondingStat.Name];
                    stat.StatType = Enum.Parse<StatType>(correspondingStat.StatType);
                    stat.HasMax = correspondingStat.HasMax;
                    stat.RegenerationTargetId = correspondingStat.RegeneratesStatId;
                }
                CanGainExperience = npcInfo.CanGainExperience;
                MaxLevel = npcInfo.MaxLevel;
                ExperiencePayoutFormula = npcInfo.ExperiencePayoutFormula;
                ExperienceToLevelUpFormula = npcInfo.ExperienceToLevelUpFormula;
                BaseSightRange = 0;
                if (!string.IsNullOrWhiteSpace(npcInfo.BaseSightRange))
                {
                    switch (npcInfo.BaseSightRange.ToLower())
                    {
                        case "full map":
                        case "fullmap":
                        case "whole map":
                        case "wholemap":
                            BaseSightRange = EngineConstants.FullMapSightRange;
                            break;
                        case "full room":
                        case "fullroom":
                        case "whole room":
                        case "wholeroom":
                            BaseSightRange = EngineConstants.FullRoomSightRange;
                            break;
                        default:
                            if (!int.TryParse(npcInfo.BaseSightRange, out int sightRange) || sightRange < 0)
                                throw new InvalidDataException($"Sight Range of {npcInfo.BaseSightRange} is not valid.");
                            BaseSightRange = sightRange;
                            break;
                    }
                }
                InventorySize = npcInfo.InventorySize;
                OnTurnStart = ActionWithEffects.Create(npcInfo.OnTurnStart, dungeon.ActionSchools);
                OnAttack = new List<ActionWithEffects>();
                MapActions(OnAttack, npcInfo.OnAttack, dungeon.ActionSchools);
                OnAttacked = ActionWithEffects.Create(npcInfo.OnAttacked, dungeon.ActionSchools);
                OnDeath = ActionWithEffects.Create(npcInfo.OnDeath, dungeon.ActionSchools);
                OnLevelUp = ActionWithEffects.Create(npcInfo.OnLevelUp, dungeon.ActionSchools);

                EntityType = EntityType.NPC;
                StartsVisible = npcInfo.StartsVisible;
                StartingInventoryIds = new List<string>(npcInfo.StartingInventory);
                Passable = false;
                AIType = Enum.Parse<AIType>(npcInfo.AIType);
                if(npcInfo.RegularLootTable != null)
                {
                    LootTable = (npcInfo.RegularLootTable.LootTableId != null && npcInfo.RegularLootTable.LootTableId != "None") ? dungeon.LootTables.Find(lt => lt.Id.Equals(npcInfo.RegularLootTable.LootTableId, StringComparison.InvariantCultureIgnoreCase)) : null;
                    DropPicks = LootTable != null ? npcInfo.RegularLootTable.DropPicks : 0;
                }
                else
                {
                    LootTable = null;
                    DropPicks = 0;
                }
                if (npcInfo.LootTableWithModifiers != null)
                {
                    LootTableModifier = (npcInfo.LootTableWithModifiers.LootTableId != null && npcInfo.LootTableWithModifiers.LootTableId != "None") ? dungeon.LootTables.Find(lt => lt.Id.Equals(npcInfo.LootTableWithModifiers.LootTableId, StringComparison.InvariantCultureIgnoreCase)) : null;
                    DropPicksModifier = LootTable != null ? npcInfo.LootTableWithModifiers.DropPicks : 0;
                }
                else
                {
                    LootTableModifier = null;
                    DropPicksModifier = 0;
                }
                OddsForModifier = npcInfo.OddsForModifier;
                ModifierTable = new();
                foreach (var modifier in npcInfo.ModifierData)
                {
                    ModifierTable.Add((modifier.Level, modifier.ModifierAmount));
                }
                ReappearsOnTheNextFloorIfAlliedToThePlayer = npcInfo.ReappearsOnTheNextFloorIfAlliedToThePlayer;
                ExperienceYieldMultiplierIfWithModifiers = npcInfo.ExperienceYieldMultiplierIfWithModifiers;
                BaseHPMultiplierIfWithModifiers = npcInfo.BaseHPMultiplierIfWithModifiers;
                RandomizesForecolorIfWithModifiers = npcInfo.RandomizesForecolorIfWithModifiers;

                KnowsAllCharacterPositions = npcInfo.KnowsAllCharacterPositions;
                PursuesOutOfSightCharacters = npcInfo.PursuesOutOfSightCharacters;
                WandersIfWithoutTarget = npcInfo.WandersIfWithoutTarget;
                DropsEquipmentOnDeath = npcInfo.DropsEquipmentOnDeath;
                OnSpawn = ActionWithEffects.Create(npcInfo.OnSpawn, dungeon.ActionSchools);
                OnInteracted = new List<ActionWithEffects>();
                MapActions(OnInteracted, npcInfo.OnInteracted, dungeon.ActionSchools);
                InitialEquipmentIds = new List<string>(npcInfo.InitialEquipment);
                DefaultOnAttack = ActionWithEffects.Create(npcInfo.DefaultOnAttack, dungeon.ActionSchools);
                AvailableSlots = dungeon.ItemSlots.FindAll(islot => npcInfo.AvailableSlots.Contains(islot.Id));
                BeforeProcessAI = ActionWithEffects.Create(npcInfo.BeforeProcessAI, dungeon.ActionSchools);
                BaseLearnset = dungeon.Learnsets.Find(ls => ls.Id.Equals(npcInfo.Learnset, StringComparison.InvariantCultureIgnoreCase));
            }
            else if (classInfo is TrapInfo trapInfo)
            {
                EntityType = EntityType.Trap;
                Power = trapInfo.Power;
                StartsVisible = trapInfo.StartsVisible;
                Passable = true;
                OnStepped = ActionWithEffects.Create(trapInfo.OnStepped, dungeon.ActionSchools);
            }
            else if (classInfo is AlteredStatusInfo alteredStatusInfo)
            {
                EntityType = EntityType.AlteredStatus;
                Passable = true;
                CanStack = alteredStatusInfo.CanStack;
                CanOverwrite = alteredStatusInfo.CanOverwrite;
                CleanseOnFloorChange = alteredStatusInfo.CleanseOnFloorChange;
                CleansedByCleanseActions = alteredStatusInfo.CleansedByCleanseActions;
                OnTurnStart = ActionWithEffects.Create(alteredStatusInfo.OnTurnStart, dungeon.ActionSchools);
                OnApply = ActionWithEffects.Create(alteredStatusInfo.OnApply, dungeon.ActionSchools);
                OnAttacked = ActionWithEffects.Create(alteredStatusInfo.OnAttacked, dungeon.ActionSchools);
                BeforeAttack = ActionWithEffects.Create(alteredStatusInfo.BeforeAttack, dungeon.ActionSchools);
                OnRemove = ActionWithEffects.Create(alteredStatusInfo.OnRemove, dungeon.ActionSchools);
            }
        }

        protected static void MapActions(List<ActionWithEffects> actionList, List<ActionWithEffectsInfo> actionInfoList, List<ActionSchool> actionSchools)
        {
            if(actionInfoList != null && actionInfoList.Count > 0)
                actionInfoList.ForEach(aa => actionList.Add(ActionWithEffects.Create(aa, actionSchools)));
        }
    }
    #pragma warning restore CS8601 // Posible asignación de referencia nula
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
