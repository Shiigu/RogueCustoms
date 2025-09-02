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
        public static async Task<DungeonValidationMessages> Validate(TrapInfo trapJson, DungeonInfo dungeonJson, Dungeon sampleDungeon)
        {
            var trapAsInstance = new Trap(new EntityClass(trapJson, sampleDungeon.LocaleToUse, EntityType.Trap, null, sampleDungeon.ActionSchools, []), sampleDungeon.CurrentFloor);
            var messages = new DungeonValidationMessages();

            messages.AddRange(dungeonJson.ValidateString(trapJson.Name, "Trap", "Name", true));
            messages.AddRange(dungeonJson.ValidateString(trapJson.Description, "Trap", "Description", false));

            messages.AddRange(trapJson.ConsoleRepresentation.Validate(trapJson.Id, false, dungeonJson));

            if (string.IsNullOrWhiteSpace(trapJson.Power))
                messages.AddWarning("Trap does not have a set Power. Remember to hardcode Action Power parameters, or the game may crash.");

            if (trapJson.OnStepped != null)
            {
                messages.AddRange(await ActionValidator.Validate(trapAsInstance.OnStepped, dungeonJson, sampleDungeon));
            }
            else
            {
                messages.AddError("Trap doesn't have any OnStepped.");
            }

            if (!dungeonJson.FloorInfos.Exists(fi => fi.PossibleTraps.Exists(pm => pm.ClassId.Equals(trapJson.Id))))
                messages.AddWarning("Trap does not show up in any list of PossibleTraps. It will never be spawned. Consider adding it to a PossibleTraps list.");

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}
