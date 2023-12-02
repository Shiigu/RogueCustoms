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
using MathNet.Numerics.Statistics.Mcmc;
using RogueCustomsGameEngine.Game.Entities.Interfaces;

namespace RogueCustomsGameEngine.Utils.Effects
{
    #pragma warning disable S2259 // Null Pointers should not be dereferenced
    #pragma warning disable S2589 // Boolean expressions should not be gratuitous
    #pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
    #pragma warning disable CS8604 // Posible argumento de referencia nulo
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

        public static bool PrintText(Entity This, Entity Source, ITargetable Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            _ = 0;

            var ignoreVisibilityChecks = ExpandoObjectHelper.HasProperty(paramsObject, "BypassesVisibilityCheck") && paramsObject.BypassesVisibilityCheck;
            var entityTypesForVisibilityCheck = new List<EntityType> { EntityType.Player, EntityType.NPC };
            var isSourceVisible = entityTypesForVisibilityCheck.Contains(Source.EntityType) && Map.Player.CanSee(Source);
            var isTargetVisible = Target is Character t && entityTypesForVisibilityCheck.Contains(t.EntityType) && Map.Player.CanSee(t);

            if (ignoreVisibilityChecks || isSourceVisible || isTargetVisible)
            {
                if (ExpandoObjectHelper.HasProperty(paramsObject, "Color"))
                    Map.AppendMessage(paramsObject.Text, paramsObject.Color);
                else
                    Map.AppendMessage(paramsObject.Text);
            }

            return true;
        }

        public static bool MessageBox(Entity This, Entity Source, ITargetable Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            _ = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            Map.AddMessageBox(paramsObject.Title, paramsObject.Text, "OK", paramsObject.Color);
            return true;
        }

        public static bool HealDamage(Entity This, Entity Source, ITargetable Target, int previousEffectOutput, out int output, params (string ParamName, string Value)[] args)
        {
            output = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (paramsObject.Target is not Character t)
                // Attempted to heal Target when it's not a Character.
                return false;
            if (t.HP >= t.MaxHP)
                return false;
            var healAmount = Math.Min(t.MaxHP - t.HP, paramsObject.Power);
            if (paramsObject.Power > 0 && paramsObject.Power < 1)
                healAmount = 1;
            healAmount = (int)healAmount;
            output = (int)healAmount;
            t.HP = Math.Min(t.MaxHP, t.HP + healAmount);

            if (t.EntityType == EntityType.Player
                || (t.EntityType == EntityType.NPC && Map.Player.CanSee(t)))
            {
                if (t.HP == t.MaxHP)
                    Map.AppendMessage(Map.Locale["CharacterHealsAllHP"].Format(new { CharacterName = t.Name }), Color.DeepSkyBlue);
                else
                    Map.AppendMessage(Map.Locale["CharacterHealsSomeHP"].Format(new { CharacterName = t.Name, HealAmount = healAmount.ToString(), CharacterHPStat = Map.Locale["CharacterHPStat"] }), Color.DeepSkyBlue);
            }

            if (t.EntityType == EntityType.Player)
                Map.PlayerGotHealed = true;
            return true;
        }

        public static bool GiveExperience(Entity This, Entity Source, ITargetable Target, int previousEffectOutput, out int output, params (string ParamName, string Value)[] args)
        {
            output = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (paramsObject.Target is not Character t || !t.CanGainExperience) return false;
            if (paramsObject.Target.Level == paramsObject.Target.MaxLevel) return false;
            if (t.EntityType == EntityType.Player
                || (t.EntityType == EntityType.NPC && Map.Player.CanSee(t)))
            {
                Map.AppendMessage(Map.Locale["CharacterGainsExperience"].Format(new { CharacterName = paramsObject.Target.Name, Amount = ((int)paramsObject.Amount).ToString() }), Color.DeepSkyBlue);
                paramsObject.Target.GainExperience((int) paramsObject.Amount);
                output = (int) paramsObject.Amount;
            }
            return true;
        }

        public static bool ApplyAlteredStatus(Entity This, Entity Source, ITargetable Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            _ = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (paramsObject.Target is not Character)
                // Attempted to apply an Altered Status on Target when it's not a Character.
                return false;
            var statusToApply = Map.PossibleStatuses.Find(ps => string.Equals(ps.ClassId, paramsObject.Id, StringComparison.InvariantCultureIgnoreCase))
                                   ?? throw new ArgumentException($"Altered status {paramsObject.Id} does not exist!");
            var statusTarget = paramsObject.Target as Character;
            var accuracyCheck = ActionHelpers.CalculateAdjustedAccuracy(Source, paramsObject.Target, paramsObject);

            if (statusTarget.ExistenceStatus == EntityExistenceStatus.Alive && Rng.NextInclusive(1, 100) <= accuracyCheck)
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
                if (statusTarget == Map.Player)
                    Map.PlayerGotStatusChange = true;
                return success;
            }
            return false;
        }

        public static bool ApplyStatAlteration(Entity This, Entity Source, ITargetable Target, int previousEffectOutput, out int output, params (string ParamName, string Value)[] args)
        {
            output = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (paramsObject.Target is not Character)
                // Attempted to alter one of Target's stats when it's not a Character.
                return false;
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
                case "accuracy":
                    statValue = statAlterationTarget.Accuracy;
                    statCap = Constants.MAX_ACCURACY_CAP;
                    break;
                case "evasion":
                    statValue = statAlterationTarget.Evasion;
                    statCap = Constants.MAX_EVASION_CAP;
                    break;
                default:
                    throw new ArgumentException($"Unrecognized stat: {paramsObject.StatName}.");
            }
            var isEvasion = string.Equals(paramsObject.StatName, "evasion", StringComparison.InvariantCultureIgnoreCase);
            if (statValue >= statCap || (isEvasion && (Math.Abs(statValue) >= statCap)))
                return false;
            var accuracyCheck = ActionHelpers.CalculateAdjustedAccuracy(Source, paramsObject.Target, paramsObject);

            if (statAlterationTarget.ExistenceStatus == EntityExistenceStatus.Alive && (paramsObject.Amount != 0 && (paramsObject.CanBeStacked || !statAlterations.Exists(sa => sa.RemainingTurns > 0 && sa.Id.Equals(paramsObject.Id)))) && Rng.NextInclusive(1, 100) <= accuracyCheck)
            {
                var isHPRegeneration = string.Equals(paramsObject.StatName, "hpregeneration", StringComparison.InvariantCultureIgnoreCase);
                var isMPRegeneration = string.Equals(paramsObject.StatName, "mpregeneration", StringComparison.InvariantCultureIgnoreCase);
                var isAccuracyOrEvasion = string.Equals(paramsObject.StatName, "accuracy", StringComparison.InvariantCultureIgnoreCase)
                                       || isEvasion;
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
                    var amountString = isAccuracyOrEvasion ? $"{Math.Abs(alterationAmount)}%" : Math.Abs(alterationAmount).ToString("0.#####");
                    var messageKey = alterationAmount > 0 ? "CharacterStatGotBuffed" : "CharacterStatGotNerfed";

                    var message = Map.Locale[messageKey].Format(new
                    {
                        CharacterName = statAlterationTarget.Name,
                        StatName = Map.Locale[$"Character{statName}Stat"],
                        Amount = amountString
                    });

                    Map.AppendMessage(message, Color.DeepSkyBlue);
                }
                return true;
            }
            return false;
        }

        public static bool CleanseAlteredStatus(Entity This, Entity Source, ITargetable Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            _ = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (paramsObject.Target is not Character c)
                // Attempted to remove an Altered Status from Target when it's not a Character.
                return false;
            var statusToRemove = Map.PossibleStatuses.Find(ps => string.Equals(ps.ClassId, paramsObject.Id, StringComparison.InvariantCultureIgnoreCase));
            if (!statusToRemove.CleansedByCleanseActions) throw new InvalidOperationException($"Attempted to remove {statusToRemove.Name} with a Cleanse action when it can't be cleansed that way.");
            var accuracyCheck = ActionHelpers.CalculateAdjustedAccuracy(Source, paramsObject.Target, paramsObject);

            if (c.AlteredStatuses.Exists(als => als.ClassId.Equals(statusToRemove.ClassId)) && Rng.NextInclusive(1, 100) <= accuracyCheck)
            {
                c.AlteredStatuses.Where(als => als.ClassId.Equals(statusToRemove.ClassId)).ForEach(als => {
                    als.OnRemove?.Do(als, c, false);
                    als.FlaggedToRemove = true;
                    als.RemainingTurns = 0;
                });
                foreach (var (statName, modifications) in c.StatModifications)
                {
                    modifications.RemoveAll(a => a.Id.Equals(statusToRemove.Name));
                }
                if (c.EntityType == EntityType.Player
                    || (c.EntityType == EntityType.NPC && Map.Player.CanSee(c)))
                {
                    Map.AppendMessage(Map.Locale["CharacterIsNoLongerStatused"].Format(new { CharacterName = c.Name, StatusName = statusToRemove.Name }), Color.DeepSkyBlue);
                }
                if (c == Map.Player)
                    Map.PlayerGotStatusChange = true;
                return true;
            }
            return false;
        }

        public static bool CleanseStatAlteration(Entity This, Entity Source, ITargetable Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            _ = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (paramsObject.Target is not Character t)
                // Attempted to remove of Target's Altered Statuses when it's not a Character.
                return false;
            var statAlterations = paramsObject.StatAlterationList as List<StatModification>;
            var accuracyCheck = ActionHelpers.CalculateAdjustedAccuracy(Source, t, paramsObject);

            if (statAlterations?.Any() == true && Rng.NextInclusive(1, 100) <= accuracyCheck)
            {
                statAlterations.Clear();

                if (t.EntityType == EntityType.Player
                    || (t.EntityType == EntityType.NPC && Map.Player.CanSee(t)))
                {
                    var isHPRegeneration = string.Equals(paramsObject.StatName, "hpregeneration", StringComparison.InvariantCultureIgnoreCase);
                    var isMPRegeneration = string.Equals(paramsObject.StatName, "mpregeneration", StringComparison.InvariantCultureIgnoreCase);
                    var statName = paramsObject.StatName;
                    if (isHPRegeneration)
                        statName = "HPRegeneration";
                    else if (isMPRegeneration)
                        statName = "MPRegeneration";
                    Map.AppendMessage(Map.Locale["CharacterStatGotNeutralized"].Format(new { CharacterName = t.Name, StatName = Map.Locale[$"Character{statName}Stat"] }), Color.DeepSkyBlue);
                }
                return true;
            }

            return false;
        }

        public static bool CleanseStatAlterations(Entity This, Entity Source, ITargetable Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            _ = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (paramsObject.Target is not Character c)
                // Attempted to alter one of Target's stats when it's not a Character.
                return false;
            var accuracyCheck = ActionHelpers.CalculateAdjustedAccuracy(Source, paramsObject.Target, paramsObject);
            var success = false;
            if (Rng.NextInclusive(1, 100) <= accuracyCheck)
            {
                foreach (var (statName, modifications) in c.StatModifications)
                {
                    if (modifications.Any())
                    {
                        success = true;
                        modifications.Clear();
                        if (c == Map.Player || Map.Player.CanSee(c))
                            Map.AppendMessage(Map.Locale["CharacterStatGotNeutralized"].Format(new { CharacterName = c.Name, StatName = statName }), Color.DeepSkyBlue);
                    }
                }
            }

            return success;
        }

        public static bool CleanseAllAlteredStatuses(Entity This, Entity Source, ITargetable Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            _ = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (paramsObject.Target is not Character c)
                // Attempted to remove all of Target's Altered Statuses when it's not a Character.
                return false;
            var accuracyCheck = ActionHelpers.CalculateAdjustedAccuracy(Source, paramsObject.Target, paramsObject);
            if (Rng.NextInclusive(1, 100) <= accuracyCheck && c.AlteredStatuses?.Any() == true)
            {
                c.AlteredStatuses.ForEach(als => {
                    als.OnRemove?.Do(als, c, false);
                    als.FlaggedToRemove = true;
                    als.RemainingTurns = 0;
                    if (c.EntityType == EntityType.Player
                        || (c.EntityType == EntityType.NPC && Map.Player.CanSee(c)))
                    {
                        Map.AppendMessage(Map.Locale["CharacterIsNoLongerStatused"].Format(new { CharacterName = c.Name, StatusName = als.Name }), Color.DeepSkyBlue);
                    }

                    foreach (var (statName, modifications) in c.StatModifications)
                    {
                        modifications.RemoveAll(a => a.Id.Equals(als.Id));
                    }
                });
                if (c == Map.Player)
                    Map.PlayerGotStatusChange = true;
                return true;
            }
            return false;
        }

        public static bool ForceSkipTurn(Entity This, Entity Source, ITargetable Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            _ = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (paramsObject.Target is not Character c)
                // Attempted to skip Target's next turn when it's not a Character.
                return false;
            var accuracyCheck = ActionHelpers.CalculateAdjustedAccuracy(Source, paramsObject.Target, paramsObject);
            if (Rng.NextInclusive(1, 100) <= accuracyCheck && c.CanTakeAction)
            {
                c.CanTakeAction = false;
                return true;
            }
            return false;
        }

        public static bool Teleport(Entity This, Entity Source, ITargetable Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            _ = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (paramsObject.Target is not Character c)
                // Attempted to teleport Target when it's not a Character.
                return false;
            var accuracyCheck = ActionHelpers.CalculateAdjustedAccuracy(Source, paramsObject.Target, paramsObject);
            if ((c.ContainingTile.Type == TileType.Floor || c.ContainingTile.Type == TileType.Stairs) && Rng.NextInclusive(1, 100) <= accuracyCheck)
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

        public static bool GenerateStairs(Entity This, Entity Source, ITargetable Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
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

        public static bool CheckCondition(Entity This, Entity Source, ITargetable Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            _ = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            return new Expression(paramsObject.Condition).Eval<bool>();
        }

        public static bool SetFlag(Entity This, Entity Source, ITargetable Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
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

        public static bool ReplenishMP(Entity This, Entity Source, ITargetable Target, int previousEffectOutput, out int output, params (string ParamName, string Value)[] args)
        {
            output = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (paramsObject.Target is not Character)
                // Attempted to recover Target's MP when it's not a Character.
                return false;
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

        public static bool ToggleVisibility(Entity This, Entity Source, ITargetable Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            _ = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (paramsObject.Target is not Character c)
                // Attempted to toggle Target's Visibility when it's not a Character.
                return false;
            var accuracyCheck = ActionHelpers.CalculateAdjustedAccuracy(Source, paramsObject.Target, paramsObject);
            if (Rng.NextInclusive(1, 100) <= accuracyCheck)
            {
                c.Visible = !c.Visible;
                return true;
            }
            return false;
        }

        public static bool GiveItem(Entity This, Entity Source, ITargetable Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            _ = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);

            if (paramsObject.Target is not Character t)
                // Attempted to give Target an Item when it's not a Character.
                return false;

            t = paramsObject.Target as Character;

            if (t.Inventory.Count == t.InventorySize)
                // Attempted to give Target an Item when their inventory is full.
                return false;

            if (paramsObject.FromInventory && paramsObject.Source is not Character s)
                // Attempted to give Target an Item from Source's inventory, when Source's not a Character.
                return false;

            s = paramsObject.Source as Character;

            var itemClass = Map.PossibleItemClasses.Find(c => c.Id.Equals(paramsObject.Id));

            if (itemClass == null)
                // Must have a valid Trap class to spawn
                return false;

            if (paramsObject.FromInventory && !s.Inventory.Exists(i => i.ClassId.Equals(itemClass.Id)))
                // Attempted to give Target an Item from Source's inventory, when Source does not have such an item.
                return false;

            var accuracyCheck = ActionHelpers.CalculateAdjustedAccuracy(Source, t, paramsObject);
            if (Rng.NextInclusive(1, 100) <= accuracyCheck)
            {
                var itemToGive = paramsObject.FromInventory
                    ? s.Inventory.Find(i => i.ClassId.Equals(itemClass.Id))
                    : Map.AddEntity(paramsObject.Id) as Item;
                if (paramsObject.FromInventory)
                    s.Inventory.Remove(itemToGive);
                t.Inventory.Add(itemToGive);
                itemToGive.Owner = t;
                Map.AppendMessage(Map.Locale["CharacterGotAnItem"].Format(new { CharacterName = t.Name, SourceName = s.Name, ItemName = itemToGive.Name }), Color.DeepSkyBlue);
                return true;
            }
            return false;
        }
    }
    #pragma warning restore S2259 // Null Pointers should not be dereferenced
    #pragma warning restore S2589 // Boolean expressions should not be gratuitous
    #pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
    #pragma warning restore CS8604 // Posible argumento de referencia nulo
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
