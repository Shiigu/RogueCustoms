using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.Representation;
using System.Collections.Generic;
using System.Linq;
using System;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using System.Numerics;

namespace RogueCustomsGameEngine.Utils.InputsAndOutputs
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public class InventoryDto
    {
        public bool TileIsOccupied { get; set; }
        public ConsoleRepresentation CurrencyConsoleRepresentation { get; set; }

        public List<InventoryItemDto> InventoryItems { get; set; }

        public InventoryDto() {
            InventoryItems = new List<InventoryItemDto>();
        }
    }

    [Serializable]
    public class InventoryItemDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PowerName { get; set; }
        public string Power { get; set; }
        public string ClassId { get; set; }
        public List<StatModificationDto> StatModifications { get; set; }
        public List<string> OnAttackActions { get; set; }
        public ConsoleRepresentation ConsoleRepresentation { get; set; }
        public bool IsEquippable { get; set; }
        public bool IsEquipped { get; set; }
        public bool CanBeUsed { get; set; }
        public bool CanBeDropped { get; set; }
        public bool IsInFloor { get; set; }
        public int ItemId { get; set; }
        public int Value { get; set; }

        public InventoryItemDto() { }

        public InventoryItemDto(IPickable p, Character character, Map map, bool forBuy)
        {
            var pickableAsEntity = p as Entity;
            if (p == null) return;
            var pickableAsItem = p as Item;
            Name = map.Locale[pickableAsEntity.Name];
            Description = pickableAsEntity.Description;
            PowerName = pickableAsItem?.EntityType switch
            {
                EntityType.Weapon => map.Locale["CharacterDamageStat"],
                EntityType.Armor => map.Locale["CharacterMitigationStat"],
                _ => string.Empty
            };
            Power = pickableAsItem?.Power ?? string.Empty;
            ConsoleRepresentation = pickableAsEntity.ConsoleRepresentation;
            CanBeUsed = pickableAsItem?.IsEquippable == true || pickableAsItem?.OnUse?.CanBeUsedOn(character) == true;
            CanBeDropped = p is not Key;
            IsEquipped = character.EquippedWeapon == pickableAsItem || character.EquippedArmor == pickableAsItem;
            IsEquippable = pickableAsItem?.IsEquippable == true;
            IsInFloor = pickableAsEntity.Position != null && pickableAsItem?.Owner == null;
            ItemId = pickableAsEntity.Id;
            ClassId = pickableAsEntity.ClassId;
            var saleValuePercentage = character is PlayerCharacter pc ? pc.SaleValuePercentage : 1.0f;
            Value = forBuy || pickableAsItem.Value == 0 ? pickableAsItem.Value : (int) Math.Max(1, (pickableAsItem.Value * saleValuePercentage));
            StatModifications = new();
            pickableAsItem?.StatModifiers.ForEach(m => {
                var correspondingStat = character.UsedStats.FirstOrDefault(s => s.Id.Equals(m.Id));
                if(correspondingStat != null)
                    StatModifications.Add(new StatModificationDto(m, correspondingStat, map));
            });
            OnAttackActions = new();
            pickableAsItem?.OwnOnAttack.ForEach(ooa => OnAttackActions.Add(ooa.Name));
        }

        public InventoryItemDto(EntityClass e, Map map, bool forBuy)
        {
            if (e == null) return;
            Name = map.Locale[e.Name];
            Description = e.Description;
            PowerName = e.EntityType switch
            {
                EntityType.Weapon => map.Locale["CharacterDamageStat"],
                EntityType.Armor => map.Locale["CharacterMitigationStat"],
                _ => string.Empty
            };
            Power = e.Power ?? string.Empty;
            ConsoleRepresentation = e.ConsoleRepresentation;
            ClassId = e.Id;
            StatModifications = [];
            e.StatModifiers.ForEach(m => {
                var correspondingStat = map.Player.UsedStats.FirstOrDefault(s => s.Id.Equals(m.Id));
                if (correspondingStat != null)
                    StatModifications.Add(new StatModificationDto(m, correspondingStat, map));
            });
            OnAttackActions = [];
            e.OnAttack.ForEach(ooa => OnAttackActions.Add(map.Locale[ooa.Name]));
            Value = forBuy || e.BaseValue == 0 ? e.BaseValue : (int) Math.Max(1,(e.BaseValue * map.Player.SaleValuePercentage));
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
