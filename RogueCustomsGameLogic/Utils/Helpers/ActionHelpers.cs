using D20Tek.DiceNotation;
using D20Tek.DiceNotation.DieRoller;
using org.matheval;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using System;
using System.Dynamic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace RogueCustomsGameEngine.Utils.Helpers
{
    public static class ActionHelpers
    {
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
                        if (value.Equals("this"))
                            paramsObject.Attacker = This;
                        else if (value.Equals("source"))
                            paramsObject.Attacker = Source;
                        else if (value.Equals("target"))
                            paramsObject.Attacker = Target;
                        break;
                    case "source":
                        if (value.Equals("this"))
                            paramsObject.Source = This;
                        else if(value.Equals("source"))
                            paramsObject.Source = Source;
                        else if (value.Equals("target"))
                            paramsObject.Source = Target;
                        break;
                    case "target":
                        if (value.Equals("this"))
                            paramsObject.Target = This;
                        else if (value.Equals("source"))
                            paramsObject.Target = Source;
                        else if (value.Equals("target"))
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
                    case "canbestacked":
                        paramsObject.CanBeStacked = new Expression(value).Eval<bool>();
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
            const string functionMatchRegex = @"(?:LEFT|RIGHT|MID|REVERSE|LOWER|UPPER|PROPER|TRIM|TEXT|REPLACE|SUBSTITUTE|CONCAT)";
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
            var parsedArg = arg;

            if (e is Character c)
                return ParseArgForCharacter(parsedArg, c, eName);
            else if (e is Item i)
                return ParseArgForItem(parsedArg, i, eName);
            else if (e is AlteredStatus als)
                return ParseArgForAlteredStatus(parsedArg, als, eName);

            return parsedArg;
        }

        private static string ParseArgForCharacter(string arg, Character c, string cName)
        {
            var parsedArg = arg;

            parsedArg = parsedArg.Replace($"{{{cName}}}", c.Name, StringComparison.InvariantCultureIgnoreCase);

            if (parsedArg.Contains($"{{{cName}.weapon}}", StringComparison.InvariantCultureIgnoreCase) && c.Weapon != null)
                parsedArg = parsedArg.Replace($"{{{cName}.weapon}}", c.Weapon.Name, StringComparison.InvariantCultureIgnoreCase);

            if (parsedArg.Contains($"{{{cName}.armor}}", StringComparison.InvariantCultureIgnoreCase) && c.Armor != null)
                parsedArg = parsedArg.Replace($"{{{cName}.armor}}", c.Armor.Name, StringComparison.InvariantCultureIgnoreCase);

            if (parsedArg.Contains($"{{{cName}.hp}}", StringComparison.InvariantCultureIgnoreCase) && c.HP > 0)
                parsedArg = parsedArg.Replace($"{{{cName}.hp}}", c.HP.ToString(), StringComparison.InvariantCultureIgnoreCase);

            if (parsedArg.Contains($"{{{cName}.maxhp}}", StringComparison.InvariantCultureIgnoreCase) && c.MaxHP > 0)
                parsedArg = parsedArg.Replace($"{{{cName}.maxhp}}", c.MaxHP.ToString(), StringComparison.InvariantCultureIgnoreCase);

            if (parsedArg.Contains($"{{{cName}.damage}}", StringComparison.InvariantCultureIgnoreCase) && c.Damage != null)
                parsedArg = parsedArg.Replace($"{{{cName}.damage}}", c.Damage, StringComparison.InvariantCultureIgnoreCase);

            if (parsedArg.Contains($"{{{cName}.attack}}", StringComparison.InvariantCultureIgnoreCase))
                parsedArg = parsedArg.Replace($"{{{cName}.attack}}", c.Attack.ToString(), StringComparison.InvariantCultureIgnoreCase);

            if (parsedArg.Contains($"{{{cName}.defense}}", StringComparison.InvariantCultureIgnoreCase))
                parsedArg = parsedArg.Replace($"{{{cName}.defense}}", c.Defense.ToString(), StringComparison.InvariantCultureIgnoreCase);

            if (parsedArg.Contains($"{{{cName}.movement}}", StringComparison.InvariantCultureIgnoreCase))
                parsedArg = parsedArg.Replace($"{{{cName}.movement}}", c.Movement.ToString(), StringComparison.InvariantCultureIgnoreCase);

            if (parsedArg.Contains($"{{{cName}.hpregeneration}}", StringComparison.InvariantCultureIgnoreCase))
                parsedArg = parsedArg.Replace($"{{{cName}.hpregeneration}}", c.HPRegeneration.ToString(new CultureInfo("en-US")), StringComparison.InvariantCultureIgnoreCase);

            if (parsedArg.Contains($"{{{cName}.mitigation}}", StringComparison.InvariantCultureIgnoreCase) && c.Mitigation != null)
                parsedArg = parsedArg.Replace($"{{{cName}.mitigation}}", c.Mitigation, StringComparison.InvariantCultureIgnoreCase);

            if (parsedArg.Contains($"{{{cName}.experiencepayout}}", StringComparison.InvariantCultureIgnoreCase) && c.ExperiencePayout > 0)
                parsedArg = parsedArg.Replace($"{{{cName}.experiencepayout}}", c.ExperiencePayout.ToString(), StringComparison.InvariantCultureIgnoreCase);

            return parsedArg;
        }

        private static string ParseArgForItem(string arg, Item i, string iName)
        {
            var parsedArg = arg;

            parsedArg = parsedArg.Replace($"{{{iName}}}", i.Name, StringComparison.InvariantCultureIgnoreCase);

            if (parsedArg.Contains($"{{{iName}.owner}}", StringComparison.InvariantCultureIgnoreCase) && i.Owner != null)
                parsedArg = parsedArg.Replace($"{{{iName}.owner}}", i.Owner.Name, StringComparison.InvariantCultureIgnoreCase);

            if (parsedArg.Contains($"{{{iName}.power}}", StringComparison.InvariantCultureIgnoreCase) && i.Power != null)
                parsedArg = parsedArg.Replace($"{{{iName}.power}}", i.Power, StringComparison.InvariantCultureIgnoreCase);

            return parsedArg;
        }

        private static string ParseArgForAlteredStatus(string arg, AlteredStatus als, string alsName)
        {
            var parsedArg = arg;

            parsedArg = parsedArg.Replace($"{{{alsName}}}", als.Name, StringComparison.InvariantCultureIgnoreCase);

            if (parsedArg.Contains($"{{{alsName}.turnlength}}", StringComparison.InvariantCultureIgnoreCase))
                parsedArg = parsedArg.Replace($"{{{alsName}.turnlength}}", als.TurnLength.ToString(), StringComparison.InvariantCultureIgnoreCase);

            if (parsedArg.Contains($"{{{alsName}.power}}", StringComparison.InvariantCultureIgnoreCase))
                parsedArg = parsedArg.Replace($"{{{alsName}.power}}", als.Power.ToString(), StringComparison.InvariantCultureIgnoreCase);

            return parsedArg;
        }
    }
}
