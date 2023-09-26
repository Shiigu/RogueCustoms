using D20Tek.DiceNotation;
using D20Tek.DiceNotation.DieRoller;
using org.matheval;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace RogueCustomsGameEngine.Utils.Helpers
{
    public static class ActionHelpers
    {
        private static List<string> FieldsToConsider = new List<string>
        {
            "Weapon",
            "Armor",
            "HP",
            "MaxHP",
            "Attack",
            "Damage",
            "Defense",
            "Mitigation",
            "Movement",
            "HPRegeneration",
            "ExperiencePayout",
            "Owner",
            "Power",
            "TurnLength"
        };
        public static Map Map;

        public static ExpandoObject ParseParams(Entity This, Entity Source, Entity Target, int previousEffectOutput, params (string ParamName, string Value)[] args)
        {
            dynamic paramsObject = new ExpandoObject();
            foreach (var (ParamName, Value) in args)
            {
                var paramName = ParamName.ToLower();
                var value = ParseArgForAction(Map.Locale[Value], This, Source, Target);
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
                        else if(value.Equals("source", StringComparison.InvariantCultureIgnoreCase))
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
                        switch(value.ToLowerInvariant())
                        {
                            case "maxhp":
                                paramsObject.StatAlterationList = c.MaxHPModifications;
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
                    case "character":
                        paramsObject.Character = value[0];
                        break;
                    case "color":
                        paramsObject.Color = value.ToGameColor();
                        break;
                    case "output":
                        paramsObject.Output = previousEffectOutput;
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
            return paramsObject;
        }

        private static decimal CalculateDiceNotationIfNeeded(string value)
        {
            if (value.IsDiceNotation())
                return new Dice().Roll(value, new RandomDieRoller()).Value;
            return new Expression(value).Eval<decimal>();
        }

        private static string ParseValueForTextDisplay(string value)
        {
            const string functionMatchRegex = "(?:LEFT|RIGHT|MID|REVERSE|LOWER|UPPER|PROPER|TRIM|TEXT|REPLACE|SUBSTITUTE|CONCAT)";
            if (Regex.Match(value, functionMatchRegex).Success)
                return new Expression($"TEXT({value})").Eval<string>();
            return value;
        }

        private static string ParseArgForAction(string arg, Entity This, Entity Source, Entity Target)
        {
            var parsedArg = arg;

            parsedArg = ParseArgForEntity(parsedArg, This, "this");
            parsedArg = ParseArgForEntity(parsedArg, Source, "source");
            parsedArg = ParseArgForEntity(parsedArg, Target, "target");

            return parsedArg;
        }

        private static string ParseArgForEntity(string arg, Entity e, string eName)
        {
            if (e == null) return arg;
            var parsedArg = arg;

            parsedArg = parsedArg.Replace($"{{{eName}}}", e.Name, StringComparison.InvariantCultureIgnoreCase);

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
                        else
                        {
                            parsedArg = parsedArg.Replace(fieldToken, FormatParameterValue(propertyValue), StringComparison.InvariantCultureIgnoreCase);
                        }
                    }
                }
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
                    return decimalValue.ToString("F2", CultureInfo.GetCultureInfo("en-US"));
                }
            }
            else
            {
                return parameterValue.ToString();
            }
        }
    }
}
