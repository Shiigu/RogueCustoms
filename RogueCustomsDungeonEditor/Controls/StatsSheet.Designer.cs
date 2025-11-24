namespace RogueCustomsDungeonEditor.Controls
{
    partial class StatsSheet
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
            label68 = new System.Windows.Forms.Label();
            chkCanGainExperience = new System.Windows.Forms.CheckBox();
            nudMaxLevel = new System.Windows.Forms.NumericUpDown();
            label78 = new System.Windows.Forms.Label();
            txtLevelUpFormula = new System.Windows.Forms.TextBox();
            label79 = new System.Windows.Forms.Label();
            label80 = new System.Windows.Forms.Label();
            nudFlatSightRange = new System.Windows.Forms.NumericUpDown();
            cmbSightRange = new System.Windows.Forms.ComboBox();
            label81 = new System.Windows.Forms.Label();
            label96 = new System.Windows.Forms.Label();
            lblSightRangeText = new System.Windows.Forms.Label();
            lblSightRangeId = new System.Windows.Forms.Label();
            dgvStats = new System.Windows.Forms.DataGridView();
            btnCopyStats = new System.Windows.Forms.Button();
            btnPasteStats = new System.Windows.Forms.Button();
            Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Used = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            Base = new System.Windows.Forms.DataGridViewTextBoxColumn();
            IncreasePerLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Minimum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Maximum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)nudMaxLevel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudFlatSightRange).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvStats).BeginInit();
            SuspendLayout();
            // 
            // label68
            // 
            label68.AutoSize = true;
            label68.Location = new System.Drawing.Point(55, 332);
            label68.Name = "label68";
            label68.Size = new System.Drawing.Size(293, 30);
            label68.TabIndex = 245;
            label68.Text = "NOTE: When not The Whole Map, Sight is significantly\r\nreduced in hallways";
            label68.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkCanGainExperience
            // 
            chkCanGainExperience.AutoSize = true;
            chkCanGainExperience.Location = new System.Drawing.Point(55, 416);
            chkCanGainExperience.Name = "chkCanGainExperience";
            chkCanGainExperience.Size = new System.Drawing.Size(168, 19);
            chkCanGainExperience.TabIndex = 241;
            chkCanGainExperience.Text = "Can gain Experience Points";
            chkCanGainExperience.UseVisualStyleBackColor = true;
            // 
            // nudMaxLevel
            // 
            nudMaxLevel.Location = new System.Drawing.Point(161, 468);
            nudMaxLevel.Name = "nudMaxLevel";
            nudMaxLevel.Size = new System.Drawing.Size(44, 23);
            nudMaxLevel.TabIndex = 240;
            nudMaxLevel.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // label78
            // 
            label78.AutoSize = true;
            label78.Location = new System.Drawing.Point(52, 471);
            label78.Name = "label78";
            label78.Size = new System.Drawing.Size(102, 15);
            label78.TabIndex = 239;
            label78.Text = "Maximum Level is";
            // 
            // txtLevelUpFormula
            // 
            txtLevelUpFormula.Location = new System.Drawing.Point(182, 440);
            txtLevelUpFormula.Name = "txtLevelUpFormula";
            txtLevelUpFormula.Size = new System.Drawing.Size(163, 23);
            txtLevelUpFormula.TabIndex = 238;
            txtLevelUpFormula.Enter += txtLevelUpFormula_Enter;
            txtLevelUpFormula.Leave += txtLevelUpFormula_Leave;
            // 
            // label79
            // 
            label79.AutoSize = true;
            label79.Location = new System.Drawing.Point(52, 443);
            label79.Name = "label79";
            label79.Size = new System.Drawing.Size(125, 15);
            label79.TabIndex = 237;
            label79.Text = "Experience to Level Up";
            // 
            // label80
            // 
            label80.AutoSize = true;
            label80.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            label80.Location = new System.Drawing.Point(127, 381);
            label80.Name = "label80";
            label80.Size = new System.Drawing.Size(148, 32);
            label80.TabIndex = 236;
            label80.Text = "Leveling Up";
            // 
            // nudFlatSightRange
            // 
            nudFlatSightRange.Location = new System.Drawing.Point(241, 298);
            nudFlatSightRange.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            nudFlatSightRange.Name = "nudFlatSightRange";
            nudFlatSightRange.Size = new System.Drawing.Size(54, 23);
            nudFlatSightRange.TabIndex = 234;
            nudFlatSightRange.Visible = false;
            // 
            // cmbSightRange
            // 
            cmbSightRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbSightRange.FormattingEnabled = true;
            cmbSightRange.Location = new System.Drawing.Point(105, 298);
            cmbSightRange.Name = "cmbSightRange";
            cmbSightRange.Size = new System.Drawing.Size(121, 23);
            cmbSightRange.TabIndex = 233;
            cmbSightRange.SelectedIndexChanged += cmbSightRange_SelectedIndexChanged;
            // 
            // label81
            // 
            label81.AutoSize = true;
            label81.Location = new System.Drawing.Point(55, 301);
            label81.Name = "label81";
            label81.Size = new System.Drawing.Size(48, 15);
            label81.TabIndex = 232;
            label81.Text = "Can see";
            // 
            // label96
            // 
            label96.AutoSize = true;
            label96.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            label96.Location = new System.Drawing.Point(174, 0);
            label96.Name = "label96";
            label96.Size = new System.Drawing.Size(68, 32);
            label96.TabIndex = 207;
            label96.Text = "Stats";
            // 
            // lblSightRangeText
            // 
            lblSightRangeText.AutoSize = true;
            lblSightRangeText.Location = new System.Drawing.Point(301, 301);
            lblSightRangeText.Name = "lblSightRangeText";
            lblSightRangeText.Size = new System.Drawing.Size(28, 15);
            lblSightRangeText.TabIndex = 235;
            lblSightRangeText.Text = "tiles";
            lblSightRangeText.Visible = false;
            // 
            // lblSightRangeId
            // 
            lblSightRangeId.AutoSize = true;
            lblSightRangeId.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblSightRangeId.Location = new System.Drawing.Point(143, 266);
            lblSightRangeId.Name = "lblSightRangeId";
            lblSightRangeId.Size = new System.Drawing.Size(102, 21);
            lblSightRangeId.TabIndex = 254;
            lblSightRangeId.Text = "Sight Range";
            // 
            // dgvStats
            // 
            dgvStats.AllowUserToAddRows = false;
            dgvStats.AllowUserToDeleteRows = false;
            dgvStats.AllowUserToResizeColumns = false;
            dgvStats.AllowUserToResizeRows = false;
            dgvStats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvStats.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { Id, Used, Base, IncreasePerLevel, Minimum, Maximum });
            dgvStats.Location = new System.Drawing.Point(3, 35);
            dgvStats.Name = "dgvStats";
            dgvStats.Size = new System.Drawing.Size(399, 192);
            dgvStats.TabIndex = 255;
            dgvStats.CellBeginEdit += dgvStats_CellBeginEdit;
            dgvStats.CellValueChanged += dgvStats_CellValueChanged;
            dgvStats.CurrentCellDirtyStateChanged += dgvStats_CurrentCellDirtyStateChanged;
            // 
            // btnCopyStats
            // 
            btnCopyStats.Location = new System.Drawing.Point(127, 233);
            btnCopyStats.Name = "btnCopyStats";
            btnCopyStats.Size = new System.Drawing.Size(75, 23);
            btnCopyStats.TabIndex = 256;
            btnCopyStats.Text = "Copy";
            btnCopyStats.UseVisualStyleBackColor = true;
            btnCopyStats.Click += btnCopyStats_Click;
            // 
            // btnPasteStats
            // 
            btnPasteStats.Location = new System.Drawing.Point(208, 233);
            btnPasteStats.Name = "btnPasteStats";
            btnPasteStats.Size = new System.Drawing.Size(75, 23);
            btnPasteStats.TabIndex = 257;
            btnPasteStats.Text = "Paste";
            btnPasteStats.UseVisualStyleBackColor = true;
            btnPasteStats.Click += btnPasteStats_Click;
            // 
            // Id
            // 
            Id.Frozen = true;
            Id.HeaderText = "Stat Id";
            Id.Name = "Id";
            Id.ReadOnly = true;
            // 
            // Used
            // 
            Used.HeaderText = "Used?";
            Used.Name = "Used";
            Used.Width = 45;
            // 
            // Base
            // 
            Base.HeaderText = "Base";
            Base.Name = "Base";
            Base.Width = 50;
            // 
            // IncreasePerLevel
            // 
            IncreasePerLevel.HeaderText = "Per Level...";
            IncreasePerLevel.Name = "IncreasePerLevel";
            IncreasePerLevel.Width = 86;
            // 
            // Minimum
            // 
            Minimum.HeaderText = "Minimum";
            Minimum.Name = "Minimum";
            Minimum.Width = 70;
            // 
            // Maximum
            // 
            Maximum.HeaderText = "Maximum";
            Maximum.Name = "Maximum";
            Maximum.Width = 70;
            // 
            // StatsSheet
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(btnPasteStats);
            Controls.Add(btnCopyStats);
            Controls.Add(dgvStats);
            Controls.Add(lblSightRangeId);
            Controls.Add(label68);
            Controls.Add(chkCanGainExperience);
            Controls.Add(nudMaxLevel);
            Controls.Add(label78);
            Controls.Add(txtLevelUpFormula);
            Controls.Add(label79);
            Controls.Add(label80);
            Controls.Add(nudFlatSightRange);
            Controls.Add(cmbSightRange);
            Controls.Add(label81);
            Controls.Add(label96);
            Controls.Add(lblSightRangeText);
            Name = "StatsSheet";
            Size = new System.Drawing.Size(405, 508);
            ((System.ComponentModel.ISupportInitialize)nudMaxLevel).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudFlatSightRange).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvStats).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label label68;
        private System.Windows.Forms.CheckBox chkCanGainExperience;
        private System.Windows.Forms.NumericUpDown nudMaxLevel;
        private System.Windows.Forms.Label label78;
        private System.Windows.Forms.TextBox txtLevelUpFormula;
        private System.Windows.Forms.Label label79;
        private System.Windows.Forms.Label label80;
        private System.Windows.Forms.NumericUpDown nudFlatSightRange;
        private System.Windows.Forms.ComboBox cmbSightRange;
        private System.Windows.Forms.Label label81;
        private System.Windows.Forms.Label label96;
        private System.Windows.Forms.Label lblSightRangeText;
        private System.Windows.Forms.Label lblSightRangeId;
        private System.Windows.Forms.DataGridView dgvStats;
        private System.Windows.Forms.Button btnCopyStats;
        private System.Windows.Forms.Button btnPasteStats;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Used;
        private System.Windows.Forms.DataGridViewTextBoxColumn Base;
        private System.Windows.Forms.DataGridViewTextBoxColumn IncreasePerLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Minimum;
        private System.Windows.Forms.DataGridViewTextBoxColumn Maximum;
    }
}
