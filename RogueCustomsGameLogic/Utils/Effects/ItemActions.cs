using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Helpers;
using System;

namespace RogueCustomsGameEngine.Utils.Effects
{
    public static class ItemActions
    {
        public static Random Rng;
        public static Map Map;

        public static bool Equip(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            _ = 0;
            if (This is not Item i || Target is not Character c)
                throw new Exception($"Attempted to equip {This.Name} on {Target.Name}, which is not valid");
            if (This.EntityType != EntityType.Weapon && This.EntityType != EntityType.Armor)
                throw new Exception("Attempted to equip an unequippable item!");

            var currentEquippedWeapon = c.EquippedWeapon;
            var currentEquippedArmor = c.EquippedArmor;
            c.SwapWithEquippedItem(i.EntityType == EntityType.Weapon ? currentEquippedWeapon : currentEquippedArmor, i);

            return true;
        }

        public static bool Remove(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);

            if (paramsObject.Target is not Item i)
                throw new Exception($"Attempted to remove {paramsObject.Target.Name} as if it were an item, which it isn't.");

            _ = 0;

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
}
