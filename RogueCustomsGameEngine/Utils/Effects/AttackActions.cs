using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Helpers;
using System;
using RogueCustomsGameEngine.Utils.Representation;
using System.Drawing;
using System.Linq;
using RogueCustomsGameEngine.Game.Entities.Interfaces;

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

        public static bool DealDamage(Entity This, Entity Source, ITargetable Target, int previousEffectOutput, out int output, params (string ParamName, string Value)[] args)
        {
            output = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (paramsObject.Target is not Character c)
                // Attempted to damage Target when it's not a Character.
                return false;
            if (c.ExistenceStatus != EntityExistenceStatus.Alive)
                return false;

            var accuracyCheck = ActionHelpers.CalculateAdjustedAccuracy(paramsObject.Attacker, paramsObject.Target, paramsObject);

            if (Rng.RollProbability() > accuracyCheck)
                return false;
            var damageDealt = Math.Max(0, paramsObject.Damage - paramsObject.Mitigation);
            if (damageDealt > 0 && damageDealt < 1)
                damageDealt = 1;
            damageDealt = (int) damageDealt;
            output = (int) damageDealt;
            if (Map.Flags.Exists(f => f.Key.Equals($"DamageTaken_{c.Id}")))
                Map.SetFlagValue($"DamageTaken_{c.Id}", damageDealt);
            else
                Map.CreateFlag($"DamageTaken_{c.Id}", damageDealt, true);
            if (damageDealt <= 0)
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

        public static bool BurnMP(Entity This, Entity Source, ITargetable Target, int previousEffectOutput, out int output, params (string ParamName, string Value)[] args)
        {
            output = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (paramsObject.Target is not Character c)
                // Attempted to burn Target's MP when it's not a Character.
                return false;
            if (c.ExistenceStatus != EntityExistenceStatus.Alive)
                return false;
            if (!c.UsesMP)
                return false;

            var accuracyCheck = ActionHelpers.CalculateAdjustedAccuracy(paramsObject.Attacker, paramsObject.Target, paramsObject);

            if (Rng.RollProbability() > accuracyCheck)
                return false;
            var burnAmount = paramsObject.Power;
            if (paramsObject.Power > 0 && paramsObject.Power < 1)
                burnAmount = 1;
            burnAmount = (int)burnAmount;
            output = burnAmount;
            if (Map.Flags.Exists(f => f.Key.Equals($"MPBurned_{c.Id}")))
                Map.SetFlagValue($"MPBurned_{c.Id}", burnAmount);
            else
                Map.CreateFlag($"MPBurned_{c.Id}", burnAmount, true);
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
        public static bool RemoveHunger(Entity This, Entity Source, ITargetable Target, int previousEffectOutput, out int output, params (string ParamName, string Value)[] args)
        {
            output = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (paramsObject.Target is not Character c)
                // Attempted to remove Target's Hunger when it's not a Character.
                return false;
            if (c.ExistenceStatus != EntityExistenceStatus.Alive)
                return false;
            if (!c.UsesMP)
                return false;

            var accuracyCheck = ActionHelpers.CalculateAdjustedAccuracy(paramsObject.Attacker, paramsObject.Target, paramsObject);

            if (Rng.RollProbability() > accuracyCheck)
                return false;
            var hungerAmount = paramsObject.Power;
            if (paramsObject.Power > 0 && paramsObject.Power < 1)
                hungerAmount = 1;
            hungerAmount = (int)hungerAmount;
            output = hungerAmount;
            if (hungerAmount <= 0)
                return false;
            if (c.EntityType == EntityType.Player
                || (c.EntityType == EntityType.NPC && Map.Player.CanSee(c)))
            {
                Map.AppendMessage(Map.Locale["CharacterLosesHunger"].Format(new { CharacterName = c.Name, LostHunger = paramsObject.Power, CharacterHungerStat = Map.Locale["CharacterHungerStat"] }), Color.DeepSkyBlue);
            }
            c.MP = Math.Max(0, c.MP - hungerAmount);
            return true;
        }
    }
}
