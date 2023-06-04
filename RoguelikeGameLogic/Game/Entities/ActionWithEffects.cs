using RoguelikeGameEngine.Utils.Helpers;
using RoguelikeGameEngine.Utils.JsonImports;
using System.Reflection;
using System.Text;
using RoguelikeGameEngine.Utils.Representation;
using RoguelikeGameEngine.Game.DungeonStructure;
using RoguelikeGameEngine.Utils.Enums;
using System.Collections.Generic;
using System;
using System.Linq;

namespace RoguelikeGameEngine.Game.Entities
{
    public class ActionWithEffects
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Entity User { get; set; }

        public Map Map { get; set; }

        #region Exclusive use for Characters
        public int MinimumRange { get; set; }
        public int MaximumRange { get; set; }
        public int CooldownBetweenUses { get; set; }
        public int StartingCooldown { get; set; }
        public int CurrentCooldown { get; set; }
        public int MaximumUses { get; set; }
        public int CurrentUses { get; set; }
        public List<TargetType> TargetTypes { get; set; }

        public bool CanBeUsed => (MaximumUses == 0 || CurrentUses < MaximumUses) && (CooldownBetweenUses == 0 || CurrentCooldown == 0);
        #endregion

        private Effect Effect { get; set; }                       // What is going to be executed when the action is called

        private Locale Locale => Map.Locale;

        private ActionWithEffects() { }

        public ActionWithEffects(ActionWithEffectsInfo info)
        {
            Name = info.Name;
            Description = info.Description;
            MinimumRange = info.MinimumRange;
            MaximumRange = info.MaximumRange;
            CooldownBetweenUses = info.CooldownBetweenUses;
            StartingCooldown = info.StartingCooldown;
            CurrentCooldown = info.StartingCooldown;
            MaximumUses = info.MaximumUses;
            CurrentUses = 0;
            TargetTypes = new List<TargetType>();
            info.TargetTypes?.ForEach(tt => TargetTypes.Add(Enum.Parse<TargetType>(tt, true)));
            Effect = new Effect(info.Effect);
        }

        public void Do(Entity source, Entity target)
        {
            Effect.Do(User, source, target);

            if (CooldownBetweenUses > 0) CurrentCooldown = CooldownBetweenUses;
            if (MaximumUses > 0) CurrentUses++;
            source.Visible = true;
        }

        public bool CanBeUsedOn(Character target, Map map)
        {
            if (target == null && !TargetTypes.Any(tt => tt == TargetType.Room || tt == TargetType.Floor)) return false;

            var character = User is Character ? User as Character : User is Item i ? i.Owner : null;

            if (character == null) return false;
            if (!CanBeUsed) return false;
            if (target.ExistenceStatus != EntityExistenceStatus.Alive) return false;
            if (!character.CanSee(target)) return false;
            if (!TargetTypes.Contains(character.CalculateTargetTypeFor(target))) return false;
            if (!((int)Point.Distance(target.Position, character.Position)).Between(MinimumRange, MaximumRange)) return false;

            return character.ContainingRoom == target.ContainingRoom ||
                (character.ContainingTile.Type == TileType.Hallway &&
                    map.Tiles.GetElementsWithinDistanceWhere(character.Position.Y, character.Position.X, MaximumRange, true, t => t.IsWalkable)
                        .Any(t => t.Position.Equals(target.Position)));
        }

        public string GetDescriptionWithUsageNotes(Character target)
        {
            var descriptionWithUsageNotes = new StringBuilder(Map.Locale[Description]);
            var character = User is Character ? User as Character : User is Item i ? i.Owner : null;
            var cannotBeUsedString = Locale["CannotBeUsed"];

            if (character == null) return "";

            var distance = target != null ? (int)Point.Distance(target.Position, character.Position) : -1;

            if (CurrentCooldown > 0)
            {
                if (!descriptionWithUsageNotes.ToString().Contains(cannotBeUsedString))
                    descriptionWithUsageNotes.AppendLine($"\n\n{cannotBeUsedString}\n");
                descriptionWithUsageNotes.AppendLine(Locale["OnCooldown"].Format(new { CurrentCooldown = CurrentCooldown.ToString() }));
            }

            if (MaximumUses > 0)
            {
                if (CurrentUses == MaximumUses)
                {
                    if (!descriptionWithUsageNotes.ToString().Contains(cannotBeUsedString))
                        descriptionWithUsageNotes.AppendLine($"\n\n{cannotBeUsedString}\n");
                    descriptionWithUsageNotes.AppendLine(Locale["MaximumUses"]);
                }
                else
                {
                    var remainingUses = MaximumUses - CurrentUses;
                    descriptionWithUsageNotes.AppendLine($"\n\n{Locale["RemainingUseCount"].Format(new { RemainingUses = remainingUses.ToString() })}");
                }
            }

            if (target != null)
            {
                if (distance < MinimumRange)
                {
                    if (!descriptionWithUsageNotes.ToString().Contains(cannotBeUsedString))
                        descriptionWithUsageNotes.AppendLine($"\n\n{cannotBeUsedString}\n");
                    descriptionWithUsageNotes.AppendLine(Locale["TargetIsTooClose"]);
                }
                else if (distance > MaximumRange)
                {
                    if (!descriptionWithUsageNotes.ToString().Contains(cannotBeUsedString))
                        descriptionWithUsageNotes.AppendLine($"\n\n{cannotBeUsedString}\n");
                    descriptionWithUsageNotes.AppendLine(Locale["TargetIsTooFarAway"]);
                }

                var targetType = character.CalculateTargetTypeFor(target);
                if (!TargetTypes.Contains(targetType))
                {
                    if (!descriptionWithUsageNotes.ToString().Contains(cannotBeUsedString))
                        descriptionWithUsageNotes.AppendLine($"\n\n{cannotBeUsedString}\n");
                    var usableTargetTypes = TargetTypes.Select(tt => Locale[$"TargetType{tt}"]);
                    var usableTargetTypesString = string.Join('/', usableTargetTypes);
                    descriptionWithUsageNotes.AppendLine(Locale["TargetIsOfWrongFaction"].Format(new { FactionTargets = usableTargetTypesString }));
                }
            }
            else
            {
                if (!descriptionWithUsageNotes.ToString().Contains(cannotBeUsedString))
                    descriptionWithUsageNotes.AppendLine($"\n\n{cannotBeUsedString}\n");
                descriptionWithUsageNotes.AppendLine(Locale["RequiresATarget"]);
            }

            if (User != null)
            {
                if (!descriptionWithUsageNotes.ToString().Contains(cannotBeUsedString))
                    descriptionWithUsageNotes.AppendLine();
                descriptionWithUsageNotes.AppendLine($"\n{Locale["FromSource"].Format(new { SourceName = User.Name })}");
            }

            return descriptionWithUsageNotes.ToString();
        }

        public ActionWithEffects Clone()
        {
            return new ActionWithEffects
            {
                Name = Name,
                Description = Description,
                MinimumRange = MinimumRange,
                MaximumRange = MaximumRange,
                CooldownBetweenUses = CooldownBetweenUses,
                StartingCooldown = StartingCooldown,
                CurrentCooldown = StartingCooldown,
                MaximumUses = MaximumUses,
                CurrentUses = 0,
                Effect = Effect.Clone(),
                TargetTypes = new List<TargetType>(TargetTypes)
            };
        }

    }

    public class Effect
    {
        private static readonly List<Type> EffectMethodTypes = ReflectionHelpers.GetTypesInNamespace(Assembly.GetExecutingAssembly(), "RoguelikeGameEngine.Utils.Effects");

        public delegate bool ActionMethod(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int output, params (string ParamName, string Value)[] args);

        public ActionMethod Function { get; set; }

        public (string ParamName, string Value)[] Params { get; set; }
        private Effect Then { get; set; }                     // "Then" is always executed, regardless of success or failure
        private Effect OnSuccess { get; set; }                // "OnSuccess" is executed only if the last effect was successful
        private Effect OnFailure { get; set; }                // "OnFailure" is executed only if the last effect was not successful

        /* 
         * IMPORTANT: The game will expect only a Then, or only an OnSuccess and/or an OnFailure
         * 
         * If both are present, the game will only call the Then.
        */

        private Effect() { }

        public Effect(EffectInfo info)
        {
            MethodInfo? effectMethod = null;
            foreach (var et in EffectMethodTypes)
            {
                effectMethod = et.GetMethod(info.EffectName, BindingFlags.Static | BindingFlags.Public);
                if (effectMethod != null) break;
            }
            if (effectMethod != null)
            {
                try
                {
                    Function = effectMethod.CreateDelegate<ActionMethod>();
                    Params = info.Params.Select(p => (p.ParamName, p.Value)).ToArray();
                    if (info.Then != null)
                        Then = new Effect(info.Then);
                    if (info.OnSuccess != null)
                        OnSuccess = new Effect(info.OnSuccess);
                    if (info.OnFailure != null)
                        OnFailure = new Effect(info.OnFailure);
                }
                catch { }
            }
        }

        public void Do(Entity This, Entity Source, Entity Target)
        {
            var currentEffect = this;

            do
            {
                int output = 0;
                var success = currentEffect.Function(This, Source, Target, output, out output, currentEffect.Params);
                if (currentEffect.Then != null)
                {
                    currentEffect = currentEffect.Then;
                }
                else
                {
                    if (success)
                        currentEffect = currentEffect.OnSuccess;
                    else
                        currentEffect = currentEffect.OnFailure;
                }
            }
            while (currentEffect != null);
        }

        public Effect Clone()
        {
            return new Effect
            {
                Function = Function,
                Params = Params,
                Then = Then?.Clone(),
                OnSuccess = OnSuccess?.Clone(),
                OnFailure = OnFailure?.Clone()
            };
        }
    }
}
