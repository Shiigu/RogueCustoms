using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Utils.InputsAndOutputs
{
    public class EntityDetailDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ConsoleRepresentation ConsoleRepresentation { get; set; }
        public EntityDetailDto() { }

        public EntityDetailDto(Entity entity)
        {
            Name = entity.Name;
            Description = entity.Description;
            ConsoleRepresentation = entity.ConsoleRepresentation;
        }
    }
}
