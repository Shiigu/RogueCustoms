using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
{
    public static class DungeonQualityLevelValidator
    {
        public static async Task<DungeonValidationMessages> Validate(DungeonInfo dungeonJson)
        {
            var messages = new DungeonValidationMessages();

            foreach (var qualityLevelJson in dungeonJson.QualityLevelInfos)
            {
                if (!string.IsNullOrWhiteSpace(qualityLevelJson.Id))
                {
                    if (dungeonJson.QualityLevelInfos.Any(a => a != qualityLevelJson && a.Id.Equals(qualityLevelJson.Id, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        messages.AddError($"Affix {qualityLevelJson.Id} has a duplicate Id.");
                    }
                    messages.AddRange(dungeonJson.ValidateString(qualityLevelJson.Name, "Quality Level", "Name", true));
                    if (qualityLevelJson.MinimumAffixes < 0)
                    {
                        messages.AddError($"Quality Level {qualityLevelJson.Id} has Minimum Affixes set to less than 0.");
                    }
                    if(qualityLevelJson.MaximumAffixes < 0)
                    {
                        messages.AddError($"Quality Level {qualityLevelJson.Id} has Maximum Affixes set to less than 0.");
                    }
                    if (qualityLevelJson.MaximumAffixes < qualityLevelJson.MinimumAffixes)
                    {
                        messages.AddError($"Quality Level {qualityLevelJson.Id} has a Maximum Affixes set to less than its Minimum Affixes.");
                    }
                    if (!Enum.TryParse<QualityLevelNameAttachment>(qualityLevelJson.AttachesWhatToItemName, out _))
                    {
                        messages.AddError($"Quality Level {qualityLevelJson.Id} has an invalid Affix Type.");
                    }
                }
                else
                {
                    messages.AddError($"Quality Level #{dungeonJson.QualityLevelInfos.IndexOf(qualityLevelJson)} lacks an Id.");
                }
            }

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}
