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
        public readonly Map Map;
        public HashSet<Tile> Tiles { get; }
        public bool WasVisited { get; set; }
        public bool MustSpawnMonsterHouse { get; set; }

        private GamePoint _topLeft, _topRight, _bottomLeft, _bottomRight;
        public GamePoint TopLeft 
        { 
            get
            {
                _topLeft ??= new(Tiles.Min(t => t.Position.X), Tiles.Min(t => t.Position.Y));
                return _topLeft;
            }
        }
        public GamePoint TopRight
        {
            get
            {
                _topRight ??= new(Tiles.Max(t => t.Position.X), Tiles.Min(t => t.Position.Y));
                return _topRight;
            }
        }
        public GamePoint BottomLeft
        {
            get
            {
                _bottomLeft ??= new(Tiles.Min(t => t.Position.X), Tiles.Max(t => t.Position.Y));
                return _bottomLeft;
            }
        }
        public GamePoint BottomRight
        {
            get
            {
                _bottomRight ??= new(Tiles.Max(t => t.Position.X), Tiles.Max(t => t.Position.Y));
                return _bottomRight;
            }
        }

        public int EffectiveWidth => BottomRight.X - TopLeft.X + 1;
        public int EffectiveHeight => BottomRight.Y - TopLeft.Y + 1;
        public bool HasStairs => Tiles.Any(t => t == Map.StairsTile);
        public bool HasItems => Tiles.Any(t => t.GetPickableObjects().Any());

        public Room(Map map, IEnumerable<Tile> tiles)
        {
            Map = map;
            Tiles = new HashSet<Tile>(tiles);
        }

        public Room Clone() => new Room(Map, Tiles);

        public override string ToString()
            => $"Room with {Tiles.Count} tiles; Bounds: {TopLeft} -> {BottomRight}; Width={EffectiveWidth}, Height={EffectiveHeight}";
    }
}
