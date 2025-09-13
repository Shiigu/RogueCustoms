using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsGameEngine.Utils.Helpers
{
    public static class ActionHelpers
    {
        public static IEnumerable<Tile> GetCandidateTilesForArea(this Map map, GamePoint center, string searchArea)
        {
            var currentTile = map.GetTileFromCoordinates(center);
            var candidateTiles = new List<Tile>();

            if (searchArea.StartsWith("Circle", StringComparison.InvariantCultureIgnoreCase))
            {
                var match = EngineConstants.CirclePattern.Match(searchArea);
                if (match.Success && int.TryParse(match.Groups[1].Value, out var radius))
                {
                    candidateTiles = map.GetTilesWithinDistance(center, (radius - 1) / 2, true);
                }
            }
            else if (searchArea.StartsWith("Square", StringComparison.InvariantCultureIgnoreCase))
            {
                var match = EngineConstants.SquarePattern.Match(searchArea);
                if (match.Success && int.TryParse(match.Groups[1].Value, out var size))
                {
                    candidateTiles = map.GetTilesWithinCenteredSquare(center, (size - 1) / 2, true);
                }
            }
            else if (searchArea == "Whole Room")
            {
                if (currentTile.Type == TileType.Hallway)
                    candidateTiles = map.GetTilesWithinCenteredSquare(center, 1, true);
                else
                    candidateTiles = [.. currentTile.Room.Tiles];
            }
            else if (searchArea == "Whole Map")
            {
                candidateTiles = map.Tiles.ToList();
            }
            else
            {
                var displayValue = string.IsNullOrWhiteSpace(searchArea) ? "NULL" : searchArea;
                throw new ArgumentException($"Search Area is {displayValue}, which is incorrect.");
            }

            return candidateTiles;
        }
    }
}
