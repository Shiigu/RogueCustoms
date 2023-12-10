using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Helpers;
using System;
using RogueCustomsGameEngine.Game.Entities.Interfaces;

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

        public static bool Remove(Entity This, Entity Source, ITargetable Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            _ = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);

            if (paramsObject.Target is not Item i)
                throw new InvalidOperationException($"Attempted to remove {paramsObject.Target.Name} as if it were an item, which it isn't.");
            var accuracyCheck = ActionHelpers.CalculateAdjustedAccuracy(Source, paramsObject.Target, paramsObject);

            if (Rng.RollProbability() <= accuracyCheck)
            {
                i.Owner?.Inventory?.Remove(i);
                i.Owner = null;
                i.ExistenceStatus = EntityExistenceStatus.Gone;
                i.Position = null;
                Map.Items.Remove(i);

                return true;
            }
            return false;
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    #pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}
