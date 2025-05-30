﻿using RogueCustomsGameEngine.Game.Entities;
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
            if (ExpandoObjectHelper.HasProperty(paramsObject, "Character"))
                Args.Source.ConsoleRepresentation.Character = paramsObject.Character;
            if (ExpandoObjectHelper.HasProperty(paramsObject, "ForeColor"))
                Args.Source.ConsoleRepresentation.ForegroundColor = paramsObject.ForeColor;
            if (ExpandoObjectHelper.HasProperty(paramsObject, "BackColor"))
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
                    stealableItems.AddRange(t.Inventory.Where(i => i.EntityType == EntityType.Consumable));
                if(stealableItems.Any())
                {
                    var itemToSteal = stealableItems.TakeRandomElement(Rng);
                    if (itemToSteal == null) return false;
                    t.Inventory.Remove(itemToSteal);
                    s.Inventory.Add(itemToSteal);
                    itemToSteal.Owner = s;
                    if ((s == Map.Player || Map.Player.CanSee(s))
                        || (t == Map.Player || Map.Player.CanSee(t)))
                    {
                        if (s == Map.Player || Map.Player.CanSee(s))
                        {
                            if (s == Map.Player)
                            {
                                events.Add(new()
                                {
                                    DisplayEventType = DisplayEventType.UpdatePlayerData,
                                    Params = new() { UpdatePlayerDataType.UpdateInventory, s.Inventory.Cast<Entity>().Union(s.KeySet.Cast<Entity>()).Select(i => new SimpleEntityDto(i)).ToList() }
                                });
                            }
                            events.Add(new()
                            {
                                DisplayEventType = DisplayEventType.PlaySpecialEffect,
                                Params = new() { SpecialEffect.ItemGet }
                            });
                        }
                        else if (t == Map.Player || Map.Player.CanSee(t))
                        {
                            if (t == Map.Player)
                            {
                                events.Add(new()
                                {
                                    DisplayEventType = DisplayEventType.UpdatePlayerData,
                                    Params = new() { UpdatePlayerDataType.UpdateInventory, s.Inventory.Cast<Entity>().Union(s.KeySet.Cast<Entity>()).Select(i => new SimpleEntityDto(i)).ToList() }
                                });
                            }
                            events.Add(new()
                            {
                                DisplayEventType = DisplayEventType.PlaySpecialEffect,
                                Params = new() { SpecialEffect.NPCItemGet }
                            });
                        }
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
                throw new ArgumentException($"Attempted to have {Args.Source.Name} learn a Script when it's not a Character.");

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
                throw new ArgumentException($"Attempted to have {Args.Source.Name} forget a Script when it's not a Character.");

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
    }
    #pragma warning restore S2589 // Boolean expressions should not be gratuitous
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}