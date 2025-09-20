using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using RogueCustomsDungeonEditor.EffectInfos;
using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    public partial class ElementTab : UserControl
    {
        private DungeonInfo ActiveDungeon;
        private List<EffectTypeData> EffectParamData;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ElementInfo LoadedElement { get; private set; }
        public event EventHandler TabInfoChanged;

        public ElementTab()
        {
            InitializeComponent();
        }

        public void LoadData(ElementInfo elementToLoad, DungeonInfo activeDungeon, List<EffectTypeData> effectParamData)
        {
            ActiveDungeon = activeDungeon;
            LoadedElement = elementToLoad;
            EffectParamData = effectParamData;
            txtElementName.Text = elementToLoad.Name;
            cmbElementResistanceStat.Text = string.Empty;
            cmbElementResistanceStat.Items.Clear();
            foreach (var stat in activeDungeon.CharacterStats.Where(cs => !FormConstants.DefaultStats.Any(ms => ms.Equals(cs.Id, StringComparison.InvariantCultureIgnoreCase)) && cs.StatType.Equals("Percentage", StringComparison.InvariantCultureIgnoreCase)))
            {
                cmbElementResistanceStat.Items.Add(stat.Id);
            }
            if (cmbElementResistanceStat.Items.Cast<string>().Contains(elementToLoad.ResistanceStatId.ToString()))
                cmbElementResistanceStat.Text = elementToLoad.ResistanceStatId.ToString();
            btnElementColor.BackColor = elementToLoad.Color.ToColor();
            chkExcessResistanceCausesHealDamage.Checked = elementToLoad.ExcessResistanceCausesHealDamage;
            SetSingleActionEditorParams(saeElementOnAfterAttack, elementToLoad.Id, elementToLoad.OnAfterAttack);
        }

        public List<string> SaveData(string id)
        {
            var validationErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(txtElementName.Text))
                validationErrors.Add("The Stat lacks a Name");

            if(!string.IsNullOrWhiteSpace(cmbElementResistanceStat.Text))
            {
                var resistanceStat = ActiveDungeon.CharacterStats.Find(s => s.Id.Equals(cmbElementResistanceStat.Text, StringComparison.InvariantCultureIgnoreCase));

                if (resistanceStat == null)
                    validationErrors.Add($"The Element's Resistance Stat, {cmbElementResistanceStat.Text}, does not exist.");
                else if (!resistanceStat.StatType.Equals("Percentage", StringComparison.InvariantCultureIgnoreCase))
                    validationErrors.Add($"The Element's Resistance Stat, {cmbElementResistanceStat.Text}, is not of the Percentage type.");
            }

            if (!validationErrors.Any())
            {
                LoadedElement = new()
                {
                    Id = id,
                    Name = txtElementName.Text,
                    Color = new GameColor(btnElementColor.BackColor),
                    ResistanceStatId = cmbElementResistanceStat.Text,
                    ExcessResistanceCausesHealDamage = chkExcessResistanceCausesHealDamage.Checked,
                    OnAfterAttack = saeElementOnAfterAttack.Action
                };
                if (LoadedElement.OnAfterAttack != null)
                    LoadedElement.OnAfterAttack.IsScript = false;
            }

            return validationErrors;
        }
        private void SetSingleActionEditorParams(SingleActionEditor sae, string classId, ActionWithEffectsInfo? action)
        {
            sae.Action = action;
            sae.ClassId = classId;
            sae.Dungeon = ActiveDungeon;
            sae.EffectParamData = EffectParamData;
            sae.ActionContentsChanged += (_, _) => TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void txtElementName_TextChanged(object sender, EventArgs e)
        {
            txtElementName.ToggleEntryInLocaleWarning(ActiveDungeon, fklblElementNameLocale);
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void btnElementColor_Click(object sender, EventArgs e)
        {
            (DialogResult Result, Color pickedColor) = ColorDialogHandler.Show(btnElementColor.BackColor);
            if (Result == DialogResult.OK)
            {
                btnElementColor.BackColor = pickedColor;
                TabInfoChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        private void cmbElementResistanceStat_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
            if(string.IsNullOrWhiteSpace(cmbElementResistanceStat.Text))
            {
                chkExcessResistanceCausesHealDamage.Enabled = false;
                chkExcessResistanceCausesHealDamage.Checked = false;
            }
            else
            {
                chkExcessResistanceCausesHealDamage.Enabled = true;
            }
        }

        private void chkExcessResistanceCausesHealDamage_CheckedChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}
