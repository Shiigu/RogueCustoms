namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    partial class QuestTab
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(QuestTab));
            fklblQuestDescriptionLocale = new System.Windows.Forms.Button();
            txtQuestDescription = new System.Windows.Forms.TextBox();
            label28 = new System.Windows.Forms.Label();
            fklblQuestNameLocale = new System.Windows.Forms.Button();
            txtQuestName = new System.Windows.Forms.TextBox();
            label27 = new System.Windows.Forms.Label();
            chkQuestIsRepeatable = new System.Windows.Forms.CheckBox();
            label1 = new System.Windows.Forms.Label();
            dgvQuestConditions = new System.Windows.Forms.DataGridView();
            cmConditionType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            cmConditionObject = new System.Windows.Forms.DataGridViewComboBoxColumn();
            cmValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            cmbQuestMinimumConditions = new System.Windows.Forms.ComboBox();
            chkQuestIsAbandonedOnFloorChange = new System.Windows.Forms.CheckBox();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            nudQuestMonetaryReward = new System.Windows.Forms.NumericUpDown();
            nudQuestExperienceReward = new System.Windows.Forms.NumericUpDown();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            nudQuestCompensationExperienceReward = new System.Windows.Forms.NumericUpDown();
            label8 = new System.Windows.Forms.Label();
            nudQuestCompensationMonetaryReward = new System.Windows.Forms.NumericUpDown();
            label9 = new System.Windows.Forms.Label();
            saeQuestComplete = new SingleActionEditor();
            qirsGuaranteed = new QuestItemRewardSheet();
            qirsSelectable = new QuestItemRewardSheet();
            label10 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)dgvQuestConditions).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudQuestMonetaryReward).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudQuestExperienceReward).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudQuestCompensationExperienceReward).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudQuestCompensationMonetaryReward).BeginInit();
            SuspendLayout();
            // 
            // fklblQuestDescriptionLocale
            // 
            fklblQuestDescriptionLocale.Enabled = false;
            fklblQuestDescriptionLocale.FlatAppearance.BorderSize = 0;
            fklblQuestDescriptionLocale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblQuestDescriptionLocale.Image = (System.Drawing.Image)resources.GetObject("fklblQuestDescriptionLocale.Image");
            fklblQuestDescriptionLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblQuestDescriptionLocale.Location = new System.Drawing.Point(5, 144);
            fklblQuestDescriptionLocale.Name = "fklblQuestDescriptionLocale";
            fklblQuestDescriptionLocale.Size = new System.Drawing.Size(331, 42);
            fklblQuestDescriptionLocale.TabIndex = 142;
            fklblQuestDescriptionLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblQuestDescriptionLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblQuestDescriptionLocale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblQuestDescriptionLocale.UseVisualStyleBackColor = true;
            fklblQuestDescriptionLocale.Visible = false;
            // 
            // txtQuestDescription
            // 
            txtQuestDescription.Location = new System.Drawing.Point(5, 115);
            txtQuestDescription.Name = "txtQuestDescription";
            txtQuestDescription.Size = new System.Drawing.Size(350, 23);
            txtQuestDescription.TabIndex = 141;
            txtQuestDescription.TextChanged += txtQuestDescription_TextChanged;
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.Location = new System.Drawing.Point(5, 97);
            label28.Name = "label28";
            label28.Size = new System.Drawing.Size(67, 15);
            label28.TabIndex = 140;
            label28.Text = "Description";
            // 
            // fklblQuestNameLocale
            // 
            fklblQuestNameLocale.Enabled = false;
            fklblQuestNameLocale.FlatAppearance.BorderSize = 0;
            fklblQuestNameLocale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblQuestNameLocale.Image = (System.Drawing.Image)resources.GetObject("fklblQuestNameLocale.Image");
            fklblQuestNameLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblQuestNameLocale.Location = new System.Drawing.Point(5, 52);
            fklblQuestNameLocale.Name = "fklblQuestNameLocale";
            fklblQuestNameLocale.Size = new System.Drawing.Size(331, 42);
            fklblQuestNameLocale.TabIndex = 139;
            fklblQuestNameLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblQuestNameLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblQuestNameLocale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblQuestNameLocale.UseVisualStyleBackColor = true;
            fklblQuestNameLocale.Visible = false;
            // 
            // txtQuestName
            // 
            txtQuestName.Location = new System.Drawing.Point(5, 23);
            txtQuestName.Name = "txtQuestName";
            txtQuestName.Size = new System.Drawing.Size(350, 23);
            txtQuestName.TabIndex = 138;
            txtQuestName.TextChanged += txtQuestName_TextChanged;
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.Location = new System.Drawing.Point(5, 5);
            label27.Name = "label27";
            label27.Size = new System.Drawing.Size(80, 15);
            label27.TabIndex = 137;
            label27.Text = "Default Name";
            // 
            // chkQuestIsRepeatable
            // 
            chkQuestIsRepeatable.AutoSize = true;
            chkQuestIsRepeatable.Location = new System.Drawing.Point(5, 192);
            chkQuestIsRepeatable.Name = "chkQuestIsRepeatable";
            chkQuestIsRepeatable.Size = new System.Drawing.Size(95, 19);
            chkQuestIsRepeatable.TabIndex = 143;
            chkQuestIsRepeatable.Text = "Is Repeatable";
            chkQuestIsRepeatable.UseVisualStyleBackColor = true;
            chkQuestIsRepeatable.CheckedChanged += chkQuestIsRepeatable_CheckedChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label1.Location = new System.Drawing.Point(131, 239);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(93, 21);
            label1.TabIndex = 144;
            label1.Text = "Conditions";
            // 
            // dgvQuestConditions
            // 
            dgvQuestConditions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvQuestConditions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { cmConditionType, cmConditionObject, cmValue });
            dgvQuestConditions.Location = new System.Drawing.Point(5, 263);
            dgvQuestConditions.Name = "dgvQuestConditions";
            dgvQuestConditions.Size = new System.Drawing.Size(350, 194);
            dgvQuestConditions.TabIndex = 145;
            dgvQuestConditions.CellValidating += dgvQuestConditions_CellValidating;
            dgvQuestConditions.CellValueChanged += dgvQuestConditions_CellValueChanged;
            // 
            // cmConditionType
            // 
            cmConditionType.Frozen = true;
            cmConditionType.HeaderText = "Condition";
            cmConditionType.Name = "cmConditionType";
            cmConditionType.Width = 115;
            // 
            // cmConditionObject
            // 
            cmConditionObject.Frozen = true;
            cmConditionObject.HeaderText = "Object";
            cmConditionObject.Name = "cmConditionObject";
            cmConditionObject.Width = 115;
            // 
            // cmValue
            // 
            cmValue.Frozen = true;
            cmValue.HeaderText = "Value";
            cmValue.Name = "cmValue";
            cmValue.Width = 70;
            // 
            // cmbQuestMinimumConditions
            // 
            cmbQuestMinimumConditions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbQuestMinimumConditions.FormattingEnabled = true;
            cmbQuestMinimumConditions.Location = new System.Drawing.Point(5, 463);
            cmbQuestMinimumConditions.Name = "cmbQuestMinimumConditions";
            cmbQuestMinimumConditions.Size = new System.Drawing.Size(350, 23);
            cmbQuestMinimumConditions.TabIndex = 146;
            cmbQuestMinimumConditions.SelectedIndexChanged += cmbQuestMinimumConditions_SelectedIndexChanged;
            // 
            // chkQuestIsAbandonedOnFloorChange
            // 
            chkQuestIsAbandonedOnFloorChange.AutoSize = true;
            chkQuestIsAbandonedOnFloorChange.Location = new System.Drawing.Point(5, 217);
            chkQuestIsAbandonedOnFloorChange.Name = "chkQuestIsAbandonedOnFloorChange";
            chkQuestIsAbandonedOnFloorChange.Size = new System.Drawing.Size(232, 19);
            chkQuestIsAbandonedOnFloorChange.TabIndex = 147;
            chkQuestIsAbandonedOnFloorChange.Text = "Will be abandoned if the Floor changes";
            chkQuestIsAbandonedOnFloorChange.UseVisualStyleBackColor = true;
            chkQuestIsAbandonedOnFloorChange.CheckedChanged += chkQuestIsAbandonedOnFloorChange_CheckedChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label2.Location = new System.Drawing.Point(526, 5);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(74, 21);
            label2.TabIndex = 148;
            label2.Text = "Rewards";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(449, 42);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(202, 15);
            label3.TabIndex = 149;
            label3.Text = "Monetary Reward:                               $";
            // 
            // nudQuestMonetaryReward
            // 
            nudQuestMonetaryReward.Location = new System.Drawing.Point(567, 40);
            nudQuestMonetaryReward.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            nudQuestMonetaryReward.Name = "nudQuestMonetaryReward";
            nudQuestMonetaryReward.Size = new System.Drawing.Size(71, 23);
            nudQuestMonetaryReward.TabIndex = 150;
            nudQuestMonetaryReward.ValueChanged += nudQuestMonetaryReward_ValueChanged;
            // 
            // nudQuestExperienceReward
            // 
            nudQuestExperienceReward.Location = new System.Drawing.Point(567, 69);
            nudQuestExperienceReward.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            nudQuestExperienceReward.Name = "nudQuestExperienceReward";
            nudQuestExperienceReward.Size = new System.Drawing.Size(71, 23);
            nudQuestExperienceReward.TabIndex = 152;
            nudQuestExperienceReward.ValueChanged += nudQuestExperienceReward_ValueChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(449, 71);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(108, 15);
            label4.TabIndex = 151;
            label4.Text = "Experience Reward:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label5.Location = new System.Drawing.Point(510, 104);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(107, 15);
            label5.TabIndex = 153;
            label5.Text = "Guaranteed items";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label6.Location = new System.Drawing.Point(510, 293);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(99, 15);
            label6.TabIndex = 155;
            label6.Text = "Selectable items";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label7.Location = new System.Drawing.Point(470, 531);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(186, 15);
            label7.TabIndex = 157;
            label7.Text = "Compensation if inventory is full";
            // 
            // nudQuestCompensationExperienceReward
            // 
            nudQuestCompensationExperienceReward.Location = new System.Drawing.Point(567, 584);
            nudQuestCompensationExperienceReward.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            nudQuestCompensationExperienceReward.Name = "nudQuestCompensationExperienceReward";
            nudQuestCompensationExperienceReward.Size = new System.Drawing.Size(71, 23);
            nudQuestCompensationExperienceReward.TabIndex = 161;
            nudQuestCompensationExperienceReward.ValueChanged += nudQuestCompensationExperienceReward_ValueChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(449, 586);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(108, 15);
            label8.TabIndex = 160;
            label8.Text = "Experience Reward:";
            // 
            // nudQuestCompensationMonetaryReward
            // 
            nudQuestCompensationMonetaryReward.Location = new System.Drawing.Point(567, 555);
            nudQuestCompensationMonetaryReward.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            nudQuestCompensationMonetaryReward.Name = "nudQuestCompensationMonetaryReward";
            nudQuestCompensationMonetaryReward.Size = new System.Drawing.Size(71, 23);
            nudQuestCompensationMonetaryReward.TabIndex = 159;
            nudQuestCompensationMonetaryReward.ValueChanged += nudQuestCompensationMonetaryReward_ValueChanged;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(449, 557);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(202, 15);
            label9.TabIndex = 158;
            label9.Text = "Monetary Reward:                               $";
            // 
            // saeQuestComplete
            // 
            saeQuestComplete.ActionDescription = "When the Quest is complete...";
            saeQuestComplete.ActionTypeText = "Quest Complete";
            saeQuestComplete.AutoSize = true;
            saeQuestComplete.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saeQuestComplete.Location = new System.Drawing.Point(25, 492);
            saeQuestComplete.Name = "saeQuestComplete";
            saeQuestComplete.PlaceholderActionId = "QuestComplete";
            saeQuestComplete.Size = new System.Drawing.Size(299, 32);
            saeQuestComplete.SourceDescription = "The player";
            saeQuestComplete.TabIndex = 162;
            saeQuestComplete.TargetDescription = "The player";
            saeQuestComplete.ThisDescription = "The player";
            // 
            // qirsGuaranteed
            // 
            qirsGuaranteed.Location = new System.Drawing.Point(396, 122);
            qirsGuaranteed.Name = "qirsGuaranteed";
            qirsGuaranteed.Size = new System.Drawing.Size(345, 168);
            qirsGuaranteed.TabIndex = 163;
            qirsGuaranteed.RewardsChanged += qirsGuaranteed_RewardsChanged;
            // 
            // qirsSelectable
            // 
            qirsSelectable.Location = new System.Drawing.Point(396, 310);
            qirsSelectable.Name = "qirsSelectable";
            qirsSelectable.Size = new System.Drawing.Size(345, 168);
            qirsSelectable.TabIndex = 164;
            qirsSelectable.RewardsChanged += qirsSelectable_RewardsChanged;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(374, 486);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(389, 30);
            label10.TabIndex = 165;
            label10.Text = "Please note that, if the selected Quality Level is not within the Item's\r\nMinimum/Maximum Quality Level range, it will be adjusted accordingly.";
            // 
            // QuestTab
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(label10);
            Controls.Add(qirsSelectable);
            Controls.Add(qirsGuaranteed);
            Controls.Add(saeQuestComplete);
            Controls.Add(nudQuestCompensationExperienceReward);
            Controls.Add(label8);
            Controls.Add(nudQuestCompensationMonetaryReward);
            Controls.Add(label9);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(nudQuestExperienceReward);
            Controls.Add(label4);
            Controls.Add(nudQuestMonetaryReward);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(chkQuestIsAbandonedOnFloorChange);
            Controls.Add(cmbQuestMinimumConditions);
            Controls.Add(dgvQuestConditions);
            Controls.Add(label1);
            Controls.Add(chkQuestIsRepeatable);
            Controls.Add(fklblQuestDescriptionLocale);
            Controls.Add(txtQuestDescription);
            Controls.Add(label28);
            Controls.Add(fklblQuestNameLocale);
            Controls.Add(txtQuestName);
            Controls.Add(label27);
            Name = "QuestTab";
            Size = new System.Drawing.Size(772, 610);
            ((System.ComponentModel.ISupportInitialize)dgvQuestConditions).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudQuestMonetaryReward).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudQuestExperienceReward).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudQuestCompensationExperienceReward).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudQuestCompensationMonetaryReward).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button fklblQuestDescriptionLocale;
        private System.Windows.Forms.TextBox txtQuestDescription;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Button fklblQuestNameLocale;
        private System.Windows.Forms.TextBox txtQuestName;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.CheckBox chkQuestIsRepeatable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvQuestConditions;
        private System.Windows.Forms.ComboBox cmbQuestMinimumConditions;
        private System.Windows.Forms.CheckBox chkQuestIsAbandonedOnFloorChange;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudQuestMonetaryReward;
        private System.Windows.Forms.NumericUpDown nudQuestExperienceReward;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudQuestCompensationExperienceReward;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown nudQuestCompensationMonetaryReward;
        private System.Windows.Forms.Label label9;
        private SingleActionEditor saeQuestComplete;
        private QuestItemRewardSheet qirsGuaranteed;
        private QuestItemRewardSheet qirsSelectable;
        private System.Windows.Forms.DataGridViewComboBoxColumn cmConditionType;
        private System.Windows.Forms.DataGridViewComboBoxColumn cmConditionObject;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmValue;
        private System.Windows.Forms.Label label10;
    }
}
