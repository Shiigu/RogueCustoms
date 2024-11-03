using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.Utils
{
    public static class FormHelpers
    {
        public static void RemoveRow(this TableLayoutPanel table, int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= table.RowCount)
                throw new ArgumentOutOfRangeException(nameof(rowIndex), "Invalid row index.");

            for (int col = 0; col < table.ColumnCount; col++)
            {
                var control = table.GetControlFromPosition(col, rowIndex);
                if (control != null)
                {
                    table.Controls.Remove(control);
                    control.Dispose();
                }
            }

            for (int i = rowIndex + 1; i < table.RowCount; i++)
            {
                for (int j = 0; j < table.ColumnCount; j++)
                {
                    var control = table.GetControlFromPosition(j, i);
                    if (control != null)
                        table.SetRow(control, i - 1);
                }
            }

            if (table.RowStyles.Count > 0)
                table.RowStyles.RemoveAt(table.RowStyles.Count - 1);

            table.RowCount--;
        }
    }
}
