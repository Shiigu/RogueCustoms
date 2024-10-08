﻿using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Helpers;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.Expressions;

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

        public static bool ReplaceConsoleRepresentation(Entity This, Entity Source, ITargetable Target, params (string ParamName, string Value)[] args)
        {
            dynamic paramsObject = ExpressionParser.ParseParams(This, Source, Target, args);
            if (ExpandoObjectHelper.HasProperty(paramsObject, "Character"))
                Source.ConsoleRepresentation.Character = paramsObject.Character;
            if (ExpandoObjectHelper.HasProperty(paramsObject, "ForeColor"))
                Source.ConsoleRepresentation.ForegroundColor = paramsObject.ForeColor;
            if (ExpandoObjectHelper.HasProperty(paramsObject, "BackColor"))
                Source.ConsoleRepresentation.BackgroundColor = paramsObject.BackColor;
            return true;
        }

        public static bool ResetConsoleRepresentation(Entity This, Entity Source, ITargetable Target, params (string ParamName, string Value)[] args)
        {
            dynamic paramsObject = ExpressionParser.ParseParams(This, Source, Target, args);
            var baseConsoleRepresentation = Source.BaseConsoleRepresentation.Clone();
            Source.ConsoleRepresentation.Character = baseConsoleRepresentation.Character;
            Source.ConsoleRepresentation.ForegroundColor = baseConsoleRepresentation.ForegroundColor;
            Source.ConsoleRepresentation.BackgroundColor = baseConsoleRepresentation.BackgroundColor;
            return true;
        }

        public static bool StealItem(Entity This, Entity Source, ITargetable Target, params (string ParamName, string Value)[] args)
        {
            dynamic paramsObject = ExpressionParser.ParseParams(This, Source, Target, args);
            if (Source is not Character s) throw new ArgumentException($"Attempted to have {Source.Name} steal an item when it's not a Character.");
            if (paramsObject.Target is not Character t)
                // Attempted to steal an item from Target when it's not a Character.
                return false;

            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Source, paramsObject.Target, paramsObject);

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
                    if (s.EntityType == EntityType.Player
                        || (s.EntityType == EntityType.NPC && Map.Player.CanSee(s))
                        || t.EntityType == EntityType.Player
                        || (t.EntityType == EntityType.NPC && Map.Player.CanSee(t)))
                    {
                        Map.AppendMessage(Map.Locale["CharacterStealsItem"].Format(new { SourceName = s.Name, TargetName = t.Name, ItemName = itemToSteal.Name }), Color.DeepSkyBlue);
                    }
                    return true;
                }
            }
            return false;
        }
    }
    #pragma warning restore S2589 // Boolean expressions should not be gratuitous
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}