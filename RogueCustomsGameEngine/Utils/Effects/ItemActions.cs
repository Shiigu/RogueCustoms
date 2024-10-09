using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.DungeonStructure;
using System;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using System.Text;
using RogueCustomsGameEngine.Utils.Expressions;

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

        public static bool Remove(Entity This, Entity Source, ITargetable Target, params (string ParamName, string Value)[] args)
        {
            dynamic paramsObject = ExpressionParser.ParseParams(This, Source, Target, args);

            var targetItem = paramsObject.Target as Item;
            var targetKey = paramsObject.Target as Key;
            var targetTrap = paramsObject.Target as Trap;

            if (targetItem == null && targetKey == null && targetTrap == null)
                throw new InvalidOperationException($"Attempted to remove {paramsObject.Target.Name}, which isn't Removable.");
            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Source, paramsObject.Target, paramsObject);

            if (Rng.RollProbability() <= accuracyCheck)
            {
                if(targetItem != null)
                {
                    targetItem.Owner?.Inventory?.Remove(targetItem);
                    targetItem.Owner = null;
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
