namespace RogueCustomsDungeonEditor.Controls.Tabs
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
            label4 = new System.Windows.Forms.Label();
            nudPlayerSaleValuePercentage = new System.Windows.Forms.NumericUpDown();
            saePlayerDefaultOnAttack = new SingleActionEditor();
            esPlayer = new EquipmentSheet();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            clbPlayerAvailableSlots = new System.Windows.Forms.CheckedListBox();
            chkPlayerNeedsToIdentifyItems = new System.Windows.Forms.CheckBox();
            cmbPlayerLearnset = new System.Windows.Forms.ComboBox();
            label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)nudPlayerInventorySize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudPlayerSaleValuePercentage).BeginInit();
            SuspendLayout();
            // 
            // ssPlayer
            // 
            ssPlayer.AutoSize = true;
            ssPlayer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ssPlayer.Location = new System.Drawing.Point(380, 91);
            ssPlayer.Name = "ssPlayer";
            ssPlayer.Size = new System.Drawing.Size(405, 494);
            ssPlayer.TabIndex = 154;
            // 
            // sisPlayerStartingInventory
            // 
            sisPlayerStartingInventory.Location = new System.Drawing.Point(434, 629);
            sisPlayerStartingInventory.Name = "sisPlayerStartingInventory";
            sisPlayerStartingInventory.Size = new System.Drawing.Size(293, 79);
            sisPlayerStartingInventory.TabIndex = 153;
            // 
            // saePlayerOnDeath
            // 
            saePlayerOnDeath.ActionDescription = "When they die...                         ";
            saePlayerOnDeath.ActionTypeText = "On Death";
            saePlayerOnDeath.AutoSize = true;
            saePlayerOnDeath.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saePlayerOnDeath.ClassId = null;
            saePlayerOnDeath.Location = new System.Drawing.Point(8, 604);
            saePlayerOnDeath.Name = "saePlayerOnDeath";
            saePlayerOnDeath.PlaceholderActionId = "Death";
            saePlayerOnDeath.Size = new System.Drawing.Size(301, 32);
            saePlayerOnDeath.SourceDescription = "The player";
            saePlayerOnDeath.TabIndex = 152;
            saePlayerOnDeath.TargetDescription = "Whoever killed them (if any)";
            saePlayerOnDeath.ThisDescription = "The player";
            // 
            // saePlayerOnAttacked
            // 
            saePlayerOnAttacked.ActionDescription = "When they get attacked...        ";
            saePlayerOnAttacked.ActionTypeText = "Interacted";
            saePlayerOnAttacked.AutoSize = true;
            saePlayerOnAttacked.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saePlayerOnAttacked.ClassId = null;
            saePlayerOnAttacked.Location = new System.Drawing.Point(8, 566);
            saePlayerOnAttacked.Name = "saePlayerOnAttacked";
            saePlayerOnAttacked.PlaceholderActionId = "Interacted";
            saePlayerOnAttacked.Size = new System.Drawing.Size(299, 32);
            saePlayerOnAttacked.SourceDescription = "The player";
            saePlayerOnAttacked.TabIndex = 151;
            saePlayerOnAttacked.TargetDescription = "Whoever interacted with them";
            saePlayerOnAttacked.ThisDescription = "The player";
            // 
            // saePlayerOnTurnStart
            // 
            saePlayerOnTurnStart.ActionDescription = "When the next turn starts...       ";
            saePlayerOnTurnStart.ActionTypeText = "Turn Start";
            saePlayerOnTurnStart.AutoSize = true;
            saePlayerOnTurnStart.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saePlayerOnTurnStart.ClassId = null;
            saePlayerOnTurnStart.Location = new System.Drawing.Point(2, 384);
            saePlayerOnTurnStart.Name = "saePlayerOnTurnStart";
            saePlayerOnTurnStart.PlaceholderActionId = "TurnStart";
            saePlayerOnTurnStart.Size = new System.Drawing.Size(303, 32);
            saePlayerOnTurnStart.SourceDescription = "The player";
            saePlayerOnTurnStart.TabIndex = 150;
            saePlayerOnTurnStart.TargetDescription = "The player";
            saePlayerOnTurnStart.ThisDescription = "The player";
            // 
            // maePlayerOnAttack
            // 
            maePlayerOnAttack.ActionDescription = "Can do the following to\r\ninteract with someone:";
            maePlayerOnAttack.ActionTypeText = "Interact";
            maePlayerOnAttack.ClassId = null;
            maePlayerOnAttack.Location = new System.Drawing.Point(8, 466);
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
            label30.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            label30.Location = new System.Drawing.Point(411, 16);
            label30.Name = "label30";
            label30.Size = new System.Drawing.Size(131, 52);
            label30.TabIndex = 140;
            label30.Text = "Appearance";
            label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkPlayerStartsVisible
            // 
            chkPlayerStartsVisible.AutoSize = true;
            chkPlayerStartsVisible.Location = new System.Drawing.Point(8, 271);
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
            crsPlayer.Location = new System.Drawing.Point(546, 5);
            crsPlayer.Name = "crsPlayer";
            crsPlayer.Size = new System.Drawing.Size(211, 83);
            crsPlayer.TabIndex = 155;
            crsPlayer.PropertyChanged += crsPlayer_PropertyChanged;
            // 
            // label67
            // 
            label67.AutoSize = true;
            label67.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            label67.Location = new System.Drawing.Point(128, 360);
            label67.Name = "label67";
            label67.Size = new System.Drawing.Size(67, 21);
            label67.TabIndex = 237;
            label67.Text = "Actions";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            label1.Location = new System.Drawing.Point(523, 605);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(85, 21);
            label1.TabIndex = 238;
            label1.Text = "Inventory";
            // 
            // saePlayerOnLevelUp
            // 
            saePlayerOnLevelUp.ActionDescription = "When they level up...                 ";
            saePlayerOnLevelUp.ActionTypeText = "On Level Up";
            saePlayerOnLevelUp.AutoSize = true;
            saePlayerOnLevelUp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saePlayerOnLevelUp.ClassId = null;
            saePlayerOnLevelUp.Location = new System.Drawing.Point(8, 642);
            saePlayerOnLevelUp.Name = "saePlayerOnLevelUp";
            saePlayerOnLevelUp.PlaceholderActionId = "LevelUp";
            saePlayerOnLevelUp.Size = new System.Drawing.Size(302, 32);
            saePlayerOnLevelUp.SourceDescription = "The player";
            saePlayerOnLevelUp.TabIndex = 239;
            saePlayerOnLevelUp.TargetDescription = "The player";
            saePlayerOnLevelUp.ThisDescription = "The player";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(8, 331);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(291, 15);
            label4.TabIndex = 244;
            label4.Text = "When selling items, they get                    % of the Value";
            // 
            // nudPlayerSaleValuePercentage
            // 
            nudPlayerSaleValuePercentage.Location = new System.Drawing.Point(165, 329);
            nudPlayerSaleValuePercentage.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudPlayerSaleValuePercentage.Name = "nudPlayerSaleValuePercentage";
            nudPlayerSaleValuePercentage.Size = new System.Drawing.Size(52, 23);
            nudPlayerSaleValuePercentage.TabIndex = 245;
            nudPlayerSaleValuePercentage.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nudPlayerSaleValuePercentage.ValueChanged += nudPlayerSaleValuePercentage_ValueChanged;
            // 
            // saePlayerDefaultOnAttack
            // 
            saePlayerDefaultOnAttack.ActionDescription = "When without Attack options...";
            saePlayerDefaultOnAttack.ActionTypeText = "Attack";
            saePlayerDefaultOnAttack.AutoSize = true;
            saePlayerDefaultOnAttack.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saePlayerDefaultOnAttack.ClassId = null;
            saePlayerDefaultOnAttack.Location = new System.Drawing.Point(2, 422);
            saePlayerDefaultOnAttack.Name = "saePlayerDefaultOnAttack";
            saePlayerDefaultOnAttack.PlaceholderActionId = "Attack";
            saePlayerDefaultOnAttack.RequiresCondition = true;
            saePlayerDefaultOnAttack.RequiresDescription = true;
            saePlayerDefaultOnAttack.RequiresName = true;
            saePlayerDefaultOnAttack.Size = new System.Drawing.Size(305, 32);
            saePlayerDefaultOnAttack.SourceDescription = "The player";
            saePlayerDefaultOnAttack.TabIndex = 246;
            saePlayerDefaultOnAttack.TargetDescription = "Whoever they are targeting";
            saePlayerDefaultOnAttack.ThisDescription = "The player";
            saePlayerDefaultOnAttack.TurnEndCriteria = HelperForms.TurnEndCriteria.MayNotEndTurn;
            saePlayerDefaultOnAttack.UsageCriteria = HelperForms.UsageCriteria.FullConditions;
            // 
            // esPlayer
            // 
            esPlayer.Location = new System.Drawing.Point(488, 740);
            esPlayer.Name = "esPlayer";
            esPlayer.Size = new System.Drawing.Size(204, 142);
            esPlayer.TabIndex = 247;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label2.Location = new System.Drawing.Point(485, 722);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(212, 15);
            label2.TabIndex = 248;
            label2.Text = "Starts with the following equipment:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label3.Location = new System.Drawing.Point(61, 726);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(206, 15);
            label3.TabIndex = 249;
            label3.Text = "Uses the following Equipment Slots:";
            // 
            // clbPlayerAvailableSlots
            // 
            clbPlayerAvailableSlots.FormattingEnabled = true;
            clbPlayerAvailableSlots.Location = new System.Drawing.Point(73, 752);
            clbPlayerAvailableSlots.Name = "clbPlayerAvailableSlots";
            clbPlayerAvailableSlots.Size = new System.Drawing.Size(175, 130);
            clbPlayerAvailableSlots.TabIndex = 250;
            // 
            // chkPlayerNeedsToIdentifyItems
            // 
            chkPlayerNeedsToIdentifyItems.AutoSize = true;
            chkPlayerNeedsToIdentifyItems.Location = new System.Drawing.Point(8, 301);
            chkPlayerNeedsToIdentifyItems.Name = "chkPlayerNeedsToIdentifyItems";
            chkPlayerNeedsToIdentifyItems.Size = new System.Drawing.Size(148, 19);
            chkPlayerNeedsToIdentifyItems.TabIndex = 251;
            chkPlayerNeedsToIdentifyItems.Text = "Needs to Identify Items";
            chkPlayerNeedsToIdentifyItems.UseVisualStyleBackColor = true;
            chkPlayerNeedsToIdentifyItems.CheckedChanged += chkPlayerNeedsToIdentifyItems_CheckedChanged;
            // 
            // cmbPlayerLearnset
            // 
            cmbPlayerLearnset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbPlayerLearnset.FormattingEnabled = true;
            cmbPlayerLearnset.Location = new System.Drawing.Point(65, 690);
            cmbPlayerLearnset.Name = "cmbPlayerLearnset";
            cmbPlayerLearnset.Size = new System.Drawing.Size(146, 23);
            cmbPlayerLearnset.TabIndex = 253;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(8, 693);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(51, 15);
            label5.TabIndex = 252;
            label5.Text = "Learnset";
            // 
            // PlayerClassTab
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Controls.Add(cmbPlayerLearnset);
            Controls.Add(label5);
            Controls.Add(chkPlayerNeedsToIdentifyItems);
            Controls.Add(clbPlayerAvailableSlots);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(esPlayer);
            Controls.Add(saePlayerDefaultOnAttack);
            Controls.Add(nudPlayerSaleValuePercentage);
            Controls.Add(label4);
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
            Size = new System.Drawing.Size(793, 886);
            ((System.ComponentModel.ISupportInitialize)nudPlayerInventorySize).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudPlayerSaleValuePercentage).EndInit();
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
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudPlayerSaleValuePercentage;
        private SingleActionEditor saePlayerDefaultOnAttack;
        private EquipmentSheet esPlayer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckedListBox clbPlayerAvailableSlots;
        private System.Windows.Forms.CheckBox chkPlayerNeedsToIdentifyItems;
        private System.Windows.Forms.ComboBox cmbPlayerLearnset;
        private System.Windows.Forms.Label label5;
    }
}
