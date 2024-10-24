using D20Tek.DiceNotation;
using org.matheval;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.DiceNotation;
using RogueCustomsGameEngine.Utils.Exceptions;
using RogueCustomsGameEngine.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

using Expression = org.matheval.Expression;

namespace RogueCustomsGameEngine.Utils.Expressions
{
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
#pragma warning disable CS8604 // Posible argumento de referencia nulo
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
    public static class ExpressionParser
    {
        public static readonly Regex ExpressionSplitterRegex = new(@"(==|!=|<=|>=|&&|\|\||<|>)", RegexOptions.Compiled);

        private readonly static List<string> PropertiesToParse = new()
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
            "Hunger",
            "MaxHunger",
            "ExperiencePayout",
            "Owner",
            "Power",
            "TurnLength",
            "Type",
            "DoorId"
        };

        private static RngHandler Rng;
        private static Map Map;

        public static void Setup(RngHandler rng, Map map)
        {
            Rng = rng;
            Map = map;
            ExpressionFunctions.Setup(rng, map);
        }
        public static CaseInsensitiveExpandoObject ParseParams(Entity This, Entity Source, ITargetable Target, params (string ParamName, string Value)[] args)
        {
            dynamic paramsObject = new CaseInsensitiveExpandoObject();
            var paramsDict = paramsObject.ToDictionary();

            foreach (var (ParamName, Value) in args)
            {
                try
                {
                    var paramName = ParamName.ToLower();
                    var value = ParseArgForExpression(Map.Locale[Value], This, Source, Target);

                    if (string.IsNullOrEmpty(value)) continue;

                    // If it's not just a "copy value to field"...
                    if (RequiresCustomParsing(paramsObject, paramName, value, This, Source, Target))
                        continue;

                    // Otherwise...
                    if (!paramsDict.ContainsKey(paramName))
                        paramsDict[paramName] = value;
                }
                catch (FlagNotFoundException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Parsing expression {Value} of parameter {ParamName} threw an exception: {ex.Message}");
                }
            }

            paramsObject.Populate(paramsDict);

            return paramsObject;
        }

        private static bool RequiresCustomParsing(dynamic paramsObject, string paramName, string value, Entity This, Entity Source, ITargetable Target)
        {
            switch (paramName)
            {
                case "attacker":
                    if (value.Equals("this", StringComparison.InvariantCultureIgnoreCase))
                        paramsObject.Attacker = This;
                    else if (value.Equals("source", StringComparison.InvariantCultureIgnoreCase))
                        paramsObject.Attacker = Source;
                    else if (value.Equals("target", StringComparison.InvariantCultureIgnoreCase))
                        paramsObject.Attacker = Target;
                    return true;
                case "source":
                    if (value.Equals("this", StringComparison.InvariantCultureIgnoreCase))
                        paramsObject.Source = This;
                    else if (value.Equals("source", StringComparison.InvariantCultureIgnoreCase))
                        paramsObject.Source = Source;
                    else if (value.Equals("target", StringComparison.InvariantCultureIgnoreCase))
                        paramsObject.Source = Target;
                    return true;
                case "target":
                    if (value.Equals("this", StringComparison.InvariantCultureIgnoreCase))
                        paramsObject.Target = This;
                    else if (value.Equals("source", StringComparison.InvariantCultureIgnoreCase))
                        paramsObject.Target = Source;
                    else if (value.Equals("target", StringComparison.InvariantCultureIgnoreCase))
                        paramsObject.Target = Target;
                    return true;
                case "stat":
                    paramsObject.StatName = char.ToUpper(value[0]) + value.ToLowerInvariant()[1..];
                    var c = Target as Character;
                    var statNameToLookUp = value.ToLowerInvariant();
                    if (statNameToLookUp.StartsWith("max") && !statNameToLookUp.Equals("max"))
                        statNameToLookUp = statNameToLookUp.TrimStart("max");
                    var correspondingStat = c.UsedStats.Find(s => s.Id.Equals(statNameToLookUp, StringComparison.InvariantCultureIgnoreCase));
                    paramsObject.StatAlterationList = correspondingStat != null ? correspondingStat.ActiveModifications : null;
                    return true;
                case "attack":
                    paramsObject.Damage = CalculateDiceNotationIfNeeded(value);
                    return true;
                case "defense":
                    paramsObject.Mitigation = CalculateDiceNotationIfNeeded(value);
                    return true;
                case "accuracy":
                    paramsObject.Accuracy = CalculateDiceNotationIfNeeded(value);
                    return true;
                case "chance":
                    paramsObject.Chance = CalculateDiceNotationIfNeeded(value);
                    return true;
                case "power":
                    paramsObject.Power = CalculateDiceNotationIfNeeded(value);
                    return true;
                case "amount":
                    paramsObject.Amount = CalculateDiceNotationIfNeeded(value);
                    return true;
                case "turnlength":
                    paramsObject.TurnLength = CalculateDiceNotationIfNeeded(value);
                    return true;
                case "level":
                    paramsObject.Level = CalculateDiceNotationIfNeeded(value);
                    return true;
                case "bypassesaccuracycheck":
                    paramsObject.BypassesAccuracyCheck = new Expression(value).Eval<bool>();
                    return true;
                case "bypassesvisibilitycheck":
                    paramsObject.BypassesVisibilityCheck = new Expression(value).Eval<bool>();
                    return true;
                case "displayonlog":
                    paramsObject.DisplayOnLog = new Expression(value).Eval<bool>();
                    return true;
                case "canbestacked":
                    paramsObject.CanBeStacked = new Expression(value).Eval<bool>();
                    return true;
                case "canstealequippables":
                    paramsObject.CanStealEquippables = new Expression(value).Eval<bool>();
                    return true;
                case "canstealconsumables":
                    paramsObject.CanStealConsumables = new Expression(value).Eval<bool>();
                    return true;
                case "frominventory":
                    paramsObject.FromInventory = new Expression(value).Eval<bool>();
                    return true;
                case "bypassesresistances":
                    paramsObject.BypassesResistances = new Expression(value).Eval<bool>();
                    return true;
                case "bypasseselementeffect":
                    paramsObject.BypassesElementEffect = new Expression(value).Eval<bool>();
                    return true;
                case "tiletype":
                    paramsObject.TileType = Map.TileTypes.FirstOrDefault(tt => tt.Id.Equals(value, StringComparison.InvariantCultureIgnoreCase));
                    return true;
                case "value":
                    paramsObject.Value = (int)CalculateDiceNotationIfNeeded(value);
                    return true;
                case "removeonfloorchange":
                    paramsObject.RemoveOnFloorChange = new Expression(value).Eval<bool>();
                    return true;
                case "character":
                    paramsObject.Character = value[0];
                    return true;
                case "color":
                    paramsObject.Color = value.ToGameColor();
                    return true;
                case "forecolor":
                    paramsObject.ForeColor = value.ToGameColor();
                    return true;
                case "backcolor":
                    paramsObject.BackColor = value.ToGameColor();
                    return true;
                case "criticalhitchance":
                    paramsObject.CriticalHitChance = CalculateDiceNotationIfNeeded(value);
                    return true;
            }
            return false; // Not a custom case, allow dynamic handling
        }

        public static decimal CalculateDiceNotationIfNeeded(string value)
        {
            if (value.IsBooleanExpression())
                throw new ArgumentException($"{value} is a boolean expression, but is being evaluated as a number.");
            var valueWithCalculatedDiceExpressions = ReplaceDiceNotationWithValues(value);
            return new Expression(valueWithCalculatedDiceExpressions).Eval<decimal>();
        }

        public static string ReplaceDiceNotationWithValues(string expression)
        {
            return Regex.Replace(expression, EngineConstants.DiceNotationRegexPattern, match =>
            {
                var diceNotation = match.Value;
                decimal rollResult = new Dice().Roll(diceNotation, new LCGDieRoller(Rng)).Value;
                return rollResult.ToString();
            }, RegexOptions.IgnoreCase);
        }

        public static bool CalculateBooleanExpression(string value)
        {
            if (string.IsNullOrEmpty(value)) return true;
            if (!value.IsBooleanExpression())
                throw new ArgumentException($"{value} is not a boolean expression but is being evaluated as one.");
            return new Expression(value).Eval<bool>();
        }

        public static List<string> SplitExpression(string expression)
        {
            var tokens = ExpressionSplitterRegex
                .Split(expression)
                .Where(token => !string.IsNullOrWhiteSpace(token))
                .Select(token => token.Trim())
                .ToList();

            return tokens;
        }

        public static string ParseArgForExpression(string arg, Entity This, Entity Source, ITargetable Target)
        {
            var tokens = SplitExpression(arg);

            for (int i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];

                token = ParseArgForEntity(token, This, "this");
                token = ParseArgForEntity(token, Source, "source");
                if (Target is Entity targetAsEntity)
                    token = ParseArgForEntity(token, targetAsEntity, "target");
                else if (Target is Tile targetAsTile)
                    token = ParseArgForTile(token, targetAsTile, "target");
                token = ParseNamedFlags(token);

                tokens[i] = token;
            }

            // Second pass: handle function parsing and execution
            // Since logical operators might be in place, split the expression based on those, and handle function parsing for each part.
            tokens = SplitExpression(string.Join("", tokens));

            for (int i = 0; i < tokens.Count; i++)
            {
                var function = Function.FromExpression(tokens[i]);
                if (function != null)
                {
                    // Execute the function and replace the token with its result
                    tokens[i] = function.Execute(This, Source, Target);
                }
            }

            // Return the fully parsed expression
            return string.Join("", tokens);
        }

        private static string ParseArgForEntity(string arg, Entity e, string eName)
        {
            if (e == null) return arg;
            var parsedArg = arg;

            parsedArg = ParseEntityNames(parsedArg, e, eName);
            parsedArg = ParseObjectProperties(parsedArg, e, eName);

            return parsedArg;
        }

        private static string ParseArgForTile(string arg, Tile t, string eName)
        {
            if (t == null) return arg;
            var parsedArg = arg;

            parsedArg = ParseObjectProperties(parsedArg, t, eName);

            return parsedArg;
        }

        private static string ParseEntityNames(string arg, Entity e, string eName)
        {
            var parsedArg = arg;

            parsedArg = parsedArg.Replace($"{{{eName}}}", e.Name, StringComparison.InvariantCultureIgnoreCase);

            return parsedArg;
        }

        private static string ParseObjectProperties(string arg, object o, string eName)
        {
            var parsedArg = arg;

            // Match tokens like {eName.Stat:StatId}, also allowing nested properties {eName.SomeOtherProperty}
            var regex = new Regex(@$"\{{{eName}\.(Stat:([A-Za-z0-9_]+)|([A-Za-z0-9_.]+))\}}", RegexOptions.IgnoreCase);
            var matches = regex.Matches(parsedArg);

            foreach (Match match in matches)
            {
                var fullPropertyPath = match.Groups[1].Value; // Full match (Stat:StatId or regular property path)
                var statId = match.Groups[2].Value;           // Group 2 will have the StatId if it's a stat expression
                var propertyPath = match.Groups[3].Value;     // Group 3 will have the regular property path

                object propertyValue = null;

                // If it's a stat expression (Stat:StatId)
                if (!string.IsNullOrEmpty(statId))
                {
                    propertyValue = GetStatValue(o, statId);
                }
                else if (!string.IsNullOrEmpty(propertyPath)) // Otherwise, treat it as a nested property
                {
                    propertyValue = GetNestedPropertyValue(o, propertyPath);
                }

                // Replace the token with the formatted value
                if (propertyValue != null)
                {
                    var formattedValue = FormatParameterValue(propertyValue);
                    if (propertyValue is string stringValue && !stringValue.IsDiceNotation() && !stringValue.IsMathExpression())
                        formattedValue = $"\"{formattedValue}\"";
                    parsedArg = parsedArg.Replace(match.Value, FormatParameterValue(formattedValue), StringComparison.InvariantCultureIgnoreCase);
                }
                else
                {
                    parsedArg = parsedArg.Replace(match.Value, "0", StringComparison.InvariantCultureIgnoreCase); // Default for missing values
                }
            }

            return parsedArg;
        }

        private static object GetNestedPropertyValue(object obj, string propertyPath)
        {
            if (obj == null || string.IsNullOrEmpty(propertyPath)) return null;

            var properties = propertyPath.Split('.');
            object currentObject = obj;

            foreach (var property in properties)
            {
                if (currentObject == null) return null;

                var propertyInfo = currentObject.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
                    .FirstOrDefault(p => p.Name.Equals(property, StringComparison.InvariantCultureIgnoreCase));

                if (propertyInfo == null) return null;

                currentObject = propertyInfo.GetValue(currentObject);

                if (currentObject is Stat stat) 
                {
                    // This is only for backwards compatibility for {eName.StatName}
                    // (Where StatName is HP, HPRegeneration, MP, MPRegeneration, Attack, Defense, Movement, Accuracy or Evasion)
                    currentObject = stat.Current;
                }
                else if (currentObject is TileType tileType)
                {
                    currentObject = tileType.Id;
                }
                else if (propertyInfo.Name.Equals("AlteredStatuses", StringComparison.InvariantCultureIgnoreCase) && currentObject is List<AlteredStatus> alteredStatuses)
                {
                    currentObject = string.Join("/", alteredStatuses.Select(status => status.Id));
                }
                else if (propertyInfo.Name.Equals("MaxHunger", StringComparison.InvariantCultureIgnoreCase) && currentObject is Stat hungerStat)
                {
                    currentObject = hungerStat.Base;
                }
                else if (propertyInfo.PropertyType.IsEnum)
                {
                    currentObject = currentObject.ToString();
                }
            }

            return currentObject;
        }
        private static decimal? GetStatValue(object obj, string statId)
        {
            if (obj == null || string.IsNullOrEmpty(statId)) return null;

            if(obj is not Character c)
                throw new ArgumentException($"Tried to retrieve a stat from an element that is not a Character.");

            var parsedStatId = statId.ToLowerInvariant();
            var lookForMax = parsedStatId.StartsWith("max") && !parsedStatId.Equals("max", StringComparison.InvariantCultureIgnoreCase);

            var stat = c.UsedStats.FirstOrDefault(s => s.Id.Equals(parsedStatId, StringComparison.InvariantCultureIgnoreCase));

            if(stat != null)
                return lookForMax && stat.HasMax ? stat.BaseAfterModifications : stat.Current;
            
            return 0; // Defaulting to 0 if the stat does not exist should make literally no difference whatsoever
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
                    return decimal.Truncate(decimalValue).ToString(CultureInfo.InvariantCulture);
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
            var targetEvasion = target is Character t ? (int)t.Evasion.Current : 0;
            var sourceAccuracy = source is Character s ? (int)s.Accuracy.Current : 0;
            var baseAccuracy = (int)paramsObject.Accuracy;

            int adjustedAccuracy;

            if (paramsObject.BypassesAccuracyCheck)
                adjustedAccuracy = baseAccuracy;
            else
            {
                var accuracyModifier = 100 - targetEvasion;
                var adjustedSourceAccuracy = sourceAccuracy * accuracyModifier / 100;
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
