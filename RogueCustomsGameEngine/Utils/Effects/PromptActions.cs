using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualBasic.FileIO;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.Effects.Utils;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.Expressions;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;

namespace RogueCustomsGameEngine.Utils.Effects
{
#pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
#pragma warning disable CS8604 // Posible argumento de referencia nulo
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    
    public static class PromptActions
    {
        private static RngHandler Rng;
        private static Map Map;

        public static void SetActionParams(RngHandler rng, Map map)
        {
            Rng = rng;
            Map = map;
        }

        public static async Task<bool> SelectYesNo(EffectCallerParams Args)
        {
            dynamic paramsObject = ExpressionParser.ParseParams(Args);

            if (Map.IsDebugMode)
            {
                var rolledYes = Rng.RollProbability() > 50;
                var response = rolledYes ? paramsObject.YesButtonText : paramsObject.NoButtonText;
                Map.AppendMessage($"PROMPT => {paramsObject.Title}: {paramsObject.Message}\n\nRESPONSE: {response}", paramsObject.Color);
                return rolledYes;
            }
            else if(Args.Source is NonPlayableCharacter)
            {
                return true; // NPCs will only try to do this if they want to, so we assume they say "yes" to the prompt.
            }

            var events = new List<DisplayEventDto>();
            var triggerPromptEvent = new DisplayEventDto()
            {
                DisplayEventType = DisplayEventType.TriggerPrompt,
                Params = [false]
            };
            events.Add(triggerPromptEvent);
            Map.DisplayEvents.Add(($"Open Yes/No Prompt {paramsObject.Title}", events));

            while (!(bool) triggerPromptEvent.Params[0]) await Task.Delay(10);

            var result = await Map.OpenYesNoPrompt(paramsObject.Title, paramsObject.Message, paramsObject.YesButtonText, paramsObject.NoButtonText, paramsObject.Color);

            Map.DisplayEvents = new();
            Map.Snapshot = new(Map.Dungeon, Map);

            return result;
        }

        public static async Task<bool> SelectOption(EffectCallerParams Args)
        {
            dynamic paramsObject = ExpressionParser.ParseParams(Args);
            SelectionItem[] options = paramsObject.Options.ToArray();
            var optionFlag = paramsObject.OptionFlag;

            string? chosenOption;
            if (Map.IsDebugMode || Args.Source is NonPlayableCharacter)
            {
                var randomChoice = options.TakeRandomElementWithWeights(i => 50, Rng);
                chosenOption = randomChoice.Id;
                if (Map.IsDebugMode)
                    Map.AppendMessage($"PROMPT => {paramsObject.Title}: {paramsObject.Message}\n\nOPTION: {randomChoice.Id} => {randomChoice.Name}", paramsObject.Color);
            }
            else
            {
                var events = new List<DisplayEventDto>();
                var triggerPromptEvent = new DisplayEventDto()
                {
                    DisplayEventType = DisplayEventType.TriggerPrompt,
                    Params = [false]
                };
                events.Add(triggerPromptEvent);
                Map.DisplayEvents.Add(($"Open Select Option Prompt {paramsObject.Title}", events));

                while (!(bool)triggerPromptEvent.Params[0]) await Task.Delay(10);

                chosenOption = await Map.OpenSelectOption(paramsObject.Title, paramsObject.Message, options, paramsObject.Cancellable, paramsObject.Color);
                Map.DisplayEvents = new();
                Map.Snapshot = new(Map.Dungeon, Map);
            }

            if(!Map.HasFlag(optionFlag))
            {
                Map.CreateFlag(optionFlag, chosenOption, true);
            }
            else
            {
                Map.SetFlagValue(optionFlag, chosenOption);
            }

            return !string.IsNullOrWhiteSpace(chosenOption);
        }

        public static async Task<bool> SelectItem(EffectCallerParams Args)
        {
            dynamic paramsObject = ExpressionParser.ParseParams(Args);
            SelectionItem[] items = paramsObject.Items.ToArray();

            var optionDtos = new InventoryDto()
            {
                CurrencyConsoleRepresentation = Map.CurrencyClass.ConsoleRepresentation.Clone()
            };

            foreach (var option in items)
            {
                var itemEntityClass = Map.PossibleItemClasses.Find(ic => ic.Id == option.Id.TrimSurroundingQuotes());
                if (itemEntityClass == null) continue; // Invalid IDs are just ignored
                optionDtos.InventoryItems.Add(new InventoryItemDto(itemEntityClass, Map, false));
            }

            if(optionDtos.InventoryItems.Count == 0)
                throw new ArgumentException($"Invalid item selection for Select Item Prompt.");

            var optionFlag = paramsObject.OptionFlag;

            string? chosenOption;
            if (Map.IsDebugMode || Args.Source is NonPlayableCharacter)
            {
                var randomChoice = optionDtos.InventoryItems.TakeRandomElementWithWeights(i => 50, Rng);
                chosenOption = randomChoice.ClassId;
                if (Map.IsDebugMode)
                    Map.AppendMessage($"PROMPT => {paramsObject.Title}\n\nOPTION: {randomChoice.ClassId}", Color.Yellow);
            }
            else
            {
                var events = new List<DisplayEventDto>();
                var triggerPromptEvent = new DisplayEventDto()
                {
                    DisplayEventType = DisplayEventType.TriggerPrompt,
                    Params = [false]
                };
                events.Add(triggerPromptEvent);
                Map.DisplayEvents.Add(($"Open Select Item Prompt {paramsObject.Title}", events));

                while (!(bool)triggerPromptEvent.Params[0]) await Task.Delay(10);

                chosenOption = await Map.OpenSelectItem(paramsObject.Title, optionDtos, paramsObject.Cancellable);
            }

            if (!string.IsNullOrWhiteSpace(chosenOption))
            {
                if (!Map.HasFlag(optionFlag))
                {
                    Map.CreateFlag(optionFlag, chosenOption, true);
                }
                else
                {
                    Map.SetFlagValue(optionFlag, chosenOption);
                }
                Map.DisplayEvents = new();
                Map.Snapshot = new(Map.Dungeon, Map);

                return true;
            }

            return false;
        }

        public static async Task<bool> SelectOfferedItem(EffectCallerParams Args)
        {
            dynamic paramsObject = ExpressionParser.ParseParams(Args);

            if(Args.Source is not Character c)
                throw new ArgumentException($"Attempted to have {Args.Source.Name} choose an Item when it's not a Character.");
            if(paramsObject.Target is not Character t)
                throw new ArgumentException($"Attempted to have {paramsObject.Target.Name} offer Items when it's not a Character.");

            var optionDtos = new InventoryDto()
            {
                CurrencyConsoleRepresentation = Map.CurrencyClass.ConsoleRepresentation.Clone()
            };

            foreach (var item in t.Inventory)
            {
                var itemEntityClass = Map.PossibleItemClasses.Find(ic => ic.Id == item.ClassId);
                if (itemEntityClass == null) continue; // Invalid IDs are just ignored
                optionDtos.InventoryItems.Add(new InventoryItemDto(itemEntityClass, Map, false));
            }

            var optionFlag = paramsObject.OptionFlag;

            string? chosenOption;
            if (Map.IsDebugMode || Args.Source is NonPlayableCharacter)
            {
                var randomChoice = optionDtos.InventoryItems.TakeRandomElementWithWeights(i => 50, Rng);
                chosenOption = randomChoice.ClassId;
                if (Map.IsDebugMode)
                    Map.AppendMessage($"PROMPT => {paramsObject.Title}\n\nOPTION: {randomChoice.ClassId}", Color.Yellow);
            }
            else
            {
                var events = new List<DisplayEventDto>();
                var triggerPromptEvent = new DisplayEventDto()
                {
                    DisplayEventType = DisplayEventType.TriggerPrompt,
                    Params = [false]
                };
                events.Add(triggerPromptEvent);
                Map.DisplayEvents.Add(($"Open Select Inventory Item Prompt {paramsObject.Title}", events));

                while (!(bool)triggerPromptEvent.Params[0]) await Task.Delay(10);

                chosenOption = await Map.OpenSelectItem(paramsObject.Title, optionDtos, paramsObject.Cancellable);
            }

            if (!string.IsNullOrWhiteSpace(chosenOption))
            {
                if (!Map.HasFlag(optionFlag))
                {
                    Map.CreateFlag(optionFlag, chosenOption, true);
                }
                else
                {
                    Map.SetFlagValue(optionFlag, chosenOption);
                }
                Map.DisplayEvents = new();
                Map.Snapshot = new(Map.Dungeon, Map);

                return true;
            }

            return false;
        }

        public static async Task<bool> SelectAction(EffectCallerParams Args)
        {
            dynamic paramsObject = ExpressionParser.ParseParams(Args);

            if (Args.Source is not Character c)
                throw new ArgumentException($"Attempted to have {Args.Source.Name} choose an Interaction when it's not a Character.");
            if (paramsObject.Target is not Character t)
                throw new ArgumentException($"Attempted to choose one of {paramsObject.Target.Name}'s Interactions when it's not a Character.");

            var actionsNotFromConsumables = t.OnAttack.Where(oa => oa.User.EntityType != EntityType.Consumable).ToList();

            // Can't choose Interactions not from Consumables if there aren't any
            if (actionsNotFromConsumables.Count == 0)
                return false;

            var optionDtos = new ActionListDto(t.Name);

            foreach (var interaction in actionsNotFromConsumables)
            {
                optionDtos.Actions.Add(new ActionItemDto(interaction, Map));
            }

            var optionFlag = paramsObject.OptionFlag;
            var schoolFlag = paramsObject.SchoolFlag;

            string? chosenOption = null;
            string? chosenOptionSchool = null;

            if (Map.IsDebugMode || Args.Source is NonPlayableCharacter)
            {
                var randomChoice = optionDtos.Actions.TakeRandomElementWithWeights(i => 50, Rng);
                chosenOption = randomChoice.SelectionId;
                if (Map.IsDebugMode)
                    Map.AppendMessage($"PROMPT => {paramsObject.Title}\n\nOPTION: {randomChoice.SelectionId} => {randomChoice.Name}", Color.DarkRed);
            }
            else
            {
                var events = new List<DisplayEventDto>();
                var triggerPromptEvent = new DisplayEventDto()
                {
                    DisplayEventType = DisplayEventType.TriggerPrompt,
                    Params = [false]
                };
                events.Add(triggerPromptEvent);
                Map.DisplayEvents.Add(($"Open Select Interaction Prompt {paramsObject.Title}", events));

                while (!(bool)triggerPromptEvent.Params[0]) await Task.Delay(10);

                chosenOption = await Map.OpenSelectAction(paramsObject.Title, optionDtos, paramsObject.Cancellable);
            }

            if (chosenOption != null)
            {
                chosenOptionSchool = actionsNotFromConsumables.Find(a => a.SelectionId == chosenOption).School.Id;

                if (!Map.HasFlag(optionFlag))
                {
                    Map.CreateFlag(optionFlag, chosenOption, true);
                }
                else
                {
                    Map.SetFlagValue(optionFlag, chosenOption);
                }

                if (!Map.HasFlag(schoolFlag))
                {
                    Map.CreateFlag(schoolFlag, chosenOptionSchool, true);
                }
                else
                {
                    Map.SetFlagValue(schoolFlag, chosenOptionSchool);
                }

                Map.DisplayEvents = new();
                Map.Snapshot = new(Map.Dungeon, Map);

                return true;
            }

            return false;
        }

        public static async Task<bool> OpenSellPrompt(EffectCallerParams Args)
        {
            dynamic paramsObject = ExpressionParser.ParseParams(Args);

            if (Args.Source is not Character c)
                throw new ArgumentException($"Attempted to have {Args.Source.Name} sell an Item when it's not a Character.");
            if (paramsObject.Target is not Character t)
                throw new ArgumentException($"Attempted to have {paramsObject.Target.Name} buy Items when it's not a Character.");

            if (c.Inventory.Count == 0)
            {
                // Need items to sell something
                return false;
            }

            var optionDtos = new InventoryDto()
            {
                CurrencyConsoleRepresentation = Map.CurrencyClass.ConsoleRepresentation.Clone()
            };

            foreach (var item in c.Inventory)
            {
                var inventoryItem = new InventoryItemDto(item, c, Map, false);
                inventoryItem.CanBeUsed = inventoryItem.Value > 0;
                optionDtos.InventoryItems.Add(inventoryItem);
            }

            int? chosenOption = null;
            if (Map.IsDebugMode || Args.Source is NonPlayableCharacter)
            {
                var randomChoice = optionDtos.InventoryItems.Where(i => i.Value < c.CurrencyCarried).TakeRandomElementWithWeights(i => 50, Rng);
                if(randomChoice != null)
                    chosenOption = randomChoice.ItemId;
                if (chosenOption != null && Map.IsDebugMode)
                    Map.AppendMessage($"PROMPT => {paramsObject.Title}\n\nOPTION: {randomChoice.ItemId} => {randomChoice.ClassId}", Color.Yellow);
            }
            else
            {
                var events = new List<DisplayEventDto>();
                var triggerPromptEvent = new DisplayEventDto()
                {
                    DisplayEventType = DisplayEventType.TriggerPrompt,
                    Params = [false]
                };
                events.Add(triggerPromptEvent);
                Map.DisplayEvents.Add(($"Open Sell Items Prompt {paramsObject.Title}", events));

                while (!(bool)triggerPromptEvent.Params[0]) await Task.Delay(10);

                chosenOption = await Map.OpenSellPrompt(paramsObject.Title, optionDtos, paramsObject.Cancellable);
            }

            if (chosenOption != null)
            {
                var itemToSell = c.Inventory.Find(i => i.Id == chosenOption);
                var choiceData = optionDtos.InventoryItems.Find(i => i.ItemId == chosenOption);
                var currencyBeforeSale = c.CurrencyCarried;

                c.CurrencyCarried += choiceData.Value;
                c.Inventory.Remove(itemToSell);
                t.Inventory.Add(itemToSell);

                Map.DisplayEvents = new();

                var events = new List<DisplayEventDto>
                {
                    new DisplayEventDto()
                    {
                        DisplayEventType = DisplayEventType.PlaySpecialEffect,
                        Params = [SpecialEffect.Currency]
                    }
                };
                Map.AppendMessage(Map.Locale["CharacterSoldItem"].Format(new { CharacterName = c.Name, ItemName = itemToSell.Name, CurrencyDisplayName = Map.Locale["CurrencyDisplayName"].Format(new { Amount = (c.CurrencyCarried - currencyBeforeSale).ToString(), CurrencyName = Map.CurrencyClass.Name }) }), Color.LightGreen);

                Map.DisplayEvents.Add(($"{c.Name} sells {itemToSell.Name}", events));

                Map.Snapshot = new(Map.Dungeon, Map);

                return true;
            }

            return false;
        }

        public static async Task<bool> OpenBuyPrompt(EffectCallerParams Args)
        {
            dynamic paramsObject = ExpressionParser.ParseParams(Args);

            if (Args.Source is not Character c)
                throw new ArgumentException($"Attempted to have {Args.Source.Name} buy an Item when it's not a Character.");
            if (paramsObject.Target is not Character t)
                throw new ArgumentException($"Attempted to have {paramsObject.Target.Name} sell Items when it's not a Character.");

            if(c.Inventory.Count >= c.InventorySize)
            {
                // Cannot buy items if the inventory is full
                return false;
            }

            var optionDtos = new InventoryDto()
            {
                CurrencyConsoleRepresentation = Map.CurrencyClass.ConsoleRepresentation.Clone()
            };

            foreach (var item in t.Inventory)
            {
                var inventoryItem = new InventoryItemDto(item, c, Map, true);
                inventoryItem.CanBeUsed = inventoryItem.Value <= c.CurrencyCarried;
                optionDtos.InventoryItems.Add(inventoryItem);
            }

            int? chosenOption = null;
            if (Map.IsDebugMode || Args.Source is NonPlayableCharacter)
            {
                var randomChoice = optionDtos.InventoryItems.TakeRandomElementWithWeights(i => 50, Rng);
                if (randomChoice != null)
                    chosenOption = randomChoice.ItemId;
                if (chosenOption != null && Map.IsDebugMode)
                    Map.AppendMessage($"PROMPT => {paramsObject.Title}\n\nOPTION: {randomChoice.ItemId} => {randomChoice.ClassId}", Color.Yellow);
            }
            else
            {
                var events = new List<DisplayEventDto>();
                var triggerPromptEvent = new DisplayEventDto()
                {
                    DisplayEventType = DisplayEventType.TriggerPrompt,
                    Params = [false]
                };
                events.Add(triggerPromptEvent);
                Map.DisplayEvents.Add(($"Open Buy Items Prompt {paramsObject.Title}", events));

                while (!(bool)triggerPromptEvent.Params[0]) await Task.Delay(10);

                chosenOption = await Map.OpenBuyPrompt(paramsObject.Title, optionDtos, paramsObject.Cancellable);
            }

            if (chosenOption != null)
            {
                var itemToBuy = t.Inventory.Find(i => i.Id == chosenOption);
                var choiceData = optionDtos.InventoryItems.Find(i => i.ItemId == chosenOption);
                var currencyBeforePurchase = c.CurrencyCarried;

                c.CurrencyCarried -= choiceData.Value;
                t.Inventory.Remove(itemToBuy);
                c.Inventory.Add(itemToBuy);

                Map.DisplayEvents = new();

                var events = new List<DisplayEventDto>
                {
                    new DisplayEventDto()
                    {
                        DisplayEventType = DisplayEventType.PlaySpecialEffect,
                        Params = [SpecialEffect.Currency]
                    }
                };
                Map.AppendMessage(Map.Locale["CharacterBoughtItem"].Format(new { CharacterName = c.Name, ItemName = itemToBuy.Name, CurrencyDisplayName = Map.Locale["CurrencyDisplayName"].Format(new { Amount = (currencyBeforePurchase - c.CurrencyCarried).ToString(), CurrencyName = Map.CurrencyClass.Name }) }), Color.LightGreen);

                Map.DisplayEvents.Add(($"{c.Name} buys {itemToBuy.Name}", events));

                Map.Snapshot = new(Map.Dungeon, Map);

                return true;
            }

            return false;
        }
    }

#pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
#pragma warning restore CS8604 // Posible argumento de referencia nulo
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
