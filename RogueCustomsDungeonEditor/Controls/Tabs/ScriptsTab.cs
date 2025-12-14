using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using RogueCustomsDungeonEditor.EffectInfos;
using RogueCustomsDungeonEditor.HelperForms;
using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Utils.JsonImports;
#pragma warning disable CA1416 // Validar la compatibilidad de la plataforma

namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    public partial class ScriptsTab : UserControl
    {
        private DungeonInfo ActiveDungeon;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<ActionWithEffectsInfo> LoadedScripts { get; private set; }
        private List<EffectTypeData> EffectParamData;
        public event EventHandler TabInfoChanged;
        private int selectedRowIndex = -1;

        public ScriptsTab()
        {
            InitializeComponent();
        }

        public void LoadData(DungeonInfo activeDungeon, List<ActionWithEffectsInfo> scripts, List<EffectTypeData> effectParamData)
        {
            ActiveDungeon = activeDungeon;
            LoadedScripts = scripts ?? new();
            EffectParamData = effectParamData;

            tlpScripts.SuspendLayout();

            tlpScripts.Controls.Clear();
            tlpScripts.RowCount = 0;

            if (LoadedScripts.Count == 0)
            {
                lblNoScripts.Visible = true;
                tlpHeader.Visible = false;
                tlpScripts.Visible = false;
                tlpScriptsTab.Visible = false;
            }
            else
            {
                lblNoScripts.Visible = false;
                tlpHeader.Visible = true;
                tlpScripts.Visible = true;
                tlpScriptsTab.Visible = true;
            }

            for (int i = 0; i < LoadedScripts.Count; i++)
            {
                var script = LoadedScripts[i];

                AddRow(script.Id, script, i);
            }

            tlpScripts.ResumeLayout(true);
        }
        public List<string> SaveData()
        {
            var validationErrors = new List<string>();
            var scriptIds = new List<string>();
            var scripts = new List<ActionWithEffectsInfo>();

            for (int i = 0; i < tlpScripts.RowCount; i++)
            {
                var lblScriptId = (Label)(tlpScripts.GetControlFromPosition(0, i).Controls[0]);
                if (string.IsNullOrWhiteSpace(lblScriptId.Text) || lblScriptId.Text.Equals("<UNDEFINED>", StringComparison.InvariantCultureIgnoreCase))
                {
                    validationErrors.Add($"Script #{i} lacks an Id");
                }
                else
                {
                    var singleActionEditor = (SingleActionEditor)(tlpScripts.GetControlFromPosition(1, i).Controls[0]);
                    if (singleActionEditor.Action != null)
                    {
                        scriptIds.Add(lblScriptId.Text);
                        singleActionEditor.Action.IsScript = true;
                        scripts.Add(singleActionEditor.Action);
                    }
                    else
                    {
                        validationErrors.Add($"Script {lblScriptId.Text} lacks an Action");
                    }
                }
            }

            if (scriptIds.Distinct().Count() != scriptIds.Count)
                validationErrors.Add("At least two Scripts have the same Id");

            if (!validationErrors.Any())
            {
                LoadedScripts = scripts;
            }

            return validationErrors;
        }

        private void AddRow(string scriptId, ActionWithEffectsInfo script, int rowIndex)
        {
            tlpScripts.SuspendLayout();

            lblNoScripts.Visible = false;
            tlpHeader.Visible = true;
            tlpScripts.Visible = true;
            tlpScriptsTab.Visible = true;

            var idCellPanel = new Panel
            {
                Dock = DockStyle.Top,
                BackColor = Color.Transparent,
                Height = 48,
                Padding = new Padding(0, 0, 0, 0),
                Margin = new Padding(0, 0, 0, 0)
            };
            idCellPanel.Click += (_, _) =>
            {
                var position = tlpScripts.GetPositionFromControl(idCellPanel);
                SelectRow(position.Row);
            };

            var lblScriptId = new Label
            {
                Text = scriptId,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Font = new Font(DefaultFont.FontFamily, 14, FontStyle.Regular)
            };

            idCellPanel.Controls.Add(lblScriptId);

            var editorCellPanel = new Panel
            {
                Dock = DockStyle.Top,
                BackColor = Color.Transparent,
                Height = 48,
                Padding = new Padding(0, 0, 0, 0),
                Margin = new Padding(0, 0, 0, 0)
            };
            editorCellPanel.Click += (_, _) =>
            {
                var position = tlpScripts.GetPositionFromControl(editorCellPanel);
                SelectRow(position.Row);
            };

            var scriptEditor = new SingleActionEditor()
            {
                Tag = rowIndex,
            };
            SetSingleActionEditorParams(scriptEditor, script);
            editorCellPanel.Controls.Add(scriptEditor);
            scriptEditor.Top = editorCellPanel.Height / 2 - scriptEditor.Height / 2;
            scriptEditor.Left = editorCellPanel.Width / 2;

            lblScriptId.Click += (_, _) =>
            {
                var position = tlpScripts.GetPositionFromControl(idCellPanel);
                SelectRow(position.Row);
            };
            scriptEditor.Click += (_, _) =>
            {
                var position = tlpScripts.GetPositionFromControl(editorCellPanel);
                SelectRow(position.Row);
            };

            tlpScripts.RowCount++;

            foreach (RowStyle rowStyle in tlpScripts.RowStyles)
            {
                rowStyle.SizeType = SizeType.Absolute;
                rowStyle.Height = 48;
            }

            tlpScripts.Controls.Add(idCellPanel, 0, rowIndex);
            tlpScripts.Controls.Add(editorCellPanel, 1, rowIndex);

            tlpScripts.HorizontalScroll.Enabled = false;
            tlpScripts.HorizontalScroll.Visible = false;

            tlpScripts.ResumeLayout(true);
        }

        private void SetSingleActionEditorParams(SingleActionEditor sae, ActionWithEffectsInfo? action)
        {
            sae.Action = action;
            sae.ClassId = "Whoever calls this";
            sae.Dungeon = ActiveDungeon;
            sae.EffectParamData = EffectParamData;
            sae.UsageCriteria = UsageCriteria.FullConditions;
            sae.RequiresActionId = true;
            sae.RequiresCondition = true;
            sae.RequiresDescription = true;
            sae.RequiresName = true;
            sae.ActionContentsChanged += (_, _) =>
            {
                if (sae.Action != null)
                {
                    if (sae.IsNewAction)
                    {
                        AddRow(sae.Action.Id, sae.Action, tlpScripts.RowCount);
                        sae.Action = sae.ActionBeforeSave;
                    }
                    else
                    {
                        var lblScriptId = (Label)(tlpScripts.GetControlFromPosition(0, (int)sae.Tag).Controls[0]);
                        lblScriptId.Text = sae.Action.Id;
                    }
                }
                else
                {
                    btnRemoveScript.Enabled = false;
                    tlpScripts.RemoveRow((int)sae.Tag);
                    SelectRow(selectedRowIndex);
                    selectedRowIndex = -1;
                }
                if (tlpScripts.RowCount == 0)
                {
                    lblNoScripts.Visible = true;
                    tlpHeader.Visible = false;
                    tlpScripts.Visible = false;
                    tlpScriptsTab.Visible = false;
                }
                TabInfoChanged?.Invoke(null, EventArgs.Empty);
            };
            sae.ActionTypeText = "On Call";
            sae.ActionDescription = "";
            sae.ThisDescription = "Same as the Action that called this / Whoever learned it";
            sae.SourceDescription = "Same as the Action that called this / Whoever learned it";
            sae.TargetDescription = "Same as the Action that called this / Whoever is being targeted";
        }

        private void btnAddScript_Click(object sender, EventArgs e)
        {
            tlpScripts.SuspendLayout();

            SelectRow(selectedRowIndex);
            selectedRowIndex = -1;
            btnRemoveScript.Enabled = false;
            AddRow("<UNDEFINED>", null, tlpScripts.RowCount);
            TabInfoChanged?.Invoke(null, EventArgs.Empty);

            tlpScripts.ResumeLayout(true);
        }

        private void btnRemoveScript_Click(object sender, EventArgs e)
        {
            tlpScripts.SuspendLayout();

            btnRemoveScript.Enabled = false;
            tlpScripts.RemoveRow(selectedRowIndex);
            SelectRow(selectedRowIndex);
            selectedRowIndex = -1;
            if (tlpScripts.RowCount == 0)
            {
                lblNoScripts.Visible = true;
                tlpHeader.Visible = false;
                tlpScripts.Visible = false;
                tlpScriptsTab.Visible = false;
            }
            TabInfoChanged?.Invoke(null, EventArgs.Empty);

            tlpScripts.ResumeLayout(true);
        }

        private void SelectRow(int rowIndex)
        {
            if (selectedRowIndex != -1)
                SetRowBackgroundColor(selectedRowIndex, Color.Transparent);

            if (rowIndex != selectedRowIndex)
            {
                SetRowBackgroundColor(rowIndex, SystemColors.Highlight);
                selectedRowIndex = rowIndex;
                btnRemoveScript.Enabled = true;
            }
            else
            {
                btnRemoveScript.Enabled = false;
                selectedRowIndex = -1;
            }
        }

        private void SetRowBackgroundColor(int rowIndex, Color color)
        {
            for (int col = 0; col < tlpScripts.ColumnCount; col++)
            {
                var control = tlpScripts.GetControlFromPosition(col, rowIndex);
                if (control != null)
                {
                    control.BackColor = color;
                }
            }
        }
    }
}
#pragma warning restore CA1416 // Validar la compatibilidad de la plataforma
