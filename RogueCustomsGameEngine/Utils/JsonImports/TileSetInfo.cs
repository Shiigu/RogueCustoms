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
        public List<TileTypeSetInfo> TileTypes { get; set; }
    }

    [Serializable]
    public class TileTypeSetInfo
    {
        public string TileTypeId { get; set; }

        public ConsoleRepresentation Connector { get; set; }
        public ConsoleRepresentation Central { get; set; }
        public ConsoleRepresentation TopLeft { get; set; }
        public ConsoleRepresentation TopRight { get; set; }
        public ConsoleRepresentation BottomLeft { get; set; }
        public ConsoleRepresentation BottomRight { get; set; }

        public ConsoleRepresentation Horizontal { get; set; }
        public ConsoleRepresentation HorizontalBottom { get; set; }
        public ConsoleRepresentation HorizontalTop { get; set; }

        public ConsoleRepresentation Vertical { get; set; }
        public ConsoleRepresentation VerticalLeft { get; set; }
        public ConsoleRepresentation VerticalRight { get; set; }
    }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
