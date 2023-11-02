using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Helpers;
using System;
using RogueCustomsGameEngine.Utils.Representation;
using System.Drawing;

namespace RogueCustomsGameEngine.Utils.Effects
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public static class AttackActions
    {
        private static RngHandler Rng;
        private static Map Map;

        public static void SetActionParams(RngHandler rng, Map map)
        {
            Rng = rng;
            Map = map;
        }

        public static bool DealDamage(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int output, params (string ParamName, string Value)[] args)
        {
            output = 0;
            if (Target == null) return false;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (paramsObject.Target is not Character c) throw new ArgumentException($"Attempted to damage {paramsObject.Target.Name} when it's not a Character.");
            if (c.ExistenceStatus != EntityExistenceStatus.Alive)
                return false;
            var damageDealt = Math.Max(0, paramsObject.Damage - paramsObject.Mitigation);
            if (damageDealt > 0 && damageDealt < 1)
                damageDealt = 1;
            damageDealt = (int) damageDealt;
            output = (int) damageDealt;
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
                Map.AppendMessage(Map.Locale["CharacterTakesDamage"].Format(new { CharacterName = c.Name, DamageDealt = damageDealt, CharacterHPStat = Map.Locale["CharacterHPStat"] }), forecolorToUse);
                if (c.EntityType == EntityType.Player)
                    Map.PlayerTookDamage = true;
            }
            c.HP = Math.Max(0, c.HP - damageDealt);
            if (c.HP == 0 && c.ExistenceStatus == EntityExistenceStatus.Alive)
                c.Die(paramsObject.Attacker);
            return true;
        }

        public static bool BurnMP(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int output, params (string ParamName, string Value)[] args)
        {
            output = 0;
            if (Target == null) return false;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (paramsObject.Target is not Character c) throw new ArgumentException($"Attempted to burn {paramsObject.Target.Name}'s MP when it's not a Character.");
            if (c.ExistenceStatus != EntityExistenceStatus.Alive)
                return false;
            if (!c.UsesMP)
                return false;
            if (Rng.NextInclusive(1, 100) > paramsObject.Accuracy)
                return false;
            var burnAmount = paramsObject.Power;
            if (paramsObject.Power > 0 && paramsObject.Power < 1)
                burnAmount = 1;
            burnAmount = (int)burnAmount;
            output = burnAmount;
            if (burnAmount <= 0)
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
                Map.AppendMessage(Map.Locale["CharacterLosesMP"].Format(new { CharacterName = c.Name, BurnedMP = paramsObject.Power, CharacterMPStat = Map.Locale["CharacterMPStat"] }), forecolorToUse);
                if (c.EntityType == EntityType.Player)
                    Map.PlayerGotMPBurned = true;
            }
            c.MP = Math.Max(0, c.MP - burnAmount);
            return true;
        }
    }
}
