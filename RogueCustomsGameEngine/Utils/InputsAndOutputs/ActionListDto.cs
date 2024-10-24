using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.DungeonStructure;
using System.Collections.Generic;
using System.Collections.Immutable;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.Enums;

namespace RogueCustomsGameEngine.Utils.InputsAndOutputs
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public class ActionListDto
    {
        public string TargetName { get; set; }
        public readonly List<ActionItemDto> Actions;

        public ActionListDto(string targetName)
        {
            TargetName = targetName;
            Actions = new List<ActionItemDto>();
        }

        public void AddAction(ActionWithEffects action, Character source, Character targetCharacter, Tile tile, Map map, bool isPlayerAction)
        {
            Actions.Add(new ActionItemDto(action, source, action.TargetTypes.Contains(TargetType.Tile) ? tile : targetCharacter, map)
            {
                SelectionId = action.ActionId
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

        public ActionItemDto(ActionWithEffects action, Character source, ITargetable target, Map map)
        {
            Name = map.Locale[action.Name];            
            Description = action.GetDescriptionWithUsageNotes(target, source);
            CanBeUsed = action.CanBeUsedOn(target, source);
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
