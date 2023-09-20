using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.HelperForms
{
    public partial class InputBox : Form
    {
        public string PromptText => txtPromptText.Text;

        private InputBox()
        {
            InitializeComponent();
        }

        public static string Show(string prompt, string title, string defaultText = "")
        {
            using var inputBox = new InputBox();
            inputBox.Text = title;
            inputBox.lblPrompt.Text = prompt;
            inputBox.txtPromptText.Text = defaultText;

            if (inputBox.ShowDialog() == DialogResult.OK)
            {
                return inputBox.PromptText;
            }

            return null;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
