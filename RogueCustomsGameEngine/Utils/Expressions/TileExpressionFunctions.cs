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
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.Representation;
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.

namespace RogueCustomsGameEngine.Utils.Expressions
{
    public static class TileExpressionFunctions
    {
        private static RngHandler rngHandler;
        private static Map Map;

        public static void Setup(RngHandler rng, Map map)
        {
            rngHandler = rng;
            Map = map;
        }

        public static string TILEISOCCUPIED(EffectCallerParams args, string[] parameters)
        {
            if (args.Target is not Tile tileToCheck) throw new ArgumentException("Invalid parameters for TileIsOccupied.");

            return (tileToCheck.Type == TileType.Door || tileToCheck.IsOccupied).ToString();
        }

        public static string TILEHASDEADALLIES(EffectCallerParams args, string[] parameters)
        {
            if (parameters.Length != 1) throw new ArgumentException("Invalid parameters for TileHasDeadAllies.");

            if (args.Target is not Tile tileToCheck) throw new ArgumentException("Invalid parameters for TileHasDeadAllies.");

            var entityName = parameters[0].ToLower();

            var entityToCheck = GetObjectByName(entityName, "TileHasDeadAllies", args);

            if (entityToCheck is not Character c)
                return false.ToString();

            var entityFaction = c.Faction;

            return (tileToCheck.GetDeadCharacters().Any(c => c.Faction.IsAlliedWith(entityFaction))).ToString();
        }

        public static string DISTANCEBETWEEN(EffectCallerParams args, string[] parameters)
        {
            if (parameters.Length != 2) throw new ArgumentException("Invalid parameters for DistanceBetween.");

            var object1Name = parameters[0].ToLower();

            var object1ToCheck = GetObjectByName(object1Name, "DistanceBetween", args);

            var object2Name = parameters[1].ToLower();

            var object2ToCheck = GetObjectByName(object2Name, "DistanceBetween", args);

            if (object1ToCheck is not ITargetable t1 || t1.Position == null)
                throw new ArgumentException("Invalid object in DistanceBetween.");
            if (object2ToCheck is not ITargetable t2 || t2.Position == null)
                throw new ArgumentException("Invalid object in DistanceBetween.");

            return Math.Floor(GamePoint.Distance(t1.Position, t2.Position)).ToString();
        }

        public static string AREINTHESAMEROOM(EffectCallerParams args, string[] parameters)
        {
            if (parameters.Length != 2) throw new ArgumentException("Invalid parameters for AreInSameRoom.");

            var object1Name = parameters[0].ToLower();

            var object1ToCheck = GetObjectByName(object1Name, "AreInSameRoom", args);

            var object2Name = parameters[1].ToLower();

            var object2ToCheck = GetObjectByName(object2Name, "AreInSameRoom", args);

            if (object1ToCheck is not ITargetable t1 || t1.Position == null)
                throw new ArgumentException("Invalid object in AreInSameRoom.");
            if (object2ToCheck is not ITargetable t2 || t2.Position == null)
                throw new ArgumentException("Invalid object in AreInSameRoom.");

            Tile t1Tile = null, t2Tile = null;
            Room t1Room = null, t2Room = null;

            if (t1 is Tile)
                t1Tile = (Tile)t1;
            else if(t1 is Entity e1)
                t1Tile = e1.ContainingTile;
            t1Room = t1Tile.Room;

            if (t2 is Tile)
                t2Tile = (Tile)t2;
            else if (t2 is Entity e2)
                t2Tile = e2.ContainingTile;
            t2Room = t2Tile.Room;

            var t1TileIsHallway = t1Tile.Type == TileType.Hallway || t1Tile.Room == null || t1Tile.IsConnectorTile;
            var t2TileIsHallway = t2Tile.Type == TileType.Hallway || t2Tile.Room == null || t2Tile.IsConnectorTile;

            if (!t1TileIsHallway && !t2TileIsHallway)
                return (t1Room == t2Room).ToString();

            return ((Map.GetFOVTilesWithinDistance(t1.Position, EngineConstants.FullRoomSightRangeForHallways).Contains(t2) || Map.GetFOVTilesWithinDistance(t1.Position, EngineConstants.FullRoomSightRangeForHallways).Contains(t2))).ToString();
        }

        private static ITargetable GetObjectByName(string name, string functionName, EffectCallerParams args)
        {
            return name.ToLowerInvariant() switch
            {
                "this" => args.This,
                "source" => args.Source,
                "target" => args.Target is Tile t ? t : throw new ArgumentException($"Invalid object name {name} in {functionName}."),
                "originaltarget" => args.OriginalTarget is Tile t ? t : throw new ArgumentException($"Invalid object name {name} in {functionName}."),
                "player" => Map.Player,
                _ => throw new ArgumentException($"Invalid object name {name} in {functionName}.")
            };
        }
    }
}
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
