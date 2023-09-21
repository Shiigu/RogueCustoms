using RogueCustomsDungeonEditor.Utils;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
{
    public class DungeonAuthorValidator
    {
        public static DungeonValidationMessages Validate(Dungeon dungeon)
        {
            var messages = new DungeonValidationMessages();
            var infosToAdd = new List<string>();
            var successesToAdd = new List<string>();
            var errorsToAdd = new List<string>();
            var warningsToAdd = new List<string>();
            var author = dungeon.Author;

            infosToAdd.Add("AUTHOR VALIDATION STARTED");

            if (author == null)
                errorsToAdd.Add("Dungeon does not have an Author.");
            else if (string.IsNullOrWhiteSpace(author))
                errorsToAdd.Add("Dungeon's Author name is empty.");
            if (author != null && !author.CanBeEncodedToIBM437())
                warningsToAdd.Add($"Dungeon's Author cannot be properly encoded to IBM437. Console clients may display it incorrectly.");
            if (!errorsToAdd.Any())
                successesToAdd.Add("AUTHOR VALIDATION SUCCESSFUL");
            else
                errorsToAdd.Add("AUTHOR VALIDATION FAILED");

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
            var author = dungeonJson.Author;

            if (author == null)
                messages.Add("Dungeon does not have an Author.", DungeonValidationMessageType.Error);
            else if (string.IsNullOrWhiteSpace(author))
                messages.Add("Dungeon's Author name is empty.", DungeonValidationMessageType.Error);
            if (author != null && !author.CanBeEncodedToIBM437())
                messages.Add("Dungeon's Author cannot be properly encoded to IBM437. Console clients may display it incorrectly.", DungeonValidationMessageType.Warning);

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}
