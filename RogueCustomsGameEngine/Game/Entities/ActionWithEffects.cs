using RogueCustomsGameEngine.Utils.Helpers;
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
using System.Runtime.Serialization;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using System.Drawing;
using RogueCustomsGameEngine.Game.Entities.NPCAIStrategies;

namespace RogueCustomsGameEngine.Game.Entities
{
    #pragma warning disable IDE0037 // Usar nombre de miembro inferido
    #pragma warning disable CS8601 // Posible asignación de referencia nula
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    #pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
    #pragma warning disable CS8604 // Posible argumento de referencia nulo
    #pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
    [Serializable]
    public sealed class ActionWithEffects
    {
        public int ActionId { get; set; }                       // Dynamic, only to be used for action selections
        public string NameLocaleKey { get; set; }
        public string Name { get; set; }
        public string DescriptionLocaleKey { get; set; }
        public string Description { get; set; }
        public Entity User { get; set; }

        public Map Map { get; set; }

        #region Exclusive use for Characters or Items
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
        public bool FinishesTurnWhenUsed { get; set; }

        public bool MayBeUsed => (MaximumUses == 0 || CurrentUses < MaximumUses) && (CooldownBetweenUses == 0 || CurrentCooldown == 0);
        #endregion

        public Effect Effect { get; set; }                       // What is going to be executed when the action is called

        private Locale Locale => Map.Locale;

        private ActionWithEffects() { }

        private ActionWithEffects(ActionWithEffectsInfo info)
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
            FinishesTurnWhenUsed = info.FinishesTurnWhenUsed;
            TargetTypes = new List<TargetType>();
            MPCost = info.MPCost;
            info.TargetTypes?.ForEach(tt => TargetTypes.Add(Enum.Parse<TargetType>(tt, true)));
            Effect = new Effect(info.Effect);
        }

        public static ActionWithEffects Create(ActionWithEffectsInfo info)
        {
            return info != null && !string.IsNullOrWhiteSpace(info.Name) ? new ActionWithEffects(info) : null;
        }

        public List<string> Do(Entity source, ITargetable target, bool turnSourceVisibleWhenDone)
        {
            var successfulEffects = Effect.Do(User, source, target);

            if (CooldownBetweenUses > 0) CurrentCooldown = CooldownBetweenUses;
            if (MaximumUses > 0) CurrentUses++;

            if(turnSourceVisibleWhenDone)
                source.Visible = true;

            return successfulEffects;
        }

        public int GetActionWeightFor(ITargetable target, NonPlayableCharacter source)
        {
            var AIStrategy = NPCAIStrategyFactory.GetNPCAIStrategy(source.AIType);

            return AIStrategy.GetActionWeight(this, Map, User, source, target);
        }

        public bool CanBeUsedOn(ITargetable target, Character? source = null)
        {
            if (target == null && !TargetTypes.Contains(TargetType.Tile)) return false;

            Character sourceAsCharacter = null;
            var userAsItem = User as Item;
            var userAsKey = User as Key;

            if (source != null)
            {
                sourceAsCharacter = source;
            }
            else
            {
                if (User is Character c)
                {
                    sourceAsCharacter = c;
                }
                else if (userAsItem != null)
                {
                    if (userAsItem.Owner != null)
                        sourceAsCharacter = userAsItem.Owner;
                    else if (userAsItem.Position != null && userAsItem.ContainingTile.LivingCharacter != null)
                        sourceAsCharacter = userAsItem.ContainingTile.LivingCharacter;
                }
                else if (userAsKey != null)
                {
                    if (userAsKey.Owner != null)
                        sourceAsCharacter = userAsKey.Owner;
                    else if (userAsKey.Position != null && userAsKey.ContainingTile.LivingCharacter != null)
                        sourceAsCharacter = userAsKey.ContainingTile.LivingCharacter;
                }
            }

            if (sourceAsCharacter == null) return false;
            if (userAsKey != null && sourceAsCharacter is NonPlayableCharacter) return false;
            if (!MayBeUsed) return false;

            if (target is Character tc && !TargetTypes.Contains(TargetType.Tile))
                return CanBeUsedOnCharacter(tc, sourceAsCharacter);

            if (target is Tile tt && TargetTypes.Contains(TargetType.Tile))
                return CanBeUsedOnTile(tt, sourceAsCharacter);

            return true;
        }

        public bool CanBeUsedOnCharacter(Character target, Character source)
        {
            if (target.ExistenceStatus != EntityExistenceStatus.Alive) return false;
            if (TargetTypes.Any() && !TargetTypes.Contains(source.CalculateTargetTypeFor(target))) return false;

            var distanceFromSourceToTarget = (int)GamePoint.Distance(target.Position, source.Position);

            if (!distanceFromSourceToTarget.Between(MinimumRange, MaximumRange)) return false;
            if (source.MP < MPCost || (source.MaxMP == 0 && MPCost > 0) || (!source.UsesMP && MPCost > 0)) return false;

            if (!string.IsNullOrWhiteSpace(UseCondition))
            {
                var parsedCondition = ActionHelpers.ParseArgForExpression(UseCondition, User, source, target);

                if (!ActionHelpers.CalculateBooleanExpression(parsedCondition)) return false;
            }

            if (source.ContainingTile.Type != TileType.Hallway && source.ContainingRoom != target.ContainingRoom) return false;

            if (!source.CanSee(target)) return false;

            if (distanceFromSourceToTarget > 1 && !source.HasNoObstructionsTowards(target)) return false;

            return true;
        }

        public bool CanBeUsedOnTile(Tile target, Character source)
        {
            if (!((int)GamePoint.Distance(target.Position, source.Position)).Between(MinimumRange, MaximumRange)) return false;
            if (source.MP < MPCost || (source.MaxMP == 0 && MPCost > 0) || (!source.UsesMP && MPCost > 0)) return false;

            if(target.Type != TileType.Door && User.EntityType == EntityType.Key) return false;

            var distanceFromSourceToTarget = (int)GamePoint.Distance(target.Position, source.Position);

            if (!distanceFromSourceToTarget.Between(MinimumRange, MaximumRange)) return false;

            if (!string.IsNullOrWhiteSpace(UseCondition))
            {
                var parsedCondition = ActionHelpers.ParseArgForExpression(UseCondition, User, source, target);

                if (!ActionHelpers.CalculateBooleanExpression(parsedCondition)) return false;
            }

            if (source.ContainingTile.Type != TileType.Hallway && source.ContainingRoom != target.Room) return false;

            if (!source.FOVTiles.Contains(target)) return false;

            return true;
        }

        public string GetDescriptionWithUsageNotes(ITargetable target, Character? source = null)
        {
            var descriptionWithUsageNotes = new StringBuilder(Map.Locale[Description]);
            Character sourceAsCharacter;

            if (source != null)
            {
                sourceAsCharacter = source;
            }
            else
            {
                if (User is Character)
                {
                    sourceAsCharacter = User as Character;
                }
                else if (User is Item i)
                {
                    sourceAsCharacter = i.Owner;
                }
                else
                {
                    throw new ArgumentException("Attempted to get Usage Notes without a Target.");
                }
            }

            var cannotBeUsedString = Locale["CannotBeUsed"];

            if (sourceAsCharacter == null) return "";

            descriptionWithUsageNotes.AppendLine();

            if (MaximumRange == 0)
            {
                descriptionWithUsageNotes.Append('\n').Append(Locale["SelfRange"]);
            }
            else if (MaximumRange == 1)
            {
                if (MinimumRange == 1)
                    descriptionWithUsageNotes.Append('\n').Append(Locale["MeleeRange"]);
                else if (MinimumRange == 0)
                    descriptionWithUsageNotes.Append('\n').Append(Locale["SelfOrMeleeRange"]);
            }
            else if (MaximumRange > 1)
            {
                var tilesRange = (MinimumRange != MaximumRange) ? $"{MinimumRange}-{MaximumRange}" : $"{MaximumRange}";
                if (MinimumRange > 0)
                    descriptionWithUsageNotes.Append('\n').Append(Locale["TilesRange"].Format(new { TilesRange = tilesRange }));
                else if (MinimumRange == 0)
                    descriptionWithUsageNotes.Append('\n').Append(Locale["SelfOrTilesRange"].Format(new { TilesRange = tilesRange }));
            }

            if (MPCost > 0)
            {
                descriptionWithUsageNotes.Append("\n\n").Append(Locale["MPCost"].Format(new { MPStat = Map.Locale["CharacterMPStat"], MPCost = MPCost }));
            }

            var distance = target != null ? (int)GamePoint.Distance(target.Position, sourceAsCharacter.Position) : -1;

            if (CurrentCooldown > 0)
            {
                if (!descriptionWithUsageNotes.ToString().Contains(cannotBeUsedString))
                    descriptionWithUsageNotes.Append("\n\n").Append(cannotBeUsedString).AppendLine("\n");
                descriptionWithUsageNotes.AppendLine(Locale["OnCooldown"].Format(new { CurrentCooldown = CurrentCooldown.ToString() }));
            }

            if (MaximumUses > 0)
            {
                if (CurrentUses == MaximumUses)
                {
                    if (!descriptionWithUsageNotes.ToString().Contains(cannotBeUsedString))
                        descriptionWithUsageNotes.Append("\n\n").Append(cannotBeUsedString).AppendLine("\n");
                    descriptionWithUsageNotes.AppendLine(Locale["MaximumUses"]);
                }
                else
                {
                    var remainingUses = MaximumUses - CurrentUses;
                    descriptionWithUsageNotes.Append("\n\n").AppendLine(Locale["RemainingUseCount"].Format(new { RemainingUses = remainingUses.ToString() }));
                }
            }

            if (target != null)
            {
                if (distance < MinimumRange)
                {
                    if (!descriptionWithUsageNotes.ToString().Contains(cannotBeUsedString))
                        descriptionWithUsageNotes.Append("\n\n").Append(cannotBeUsedString).AppendLine("\n");
                    descriptionWithUsageNotes.AppendLine(Locale["TargetIsTooClose"]);
                }
                else if (distance > MaximumRange)
                {
                    if (!descriptionWithUsageNotes.ToString().Contains(cannotBeUsedString))
                        descriptionWithUsageNotes.Append("\n\n").Append(cannotBeUsedString).AppendLine("\n");
                    descriptionWithUsageNotes.AppendLine(Locale["TargetIsTooFarAway"]);
                }

                if(target is Character tc && !TargetTypes.Contains(TargetType.Tile))
                {
                    var targetType = sourceAsCharacter.CalculateTargetTypeFor(tc);
                    if (!TargetTypes.Contains(targetType))
                    {
                        if (!descriptionWithUsageNotes.ToString().Contains(cannotBeUsedString))
                            descriptionWithUsageNotes.Append("\n\n").Append(cannotBeUsedString).AppendLine("\n");
                        var usableTargetTypes = TargetTypes.Select(tt => Locale[$"TargetType{tt}"]);
                        var usableTargetTypesString = string.Join('/', usableTargetTypes);
                        descriptionWithUsageNotes.AppendLine(Locale["TargetIsOfWrongFaction"].Format(new { FactionTargets = usableTargetTypesString }));
                    }
                }

                if(!string.IsNullOrWhiteSpace(UseCondition))
                {
                    var parsedCondition = ActionHelpers.ParseArgForExpression(UseCondition, User, sourceAsCharacter, target);

                    if (!ActionHelpers.CalculateBooleanExpression(parsedCondition))
                    {
                        if (!descriptionWithUsageNotes.ToString().Contains(cannotBeUsedString))
                            descriptionWithUsageNotes.Append("\n\n").Append(cannotBeUsedString).AppendLine("\n");
                        descriptionWithUsageNotes.AppendLine(Locale["TargetDoesNotFulfillConditions"]);
                    }
                }

                if (sourceAsCharacter.MP < MPCost || (sourceAsCharacter.MaxMP == 0 && MPCost > 0) || (!sourceAsCharacter.UsesMP && MPCost > 0))
                {
                    if (!descriptionWithUsageNotes.ToString().Contains(cannotBeUsedString))
                        descriptionWithUsageNotes.Append("\n\n").Append(cannotBeUsedString).AppendLine("\n");
                    descriptionWithUsageNotes.AppendLine(Locale["NotEnoughMP"].Format(new { MPStat = Map.Locale["CharacterMPStat"].ToUpperInvariant() }));
                }
            }
            else if (!TargetTypes.Contains(TargetType.Tile))
            {
                if (!descriptionWithUsageNotes.ToString().Contains(cannotBeUsedString))
                    descriptionWithUsageNotes.Append("\n\n").Append(cannotBeUsedString).AppendLine("\n");
                descriptionWithUsageNotes.AppendLine(Locale["RequiresATarget"]);
            }

            if (User != null)
            {
                if (!descriptionWithUsageNotes.ToString().Contains(cannotBeUsedString))
                    descriptionWithUsageNotes.AppendLine();
                descriptionWithUsageNotes.Append('\n').AppendLine(Locale["FromSource"].Format(new { SourceName = User.Name }));
            }

            return descriptionWithUsageNotes.ToString();
        }

        public bool ChecksCondition(Character character, Character target)
        {
            if (!MayBeUsed) return false;
            if (!string.IsNullOrWhiteSpace(UseCondition))
            {
                var parsedCondition = ActionHelpers.ParseArgForExpression(UseCondition, User, character, target);

                if (!ActionHelpers.CalculateBooleanExpression(parsedCondition)) return false;
            }
            return true;
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
                MPCost = MPCost,
                FinishesTurnWhenUsed = FinishesTurnWhenUsed
            };
        }
    }

    [Serializable]
    public class Effect : ISerializable
    {
        private static readonly List<Type> EffectMethodTypes = ReflectionHelpers.GetTypesInNamespace(Assembly.GetExecutingAssembly(), "RogueCustomsGameEngine.Utils.Effects");

        public delegate bool ActionMethod(Entity This, Entity Source, ITargetable Target, int previousEffectOutput, out int output, params (string ParamName, string Value)[] args);

        public string EffectMethodName { get; set; }
        public string EffectClassName { get; set; }

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
                catch
                {
                    // Ignore. Don't call anything at all.
                }
            }
        }

        public List<string> Do(Entity This, Entity Source, ITargetable Target)
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

        // Explicit implementation of ISerializable
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Serialize necessary information
            EffectMethodName = Function.GetMethodInfo().Name;
            EffectClassName = Function.GetMethodInfo().DeclaringType.FullName;
            info.AddValue("EffectMethodName", EffectMethodName);
            info.AddValue("EffectClassName", EffectClassName);
            info.AddValue("Params", Params);
            info.AddValue("Then", Then);
            info.AddValue("OnSuccess", OnSuccess);
            info.AddValue("OnFailure", OnFailure);
        }

        // Explicit constructor for deserialization
        protected Effect(SerializationInfo info, StreamingContext context)
        {
            // Deserialize information
            EffectMethodName = info.GetString("EffectMethodName");
            EffectClassName = info.GetString("EffectClassName");
            Params = (ValueTuple<string, string>[])info.GetValue("Params", typeof(ValueTuple<string, string>[]));
            Then = (Effect)info.GetValue("Then", typeof(Effect));
            OnSuccess = (Effect)info.GetValue("OnSuccess", typeof(Effect));
            OnFailure = (Effect)info.GetValue("OnFailure", typeof(Effect));

            // Rebuild the delegate during deserialization
            if (!string.IsNullOrEmpty(EffectMethodName) && !string.IsNullOrEmpty(EffectClassName))
            {
                Type effectType = EffectMethodTypes.FirstOrDefault(t => t.FullName == EffectClassName);
                if (effectType != null)
                {
                    MethodInfo methodInfo = effectType.GetMethod(EffectMethodName, BindingFlags.Static | BindingFlags.Public);
                    if (methodInfo != null)
                    {
                        Function = methodInfo.CreateDelegate<ActionMethod>();
                    }
                }
            }
        }
    }
    #pragma warning restore IDE0037 // Usar nombre de miembro inferido
    #pragma warning restore CS8601 // Posible asignación de referencia nula
    #pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
    #pragma warning restore CS8604 // Posible argumento de referencia nulo
    #pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
