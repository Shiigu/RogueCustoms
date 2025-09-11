using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.Representation;
using System.Collections.Generic;
using System.Linq;
using System;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using System.Numerics;
using System.Drawing;
using RogueCustomsGameEngine.Utils.Enums;

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
        public int ItemLevel { get; set; }
        public string QualityLevel { get; set; }
        public string ItemType { get; set; }
        public List<string> SlotsItOccupies { get; set; }
        public GameColor QualityColor { get; set; }
        public string ClassId { get; set; }
        public List<StatModificationDto> StatModifications { get; set; }
        public List<string> OnAttackActions { get; set; }
        public ConsoleRepresentation ConsoleRepresentation { get; set; }
        public bool IsEquippable { get; set; }
        public bool IsEquipped { get; set; }
        public bool CanBeEquipped { get; set; }
        public bool CanBeUsed { get; set; }
        public bool CanBeDropped { get; set; }
        public bool IsInFloor { get; set; }
        public int ItemId { get; set; }
        public int Value { get; set; }
        public List<ExtraDamageDto> ExtraDamages { get; set; }

        public InventoryItemDto() { }

        public InventoryItemDto(IPickable p, Character character, Map map, bool forBuy)
        {
            var pickableAsEntity = p as Entity;
            if (p == null) return;
            var pickableAsItem = p as Item;
            Name = map.Locale[pickableAsEntity.Name];
            Description = pickableAsEntity.Description;
            PowerName = pickableAsItem?.ItemType?.PowerType switch
            {
                ItemPowerType.Damage => map.Locale["CharacterDamageStat"],
                ItemPowerType.Mitigation => map.Locale["CharacterMitigationStat"],
                _ => string.Empty
            };
            Power = pickableAsItem?.Power ?? string.Empty;
            ItemLevel = pickableAsItem?.ItemLevel ?? 0;
            ItemType = pickableAsItem != null ? pickableAsItem.ItemType.Name : "";
            SlotsItOccupies = pickableAsItem != null ? pickableAsItem.SlotsItOccupies.Select(s => s.Name).ToList() : new();
            QualityLevel = pickableAsItem != null ? pickableAsItem.QualityLevel.Name : string.Empty;
            QualityColor = pickableAsItem != null ? pickableAsItem.QualityLevel.ItemNameColor : new GameColor(Color.White);
            ConsoleRepresentation = pickableAsEntity.ConsoleRepresentation;
            CanBeDropped = p is not Key && character.ContainingTile.Type != TileType.Stairs && character.ContainingTile.Type.AcceptsItems;
            CanBeEquipped = pickableAsItem?.SlotsItOccupies.All(character.AvailableSlots.Contains) == true;
            CanBeUsed = (pickableAsItem?.IsEquippable == true && CanBeEquipped) || pickableAsItem?.OnUse?.CanBeUsedOn(character) == true;
            IsEquipped = character.Equipment.Contains(p);
            IsEquippable = pickableAsItem?.IsEquippable == true;
            IsInFloor = pickableAsEntity.Position != null && pickableAsItem?.Owner == null;
            ItemId = pickableAsEntity.Id;
            ClassId = pickableAsEntity.ClassId;
            var saleValuePercentage = character is PlayerCharacter pc ? pc.SaleValuePercentage : 1.0f;
            if (pickableAsItem != null)
                Value = forBuy || pickableAsItem.Value == 0 ? pickableAsItem.Value : (int)Math.Max(1, (pickableAsItem.Value * saleValuePercentage));
            else
                Value = 0;
            StatModifications = new();
            pickableAsItem?.StatModifiers.ForEach(m => {
                var correspondingStat = character.UsedStats.FirstOrDefault(s => s.Id.Equals(m.Id));
                if(correspondingStat != null)
                    StatModifications.Add(new StatModificationDto(m, correspondingStat, map));
            });
            OnAttackActions = new();
            pickableAsItem?.OwnOnAttack.ForEach(ooa => OnAttackActions.Add(ooa.Name));
            ExtraDamages = [];
            if(pickableAsItem?.ExtraDamage != null)
            {
                foreach (var extraDamage in pickableAsItem.ExtraDamage)
                {
                    if (extraDamage.MaximumDamage == 0) continue;
                    ExtraDamages.Add(new(extraDamage));
                }
            }
        }

        public InventoryItemDto(EntityClass e, Map map, bool forBuy)
        {
            if (e == null) return;
            Name = map.Locale[e.Name];
            Description = e.Description;
            PowerName = e.ItemType?.PowerType switch
            {
                ItemPowerType.Damage => map.Locale["CharacterDamageStat"],
                ItemPowerType.Mitigation => map.Locale["CharacterMitigationStat"],
                _ => string.Empty
            };
            ItemType = e.ItemType.Name;
            SlotsItOccupies = e.ItemType.SlotsItOccupies.Select(s => s.Name).ToList();
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
            ExtraDamages = [];
            QualityColor = new(Color.White);
        }
    }

    public class ExtraDamageDto()
    {
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public string DamageString => MinDamage != MaxDamage ? $"{MinDamage}-{MaxDamage}" : MinDamage.ToString();
        public string Element { get; set; }

        public ExtraDamageDto(ExtraDamage extraDamage) : this()
        {
            if (extraDamage == null) return;
            MinDamage = extraDamage.MinimumDamage;
            MaxDamage = extraDamage.MaximumDamage;
            Element = extraDamage.Element.Name;
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
