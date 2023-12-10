using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.Representation;
using System.Collections.Generic;
using System.Linq;
using System;

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
        public bool IsInFloor { get; set; }
        public int ItemId { get; set; }

        public InventoryItemDto() { }

        public InventoryItemDto(Item item, PlayerCharacter player, Map map)
        {
            Name = map.Locale[item.Name];
            Description = item.Description;
            ConsoleRepresentation = item.ConsoleRepresentation;
            CanBeUsed = item.IsEquippable || item.OnUse?.CanBeUsedOn(player) == true;
            IsEquipped = player.EquippedWeapon == item || player.EquippedArmor == item;
            IsEquippable = item.IsEquippable;
            IsInFloor = item.Position != null && item.Owner == null;
            ItemId = item.Id;
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
