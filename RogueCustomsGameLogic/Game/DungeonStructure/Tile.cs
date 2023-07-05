using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.Representation;
using Point = RogueCustomsGameEngine.Utils.Representation.Point;

namespace RogueCustomsGameEngine.Game.DungeonStructure
{
    public class Tile
    {
        public Point Position { get; set; }

        private TileType _type { get; set; } = TileType.Empty;
        public TileType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                _consoleRepresentation = null;
            }
        }
        public bool IsConnectorTile { get; set; } = false;
        public bool IsWalkable => Type != TileType.Empty && Type != TileType.Wall;
        public bool IsOccupied => Character != null && Character.ExistenceStatus == EntityExistenceStatus.Alive;

        private ConsoleRepresentation _consoleRepresentation;
        public ConsoleRepresentation ConsoleRepresentation
        {
            get
            {
                if (_consoleRepresentation == null)
                {
                    _consoleRepresentation = new ConsoleRepresentation
                    {
                        Character = (char)Type
                    };
                    switch (Type)
                    {
                        case TileType.Empty:
                            _consoleRepresentation.BackgroundColor = new GameColor(Color.Black);
                            _consoleRepresentation.ForegroundColor = new GameColor(Color.Black);
                            break;
                        case TileType.Floor:
                            _consoleRepresentation.BackgroundColor = new GameColor(Color.Black);
                            _consoleRepresentation.ForegroundColor = new GameColor(Color.DarkGray);
                            break;
                        case TileType.Wall:
                        case TileType.Hallway:
                            _consoleRepresentation.BackgroundColor = new GameColor(Color.Black);
                            _consoleRepresentation.ForegroundColor = new GameColor(Color.Blue);
                            break;
                        case TileType.Stairs:
                            _consoleRepresentation.BackgroundColor = new GameColor(Color.Yellow);
                            _consoleRepresentation.ForegroundColor = new GameColor(Color.DarkGreen);
                            break;
                    }
                }
                return _consoleRepresentation;
            }
        }

        public bool Discovered { get; set; }

        public bool Visible { get; set; }

        public Map Map { get; set; }
        public Character Character => Map.Characters.Find(e => e?.Position?.Equals(Position) == true && e.ExistenceStatus == EntityExistenceStatus.Alive);
        public List<Item> Items => Map.Items.Where(i => i != null && i.Position?.Equals(Position) == true && i.ExistenceStatus == EntityExistenceStatus.Alive).ToList();
        public Item Trap => Map.Traps.Find(t => t?.Position?.Equals(Position) == true && t.ExistenceStatus == EntityExistenceStatus.Alive);

        public override string ToString() => $"Position: {Position}; Type: {Type}; Char: {ConsoleRepresentation.Character}";
    }

    public enum TileType
    {
        Empty = ' ',
        Floor = '.',
        Wall = '█',
        Hallway = '▒',
        Stairs = '>'
    }
}
