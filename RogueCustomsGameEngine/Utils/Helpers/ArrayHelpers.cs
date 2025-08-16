using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsGameEngine.Utils.Helpers
{
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
#pragma warning disable S2368 // Public methods should not have multidimensional array parameters

    public static class ArrayHelpers
    {
        private sealed class AStarNodeData<T> where T : class
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
        public static double GetChebyshevDistanceBetweenCells(int x1, int y1, int x2, int y2) => Math.Max(Math.Abs(x2 - x1), Math.Abs(y2 - y1));
        
        public static List<T> GetShortestPathBetween<T>(this T[,] grid, (int X, int Y) source, (int X, int Y) target, bool includeDiagonals, Func<T, int> XDataFunction, Func<T, int> YDataFunction, Func<int, int, int, int, double> GFunction, Func<int, int, int, int, double> HFunction, Func<T, bool> validCellPredicate) where T : class
        {
            if (!validCellPredicate(grid[source.Y, source.X])) throw new ArgumentException("Cannot find a path from an invalid node!");
            if (!validCellPredicate(grid[target.Y, target.X])) throw new ArgumentException("Cannot find a path towards an invalid node!");

            var sourceT = grid[source.Y, source.X];
            var targetT = grid[target.Y, target.X];

            if (sourceT == targetT) return new List<T> { sourceT };


            var gridIslands = grid.GetIslands(validCellPredicate);
            if (!gridIslands.Any(i => i.Contains(sourceT) && i.Contains(targetT)))
            {
                // If the Target Tile is inaccessible, stop searching.
                return new List<T> { sourceT };
            }

            var openList = new Dictionary<T, AStarNodeData<T>>();
            var closedList = new Dictionary<T, AStarNodeData<T>>();

            openList[sourceT] = grid.GetAStarNodeData(source.Y, source.X, sourceT, targetT, null, XDataFunction, YDataFunction, GFunction, HFunction);

            while (openList.Count > 0)
            {
                var currentCell = openList.Values.MinBy(nodeData => nodeData.F);
                var currentNode = currentCell.Node;

                openList.Remove(currentNode);
                closedList[currentNode] = currentCell;

                if (currentNode.Equals(targetT)) break;

                var adjacentValidCells = grid.GetAdjacentElementsWhere(grid.IndexOf(currentNode).i, grid.IndexOf(currentNode).j, includeDiagonals, validCellPredicate);

                foreach (var adjacentCell in adjacentValidCells)
                {
                    if (closedList.ContainsKey(adjacentCell)) continue;

                    var (i, j) = grid.IndexOf(adjacentCell);
                    var existingAStarNodeData = openList.GetValueOrDefault(adjacentCell);
                    var newAStarNodeData = grid.GetAStarNodeData(i, j, sourceT, targetT, currentCell, XDataFunction, YDataFunction, GFunction, HFunction);

                    if (newAStarNodeData == null || newAStarNodeData.Node == null) continue;

                    if (existingAStarNodeData == null || newAStarNodeData.G < existingAStarNodeData.G)
                    {
                        openList[adjacentCell] = newAStarNodeData;
                    }
                }
            }

            var path = new Stack<T>();
            AStarNodeData<T> cellInPath;

            if (closedList.ContainsKey(targetT))
            {
                cellInPath = closedList[targetT];
            }
            else
            {
                var minClosedF = closedList.Values.Min(nodeData => nodeData.F);
                cellInPath = closedList.Values.First(nodeData => nodeData.F == minClosedF);
            }

            while (cellInPath?.Node != null)
            {
                path.Push(cellInPath.Node);
                cellInPath = cellInPath.PrecedingNode;
            }

            return path.ToList();
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
        public static List<T> GetElementsWithinDistanceWhere<T>(this T[,] grid, int i, int j, int maxDistance, bool includeDiagonals, Func<T, bool> predicate)
        {
            var nearbyElements = new List<T>();
            var directions = includeDiagonals ? new List<(int, int)>
            {
                (-1, -1), (-1, 0), (-1, 1),
                (0, -1) ,          (0, 1),
                (1, -1) , (1, 0) , (1, 1)
            } : new List<(int, int)>
            {
                          (-1, 0),
                (0, -1) ,          (0, 1),
                          (1, 0)
            };

            bool IsValidTile(int x, int y)
            {
                return x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1) && predicate(grid[x, y]);
            }

            Queue<(int, int, int)> queue = new Queue<(int, int, int)>();
            HashSet<(int, int)> visited = new HashSet<(int, int)>();

            queue.Enqueue((i, j, 0));
            visited.Add((i, j));

            while(queue.Count > 0)
            {
                (int x, int y, int distance) = queue.Dequeue();

                if(distance <= maxDistance)
                {
                    if (x != i || y != j)
                        nearbyElements.Add(grid[x, y]);
                    foreach ((int dx, int dy) in directions)
                    {
                        var newX = x + dx;
                        var newY = y + dy;

                        if ((dx == 0 || dy == 0 || (IsValidTile(newX, y) && IsValidTile(x, newY))) && IsValidTile(newX, newY) && !visited.Contains((newX, newY)))
                        {
                            visited.Add((newX, newY));
                            queue.Enqueue((newX, newY, distance + 1));
                        }
                    }
                }
            }

            return nearbyElements;
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

        public static List<List<T>> GetIslands<T>(this T[,] grid, Func<T, bool> predicate)
        {
            var visited = new bool[grid.GetLength(0), grid.GetLength(1)];
            var islands = new List<List<T>>();

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (predicate(grid[i, j]) && !visited[i, j])
                    {
                        var currentIsland = new List<T>();
                        DepthFirstSearchForIsland(grid, visited, predicate, i, j, currentIsland);
                        islands.Add(currentIsland);
                    }
                }
            }

            return islands;
        }

        public static void DepthFirstSearchForIsland<T>(this T[,] grid, bool[,] visited, Func<T, bool> predicate, int i, int j, List<T> currentIsland)
        {
            if (i < 0 || i >= grid.GetLength(0)) return;
            if (j < 0 || j >= grid.GetLength(1)) return;
            if (!predicate(grid[i, j]) || visited[i, j]) return;

            visited[i, j] = true;
            currentIsland.Add(grid[i, j]);

            DepthFirstSearchForIsland(grid, visited, predicate, i + 1, j, currentIsland);
            DepthFirstSearchForIsland(grid, visited, predicate, i - 1, j, currentIsland);
            DepthFirstSearchForIsland(grid, visited, predicate, i, j + 1, currentIsland);
            DepthFirstSearchForIsland(grid, visited, predicate, i, j - 1, currentIsland);
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
            {
                for (int j = 0; j < array.GetLength(1); j++)
                    if (array[i, j] != null)
                        action(array[i, j]);
            }
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
            return array.ToList().Count;
        }
        public static int Count<T>(this T[,] array, Func<T, bool> predicate)
        {
            return array.Count(predicate);
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

        public static T[,] Copy<T>(this T[,] original)
        {
            int rows = original.GetLength(0);
            int cols = original.GetLength(1);

            // Create a new array of the same size
            T[,] copy = new T[rows, cols];

            // Copy each element from 'original' to 'copy'
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    copy[i, j] = original[i, j];
                }
            }

            return copy;
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
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
#pragma warning restore S2368 // Public methods should not have multidimensional array parameters
}
