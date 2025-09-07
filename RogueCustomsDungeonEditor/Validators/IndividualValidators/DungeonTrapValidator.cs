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
            var trapAsInstance = sampleDungeon != null ? new Trap(new EntityClass(trapJson, EntityType.Trap, sampleDungeon, dungeonJson.CharacterStats), sampleDungeon.CurrentFloor) : null;
            var messages = new DungeonValidationMessages();

            messages.AddRange(dungeonJson.ValidateString(trapJson.Name, "Trap", "Name", true));
            messages.AddRange(dungeonJson.ValidateString(trapJson.Description, "Trap", "Description", false));

            messages.AddRange(trapJson.ConsoleRepresentation.Validate(trapJson.Id, false, dungeonJson));

            if (string.IsNullOrWhiteSpace(trapJson.Power))
                messages.AddError("Trap does not have a set Power.");

            if (trapJson.OnStepped != null)
            {
                if(trapAsInstance != null)
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
