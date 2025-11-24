namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    partial class StatTab
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(StatTab));
            fklblStatNameLocale = new System.Windows.Forms.Button();
            txtStatName = new System.Windows.Forms.TextBox();
            label101 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            cmbStatType = new System.Windows.Forms.ComboBox();
            chkStatHasMax = new System.Windows.Forms.CheckBox();
            label2 = new System.Windows.Forms.Label();
            cmbStatRegenerationTarget = new System.Windows.Forms.ComboBox();
            SuspendLayout();
            // 
            // fklblStatNameLocale
            // 
            fklblStatNameLocale.Enabled = false;
            fklblStatNameLocale.FlatAppearance.BorderSize = 0;
            fklblStatNameLocale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblStatNameLocale.Image = (System.Drawing.Image)resources.GetObject("fklblStatNameLocale.Image");
            fklblStatNameLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblStatNameLocale.Location = new System.Drawing.Point(3, 52);
            fklblStatNameLocale.Name = "fklblStatNameLocale";
            fklblStatNameLocale.Size = new System.Drawing.Size(331, 42);
            fklblStatNameLocale.TabIndex = 226;
            fklblStatNameLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblStatNameLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblStatNameLocale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblStatNameLocale.UseVisualStyleBackColor = true;
            fklblStatNameLocale.Visible = false;
            // 
            // txtStatName
            // 
            txtStatName.Location = new System.Drawing.Point(3, 23);
            txtStatName.Name = "txtStatName";
            txtStatName.Size = new System.Drawing.Size(350, 23);
            txtStatName.TabIndex = 225;
            txtStatName.TextChanged += txtStatName_TextChanged;
            // 
            // label101
            // 
            label101.AutoSize = true;
            label101.Location = new System.Drawing.Point(3, 5);
            label101.Name = "label101";
            label101.Size = new System.Drawing.Size(39, 15);
            label101.TabIndex = 224;
            label101.Text = "Name";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(4, 110);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(32, 15);
            label1.TabIndex = 227;
            label1.Text = "Type";
            // 
            // cmbStatType
            // 
            cmbStatType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbStatType.FormattingEnabled = true;
            cmbStatType.Items.AddRange(new object[] { "Integer", "Decimal", "Percentage", "Regeneration" });
            cmbStatType.Location = new System.Drawing.Point(41, 107);
            cmbStatType.Name = "cmbStatType";
            cmbStatType.Size = new System.Drawing.Size(121, 23);
            cmbStatType.TabIndex = 228;
            cmbStatType.TextChanged += cmbStatType_TextChanged;
            // 
            // chkStatHasMax
            // 
            chkStatHasMax.AutoSize = true;
            chkStatHasMax.Location = new System.Drawing.Point(4, 172);
            chkStatHasMax.Name = "chkStatHasMax";
            chkStatHasMax.Size = new System.Drawing.Size(278, 19);
            chkStatHasMax.TabIndex = 229;
            chkStatHasMax.Text = "Has a current and a maximum value (e.g. 11/23)";
            chkStatHasMax.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(4, 143);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(170, 15);
            label2.TabIndex = 230;
            label2.Text = "Regenerates the following Stat:";
            // 
            // cmbStatRegenerationTarget
            // 
            cmbStatRegenerationTarget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbStatRegenerationTarget.FormattingEnabled = true;
            cmbStatRegenerationTarget.Location = new System.Drawing.Point(180, 140);
            cmbStatRegenerationTarget.Name = "cmbStatRegenerationTarget";
            cmbStatRegenerationTarget.Size = new System.Drawing.Size(121, 23);
            cmbStatRegenerationTarget.TabIndex = 231;
            // 
            // StatTab
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            Controls.Add(cmbStatRegenerationTarget);
            Controls.Add(label2);
            Controls.Add(chkStatHasMax);
            Controls.Add(cmbStatType);
            Controls.Add(label1);
            Controls.Add(fklblStatNameLocale);
            Controls.Add(txtStatName);
            Controls.Add(label101);
            Name = "StatTab";
            Size = new System.Drawing.Size(370, 194);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button fklblStatNameLocale;
        private System.Windows.Forms.TextBox txtStatName;
        private System.Windows.Forms.Label label101;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbStatType;
        private System.Windows.Forms.CheckBox chkStatHasMax;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbStatRegenerationTarget;
    }
}
