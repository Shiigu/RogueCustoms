using RogueCustomsDungeonEditor.Utils;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
{
    public class DungeonFactionValidator
    {
        public static DungeonValidationMessages Validate(Dungeon dungeon)
        {
            var messages = new DungeonValidationMessages();
            var infosToAdd = new List<string>();
            var successesToAdd = new List<string>();
            var errorsToAdd = new List<string>();
            var warningsToAdd = new List<string>();

            infosToAdd.Add("FACTION VALIDATION STARTED");

            foreach (var faction in dungeon.Factions)
            {
                var name = faction.Name;
                var description = faction.Description;
                if(!faction.AlliedWith.Any() && !faction.NeutralWith.Any() && !faction.EnemiesWith.Any())
                    errorsToAdd.Add($"Faction {faction.Id} does not have any interactions with any faction.");
                foreach (var alliedFaction in faction.AlliedWith)
                {
                    if(!alliedFaction.AlliedWith.Contains(faction))
                        errorsToAdd.Add($"Faction {faction.Id} is allied with {alliedFaction.Id}, but not viceversa.");
                }
                foreach (var neutralFaction in faction.NeutralWith)
                {
                    if (!neutralFaction.NeutralWith.Contains(faction))
                        errorsToAdd.Add($"Faction {faction.Id} is neutral with {neutralFaction.Id}, but not viceversa.");
                }
                foreach (var enemyFaction in faction.EnemiesWith)
                {
                    if (!enemyFaction.EnemiesWith.Contains(faction))
                        errorsToAdd.Add($"Faction {faction.Id} is enemies with {enemyFaction.Id}, but not viceversa.");
                }
                if (name == null)
                    errorsToAdd.Add($"Faction {faction.Id} does not have a Name.");
                else if (string.IsNullOrWhiteSpace(name))
                    errorsToAdd.Add($"Faction {faction.Id}'s Name is empty.");
                if (name != null && !dungeon.LocaleToUse.ContainsKey(name) && !dungeon.LocaleToUse.IsValueInAKey(name))
                    warningsToAdd.Add($"Faction {faction.Id}'s Name isn't in the Locales dictionary. It's not localizable.");
                if (name != null && !name.CanBeEncodedToIBM437())
                    warningsToAdd.Add($"Faction {faction.Id}'s Name cannot be properly encoded to IBM437. Console clients may display it incorrectly.");
            }

            if (!errorsToAdd.Any() && !messages.HasErrors)
                successesToAdd.Add("FACTION VALIDATION SUCCESSFUL");
            else
                errorsToAdd.Add("FACTION VALIDATION FAILED");

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
        public static DungeonValidationMessages Validate(FactionInfo faction, DungeonInfo dungeonJson)
        {
            var messages = new DungeonValidationMessages();

            messages.AddRange(dungeonJson.ValidateString(faction.Name, $"Faction {faction.Id}", "Name", true));
            messages.AddRange(dungeonJson.ValidateString(faction.Description, $"Faction {faction.Id}", "Description", false));

            if (!faction.AlliedWith.Any() && !faction.NeutralWith.Any() && !faction.EnemiesWith.Any())
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