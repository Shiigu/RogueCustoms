using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Utils.InputsAndOutputs
{
    public class ItemDetailDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ConsoleRepresentation ConsoleRepresentation { get; set; }
        public ItemDetailDto() { }

        public ItemDetailDto(Item i)
        {
            Name = i.Name;
            Description = i.Description;
            ConsoleRepresentation = i.ConsoleRepresentation;
        }
        public ItemDetailDto(EntityClass itemClass, Dungeon dungeon)
        {
            Name = dungeon.LocaleToUse[itemClass.Name];
            Description = dungeon.LocaleToUse[itemClass.Description];
            ConsoleRepresentation = itemClass.ConsoleRepresentation;
        }
    }
}
