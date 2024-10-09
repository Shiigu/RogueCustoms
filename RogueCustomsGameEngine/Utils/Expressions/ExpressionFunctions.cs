using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using org.matheval;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.Helpers;
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.

namespace RogueCustomsGameEngine.Utils.Expressions
{
    public static class ExpressionFunctions
    {
        private static RngHandler rngHandler;
        private static Map Map;

        public static void Setup(RngHandler rng, Map map)
        {
            rngHandler = rng;
            Map = map;
        }

        public static string RNG(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            if (parameters.Length != 2 || !int.TryParse(parameters[0], out int min) || !int.TryParse(parameters[1], out int max) || min > max)
                throw new ArgumentException("Invalid parameters for rng.");

            return rngHandler.Next(min, max + 1).ToString();
        }

        public static string FLAGEXISTS(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            if (parameters.Length != 1) throw new ArgumentException("Invalid parameters for FlagExists.");

            string flagName = parameters[0];
            bool flagExists = Map.HasFlag(flagName); 
            return flagExists.ToString();
        }

        public static string HASSTATUS(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            if (parameters.Length != 2) throw new ArgumentException("Invalid parameters for HasStatus.");

            var entityName = parameters[0].ToLower();

            var entityToCheck = entityName.ToLowerInvariant() switch { 
                "this" => This,
                "source" => Source,
                "target" => Target,
                _ => throw new ArgumentException("Invalid entity name in HasStatus.")
            };

            var statusName = parameters[1];

            if (entityToCheck is Character character)
            {
                var statusExists = character.AlteredStatuses.Exists(als => als.ClassId.Equals(statusName, StringComparison.InvariantCultureIgnoreCase));
                return statusExists.ToString();
            }

            return false.ToString();
        }

        public static string DOESNOTHAVESTATUS(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            if (parameters.Length != 2) throw new ArgumentException("Invalid parameters for DoesNotHaveStatus.");

            var entityName = parameters[0].ToLower();

            var entityToCheck = entityName.ToLowerInvariant() switch
            {
                "this" => This,
                "source" => Source,
                "target" => Target,
                _ => throw new ArgumentException("Invalid entity name in DoesNotHaveStatus.")
            };

            var statusExists = HASSTATUS(This, Source, Target, parameters);
            return (!(bool.Parse(statusExists))).ToString();
        }

        public static string CONCAT(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            if (parameters.Length < 2) throw new ArgumentException("Invalid parameters for Concat.");

            var concatResult = new StringBuilder();

            foreach (var param in parameters)
            {
                concatResult.Append(param.Trim('"'));
            }

            return concatResult.ToString();
        }

        public static string REPLACE(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            if (parameters.Length != 3) throw new ArgumentException("Invalid parameters for Replace.");

            return parameters[0].Trim('"').Replace(parameters[1].Trim('"'), parameters[2].Trim('"')).ToString();
        }

        public static string REVERSE(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            if (parameters.Length != 1) throw new ArgumentException("Invalid parameters for Reverse.");

            return parameters[0].Trim('"').Reverse().ToString() ?? string.Empty;
        }

        public static string LOWER(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            if (parameters.Length != 1) throw new ArgumentException("Invalid parameters for Lower.");

            return parameters[0].Trim('"').ToLowerInvariant();
        }

        public static string UPPER(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            if (parameters.Length != 1) throw new ArgumentException("Invalid parameters for Upper.");

            return parameters[0].Trim('"').ToUpperInvariant();
        }

        public static string TRIM(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            if (parameters.Length != 1) throw new ArgumentException("Invalid parameters for Trim.");

            return parameters[0].Trim('"').Trim();
        }

        public static string FLOOR(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            if (parameters.Length != 1) throw new ArgumentException("Invalid parameters for Floor.");
            
            var numberToFloor = parameters[0].IsMathExpression() ? new Expression(parameters[0]).Eval<decimal>() : decimal.Parse(parameters[0]);

            return ((int) Math.Floor(numberToFloor)).ToString();
        }

        public static string CEILING(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            if (parameters.Length != 2) throw new ArgumentException("Invalid parameters for Ceiling.");

            var numberToRound = parameters[0].IsMathExpression() ? new Expression(parameters[0]).Eval<decimal>() : decimal.Parse(parameters[0]);
            var decimals = parameters[1].IsMathExpression() ? new Expression(parameters[1]).Eval<int>() : int.Parse(parameters[1]);

            return ((int) Math.Round(numberToRound, decimals)).ToString();
        }
    }
}
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
