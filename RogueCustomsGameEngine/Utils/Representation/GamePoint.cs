using RogueCustomsGameEngine.Utils.Helpers;
using System;

namespace RogueCustomsGameEngine.Utils.Representation
{
    [Serializable]
    public class GamePoint
    {
        public GamePoint() { }
        public GamePoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not GamePoint other)
                return base.Equals(obj);
            else
                return X == other.X && Y == other.Y;
        }

        public override string ToString() => $"({X}, {Y})";

        public static double Distance(GamePoint p1, GamePoint p2)
        {
            return Math.Sqrt(ArrayHelpers.GetSquaredEuclideanDistanceBetweenCells(p1.X, p1.Y, p2.X, p2.Y));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
