using RoguelikeGameEngine.Game.Entities;
using RoguelikeGameEngine.Game.DungeonStructure;

namespace RoguelikeGameEngine.Utils.InputsAndOutputs
{

    public class ActionItemDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool CanBeUsed { get; set; }

        public ActionItemDto() { }

        public ActionItemDto(ActionWithEffects action, Character target, Map map)
        {
            Name = action.Name;
            Description = action.GetDescriptionWithUsageNotes(target);
            CanBeUsed = action.CanBeUsedOn(target, map);
        }
    }
}
