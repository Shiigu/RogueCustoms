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
            hsbStats = new System.Windows.Forms.HScrollBar();
            chkIsUsed = new System.Windows.Forms.CheckBox();
            label1 = new System.Windows.Forms.Label();
            nudBase = new System.Windows.Forms.NumericUpDown();
            lblPercentage = new System.Windows.Forms.Label();
            nudIncreasePerLevel = new System.Windows.Forms.NumericUpDown();
            label3 = new System.Windows.Forms.Label();
            lblStatId = new System.Windows.Forms.Label();
            lblSightRangeId = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)nudMaxLevel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudFlatSightRange).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudBase).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudIncreasePerLevel).BeginInit();
            SuspendLayout();
            // 
            // label68
            // 
            label68.AutoSize = true;
            label68.Location = new System.Drawing.Point(9, 246);
            label68.Name = "label68";
            label68.Size = new System.Drawing.Size(291, 30);
            label68.TabIndex = 245;
            label68.Text = "NOTE: When not The Whole Map, Sight is significantly\r\nreduced in hallways";
            label68.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkCanGainExperience
            // 
            chkCanGainExperience.AutoSize = true;
            chkCanGainExperience.Location = new System.Drawing.Point(9, 330);
            chkCanGainExperience.Name = "chkCanGainExperience";
            chkCanGainExperience.Size = new System.Drawing.Size(169, 19);
            chkCanGainExperience.TabIndex = 241;
            chkCanGainExperience.Text = "Can gain Experience Points";
            chkCanGainExperience.UseVisualStyleBackColor = true;
            // 
            // nudMaxLevel
            // 
            nudMaxLevel.Location = new System.Drawing.Point(115, 382);
            nudMaxLevel.Name = "nudMaxLevel";
            nudMaxLevel.Size = new System.Drawing.Size(44, 23);
            nudMaxLevel.TabIndex = 240;
            nudMaxLevel.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // label78
            // 
            label78.AutoSize = true;
            label78.Location = new System.Drawing.Point(6, 385);
            label78.Name = "label78";
            label78.Size = new System.Drawing.Size(103, 15);
            label78.TabIndex = 239;
            label78.Text = "Maximum Level is";
            // 
            // txtLevelUpFormula
            // 
            txtLevelUpFormula.Location = new System.Drawing.Point(136, 354);
            txtLevelUpFormula.Name = "txtLevelUpFormula";
            txtLevelUpFormula.Size = new System.Drawing.Size(163, 23);
            txtLevelUpFormula.TabIndex = 238;
            txtLevelUpFormula.Enter += txtLevelUpFormula_Enter;
            txtLevelUpFormula.Leave += txtLevelUpFormula_Leave;
            // 
            // label79
            // 
            label79.AutoSize = true;
            label79.Location = new System.Drawing.Point(6, 357);
            label79.Name = "label79";
            label79.Size = new System.Drawing.Size(126, 15);
            label79.TabIndex = 237;
            label79.Text = "Experience to Level Up";
            // 
            // label80
            // 
            label80.AutoSize = true;
            label80.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label80.Location = new System.Drawing.Point(81, 295);
            label80.Name = "label80";
            label80.Size = new System.Drawing.Size(148, 32);
            label80.TabIndex = 236;
            label80.Text = "Leveling Up";
            // 
            // nudFlatSightRange
            // 
            nudFlatSightRange.Location = new System.Drawing.Point(195, 212);
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
            cmbSightRange.Location = new System.Drawing.Point(59, 212);
            cmbSightRange.Name = "cmbSightRange";
            cmbSightRange.Size = new System.Drawing.Size(121, 23);
            cmbSightRange.TabIndex = 233;
            cmbSightRange.SelectedIndexChanged += cmbSightRange_SelectedIndexChanged;
            // 
            // label81
            // 
            label81.AutoSize = true;
            label81.Location = new System.Drawing.Point(9, 215);
            label81.Name = "label81";
            label81.Size = new System.Drawing.Size(48, 15);
            label81.TabIndex = 232;
            label81.Text = "Can see";
            // 
            // label96
            // 
            label96.AutoSize = true;
            label96.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label96.Location = new System.Drawing.Point(113, 0);
            label96.Name = "label96";
            label96.Size = new System.Drawing.Size(68, 32);
            label96.TabIndex = 207;
            label96.Text = "Stats";
            // 
            // lblSightRangeText
            // 
            lblSightRangeText.AutoSize = true;
            lblSightRangeText.Location = new System.Drawing.Point(255, 215);
            lblSightRangeText.Name = "lblSightRangeText";
            lblSightRangeText.Size = new System.Drawing.Size(28, 15);
            lblSightRangeText.TabIndex = 235;
            lblSightRangeText.Text = "tiles";
            lblSightRangeText.Visible = false;
            // 
            // hsbStats
            // 
            hsbStats.LargeChange = 1;
            hsbStats.Location = new System.Drawing.Point(8, 32);
            hsbStats.Name = "hsbStats";
            hsbStats.RightToLeft = System.Windows.Forms.RightToLeft.No;
            hsbStats.Size = new System.Drawing.Size(290, 17);
            hsbStats.TabIndex = 246;
            hsbStats.ValueChanged += hsbStats_ValueChanged;
            hsbStats.MouseEnter += hsbStats_MouseEnter;
            // 
            // chkIsUsed
            // 
            chkIsUsed.AutoSize = true;
            chkIsUsed.Location = new System.Drawing.Point(8, 89);
            chkIsUsed.Name = "chkIsUsed";
            chkIsUsed.Size = new System.Drawing.Size(63, 19);
            chkIsUsed.TabIndex = 247;
            chkIsUsed.Text = "Is Used";
            chkIsUsed.UseVisualStyleBackColor = true;
            chkIsUsed.CheckedChanged += chkIsUsed_CheckedChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(8, 120);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(115, 15);
            label1.TabIndex = 248;
            label1.Text = "At Level 1, it starts at";
            // 
            // nudBase
            // 
            nudBase.Location = new System.Drawing.Point(159, 118);
            nudBase.Name = "nudBase";
            nudBase.Size = new System.Drawing.Size(80, 23);
            nudBase.TabIndex = 249;
            nudBase.Leave += nudBase_Leave;
            // 
            // lblPercentage
            // 
            lblPercentage.AutoSize = true;
            lblPercentage.Location = new System.Drawing.Point(245, 120);
            lblPercentage.Name = "lblPercentage";
            lblPercentage.Size = new System.Drawing.Size(17, 45);
            lblPercentage.TabIndex = 250;
            lblPercentage.Text = "%\r\n\r\n%";
            // 
            // nudIncreasePerLevel
            // 
            nudIncreasePerLevel.DecimalPlaces = 5;
            nudIncreasePerLevel.Location = new System.Drawing.Point(159, 148);
            nudIncreasePerLevel.Name = "nudIncreasePerLevel";
            nudIncreasePerLevel.Size = new System.Drawing.Size(80, 23);
            nudIncreasePerLevel.TabIndex = 252;
            nudIncreasePerLevel.Leave += nudIncreasePerLevel_Leave;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(8, 150);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(145, 15);
            label3.TabIndex = 251;
            label3.Text = "Leveling up increases it by";
            // 
            // lblStatId
            // 
            lblStatId.AutoSize = true;
            lblStatId.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblStatId.Location = new System.Drawing.Point(104, 61);
            lblStatId.Name = "lblStatId";
            lblStatId.Size = new System.Drawing.Size(86, 21);
            lblStatId.TabIndex = 253;
            lblStatId.Text = "StatName";
            // 
            // lblSightRangeId
            // 
            lblSightRangeId.AutoSize = true;
            lblSightRangeId.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblSightRangeId.Location = new System.Drawing.Point(83, 181);
            lblSightRangeId.Name = "lblSightRangeId";
            lblSightRangeId.Size = new System.Drawing.Size(144, 21);
            lblSightRangeId.TabIndex = 254;
            lblSightRangeId.Text = "SightRangeName";
            // 
            // StatsSheet
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Controls.Add(lblSightRangeId);
            Controls.Add(lblStatId);
            Controls.Add(nudIncreasePerLevel);
            Controls.Add(label3);
            Controls.Add(lblPercentage);
            Controls.Add(nudBase);
            Controls.Add(label1);
            Controls.Add(chkIsUsed);
            Controls.Add(hsbStats);
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
            Size = new System.Drawing.Size(303, 408);
            ((System.ComponentModel.ISupportInitialize)nudMaxLevel).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudFlatSightRange).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudBase).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudIncreasePerLevel).EndInit();
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
        private System.Windows.Forms.HScrollBar hsbStats;
        private System.Windows.Forms.CheckBox chkIsUsed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudBase;
        private System.Windows.Forms.Label lblPercentage;
        private System.Windows.Forms.NumericUpDown nudIncreasePerLevel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblStatId;
        private System.Windows.Forms.Label lblSightRangeId;
    }
}
