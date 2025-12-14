using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    public partial class LearnsetTab : UserControl
    {
        private List<string> ValidScriptIds;
        private DungeonInfo ActiveDungeon;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LearnsetInfo LoadedLearnset { get; private set; }
        public event EventHandler TabInfoChanged;
        public LearnsetTab()
        {
            InitializeComponent();
        }

        public void LoadData(DungeonInfo activeDungeon, LearnsetInfo learnsetToLoad)
        {
            ActiveDungeon = activeDungeon;
            LoadedLearnset = learnsetToLoad ?? new();

            ValidScriptIds = activeDungeon.Scripts.Select(s => s.Id).ToList();

            var validForgettableScriptIds = new List<string>([""]).Union(ValidScriptIds).ToList();

            var learnsColumn = (DataGridViewComboBoxColumn)dgvLearnset.Columns["cmLearns"];
            learnsColumn.DataSource = ValidScriptIds;

            var forgetsColumn = (DataGridViewComboBoxColumn)dgvLearnset.Columns["cmForgets"];
            forgetsColumn.DataSource = validForgettableScriptIds;

            dgvLearnset.Rows.Clear();

            foreach (var entry in LoadedLearnset.Entries)
            {
                var learnedScriptId = ValidScriptIds.Contains(entry.LearnedScriptId) ? entry.LearnedScriptId : "";
                var forgottenScriptId = ValidScriptIds.Contains(entry.ForgotScriptId) ? entry.ForgotScriptId : "";
                dgvLearnset.Rows.Add(entry.Level, learnedScriptId, forgottenScriptId);
            }
        }

        public List<string> SaveData(string id)
        {
            dgvLearnset.EndEdit();
            var validationErrors = new List<string>();
            var learnsetToSave = new LearnsetInfo()
            {
                Id = id,
                Entries = new()
            };
            var learnsetScripts = new List<string>();

            foreach (DataGridViewRow row in dgvLearnset.Rows)
            {
                if (row.IsNewRow) continue;

                var isValidEntry = true;

                var level = Convert.ToInt32(row.Cells[0].Value);
                var learnedScriptId = Convert.ToString(row.Cells[1].Value);
                var forgottenScriptId = Convert.ToString(row.Cells[2].Value);

                if (string.IsNullOrWhiteSpace(learnedScriptId))
                {
                    validationErrors.Add($"Learnset is set not learn a Script at level {level}.");
                    isValidEntry = false;
                }
                else if (!ActiveDungeon.Scripts.Any(s => s.Id.Equals(learnedScriptId)))
                {
                    validationErrors.Add($"Learnset is set to learn {learnedScriptId}, a non-existing Script, at level {level}.");
                    isValidEntry = false;
                }

                if (!string.IsNullOrWhiteSpace(forgottenScriptId) && !ActiveDungeon.Scripts.Any(s => s.Id.Equals(forgottenScriptId)))
                {
                    validationErrors.Add($"Learnset is set to forget {forgottenScriptId}, a non-existing Script, at level {level}.");
                    isValidEntry = false;
                }

                if (learnsetScripts.Contains(learnedScriptId))
                {
                    validationErrors.Add($"Learnset is set to learn {learnedScriptId} at level {level}, which was already learned.");
                    isValidEntry = false;
                }
                else
                {
                    learnsetScripts.Add(learnedScriptId);
                }

                if (!string.IsNullOrWhiteSpace(forgottenScriptId) && !learnsetScripts.Contains(forgottenScriptId))
                {
                    validationErrors.Add($"Learnset is set to forget {forgottenScriptId} at level {level}, which hasn't been learned yet.");
                    isValidEntry = false;
                }
                else if (!string.IsNullOrWhiteSpace(forgottenScriptId))
                {
                    learnsetScripts.Remove(forgottenScriptId);
                }

                if (isValidEntry)
                {
                    learnsetToSave.Entries.Add(new LearnsetEntryInfo()
                    {
                        Level = level,
                        LearnedScriptId = learnedScriptId,
                        ForgotScriptId = forgottenScriptId ?? ""
                    });
                }
            }

            if (!validationErrors.Any())
            {
                LoadedLearnset = learnsetToSave;
            }

            return validationErrors.Distinct().ToList();
        }

        private void dgvLearnset_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.FormattedValue != string.Empty && (!int.TryParse(Convert.ToString(e.FormattedValue), out int level) || level < 1))
            {
                e.Cancel = true;
            }
        }

        private void dgvLearnset_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            TabInfoChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
