using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RogueCustomsDungeonValidator.Utils
{
    public static class RichTextBoxHelper
    {
        public static void AppendText(this RichTextBox box, string text, Color color, bool addNewLine = false, FontStyle fontStyle = FontStyle.Regular)
        {
            var oldFontStyle = box.SelectionFont.Style;
            box.SuspendLayout();
            box.SelectionFont = new Font(box.SelectionFont, fontStyle);
            box.SelectionColor = color;
            box.AppendText(addNewLine
                ? $"{text}{Environment.NewLine}"
                : text);
            box.ScrollToCaret();
            box.SelectionFont = new Font(box.SelectionFont, oldFontStyle);
            box.ResumeLayout();
        }
    }
}
