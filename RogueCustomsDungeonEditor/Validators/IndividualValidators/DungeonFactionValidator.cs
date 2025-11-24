using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.JsonImports;

using System.Linq;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
{
    public static class DungeonFactionValidator
    {
        public static DungeonValidationMessages Validate(FactionInfo faction, DungeonInfo dungeonJson)
        {
            var messages = new DungeonValidationMessages();

            messages.AddRange(dungeonJson.ValidateString(faction.Name, $"Faction {faction.Id}", "Name", true));
            messages.AddRange(dungeonJson.ValidateString(faction.Description, $"Faction {faction.Id}", "Description", false));

            if (dungeonJson.FactionInfos.Count > 1 && !faction.AlliedWith.Any() && !faction.NeutralWith.Any() && !faction.EnemiesWith.Any())
                messages.AddError($"Faction {faction.Id} does not have any interactions with any faction.");
            foreach (var alliedFaction in faction.AlliedWith.ConvertAll(aw => dungeonJson.FactionInfos.Find(fi => fi.Id.Equals(aw))))
            {
                if (!alliedFaction.AlliedWith.Contains(faction.Id))
                    messages.AddError($"Faction {faction.Id} is allied with {alliedFaction.Id}, but not viceversa.");
            }
            foreach (var neutralFaction in faction.NeutralWith.ConvertAll(nw => dungeonJson.FactionInfos.Find(fi => fi.Id.Equals(nw))))
            {
                if (!neutralFaction.NeutralWith.Contains(faction.Id))
                    messages.AddError($"Faction {faction.Id} is neutral with {neutralFaction.Id}, but not viceversa.");
            }
            foreach (var enemyFaction in faction.EnemiesWith.ConvertAll(ew => dungeonJson.FactionInfos.Find(fi => fi.Id.Equals(ew))))
            {
                if (!enemyFaction.EnemiesWith.Contains(faction.Id))
                    messages.AddError($"Faction {faction.Id} is enemies with {enemyFaction.Id}, but not viceversa.");
            }

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}