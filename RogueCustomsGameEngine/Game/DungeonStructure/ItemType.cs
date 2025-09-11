using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsGameEngine.Game.DungeonStructure
{
    [Serializable]
    public class ItemType
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ItemUsability Usability { get; set; }
        public ItemPowerType PowerType { get; set; }
        public List<ItemSlot> SlotsItOccupies { get; set; }

        public ItemType(ItemTypeInfo info, Dungeon dungeon)
        {
            Id = info.Id;
            Name = dungeon.LocaleToUse[info.Name];
            Usability = info.Usability;
            PowerType = info.PowerType;
            SlotsItOccupies = [];
            if (!string.IsNullOrEmpty(info.Slot1))
            {
                var slot1 = dungeon.ItemSlots.Find(x => x.Id == info.Slot1);
                if (slot1 != null)
                {
                    SlotsItOccupies.Add(slot1);
                }
                var slot2 = dungeon.ItemSlots.Find(x => x.Id == info.Slot2);
                if (slot2 != null)
                {
                    SlotsItOccupies.Add(slot2);
                }
            }
        }
    }
}
