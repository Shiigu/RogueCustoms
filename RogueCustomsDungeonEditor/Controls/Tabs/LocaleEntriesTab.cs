using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.JsonImports;

#pragma warning disable CS8601 // Posible asignación de referencia nula
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.


namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    public partial class LocaleEntriesTab : UserControl
    {
        private DungeonInfo ActiveDungeon;
        private List<string> MandatoryLocaleKeys;
        private bool IsLoadingTab;
        public LocaleInfo LoadedLocale { get; private set; }
        public bool UnsavedLocaleEntryChanges { get; private set; }
        public event EventHandler TabInfoChanged;

        public LocaleEntriesTab()
        {
            InitializeComponent();
        }
        
        public void LoadData(DungeonInfo activeDungeon, LocaleInfo localeToLoad, List<string> mandatoryLocaleKeys)
        {
            ActiveDungeon = activeDungeon;
            UnsavedLocaleEntryChanges = false;
            IsLoadingTab = true;
            LoadedLocale = localeToLoad;
            MandatoryLocaleKeys = mandatoryLocaleKeys;
            dgvLocales.Rows.Clear();
            foreach (var entry in LoadedLocale.LocaleStrings)
            {
                dgvLocales.Rows.Add(entry.Key, entry.Value);
            }
            dgvLocales.Rows[0].Selected = true;
            UnsavedLocaleEntryChanges = false;
            IsLoadingTab = false;
        }

        public List<string> SaveData(string language)
        {
            if(UnsavedLocaleEntryChanges)
            {
                var unsavedKey = dgvLocales.SelectedRows[0].Cells[0].Value.ToString();
                var messageBoxResult = UnsavedLocaleEntryChanges ? MessageBox.Show(
                    $"Do you want to save your current Locale Entry changes before saving?",
                    "Locale",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                ) : DialogResult.Yes;

                if (messageBoxResult == DialogResult.Yes)
                {
                    SaveOrUpdateLocaleEntry();
                }
            }

            var validationErrors = new List<string>();
            var localeToSave = new LocaleInfo()
            {
                Language = language,
                LocaleStrings = new()
            };

            foreach (DataGridViewRow row in dgvLocales.Rows)
            {
                try
                {
                    var isValidLocale = true;
                    var key = row.Cells[0].Value.ToString();
                    var value = row.Cells[1].Value.ToString();
                    if (string.IsNullOrWhiteSpace(key))
                    {
                        isValidLocale = false;
                        validationErrors.Add("At least one Locale Entry lacks a Key.");
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            isValidLocale = false;
                            validationErrors.Add($"Locale Entry {key} lacks a Value.");
                        }
                    }
                    if (isValidLocale)
                    {
                        localeToSave.LocaleStrings.Add(new()
                        {
                            Key = key,
                            Value = value
                        });
                    }
                }
                catch
                {
                    validationErrors.Add("At least one Locale Entry is invalid.");
                }
            }

            if (!validationErrors.Any())
            {
                LoadedLocale = localeToSave;
            }

            return validationErrors.Distinct().ToList();
        }

        private void dgvLocales_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLocales.SelectedRows.Count == 0) return;
            var key = dgvLocales.SelectedRows[0].Cells[0].Value.ToString();
            var messageBoxResult = UnsavedLocaleEntryChanges ? MessageBox.Show(
                $"Do you want to save your current Locale Entry changes before moving to Entry {key}?",
                "Locale",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            ) : DialogResult.Yes;

            if (messageBoxResult == DialogResult.Yes && UnsavedLocaleEntryChanges)
            {
                SaveOrUpdateLocaleEntry();
            }

            var localeString = LoadedLocale.LocaleStrings.Find(ls => ls.Key.Equals(key));
            dgvLocales.CurrentCell = dgvLocales.SelectedRows[0].Cells[0];
            if (localeString != null)
            {
                SetLocaleStringForEdit(localeString);
            }
        }

        private void SaveOrUpdateLocaleEntry()
        {
            var localeStringToUpdate = LoadedLocale.LocaleStrings.Find(ls => ls.Key.Equals(txtLocaleEntryKey.Text));
            if (localeStringToUpdate != null)
            {
                localeStringToUpdate.Value = txtLocaleEntryValue.Text;
                dgvLocales.Rows[LoadedLocale.LocaleStrings.IndexOf(localeStringToUpdate)].SetValues(txtLocaleEntryKey.Text, txtLocaleEntryValue.Text);
            }
            else
            {
                LoadedLocale.LocaleStrings.Add(new LocaleInfoString
                {
                    Key = txtLocaleEntryKey.Text,
                    Value = txtLocaleEntryValue.Text.Replace(Environment.NewLine, "\n")
                });
                dgvLocales.Rows.Add(txtLocaleEntryKey.Text, txtLocaleEntryValue.Text);
            }
        }

        private void SetLocaleStringForEdit(LocaleInfoString localeString)
        {
            txtLocaleEntryKey.Text = localeString.Key;
            txtLocaleEntryKey.Enabled = true;
            txtLocaleEntryValue.Text = localeString.Value.Replace("\n", Environment.NewLine);
            txtLocaleEntryValue.Enabled = true;
            UnsavedLocaleEntryChanges = false;
        }

        private void txtLocaleEntryKey_TextChanged(object sender, EventArgs e)
        {
            var keyExistsInLocale = LoadedLocale.LocaleStrings.Exists(ls => ls.Key == txtLocaleEntryKey.Text);
            btnDeleteLocale.Enabled = !MandatoryLocaleKeys.Contains(txtLocaleEntryKey.Text) && keyExistsInLocale;
            btnAddLocale.Enabled = !keyExistsInLocale;
            btnUpdateLocale.Enabled = keyExistsInLocale;

            if (keyExistsInLocale)
            {
                var missingLanguages = new List<string>();
                foreach (var localeToCheck in ActiveDungeon.Locales)
                {
                    if (!localeToCheck.Language.Equals(LoadedLocale.Language) && !localeToCheck.LocaleStrings.Exists(ls => ls.Key == txtLocaleEntryKey.Text))
                    {
                        missingLanguages.Add(localeToCheck.Language);
                    }
                }

                if (missingLanguages.Any())
                {
                    fklblMissingLocale.Visible = true;
                    fklblMissingLocale.Text = $"This locale is missing in the following languages:\n{string.Join(", ", missingLanguages)}";
                }
                else
                {
                    fklblMissingLocale.Visible = false;
                }
            }
        }

        private void txtLocaleEntryValue_TextChanged(object sender, EventArgs e)
        {
            UnsavedLocaleEntryChanges = true;
        }

        public void UpdateCurrentLocaleEntry()
        {
            var localeString = LoadedLocale.LocaleStrings.Find(ls => ls.Key == txtLocaleEntryKey.Text);
            if (localeString != null)
            {
                localeString.Value = txtLocaleEntryValue.Text;
                dgvLocales.Rows[LoadedLocale.LocaleStrings.IndexOf(localeString)].SetValues(txtLocaleEntryKey.Text, txtLocaleEntryValue.Text);
                UnsavedLocaleEntryChanges = false;
                txtLocaleEntryKey_TextChanged(this, EventArgs.Empty);
                TabInfoChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                btnAddLocale_Click(null, EventArgs.Empty);
            }
        }

        private void btnUpdateLocale_Click(object sender, EventArgs e)
        {
            UpdateCurrentLocaleEntry();
        }

        private void btnAddLocale_Click(object sender, EventArgs e)
        {
            LoadedLocale.LocaleStrings.Add(new LocaleInfoString
            {
                Key = txtLocaleEntryKey.Text,
                Value = txtLocaleEntryValue.Text.Replace(Environment.NewLine, "\n")
            }); 
            UnsavedLocaleEntryChanges = false;
            dgvLocales.Rows.Add(txtLocaleEntryKey.Text, txtLocaleEntryValue.Text);
            dgvLocales.Rows[^1].Selected = true;
            txtLocaleEntryKey_TextChanged(this, EventArgs.Empty);
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
        }

        private void btnDeleteLocale_Click(object sender, EventArgs e)
        {
            var messageBoxResult = MessageBox.Show(
                $"Are you sure you want to delete the locale Entry {dgvLocales.SelectedRows[0].Cells[0].Value}?",
                "Delete Locale Entry",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (messageBoxResult == DialogResult.Yes)
            {
                LoadedLocale.LocaleStrings.RemoveAll(ls => ls.Key.Equals(dgvLocales.SelectedRows[0].Cells[0].Value));
                dgvLocales.Rows.RemoveAt(dgvLocales.SelectedRows[0].Index);
                dgvLocales.Rows[^1].Selected = true;
                TabInfoChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
#pragma warning restore CS8601 // Posible asignación de referencia nula
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.