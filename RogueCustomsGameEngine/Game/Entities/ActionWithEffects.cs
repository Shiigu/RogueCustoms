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
using System.Globalization;
using RogueCustomsGameEngine.Utils.Expressions;
using System.Diagnostics.Contracts;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using System.Runtime.ExceptionServices;
using RogueCustomsGameEngine.Utils.Exceptions;
using RogueCustomsGameEngine.Utils.Effects.Utils;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Game.Entities
{
#pragma warning disable IDE0037 // Usar nombre de miembro inferido
#pragma warning disable CS8601 // Posible asignación de referencia nula
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
#pragma warning disable CS8604 // Posible argumento de referencia nulo
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
#pragma warning disable RCS1075 // Avoid empty catch clause that catches System.Exception
    [Serializable]
    public sealed class ActionWithEffects
    {
        public string Id { get; set; }
        public string SelectionId { get; set; }
        public bool IsScript { get; set; }
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
        public string AIUseCondition { get; set; }
        public bool FinishesTurnWhenUsed { get; set; }

        public bool MayBeUsed => (MaximumUses == 0 || CurrentUses < MaximumUses) && (CooldownBetweenUses == 0 || CurrentCooldown == 0);
        #endregion

        public Effect Effect { get; set; }                       // What is going to be executed when the action is called

        private Locale Locale => Map.Locale;

        private ActionWithEffects() { }

        private ActionWithEffects(ActionWithEffectsInfo info)
        {
            Id = info.Id;
            Name = info.Name;
            NameLocaleKey = info.Name;
            Description = info.Description;
            DescriptionLocaleKey = info.Description;
            IsScript = info.IsScript;
            MinimumRange = info.MinimumRange;
            MaximumRange = info.MaximumRange;
            CooldownBetweenUses = info.CooldownBetweenUses;
            StartingCooldown = info.StartingCooldown;
            CurrentCooldown = info.StartingCooldown;
            MaximumUses = info.MaximumUses;
            CurrentUses = 0;
            UseCondition = info.UseCondition;
            AIUseCondition = info.AIUseCondition;
            FinishesTurnWhenUsed = info.FinishesTurnWhenUsed;
            TargetTypes = new List<TargetType>();
            MPCost = info.MPCost;
            info.TargetTypes?.ForEach(tt => TargetTypes.Add(Enum.Parse<TargetType>(tt, true)));
            Effect = new Effect(info.Effect);
        }

        public static ActionWithEffects Create(ActionWithEffectsInfo info)
        {
            return info != null && !string.IsNullOrWhiteSpace(info.Id) ? new ActionWithEffects(info) : null;
        }

        public async Task<List<string>> Do(Entity source, ITargetable target, bool turnSourceVisibleWhenDone)
        {
            var successfulEffects = await Effect.Do(User, source, target);

            if (CooldownBetweenUses > 0) CurrentCooldown = CooldownBetweenUses;
            if (MaximumUses > 0) CurrentUses++;

            if (source != null && source.Position != null && turnSourceVisibleWhenDone)
            {
                var wasAlreadyVisible = source.Visible;
                source.Visible = true;
                if (!wasAlreadyVisible && !source.Map.IsDebugMode)
                {
                    source.Map.DisplayEvents.Add(($"{source.Name} got revealed after action", new()
                    {
                        new()
                        {
                            DisplayEventType = DisplayEventType.UpdateTileRepresentation,
                            Params = new() { source.Position, source.Map.GetConsoleRepresentationForCoordinates(source.Position.X, source.Position.Y) }
                        }
                    }));
                }
            }

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

            if (MPCost > 0)
            {
                if (source.MP == null || source.MP.Current < MPCost || (source.MaxMP == 0 && MPCost > 0))
                    return false;
            }

            if (!string.IsNullOrWhiteSpace(UseCondition))
            {
                var parsedCondition = ExpressionParser.ParseArgForExpression(UseCondition, User, source, target);

                if (!ExpressionParser.CalculateBooleanExpression(parsedCondition)) return false;
            }

            if (source.ContainingTile.Type != TileType.Hallway && source.ContainingRoom != target.ContainingRoom) return false;

            if (!source.CanSee(target)) return false;

            if (distanceFromSourceToTarget > 1 && !source.HasNoObstructionsTowards(target)) return false;

            return true;
        }

        public bool CanBeUsedOnTile(Tile target, Character source)
        {
            if (!((int)GamePoint.Distance(target.Position, source.Position)).Between(MinimumRange, MaximumRange)) return false;

            if (MPCost > 0)
            {
                if (source.MP == null || source.MP.Current < MPCost || (source.MaxMP == 0 && MPCost > 0))
                    return false;
            }

            if (target.Type != TileType.Door && User.EntityType == EntityType.Key) return false;

            var distanceFromSourceToTarget = (int)GamePoint.Distance(target.Position, source.Position);

            if (!distanceFromSourceToTarget.Between(MinimumRange, MaximumRange)) return false;

            if (!string.IsNullOrWhiteSpace(UseCondition))
            {
                var parsedCondition = ExpressionParser.ParseArgForExpression(UseCondition, User, source, target);

                if (!ExpressionParser.CalculateBooleanExpression(parsedCondition)) return false;
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

            var imperfectAccuracyParam = FindFirstImperfectAccuracyParameter(Effect);

            if (!string.IsNullOrWhiteSpace(imperfectAccuracyParam))
            {
                descriptionWithUsageNotes.Append($"\n{Locale["CharacterAccuracyStat"]}: {imperfectAccuracyParam:0}%");
            }

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
                    var parsedCondition = ExpressionParser.ParseArgForExpression(UseCondition, User, sourceAsCharacter, target);

                    if (!ExpressionParser.CalculateBooleanExpression(parsedCondition))
                    {
                        if (!descriptionWithUsageNotes.ToString().Contains(cannotBeUsedString))
                            descriptionWithUsageNotes.Append("\n\n").Append(cannotBeUsedString).AppendLine("\n");
                        descriptionWithUsageNotes.AppendLine(Locale["TargetDoesNotFulfillConditions"]);
                    }
                }

                if(MPCost > 0)
                {
                    if (sourceAsCharacter.MP == null || sourceAsCharacter.MP.Current < MPCost || (sourceAsCharacter.MaxMP == 0 && MPCost > 0))
                    {
                        if (!descriptionWithUsageNotes.ToString().Contains(cannotBeUsedString))
                            descriptionWithUsageNotes.Append("\n\n").Append(cannotBeUsedString).AppendLine("\n");
                        descriptionWithUsageNotes.AppendLine(Locale["NotEnoughMP"].Format(new { MPStat = Map.Locale["CharacterMPStat"].ToUpperInvariant() }));
                    }
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

        public bool HasFunction(Effect effect, string fName)
        {
            if (effect == null) return false;
            var accuracyParam = string.Empty;

            if (effect.EffectMethodName.Equals(fName, StringComparison.InvariantCultureIgnoreCase))
                return true;

            if (effect.Then != null)
                return HasFunction(effect.Then, fName);
            if (effect.OnSuccess != null || effect.OnFailure != null)
            {
                var onSuccessHasFunction = (effect.OnSuccess != null) ? HasFunction(effect.OnSuccess, fName) : false;
                var onFailureHasFunction = (effect.OnFailure != null) ? HasFunction(effect.OnSuccess, fName) : false;
                return onSuccessHasFunction || onFailureHasFunction;
            }

            return false;
        }

        private string FindFirstImperfectAccuracyParameter(Effect effect)
        {
            if (effect == null) return string.Empty;
            var accuracyParam = string.Empty;

            foreach (var param in effect.Params)
            {
                if (param.ParamName.Equals("Accuracy", StringComparison.InvariantCultureIgnoreCase) && decimal.TryParse(param.Value, out decimal paramValue))
                {
                    if(paramValue < 100)
                    {
                        accuracyParam = param.Value;
                        break;
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(accuracyParam))
                accuracyParam = FindFirstImperfectAccuracyParameter(effect.Then);
            if (string.IsNullOrWhiteSpace(accuracyParam))
                accuracyParam = FindFirstImperfectAccuracyParameter(effect.OnSuccess);
            if (string.IsNullOrWhiteSpace(accuracyParam))
                accuracyParam = FindFirstImperfectAccuracyParameter(effect.OnFailure);

            return accuracyParam;
        }

        public bool HasFunction(string fName)
        {
            return Effect != null ? Effect.HasFunction(fName) : false;
        }

        public bool ChecksCondition(Character character, ITargetable target)
        {
            if (!MayBeUsed) return false;
            if (!string.IsNullOrWhiteSpace(UseCondition))
            {
                var parsedCondition = ExpressionParser.ParseArgForExpression(UseCondition, User, character, target);

                if (!ExpressionParser.CalculateBooleanExpression(parsedCondition)) return false;
            }
            return true;
        }

        public bool ChecksAICondition(Character character, ITargetable target)
        {
            if (!MayBeUsed) return false;
            if (!string.IsNullOrWhiteSpace(AIUseCondition))
            {
                var parsedCondition = ExpressionParser.ParseArgForExpression(AIUseCondition, User, character, target);

                if (!ExpressionParser.CalculateBooleanExpression(parsedCondition)) return false;
            }
            return true;
        }

        public ActionWithEffects Clone()
        {
            return new ActionWithEffects
            {
                Id = Id,
                Name = Name,
                NameLocaleKey = NameLocaleKey,
                Description = Description,
                DescriptionLocaleKey = DescriptionLocaleKey,
                IsScript = IsScript,
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
                FinishesTurnWhenUsed = FinishesTurnWhenUsed,
                AIUseCondition = AIUseCondition
            };
        }
    }

    [Serializable]
    public class Effect : ISerializable
    {
        private static readonly List<Type> EffectMethodTypes = ReflectionHelpers.GetTypesInNamespace(Assembly.GetExecutingAssembly(), "RogueCustomsGameEngine.Utils.Effects");

        public delegate bool ActionMethod(EffectCallerParams Args);
        public delegate Task<bool> AsyncActionMethod(EffectCallerParams Args);

        public string EffectMethodName { get; set; }
        public string EffectClassName { get; set; }
        public ActionMethod Function { get; set; }
        public AsyncActionMethod AsyncFunction { get; set; }

        public List<EffectParam> Params { get; set; }
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
                    if (effectMethod.ReturnType == typeof(Task<bool>))
                    {
                        AsyncFunction = (AsyncActionMethod)Delegate.CreateDelegate(typeof(AsyncActionMethod), effectMethod);
                    }
                    else if (effectMethod.ReturnType == typeof(bool))
                    {
                        Function = (ActionMethod)Delegate.CreateDelegate(typeof(ActionMethod), effectMethod);
                    }
                    else
                    {

                    }
                    var paramsArray = info.Params.Select(p => (p.ParamName, p.Value)).ToArray();
                    Params = [];
                    foreach (var (paramName, Value) in paramsArray)
                    {
                        Params.Add(new EffectParam
                        {
                            ParamName = paramName,
                            Value = Value
                        });
                    }
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
            else
            {

            }
        }

        public async Task<List<string>> Do(Entity This, Entity Source, ITargetable Target)
        {
            ExecutionContext.Current = new();
            var currentEffect = this;
            var successfulEffects = new List<string>();

            do
            {
                try
                {
                    ExecutionContext.Current.CurrentEffect = currentEffect;
                    bool success = false;
                    if (currentEffect.Function != null)
                    {
                        success = currentEffect.Function(new EffectCallerParams
                        {
                            This = This,
                            Source = Source,
                            Target = Target,
                            Params = currentEffect.Params,
                            OriginalTarget = Target
                        });
                        if (success)
                            successfulEffects.Add(currentEffect.Function.Method.Name);
                    }
                    else
                    {
                        success = await currentEffect.AsyncFunction(new EffectCallerParams
                        {
                            This = This,
                            Source = Source,
                            Target = Target,
                            Params = currentEffect.Params,
                            OriginalTarget = Target
                        });
                        if (success)
                            successfulEffects.Add(currentEffect.AsyncFunction.Method.Name);
                    }
                    

                    currentEffect = currentEffect.Then ?? (success
                                            ? currentEffect.OnSuccess
                                            : currentEffect.OnFailure);

                    if (currentEffect == null && ExecutionContext.Current.LoopStack.TryPeek(out var frame))
                    {
                        switch (frame)
                        {
                            case WhileFrame wf:
                                currentEffect = wf.StartEffect;
                                ExecutionContext.Current.LoopStack.Pop(); // We remove it in case the While fails; if it succeeds, it will be added again, no harm done
                                continue;

                            case ForFrame ff:
                                ff.Advance();
                                if (ff.ShouldContinue())
                                {
                                    currentEffect = ff.StartEffect.OnSuccess;
                                    continue;
                                }
                                else
                                {
                                    currentEffect = ff.StartEffect.OnFailure;
                                    ExecutionContext.Current.LoopStack.Pop();
                                }
                                break;
                        }
                    }
                }
                catch (FlagNotFoundException fe)
                {
                    if (!This.Map.IsDebugMode)
                    {
                        var e = new Exception(fe.Message);
                        ExceptionDispatchInfo.SetRemoteStackTrace(e, fe.StackTrace);
                        throw e;
                    }
                    else
                    {
                        This.Map.CreateFlag(fe.FlagName, 0, false);
                        This.Map.AppendMessage($"WARNING - {fe.FlagName} is used but not declared. It has been set to 0 to allow testing.", new GameColor(Color.Red));
                    }
                }
                catch (Exception ex)
                {
                    var e = new Exception(ex.Message);
                    ExceptionDispatchInfo.SetRemoteStackTrace(e, ex.StackTrace);
                    throw e;
                }
            }
            while (currentEffect != null);
            return successfulEffects.Distinct().ToList();
        }

        public bool HasFunction(string fName)
        {
            if (string.IsNullOrWhiteSpace(EffectMethodName))
                return false;

            if (EffectMethodName.Equals(fName, StringComparison.InvariantCultureIgnoreCase))
                return true;

            if (Then != null)
                return Then.HasFunction(fName);
            if (OnSuccess != null || OnFailure != null)
            {
                var onSuccessHasFunction = (OnSuccess != null) ? OnSuccess.HasFunction(fName) : false;
                var onFailureHasFunction = (OnFailure != null) ? OnFailure.HasFunction(fName) : false;
                return onSuccessHasFunction || onFailureHasFunction;
            }

            return false;
        }

        public Effect Clone()
        {
            return new Effect
            {
                AsyncFunction = AsyncFunction,
                Function = Function,
                EffectClassName = EffectClassName,
                EffectMethodName = EffectMethodName,
                Params = Params,
                Then = Then?.Clone(),
                OnSuccess = OnSuccess?.Clone(),
                OnFailure = OnFailure?.Clone()
            };
        }

        #region ISerializable implementation
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // We only keep the class and method because we can't serialize a delegate

            if(AsyncFunction != null)
            {
                EffectMethodName = AsyncFunction.GetMethodInfo().Name;
                EffectClassName = AsyncFunction.GetMethodInfo().DeclaringType.FullName;
            }
            else
            {
                EffectMethodName = Function.GetMethodInfo().Name;
                EffectClassName = Function.GetMethodInfo().DeclaringType.FullName;
            }
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
            EffectMethodName = info.GetString("EffectMethodName");
            EffectClassName = info.GetString("EffectClassName");
            try
            {
                Params = (List<EffectParam>)info.GetValue("Params", typeof(List<EffectParam>));
            }
            catch (Exception)
            {
                var paramsArray = (ValueTuple<string, string>[])info.GetValue("Params", typeof(ValueTuple<string, string>[]));
                Params = [];
                foreach (var (paramName, Value) in paramsArray)
                {
                    Params.Add(new EffectParam
                    {
                        ParamName = paramName,
                        Value = Value
                    });
                }
            }
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
                        if (methodInfo.ReturnType == typeof(Task<bool>))
                        {
                            AsyncFunction = (AsyncActionMethod)Delegate.CreateDelegate(typeof(AsyncActionMethod), methodInfo);
                        }
                        else if (methodInfo.ReturnType == typeof(bool))
                        {
                            Function = (ActionMethod)Delegate.CreateDelegate(typeof(ActionMethod), methodInfo);
                        }
                    } 
                    else
                    {

                    }
                }
                else
                {

                }
            }
        }

        #endregion
    }
#pragma warning restore IDE0037 // Usar nombre de miembro inferido
#pragma warning restore CS8601 // Posible asignación de referencia nula
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
#pragma warning restore CS8604 // Posible argumento de referencia nulo
#pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning restore RCS1075 // Avoid empty catch clause that catches System.Exception
}
