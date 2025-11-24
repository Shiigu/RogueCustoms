using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    public partial class CurrencyTab : UserControl
    {
        private DungeonInfo ActiveDungeon;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CurrencyInfo LoadedCurrency { get; private set; }
        public event EventHandler TabInfoChanged;
        public CurrencyTab()
        {
            InitializeComponent();
        }

        public void LoadData(DungeonInfo activeDungeon)
        {
            ActiveDungeon = activeDungeon;
            LoadedCurrency = activeDungeon.CurrencyInfo;

            txtCurrencyName.Text = LoadedCurrency.Name;
            txtCurrencyDescription.Text = LoadedCurrency.Description;

            try
            {
                crsCurrency.Character = LoadedCurrency.ConsoleRepresentation.Character;
                crsCurrency.BackgroundColor = LoadedCurrency.ConsoleRepresentation.BackgroundColor;
                crsCurrency.ForegroundColor = LoadedCurrency.ConsoleRepresentation.ForegroundColor;
            }
            catch
            {
                crsCurrency.Character = '\0';
                crsCurrency.BackgroundColor = new GameColor(Color.Black);
                crsCurrency.ForegroundColor = new GameColor(Color.White);
            }

            dgvCurrencyPileTypes.Rows.Clear();

            foreach (var pileType in LoadedCurrency.CurrencyPiles)
            {
                dgvCurrencyPileTypes.Rows.Add(pileType.Id, pileType.Minimum, pileType.Maximum);
            }
        }

        public List<string> SaveData()
        {
            dgvCurrencyPileTypes.EndEdit();
            var validationErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(txtCurrencyName.Text))
                validationErrors.Add("Enter the Currency Name first.");
            if (string.IsNullOrWhiteSpace(txtCurrencyDescription.Text))
                validationErrors.Add("Enter the Currency Description first.");
            if (crsCurrency.Character == '\0')
                validationErrors.Add("The Currency does not have a Console Representation character.");

            foreach (DataGridViewRow row in dgvCurrencyPileTypes.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.Cells["Id"].Value == null || string.IsNullOrWhiteSpace(row.Cells["Id"].Value.ToString()))
                {
                    validationErrors.Add("The Currency has a Pile Type without an Id.");
                    break;
                }
                if (row.Cells["Minimum"].Value == null || string.IsNullOrWhiteSpace(row.Cells["Minimum"].Value.ToString()))
                {
                    validationErrors.Add("The Currency has a Pile Type without a Minimum.");
                    break;
                }
                if (!int.TryParse(row.Cells["Minimum"].Value.ToString(), out int minimum) || minimum <= 0)
                {
                    validationErrors.Add("The Currency has a Pile Type without a valid Minimum.");
                    break;
                }
                if (row.Cells["Maximum"].Value == null || string.IsNullOrWhiteSpace(row.Cells["Maximum"].Value.ToString()))
                {
                    validationErrors.Add("The Currency has a Pile Type without a Maximum.");
                    break;
                }
                if (!int.TryParse(row.Cells["Maximum"].Value.ToString(), out int maximum) || maximum <= 0)
                {
                    validationErrors.Add("The Currency has a Pile Type without a valid Maximum.");
                    break;
                }
                if (minimum > maximum)
                {
                    validationErrors.Add("The Currency has a Pile Type with a Minimum higher than its Maximum.");
                    break;
                }
            }

            if (validationErrors.Count == 0)
            {
                LoadedCurrency = new()
                {
                    Name = txtCurrencyName.Text,
                    Description = txtCurrencyDescription.Text,
                    ConsoleRepresentation = new()
                    {
                        Character = crsCurrency.Character,
                        BackgroundColor = crsCurrency.BackgroundColor,
                        ForegroundColor = crsCurrency.ForegroundColor
                    },

                    CurrencyPiles = []
                };

                foreach (DataGridViewRow row in dgvCurrencyPileTypes.Rows)
                {
                    if (row.IsNewRow) continue;
                    var id = row.Cells["Id"].Value.ToString();
                    var minimum = int.Parse(row.Cells["Minimum"].Value.ToString());
                    var maximum = int.Parse(row.Cells["Maximum"].Value.ToString());

                    LoadedCurrency.CurrencyPiles.Add(new()
                    {
                        Id = id,
                        Minimum = minimum,
                        Maximum = maximum
                    });
                }
            }

            return validationErrors;
        }

        private void txtCurrencyName_TextChanged(object sender, EventArgs e)
        {
            txtCurrencyName.ToggleEntryInLocaleWarning(ActiveDungeon, fklblCurrencyNameLocale);
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void txtCurrencyDescription_TextChanged(object sender, EventArgs e)
        {
            txtCurrencyDescription.ToggleEntryInLocaleWarning(ActiveDungeon, fklblCurrencyDescriptionLocale);
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void dgvCurrencyPileTypes_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox tb)
            {
                tb.KeyPress -= NumericTextBox_KeyPress;
                if (dgvCurrencyPileTypes.CurrentCell.OwningColumn.Name is "Minimum" or "Maximum")
                    tb.KeyPress += NumericTextBox_KeyPress;
            }
        }

        private void NumericTextBox_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
