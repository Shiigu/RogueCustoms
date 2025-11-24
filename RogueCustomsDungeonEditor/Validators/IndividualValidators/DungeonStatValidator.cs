using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.JsonImports;

using System;
using System.Linq;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
{
    public static class DungeonStatValidator
    {
        public static DungeonValidationMessages Validate(StatInfo stat, DungeonInfo dungeonJson)
        {
            var messages = new DungeonValidationMessages();

            messages.AddRange(dungeonJson.ValidateString(stat.Name, $"Stat {stat.Id}", "Name", true));

            if (string.IsNullOrWhiteSpace(stat.StatType))
                messages.AddError("The Stat lacks a Type");

            if (stat.StatType.Equals("Regeneration", StringComparison.InvariantCultureIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(stat.RegeneratesStatId))
                    messages.AddError("The Stat's Type is Regeneration, but no Regeneration Target has been set");
                else if (!dungeonJson.CharacterStats.Any(s => s.Id.Equals(stat.RegeneratesStatId, StringComparison.InvariantCultureIgnoreCase)))
                    messages.AddError($"The Stat's Regeneration Target, {stat.RegeneratesStatId}, is not valid");
            }

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}