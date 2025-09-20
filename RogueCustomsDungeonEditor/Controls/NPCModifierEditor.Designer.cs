namespace RogueCustomsDungeonEditor.Controls
{
    partial class NPCModifierEditor
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(NPCModifierEditor));
            fklblNPCModifierNameLocale = new System.Windows.Forms.Button();
            txtNPCModifierName = new System.Windows.Forms.TextBox();
            label27 = new System.Windows.Forms.Label();
            txtNPCModifierId = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            NPCModifierStatsSheet = new ItemStatsSheet();
            lblStatsModifier = new System.Windows.Forms.Label();
            saeNPCModifierOnAttack = new SingleActionEditor();
            saeNPCModifierOnTurnStart = new SingleActionEditor();
            saeNPCModifierOnAttacked = new SingleActionEditor();
            label5 = new System.Windows.Forms.Label();
            nudNPCModifierMinDamage = new System.Windows.Forms.NumericUpDown();
            nudNPCModifierMaxDamage = new System.Windows.Forms.NumericUpDown();
            cmbNPCModifierElementDamage = new System.Windows.Forms.ComboBox();
            label6 = new System.Windows.Forms.Label();
            saeNPCModifierOnSpawn = new SingleActionEditor();
            saeNPCModifierOnDeath = new SingleActionEditor();
            btnNPCModifierColor = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)nudNPCModifierMinDamage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudNPCModifierMaxDamage).BeginInit();
            SuspendLayout();
            // 
            // fklblNPCModifierNameLocale
            // 
            fklblNPCModifierNameLocale.BackColor = System.Drawing.Color.Transparent;
            fklblNPCModifierNameLocale.Enabled = false;
            fklblNPCModifierNameLocale.FlatAppearance.BorderSize = 0;
            fklblNPCModifierNameLocale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblNPCModifierNameLocale.Image = (System.Drawing.Image)resources.GetObject("fklblNPCModifierNameLocale.Image");
            fklblNPCModifierNameLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblNPCModifierNameLocale.Location = new System.Drawing.Point(3, 105);
            fklblNPCModifierNameLocale.Name = "fklblNPCModifierNameLocale";
            fklblNPCModifierNameLocale.Size = new System.Drawing.Size(329, 43);
            fklblNPCModifierNameLocale.TabIndex = 142;
            fklblNPCModifierNameLocale.Text = "This value has been found as Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblNPCModifierNameLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblNPCModifierNameLocale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblNPCModifierNameLocale.UseVisualStyleBackColor = false;
            fklblNPCModifierNameLocale.Visible = false;
            // 
            // txtNPCModifierName
            // 
            txtNPCModifierName.Location = new System.Drawing.Point(3, 76);
            txtNPCModifierName.Name = "txtNPCModifierName";
            txtNPCModifierName.Size = new System.Drawing.Size(329, 23);
            txtNPCModifierName.TabIndex = 141;
            txtNPCModifierName.TextChanged += txtNPCModifierName_TextChanged;
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.BackColor = System.Drawing.Color.Transparent;
            label27.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            label27.Location = new System.Drawing.Point(3, 58);
            label27.Name = "label27";
            label27.Size = new System.Drawing.Size(91, 15);
            label27.TabIndex = 140;
            label27.Text = "Modifier Name";
            // 
            // txtNPCModifierId
            // 
            txtNPCModifierId.Location = new System.Drawing.Point(3, 23);
            txtNPCModifierId.Name = "txtNPCModifierId";
            txtNPCModifierId.Size = new System.Drawing.Size(329, 23);
            txtNPCModifierId.TabIndex = 144;
            txtNPCModifierId.TextChanged += txtNPCModifierId_TextChanged;
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
            // NPCModifierStatsSheet
            // 
            NPCModifierStatsSheet.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            NPCModifierStatsSheet.Location = new System.Drawing.Point(44, 191);
            NPCModifierStatsSheet.Name = "NPCModifierStatsSheet";
            NPCModifierStatsSheet.Size = new System.Drawing.Size(234, 119);
            NPCModifierStatsSheet.TabIndex = 250;
            // 
            // lblStatsModifier
            // 
            lblStatsModifier.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblStatsModifier.Location = new System.Drawing.Point(24, 149);
            lblStatsModifier.Name = "lblStatsModifier";
            lblStatsModifier.Size = new System.Drawing.Size(289, 52);
            lblStatsModifier.TabIndex = 249;
            lblStatsModifier.Text = "Modifies the NPC's following stats:";
            lblStatsModifier.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // saeNPCModifierOnAttack
            // 
            saeNPCModifierOnAttack.ActionDescription = "The NPC can interact with this:     ";
            saeNPCModifierOnAttack.ActionTypeText = "Interact";
            saeNPCModifierOnAttack.AutoSize = true;
            saeNPCModifierOnAttack.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saeNPCModifierOnAttack.ClassId = null;
            saeNPCModifierOnAttack.Location = new System.Drawing.Point(341, 172);
            saeNPCModifierOnAttack.Name = "saeNPCModifierOnAttack";
            saeNPCModifierOnAttack.PlaceholderActionId = "NPCModifierAttack";
            saeNPCModifierOnAttack.RequiresActionId = true;
            saeNPCModifierOnAttack.RequiresCondition = true;
            saeNPCModifierOnAttack.RequiresDescription = true;
            saeNPCModifierOnAttack.RequiresName = true;
            saeNPCModifierOnAttack.Size = new System.Drawing.Size(319, 32);
            saeNPCModifierOnAttack.SourceDescription = "The NPC";
            saeNPCModifierOnAttack.TabIndex = 254;
            saeNPCModifierOnAttack.TargetDescription = "Whoever they are targeting";
            saeNPCModifierOnAttack.ThisDescription = "The NPC";
            saeNPCModifierOnAttack.TurnEndCriteria = HelperForms.TurnEndCriteria.MustEndTurn;
            saeNPCModifierOnAttack.UsageCriteria = HelperForms.UsageCriteria.FullConditions;
            // 
            // saeNPCModifierOnTurnStart
            // 
            saeNPCModifierOnTurnStart.ActionDescription = "When the NPC starts a new turn...";
            saeNPCModifierOnTurnStart.ActionTypeText = "Turn Start";
            saeNPCModifierOnTurnStart.AutoSize = true;
            saeNPCModifierOnTurnStart.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saeNPCModifierOnTurnStart.ClassId = null;
            saeNPCModifierOnTurnStart.Location = new System.Drawing.Point(341, 140);
            saeNPCModifierOnTurnStart.Name = "saeNPCModifierOnTurnStart";
            saeNPCModifierOnTurnStart.PlaceholderActionId = "NPCModifierTurnStart";
            saeNPCModifierOnTurnStart.Size = new System.Drawing.Size(318, 32);
            saeNPCModifierOnTurnStart.SourceDescription = "The NPC";
            saeNPCModifierOnTurnStart.TabIndex = 256;
            saeNPCModifierOnTurnStart.TargetDescription = "The NPC";
            saeNPCModifierOnTurnStart.ThisDescription = "The NPC";
            // 
            // saeNPCModifierOnAttacked
            // 
            saeNPCModifierOnAttacked.ActionDescription = "When the NPC gets interacted...   ";
            saeNPCModifierOnAttacked.ActionTypeText = "Interacted";
            saeNPCModifierOnAttacked.AutoSize = true;
            saeNPCModifierOnAttacked.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saeNPCModifierOnAttacked.ClassId = null;
            saeNPCModifierOnAttacked.Location = new System.Drawing.Point(341, 204);
            saeNPCModifierOnAttacked.Name = "saeNPCModifierOnAttacked";
            saeNPCModifierOnAttacked.PlaceholderActionId = "NPCModifierInteracted";
            saeNPCModifierOnAttacked.Size = new System.Drawing.Size(318, 32);
            saeNPCModifierOnAttacked.SourceDescription = "The NPC";
            saeNPCModifierOnAttacked.TabIndex = 257;
            saeNPCModifierOnAttacked.TargetDescription = "The owner's interactor";
            saeNPCModifierOnAttacked.ThisDescription = "The NPC";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(341, 59);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(355, 15);
            label5.TabIndex = 258;
            label5.Text = "Adds                -                                               damage to DealDamage";
            // 
            // nudNPCModifierMinDamage
            // 
            nudNPCModifierMinDamage.Location = new System.Drawing.Point(375, 57);
            nudNPCModifierMinDamage.Name = "nudNPCModifierMinDamage";
            nudNPCModifierMinDamage.Size = new System.Drawing.Size(40, 23);
            nudNPCModifierMinDamage.TabIndex = 259;
            nudNPCModifierMinDamage.ValueChanged += nudNPCModifierMinDamage_ValueChanged;
            // 
            // nudNPCModifierMaxDamage
            // 
            nudNPCModifierMaxDamage.Location = new System.Drawing.Point(431, 57);
            nudNPCModifierMaxDamage.Name = "nudNPCModifierMaxDamage";
            nudNPCModifierMaxDamage.Size = new System.Drawing.Size(40, 23);
            nudNPCModifierMaxDamage.TabIndex = 260;
            nudNPCModifierMaxDamage.ValueChanged += nudNPCModifierMaxDamage_ValueChanged;
            // 
            // cmbNPCModifierElementDamage
            // 
            cmbNPCModifierElementDamage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbNPCModifierElementDamage.FormattingEnabled = true;
            cmbNPCModifierElementDamage.Location = new System.Drawing.Point(477, 57);
            cmbNPCModifierElementDamage.Name = "cmbNPCModifierElementDamage";
            cmbNPCModifierElementDamage.Size = new System.Drawing.Size(82, 23);
            cmbNPCModifierElementDamage.TabIndex = 261;
            cmbNPCModifierElementDamage.SelectedIndexChanged += cmbNPCModifierElementDamage_SelectedIndexChanged;
            cmbNPCModifierElementDamage.TextChanged += cmbNPCModifierElementDamage_TextChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(493, 80);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(205, 15);
            label6.TabIndex = 262;
            label6.Text = "(ignores defenses but not resistances)";
            // 
            // saeNPCModifierOnSpawn
            // 
            saeNPCModifierOnSpawn.ActionDescription = "When the NPC spawns...                ";
            saeNPCModifierOnSpawn.ActionTypeText = "Spawn";
            saeNPCModifierOnSpawn.AutoSize = true;
            saeNPCModifierOnSpawn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saeNPCModifierOnSpawn.ClassId = null;
            saeNPCModifierOnSpawn.Location = new System.Drawing.Point(342, 109);
            saeNPCModifierOnSpawn.Name = "saeNPCModifierOnSpawn";
            saeNPCModifierOnSpawn.PlaceholderActionId = "NPCModifierSpawn";
            saeNPCModifierOnSpawn.Size = new System.Drawing.Size(318, 32);
            saeNPCModifierOnSpawn.SourceDescription = "The NPC (won't become visible)";
            saeNPCModifierOnSpawn.TabIndex = 263;
            saeNPCModifierOnSpawn.TargetDescription = "The NPC (won't become visible)";
            saeNPCModifierOnSpawn.ThisDescription = "The NPC (won't become visible)";
            // 
            // saeNPCModifierOnDeath
            // 
            saeNPCModifierOnDeath.ActionDescription = "When the NPC dies...                      ";
            saeNPCModifierOnDeath.ActionTypeText = "Spawn";
            saeNPCModifierOnDeath.AutoSize = true;
            saeNPCModifierOnDeath.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saeNPCModifierOnDeath.ClassId = null;
            saeNPCModifierOnDeath.Location = new System.Drawing.Point(341, 239);
            saeNPCModifierOnDeath.Name = "saeNPCModifierOnDeath";
            saeNPCModifierOnDeath.PlaceholderActionId = "NPCModifierSpawn";
            saeNPCModifierOnDeath.Size = new System.Drawing.Size(318, 32);
            saeNPCModifierOnDeath.SourceDescription = "The NPC";
            saeNPCModifierOnDeath.TabIndex = 264;
            saeNPCModifierOnDeath.TargetDescription = "Whoever killed them (if any)";
            saeNPCModifierOnDeath.ThisDescription = "The NPC";
            // 
            // btnNPCModifierColor
            // 
            btnNPCModifierColor.BackColor = System.Drawing.Color.White;
            btnNPCModifierColor.Location = new System.Drawing.Point(557, 23);
            btnNPCModifierColor.Name = "btnNPCModifierColor";
            btnNPCModifierColor.Size = new System.Drawing.Size(24, 24);
            btnNPCModifierColor.TabIndex = 266;
            btnNPCModifierColor.UseVisualStyleBackColor = false;
            btnNPCModifierColor.Click += btnNPCModifierColor_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(343, 26);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(211, 15);
            label2.TabIndex = 265;
            label2.Text = "Modifier will be printed with this color:";
            // 
            // NPCModifierEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(btnNPCModifierColor);
            Controls.Add(label2);
            Controls.Add(saeNPCModifierOnDeath);
            Controls.Add(saeNPCModifierOnSpawn);
            Controls.Add(label6);
            Controls.Add(cmbNPCModifierElementDamage);
            Controls.Add(nudNPCModifierMaxDamage);
            Controls.Add(nudNPCModifierMinDamage);
            Controls.Add(label5);
            Controls.Add(saeNPCModifierOnAttacked);
            Controls.Add(saeNPCModifierOnAttack);
            Controls.Add(NPCModifierStatsSheet);
            Controls.Add(lblStatsModifier);
            Controls.Add(txtNPCModifierId);
            Controls.Add(label1);
            Controls.Add(fklblNPCModifierNameLocale);
            Controls.Add(txtNPCModifierName);
            Controls.Add(label27);
            Controls.Add(saeNPCModifierOnTurnStart);
            Name = "NPCModifierEditor";
            Size = new System.Drawing.Size(699, 313);
            ((System.ComponentModel.ISupportInitialize)nudNPCModifierMinDamage).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudNPCModifierMaxDamage).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button fklblNPCModifierNameLocale;
        private System.Windows.Forms.TextBox txtNPCModifierName;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox txtNPCModifierId;
        private System.Windows.Forms.Label label1;
        private ItemStatsSheet NPCModifierStatsSheet;
        private System.Windows.Forms.Label lblStatsModifier;
        private SingleActionEditor saeNPCModifierOnAttack;
        private SingleActionEditor saeNPCModifierOnTurnStart;
        private SingleActionEditor saeNPCModifierOnAttacked;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudNPCModifierMinDamage;
        private System.Windows.Forms.NumericUpDown nudNPCModifierMaxDamage;
        private System.Windows.Forms.ComboBox cmbNPCModifierElementDamage;
        private System.Windows.Forms.Label label6;
        private SingleActionEditor saeNPCModifierOnSpawn;
        private SingleActionEditor saeNPCModifierOnDeath;
        private System.Windows.Forms.Button btnNPCModifierColor;
        private System.Windows.Forms.Label label2;
    }
}
