using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Linq;

namespace RogueCustomsConsoleClient.Helpers
{
    public static class DungeonHelper
    {
        public static bool PlayerHasPickableItemUnderneath(this DungeonDto dungeon)
        {
            (int X, int Y) playerPosition = (dungeon.PlayerEntity.X, dungeon.PlayerEntity.Y);

            foreach (var e in dungeon.Entities.Where(e => e.Type == EntityDtoType.PickableObject))
            {
                (int X, int Y) itemPosition = (e.X, e.Y);

                if (itemPosition.Equals(playerPosition))
                    return true;
            }

            return false;
        }

        public static bool IsPlayerOnStairs(this DungeonDto dungeon)
        {
            (int X, int Y) playerPosition = (dungeon.PlayerEntity.X, dungeon.PlayerEntity.Y);

            var tile = dungeon.GetTileFromCoordinates(playerPosition.X, playerPosition.Y);

            return tile.IsStairs;
        }

        public static TileDto GetTileFromCoordinates(this DungeonDto dungeon, int x, int y)
        {
            return dungeon.Tiles.Find(t => t.X == x && t.Y == y)
                ?? throw new ArgumentException("Tile does not exist");
        }

        public static ConsoleRepresentation GetTileConsoleRepresentationFromCoordinates(this DungeonDto dungeon, int x, int y)
        {
            return dungeon.GetTileFromCoordinates(x, y).ConsoleRepresentation;
        }
    }
}
