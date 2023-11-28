using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Utils.Effects
{
    public static class OnTileActions
    {
        private static RngHandler Rng;
        private static Map Map;

        public static void SetActionParams(RngHandler rng, Map map)
        {
            Rng = rng;
            Map = map;
        }

        public static bool TransformTile(Entity This, Entity Source, ITargetable Target, int previousEffectOutput, out int output, params (string ParamName, string Value)[] args)
        {
            output = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);

            if (Source is not Character c)
                // Source must be a Character
                return false;

            if (Target is not Tile t)
                // Target must be a Tile
                return false;

            if (paramsObject.TileType != TileType.Floor && paramsObject.TileType != TileType.Wall)
                // Cannot turn the tile into Hallway, Stairs or Empty
                return false;

            if (paramsObject.TileType == t.Type)
                // Cannot turn the tile into something it already is
                return false;

            if (Map.GetAdjacentTiles(t.Position, false).Exists(t => t.Type == TileType.Empty))
                // Cannot convert a Tile that is adjacent to an empty tile
                return false;

            var accuracyCheck = ActionHelpers.CalculateAdjustedAccuracy(Source, null, paramsObject);

            if (Rng.NextInclusive(1, 100) > accuracyCheck)
                return false;

            var success = false;
            var oldType = t.Type;

            if (t.Character == null && t.Trap == null && t.GetItems().Count == 0)
            {
                t.Type = paramsObject.TileType;
                success = Map.Tiles.IsFullyConnected(t => t.IsWalkable);
            }

            if (!success)
            {
                t.Type = oldType;
            }
            else if (c.EntityType == EntityType.Player
                || (c.EntityType == EntityType.NPC && Map.Player.CanSee(c)))
            {
                Map.AppendMessage(Map.Locale["CharacterConvertedTile"].Format(new { CharacterName = c.Name, TileType = Map.Locale[$"TileType{t.Type}"] }), Color.DeepSkyBlue);
            }

            return success;
        }

        public static bool PlaceTrap(Entity This, Entity Source, ITargetable Target, int previousEffectOutput, out int output, params (string ParamName, string Value)[] args)
        {
            output = 0;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, previousEffectOutput, args);

            if (Source is not Character c)
                // Source must be a Character
                return false;

            if (Target is not Tile t)
                // Target must be a Tile
                return false;

            if (t.Type != TileType.Floor && t.Type != TileType.Hallway)
                // Target Tile must be a Floor or Hallway
                return false;

            if (t.Trap != null)
                // Target Tile must not already have a Trap
                return false;

            var accuracyCheck = ActionHelpers.CalculateAdjustedAccuracy(Source, null, paramsObject);

            if (Rng.NextInclusive(1, 100) > accuracyCheck)
                return false;

            var trapClass = Map.PossibleTrapClasses.Find(c => c.Id.Equals(paramsObject.Id));

            if (trapClass == null)
                // Must have a valid Trap class to spawn
                return false;

            var trap = Map.AddEntity(paramsObject.Id, 1, Target.Position) as Item;
            trap.Visible = trapClass.StartsVisible;
            trap.Faction = c.Faction;

            if (c.EntityType == EntityType.Player
                || (c.EntityType == EntityType.NPC && Map.Player.CanSee(c)))
            {
                Map.AppendMessage(Map.Locale["CharacterCreatedATrap"].Format(new { CharacterName = c.Name, TrapName = Map.Locale[trapClass.Name] }), Color.DeepSkyBlue);
            }

            return true;
        }
    }
}
