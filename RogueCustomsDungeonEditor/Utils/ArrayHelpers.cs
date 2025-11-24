using System;
using System.Collections.Generic;
using System.Linq;

using RogueCustomsGameEngine.Utils.Helpers;

namespace RogueCustomsDungeonEditor.Utils
{
    public static class ArrayHelpers
    {
        private static readonly Random random = new();

        public static T GetRandomElement<T>(this T[,] jaggedArray)
        {
            int rows = jaggedArray.GetLength(0);
            int cols = jaggedArray.GetLength(1);

            if (rows == 0 || cols == 0)
            {
                throw new InvalidOperationException("Jagged array must have at least one row and one column.");
            }

            int randomRow = random.Next(rows);
            int randomCol = random.Next(cols);

            return jaggedArray[randomRow, randomCol];
        }

        public static bool IsFullyConnectedAdjacencyMatrix<T>(this T[,] grid, Func<T, bool> predicate)
        {
            return grid.GetLength(0) == 1 && grid.GetLength(1) == 1 || grid.GetNumberOfIslandsAdjacencyMatrix(predicate) == 1;
        }

        public static int GetNumberOfIslandsAdjacencyMatrix<T>(this T[,] grid, Func<T, bool> predicate)
        {
            var rows = grid.GetLength(0);
            var cols = grid.GetLength(1);
            var visited = new bool[rows, cols];
            var islandCount = 0;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (predicate(grid[i, j]) && !visited[i, j])
                    {
                        DepthFirstSearchAdjacencyMatrix(grid, visited, predicate, i, j);
                        islandCount++;
                    }
                }
            }

            return islandCount;
        }

        public static void DepthFirstSearchAdjacencyMatrix<T>(this T[,] grid, bool[,] visited, Func<T, bool> predicate, int i, int j)
        {
            var rows = grid.GetLength(0);
            var cols = grid.GetLength(1);

            if (i < 0 || i >= rows || j < 0 || j >= cols || visited[i, j] || !predicate(grid[i, j]))
            {
                return;
            }

            visited[i, j] = true;

            // Explore neighbors
            DepthFirstSearchAdjacencyMatrix(grid, visited, predicate, i - 1, j); // Up
            DepthFirstSearchAdjacencyMatrix(grid, visited, predicate, i + 1, j); // Down
            DepthFirstSearchAdjacencyMatrix(grid, visited, predicate, i, j - 1); // Left
            DepthFirstSearchAdjacencyMatrix(grid, visited, predicate, i, j + 1); // Right
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
                if (i != null && predicate(i)) trueItems.Add(i);
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
    }
}
