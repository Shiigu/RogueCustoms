using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Helpers;
using System;
using RogueCustomsGameEngine.Utils.Representation;
using System.Drawing;
using System.Linq;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.Expressions;
using System.Collections.Generic;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using System.Xml.Linq;

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

        public static bool DealDamage(Entity This, Entity Source, ITargetable Target, params (string ParamName, string Value)[] args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(This, Source, Target, args);
            if (paramsObject.Target is not Character c)
                // Attempted to damage Target when it's not a Character.
                return false;
            if (c.ExistenceStatus != EntityExistenceStatus.Alive)
                return false;

            var attackElement = Map.Elements.Find(e => e.Id.Equals(paramsObject.Element, StringComparison.InvariantCultureIgnoreCase))
                ?? throw new ArgumentException($"DealDamage tries to use Element {paramsObject.Element}, which does not exist.");

            var resistanceStat = c.UsedStats.Find(s => s.Id.Equals(attackElement.ResistanceStatId, StringComparison.InvariantCultureIgnoreCase));
            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(paramsObject.Attacker, paramsObject.Target, paramsObject);
            var canCallElementEffect = !paramsObject.BypassesElementEffect;
            if (Rng.RollProbability() > accuracyCheck)
            {
                if (c == Map.Player || Map.Player.CanSee(c))
                {
                    Map.AppendMessage(Map.Locale["AttackMissedText"], Color.White);
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.PlaySpecialEffect,
                        Params = new() { SpecialEffect.Miss }
                    }
                    );
                }
                Map.DisplayEvents.Add(($"{c.Name}'s attack missed", events));
                return false;
            }
            if (canCallElementEffect && attackElement.OnAfterAttack != null && !string.IsNullOrWhiteSpace(attackElement.OnAfterAttack.UseCondition))
            {
                var parsedCondition = ExpressionParser.ParseArgForExpression(attackElement.OnAfterAttack.UseCondition, This, Source, c);

                if (!ExpressionParser.CalculateBooleanExpression(parsedCondition))
                    canCallElementEffect = false;
            }
            int damageResistance = 0;
            if(resistanceStat != null && !paramsObject.BypassesResistances)
            {
                damageResistance = (int) Math.Ceiling((float) paramsObject.Damage * (float) resistanceStat.BaseAfterModifications / 100);
            }
            if(damageResistance > paramsObject.Damage && attackElement.ExcessResistanceCausesHealDamage)
            {
                var targetParam = args.FirstOrDefault(a => a.ParamName.Equals("Target", StringComparison.InvariantCultureIgnoreCase));
                var healDamageParams = new List<(string ParamName, string Value)>
                {
                    ("Source", "source"),
                    ("Target", targetParam.Value),
                    ("Power", (damageResistance - paramsObject.Damage).ToString())
                };
                var healResult = GenericActions.HealDamage(This, paramsObject.Source, c, healDamageParams.ToArray());

                if (Map.Flags.Exists(f => f.Key.Equals($"ElementCausedHealDamage")))
                    Map.SetFlagValue($"ElementCausedHealDamage", 1);
                else
                    Map.CreateFlag($"ElementCausedHealDamage", 1, true); 
                
                if (healResult && canCallElementEffect)
                    attackElement.OnAfterAttack?.Do(Source, c, false);

                Map.SetFlagValue($"ElementCausedHealDamage", 0);

                return true;
            }
            var damageDealt = Math.Max(0, paramsObject.Damage - paramsObject.Mitigation - damageResistance);
            if (damageDealt > 0 && damageDealt < 1)
                damageDealt = 1;
            damageDealt = (int) damageDealt;
            if (Map.Flags.Exists(f => f.Key.Equals($"DamageTaken_{c.Id}")))
                Map.SetFlagValue($"DamageTaken_{c.Id}", damageDealt);
            else
                Map.CreateFlag($"DamageTaken_{c.Id}", damageDealt, true);
            if (damageDealt <= 0)
            {
                if (c == Map.Player || Map.Player.CanSee(c))
                {
                    Map.AppendMessage(Map.Locale["AttackDealtNoDamageText"], Color.White);
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.PlaySpecialEffect,
                        Params = new() { SpecialEffect.Miss }
                    }
                    );
                }
                Map.DisplayEvents.Add(($"{c.Name}'s attack failed to deal damage", events));
                return false;
            }
            if (Rng.RollProbability() <= paramsObject.CriticalHitChance)
            {
                if (c == Map.Player || Map.Player.CanSee(c))
                {
                    Map.AppendMessage(Map.Locale["AttackCriticalHitText"], attackElement.Color);
                }
                damageDealt = (int) ExpressionParser.CalculateNumericExpression(paramsObject.CriticalHitFormula.Replace("{CalculatedDamage}", damageDealt.ToString()));
            }
            if (c == Map.Player || Map.Player.CanSee(c))
            {
                Map.AppendMessage(Map.Locale["CharacterTakesDamage"].Format(new { CharacterName = c.Name, DamageDealt = damageDealt, CharacterHPStat = Map.Locale["CharacterHPStat"], ElementName = attackElement.Name }), attackElement.Color);
            }
            c.HP.Current = Math.Max(0, c.HP.Current - damageDealt);
            if (c == Map.Player)
            {
                events.Add(new()
                {
                    DisplayEventType = DisplayEventType.UpdatePlayerData,
                    Params = new() { UpdatePlayerDataType.ModifyStat, "HP", c.HP.Current }
                });
            }
            if (Map.Player.CanSee(c) && c.EntityType == EntityType.NPC)
            {
                events.Add(new()
                {
                    DisplayEventType = DisplayEventType.PlaySpecialEffect,
                    Params = new() { SpecialEffect.NPCDamaged }
                }
                );
            }
            else if (c == Map.Player)
            {
                events.Add(new()
                {
                    DisplayEventType = DisplayEventType.PlaySpecialEffect,
                    Params = new() { SpecialEffect.PlayerDamaged }
                }
                );
            }
            Map.DisplayEvents.Add(($"{c.Name} took damage", events));
            if (c.HP.Current == 0 && c.ExistenceStatus == EntityExistenceStatus.Alive)
                c.Die(paramsObject.Attacker);
            else if (canCallElementEffect)
            {
                attackElement.OnAfterAttack?.Do(Source, c, false);
                if (c.HP.Current == 0 && c.ExistenceStatus == EntityExistenceStatus.Alive)
                    c.Die(paramsObject.Attacker);
            }
            return true;
        }

        public static bool BurnMP(Entity This, Entity Source, ITargetable Target, params (string ParamName, string Value)[] args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(This, Source, Target, args);
            if (paramsObject.Target is not Character c)
                // Attempted to burn Target's MP when it's not a Character.
                return false;
            if (c.ExistenceStatus != EntityExistenceStatus.Alive)
                return false;
            if (c.MP == null)
                return false;

            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(paramsObject.Attacker, paramsObject.Target, paramsObject);

            if (Rng.RollProbability() > accuracyCheck)
                return false;
            var burnAmount = paramsObject.Power;
            if (paramsObject.Power > 0 && paramsObject.Power < 1)
                burnAmount = 1;
            burnAmount = (int)burnAmount;
            if (Map.Flags.Exists(f => f.Key.Equals($"MPBurned_{c.Id}")))
                Map.SetFlagValue($"MPBurned_{c.Id}", burnAmount);
            else
                Map.CreateFlag($"MPBurned_{c.Id}", burnAmount, true);
            if (burnAmount <= 0)
                return false;
            if (c == Map.Player || Map.Player.CanSee(c))
            {
                Map.AppendMessage(Map.Locale["CharacterLosesMP"].Format(new { CharacterName = c.Name, BurnedMP = paramsObject.Power, CharacterMPStat = Map.Locale["CharacterMPStat"] }), Color.DeepSkyBlue);
            }
            c.MP.Current = Math.Max(0, c.MP.Current - burnAmount);
            if (c == Map.Player || Map.Player.CanSee(c))
            {
                if (c == Map.Player)
                {
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.UpdatePlayerData,
                        Params = new() { UpdatePlayerDataType.ModifyStat, "MP", c.MP.Current }
                    });
                }
                events.Add(new()
                {
                    DisplayEventType = DisplayEventType.PlaySpecialEffect,
                    Params = new() { SpecialEffect.MPDown }
                }
                );
            }
            Map.DisplayEvents.Add(($"{c.Name} lost MP", events));
            return true;
        }
        public static bool RemoveHunger(Entity This, Entity Source, ITargetable Target, params (string ParamName, string Value)[] args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(This, Source, Target, args);
            if (paramsObject.Target is not Character c)
                // Attempted to remove Target's Hunger when it's not a Character.
                return false;
            if (c.ExistenceStatus != EntityExistenceStatus.Alive)
                return false;
            if (c.Hunger == null)
                return false;

            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(paramsObject.Attacker, paramsObject.Target, paramsObject);

            if (Rng.RollProbability() > accuracyCheck)
                return false;
            var lossAmount = paramsObject.Power;
            if (paramsObject.Power > 0 && paramsObject.Power < 1)
                lossAmount = 1;
            lossAmount = (int)lossAmount;
            if (lossAmount <= 0)
                return false;
            if (c == Map.Player || Map.Player.CanSee(c))
            {
                Map.AppendMessage(Map.Locale["CharacterLosesHunger"].Format(new { CharacterName = c.Name, LostHunger = paramsObject.Power, CharacterHungerStat = Map.Locale["CharacterHungerStat"] }), Color.DeepSkyBlue);
            }
            c.Hunger.Current = Math.Max(0, c.Hunger.Current - lossAmount);
            if (c == Map.Player || Map.Player.CanSee(c))
            {
                if (c == Map.Player)
                {
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.UpdatePlayerData,
                        Params = new() { UpdatePlayerDataType.ModifyStat, "Hunger", c.Hunger.Current }
                    });
                }
                events.Add(new()
                {
                    DisplayEventType = DisplayEventType.PlaySpecialEffect,
                    Params = new() { SpecialEffect.HungerDown }
                }
                );
            }
            Map.DisplayEvents.Add(($"{c.Name} lost hunger", events));
            return true;
        }
    }
}
