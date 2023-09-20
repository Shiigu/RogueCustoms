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
    public partial class ComboInputBox : Form
    {
        public string SelectionText => cmbPrompt.Text;

        private ComboInputBox()
        {
            InitializeComponent();
        }

        public static string Show(string prompt, string title, List<string> options, string defaultOption = "")
        {
            using var comboInputBox = new ComboInputBox();
            comboInputBox.Text = title;
            comboInputBox.lblPrompt.Text = prompt;

            comboInputBox.cmbPrompt.Items.Clear();

            foreach (var option in options)
            {
                comboInputBox.cmbPrompt.Items.Add(option);
                if (!string.IsNullOrWhiteSpace(defaultOption) && defaultOption.Equals(option))
                    comboInputBox.cmbPrompt.Text = option;
            }

            if (comboInputBox.ShowDialog() == DialogResult.OK)
            {
                return comboInputBox.SelectionText;
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
