using RoguelikeGameEngine.Utils.Helpers;

namespace RoguelikeGameEngine.Utils.Representation
{
    public class Point
    {
        public Point() { }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not Point other)
                return base.Equals(obj);
            else
                return X == other.X && Y == other.Y;
        }

        public override string ToString() => $"({X}, {Y})";

        public static double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(ArrayHelpers.GetSquaredEuclideanDistanceBetweenCells(p1.X, p1.Y, p2.X, p2.Y));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
