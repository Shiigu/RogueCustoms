using RogueCustomsDungeonEditor.HelperForms;
using RogueCustomsDungeonEditor.Utils;
using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.Controls
{
#pragma warning disable CA1416 // Validar la compatibilidad de la plataforma
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

            // Check if we are in design mode
            if (DesignMode || this.DesignMode || IsControlInDesignMode(this))
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
                        lblConsoleRepresentation.Font = new Font(loadedFont, 24f, FontStyle.Regular);
                    }
                }
            }
            catch
            {
                // Do nothing if the Font can't be found
            }
        }

        public void SetConsoleRepresentation(ConsoleRepresentation consoleRepresentation)
        {
            Character = consoleRepresentation.Character;
            ForegroundColor = consoleRepresentation.ForegroundColor;
            BackgroundColor = consoleRepresentation.BackgroundColor;
        }

        private void btnChangeConsoleCharacter_Click(object sender, EventArgs e)
        {
            var characterMapResult = CharacterMapInputBox.ShowDialog(!string.IsNullOrWhiteSpace(lblConsoleRepresentation.Text) ? lblConsoleRepresentation.Text[0] : '\0');
            if (characterMapResult.Saved)
            {
                Character = characterMapResult.CharacterToSave ?? ' ';
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


        private bool IsControlInDesignMode(Control control)
        {
            if (control == null)
                return false;

            // Traverse up the control hierarchy to check if any parent is in design mode
            while (control != null)
            {
                if (control.Site != null && control.Site.DesignMode)
                {
                    return true;
                }
                control = control.Parent;
            }
            return false;
        }
    }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning restore CA1416 // Validar la compatibilidad de la plataforma
}
