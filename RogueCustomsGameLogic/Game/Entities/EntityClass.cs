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
        public readonly string Description;
        public readonly string FactionId;
        public Faction Faction { get; set; }
        public readonly ConsoleRepresentation ConsoleRepresentation;
        public readonly EntityType EntityType;
        public readonly bool Passable;
        public readonly bool StartsVisible;

        #region Character-only data

        public readonly int BaseHP;
        public readonly int BaseAttack;
        public readonly int BaseDefense;
        public readonly int BaseMovement;
        public readonly decimal BaseHPRegeneration;
        public readonly int BaseSightRange;
        public readonly bool KnowsAllCharacterPositions;
        public readonly int InventorySize;
        public readonly string StartingWeaponId;
        public readonly string StartingArmorId;
        public readonly int MaxLevel;
        public readonly bool CanGainExperience;
        public readonly string ExperiencePayoutFormula;
        public readonly string ExperienceToLevelUpFormula;
        public readonly decimal MaxHPIncreasePerLevel;
        public readonly decimal AttackIncreasePerLevel;
        public readonly decimal DefenseIncreasePerLevel;
        public readonly decimal MovementIncreasePerLevel;
        public readonly decimal HPRegenerationIncreasePerLevel;

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
        public EntityClass(ClassInfo classInfo, Locale Locale)
        {
            Id = classInfo.Id;
            Name = Locale[classInfo.Name];
            Description = (classInfo.Description != null) ? Locale[classInfo.Description] : null;
            FactionId = classInfo.Faction;
            ConsoleRepresentation = classInfo.ConsoleRepresentation;
            EntityType = (EntityType)Enum.Parse(typeof(EntityType), classInfo.EntityType);
            StartsVisible = classInfo.StartsVisible;
            if (EntityType == EntityType.Player || EntityType == EntityType.NPC)
            {
                Passable = false;
            }
            else if (EntityType == EntityType.Consumable || EntityType == EntityType.Weapon || EntityType == EntityType.Armor || EntityType == EntityType.Trap)
            {
                Passable = true;
            }

            Power = classInfo.Power;

            #region Character-only data

            BaseHP = classInfo.BaseHP;
            BaseAttack = classInfo.BaseAttack;
            BaseDefense = classInfo.BaseDefense;
            BaseMovement = classInfo.BaseMovement;
            BaseHPRegeneration = classInfo.BaseHPRegeneration;
            StartingWeaponId = classInfo.StartingWeapon;
            StartingArmorId = classInfo.StartingArmor;
            CanGainExperience = classInfo.CanGainExperience;
            MaxLevel = classInfo.MaxLevel;
            ExperiencePayoutFormula = classInfo.ExperiencePayoutFormula;
            ExperienceToLevelUpFormula = classInfo.ExperienceToLevelUpFormula;
            MaxHPIncreasePerLevel = classInfo.MaxHPIncreasePerLevel;
            AttackIncreasePerLevel = classInfo.AttackIncreasePerLevel;
            DefenseIncreasePerLevel = classInfo.DefenseIncreasePerLevel;
            MovementIncreasePerLevel = classInfo.MovementIncreasePerLevel;
            HPRegenerationIncreasePerLevel = classInfo.HPRegenerationIncreasePerLevel;
            KnowsAllCharacterPositions = classInfo.KnowsAllCharacterPositions;
            BaseSightRange = 0;
            if (!string.IsNullOrWhiteSpace(classInfo.BaseSightRange))
            {
                switch (classInfo.BaseSightRange.ToLower())
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
                        if (!int.TryParse(classInfo.BaseSightRange, out int sightRange) || sightRange < 0)
                            throw new InvalidDataException($"Sight Range of {classInfo.BaseSightRange} is not valid.");
                        BaseSightRange = sightRange;
                        break;
                }
            }
            InventorySize = classInfo.InventorySize;

            #endregion

            #region Item-only datas

            CanBePickedUp = classInfo.CanBePickedUp;
            OnItemSteppedActions = new List<ActionWithEffects>();
            MapActions(OnItemSteppedActions, classInfo.OnItemSteppedActions);
            OnItemUseActions = new List<ActionWithEffects>();
            MapActions(OnItemUseActions, classInfo.OnItemUseActions);

            #endregion
            
            #region Status-only datas

            CanStack = classInfo.CanStack;
            CanOverwrite = classInfo.CanOverwrite;
            CleanseOnFloorChange = classInfo.CleanseOnFloorChange;
            CleansedByCleanseActions = classInfo.CleansedByCleanseActions;
            OnStatusApplyActions = new List<ActionWithEffects>();
            MapActions(OnStatusApplyActions, classInfo.OnStatusApplyActions);

            #endregion

            OnTurnStartActions = new List<ActionWithEffects>();
            MapActions(OnTurnStartActions, classInfo.OnTurnStartActions);
            OnAttackActions = new List<ActionWithEffects>();
            MapActions(OnAttackActions, classInfo.OnAttackActions);
            OnAttackedActions = new List<ActionWithEffects>();
            MapActions(OnAttackedActions, classInfo.OnAttackedActions);
            OnDeathActions = new List<ActionWithEffects>();
            MapActions(OnDeathActions, classInfo.OnDeathActions);
        }

        protected void MapActions(List<ActionWithEffects> actionList, List<ActionWithEffectsInfo> actionInfoList)
        {
            actionInfoList.ForEach(aa => actionList.Add(new ActionWithEffects(aa)));
        }
    }
}
