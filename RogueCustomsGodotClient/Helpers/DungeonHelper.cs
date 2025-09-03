using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsGodotClient.Helpers
{
    public static class DungeonHelper
    {
        public static bool PlayerHasPickableItemUnderneath(this DungeonDto dungeon)
        {
            return dungeon.PickableItemPositions.Any(p => p.X == dungeon.PlayerEntity.X && p.Y == dungeon.PlayerEntity.Y);
        }

        public static bool IsPlayerOnStairs(this DungeonDto dungeon)
        {
            (int X, int Y) playerPosition = (dungeon.PlayerEntity.X, dungeon.PlayerEntity.Y);

            var tile = dungeon.GetTileFromCoordinates(playerPosition.X, playerPosition.Y);

            return tile.IsStairs;
        }

        public static TileDto GetTileFromCoordinates(this DungeonDto dungeon, int x, int y)
        {
            return dungeon.Tiles.GetTileFromCoordinates(x,y);
        }

        public static TileDto GetTileFromCoordinates(this List<TileDto> tiles, int x, int y)
        {
            return tiles.Find(t => t.X == x && t.Y == y)
                ?? throw new ArgumentException("Tile does not exist");
        }

        public static ConsoleRepresentation GetTileConsoleRepresentationFromCoordinates(this DungeonDto dungeon, int x, int y)
        {
            return dungeon.GetTileFromCoordinates(x, y).ConsoleRepresentation;
        }

        public static ConsoleRepresentation GetTileConsoleRepresentationFromCoordinates(this List<TileDto> tiles, int x, int y)
        {
            return tiles.GetTileFromCoordinates(x,y).ConsoleRepresentation;
        }
    }
}
