using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Game.DungeonStructure
{
    [Serializable]
    public class TileSet
    {
        public string Id { get; set; }

        public TileSet(TileSetInfo importable, Dungeon dungeon)
        {
            Id = importable.Id;
            TileTypeSets = new();
            foreach (var tileTypeSet in importable.TileTypes)
            {
                TileTypeSets.Add(new TileTypeSet(tileTypeSet, dungeon));
            }
        }

        public ConsoleRepresentation Empty => TileTypeSets.Find(tts => tts.TileType == TileType.Empty).Central;

        public List<TileTypeSet> TileTypeSets { get; set; }
    }

    public class TileTypeSet
    {
        public TileType TileType { get; set; }


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

        public TileTypeSet(TileTypeSetInfo importable, Dungeon dungeon)
        {
            TileType = dungeon.TileTypes.FirstOrDefault(tt => tt.Id.Equals(importable.TileTypeId))
                            ?? throw new ArgumentException($"There is no Tile of Type {importable.TileTypeId}");
            TopLeft = importable.TopLeft;
            TopRight = importable.TopRight;
            BottomLeft = importable.BottomLeft;
            BottomRight = importable.BottomRight;
            Horizontal = importable.Horizontal;
            Connector = importable.Connector;
            Vertical = importable.Vertical;
            TopLeft = importable.TopLeft;
            TopRight = importable.TopRight;
            BottomLeft = importable.BottomLeft;
            BottomRight = importable.BottomRight;
            Horizontal = importable.Horizontal;
            HorizontalBottom = importable.HorizontalBottom;
            HorizontalTop = importable.HorizontalTop;
            Vertical = importable.Vertical;
            VerticalLeft = importable.VerticalLeft;
            VerticalRight = importable.VerticalRight;
            Central = importable.Central;
        }
    }
}
