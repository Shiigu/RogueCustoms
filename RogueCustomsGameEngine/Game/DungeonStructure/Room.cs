using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RogueCustomsGameEngine.Game.DungeonStructure
{
    [Serializable]
    public class Room
    {
        public readonly GamePoint Position;
        public readonly Map Map;
        public readonly int RoomRow;
        public readonly int RoomColumn;
        public readonly int Width;
        public readonly int Height;

        public GamePoint TopLeft => new(Position.X, Position.Y);
        public GamePoint TopRight => new(Position.X + Width - 1, Position.Y);
        public GamePoint BottomLeft => new(Position.X, Position.Y + Height - 1);
        public GamePoint BottomRight => new(Position.X + Width - 1, Position.Y + Height - 1);

        public bool IsDummy => Width == 1 && Height == 1;
        public bool IsFused { get; set; }
        public bool HasStairs => GetTiles().Any(t => t.Type == TileType.Stairs);
        public bool HasItems => GetTiles().Any(t => t.GetItems().Any());

        public List<Tile> GetTiles() => Map.Tiles.Where(t => t.Position.X.Between(TopLeft.X, TopRight.X) && t.Position.Y.Between(TopLeft.Y, BottomRight.Y)).ToList();

        public Room(Map map, GamePoint position, int roomRow, int roomColumn, int width, int height)
        {
            Map = map;
            Position = position;
            RoomRow = roomRow;
            RoomColumn = roomColumn;
            Width = width;
            Height = height;
        }

        public void CreateTiles()
        {
            if (Height > 1 && Width > 1)
            {
                // Upper wall
                for (var i = 0; i < Width; i++)
                {
                    var tile = Map.GetTileFromCoordinates(Position.X + i, Position.Y);
                    if (tile.IsConnectorTile) continue;
                    tile.Type = TileType.Wall;
                }
                // Lower wall
                for (var i = 0; i < Width; i++)
                {
                    var tile = Map.GetTileFromCoordinates(Position.X + i, Position.Y + Height - 1);
                    if (tile.IsConnectorTile) continue;
                    tile.Type = TileType.Wall;
                }
                // Left wall
                for (var i = 0; i < Height; i++)
                {
                    var tile = Map.GetTileFromCoordinates(Position.X, Position.Y + i);
                    if (tile.IsConnectorTile) continue;
                    tile.Type = TileType.Wall;
                }
                // Right wall
                for (var i = 0; i < Height; i++)
                {
                    var tile = Map.GetTileFromCoordinates(Position.X + Width - 1, Position.Y + i);
                    if (tile.IsConnectorTile) continue;
                    tile.Type = TileType.Wall;
                }
                // Floor
                for (var i = 1; i < Width - 1; i++)
                {
                    for (var j = 1; j < Height - 1; j++)
                    {
                        var tile = Map.GetTileFromCoordinates(Position.X + i, Position.Y + j);
                        tile.Type = TileType.Floor;
                    }
                }
            }
            else if (IsDummy)
            {
                // Dummy room
                var tile = Map.GetTileFromCoordinates(Position.X, Position.Y);
                tile.Type = TileType.Hallway;
            }
        }

        public Room Clone()
        {
            return new Room(Map, Position, RoomRow, RoomColumn, Width, Height);
        }

        public override string ToString() => $"Index: [{RoomRow}, {RoomColumn}]; Top left: {Position}; Bottom right: {BottomRight}; Width: {Width}; Height: {Height}";
    }
}
