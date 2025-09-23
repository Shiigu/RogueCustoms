using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Helpers;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.Expressions;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsGameEngine.Utils.Representation;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.Effects.Utils;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Utils.Effects
{
#pragma warning disable S2589 // Boolean expressions should not be gratuitous
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    // Represents Actions that are only expected to be used by Characters.
    public static class CharacterActions
    {
        private static RngHandler Rng;
        private static Map Map;

        public static void SetActionParams(RngHandler rng, Map map)
        {
            Rng = rng;
            Map = map;
        }

        public static bool ReplaceConsoleRepresentation(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);
            if (ExpandoObjectHelper.HasProperty(paramsObject, "Character") && paramsObject.Character != '\0')
                Args.Source.ConsoleRepresentation.Character = paramsObject.Character;
            if (ExpandoObjectHelper.HasProperty(paramsObject, "ForeColor") && paramsObject.ForeColor != null)
                Args.Source.ConsoleRepresentation.ForegroundColor = paramsObject.ForeColor;
            if (ExpandoObjectHelper.HasProperty(paramsObject, "BackColor") && paramsObject.BackColor != null)
                Args.Source.ConsoleRepresentation.BackgroundColor = paramsObject.BackColor;
            if (!Map.IsDebugMode)
            {
                events.Add(new()
                {
                    DisplayEventType = DisplayEventType.UpdateTileRepresentation,
                    Params = new() { Args.Source.Position, Map.GetConsoleRepresentationForCoordinates(Args.Source.Position.X, Args.Source.Position.Y) }
                }
                );
            }
            if (Args.Source == Map.Player)
            {
                events.Add(new()
                {
                    DisplayEventType = DisplayEventType.UpdatePlayerData,
                    Params = new() { UpdatePlayerDataType.UpdateConsoleRepresentation, Args.Source.ConsoleRepresentation }
                });
            }
            Map.DisplayEvents.Add(("ChangeConsoleRepresentation", events));
            return true;
        }

        public static bool ResetConsoleRepresentation(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);
            var baseConsoleRepresentation = Args.Source.BaseConsoleRepresentation.Clone();
            Args.Source.ConsoleRepresentation.Character = baseConsoleRepresentation.Character;
            Args.Source.ConsoleRepresentation.ForegroundColor = baseConsoleRepresentation.ForegroundColor;
            Args.Source.ConsoleRepresentation.BackgroundColor = baseConsoleRepresentation.BackgroundColor;
            if (!Map.IsDebugMode)
            {
                events.Add(new()
                {
                    DisplayEventType = DisplayEventType.UpdateTileRepresentation,
                    Params = new() { Args.Source.Position, Map.GetConsoleRepresentationForCoordinates(Args.Source.Position.X, Args.Source.Position.Y) }
                }
                );
            }
            if (Args.Source == Map.Player)
            {
                events.Add(new()
                {
                    DisplayEventType = DisplayEventType.UpdatePlayerData,
                    Params = new() { UpdatePlayerDataType.UpdateConsoleRepresentation, Args.Source.ConsoleRepresentation }
                });
            }
            Map.DisplayEvents.Add(("ResetConsoleRepresentation", events));
            return true;
        }

        public static async Task<bool> GiveItem(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);

            if (paramsObject.Target is not Character t)
                // Attempted to give Target an Item when it's not a Character.
                return false;

            t = paramsObject.Target as Character;

            if (t.ItemCount == t.InventorySize)
                // Attempted to give Target an Item when their inventory is full.
                return false;

            if (paramsObject.FromInventory && paramsObject.Source is not Character s)
                // Attempted to give Target an Item from Source's inventory, when Source's not a Character.
                return false;

            s = paramsObject.Source as Character;

            string itemId = paramsObject.Id;
            string idToLookUp = itemId;

            if (itemId == "<<CUSTOM>>")
                idToLookUp = paramsObject.CustomId;

            var itemClass = Map.PossibleItemClasses.Find(c => c.Id.Equals(idToLookUp));

            if (itemClass == null)
                // Must have a valid Item class to spawn
                return false;

            if (paramsObject.FromInventory && !s.Inventory.Exists(i => i.ClassId.Equals(idToLookUp)))
                // Attempted to give Target an Item from Source's inventory, when Source does not have such an item.
                return false;

            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Args.Source, t, paramsObject);
            if (Rng.RollProbability() <= accuracyCheck)
            {
                var itemToGive = paramsObject.FromInventory
                    ? s.Inventory.Find(i => i.ClassId.Equals(idToLookUp))
                    : await Map.AddEntity(idToLookUp, 1, new GamePoint(0, 0)) as Item;
                if (paramsObject.FromInventory)
                    s.Inventory.Remove(itemToGive);
                t.Inventory.Add(itemToGive);
                itemToGive.Position = null;
                if (t == Map.Player || Map.Player.CanSee(t))
                {
                    if (t == Map.Player)
                    {
                        itemToGive.GotSpecificallyIdentified = true;
                        Map.Player.InformRefreshedPlayerData(events);
                    }
                    if (t == Map.Player || paramsObject.InformThePlayer)
                    {
                        events.Add(new()
                        {
                            DisplayEventType = DisplayEventType.PlaySpecialEffect,
                            Params = new() { SpecialEffect.ItemGet }
                        });
                        var message = paramsObject.InformOfSource
                            ? Map.Locale["CharacterGotAnItem"].Format(new { CharacterName = t.Name, SourceName = s.Name, ItemName = itemToGive.Name })
                            : Map.Locale["CharacterObtainedAnItem"].Format(new { CharacterName = t.Name, ItemName = itemToGive.Name });
                        Map.AppendMessage(message, Color.DeepSkyBlue, events);
                    }
                }
                Map.DisplayEvents.Add(($"{t.Name} received an item", events));
                return true;
            }
            return false;
        }

        public static bool StealItem(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);
            if (Args.Source is not Character s) throw new ArgumentException($"Attempted to have {Args.Source.Name} steal an item when it's not a Character.");
            if (paramsObject.Target is not Character t)
                // Attempted to steal an item from Target when it's not a Character.
                return false;

            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Args.Source, paramsObject.Target, paramsObject);

            if (s.ItemCount < s.InventorySize && t.ItemCount > 0 && Rng.RollProbability() <= accuracyCheck)
            {
                var stealableItems = new List<Item>();
                if (paramsObject.CanStealEquippables)
                    stealableItems.AddRange(t.Inventory.Where(i => i.IsEquippable));
                if (paramsObject.CanStealConsumables)
                    stealableItems.AddRange(t.Inventory.Where(i => i.IsConsumable));
                if(stealableItems.Any())
                {
                    var itemToSteal = stealableItems.TakeRandomElement(Rng);
                    if (itemToSteal == null) return false;
                    t.Inventory.Remove(itemToSteal);
                    s.Inventory.Add(itemToSteal);
                    if ((s == Map.Player || Map.Player.CanSee(s))
                        || (t == Map.Player || Map.Player.CanSee(t)))
                    {
                        if (s == Map.Player || t == Map.Player)
                        {
                            Map.Player.InformRefreshedPlayerData(events);
                            events.Add(new()
                            {
                                DisplayEventType = DisplayEventType.PlaySpecialEffect,
                                Params = new() { SpecialEffect.ItemGet }
                            });
                        }
                        events.Add(new()
                        {
                            DisplayEventType = DisplayEventType.PlaySpecialEffect,
                            Params = new() { s == Map.Player ? SpecialEffect.ItemGet : SpecialEffect.NPCItemGet }
                        });
                        Map.AppendMessage(Map.Locale["CharacterStealsItem"].Format(new { SourceName = s.Name, TargetName = t.Name, ItemName = itemToSteal.Name }), Color.DeepSkyBlue, events);
                    }
                    Map.DisplayEvents.Add(($"{t.Name} got an item stolen", events));
                    return true;
                }
            }
            return false;
        }

        public static bool LearnScript(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);
            if (paramsObject.Target is not Character c)
                throw new ArgumentException($"Attempted to have {paramsObject.Target.Name} learn a Script when it's not a Character.");

            var script = Map.Scripts.Find(s => s.Id.Equals(paramsObject.ScriptId, StringComparison.InvariantCultureIgnoreCase))
                ?? throw new ArgumentException($"Attempted to learn {paramsObject.ScriptId} when it's not a Script.");

            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Args.Source, paramsObject.Target, paramsObject);

            if (!c.OwnOnAttack.Any(oaa => oaa.IsScript && oaa.Id.Equals(script.Id)) && Rng.RollProbability() <= accuracyCheck)
            {
                var clonedScript = script.Clone();
                clonedScript.User = c;
                clonedScript.Map = Map;
                c.OwnOnAttack.Add(clonedScript);
                c.SetActionIds();
                if ((c == Map.Player || Map.Player.CanSee(c)) && paramsObject.InformThePlayer)
                {
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.PlaySpecialEffect,
                        Params = new() { SpecialEffect.StatBuff }
                    });
                    Map.AppendMessage(Map.Locale["CharacterLearnedScript"].Format(new { CharacterName = c.Name, ScriptName = script.Name }), Color.DeepSkyBlue, events);
                }
                Map.DisplayEvents.Add(($"{c.Name} learned a Script", events));
                return true;
            }
            return false;
        }

        public static bool ForgetScript(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);
            if (paramsObject.Target is not Character c)
                throw new ArgumentException($"Attempted to have {paramsObject.Target.Name} forget a Script when it's not a Character.");

            var script = Map.Scripts.Find(s => s.Id.Equals(paramsObject.ScriptId, StringComparison.InvariantCultureIgnoreCase))
                ?? throw new ArgumentException($"Attempted to forget {paramsObject.ScriptId} when it's not a Script.");

            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Args.Source, paramsObject.Target, paramsObject);

            if (c.OwnOnAttack.Any(oaa => oaa.IsScript && oaa.Id.Equals(script.Id)) && Rng.RollProbability() <= accuracyCheck)
            {
                c.OwnOnAttack.RemoveAll(oaa => oaa.IsScript && oaa.Id.Equals(script.Id));
                c.SetActionIds();
                if ((c == Map.Player || Map.Player.CanSee(c)) && paramsObject.InformThePlayer)
                {
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.PlaySpecialEffect,
                        Params = new() { SpecialEffect.StatNerf }
                    });
                    Map.AppendMessage(Map.Locale["CharacterForgotScript"].Format(new { CharacterName = c.Name, ScriptName = script.Name }), Color.DeepSkyBlue, events);
                }
                Map.DisplayEvents.Add(($"{c.Name} forgot a Script", events));
                return true;
            }
            return false;
        }

        public static bool ChangeExperiencePayoutFormula(EffectCallerParams Args)
        {
            dynamic paramsObject = ExpressionParser.ParseParams(Args);
            if (Args.Source is not Character c)
                throw new ArgumentException($"Attempted to change {Args.Source.Name}'s Experience Payout Formula when it's not a Character.");

            c.ExperiencePayoutFormula = paramsObject.Formula;

            return true;
        }

        public static bool Reveal(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);
            if (paramsObject.Target is not Character c)
                throw new ArgumentException($"Attempted to have {paramsObject.Target.Name} get a revelation when it's not a Character.");

            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Args.Source, paramsObject.Target, paramsObject);

            if (Rng.RollProbability() <= accuracyCheck)
            {
                var message = string.Empty;
                string whatToReveal = paramsObject.WhatToReveal;
                var wasAnythingLeftUndiscovered = false;

                switch(whatToReveal.ToLowerInvariant())
                {
                    case "floor":
                        wasAnythingLeftUndiscovered = Map.Tiles.Any(t => !t.Discovered);
                        if (c == Map.Player)
                            Map.Tiles.ForEach(t => t.Discovered = true);
                        message = Map.Locale["CharacterRevealsFloor"].Format(new { CharacterName = c.Name });
                        break;
                    case "traps":
                        wasAnythingLeftUndiscovered = Map.Traps.Any(t => !t.Discovered);
                        if (c == Map.Player)
                        {
                            Map.Traps.ForEach(t => t.Discovered = true);
                        }
                        else if (c is NonPlayableCharacter npc)
                        {
                            npc.CanSeeTraps = true;
                        }
                        message = Map.Locale["CharacterRevealsTraps"].Format(new { CharacterName = c.Name });
                        break;
                    default:
                        throw new ArgumentException($"Attempted to have {c.Name} get an invalid revelation of {whatToReveal}.");
                }

                if(c == Map.Player)
                {
                    if (wasAnythingLeftUndiscovered)
                    {
                        events.Add(new()
                        {
                            DisplayEventType = DisplayEventType.PlaySpecialEffect,
                            Params = [SpecialEffect.Reveal]
                        });
                        events.Add(new()
                        {
                            DisplayEventType = DisplayEventType.RedrawMap,
                            Params = new() { Map.Snapshot.GetTiles() }
                        });
                    }
                    else
                    {
                        return false;
                    }
                }

                if ((c == Map.Player || Map.Player.CanSee(c)) && paramsObject.InformThePlayer)
                {
                    Map.AppendMessage(message, Color.DeepSkyBlue, events);
                }
                Map.DisplayEvents.Add(($"{c.Name} got a revelation", events));
                return true;
            }
            return false;
        }

        public static async Task<bool> GenerateInventoryFromLootTable(EffectCallerParams Args)
        {
            dynamic paramsObject = ExpressionParser.ParseParams(Args);
            if (Args.Source is not Character c)
                throw new ArgumentException($"Attempted to fill {Args.Source.Name}'s Inventory when it's not a Character.");

            string id = paramsObject.Id;
            int maximumPicks = (int) paramsObject.Amount;

            if(maximumPicks > (c.InventorySize - c.Inventory.Count))
                maximumPicks = c.InventorySize - c.Inventory.Count;

            LootTable lootTable = Map.LootTables.Find(lt => lt.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));

            if (lootTable == null || lootTable.Entries.Count == 0)
                throw new ArgumentException($"Attempted to fill {Args.Source.Name}'s Inventory with an invalid Loot Table of id {id}.");

            var validItemClasses = Map.PossibleItemClasses.Except(Map.UndroppableItemClasses).ToList();
            var itemPicks = new List<EntityClass>();
            object pickedObject = null;

            for (int i = 0; i < maximumPicks; i++)
            {
                var currentLootTable = lootTable;
                var foundAPick = false;
                do
                {
                    pickedObject = currentLootTable.Entries.TakeRandomElementWithWeights(e => e.Weight, Rng).Pick;
                    if (pickedObject is LootTable lt)
                    {
                        currentLootTable = lt;
                    }
                    else if (pickedObject is EntityClass ec)
                    {
                        itemPicks.Add(ec);
                        foundAPick = true;
                    }
                    else if (pickedObject is ItemType it)
                    {
                        var appropriateItemClasses = validItemClasses.Where(ic => ic.ItemType == it).ToList();
                        if (appropriateItemClasses.Count == 0) continue;
                        var chosenItemOfType = appropriateItemClasses.TakeRandomElement(Rng);
                        itemPicks.Add(chosenItemOfType);
                        foundAPick = true;
                    }
                    else if (pickedObject is string s && EngineConstants.SPECIAL_LOOT_ENTRIES.Contains(s))
                    {
                        if (s == EngineConstants.LOOT_NO_DROP || pickedObject is CurrencyPile)
                        {
                            // Do nothing
                            foundAPick = true;
                        }
                        else if (s == EngineConstants.LOOT_EQUIPPABLE)
                        {
                            var appropriateItemClasses = validItemClasses.Where(ic => ic.ItemType.Usability == ItemUsability.Equip).ToList();
                            if (appropriateItemClasses.Count == 0) continue;
                            var chosenEquippable = appropriateItemClasses.TakeRandomElement(Rng);
                            itemPicks.Add(chosenEquippable);
                            foundAPick = true;
                        }
                    }
                }
                while (!foundAPick);
            }

            foreach (var pick in itemPicks)
            {
                var newItem = await Map.AddEntity(pick, c.Level, null, false) as Item;
                newItem.GotSpecificallyIdentified = true;
                c.Inventory.Add(newItem);
                if (lootTable.OverridesQualityLevelOddsOfItems)
                {
                    newItem.SetQualityLevel(lootTable.QualityLevelOdds.TakeRandomElementWithWeights(qlo => qlo.ChanceToPick, Rng).QualityLevel);
                }
                if (Map.IsDebugMode)
                    Map.AppendMessage($"DEBUG: Item {newItem.Name} has been added to {c.Name}'s inventory", Color.LightGreen);
            }

            return true;
        }

        public static bool FollowWaypoint(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);
            if (Args.Source is not NonPlayableCharacter npc)
                // Do nothing if it's not an NPC
                return true;

            var possibleWaypoints = Map.Tiles.Where(t => t.WaypointId?.Equals(paramsObject.Id) == true);

            if(possibleWaypoints.Any(t => !t.IsOccupied))
                // Prioritize unoccupied Waypoint tiles
                possibleWaypoints = possibleWaypoints.Where(t => !t.IsOccupied).ToList();

            if(possibleWaypoints.Count > 0)
            {
                npc.SetWaypoint(possibleWaypoints.TakeRandomElement(Rng));
            }

            return true;
        }

        public static bool StopFollowingWaypoint(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);
            if (Args.Source is not NonPlayableCharacter npc)
                // Do nothing if it's not an NPC
                return true;

            npc.UnsetWaypoint();

            return true;
        }

        public static async Task<bool> GiveCurrency(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);

            if (paramsObject.Target is not Character t)
                // Attempted to give Target Currency when it's not a Character.
                return false;

            t = paramsObject.Target as Character;

            if (paramsObject.Amount <= 0)
                // Attempted to give Target no Currency whatsoever.
                return false;

            if (paramsObject.FromPockets && paramsObject.Source is not Character s)
                // Attempted to give Target Currency from Source's pockets, when Source's not a Character.
                return false;

            s = paramsObject.Source as Character;

            var amount = (int)paramsObject.Amount;
            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Args.Source, t, paramsObject);
            if (Rng.RollProbability() <= accuracyCheck)
            {
                if (paramsObject.FromPockets)
                {
                    amount = Math.Min(amount, s.CurrencyCarried);
                    if (amount <= 0)
                        return false;
                    s.CurrencyCarried -= amount;
                }
                if (s.CurrencyCarried < 0)
                    s.CurrencyCarried = 0;
                t.CurrencyCarried += amount;
                if (t == Map.Player || Map.Player.CanSee(t))
                {
                    if (t == Map.Player)
                    {
                        Map.Player.InformRefreshedPlayerData(events);
                    }
                    if (t == Map.Player || paramsObject.InformThePlayer)
                    {
                        events.Add(new()
                        {
                            DisplayEventType = DisplayEventType.PlaySpecialEffect,
                            Params = new() { SpecialEffect.Currency }
                        });
                        var currencyDisplayText = Map.Locale["CurrencyDisplayName"].Format(new { Amount = amount, CurrencyName = Map.CurrencyClass.Name });
                        var message = paramsObject.InformOfSource
                            ? Map.Locale["CharacterGotAnItem"].Format(new { CharacterName = t.Name, SourceName = s.Name, ItemName = currencyDisplayText })
                            : Map.Locale["CharacterObtainedAnItem"].Format(new { CharacterName = t.Name, ItemName = currencyDisplayText });
                        Map.AppendMessage(message, Color.DeepSkyBlue, events);
                    }
                }
                Map.DisplayEvents.Add(($"{t.Name} received currency", events));
                return true;
            }
            return false;
        }

        public static bool StealCurrency(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);
            if (Args.Source is not Character s) throw new ArgumentException($"Attempted to have {Args.Source.Name} steal Currency when it's not a Character.");
            if (paramsObject.Target is not Character t)
                // Attempted to steal Currency from Target when it's not a Character.
                return false;

            var amount = (int)paramsObject.Amount;
            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Args.Source, paramsObject.Target, paramsObject);

            if (t.CurrencyCarried > 0 && Rng.RollProbability() <= accuracyCheck)
            {
                amount = Math.Min(amount, t.CurrencyCarried);
                if (amount <= 0)
                    return false;
                t.CurrencyCarried -= amount;
                if (t.CurrencyCarried < 0)
                    t.CurrencyCarried = 0;
                s.CurrencyCarried += amount;
                if ((s == Map.Player || Map.Player.CanSee(s))
                    || (t == Map.Player || Map.Player.CanSee(t)))
                {
                    if (s == Map.Player || t == Map.Player)
                    {
                        Map.Player.InformRefreshedPlayerData(events);
                        events.Add(new()
                        {
                            DisplayEventType = DisplayEventType.PlaySpecialEffect,
                            Params = new() { SpecialEffect.ItemGet }
                        });
                    }
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.PlaySpecialEffect,
                        Params = new() { s == Map.Player ? SpecialEffect.ItemGet : SpecialEffect.NPCItemGet }
                    });
                    var currencyDisplayText = Map.Locale["CurrencyDisplayName"].Format(new { Amount = amount, CurrencyName = Map.CurrencyClass.Name });
                    Map.AppendMessage(Map.Locale["CharacterStealsItem"].Format(new { SourceName = s.Name, TargetName = t.Name, ItemName = currencyDisplayText }), Color.DeepSkyBlue, events);
                }
                Map.DisplayEvents.Add(($"{t.Name} got Currency stolen", events));
                return true;
            }
            return false;
        }

        public static async Task<bool> ClearInventory(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);

            if (paramsObject.Target is not Character t)
                // Attempted to clear Target's Inventory when it's not a Character.
                return false;

            t = paramsObject.Target as Character;

            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Args.Source, t, paramsObject);
            if (Rng.RollProbability() <= accuracyCheck)
            {
                foreach (var item in t.Inventory)
                {
                    item.Position = null;
                    item.ExistenceStatus = EntityExistenceStatus.Gone;
                }
                t.Inventory.Clear();
                if (t == Map.Player || Map.Player.CanSee(t))
                {
                    if (t == Map.Player)
                    {
                        Map.Player.InformRefreshedPlayerData(events);
                    }
                    if (t == Map.Player || paramsObject.DisplayOnLog)
                    {
                        events.Add(new()
                        {
                            DisplayEventType = DisplayEventType.PlaySpecialEffect,
                            Params = new() { SpecialEffect.ItemDrop }
                        });
                        var message = Map.Locale["CharacterLostAllItems"].Format(new { CharacterName = t.Name });
                        Map.AppendMessage(message, Color.DeepSkyBlue, events);
                    }
                }
                Map.DisplayEvents.Add(($"{t.Name} lost all items", events));
                return true;
            }
            return false;
        }
    }
    #pragma warning restore S2589 // Boolean expressions should not be gratuitous
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}