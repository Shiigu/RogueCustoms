using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    public partial class StatTab : UserControl
    {
        private DungeonInfo ActiveDungeon;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public StatInfo LoadedStat { get; private set; }
        public event EventHandler TabInfoChanged;

        public StatTab()
        {
            InitializeComponent();
        }

        public void LoadData(StatInfo statInfoToLoad, DungeonInfo activeDungeon)
        {
            ActiveDungeon = activeDungeon;
            LoadedStat = statInfoToLoad;
            txtStatName.Text = statInfoToLoad.Name;
            cmbStatType.Text = string.Empty;
            if (cmbStatType.Items.Cast<string>().Contains(statInfoToLoad.StatType.ToString()))
                cmbStatType.Text = statInfoToLoad.StatType.ToString();
            cmbStatRegenerationTarget.Items.Clear();
            var pickableRegenerationTargets = activeDungeon.CharacterStats.Where(s => s != statInfoToLoad).Select(s => s.Id);
            foreach (var statId in pickableRegenerationTargets)
            {
                cmbStatRegenerationTarget.Items.Add(statId);
            }
            chkStatHasMax.Checked = statInfoToLoad.HasMax;
            nudStatMinCap.Value = statInfoToLoad.MinCap;
            nudStatMaxCap.Value = statInfoToLoad.MaxCap;
        }

        public List<string> SaveData(string id)
        {
            var validationErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(txtStatName.Text))
                validationErrors.Add("The Stat lacks a Name");

            if (string.IsNullOrWhiteSpace(cmbStatType.Text))
                validationErrors.Add("The Stat lacks a Type");

            if (nudStatMinCap.Value > nudStatMaxCap.Value)
                validationErrors.Add("The Stat's Minimum Cap is higher than its Maximum Cap");

            if (cmbStatType.Text.Equals("Regeneration", StringComparison.InvariantCultureIgnoreCase) && string.IsNullOrWhiteSpace(cmbStatRegenerationTarget.Text))
                validationErrors.Add("The Stat's Type is Regeneration, but no Regeneration Target has been set");

            if (!validationErrors.Any())
            {
                LoadedStat = new()
                {
                    Id = id,
                    Name = txtStatName.Text,
                    StatType = cmbStatType.Text,
                    HasMax = chkStatHasMax.Checked,
                    RegeneratesStatId = cmbStatType.Text.Equals("Regeneration", StringComparison.InvariantCultureIgnoreCase) ? cmbStatRegenerationTarget.Text : null,
                    MinCap = cmbStatType.Text.Equals("Integer", StringComparison.InvariantCultureIgnoreCase) ? (int)nudStatMinCap.Value : nudStatMinCap.Value,
                    MaxCap = cmbStatType.Text.Equals("Integer", StringComparison.InvariantCultureIgnoreCase) ? (int)nudStatMaxCap.Value : nudStatMaxCap.Value,
                };
            }

            return validationErrors;
        }

        private void txtStatName_TextChanged(object sender, EventArgs e)
        {
            txtStatName.ToggleEntryInLocaleWarning(ActiveDungeon, fklblStatNameLocale);
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void cmbStatType_TextChanged(object sender, EventArgs e)
        {
            lblPercentage.Visible = cmbStatType.Text.Equals("Percentage", StringComparison.InvariantCultureIgnoreCase);
            if (cmbStatType.Text.Equals("Decimal", StringComparison.InvariantCultureIgnoreCase) || cmbStatType.Text.Equals("Regeneration", StringComparison.InvariantCultureIgnoreCase))
            {
                nudStatMinCap.DecimalPlaces = 5;
                nudStatMaxCap.DecimalPlaces = 5;
            }
            else
            {
                nudStatMinCap.DecimalPlaces = 0;
                nudStatMinCap.Value = (int)nudStatMinCap.Value;
                nudStatMaxCap.DecimalPlaces = 0;
                nudStatMaxCap.Value = (int)nudStatMaxCap.Value;
            }
            if (cmbStatType.Text.Equals("Regeneration", StringComparison.InvariantCultureIgnoreCase))
            {
                cmbStatRegenerationTarget.Enabled = true;
            }
            else
            {
                cmbStatRegenerationTarget.Enabled = false;
                cmbStatRegenerationTarget.Text = string.Empty;
            }
        }

        private void nudStatMinCap_ValueChanged(object sender, EventArgs e)
        {
            if (nudStatMinCap.Value > nudStatMaxCap.Value)
                nudStatMaxCap.Value = nudStatMinCap.Value;
        }

        private void nudStatMaxCap_ValueChanged(object sender, EventArgs e)
        {
            if (nudStatMinCap.Value > nudStatMaxCap.Value)
                nudStatMinCap.Value = nudStatMaxCap.Value;
        }
    }
}
