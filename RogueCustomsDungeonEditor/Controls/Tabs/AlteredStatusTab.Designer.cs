namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    partial class AlteredStatusTab
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(AlteredStatusTab));
            saeAlteredStatusOnAttacked = new SingleActionEditor();
            saeAlteredStatusBeforeAttack = new SingleActionEditor();
            saeAlteredStatusOnRemove = new SingleActionEditor();
            saeAlteredStatusOnTurnStart = new SingleActionEditor();
            saeAlteredStatusOnApply = new SingleActionEditor();
            chkAlteredStatusCleansedOnCleanseActions = new System.Windows.Forms.CheckBox();
            chkAlteredStatusCleanseOnFloorChange = new System.Windows.Forms.CheckBox();
            chkAlteredStatusCanOverwrite = new System.Windows.Forms.CheckBox();
            chkAlteredStatusCanStack = new System.Windows.Forms.CheckBox();
            label111 = new System.Windows.Forms.Label();
            fklblAlteredStatusDescriptionLocale = new System.Windows.Forms.Button();
            txtAlteredStatusDescription = new System.Windows.Forms.TextBox();
            label114 = new System.Windows.Forms.Label();
            fklblAlteredStatusNameLocale = new System.Windows.Forms.Button();
            txtAlteredStatusName = new System.Windows.Forms.TextBox();
            label115 = new System.Windows.Forms.Label();
            crsAlteredStatus = new ConsoleRepresentationSelector();
            SuspendLayout();
            // 
            // saeAlteredStatusOnAttacked
            // 
            saeAlteredStatusOnAttacked.Action = null;
            saeAlteredStatusOnAttacked.ActionDescription = "When someone afflicted  \r\nby it is attacked...";
            saeAlteredStatusOnAttacked.ActionTypeText = "On Statused Attacked";
            saeAlteredStatusOnAttacked.AutoSize = true;
            saeAlteredStatusOnAttacked.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saeAlteredStatusOnAttacked.ClassId = null;
            saeAlteredStatusOnAttacked.Dungeon = null;
            saeAlteredStatusOnAttacked.EffectParamData = null;
            saeAlteredStatusOnAttacked.Location = new System.Drawing.Point(386, 263);
            saeAlteredStatusOnAttacked.Name = "saeAlteredStatusOnAttacked";
            saeAlteredStatusOnAttacked.PlaceholderActionId = "OnAttacked";
            saeAlteredStatusOnAttacked.RequiresCondition = false;
            saeAlteredStatusOnAttacked.Size = new System.Drawing.Size(276, 32);
            saeAlteredStatusOnAttacked.SourceDescription = "Whoever it's inflicting";
            saeAlteredStatusOnAttacked.TabIndex = 282;
            saeAlteredStatusOnAttacked.TargetDescription = "Whoever attacked them";
            saeAlteredStatusOnAttacked.ThisDescription = "The Altered Status";
            saeAlteredStatusOnAttacked.TurnEndCriteria = HelperForms.TurnEndCriteria.CannotEndTurn;
            saeAlteredStatusOnAttacked.UsageCriteria = HelperForms.UsageCriteria.AnyTargetAnyTime;
            // 
            // saeAlteredStatusBeforeAttack
            // 
            saeAlteredStatusBeforeAttack.Action = null;
            saeAlteredStatusBeforeAttack.ActionDescription = "When someone afflicted\r\nby it is about to attack...   ";
            saeAlteredStatusBeforeAttack.ActionTypeText = "Before Statused Attack";
            saeAlteredStatusBeforeAttack.AutoSize = true;
            saeAlteredStatusBeforeAttack.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saeAlteredStatusBeforeAttack.ClassId = null;
            saeAlteredStatusBeforeAttack.Dungeon = null;
            saeAlteredStatusBeforeAttack.EffectParamData = null;
            saeAlteredStatusBeforeAttack.Location = new System.Drawing.Point(386, 226);
            saeAlteredStatusBeforeAttack.Name = "saeAlteredStatusBeforeAttack";
            saeAlteredStatusBeforeAttack.PlaceholderActionId = "BeforeAttack";
            saeAlteredStatusBeforeAttack.RequiresCondition = false;
            saeAlteredStatusBeforeAttack.Size = new System.Drawing.Size(276, 32);
            saeAlteredStatusBeforeAttack.SourceDescription = "Whoever it's inflicting";
            saeAlteredStatusBeforeAttack.TabIndex = 281;
            saeAlteredStatusBeforeAttack.TargetDescription = "Whoever is being targeted";
            saeAlteredStatusBeforeAttack.ThisDescription = "The Altered Status";
            saeAlteredStatusBeforeAttack.TurnEndCriteria = HelperForms.TurnEndCriteria.CannotEndTurn;
            saeAlteredStatusBeforeAttack.UsageCriteria = HelperForms.UsageCriteria.AnyTargetAnyTime;
            // 
            // saeAlteredStatusOnRemove
            // 
            saeAlteredStatusOnRemove.Action = null;
            saeAlteredStatusOnRemove.ActionDescription = "When someone gets this\r\nAltered Status removed... ";
            saeAlteredStatusOnRemove.ActionTypeText = "On Status Remove";
            saeAlteredStatusOnRemove.AutoSize = true;
            saeAlteredStatusOnRemove.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saeAlteredStatusOnRemove.ClassId = null;
            saeAlteredStatusOnRemove.Dungeon = null;
            saeAlteredStatusOnRemove.EffectParamData = null;
            saeAlteredStatusOnRemove.Location = new System.Drawing.Point(386, 150);
            saeAlteredStatusOnRemove.Name = "saeAlteredStatusOnRemove";
            saeAlteredStatusOnRemove.PlaceholderActionId = "OnRemove";
            saeAlteredStatusOnRemove.RequiresCondition = false;
            saeAlteredStatusOnRemove.Size = new System.Drawing.Size(276, 32);
            saeAlteredStatusOnRemove.SourceDescription = "The Altered Status";
            saeAlteredStatusOnRemove.TabIndex = 280;
            saeAlteredStatusOnRemove.TargetDescription = "Whoever it's targeting";
            saeAlteredStatusOnRemove.ThisDescription = "The Altered Status";
            saeAlteredStatusOnRemove.TurnEndCriteria = HelperForms.TurnEndCriteria.CannotEndTurn;
            saeAlteredStatusOnRemove.UsageCriteria = HelperForms.UsageCriteria.AnyTargetAnyTime;
            // 
            // saeAlteredStatusOnTurnStart
            // 
            saeAlteredStatusOnTurnStart.Action = null;
            saeAlteredStatusOnTurnStart.ActionDescription = "When someone afflicted\r\nby it begins a new turn...  ";
            saeAlteredStatusOnTurnStart.ActionTypeText = "On Statused Turn Start";
            saeAlteredStatusOnTurnStart.AutoSize = true;
            saeAlteredStatusOnTurnStart.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saeAlteredStatusOnTurnStart.ClassId = null;
            saeAlteredStatusOnTurnStart.Dungeon = null;
            saeAlteredStatusOnTurnStart.EffectParamData = null;
            saeAlteredStatusOnTurnStart.Location = new System.Drawing.Point(386, 188);
            saeAlteredStatusOnTurnStart.Name = "saeAlteredStatusOnTurnStart";
            saeAlteredStatusOnTurnStart.PlaceholderActionId = "TurnStart";
            saeAlteredStatusOnTurnStart.RequiresCondition = false;
            saeAlteredStatusOnTurnStart.Size = new System.Drawing.Size(276, 32);
            saeAlteredStatusOnTurnStart.SourceDescription = "The Altered Status";
            saeAlteredStatusOnTurnStart.TabIndex = 279;
            saeAlteredStatusOnTurnStart.TargetDescription = "Whoever it's inflicting";
            saeAlteredStatusOnTurnStart.ThisDescription = "The Altered Status";
            saeAlteredStatusOnTurnStart.TurnEndCriteria = HelperForms.TurnEndCriteria.CannotEndTurn;
            saeAlteredStatusOnTurnStart.UsageCriteria = HelperForms.UsageCriteria.AnyTarget;
            // 
            // saeAlteredStatusOnApply
            // 
            saeAlteredStatusOnApply.Action = null;
            saeAlteredStatusOnApply.ActionDescription = "When someone gets this\r\nAltered Status inflicted...  ";
            saeAlteredStatusOnApply.ActionTypeText = "On Status Apply";
            saeAlteredStatusOnApply.AutoSize = true;
            saeAlteredStatusOnApply.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saeAlteredStatusOnApply.ClassId = null;
            saeAlteredStatusOnApply.Dungeon = null;
            saeAlteredStatusOnApply.EffectParamData = null;
            saeAlteredStatusOnApply.Location = new System.Drawing.Point(387, 112);
            saeAlteredStatusOnApply.Name = "saeAlteredStatusOnApply";
            saeAlteredStatusOnApply.PlaceholderActionId = "StatusApply";
            saeAlteredStatusOnApply.RequiresCondition = false;
            saeAlteredStatusOnApply.Size = new System.Drawing.Size(275, 32);
            saeAlteredStatusOnApply.SourceDescription = "The Altered Status";
            saeAlteredStatusOnApply.TabIndex = 278;
            saeAlteredStatusOnApply.TargetDescription = "Whoever it's targeting";
            saeAlteredStatusOnApply.ThisDescription = "The Altered Status";
            saeAlteredStatusOnApply.TurnEndCriteria = HelperForms.TurnEndCriteria.CannotEndTurn;
            saeAlteredStatusOnApply.UsageCriteria = HelperForms.UsageCriteria.AnyTargetAnyTime;
            // 
            // chkAlteredStatusCleansedOnCleanseActions
            // 
            chkAlteredStatusCleansedOnCleanseActions.AutoSize = true;
            chkAlteredStatusCleansedOnCleanseActions.Location = new System.Drawing.Point(8, 288);
            chkAlteredStatusCleansedOnCleanseActions.Name = "chkAlteredStatusCleansedOnCleanseActions";
            chkAlteredStatusCleansedOnCleanseActions.Size = new System.Drawing.Size(247, 19);
            chkAlteredStatusCleansedOnCleanseActions.TabIndex = 276;
            chkAlteredStatusCleansedOnCleanseActions.Text = "Can be removed by 'Cleanse' Action steps";
            chkAlteredStatusCleansedOnCleanseActions.UseVisualStyleBackColor = true;
            chkAlteredStatusCleansedOnCleanseActions.CheckedChanged += chkAlteredStatusCleansedOnCleanseActions_CheckedChanged;
            // 
            // chkAlteredStatusCleanseOnFloorChange
            // 
            chkAlteredStatusCleanseOnFloorChange.AutoSize = true;
            chkAlteredStatusCleanseOnFloorChange.Location = new System.Drawing.Point(8, 263);
            chkAlteredStatusCleanseOnFloorChange.Name = "chkAlteredStatusCleanseOnFloorChange";
            chkAlteredStatusCleanseOnFloorChange.Size = new System.Drawing.Size(330, 19);
            chkAlteredStatusCleanseOnFloorChange.TabIndex = 275;
            chkAlteredStatusCleanseOnFloorChange.Text = "Is removed if the afflicted Character moves to a new Floor";
            chkAlteredStatusCleanseOnFloorChange.UseVisualStyleBackColor = true;
            chkAlteredStatusCleanseOnFloorChange.CheckedChanged += chkAlteredStatusCleanseOnFloorChange_CheckedChanged;
            // 
            // chkAlteredStatusCanOverwrite
            // 
            chkAlteredStatusCanOverwrite.AutoSize = true;
            chkAlteredStatusCanOverwrite.Location = new System.Drawing.Point(8, 238);
            chkAlteredStatusCanOverwrite.Name = "chkAlteredStatusCanOverwrite";
            chkAlteredStatusCanOverwrite.Size = new System.Drawing.Size(342, 19);
            chkAlteredStatusCanOverwrite.TabIndex = 274;
            chkAlteredStatusCanOverwrite.Text = "Overwrites other Altered Statuses with the same Id if applied";
            chkAlteredStatusCanOverwrite.UseVisualStyleBackColor = true;
            chkAlteredStatusCanOverwrite.CheckedChanged += chkAlteredStatusCanOverwrite_CheckedChanged;
            // 
            // chkAlteredStatusCanStack
            // 
            chkAlteredStatusCanStack.AutoSize = true;
            chkAlteredStatusCanStack.Location = new System.Drawing.Point(8, 213);
            chkAlteredStatusCanStack.Name = "chkAlteredStatusCanStack";
            chkAlteredStatusCanStack.Size = new System.Drawing.Size(311, 19);
            chkAlteredStatusCanStack.TabIndex = 273;
            chkAlteredStatusCanStack.Text = "Can stack with other Altered Statuses with the same Id";
            chkAlteredStatusCanStack.UseVisualStyleBackColor = true;
            chkAlteredStatusCanStack.CheckedChanged += chkAlteredStatusCanStack_CheckedChanged;
            // 
            // label111
            // 
            label111.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label111.Location = new System.Drawing.Point(385, 17);
            label111.Name = "label111";
            label111.Size = new System.Drawing.Size(131, 52);
            label111.TabIndex = 272;
            label111.Text = "Appearance";
            label111.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fklblAlteredStatusDescriptionLocale
            // 
            fklblAlteredStatusDescriptionLocale.Enabled = false;
            fklblAlteredStatusDescriptionLocale.FlatAppearance.BorderSize = 0;
            fklblAlteredStatusDescriptionLocale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblAlteredStatusDescriptionLocale.Image = (System.Drawing.Image)resources.GetObject("fklblAlteredStatusDescriptionLocale.Image");
            fklblAlteredStatusDescriptionLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblAlteredStatusDescriptionLocale.Location = new System.Drawing.Point(8, 150);
            fklblAlteredStatusDescriptionLocale.Name = "fklblAlteredStatusDescriptionLocale";
            fklblAlteredStatusDescriptionLocale.Size = new System.Drawing.Size(331, 42);
            fklblAlteredStatusDescriptionLocale.TabIndex = 271;
            fklblAlteredStatusDescriptionLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblAlteredStatusDescriptionLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblAlteredStatusDescriptionLocale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblAlteredStatusDescriptionLocale.UseVisualStyleBackColor = true;
            fklblAlteredStatusDescriptionLocale.Visible = false;
            // 
            // txtAlteredStatusDescription
            // 
            txtAlteredStatusDescription.Location = new System.Drawing.Point(8, 121);
            txtAlteredStatusDescription.Name = "txtAlteredStatusDescription";
            txtAlteredStatusDescription.Size = new System.Drawing.Size(350, 23);
            txtAlteredStatusDescription.TabIndex = 270;
            txtAlteredStatusDescription.TextChanged += txtAlteredStatusDescription_TextChanged;
            // 
            // label114
            // 
            label114.AutoSize = true;
            label114.Location = new System.Drawing.Point(8, 103);
            label114.Name = "label114";
            label114.Size = new System.Drawing.Size(67, 15);
            label114.TabIndex = 269;
            label114.Text = "Description";
            // 
            // fklblAlteredStatusNameLocale
            // 
            fklblAlteredStatusNameLocale.Enabled = false;
            fklblAlteredStatusNameLocale.FlatAppearance.BorderSize = 0;
            fklblAlteredStatusNameLocale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblAlteredStatusNameLocale.Image = (System.Drawing.Image)resources.GetObject("fklblAlteredStatusNameLocale.Image");
            fklblAlteredStatusNameLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblAlteredStatusNameLocale.Location = new System.Drawing.Point(8, 52);
            fklblAlteredStatusNameLocale.Name = "fklblAlteredStatusNameLocale";
            fklblAlteredStatusNameLocale.Size = new System.Drawing.Size(331, 42);
            fklblAlteredStatusNameLocale.TabIndex = 268;
            fklblAlteredStatusNameLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblAlteredStatusNameLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblAlteredStatusNameLocale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblAlteredStatusNameLocale.UseVisualStyleBackColor = true;
            fklblAlteredStatusNameLocale.Visible = false;
            // 
            // txtAlteredStatusName
            // 
            txtAlteredStatusName.Location = new System.Drawing.Point(8, 23);
            txtAlteredStatusName.Name = "txtAlteredStatusName";
            txtAlteredStatusName.Size = new System.Drawing.Size(350, 23);
            txtAlteredStatusName.TabIndex = 267;
            txtAlteredStatusName.TextChanged += txtAlteredStatusName_TextChanged;
            // 
            // label115
            // 
            label115.AutoSize = true;
            label115.Location = new System.Drawing.Point(8, 5);
            label115.Name = "label115";
            label115.Size = new System.Drawing.Size(80, 15);
            label115.TabIndex = 266;
            label115.Text = "Default Name";
            // 
            // crsAlteredStatus
            // 
            crsAlteredStatus.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("crsAlteredStatus.BackgroundColor");
            crsAlteredStatus.Character = '\0';
            crsAlteredStatus.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("crsAlteredStatus.ForegroundColor");
            crsAlteredStatus.Location = new System.Drawing.Point(519, 6);
            crsAlteredStatus.Name = "crsAlteredStatus";
            crsAlteredStatus.Size = new System.Drawing.Size(211, 83);
            crsAlteredStatus.TabIndex = 277;
            crsAlteredStatus.PropertyChanged += crsAlteredStatus_PropertyChanged;
            // 
            // AlteredStatusTab
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Controls.Add(saeAlteredStatusOnAttacked);
            Controls.Add(saeAlteredStatusBeforeAttack);
            Controls.Add(saeAlteredStatusOnRemove);
            Controls.Add(saeAlteredStatusOnTurnStart);
            Controls.Add(saeAlteredStatusOnApply);
            Controls.Add(chkAlteredStatusCleansedOnCleanseActions);
            Controls.Add(chkAlteredStatusCleanseOnFloorChange);
            Controls.Add(chkAlteredStatusCanOverwrite);
            Controls.Add(chkAlteredStatusCanStack);
            Controls.Add(label111);
            Controls.Add(fklblAlteredStatusDescriptionLocale);
            Controls.Add(txtAlteredStatusDescription);
            Controls.Add(label114);
            Controls.Add(fklblAlteredStatusNameLocale);
            Controls.Add(txtAlteredStatusName);
            Controls.Add(label115);
            Controls.Add(crsAlteredStatus);
            Name = "AlteredStatusTab";
            Size = new System.Drawing.Size(733, 310);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private SingleActionEditor saeAlteredStatusOnAttacked;
        private SingleActionEditor saeAlteredStatusBeforeAttack;
        private SingleActionEditor saeAlteredStatusOnRemove;
        private SingleActionEditor saeAlteredStatusOnTurnStart;
        private SingleActionEditor saeAlteredStatusOnApply;
        private System.Windows.Forms.CheckBox chkAlteredStatusCleansedOnCleanseActions;
        private System.Windows.Forms.CheckBox chkAlteredStatusCleanseOnFloorChange;
        private System.Windows.Forms.CheckBox chkAlteredStatusCanOverwrite;
        private System.Windows.Forms.CheckBox chkAlteredStatusCanStack;
        private System.Windows.Forms.Label label111;
        private System.Windows.Forms.Button fklblAlteredStatusDescriptionLocale;
        private System.Windows.Forms.TextBox txtAlteredStatusDescription;
        private System.Windows.Forms.Label label114;
        private System.Windows.Forms.Button fklblAlteredStatusNameLocale;
        private System.Windows.Forms.TextBox txtAlteredStatusName;
        private System.Windows.Forms.Label label115;
        private ConsoleRepresentationSelector crsAlteredStatus;
    }
}
