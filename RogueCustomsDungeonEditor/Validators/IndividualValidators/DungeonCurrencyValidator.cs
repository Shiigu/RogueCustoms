using System;
using System.Linq;
using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsDungeonEditor.Utils;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
{
    public static class DungeonCurrencyValidator
    {
        public static DungeonValidationMessages Validate(DungeonInfo dungeonJson)
        {
            var messages = new DungeonValidationMessages();

            var currencyJson = dungeonJson.CurrencyInfo;

            messages.AddRange(dungeonJson.ValidateString(currencyJson.Name, "Currency", "Name", true));
            messages.AddRange(dungeonJson.ValidateString(currencyJson.Description, "Currency", "Description", false));
            messages.AddRange(currencyJson.ConsoleRepresentation.Validate("Currency", false, dungeonJson));

            var duplicatePiles = currencyJson.CurrencyPiles
                    .GroupBy(e => e.Id)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();

            if (duplicatePiles.Count > 0)
            {
                messages.AddError("Currency has duplicate Pile Types.");
            }

            foreach (var type in currencyJson.CurrencyPiles)
            {
                if (string.IsNullOrWhiteSpace(type.Id))
                {
                    messages.AddError("There's a Currency Pile Type without an Id.");
                    continue;
                }
                if (type.Minimum < 0)
                    messages.AddError($"Currency Pile Type {type.Id} has a Minimum lower than 0.");
                if (type.Maximum < 0)
                    messages.AddError($"Currency Pile Type {type.Id} has a Maximum lower than 0.");
                if (type.Maximum < type.Minimum)
                    messages.AddError($"Currency Pile Type {type.Id} has a Maximum lower than its Minimum.");
                else if (type.Maximum == type.Minimum)
                    messages.AddWarning($"Currency Pile Type {type.Id} has a Maximum equal to its Minimum. All Currency Piles of this Type will hold the same amount.");

                if (!dungeonJson.FloorInfos.Any(fi => fi.PossibleItems.Any(pi => pi.ClassId == $"Currency ({type.Id})"))
                    && !dungeonJson.LootTableInfos.Any(lt => lt.Entries.Any(pi => pi.PickId == $"Currency ({type.Id})")))
                {
                    messages.AddWarning($"There's a Currency Pile Type, {type.Id}, that is not found to drop from any Loot Table or spawn in any Floor Configuration.");
                }
            }

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}
