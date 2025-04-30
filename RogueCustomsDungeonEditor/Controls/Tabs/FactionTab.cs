using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    public partial class FactionTab : UserControl
    {
        private DungeonInfo ActiveDungeon;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FactionInfo LoadedFaction { get; private set; }
        public event EventHandler TabInfoChanged;
        public FactionTab()
        {
            InitializeComponent();
        }

        public void LoadData(DungeonInfo dungeon, FactionInfo faction)
        {
            ActiveDungeon = dungeon;
            LoadedFaction = faction;
            txtFactionName.Text = faction.Name;
            txtFactionName.ToggleEntryInLocaleWarning(ActiveDungeon, fklblFactionNameLocale);
            txtFactionDescription.Text = faction.Description;
            txtFactionDescription.ToggleEntryInLocaleWarning(ActiveDungeon, fklblFactionDescriptionLocale);
            lbAllies.Items.Clear();
            lbEnemies.Items.Clear();
            lbNeutrals.Items.Clear();

            var factionId = faction.Id;
            var allies = dungeon.FactionInfos
                .Where(otherFaction => otherFaction.Id != factionId && faction.AlliedWith.Contains(otherFaction.Id))
                .Select(otherFaction => otherFaction.Id);
            var enemies = dungeon.FactionInfos
                .Where(otherFaction => otherFaction.Id != factionId && faction.EnemiesWith.Contains(otherFaction.Id))
                .Select(otherFaction => otherFaction.Id);
            var neutrals = dungeon.FactionInfos
                .Where(otherFaction => otherFaction.Id != factionId && !faction.AlliedWith.Contains(otherFaction.Id) && !faction.EnemiesWith.Contains(otherFaction.Id))
                .Select(otherFaction => otherFaction.Id);

            lbAllies.Items.AddRange(allies.ToArray());
            lbEnemies.Items.AddRange(enemies.ToArray());
            lbNeutrals.Items.AddRange(neutrals.ToArray());

            btnAlliesToNeutrals.Enabled = lbAllies.Items.Count > 0;
            btnNeutralsToAllies.Enabled = lbNeutrals.Items.Count > 0;
            btnNeutralsToEnemies.Enabled = lbNeutrals.Items.Count > 0;
            btnEnemiesToNeutrals.Enabled = lbEnemies.Items.Count > 0;
        }

        public List<string> SaveData(string id)
        {
            var validationErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(txtFactionName.Text))
                validationErrors.Add("Enter a Faction Name first.");
            if (string.IsNullOrWhiteSpace(txtFactionDescription.Text))
                validationErrors.Add("Enter a Faction Description first.");

            if (!validationErrors.Any())
            {
                LoadedFaction = new();
                LoadedFaction.Id = id;
                LoadedFaction.Name = txtFactionName.Text;
                LoadedFaction.Description = txtFactionDescription.Text;
                LoadedFaction.AlliedWith = new();
                foreach (string allyId in lbAllies.Items)
                {
                    LoadedFaction.AlliedWith.Add(allyId);
                }
                LoadedFaction.NeutralWith = new();
                foreach (string neutralId in lbNeutrals.Items)
                {
                    LoadedFaction.NeutralWith.Add(neutralId);
                }
                LoadedFaction.EnemiesWith = new();
                foreach (string enemyId in lbEnemies.Items)
                {
                    LoadedFaction.EnemiesWith.Add(enemyId);
                }
            }

            return validationErrors;
        }


        private void txtFactionName_TextChanged(object sender, EventArgs e)
        {
            txtFactionName.ToggleEntryInLocaleWarning(ActiveDungeon, fklblFactionNameLocale);
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void txtFactionDescription_TextChanged(object sender, EventArgs e)
        {
            txtFactionDescription.ToggleEntryInLocaleWarning(ActiveDungeon, fklblFactionDescriptionLocale);
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void lbAllies_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnAllyToNeutral.Enabled = lbAllies.SelectedItem != null;
        }

        private void lbNeutrals_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnNeutralToAlly.Enabled = lbNeutrals.SelectedItem != null;
            btnNeutralToEnemy.Enabled = lbNeutrals.SelectedItem != null;
        }

        private void lbEnemies_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnEnemyToNeutral.Enabled = lbEnemies.SelectedItem != null;
        }

        private void btnNeutralToAlly_Click(object sender, EventArgs e)
        {
            lbAllies.Items.Add(lbNeutrals.SelectedItem);
            lbNeutrals.Items.Remove(lbNeutrals.SelectedItem);
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
            btnNeutralToAlly.Enabled = false;
        }

        private void btnAllyToNeutral_Click(object sender, EventArgs e)
        {
            lbNeutrals.Items.Add(lbAllies.SelectedItem);
            lbAllies.Items.Remove(lbAllies.SelectedItem);
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
            btnAllyToNeutral.Enabled = false;
        }

        private void btnEnemyToNeutral_Click(object sender, EventArgs e)
        {
            lbNeutrals.Items.Add(lbEnemies.SelectedItem);
            lbEnemies.Items.Remove(lbEnemies.SelectedItem);
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
            btnEnemyToNeutral.Enabled = false;
        }

        private void btnNeutralToEnemy_Click(object sender, EventArgs e)
        {
            lbEnemies.Items.Add(lbNeutrals.SelectedItem);
            lbNeutrals.Items.Remove(lbNeutrals.SelectedItem);
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
            btnNeutralToEnemy.Enabled = false;
        }

        private void btnNeutralsToAllies_Click(object sender, EventArgs e)
        {
            var neutralsToRemove = new List<string>();
            foreach (string neutralId in lbNeutrals.Items)
            {
                lbAllies.Items.Add(neutralId);
                neutralsToRemove.Add(neutralId);
            }
            foreach (var neutralToRemove in neutralsToRemove)
            {
                lbNeutrals.Items.Remove(neutralToRemove);
            }
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
            btnNeutralToAlly.Enabled = false;
            UpdateMultiButtonsEnabled();
        }

        private void btnAlliesToNeutrals_Click(object sender, EventArgs e)
        {
            var alliesToRemove = new List<string>();
            foreach (string allyId in lbAllies.Items)
            {
                lbNeutrals.Items.Add(allyId);
                alliesToRemove.Add(allyId);
            }
            foreach (var allyToRemove in alliesToRemove)
            {
                lbAllies.Items.Remove(allyToRemove);
            }
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
            btnAllyToNeutral.Enabled = false;
            UpdateMultiButtonsEnabled();
        }

        private void btnEnemiesToNeutrals_Click(object sender, EventArgs e)
        {
            var enemiesToRemove = new List<string>();
            foreach (string enemyId in lbEnemies.Items)
            {
                lbNeutrals.Items.Add(enemyId);
                enemiesToRemove.Add(enemyId);
            }
            foreach (var enemyToRemove in enemiesToRemove)
            {
                lbEnemies.Items.Remove(enemyToRemove);
            }
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
            btnEnemyToNeutral.Enabled = false;
            UpdateMultiButtonsEnabled();
        }

        private void btnNeutralsToEnemies_Click(object sender, EventArgs e)
        {
            var neutralsToRemove = new List<string>();
            foreach (string neutralId in lbNeutrals.Items)
            {
                lbEnemies.Items.Add(neutralId);
                neutralsToRemove.Add(neutralId);
            }
            foreach (var neutralToRemove in neutralsToRemove)
            {
                lbNeutrals.Items.Remove(neutralToRemove);
            }
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
            btnNeutralToEnemy.Enabled = false;
            UpdateMultiButtonsEnabled();
        }

        private void UpdateMultiButtonsEnabled()
        {
            btnNeutralsToAllies.Enabled = lbNeutrals.Items.Count > 0;
            btnAlliesToNeutrals.Enabled = lbAllies.Items.Count > 0;
            btnEnemiesToNeutrals.Enabled = lbEnemies.Items.Count > 0;
            btnNeutralsToEnemies.Enabled = lbNeutrals.Items.Count > 0;
        }
    }
}
