using RogueCustomsDungeonEditor.Clipboard;
using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsGameEngine.Utils.JsonImports;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.

namespace RogueCustomsDungeonEditor.Controls
{
    public partial class StatsSheet : UserControl
    {
        private string PreviousTextBoxValue = string.Empty;
        private string PreviousCellValue = string.Empty;

        private List<(string Id, bool IsDecimal, bool IsPercentage)> StatTableData = new();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<StatInfo> StatData
        {
            set
            {
                StatTableData.Clear();
                foreach (var stat in value)
                {
                    var isDecimal = stat.StatType.Equals("Decimal", StringComparison.InvariantCultureIgnoreCase) || stat.StatType.Equals("Regeneration", StringComparison.InvariantCultureIgnoreCase);
                    var isPercentage = stat.StatType.Equals("Percentage", StringComparison.InvariantCultureIgnoreCase);
                    StatTableData.Add((stat.Id, isDecimal, isPercentage));
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<CharacterStatInfo> Stats
        {
            get
            {
                dgvStats.EndEdit();

                var statsList = new List<CharacterStatInfo>();

                foreach (DataGridViewRow row in dgvStats.Rows)
                {
                    if (row.IsNewRow) continue;
                    var isUsed = Convert.ToBoolean(row.Cells["Used"].Value);
                    if (isUsed)
                    {
                        var statInfo = StatTableData.First(s => s.Id.Equals(row.Cells["Id"].Value.ToString()));
                        var characterStat = new CharacterStatInfo()
                        {
                            StatId = statInfo.Id,
                            Base = row.Cells["Base"].Value?.ToString() != null ? decimal.Parse(row.Cells["Base"].Value.ToString().Replace("%", "").Replace("+", ""), NumberStyles.Float, CultureInfo.InvariantCulture) : 0,
                            IncreasePerLevel = row.Cells["Base"].Value?.ToString() != null ? decimal.Parse(row.Cells["IncreasePerLevel"].Value.ToString().Replace("%", "").Replace("+", ""), NumberStyles.Float, CultureInfo.InvariantCulture) : 0,
                            Minimum = row.Cells["Base"].Value?.ToString() != null ? decimal.Parse(row.Cells["Minimum"].Value.ToString().Replace("%", "").Replace("+", ""), NumberStyles.Float, CultureInfo.InvariantCulture) : 0,
                            Maximum = row.Cells["Base"].Value?.ToString() != null ? decimal.Parse(row.Cells["Maximum"].Value.ToString().Replace("%", "").Replace("+", ""), NumberStyles.Float, CultureInfo.InvariantCulture) : 0
                        };
                        statsList.Add(characterStat);
                    }
                }

                return statsList;
            }
            set
            {
                dgvStats.Rows.Clear();

                foreach (var stat in StatTableData)
                {
                    var characterStat = value.FirstOrDefault(s => s.StatId.Equals(stat.Id));
                    var isUsed = characterStat != null;
                    characterStat ??= new CharacterStatInfo()
                    {
                        StatId = stat.Id,
                        Base = 1,
                        IncreasePerLevel = 0,
                        Minimum = 0,
                        Maximum = 9999
                    };

                    var baseDisplayStat = characterStat.Base.ToString();
                    var increasePerLevelDisplayStat = characterStat.IncreasePerLevel.ToString();
                    var minimumDisplayStat = characterStat.Minimum.ToString();
                    var maximumDisplayStat = characterStat.Maximum.ToString();

                    if (!stat.IsPercentage)
                    {
                        if (!stat.IsDecimal)
                        {
                            increasePerLevelDisplayStat = characterStat.IncreasePerLevel.ToString("+0;-0", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            baseDisplayStat = characterStat.Base.ToString("0.00###", CultureInfo.InvariantCulture);
                            increasePerLevelDisplayStat = characterStat.IncreasePerLevel.ToString("+0.00###;-0.00###", CultureInfo.InvariantCulture);
                            minimumDisplayStat = characterStat.Minimum.ToString("0.00###", CultureInfo.InvariantCulture);
                            maximumDisplayStat = characterStat.Maximum.ToString("0.00###", CultureInfo.InvariantCulture);
                        }
                    }
                    else
                    {
                        baseDisplayStat = characterStat.Base.ToString("0.#####", CultureInfo.InvariantCulture) + "%";
                        increasePerLevelDisplayStat = characterStat.IncreasePerLevel.ToString("+0.#####;-0.#####", CultureInfo.InvariantCulture) + "%";
                        minimumDisplayStat = characterStat.Minimum.ToString("0.#####", CultureInfo.InvariantCulture) + "%";
                        maximumDisplayStat = characterStat.Maximum.ToString("0.#####", CultureInfo.InvariantCulture) + "%";
                    }

                    dgvStats.Rows.Add(
                        stat.Id,
                        isUsed,
                        baseDisplayStat,
                        increasePerLevelDisplayStat,
                        minimumDisplayStat,
                        maximumDisplayStat
                    );
                }

                ApplyReadOnlyRules();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Dictionary<string, string> BaseSightRangeDisplayNames { get; set; } = new Dictionary<string, string>();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string BaseSightRange
        {
            get
            {
                if (cmbSightRange.Text.Equals(BaseSightRangeDisplayNames["FlatNumber"]))
                {
                    return ((int)nudFlatSightRange.Value).ToString();
                }
                else
                {
                    return BaseSightRangeDisplayNames.FirstOrDefault(bsrdn => bsrdn.Value.Equals(cmbSightRange.Text)).Key;
                }
            }
            set
            {
                cmbSightRange.Items.Clear();
                cmbSightRange.Text = "";
                foreach (var sightRange in BaseSightRangeDisplayNames)
                {
                    cmbSightRange.Items.Add(sightRange.Value);
                    if (sightRange.Key.Equals(value))
                        cmbSightRange.Text = sightRange.Value;
                }
                if (string.IsNullOrWhiteSpace(cmbSightRange.Text))
                {
                    cmbSightRange.Text = BaseSightRangeDisplayNames["FlatNumber"];
                    lblSightRangeText.Visible = true;
                    nudFlatSightRange.Visible = true;
                    nudFlatSightRange.Enabled = true;
                    try
                    {
                        nudFlatSightRange.Value = int.Parse(value);
                    }
                    catch
                    {
                        nudFlatSightRange.Value = 1;
                    }
                }
                else
                {
                    lblSightRangeText.Visible = false;
                    nudFlatSightRange.Visible = false;
                    nudFlatSightRange.Enabled = false;
                    nudFlatSightRange.Value = 1;
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanGainExperience
        {
            get
            {
                return chkCanGainExperience.Checked;
            }
            set
            {
                chkCanGainExperience.Checked = value;
                ToggleLevelUpControls();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ExperienceToLevelUpFormula
        {
            get
            {
                return txtLevelUpFormula.Text;
            }
            set
            {
                txtLevelUpFormula.Text = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int MaxLevel
        {
            get
            {
                return (int)nudMaxLevel.Value;
            }
            set
            {
                nudMaxLevel.Value = value;
                ToggleLevelUpControls();
            }
        }

        public event EventHandler StatsChanged = delegate { };

        public StatsSheet()
        {
            InitializeComponent();
            nudFlatSightRange.ValueChanged += (_, _) =>
            {
                StatsChanged.Invoke(this, EventArgs.Empty);
            };
            chkCanGainExperience.CheckedChanged += (_, _) => ToggleLevelUpControls();
            nudMaxLevel.ValueChanged += (_, _) => ToggleLevelUpControls();
            btnPasteStats.Enabled = ClipboardManager.ContainsData(FormConstants.StatsClipboardKey);
            ClipboardManager.ClipboardContentsChanged += ClipboardManager_ClipboardContentsChanged;
        }

        private void ClipboardManager_ClipboardContentsChanged(object? sender, EventArgs e)
        {
            btnPasteStats.Enabled = ClipboardManager.ContainsData(FormConstants.StatsClipboardKey);
        }

        private void cmbSightRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSightRange.Text.Equals(BaseSightRangeDisplayNames["FlatNumber"]))
            {
                lblSightRangeText.Visible = true;
                nudFlatSightRange.Visible = true;
                nudFlatSightRange.Enabled = true;
            }
            else
            {
                lblSightRangeText.Visible = false;
                nudFlatSightRange.Visible = false;
                nudFlatSightRange.Enabled = false;
            }
            StatsChanged.Invoke(this, EventArgs.Empty);
        }


        private void ToggleLevelUpControls()
        {
            txtLevelUpFormula.Enabled = chkCanGainExperience.Checked || nudMaxLevel.Value > 1;
            StatsChanged.Invoke(this, EventArgs.Empty);
        }

        private void txtLevelUpFormula_Enter(object sender, EventArgs e)
        {
            PreviousTextBoxValue = txtLevelUpFormula.Text;
        }

        private void txtLevelUpFormula_Leave(object sender, EventArgs e)
        {
            if (!PreviousTextBoxValue.Equals(txtLevelUpFormula.Text))
            {
                var parsedLevelUpFormula = Regex.Replace(txtLevelUpFormula.Text, @"\blevel\b", "1", RegexOptions.IgnoreCase);

                if (!string.IsNullOrWhiteSpace(parsedLevelUpFormula) && !parsedLevelUpFormula.TestNumericExpression(false, out string errorMessage))
                {
                    MessageBox.Show(
                        $"You have entered an invalid Experience Formula: {errorMessage}",
                        "Invalid Formula",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    txtLevelUpFormula.Text = PreviousTextBoxValue;
                }
                else
                {
                    StatsChanged.Invoke(this, EventArgs.Empty);
                }
            }

            PreviousTextBoxValue = string.Empty;
        }

        private void ApplyReadOnlyRules()
        {
            foreach (DataGridViewRow row in dgvStats.Rows)
            {
                ApplyReadOnlyToRow(row.Index);
            }
        }

        private void dgvStats_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            PreviousCellValue = dgvStats[e.ColumnIndex, e.RowIndex].Value?.ToString() ?? string.Empty;
        }

        private void dgvStats_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            // This triggers the update immediately
            if (dgvStats.IsCurrentCellDirty && dgvStats.CurrentCell is DataGridViewCheckBoxCell)
            {
                dgvStats.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvStats_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == dgvStats.Columns["Used"].Index)
            {
                ApplyReadOnlyToRow(e.RowIndex);
            }
            else if (e.ColumnIndex > dgvStats.Columns["Used"].Index)
            {
                var cellValue = dgvStats[e.ColumnIndex, e.RowIndex].Value?.ToString() ?? string.Empty;

                if (decimal.TryParse(cellValue, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal result))
                {
                    var firstColumnValue = dgvStats[0, e.RowIndex].Value?.ToString() ?? string.Empty;
                    var statData = StatTableData.FirstOrDefault(s => s.Id.Equals(firstColumnValue));
                    if (!statData.IsPercentage)
                    {
                        if (!statData.IsDecimal)
                        {
                            result = (int)result;
                            if (e.ColumnIndex == dgvStats.Columns["IncreasePerLevel"].Index)
                                dgvStats[e.ColumnIndex, e.RowIndex].Value = result.ToString("+0;-0", CultureInfo.InvariantCulture);
                            else
                                dgvStats[e.ColumnIndex, e.RowIndex].Value = result.ToString("0", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            if (e.ColumnIndex == dgvStats.Columns["IncreasePerLevel"].Index)
                                dgvStats[e.ColumnIndex, e.RowIndex].Value = result.ToString("+0.00###;-0.00###", CultureInfo.InvariantCulture);
                            else
                                dgvStats[e.ColumnIndex, e.RowIndex].Value = result.ToString("0.00###", CultureInfo.InvariantCulture);
                        }
                    }
                    else
                    {
                        result = (int)result;
                        if (e.ColumnIndex == dgvStats.Columns["IncreasePerLevel"].Index)
                            dgvStats[e.ColumnIndex, e.RowIndex].Value = result.ToString("+0.###;-0.###", CultureInfo.InvariantCulture) + "%";
                        else
                            dgvStats[e.ColumnIndex, e.RowIndex].Value = result.ToString("0.###", CultureInfo.InvariantCulture) + "%";
                    }
                }
            }
            else
            {
                dgvStats[e.ColumnIndex, e.RowIndex].Value = PreviousCellValue;
            }

            StatsChanged.Invoke(this, EventArgs.Empty);
        }

        private void ApplyReadOnlyToRow(int rowIndex)
        {
            if (rowIndex < 0) return;

            var row = dgvStats.Rows[rowIndex];

            if (row.IsNewRow) return;

            var statId = row.Cells["Id"].Value?.ToString() ?? string.Empty;

            if(FormConstants.MandatoryStats.Contains(statId))
            {
                row.Cells["Used"].Value = true;
                row.Cells["Used"].ReadOnly = true;

                for (int i = 0; i < row.Cells.Count; i++)
                {
                    if (i == dgvStats.Columns["Used"].Index)
                        continue;
                    row.Cells[i].ReadOnly = false;
                }

                row.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                row.Cells["Used"].Style.BackColor = System.Drawing.Color.LightGray;

                return;
            }

            if (row.Cells["Used"] is not DataGridViewCheckBoxCell chkUsed) return;

            var isChecked = Convert.ToBoolean(chkUsed.Value);

            for (int i = 0; i < row.Cells.Count; i++)
            {
                if (i == dgvStats.Columns["Used"].Index)
                    continue;
                row.Cells[i].ReadOnly = !isChecked;
            }

            // Grey out read-only rows
            row.DefaultCellStyle.BackColor = isChecked
                ? System.Drawing.Color.White
                : System.Drawing.Color.LightGray;
        }

        private void btnCopyStats_Click(object sender, EventArgs e)
        {
            ClipboardManager.Copy(FormConstants.StatsClipboardKey, Stats);
        }

        private void btnPasteStats_Click(object sender, EventArgs e)
        {
            Stats = ClipboardManager.Paste<List<CharacterStatInfo>>(FormConstants.StatsClipboardKey);
            StatsChanged.Invoke(this, EventArgs.Empty);
        }
    }
}
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
