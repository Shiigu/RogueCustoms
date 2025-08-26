using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
{
    public class DungeonActionSchoolValidator
    {
        public static DungeonValidationMessages Validate(DungeonInfo dungeonJson)
        {
            var messages = new DungeonValidationMessages();

            foreach (var school in dungeonJson.ActionSchoolInfos)
            {
                if(string.IsNullOrWhiteSpace(school.Id))
                    messages.AddError("An Action School has a blank Id.");
                else if (dungeonJson.ActionSchoolInfos.Any(s => s != school && s.Id.Equals(school.Id, StringComparison.InvariantCultureIgnoreCase)))
                    messages.AddError($"The Action School Id '{school.Id}' is not unique.");
                else
                    messages.AddRange(dungeonJson.ValidateString(school.Name, school.Id, "Name", true));
            }

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}
