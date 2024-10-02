using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.Representation;
using System.Collections.Generic;
using System.Linq;
using System;
using RogueCustomsGameEngine.Game.Entities.Interfaces;

namespace RogueCustomsGameEngine.Utils.InputsAndOutputs
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public class InventoryDto
    {
        public bool TileIsOccupied { get; set; }

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
        public ConsoleRepresentation ConsoleRepresentation { get; set; }
        public bool IsEquippable { get; set; }
        public bool IsEquipped { get; set; }
        public bool CanBeUsed { get; set; }
        public bool CanBeDropped { get; set; }
        public bool IsInFloor { get; set; }
        public int ItemId { get; set; }

        public InventoryItemDto() { }

        public InventoryItemDto(IPickable p, PlayerCharacter player, Map map)
        {
            var pickableAsEntity = p as Entity;
            if (p == null) return;
            var pickableAsItem = p as Item;
            Name = map.Locale[pickableAsEntity.Name];
            Description = pickableAsEntity.Description;
            ConsoleRepresentation = pickableAsEntity.ConsoleRepresentation;
            CanBeUsed = pickableAsItem?.IsEquippable == true || pickableAsItem?.OnUse?.CanBeUsedOn(player) == true;
            CanBeDropped = p is not Key;
            IsEquipped = player.EquippedWeapon == pickableAsItem || player.EquippedArmor == pickableAsItem;
            IsEquippable = pickableAsItem?.IsEquippable == true;
            IsInFloor = pickableAsItem.Position != null && pickableAsItem.Owner == null;
            ItemId = pickableAsEntity.Id;
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
