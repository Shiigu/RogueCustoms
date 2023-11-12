using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.DungeonStructure;
using System.Collections.Generic;
using System.Collections.Immutable;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;

namespace RogueCustomsGameEngine.Utils.InputsAndOutputs
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public class ActionListDto
    {
        public string TargetName { get; set; }
        public ImmutableList<ActionItemDto> Actions { get; private set; }

        public ActionListDto(string targetName)
        {
            TargetName = targetName;
            Actions = ImmutableList.Create<ActionItemDto>();
        }

        public void AddAction(ActionWithEffects action, Character source, Character target, Map map, bool isPlayerAction)
        {
            var entityDescriptor = isPlayerAction ? "Player" : "Target";
            Actions = Actions.Add(new ActionItemDto(action, source, target, map)
            {
                SelectionId = $"{entityDescriptor}_{action.ActionId}"
            });
        }
    }

    [Serializable]
    public class ActionItemDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool CanBeUsed { get; set; }
        public string SelectionId { get; set; }

        public ActionItemDto() { }

        public ActionItemDto(ActionWithEffects action, Character source, Character target, Map map)
        {
            Name = map.Locale[action.Name];
            Description = action.GetDescriptionWithUsageNotes(target, source);
            CanBeUsed = action.CanBeUsedOn(target, source);
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
