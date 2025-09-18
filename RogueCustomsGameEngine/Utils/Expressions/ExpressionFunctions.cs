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
using RogueCustomsGameEngine.Utils.Effects.Utils;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.Representation;
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.

namespace RogueCustomsGameEngine.Utils.Expressions
{
    public static class ExpressionFunctions
    {
        private static RngHandler Rng;
        private static Map Map;

        public static void Setup(RngHandler rng, Map map)
        {
            Rng = rng;
            Map = map;
        }

        public static string RNG(EffectCallerParams args, string[] parameters)
        {
            if (parameters.Length != 2 || !int.TryParse(parameters[0], out int min) || !int.TryParse(parameters[1], out int max) || min > max)
                throw new ArgumentException("Invalid parameters for rng.");

            return Rng.Next(min, max + 1).ToString();
        }

        public static string FLAGEXISTS(EffectCallerParams args, string[] parameters)
        {
            if (parameters.Length != 1) throw new ArgumentException("Invalid parameters for FlagExists.");

            string flagName = parameters[0];
            bool flagExists = Map.HasFlag(flagName); 
            return flagExists.ToString();
        }

        public static string HASSTATUS(EffectCallerParams args, string[] parameters)
        {
            if (parameters.Length != 2) throw new ArgumentException("Invalid parameters for HasStatus.");

            var entityName = parameters[0].ToLower();

            var entityToCheck = GetEntityByName(entityName, "HasStatus", args);

            var statusName = parameters[1];

            if (entityToCheck is Character character)
            {
                var statusExists = character.AlteredStatuses.Exists(als => als.ClassId.Equals(statusName, StringComparison.InvariantCultureIgnoreCase));
                return statusExists.ToString();
            }

            return false.ToString();
        }

        public static string DOESNOTHAVESTATUS(EffectCallerParams args, string[] parameters)
        {
            if (parameters.Length != 2) throw new ArgumentException("Invalid parameters for DoesNotHaveStatus.");

            var entityName = parameters[0].ToLower();

            var entityToCheck = GetEntityByName(entityName, "DoesNotHaveStatus", args);

            var statusExists = HASSTATUS(args, parameters);
            return (!(bool.Parse(statusExists))).ToString();
        }

        public static string CONCAT(EffectCallerParams args, string[] parameters)
        {
            if (parameters.Length < 2) throw new ArgumentException("Invalid parameters for Concat.");

            var concatResult = new StringBuilder();

            foreach (var param in parameters)
            {
                concatResult.Append(param.TrimSurroundingQuotes());
            }

            return concatResult.ToString();
        }

        public static string REPLACE(EffectCallerParams args, string[] parameters)
        {
            if (parameters.Length != 3) throw new ArgumentException("Invalid parameters for Replace.");

            return parameters[0].TrimSurroundingQuotes().Replace(parameters[1].TrimSurroundingQuotes(), parameters[2].TrimSurroundingQuotes()).ToString();
        }

        public static string REVERSE(EffectCallerParams args, string[] parameters)
        {
            if (parameters.Length != 1) throw new ArgumentException("Invalid parameters for Reverse.");

            return parameters[0].TrimSurroundingQuotes().Reverse().ToString() ?? string.Empty;
        }

        public static string LOWER(EffectCallerParams args, string[] parameters)
        {
            if (parameters.Length != 1) throw new ArgumentException("Invalid parameters for Lower.");

            return parameters[0].TrimSurroundingQuotes().ToLowerInvariant();
        }

        public static string UPPER(EffectCallerParams args, string[] parameters)
        {
            if (parameters.Length != 1) throw new ArgumentException("Invalid parameters for Upper.");

            return parameters[0].TrimSurroundingQuotes().ToUpperInvariant();
        }

        public static string TRIM(EffectCallerParams args, string[] parameters)
        {
            if (parameters.Length != 1) throw new ArgumentException("Invalid parameters for Trim.");

            return parameters[0].TrimSurroundingQuotes().Trim();
        }

        public static string FLOOR(EffectCallerParams args, string[] parameters)
        {
            if (parameters.Length != 2) throw new ArgumentException("Invalid parameters for Floor.");
            
            var numberToFloor = parameters[0].IsMathExpression() ? new Expression(parameters[0]).Eval<decimal>() : decimal.Parse(parameters[0]);
            var decimals = parameters[1].IsMathExpression() ? new Expression(parameters[1]).Eval<int>() : int.Parse(parameters[1]);

            var factor = (decimal)Math.Pow(10, decimals);
            var result = Math.Floor(numberToFloor * factor) / factor;
            var formatString = decimals == 0 ? "0" : "0." + new string('#', decimals);

            return result.ToString(formatString, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string CEILING(EffectCallerParams args, string[] parameters)
        {
            if (parameters.Length != 2) throw new ArgumentException("Invalid parameters for Ceiling.");

            var numberToRound = parameters[0].IsMathExpression() ? new Expression(parameters[0]).Eval<decimal>() : decimal.Parse(parameters[0]);
            var decimals = parameters[1].IsMathExpression() ? new Expression(parameters[1]).Eval<int>() : int.Parse(parameters[1]);

            var factor = (decimal)Math.Pow(10, decimals);
            var result = Math.Ceiling(numberToRound * factor) / factor;
            var formatString = decimals == 0 ? "0" : "0." + new string('#', decimals);

            return result.ToString(formatString, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string USESSTAT(EffectCallerParams args, string[] parameters)
        {
            if (parameters.Length != 2) throw new ArgumentException("Invalid parameters for UsesStat.");

            var entityName = parameters[0].ToLower();

            var entityToCheck = GetEntityByName(entityName, "UsesStat", args);

            var statId = parameters[1];

            if (entityToCheck is Character character)
            {
                var statusExists = character.UsedStats.Exists(als => als.Id.Equals(statId, StringComparison.InvariantCultureIgnoreCase));
                return statusExists.ToString();
            }

            return false.ToString();
        }

        public static string CURRENTITEM(EffectCallerParams args, string[] parameters)
        {
            if (parameters.Length != 2) throw new ArgumentException("Invalid parameters for CurrentItem.");

            var entityName = parameters[0].ToLower();

            var entityToCheck = GetEntityByName(entityName, "CurrentItem", args);

            if (entityToCheck is not Character c)
                throw new ArgumentException("Invalid entity in CurrentItem.");

            var slotToCheck = parameters[1].ToLower();

            if (!c.AvailableSlots.Any(s => s.Id.Equals(slotToCheck, StringComparison.InvariantCultureIgnoreCase)))
                // Default to nothing if the slot doesn't exist
                return "\"\"";

            var itemInSlot = c.ItemInSlot(slotToCheck);

            if(itemInSlot != null)
                return $"\"{itemInSlot.ClassId}\"";
            else
                return "\"\"";
        }

        public static string CURRENTWEAPON(EffectCallerParams args, string[] parameters)
        {
            if(Map.IsDebugMode)
                Map.AppendMessage("DEBUG: WARNING, CurrentWeapon is deprecated. Use CurrentItem with 'Weapon' as the second parameter instead.");
            return CURRENTITEM(args, [parameters[0], "Weapon"]);
        }

        public static string CURRENTARMOR(EffectCallerParams args, string[] parameters)
        {
            if (Map.IsDebugMode)
                Map.AppendMessage("DEBUG: WARNING, CurrentArmor is deprecated. Use CurrentItem with 'Armor' as the second parameter instead.");
            return CURRENTITEM(args, [parameters[0], "Armor"]);
        }

        public static string DISTANCEBETWEEN(EffectCallerParams args, string[] parameters)
        {
            if (parameters.Length != 2) throw new ArgumentException("Invalid parameters for DistanceBetween.");

            var entity1Name = parameters[0].ToLower();

            var entity1ToCheck = GetEntityByName(entity1Name, "DistanceBetween", args);

            var entity2Name = parameters[1].ToLower();

            var entity2ToCheck = GetEntityByName(entity2Name, "DistanceBetween", args);

            if (entity1ToCheck is not Character c1 || c1.Position == null)
                throw new ArgumentException("Invalid entity in DistanceBetween.");
            if (entity2ToCheck is not Character c2 || c2.Position == null)
                throw new ArgumentException("Invalid entity in DistanceBetween.");

            return Math.Floor(GamePoint.Distance(c1.Position, c2.Position)).ToString();
        }

        public static string AREINTHESAMEROOM(EffectCallerParams args, string[] parameters)
        {
            if (parameters.Length != 2) throw new ArgumentException("Invalid parameters for AreInSameRoom.");

            var entity1Name = parameters[0].ToLower();

            var entity1ToCheck = GetEntityByName(entity1Name, "AreInSameRoom", args);

            var entity2Name = parameters[1].ToLower();

            var entity2ToCheck = GetEntityByName(entity2Name, "AreInSameRoom", args);

            if (entity1ToCheck is not Character c1 || c1.Position == null)
                throw new ArgumentException("Invalid entity in AreInSameRoom.");
            if (entity2ToCheck is not Character c2 || c2.Position == null)
                throw new ArgumentException("Invalid entity in AreInSameRoom.");

            var c1IsInHallwayTile = c1.ContainingTile.Type == TileType.Hallway || c1.ContainingRoom == null;
            var c2IsInHallwayTile = c2.ContainingTile.Type == TileType.Hallway || c2.ContainingRoom == null;

            if (!c1IsInHallwayTile && !c2IsInHallwayTile)
                return (c1.ContainingRoom == c2.ContainingRoom).ToString();

            return ((Map.GetFOVTilesWithinDistance(c1.Position, EngineConstants.FullRoomSightRangeForHallways).Contains(c2.ContainingTile) || Map.GetFOVTilesWithinDistance(c2.Position, EngineConstants.FullRoomSightRangeForHallways).Contains(c1.ContainingTile))).ToString();
        }

        public static string ROLLACLASS(EffectCallerParams args, string[] parameters)
        {
            if (parameters.Length < 2) throw new ArgumentException("Invalid parameters for RollAClass.");

            var classesAndWeights = new List<(string Id, int Weight)>();

            foreach (var parameter in parameters)
            {
                var paramParts = parameter.Split('|');
                if(paramParts.Length != 2) throw new ArgumentException("Invalid parameters for RollAClass.");
                var id = paramParts[0];
                if(!Map.PossibleClasses.Any(pc => pc.Id.Equals(id))) throw new ArgumentException("Invalid parameters for RollAClass.");
                if(!int.TryParse(paramParts[1], out int weight)) throw new ArgumentException("Invalid parameters for RollAClass.");
                classesAndWeights.Add((paramParts[0], weight));
            }

            return classesAndWeights.TakeRandomElementWithWeights(iaw => iaw.Weight, Rng).Id;
        }

        public static string ROLLANITEM(EffectCallerParams args, string[] parameters)
        {
            if (parameters.Length > 1) throw new ArgumentException("Invalid parameters for RollAnItem.");

            var acceptableItemClasses = Map.PossibleItemClasses.Except(Map.UndroppableItemClasses).ToList();

            if (parameters.Length == 1)
            {
                var specificItemType = parameters[0];
                acceptableItemClasses = specificItemType.ToLowerInvariant() switch
                {
                    "weapon" => acceptableItemClasses.Where(ic => ic.ItemType.PowerType == ItemPowerType.Damage).ToList(),
                    "armor" => acceptableItemClasses.Where(ic => ic.ItemType.PowerType == ItemPowerType.Mitigation).ToList(),
                    "equippable" => acceptableItemClasses.Where(ic => ic.ItemType.Usability == ItemUsability.Equip).ToList(),
                    "consumable" => acceptableItemClasses.Where(ic => ic.ItemType.Usability == ItemUsability.Use).ToList(),
                    _ => throw new ArgumentException("Invalid parameters for RollAnItem."),
                };
            }

            return acceptableItemClasses.TakeRandomElement(Rng).Id;
        }

        public static string CURRENTFLOORLEVEL(EffectCallerParams args, string[] parameters)
        {
            if (parameters.Length > 0) throw new ArgumentException("Invalid parameters for CurrentFloorLevel.");

            return Map.FloorLevel.ToString();
        }

        public static string ISINVENTORYFULL(EffectCallerParams args, string[] parameters)
        {
            if (parameters.Length > 1) throw new ArgumentException("Invalid parameters for IsInventoryFull.");

            var entityName = parameters[0].ToLower();

            var entityToCheck = GetEntityByName(entityName, "IsInventoryFull", args);

            if (entityToCheck is not Character c)
                throw new ArgumentException("Invalid entity in IsInventoryFull.");

            return (c.Inventory.Count == c.InventorySize).ToString();
        }

        public static string ROLLANACTION(EffectCallerParams args, string[] parameters)
        {
            if (parameters.Length != 1) throw new ArgumentException("Invalid parameters for RollAnAction.");

            var entityName = parameters[0].ToLower();

            var entityToCheck = GetEntityByName(entityName, "RollAnAction", args);

            if (entityToCheck is not Character c)
                throw new ArgumentException("Invalid entity in RollAnAction.");

            var actionsNotFromConsumables = c.OnAttack.Where(oa => oa.User is not Item i || !i.IsConsumable).ToList();

            return actionsNotFromConsumables.Count > 0 ? actionsNotFromConsumables.TakeRandomElement(Rng).SelectionId : "<<NULL>>";
        }

        private static Entity GetEntityByName(string name, string functionName, EffectCallerParams args)
        {
            return name.ToLowerInvariant() switch
            {
                "this" => args.This,
                "source" => args.Source,
                "target" => args.Target is Entity e ? e : throw new ArgumentException($"Invalid entity name {name} in {functionName}."),
                "originaltarget" => args.OriginalTarget is Entity e ? e : throw new ArgumentException($"Invalid entity name {name} in {functionName}."),
                "player" => args.This.Map.Player,
                _ => throw new ArgumentException($"Invalid entity name {name} in {functionName}.")
            };
        }
    }
}
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
