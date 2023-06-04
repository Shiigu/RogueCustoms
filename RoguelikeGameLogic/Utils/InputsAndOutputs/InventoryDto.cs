using RoguelikeGameEngine.Game.DungeonStructure;
using RoguelikeGameEngine.Game.Entities;
using RoguelikeGameEngine.Utils.Representation;
using System.Collections.Generic;
using System.Linq;

namespace RoguelikeGameEngine.Utils.InputsAndOutputs
{
    public class InventoryDto
    {
        public bool TileIsOccupied { get; set; }

        public List<InventoryItemDto> InventoryItems { get; set; }

        public InventoryDto() {
            InventoryItems = new List<InventoryItemDto>();
        }
    }

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
            CanBeUsed = item.OnItemUseActions.Any(oiua => oiua.CanBeUsed);
            IsEquipped = player.EquippedWeapon == item || player.EquippedArmor == item;
            IsEquippable = item.EntityType == EntityType.Weapon || item.EntityType == EntityType.Armor;
            IsInFloor = item.Position != null && item.Owner == null;
            ItemId = item.Id;
        }
    }
}
