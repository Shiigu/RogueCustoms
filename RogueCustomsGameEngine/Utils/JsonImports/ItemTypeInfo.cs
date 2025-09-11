using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Utils.Enums;

namespace RogueCustomsGameEngine.Utils.JsonImports
{
    [Serializable]
    public class ItemTypeInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ItemUsability Usability { get; set; }
        public ItemPowerType PowerType { get; set; }
        public string Slot1 { get; set; }
        public string Slot2 { get; set; }
    }
}
