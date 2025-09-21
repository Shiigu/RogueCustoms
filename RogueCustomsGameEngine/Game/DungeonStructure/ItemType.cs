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
        public QualityLevel MinimumQualityLevelForUnidentified { get; set; }
        public string UnidentifiedItemName { get; set; }
        public string UnidentifiedItemDescription { get; set; }
        public string UnidentifiedItemActionName { get; set; }
        public string UnidentifiedItemActionDescription { get; set; }

        public ItemType(ItemTypeInfo info, Dungeon dungeon)
        {
            Id = info.Id;
            Name = dungeon.LocaleToUse[info.Name];
            Usability = info.Usability;
            PowerType = info.PowerType;
            SlotsItOccupies = [];
            if (!string.IsNullOrWhiteSpace(info.Slot1))
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
            MinimumQualityLevelForUnidentified = null;
            if (!string.IsNullOrWhiteSpace(info.MinimumQualityLevelForUnidentified))
            {
                var qualityLevel = dungeon.QualityLevels.Find(x => x.Id == info.MinimumQualityLevelForUnidentified);
                if (qualityLevel != null)
                {
                    MinimumQualityLevelForUnidentified = qualityLevel;
                }
            }
            UnidentifiedItemName = dungeon.LocaleToUse[info.UnidentifiedItemName];
            UnidentifiedItemDescription = dungeon.LocaleToUse[info.UnidentifiedItemDescription];
            UnidentifiedItemActionName = dungeon.LocaleToUse[info.UnidentifiedItemActionName];
            UnidentifiedItemActionDescription = dungeon.LocaleToUse[info.UnidentifiedItemActionDescription];
        }
    }
}
