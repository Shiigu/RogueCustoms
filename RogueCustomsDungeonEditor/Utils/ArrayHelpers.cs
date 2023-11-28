using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
