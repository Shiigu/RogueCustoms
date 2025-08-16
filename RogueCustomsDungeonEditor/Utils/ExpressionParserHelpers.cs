using D20Tek.DiceNotation.DieRoller;
using D20Tek.DiceNotation;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using org.matheval;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.JsonImports;
using System.Linq.Expressions;
using Expression = org.matheval.Expression;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.Exceptions;
using RogueCustomsGameEngine.Utils.Expressions;
using System.Reflection;
using RogueCustomsDungeonEditor.Utils.ExpressionFunctions;
using RogueCustomsGameEngine.Utils;
using RogueCustomsGameEngine.Utils.Effects.Utils;
#pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.

namespace RogueCustomsDungeonEditor.Utils
{
    public static class ExpressionParserHelpers
    {
        public static bool TestNumericExpression(this string expression, bool checkDiceNotationAsWell, out string errorMessage)
        {
            try
            {
                var parsedExpression = ConvertArgsToPlaceholders(expression, "1", "\"name\"", "true");
                if (checkDiceNotationAsWell && parsedExpression.IsDiceNotation())
                    _ = ExpressionParser.RollDiceNotations(parsedExpression);
                else if (parsedExpression.IsIntervalNotation())
                {
                    _ = ExpressionParser.RollRangeNotations(parsedExpression);
                }
                else
                    _ = new Expression(parsedExpression).Eval<decimal>();
                errorMessage = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public static bool TestBooleanExpression(this string expression, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(expression))
                return true;
            try
            {
                var parsedExpression = ConvertArgsToPlaceholders(expression, "1", "\"name\"", "true");
                _ = new Expression(parsedExpression).Eval<bool>();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        private static string ConvertArgsToPlaceholders(this string arg, string numericPlaceholder, string stringPlaceholder, string booleanPlaceholder)
        {
            var tokens = ExpressionParser.SplitExpression(arg);

            for (int i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];
                if (Regex.IsMatch(token, @"\[.+\]")) // It's a flag token
                {
                    string inferredPlaceholder = numericPlaceholder;

                    // Look at the next token (right-hand side)
                    if (i + 2 < tokens.Count && IsComparisonOperator(tokens[i + 1]))
                    {
                        var rhs = tokens[i + 2];

                        if (Regex.IsMatch(rhs, "^\".*\"$")) // string literal
                            inferredPlaceholder = stringPlaceholder;
                        else if (rhs.Equals("true", StringComparison.OrdinalIgnoreCase) || rhs.Equals("false", StringComparison.OrdinalIgnoreCase))
                            inferredPlaceholder = booleanPlaceholder;
                        else if (double.TryParse(rhs, out _))
                            inferredPlaceholder = numericPlaceholder;
                    }

                    tokens[i] = inferredPlaceholder;
                }
                else
                {
                    // Replace tokens for known entities (player, this, etc.)
                    token = token.Replace("{CalculatedDamage}", numericPlaceholder);
                    token = token.ParseArgsForPlaceHolder(numericPlaceholder, stringPlaceholder, booleanPlaceholder, "player");
                    token = token.ParseArgsForPlaceHolder(numericPlaceholder, stringPlaceholder, booleanPlaceholder, "this");
                    token = token.ParseArgsForPlaceHolder(numericPlaceholder, stringPlaceholder, booleanPlaceholder, "source");
                    token = token.ParseArgsForPlaceHolder(numericPlaceholder, stringPlaceholder, booleanPlaceholder, "target");
                    tokens[i] = token;
                }
            }


            // Second pass: handle function parsing and execution
            // Since logical operators might be in place, split the expression based on those, and handle function parsing for each part.
            tokens = ExpressionParser.SplitExpression(string.Concat(tokens));

            for (int i = 0; i < tokens.Count; i++)
            {
                var function = DummyFunction.FromExpression(tokens[i]);
                if (function != null)
                {
                    // Execute the function and replace the token with its result
                    tokens[i] = function.Execute(null);
                }
            }

            // Return the fully parsed expression
            return string.Join("", tokens);
        }
        private static bool IsComparisonOperator(string token)
        {
            return token switch
            {
                "==" or "!=" or ">" or "<" or ">=" or "<=" => true,
                _ => false
            };
        }

        private static string ParseArgsForPlaceHolder(this string arg, string numericPlaceholder, string stringPlaceholder, string booleanPlaceholder, string eName)
        {
            var parsedArg = arg;

            parsedArg = ParseEntityNames(parsedArg, stringPlaceholder, eName);
            parsedArg = ParseObjectProperties(parsedArg, numericPlaceholder, stringPlaceholder, eName);

            return parsedArg;
        }

        private static string ParseEntityNames(string arg, string stringPlaceholder, string eName)
        {
            var parsedArg = arg;

            parsedArg = parsedArg.Replace($"{{{eName}}}", stringPlaceholder, StringComparison.InvariantCultureIgnoreCase);

            return parsedArg;
        }

        private static string ParseObjectProperties(string arg, string numericPlaceholder, string stringPlaceholder, string eName)
        {
            var parsedArg = arg;
            var entityTypes = new List<Type> { typeof(PlayerCharacter), typeof(NonPlayableCharacter), typeof(Item), typeof(Trap), typeof(AlteredStatus), typeof(Tile) };

            // Regex to match both property and Stat: expressions like {eName.Stat:StatId}
            var regex = new Regex(@$"\{{{eName}\.(Stat:([A-Za-z0-9_]+)|([A-Za-z0-9_.]+))\}}", RegexOptions.IgnoreCase);
            var matches = regex.Matches(parsedArg);

            foreach (Match match in matches)
            {
                var fullPropertyPath = match.Groups[1].Value; // Full match (Stat:StatId or regular property path)
                var statId = match.Groups[2].Value;           // Group 2 will have the StatId if it's a stat expression
                var propertyPath = match.Groups[3].Value;     // Group 3 will have the regular property path

                // If it's a stat expression (Stat:StatId), replace with numericPlaceholder
                if (!string.IsNullOrEmpty(statId))
                {
                    parsedArg = parsedArg.Replace(match.Value, numericPlaceholder, StringComparison.InvariantCultureIgnoreCase);
                }
                else if (!string.IsNullOrEmpty(propertyPath)) // Handle nested properties
                {
                    string replacement = ResolveNestedPropertyValue(entityTypes, propertyPath, numericPlaceholder, stringPlaceholder);
                    if (replacement != null)
                    {
                        parsedArg = parsedArg.Replace(match.Value, replacement, StringComparison.InvariantCultureIgnoreCase);
                    }
                }
            }

            return parsedArg;
        }

        private static string ResolveNestedPropertyValue(IEnumerable<Type> entityTypes, string propertyPath, string numericPlaceholder, string stringPlaceholder)
        {
            var numericStringProperties = new List<string> { "Power", "Damage", "Mitigation" };
            var stringProperties = new List<string> { "Type" };
            var properties = propertyPath.Split('.');
            Type currentType = null;
            bool isNumeric = true;

            foreach (var entityType in entityTypes)
            {
                if (entityType.GetProperty(properties[0]) != null)
                {
                    currentType = entityType;
                    break;
                }
            }

            if (currentType == null) return null;

            for (int i = 0; i < properties.Length; i++)
            {
                var property = currentType.GetProperty(properties[i]);
                if (property == null) return null;

                // Determine the placeholder based on property type for the last property
                if (i == properties.Length - 1)
                {
                    if ((property.PropertyType == typeof(string) || stringProperties.Contains(property.Name)) && !numericStringProperties.Contains(property.Name, StringComparer.InvariantCultureIgnoreCase))
                    {
                        isNumeric = false;
                    }
                }
                else
                {
                    currentType = property.PropertyType;
                }
            }

            return isNumeric ? numericPlaceholder : stringPlaceholder;
        }

        private static string ParseNamedFlag(this string arg, string placeholder)
        {
            var parsedArg = arg;

            var matches = Regex.Matches(parsedArg, @"\[(.*?)\]");

            foreach (Match match in matches)
            {
                var flagToken = match.Value;

                parsedArg = parsedArg.Replace(flagToken, placeholder, StringComparison.InvariantCultureIgnoreCase);
            }

            return parsedArg;
        }

        public static bool HaveAllParametersBeenParsed(this Effect effect, Entity This, Entity Source, Entity Target, Map map, out bool flagsAreInvolved)
        {
            flagsAreInvolved = false;

            // Hardcode Flags due to unpredictability
            var regexFlagExists = @"FlagExists\(([^)]+)\)";
            var regexFlagValue = EngineConstants.FlagRegexPattern;

            var paramValuesToReplace = new List<(string ParamName, string Value)>();

            foreach (var param in effect.Params)
            {
                var flagExistsMatches = Regex.Matches(param.Value, regexFlagExists);

                foreach (Match match in flagExistsMatches)
                {
                    paramValuesToReplace.Add((param.ParamName, param.Value.Replace(match.Value, "true", StringComparison.InvariantCultureIgnoreCase)));
                }

                var flagValueMatches = Regex.Matches(param.Value, regexFlagValue);

                foreach (Match match in flagValueMatches)
                {
                    paramValuesToReplace.Add((param.ParamName, ConvertFlagToPlaceholder(param.Value, "1", "\"1\"", "true")));
                }

                if (!flagValueMatches.Any())
                {
                    var flagValueLocaleMatches = Regex.Matches(map.Locale[param.Value], regexFlagValue);

                    foreach (Match match in flagValueLocaleMatches)
                    {
                        paramValuesToReplace.Add((param.ParamName, ConvertFlagToPlaceholder(map.Locale[param.Value], "1", "\"1\"", "true")));
                    }
                }
            }

            if (paramValuesToReplace.Any())
            {
                foreach (var paramValue in paramValuesToReplace)
                {
                    var paramList = effect.Params.ToList();
                    paramList.RemoveAll(p => p.ParamName.Equals(paramValue.ParamName));
                    (string ParamName, string ParamValue) paramToReplace = new (paramValue.ParamName, paramValue.Value);
                    paramList.Add(new EffectParam
                    {
                        ParamName = paramToReplace.ParamName,
                        Value = paramToReplace.ParamValue
                    });
                    effect.Params = paramList;
                }
                flagsAreInvolved = true;
            }

            dynamic paramsObject = ExpressionParser.ParseParams(new EffectCallerParams
            {
                This = This,
                Source = Source,
                Target = Target,
                Params = effect.Params,
                OriginalTarget = Target
            });

            var paramsObjectAsDictionary = paramsObject.ToDictionary();

            return paramsObjectAsDictionary.Count >= effect.Params.DistinctBy(p => p.ParamName).Count(p => !string.IsNullOrEmpty(p.Value));
        }

        private static string ConvertFlagToPlaceholder(this string arg, string numericPlaceholder, string stringPlaceholder, string booleanPlaceholder)
        {
            var tokens = ExpressionParser.SplitExpression(arg);

            for (int i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];
                if (Regex.IsMatch(token, @"\[.+\]")) // It's a flag token
                {
                    string inferredPlaceholder = numericPlaceholder;

                    // Look at the next token (right-hand side)
                    if (i + 2 < tokens.Count && IsComparisonOperator(tokens[i + 1]))
                    {
                        var rhs = tokens[i + 2];

                        if (Regex.IsMatch(rhs, "^\".*\"$")) // string literal
                            inferredPlaceholder = stringPlaceholder;
                        else if (rhs.Equals("true", StringComparison.OrdinalIgnoreCase) || rhs.Equals("false", StringComparison.OrdinalIgnoreCase))
                            inferredPlaceholder = booleanPlaceholder;
                        else if (double.TryParse(rhs, out _))
                            inferredPlaceholder = numericPlaceholder;
                    }

                    tokens[i] = inferredPlaceholder;
                }
            }

            return string.Concat(tokens);
        }

        public static async Task<bool> TestFunction(this Effect effect, Entity This, Entity Source, ITargetable Target)
        {
            var priorContext = ExecutionContext.Current;
            ExecutionContext.Current = new();
            bool result = false;
            if (effect.AsyncFunction != null)
            {
                result = await effect.AsyncFunction(new EffectCallerParams
                {
                    This = This,
                    Source = Source,
                    Target = Target,
                    Params = effect.Params,
                    OriginalTarget = Target
                });
            }
            else
            {
                result = effect.Function(new EffectCallerParams
                {
                    This = This,
                    Source = Source,
                    Target = Target,
                    Params = effect.Params,
                    OriginalTarget = Target
                });
            }
            ExecutionContext.Current = priorContext;
            return result;
        }
    }
}
#pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
