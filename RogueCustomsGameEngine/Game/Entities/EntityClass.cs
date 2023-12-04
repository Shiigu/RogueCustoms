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
        public readonly bool RequiresNamePrompt;
        public readonly string Description;
        public readonly string FactionId;
        public Faction Faction { get; set; }
        public readonly ConsoleRepresentation ConsoleRepresentation;
        public readonly EntityType EntityType;
        public readonly bool Passable;
        public readonly bool StartsVisible;

        #region Character-only data

        public bool UsesMP { get; set; }
        public readonly int BaseHP;
        public readonly int BaseMP;
        public readonly int BaseAttack;
        public readonly int BaseDefense;
        public readonly int BaseMovement;
        public readonly decimal BaseHPRegeneration;
        public readonly decimal BaseMPRegeneration;
        public readonly int BaseSightRange;
        public readonly int BaseAccuracy;
        public readonly int BaseEvasion;
        public readonly int InventorySize;
        public readonly string StartingWeaponId;
        public readonly string StartingArmorId;
        public List<string> StartingInventoryIds { get; private set; }
        public readonly int MaxLevel;
        public readonly bool CanGainExperience;
        public readonly string ExperiencePayoutFormula;
        public readonly string ExperienceToLevelUpFormula;
        public readonly decimal MaxHPIncreasePerLevel;
        public readonly decimal MaxMPIncreasePerLevel;
        public readonly decimal AttackIncreasePerLevel;
        public readonly decimal DefenseIncreasePerLevel;
        public readonly decimal MovementIncreasePerLevel;
        public readonly decimal HPRegenerationIncreasePerLevel;
        public readonly decimal MPRegenerationIncreasePerLevel;

        #endregion

        #region NPC-only data

        public ActionWithEffects OnSpawn { get; set; }
        public List<ActionWithEffects> OnInteracted { get; set; }

        public readonly bool KnowsAllCharacterPositions;
        public readonly AIType AIType;
        public readonly int AIOddsToUseActionsOnSelf;

        #endregion

        public readonly string Power;

        #region Item-only data

        public readonly bool CanBePickedUp;
        public ActionWithEffects OnStepped { get; set; }
        public ActionWithEffects OnUse { get; set; }

        #endregion

        public ActionWithEffects OnTurnStart { get; set; }
        public List<ActionWithEffects> OnAttack { get; set; }
        public ActionWithEffects OnAttacked { get; set; }
        public ActionWithEffects OnDeath { get; set; }

        #region Status-only data
        public readonly bool CanStack;
        public readonly bool CanOverwrite;
        public readonly bool CleanseOnFloorChange;
        public readonly bool CleansedByCleanseActions;

        public ActionWithEffects BeforeAttack { get; set; }

        public ActionWithEffects OnApply { get; set; }

        public ActionWithEffects OnRemove { get; set; }
        #endregion
        public EntityClass(ClassInfo classInfo, Locale Locale, EntityType? entityType)
        {
            Id = classInfo.Id;
            Name = Locale[classInfo.Name];
            Description = (classInfo.Description != null) ? Locale[classInfo.Description] : null;
            ConsoleRepresentation = classInfo.ConsoleRepresentation;

            if (classInfo is ItemInfo itemInfo)
            {
                EntityType = entityType ?? (EntityType)Enum.Parse(typeof(EntityType), itemInfo.EntityType);
                Power = itemInfo.Power;
                StartsVisible = itemInfo.StartsVisible;
                Passable = true;
                CanBePickedUp = itemInfo.CanBePickedUp;
                OnTurnStart = ActionWithEffects.Create(itemInfo.OnTurnStart);
                OnAttack = new List<ActionWithEffects>();
                MapActions(OnAttack, itemInfo.OnAttack);
                OnAttacked = ActionWithEffects.Create(itemInfo.OnAttacked);
                OnDeath = ActionWithEffects.Create(itemInfo.OnDeath);
                OnStepped = ActionWithEffects.Create(itemInfo.OnStepped);
                OnUse = ActionWithEffects.Create(itemInfo.OnUse);
            }
            else if (classInfo is PlayerClassInfo playerClassInfo)
            {
                UsesMP = playerClassInfo.UsesMP;
                FactionId = playerClassInfo.Faction;
                BaseHP = playerClassInfo.BaseHP;
                BaseMP = playerClassInfo.BaseMP;
                BaseAttack = playerClassInfo.BaseAttack;
                BaseDefense = playerClassInfo.BaseDefense;
                BaseMovement = playerClassInfo.BaseMovement;
                BaseHPRegeneration = playerClassInfo.BaseHPRegeneration;
                BaseMPRegeneration = playerClassInfo.BaseMPRegeneration;
                StartingWeaponId = playerClassInfo.StartingWeapon;
                StartingArmorId = playerClassInfo.StartingArmor;
                CanGainExperience = playerClassInfo.CanGainExperience;
                MaxLevel = playerClassInfo.MaxLevel;
                ExperiencePayoutFormula = playerClassInfo.ExperiencePayoutFormula;
                ExperienceToLevelUpFormula = playerClassInfo.ExperienceToLevelUpFormula;
                MaxHPIncreasePerLevel = playerClassInfo.MaxHPIncreasePerLevel;
                MaxMPIncreasePerLevel = playerClassInfo.MaxMPIncreasePerLevel;
                AttackIncreasePerLevel = playerClassInfo.AttackIncreasePerLevel;
                DefenseIncreasePerLevel = playerClassInfo.DefenseIncreasePerLevel;
                MovementIncreasePerLevel = playerClassInfo.MovementIncreasePerLevel;
                HPRegenerationIncreasePerLevel = playerClassInfo.HPRegenerationIncreasePerLevel;
                MPRegenerationIncreasePerLevel = playerClassInfo.MPRegenerationIncreasePerLevel;
                BaseSightRange = 0;
                BaseAccuracy = playerClassInfo.BaseAccuracy;
                BaseEvasion = playerClassInfo.BaseEvasion;
                if (!string.IsNullOrWhiteSpace(playerClassInfo.BaseSightRange))
                {
                    switch (playerClassInfo.BaseSightRange.ToLower())
                    {
                        case "full map":
                        case "fullmap":
                        case "whole map":
                        case "wholemap":
                            BaseSightRange = Constants.FullMapSightRange;
                            break;
                        case "full room":
                        case "fullroom":
                        case "whole room":
                        case "wholeroom":
                            BaseSightRange = Constants.FullRoomSightRange;
                            break;
                        default:
                            if (!int.TryParse(playerClassInfo.BaseSightRange, out int sightRange) || sightRange < 0)
                                throw new InvalidDataException($"Sight Range of {playerClassInfo.BaseSightRange} is not valid.");
                            BaseSightRange = sightRange;
                            break;
                    }
                }
                InventorySize = playerClassInfo.InventorySize;
                OnTurnStart = ActionWithEffects.Create(playerClassInfo.OnTurnStart);
                OnAttack = new List<ActionWithEffects>();
                MapActions(OnAttack, playerClassInfo.OnAttack);
                OnAttacked = ActionWithEffects.Create(playerClassInfo.OnAttacked);
                OnDeath = ActionWithEffects.Create(playerClassInfo.OnDeath);
                EntityType = EntityType.Player;
                StartsVisible = playerClassInfo.StartsVisible;
                StartingInventoryIds = new List<string>(playerClassInfo.StartingInventory);
                Passable = false;
                RequiresNamePrompt = playerClassInfo.RequiresNamePrompt;
            }
            else if (classInfo is NPCInfo npcInfo)
            {
                UsesMP = npcInfo.UsesMP;
                FactionId = npcInfo.Faction;
                BaseHP = npcInfo.BaseHP;
                BaseMP = npcInfo.BaseMP;
                BaseAttack = npcInfo.BaseAttack;
                BaseDefense = npcInfo.BaseDefense;
                BaseMovement = npcInfo.BaseMovement;
                BaseHPRegeneration = npcInfo.BaseHPRegeneration;
                BaseMPRegeneration = npcInfo.BaseMPRegeneration;
                StartingWeaponId = npcInfo.StartingWeapon;
                StartingArmorId = npcInfo.StartingArmor;
                CanGainExperience = npcInfo.CanGainExperience;
                MaxLevel = npcInfo.MaxLevel;
                ExperiencePayoutFormula = npcInfo.ExperiencePayoutFormula;
                ExperienceToLevelUpFormula = npcInfo.ExperienceToLevelUpFormula;
                MaxHPIncreasePerLevel = npcInfo.MaxHPIncreasePerLevel;
                MaxMPIncreasePerLevel = npcInfo.MaxMPIncreasePerLevel;
                AttackIncreasePerLevel = npcInfo.AttackIncreasePerLevel;
                DefenseIncreasePerLevel = npcInfo.DefenseIncreasePerLevel;
                MovementIncreasePerLevel = npcInfo.MovementIncreasePerLevel;
                HPRegenerationIncreasePerLevel = npcInfo.HPRegenerationIncreasePerLevel;
                MPRegenerationIncreasePerLevel = npcInfo.MPRegenerationIncreasePerLevel;
                BaseSightRange = 0;
                BaseAccuracy = npcInfo.BaseAccuracy;
                BaseEvasion = npcInfo.BaseEvasion;
                if (!string.IsNullOrWhiteSpace(npcInfo.BaseSightRange))
                {
                    switch (npcInfo.BaseSightRange.ToLower())
                    {
                        case "full map":
                        case "fullmap":
                        case "whole map":
                        case "wholemap":
                            BaseSightRange = Constants.FullMapSightRange;
                            break;
                        case "full room":
                        case "fullroom":
                        case "whole room":
                        case "wholeroom":
                            BaseSightRange = Constants.FullRoomSightRange;
                            break;
                        default:
                            if (!int.TryParse(npcInfo.BaseSightRange, out int sightRange) || sightRange < 0)
                                throw new InvalidDataException($"Sight Range of {npcInfo.BaseSightRange} is not valid.");
                            BaseSightRange = sightRange;
                            break;
                    }
                }
                InventorySize = npcInfo.InventorySize;
                OnTurnStart = ActionWithEffects.Create(npcInfo.OnTurnStart);
                OnAttack = new List<ActionWithEffects>();
                MapActions(OnAttack, npcInfo.OnAttack);
                OnAttacked = ActionWithEffects.Create(npcInfo.OnAttacked);
                OnDeath = ActionWithEffects.Create(npcInfo.OnDeath);

                EntityType = EntityType.NPC;
                StartsVisible = npcInfo.StartsVisible;
                StartingInventoryIds = new List<string>(npcInfo.StartingInventory);
                Passable = false;
                AIOddsToUseActionsOnSelf = npcInfo.AIOddsToUseActionsOnSelf;
                AIType = Enum.Parse<AIType>(npcInfo.AIType);
                KnowsAllCharacterPositions = npcInfo.KnowsAllCharacterPositions;
                OnSpawn = ActionWithEffects.Create(npcInfo.OnSpawn);
                OnInteracted = new List<ActionWithEffects>();
                MapActions(OnInteracted, npcInfo.OnInteracted);
            }
            else if (classInfo is TrapInfo trapInfo)
            {
                EntityType = EntityType.Trap;
                Power = trapInfo.Power;
                StartsVisible = trapInfo.StartsVisible;
                Passable = true;
                CanBePickedUp = false;
                OnStepped = ActionWithEffects.Create(trapInfo.OnStepped);
            }
            else if (classInfo is AlteredStatusInfo alteredStatusInfo)
            {
                EntityType = EntityType.AlteredStatus;
                Passable = true;
                CanStack = alteredStatusInfo.CanStack;
                CanOverwrite = alteredStatusInfo.CanOverwrite;
                CleanseOnFloorChange = alteredStatusInfo.CleanseOnFloorChange;
                CleansedByCleanseActions = alteredStatusInfo.CleansedByCleanseActions;
                OnTurnStart = ActionWithEffects.Create(alteredStatusInfo.OnTurnStart);
                OnApply = ActionWithEffects.Create(alteredStatusInfo.OnApply);
                OnAttacked = ActionWithEffects.Create(alteredStatusInfo.OnAttacked);
                BeforeAttack = ActionWithEffects.Create(alteredStatusInfo.BeforeAttack);
                OnRemove = ActionWithEffects.Create(alteredStatusInfo.OnRemove);
            }
        }

        protected static void MapActions(List<ActionWithEffects> actionList, List<ActionWithEffectsInfo> actionInfoList)
        {
            actionInfoList.ForEach(aa => actionList.Add(ActionWithEffects.Create(aa)));
        }
    }
    #pragma warning restore CS8601 // Posible asignación de referencia nula
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
