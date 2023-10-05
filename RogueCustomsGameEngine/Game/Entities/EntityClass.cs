using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils;
using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;
using System.IO;

namespace RogueCustomsGameEngine.Game.Entities
{
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
        public readonly bool KnowsAllCharacterPositions;
        public readonly int InventorySize;
        public readonly int AIOddsToUseActionsOnSelf;
        public readonly string StartingWeaponId;
        public readonly string StartingArmorId;
        public readonly List<string> StartingInventoryIds;
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

        public readonly string Power;

        #region Item-only data

        public readonly bool CanBePickedUp;
        public List<ActionWithEffects> OnItemSteppedActions { get; set; }
        public List<ActionWithEffects> OnItemUseActions { get; set; }

        #endregion

        public List<ActionWithEffects> OnTurnStartActions { get; set; }
        public List<ActionWithEffects> OnAttackActions { get; set; }
        public List<ActionWithEffects> OnAttackedActions { get; set; }
        public List<ActionWithEffects> OnDeathActions { get; set; }

        #region Status-only data
        public readonly bool CanStack;
        public readonly bool CanOverwrite;
        public readonly bool CleanseOnFloorChange;
        public readonly bool CleansedByCleanseActions;
        public List<ActionWithEffects> OnStatusApplyActions { get; set; }
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
                OnTurnStartActions = new List<ActionWithEffects>();
                MapActions(OnTurnStartActions, itemInfo.OnTurnStartActions);
                OnAttackActions = new List<ActionWithEffects>();
                MapActions(OnAttackActions, itemInfo.OnAttackActions);
                OnAttackedActions = new List<ActionWithEffects>();
                MapActions(OnAttackedActions, itemInfo.OnAttackedActions);
                OnItemSteppedActions = new List<ActionWithEffects>();
                MapActions(OnItemSteppedActions, itemInfo.OnItemSteppedActions);
                OnItemUseActions = new List<ActionWithEffects>();
                MapActions(OnItemUseActions, itemInfo.OnItemUseActions);
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
                OnTurnStartActions = new List<ActionWithEffects>();
                MapActions(OnTurnStartActions, playerClassInfo.OnTurnStartActions);
                OnAttackActions = new List<ActionWithEffects>();
                MapActions(OnAttackActions, playerClassInfo.OnAttackActions);
                OnAttackedActions = new List<ActionWithEffects>();
                MapActions(OnAttackedActions, playerClassInfo.OnAttackedActions);
                OnDeathActions = new List<ActionWithEffects>();
                MapActions(OnDeathActions, playerClassInfo.OnDeathActions);

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
                OnTurnStartActions = new List<ActionWithEffects>();
                MapActions(OnTurnStartActions, npcInfo.OnTurnStartActions);
                OnAttackActions = new List<ActionWithEffects>();
                MapActions(OnAttackActions, npcInfo.OnAttackActions);
                OnAttackedActions = new List<ActionWithEffects>();
                MapActions(OnAttackedActions, npcInfo.OnAttackedActions);
                OnDeathActions = new List<ActionWithEffects>();
                MapActions(OnDeathActions, npcInfo.OnDeathActions);

                EntityType = EntityType.NPC;
                StartsVisible = npcInfo.StartsVisible;
                StartingInventoryIds = new List<string>(npcInfo.StartingInventory);
                Passable = false;
                AIOddsToUseActionsOnSelf = npcInfo.AIOddsToUseActionsOnSelf;
                KnowsAllCharacterPositions = npcInfo.KnowsAllCharacterPositions;
            }
            else if (classInfo is TrapInfo trapInfo)
            {
                EntityType = EntityType.Trap;
                Power = trapInfo.Power;
                StartsVisible = trapInfo.StartsVisible;
                Passable = true;
                CanBePickedUp = false;
                OnItemSteppedActions = new List<ActionWithEffects>();
                MapActions(OnItemSteppedActions, trapInfo.OnItemSteppedActions);
            }
            else if (classInfo is AlteredStatusInfo alteredStatusInfo)
            {
                EntityType = EntityType.AlteredStatus;
                Passable = true;
                OnTurnStartActions = new List<ActionWithEffects>();
                MapActions(OnTurnStartActions, alteredStatusInfo.OnTurnStartActions);
                CanStack = alteredStatusInfo.CanStack;
                CanOverwrite = alteredStatusInfo.CanOverwrite;
                CleanseOnFloorChange = alteredStatusInfo.CleanseOnFloorChange;
                CleansedByCleanseActions = alteredStatusInfo.CleansedByCleanseActions;
                OnStatusApplyActions = new List<ActionWithEffects>();
                MapActions(OnStatusApplyActions, alteredStatusInfo.OnStatusApplyActions);
            }
        }

        protected void MapActions(List<ActionWithEffects> actionList, List<ActionWithEffectsInfo> actionInfoList)
        {
            actionInfoList.ForEach(aa => actionList.Add(new ActionWithEffects(aa)));
        }
    }
}
