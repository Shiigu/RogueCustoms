using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.Json.Serialization;

using RogueCustomsGameEngine.Game.DungeonStructure;

namespace RogueCustomsGameEngine.Utils.Representation
{
    [Serializable]
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

        public GameColor AsDarkened()
        {
            return new GameColor
            {
                R = R,
                G = G,
                B = B,
                A = (byte) (A / 2)
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

        public static GameColor GetRandomContrastingColor(GameColor background, RngHandler Rng)
        {
            while (true)
            {
                var candidate = new GameColor(Color.FromArgb(255, Rng.Next(256), Rng.Next(256), Rng.Next(256)));

                if (HasSufficientContrast(background, candidate))
                    return candidate;
            }
        }

        private static bool HasSufficientContrast(GameColor c1, GameColor c2)
        {
            double b1 = GetBrightness(c1);
            double b2 = GetBrightness(c2);

            if (Math.Abs(b1 - b2) < 125)
                return false;

            double distance = Math.Sqrt(
                Math.Pow(c1.R - c2.R, 2) +
                Math.Pow(c1.G - c2.G, 2) +
                Math.Pow(c1.B - c2.B, 2));

            return distance > 100;
        }

        private static double GetBrightness(GameColor c)
        {
            return Math.Sqrt(
                c.R * c.R * 0.299 +
                c.G * c.G * 0.587 +
                c.B * c.B * 0.114);
        }
    }
}
