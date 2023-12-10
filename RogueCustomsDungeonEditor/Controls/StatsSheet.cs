using RogueCustomsDungeonEditor.Utils;
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

namespace RogueCustomsDungeonEditor.Controls
{
    public partial class StatsSheet : UserControl
    {
        private string PreviousTextBoxValue = string.Empty;

        public int BaseHP
        {
            get
            {
                return (int)nudBaseHP.Value;
            }
            set
            {
                nudBaseHP.Value = value;
            }
        }

        public decimal BaseHPRegeneration
        {
            get
            {
                return nudBaseHPRegeneration.Value;
            }
            set
            {
                nudBaseHPRegeneration.Value = value;
            }
        }

        public bool UsesMP
        {
            get
            {
                return chkUsesMP.Checked;
            }
            set
            {
                chkUsesMP.Checked = value;
                ToggleMPControls();
            }
        }

        public int BaseMP
        {
            get
            {
                return UsesMP ? (int)nudBaseMP.Value : 0;
            }
            set
            {
                if (UsesMP)
                    nudBaseMP.Value = value;
            }
        }

        public decimal BaseMPRegeneration
        {
            get
            {
                return UsesMP ? nudBaseMPRegeneration.Value : 0;
            }
            set
            {
                if (UsesMP)
                    nudBaseMPRegeneration.Value = value;
            }
        }

        public int BaseAttack
        {
            get
            {
                return (int)nudBaseAttack.Value;
            }
            set
            {
                nudBaseAttack.Value = value;
            }
        }
        public int BaseDefense
        {
            get
            {
                return (int)nudBaseDefense.Value;
            }
            set
            {
                nudBaseDefense.Value = value;
            }
        }

        public int BaseAccuracy
        {
            get
            {
                return (int)nudBaseAccuracy.Value;
            }
            set
            {
                nudBaseAccuracy.Value = value;
            }
        }

        public int BaseEvasion
        {
            get
            {
                return (int)nudBaseEvasion.Value;
            }
            set
            {
                nudBaseEvasion.Value = value;
            }
        }

        public int BaseMovement
        {
            get
            {
                return (int)nudBaseMovement.Value;
            }
            set
            {
                nudBaseMovement.Value = value;
            }
        }

        public Dictionary<string, string> BaseSightRangeDisplayNames { get; set; } = new Dictionary<string, string>();

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
        public bool UsesHunger
        {
            get
            {
                return chkUsesHunger.Checked;
            }
            set
            {
                chkUsesHunger.Checked = value;
                ToggleHungerControls();
            }
        }

        public int BaseHunger
        {
            get
            {
                return UsesHunger ? (int)nudBaseMaxHunger.Value : 0;
            }
            set
            {
                if (UsesHunger)
                    nudBaseMaxHunger.Value = value;
            }
        }

        public decimal HungerHPDegeneration
        {
            get
            {
                return UsesHunger ? nudHungerHPDegeneration.Value : 0;
            }
            set
            {
                if (UsesHunger)
                    nudHungerHPDegeneration.Value = value;
            }
        }

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

        public decimal HPPerLevelUp
        {
            get
            {
                return nudHPPerLevelUp.Value;
            }
            set
            {
                nudHPPerLevelUp.Value = value;
            }
        }

        public decimal HPRegenerationPerLevelUp
        {
            get
            {
                return nudHPRegenerationPerLevelUp.Value;
            }
            set
            {
                nudHPRegenerationPerLevelUp.Value = value;
            }
        }

        public decimal MPPerLevelUp
        {
            get
            {
                return UsesMP ? nudMPPerLevelUp.Value : 0;
            }
            set
            {
                if (UsesMP)
                    nudMPPerLevelUp.Value = value;
            }
        }

        public decimal MPRegenerationPerLevelUp
        {
            get
            {
                return UsesMP ? nudMPRegenerationPerLevelUp.Value : 0;
            }
            set
            {
                if (UsesMP)
                    nudMPRegenerationPerLevelUp.Value = value;
            }
        }

        public decimal AttackPerLevelUp
        {
            get
            {
                return nudAttackPerLevelUp.Value;
            }
            set
            {
                nudAttackPerLevelUp.Value = value;
            }
        }

        public decimal DefensePerLevelUp
        {
            get
            {
                return nudDefensePerLevelUp.Value;
            }
            set
            {
                nudDefensePerLevelUp.Value = value;
            }
        }

        public decimal MovementPerLevelUp
        {
            get
            {
                return nudMovementPerLevelUp.Value;
            }
            set
            {
                nudMovementPerLevelUp.Value = value;
            }
        }

        public event EventHandler StatsChanged = delegate { };

        public StatsSheet()
        {
            InitializeComponent();
            nudBaseHP.ValueChanged += (_, _) => StatsChanged.Invoke(this, EventArgs.Empty);
            nudBaseMP.ValueChanged += (_, _) => StatsChanged.Invoke(this, EventArgs.Empty);
            chkUsesMP.CheckedChanged += (_, _) => ToggleMPControls();
            nudBaseAttack.ValueChanged += (_, _) => StatsChanged.Invoke(this, EventArgs.Empty);
            nudBaseDefense.ValueChanged += (_, _) => StatsChanged.Invoke(this, EventArgs.Empty);
            nudBaseMovement.ValueChanged += (_, _) => StatsChanged.Invoke(this, EventArgs.Empty);
            nudBaseAccuracy.ValueChanged += (_, _) => StatsChanged.Invoke(this, EventArgs.Empty);
            nudBaseEvasion.ValueChanged += (_, _) => StatsChanged.Invoke(this, EventArgs.Empty);
            nudBaseHPRegeneration.ValueChanged += (_, _) => StatsChanged.Invoke(this, EventArgs.Empty);
            nudBaseMPRegeneration.ValueChanged += (_, _) => StatsChanged.Invoke(this, EventArgs.Empty);
            nudFlatSightRange.ValueChanged += (_, _) => StatsChanged.Invoke(this, EventArgs.Empty);
            chkUsesHunger.CheckedChanged += (_, _) => ToggleHungerControls();
            nudBaseMaxHunger.ValueChanged += (_, _) => StatsChanged.Invoke(this, EventArgs.Empty);
            nudHungerHPDegeneration.ValueChanged += (_, _) => StatsChanged.Invoke(this, EventArgs.Empty);
            chkCanGainExperience.CheckedChanged += (_, _) => ToggleLevelUpControls();
            nudMaxLevel.ValueChanged += (_, _) => ToggleLevelUpControls();
            nudHPPerLevelUp.ValueChanged += (_, _) => StatsChanged.Invoke(this, EventArgs.Empty);
            nudMPPerLevelUp.ValueChanged += (_, _) => StatsChanged.Invoke(this, EventArgs.Empty);
            nudAttackPerLevelUp.ValueChanged += (_, _) => StatsChanged.Invoke(this, EventArgs.Empty);
            nudDefensePerLevelUp.ValueChanged += (_, _) => StatsChanged.Invoke(this, EventArgs.Empty);
            nudMovementPerLevelUp.ValueChanged += (_, _) => StatsChanged.Invoke(this, EventArgs.Empty);
            nudHPRegenerationPerLevelUp.ValueChanged += (_, _) => StatsChanged.Invoke(this, EventArgs.Empty);
            nudMPRegenerationPerLevelUp.ValueChanged += (_, _) => StatsChanged.Invoke(this, EventArgs.Empty);
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
            nudHPPerLevelUp.Enabled = chkCanGainExperience.Checked || nudMaxLevel.Value > 1;
            nudMPPerLevelUp.Enabled = (chkCanGainExperience.Checked || nudMaxLevel.Value > 1) && chkUsesMP.Checked;
            nudAttackPerLevelUp.Enabled = chkCanGainExperience.Checked || nudMaxLevel.Value > 1;
            nudDefensePerLevelUp.Enabled = chkCanGainExperience.Checked || nudMaxLevel.Value > 1;
            nudMovementPerLevelUp.Enabled = chkCanGainExperience.Checked || nudMaxLevel.Value > 1;
            nudHPRegenerationPerLevelUp.Enabled = chkCanGainExperience.Checked || nudMaxLevel.Value > 1;
            nudMPRegenerationPerLevelUp.Enabled = (chkCanGainExperience.Checked || nudMaxLevel.Value > 1) && chkUsesMP.Checked;
            StatsChanged.Invoke(this, EventArgs.Empty);
        }

        private void ToggleMPControls()
        {
            nudBaseMP.Enabled = chkUsesMP.Checked;
            if (!chkUsesMP.Checked)
                nudBaseMP.Value = 0;
            nudBaseMPRegeneration.Enabled = chkUsesMP.Checked;
            if (!chkUsesMP.Checked)
                nudBaseMPRegeneration.Value = 0;
            nudMPPerLevelUp.Enabled = (chkCanGainExperience.Checked || nudMaxLevel.Value > 1) && chkUsesMP.Checked;
            if (!chkUsesMP.Checked)
                nudMPPerLevelUp.Value = 0;
            nudMPRegenerationPerLevelUp.Enabled = (chkCanGainExperience.Checked || nudMaxLevel.Value > 1) && chkUsesMP.Checked;
            if (!chkUsesMP.Checked)
                nudMPRegenerationPerLevelUp.Value = 0;
            StatsChanged.Invoke(this, EventArgs.Empty);
        }

        private void ToggleHungerControls()
        {
            nudBaseMaxHunger.Enabled = chkUsesHunger.Checked;
            if (!chkUsesHunger.Checked)
                nudBaseMaxHunger.Value = 0;
            nudHungerHPDegeneration.Enabled = chkUsesHunger.Checked;
            if (!chkUsesHunger.Checked)
                nudHungerHPDegeneration.Value = 0;
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
    }
}
