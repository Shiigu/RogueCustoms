using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Helpers;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using RogueCustomsGameEngine.Utils.Representation;
using System.Drawing;
using org.matheval;

namespace RogueCustomsGameEngine.Utils.Effects
{
    #pragma warning disable S2259 // Null pointers should not be dereferenced
    #pragma warning disable S2589 // Boolean expressions should not be gratuitous
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    // Represents Actions that are not expected to be used by any type of Entity in particular. Free to use by everyone.
    public static class GenericActions
    {
        private static RngHandler Rng;
        private static Map Map;
        public static void SetActionParams(RngHandler rng, Map map)
        {
            Rng = rng;
            Map = map;
        }

        public static bool PrintText(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            _ = 0;
            var entityTypesForVisibilityCheck = new List<EntityType> { EntityType.Player, EntityType.NPC };

            if ((entityTypesForVisibilityCheck.Contains(Source.EntityType) && Map.Player.CanSee(Source))
                || (Target != null && entityTypesForVisibilityCheck.Contains(Target.EntityType) && Map.Player.CanSee(Target)))
            {
                if(ExpandoObjectHelper.HasProperty(paramsObject, "Color"))
                    Map.AppendMessage(paramsObject.Text, paramsObject.Color);
                else
                    Map.AppendMessage(paramsObject.Text);
            }

            return true;
        }

        public static bool MessageBox(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            _ = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            Map.AddMessageBox(paramsObject.Title, paramsObject.Text, "OK", paramsObject.Color);
            return true;
        }

        public static bool HealDamage(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int output, params (string ParamName, string Value)[] args)
        {
            output = 0;
            if (Target == null) return false;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (paramsObject.Target is not Character) throw new ArgumentException($"Attempted to heal {paramsObject.Target.Name} when it's not a Character.");
            if (paramsObject.Target.HP >= paramsObject.Target.MaxHP)
                return false;
            var healAmount = Math.Min(paramsObject.Target.MaxHP - paramsObject.Target.HP, paramsObject.Power);
            if (paramsObject.Power > 0 && paramsObject.Power < 1)
                healAmount = 1;
            healAmount = (int)healAmount;
            output = (int)healAmount;
            paramsObject.Target.HP = Math.Min(paramsObject.Target.MaxHP, paramsObject.Target.HP + healAmount);

            if (paramsObject.Target.EntityType == EntityType.Player
                || (paramsObject.Target.EntityType == EntityType.NPC && Map.Player.CanSee(paramsObject.Target)))
            {
                if (paramsObject.Target.HP == paramsObject.Target.MaxHP)
                    Map.AppendMessage(Map.Locale["CharacterHealsAllHP"].Format(new { CharacterName = paramsObject.Target.Name }), Color.DeepSkyBlue);
                else
                    Map.AppendMessage(Map.Locale["CharacterHealsSomeHP"].Format(new { CharacterName = paramsObject.Target.Name, HealAmount = healAmount.ToString(), CharacterHPStat = Map.Locale["CharacterHPStat"] }), Color.DeepSkyBlue);
            }

            if (paramsObject.Target.EntityType == EntityType.Player)
                Map.PlayerGotHealed = true;
            return true;
        }

        public static bool GiveExperience(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int output, params (string ParamName, string Value)[] args)
        {
            output = 0;
            if (Target == null) return false;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (paramsObject.Target is not Character || !paramsObject.Target.CanGainExperience) return false;
            if (paramsObject.Target.Level == paramsObject.Target.MaxLevel) return false;
            if (Target.EntityType == EntityType.Player
                || (Target.EntityType == EntityType.NPC && Map.Player.CanSee(Target)))
            {
                Map.AppendMessage(Map.Locale["CharacterGainsExperience"].Format(new { CharacterName = paramsObject.Target.Name, Amount = ((int)paramsObject.Amount).ToString() }), Color.DeepSkyBlue);
                paramsObject.Target.GainExperience((int) paramsObject.Amount);
                output = (int) paramsObject.Amount;
            }
            return true;
        }

        public static bool ApplyAlteredStatus(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            _ = 0;
            if (Target == null) return false;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (paramsObject.Target is not Character) throw new ArgumentException($"Attempted to apply an Altered Status on {paramsObject.Target.Name} when it's not a Character.");
            var statusToApply = Map.PossibleStatuses.Find(ps => string.Equals(ps.ClassId, paramsObject.Id, StringComparison.InvariantCultureIgnoreCase))
                                   ?? throw new ArgumentException($"Altered status {paramsObject.Id} does not exist!");
            var statusTarget = paramsObject.Target as Character;
            if (statusTarget.ExistenceStatus == EntityExistenceStatus.Alive && Rng.NextInclusive(1, 100) <= paramsObject.Chance)
            {
                var targetAlreadyHadStatus = statusTarget.AlteredStatuses.Exists(als => als.RemainingTurns != 0 && als.ClassId.Equals(paramsObject.Id));
                var statusPower = (decimal) paramsObject.Power;
                var turnlength = (int)paramsObject.TurnLength;
                var success = statusToApply.ApplyTo(statusTarget, statusPower, turnlength);
                if (success && (statusTarget.EntityType == EntityType.Player
                        || (statusTarget.EntityType == EntityType.NPC && Map.Player.CanSee(statusTarget))))
                {
                    if(!targetAlreadyHadStatus)
                        Map.AppendMessage(Map.Locale["CharacterGotStatused"].Format(new { CharacterName = paramsObject.Target.Name, StatusName = statusToApply.Name }), Color.DeepSkyBlue);
                    else
                        Map.AppendMessage(Map.Locale["CharacterStatusGotRefreshed"].Format(new { CharacterName = paramsObject.Target.Name, StatusName = statusToApply.Name }), Color.DeepSkyBlue);
                }
                return success;
            }
            return false;
        }

        public static bool ApplyStatAlteration(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int output, params (string ParamName, string Value)[] args)
        {
            output = 0;
            if (Target == null) return false;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (paramsObject.Target is not Character) throw new ArgumentException($"Attempted to alter one of {paramsObject.Target.Name}'s stats when it's not a Character.");
            var statAlterationTarget = paramsObject.Target as Character;
            if ((string.Equals(paramsObject.StatName, "mpregeneration", StringComparison.InvariantCultureIgnoreCase) || string.Equals(paramsObject.StatName, "mp", StringComparison.InvariantCultureIgnoreCase))
                && !statAlterationTarget.UsesMP)
            {
                return false;
            }

            var statAlterations = paramsObject.StatAlterationList as List<StatModification>;
            var statCap = 0m;
            var statValue = 0m;
            switch (paramsObject.StatName.ToLowerInvariant())
            {
                case "hp":
                case "maxhp":
                    statValue = statAlterationTarget.HP;
                    statCap = Constants.RESOURCE_STAT_CAP;
                    break;
                case "mp":
                case "maxmp":
                    statValue = statAlterationTarget.MP;
                    statCap = Constants.RESOURCE_STAT_CAP;
                    break;
                case "attack":
                    statValue = statAlterationTarget.Attack;
                    statCap = Constants.NORMAL_STAT_CAP;
                    break;
                case "defense":
                    statValue = statAlterationTarget.Defense;
                    statCap = Constants.NORMAL_STAT_CAP;
                    break;
                case "movement":
                    statValue = statAlterationTarget.Movement;
                    statCap = Constants.MOVEMENT_STAT_CAP;
                    break;
                case "hpregeneration":
                    statValue = statAlterationTarget.HPRegeneration;
                    statCap = Constants.REGEN_STAT_CAP;
                    break;
                case "mpregeneration":
                    statValue = statAlterationTarget.MPRegeneration;
                    statCap = Constants.REGEN_STAT_CAP;
                    break;
                default:
                    throw new ArgumentException($"Unrecognized stat: {paramsObject.StatName}.");
            }
            if (statValue >= statCap)
                return false;
            if (statAlterationTarget.ExistenceStatus == EntityExistenceStatus.Alive && (paramsObject.Amount != 0 && (paramsObject.CanBeStacked || !statAlterations.Exists(sa => sa.RemainingTurns > 0 && sa.Id.Equals(paramsObject.Id)))) && Rng.NextInclusive(1, 100) <= paramsObject.Chance)
            {
                var isHPRegeneration = string.Equals(paramsObject.StatName, "hpregeneration", StringComparison.InvariantCultureIgnoreCase);
                var isMPRegeneration = string.Equals(paramsObject.StatName, "mpregeneration", StringComparison.InvariantCultureIgnoreCase);
                var alterationAmount = isHPRegeneration || isMPRegeneration ? paramsObject.Amount : (int)paramsObject.Amount;

                if(!isHPRegeneration && !isMPRegeneration)
                {
                    if (paramsObject.Amount > 0 && paramsObject.Amount < 1)
                        alterationAmount = 1;
                    else if (paramsObject.Amount < 0 && paramsObject.Amount > -1)
                        alterationAmount = -1;
                }

                if (alterationAmount > statCap)
                    alterationAmount = statCap;

                statAlterations.Add(new StatModification
                {
                    Id = paramsObject.Id,
                    Amount = alterationAmount,
                    RemainingTurns = (int)paramsObject.TurnLength
                });
                output = (int)alterationAmount;
                var statName = paramsObject.StatName;
                if (isHPRegeneration)
                    statName = "HPRegeneration";
                else if (isMPRegeneration)
                    statName = "MPRegeneration";
                if (paramsObject.DisplayOnLog && (statAlterationTarget.EntityType == EntityType.Player
                    || (statAlterationTarget.EntityType == EntityType.NPC && Map.Player.CanSee(paramsObject.Target))))
                {
                    if(paramsObject.Amount > 0)
                        Map.AppendMessage(Map.Locale["CharacterStatGotBuffed"].Format(new { CharacterName = statAlterationTarget.Name, StatName = Map.Locale[$"Character{statName}Stat"], Amount = Math.Abs(alterationAmount).ToString("0.#####") }), Color.DeepSkyBlue);
                    else
                        Map.AppendMessage(Map.Locale["CharacterStatGotNerfed"].Format(new { CharacterName = statAlterationTarget.Name, StatName = Map.Locale[$"Character{statName}Stat"], Amount = Math.Abs(alterationAmount).ToString("0.#####") }), Color.DeepSkyBlue);
                }
                return true;
            }
            return false;
        }

        public static bool CleanseAlteredStatus(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            _ = 0;
            if (Target == null) return false;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (paramsObject.Target is not Character c) throw new ArgumentException($"Attempted to remove an Altered Status on {paramsObject.Target.Name} when it's not a Character.");
            var statusToRemove = Map.PossibleStatuses.Find(ps => string.Equals(ps.ClassId, paramsObject.Id, StringComparison.InvariantCultureIgnoreCase));
            if (!statusToRemove.CleansedByCleanseActions) throw new InvalidOperationException($"Attempted to remove {statusToRemove.Name} with a Cleanse action when it can't be cleansed that way.");
            if (c.AlteredStatuses.Exists(als => als.ClassId.Equals(statusToRemove.ClassId)) && Rng.NextInclusive(1, 100) <= paramsObject.Chance)
            {
                c.AlteredStatuses.RemoveAll(als => als.ClassId.Equals(statusToRemove.ClassId));
                c.MaxHPModifications?.RemoveAll(a => a.Id.Equals(statusToRemove.Name));
                c.MaxMPModifications?.RemoveAll(a => a.Id.Equals(statusToRemove.Name));
                c.AttackModifications?.RemoveAll(a => a.Id.Equals(statusToRemove.Name));
                c.DefenseModifications?.RemoveAll(a => a.Id.Equals(statusToRemove.Name));
                c.MovementModifications?.RemoveAll(a => a.Id.Equals(statusToRemove.Name));
                c.HPRegenerationModifications?.RemoveAll(a => a.Id.Equals(statusToRemove.Name));
                c.MPRegenerationModifications?.RemoveAll(a => a.Id.Equals(statusToRemove.Name));
                if (c.EntityType == EntityType.Player
                    || (c.EntityType == EntityType.NPC && Map.Player.CanSee(c)))
                {
                    Map.AppendMessage(Map.Locale["CharacterIsNoLongerStatused"].Format(new { CharacterName = c.Name, StatusName = statusToRemove.Name }), Color.DeepSkyBlue);
                }
                return true;
            }
            return false;
        }

        public static bool CleanseStatAlteration(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            _ = 0;
            if (Target == null) return false;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (paramsObject.Target is not Character) throw new ArgumentException($"Attempted to alter one of {paramsObject.Target.Name}'s stats when it's not a Character.");
            var statAlterations = paramsObject.StatAlterationList as List<StatModification>;

            if (statAlterations?.Any() == true && Rng.NextInclusive(1, 100) <= paramsObject.Chance)
            {
                statAlterations.Clear();

                if (paramsObject.Target.EntityType == EntityType.Player
                    || (paramsObject.Target.EntityType == EntityType.NPC && Map.Player.CanSee(Target)))
                {
                    var isHPRegeneration = string.Equals(paramsObject.StatName, "hpregeneration", StringComparison.InvariantCultureIgnoreCase);
                    var isMPRegeneration = string.Equals(paramsObject.StatName, "mpregeneration", StringComparison.InvariantCultureIgnoreCase);
                    var statName = paramsObject.StatName;
                    if (isHPRegeneration)
                        statName = "HPRegeneration";
                    else if (isMPRegeneration)
                        statName = "MPRegeneration";
                    Map.AppendMessage(Map.Locale["CharacterStatGotNeutralized"].Format(new { CharacterName = paramsObject.Target.Name, StatName = Map.Locale[$"Character{statName}Stat"] }), Color.DeepSkyBlue);
                }
                return true;
            }

            return false;
        }

        public static bool CleanseStatAlterations(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            _ = 0;
            if (Target == null) return false;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (paramsObject.Target is not Character c) throw new ArgumentException($"Attempted to alter one of {paramsObject.Target.Name}'s stats when it's not a Character.");

            if ((c.MaxHPModifications?.Any() == true || c.AttackModifications?.Any() == true
                || c.DefenseModifications?.Any() == true || c.MovementModifications?.Any() == true || c.HPRegenerationModifications?.Any() == true) && Rng.NextInclusive(1, 100) <= paramsObject.Chance)
            {
                c.MaxHPModifications.Clear();
                if(c == Map.Player || Map.Player.CanSee(c))
                    Map.AppendMessage(Map.Locale["CharacterStatGotNeutralized"].Format(new { CharacterName = c.Name, StatName = Map.Locale["CharacterMaxHPStat"] }), Color.DeepSkyBlue);
                if(c.UsesMP)
                {
                    c.MaxMPModifications.Clear();
                    if (c == Map.Player || Map.Player.CanSee(c))
                        Map.AppendMessage(Map.Locale["CharacterStatGotNeutralized"].Format(new { CharacterName = c.Name, StatName = Map.Locale["CharacterMaxMPStat"] }), Color.DeepSkyBlue);
                }
                c.AttackModifications.Clear();
                if (c == Map.Player || Map.Player.CanSee(c))
                    Map.AppendMessage(Map.Locale["CharacterStatGotNeutralized"].Format(new { CharacterName = c.Name, StatName = Map.Locale["CharacterAttackStat"] }), Color.DeepSkyBlue);
                c.DefenseModifications.Clear();
                if (c == Map.Player || Map.Player.CanSee(c))
                    Map.AppendMessage(Map.Locale["CharacterStatGotNeutralized"].Format(new { CharacterName = c.Name, StatName = Map.Locale["CharacterDefenseStat"] }), Color.DeepSkyBlue);
                c.MovementModifications.Clear();
                if (c == Map.Player || Map.Player.CanSee(c))
                    Map.AppendMessage(Map.Locale["CharacterStatGotNeutralized"].Format(new { CharacterName = c.Name, StatName = Map.Locale["CharacterMovementStat"] }), Color.DeepSkyBlue);
                c.HPRegenerationModifications.Clear();
                if (c == Map.Player || Map.Player.CanSee(c))
                    Map.AppendMessage(Map.Locale["CharacterStatGotNeutralized"].Format(new { CharacterName = c.Name, StatName = Map.Locale["CharacterHPRegenerationStat"] }), Color.DeepSkyBlue);
                if(c.UsesMP)
                {
                    c.MPRegenerationModifications.Clear();
                    if (c == Map.Player || Map.Player.CanSee(c))
                        Map.AppendMessage(Map.Locale["CharacterStatGotNeutralized"].Format(new { CharacterName = c.Name, StatName = Map.Locale["CharacterMPRegenerationStat"] }), Color.DeepSkyBlue);
                }

                return true;
            }

            return false;
        }

        public static bool CleanseAllAlteredStatuses(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            _ = 0;
            if (Target == null) return false;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (paramsObject.Target is not Character c) throw new ArgumentException($"Attempted to remove an Altered Status on {paramsObject.Target.Name} when it's not a Character.");
            if (Rng.NextInclusive(1, 100) <= paramsObject.Chance && c.AlteredStatuses?.Any() == true)
            {
                c.AlteredStatuses.ForEach(als => {
                    if (c.EntityType == EntityType.Player
                        || (c.EntityType == EntityType.NPC && Map.Player.CanSee(c)))
                    {
                        Map.AppendMessage(Map.Locale["CharacterIsNoLongerStatused"].Format(new { CharacterName = c.Name, StatusName = als.Name }), Color.DeepSkyBlue);
                    }
                    c.MaxHPModifications?.RemoveAll(a => a.Id.Equals(als.Id));
                    c.AttackModifications?.RemoveAll(a => a.Id.Equals(als.Id));
                    c.DefenseModifications?.RemoveAll(a => a.Id.Equals(als.Id));
                    c.MovementModifications?.RemoveAll(a => a.Id.Equals(als.Id));
                    c.HPRegenerationModifications?.RemoveAll(a => a.Id.Equals(als.Id));
                });
                c.AlteredStatuses.Clear();
                return true;
            }
            return false;
        }

        public static bool ForceSkipTurn(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            _ = 0;
            if (Target == null) return false;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (paramsObject.Target is not Character c) throw new ArgumentException($"Attempted to force {paramsObject.Target.Name} to skip its turn when it's not a Character.");
            if (Rng.NextInclusive(1, 100) <= paramsObject.Chance && c.CanTakeAction)
            {
                c.CanTakeAction = false;
                return true;
            }
            return false;
        }

        public static bool Teleport(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            _ = 0;
            if (Target == null) return false;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (paramsObject.Target is not Character c) throw new ArgumentException($"Attempted to teleport {paramsObject.Target.Name} when it's not a Character.");
            if ((c.ContainingTile.Type == TileType.Floor || c.ContainingTile.Type == TileType.Stairs) && Rng.NextInclusive(1, 100) <= paramsObject.Chance)
            {
                if (c.EntityType == EntityType.Player
                    || (c.EntityType == EntityType.NPC && Map.Player.CanSee(c)))
                {
                    Map.AppendMessage(Map.Locale["CharacterWasTeleported"].Format(new { CharacterName = c.Name }), Color.DeepSkyBlue);
                }
                c.Position = Map.PickEmptyPosition(true);
                return true;
            }
            return false;
        }

        public static bool GenerateStairs(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            _ = 0;
            if (!Map.StairsAreSet)
            {
                Map.SetStairs();
                Map.AppendMessage(Map.Locale["StairsGotRevealed"], Color.Lime);
                return true;
            }
            return false;
        }

        public static bool CheckCondition(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            _ = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            return new Expression(paramsObject.Condition).Eval<bool>();
        }

        public static bool SetFlag(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            _ = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            var existingFlag = Map.Flags.Find(f => f.Key.Equals(paramsObject.Key));
            if(existingFlag != null)
                Map.SetFlagValue(paramsObject.Key, paramsObject.Value);
            else
                Map.CreateFlag(paramsObject.Key, paramsObject.Value, paramsObject.RemoveOnFloorChange);
            return true;
        }

        public static bool ReplenishMP(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int output, params (string ParamName, string Value)[] args)
        {
            output = 0;
            if (Target == null) return false;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (paramsObject.Target is not Character) throw new ArgumentException($"Attempted to recover {paramsObject.Target.Name}'s MP when it's not a Character.");
            if (!paramsObject.Target.UsesMP) return false;
            if (paramsObject.Target.MP >= paramsObject.Target.MaxMP)
                return false;
            var replenishAmount = Math.Min(paramsObject.Target.MaxMP - paramsObject.Target.MP, paramsObject.Power);
            if (paramsObject.Power > 0 && paramsObject.Power < 1)
                replenishAmount = 1;
            replenishAmount = (int)replenishAmount;
            output = (int)replenishAmount;
            paramsObject.Target.MP = Math.Min(paramsObject.Target.MaxMP, paramsObject.Target.MP + replenishAmount);

            if (paramsObject.Target.EntityType == EntityType.Player
                || (paramsObject.Target.EntityType == EntityType.NPC && Map.Player.CanSee(paramsObject.Target)))
            {
                if (paramsObject.Target.MP == paramsObject.Target.MaxMP)
                    Map.AppendMessage(Map.Locale["CharacterRecoversAllMP"].Format(new { CharacterName = paramsObject.Target.Name, CharacterMPStat = Map.Locale["CharacterMPStat"] }), Color.DeepSkyBlue);
                else
                    Map.AppendMessage(Map.Locale["CharacterRecoversSomeMP"].Format(new { CharacterName = paramsObject.Target.Name, ReplenishAmount = replenishAmount.ToString(), CharacterMPStat = Map.Locale["CharacterMPStat"] }), Color.DeepSkyBlue);
            }

            if (paramsObject.Target.EntityType == EntityType.Player)
                Map.PlayerGotMPReplenished = true;
            return true;
        }
    }
    #pragma warning restore S2259 // Null pointers should not be dereferenced
    #pragma warning restore S2589 // Boolean expressions should not be gratuitous
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
