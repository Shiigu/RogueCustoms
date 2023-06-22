﻿using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Helpers;
using System;

namespace RogueCustomsGameEngine.Utils.Effects
{
    public static class AttackActions
    {
        public static Random Rng;
        public static Map Map;

        public static bool DealDamage(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int output, params (string ParamName, string Value)[] args)
        {
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            var damageDealt = Math.Max(0, (int) paramsObject.Damage - (int) paramsObject.Mitigation);
            output = damageDealt;
            if(damageDealt <= 0 || Rng.NextInclusive(1, 100) > paramsObject.Accuracy)
                return false;
            if (paramsObject.Target.EntityType == EntityType.Player
                || (paramsObject.Target.EntityType == EntityType.NPC && Map.Player.CanSee(paramsObject.Target)))
            {
                Map.AppendMessage(Map.Locale["CharacterTakesDamage"].Format(new { CharacterName = paramsObject.Target.Name, DamageDealt = damageDealt }));
            }
            paramsObject.Target.HP = Math.Max(0, paramsObject.Target.HP - damageDealt);
            if (paramsObject.Target.HP == 0)
                paramsObject.Target.Die(paramsObject.Attacker);
            return true;
        }
    }
}