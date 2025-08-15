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
using RogueCustomsGameEngine.Utils.Representation;
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

            var entityToCheck = GetEntityByName(entityName, "HasStatus", This, Source, Target);

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

            var entityToCheck = GetEntityByName(entityName, "DoesNotHaveStatus", This, Source, Target);

            var statusExists = HASSTATUS(This, Source, Target, parameters);
            return (!(bool.Parse(statusExists))).ToString();
        }

        public static string CONCAT(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            if (parameters.Length < 2) throw new ArgumentException("Invalid parameters for Concat.");

            var concatResult = new StringBuilder();

            foreach (var param in parameters)
            {
                concatResult.Append(param.TrimSurroundingQuotes());
            }

            return concatResult.ToString();
        }

        public static string REPLACE(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            if (parameters.Length != 3) throw new ArgumentException("Invalid parameters for Replace.");

            return parameters[0].TrimSurroundingQuotes().Replace(parameters[1].TrimSurroundingQuotes(), parameters[2].TrimSurroundingQuotes()).ToString();
        }

        public static string REVERSE(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            if (parameters.Length != 1) throw new ArgumentException("Invalid parameters for Reverse.");

            return parameters[0].TrimSurroundingQuotes().Reverse().ToString() ?? string.Empty;
        }

        public static string LOWER(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            if (parameters.Length != 1) throw new ArgumentException("Invalid parameters for Lower.");

            return parameters[0].TrimSurroundingQuotes().ToLowerInvariant();
        }

        public static string UPPER(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            if (parameters.Length != 1) throw new ArgumentException("Invalid parameters for Upper.");

            return parameters[0].TrimSurroundingQuotes().ToUpperInvariant();
        }

        public static string TRIM(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            if (parameters.Length != 1) throw new ArgumentException("Invalid parameters for Trim.");

            return parameters[0].TrimSurroundingQuotes().Trim();
        }

        public static string FLOOR(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            if (parameters.Length != 2) throw new ArgumentException("Invalid parameters for Floor.");
            
            var numberToFloor = parameters[0].IsMathExpression() ? new Expression(parameters[0]).Eval<decimal>() : decimal.Parse(parameters[0]);
            var decimals = parameters[1].IsMathExpression() ? new Expression(parameters[1]).Eval<int>() : int.Parse(parameters[1]);

            var factor = (decimal)Math.Pow(10, decimals);
            var result = Math.Floor(numberToFloor * factor) / factor;
            var formatString = decimals == 0 ? "0" : "0." + new string('#', decimals);

            return result.ToString(formatString, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string CEILING(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            if (parameters.Length != 2) throw new ArgumentException("Invalid parameters for Ceiling.");

            var numberToRound = parameters[0].IsMathExpression() ? new Expression(parameters[0]).Eval<decimal>() : decimal.Parse(parameters[0]);
            var decimals = parameters[1].IsMathExpression() ? new Expression(parameters[1]).Eval<int>() : int.Parse(parameters[1]);

            var factor = (decimal)Math.Pow(10, decimals);
            var result = Math.Ceiling(numberToRound * factor) / factor;
            var formatString = decimals == 0 ? "0" : "0." + new string('#', decimals);

            return result.ToString(formatString, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string USESSTAT(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            if (parameters.Length != 2) throw new ArgumentException("Invalid parameters for UsesStat.");

            var entityName = parameters[0].ToLower();

            var entityToCheck = GetEntityByName(entityName, "UsesStat", This, Source, Target);

            var statId = parameters[1];

            if (entityToCheck is Character character)
            {
                var statusExists = character.UsedStats.Exists(als => als.Id.Equals(statId, StringComparison.InvariantCultureIgnoreCase));
                return statusExists.ToString();
            }

            return false.ToString();
        }

        public static string CURRENTWEAPON(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            if (parameters.Length != 1) throw new ArgumentException("Invalid parameters for CurrentWeapon.");

            var entityName = parameters[0].ToLower();

            var entityToCheck = GetEntityByName(entityName, "CurrentWeapon", This, Source, Target);

            if (entityToCheck is not Character c)
                throw new ArgumentException("Invalid entity in CurrentWeapon.");

            return $"\"{c.Weapon.ClassId}\"";
        }

        public static string CURRENTARMOR(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            if (parameters.Length != 1) throw new ArgumentException("Invalid parameters for CurrentArmor.");

            var entityName = parameters[0].ToLower();

            var entityToCheck = GetEntityByName(entityName, "CurrentArmor", This, Source, Target);

            if (entityToCheck is not Character c)
                throw new ArgumentException("Invalid entity in CurrentArmor.");

            return $"\"{c.Armor.ClassId}\"";
        }

        public static string DISTANCEBETWEEN(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            if (parameters.Length != 2) throw new ArgumentException("Invalid parameters for DistanceBetween.");

            var entity1Name = parameters[0].ToLower();

            var entity1ToCheck = GetEntityByName(entity1Name, "DistanceBetween", This, Source, Target);

            var entity2Name = parameters[1].ToLower();

            var entity2ToCheck = GetEntityByName(entity2Name, "DistanceBetween", This, Source, Target);

            if (entity1ToCheck is not Character c1 || c1.Position == null)
                throw new ArgumentException("Invalid entity in DistanceBetween.");
            if (entity2ToCheck is not Character c2 || c2.Position == null)
                throw new ArgumentException("Invalid entity in DistanceBetween.");

            return Math.Floor(GamePoint.Distance(c1.Position, c2.Position)).ToString();
        }
        public static string AREINTHESAMEROOM(Entity This, Entity Source, Entity Target, string[] parameters)
        {
            if (parameters.Length != 2) throw new ArgumentException("Invalid parameters for AreInSameRoom.");

            var entity1Name = parameters[0].ToLower();

            var entity1ToCheck = GetEntityByName(entity1Name, "AreInSameRoom", This, Source, Target);

            var entity2Name = parameters[1].ToLower();

            var entity2ToCheck = GetEntityByName(entity2Name, "AreInSameRoom", This, Source, Target);

            if (entity1ToCheck is not Character c1 || c1.Position == null)
                throw new ArgumentException("Invalid entity in AreInSameRoom.");
            if (entity2ToCheck is not Character c2 || c2.Position == null)
                throw new ArgumentException("Invalid entity in AreInSameRoom.");

            var c1IsInHallwayTile = c1.ContainingTile.Type == TileType.Hallway || c1.ContainingRoom == null || c1.ContainingTile.IsConnectorTile;
            var c2IsInHallwayTile = c2.ContainingTile.Type == TileType.Hallway || c2.ContainingRoom == null || c2.ContainingTile.IsConnectorTile;

            if (!c1IsInHallwayTile && !c2IsInHallwayTile)
                return (c1.ContainingRoom == c2.ContainingRoom).ToString();

            return ((Map.GetFOVTilesWithinDistance(c1.Position, EngineConstants.FullRoomSightRangeForHallways).Contains(c2.ContainingTile) || Map.GetFOVTilesWithinDistance(c2.Position, EngineConstants.FullRoomSightRangeForHallways).Contains(c1.ContainingTile))).ToString();
        }

        private static Entity GetEntityByName(string name, string functionName, Entity This, Entity Source, Entity Target)
        {
            return name.ToLowerInvariant() switch
            {
                "this" => This,
                "source" => Source,
                "target" => Target,
                "player" => This.Map.Player,
                _ => throw new ArgumentException($"Invalid entity name {name} in {functionName}.")
            };
        }
    }
}
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
