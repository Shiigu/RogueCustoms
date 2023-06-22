using org.matheval;
using RogueCustomsDungeonValidator.Utils;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsDungeonValidator.Validators.IndividualValidators
{
    public class DungeonAlteredStatusValidator
    {
        public static DungeonValidationMessages Validate(ClassInfo alteredStatus, DungeonInfo dungeonJson, Dungeon sampleDungeon)
        {
            var alteredStatusAsInstance = new AlteredStatus(new EntityClass(alteredStatus, sampleDungeon.LocaleToUse), sampleDungeon.CurrentFloor);
            var messages = new DungeonValidationMessages();

            messages.AddRange(dungeonJson.ValidateString(alteredStatus.Name, "Trap", "Name", true));
            messages.AddRange(dungeonJson.ValidateString(alteredStatus.Description, "Trap", "Description", false));

            messages.AddRange(alteredStatus.ConsoleRepresentation.Validate(alteredStatus.Id, dungeonJson));

            if (alteredStatus.CanStack && alteredStatus.CanOverwrite)
                messages.AddWarning("Altered Status has both CanStack and CanOverwrite set to True, making the latter be ignored by the game. Consider making one of them false.");

            if (!alteredStatus.CleansedByCleanseActions && !alteredStatus.CleanseOnFloorChange)
                messages.AddWarning("Altered Status has both CleansedByCleanseActions and CleanseOnFloorChange set to False, making the status unremovable if TurnLength is a negative number. Consider making one of them True.");

            if (alteredStatus.OnTurnStartActions.Any())
            {
                foreach (var onTurnStartAction in alteredStatusAsInstance.OwnOnTurnStartActions)
                {
                    messages.AddRange(ActionValidator.Validate(onTurnStartAction, dungeonJson, sampleDungeon));
                }
            }

            if (alteredStatus.OnStatusApplyActions.Any())
            {
                foreach (var onStatusApplyAction in alteredStatusAsInstance.OnStatusApplyActions)
                {
                    messages.AddRange(ActionValidator.Validate(onStatusApplyAction, dungeonJson, sampleDungeon));
                }
            }

            if (!alteredStatus.OnTurnStartActions.Any() && !alteredStatus.OnStatusApplyActions.Any())
                messages.AddError("Altered Status does not have OnTurnStartActions or OnStatusApplyActions. It needs to have both.");
            if (alteredStatus.OnAttackActions.Any())
                messages.AddWarning("Altered Status has OnAttackActions, which will be ignored by the game. Consider removing it.");
            if (alteredStatus.OnAttackedActions.Any())
                messages.AddWarning("Altered Status has OnAttackedActions, which will be ignored by the game. Consider removing it.");
            if (alteredStatus.OnDeathActions.Any())
                messages.AddWarning("Altered Status has OnDeathActions, which will be ignored by the game. Consider removing it.");
            if (alteredStatus.OnItemUseActions.Any())
                messages.AddWarning("Altered Status has OnItemSteppedActions, which will be ignored by the game. Consider removing it.");
            if (alteredStatus.OnItemUseActions.Any())
                messages.AddWarning("Altered Status has OnItemUseActions, which will be ignored by the game. Consider removing it.");

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}
