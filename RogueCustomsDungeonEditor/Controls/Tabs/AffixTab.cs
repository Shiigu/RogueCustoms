using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using RogueCustomsDungeonEditor.EffectInfos;
using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Utils.JsonImports;


#pragma warning disable CA1416 // Validar la compatibilidad de la plataforma
namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    public partial class AffixTab : UserControl
    {
        private DungeonInfo _activeDungeon;
        private List<EffectTypeData> EffectParamData;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DungeonInfo ActiveDungeon
        {
            set
            {
                _activeDungeon = value;
                AffixStatsSheet.StatData = value.CharacterStats;
                clbAffixAffects.Items.Clear();
                clbAffixAffects.Items.AddRange(value.ItemTypeInfos.Select(it => it.Id).ToArray());
                cmbAffixElementDamage.Items.Clear();
                cmbAffixElementDamage.Items.AddRange(value.ElementInfos.Select(et => et.Id).ToArray());
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AffixInfo LoadedAffix { get; private set; }
        public event EventHandler TabInfoChanged;

        public AffixTab()
        {
            InitializeComponent();
        }

        public void LoadData(DungeonInfo dungeon, AffixInfo affix, List<EffectTypeData> effectParamData)
        {
            ActiveDungeon = dungeon;
            LoadedAffix = affix ?? new();
            EffectParamData = effectParamData;
            txtAffixName.Text = affix?.Name;
            cmbAffixType.Text = affix?.AffixType;
            nudAffixMinimumItemLevel.Value = affix.MinimumItemLevel;
            nudAffixItemValuePercentageModifier.Value = affix.ItemValueModifierPercentage;
            AffixStatsSheet.Stats = affix.StatModifiers ?? new List<PassiveStatModifierInfo>();
            nudAffixMinDamage.Value = affix.ExtraDamage?.MinDamage ?? 0;
            nudAffixMaxDamage.Value = affix.ExtraDamage?.MaxDamage ?? 0;
            nudAffixRequiredPlayerLevel.Value = affix.RequiredPlayerLevel;
            cmbAffixElementDamage.Text = affix.ExtraDamage?.Element ?? string.Empty;
            saeAffixOnTurnStart.SetActionEditorParams(affix.Name, affix.OnTurnStart, EffectParamData, dungeon, (_ , _) => TabInfoChanged?.Invoke(null, EventArgs.Empty));
            saeAffixOnAttacked.SetActionEditorParams(affix.Name, affix.OnAttacked, EffectParamData, dungeon, (_, _) => TabInfoChanged?.Invoke(null, EventArgs.Empty));
            saeAffixOnAttack.SetActionEditorParams(affix.Name, affix.OnAttack, EffectParamData, dungeon, (_, _) => TabInfoChanged?.Invoke(null, EventArgs.Empty));
            for (int i = 0; i < clbAffixAffects.Items.Count; i++)
            {
                clbAffixAffects.SetItemChecked(i, affix.AffectedItemTypes != null && affix.AffectedItemTypes.Contains(clbAffixAffects.Items[i].ToString()));
            }
        }

        public List<string> SaveData(string id)
        {
            AffixStatsSheet.EndEdit();

            var validationErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(txtAffixName.Text))
                validationErrors.Add("The Affix must have a Name.");
            if (string.IsNullOrWhiteSpace(cmbAffixType.Text))
                validationErrors.Add("The Affix must have a Type.");
            if (clbAffixAffects.CheckedItems.Count == 0)
                validationErrors.Add("The Affix must be able to spawn on at least one Item Type.");
            if (nudAffixMinDamage.Value > nudAffixMaxDamage.Value)
                validationErrors.Add("The Affix has a Minimum Damage higher than its Maximum Damage.");
            if ((nudAffixMinDamage.Value > 0 || nudAffixMaxDamage.Value > 0) && string.IsNullOrWhiteSpace(cmbAffixElementDamage.Text))
                validationErrors.Add("The Affix has been set to deal damage but the Element wasn't specified.");
            if (nudAffixMinDamage.Value == 0 && nudAffixMaxDamage.Value == 0 && !string.IsNullOrWhiteSpace(cmbAffixElementDamage.Text))
                validationErrors.Add("The Affix has been set an Element but wasn't set to deal damage.");

            var maximumPlayerLevel = _activeDungeon.PlayerClasses.Max(pl => pl.MaxLevel);

            if (nudAffixRequiredPlayerLevel.Value > maximumPlayerLevel)
                validationErrors.Add($"This Affix has a Required Player Level higher than the Dungeon's Maximum, {maximumPlayerLevel}.");

            if (validationErrors.Count == 0)
            {
                var selectedAffectedItemTypes = new List<string>();

                foreach (var item in clbAffixAffects.CheckedItems)
                {
                    selectedAffectedItemTypes.Add(item.ToString());
                }

                LoadedAffix = new()
                {
                    Id = id,
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
                    RequiredPlayerLevel = (int)nudAffixRequiredPlayerLevel.Value,
                    OnTurnStart = saeAffixOnTurnStart.Action,
                    OnAttacked = saeAffixOnAttacked.Action,
                    OnAttack = saeAffixOnAttack.Action
                };
            }

            return validationErrors;
        }

        private void txtAffixName_TextChanged(object sender, EventArgs e)
        {
            txtAffixName.ToggleEntryInLocaleWarning(_activeDungeon, fklblAffixNameLocale);
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void cmbAffixType_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void cmbAffixType_TextChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void nudAffixMinimumItemLevel_ValueChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void nudAffixItemValuePercentageModifier_ValueChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void nudAffixMinDamage_ValueChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void nudAffixMaxDamage_ValueChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void cmbAffixElementDamage_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void cmbAffixElementDamage_TextChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }
        private void clbAffixAffects_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void AffixStatsSheet_StatsChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void nudAffixRequiredPlayerLevel_ValueChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}
#pragma warning restore CA1416 // Validar la compatibilidad de la plataforma
