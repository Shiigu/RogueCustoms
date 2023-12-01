using D20Tek.DiceNotation;
using D20Tek.DiceNotation.DieRoller;
using org.matheval;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.DiceNotation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace RogueCustomsGameEngine.Utils.Helpers
{
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
#pragma warning disable CS8604 // Posible argumento de referencia nulo
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
    public static class ActionHelpers
    {
        private readonly static List<string> FieldsToConsider = new()
        {
            "Id",
            "ClassId",
            "Name",
            "Weapon",
            "Armor",
            "HP",
            "MaxHP",
            "MP",
            "MaxMP",
            "Attack",
            "Damage",
            "Defense",
            "Mitigation",
            "Movement",
            "HPRegeneration",
            "MPRegeneration",
            "Accuracy",
            "Evasion",
            "ExperiencePayout",
            "Owner",
            "Power",
            "TurnLength"
        };

        private static RngHandler Rng;
        private static Map Map;

        public static void SetActionParams(RngHandler rng, Map map)
        {
            Rng = rng;
            Map = map;
        }

        public static ExpandoObject ParseParams(Entity This, Entity Source, ITargetable Target, int previousEffectOutput, params (string ParamName, string Value)[] args)
        {
            dynamic paramsObject = new ExpandoObject();
            foreach (var (ParamName, Value) in args)
            {
                try
                {
                    var paramName = ParamName.ToLower();
                    var value = ParseArgForExpression(Map.Locale[Value], This, Source, Target);
                    if (string.IsNullOrEmpty(value)) continue;
                    switch (paramName)
                    {
                        case "attacker":
                            if (value.Equals("this", StringComparison.InvariantCultureIgnoreCase))
                                paramsObject.Attacker = This;
                            else if (value.Equals("source", StringComparison.InvariantCultureIgnoreCase))
                                paramsObject.Attacker = Source;
                            else if (value.Equals("target", StringComparison.InvariantCultureIgnoreCase))
                                paramsObject.Attacker = Target;
                            break;
                        case "source":
                            if (value.Equals("this", StringComparison.InvariantCultureIgnoreCase))
                                paramsObject.Source = This;
                            else if (value.Equals("source", StringComparison.InvariantCultureIgnoreCase))
                                paramsObject.Source = Source;
                            else if (value.Equals("target", StringComparison.InvariantCultureIgnoreCase))
                                paramsObject.Source = Target;
                            break;
                        case "target":
                            if (value.Equals("this", StringComparison.InvariantCultureIgnoreCase))
                                paramsObject.Target = This;
                            else if (value.Equals("source", StringComparison.InvariantCultureIgnoreCase))
                                paramsObject.Target = Source;
                            else if (value.Equals("target", StringComparison.InvariantCultureIgnoreCase))
                                paramsObject.Target = Target;
                            break;
                        case "stat":
                            paramsObject.StatName = char.ToUpper(value[0]) + value.ToLowerInvariant()[1..];
                            var c = Target as Character;
                            switch (value.ToLowerInvariant())
                            {
                                case "maxhp":
                                    paramsObject.StatAlterationList = c.MaxHPModifications;
                                    break;
                                case "maxmp":
                                    paramsObject.StatAlterationList = c.MaxMPModifications;
                                    break;
                                case "attack":
                                    paramsObject.StatAlterationList = c.AttackModifications;
                                    break;
                                case "defense":
                                    paramsObject.StatAlterationList = c.DefenseModifications;
                                    break;
                                case "movement":
                                    paramsObject.StatAlterationList = c.MovementModifications;
                                    break;
                                case "hpregeneration":
                                    paramsObject.StatAlterationList = c.HPRegenerationModifications;
                                    break;
                                case "mpregeneration":
                                    paramsObject.StatAlterationList = c.MPRegenerationModifications;
                                    break;
                                case "accuracy":
                                    paramsObject.StatAlterationList = c.AccuracyModifications;
                                    break;
                                case "evasion":
                                    paramsObject.StatAlterationList = c.EvasionModifications;
                                    break;
                            }
                            break;
                        case "attack":
                            paramsObject.Damage = CalculateDiceNotationIfNeeded(value);
                            break;
                        case "defense":
                            paramsObject.Mitigation = CalculateDiceNotationIfNeeded(value);
                            break;
                        case "accuracy":
                            paramsObject.Accuracy = CalculateDiceNotationIfNeeded(value);
                            break;
                        case "chance":
                            paramsObject.Chance = CalculateDiceNotationIfNeeded(value);
                            break;
                        case "power":
                            paramsObject.Power = CalculateDiceNotationIfNeeded(value);
                            break;
                        case "amount":
                            paramsObject.Amount = CalculateDiceNotationIfNeeded(value);
                            break;
                        case "turnlength":
                            paramsObject.TurnLength = CalculateDiceNotationIfNeeded(value);
                            break;
                        case "bypassesaccuracycheck":
                            paramsObject.BypassesAccuracyCheck = new Expression(value).Eval<bool>();
                            break;
                        case "bypassesvisibilitycheck":
                            paramsObject.BypassesVisibilityCheck = new Expression(value).Eval<bool>();
                            break;
                        case "displayonlog":
                            paramsObject.DisplayOnLog = new Expression(value).Eval<bool>();
                            break;
                        case "canbestacked":
                            paramsObject.CanBeStacked = new Expression(value).Eval<bool>();
                            break;
                        case "canstealequippables":
                            paramsObject.CanStealEquippables = new Expression(value).Eval<bool>();
                            break;
                        case "canstealconsumables":
                            paramsObject.CanStealConsumables = new Expression(value).Eval<bool>();
                            break;
                        case "condition":
                            paramsObject.Condition = value;
                            break;
                        case "key":
                            paramsObject.Key = value;
                            break;
                        case "value":
                            paramsObject.Value = (int) CalculateDiceNotationIfNeeded(value);
                            break;
                        case "removeonfloorchange":
                            paramsObject.RemoveOnFloorChange = new Expression(value).Eval<bool>();
                            break;
                        case "character":
                            paramsObject.Character = value[0];
                            break;
                        case "color":
                            paramsObject.Color = value.ToGameColor();
                            break;
                        case "forecolor":
                            paramsObject.ForeColor = value.ToGameColor();
                            break;
                        case "backcolor":
                            paramsObject.BackColor = value.ToGameColor();
                            break;
                        case "output":
                            paramsObject.Output = previousEffectOutput;
                            break;
                        case "tiletype":
                            paramsObject.TileType = Enum.Parse<TileType>(value, true);
                            break;
                        case "id":
                            paramsObject.Id = value;
                            break;
                        case "title":
                            paramsObject.Title = ParseValueForTextDisplay(value);
                            break;
                        case "text":
                            paramsObject.Text = ParseValueForTextDisplay(value);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Parsing expression {Value} of parameter {ParamName} threw an exception: {ex.Message}");
                }
            }
            return paramsObject;
        }

        private static decimal CalculateDiceNotationIfNeeded(string value)
        {
            if (value.IsBooleanExpression())
                throw new ArgumentException($"{value} is a boolean expression, but is being evaluated as a number.");
            if (value.IsDiceNotation())
                return new Dice().Roll(value, new LCGDieRoller(Rng)).Value;
            return new Expression(value).Eval<decimal>();
        }

        public static bool CalculateBooleanExpression(string value)
        {
            if(string.IsNullOrEmpty(value)) return true;
            if(!value.IsBooleanExpression())
                throw new ArgumentException($"{value} is not a boolean expression but is being evaluated as one.");
            return new Expression(value).Eval<bool>();
        }

        private static string ParseValueForTextDisplay(string value)
        {
            const string functionMatchRegex = "(?:LEFT|RIGHT|MID|REVERSE|LOWER|UPPER|PROPER|TRIM|TEXT|REPLACE|SUBSTITUTE|CONCAT)";
            if (Regex.Match(value, functionMatchRegex).Success)
                return new Expression($"TEXT({value})").Eval<string>();
            return value;
        }

        public static string ParseArgForExpression(string arg, Entity This, Entity Source, ITargetable Target)
        {
            var parsedArg = arg;

            parsedArg = ParseArgForEntity(parsedArg, This, "this");
            parsedArg = ParseArgForEntity(parsedArg, Source, "source");
            parsedArg = ParseArgForEntity(parsedArg, Target as Entity, "target");

            return parsedArg;
        }

        private static string ParseArgForEntity(string arg, Entity e, string eName)
        {
            if (e == null) return arg;
            var parsedArg = arg;

            parsedArg = ParseEntityNames(parsedArg, e, eName);
            parsedArg = ParseEntityProperties(parsedArg, e, eName);
            parsedArg = ParseRngExpressions(parsedArg);
            parsedArg = ParseFlagExistsExpressions(parsedArg);
            parsedArg = ParseNamedFlags(parsedArg);
            parsedArg = ParseStatusCheck(parsedArg, e, eName);

            return parsedArg;
        }

        private static string ParseEntityNames(string arg, Entity e, string eName)
        {
            var parsedArg = arg;

            parsedArg = parsedArg.Replace($"{{{eName}}}", e.Name, StringComparison.InvariantCultureIgnoreCase);

            return parsedArg;
        }

        private static string ParseEntityProperties(string arg, Entity e, string eName)
        {
            var parsedArg = arg;

            var entityType = e.GetType();
            foreach (var property in entityType.GetProperties().Where(p => FieldsToConsider.Contains(p.Name)))
            {
                string propertyName = property.Name;
                string fieldToken = $"{{{eName}.{propertyName}}}";

                if (parsedArg.Contains(fieldToken, StringComparison.InvariantCultureIgnoreCase))
                {
                    object propertyValue = property.GetValue(e);
                    if (propertyValue != null)
                    {
                        if (propertyValue is Entity entityProperty)
                        {
                            parsedArg = parsedArg.Replace(fieldToken, entityProperty.Name, StringComparison.InvariantCultureIgnoreCase);
                        }
                        else if (propertyName.Equals("Status", StringComparison.InvariantCultureIgnoreCase) &&
                                 entityType.GetProperty("AlteredStatuses") != null &&
                                 entityType.GetProperty("AlteredStatuses").PropertyType == typeof(List<AlteredStatus>))
                        {
                            var alteredStatuses = (List<AlteredStatus>)entityType.GetProperty("AlteredStatuses").GetValue(e);
                            var statusString = string.Join("/", alteredStatuses.Select(als => als.Id));
                            parsedArg = parsedArg.Replace(fieldToken, statusString, StringComparison.InvariantCultureIgnoreCase);
                        }
                        else if (propertyName.Equals("ClassId") || propertyName.Equals("Name"))
                        {
                            parsedArg = parsedArg.Replace(fieldToken, FormatParameterValue($"\"{propertyValue}\""), StringComparison.InvariantCultureIgnoreCase);
                        }
                        else
                        {
                            parsedArg = parsedArg.Replace(fieldToken, FormatParameterValue(propertyValue), StringComparison.InvariantCultureIgnoreCase);
                        }
                    }
                }
            }

            return parsedArg;
        }

        private static string ParseStatusCheck(string arg, Entity e, string eName)
        {
            var parsedArg = arg;
            if (e == null) return parsedArg;

            var regex = new Regex(@"HasStatus\(([^,]+),\s*([^)]+)\)|DoesNotHaveStatus\(([^,]+),\s*([^)]+)\)", RegexOptions.IgnoreCase);
            if (regex.IsMatch(parsedArg))
            {
                var logicalOperators = new string[] { "&&", "||" };
                var subExpressions = parsedArg.Split(logicalOperators, StringSplitOptions.RemoveEmptyEntries);

                foreach (var subExpression in subExpressions)
                {
                    var parsedSubExpression = new string(subExpression.Trim());
                    var match = regex.Match(subExpression);

                    if (match.Success)
                    {
                        if (match.Groups.Count < 5) continue;

                        var isNot = subExpression.Contains("DoesNotHaveStatus", StringComparison.InvariantCultureIgnoreCase);

                        var entityName = isNot ? match.Groups[3].Value : match.Groups[1].Value;

                        if (!entityName.Equals(eName)) continue;

                        var statusName = isNot ? match.Groups[4].Value : match.Groups[2].Value;

                        if (!Map.PossibleStatuses.Exists(als => als.ClassId.Equals(statusName))) continue;

                        if (e is Character character)
                        {
                            var statusExists = character.AlteredStatuses.Exists(als => als.ClassId.Equals(statusName, StringComparison.InvariantCultureIgnoreCase));

                            if (isNot)
                            {
                                statusExists = !statusExists;
                            }

                            parsedSubExpression = subExpression.Replace(match.Value, statusExists.ToString());
                        }
                        parsedArg = parsedArg.Replace(match.Value.Trim(), parsedSubExpression.Trim(), StringComparison.InvariantCultureIgnoreCase);
                    }
                }
            }

            return parsedArg;
        }

        private static string ParseRngExpressions(string arg)
        {
            var rngRegex = @"rng\((\d+),\s*(\d+)\)";
            var parsedArg = arg;
            var matches = Regex.Matches(arg, rngRegex);

            foreach (Match match in matches)
            {
                if (int.TryParse(match.Groups[1].Value, out int x) && int.TryParse(match.Groups[2].Value, out int y))
                {
                    if (x > y)
                    {
                        throw new ArgumentException($"Invalid rng({x},{y}) expression: first parameter cannot be greater than the second.");
                    }

                    int randomValue = Rng.Next(x, y + 1);
                    parsedArg = parsedArg.Replace(match.Value, randomValue.ToString(), StringComparison.InvariantCultureIgnoreCase);
                }
            }

            return parsedArg;
        }

        private static string ParseFlagExistsExpressions(string arg)
        {
            var regexFlagExists = @"FlagExists\(([^)]+)\)";
            string parsedArg = arg;
            var matches = Regex.Matches(arg, regexFlagExists);

            foreach (Match match in matches)
            {
                var flagName = match.Groups[1].Value;

                bool flagExists = Map.HasFlag(flagName);
                parsedArg = parsedArg.Replace(match.Value, flagExists.ToString(), StringComparison.InvariantCultureIgnoreCase);
            }

            return parsedArg;
        }

        private static string ParseNamedFlags(string arg)
        {
            var parsedArg = arg;

            var matches = Regex.Matches(parsedArg, @"\[(.*?)\]");

            foreach (Match match in matches)
            {
                var flagToken = match.Value;
                var flagName = match.Groups[1].Value;

                var flagValue = Map.GetFlagValue(flagName);
                parsedArg = parsedArg.Replace(flagToken, FormatParameterValue(flagValue), StringComparison.InvariantCultureIgnoreCase);
            }

            return parsedArg;
        }

        private static string FormatParameterValue(object parameterValue)
        {
            if (parameterValue is decimal decimalValue)
            {
                if (decimalValue == decimal.Truncate(decimalValue))
                {
                    return decimal.Truncate(decimalValue).ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    return decimalValue.ToString("0.#####", CultureInfo.GetCultureInfo("en-US"));
                }
            }
            else
            {
                return parameterValue.ToString();
            }
        }

        public static int CalculateAdjustedAccuracy(Entity source, Entity target, dynamic paramsObject)
        {
            var targetEvasion = (target is Character t) ? (int) t.Evasion : 0;
            var sourceAccuracy = (source is Character s) ? (int) s.Accuracy : 0;
            var baseAccuracy = (int) paramsObject.Accuracy;

            int adjustedAccuracy;

            if (paramsObject.BypassesAccuracyCheck)
            {
                adjustedAccuracy = baseAccuracy;
            }
            else
            {
                var accuracyModifier = 100 - targetEvasion;
                var adjustedSourceAccuracy = (sourceAccuracy * accuracyModifier / 100);
                adjustedAccuracy = baseAccuracy * adjustedSourceAccuracy / 100;
            }

            return Math.Max(0, Math.Min(100, adjustedAccuracy));
        }
    }
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
#pragma warning restore CS8604 // Posible argumento de referencia nulo
#pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
