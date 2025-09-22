using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.DungeonStructure;
using System;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using System.Text;
using RogueCustomsGameEngine.Utils.Expressions;
using RogueCustomsGameEngine.Utils.Effects.Utils;
using System.Xml.Linq;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using System.Collections.Generic;
using System.Linq;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.Representation;
using System.Drawing;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Utils.Effects
{
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
    public static class ItemActions
    {
        private static RngHandler Rng;
        private static Map Map;
        public static void SetActionParams(RngHandler rng, Map map)
        {
            Rng = rng;
            Map = map;
        }

        public static async Task<bool> Identify(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);

            if (Args.Source is not Character s)
                // Attempted to Identify one of Source's Items when it's not a Character.
                return false;


            int itemId = paramsObject.ItemId is string sid ? int.Parse(sid) : (int) paramsObject.ItemId;

            var item = s.Equipment.Find(i => i.Id == itemId) ?? s.Inventory.Find(i => i.Id == itemId);

            if(item == null)
                // Attempted to Identify an Item Source does not have.
                return false;

            if (item.IsIdentified)
                // Attempted to Identify an Item that is already Identified.
                return false;

            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Args.Source, s, paramsObject);
            if (Rng.RollProbability() <= accuracyCheck)
            {
                var previousName = item.Name;
                item.GotSpecificallyIdentified = true;
                item.UpdateNameIfNeeded();
                if (s == Map.Player)
                {
                    if (!Map.Player.IdentifiedItemClasses.Contains(item.ClassId))
                        Map.Player.IdentifiedItemClasses.Add(item.ClassId);
                }
                if (s == Map.Player || Map.Player.CanSee(s))
                {
                    if (s == Map.Player)
                    {
                        Map.DisplayEvents.Add(($"{item.Name} was identified", [new()
                        {
                            DisplayEventType = DisplayEventType.PlaySpecialEffect,
                            Params = new() { SpecialEffect.Identify }
                        }]));
                    }
                    Map.AppendMessage(Map.Locale["ItemWasIdentified"].Format(new { FakeName = previousName, TrueName = item.Name }), Color.Yellow);
                }
                return true;
            }
            return false;
        }

        public static async Task<bool> IdentifyAllItems(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);

            if (paramsObject.Target is not Character t)
                // Attempted to Identify all of Target's Items when it's not a Character.
                return false;

            if (t.Equipment.Count == 0 && t.Inventory.Count == 0)
                // Attempted to Identify all of Target's Items when they don't have any.
                return false;

            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Args.Source, t, paramsObject);

            if(t.Equipment.All(i => i.IsIdentified) && t.Inventory.All(i => i.IsIdentified))
                // Attempted to Identify all of Target's Items when they're already Identified.
                return false;

            if (Rng.RollProbability() <= accuracyCheck)
            {
                foreach (var item in t.Equipment)
                {
                    var previousName = item.Name;
                    item.GotSpecificallyIdentified = true;
                    item.UpdateNameIfNeeded();
                    if (t == Map.Player)
                    {
                        if (!Map.Player.IdentifiedItemClasses.Contains(item.ClassId))
                            Map.Player.IdentifiedItemClasses.Add(item.ClassId);
                    }
                }
                foreach (var item in t.Inventory)
                {
                    var previousName = item.Name;
                    item.GotSpecificallyIdentified = true;
                    item.UpdateNameIfNeeded();
                    if (t == Map.Player)
                    {
                        if (!Map.Player.IdentifiedItemClasses.Contains(item.ClassId))
                            Map.Player.IdentifiedItemClasses.Add(item.ClassId);
                    }
                }
                if (t == Map.Player || Map.Player.CanSee(t))
                    Map.AppendMessage(Map.Locale["CharacterItemsWereIdentified"].Format(new { CharacterName = t.Name }));
                return true;
            }
            return false;
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    #pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}
