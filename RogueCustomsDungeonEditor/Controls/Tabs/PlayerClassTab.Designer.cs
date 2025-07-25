﻿namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    partial class PlayerClassTab
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayerClassTab));
            ssPlayer = new StatsSheet();
            sisPlayerStartingInventory = new StartingInventorySelector();
            saePlayerOnDeath = new SingleActionEditor();
            saePlayerOnAttacked = new SingleActionEditor();
            saePlayerOnTurnStart = new SingleActionEditor();
            maePlayerOnAttack = new MultiActionEditor();
            cmbPlayerStartingArmor = new System.Windows.Forms.ComboBox();
            label57 = new System.Windows.Forms.Label();
            cmbPlayerStartingWeapon = new System.Windows.Forms.ComboBox();
            label56 = new System.Windows.Forms.Label();
            label54 = new System.Windows.Forms.Label();
            nudPlayerInventorySize = new System.Windows.Forms.NumericUpDown();
            label53 = new System.Windows.Forms.Label();
            label30 = new System.Windows.Forms.Label();
            chkPlayerStartsVisible = new System.Windows.Forms.CheckBox();
            cmbPlayerFaction = new System.Windows.Forms.ComboBox();
            label29 = new System.Windows.Forms.Label();
            chkRequirePlayerPrompt = new System.Windows.Forms.CheckBox();
            fklblPlayerClassDescriptionLocale = new System.Windows.Forms.Button();
            txtPlayerClassDescription = new System.Windows.Forms.TextBox();
            label28 = new System.Windows.Forms.Label();
            fklblPlayerClassNameLocale = new System.Windows.Forms.Button();
            txtPlayerClassName = new System.Windows.Forms.TextBox();
            label27 = new System.Windows.Forms.Label();
            crsPlayer = new ConsoleRepresentationSelector();
            label67 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            saePlayerOnLevelUp = new SingleActionEditor();
            ((System.ComponentModel.ISupportInitialize)nudPlayerInventorySize).BeginInit();
            SuspendLayout();
            // 
            // ssPlayer
            // 
            ssPlayer.AutoSize = true;
            ssPlayer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ssPlayer.BaseSightRangeDisplayNames = (System.Collections.Generic.Dictionary<string, string>)resources.GetObject("ssPlayer.BaseSightRangeDisplayNames");
            ssPlayer.CanGainExperience = false;
            ssPlayer.ExperienceToLevelUpFormula = "";
            ssPlayer.Location = new System.Drawing.Point(380, 91);
            ssPlayer.MaxLevel = 1;
            ssPlayer.Name = "ssPlayer";
            ssPlayer.Size = new System.Drawing.Size(303, 408);
            ssPlayer.StatInfos = null;
            ssPlayer.TabIndex = 154;
            // 
            // sisPlayerStartingInventory
            // 
            sisPlayerStartingInventory.Inventory = (System.Collections.Generic.List<string>)resources.GetObject("sisPlayerStartingInventory.Inventory");
            sisPlayerStartingInventory.InventorySize = 0;
            sisPlayerStartingInventory.Location = new System.Drawing.Point(387, 589);
            sisPlayerStartingInventory.Name = "sisPlayerStartingInventory";
            sisPlayerStartingInventory.SelectableItems = null;
            sisPlayerStartingInventory.Size = new System.Drawing.Size(293, 79);
            sisPlayerStartingInventory.TabIndex = 153;
            // 
            // saePlayerOnDeath
            // 
            saePlayerOnDeath.Action = null;
            saePlayerOnDeath.ActionDescription = "When they die...                   ";
            saePlayerOnDeath.ActionTypeText = "On Death";
            saePlayerOnDeath.AutoSize = true;
            saePlayerOnDeath.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saePlayerOnDeath.ClassId = null;
            saePlayerOnDeath.Dungeon = null;
            saePlayerOnDeath.EffectParamData = null;
            saePlayerOnDeath.Location = new System.Drawing.Point(2, 499);
            saePlayerOnDeath.Name = "saePlayerOnDeath";
            saePlayerOnDeath.PlaceholderActionId = "Death";
            saePlayerOnDeath.RequiresActionId = false;
            saePlayerOnDeath.RequiresCondition = false;
            saePlayerOnDeath.RequiresDescription = false;
            saePlayerOnDeath.RequiresName = false;
            saePlayerOnDeath.Size = new System.Drawing.Size(283, 32);
            saePlayerOnDeath.SourceDescription = "The player";
            saePlayerOnDeath.TabIndex = 152;
            saePlayerOnDeath.TargetDescription = "Whoever killed them (if any)";
            saePlayerOnDeath.ThisDescription = "The player";
            saePlayerOnDeath.TurnEndCriteria = HelperForms.TurnEndCriteria.CannotEndTurn;
            saePlayerOnDeath.UsageCriteria = HelperForms.UsageCriteria.AnyTargetAnyTime;
            // 
            // saePlayerOnAttacked
            // 
            saePlayerOnAttacked.Action = null;
            saePlayerOnAttacked.ActionDescription = "When they get attacked...   ";
            saePlayerOnAttacked.ActionTypeText = "Interacted";
            saePlayerOnAttacked.AutoSize = true;
            saePlayerOnAttacked.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saePlayerOnAttacked.ClassId = null;
            saePlayerOnAttacked.Dungeon = null;
            saePlayerOnAttacked.EffectParamData = null;
            saePlayerOnAttacked.Location = new System.Drawing.Point(2, 461);
            saePlayerOnAttacked.Name = "saePlayerOnAttacked";
            saePlayerOnAttacked.PlaceholderActionId = "Interacted";
            saePlayerOnAttacked.RequiresActionId = false;
            saePlayerOnAttacked.RequiresCondition = false;
            saePlayerOnAttacked.RequiresDescription = false;
            saePlayerOnAttacked.RequiresName = false;
            saePlayerOnAttacked.Size = new System.Drawing.Size(284, 32);
            saePlayerOnAttacked.SourceDescription = "The player";
            saePlayerOnAttacked.TabIndex = 151;
            saePlayerOnAttacked.TargetDescription = "Whoever interacted with them";
            saePlayerOnAttacked.ThisDescription = "The player";
            saePlayerOnAttacked.TurnEndCriteria = HelperForms.TurnEndCriteria.CannotEndTurn;
            saePlayerOnAttacked.UsageCriteria = HelperForms.UsageCriteria.AnyTargetAnyTime;
            // 
            // saePlayerOnTurnStart
            // 
            saePlayerOnTurnStart.Action = null;
            saePlayerOnTurnStart.ActionDescription = "When the next turn starts...";
            saePlayerOnTurnStart.ActionTypeText = "Turn Start";
            saePlayerOnTurnStart.AutoSize = true;
            saePlayerOnTurnStart.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saePlayerOnTurnStart.ClassId = null;
            saePlayerOnTurnStart.Dungeon = null;
            saePlayerOnTurnStart.EffectParamData = null;
            saePlayerOnTurnStart.Location = new System.Drawing.Point(2, 323);
            saePlayerOnTurnStart.Name = "saePlayerOnTurnStart";
            saePlayerOnTurnStart.PlaceholderActionId = "TurnStart";
            saePlayerOnTurnStart.RequiresActionId = false;
            saePlayerOnTurnStart.RequiresCondition = false;
            saePlayerOnTurnStart.RequiresDescription = false;
            saePlayerOnTurnStart.RequiresName = false;
            saePlayerOnTurnStart.Size = new System.Drawing.Size(283, 32);
            saePlayerOnTurnStart.SourceDescription = "The player";
            saePlayerOnTurnStart.TabIndex = 150;
            saePlayerOnTurnStart.TargetDescription = "The player";
            saePlayerOnTurnStart.ThisDescription = "The player";
            saePlayerOnTurnStart.TurnEndCriteria = HelperForms.TurnEndCriteria.CannotEndTurn;
            saePlayerOnTurnStart.UsageCriteria = HelperForms.UsageCriteria.AnyTargetAnyTime;
            // 
            // maePlayerOnAttack
            // 
            maePlayerOnAttack.ActionDescription = "Can do the following to\r\ninteract with someone:";
            maePlayerOnAttack.Actions = (System.Collections.Generic.List<RogueCustomsGameEngine.Utils.JsonImports.ActionWithEffectsInfo>)resources.GetObject("maePlayerOnAttack.Actions");
            maePlayerOnAttack.ActionTypeText = "Interact";
            maePlayerOnAttack.ClassId = null;
            maePlayerOnAttack.Dungeon = null;
            maePlayerOnAttack.EffectParamData = null;
            maePlayerOnAttack.Location = new System.Drawing.Point(2, 361);
            maePlayerOnAttack.Name = "maePlayerOnAttack";
            maePlayerOnAttack.PlaceholderActionName = null;
            maePlayerOnAttack.RequiresActionId = true;
            maePlayerOnAttack.RequiresActionName = true;
            maePlayerOnAttack.RequiresCondition = true;
            maePlayerOnAttack.RequiresDescription = true;
            maePlayerOnAttack.Size = new System.Drawing.Size(368, 94);
            maePlayerOnAttack.SourceDescription = "The player";
            maePlayerOnAttack.TabIndex = 149;
            maePlayerOnAttack.TargetDescription = "Whoever they are targeting";
            maePlayerOnAttack.ThisDescription = "The player";
            maePlayerOnAttack.TurnEndCriteria = HelperForms.TurnEndCriteria.MayNotEndTurn;
            maePlayerOnAttack.UsageCriteria = HelperForms.UsageCriteria.FullConditions;
            // 
            // cmbPlayerStartingArmor
            // 
            cmbPlayerStartingArmor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbPlayerStartingArmor.FormattingEnabled = true;
            cmbPlayerStartingArmor.Location = new System.Drawing.Point(522, 706);
            cmbPlayerStartingArmor.Name = "cmbPlayerStartingArmor";
            cmbPlayerStartingArmor.Size = new System.Drawing.Size(158, 23);
            cmbPlayerStartingArmor.TabIndex = 147;
            cmbPlayerStartingArmor.SelectedIndexChanged += cmbPlayerStartingArmor_SelectedIndexChanged;
            // 
            // label57
            // 
            label57.AutoSize = true;
            label57.Location = new System.Drawing.Point(387, 709);
            label57.Name = "label57";
            label57.Size = new System.Drawing.Size(131, 15);
            label57.TabIndex = 146;
            label57.Text = "When unarmored, wear";
            // 
            // cmbPlayerStartingWeapon
            // 
            cmbPlayerStartingWeapon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbPlayerStartingWeapon.FormattingEnabled = true;
            cmbPlayerStartingWeapon.Location = new System.Drawing.Point(515, 675);
            cmbPlayerStartingWeapon.Name = "cmbPlayerStartingWeapon";
            cmbPlayerStartingWeapon.Size = new System.Drawing.Size(165, 23);
            cmbPlayerStartingWeapon.TabIndex = 145;
            cmbPlayerStartingWeapon.SelectedIndexChanged += cmbPlayerStartingWeapon_SelectedIndexChanged;
            // 
            // label56
            // 
            label56.AutoSize = true;
            label56.Location = new System.Drawing.Point(387, 678);
            label56.Name = "label56";
            label56.Size = new System.Drawing.Size(123, 15);
            label56.TabIndex = 144;
            label56.Text = "When unarmed, wield";
            // 
            // label54
            // 
            label54.AutoSize = true;
            label54.Location = new System.Drawing.Point(546, 557);
            label54.Name = "label54";
            label54.Size = new System.Drawing.Size(36, 15);
            label54.TabIndex = 143;
            label54.Text = "items";
            // 
            // nudPlayerInventorySize
            // 
            nudPlayerInventorySize.Location = new System.Drawing.Point(495, 552);
            nudPlayerInventorySize.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            nudPlayerInventorySize.Name = "nudPlayerInventorySize";
            nudPlayerInventorySize.Size = new System.Drawing.Size(45, 23);
            nudPlayerInventorySize.TabIndex = 142;
            nudPlayerInventorySize.ValueChanged += nudPlayerInventorySize_ValueChanged;
            // 
            // label53
            // 
            label53.AutoSize = true;
            label53.Location = new System.Drawing.Point(387, 555);
            label53.Name = "label53";
            label53.Size = new System.Drawing.Size(109, 15);
            label53.TabIndex = 141;
            label53.Text = "Inventory Capacity:";
            // 
            // label30
            // 
            label30.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label30.Location = new System.Drawing.Point(365, 16);
            label30.Name = "label30";
            label30.Size = new System.Drawing.Size(131, 52);
            label30.TabIndex = 140;
            label30.Text = "Appearance";
            label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkPlayerStartsVisible
            // 
            chkPlayerStartsVisible.AutoSize = true;
            chkPlayerStartsVisible.Location = new System.Drawing.Point(8, 269);
            chkPlayerStartsVisible.Name = "chkPlayerStartsVisible";
            chkPlayerStartsVisible.Size = new System.Drawing.Size(102, 19);
            chkPlayerStartsVisible.TabIndex = 139;
            chkPlayerStartsVisible.Text = "Spawns visible";
            chkPlayerStartsVisible.UseVisualStyleBackColor = true;
            chkPlayerStartsVisible.CheckedChanged += chkPlayerStartsVisible_CheckedChanged;
            // 
            // cmbPlayerFaction
            // 
            cmbPlayerFaction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbPlayerFaction.FormattingEnabled = true;
            cmbPlayerFaction.Location = new System.Drawing.Point(60, 238);
            cmbPlayerFaction.Name = "cmbPlayerFaction";
            cmbPlayerFaction.Size = new System.Drawing.Size(146, 23);
            cmbPlayerFaction.TabIndex = 138;
            cmbPlayerFaction.SelectedIndexChanged += cmbPlayerFaction_SelectedIndexChanged;
            // 
            // label29
            // 
            label29.AutoSize = true;
            label29.Location = new System.Drawing.Point(8, 241);
            label29.Name = "label29";
            label29.Size = new System.Drawing.Size(46, 15);
            label29.TabIndex = 137;
            label29.Text = "Faction";
            // 
            // chkRequirePlayerPrompt
            // 
            chkRequirePlayerPrompt.AutoSize = true;
            chkRequirePlayerPrompt.Location = new System.Drawing.Point(8, 104);
            chkRequirePlayerPrompt.Name = "chkRequirePlayerPrompt";
            chkRequirePlayerPrompt.Size = new System.Drawing.Size(287, 19);
            chkRequirePlayerPrompt.TabIndex = 136;
            chkRequirePlayerPrompt.Text = "Player will have to provide a name upon selection";
            chkRequirePlayerPrompt.UseVisualStyleBackColor = true;
            chkRequirePlayerPrompt.CheckedChanged += chkRequirePlayerPrompt_CheckedChanged;
            // 
            // fklblPlayerClassDescriptionLocale
            // 
            fklblPlayerClassDescriptionLocale.Enabled = false;
            fklblPlayerClassDescriptionLocale.FlatAppearance.BorderSize = 0;
            fklblPlayerClassDescriptionLocale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblPlayerClassDescriptionLocale.Image = (System.Drawing.Image)resources.GetObject("fklblPlayerClassDescriptionLocale.Image");
            fklblPlayerClassDescriptionLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblPlayerClassDescriptionLocale.Location = new System.Drawing.Point(8, 182);
            fklblPlayerClassDescriptionLocale.Name = "fklblPlayerClassDescriptionLocale";
            fklblPlayerClassDescriptionLocale.Size = new System.Drawing.Size(331, 42);
            fklblPlayerClassDescriptionLocale.TabIndex = 135;
            fklblPlayerClassDescriptionLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblPlayerClassDescriptionLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblPlayerClassDescriptionLocale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblPlayerClassDescriptionLocale.UseVisualStyleBackColor = true;
            fklblPlayerClassDescriptionLocale.Visible = false;
            // 
            // txtPlayerClassDescription
            // 
            txtPlayerClassDescription.Location = new System.Drawing.Point(8, 153);
            txtPlayerClassDescription.Name = "txtPlayerClassDescription";
            txtPlayerClassDescription.Size = new System.Drawing.Size(350, 23);
            txtPlayerClassDescription.TabIndex = 134;
            txtPlayerClassDescription.TextChanged += txtPlayerClassDescription_TextChanged;
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.Location = new System.Drawing.Point(8, 135);
            label28.Name = "label28";
            label28.Size = new System.Drawing.Size(67, 15);
            label28.TabIndex = 133;
            label28.Text = "Description";
            // 
            // fklblPlayerClassNameLocale
            // 
            fklblPlayerClassNameLocale.Enabled = false;
            fklblPlayerClassNameLocale.FlatAppearance.BorderSize = 0;
            fklblPlayerClassNameLocale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblPlayerClassNameLocale.Image = (System.Drawing.Image)resources.GetObject("fklblPlayerClassNameLocale.Image");
            fklblPlayerClassNameLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblPlayerClassNameLocale.Location = new System.Drawing.Point(8, 52);
            fklblPlayerClassNameLocale.Name = "fklblPlayerClassNameLocale";
            fklblPlayerClassNameLocale.Size = new System.Drawing.Size(331, 42);
            fklblPlayerClassNameLocale.TabIndex = 132;
            fklblPlayerClassNameLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblPlayerClassNameLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblPlayerClassNameLocale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblPlayerClassNameLocale.UseVisualStyleBackColor = true;
            fklblPlayerClassNameLocale.Visible = false;
            // 
            // txtPlayerClassName
            // 
            txtPlayerClassName.Location = new System.Drawing.Point(8, 23);
            txtPlayerClassName.Name = "txtPlayerClassName";
            txtPlayerClassName.Size = new System.Drawing.Size(350, 23);
            txtPlayerClassName.TabIndex = 131;
            txtPlayerClassName.TextChanged += txtPlayerClassName_TextChanged;
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.Location = new System.Drawing.Point(8, 5);
            label27.Name = "label27";
            label27.Size = new System.Drawing.Size(80, 15);
            label27.TabIndex = 130;
            label27.Text = "Default Name";
            // 
            // crsPlayer
            // 
            crsPlayer.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("crsPlayer.BackgroundColor");
            crsPlayer.Character = '\0';
            crsPlayer.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("crsPlayer.ForegroundColor");
            crsPlayer.Location = new System.Drawing.Point(500, 5);
            crsPlayer.Name = "crsPlayer";
            crsPlayer.Size = new System.Drawing.Size(211, 83);
            crsPlayer.TabIndex = 155;
            crsPlayer.PropertyChanged += crsPlayer_PropertyChanged;
            // 
            // label67
            // 
            label67.AutoSize = true;
            label67.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label67.Location = new System.Drawing.Point(128, 299);
            label67.Name = "label67";
            label67.Size = new System.Drawing.Size(67, 21);
            label67.TabIndex = 237;
            label67.Text = "Actions";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label1.Location = new System.Drawing.Point(482, 516);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(85, 21);
            label1.TabIndex = 238;
            label1.Text = "Inventory";
            // 
            // saePlayerOnLevelUp
            // 
            saePlayerOnLevelUp.Action = null;
            saePlayerOnLevelUp.ActionDescription = "When they level up...           ";
            saePlayerOnLevelUp.ActionTypeText = "On Level Up";
            saePlayerOnLevelUp.AutoSize = true;
            saePlayerOnLevelUp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saePlayerOnLevelUp.ClassId = null;
            saePlayerOnLevelUp.Dungeon = null;
            saePlayerOnLevelUp.EffectParamData = null;
            saePlayerOnLevelUp.Location = new System.Drawing.Point(2, 537);
            saePlayerOnLevelUp.Name = "saePlayerOnLevelUp";
            saePlayerOnLevelUp.PlaceholderActionId = "LevelUp";
            saePlayerOnLevelUp.RequiresActionId = false;
            saePlayerOnLevelUp.RequiresCondition = false;
            saePlayerOnLevelUp.RequiresDescription = false;
            saePlayerOnLevelUp.RequiresName = false;
            saePlayerOnLevelUp.Size = new System.Drawing.Size(284, 32);
            saePlayerOnLevelUp.SourceDescription = "The player";
            saePlayerOnLevelUp.TabIndex = 239;
            saePlayerOnLevelUp.TargetDescription = "The player";
            saePlayerOnLevelUp.ThisDescription = "The player";
            saePlayerOnLevelUp.TurnEndCriteria = HelperForms.TurnEndCriteria.CannotEndTurn;
            saePlayerOnLevelUp.UsageCriteria = HelperForms.UsageCriteria.AnyTargetAnyTime;
            // 
            // PlayerClassTab
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Controls.Add(saePlayerOnLevelUp);
            Controls.Add(label1);
            Controls.Add(label67);
            Controls.Add(crsPlayer);
            Controls.Add(ssPlayer);
            Controls.Add(sisPlayerStartingInventory);
            Controls.Add(saePlayerOnDeath);
            Controls.Add(saePlayerOnAttacked);
            Controls.Add(saePlayerOnTurnStart);
            Controls.Add(maePlayerOnAttack);
            Controls.Add(cmbPlayerStartingArmor);
            Controls.Add(label57);
            Controls.Add(cmbPlayerStartingWeapon);
            Controls.Add(label56);
            Controls.Add(label54);
            Controls.Add(nudPlayerInventorySize);
            Controls.Add(label53);
            Controls.Add(label30);
            Controls.Add(chkPlayerStartsVisible);
            Controls.Add(cmbPlayerFaction);
            Controls.Add(label29);
            Controls.Add(chkRequirePlayerPrompt);
            Controls.Add(fklblPlayerClassDescriptionLocale);
            Controls.Add(txtPlayerClassDescription);
            Controls.Add(label28);
            Controls.Add(fklblPlayerClassNameLocale);
            Controls.Add(txtPlayerClassName);
            Controls.Add(label27);
            Name = "PlayerClassTab";
            Size = new System.Drawing.Size(714, 740);
            ((System.ComponentModel.ISupportInitialize)nudPlayerInventorySize).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private StatsSheet ssPlayer;
        private StartingInventorySelector sisPlayerStartingInventory;
        private SingleActionEditor saePlayerOnDeath;
        private SingleActionEditor saePlayerOnAttacked;
        private SingleActionEditor saePlayerOnTurnStart;
        private MultiActionEditor maePlayerOnAttack;
        private System.Windows.Forms.ComboBox cmbPlayerStartingArmor;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.ComboBox cmbPlayerStartingWeapon;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.NumericUpDown nudPlayerInventorySize;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.CheckBox chkPlayerStartsVisible;
        private System.Windows.Forms.ComboBox cmbPlayerFaction;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.CheckBox chkRequirePlayerPrompt;
        private System.Windows.Forms.Button fklblPlayerClassDescriptionLocale;
        private System.Windows.Forms.TextBox txtPlayerClassDescription;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Button fklblPlayerClassNameLocale;
        private System.Windows.Forms.TextBox txtPlayerClassName;
        private System.Windows.Forms.Label label27;
        private ConsoleRepresentationSelector crsPlayer;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.Label label1;
        private SingleActionEditor saePlayerOnLevelUp;
    }
}
