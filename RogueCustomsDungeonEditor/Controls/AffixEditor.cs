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

using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsDungeonEditor.Controls
{
    public partial class AffixEditor : UserControl
    {
        private List<EffectTypeData> effectParamData = null;
        private DungeonInfo _activeDungeon;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<EffectTypeData> EffectParamData
        {
            get => effectParamData;
            set => effectParamData = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DungeonInfo ActiveDungeon
        {
            set
            {
                _activeDungeon = value;
                AffixStatsSheet.StatData = value.CharacterStats;
                cmbAffixElementDamage.Items.Clear();
                cmbAffixElementDamage.Items.AddRange(value.ElementInfos.Select(et => et.Id).ToArray());
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AffixInfo Affix
        {
            get
            {
                var selectedAffectedItemTypes = new List<string>();

                foreach (var item in clbAffixAffects.CheckedItems)
                {
                    selectedAffectedItemTypes.Add(item.ToString());
                }

                return new()
                {
                    Id = txtAffixId.Text,
                    Name = txtAffixName.Text,
                    AffixType = cmbAffixType.Text,
                    AffectedItemTypes = selectedAffectedItemTypes,
                    MinimumItemLevel = (int)nudAffixMinimumItemLevel.Value,
                    StatModifiers = AffixStatsSheet.Stats,
                    ItemValueModifierPercentage = (int)nudAffixItemValuePercentageModifier.Value,
                    ExtraDamage = new()
                    {
                        MinDamage = (int)nudAffixMinDamage.Value,
                        MaxDamage = (int)nudAffixMaxDamage.Value,
                        Element = cmbAffixElementDamage.Text
                    },
                    OnTurnStart = saeAffixOnTurnStart.Action,
                    OnAttacked = saeAffixOnAttacked.Action,
                    OnAttack = saeAffixOnAttack.Action
                };
            }
            set
            {
                txtAffixId.Text = value.Id;
                txtAffixName.Text = value.Name;
                cmbAffixType.Text = value.AffixType;
                nudAffixMinimumItemLevel.Value = value.MinimumItemLevel;
                nudAffixItemValuePercentageModifier.Value = value.ItemValueModifierPercentage;
                AffixStatsSheet.Stats = value.StatModifiers ?? new List<PassiveStatModifierInfo>();
                nudAffixMinDamage.Value = value.ExtraDamage?.MinDamage ?? 0;
                nudAffixMaxDamage.Value = value.ExtraDamage?.MaxDamage ?? 0;
                cmbAffixElementDamage.Text = value.ExtraDamage?.Element ?? string.Empty;
                saeAffixOnTurnStart.Action = value.OnTurnStart;
                saeAffixOnAttacked.Action = value.OnAttacked;
                saeAffixOnAttack.Action = value.OnAttack;
                for (int i = 0; i < clbAffixAffects.Items.Count; i++)
                {
                    clbAffixAffects.SetItemChecked(i, value.AffectedItemTypes != null && value.AffectedItemTypes.Contains(clbAffixAffects.Items[i].ToString()));
                }
            }
        }
        public event EventHandler EditorInfoChanged;

        public AffixEditor()
        {
            InitializeComponent();
        }

        public List<string> GetValidationErrors()
        {
            AffixStatsSheet.EndEdit();
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(txtAffixId.Text))
                errors.Add("The Affix must have an Id.");
            if (string.IsNullOrWhiteSpace(txtAffixName.Text))
                errors.Add("The Affix must have a Name.");
            if (string.IsNullOrWhiteSpace(cmbAffixType.Text))
                errors.Add("The Affix must have a Type.");
            if (clbAffixAffects.CheckedItems.Count == 0)
                errors.Add("The Affix must affect at least one Affected Item Type.");
            if (nudAffixMinDamage.Value > nudAffixMaxDamage.Value)
                errors.Add("The Affix has a Minimum Damage higher than its Maximum Damage.");
            if ((nudAffixMinDamage.Value > 0 || nudAffixMaxDamage.Value > 0) && string.IsNullOrWhiteSpace(cmbAffixElementDamage.Text))
                errors.Add("The Affix has been set to deal damage but the Element wasn't specified.");
            if (nudAffixMinDamage.Value == 0 && nudAffixMaxDamage.Value == 0 && !string.IsNullOrWhiteSpace(cmbAffixElementDamage.Text))
                errors.Add("The Affix has been set an Element but wasn't set to deal damage.");
            return errors;
        }

        private void txtAffixId_TextChanged(object sender, EventArgs e)
        {
            EditorInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void txtAffixName_TextChanged(object sender, EventArgs e)
        {
            txtAffixName.ToggleEntryInLocaleWarning(_activeDungeon, fklblAffixNameLocale);
            EditorInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void cmbAffixType_SelectedIndexChanged(object sender, EventArgs e)
        {
            EditorInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void cmbAffixType_TextChanged(object sender, EventArgs e)
        {
            EditorInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void nudAffixMinimumItemLevel_ValueChanged(object sender, EventArgs e)
        {
            EditorInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void nudAffixItemValuePercentageModifier_ValueChanged(object sender, EventArgs e)
        {
            EditorInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void nudAffixMinDamage_ValueChanged(object sender, EventArgs e)
        {
            EditorInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void nudAffixMaxDamage_ValueChanged(object sender, EventArgs e)
        {
            EditorInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void cmbAffixElementDamage_SelectedIndexChanged(object sender, EventArgs e)
        {
            EditorInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void cmbAffixElementDamage_TextChanged(object sender, EventArgs e)
        {
            EditorInfoChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}
