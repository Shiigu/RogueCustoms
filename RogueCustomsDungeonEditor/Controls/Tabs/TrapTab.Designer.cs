namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    partial class TrapTab
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(TrapTab));
            saeTrapOnStepped = new SingleActionEditor();
            txtTrapPower = new System.Windows.Forms.TextBox();
            label113 = new System.Windows.Forms.Label();
            chkTrapStartsVisible = new System.Windows.Forms.CheckBox();
            label116 = new System.Windows.Forms.Label();
            fklblTrapDescriptionLocale = new System.Windows.Forms.Button();
            txtTrapDescription = new System.Windows.Forms.TextBox();
            label117 = new System.Windows.Forms.Label();
            fklblTrapNameLocale = new System.Windows.Forms.Button();
            txtTrapName = new System.Windows.Forms.TextBox();
            label118 = new System.Windows.Forms.Label();
            crsTrap = new ConsoleRepresentationSelector();
            SuspendLayout();
            // 
            // saeTrapOnStepped
            // 
            saeTrapOnStepped.Action = null;
            saeTrapOnStepped.ActionDescription = "When someone steps on it...";
            saeTrapOnStepped.ActionTypeText = "Stepped";
            saeTrapOnStepped.AutoSize = true;
            saeTrapOnStepped.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saeTrapOnStepped.ClassId = null;
            saeTrapOnStepped.Dungeon = null;
            saeTrapOnStepped.EffectParamData = null;
            saeTrapOnStepped.Location = new System.Drawing.Point(8, 269);
            saeTrapOnStepped.Name = "saeTrapOnStepped";
            saeTrapOnStepped.PlaceholderActionName = "Stepped";
            saeTrapOnStepped.RequiresActionName = false;
            saeTrapOnStepped.RequiresCondition = false;
            saeTrapOnStepped.RequiresDescription = false;
            saeTrapOnStepped.Size = new System.Drawing.Size(290, 32);
            saeTrapOnStepped.SourceDescription = "The trap";
            saeTrapOnStepped.TabIndex = 252;
            saeTrapOnStepped.TargetDescription = "Whoever steps on it";
            saeTrapOnStepped.ThisDescription = "The trap";
            saeTrapOnStepped.TurnEndCriteria = HelperForms.TurnEndCriteria.CannotEndTurn;
            saeTrapOnStepped.UsageCriteria = HelperForms.UsageCriteria.AnyTargetAnyTime;
            // 
            // txtTrapPower
            // 
            txtTrapPower.Location = new System.Drawing.Point(81, 206);
            txtTrapPower.Name = "txtTrapPower";
            txtTrapPower.Size = new System.Drawing.Size(150, 23);
            txtTrapPower.TabIndex = 250;
            txtTrapPower.Enter += txtTrapPower_Enter;
            txtTrapPower.Leave += txtTrapPower_Leave;
            // 
            // label113
            // 
            label113.AutoSize = true;
            label113.Location = new System.Drawing.Point(8, 212);
            label113.Name = "label113";
            label113.Size = new System.Drawing.Size(65, 15);
            label113.TabIndex = 249;
            label113.Text = "Trap Power";
            // 
            // chkTrapStartsVisible
            // 
            chkTrapStartsVisible.AutoSize = true;
            chkTrapStartsVisible.Location = new System.Drawing.Point(8, 244);
            chkTrapStartsVisible.Name = "chkTrapStartsVisible";
            chkTrapStartsVisible.Size = new System.Drawing.Size(102, 19);
            chkTrapStartsVisible.TabIndex = 248;
            chkTrapStartsVisible.Text = "Spawns visible";
            chkTrapStartsVisible.UseVisualStyleBackColor = true;
            chkTrapStartsVisible.CheckedChanged += chkTrapStartsVisible_CheckedChanged;
            // 
            // label116
            // 
            label116.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label116.Location = new System.Drawing.Point(385, 17);
            label116.Name = "label116";
            label116.Size = new System.Drawing.Size(131, 52);
            label116.TabIndex = 247;
            label116.Text = "Appearance";
            label116.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fklblTrapDescriptionLocale
            // 
            fklblTrapDescriptionLocale.Enabled = false;
            fklblTrapDescriptionLocale.FlatAppearance.BorderSize = 0;
            fklblTrapDescriptionLocale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblTrapDescriptionLocale.Image = (System.Drawing.Image)resources.GetObject("fklblTrapDescriptionLocale.Image");
            fklblTrapDescriptionLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblTrapDescriptionLocale.Location = new System.Drawing.Point(8, 150);
            fklblTrapDescriptionLocale.Name = "fklblTrapDescriptionLocale";
            fklblTrapDescriptionLocale.Size = new System.Drawing.Size(331, 42);
            fklblTrapDescriptionLocale.TabIndex = 246;
            fklblTrapDescriptionLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblTrapDescriptionLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblTrapDescriptionLocale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblTrapDescriptionLocale.UseVisualStyleBackColor = true;
            fklblTrapDescriptionLocale.Visible = false;
            // 
            // txtTrapDescription
            // 
            txtTrapDescription.Location = new System.Drawing.Point(8, 121);
            txtTrapDescription.Name = "txtTrapDescription";
            txtTrapDescription.Size = new System.Drawing.Size(350, 23);
            txtTrapDescription.TabIndex = 245;
            txtTrapDescription.TextChanged += txtTrapDescription_TextChanged;
            // 
            // label117
            // 
            label117.AutoSize = true;
            label117.Location = new System.Drawing.Point(8, 103);
            label117.Name = "label117";
            label117.Size = new System.Drawing.Size(67, 15);
            label117.TabIndex = 244;
            label117.Text = "Description";
            // 
            // fklblTrapNameLocale
            // 
            fklblTrapNameLocale.Enabled = false;
            fklblTrapNameLocale.FlatAppearance.BorderSize = 0;
            fklblTrapNameLocale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblTrapNameLocale.Image = (System.Drawing.Image)resources.GetObject("fklblTrapNameLocale.Image");
            fklblTrapNameLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblTrapNameLocale.Location = new System.Drawing.Point(8, 52);
            fklblTrapNameLocale.Name = "fklblTrapNameLocale";
            fklblTrapNameLocale.Size = new System.Drawing.Size(331, 42);
            fklblTrapNameLocale.TabIndex = 243;
            fklblTrapNameLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblTrapNameLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblTrapNameLocale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblTrapNameLocale.UseVisualStyleBackColor = true;
            fklblTrapNameLocale.Visible = false;
            // 
            // txtTrapName
            // 
            txtTrapName.Location = new System.Drawing.Point(8, 23);
            txtTrapName.Name = "txtTrapName";
            txtTrapName.Size = new System.Drawing.Size(350, 23);
            txtTrapName.TabIndex = 242;
            txtTrapName.TextChanged += txtTrapName_TextChanged;
            // 
            // label118
            // 
            label118.AutoSize = true;
            label118.Location = new System.Drawing.Point(8, 5);
            label118.Name = "label118";
            label118.Size = new System.Drawing.Size(80, 15);
            label118.TabIndex = 241;
            label118.Text = "Default Name";
            // 
            // crsTrap
            // 
            crsTrap.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("crsTrap.BackgroundColor");
            crsTrap.Character = '\0';
            crsTrap.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("crsTrap.ForegroundColor");
            crsTrap.Location = new System.Drawing.Point(519, 6);
            crsTrap.Name = "crsTrap";
            crsTrap.Size = new System.Drawing.Size(211, 83);
            crsTrap.TabIndex = 251;
            crsTrap.PropertyChanged += crsTrap_PropertyChanged;
            // 
            // TrapTab
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Controls.Add(saeTrapOnStepped);
            Controls.Add(txtTrapPower);
            Controls.Add(label113);
            Controls.Add(chkTrapStartsVisible);
            Controls.Add(label116);
            Controls.Add(fklblTrapDescriptionLocale);
            Controls.Add(txtTrapDescription);
            Controls.Add(label117);
            Controls.Add(fklblTrapNameLocale);
            Controls.Add(txtTrapName);
            Controls.Add(label118);
            Controls.Add(crsTrap);
            Name = "TrapTab";
            Size = new System.Drawing.Size(733, 304);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private SingleActionEditor saeTrapOnStepped;
        private System.Windows.Forms.TextBox txtTrapPower;
        private System.Windows.Forms.Label label113;
        private System.Windows.Forms.CheckBox chkTrapStartsVisible;
        private System.Windows.Forms.Label label116;
        private System.Windows.Forms.Button fklblTrapDescriptionLocale;
        private System.Windows.Forms.TextBox txtTrapDescription;
        private System.Windows.Forms.Label label117;
        private System.Windows.Forms.Button fklblTrapNameLocale;
        private System.Windows.Forms.TextBox txtTrapName;
        private System.Windows.Forms.Label label118;
        private ConsoleRepresentationSelector crsTrap;
    }
}
