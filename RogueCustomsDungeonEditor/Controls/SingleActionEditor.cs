using RogueCustomsDungeonEditor.Clipboard;
using RogueCustomsDungeonEditor.EffectInfos;
using RogueCustomsDungeonEditor.HelperForms;
using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Utils.JsonImports;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.Controls
{
#pragma warning disable CS8604 // Posible argumento de referencia nulo
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public partial class SingleActionEditor : UserControl
    {
        // For some weird reason, I need this because it otherwise loses all values on a Designer change.
        private string actionDescription = "SingleActionEditor";
        private ActionWithEffectsInfo? action = null;
        private DungeonInfo dungeon = null;
        private string actionTypeText = "";
        private string classId = "";
        private bool requiresActionId = false;
        private bool requiresName = false;
        private bool requiresDescription = false;
        private bool requiresCondition = false;
        private TurnEndCriteria turnEndCriteria = TurnEndCriteria.CannotEndTurn;
        private string placeholderActionId = "Placeholder";
        private UsageCriteria usageCriteria = UsageCriteria.AnyTargetAnyTime;
        private List<EffectTypeData> effectParamData = null;
        private string thisDescription = "";
        private string sourceDescription = "";
        private string targetDescription = "";

        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        [DefaultValue("SingleActionEditor")]
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
        public bool RequiresActionId
        {
            get => requiresActionId;
            set => requiresActionId = value;
        }

        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool RequiresName
        {
            get => requiresName;
            set => requiresName = value;
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
        public bool RequiresCondition
        {
            get => requiresCondition;
            set => requiresCondition = value;
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
        public string PlaceholderActionId
        {
            get => placeholderActionId;
            set => placeholderActionId = value;
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ActionWithEffectsInfo? Action
        {
            get => !action.IsNullOrEmpty() ? action : null;
            set
            {
                action = value;
                btnCopy.Enabled = !action.IsNullOrEmpty();
                btnDelete.Enabled = !action.IsNullOrEmpty();
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

        public SingleActionEditor()
        {
            InitializeComponent();
            ActionDescription = Name;
            btnPaste.Enabled = ClipboardManager.ContainsData(FormConstants.ActionClipboardKey);
            ClipboardManager.ClipboardContentsChanged += ClipboardManager_ClipboardContentsChanged;
        }

        private void ClipboardManager_ClipboardContentsChanged(object? sender, EventArgs e)
        {
            btnPaste.Enabled = ClipboardManager.ContainsData(FormConstants.ActionClipboardKey);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var frmActionEdit = new frmActionEdit(
                action,
                Dungeon,
                ClassId,
                ActionTypeText,
                RequiresCondition,
                RequiresActionId,
                RequiresDescription,
                RequiresName,
                TurnEndCriteria,
                PlaceholderActionId,
                UsageCriteria,
                EffectParamData,
                ThisDescription,
                SourceDescription,
                TargetDescription);
            frmActionEdit.ShowDialog();
            if (frmActionEdit.Saved)
            {
                Action = frmActionEdit.ActionToSave;
                btnCopy.Enabled = true;
                ActionContentsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (Action.IsNullOrEmpty()) return;
            ClipboardManager.Copy(FormConstants.ActionClipboardKey, action);
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (!ClipboardManager.ContainsData(FormConstants.ActionClipboardKey)) return;
            if (!action.IsNullOrEmpty())
            {
                var messageBoxResult = MessageBox.Show(
                    $"Do you want to overwrite the currently-set Action?\n\nNOTE: This operation is NOT reversible!",
                    "Paste Action",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                    );
                if (messageBoxResult != DialogResult.Yes) return;
            }
            Action = ClipboardManager.Paste<ActionWithEffectsInfo>(FormConstants.ActionClipboardKey);
            btnCopy.Enabled = true;
            ActionContentsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (Action.IsNullOrEmpty()) return;
            var messageBoxResult = MessageBox.Show(
                    $"Do you want to delete the currently-set {ActionTypeText} Action?\n\nNOTE: This operation is NOT reversible!",
                    "Delete Action",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                    );
            if (messageBoxResult == DialogResult.Yes)
            {
                ClipboardManager.RemoveData(FormConstants.ActionClipboardKey);
                Action = null;
                btnCopy.Enabled = false;
                ActionContentsChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    #pragma warning restore CS8604 // Posible argumento de referencia nulo
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
