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

            if(itemId == "<<CUSTOM>>")
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
                itemToGive.Owner = t;
                itemToGive.Position = null;
                if (t == Map.Player || Map.Player.CanSee(t))
                {
                    if (t == Map.Player)
                    {
                        events.Add(new()
                        {
                            DisplayEventType = DisplayEventType.UpdatePlayerData,
                            Params = new() { UpdatePlayerDataType.UpdateInventory, t.Inventory.Cast<Entity>().Union(t.KeySet.Cast<Entity>()).Select(i => new SimpleEntityDto(i)).ToList() }
                        });
                    }
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
                Map.DisplayEvents.Add(($"{t.Name} received an item", events));
                return true;
            }
            return false;
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    #pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}
