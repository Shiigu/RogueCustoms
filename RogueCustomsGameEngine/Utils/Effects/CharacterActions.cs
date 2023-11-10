using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Helpers;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

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

        public static bool ReplaceConsoleRepresentation(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (ExpandoObjectHelper.HasProperty(paramsObject, "Character"))
                Source.ConsoleRepresentation.Character = paramsObject.Character;
            if (ExpandoObjectHelper.HasProperty(paramsObject, "ForeColor"))
                Source.ConsoleRepresentation.ForegroundColor = paramsObject.ForeColor;
            if (ExpandoObjectHelper.HasProperty(paramsObject, "BackColor"))
                Source.ConsoleRepresentation.BackgroundColor = paramsObject.BackColor;
            _ = 0;
            return true;
        }

        public static bool StealItem(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            _ = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (Source is not Character s) throw new ArgumentException($"Attempted to have {Source.Name} steal an item when it's not a Character.");
            if (paramsObject.Target is not Character t) throw new ArgumentException($"Attempted to steal an item from {paramsObject.Target?.Name} when it's not a Character.");

            var accuracyCheck = ActionHelpers.CalculateAdjustedAccuracy(Source, paramsObject.Target, paramsObject);

            if (s.Inventory.Count < s.InventorySize && t.Inventory.Count > 0 && Rng.NextInclusive(1, 100) <= accuracyCheck)
            {
                var stealableItems = new List<Item>();
                if (paramsObject.CanStealEquippables)
                    stealableItems.AddRange(t.Inventory.Where(i => i.EntityType == EntityType.Weapon || i.EntityType == EntityType.Armor));
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