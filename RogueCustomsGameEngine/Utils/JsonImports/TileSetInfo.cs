using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Utils.JsonImports
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public class TileSetInfo
    {
        public string Id { get; set; }

        #region Wall Tiles

        public ConsoleRepresentation TopLeftWall { get; set; }
        public ConsoleRepresentation TopRightWall { get; set; }
        public ConsoleRepresentation BottomLeftWall { get; set; }
        public ConsoleRepresentation BottomRightWall { get; set; }

        public ConsoleRepresentation HorizontalWall { get; set; }

        public ConsoleRepresentation VerticalWall { get; set; }

        #endregion

        #region Hallway Tiles

        public ConsoleRepresentation ConnectorWall { get; set; }
        public ConsoleRepresentation TopLeftHallway { get; set; }
        public ConsoleRepresentation TopRightHallway { get; set; }
        public ConsoleRepresentation BottomLeftHallway { get; set; }
        public ConsoleRepresentation BottomRightHallway { get; set; }

        public ConsoleRepresentation HorizontalHallway { get; set; }
        public ConsoleRepresentation HorizontalBottomHallway { get; set; }
        public ConsoleRepresentation HorizontalTopHallway { get; set; }

        public ConsoleRepresentation VerticalHallway { get; set; }
        public ConsoleRepresentation VerticalLeftHallway { get; set; }
        public ConsoleRepresentation VerticalRightHallway { get; set; }

        public ConsoleRepresentation CentralHallway { get; set; }

        #endregion

        #region Floor Tiles

        public ConsoleRepresentation Floor { get; set; }
        public ConsoleRepresentation Stairs { get; set; }

        #endregion

        public ConsoleRepresentation Empty { get; set; }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
