using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.Representation;
using GamePoint = RogueCustomsGameEngine.Utils.Representation.GamePoint;
using System;
using System.Runtime.InteropServices.ObjectiveC;
using RogueCustomsGameEngine.Game.Entities.Interfaces;

namespace RogueCustomsGameEngine.Game.DungeonStructure
{
    #pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    #pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
    [Serializable]
    public sealed class Tile : ITargetable, IEquatable<Tile?>
    {
        public GamePoint Position { get; set; }

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
                    _consoleRepresentation = new ConsoleRepresentation();
                    if (Type == TileType.Empty)
                    {
                        _consoleRepresentation = Map.TileSet.Empty;
                    }
                    else if (Type == TileType.Stairs)
                    {
                        _consoleRepresentation = Map.TileSet.Stairs;
                    }
                    else if (Type == TileType.Floor)
                    {
                        _consoleRepresentation = Map.TileSet.Floor;
                    }
                    else if (Type == TileType.Wall)
                    {
                        if (Room != null)
                        {
                            if (Position.Equals(Room.TopLeft))
                                _consoleRepresentation = Map.TileSet.TopLeftWall;
                            else if (Position.Equals(Room.TopRight))
                                _consoleRepresentation = Map.TileSet.TopRightWall;
                            else if (Position.Equals(Room.BottomLeft))
                                _consoleRepresentation = Map.TileSet.BottomLeftWall;
                            else if (Position.Equals(Room.BottomRight))
                                _consoleRepresentation = Map.TileSet.BottomRightWall;
                            else if (Position.X.Equals(Room.BottomLeft.X) || Position.X.Equals(Room.BottomRight.X))
                                _consoleRepresentation = Map.TileSet.VerticalWall;
                            else if (Position.Y.Equals(Room.TopLeft.Y) || Position.Y.Equals(Room.BottomRight.Y))
                                _consoleRepresentation = Map.TileSet.HorizontalWall;
                            else // This should only trigger when it's a Wall that was created by a Character
                                _consoleRepresentation = Map.TileSet.HorizontalWall;
                        }
                        else // This should only trigger when it's a Wall that was created by a Character on a Hallway
                        {
                            _consoleRepresentation = Map.TileSet.HorizontalWall;
                        }
                    }
                    else if (Type == TileType.Hallway)
                    {
                        if (IsConnectorTile && (Room.Width > 1 || Room.Height > 1))
                        {
                            _consoleRepresentation = Map.TileSet.ConnectorWall;
                        }
                        else
                        {
                            if (Map.GetTileFromCoordinates(Position.X - 1, Position.Y)?.Type != TileType.Hallway
                            && Map.GetTileFromCoordinates(Position.X + 1, Position.Y)?.Type == TileType.Hallway
                            && Map.GetTileFromCoordinates(Position.X, Position.Y - 1)?.Type != TileType.Hallway
                            && Map.GetTileFromCoordinates(Position.X, Position.Y + 1)?.Type == TileType.Hallway)
                            {
                                _consoleRepresentation = Map.TileSet.TopLeftHallway;
                            }
                            else if (Map.GetTileFromCoordinates(Position.X - 1, Position.Y)?.Type == TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X + 1, Position.Y)?.Type != TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X, Position.Y - 1)?.Type != TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X, Position.Y + 1)?.Type == TileType.Hallway)
                            {
                                _consoleRepresentation = Map.TileSet.TopRightHallway;
                            }
                            else if (Map.GetTileFromCoordinates(Position.X - 1, Position.Y)?.Type != TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X + 1, Position.Y)?.Type == TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X, Position.Y - 1)?.Type == TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X, Position.Y + 1)?.Type != TileType.Hallway)
                            {
                                _consoleRepresentation = Map.TileSet.BottomLeftHallway;
                            }
                            else if (Map.GetTileFromCoordinates(Position.X - 1, Position.Y)?.Type == TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X + 1, Position.Y)?.Type != TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X, Position.Y - 1)?.Type == TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X, Position.Y + 1)?.Type != TileType.Hallway)
                            {
                                _consoleRepresentation = Map.TileSet.BottomRightHallway;
                            }
                            else if (Map.GetTileFromCoordinates(Position.X - 1, Position.Y)?.Type == TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X + 1, Position.Y)?.Type == TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X, Position.Y - 1)?.Type != TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X, Position.Y + 1)?.Type != TileType.Hallway)
                            {
                                _consoleRepresentation = Map.TileSet.HorizontalHallway;
                            }
                            else if (Map.GetTileFromCoordinates(Position.X - 1, Position.Y)?.Type != TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X + 1, Position.Y)?.Type != TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X, Position.Y - 1)?.Type == TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X, Position.Y + 1)?.Type == TileType.Hallway)
                            {
                                _consoleRepresentation = Map.TileSet.VerticalHallway;
                            }
                            else if (Map.GetTileFromCoordinates(Position.X - 1, Position.Y)?.Type == TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X + 1, Position.Y)?.Type == TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X, Position.Y - 1)?.Type != TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X, Position.Y + 1)?.Type == TileType.Hallway)
                            {
                                _consoleRepresentation = Map.TileSet.HorizontalTopHallway;
                            }
                            else if (Map.GetTileFromCoordinates(Position.X - 1, Position.Y)?.Type == TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X + 1, Position.Y)?.Type == TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X, Position.Y - 1)?.Type == TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X, Position.Y + 1)?.Type != TileType.Hallway)
                            {
                                _consoleRepresentation = Map.TileSet.HorizontalBottomHallway;
                            }
                            else if (Map.GetTileFromCoordinates(Position.X - 1, Position.Y)?.Type == TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X + 1, Position.Y)?.Type != TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X, Position.Y - 1)?.Type == TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X, Position.Y + 1)?.Type == TileType.Hallway)
                            {
                                _consoleRepresentation = Map.TileSet.VerticalLeftHallway;
                            }
                            else if (Map.GetTileFromCoordinates(Position.X - 1, Position.Y)?.Type != TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X + 1, Position.Y)?.Type == TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X, Position.Y - 1)?.Type == TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X, Position.Y + 1)?.Type == TileType.Hallway)
                            {
                                _consoleRepresentation = Map.TileSet.VerticalRightHallway;
                            }
                            else if (Map.GetTileFromCoordinates(Position.X - 1, Position.Y)?.Type == TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X + 1, Position.Y)?.Type != TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X, Position.Y - 1)?.Type != TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X, Position.Y + 1)?.Type != TileType.Hallway)
                            {
                                _consoleRepresentation = Map.TileSet.HorizontalHallway;
                            }
                            else if (Map.GetTileFromCoordinates(Position.X - 1, Position.Y)?.Type != TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X + 1, Position.Y)?.Type == TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X, Position.Y - 1)?.Type != TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X, Position.Y + 1)?.Type != TileType.Hallway)
                            {
                                _consoleRepresentation = Map.TileSet.HorizontalHallway;
                            }
                            else if (Map.GetTileFromCoordinates(Position.X - 1, Position.Y)?.Type != TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X + 1, Position.Y)?.Type != TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X, Position.Y - 1)?.Type != TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X, Position.Y + 1)?.Type == TileType.Hallway)
                            {
                                _consoleRepresentation = Map.TileSet.VerticalHallway;
                            }
                            else if (Map.GetTileFromCoordinates(Position.X - 1, Position.Y)?.Type != TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X + 1, Position.Y)?.Type != TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X, Position.Y - 1)?.Type == TileType.Hallway
                                                        && Map.GetTileFromCoordinates(Position.X, Position.Y + 1)?.Type != TileType.Hallway)
                            {
                                _consoleRepresentation = Map.TileSet.VerticalHallway;
                            }
                            else
                            {
                                _consoleRepresentation = Map.TileSet.CentralHallway;
                            }
                        }
                    }
                }
                return _consoleRepresentation;
            }
        }

        public bool Discovered { get; set; }

        public bool Visible { get; set; }

        public Map Map { get; set; }
        public Room Room => Map.GetRoomInCoordinates(Position.X, Position.Y);
        public Character Character => Map.GetCharacters().Find(e => e?.Position?.Equals(Position) == true && e.ExistenceStatus == EntityExistenceStatus.Alive);
        public List<Character> GetDeadCharacters() => Map.GetCharacters().Where(e => e?.Position?.Equals(Position) == true && e.ExistenceStatus == EntityExistenceStatus.Dead).ToList();
        public List<Item> GetItems() => Map.Items.Where(i => i != null && i.Position?.Equals(Position) == true && i.ExistenceStatus == EntityExistenceStatus.Alive).ToList();
        public Item Trap => Map.Traps.Find(t => t?.Position?.Equals(Position) == true && t.ExistenceStatus == EntityExistenceStatus.Alive);

        public override string ToString() => $"Position: {Position}; Type: {Type}; Char: {ConsoleRepresentation.Character}";

        public override bool Equals(object? obj)
        {
            if (obj is not Tile t) return false;
            return t.Position.Equals(Position) && t.Type == Type;
        }

        public bool Equals(Tile? other)
        {
            return other is not null &&
                   EqualityComparer<GamePoint>.Default.Equals(Position, other.Position) &&
                   _type == other._type &&
                   Type == other.Type &&
                   IsConnectorTile == other.IsConnectorTile &&
                   IsWalkable == other.IsWalkable &&
                   IsOccupied == other.IsOccupied &&
                   EqualityComparer<ConsoleRepresentation>.Default.Equals(_consoleRepresentation, other._consoleRepresentation) &&
                   EqualityComparer<ConsoleRepresentation>.Default.Equals(ConsoleRepresentation, other.ConsoleRepresentation) &&
                   Discovered == other.Discovered &&
                   Visible == other.Visible &&
                   EqualityComparer<Map>.Default.Equals(Map, other.Map) &&
                   EqualityComparer<Room>.Default.Equals(Room, other.Room) &&
                   EqualityComparer<Character>.Default.Equals(Character, other.Character) &&
                   EqualityComparer<Item>.Default.Equals(Trap, other.Trap);
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Position);
            hash.Add(Type);
            hash.Add(IsConnectorTile);
            hash.Add(IsWalkable);
            hash.Add(IsOccupied);
            hash.Add(ConsoleRepresentation);
            hash.Add(Discovered);
            hash.Add(Visible);
            return hash.ToHashCode();
        }

        public static bool operator ==(Tile? left, Tile? right)
        {
            return EqualityComparer<Tile>.Default.Equals(left, right);
        }

        public static bool operator !=(Tile? left, Tile? right)
        {
            return !(left == right);
        }
    }

    public enum TileType
    {
        Empty,
        Floor,
        Wall,
        Hallway,
        Stairs
    }
    #pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    #pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}
