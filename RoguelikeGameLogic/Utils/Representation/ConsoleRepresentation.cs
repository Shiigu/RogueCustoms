using System.Drawing;
using System.Text.Json.Serialization;

namespace RoguelikeGameEngine.Utils.Representation
{
    public class ConsoleRepresentation
    {
        [JsonConstructor]
        public ConsoleRepresentation() { }

        public char Character { get; set; }
        public GameColor BackgroundColor { get; set; } = new GameColor(Color.Black);
        public GameColor ForegroundColor { get; set; } = new GameColor(Color.White);

        public static ConsoleRepresentation EmptyTile => new ConsoleRepresentation
        {
            BackgroundColor = new GameColor(Color.Black),
            ForegroundColor = new GameColor(Color.Black),
            Character = ' '
        };
    }
}
