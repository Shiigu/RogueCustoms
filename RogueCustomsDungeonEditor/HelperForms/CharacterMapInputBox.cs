using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.HelperForms
{
    #pragma warning disable CA1416 // Validar la compatibilidad de la plataforma
    public partial class CharacterMapInputBox : Form
    {
        public char? CharacterToSave { get; private set; }
        public bool Saved { get; private set; }
        private TableLayoutPanelCellPosition SelectedCell;
        private List<char> CharMapList;

        public CharacterMapInputBox(List<char> charMapList, char? defaultSelection = null)
        {
            CharMapList = charMapList ?? new List<char>();
            CharacterToSave = defaultSelection;
            InitializeComponent();
            InitializeCharacterMapTable();
        }

        private void InitializeCharacterMapTable()
        {
            int totalCharacters = CharMapList.Count;
            int numColumns = tlpCharacters.ColumnCount;
            int numRows = (int)Math.Ceiling((double)totalCharacters / numColumns);

            tlpCharacters.RowCount = numRows;

            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numColumns; col++)
                {
                    int charValue = (row * numColumns) + col;

                    if (charValue >= totalCharacters) break;

                    var characterLabel = new Label
                    {
                        Text = CharMapList[charValue].ToString(),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Fill,
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = Color.White,
                        Font = new Font("Courier New", 11),
                        Margin = new Padding(0, 0, 0, 0)
                    };

                    characterLabel.Click += CharacterLabel_Click;
                    tlpCharacters.Controls.Add(characterLabel, col, row);
                    tlpCharacters.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30));
                    if (CharacterToSave != null && CharacterToSave == CharMapList[charValue])
                        CharacterLabel_Click(characterLabel, EventArgs.Empty);
                }
                tlpCharacters.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            }
        }
        private void CharacterLabel_Click(object? sender, EventArgs e)
        {
            var previousLabel = tlpCharacters.GetControlFromPosition(SelectedCell.Column, SelectedCell.Row) as Label;
            previousLabel.BackColor = Color.White;
            var clickedLabel = sender as Label;
            SelectedCell = tlpCharacters.GetPositionFromControl(clickedLabel);
            CharacterToSave = clickedLabel.Text[0];
            clickedLabel.BackColor = SystemColors.Highlight;
            btnSave.Enabled = CharacterToSave != null;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Saved = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
    #pragma warning restore CA1416 // Validar la compatibilidad de la plataforma
}
