using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using RogueCustomsDungeonEditor.Controls;
using RogueCustomsDungeonEditor.EffectInfos;

using RogueCustomsGameEngine.Utils.JsonImports;

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
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }

        public static void SetActionEditorParams(this SingleActionEditor sae, string classId, ActionWithEffectsInfo? action, List<EffectTypeData> effectParamData, DungeonInfo dungeon, EventHandler changeEvent)
        {
            sae.Action = action;
            sae.ClassId = classId;
            sae.Dungeon = dungeon;
            sae.EffectParamData = effectParamData;
            sae.ActionContentsChanged += changeEvent;
        }
        public static void SetActionEditorParams(this MultiActionEditor mae, string classId, List<ActionWithEffectsInfo> actions, List<EffectTypeData> effectParamData, DungeonInfo dungeon, EventHandler changeEvent)
        {
            mae.Actions = actions;
            mae.ClassId = classId;
            mae.Dungeon = dungeon;
            mae.EffectParamData = effectParamData;
            mae.ActionContentsChanged += changeEvent;
        }
    }
}
