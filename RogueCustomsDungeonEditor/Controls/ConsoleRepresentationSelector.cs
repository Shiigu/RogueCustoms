using RogueCustomsDungeonEditor.HelperForms;
using RogueCustomsDungeonEditor.Utils;
using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.Controls
{
    public partial class ConsoleRepresentationSelector : UserControl
    {
        private ConsoleRepresentation consoleRepresentation = new();

        public ConsoleRepresentation ConsoleRepresentation => new()
        {
            Character = consoleRepresentation.Character,
            ForegroundColor = consoleRepresentation.ForegroundColor,
            BackgroundColor = consoleRepresentation.BackgroundColor
        };

        public event EventHandler<PropertyChangedEventArgs> PropertyChanged;

        public char Character
        {
            get { return consoleRepresentation.Character; }
            set
            {
                if (consoleRepresentation.Character != value)
                {
                    consoleRepresentation.Character = value;
                    OnPropertyChanged(nameof(Character));
                }
            }
        }

        public GameColor ForegroundColor
        {
            get { return consoleRepresentation.ForegroundColor; }
            set
            {
                if (consoleRepresentation.ForegroundColor != value)
                {
                    consoleRepresentation.ForegroundColor = value;
                    OnPropertyChanged(nameof(ForegroundColor));
                }
            }
        }

        public GameColor BackgroundColor
        {
            get { return consoleRepresentation.BackgroundColor; }
            set
            {
                if (consoleRepresentation.BackgroundColor != value)
                {
                    consoleRepresentation.BackgroundColor = value;
                    OnPropertyChanged(nameof(BackgroundColor));
                }
            }
        }

        private void UpdateLabel()
        {
            lblConsoleRepresentation.Text = consoleRepresentation.Character.ToString();
            lblConsoleRepresentation.ForeColor = consoleRepresentation.ForegroundColor.ToColor();
            lblConsoleRepresentation.BackColor = consoleRepresentation.BackgroundColor.ToColor();
        }

        public ConsoleRepresentationSelector()
        {
            InitializeComponent();
        }

        public void SetConsoleRepresentation(ConsoleRepresentation consoleRepresentation)
        {
            Character = consoleRepresentation.Character;
            ForegroundColor = consoleRepresentation.ForegroundColor;
            BackgroundColor = consoleRepresentation.BackgroundColor;
        }

        private void btnChangeConsoleCharacter_Click(object sender, EventArgs e)
        {
            var characterMapForm = new CharacterMapInputBox(CharHelpers.GetIBM437PrintableCharacters(), (!string.IsNullOrWhiteSpace(lblConsoleRepresentation.Text)) ? lblConsoleRepresentation.Text[0] : '\0');
            characterMapForm.ShowDialog();
            if (characterMapForm.Saved)
            {
                Character = characterMapForm.CharacterToSave ?? ' ';
            }
        }

        private void btnChangeConsoleCharacterForeColor_Click(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog
            {
                Color = lblConsoleRepresentation.ForeColor
            };
            colorDialog.CustomColors = new int[] { ColorTranslator.ToOle(colorDialog.Color) };
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                ForegroundColor = new GameColor(colorDialog.Color);
            }
        }

        private void btnChangeConsoleCharacterBackColor_Click(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog
            {
                Color = lblConsoleRepresentation.BackColor
            };
            colorDialog.CustomColors = new int[] { ColorTranslator.ToOle(colorDialog.Color) };
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                BackgroundColor = new GameColor(colorDialog.Color);
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            UpdateLabel();
        }
    }
}
