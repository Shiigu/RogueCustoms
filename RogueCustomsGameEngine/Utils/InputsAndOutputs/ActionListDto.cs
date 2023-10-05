using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.DungeonStructure;
using System.Collections.Generic;

namespace RogueCustomsGameEngine.Utils.InputsAndOutputs
{
    public class ActionListDto
    {
        public string TargetName { get; set; }
        public List<ActionItemDto> Actions { get; set; }
    }

    public class ActionItemDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool CanBeUsed { get; set; }

        public ActionItemDto() { }

        public ActionItemDto(ActionWithEffects action, Character target, Map map)
        {
            Name = map.Locale[action.Name];
            Description = action.GetDescriptionWithUsageNotes(target);
            CanBeUsed = action.CanBeUsedOn(target, map);
        }
    }
}
