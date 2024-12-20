﻿using org.matheval;

using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace RogueCustomsGameEngine.Utils.Helpers
{
    public static class StringHelpers
    {
        public static bool IsDiceNotation(this string s)
        {
            return Regex.Match(s, EngineConstants.DiceNotationRegexPattern).Success;
        }
        public static bool IsIntervalNotation(this string s)
        {
            return Regex.Match(s, EngineConstants.IntervalRegexPattern, RegexOptions.IgnoreCase).Success;
        }

        public static bool IsMathExpression(this string input)
        {
            try
            {
                new Expression(input).Eval<decimal>();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsBooleanExpression(this string input)
        {
            var pattern = @"[<>!=]=?|&&|\|\||\b(true|false)\b";

            return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }

        public static GameColor ToGameColor(this string s)
        {
            var colorValues = s.Split(',').Select(s => byte.Parse(s)).ToArray();
            var r = colorValues[0];
            var g = colorValues[1];
            var b = colorValues[2];
            var a = colorValues[3];
            return new GameColor
            {
                R = r,
                G = g,
                B = b,
                A = a
            };
        }

        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }

        public static string Format(this string input, object p)
        {
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(p))
                input = input.Replace("{" + prop.Name + "}", (prop.GetValue(p) ?? "(null)").ToString());

            return input;
        }
        public static string TrimStart(this string value, string removeString)
        {
            int index = value.IndexOf(removeString, StringComparison.Ordinal);
            return index < 0 ? value : value.Remove(index, removeString.Length);
        }

        public static string TrimSurrounding(this string input, char surroundingChar)
        {
            if (input.Length >= 2 &&
                (input.StartsWith(surroundingChar) && input.EndsWith(surroundingChar)))
            {
                return input.Substring(1, input.Length - 2);
            }
            return input;
        }

        public static string TrimSurroundingQuotes(this string input)
        {
            return input.TrimSurrounding('\"').TrimSurrounding('\'');
        }
    }
}
