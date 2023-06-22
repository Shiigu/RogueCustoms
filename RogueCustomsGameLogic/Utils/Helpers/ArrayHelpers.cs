using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Utils.Helpers
{
    public static class ArrayHelpers
    {
        private class AStarNodeData<T> where T : class
        {
            public T Node;
            public AStarNodeData<T> PrecedingNode;
            public double G;
            public double H;
            public double F;

            public override string ToString() => $"Node: {Node}, PrecedingNode: {PrecedingNode.Node}, G: {G}, H: {H}, F: {F}";
        }

        public static double GetSquaredEuclideanDistanceBetweenCells(int x1, int y1, int x2, int y2) => Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2);
        public static double GetManhattanDistanceBetweenCells(int x1, int y1, int x2, int y2) => Math.Abs(x2 - x1) + Math.Abs(y2 - y1);

        public static List<T> GetShortestPathBetween<T>(this T[,] grid, (int X, int Y) source, (int X, int Y) target, bool includeDiagonals, Func<T, int> XDataFunction, Func<T, int> YDataFunction, Func<int, int, int, int, double> GFunction, Func<int, int, int, int, double> HFunction, Func<T, bool> validCellPredicate) where T : class
        {
            if (!validCellPredicate(grid[source.Y, source.X])) throw new Exception("Cannot find a path from an invalid node!");
            if (!validCellPredicate(grid[target.Y, target.X])) throw new Exception("Cannot find a path towards an invalid node!");
            if (source.X == target.X && source.Y == target.Y) return new List<T> { grid[target.Y, target.X] };
            var sourceT = grid[source.Y, source.X];
            var targetT = grid[target.Y, target.X];
            var openList = new List<AStarNodeData<T>>();
            var closedList = new List<AStarNodeData<T>>();
            openList.Add(grid.GetAStarNodeData(source.Y, source.X, sourceT, targetT, null, XDataFunction, YDataFunction, GFunction, HFunction));
            while (openList.Any(n => n != null))
            {
                var minOpenF = openList.Where(n => n != null).Min(n => n.F);
                var currentCell = openList.First(n => n != null && n.F == minOpenF);
                var indexOfCell = grid.IndexOf(currentCell.Node);
                var adjacentValidCells = grid.GetAdjacentElementsWhere(indexOfCell.i, indexOfCell.j, includeDiagonals, validCellPredicate);
                Parallel.ForEach(adjacentValidCells, c => {
                    if (closedList.Any(l => l.Node.Equals(c))) return;
                    var (i, j) = grid.IndexOf(c);
                    var existingAStarNodeData = openList.Find(l => l?.Node.Equals(c) == true);
                    var newAStarNodeData = grid.GetAStarNodeData(i, j, sourceT, targetT, currentCell, XDataFunction, YDataFunction, GFunction, HFunction);
                    if (newAStarNodeData == null || newAStarNodeData.Node == null) return;
                    if (existingAStarNodeData == null)
                    {
                        openList.Add(newAStarNodeData);
                    }
                    else
                    {
                        if(newAStarNodeData.G < existingAStarNodeData.G)
                            existingAStarNodeData = newAStarNodeData;
                    }
                });
                openList.Remove(currentCell);
                closedList.Add(currentCell);
                if (currentCell.Node.Equals(targetT)) break;
            }
            var path = new List<T>();
            var minClosedF = closedList.Min(l => l.F);
            var cellInPath = (closedList.Any(l => l.Node.Equals(targetT))) ? closedList.Last() : closedList.First(l => l.F == minClosedF);
            while(cellInPath?.Node != null)
            {
                path.Add(cellInPath.Node);
                cellInPath = cellInPath.PrecedingNode;
            }
            path.Reverse();
            return path;
        }

        private static AStarNodeData<T> GetAStarNodeData<T>(this T[,] grid, int X, int Y, T source, T target, AStarNodeData<T> precedingNode, Func<T, int> XDataFunction, Func<T, int> YDataFunction, Func<int, int, int, int, double> GFunction, Func<int, int, int, int, double> HFunction) where T : class
        {
            var nodeData = new AStarNodeData<T>()
            {
                Node = grid[X, Y],
                PrecedingNode = precedingNode
            };
            nodeData.G = GFunction(XDataFunction(source), YDataFunction(source), XDataFunction(nodeData.Node), YDataFunction(nodeData.Node));
            nodeData.H = HFunction(XDataFunction(nodeData.Node), YDataFunction(nodeData.Node), XDataFunction(target), YDataFunction(target));
            nodeData.F = nodeData.G + nodeData.H;
            return nodeData;
        }

        private static (int i, int j) IndexOf<T>(this T[,] grid, T element)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    if (grid[x, y].Equals(element))
                        return (x, y);
                }
            }

            return (-1, -1);
        }

        public static List<T> GetAdjacentElements<T>(this T[,] grid, T element, bool includeDiagonals)
        {
            return grid.GetAdjacentElementsWhere(element, includeDiagonals, _ => true);
        }

        public static List<T> GetAdjacentElementsWhere<T>(this T[,] grid, int i, int j, bool includeDiagonals, Func<T, bool> predicate)
        {
            return grid.GetElementsWithinDistanceWhere(i, j, 1, includeDiagonals, predicate);
        }

        public static List<T> GetAdjacentElementsWhere<T>(this T[,] grid, T element, bool includeDiagonals, Func<T, bool> predicate)
        {
            var indexOfElement = grid.IndexOf(element);
            return grid.GetElementsWithinDistanceWhere(indexOfElement.i, indexOfElement.j, 1, includeDiagonals, predicate);
        }

        public static List<T> GetElementsWithinDistance<T>(this T[,] grid, int i, int j, int distance, bool includeDiagonals)
        {
            return grid.GetElementsWithinDistanceWhere(i, j, distance, includeDiagonals, _ => true);
        }
        public static List<T> GetElementsWithinDistanceWhere<T>(this T[,] grid, int i, int j, int distance, bool includeDiagonals, Func<T, bool> predicate)
        {
            var nearbyElements = new List<T>();
            var visitedDistances = new int[grid.GetLength(0), grid.GetLength(1)];

            for (int x = 0; x < visitedDistances.GetLength(0); x++)
            {
                for (int y = 0; y < visitedDistances.GetLength(1); y++)
                {
                    visitedDistances[x, y] = -1;
                }
            }

            if (distance > 0)
            {
                SetDistancesFromCenter(grid, visitedDistances, distance, includeDiagonals, i, j, predicate);
            }

            for (int x = 0; x < visitedDistances.GetLength(0); x++)
            {
                for (int y = 0; y < visitedDistances.GetLength(1); y++)
                {
                    if ((x != i || y != j) && visitedDistances[x, y] >= 0)
                    {
                        nearbyElements.Add(grid[x, y]);
                    }
                }
            }

            return nearbyElements;
        }

        private static void SetDistancesFromCenter<T>(T[,] grid, int[,] visitedDistances, int stepsLeft, bool includeDiagonals, int i, int j, Func<T, bool> predicate)
        {
            if (i < 0 || i >= grid.GetLength(0)) return;
            if (j < 0 || j >= grid.GetLength(1)) return;
            if (visitedDistances[i, j] > stepsLeft) return;
            if (!predicate(grid[i, j])) return;
            visitedDistances[i, j] = stepsLeft;
            if (stepsLeft < 0) return;
            SetDistancesFromCenter(grid, visitedDistances, stepsLeft - 1, includeDiagonals, i + 1, j, predicate);
            SetDistancesFromCenter(grid, visitedDistances, stepsLeft - 1, includeDiagonals, i - 1, j, predicate);
            SetDistancesFromCenter(grid, visitedDistances, stepsLeft - 1, includeDiagonals, i, j + 1, predicate);
            SetDistancesFromCenter(grid, visitedDistances, stepsLeft - 1, includeDiagonals, i, j - 1, predicate);
            if (includeDiagonals)
            {
                if (i > 0 && j > 0 && visitedDistances[i - 1, j] >= 0 && visitedDistances[i, j - 1] >= 0)
                    SetDistancesFromCenter(grid, visitedDistances, stepsLeft - 1, includeDiagonals, i - 1, j - 1, predicate);
                if (i > 0 && j < (grid.GetLength(1) - 1) && visitedDistances[i - 1, j] >= 0 && visitedDistances[i, j + 1] >= 0)
                    SetDistancesFromCenter(grid, visitedDistances, stepsLeft - 1, includeDiagonals, i - 1, j + 1, predicate);
                if (i < (grid.GetLength(0) - 1) && j > 0 && visitedDistances[i + 1, j] >= 0 && visitedDistances[i, j - 1] >= 0)
                    SetDistancesFromCenter(grid, visitedDistances, stepsLeft - 1, includeDiagonals, i + 1, j - 1, predicate);
                if (i < (grid.GetLength(0) - 1) && j < (grid.GetLength(1) - 1) && visitedDistances[i + 1, j] >= 0 && visitedDistances[i, j + 1] >= 0)
                    SetDistancesFromCenter(grid, visitedDistances, stepsLeft - 1, includeDiagonals, i + 1, j + 1, predicate);
            }
        }

        public static bool IsFullyConnected<T>(this T[,] grid, Func<T, bool> predicate)
        {
            return grid.GetNumberOfIslands(predicate) == 1;
        }

        public static int GetNumberOfIslands<T>(this T[,] grid, Func<T, bool> predicate)
        {
            var visited = new bool[grid.GetLength(0), grid.GetLength(1)];
            var islandCount = 0;
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (predicate(grid[i, j]) && !visited[i, j])
                    {
                        DepthFirstSearch(grid, visited, predicate, i, j);
                        islandCount++;
                    }
                }
            }
            return islandCount;
        }

        public static void DepthFirstSearch<T>(this T[,] grid, bool[,] visited, Func<T, bool> predicate, int i, int j)
        {
            if (i < 0 || i >= grid.GetLength(0)) return;
            if (j < 0 || j >= grid.GetLength(1)) return;
            if (!predicate(grid[i, j]) || visited[i, j]) return;
            visited[i, j] = true;
            DepthFirstSearch(grid, visited, predicate, i + 1, j);
            DepthFirstSearch(grid, visited, predicate, i - 1, j);
            DepthFirstSearch(grid, visited, predicate, i, j + 1);
            DepthFirstSearch(grid, visited, predicate, i, j - 1);
        }

        public static bool IsFullyConnectedAdjacencyMatrix<T>(this T[,] grid, Func<T, bool> predicate)
        {
            return grid.Count() == 1 || grid.GetNumberOfIslandsAdjacencyMatrix(predicate) == 1;
        }

        public static int GetNumberOfIslandsAdjacencyMatrix<T>(this T[,] grid, Func<T, bool> predicate)
        {
            var visited = new bool[grid.GetLength(0)];
            var islandCount = 0;
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (predicate(grid[i, j]) && !visited[i])
                    {
                        DepthFirstSearchAdjacencyMatrix(grid, visited, predicate, i);
                        islandCount++;
                    }
                }
            }
            return islandCount;
        }

        public static void DepthFirstSearchAdjacencyMatrix<T>(this T[,] grid, bool[] visited, Func<T, bool> predicate, int i)
        {
            if (i < 0 || i >= grid.GetLength(0)) return;
            if (visited[i]) return;
            visited[i] = true;
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (predicate(grid[i, j]) && !visited[j])
                {
                    DepthFirstSearchAdjacencyMatrix(grid, visited, predicate, j);
                }
            }
        }

        public static List<T> ToList<T>(this T[,] array)
        {
            var itemList = new List<T>();
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                    itemList.Add(array[i, j]);
            }
            return itemList;
        }

        public static void ForEach<T>(this T[,] array, Action<T> action)
        {
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                    if (array[i, j] != null)
                        action(array[i, j]);
        }
        public static List<T> Where<T>(this T[,] array, Func<T, bool> predicate)
        {
            var trueItems = new List<T>();
            array.ForEach(i =>
            {
                if(i != null && predicate(i)) trueItems.Add(i);
            });
            return trueItems;
        }
        public static int Count<T>(this T[,] array)
        {
            return array.ToList().Count();
        }
        public static int Count<T>(this T[,] array, Func<T, bool> predicate)
        {
            return array.Where(predicate).Count();
        }

        public static T? Find<T>(this T[,] array, Func<T, bool> predicate)
        {
            return array.Where(predicate).FirstOrDefault();
        }

        public static bool Any<T>(this T[,] array, Func<T, bool> predicate)
        {
            return array.Where(predicate).Any();
        }

        public static bool All<T>(this T[,] array, Func<T, bool> predicate)
        {
            foreach (var item in array)
            {
                if (item != null && !predicate(item)) return false;
            }
            return true;
        }

        public static string Print<T>(this T[,] array)
        {
            var arrayString = new StringBuilder();

            for (int i = 0; i < array.GetLength(0); i++)
            {
                arrayString.Append("[ ");
                for (int j = 0; j < array.GetLength(1); j++)
                    arrayString.Append(j < array.GetLength(1) - 1 ? $"{array[i, j]},\t" : $"{array[i, j]}");
                arrayString.AppendLine(" ]");
            }

            return arrayString.ToString();
        }
    }
}
