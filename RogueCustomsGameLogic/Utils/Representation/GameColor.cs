using System.Drawing;
using System.Text.Json.Serialization;

namespace RogueCustomsGameEngine.Utils.Representation
{
    public class GameColor
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }

        [JsonConstructor]
        public GameColor() { }

        public GameColor(Color color)
        {
            R = color.R;
            G = color.G;
            B = color.B;
            A = color.A;
        }

        public GameColor Clone()
        {
            return new GameColor
            {
                R = R,
                G = G,
                B = B,
                A = A
            };
        }
    }
}
