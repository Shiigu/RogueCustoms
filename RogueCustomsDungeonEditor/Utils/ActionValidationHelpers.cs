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

namespace RogueCustomsDungeonEditor.Utils
{
    public static class ActionValidationHelpers
    {
        public static bool TestNumericExpression(this string expression, bool checkDiceNotationAsWell, out string errorMessage)
        {
            try
            {
                var parsedExpression = ConvertArgsToPlaceholders(expression, "1");
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
            try
            {
                var parsedExpression = ConvertArgsToPlaceholders(expression, "1");
                _ = new Expression(parsedExpression).Eval<bool>();
                errorMessage = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        private static string ConvertArgsToPlaceholders(this string arg, string placeholder)
        {
            var parsedArg = arg;

            parsedArg = parsedArg.ParseArgsForPlaceHolder(placeholder, "player");
            parsedArg = parsedArg.ParseArgsForPlaceHolder(placeholder, "this");
            parsedArg = parsedArg.ParseArgsForPlaceHolder(placeholder, "source");
            parsedArg = parsedArg.ParseArgsForPlaceHolder(placeholder, "target");

            return parsedArg;
        }

        private static string ParseArgsForPlaceHolder(this string arg, string placeholder, string eName)
        {
            var parsedArg = arg;
            parsedArg = parsedArg.Replace($"{{{eName}}}", placeholder, StringComparison.InvariantCultureIgnoreCase);
            var entityTypes = new List<Type> { typeof(PlayerCharacter), typeof(NonPlayableCharacter), typeof(Item), typeof(AlteredStatus) };

            foreach (var entityType in entityTypes)
            {
                foreach (var property in entityType.GetProperties())
                {
                    string propertyName = property.Name;
                    string fieldToken = $"{{{eName}.{propertyName}}}";

                    if (parsedArg.Contains(fieldToken, StringComparison.InvariantCultureIgnoreCase))
                    {
                        parsedArg = parsedArg.Replace(fieldToken, placeholder, StringComparison.InvariantCultureIgnoreCase);
                    }
                }
            }

            return parsedArg;
        }

        private static bool IsDiceNotation(this string s)
        {
            return Regex.Match(s, RogueCustomsGameEngine.Utils.Constants.DiceNotationRegexPattern).Success;
        }

        public static bool HaveAllParametersBeenParsed(this Effect effect, Entity This, Entity Source, Entity Target)
        {
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, 0, effect.Params);

            var paramsObjectAsDictionary = ((IDictionary<string, object>)paramsObject);

            return paramsObjectAsDictionary.Count >= effect.Params.Length;
        }

        public static bool TestFunction(this Effect effect, Entity This, Entity Source, Entity Target)
        {
            return effect.Function(This, Source, Target, 0, out _, effect.Params);
        }
    }
}
