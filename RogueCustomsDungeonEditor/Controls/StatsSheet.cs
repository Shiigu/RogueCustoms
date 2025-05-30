﻿using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Utils.JsonImports;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.

namespace RogueCustomsDungeonEditor.Controls
{
    public partial class StatsSheet : UserControl
    {
        private string PreviousTextBoxValue = string.Empty;
        private bool SwitchingStats;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<StatInfo> StatInfos { get; set; }
        private List<CharacterStatInfoControlParams> CharacterStats;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<CharacterStatInfo> Stats
        {
            get
            {
                var statsList = new List<CharacterStatInfo>();

                foreach (var stat in CharacterStats)
                {
                    if (!stat.Used) continue;
                    statsList.Add(new()
                    {
                        StatId = stat.StatId,
                        Base = stat.Base,
                        IncreasePerLevel = stat.IncreasePerLevel
                    });
                }

                return statsList;
            }
            set
            {
                CharacterStats = new();

                foreach (var stat in StatInfos)
                {
                    var characterStat = value.Find(v => v.StatId.Equals(stat.Id, StringComparison.InvariantCultureIgnoreCase));
                    if (characterStat != null)
                    {
                        CharacterStats.Add(new()
                        {
                            Used = true,
                            StatId = characterStat.StatId,
                            Base = characterStat.Base,
                            IncreasePerLevel = characterStat.IncreasePerLevel
                        });
                    }
                    else
                    {
                        CharacterStats.Add(new()
                        {
                            Used = false,
                            StatId = stat.Id,
                            Base = 0,
                            IncreasePerLevel = 0
                        });
                    }
                }

                hsbStats.Value = 0;
                hsbStats.Maximum = CharacterStats.Count - 1;
                UpdateStatControls();
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
                if (!SwitchingStats)
                    StatsChanged.Invoke(this, EventArgs.Empty);
            };
            chkCanGainExperience.CheckedChanged += (_, _) => ToggleLevelUpControls();
            nudMaxLevel.ValueChanged += (_, _) => ToggleLevelUpControls();
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
            if (!SwitchingStats)
                StatsChanged.Invoke(this, EventArgs.Empty);
        }

        private void UpdateStatControls()
        {
            SwitchingStats = true;
            var statToUse = CharacterStats[hsbStats.Value];
            var correspondingStatInfo = StatInfos[hsbStats.Value];
            var regenerationTargetStat = CharacterStats.Find(cs => cs.StatId.Equals(correspondingStatInfo.RegeneratesStatId, StringComparison.InvariantCultureIgnoreCase));
            var isMandatoryStat = FormConstants.MandatoryStats.Contains(statToUse.StatId);

            lblStatId.Text = statToUse.StatId;
            nudBase.Minimum = correspondingStatInfo.MinCap;
            nudBase.Maximum = correspondingStatInfo.MaxCap;
            nudIncreasePerLevel.Minimum = 0;
            nudIncreasePerLevel.Maximum = correspondingStatInfo.MaxCap;

            if (correspondingStatInfo.StatType.Equals("Decimal", StringComparison.InvariantCultureIgnoreCase) || correspondingStatInfo.StatType.Equals("Regeneration", StringComparison.InvariantCultureIgnoreCase))
                nudBase.DecimalPlaces = 5;
            else
                nudBase.DecimalPlaces = 0;

            nudBase.Value = statToUse.Base;
            nudIncreasePerLevel.Value = statToUse.IncreasePerLevel;
            lblPercentage.Visible = correspondingStatInfo.StatType.Equals("Percentage", StringComparison.InvariantCultureIgnoreCase);

            chkIsUsed.Enabled = !isMandatoryStat;

            if (statToUse == null || !statToUse.Used || (regenerationTargetStat != null && !regenerationTargetStat.Used))
            {
                chkIsUsed.Checked = false;
            }
            else
            {
                chkIsUsed.Checked = true;
            }

            ToggleByCheckedStatus();
            SwitchingStats = false;
        }

        private void ToggleLevelUpControls()
        {
            txtLevelUpFormula.Enabled = chkCanGainExperience.Checked || nudMaxLevel.Value > 1;
            if (!SwitchingStats)
                StatsChanged.Invoke(this, EventArgs.Empty);
        }

        private void txtLevelUpFormula_Enter(object sender, EventArgs e)
        {
            PreviousTextBoxValue = txtLevelUpFormula.Text;
        }

        private void txtLevelUpFormula_Leave(object sender, EventArgs e)
        {
            if (SwitchingStats) return;
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

        private void hsbStats_ValueChanged(object sender, EventArgs e)
        {
            UpdateStatControls();
        }

        private void chkIsUsed_CheckedChanged(object sender, EventArgs e)
        {
            ToggleByCheckedStatus();
            if (!SwitchingStats)
                StatsChanged.Invoke(this, EventArgs.Empty);
        }

        private void ToggleByCheckedStatus()
        {
            var statToUse = CharacterStats[hsbStats.Value];
            CharacterStats[hsbStats.Value].Used = chkIsUsed.Checked;
            var correspondingStatInfo = StatInfos[hsbStats.Value];
            if (!chkIsUsed.Checked)
            {
                nudBase.Enabled = false;
                nudBase.Minimum = 0;
                nudBase.Maximum = 0;
                nudBase.Value = 0;
                nudIncreasePerLevel.Enabled = false;
                nudIncreasePerLevel.Minimum = 0;
                nudIncreasePerLevel.Maximum = 0;
                nudIncreasePerLevel.Value = 0;
            }
            else
            {
                nudBase.Enabled = true;
                nudBase.Minimum = correspondingStatInfo.MinCap;
                nudBase.Maximum = correspondingStatInfo.MaxCap;
                nudBase.Value = statToUse.Base;
                nudIncreasePerLevel.Enabled = true;
                nudIncreasePerLevel.Minimum = 0;
                nudIncreasePerLevel.Maximum = correspondingStatInfo.MaxCap;
                nudIncreasePerLevel.Value = statToUse.IncreasePerLevel;
            }
        }

        private void nudBase_Leave(object sender, EventArgs e)
        {
            if (SwitchingStats) return;
            var correspondingStatInfo = StatInfos[hsbStats.Value];
            if (correspondingStatInfo.StatType.Equals("Decimal", StringComparison.InvariantCultureIgnoreCase) || correspondingStatInfo.StatType.Equals("Regeneration", StringComparison.InvariantCultureIgnoreCase))
                CharacterStats[hsbStats.Value].Base = nudBase.Value;
            else
                CharacterStats[hsbStats.Value].Base = (int)nudBase.Value;
            StatsChanged.Invoke(this, EventArgs.Empty);
        }

        private void nudIncreasePerLevel_Leave(object sender, EventArgs e)
        {
            if (SwitchingStats) return;
            var correspondingStatInfo = StatInfos[hsbStats.Value];
            CharacterStats[hsbStats.Value].IncreasePerLevel = nudIncreasePerLevel.Value;
            StatsChanged.Invoke(this, EventArgs.Empty);
        }

        private void hsbStats_MouseEnter(object sender, EventArgs e)
        {
            hsbStats.Focus();
        }
    }

    public class CharacterStatInfoControlParams
    {
        public bool Used { get; set; }
        public string StatId { get; set; }
        public decimal Base { get; set; }
        public decimal IncreasePerLevel { get; set; }
    }
}
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
