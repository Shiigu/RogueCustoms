namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    partial class AffixTab
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(AffixTab));
            button1 = new System.Windows.Forms.Button();
            label7 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            cmbAffixElementDamage = new System.Windows.Forms.ComboBox();
            nudAffixMaxDamage = new System.Windows.Forms.NumericUpDown();
            nudAffixMinDamage = new System.Windows.Forms.NumericUpDown();
            label5 = new System.Windows.Forms.Label();
            saeAffixOnAttacked = new SingleActionEditor();
            clbAffixAffects = new System.Windows.Forms.CheckedListBox();
            saeAffixOnAttack = new SingleActionEditor();
            nudAffixItemValuePercentageModifier = new System.Windows.Forms.NumericUpDown();
            label4 = new System.Windows.Forms.Label();
            AffixStatsSheet = new ItemStatsSheet();
            lblStatsModifier = new System.Windows.Forms.Label();
            nudAffixMinimumItemLevel = new System.Windows.Forms.NumericUpDown();
            label3 = new System.Windows.Forms.Label();
            cmbAffixType = new System.Windows.Forms.ComboBox();
            label2 = new System.Windows.Forms.Label();
            fklblAffixNameLocale = new System.Windows.Forms.Button();
            txtAffixName = new System.Windows.Forms.TextBox();
            label27 = new System.Windows.Forms.Label();
            saeAffixOnTurnStart = new SingleActionEditor();
            ((System.ComponentModel.ISupportInitialize)nudAffixMaxDamage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudAffixMinDamage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudAffixItemValuePercentageModifier).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudAffixMinimumItemLevel).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackColor = System.Drawing.Color.Transparent;
            button1.Enabled = false;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            button1.Image = (System.Drawing.Image)resources.GetObject("button1.Image");
            button1.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            button1.Location = new System.Drawing.Point(3, 89);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(329, 43);
            button1.TabIndex = 288;
            button1.Text = "To include the Item's base name alongside the Affixes,\r\nmake sure the Affix Name includes {baseName}.";
            button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            button1.UseVisualStyleBackColor = false;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(4, 173);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(112, 15);
            label7.TabIndex = 287;
            label7.Text = "Affected Item Types";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(582, 246);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(205, 15);
            label6.TabIndex = 286;
            label6.Text = "(ignores defenses but not resistances)";
            // 
            // cmbAffixElementDamage
            // 
            cmbAffixElementDamage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbAffixElementDamage.FormattingEnabled = true;
            cmbAffixElementDamage.Location = new System.Drawing.Point(566, 223);
            cmbAffixElementDamage.Name = "cmbAffixElementDamage";
            cmbAffixElementDamage.Size = new System.Drawing.Size(82, 23);
            cmbAffixElementDamage.TabIndex = 285;
            cmbAffixElementDamage.SelectedIndexChanged += cmbAffixElementDamage_SelectedIndexChanged;
            cmbAffixElementDamage.TextChanged += cmbAffixElementDamage_TextChanged;
            // 
            // nudAffixMaxDamage
            // 
            nudAffixMaxDamage.Location = new System.Drawing.Point(520, 223);
            nudAffixMaxDamage.Name = "nudAffixMaxDamage";
            nudAffixMaxDamage.Size = new System.Drawing.Size(40, 23);
            nudAffixMaxDamage.TabIndex = 284;
            nudAffixMaxDamage.ValueChanged += nudAffixMaxDamage_ValueChanged;
            // 
            // nudAffixMinDamage
            // 
            nudAffixMinDamage.Location = new System.Drawing.Point(464, 223);
            nudAffixMinDamage.Name = "nudAffixMinDamage";
            nudAffixMinDamage.Size = new System.Drawing.Size(40, 23);
            nudAffixMinDamage.TabIndex = 283;
            nudAffixMinDamage.ValueChanged += nudAffixMinDamage_ValueChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(430, 225);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(355, 15);
            label5.TabIndex = 282;
            label5.Text = "Adds                -                                               damage to DealDamage";
            // 
            // saeAffixOnAttacked
            // 
            saeAffixOnAttacked.ActionDescription = "When the Item's owner gets interacted...   ";
            saeAffixOnAttacked.ActionTypeText = "Interacted";
            saeAffixOnAttacked.AutoSize = true;
            saeAffixOnAttacked.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saeAffixOnAttacked.ClassId = null;
            saeAffixOnAttacked.Location = new System.Drawing.Point(426, 332);
            saeAffixOnAttacked.Name = "saeAffixOnAttacked";
            saeAffixOnAttacked.PlaceholderActionId = "AffixInteracted";
            saeAffixOnAttacked.Size = new System.Drawing.Size(362, 32);
            saeAffixOnAttacked.SourceDescription = "Whoever is equipping it";
            saeAffixOnAttacked.TabIndex = 281;
            saeAffixOnAttacked.TargetDescription = "The owner's interactor";
            saeAffixOnAttacked.ThisDescription = "The item";
            // 
            // clbAffixAffects
            // 
            clbAffixAffects.FormattingEnabled = true;
            clbAffixAffects.Location = new System.Drawing.Point(4, 197);
            clbAffixAffects.Name = "clbAffixAffects";
            clbAffixAffects.Size = new System.Drawing.Size(134, 94);
            clbAffixAffects.TabIndex = 279;
            clbAffixAffects.SelectedIndexChanged += clbAffixAffects_SelectedIndexChanged;
            // 
            // saeAffixOnAttack
            // 
            saeAffixOnAttack.ActionDescription = "The item's owner can interact with this:     ";
            saeAffixOnAttack.ActionTypeText = "Interact";
            saeAffixOnAttack.AutoSize = true;
            saeAffixOnAttack.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saeAffixOnAttack.ClassId = null;
            saeAffixOnAttack.Location = new System.Drawing.Point(426, 300);
            saeAffixOnAttack.Name = "saeAffixOnAttack";
            saeAffixOnAttack.PlaceholderActionId = "AffixAttack";
            saeAffixOnAttack.RequiresActionId = true;
            saeAffixOnAttack.RequiresCondition = true;
            saeAffixOnAttack.RequiresDescription = true;
            saeAffixOnAttack.RequiresName = true;
            saeAffixOnAttack.Size = new System.Drawing.Size(363, 32);
            saeAffixOnAttack.SourceDescription = "The Item's Owner";
            saeAffixOnAttack.TabIndex = 278;
            saeAffixOnAttack.TargetDescription = "Whoever the Item's Owner are targeting";
            saeAffixOnAttack.ThisDescription = "The Item with this Affix";
            saeAffixOnAttack.TurnEndCriteria = HelperForms.TurnEndCriteria.MustEndTurn;
            saeAffixOnAttack.UsageCriteria = HelperForms.UsageCriteria.FullConditions;
            // 
            // nudAffixItemValuePercentageModifier
            // 
            nudAffixItemValuePercentageModifier.Location = new System.Drawing.Point(116, 335);
            nudAffixItemValuePercentageModifier.Name = "nudAffixItemValuePercentageModifier";
            nudAffixItemValuePercentageModifier.Size = new System.Drawing.Size(52, 23);
            nudAffixItemValuePercentageModifier.TabIndex = 277;
            nudAffixItemValuePercentageModifier.ValueChanged += nudAffixItemValuePercentageModifier_ValueChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(4, 338);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(180, 15);
            label4.TabIndex = 276;
            label4.Text = "Raises Item Value by                   %";
            // 
            // AffixStatsSheet
            // 
            AffixStatsSheet.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            AffixStatsSheet.Location = new System.Drawing.Point(491, 40);
            AffixStatsSheet.Name = "AffixStatsSheet";
            AffixStatsSheet.Size = new System.Drawing.Size(234, 170);
            AffixStatsSheet.TabIndex = 275;
            AffixStatsSheet.StatsChanged += AffixStatsSheet_StatsChanged;
            // 
            // lblStatsModifier
            // 
            lblStatsModifier.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblStatsModifier.Location = new System.Drawing.Point(436, -2);
            lblStatsModifier.Name = "lblStatsModifier";
            lblStatsModifier.Size = new System.Drawing.Size(340, 52);
            lblStatsModifier.TabIndex = 274;
            lblStatsModifier.Text = "Modifies the item's owner's following stats:";
            lblStatsModifier.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudAffixMinimumItemLevel
            // 
            nudAffixMinimumItemLevel.Location = new System.Drawing.Point(130, 301);
            nudAffixMinimumItemLevel.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudAffixMinimumItemLevel.Name = "nudAffixMinimumItemLevel";
            nudAffixMinimumItemLevel.Size = new System.Drawing.Size(60, 23);
            nudAffixMinimumItemLevel.TabIndex = 273;
            nudAffixMinimumItemLevel.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nudAffixMinimumItemLevel.ValueChanged += nudAffixMinimumItemLevel_ValueChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(4, 303);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(120, 15);
            label3.TabIndex = 272;
            label3.Text = "Minimum Item Level:";
            // 
            // cmbAffixType
            // 
            cmbAffixType.FormattingEnabled = true;
            cmbAffixType.Items.AddRange(new object[] { "Prefix", "Suffix" });
            cmbAffixType.Location = new System.Drawing.Point(69, 138);
            cmbAffixType.Name = "cmbAffixType";
            cmbAffixType.Size = new System.Drawing.Size(121, 23);
            cmbAffixType.TabIndex = 271;
            cmbAffixType.SelectedIndexChanged += cmbAffixType_SelectedIndexChanged;
            cmbAffixType.TextChanged += cmbAffixType_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(4, 141);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(59, 15);
            label2.TabIndex = 270;
            label2.Text = "Affix Type";
            // 
            // fklblAffixNameLocale
            // 
            fklblAffixNameLocale.BackColor = System.Drawing.Color.Transparent;
            fklblAffixNameLocale.Enabled = false;
            fklblAffixNameLocale.FlatAppearance.BorderSize = 0;
            fklblAffixNameLocale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblAffixNameLocale.Image = (System.Drawing.Image)resources.GetObject("fklblAffixNameLocale.Image");
            fklblAffixNameLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblAffixNameLocale.Location = new System.Drawing.Point(3, 51);
            fklblAffixNameLocale.Name = "fklblAffixNameLocale";
            fklblAffixNameLocale.Size = new System.Drawing.Size(329, 43);
            fklblAffixNameLocale.TabIndex = 267;
            fklblAffixNameLocale.Text = "This value has been found as Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblAffixNameLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblAffixNameLocale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblAffixNameLocale.UseVisualStyleBackColor = false;
            fklblAffixNameLocale.Visible = false;
            // 
            // txtAffixName
            // 
            txtAffixName.Location = new System.Drawing.Point(3, 22);
            txtAffixName.Name = "txtAffixName";
            txtAffixName.Size = new System.Drawing.Size(329, 23);
            txtAffixName.TabIndex = 266;
            txtAffixName.TextChanged += txtAffixName_TextChanged;
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.BackColor = System.Drawing.Color.Transparent;
            label27.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            label27.Location = new System.Drawing.Point(3, 4);
            label27.Name = "label27";
            label27.Size = new System.Drawing.Size(71, 15);
            label27.TabIndex = 265;
            label27.Text = "Affix Name";
            // 
            // saeAffixOnTurnStart
            // 
            saeAffixOnTurnStart.ActionDescription = "When the Item's Owner starts a new turn...";
            saeAffixOnTurnStart.ActionTypeText = "Turn Start";
            saeAffixOnTurnStart.AutoSize = true;
            saeAffixOnTurnStart.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saeAffixOnTurnStart.ClassId = null;
            saeAffixOnTurnStart.Location = new System.Drawing.Point(426, 268);
            saeAffixOnTurnStart.Name = "saeAffixOnTurnStart";
            saeAffixOnTurnStart.PlaceholderActionId = "AffixTurnStart";
            saeAffixOnTurnStart.Size = new System.Drawing.Size(364, 32);
            saeAffixOnTurnStart.SourceDescription = "Whoever is equipping This";
            saeAffixOnTurnStart.TabIndex = 280;
            saeAffixOnTurnStart.TargetDescription = "Whoever is equipping This";
            saeAffixOnTurnStart.ThisDescription = "The Item with this Affix";
            // 
            // AffixTab
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
            Controls.Add(fklblAffixNameLocale);
            Controls.Add(txtAffixName);
            Controls.Add(label27);
            Controls.Add(saeAffixOnTurnStart);
            Name = "AffixTab";
            Size = new System.Drawing.Size(829, 373);
            ((System.ComponentModel.ISupportInitialize)nudAffixMaxDamage).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudAffixMinDamage).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudAffixItemValuePercentageModifier).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudAffixMinimumItemLevel).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbAffixElementDamage;
        private System.Windows.Forms.NumericUpDown nudAffixMaxDamage;
        private System.Windows.Forms.NumericUpDown nudAffixMinDamage;
        private System.Windows.Forms.Label label5;
        private SingleActionEditor saeAffixOnAttacked;
        private System.Windows.Forms.CheckedListBox clbAffixAffects;
        private SingleActionEditor saeAffixOnAttack;
        private System.Windows.Forms.NumericUpDown nudAffixItemValuePercentageModifier;
        private System.Windows.Forms.Label label4;
        private ItemStatsSheet AffixStatsSheet;
        private System.Windows.Forms.Label lblStatsModifier;
        private System.Windows.Forms.NumericUpDown nudAffixMinimumItemLevel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbAffixType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button fklblAffixNameLocale;
        private System.Windows.Forms.TextBox txtAffixName;
        private System.Windows.Forms.Label label27;
        private SingleActionEditor saeAffixOnTurnStart;
    }
}
