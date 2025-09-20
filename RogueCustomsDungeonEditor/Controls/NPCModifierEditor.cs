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
using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsDungeonEditor.Controls
{
    public partial class NPCModifierEditor : UserControl
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
                NPCModifierStatsSheet.StatData = value.CharacterStats;
                cmbNPCModifierElementDamage.Items.Clear();
                cmbNPCModifierElementDamage.Items.AddRange(value.ElementInfos.Select(et => et.Id).ToArray());
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public NPCModifierInfo NPCModifier
        {
            get
            {
                var selectedAffectedItemTypes = new List<string>();

                return new()
                {
                    Id = txtNPCModifierId.Text,
                    Name = txtNPCModifierName.Text,
                    StatModifiers = NPCModifierStatsSheet.Stats,
                    ExtraDamage = new()
                    {
                        MinDamage = (int)nudNPCModifierMinDamage.Value,
                        MaxDamage = (int)nudNPCModifierMaxDamage.Value,
                        Element = cmbNPCModifierElementDamage.Text
                    },
                    OnSpawn = saeNPCModifierOnSpawn.Action,
                    OnTurnStart = saeNPCModifierOnTurnStart.Action,
                    OnAttacked = saeNPCModifierOnAttacked.Action,
                    OnAttack = saeNPCModifierOnAttack.Action,
                    OnDeath = saeNPCModifierOnDeath.Action,
                    NameColor = new GameColor(btnNPCModifierColor.BackColor)
                };
            }
            set
            {
                txtNPCModifierId.Text = value.Id;
                txtNPCModifierName.Text = value.Name;
                NPCModifierStatsSheet.Stats = value.StatModifiers ?? new List<PassiveStatModifierInfo>();
                nudNPCModifierMinDamage.Value = value.ExtraDamage?.MinDamage ?? 0;
                nudNPCModifierMaxDamage.Value = value.ExtraDamage?.MaxDamage ?? 0;
                cmbNPCModifierElementDamage.Text = value.ExtraDamage?.Element ?? string.Empty;
                SetSingleActionEditorParams(saeNPCModifierOnSpawn, value.Name, value.OnSpawn);
                SetSingleActionEditorParams(saeNPCModifierOnTurnStart, value.Name, value.OnTurnStart);
                SetSingleActionEditorParams(saeNPCModifierOnAttacked, value.Name, value.OnAttacked);
                SetSingleActionEditorParams(saeNPCModifierOnAttack, value.Name, value.OnAttack);
                SetSingleActionEditorParams(saeNPCModifierOnDeath, value.Name, value.OnDeath);
                btnNPCModifierColor.BackColor = value.NameColor != null ? value.NameColor.ToColor() : Color.White;
            }
        }
        public event EventHandler EditorInfoChanged;

        public NPCModifierEditor()
        {
            InitializeComponent();
        }

        public List<string> GetValidationErrors()
        {
            NPCModifierStatsSheet.EndEdit();
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(txtNPCModifierId.Text))
                errors.Add("The NPC Modifier must have an Id.");
            if (string.IsNullOrWhiteSpace(txtNPCModifierName.Text))
                errors.Add("The NPC Modifier must have a Name.");
            if (nudNPCModifierMinDamage.Value > nudNPCModifierMaxDamage.Value)
                errors.Add("The NPC Modifier has a Minimum Damage higher than its Maximum Damage.");
            if ((nudNPCModifierMinDamage.Value > 0 || nudNPCModifierMaxDamage.Value > 0) && string.IsNullOrWhiteSpace(cmbNPCModifierElementDamage.Text))
                errors.Add("The NPC Modifier has been set to deal damage but the Element wasn't specified.");
            if (nudNPCModifierMinDamage.Value == 0 && nudNPCModifierMaxDamage.Value == 0 && !string.IsNullOrWhiteSpace(cmbNPCModifierElementDamage.Text))
                errors.Add("The NPC Modifier has been set an Element but wasn't set to deal damage.");
            return errors;
        }

        private void txtNPCModifierId_TextChanged(object sender, EventArgs e)
        {
            EditorInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void txtNPCModifierName_TextChanged(object sender, EventArgs e)
        {
            txtNPCModifierName.ToggleEntryInLocaleWarning(_activeDungeon, fklblNPCModifierNameLocale);
            EditorInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void cmbNPCModifierType_SelectedIndexChanged(object sender, EventArgs e)
        {
            EditorInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void cmbNPCModifierType_TextChanged(object sender, EventArgs e)
        {
            EditorInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void nudNPCModifierMinimumItemLevel_ValueChanged(object sender, EventArgs e)
        {
            EditorInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void nudNPCModifierItemValuePercentageModifier_ValueChanged(object sender, EventArgs e)
        {
            EditorInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void nudNPCModifierMinDamage_ValueChanged(object sender, EventArgs e)
        {
            EditorInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void nudNPCModifierMaxDamage_ValueChanged(object sender, EventArgs e)
        {
            EditorInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void cmbNPCModifierElementDamage_SelectedIndexChanged(object sender, EventArgs e)
        {
            EditorInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void cmbNPCModifierElementDamage_TextChanged(object sender, EventArgs e)
        {
            EditorInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void btnNPCModifierColor_Click(object sender, EventArgs e)
        {
            (DialogResult Result, Color pickedColor) = ColorDialogHandler.Show(btnNPCModifierColor.BackColor);
            if (Result == DialogResult.OK)
            {
                btnNPCModifierColor.BackColor = pickedColor;
                EditorInfoChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        private void SetSingleActionEditorParams(SingleActionEditor sae, string classId, ActionWithEffectsInfo? action)
        {
            sae.Action = action;
            sae.ClassId = classId;
            sae.Dungeon = _activeDungeon;
            sae.EffectParamData = EffectParamData;
            sae.ActionContentsChanged += (_, _) => EditorInfoChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}
