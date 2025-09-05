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
    public class DungeonAlteredStatusValidator
    {
        public static async Task<DungeonValidationMessages> Validate(AlteredStatusInfo alteredStatus, DungeonInfo dungeonJson, Dungeon sampleDungeon)
        {
            var alteredStatusAsInstance = sampleDungeon != null ? new AlteredStatus(new EntityClass(alteredStatus, sampleDungeon.LocaleToUse, EntityType.AlteredStatus, null, sampleDungeon.ActionSchools, []), sampleDungeon.CurrentFloor) : null;
            var messages = new DungeonValidationMessages();

            messages.AddRange(dungeonJson.ValidateString(alteredStatus.Name, "Trap", "Name", true));
            messages.AddRange(dungeonJson.ValidateString(alteredStatus.Description, "Trap", "Description", false));

            messages.AddRange(alteredStatus.ConsoleRepresentation.Validate(alteredStatus.Id, false, dungeonJson));

            if (alteredStatus.CanStack && alteredStatus.CanOverwrite)
                messages.AddWarning("Altered Status has both CanStack and CanOverwrite set to True, making the latter be ignored by the game. Consider making one of them false.");

            if (!alteredStatus.CleansedByCleanseActions && !alteredStatus.CleanseOnFloorChange)
                messages.AddWarning("Altered Status has both CleansedByCleanseActions and CleanseOnFloorChange set to False, making the status unremovable if TurnLength is a negative number. Consider making one of them True.");

            if (alteredStatus.OnTurnStart == null && alteredStatus.OnApply == null)
            {
                messages.AddError("Altered Status does not have OnTurnStart or OnApply. It needs to have at least one of them.");
            }
            else
            {
                if (alteredStatus.OnTurnStart != null)
                {
                    messages.AddRange(await ActionValidator.Validate(alteredStatus.OnTurnStart, dungeonJson));
                    if(alteredStatusAsInstance != null)
                        messages.AddRange(await ActionValidator.Validate(alteredStatusAsInstance.OwnOnTurnStart, dungeonJson, sampleDungeon));
                }

                if (alteredStatus.OnApply != null)
                {
                    messages.AddRange(await ActionValidator.Validate(alteredStatus.OnApply, dungeonJson));
                    if (alteredStatusAsInstance != null)
                        messages.AddRange(await ActionValidator.Validate(alteredStatusAsInstance.OnApply, dungeonJson, sampleDungeon));
                }
            }

            if (alteredStatus.BeforeAttack != null)
            {
                messages.AddRange(await ActionValidator.Validate(alteredStatus.BeforeAttack, dungeonJson));
                if (alteredStatusAsInstance != null)
                    messages.AddRange(await ActionValidator.Validate(alteredStatusAsInstance.BeforeAttack, dungeonJson, sampleDungeon));
            }

            if (alteredStatus.OnAttacked != null)
            {
                messages.AddRange(await ActionValidator.Validate(alteredStatus.OnAttacked, dungeonJson));
                if (alteredStatusAsInstance != null)
                    messages.AddRange(await ActionValidator.Validate(alteredStatusAsInstance.OwnOnAttacked, dungeonJson, sampleDungeon));
            }

            if (alteredStatus.OnRemove != null)
            {
                messages.AddRange(await ActionValidator.Validate(alteredStatus.OnRemove, dungeonJson));
                if (alteredStatusAsInstance != null)
                    messages.AddRange(await ActionValidator.Validate(alteredStatusAsInstance.OnRemove, dungeonJson, sampleDungeon));
            }

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}
