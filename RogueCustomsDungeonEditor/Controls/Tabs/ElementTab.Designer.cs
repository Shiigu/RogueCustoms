namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    partial class ElementTab
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(ElementTab));
            fklblElementNameLocale = new System.Windows.Forms.Button();
            txtElementName = new System.Windows.Forms.TextBox();
            label101 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            cmbElementResistanceStat = new System.Windows.Forms.ComboBox();
            chkExcessResistanceCausesHealDamage = new System.Windows.Forms.CheckBox();
            saeElementOnAfterAttack = new SingleActionEditor();
            label1 = new System.Windows.Forms.Label();
            btnElementColor = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // fklblElementNameLocale
            // 
            fklblElementNameLocale.Enabled = false;
            fklblElementNameLocale.FlatAppearance.BorderSize = 0;
            fklblElementNameLocale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblElementNameLocale.Image = (System.Drawing.Image)resources.GetObject("fklblElementNameLocale.Image");
            fklblElementNameLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblElementNameLocale.Location = new System.Drawing.Point(3, 52);
            fklblElementNameLocale.Name = "fklblElementNameLocale";
            fklblElementNameLocale.Size = new System.Drawing.Size(331, 42);
            fklblElementNameLocale.TabIndex = 226;
            fklblElementNameLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblElementNameLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblElementNameLocale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblElementNameLocale.UseVisualStyleBackColor = true;
            fklblElementNameLocale.Visible = false;
            // 
            // txtElementName
            // 
            txtElementName.Location = new System.Drawing.Point(3, 23);
            txtElementName.Name = "txtElementName";
            txtElementName.Size = new System.Drawing.Size(350, 23);
            txtElementName.TabIndex = 225;
            txtElementName.TextChanged += txtElementName_TextChanged;
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
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(3, 143);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(212, 15);
            label2.TabIndex = 230;
            label2.Text = "The following Stat serves as Resistance:";
            // 
            // cmbElementResistanceStat
            // 
            cmbElementResistanceStat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbElementResistanceStat.FormattingEnabled = true;
            cmbElementResistanceStat.Location = new System.Drawing.Point(221, 140);
            cmbElementResistanceStat.Name = "cmbElementResistanceStat";
            cmbElementResistanceStat.Size = new System.Drawing.Size(132, 23);
            cmbElementResistanceStat.TabIndex = 231;
            cmbElementResistanceStat.SelectedIndexChanged += cmbElementResistanceStat_SelectedIndexChanged;
            // 
            // chkExcessResistanceCausesHealDamage
            // 
            chkExcessResistanceCausesHealDamage.AutoSize = true;
            chkExcessResistanceCausesHealDamage.Location = new System.Drawing.Point(3, 176);
            chkExcessResistanceCausesHealDamage.Name = "chkExcessResistanceCausesHealDamage";
            chkExcessResistanceCausesHealDamage.Size = new System.Drawing.Size(383, 19);
            chkExcessResistanceCausesHealDamage.TabIndex = 232;
            chkExcessResistanceCausesHealDamage.Text = "If Resistance exceeds Damage, call HealDamage with the difference.";
            chkExcessResistanceCausesHealDamage.UseVisualStyleBackColor = true;
            chkExcessResistanceCausesHealDamage.CheckedChanged += chkExcessResistanceCausesHealDamage_CheckedChanged;
            // 
            // saeElementOnAfterAttack
            // 
            saeElementOnAfterAttack.Action = null;
            saeElementOnAfterAttack.ActionDescription = "After trying to DealDamage\r\nwith this Element...";
            saeElementOnAfterAttack.ActionTypeText = "After Attack";
            saeElementOnAfterAttack.AutoSize = true;
            saeElementOnAfterAttack.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saeElementOnAfterAttack.ClassId = null;
            saeElementOnAfterAttack.Dungeon = null;
            saeElementOnAfterAttack.EffectParamData = null;
            saeElementOnAfterAttack.Location = new System.Drawing.Point(369, 131);
            saeElementOnAfterAttack.Name = "saeElementOnAfterAttack";
            saeElementOnAfterAttack.PlaceholderActionName = "ElementAfterAttack";
            saeElementOnAfterAttack.RequiresActionName = false;
            saeElementOnAfterAttack.RequiresCondition = true;
            saeElementOnAfterAttack.RequiresDescription = false;
            saeElementOnAfterAttack.Size = new System.Drawing.Size(285, 32);
            saeElementOnAfterAttack.SourceDescription = "The attacker";
            saeElementOnAfterAttack.TabIndex = 233;
            saeElementOnAfterAttack.TargetDescription = "The attacked";
            saeElementOnAfterAttack.ThisDescription = "The Element";
            saeElementOnAfterAttack.TurnEndCriteria = HelperForms.TurnEndCriteria.CannotEndTurn;
            saeElementOnAfterAttack.UsageCriteria = HelperForms.UsageCriteria.AnyTargetAnyTime;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(4, 109);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(210, 15);
            label1.TabIndex = 234;
            label1.Text = "Damage will be printed with this color:";
            // 
            // btnElementColor
            // 
            btnElementColor.BackColor = System.Drawing.Color.White;
            btnElementColor.Location = new System.Drawing.Point(218, 106);
            btnElementColor.Name = "btnElementColor";
            btnElementColor.Size = new System.Drawing.Size(24, 24);
            btnElementColor.TabIndex = 235;
            btnElementColor.UseVisualStyleBackColor = false;
            btnElementColor.Click += btnElementColor_Click;
            // 
            // ElementTab
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Controls.Add(btnElementColor);
            Controls.Add(label1);
            Controls.Add(saeElementOnAfterAttack);
            Controls.Add(chkExcessResistanceCausesHealDamage);
            Controls.Add(cmbElementResistanceStat);
            Controls.Add(label2);
            Controls.Add(fklblElementNameLocale);
            Controls.Add(txtElementName);
            Controls.Add(label101);
            Name = "ElementTab";
            Size = new System.Drawing.Size(657, 198);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button fklblElementNameLocale;
        private System.Windows.Forms.TextBox txtElementName;
        private System.Windows.Forms.Label label101;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbElementResistanceStat;
        private System.Windows.Forms.CheckBox chkExcessResistanceCausesHealDamage;
        private SingleActionEditor saeElementOnAfterAttack;
        private System.Windows.Forms.Button btnElementColor;
    }
}
