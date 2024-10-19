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
            label98 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            nudStatMaxCap = new System.Windows.Forms.NumericUpDown();
            lblPercentage = new System.Windows.Forms.Label();
            nudStatMinCap = new System.Windows.Forms.NumericUpDown();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)nudStatMaxCap).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudStatMinCap).BeginInit();
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
            label1.Size = new System.Drawing.Size(31, 15);
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
            chkStatHasMax.Location = new System.Drawing.Point(375, 59);
            chkStatHasMax.Name = "chkStatHasMax";
            chkStatHasMax.Size = new System.Drawing.Size(279, 19);
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
            // label98
            // 
            label98.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label98.Location = new System.Drawing.Point(456, 4);
            label98.Name = "label98";
            label98.Size = new System.Drawing.Size(131, 52);
            label98.TabIndex = 239;
            label98.Text = "Values";
            label98.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label3.Location = new System.Drawing.Point(375, 115);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(60, 15);
            label3.TabIndex = 240;
            label3.Text = "Stat caps:";
            // 
            // nudStatMaxCap
            // 
            nudStatMaxCap.Location = new System.Drawing.Point(509, 132);
            nudStatMaxCap.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            nudStatMaxCap.Minimum = new decimal(new int[] { 9999, 0, 0, int.MinValue });
            nudStatMaxCap.Name = "nudStatMaxCap";
            nudStatMaxCap.Size = new System.Drawing.Size(78, 23);
            nudStatMaxCap.TabIndex = 255;
            nudStatMaxCap.ValueChanged += nudStatMaxCap_ValueChanged;
            // 
            // lblPercentage
            // 
            lblPercentage.AutoSize = true;
            lblPercentage.Location = new System.Drawing.Point(592, 104);
            lblPercentage.Name = "lblPercentage";
            lblPercentage.Size = new System.Drawing.Size(17, 45);
            lblPercentage.TabIndex = 254;
            lblPercentage.Text = "%\r\n\r\n%";
            // 
            // nudStatMinCap
            // 
            nudStatMinCap.Location = new System.Drawing.Point(509, 102);
            nudStatMinCap.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            nudStatMinCap.Minimum = new decimal(new int[] { 9999, 0, 0, int.MinValue });
            nudStatMinCap.Name = "nudStatMinCap";
            nudStatMinCap.Size = new System.Drawing.Size(78, 23);
            nudStatMinCap.TabIndex = 253;
            nudStatMinCap.ValueChanged += nudStatMinCap_ValueChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(443, 107);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(60, 15);
            label4.TabIndex = 256;
            label4.Text = "Minimum";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(443, 134);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(62, 15);
            label5.TabIndex = 257;
            label5.Text = "Maximum";
            // 
            // StatTab
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(nudStatMaxCap);
            Controls.Add(lblPercentage);
            Controls.Add(nudStatMinCap);
            Controls.Add(label3);
            Controls.Add(label98);
            Controls.Add(cmbStatRegenerationTarget);
            Controls.Add(label2);
            Controls.Add(chkStatHasMax);
            Controls.Add(cmbStatType);
            Controls.Add(label1);
            Controls.Add(fklblStatNameLocale);
            Controls.Add(txtStatName);
            Controls.Add(label101);
            Name = "StatTab";
            Size = new System.Drawing.Size(657, 166);
            ((System.ComponentModel.ISupportInitialize)nudStatMaxCap).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudStatMinCap).EndInit();
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
        private System.Windows.Forms.Label label98;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudStatMaxCap;
        private System.Windows.Forms.Label lblPercentage;
        private System.Windows.Forms.NumericUpDown nudStatMinCap;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}
