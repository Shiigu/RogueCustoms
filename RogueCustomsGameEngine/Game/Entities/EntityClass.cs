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
        public readonly EntityType EntityType;
        public readonly bool Passable;
        public readonly bool StartsVisible;

        #region Character-only data

        public readonly List<Stat> Stats;
        public readonly int BaseSightRange;
        public readonly int InventorySize;
        public readonly string StartingWeaponId;
        public readonly string StartingArmorId;
        public List<string> StartingInventoryIds { get; private set; }
        public readonly int MaxLevel;
        public readonly bool CanGainExperience;
        public readonly string ExperiencePayoutFormula;
        public readonly string ExperienceToLevelUpFormula;

        #endregion

        #region Player Character-only data

        public readonly bool RequiresNamePrompt;
        public readonly string InitialEquippedWeaponId;
        public readonly string InitialEquippedArmorId;
        public readonly float SaleValuePercentage;

        #endregion

        #region NPC-only data

        public ActionWithEffects OnSpawn { get; set; }
        public List<ActionWithEffects> OnInteracted { get; set; }

        public readonly bool KnowsAllCharacterPositions;
        public readonly bool PursuesOutOfSightCharacters;
        public readonly bool WandersIfWithoutTarget;
        public readonly AIType AIType;

        public readonly LootTable LootTable;
        public readonly int DropPicks;

        #endregion
        public readonly string Power;

        #region Item-only data
        public ActionWithEffects OnUse { get; set; }
        public List<PassiveStatModifier> StatModifiers { get; set; }
        public int BaseValue { get; set; }
       
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
        public EntityClass(ClassInfo classInfo, Locale Locale, EntityType? entityType, List<StatInfo> statInfos, List<ActionSchool> actionSchools, List<LootTable> lootTables)
        {
            Id = classInfo.Id;
            Name = Locale[classInfo.Name];
            Description = (classInfo.Description != null) ? Locale[classInfo.Description] : null;
            ConsoleRepresentation = classInfo.ConsoleRepresentation;

            if (classInfo is ItemInfo itemInfo)
            {
                EntityType = entityType ?? (EntityType)Enum.Parse(typeof(EntityType), itemInfo.EntityType);
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
                OnTurnStart = ActionWithEffects.Create(itemInfo.OnTurnStart, actionSchools);
                OnAttack = new List<ActionWithEffects>();
                MapActions(OnAttack, itemInfo.OnAttack, actionSchools);
                OnAttacked = ActionWithEffects.Create(itemInfo.OnAttacked, actionSchools);
                OnDeath = ActionWithEffects.Create(itemInfo.OnDeath, actionSchools);
                OnUse = ActionWithEffects.Create(itemInfo.OnUse, actionSchools);
                BaseValue = itemInfo.BaseValue;
            }
            else if (classInfo is PlayerClassInfo playerClassInfo)
            {
                FactionId = playerClassInfo.Faction;
                StartingWeaponId = playerClassInfo.StartingWeapon;
                StartingArmorId = playerClassInfo.StartingArmor;
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
                        CarriedRegeneration = 0
                    });
                }
                foreach (var stat in Stats)
                {
                    var correspondingStat = statInfos.Find(s => s.Id.Equals(stat.Id, StringComparison.InvariantCultureIgnoreCase))
                        ?? throw new InvalidDataException($"EntityClass {playerClassInfo.Id} has a Stat of Id {stat.Id}, which isn't found in the Dungeon Data.");
                    stat.Name = Locale[correspondingStat.Name];
                    stat.StatType = Enum.Parse<StatType>(correspondingStat.StatType);
                    stat.HasMax = correspondingStat.HasMax;
                    stat.MinCap = correspondingStat.MinCap;
                    stat.MaxCap = correspondingStat.MaxCap;
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
                OnTurnStart = ActionWithEffects.Create(playerClassInfo.OnTurnStart, actionSchools);
                OnAttack = new List<ActionWithEffects>();
                MapActions(OnAttack, playerClassInfo.OnAttack, actionSchools);
                OnAttacked = ActionWithEffects.Create(playerClassInfo.OnAttacked, actionSchools);
                OnDeath = ActionWithEffects.Create(playerClassInfo.OnDeath, actionSchools);
                OnLevelUp = ActionWithEffects.Create(playerClassInfo.OnLevelUp, actionSchools);
                EntityType = EntityType.Player;
                StartsVisible = playerClassInfo.StartsVisible;
                StartingInventoryIds = new List<string>(playerClassInfo.StartingInventory);
                Passable = false;
                RequiresNamePrompt = playerClassInfo.RequiresNamePrompt;
                InitialEquippedWeaponId = playerClassInfo.InitialEquippedWeapon;
                InitialEquippedArmorId = playerClassInfo.InitialEquippedArmor;
                SaleValuePercentage = playerClassInfo.SaleValuePercentage / 100f;
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
                        CarriedRegeneration = 0
                    });
                }
                foreach (var stat in Stats)
                {
                    var correspondingStat = statInfos.Find(s => s.Id.Equals(stat.Id, StringComparison.InvariantCultureIgnoreCase))
                        ?? throw new InvalidDataException($"EntityClass {npcInfo.Id} has a Stat of Id {stat.Id}, which isn't found in the Dungeon Data.");
                    stat.Name = Locale[correspondingStat.Name];
                    stat.StatType = Enum.Parse<StatType>(correspondingStat.StatType);
                    stat.HasMax = correspondingStat.HasMax;
                    stat.MinCap = correspondingStat.MinCap;
                    stat.MaxCap = correspondingStat.MaxCap;
                    stat.RegenerationTargetId = correspondingStat.RegeneratesStatId;
                }
                StartingWeaponId = npcInfo.StartingWeapon;
                StartingArmorId = npcInfo.StartingArmor;
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
                OnTurnStart = ActionWithEffects.Create(npcInfo.OnTurnStart, actionSchools);
                OnAttack = new List<ActionWithEffects>();
                MapActions(OnAttack, npcInfo.OnAttack, actionSchools);
                OnAttacked = ActionWithEffects.Create(npcInfo.OnAttacked, actionSchools);
                OnDeath = ActionWithEffects.Create(npcInfo.OnDeath, actionSchools);
                OnLevelUp = ActionWithEffects.Create(npcInfo.OnLevelUp, actionSchools);

                EntityType = EntityType.NPC;
                StartsVisible = npcInfo.StartsVisible;
                StartingInventoryIds = new List<string>(npcInfo.StartingInventory);
                Passable = false;
                AIType = Enum.Parse<AIType>(npcInfo.AIType);
                LootTable = (npcInfo.LootTableId != null && npcInfo.LootTableId != "None") ? lootTables.Find(lt => lt.Id.Equals(npcInfo.LootTableId, StringComparison.InvariantCultureIgnoreCase)) : null;
                DropPicks = npcInfo.DropPicks;
                KnowsAllCharacterPositions = npcInfo.KnowsAllCharacterPositions;
                PursuesOutOfSightCharacters = npcInfo.PursuesOutOfSightCharacters;
                WandersIfWithoutTarget = npcInfo.WandersIfWithoutTarget;
                OnSpawn = ActionWithEffects.Create(npcInfo.OnSpawn, actionSchools);
                OnInteracted = new List<ActionWithEffects>();
                MapActions(OnInteracted, npcInfo.OnInteracted, actionSchools);
            }
            else if (classInfo is TrapInfo trapInfo)
            {
                EntityType = EntityType.Trap;
                Power = trapInfo.Power;
                StartsVisible = trapInfo.StartsVisible;
                Passable = true;
                OnStepped = ActionWithEffects.Create(trapInfo.OnStepped, actionSchools);
            }
            else if (classInfo is AlteredStatusInfo alteredStatusInfo)
            {
                EntityType = EntityType.AlteredStatus;
                Passable = true;
                CanStack = alteredStatusInfo.CanStack;
                CanOverwrite = alteredStatusInfo.CanOverwrite;
                CleanseOnFloorChange = alteredStatusInfo.CleanseOnFloorChange;
                CleansedByCleanseActions = alteredStatusInfo.CleansedByCleanseActions;
                OnTurnStart = ActionWithEffects.Create(alteredStatusInfo.OnTurnStart, actionSchools);
                OnApply = ActionWithEffects.Create(alteredStatusInfo.OnApply, actionSchools);
                OnAttacked = ActionWithEffects.Create(alteredStatusInfo.OnAttacked, actionSchools);
                BeforeAttack = ActionWithEffects.Create(alteredStatusInfo.BeforeAttack, actionSchools);
                OnRemove = ActionWithEffects.Create(alteredStatusInfo.OnRemove, actionSchools);
            }
        }

        protected static void MapActions(List<ActionWithEffects> actionList, List<ActionWithEffectsInfo> actionInfoList, List<ActionSchool> actionSchools)
        {
            actionInfoList.ForEach(aa => actionList.Add(ActionWithEffects.Create(aa, actionSchools)));
        }
    }
    #pragma warning restore CS8601 // Posible asignación de referencia nula
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
