using RogueCustomsDungeonEditor.Clipboard;
using RogueCustomsDungeonEditor.EffectInfos;
using RogueCustomsDungeonEditor.HelperForms;
using RogueCustomsDungeonEditor.Utils;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.Controls
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public partial class MultiActionEditor : UserControl
    {
        // For some weird reason, I need this because it otherwise loses all values on a Designer change.
        private string actionDescription = "MultiActionEditor";
        private List<ActionWithEffectsInfo> actions = new();
        private DungeonInfo dungeon = null;
        private string actionTypeText = "";
        private string classId = "";
        private bool requiresCondition = false;
        private bool requiresDescription = false;
        private bool requiresActionId = false;
        private bool requiresActionName = false;
        private TurnEndCriteria turnEndCriteria = TurnEndCriteria.CannotEndTurn;
        private string placeholderActionName = "Placeholder";
        private UsageCriteria usageCriteria = UsageCriteria.AnyTargetAnyTime;
        private List<EffectTypeData> effectParamData = null;
        private string thisDescription = "";
        private string sourceDescription = "";
        private string targetDescription = "";

        // Serializable in Designer (safe types)

        // Label text for the editor
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        [DefaultValue("MultiActionEditor")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string ActionDescription
        {
            get => actionDescription;
            set
            {
                actionDescription = value;
                lblDescription.Text = actionDescription;
            }
        }

        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string ActionTypeText
        {
            get => actionTypeText;
            set => actionTypeText = value;
        }

        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string ClassId
        {
            get => classId;
            set => classId = value;
        }

        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool RequiresCondition
        {
            get => requiresCondition;
            set => requiresCondition = value;
        }

        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool RequiresDescription
        {
            get => requiresDescription;
            set => requiresDescription = value;
        }

        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool RequiresActionId
        {
            get => requiresActionId;
            set => requiresActionId = value;
        }

        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool RequiresActionName
        {
            get => requiresActionName;
            set => requiresActionName = value;
        }

        [DefaultValue(TurnEndCriteria.CannotEndTurn)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public TurnEndCriteria TurnEndCriteria
        {
            get => turnEndCriteria;
            set => turnEndCriteria = value;
        }

        [DefaultValue("Placeholder")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string PlaceholderActionName
        {
            get => placeholderActionName;
            set => placeholderActionName = value;
        }

        [DefaultValue(UsageCriteria.AnyTargetAnyTime)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public UsageCriteria UsageCriteria
        {
            get => usageCriteria;
            set => usageCriteria = value;
        }

        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string ThisDescription
        {
            get => thisDescription;
            set => thisDescription = value;
        }

        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string SourceDescription
        {
            get => sourceDescription;
            set => sourceDescription = value;
        }

        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string TargetDescription
        {
            get => targetDescription;
            set => targetDescription = value;
        }

        // Hidden from Designer (complex runtime-only types)

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<ActionWithEffectsInfo> Actions
        {
            get
            {
                var actions = new List<ActionWithEffectsInfo>();
                foreach (ListBoxItem actionItem in lbActions.Items)
                {
                    if (actionItem.Tag is not ActionWithEffectsInfo actionToReturn) continue;
                    actions.Add(actionToReturn);
                }
                return actions;
            }
            set
            {
                lbActions.Items.Clear();
                foreach (var action in value ?? [])
                {
                    var actionItem = new ListBoxItem
                    {
                        Text = action.Id,
                        Tag = action
                    };
                    lbActions.Items.Add(actionItem);
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DungeonInfo Dungeon
        {
            get => dungeon;
            set => dungeon = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<EffectTypeData> EffectParamData
        {
            get => effectParamData;
            set => effectParamData = value;
        }

        public event EventHandler ActionContentsChanged;

        public MultiActionEditor()
        {
            InitializeComponent();
            ActionDescription = Name;
            ClipboardManager.ClipboardContentsChanged += ClipboardManager_ClipboardContentsChanged;
        }
        private void ClipboardManager_ClipboardContentsChanged(object? sender, EventArgs e)
        {
            btnPaste.Enabled = ClipboardManager.ContainsData(FormConstants.ActionClipboardKey);
        }

        private void lbActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnCopy.Enabled = lbActions.SelectedItem != null;
            btnEdit.Enabled = lbActions.SelectedItem != null;
            btnDelete.Enabled = lbActions.SelectedItem != null;
        }

        private void OpenActionEditScreen(bool isNewAction)
        {
            var action = new ActionWithEffectsInfo();
            if (!isNewAction)
            {
                if ((lbActions.SelectedItem as ListBoxItem)?.Tag is not ActionWithEffectsInfo itemTag) return;
                action = itemTag;
            }
            var frmActionEdit = new frmActionEdit(
                action,
                Dungeon,
                ClassId,
                ActionTypeText,
                RequiresCondition,
                RequiresActionId,
                RequiresDescription,
                RequiresActionName,
                TurnEndCriteria,
                PlaceholderActionName,
                UsageCriteria,
                EffectParamData,
                ThisDescription,
                SourceDescription,
                TargetDescription);
            frmActionEdit.ShowDialog();
            if (frmActionEdit.Saved)
            {
                if (frmActionEdit.IsNewAction && !string.IsNullOrWhiteSpace(frmActionEdit.ActionToSave?.Effect?.EffectName))
                {
                    lbActions.Items.Add(new ListBoxItem
                    {
                        Text = frmActionEdit.ActionToSave.Id,
                        Tag = frmActionEdit.ActionToSave
                    });
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(frmActionEdit.ActionToSave?.Effect?.EffectName))
                        (lbActions.SelectedItem as ListBoxItem).Tag = frmActionEdit.ActionToSave;
                    else
                        lbActions.Items.Remove(lbActions.SelectedItem);
                }
                ActionContentsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            OpenActionEditScreen(true);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            OpenActionEditScreen(false);
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (lbActions.SelectedItem == null) return;
            if ((lbActions.SelectedItem as ListBoxItem)?.Tag is not ActionWithEffectsInfo itemTag) return;
            ClipboardManager.Copy(FormConstants.ActionClipboardKey, itemTag);
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (!ClipboardManager.ContainsData(FormConstants.ActionClipboardKey)) return;
            var action = ClipboardManager.Paste<ActionWithEffectsInfo>(FormConstants.ActionClipboardKey);
            lbActions.Items.Add(new ListBoxItem
            {
                Text = action.Name,
                Tag = action
            });
            ActionContentsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lbActions.SelectedItem == null) return;
            if ((lbActions.SelectedItem as ListBoxItem)?.Tag is not ActionWithEffectsInfo action || action.IsNullOrEmpty()) return;
            var messageBoxResult = MessageBox.Show(
                    $"Do you want to delete the currently-selected {ActionTypeText} Action?\n\nNOTE: This operation is NOT reversible!",
                    "Delete Action",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                    );
            if (messageBoxResult == DialogResult.Yes)
            {
                ClipboardManager.RemoveData(FormConstants.ActionClipboardKey);
                lbActions.Items.Remove(lbActions.SelectedItem);
                btnCopy.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                ActionContentsChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
