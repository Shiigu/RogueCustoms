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

        public static bool Remove(EffectCallerParams Args)
        {
            dynamic paramsObject = ExpressionParser.ParseParams(Args);

            var targetItem = paramsObject.Target as Item;
            var targetKey = paramsObject.Target as Key;
            var targetTrap = paramsObject.Target as Trap;

            if (targetItem == null && targetKey == null && targetTrap == null)
                throw new InvalidOperationException($"Attempted to remove {paramsObject.Target.Name}, which isn't Removable.");
            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Args.Source, paramsObject.Target, paramsObject);

            if (Rng.RollProbability() <= accuracyCheck)
            {
                if(targetItem != null)
                {
                    if (targetItem.Owner is Character c)
                    {
                        var events = new List<DisplayEventDto>();
                        targetItem.Owner?.Inventory?.Remove(targetItem);
                        targetItem.Owner = null;
                        events.Add(new()
                        {
                            DisplayEventType = DisplayEventType.UpdatePlayerData,
                            Params = new() { UpdatePlayerDataType.UpdateInventory, c.Inventory.Cast<Entity>().Union(c.KeySet.Cast<Entity>()).Select(i => new SimpleEntityDto(i)).ToList() }
                        });
                        Map.DisplayEvents.Add(($"{targetItem.Name} disappears from {c.Name}'s inventory", events));
                    }
                    targetItem.ExistenceStatus = EntityExistenceStatus.Gone;
                    targetItem.Position = null;
                    Map.Items.Remove(targetItem);
                }
                else if (targetKey != null)
                {
                    targetKey.Owner?.KeySet?.Remove(targetKey);
                    targetKey.Owner = null;
                    targetKey.ExistenceStatus = EntityExistenceStatus.Gone;
                    targetKey.Position = null;
                    Map.Keys.Remove(targetKey);
                }
                else if (targetTrap != null)
                {
                    targetTrap.ExistenceStatus = EntityExistenceStatus.Gone;
                    targetTrap.Position = null;
                    Map.Traps.Remove(targetTrap);
                }
                else
                    return false;

                return true;
            }
            return false;
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    #pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}
