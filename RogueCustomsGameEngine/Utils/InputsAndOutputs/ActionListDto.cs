using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.DungeonStructure;
using System.Collections.Generic;

namespace RogueCustomsGameEngine.Utils.InputsAndOutputs
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
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
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
