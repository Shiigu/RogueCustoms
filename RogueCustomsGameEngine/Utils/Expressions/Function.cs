using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using org.matheval.Functions;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.Effects.Utils;
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
#pragma warning disable CS8620 // El argumento no se puede usar para el parámetro debido a las diferencias en la nulabilidad de los tipos de referencia.
#pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL

namespace RogueCustomsGameEngine.Utils.Expressions
{
    public class Function
    {
        public string FunctionName { get; }
        public List<object> Parameters { get; }
        public string Result { get; private set; }

        private Function(string functionName, List<object> parameters)
        {
            FunctionName = functionName;
            Parameters = parameters;
        }

        public static Function FromExpression(string expression)
        {
            // Check if expression matches the function format: FUNC_NAME(param1, param2, ...)
            var functionPattern = new Regex(@"^([a-zA-Z_][a-zA-Z0-9_]*)\((.*)\)$");
            var match = functionPattern.Match(expression);

            if (match.Success)
            {
                string functionName = match.Groups[1].Value;
                string paramList = match.Groups[2].Value;

                var parameters = TokenizeExpression(paramList);

                // Parse parameters into Function objects or literals
                var parsedParams = new List<object>();
                foreach (var param in parameters)
                {
                    if (functionPattern.IsMatch(param))
                    {
                        parsedParams.Add(FromExpression(param));
                    }
                    else
                    {
                        parsedParams.Add(param);
                    }
                }

                return new Function(functionName, parsedParams);
            }

            return null;  // Not a function expression
        }

        public static List<string> TokenizeExpression(string expression)
        {
            var tokens = new List<string>();
            var currentToken = new StringBuilder();
            int nestedLevel = 0;

            foreach (char c in expression)
            {
                if (c == ',' && nestedLevel == 0)
                {
                    tokens.Add(currentToken.ToString().Trim());
                    currentToken.Clear();
                }
                else
                {
                    if (c == '(' || c == '{' || c == '[')
                        nestedLevel++;
                    else if (c == ')' || c == '}' || c == ']')
                        nestedLevel--;

                    currentToken.Append(c);
                }
            }

            if (currentToken.Length > 0)
                tokens.Add(currentToken.ToString().Trim());

            return tokens;
        }
        public string Execute(EffectCallerParams args)
        {
            // Recursively evaluate parameters first
            var evaluatedParameters = Parameters.Select(param =>
            {
                if (param is Function nestedFunction)
                {
                    // Recursively evaluate nested function
                    return nestedFunction.Execute(args);
                }

                // If it's a simple value (string, number), just return it
                return param.ToString();
            }).ToArray();

            // Now invoke the function
            return InvokeAppropriateFunction(FunctionName, args, evaluatedParameters);
        }

        private string InvokeAppropriateFunction(string functionName, EffectCallerParams args, string[] parameters)
        {
            if (args.OriginalTarget is Entity)
            {
                return InvokeExpressionFunction(functionName, args, parameters);
            }
            else if (args.OriginalTarget is Tile)
            {
                return InvokeTileExpressionFunction(functionName, args, parameters);
            }

            // Return the parameters joined if no function is matched
            return string.Join(",", parameters);
        }

        private string InvokeExpressionFunction(string functionName, EffectCallerParams args, string[] parameters)
        {
            // Assuming functions are defined in a class called ExpressionFunctions
            var type = typeof(ExpressionFunctions);
            var method = type.GetMethod(functionName.ToUpperInvariant(), BindingFlags.Static | BindingFlags.Public);
            if (method == null) return string.Join(",", parameters);

            var result = method.Invoke(null, new object[] { args, parameters });

            return result?.ToString() ?? string.Empty;
        }

        private string InvokeTileExpressionFunction(string functionName, EffectCallerParams args, string[] parameters)
        {
            // Assuming functions are defined in a class called ExpressionFunctions
            var type = typeof(TileExpressionFunctions);
            var method = type.GetMethod(functionName.ToUpperInvariant(), BindingFlags.Static | BindingFlags.Public);
            if (method == null) return string.Join(",", parameters);

            var result = method.Invoke(null, new object[] { args, parameters });

            return result?.ToString() ?? string.Empty;
        }
    }
}
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
#pragma warning restore CS8620 // El argumento no se puede usar para el parámetro debido a las diferencias en la nulabilidad de los tipos de referencia.
#pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
