namespace RogueCustomsDungeonEditor.Controls
{
    partial class AffixEditor
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(AffixEditor));
            fklblAffixNameLocale = new System.Windows.Forms.Button();
            txtAffixName = new System.Windows.Forms.TextBox();
            label27 = new System.Windows.Forms.Label();
            txtAffixId = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            cmbAffixType = new System.Windows.Forms.ComboBox();
            label3 = new System.Windows.Forms.Label();
            nudAffixMinimumItemLevel = new System.Windows.Forms.NumericUpDown();
            AffixStatsSheet = new ItemStatsSheet();
            lblStatsModifier = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            nudAffixItemValuePercentageModifier = new System.Windows.Forms.NumericUpDown();
            saeAffixOnAttack = new SingleActionEditor();
            clbAffixAffects = new System.Windows.Forms.CheckedListBox();
            saeAffixOnTurnStart = new SingleActionEditor();
            saeAffixOnAttacked = new SingleActionEditor();
            label5 = new System.Windows.Forms.Label();
            nudAffixMinDamage = new System.Windows.Forms.NumericUpDown();
            nudAffixMaxDamage = new System.Windows.Forms.NumericUpDown();
            cmbAffixElementDamage = new System.Windows.Forms.ComboBox();
            label6 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)nudAffixMinimumItemLevel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudAffixItemValuePercentageModifier).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudAffixMinDamage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudAffixMaxDamage).BeginInit();
            SuspendLayout();
            // 
            // fklblAffixNameLocale
            // 
            fklblAffixNameLocale.BackColor = System.Drawing.Color.Transparent;
            fklblAffixNameLocale.Enabled = false;
            fklblAffixNameLocale.FlatAppearance.BorderSize = 0;
            fklblAffixNameLocale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblAffixNameLocale.Image = (System.Drawing.Image)resources.GetObject("fklblAffixNameLocale.Image");
            fklblAffixNameLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblAffixNameLocale.Location = new System.Drawing.Point(3, 105);
            fklblAffixNameLocale.Name = "fklblAffixNameLocale";
            fklblAffixNameLocale.Size = new System.Drawing.Size(329, 43);
            fklblAffixNameLocale.TabIndex = 142;
            fklblAffixNameLocale.Text = "This value has been found as Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblAffixNameLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblAffixNameLocale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblAffixNameLocale.UseVisualStyleBackColor = false;
            fklblAffixNameLocale.Visible = false;
            // 
            // txtAffixName
            // 
            txtAffixName.Location = new System.Drawing.Point(3, 76);
            txtAffixName.Name = "txtAffixName";
            txtAffixName.Size = new System.Drawing.Size(329, 23);
            txtAffixName.TabIndex = 141;
            txtAffixName.TextChanged += txtAffixName_TextChanged;
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.BackColor = System.Drawing.Color.Transparent;
            label27.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            label27.Location = new System.Drawing.Point(3, 58);
            label27.Name = "label27";
            label27.Size = new System.Drawing.Size(71, 15);
            label27.TabIndex = 140;
            label27.Text = "Affix Name";
            // 
            // txtAffixId
            // 
            txtAffixId.Location = new System.Drawing.Point(3, 23);
            txtAffixId.Name = "txtAffixId";
            txtAffixId.Size = new System.Drawing.Size(329, 23);
            txtAffixId.TabIndex = 144;
            txtAffixId.TextChanged += txtAffixId_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = System.Drawing.Color.Transparent;
            label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            label1.Location = new System.Drawing.Point(3, 5);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(18, 15);
            label1.TabIndex = 143;
            label1.Text = "Id";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(4, 195);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(59, 15);
            label2.TabIndex = 145;
            label2.Text = "Affix Type";
            // 
            // cmbAffixType
            // 
            cmbAffixType.FormattingEnabled = true;
            cmbAffixType.Items.AddRange(new object[] { "Prefix", "Suffix" });
            cmbAffixType.Location = new System.Drawing.Point(69, 192);
            cmbAffixType.Name = "cmbAffixType";
            cmbAffixType.Size = new System.Drawing.Size(121, 23);
            cmbAffixType.TabIndex = 146;
            cmbAffixType.SelectedIndexChanged += cmbAffixType_SelectedIndexChanged;
            cmbAffixType.TextChanged += cmbAffixType_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(3, 256);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(120, 15);
            label3.TabIndex = 150;
            label3.Text = "Minimum Item Level:";
            // 
            // nudAffixMinimumItemLevel
            // 
            nudAffixMinimumItemLevel.Location = new System.Drawing.Point(129, 254);
            nudAffixMinimumItemLevel.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudAffixMinimumItemLevel.Name = "nudAffixMinimumItemLevel";
            nudAffixMinimumItemLevel.Size = new System.Drawing.Size(60, 23);
            nudAffixMinimumItemLevel.TabIndex = 151;
            nudAffixMinimumItemLevel.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nudAffixMinimumItemLevel.ValueChanged += nudAffixMinimumItemLevel_ValueChanged;
            // 
            // AffixStatsSheet
            // 
            AffixStatsSheet.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            AffixStatsSheet.Location = new System.Drawing.Point(409, 36);
            AffixStatsSheet.Name = "AffixStatsSheet";
            AffixStatsSheet.Size = new System.Drawing.Size(234, 119);
            AffixStatsSheet.TabIndex = 250;
            // 
            // lblStatsModifier
            // 
            lblStatsModifier.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblStatsModifier.Location = new System.Drawing.Point(354, -6);
            lblStatsModifier.Name = "lblStatsModifier";
            lblStatsModifier.Size = new System.Drawing.Size(340, 52);
            lblStatsModifier.TabIndex = 249;
            lblStatsModifier.Text = "Modifies the item's owner's following stats:";
            lblStatsModifier.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(3, 286);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(180, 15);
            label4.TabIndex = 251;
            label4.Text = "Raises Item Value by                   %";
            // 
            // nudAffixItemValuePercentageModifier
            // 
            nudAffixItemValuePercentageModifier.Location = new System.Drawing.Point(115, 283);
            nudAffixItemValuePercentageModifier.Name = "nudAffixItemValuePercentageModifier";
            nudAffixItemValuePercentageModifier.Size = new System.Drawing.Size(52, 23);
            nudAffixItemValuePercentageModifier.TabIndex = 252;
            nudAffixItemValuePercentageModifier.ValueChanged += nudAffixItemValuePercentageModifier_ValueChanged;
            // 
            // saeAffixOnAttack
            // 
            saeAffixOnAttack.ActionDescription = "The item's owner can interact with this:     ";
            saeAffixOnAttack.ActionTypeText = "Interact";
            saeAffixOnAttack.AutoSize = true;
            saeAffixOnAttack.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saeAffixOnAttack.ClassId = null;
            saeAffixOnAttack.Location = new System.Drawing.Point(335, 242);
            saeAffixOnAttack.Name = "saeAffixOnAttack";
            saeAffixOnAttack.PlaceholderActionId = "AffixAttack";
            saeAffixOnAttack.RequiresActionId = true;
            saeAffixOnAttack.RequiresCondition = true;
            saeAffixOnAttack.RequiresDescription = true;
            saeAffixOnAttack.RequiresName = true;
            saeAffixOnAttack.Size = new System.Drawing.Size(363, 32);
            saeAffixOnAttack.SourceDescription = "The Item's Owner";
            saeAffixOnAttack.TabIndex = 254;
            saeAffixOnAttack.TargetDescription = "Whoever the Item's Owner are targeting";
            saeAffixOnAttack.ThisDescription = "The Item with this Affix";
            saeAffixOnAttack.TurnEndCriteria = HelperForms.TurnEndCriteria.MustEndTurn;
            saeAffixOnAttack.UsageCriteria = HelperForms.UsageCriteria.FullConditions;
            // 
            // clbAffixAffects
            // 
            clbAffixAffects.FormattingEnabled = true;
            clbAffixAffects.Location = new System.Drawing.Point(206, 217);
            clbAffixAffects.Name = "clbAffixAffects";
            clbAffixAffects.Size = new System.Drawing.Size(106, 76);
            clbAffixAffects.TabIndex = 255;
            // 
            // saeAffixOnTurnStart
            // 
            saeAffixOnTurnStart.ActionDescription = "When the Item's Owner starts a new turn...";
            saeAffixOnTurnStart.ActionTypeText = "Turn Start";
            saeAffixOnTurnStart.AutoSize = true;
            saeAffixOnTurnStart.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saeAffixOnTurnStart.ClassId = null;
            saeAffixOnTurnStart.Location = new System.Drawing.Point(335, 210);
            saeAffixOnTurnStart.Name = "saeAffixOnTurnStart";
            saeAffixOnTurnStart.PlaceholderActionId = "AffixTurnStart";
            saeAffixOnTurnStart.Size = new System.Drawing.Size(364, 32);
            saeAffixOnTurnStart.SourceDescription = "Whoever is equipping This";
            saeAffixOnTurnStart.TabIndex = 256;
            saeAffixOnTurnStart.TargetDescription = "Whoever is equipping This";
            saeAffixOnTurnStart.ThisDescription = "The Item with this Affix";
            // 
            // saeAffixOnAttacked
            // 
            saeAffixOnAttacked.ActionDescription = "When the Item's owner gets interacted...   ";
            saeAffixOnAttacked.ActionTypeText = "Interacted";
            saeAffixOnAttacked.AutoSize = true;
            saeAffixOnAttacked.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saeAffixOnAttacked.ClassId = null;
            saeAffixOnAttacked.Location = new System.Drawing.Point(335, 274);
            saeAffixOnAttacked.Name = "saeAffixOnAttacked";
            saeAffixOnAttacked.PlaceholderActionId = "AffixInteracted";
            saeAffixOnAttacked.Size = new System.Drawing.Size(362, 32);
            saeAffixOnAttacked.SourceDescription = "Whoever is equipping it";
            saeAffixOnAttacked.TabIndex = 257;
            saeAffixOnAttacked.TargetDescription = "The owner's interactor";
            saeAffixOnAttacked.ThisDescription = "The item";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(339, 167);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(355, 15);
            label5.TabIndex = 258;
            label5.Text = "Adds                -                                               damage to DealDamage";
            // 
            // nudAffixMinDamage
            // 
            nudAffixMinDamage.Location = new System.Drawing.Point(373, 165);
            nudAffixMinDamage.Name = "nudAffixMinDamage";
            nudAffixMinDamage.Size = new System.Drawing.Size(40, 23);
            nudAffixMinDamage.TabIndex = 259;
            nudAffixMinDamage.ValueChanged += nudAffixMinDamage_ValueChanged;
            // 
            // nudAffixMaxDamage
            // 
            nudAffixMaxDamage.Location = new System.Drawing.Point(429, 165);
            nudAffixMaxDamage.Name = "nudAffixMaxDamage";
            nudAffixMaxDamage.Size = new System.Drawing.Size(40, 23);
            nudAffixMaxDamage.TabIndex = 260;
            nudAffixMaxDamage.ValueChanged += nudAffixMaxDamage_ValueChanged;
            // 
            // cmbAffixElementDamage
            // 
            cmbAffixElementDamage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbAffixElementDamage.FormattingEnabled = true;
            cmbAffixElementDamage.Location = new System.Drawing.Point(475, 165);
            cmbAffixElementDamage.Name = "cmbAffixElementDamage";
            cmbAffixElementDamage.Size = new System.Drawing.Size(82, 23);
            cmbAffixElementDamage.TabIndex = 261;
            cmbAffixElementDamage.SelectedIndexChanged += cmbAffixElementDamage_SelectedIndexChanged;
            cmbAffixElementDamage.TextChanged += cmbAffixElementDamage_TextChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(491, 188);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(205, 15);
            label6.TabIndex = 262;
            label6.Text = "(ignores defenses but not resistances)";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(206, 193);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(112, 15);
            label7.TabIndex = 263;
            label7.Text = "Affected Item Types";
            // 
            // button1
            // 
            button1.BackColor = System.Drawing.Color.Transparent;
            button1.Enabled = false;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            button1.Image = (System.Drawing.Image)resources.GetObject("button1.Image");
            button1.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            button1.Location = new System.Drawing.Point(3, 143);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(329, 43);
            button1.TabIndex = 264;
            button1.Text = "To include the Item's base name alongside the Affixes,\r\nmake sure the Affix Name includes {baseName}.";
            button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            button1.UseVisualStyleBackColor = false;
            // 
            // AffixEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(button1);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(cmbAffixElementDamage);
            Controls.Add(nudAffixMaxDamage);
            Controls.Add(nudAffixMinDamage);
            Controls.Add(label5);
            Controls.Add(saeAffixOnAttacked);
            Controls.Add(clbAffixAffects);
            Controls.Add(saeAffixOnAttack);
            Controls.Add(nudAffixItemValuePercentageModifier);
            Controls.Add(label4);
            Controls.Add(AffixStatsSheet);
            Controls.Add(lblStatsModifier);
            Controls.Add(nudAffixMinimumItemLevel);
            Controls.Add(label3);
            Controls.Add(cmbAffixType);
            Controls.Add(label2);
            Controls.Add(txtAffixId);
            Controls.Add(label1);
            Controls.Add(fklblAffixNameLocale);
            Controls.Add(txtAffixName);
            Controls.Add(label27);
            Controls.Add(saeAffixOnTurnStart);
            Name = "AffixEditor";
            Size = new System.Drawing.Size(699, 313);
            ((System.ComponentModel.ISupportInitialize)nudAffixMinimumItemLevel).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudAffixItemValuePercentageModifier).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudAffixMinDamage).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudAffixMaxDamage).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button fklblAffixNameLocale;
        private System.Windows.Forms.TextBox txtAffixName;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox txtAffixId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbAffixType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudAffixMinimumItemLevel;
        private ItemStatsSheet AffixStatsSheet;
        private System.Windows.Forms.Label lblStatsModifier;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudAffixItemValuePercentageModifier;
        private SingleActionEditor saeAffixOnAttack;
        private System.Windows.Forms.CheckedListBox clbAffixAffects;
        private SingleActionEditor saeAffixOnTurnStart;
        private SingleActionEditor saeAffixOnAttacked;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudAffixMinDamage;
        private System.Windows.Forms.NumericUpDown nudAffixMaxDamage;
        private System.Windows.Forms.ComboBox cmbAffixElementDamage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button1;
    }
}
