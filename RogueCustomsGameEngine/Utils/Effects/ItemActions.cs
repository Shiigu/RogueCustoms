using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Helpers;
using System;

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

        public static bool Equip(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            _ = 0;
            if (Target == null) return false;
            if (This is not Item i || Target is not Character c)
                throw new InvalidOperationException($"Attempted to equip {This.Name} on {Target.Name}, which is not valid");
            if (This.EntityType != EntityType.Weapon && This.EntityType != EntityType.Armor)
                throw new InvalidOperationException("Attempted to equip an unequippable item!");

            var currentEquippedWeapon = c.EquippedWeapon;
            var currentEquippedArmor = c.EquippedArmor;
            c.SwapWithEquippedItem(i.EntityType == EntityType.Weapon ? currentEquippedWeapon : currentEquippedArmor, i);

            return true;
        }

        public static bool Remove(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            _ = 0;
            if (Target == null) return false;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);

            if (paramsObject.Target is not Item i)
                throw new InvalidOperationException($"Attempted to remove {paramsObject.Target.Name} as if it were an item, which it isn't.");

            if (Rng.NextInclusive(1, 100) <= paramsObject.Chance)
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
