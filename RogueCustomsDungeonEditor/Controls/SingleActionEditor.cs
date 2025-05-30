﻿using RogueCustomsDungeonEditor.Clipboard;
using RogueCustomsDungeonEditor.EffectInfos;
using RogueCustomsDungeonEditor.HelperForms;
using RogueCustomsDungeonEditor.Utils;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.Controls
{
    #pragma warning disable CS8604 // Posible argumento de referencia nulo
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public partial class SingleActionEditor : UserControl
    {
        private string actionDescription = "Action Description";
        private ActionWithEffectsInfo? action;

        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ActionDescription
        {
            get { return actionDescription; }
            set
            {
                actionDescription = value;
                lblDescription.Text = actionDescription;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ActionWithEffectsInfo? Action
        {
            get { return !action.IsNullOrEmpty() ? action : null; }
            set
            {
                action = value;
                btnCopy.Enabled = !action.IsNullOrEmpty();
                btnDelete.Enabled = !action.IsNullOrEmpty();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DungeonInfo Dungeon { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ActionTypeText { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ClassId { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RequiresActionId { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RequiresName { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RequiresDescription { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RequiresCondition { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TurnEndCriteria TurnEndCriteria { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string PlaceholderActionId { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public UsageCriteria UsageCriteria { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<EffectTypeData> EffectParamData { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ThisDescription { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SourceDescription { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TargetDescription { get; set; }

        public event EventHandler ActionContentsChanged;

        public SingleActionEditor()
        {
            InitializeComponent();
            ActionDescription = Name;
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
            var messageBoxResult = DialogResult.Yes;
            if (!action.IsNullOrEmpty())
            {
                messageBoxResult = MessageBox.Show(
                    $"Do you want to overwrite the currently-set Action?\n\nNOTE: This operation is NOT reversible!",
                    "Paste Action",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                    );
            }
            if (messageBoxResult == DialogResult.Yes)
            {
                Action = ClipboardManager.Paste<ActionWithEffectsInfo>(FormConstants.ActionClipboardKey);
                btnCopy.Enabled = true;
                ActionContentsChanged?.Invoke(this, EventArgs.Empty);
            }
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
