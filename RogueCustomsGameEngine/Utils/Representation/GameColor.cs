using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.Json.Serialization;

namespace RogueCustomsGameEngine.Utils.Representation
{
    public sealed class GameColor : IEquatable<GameColor?>
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

        public override string ToString()
        {
            return $"{R},{G},{B},{A}";
        }

        public override bool Equals(object? obj)
        {
            if(obj is GameColor gc)
                return R == gc.R && G == gc.G && B == gc.B && A == gc.A;
            return false;
        }

        public bool Equals(GameColor? other)
        {
            return other is not null &&
                   R == other.R &&
                   G == other.G &&
                   B == other.B &&
                   A == other.A;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(R, G, B, A);
        }

        public static bool operator ==(GameColor? left, GameColor? right)
        {
            return EqualityComparer<GameColor>.Default.Equals(left, right);
        }

        public static bool operator !=(GameColor? left, GameColor? right)
        {
            return !(left == right);
        }
    }
}
