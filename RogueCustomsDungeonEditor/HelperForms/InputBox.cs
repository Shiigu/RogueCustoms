using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Utils;

namespace RogueCustomsDungeonEditor.HelperForms
{
    #pragma warning disable IDE1006 // Estilos de nombres
    public partial class InputBox : Form
    {
        private bool _rejectReservedWords;
        private List<string> _inputsToReject;
        public string PromptText => txtPromptText.Text;

        private InputBox()
        {
            InitializeComponent();
        }

        public static string? Show(string prompt, string title, string defaultText = "", bool rejectReservedWords = false, List<string> inputsToReject = null)
        {
            using var inputBox = new InputBox();
            inputBox.Text = title;
            inputBox.lblPrompt.Text = prompt;
            inputBox.txtPromptText.Text = defaultText;
            inputBox._rejectReservedWords = rejectReservedWords;
            inputBox._inputsToReject = inputsToReject ?? [];

            if (inputBox.ShowDialog() == DialogResult.OK)
            {
                return inputBox.PromptText;
            }

            return null;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if(EditorConstants.ReservedWords.Any(rw => txtPromptText.Text.Equals(rw, StringComparison.InvariantCultureIgnoreCase) || EditorConstants.PartialReservedWords.Any(rw => txtPromptText.Text.Contains(rw, StringComparison.InvariantCultureIgnoreCase)) && _rejectReservedWords))
            {
                MessageBox.Show("The input is invalid, for it contains a reserved word.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_inputsToReject.Any(itr => txtPromptText.Text.Equals(itr, StringComparison.InvariantCultureIgnoreCase)))
            {
                MessageBox.Show("The input is invalid, for it contains a unique value that already exists in another element.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
    #pragma warning restore IDE1006 // Estilos de nombres
}
