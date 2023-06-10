using System;
using System.Collections.Generic;

namespace RogueCustomsGameEngine.Utils.Helpers
{
    public static class MathAlgorithms
    {

        public static IEnumerable<T> BresenhamLine<T>(T p1, T p2, Func<T, int> xFunc, Func<T, int> yFunc, Func<int, int, T> coordToTFunc, Func<T, bool> predicate)
        {
            var (x1, y1) = (xFunc(p1), yFunc(p1));
            var (x2, y2) = (xFunc(p2), yFunc(p2));
            return BresenhamLine(x1, y1, x2, y2, coordToTFunc, predicate);
        }

        private static IEnumerable<T> BresenhamLine<T>(int x0, int y0, int x1, int y1, Func<int, int, T> coordToTFunc, Func<T, bool> predicate)
        {
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                int t;
                t = x0; // swap x0 and y0
                x0 = y0;
                y0 = t;
                t = x1; // swap x1 and y1
                x1 = y1;
                y1 = t;
            }
            if (x0 > x1)
            {
                int t;
                t = x0; // swap x0 and x1
                x0 = x1;
                x1 = t;
                t = y0; // swap y0 and y1
                y0 = y1;
                y1 = t;
            }
            int dx = x1 - x0;
            int dy = Math.Abs(y1 - y0);
            int error = dx / 2;
            int ystep = (y0 < y1) ? 1 : -1;
            int y = y0;
            for (int x = x0; x <= x1; x++)
            {
                var tValue = coordToTFunc((steep ? y : x), (steep ? x : y));
                yield return tValue;
                if ((x != x0 || y != y0) && !predicate(tValue)) break;
                error -= dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
        }
    }
}
