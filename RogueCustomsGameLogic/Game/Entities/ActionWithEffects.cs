﻿using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.JsonImports;
using System.Reflection;
using System.Text;
using RogueCustomsGameEngine.Utils.Representation;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Enums;
using System.Collections.Generic;
using System;
using System.Linq;
using org.matheval;

namespace RogueCustomsGameEngine.Game.Entities
{
    public class ActionWithEffects
    {
        public string NameLocaleKey { get; set; }
        public string Name { get; set; }
        public string DescriptionLocaleKey { get; set; }
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
        public int MPCost { get; set; }

        public string UseCondition { get; set; }

        public bool MayBeUsed => (MaximumUses == 0 || CurrentUses < MaximumUses) && (CooldownBetweenUses == 0 || CurrentCooldown == 0);
        #endregion

        public Effect Effect { get; set; }                       // What is going to be executed when the action is called

        private Locale Locale => Map.Locale;

        private ActionWithEffects() { }

        public ActionWithEffects(ActionWithEffectsInfo info)
        {
            Name = info.Name;
            NameLocaleKey = info.Name;
            Description = info.Description;
            DescriptionLocaleKey = info.Description;
            MinimumRange = info.MinimumRange;
            MaximumRange = info.MaximumRange;
            CooldownBetweenUses = info.CooldownBetweenUses;
            StartingCooldown = info.StartingCooldown;
            CurrentCooldown = info.StartingCooldown;
            MaximumUses = info.MaximumUses;
            CurrentUses = 0;
            UseCondition = info.UseCondition;
            TargetTypes = new List<TargetType>();
            MPCost = info.MPCost;
            info.TargetTypes?.ForEach(tt => TargetTypes.Add(Enum.Parse<TargetType>(tt, true)));
            Effect = new Effect(info.Effect);
        }

        public List<string> Do(Entity source, Entity target)
        {
            var successfulEffects = Effect.Do(User, source, target);

            if (CooldownBetweenUses > 0) CurrentCooldown = CooldownBetweenUses;
            if (MaximumUses > 0) CurrentUses++;
            source.Visible = true;

            return successfulEffects;
        }

        public bool CanBeUsedOn(Character target, Map map)
        {
            if (target == null && !TargetTypes.Any(tt => tt == TargetType.Room || tt == TargetType.Floor)) return false;

            Character character = null;

            if (User is Character c)
            {
                character = c;
            }
            else if (User is Item i)
            {
                if (i.Owner != null)
                    character = i.Owner;
                else if (i.Position != null && i.ContainingTile.Character != null)
                    character = i.ContainingTile.Character;
            }

            if (character == null) return false;
            if (!MayBeUsed) return false;
            if (target.ExistenceStatus != EntityExistenceStatus.Alive) return false;
            if (!character.CanSee(target)) return false;
            if (TargetTypes.Any() && !TargetTypes.Contains(character.CalculateTargetTypeFor(target))) return false;
            if (!((int)Point.Distance(target.Position, character.Position)).Between(MinimumRange, MaximumRange)) return false;
            if (character.MP < MPCost || (character.MaxMP == 0 && MPCost > 0) || (!character.UsesMP && MPCost > 0)) return false;

            if (!string.IsNullOrWhiteSpace(UseCondition))
            {
                var parsedCondition = ActionHelpers.ParseArgForExpression(UseCondition, User, character, target);

                if (!ActionHelpers.CalculateBooleanExpression(parsedCondition)) return false;
            }

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

            descriptionWithUsageNotes.AppendLine();

            if (MaximumRange == 0)
            {
                descriptionWithUsageNotes.Append($"\n{Locale["SelfRange"]}");
            }
            else if (MaximumRange == 1)
            {
                if (MinimumRange == 1)
                    descriptionWithUsageNotes.Append($"\n{Locale["MeleeRange"]}");
                else if (MinimumRange == 0)
                    descriptionWithUsageNotes.Append($"\n{Locale["SelfOrMeleeRange"]}");
            }
            else if (MaximumRange > 1)
            {
                var tilesRange = (MinimumRange != MaximumRange) ? $"{MinimumRange}-{MaximumRange}" : $"{MaximumRange}";
                if (MinimumRange > 0)
                    descriptionWithUsageNotes.Append($"\n{Locale["MeleeRange"].Format(new { TilesRange = tilesRange })}");
                else if (MinimumRange == 0)
                    descriptionWithUsageNotes.Append($"\n{Locale["SelfOrMeleeRange"].Format(new { TilesRange = tilesRange })}");
            }

            if (MPCost > 0)
            {
                descriptionWithUsageNotes.Append($"\n{Locale["MPCost"].Format(new { MPStat = Map.Locale["CharacterMPStat"], MPCost = MPCost })}");
            }

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

                if(!string.IsNullOrWhiteSpace(UseCondition))
                {
                    var parsedCondition = ActionHelpers.ParseArgForExpression(UseCondition, User, character, target);

                    if (!ActionHelpers.CalculateBooleanExpression(parsedCondition))
                    {
                        if (!descriptionWithUsageNotes.ToString().Contains(cannotBeUsedString))
                            descriptionWithUsageNotes.AppendLine($"\n\n{cannotBeUsedString}\n");
                        descriptionWithUsageNotes.AppendLine(Locale["TargetDoesNotFulfillConditions"]);
                    }
                }

                if (character.MP < MPCost || (character.MaxMP == 0 && MPCost > 0) || (!character.UsesMP && MPCost > 0))
                {
                    if (!descriptionWithUsageNotes.ToString().Contains(cannotBeUsedString))
                        descriptionWithUsageNotes.AppendLine($"\n\n{cannotBeUsedString}\n");
                    descriptionWithUsageNotes.AppendLine(Locale["NotEnoughMP"].Format(new { MPStat = Map.Locale["CharacterMPStat"].ToUpperInvariant() }));
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
                NameLocaleKey = NameLocaleKey,
                Description = Description,
                DescriptionLocaleKey = DescriptionLocaleKey,
                MinimumRange = MinimumRange,
                MaximumRange = MaximumRange,
                CooldownBetweenUses = CooldownBetweenUses,
                StartingCooldown = StartingCooldown,
                CurrentCooldown = StartingCooldown,
                MaximumUses = MaximumUses,
                CurrentUses = 0,
                Effect = Effect.Clone(),
                UseCondition = UseCondition,
                TargetTypes = new List<TargetType>(TargetTypes),
                MPCost = MPCost
            };
        }

    }

    public class Effect
    {
        private static readonly List<Type> EffectMethodTypes = ReflectionHelpers.GetTypesInNamespace(Assembly.GetExecutingAssembly(), "RogueCustomsGameEngine.Utils.Effects");

        public delegate bool ActionMethod(Entity This, Entity Source, Entity Target, int previousEffectOutput, out int output, params (string ParamName, string Value)[] args);

        public ActionMethod Function { get; set; }

        public (string ParamName, string Value)[] Params { get; set; }
        public Effect Then { get; set; }                     // "Then" is always executed, regardless of success or failure
        public Effect OnSuccess { get; set; }                // "OnSuccess" is executed only if the last effect was successful
        public Effect OnFailure { get; set; }                // "OnFailure" is executed only if the last effect was not successful

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

        public List<string> Do(Entity This, Entity Source, Entity Target)
        {
            var currentEffect = this;
            var successfulEffects = new List<string>();

            do
            {
                int output = 0;
                var success = currentEffect.Function(This, Source, Target, output, out output, currentEffect.Params);
                if (success)
                    successfulEffects.Add(currentEffect.Function.Method.Name);
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
            return successfulEffects.Distinct().ToList();
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
