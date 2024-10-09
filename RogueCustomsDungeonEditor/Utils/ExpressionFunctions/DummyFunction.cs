using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities.Interfaces;

using RogueCustomsGameEngine.Game.Entities;

using RogueCustomsGameEngine.Utils.Expressions;
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
#pragma warning disable CS8620 // El argumento no se puede usar para el parámetro debido a las diferencias en la nulabilidad de los tipos de referencia.
#pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL

namespace RogueCustomsDungeonEditor.Utils.ExpressionFunctions
{
    public class DummyFunction
    {
        public string FunctionName { get; }
        public List<object> Parameters { get; }
        public string Result { get; private set; }

        private DummyFunction(string functionName, List<object> parameters)
        {
            FunctionName = functionName;
            Parameters = parameters;
        }

        public static DummyFunction FromExpression(string expression)
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

                return new DummyFunction(functionName, parsedParams);
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
        public string Execute(Entity This, Entity Source, ITargetable Target)
        {
            // Recursively evaluate parameters first
            var evaluatedParameters = Parameters.Select(param =>
            {
                if (param is DummyFunction nestedFunction)
                {
                    // Recursively evaluate nested function
                    return nestedFunction.Execute(This, Source, Target);
                }

                // If it's a simple value (string, number), just return it
                return param.ToString();
            }).ToArray();

            // Determine the entity or tile context for the function call
            Entity e = null;
            Tile t = null;

            if (Target is Entity targetEntity)
            {
                e = targetEntity;
            }
            else if (Target is Tile targetTile)
            {
                t = targetTile;
            }

            // Now invoke the function
            return InvokeAppropriateFunction(FunctionName, This, Source, e, t, evaluatedParameters);
        }

        private string InvokeAppropriateFunction(string functionName, Entity This, Entity Source, Entity e, Tile t, string[] parameters)
        {
            return InvokeDummyExpressionFunction(functionName, This, Source, e, parameters);
        }

        private string InvokeDummyExpressionFunction(string functionName, Entity This, Entity Source, Entity Target, string[] parameters)
        {
            var type = typeof(DummyExpressionFunctions);
            var method = type.GetMethod(functionName.ToUpperInvariant(), BindingFlags.Static | BindingFlags.Public);
            if (method == null) return string.Join(",", parameters);

            var result = method.Invoke(null, new object[] { This, Source, Target, parameters });

            return result?.ToString() ?? string.Empty;
        }
    }
}

#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
#pragma warning restore CS8620 // El argumento no se puede usar para el parámetro debido a las diferencias en la nulabilidad de los tipos de referencia.
#pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
