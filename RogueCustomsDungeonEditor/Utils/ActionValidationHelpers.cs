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

namespace RogueCustomsDungeonEditor.Utils
{
    public static class ActionValidationHelpers
    {
        public static bool TestNumericExpression(this string expression, bool checkDiceNotationAsWell, out string errorMessage)
        {
            try
            {
                var parsedExpression = ConvertArgsToPlaceholders(expression, "1", "\"name\"", "true");
                if (checkDiceNotationAsWell && parsedExpression.IsDiceNotation())
                    _ = new Dice().Roll(parsedExpression, new RandomDieRoller()).Value;
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
            var parsedArg = arg;

            parsedArg = parsedArg.ParseArgsForPlaceHolder(numericPlaceholder, stringPlaceholder, booleanPlaceholder, "this");
            parsedArg = parsedArg.ParseArgsForPlaceHolder(numericPlaceholder, stringPlaceholder, booleanPlaceholder, "source");
            parsedArg = parsedArg.ParseArgsForPlaceHolder(numericPlaceholder, stringPlaceholder, booleanPlaceholder, "target");

            return parsedArg;
        }

        private static string ParseArgsForPlaceHolder(this string arg, string numericPlaceholder, string stringPlaceholder, string booleanPlaceholder, string eName)
        {
            var parsedArg = arg;

            parsedArg = ParseEntityNames(parsedArg, stringPlaceholder, eName);
            parsedArg = ParseEntityProperties(parsedArg, numericPlaceholder, stringPlaceholder, eName);
            parsedArg = ParseRngExpressions(parsedArg, numericPlaceholder, stringPlaceholder, booleanPlaceholder, eName);
            parsedArg = ParseFlagExistsExpressions(parsedArg, booleanPlaceholder);
            parsedArg = ParseNamedFlags(parsedArg, numericPlaceholder);
            parsedArg = ParseStatusCheck(parsedArg, numericPlaceholder, stringPlaceholder, booleanPlaceholder, eName);

            return parsedArg;
        }

        private static string ParseEntityNames(string arg, string stringPlaceholder, string eName)
        {
            var parsedArg = arg;

            parsedArg = parsedArg.Replace($"{{{eName}}}", stringPlaceholder, StringComparison.InvariantCultureIgnoreCase);

            return parsedArg;
        }

        private static string ParseEntityProperties(string arg, string numericPlaceholder, string stringPlaceholder, string eName)
        {
            var parsedArg = arg;

            var entityTypes = new List<Type> { typeof(PlayerCharacter), typeof(NonPlayableCharacter), typeof(Item), typeof(AlteredStatus) };

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

            return parsedArg;
        }

        private static string ParseStatusCheck(string arg, string numericPlaceholder, string stringPlaceholder, string booleanPlaceholder, string eName)
        {
            var parsedArg = arg;

            var statusRegex = new Regex(@"HasStatus\(([^,]+),\s*([^)]+)\)|DoesNotHaveStatus\(([^,]+),\s*([^)]+)\)", RegexOptions.IgnoreCase);
            if (statusRegex.IsMatch(parsedArg))
            {
                var logicalOperators = new string[] { "&&", "||" };
                var subExpressions = parsedArg.Split(logicalOperators, StringSplitOptions.RemoveEmptyEntries);

                foreach (var subExpression in subExpressions)
                {
                    var parsedSubExpression = new string(subExpression.Trim());
                    var match = statusRegex.Match(subExpression);

                    if (match.Success)
                    {
                        if (match.Groups.Count < 5) continue;

                        var isNot = subExpression.Contains("DoesNotHaveStatus", StringComparison.InvariantCultureIgnoreCase);

                        var entityName = isNot ? match.Groups[3].Value : match.Groups[1].Value;

                        if (!entityName.Equals(eName)) continue;

                        parsedArg = parsedArg.Replace(match.Value.Trim(), booleanPlaceholder, StringComparison.InvariantCultureIgnoreCase);
                    }
                }
            }

            return parsedArg;
        }

        private static string ParseRngExpressions(string arg, string numericPlaceholder, string stringPlaceholder, string booleanPlaceholder, string eName)
        {
            var parsedArg = arg;
            var rngRegex = @"rng\((\d+),\s*(\d+)\)";
            var matches = Regex.Matches(arg, rngRegex, RegexOptions.IgnoreCase);

            foreach (Match match in matches)
            {
                if (int.TryParse(match.Groups[1].Value, out int x) && int.TryParse(match.Groups[2].Value, out int y))
                {
                    if (x > y)
                    {
                        throw new ArgumentException($"Invalid rng({x},{y}) expression: first parameter cannot be greater than the second.");
                    }

                    parsedArg = parsedArg.Replace(match.Value, numericPlaceholder, StringComparison.InvariantCultureIgnoreCase);
                }
            }

            return parsedArg;
        }
        private static string ParseFlagExistsExpressions(string arg, string booleanPlaceholder)
        {
            string regexFlagExists = @"FlagExists\(([^)]+)\)";
            string parsedArg = arg;
            var matches = Regex.Matches(arg, regexFlagExists);

            foreach (Match match in matches)
            {
                parsedArg = parsedArg.Replace(match.Value, booleanPlaceholder, StringComparison.InvariantCultureIgnoreCase);
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

        private static bool IsDiceNotation(this string s)
        {
            return Regex.Match(s, RogueCustomsGameEngine.Utils.Constants.DiceNotationRegexPattern).Success;
        }

        public static bool HaveAllParametersBeenParsed(this Effect effect, Entity This, Entity Source, Entity Target, Map map, out bool flagsAreInvolved)
        {
            flagsAreInvolved = false;

            // Hardcode Flags due to unpredictability
            var regexFlagExists = @"FlagExists\(([^)]+)\)";
            var regexFlagValue = @"\[(.*?)\]";

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

            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, 0, effect.Params);

            var paramsObjectAsDictionary = (IDictionary<string, object>)paramsObject;

            return paramsObjectAsDictionary.Count >= effect.Params.Count(p => !string.IsNullOrEmpty(p.Value));
        }

        public static bool TestFunction(this Effect effect, Entity This, Entity Source, ITargetable Target)
        {
            return effect.Function(This, Source, Target, 0, out _, effect.Params);
        }
    }
}
