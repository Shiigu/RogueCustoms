using RoguelikeGameEngine.Game.Entities;
using RoguelikeGameEngine.Game.DungeonStructure;
using RoguelikeGameEngine.Utils.Helpers;

namespace RoguelikeGameEngine.Utils.Effects
{
    // Represents Actions that are not expected to be used by any type of Entity in particular. Free to use by everyone.
    public static class GenericActions
    {
        public static Random Rng;
        public static Map Map;

        public static bool PrintText(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            _ = 0;
            var entityTypesForVisibilityCheck = new List<EntityType> { EntityType.Player, EntityType.NPC };

            if (entityTypesForVisibilityCheck.Contains(Source.EntityType) && Map.Player.CanSee(Source)
                || entityTypesForVisibilityCheck.Contains(Target.EntityType) && Map.Player.CanSee(Target))
            {
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

        public static bool GiveExperience(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int output, params (string ParamName, string Value)[] args)
        {
            output = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (paramsObject.Target is not Character || !paramsObject.Target.CanGainExperience) return false;
            if (paramsObject.Target.Level == paramsObject.Target.MaxLevel) return false;
            if (Target.EntityType == EntityType.Player
                || (Target.EntityType == EntityType.NPC && Map.Player.CanSee(Target)))
            {
                Map.AppendMessage($"{paramsObject.Target.Name} gained {(int) paramsObject.Amount} experience points.");
                paramsObject.Target.GainExperience((int) paramsObject.Amount);
                output = (int) paramsObject.Amount;
            }
            return true;
        }

        public static bool ApplyAlteredStatus(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            if (Target is not Character) throw new Exception($"Attempted to apply an Altered Status on {Target.Name} when it's not a Character.");
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            var statusToApply = Map.PossibleStatuses.Find(ps => string.Equals(ps.ClassId, paramsObject.Id, StringComparison.InvariantCultureIgnoreCase))
                                   ?? throw new Exception($"Altered status {paramsObject.Id} does not exist!");
            _ = 0;
            if (Rng.NextInclusive(1, 100) <= paramsObject.Chance)
            {
                var statusTarget = paramsObject.Target as Character;
                var success = statusToApply.ApplyTo(statusTarget, (int) paramsObject.Power, (int) paramsObject.TurnLength);
                if (success && statusTarget.EntityType == EntityType.Player
                        || (statusTarget.EntityType == EntityType.NPC && Map.Player.CanSee(statusTarget)))
                {
                    Map.AppendMessage($"{statusTarget.Name} is now {statusToApply.Name}!");
                }
                return success;
            }
            return false;
        }

        public static bool ApplyStatAlteration(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int output, params (string ParamName, string Value)[] args)
        {
            if (Target is not Character) throw new Exception($"Attempted to alter one of {Target.Name}'s stats when it's not a Character.");
            output = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            var statAlterations = (paramsObject.StatAlterationList) as List<StatModification>;
            if ((paramsObject.Amount != 0 && (paramsObject.CanBeStacked || !statAlterations.Any(sa => sa.RemainingTurns > 0 && sa.Id.Equals(paramsObject.Id)))) && Rng.NextInclusive(1, 100) <= paramsObject.Chance)
            {
                statAlterations.Add(new StatModification {
                    Id = paramsObject.Id,
                    Amount = paramsObject.Amount,
                    RemainingTurns = (int) paramsObject.TurnLength
                });
                var verb = paramsObject.Amount > 0 ? "increased" : "decreased";
                output = (int) paramsObject.Amount;
                var statName = string.Equals(paramsObject.StatName, "hpregeneration", StringComparison.InvariantCultureIgnoreCase) ? "HP Regeneration" : paramsObject.StatName;
                if (paramsObject.Target.EntityType == EntityType.Player
                    || (paramsObject.Target.EntityType == EntityType.NPC && Map.Player.CanSee(paramsObject.Target)))
                {
                    Map.AppendMessage($"{paramsObject.Target.Name}'s {statName} has {verb} by {Math.Abs(paramsObject.Amount)}!");
                }
                return true;
            }
            return false;
        }

        public static bool CleanseAlteredStatus(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            if (Target is not Character c) throw new Exception($"Attempted to remove an Altered Status on {Target.Name} when it's not a Character.");
            _ = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            var statusToRemove = Map.PossibleStatuses.Find(ps => string.Equals(ps.ClassId, paramsObject.Id, StringComparison.InvariantCultureIgnoreCase));
            if (!statusToRemove.CleansedByCleanseActions) throw new Exception($"Attempted to remove {statusToRemove.Name} with a Cleanse action when it can't be cleansed that way.");
            if (c.AlteredStatuses.Any(als => als.ClassId.Equals(statusToRemove.ClassId)) && Rng.NextInclusive(1, 100) <= paramsObject.Chance)
            {
                c.AlteredStatuses.RemoveAll(als => als.ClassId.Equals(statusToRemove.ClassId));
                c.MaxHPModifications?.RemoveAll(a => a.Id.Equals(statusToRemove.Name));
                c.AttackModifications?.RemoveAll(a => a.Id.Equals(statusToRemove.Name));
                c.DefenseModifications?.RemoveAll(a => a.Id.Equals(statusToRemove.Name));
                c.MovementModifications?.RemoveAll(a => a.Id.Equals(statusToRemove.Name));
                c.HPRegenerationModifications?.RemoveAll(a => a.Id.Equals(statusToRemove.Name));
                if (c.EntityType == EntityType.Player
                    || (c.EntityType == EntityType.NPC && Map.Player.CanSee(paramsObject.Target)))
                {
                    Map.AppendMessage($"{c.Name} is no longer {statusToRemove.Name}!");
                }
                return true;
            }
            return false;
        }

        public static bool CleanseStatAlteration(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            if (Target is not Character) throw new Exception($"Attempted to alter one of {Target.Name}'s stats when it's not a Character.");
            _ = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            var statAlterations = (paramsObject.StatAlterationList) as List<StatModification>;

            if (statAlterations?.Any() == true && Rng.NextInclusive(1, 100) <= paramsObject.Chance)
            {
                statAlterations.Clear();

                if (Target.EntityType == EntityType.Player
                    || (Target.EntityType == EntityType.NPC && Map.Player.CanSee(Target)))
                {
                    Map.AppendMessage($"{Target.Name}'s {paramsObject.StatName} returned to normal.");
                }
                return true;
            }

            return false;
        }

        public static bool CleanseStatAlterations(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            if (Target is not Character c) throw new Exception($"Attempted to alter one of {Target.Name}'s stats when it's not a Character.");
            _ = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);

            if ((c.MaxHPModifications?.Any() == true || c.AttackModifications?.Any() == true
                || c.DefenseModifications?.Any() == true || c.MovementModifications?.Any() == true || c.HPRegenerationModifications?.Any() == true) && Rng.NextInclusive(1, 100) <= paramsObject.Chance)
            {
                c.MaxHPModifications.Clear();
                c.AttackModifications.Clear();
                c.DefenseModifications.Clear();
                c.MovementModifications.Clear();
                c.HPRegenerationModifications.Clear();

                return true;
            }

            return false;
        }

        public static bool CleanseAllAlteredStatuses(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int _, params (string ParamName, string Value)[] args)
        {
            if (Target is not Character c) throw new Exception($"Attempted to remove an Altered Status on {Target.Name} when it's not a Character.");
            _ = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);
            if (Rng.NextInclusive(1, 100) <= paramsObject.Chance && c.AlteredStatuses?.Any() == true)
            {
                c.AlteredStatuses.ForEach(als => {
                    if (c.EntityType == EntityType.Player
                        || (c.EntityType == EntityType.NPC && Map.Player.CanSee(c)))
                    {
                        Map.AppendMessage($"{c.Name} is no longer {als.Name}!");
                    }
                    c.MaxHPModifications?.RemoveAll(a => a.Id.Equals(als.Id));
                    c.AttackModifications?.RemoveAll(a => a.Id.Equals(als.Id));
                    c.DefenseModifications?.RemoveAll(a => a.Id.Equals(als.Id));
                    c.MovementModifications?.RemoveAll(a => a.Id.Equals(als.Id));
                });
                c.AlteredStatuses.Clear();
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
                Map.AppendMessage($"The stairs have been revealed!");
                return true;
            }
            return false;
        }
    }
}
