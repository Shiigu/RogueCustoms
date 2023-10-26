using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public static IEnumerable<T> Raycast<T>(T p1, T p2, Func<T, int> xFunc, Func<T, int> yFunc, Func<int, int, T> coordToTFunc, Func<T, bool> predicate, Func<T, bool> hallwayPredicate, double sightRange, Func<T, T, double> distanceFunc)
        {
            var (x1, y1) = (xFunc(p1), yFunc(p1));
            var (ox1, oy1) = (x1, y1);
            var (x2, y2) = (xFunc(p2), yFunc(p2));

            double maxDistance = sightRange;
            double currentDistance = 0;

            // Calculate the direction vector of the ray.
            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);

            int sx = x1 < x2 ? 1 : -1;
            int sy = y1 < y2 ? 1 : -1;

            int err = dx - dy;

            bool hasReducedSight = false;

            while (currentDistance <= maxDistance)
            {
                var otValue = coordToTFunc(ox1, oy1);
                var tValue = coordToTFunc(x1, y1);

                if (hallwayPredicate(tValue) && !hasReducedSight)
                {
                    maxDistance = Math.Max(maxDistance / 6, Math.Sqrt(2));
                    hasReducedSight = true;
                }

                if (distanceFunc(tValue, p1) > sightRange)
                    break;

                if(hasReducedSight && hallwayPredicate(otValue) && x1 != ox1 && y1 != oy1)
                {
                    var tX1OY1 = coordToTFunc(x1, oy1);
                    var tOX1Y1 = coordToTFunc(ox1, y1);

                    if(!predicate(tX1OY1) || !predicate(tOX1Y1))
                        break;
                }

                yield return tValue;

                if (!predicate(tValue) || (x1 == x2 && y1 == y2))
                    break;

                int e2 = 2 * err;

                (ox1, oy1) = (x1, y1);

                if (e2 > -dy)
                {
                    err -= dy;
                    x1 += sx;
                }

                //if (x1 == x2 && y1 == y2)
                //{
                //    var finalTValue = coordToTFunc(x1, y1);
                //    yield return finalTValue;
                //    break;
                //}

                if (e2 < dx)
                {
                    err += dx;
                    y1 += sy;
                }

                currentDistance = distanceFunc(coordToTFunc(x1, y1), p1);
            }
        }

    }
}
