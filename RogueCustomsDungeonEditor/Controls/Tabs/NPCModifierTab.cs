using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using RogueCustomsDungeonEditor.EffectInfos;
using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    public partial class NPCModifierTab : UserControl
    {
        private DungeonInfo _activeDungeon;

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
        public NPCModifierInfo LoadedNPCModifier { get; private set; }
        private List<EffectTypeData> EffectParamData;
        public event EventHandler TabInfoChanged;

        public NPCModifierTab()
        {
            InitializeComponent();
        }

        public void LoadData(NPCModifierInfo npcModifier, DungeonInfo activeDungeon, List<EffectTypeData> effectParamData)
        {
            ActiveDungeon = activeDungeon;
            LoadedNPCModifier = npcModifier ?? new();
            EffectParamData = effectParamData;
            txtNPCModifierName.Text = LoadedNPCModifier.Name;
            NPCModifierStatsSheet.Stats = LoadedNPCModifier.StatModifiers ?? new List<PassiveStatModifierInfo>();
            nudNPCModifierMinDamage.Value = LoadedNPCModifier.ExtraDamage?.MinDamage ?? 0;
            nudNPCModifierMaxDamage.Value = LoadedNPCModifier.ExtraDamage?.MaxDamage ?? 0;
            cmbNPCModifierElementDamage.Text = LoadedNPCModifier.ExtraDamage?.Element ?? string.Empty;
            saeNPCModifierOnSpawn.SetActionEditorParams(LoadedNPCModifier.Name, LoadedNPCModifier.OnSpawn, EffectParamData, activeDungeon, (_, _) => TabInfoChanged?.Invoke(null, EventArgs.Empty));
            saeNPCModifierOnTurnStart.SetActionEditorParams(LoadedNPCModifier.Name, LoadedNPCModifier.OnTurnStart, EffectParamData, activeDungeon, (_, _) => TabInfoChanged?.Invoke(null, EventArgs.Empty));
            saeNPCModifierOnAttacked.SetActionEditorParams(LoadedNPCModifier.Name, LoadedNPCModifier.OnAttacked, EffectParamData, activeDungeon, (_, _) => TabInfoChanged?.Invoke(null, EventArgs.Empty));
            saeNPCModifierOnAttack.SetActionEditorParams(LoadedNPCModifier.Name, LoadedNPCModifier.OnAttack, EffectParamData, activeDungeon, (_, _) => TabInfoChanged?.Invoke(null, EventArgs.Empty));
            saeNPCModifierOnDeath.SetActionEditorParams(LoadedNPCModifier.Name, LoadedNPCModifier.OnDeath, EffectParamData, activeDungeon, (_, _) => TabInfoChanged?.Invoke(null, EventArgs.Empty));
            btnNPCModifierColor.BackColor = LoadedNPCModifier.NameColor != null ? LoadedNPCModifier.NameColor.ToColor() : Color.White;
        }

        public List<string> SaveData(string id)
        {
            var validationErrors = new List<string>();

            if (validationErrors.Count == 0)
            {
                LoadedNPCModifier = new()
                {
                    Id = id,
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

            return validationErrors;
        }

        private void txtNPCModifierName_TextChanged(object sender, EventArgs e)
        {
            txtNPCModifierName.ToggleEntryInLocaleWarning(_activeDungeon, fklblNPCModifierNameLocale);
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void nudNPCModifierMinDamage_ValueChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void nudNPCModifierMaxDamage_ValueChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void cmbNPCModifierElementDamage_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void cmbNPCModifierElementDamage_TextChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void btnNPCModifierColor_Click(object sender, EventArgs e)
        {
            (DialogResult Result, Color pickedColor) = ColorDialogHandler.Show(btnNPCModifierColor.BackColor);
            if (Result == DialogResult.OK)
            {
                btnNPCModifierColor.BackColor = pickedColor;
                TabInfoChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        private void NPCModifierStatsSheet_StatsChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}
