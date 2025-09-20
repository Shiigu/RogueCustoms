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
    public partial class AffixTab : UserControl
    {
        private DungeonInfo ActiveDungeon;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<AffixInfo> LoadedAffixes { get; private set; }
        private List<EffectTypeData> EffectParamData;
        public event EventHandler TabInfoChanged;
        private int _selectedIndex;

        public AffixTab()
        {
            InitializeComponent();
        }

        public void LoadData(DungeonInfo activeDungeon, List<EffectTypeData> effectParamData)
        {
            ActiveDungeon = activeDungeon;
            LoadedAffixes = activeDungeon.AffixInfos ?? new();
            EffectParamData = effectParamData;
            tlpAffixes.SuspendLayout();
            tlpAffixes.Controls.Clear();
            tlpAffixes.RowCount = 0;
            foreach (var affix in LoadedAffixes)
            {
                var affixEditor = new AffixEditor()
                {
                    ActiveDungeon = ActiveDungeon,
                    EffectParamData = EffectParamData,
                    Affix = affix,
                    Dock = DockStyle.Fill,
                };
                affixEditor.Click += AffixEditor_Click;
                affixEditor.EditorInfoChanged += (sender, args) => TabInfoChanged?.Invoke(null, EventArgs.Empty);
                tlpAffixes.RowCount++;
                tlpAffixes.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                tlpAffixes.Controls.Add(affixEditor, 0, tlpAffixes.RowCount - 1);
            }
            tlpAffixes.ResumeLayout(true);
            _selectedIndex = -1;
        }

        public List<string> SaveData()
        {
            var validationErrors = new List<string>();
            var affixesToSave = new List<AffixInfo>();
            var affixIds = new List<string>();

            foreach (AffixEditor editor in tlpAffixes.Controls)
            {
                validationErrors.AddRange(editor.GetValidationErrors());
                if (!affixIds.Contains(editor.Affix.Id))
                {
                    affixIds.Add(editor.Affix.Id);
                    affixesToSave.Add(editor.Affix);
                }
                else
                {
                    validationErrors.Add($"Duplicate affix id '{editor.Affix.Id}' found.");
                }
            }

            if (validationErrors.Count == 0)
            {
                LoadedAffixes = affixesToSave;
            }

            return validationErrors;
        }

        private void AffixEditor_Click(object? sender, EventArgs e)
        {
            if (sender is AffixEditor clickedEditor)
            {
                foreach (AffixEditor editor in tlpAffixes.Controls)
                {
                    editor.BackColor = SystemColors.Control;
                    if (editor == clickedEditor)
                    {
                        _selectedIndex = tlpAffixes.Controls.IndexOf(editor);
                    }
                }
                clickedEditor.BackColor = Color.LightBlue;
            }
        }

        private void btnAddAffix_Click(object sender, EventArgs e)
        {
            tlpAffixes.SuspendLayout();
            var affixEditor = new AffixEditor()
            {
                Dock = DockStyle.Fill,
                EffectParamData = EffectParamData,
                ActiveDungeon = ActiveDungeon,
                Affix = new()
                {
                    MinimumItemLevel = 1
                }
            };
            foreach (AffixEditor editor in tlpAffixes.Controls)
            {
                editor.BackColor = SystemColors.Control;
            }
            _selectedIndex = -1;
            affixEditor.Click += AffixEditor_Click;
            affixEditor.EditorInfoChanged += (sender, args) => TabInfoChanged?.Invoke(null, EventArgs.Empty);
            tlpAffixes.RowCount++;
            tlpAffixes.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpAffixes.Controls.Add(affixEditor, 0, tlpAffixes.RowCount - 1);
            tlpAffixes.ResumeLayout(true);
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void btnRemoveAffix_Click(object sender, EventArgs e)
        {
            if (_selectedIndex < 0 || _selectedIndex >= tlpAffixes.RowCount) return;

            var control = tlpAffixes.GetControlFromPosition(0, _selectedIndex);
            if (control != null)
            {
                tlpAffixes.Controls.Remove(control);
                control.Dispose();

                for (int i = _selectedIndex + 1; i < tlpAffixes.RowCount; i++)
                {
                    var c = tlpAffixes.GetControlFromPosition(0, i);
                    if (c != null)
                    {
                        tlpAffixes.SetRow(c, i - 1);
                    }
                }

                tlpAffixes.RowCount--;
                if (tlpAffixes.RowStyles.Count > _selectedIndex)
                {
                    tlpAffixes.RowStyles.RemoveAt(_selectedIndex);
                }

                _selectedIndex = -1;
                TabInfoChanged?.Invoke(null, EventArgs.Empty);
            }
        }
    }
}
