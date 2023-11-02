using RogueCustomsDungeonEditor.Utils;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
{
    public class DungeonNameValidator
    {
        public static DungeonValidationMessages Validate(Dungeon dungeon)
        {
            var messages = new DungeonValidationMessages();
            var infosToAdd = new List<string>();
            var successesToAdd = new List<string>();
            var errorsToAdd = new List<string>();
            var warningsToAdd = new List<string>();
            var name = dungeon.Name;

            infosToAdd.Add("NAME VALIDATION STARTED");
            if (name == null)
                errorsToAdd.Add("Dungeon does not have a Name.");
            else if (string.IsNullOrWhiteSpace(name))
                errorsToAdd.Add("Dungeon's Name is empty.");
            if (name != null && !dungeon.LocaleToUse.ContainsKey(name) && !dungeon.LocaleToUse.IsValueInAKey(name))
                warningsToAdd.Add($"Dungeon's Name isn't in the Locales dictionary. It's not localizable.");
            if (name?.CanBeEncodedToIBM437() == false)
                warningsToAdd.Add($"Dungeon's Name cannot be properly encoded to IBM437. Console clients may display it incorrectly.");
            if (!errorsToAdd.Any())
                successesToAdd.Add("NAME VALIDATION SUCCESSFUL");
            else
                errorsToAdd.Add("NAME VALIDATION FAILED");

            foreach (var info in infosToAdd)
            {
                messages.Add(info, DungeonValidationMessageType.Info);
            }
            foreach (var warnings in warningsToAdd)
            {
                messages.Add(warnings, DungeonValidationMessageType.Warning);
            }
            foreach (var success in successesToAdd)
            {
                messages.Add(success, DungeonValidationMessageType.Success);
            }
            foreach (var errors in errorsToAdd)
            {
                messages.Add(errors, DungeonValidationMessageType.Error);
            }

            return messages;
        }

        public static DungeonValidationMessages Validate(DungeonInfo dungeonJson)
        {
            var messages = new DungeonValidationMessages();

            messages.AddRange(dungeonJson.ValidateString(dungeonJson.Name, "Dungeon", "Name", true));

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}
