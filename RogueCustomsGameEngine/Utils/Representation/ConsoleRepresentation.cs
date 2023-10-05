using System.Drawing;
using System.Text.Json.Serialization;

namespace RogueCustomsGameEngine.Utils.Representation
{
    public class ConsoleRepresentation
    {
        [JsonConstructor]
        public ConsoleRepresentation() { }

        public char Character { get; set; }
        public GameColor BackgroundColor { get; set; } = new GameColor(Color.Black);
        public GameColor ForegroundColor { get; set; } = new GameColor(Color.White);

        public static ConsoleRepresentation EmptyTile { get; set; }

        public override bool Equals(object? obj)
        {
            if(obj is ConsoleRepresentation cr)
            {
                return Character == cr.Character
                    && BackgroundColor.Equals(cr.BackgroundColor)
                    && ForegroundColor.Equals(cr.ForegroundColor);
            }
            return false;
        }
    }
}
