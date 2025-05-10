using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using RogueCustomsDungeonEditor.Utils;

namespace RogueCustomsDungeonEditor.HelperForms
{
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
#pragma warning disable CA1416 // Validar la compatibilidad de la plataforma
    public partial class CharacterMapInputBox : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public char? CharacterToSave { get; private set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Saved { get; private set; }
        private TableLayoutPanelCellPosition SelectedCell;
        private List<char> CharMapList;
        private Font FontToUse;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static CharacterMapInputBox Instance { get; private set; }

        public static void ConstructCharacterMap()
        {
            Instance = new CharacterMapInputBox(null);
            Instance.Saved = false;
            Instance.CharacterToSave = null;
        }

        public static (bool Saved, char? CharacterToSave) ShowDialog(char? defaultSelection)
        {
            Instance ??= new CharacterMapInputBox(defaultSelection);
            Instance.Saved = false;
            Instance.CharacterToSave = null;
            Instance.ShowDialog();
            return (Instance.Saved, Instance.CharacterToSave);
        }

        private CharacterMapInputBox(char? defaultSelection = null)
        {
            // Check if we are in design mode
            if (DesignMode || this.DesignMode)
            {
                return; // Do nothing in design mode
            }

            try
            {
                var fontPath = Path.Combine(Application.StartupPath, "Resources\\PxPlus_Tandy1K-II_200L.ttf");
                var fontName = "PxPlus Tandy1K-II 200L";
                if (FontHelpers.LoadFont(fontPath))
                {
                    var loadedFont = FontHelpers.GetFontByName(fontName);
                    if (loadedFont != null)
                    {
                        FontToUse = new Font(loadedFont, 8f, FontStyle.Regular);
                    }
                }
            }
            catch
            {
                // Do nothing if the Font can't be found
            }
            CharMapList = FontHelpers.GetPrintableCharactersFromFont(FontToUse.FontFamily);
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
                        UseMnemonic = false,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Fill,
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = Color.White,
                        Font = FontToUse,
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
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
}
