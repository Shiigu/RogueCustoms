using org.matheval;
using RogueCustomsDungeonEditor.Utils;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
{
    public class DungeonTrapValidator
    {
        public static DungeonValidationMessages Validate(ClassInfo trapJson, DungeonInfo dungeonJson, Dungeon sampleDungeon)
        {
            var trapAsInstance = new Item(new EntityClass(trapJson, sampleDungeon.LocaleToUse, EntityType.Trap), sampleDungeon.CurrentFloor);
            var messages = new DungeonValidationMessages();

            messages.AddRange(dungeonJson.ValidateString(trapJson.Name, "Trap", "Name", true));
            messages.AddRange(dungeonJson.ValidateString(trapJson.Description, "Trap", "Description", false));

            messages.AddRange(trapJson.ConsoleRepresentation.Validate(trapJson.Id, false, dungeonJson));

            if (string.IsNullOrWhiteSpace(trapJson.Power))
                messages.AddWarning("Trap does not have a set Power. Remember to hardcode Action Power parameters, or the game may crash.");

            if (trapJson.OnItemSteppedActions.Any())
            {
                foreach (var onItemSteppedAction in trapAsInstance.OnItemSteppedActions)
                {
                    messages.AddRange(ActionValidator.Validate(onItemSteppedAction, dungeonJson, sampleDungeon));
                }
            }
            else
            {
                messages.AddError("Trap doesn't have any OnItemSteppedActions.");
            }

            if (trapJson.OnTurnStartActions.Any())
                messages.AddWarning("Trap has OnTurnStartActions, which will be ignored by the game. Consider removing it.");
            if (trapJson.OnAttackActions.Any())
                messages.AddWarning("Trap has OnAttackActions, which will be ignored by the game. Consider removing it.");
            if (trapJson.OnAttackedActions.Any())
                messages.AddWarning("Trap has OnAttackedActions, which will be ignored by the game. Consider removing it.");
            if (trapJson.OnDeathActions.Any())
                messages.AddWarning("Trap has OnDeathActions, which will be ignored by the game. Consider removing it.");
            if (trapJson.OnItemUseActions.Any())
                messages.AddWarning("Trap has OnItemUseActions, which will be ignored by the game. Consider removing it.");
            if (trapJson.OnStatusApplyActions.Any())
                messages.AddWarning("Trap has OnStatusApplyActions, which will be ignored by the game. Consider removing it.");

            if (!dungeonJson.FloorInfos.Any(fi => fi.PossibleTraps.Any(pm => pm.ClassId.Equals(trapJson.Id))))
                messages.AddWarning("Trap does not show up in any list of PossibleTraps. It will never be spawned. Consider adding it to a PossibleTraps list.");

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}
