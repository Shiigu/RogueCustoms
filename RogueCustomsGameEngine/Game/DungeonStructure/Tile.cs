using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.Representation;
using GamePoint = RogueCustomsGameEngine.Utils.Representation.GamePoint;
using System;
using System.Runtime.InteropServices.ObjectiveC;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.Exceptions;
using System.Reflection.Metadata.Ecma335;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;

namespace RogueCustomsGameEngine.Game.DungeonStructure
{
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
    [Serializable]
    public sealed class Tile : ITargetable, IEquatable<Tile?>
    {
        private static List<string> FunctionsThatCanMakeATileHarmful = new() { "DealDamage", "ApplyAlteredStatus", "ApplyStatAlteration", "Teleport" };
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

        public string TypeName => Map.Locale[Type.Name];
        public string TypeDescription => Map.Locale[Type.Description];

        private bool _isConnectorTile = false;
        public bool IsConnectorTile
        {
            get { return _isConnectorTile; }
            set
            {
                _isConnectorTile = value;
                _consoleRepresentation = null;
            }
        }
        private string _doorId;
        public string DoorId
        {
            get { return Type == TileType.Door ? _doorId : string.Empty; }
            set
            {
                if (Type == TileType.Door)
                    _doorId = value;
                _consoleRepresentation = null;
            }
        }
        public bool IsWalkable => Type.IsWalkable;
        public bool IsSolid => Type.IsSolid;
        public bool IsOccupied {
            get
            {
                try
                {
                    return LivingCharacter != null && LivingCharacter.ExistenceStatus == EntityExistenceStatus.Alive;
                }
                catch
                {
                    // Somehow, this can throw a NullReferenceException
                    return false;
                }
            }
        }
        public ActionWithEffects OnStood => Type.OnStood;

        private ConsoleRepresentation _consoleRepresentation;
        public ConsoleRepresentation ConsoleRepresentation
        {
            get
            {
                if (_consoleRepresentation == null)
                {
                    if (!Map.DefaultTileTypes.Contains(Type))
                        _consoleRepresentation = GetConsoleRepresentationOfCustomType();
                    else
                    {
                        _consoleRepresentation = new ConsoleRepresentation();
                        if (Type == TileType.Empty)
                        {
                            _consoleRepresentation = Type.TileTypeSet.Central;
                        }
                        else if (Type == TileType.Stairs)
                        {
                            _consoleRepresentation = Type.TileTypeSet.Central;
                        }
                        else if (Type == TileType.Floor)
                        {
                            _consoleRepresentation = Type.TileTypeSet.Central;
                        }
                        else if (Type == TileType.Door)
                        {
                            var keyType = Map.FloorConfigurationToUse.PossibleKeys.KeyTypes.Find(kt => kt.KeyTypeName.Equals(DoorId));
                            _consoleRepresentation = keyType.DoorConsoleRepresentation;
                        }
                        else if (Type == TileType.Wall)
                        {
                            if (Room != null)
                            {
                                if (Position.Equals(Room.TopLeft))
                                    _consoleRepresentation = Type.TileTypeSet.TopLeft;
                                else if (Position.Equals(Room.TopRight))
                                    _consoleRepresentation = Type.TileTypeSet.TopRight;
                                else if (Position.Equals(Room.BottomLeft))
                                    _consoleRepresentation = Type.TileTypeSet.BottomLeft;
                                else if (Position.Equals(Room.BottomRight))
                                    _consoleRepresentation = Type.TileTypeSet.BottomRight;
                                else if (Position.X.Equals(Room.BottomLeft.X) || Position.X.Equals(Room.BottomRight.X))
                                    _consoleRepresentation = Type.TileTypeSet.Vertical;
                                else if (Position.Y.Equals(Room.TopLeft.Y) || Position.Y.Equals(Room.BottomRight.Y))
                                    _consoleRepresentation = Type.TileTypeSet.Horizontal;
                                else // This should only trigger when it's a Wall that was created by a Character
                                    _consoleRepresentation = Type.TileTypeSet.Horizontal;
                            }
                            else // This should only trigger when it's a Wall that was created by a Character on a Hallway
                            {
                                _consoleRepresentation = Type.TileTypeSet.Horizontal;
                            }
                        }
                        else if (Type == TileType.Hallway)
                        {
                            if (_isConnectorTile && !Room.IsDummy)
                            {
                                _consoleRepresentation = Type.TileTypeSet.Connector;
                            }
                            else
                            {

                                var leftTile = Map.GetTileFromCoordinates(Position.X - 1, Position.Y);
                                var rightTile = Map.GetTileFromCoordinates(Position.X + 1, Position.Y);
                                var topTile = Map.GetTileFromCoordinates(Position.X, Position.Y - 1);
                                var bottomTile = Map.GetTileFromCoordinates(Position.X, Position.Y + 1);

                                bool leftIsHallway = leftTile?.Type == Type;
                                bool rightIsHallway = rightTile?.Type == Type;
                                bool topIsHallway = topTile?.Type == Type;
                                bool bottomIsHallway = bottomTile?.Type == Type;

                                // Logic for Hallway representation based on adjacent Hallway tiles
                                if (!leftIsHallway && rightIsHallway && !topIsHallway && bottomIsHallway)
                                    _consoleRepresentation = Type.TileTypeSet.TopLeft;
                                if (leftIsHallway && !rightIsHallway && !topIsHallway && bottomIsHallway)
                                    _consoleRepresentation = Type.TileTypeSet.TopRight;
                                if (!leftIsHallway && rightIsHallway && topIsHallway && !bottomIsHallway)
                                    _consoleRepresentation = Type.TileTypeSet.BottomLeft;
                                if (leftIsHallway && !rightIsHallway && topIsHallway && !bottomIsHallway)
                                    _consoleRepresentation = Type.TileTypeSet.BottomRight;
                                if (leftIsHallway && rightIsHallway && !topIsHallway && !bottomIsHallway)
                                    _consoleRepresentation = Type.TileTypeSet.Horizontal;
                                if (!leftIsHallway && !rightIsHallway && topIsHallway && bottomIsHallway)
                                    _consoleRepresentation = Type.TileTypeSet.Vertical;

                                // Top or bottom connectors
                                if (leftIsHallway && rightIsHallway && topIsHallway && !bottomIsHallway)
                                    _consoleRepresentation = Type.TileTypeSet.HorizontalTop;
                                if (leftIsHallway && rightIsHallway && !topIsHallway && bottomIsHallway)
                                    _consoleRepresentation = Type.TileTypeSet.HorizontalBottom;

                                // Left or right connectors
                                if (leftIsHallway && !rightIsHallway && topIsHallway && bottomIsHallway)
                                    _consoleRepresentation = Type.TileTypeSet.VerticalLeft;
                                if (!leftIsHallway && rightIsHallway && topIsHallway && bottomIsHallway)
                                    _consoleRepresentation = Type.TileTypeSet.VerticalRight;

                                _consoleRepresentation = Type.TileTypeSet.Central; // Default if no clear adjacency pattern
                            }
                        }
                    }      
                }
                return _consoleRepresentation;
            }
        }

        public bool Discovered { get; set; }

        public bool Visible { get; set; }
        public bool Targetable => Type.IsVisible && (IsWalkable || Type == TileType.Door) && Visible;

        public Map Map { get; set; }
        public Room Room => Map.GetRoomInCoordinates(Position.X, Position.Y);
        public Character LivingCharacter => Map.GetCharacters().Find(e => e != null && e.Position.Equals(Position) && e.ExistenceStatus == EntityExistenceStatus.Alive);
        public List<Character> GetDeadCharacters() => Map.GetCharacters().Where(e => e != null && e.Position.Equals(Position) && e.ExistenceStatus == EntityExistenceStatus.Dead).ToList();
        public List<Item> GetItems() => Map.Items.Where(i => i != null && i.Position?.Equals(Position) == true && i.ExistenceStatus == EntityExistenceStatus.Alive).ToList();
        public Key Key => Map.Keys.Find(k => k?.Position?.Equals(Position) == true && k.ExistenceStatus == EntityExistenceStatus.Alive);
        public List<Entity> GetPickableObjects() {
            var itemsAsEntities = GetItems().Cast<Entity>().ToList();
            if (Key != null)
                itemsAsEntities.Add(Key);
            return itemsAsEntities;
        }
        public Trap Trap => Map.Traps.Find(t => t?.Position?.Equals(Position) == true && t.ExistenceStatus == EntityExistenceStatus.Alive);

        public override string ToString() => $"Position: {Position}; Type: {Type}; Char: {ConsoleRepresentation.Character}";


        public void StoodOn(Character stomper)
        {
            if (OnStood == null || !OnStood.ChecksCondition(stomper, stomper)) return;
            var successfulEffects = OnStood?.Do(null, stomper, true);
            if (successfulEffects != null)
            {
                stomper.Visible = true;
                if (!stomper.Map.IsDebugMode)
                {
                    stomper.Map.DisplayEvents.Add(("TileType step", new()
                    {
                        new()
                        {
                            DisplayEventType = DisplayEventType.UpdateTileRepresentation,
                            Params = new() { stomper.Position, Map.GetConsoleRepresentationForCoordinates(stomper.Position.X, stomper.Position.Y) }
                        }
                    }));
                }
            }
            if (successfulEffects != null && EngineConstants.EffectsThatTriggerOnAttacked.Intersect(successfulEffects).Any())
                stomper.AttackedBy(null);
        }

        public bool IsHarmfulFor(Character character)
        {
            return (Trap != null && Trap.ExistenceStatus == EntityExistenceStatus.Alive && Trap.CanBeSeenBy(character) && Trap.OnStepped != null && FunctionsThatCanMakeATileHarmful.Any(f => Trap.OnStepped.HasFunction(f))) || (OnStood != null && FunctionsThatCanMakeATileHarmful.Any(f => OnStood.HasFunction(f)));
        }

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
                   _isConnectorTile == other._isConnectorTile &&
                   IsWalkable == other.IsWalkable &&
                   IsOccupied == other.IsOccupied &&
                   EqualityComparer<ConsoleRepresentation>.Default.Equals(_consoleRepresentation, other._consoleRepresentation) &&
                   EqualityComparer<ConsoleRepresentation>.Default.Equals(ConsoleRepresentation, other.ConsoleRepresentation) &&
                   Discovered == other.Discovered &&
                   Visible == other.Visible &&
                   EqualityComparer<Map>.Default.Equals(Map, other.Map) &&
                   EqualityComparer<Room>.Default.Equals(Room, other.Room) &&
                   EqualityComparer<Character>.Default.Equals(LivingCharacter, other.LivingCharacter) &&
                   EqualityComparer<Trap>.Default.Equals(Trap, other.Trap);
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Position);
            hash.Add(Type);
            hash.Add(_isConnectorTile);
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

        private ConsoleRepresentation GetConsoleRepresentationOfCustomType()
        {
            ConsoleRepresentation _consoleRepresentation = null;

            if (!Type.CanVisiblyConnectWithOtherTiles)
            {
                _consoleRepresentation = Type.TileTypeSet.Central;
            }
            else if (Type.CanVisiblyConnectWithOtherTiles)
            {
                var leftTile = Map.GetTileFromCoordinates(Position.X - 1, Position.Y);
                var rightTile = Map.GetTileFromCoordinates(Position.X + 1, Position.Y);
                var topTile = Map.GetTileFromCoordinates(Position.X, Position.Y - 1);
                var bottomTile = Map.GetTileFromCoordinates(Position.X, Position.Y + 1);

                bool leftIsSameType = leftTile?.Type == Type;
                bool rightIsSameType = rightTile?.Type == Type;
                bool topIsSameType = topTile?.Type == Type;
                bool bottomIsSameType = bottomTile?.Type == Type;

                if (!leftIsSameType && rightIsSameType && !topIsSameType && bottomIsSameType)
                    _consoleRepresentation = Type.TileTypeSet.TopLeft;
                if (leftIsSameType && !rightIsSameType && !topIsSameType && bottomIsSameType)
                    _consoleRepresentation = Type.TileTypeSet.TopRight;
                if (!leftIsSameType && rightIsSameType && topIsSameType && !bottomIsSameType)
                    _consoleRepresentation = Type.TileTypeSet.BottomLeft;
                if (leftIsSameType && !rightIsSameType && topIsSameType && !bottomIsSameType)
                    _consoleRepresentation = Type.TileTypeSet.BottomRight;
                if (leftIsSameType && rightIsSameType && !topIsSameType && !bottomIsSameType)
                    _consoleRepresentation = Type.TileTypeSet.Horizontal;
                if (!leftIsSameType && !rightIsSameType && topIsSameType && bottomIsSameType)
                    _consoleRepresentation = Type.TileTypeSet.Vertical;

                if (Type.CanHaveMultilineConnections)
                {
                    if (leftIsSameType && rightIsSameType && topIsSameType && !bottomIsSameType)
                        _consoleRepresentation = Type.TileTypeSet.HorizontalTop;
                    if (leftIsSameType && rightIsSameType && !topIsSameType && bottomIsSameType)
                        _consoleRepresentation = Type.TileTypeSet.HorizontalBottom;
                    if (leftIsSameType && !rightIsSameType && topIsSameType && bottomIsSameType)
                        _consoleRepresentation = Type.TileTypeSet.VerticalLeft;
                    if (!leftIsSameType && rightIsSameType && topIsSameType && bottomIsSameType)
                        _consoleRepresentation = Type.TileTypeSet.VerticalRight;
                }

                if (_consoleRepresentation == null)
                    _consoleRepresentation = Type.TileTypeSet.Central;
            }

            return _consoleRepresentation;
        }
    }
    #pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    #pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}
