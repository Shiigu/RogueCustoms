using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsGameEngine.Game.DungeonStructure
{
    [Serializable]
    public class ItemSlot
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public ItemSlot(ItemSlotInfo info, Dungeon dungeon)
        {
            Id = info.Id;
            Name = dungeon.LocaleToUse[info.Name];
        }
    }
}
