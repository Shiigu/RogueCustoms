using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Game.DungeonStructure
{
    public class TileSet
    {
        public string Id { get; set; }

        public TileSet(TileSetInfo importable)
        {
            Id = importable.Id;
            TopLeftWall = importable.TopLeftWall;
            TopRightWall = importable.TopRightWall;
            BottomLeftWall = importable.BottomLeftWall;
            BottomRightWall = importable.BottomRightWall;
            HorizontalWall = importable.HorizontalWall;
            ConnectorWall = importable.ConnectorWall;
            VerticalWall = importable.VerticalWall;
            TopLeftHallway = importable.TopLeftHallway;
            TopRightHallway = importable.TopRightHallway;
            BottomLeftHallway = importable.BottomLeftHallway;
            BottomRightHallway = importable.BottomRightHallway;
            HorizontalHallway = importable.HorizontalHallway;
            HorizontalBottomHallway = importable.HorizontalBottomHallway;
            HorizontalTopHallway = importable.HorizontalTopHallway;
            VerticalHallway = importable.VerticalHallway;
            VerticalLeftHallway = importable.VerticalLeftHallway;
            VerticalRightHallway = importable.VerticalRightHallway;
            CentralHallway = importable.CentralHallway;
            Floor = importable.Floor;
            Stairs = importable.Stairs;
            Empty = importable.Empty;
        }

        #region Wall Tiles

        public ConsoleRepresentation TopLeftWall { get; private set; }
        public ConsoleRepresentation TopRightWall { get; private set; }
        public ConsoleRepresentation BottomLeftWall { get; private set; }
        public ConsoleRepresentation BottomRightWall { get; private set; }

        public ConsoleRepresentation HorizontalWall { get; private set; }
        public ConsoleRepresentation VerticalWall { get; private set; }

        #endregion

        #region Hallway Tiles
        public ConsoleRepresentation ConnectorWall { get; private set; }
        public ConsoleRepresentation TopLeftHallway { get; private set; }
        public ConsoleRepresentation TopRightHallway { get; private set; }
        public ConsoleRepresentation BottomLeftHallway { get; private set; }
        public ConsoleRepresentation BottomRightHallway { get; private set; }

        public ConsoleRepresentation HorizontalHallway { get; private set; }
        public ConsoleRepresentation HorizontalBottomHallway { get; private set; }
        public ConsoleRepresentation HorizontalTopHallway { get; private set; }

        public ConsoleRepresentation VerticalHallway { get; private set; }
        public ConsoleRepresentation VerticalLeftHallway { get; private set; }
        public ConsoleRepresentation VerticalRightHallway { get; private set; }

        public ConsoleRepresentation CentralHallway { get; private set; }

        #endregion

        #region Floor Tiles

        public ConsoleRepresentation Floor { get; private set; }
        public ConsoleRepresentation Stairs { get; private set; }

        #endregion

        public ConsoleRepresentation Empty { get; private set; }
    }
}
