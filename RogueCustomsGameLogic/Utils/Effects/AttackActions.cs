using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Helpers;
using System;
using RogueCustomsGameEngine.Utils.Representation;
using System.Drawing;

namespace RogueCustomsGameEngine.Utils.Effects
{
    public static class AttackActions
    {
        public static Random Rng;
        public static Map Map;

        public static bool DealDamage(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int output, params (string ParamName, string Value)[] args)
        {
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (paramsObject.Target is not Character c) throw new ArgumentException($"Attempted to damage {paramsObject.Target.Name} when it's not a Character.");
            if (c.ExistenceStatus != EntityExistenceStatus.Alive)
            {
                output = 0;
                return false;
            }
            var damageDealt = Math.Max(0, (int) paramsObject.Damage - (int) paramsObject.Mitigation);
            output = damageDealt;
            if(damageDealt <= 0 || Rng.NextInclusive(1, 100) > paramsObject.Accuracy)
                return false;
            if (c.EntityType == EntityType.Player
                || (c.EntityType == EntityType.NPC && Map.Player.CanSee(c)))
            {
                Faction targetFaction = c.Faction;
                Color forecolorToUse;
                if (c.EntityType == EntityType.Player || targetFaction.AlliedWith.Contains(Map.Player.Faction))
                    forecolorToUse = Color.Red;
                else if (targetFaction.EnemiesWith.Contains(Map.Player.Faction))
                    forecolorToUse = Color.Lime;
                else
                    forecolorToUse = Color.DeepSkyBlue;
                Map.AppendMessage(Map.Locale["CharacterTakesDamage"].Format(new { CharacterName = c.Name, DamageDealt = damageDealt }), forecolorToUse);
            }
            c.HP = Math.Max(0, c.HP - damageDealt);
            if (c.HP == 0 && c.ExistenceStatus == EntityExistenceStatus.Alive)
                c.Die(paramsObject.Attacker);
            return true;
        }
    }
}
