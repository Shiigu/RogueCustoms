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

using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    public partial class NPCModifierTab : UserControl
    {
        private DungeonInfo ActiveDungeon;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<NPCModifierInfo> LoadedNPCModifiers { get; private set; }
        private List<EffectTypeData> EffectParamData;
        public event EventHandler TabInfoChanged;
        private int _selectedIndex;

        public NPCModifierTab()
        {
            InitializeComponent();
        }

        public void LoadData(DungeonInfo activeDungeon, List<EffectTypeData> effectParamData)
        {
            ActiveDungeon = activeDungeon;
            LoadedNPCModifiers = activeDungeon.NPCModifierInfos ?? new();
            EffectParamData = effectParamData;
            tlpNPCModifiers.SuspendLayout();
            tlpNPCModifiers.Controls.Clear();
            tlpNPCModifiers.RowCount = 0;
            foreach (var NPCModifier in LoadedNPCModifiers)
            {
                var NPCModifierEditor = new NPCModifierEditor()
                {
                    ActiveDungeon = ActiveDungeon,
                    EffectParamData = EffectParamData,
                    NPCModifier = NPCModifier,
                    Dock = DockStyle.Fill,
                };
                NPCModifierEditor.Click += NPCModifierEditor_Click;
                NPCModifierEditor.EditorInfoChanged += (sender, args) => TabInfoChanged?.Invoke(null, EventArgs.Empty);
                tlpNPCModifiers.RowCount++;
                tlpNPCModifiers.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                tlpNPCModifiers.Controls.Add(NPCModifierEditor, 0, tlpNPCModifiers.RowCount - 1);
            }
            tlpNPCModifiers.ResumeLayout(true);
            _selectedIndex = -1;
        }

        public List<string> SaveData()
        {
            var validationErrors = new List<string>();
            var NPCModifiersToSave = new List<NPCModifierInfo>();
            var NPCModifierIds = new List<string>();

            foreach (NPCModifierEditor editor in tlpNPCModifiers.Controls)
            {
                validationErrors.AddRange(editor.GetValidationErrors());
                if (!NPCModifierIds.Contains(editor.NPCModifier.Id))
                {
                    NPCModifierIds.Add(editor.NPCModifier.Id);
                    NPCModifiersToSave.Add(editor.NPCModifier);
                }
                else
                {
                    validationErrors.Add($"Duplicate NPC Modifier id '{editor.NPCModifier.Id}' found.");
                }
            }

            if (validationErrors.Count == 0)
            {
                LoadedNPCModifiers = NPCModifiersToSave;
            }

            return validationErrors;
        }

        private void NPCModifierEditor_Click(object? sender, EventArgs e)
        {
            if (sender is NPCModifierEditor clickedEditor)
            {
                foreach (NPCModifierEditor editor in tlpNPCModifiers.Controls)
                {
                    editor.BackColor = SystemColors.Control;
                    if (editor == clickedEditor)
                    {
                        _selectedIndex = tlpNPCModifiers.Controls.IndexOf(editor);
                    }
                }
                clickedEditor.BackColor = Color.LightBlue;
            }
        }

        private void btnAddNPCModifier_Click(object sender, EventArgs e)
        {
            tlpNPCModifiers.SuspendLayout();
            var NPCModifierEditor = new NPCModifierEditor()
            {
                Dock = DockStyle.Fill,
                EffectParamData = EffectParamData,
                ActiveDungeon = ActiveDungeon,
                NPCModifier = new()
            };
            foreach (NPCModifierEditor editor in tlpNPCModifiers.Controls)
            {
                editor.BackColor = SystemColors.Control;
            }
            _selectedIndex = -1;
            NPCModifierEditor.Click += NPCModifierEditor_Click;
            NPCModifierEditor.EditorInfoChanged += (sender, args) => TabInfoChanged?.Invoke(null, EventArgs.Empty);
            tlpNPCModifiers.RowCount++;
            tlpNPCModifiers.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpNPCModifiers.Controls.Add(NPCModifierEditor, 0, tlpNPCModifiers.RowCount - 1);
            tlpNPCModifiers.ResumeLayout(true);
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void btnRemoveNPCModifier_Click(object sender, EventArgs e)
        {
            if (_selectedIndex < 0 || _selectedIndex >= tlpNPCModifiers.RowCount) return;

            var control = tlpNPCModifiers.GetControlFromPosition(0, _selectedIndex);
            if (control != null)
            {
                tlpNPCModifiers.Controls.Remove(control);
                control.Dispose();

                for (int i = _selectedIndex + 1; i < tlpNPCModifiers.RowCount; i++)
                {
                    var c = tlpNPCModifiers.GetControlFromPosition(0, i);
                    if (c != null)
                    {
                        tlpNPCModifiers.SetRow(c, i - 1);
                    }
                }

                tlpNPCModifiers.RowCount--;
                if (tlpNPCModifiers.RowStyles.Count > _selectedIndex)
                {
                    tlpNPCModifiers.RowStyles.RemoveAt(_selectedIndex);
                }

                _selectedIndex = -1;
                TabInfoChanged?.Invoke(null, EventArgs.Empty);
            }
        }
    }
}
