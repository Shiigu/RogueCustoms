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
                token = token.Replace("{CalculatedDamage}", "1");
                token = token.ParseArgsForPlaceHolder(numericPlaceholder, stringPlaceholder, booleanPlaceholder, "player");
                token = token.ParseArgsForPlaceHolder(numericPlaceholder, stringPlaceholder, booleanPlaceholder, "this");
                token = token.ParseArgsForPlaceHolder(numericPlaceholder, stringPlaceholder, booleanPlaceholder, "source");
                token = token.ParseArgsForPlaceHolder(numericPlaceholder, stringPlaceholder, booleanPlaceholder, "target");
                token = ParseNamedFlags(token, numericPlaceholder);
                tokens[i] = token;
            }

            // Second pass: handle function parsing and execution
            // Since logical operators might be in place, split the expression based on those, and handle function parsing for each part.
            tokens = ExpressionParser.SplitExpression(string.Join("", tokens));

            for (int i = 0; i < tokens.Count; i++)
            {
                var function = DummyFunction.FromExpression(tokens[i]);
                if (function != null)
                {
                    // Execute the function and replace the token with its result
                    tokens[i] = function.Execute(null, null, null);
                }
            }

            // Return the fully parsed expression
            return string.Join("", tokens);
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
                else if (!string.IsNullOrEmpty(propertyPath)) // Handle regular properties
                {
                    foreach (var entityType in entityTypes)
                    {
                        foreach (var property in entityType.GetProperties())
                        {
                            string propertyName = property.Name;
                            string fieldToken = $"{{{eName}.{propertyName}}}";

                            if (parsedArg.Contains(fieldToken, StringComparison.InvariantCultureIgnoreCase))
                            {
                                if (propertyName.Equals("ClassId", StringComparison.InvariantCultureIgnoreCase)
                                    || propertyName.Equals("Name", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    parsedArg = parsedArg.Replace(fieldToken, stringPlaceholder, StringComparison.InvariantCultureIgnoreCase);
                                }
                                else
                                {
                                    parsedArg = parsedArg.Replace(fieldToken, numericPlaceholder, StringComparison.InvariantCultureIgnoreCase);
                                }
                            }
                        }
                    }
                }
            }

            return parsedArg;
        }

        private static string ParseNamedFlags(string arg, string numericPlaceholder)
        {
            var parsedArg = arg;

            var matches = Regex.Matches(parsedArg, @"\[(.*?)\]");

            foreach (Match match in matches)
            {
                var flagToken = match.Value;

                parsedArg = parsedArg.Replace(flagToken, numericPlaceholder, StringComparison.InvariantCultureIgnoreCase);
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
                    paramValuesToReplace.Add((param.ParamName, param.Value.Replace(match.Value, "1", StringComparison.InvariantCultureIgnoreCase)));
                }

                var flagValueLocaleMatches = Regex.Matches(map.Locale[param.Value], regexFlagValue);

                foreach (Match match in flagValueLocaleMatches)
                {
                    paramValuesToReplace.Add((param.ParamName, map.Locale[param.Value].Replace(match.Value, "1", StringComparison.InvariantCultureIgnoreCase)));
                }
            }

            if (paramValuesToReplace.Any())
            {
                foreach (var paramValue in paramValuesToReplace)
                {
                    var paramList = effect.Params.ToList();
                    paramList.RemoveAll(p => p.ParamName.Equals(paramValue.ParamName));
                    (string ParamName, string ParamValue) paramToReplace = new (paramValue.ParamName, paramValue.Value);
                    paramList.Add(paramToReplace);
                    effect.Params = paramList.ToArray();
                }
                flagsAreInvolved = true;
            }

            dynamic paramsObject = ExpressionParser.ParseParams(This, Source, Target, effect.Params);

            var paramsObjectAsDictionary = paramsObject.ToDictionary();

            return paramsObjectAsDictionary.Count >= effect.Params.Count(p => !string.IsNullOrEmpty(p.Value));
        }

        public static bool TestFunction(this Effect effect, Entity This, Entity Source, ITargetable Target)
        {
            return effect.Function(This, Source, Target, effect.Params);
        }
    }
}
#pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
