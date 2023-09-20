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

namespace RogueCustomsDungeonEditor.Utils
{
    public static class ActionValidationHelpers
    {
        public static bool TestExpression(this string expression, bool checkDiceNotationAsWell, out string errorMessage)
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

        private static string ConvertArgsToPlaceholders(this string arg, string placeholder)
        {
            var parsedArg = arg;

            parsedArg = parsedArg.ParseArgsForPlaceHolder(placeholder, "this");
            parsedArg = parsedArg.ParseArgsForPlaceHolder(placeholder, "source");
            parsedArg = parsedArg.ParseArgsForPlaceHolder(placeholder, "target");

            return parsedArg;
        }

        private static string ParseArgsForPlaceHolder(this string arg, string placeholder, string eName)
        {
            var parsedArg = arg;

            parsedArg = parsedArg.Replace($"{{{eName}}}", placeholder, StringComparison.InvariantCultureIgnoreCase);

            if (parsedArg.Contains($"{{{eName}.weapon}}", StringComparison.InvariantCultureIgnoreCase))
                parsedArg = parsedArg.Replace($"{{{eName}.weapon}}", placeholder, StringComparison.InvariantCultureIgnoreCase);

            if (parsedArg.Contains($"{{{eName}.armor}}", StringComparison.InvariantCultureIgnoreCase))
                parsedArg = parsedArg.Replace($"{{{eName}.armor}}", placeholder, StringComparison.InvariantCultureIgnoreCase);

            if (parsedArg.Contains($"{{{eName}.hp}}", StringComparison.InvariantCultureIgnoreCase))
                parsedArg = parsedArg.Replace($"{{{eName}.hp}}", placeholder, StringComparison.InvariantCultureIgnoreCase);

            if (parsedArg.Contains($"{{{eName}.maxhp}}", StringComparison.InvariantCultureIgnoreCase))
                parsedArg = parsedArg.Replace($"{{{eName}.maxhp}}", placeholder, StringComparison.InvariantCultureIgnoreCase);

            if (parsedArg.Contains($"{{{eName}.damage}}", StringComparison.InvariantCultureIgnoreCase))
                parsedArg = parsedArg.Replace($"{{{eName}.damage}}", placeholder, StringComparison.InvariantCultureIgnoreCase);

            if (parsedArg.Contains($"{{{eName}.attack}}", StringComparison.InvariantCultureIgnoreCase))
                parsedArg = parsedArg.Replace($"{{{eName}.attack}}", placeholder, StringComparison.InvariantCultureIgnoreCase);

            if (parsedArg.Contains($"{{{eName}.defense}}", StringComparison.InvariantCultureIgnoreCase))
                parsedArg = parsedArg.Replace($"{{{eName}.defense}}", placeholder, StringComparison.InvariantCultureIgnoreCase);

            if (parsedArg.Contains($"{{{eName}.movement}}", StringComparison.InvariantCultureIgnoreCase))
                parsedArg = parsedArg.Replace($"{{{eName}.movement}}", placeholder, StringComparison.InvariantCultureIgnoreCase);

            if (parsedArg.Contains($"{{{eName}.hpregeneration}}", StringComparison.InvariantCultureIgnoreCase))
                parsedArg = parsedArg.Replace($"{{{eName}.hpregeneration}}", placeholder, StringComparison.InvariantCultureIgnoreCase);

            if (parsedArg.Contains($"{{{eName}.mitigation}}", StringComparison.InvariantCultureIgnoreCase))
                parsedArg = parsedArg.Replace($"{{{eName}.mitigation}}", placeholder, StringComparison.InvariantCultureIgnoreCase);

            if (parsedArg.Contains($"{{{eName}.experiencepayout}}", StringComparison.InvariantCultureIgnoreCase))
                parsedArg = parsedArg.Replace($"{{{eName}.experiencepayout}}", placeholder, StringComparison.InvariantCultureIgnoreCase);

            if (parsedArg.Contains($"{{{eName}.owner}}", StringComparison.InvariantCultureIgnoreCase))
                parsedArg = parsedArg.Replace($"{{{eName}.owner}}", placeholder, StringComparison.InvariantCultureIgnoreCase);

            if (parsedArg.Contains($"{{{eName}.power}}", StringComparison.InvariantCultureIgnoreCase))
                parsedArg = parsedArg.Replace($"{{{eName}.power}}", placeholder, StringComparison.InvariantCultureIgnoreCase);

            if (parsedArg.Contains($"{{{eName}.turnlength}}", StringComparison.InvariantCultureIgnoreCase))
                parsedArg = parsedArg.Replace($"{{{eName}.turnlength}}", placeholder, StringComparison.InvariantCultureIgnoreCase);

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
