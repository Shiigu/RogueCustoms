using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure.FloorGenerators.Interfaces;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.Representation;
using System.IO;
using RogueCustomsGameEngine.Game.Entities;
using System.Numerics;

namespace RogueCustomsGameEngine.Game.DungeonStructure.FloorGenerators
{
    public class ProceduralFloorGenerator : IFloorGenerator
    {
        private Map _map;

        private int GenerationTries => _map.GenerationTries;
        private RngHandler Rng => _map.Rng;

        private FloorLayoutGenerator _generatorToUse;
        private FloorType FloorConfigurationToUse => _map.FloorConfigurationToUse;
        private int Width => _map.Width;
        private int Height => _map.Height;
        private int FloorLevel => _map.FloorLevel;

        private RoomDispositionType[,] RoomDispositionToUse;

        private int RoomCountRows => _generatorToUse.Rows;
        private int RoomCountColumns => _generatorToUse.Columns;
        private int MaxConnectionsBetweenRooms => FloorConfigurationToUse.MaxConnectionsBetweenRooms;
        private int OddsForExtraConnections => FloorConfigurationToUse.OddsForExtraConnections;
        private int RoomFusionOdds => FloorConfigurationToUse.RoomFusionOdds;


        private (GamePoint TopLeftCorner, GamePoint BottomRightCorner)[,] RoomLimitsTable;
        private int MinRoomWidth;
        private int MaxRoomWidth;
        private int MinRoomHeight;
        private int MaxRoomHeight;

        public ProceduralFloorGenerator(Map map, FloorLayoutGenerator generatorToUse)
        {
            _map = map;
            _generatorToUse = generatorToUse;
        }

        #region Normal Tiles
        public void CreateNormalTiles()
        {
            _map.Hallways = new();
            _map.Fusions = new();
            _map.Rooms = new();

            if (RoomDispositionToUse == null || GenerationTries % 100 == 0)
            {
                var possibleRoomDisposition = RollRoomDistributionToUse();

                if (!possibleRoomDisposition.IsFullyConnected(d => d != RoomDispositionType.NoRoom && d != RoomDispositionType.NoConnection && d != RoomDispositionType.ConnectionImpossible)) return;

                RoomDispositionToUse = possibleRoomDisposition;
            }

            _map.ResetAndCreateTiles();
            CreateRooms();
            foreach (var room in _map.Rooms)
            {
                CreateTilesForRoom(room);
            }
            FuseRooms();
            _map.Rooms = _map.Rooms.Distinct().ToList();
            if (_map.Rooms.Count(r => !r.IsDummy) > 1)
                ConnectRooms();
        }

        private RoomDispositionType[,] RollRoomDistributionToUse()
        {
            var rolledRoomDistribution = _generatorToUse.RoomDisposition.Copy();

            var totalRandomRooms = rolledRoomDistribution.Where(rd => rd == RoomDispositionType.RandomRoom).Count;
            var randomRoomsRemoved = 0;
            var maximumRandomRoomsToRemove = Math.Max(1, totalRandomRooms / (3 + (GenerationTries * 2) / EngineConstants.MaxGenerationTries));
            var totalRandomConnections = rolledRoomDistribution.Where(rd => rd == RoomDispositionType.RandomConnection).Count;
            var randomConnectionsRemoved = 0;
            var maximumRandomConnectionsToRemove = Math.Max(1, totalRandomConnections / (3 + (GenerationTries * 2) / EngineConstants.MaxGenerationTries));

            for (var row = 0; row < rolledRoomDistribution.GetLength(0); row++)
            {
                for (var column = 0; column < rolledRoomDistribution.GetLength(1); column++)
                {
                    var generatorTile = rolledRoomDistribution[row, column];
                    var random = Rng.RollProbability();
                    if (generatorTile == RoomDispositionType.RandomRoom)
                    {
                        if (random <= 70)
                            rolledRoomDistribution[row, column] = RoomDispositionType.GuaranteedRoom;
                        else if (random <= 85)
                            rolledRoomDistribution[row, column] = RoomDispositionType.GuaranteedDummyRoom;
                        else if (randomRoomsRemoved < maximumRandomRoomsToRemove)
                        {
                            rolledRoomDistribution[row, column] = RoomDispositionType.NoRoom;
                            randomRoomsRemoved++;
                        }
                    }
                    else if (generatorTile == RoomDispositionType.RandomConnection)
                    {
                        if (randomConnectionsRemoved < maximumRandomConnectionsToRemove && random < 30)
                        {
                            rolledRoomDistribution[row, column] = RoomDispositionType.NoConnection;
                            randomConnectionsRemoved++;
                        }
                        else if (random < 100 - RoomFusionOdds)
                        {
                            rolledRoomDistribution[row, column] = RoomDispositionType.GuaranteedHallway;
                        }
                    }
                }
            }

            return rolledRoomDistribution;
        }

        private void CreateRooms()
        {
            var validRoomTileTypes = new List<RoomDispositionType>() { RoomDispositionType.GuaranteedRoom, RoomDispositionType.GuaranteedDummyRoom, RoomDispositionType.RandomRoom };

            GetPossibleRoomData();
            _map.Rooms = new List<Room>();

            for (var row = 0; row < RoomDispositionToUse.GetLength(0); row++)
            {
                for (var column = 0; column < RoomDispositionToUse.GetLength(1); column++)
                {
                    var roomTile = RoomDispositionToUse[row, column];
                    if (!validRoomTileTypes.Contains(roomTile)) continue;
                    var roomRow = row / 2;
                    var roomColumn = column / 2;

                    var (MinX, MinY, MaxX, MaxY) = GetPossibleCoordinatesForRoom(roomRow, roomColumn);

                    var actualMaxX = MaxX - _generatorToUse.MinRoomSize.Width;
                    var actualMaxY = MaxY - _generatorToUse.MinRoomSize.Height;

                    if (roomTile == RoomDispositionType.GuaranteedRoom)
                    {
                        // Adjust room width and height ensuring they meet the min size requirements
                        var rngX1 = Rng.NextInclusive(MinX, actualMaxX);
                        var rngX2 = Rng.NextInclusive(rngX1 + _generatorToUse.MinRoomSize.Width, MaxX);
                        var roomWidth = Math.Min(_generatorToUse.MaxRoomSize.Width, rngX2 - rngX1 + 1);

                        var rngY1 = Rng.NextInclusive(MinY, actualMaxY);
                        var rngY2 = Rng.NextInclusive(rngY1 + _generatorToUse.MinRoomSize.Height, MaxY);
                        var roomHeight = Math.Min(_generatorToUse.MaxRoomSize.Height, rngY2 - rngY1 + 1);

                        _map.Rooms.Add(new Room(_map, new GamePoint(rngX1, rngY1), roomRow, roomColumn, roomWidth, roomHeight));
                    }
                    else if (roomTile == RoomDispositionType.GuaranteedDummyRoom)
                    {
                        // Dummy rooms are 1x1
                        var rngX = Rng.NextInclusive(MinX + 1, MaxX - 1);
                        var rngY = Rng.NextInclusive(MinY + 1, MaxY - 1);
                        _map.Rooms.Add(new Room(_map, new GamePoint(rngX, rngY), roomRow, roomColumn, 1, 1));
                    }
                }
            }
        }

        private void CreateTilesForRoom(Room room)
        {
            if (room.Height > 1 && room.Width > 1)
            {
                // Upper wall
                for (var i = 0; i < room.Width; i++)
                {
                    var tile = _map.GetTileFromCoordinates(room.Position.X + i, room.Position.Y);
                    if (tile.IsConnectorTile) continue;
                    tile.Type = TileType.Wall;
                }
                // Lower wall
                for (var i = 0; i < room.Width; i++)
                {
                    var tile = _map.GetTileFromCoordinates(room.Position.X + i, room.Position.Y + room.Height - 1);
                    if (tile.IsConnectorTile) continue;
                    tile.Type = TileType.Wall;
                }
                // Left wall
                for (var i = 0; i < room.Height; i++)
                {
                    var tile = _map.GetTileFromCoordinates(room.Position.X, room.Position.Y + i);
                    if (tile.IsConnectorTile) continue;
                    tile.Type = TileType.Wall;
                }
                // Right wall
                for (var i = 0; i < room.Height; i++)
                {
                    var tile = _map.GetTileFromCoordinates(room.Position.X + room.Width - 1, room.Position.Y + i);
                    if (tile.IsConnectorTile) continue;
                    tile.Type = TileType.Wall;
                }
                // Floor
                for (var i = 1; i < room.Width - 1; i++)
                {
                    for (var j = 1; j < room.Height - 1; j++)
                    {
                        var tile = _map.GetTileFromCoordinates(room.Position.X + i, room.Position.Y + j);
                        tile.Type = TileType.Floor;
                    }
                }
            }
            else if (room.IsDummy)
            {
                // Dummy room
                var tile = _map.GetTileFromCoordinates(room.Position.X, room.Position.Y);
                tile.Type = TileType.Hallway;
            }
        }

        private void FuseRooms()
        {
            var validFusionTileTypes = new List<RoomDispositionType>() { RoomDispositionType.GuaranteedFusion, RoomDispositionType.RandomConnection };

            for (var row = 0; row < RoomDispositionToUse.GetLength(0); row++)
            {
                for (var column = 0; column < RoomDispositionToUse.GetLength(1); column++)
                {
                    var connectionTile = RoomDispositionToUse[row, column];
                    if (!validFusionTileTypes.Contains(connectionTile)) continue;
                    var (RoomRoow, RoomColumn) = (row / 2, column / 2);
                    var leftRoom = GetRoomByRowAndColumn(row / 2, (column - 1) / 2);
                    var rightRoom = GetRoomByRowAndColumn(row / 2, (column + 1) / 2);
                    var upRoom = GetRoomByRowAndColumn((row - 1) / 2, column / 2);
                    var downRoom = GetRoomByRowAndColumn((row + 1) / 2, column / 2);
                    var isVerticalConnection = column % 2 == 0 && row % 2 != 0;
                    var isHorizontalConnection = column % 2 != 0 && row % 2 == 0;
                    var isHorizontalConnectionValid = isHorizontalConnection && leftRoom != null && !leftRoom.IsDummy && !leftRoom.IsFused && rightRoom != null && !rightRoom.IsDummy && !rightRoom.IsFused;
                    var isVerticalConnectionValid = isVerticalConnection && upRoom != null && !upRoom.IsDummy && !upRoom.IsFused && downRoom != null && !downRoom.IsDummy && !downRoom.IsFused;
                    if (connectionTile == RoomDispositionType.RandomConnection)
                    {
                        if (Rng.RollProbability() < RoomFusionOdds && (isHorizontalConnectionValid || isVerticalConnectionValid))
                            connectionTile = RoomDispositionType.GuaranteedFusion;
                        else
                            connectionTile = RoomDispositionType.GuaranteedHallway;
                    }
                    if (connectionTile == RoomDispositionType.GuaranteedFusion)
                    {
                        Room? fusedRoom;
                        if (isHorizontalConnectionValid)
                        {
                            fusedRoom = CombineRooms(leftRoom, rightRoom);
                            if (fusedRoom != null)
                            {
                                _map.Fusions.Add((leftRoom, rightRoom, fusedRoom));
                                _map.Rooms[_map.Rooms.IndexOf(leftRoom)] = _map.Rooms[_map.Rooms.IndexOf(rightRoom)] = fusedRoom;
                                leftRoom = rightRoom = fusedRoom;
                            }
                        }
                        else if (isVerticalConnectionValid)
                        {
                            fusedRoom = CombineRooms(downRoom, upRoom);
                            if (fusedRoom != null)
                            {
                                _map.Fusions.Add((upRoom, downRoom, fusedRoom));
                                _map.Rooms[_map.Rooms.IndexOf(upRoom)] = _map.Rooms[_map.Rooms.IndexOf(downRoom)] = fusedRoom;
                                downRoom = upRoom = fusedRoom;
                            }
                        }
                        else
                            connectionTile = validFusionTileTypes.TakeRandomElement(Rng);
                    }
                }
            }
        }

        private Room CombineRooms(Room thisRoom, Room adjacentRoom)
        {
            if (adjacentRoom.Width <= 1 || adjacentRoom.Height <= 1) return null;

            // Prevent non-square room fusions
            if (thisRoom.Width > MaxRoomWidth && thisRoom.RoomRow != adjacentRoom.RoomRow) return null;
            if (adjacentRoom.Width > MaxRoomWidth && thisRoom.RoomRow != adjacentRoom.RoomRow) return null;
            if (thisRoom.Height > MaxRoomHeight && thisRoom.RoomColumn != adjacentRoom.RoomColumn) return null;
            if (adjacentRoom.Height > MaxRoomHeight && thisRoom.RoomColumn != adjacentRoom.RoomColumn) return null;

            var minX = Math.Min(thisRoom.Position.X, adjacentRoom.Position.X);
            var maxX = Math.Max(thisRoom.Position.X + thisRoom.Width, adjacentRoom.Position.X + adjacentRoom.Width);
            var minY = Math.Min(thisRoom.Position.Y, adjacentRoom.Position.Y);
            var maxY = Math.Max(thisRoom.Position.Y + thisRoom.Height, adjacentRoom.Position.Y + adjacentRoom.Height);
            var width = Math.Max(maxX - minX, MinRoomWidth);
            var height = Math.Max(maxY - minY, MinRoomHeight);
            return new Room(_map, new GamePoint(minX, minY), thisRoom.RoomRow, thisRoom.RoomColumn, width, height)
            {
                IsFused = true
            };
        }

        private void ConnectRooms()
        {
            var validHallwayTileTypes = new List<RoomDispositionType>() { RoomDispositionType.GuaranteedFusion, RoomDispositionType.GuaranteedHallway, RoomDispositionType.RandomConnection };

            for (var row = 0; row < RoomDispositionToUse.GetLength(0); row++)
            {
                for (var column = 0; column < RoomDispositionToUse.GetLength(1); column++)
                {
                    var connectionTile = RoomDispositionToUse[row, column];
                    if (!validHallwayTileTypes.Contains(connectionTile)) continue;
                    var leftRoom = GetRoomOrFusionByRowAndColumn(row / 2, (column - 1) / 2);
                    var rightRoom = GetRoomOrFusionByRowAndColumn(row / 2, (column + 1) / 2);
                    var upRoom = GetRoomOrFusionByRowAndColumn((row - 1) / 2, column / 2);
                    var downRoom = GetRoomOrFusionByRowAndColumn((row + 1) / 2, column / 2);
                    var isVerticalConnection = column % 2 == 0 && row % 2 != 0;
                    var isHorizontalConnection = column % 2 != 0 && row % 2 == 0;
                    if (connectionTile == RoomDispositionType.RandomConnection)
                        connectionTile = RoomDispositionType.GuaranteedHallway;
                    if (connectionTile == RoomDispositionType.GuaranteedHallway)
                    {
                        var maxConnections = MaxConnectionsBetweenRooms > 1 && Rng.RollProbability() < OddsForExtraConnections
                                             ? Rng.NextInclusive(2, MaxConnectionsBetweenRooms)
                                             : 1;
                        for (int i = 0; i < maxConnections; i++)
                        {
                            if (isHorizontalConnection && leftRoom != null && rightRoom != null)
                                CreateHallway((leftRoom, rightRoom, RoomConnectionType.Horizontal));
                            else if (isVerticalConnection && downRoom != null && upRoom != null)
                                CreateHallway((upRoom, downRoom, RoomConnectionType.Vertical));
                        }
                    }
                }
            }
        }

        private void GetPossibleRoomData()
        {
            // Calculate MaxRoomWidth and MaxRoomHeight with adjusted constraints
            MaxRoomWidth = Math.Max(Math.Min(_generatorToUse.MaxRoomSize.Width, Width), Width / RoomCountColumns);
            MaxRoomHeight = Math.Max(Math.Min(_generatorToUse.MaxRoomSize.Height, Height), Height / RoomCountRows);

            // Ensure the room size is at least 5x5 and the min width/height is not less than that
            if (MaxRoomWidth < EngineConstants.MinRoomWidthOrHeight || MaxRoomHeight < EngineConstants.MinRoomWidthOrHeight)
                throw new InvalidDataException("Grid size or floor dimensions are too small to support minimum room size of 5x5");

            MinRoomWidth = Math.Max(MaxRoomWidth / 4, EngineConstants.MinRoomWidthOrHeight);
            MinRoomWidth = Math.Max(MinRoomWidth, _generatorToUse.MinRoomSize.Width);

            MinRoomHeight = Math.Max(MaxRoomHeight / 4, EngineConstants.MinRoomWidthOrHeight);
            MinRoomHeight = Math.Max(MinRoomHeight, _generatorToUse.MinRoomSize.Height);

            // Centering the grid in the floor
            var widthGap = (Width - (MaxRoomWidth * RoomCountColumns)) / 2;
            var heightGap = (Height - (MaxRoomHeight * RoomCountRows)) / 2;

            RoomLimitsTable = new (GamePoint topLeftCorner, GamePoint bottomRightCorner)[RoomCountRows, RoomCountColumns];

            // Calculating grid cell boundaries
            for (int i = 0; i < RoomCountRows; i++)
            {
                for (int j = 0; j < RoomCountColumns; j++)
                {
                    var topLeftCorner = new GamePoint
                    {
                        X = widthGap + (MaxRoomWidth * j),
                        Y = heightGap + (MaxRoomHeight * i)
                    };
                    var bottomRightCorner = new GamePoint
                    {
                        X = Math.Min(Width - widthGap, topLeftCorner.X + MaxRoomWidth - 1),
                        Y = Math.Min(Height - heightGap, topLeftCorner.Y + MaxRoomHeight - 1)
                    };

                    if (topLeftCorner.X < 0 || topLeftCorner.Y < 0 || bottomRightCorner.X < 0 || bottomRightCorner.Y < 0 || bottomRightCorner.X < topLeftCorner.X || bottomRightCorner.Y < topLeftCorner.Y)
                        throw new InvalidDataException($"Floor data for Floor Level {FloorLevel} is incorrect. Room cell ({j}, {i}) produced incorrect boundaries of ({topLeftCorner.X}, {topLeftCorner.Y}) to ({bottomRightCorner.X}, {bottomRightCorner.Y}).");

                    RoomLimitsTable[i, j] = (topLeftCorner, bottomRightCorner);
                }
            }
        }
        private void CreateHallway((Room Source, Room Target, RoomConnectionType Tag) edge)
        {
            var roomA = edge.Source;
            var roomB = edge.Target;

            try
            {
                if (edge.Tag == RoomConnectionType.Horizontal)
                {
                    if (roomA.BottomRight.X < roomB.BottomLeft.X)
                        CreateHorizontalHallway(roomA, roomB);
                    else if (roomA.BottomRight.X > roomB.BottomLeft.X)
                        CreateHorizontalHallway(roomB, roomA);
                    else
                        return;
                }
                else if (edge.Tag == RoomConnectionType.Vertical)
                {
                    if (roomA.BottomLeft.Y < roomB.BottomRight.Y)
                        CreateVerticalHallway(roomA, roomB);
                    else if (roomA.BottomLeft.Y > roomB.BottomRight.Y)
                        CreateVerticalHallway(roomB, roomA);
                    else
                        return;
                }
            }
            catch
            {
                // If cannot build Hallway, pretend we never tried.
            }
        }

        private void CreateHorizontalHallway(Room leftRoom, Room rightRoom)
        {
            Tile? leftConnector = null, rightConnector = null, connectorA = null, connectorB = null;

            if (leftRoom.IsDummy)
                leftConnector = _map.GetTileFromCoordinates(leftRoom.Position.X, leftRoom.Position.Y);
            if (rightRoom.IsDummy)
                rightConnector = _map.GetTileFromCoordinates(rightRoom.Position.X, rightRoom.Position.Y);

            var leftRoomMinY = leftRoom.Position.Y + 1;
            var leftRoomMaxY = leftRoom.Position.Y + leftRoom.Height - 2;

            var rightRoomMinY = rightRoom.Position.Y + 1;
            var rightRoomMaxY = rightRoom.Position.Y + rightRoom.Height - 2;

            var largerMinY = leftRoomMinY > rightRoomMinY ? leftRoomMinY : rightRoomMinY;
            var lowerMaxY = leftRoomMaxY > rightRoomMaxY ? rightRoomMaxY : leftRoomMaxY;

            var minY = largerMinY > lowerMaxY ? lowerMaxY : largerMinY;
            var maxY = largerMinY > lowerMaxY ? largerMinY : lowerMaxY;

            if (leftConnector == null)
            {
                var x = leftRoom.Position.X + leftRoom.Width - 1;
                var y = Rng.NextInclusive(leftRoomMinY, leftRoomMaxY);
                leftConnector = _map.GetTileFromCoordinates(x, y);
            }

            if (rightConnector == null)
            {
                var x = rightRoom.Position.X;
                var y = Rng.NextInclusive(rightRoomMinY, rightRoomMaxY);
                rightConnector = _map.GetTileFromCoordinates(x, y);
            }

            var hallwayGenerationTries = 0;
            List<Tile> tilesToConvert;
            bool isValidHallway;

            do
            {
                hallwayGenerationTries++;
                tilesToConvert = new();

                var (InBetweenConnectorPosition, ConnectorAPosition, ConnectorBPosition) = CalculateConnectionPosition(leftConnector, rightConnector, RoomConnectionType.Horizontal);

                if (ConnectorAPosition != null)
                {
                    connectorA = _map.GetTileFromCoordinates(ConnectorAPosition);
                    tilesToConvert.Add(connectorA);
                }
                if (ConnectorBPosition != null)
                {
                    connectorB = _map.GetTileFromCoordinates(ConnectorBPosition);
                    tilesToConvert.Add(connectorB);
                }

                if (InBetweenConnectorPosition != null)
                {
                    // Horizontal line from Left Room to Hallway connection column
                    for (var i = ConnectorAPosition.X; i <= InBetweenConnectorPosition.X; i++)
                    {
                        tilesToConvert.Add(_map.GetTileFromCoordinates(i, ConnectorAPosition.Y));
                    }

                    // Horizontal line from Right Room to Hallway connection column
                    for (var i = InBetweenConnectorPosition.X; i <= ConnectorBPosition.X; i++)
                    {
                        tilesToConvert.Add(_map.GetTileFromCoordinates(i, ConnectorBPosition.Y));
                    }

                    // Draw a downwards line in case entryGamePoint from left is higher or equal to entryGamePoint from right
                    for (var i = rightConnector.Position.Y + 1; i < ConnectorAPosition.Y; i++)
                    {
                        tilesToConvert.Add(_map.GetTileFromCoordinates(InBetweenConnectorPosition.X, i));
                    }
                    // Draw an upwards line in case entryGamePoint from left is lower than entryGamePoint from right
                    for (var i = ConnectorAPosition.Y + 1; i < ConnectorBPosition.Y; i++)
                    {
                        tilesToConvert.Add(_map.GetTileFromCoordinates(InBetweenConnectorPosition.X, i));
                    }
                }

                tilesToConvert = tilesToConvert.Distinct().OrderByDescending(t => t.Position.Y).ThenBy(t => t.Position.X).ToList();

                isValidHallway = IsHallwayTileGroupValid(tilesToConvert, connectorA, connectorB);
            }
            while (!isValidHallway && hallwayGenerationTries < EngineConstants.MaxGenerationTriesForHallway);

            if (isValidHallway)
            {
                BuildHallwayTiles(tilesToConvert, connectorA, connectorB);
                _map.Hallways.Add((leftRoom, rightRoom, connectorA, connectorB, tilesToConvert));
            }
        }

        private void CreateVerticalHallway(Room topRoom, Room downRoom)
        {
            Tile? topConnector = null, downConnector = null, connectorA = null, connectorB = null;

            if (topRoom.IsDummy)
                topConnector = _map.GetTileFromCoordinates(topRoom.Position.X, topRoom.Position.Y);
            if (downRoom.IsDummy)
                downConnector = _map.GetTileFromCoordinates(downRoom.Position.X, downRoom.Position.Y);

            var topRoomMinX = topRoom.Position.X + 1;
            var topRoomMaxX = topRoom.Position.X + topRoom.Width - 2;

            var downRoomMinX = downRoom.Position.X + 1;
            var downRoomMaxX = downRoom.Position.X + downRoom.Width - 2;

            var largerMinX = topRoomMinX > downRoomMinX ? topRoomMinX : downRoomMinX;
            var lowerMaxX = topRoomMinX > downRoomMinX ? downRoomMinX : topRoomMinX;

            var minX = largerMinX > lowerMaxX ? lowerMaxX : largerMinX;
            var maxX = largerMinX > lowerMaxX ? largerMinX : lowerMaxX;

            if (topConnector == null)
            {
                var x = Rng.NextInclusive(topRoomMinX, topRoomMaxX);
                var y = topRoom.Position.Y + topRoom.Height - 1;
                topConnector = _map.GetTileFromCoordinates(x, y);
            }

            if (downConnector == null)
            {
                var x = Rng.NextInclusive(downRoomMinX, downRoomMaxX);
                var y = downRoom.Position.Y;
                downConnector = _map.GetTileFromCoordinates(x, y);
            }

            var hallwayGenerationTries = 0;
            List<Tile> tilesToConvert;
            bool isValidHallway;

            do
            {
                hallwayGenerationTries++;
                tilesToConvert = new();

                var (InBetweenConnectorPosition, ConnectorAPosition, ConnectorBPosition) = CalculateConnectionPosition(topConnector, downConnector, RoomConnectionType.Vertical);

                if (ConnectorAPosition != null)
                {
                    connectorA = _map.GetTileFromCoordinates(ConnectorAPosition);
                    tilesToConvert.Add(connectorA);
                }
                if (ConnectorBPosition != null)
                {
                    connectorB = _map.GetTileFromCoordinates(ConnectorBPosition);
                    tilesToConvert.Add(connectorB);
                }

                if (InBetweenConnectorPosition != null)
                {
                    // Vertical line from Up Room to Hallway connection row
                    for (var i = ConnectorAPosition.Y; i <= InBetweenConnectorPosition.Y; i++)
                    {
                        tilesToConvert.Add(_map.GetTileFromCoordinates(ConnectorAPosition.X, i));
                    }

                    // Vertical line from Down Room to Hallway connection row
                    for (var i = InBetweenConnectorPosition.Y; i <= ConnectorBPosition.Y; i++)
                    {
                        tilesToConvert.Add(_map.GetTileFromCoordinates(ConnectorBPosition.X, i));
                    }

                    // Draw a rightwards line in case entryGamePoint from up is more or equal to the right to entryGamePoint from below
                    for (var i = ConnectorBPosition.X + 1; i < ConnectorAPosition.X; i++)
                    {
                        tilesToConvert.Add(_map.GetTileFromCoordinates(i, InBetweenConnectorPosition.Y));
                    }
                    // Draw a leftwards line in case entryGamePoint from up is more to the left than entryGamePoint from below
                    for (var i = ConnectorAPosition.X + 1; i < ConnectorBPosition.X; i++)
                    {
                        tilesToConvert.Add(_map.GetTileFromCoordinates(i, InBetweenConnectorPosition.Y));
                    }
                }

                tilesToConvert = tilesToConvert.Distinct().OrderBy(t => t.Position.Y).ThenBy(t => t.Position.X).ToList();
                isValidHallway = IsHallwayTileGroupValid(tilesToConvert, topConnector, downConnector);
            }
            while (!isValidHallway && hallwayGenerationTries < EngineConstants.MaxGenerationTriesForHallway);

            if (isValidHallway)
            {
                BuildHallwayTiles(tilesToConvert, connectorA, connectorB);
                _map.Hallways.Add((topRoom, downRoom, connectorA, connectorB, tilesToConvert));
            }
        }

        private (GamePoint? InBetweenConnectorPosition, GamePoint? ConnectorAPosition, GamePoint? ConnectorBPosition) CalculateConnectionPosition(Tile connectorA, Tile connectorB, RoomConnectionType connectionType)
        {
            int minX = 0, maxX = -1, minY = 0, maxY = -1;
            var connectorAPosition = new GamePoint(connectorA.Position.X, connectorA.Position.Y);
            var connectorBPosition = new GamePoint(connectorB.Position.X, connectorB.Position.Y);

            if (connectionType == RoomConnectionType.Horizontal)
            {
                if (connectorB.Position.X < connectorA.Position.X)
                {
                    (connectorB, connectorA) = (connectorA, connectorB);
                    (connectorBPosition, connectorAPosition) = (connectorAPosition, connectorBPosition);
                }
                if (Math.Abs(connectorA.Position.X - connectorB.Position.X) == 1)
                {
                    if (connectorA.Position.Y == connectorB.Position.Y)
                        return (null, connectorA.Position, connectorB.Position);
                    var leftYs = Enumerable.Range(connectorA.Room.Position.Y + 1, Math.Max(1, connectorA.Room.Height - 2)).ToList();
                    var rightYs = Enumerable.Range(connectorB.Room.Position.Y + 1, Math.Max(1, connectorB.Room.Height - 2)).ToList();
                    var sharedYs = leftYs.Intersect(rightYs);
                    if (!sharedYs.Any()) return (null, null, null);
                    var pickedY = sharedYs.TakeRandomElement(Rng);
                    connectorAPosition = new GamePoint(connectorA.Position.X, pickedY);
                    connectorBPosition = new GamePoint(connectorB.Position.X, pickedY);
                    return (null, connectorAPosition, connectorBPosition); // No need for a connection GamePoint in this case.
                }

                minX = connectorA.Position.X + 1;
                maxX = connectorB.Position.X - 1;
                minY = Math.Min(connectorA.Position.Y, connectorB.Position.Y);
                maxY = Math.Max(connectorA.Position.Y, connectorB.Position.Y);
            }
            else if (connectionType == RoomConnectionType.Vertical)
            {
                if (connectorB.Position.Y < connectorA.Position.Y)
                {
                    (connectorB, connectorA) = (connectorA, connectorB);
                    (connectorBPosition, connectorAPosition) = (connectorAPosition, connectorBPosition);
                }

                if (Math.Abs(connectorA.Position.Y - connectorB.Position.Y) == 1)
                {
                    if (connectorA.Position.X == connectorB.Position.X)
                        return (null, connectorA.Position, connectorB.Position);
                    var topXs = Enumerable.Range(connectorA.Room.Position.X + 1, Math.Max(1, connectorA.Room.Width - 2)).ToList();
                    var downXs = Enumerable.Range(connectorB.Room.Position.X + 1, Math.Max(1, connectorB.Room.Width - 2)).ToList();
                    var sharedXs = topXs.Intersect(downXs);
                    if (!sharedXs.Any()) return (null, null, null);
                    var pickedX = sharedXs.TakeRandomElement(Rng);
                    connectorAPosition = new GamePoint(pickedX, connectorA.Position.Y);
                    connectorBPosition = new GamePoint(pickedX, connectorB.Position.Y);
                    return (null, connectorAPosition, connectorBPosition); // No need for a connection GamePoint in this case.
                }

                minY = connectorA.Position.Y + 1;
                maxY = connectorB.Position.Y - 1;
                minX = Math.Min(connectorA.Position.X, connectorB.Position.X);
                maxX = Math.Max(connectorA.Position.X, connectorB.Position.X);
            }

            try
            {
                var connectionPosition = new GamePoint
                {
                    X = Rng.NextInclusive(minX, maxX),
                    Y = Rng.NextInclusive(minY, maxY)
                };

                return (connectionPosition, connectorAPosition, connectorBPosition);
            }
            catch
            {
                return (null, null, null);
            }
        }

        private static bool IsHallwayTileGroupValid(List<Tile> tilesToConvert, Tile connectorA, Tile connectorB)
        {
            if (!tilesToConvert.Any()) return false;

            foreach (var tile in tilesToConvert)
            {
                if (tile == connectorA || tile == connectorB) continue;
                if (tile.Room != null)
                    return false;
            }
            return true;
        }

        private static void BuildHallwayTiles(List<Tile> tilesToConvert, Tile connectorA, Tile connectorB)
        {
            foreach (var tile in tilesToConvert)
            {
                tile.Type = TileType.Hallway;
                if (tile.Room != null)
                    tile.IsConnectorTile = true;
            }
        }

        private (int MinX, int MinY, int MaxX, int MaxY) GetPossibleCoordinatesForRoom(int row, int column)
        {
            var (TopLeftCorner, BottomRightCorner) = RoomLimitsTable[row, column];
            return (TopLeftCorner.X, TopLeftCorner.Y, BottomRightCorner.X, BottomRightCorner.Y);
        }

        private Room? GetRoomByRowAndColumn(int row, int column) => _map.Rooms.Find(r => r.RoomRow == row && r.RoomColumn == column);

        private Room? GetRoomOrFusionByRowAndColumn(int row, int column)
        {
            var room = GetRoomByRowAndColumn(row, column);
            if (room == null)
            {
                var fusionWithRoom = _map.Fusions.FirstOrDefault(f => (f.RoomA.RoomColumn == column && f.RoomA.RoomRow == row)
                                                        || f.RoomB.RoomColumn == column && f.RoomB.RoomRow == row);
                if (fusionWithRoom != default)
                    room = fusionWithRoom.FusedRoom;
            }
            return room;
        }
        private enum RoomConnectionType
        {
            Horizontal = 1,
            Vertical = 2,
            None = 0
        }

        #endregion

        #region Special Tiles
        public void CreateSpecialTiles()
        {
            if (FloorConfigurationToUse.PossibleSpecialTiles == null || !FloorConfigurationToUse.PossibleSpecialTiles.Any()) return;
            foreach (var specialTileGenerator in FloorConfigurationToUse.PossibleSpecialTiles)
            {
                if (specialTileGenerator.GeneratorType == SpecialTileGenerationAlgorithm.Lake)
                {
                    var roomCellsToUse = RoomLimitsTable.TakeNDifferentRandomElements(Rng.NextInclusive(specialTileGenerator.MinSpecialTileGenerations, specialTileGenerator.MaxSpecialTileGenerations), Rng);
                    foreach (var roomCell in roomCellsToUse)
                    {
                        CreateLake((roomCell.TopLeftCorner, roomCell.BottomRightCorner), specialTileGenerator.TileType);
                    }
                }
                else if (RoomLimitsTable.Count() > 1)
                {
                    for (int i = 0; i < Rng.NextInclusive(specialTileGenerator.MinSpecialTileGenerations, specialTileGenerator.MaxSpecialTileGenerations); i++)
                    {
                        var roomCellsToUse = RoomLimitsTable.TakeNDifferentRandomElements(2, Rng);
                        var cellA = roomCellsToUse[0];
                        var cellB = roomCellsToUse[1];
                        CreateRiver((cellA.TopLeftCorner, cellA.BottomRightCorner), (cellB.TopLeftCorner, cellB.BottomRightCorner), specialTileGenerator.TileType);
                    }
                }
            }
        }

        private void CreateRiver((GamePoint TopLeftCorner, GamePoint BottomRightCorner) topLimits, (GamePoint TopLeftCorner, GamePoint BottomRightCorner) downLimits, TileType specialTileType)
        {
            Tile? topConnector = null, downConnector = null, connectorA = null, connectorB = null;

            var topLimitsMinX = topLimits.TopLeftCorner.X + 1;
            var topLimitsMaxX = topLimits.BottomRightCorner.X - 1;
            var topLimitsMinY = topLimits.TopLeftCorner.Y + 1;
            var topLimitsMaxY = topLimits.BottomRightCorner.Y - 1;

            var downLimitsMinX = downLimits.TopLeftCorner.X + 1;
            var downLimitsMaxX = downLimits.BottomRightCorner.X - 1;
            var downLimitsMinY = downLimits.TopLeftCorner.Y + 1;
            var downLimitsMaxY = downLimits.BottomRightCorner.Y - 1;

            if (topConnector == null)
            {
                var x = Rng.NextInclusive(topLimitsMinX, topLimitsMaxX);
                var y = Rng.NextInclusive(topLimitsMinY, topLimitsMaxY);
                topConnector = _map.GetTileFromCoordinates(x, y);
            }

            if (downConnector == null)
            {
                var x = Rng.NextInclusive(downLimitsMinX, downLimitsMaxX);
                var y = Rng.NextInclusive(downLimitsMinY, downLimitsMaxY);
                downConnector = _map.GetTileFromCoordinates(x, y);
            }

            var riverGenerationTries = 0;
            List<Tile> tilesToConvert;
            bool isValidRiver;

            do
            {
                riverGenerationTries++;
                tilesToConvert = new();

                var (InBetweenConnectorPosition, ConnectorAPosition, ConnectorBPosition) = CalculateConnectionPosition(topConnector, downConnector, RoomConnectionType.Vertical);

                if (ConnectorAPosition != null)
                {
                    connectorA = _map.GetTileFromCoordinates(ConnectorAPosition);
                    tilesToConvert.Add(connectorA);
                }
                if (ConnectorBPosition != null)
                {
                    connectorB = _map.GetTileFromCoordinates(ConnectorBPosition);
                    tilesToConvert.Add(connectorB);
                }

                if (InBetweenConnectorPosition != null)
                {
                    // Vertical line from Up Room to Hallway connection row
                    for (var i = ConnectorAPosition.Y; i <= InBetweenConnectorPosition.Y; i++)
                    {
                        tilesToConvert.Add(_map.GetTileFromCoordinates(ConnectorAPosition.X, i));
                    }

                    // Vertical line from Down Room to Hallway connection row
                    for (var i = InBetweenConnectorPosition.Y; i <= ConnectorBPosition.Y; i++)
                    {
                        tilesToConvert.Add(_map.GetTileFromCoordinates(ConnectorBPosition.X, i));
                    }

                    // Draw a rightwards line in case entryGamePoint from up is more or equal to the right to entryGamePoint from below
                    for (var i = ConnectorBPosition.X + 1; i < ConnectorAPosition.X; i++)
                    {
                        tilesToConvert.Add(_map.GetTileFromCoordinates(i, InBetweenConnectorPosition.Y));
                    }
                    // Draw a leftwards line in case entryGamePoint from up is more to the left than entryGamePoint from below
                    for (var i = ConnectorAPosition.X + 1; i < ConnectorBPosition.X; i++)
                    {
                        tilesToConvert.Add(_map.GetTileFromCoordinates(i, InBetweenConnectorPosition.Y));
                    }
                }

                tilesToConvert = tilesToConvert.Distinct().OrderBy(t => t.Position.Y).ThenBy(t => t.Position.X).ToList();
                isValidRiver = IsSpecialTileGroupValid(tilesToConvert);
            }
            while (!isValidRiver && riverGenerationTries < EngineConstants.MaxGenerationTriesForRiver);

            if (isValidRiver)
            {
                BuildSpecialTiles(tilesToConvert, specialTileType);
            }
        }

        private void CreateLake((GamePoint TopLeftCorner, GamePoint BottomRightCorner) limits, TileType specialTileType)
        {
            var minX = limits.TopLeftCorner.X + 1;
            var maxX = limits.BottomRightCorner.X - 1;
            var minY = limits.TopLeftCorner.Y + 1;
            var maxY = limits.BottomRightCorner.Y - 1;
            var lakeGenerationTries = 0;

            List<Tile> tilesToConvert;
            bool isValidLake;

            do
            {
                lakeGenerationTries++;
                tilesToConvert = new();

                var filteredTiles = _map.Tiles.Where(tile =>
                    tile.Position.X >= minX && tile.Position.X <= maxX &&
                    tile.Position.Y >= minY && tile.Position.Y <= maxY);

                // We remove all the picked Tiles that lack a direct connection with one that is already walkable (to avoid producing unnecessary Islands)
                filteredTiles = filteredTiles.Where(tile => _map.GetAdjacentWalkableTiles(tile.Position, false).Any()).ToList();

                var maximumElements = filteredTiles.Count / 10;
                tilesToConvert = filteredTiles.TakeNDifferentRandomElements(Rng.NextInclusive(maximumElements), Rng);

                isValidLake = IsSpecialTileGroupValid(tilesToConvert);
            }
            while (!isValidLake && lakeGenerationTries < EngineConstants.MaxGenerationTriesForRiver);

            if (isValidLake)
            {
                BuildSpecialTiles(tilesToConvert, specialTileType);
            }
        }

        private static bool IsSpecialTileGroupValid(List<Tile> tilesToConvert)
        {
            if (!tilesToConvert.Any()) return false;

            foreach (var tile in tilesToConvert)
            {
                if (tile.Type == TileType.Stairs || (tile.Room != null && tile.Room.IsDummy))
                    return false;
            }
            return true;
        }

        private static void BuildSpecialTiles(List<Tile> tilesToConvert, TileType tileType)
        {
            foreach (var tile in tilesToConvert)
            {
                if (tile.Type == TileType.Hallway) continue;
                tile.Type = tileType;
            }
        }

        #endregion

        #region Entities

        public async Task PlaceEntities()
        {
            // Do nothing here. Entities are placed in the MapGenerator after the map is fully generated.
        }

        #endregion

        #region Keys and Doors
        public async Task PlaceKeysAndDoors()
        {
            var keyGenerationData = FloorConfigurationToUse.PossibleKeys;
            if (!keyGenerationData.KeyTypes.Any() || keyGenerationData.MaxPercentageOfLockedCandidateRooms < 1) return;
            foreach (var doorTile in _map.Tiles.Where(t => t.Type == TileType.Door))
            {
                doorTile.Type = TileType.Hallway;
                doorTile.DoorId = string.Empty;
            }
            var nonDummyRooms = _map.Rooms.Where(r => !r.IsDummy).ToList().Shuffle(Rng);
            if (nonDummyRooms.Count == 1) return;
            var maximumLockableRooms = (int)Math.Round(nonDummyRooms.Count * ((decimal)keyGenerationData.MaxPercentageOfLockedCandidateRooms / 100), 0, MidpointRounding.AwayFromZero);
            var lockedRooms = 0;
            var usedKeyTypes = new List<KeyType>();
            _map.Keys.Clear();
            foreach (var AICharacter in _map.AICharacters)
            {
                var keysInInventory = AICharacter.Inventory.Where(i => i.EntityType == EntityType.Key);
                AICharacter.Inventory = AICharacter.Inventory.Except(keysInInventory).ToList();
                foreach (var key in keysInInventory)
                {
                    key.ExistenceStatus = EntityExistenceStatus.Gone;
                    key.Owner = null;
                    key.Position = null;
                }
            }
            foreach (var room in nonDummyRooms)
            {
                if (usedKeyTypes.Count >= keyGenerationData.KeyTypes.Count) break;
                if (lockedRooms >= maximumLockableRooms) break;
                if (!IsCandidateRoom(room)) continue;
                if (Rng.RollProbability() > keyGenerationData.LockedRoomOdds) continue;
                var exitTiles = room.GetTiles().Where(t => t.IsConnectorTile);
                var usableKeyTypes = keyGenerationData.KeyTypes.Where(kt => !usedKeyTypes.Contains(kt)
                && ((room.HasStairs && kt.CanLockStairs) || (room.HasItems && kt.CanLockItems)));
                if (!usableKeyTypes.Any()) continue;
                var keyTypeToUse = usableKeyTypes.TakeRandomElement(Rng);
                if (keyTypeToUse.KeyTypeName == "") continue;
                foreach (var tile in exitTiles)
                {
                    tile.Type = TileType.Door;
                    tile.DoorId = keyTypeToUse.KeyTypeName;
                }
                lockedRooms++;

                var islands = _map.Tiles.GetIslands(t => t.IsWalkable || usedKeyTypes.Select(ukt => ukt.KeyTypeName).Contains(t.DoorId));
                var islandWithPlayer = islands.FirstOrDefault(i => i.Contains(_map.Player.ContainingTile));

                if (await _map.AddEntity(keyTypeToUse.KeyClass) is Item keyEntity && Rng.RollProbability() <= keyGenerationData.KeySpawnInEnemyInventoryOdds)
                {
                    var enemiesInPlayerIsland = _map.AICharacters.Where(c => !c.Inventory.Any(i => i.EntityType == EntityType.Key) && islandWithPlayer.Contains(c.ContainingTile) && c.Faction.IsEnemyWith(_map.Player.Faction) && c.Visible);
                    if (enemiesInPlayerIsland.Any())
                        enemiesInPlayerIsland.TakeRandomElement(Rng).PickItem(keyEntity, false);
                }

                usedKeyTypes.Add(keyTypeToUse);
            }
        }

        private bool IsCandidateRoom(Room room)
        {
            if (room.IsDummy) return false;
            if (room.HasStairs)
                return true;
            if (room.HasItems)
                return true;

            foreach (var (RoomA, RoomB, _, _, _) in _map.Hallways.Where(h => h.RoomA == room || h.RoomB == room))
            {
                if (RoomA == room)
                    if (RoomB.GetTiles().Any(t => t.Type == TileType.Door))
                        return true;
                if (RoomB == room)
                    if (RoomA.GetTiles().Any(t => t.Type == TileType.Door))
                        return true;
            }

            return false;
        }
        #endregion

        #region Player
        public Task PlacePlayer() => _map.PlacePlayer();
        #endregion

        #region Stairs
        public void PlaceStairs() => _map.SetStairs();
        #endregion
    }
}
