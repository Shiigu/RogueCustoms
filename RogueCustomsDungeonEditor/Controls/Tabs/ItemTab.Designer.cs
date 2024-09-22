namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    partial class ItemTab
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemTab));
            saeItemOnDeath = new SingleActionEditor();
            saeItemOnTurnStart = new SingleActionEditor();
            saeItemOnAttacked = new SingleActionEditor();
            maeItemOnAttack = new MultiActionEditor();
            saeItemOnUse = new SingleActionEditor();
            saeItemOnStepped = new SingleActionEditor();
            txtItemPower = new System.Windows.Forms.TextBox();
            label108 = new System.Windows.Forms.Label();
            chkItemCanBePickedUp = new System.Windows.Forms.CheckBox();
            chkItemStartsVisible = new System.Windows.Forms.CheckBox();
            cmbItemType = new System.Windows.Forms.ComboBox();
            label107 = new System.Windows.Forms.Label();
            label102 = new System.Windows.Forms.Label();
            fklblItemDescriptionLocale = new System.Windows.Forms.Button();
            txtItemDescription = new System.Windows.Forms.TextBox();
            label105 = new System.Windows.Forms.Label();
            fklblItemNameLocale = new System.Windows.Forms.Button();
            txtItemName = new System.Windows.Forms.TextBox();
            label106 = new System.Windows.Forms.Label();
            crsItem = new ConsoleRepresentationSelector();
            SuspendLayout();
            // 
            // saeItemOnDeath
            // 
            saeItemOnDeath.Action = null;
            saeItemOnDeath.ActionDescription = "When someone carrying it dies...                ";
            saeItemOnDeath.ActionTypeText = "On Death";
            saeItemOnDeath.AutoSize = true;
            saeItemOnDeath.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saeItemOnDeath.ClassId = null;
            saeItemOnDeath.Dungeon = null;
            saeItemOnDeath.EffectParamData = null;
            saeItemOnDeath.Location = new System.Drawing.Point(362, 313);
            saeItemOnDeath.Name = "saeItemOnDeath";
            saeItemOnDeath.PlaceholderActionName = "Death";
            saeItemOnDeath.RequiresActionName = false;
            saeItemOnDeath.RequiresCondition = false;
            saeItemOnDeath.RequiresDescription = false;
            saeItemOnDeath.Size = new System.Drawing.Size(361, 32);
            saeItemOnDeath.SourceDescription = "The item";
            saeItemOnDeath.TabIndex = 246;
            saeItemOnDeath.TargetDescription = "Whoever killed on them (if any)";
            saeItemOnDeath.ThisDescription = "The item";
            saeItemOnDeath.TurnEndCriteria = HelperForms.TurnEndCriteria.CannotEndTurn;
            saeItemOnDeath.UsageCriteria = HelperForms.UsageCriteria.AnyTargetAnyTime;
            // 
            // saeItemOnTurnStart
            // 
            saeItemOnTurnStart.Action = null;
            saeItemOnTurnStart.ActionDescription = "When the Item's owner starts a new turn...";
            saeItemOnTurnStart.ActionTypeText = "On Death";
            saeItemOnTurnStart.AutoSize = true;
            saeItemOnTurnStart.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saeItemOnTurnStart.ClassId = null;
            saeItemOnTurnStart.Dungeon = null;
            saeItemOnTurnStart.EffectParamData = null;
            saeItemOnTurnStart.Location = new System.Drawing.Point(362, 241);
            saeItemOnTurnStart.Name = "saeItemOnTurnStart";
            saeItemOnTurnStart.PlaceholderActionName = "Death";
            saeItemOnTurnStart.RequiresActionName = false;
            saeItemOnTurnStart.RequiresCondition = false;
            saeItemOnTurnStart.RequiresDescription = false;
            saeItemOnTurnStart.Size = new System.Drawing.Size(362, 32);
            saeItemOnTurnStart.SourceDescription = "Whoever is equipping This";
            saeItemOnTurnStart.TabIndex = 245;
            saeItemOnTurnStart.TargetDescription = "Whoever is equipping This";
            saeItemOnTurnStart.ThisDescription = "The item";
            saeItemOnTurnStart.TurnEndCriteria = HelperForms.TurnEndCriteria.CannotEndTurn;
            saeItemOnTurnStart.UsageCriteria = HelperForms.UsageCriteria.AnyTargetAnyTime;
            // 
            // saeItemOnAttacked
            // 
            saeItemOnAttacked.Action = null;
            saeItemOnAttacked.ActionDescription = "When the Item's owner gets interacted...   ";
            saeItemOnAttacked.ActionTypeText = "Interacted";
            saeItemOnAttacked.AutoSize = true;
            saeItemOnAttacked.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saeItemOnAttacked.ClassId = null;
            saeItemOnAttacked.Dungeon = null;
            saeItemOnAttacked.EffectParamData = null;
            saeItemOnAttacked.Location = new System.Drawing.Point(362, 203);
            saeItemOnAttacked.Name = "saeItemOnAttacked";
            saeItemOnAttacked.PlaceholderActionName = "Interacted";
            saeItemOnAttacked.RequiresActionName = false;
            saeItemOnAttacked.RequiresCondition = false;
            saeItemOnAttacked.RequiresDescription = false;
            saeItemOnAttacked.Size = new System.Drawing.Size(362, 32);
            saeItemOnAttacked.SourceDescription = "Whoever is equipping it";
            saeItemOnAttacked.TabIndex = 244;
            saeItemOnAttacked.TargetDescription = "The owner's interactor";
            saeItemOnAttacked.ThisDescription = "The item";
            saeItemOnAttacked.TurnEndCriteria = HelperForms.TurnEndCriteria.CannotEndTurn;
            saeItemOnAttacked.UsageCriteria = HelperForms.UsageCriteria.AnyTargetAnyTime;
            // 
            // maeItemOnAttack
            // 
            maeItemOnAttack.ActionDescription = "The Item's owner can do the following to interact with someone:";
            maeItemOnAttack.Actions = (System.Collections.Generic.List<RogueCustomsGameEngine.Utils.JsonImports.ActionWithEffectsInfo>)resources.GetObject("maeItemOnAttack.Actions");
            maeItemOnAttack.ActionTypeText = "Interact";
            maeItemOnAttack.ClassId = null;
            maeItemOnAttack.Dungeon = null;
            maeItemOnAttack.EffectParamData = null;
            maeItemOnAttack.Location = new System.Drawing.Point(362, 103);
            maeItemOnAttack.Name = "maeItemOnAttack";
            maeItemOnAttack.PlaceholderActionName = null;
            maeItemOnAttack.RequiresActionName = true;
            maeItemOnAttack.RequiresCondition = true;
            maeItemOnAttack.RequiresDescription = true;
            maeItemOnAttack.Size = new System.Drawing.Size(368, 94);
            maeItemOnAttack.SourceDescription = null;
            maeItemOnAttack.TabIndex = 243;
            maeItemOnAttack.TargetDescription = "Whoever is being targeted";
            maeItemOnAttack.ThisDescription = "The item";
            maeItemOnAttack.TurnEndCriteria = HelperForms.TurnEndCriteria.MustEndTurn;
            maeItemOnAttack.UsageCriteria = HelperForms.UsageCriteria.FullConditions;
            // 
            // saeItemOnUse
            // 
            saeItemOnUse.Action = null;
            saeItemOnUse.ActionDescription = "When someone uses it on     \r\nthemselves...";
            saeItemOnUse.ActionTypeText = "Item Use";
            saeItemOnUse.AutoSize = true;
            saeItemOnUse.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saeItemOnUse.ClassId = null;
            saeItemOnUse.Dungeon = null;
            saeItemOnUse.EffectParamData = null;
            saeItemOnUse.Location = new System.Drawing.Point(8, 313);
            saeItemOnUse.Name = "saeItemOnUse";
            saeItemOnUse.PlaceholderActionName = "ItemUse";
            saeItemOnUse.RequiresActionName = false;
            saeItemOnUse.RequiresCondition = true;
            saeItemOnUse.RequiresDescription = false;
            saeItemOnUse.Size = new System.Drawing.Size(292, 32);
            saeItemOnUse.SourceDescription = "The item";
            saeItemOnUse.TabIndex = 242;
            saeItemOnUse.TargetDescription = "Whoever is using it";
            saeItemOnUse.ThisDescription = "The item";
            saeItemOnUse.TurnEndCriteria = HelperForms.TurnEndCriteria.MustEndTurn;
            saeItemOnUse.UsageCriteria = HelperForms.UsageCriteria.AnyTargetAnyTime;
            // 
            // saeItemOnStepped
            // 
            saeItemOnStepped.Action = null;
            saeItemOnStepped.ActionDescription = "When someone steps on it... ";
            saeItemOnStepped.ActionTypeText = "Stepped";
            saeItemOnStepped.AutoSize = true;
            saeItemOnStepped.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saeItemOnStepped.ClassId = null;
            saeItemOnStepped.Dungeon = null;
            saeItemOnStepped.EffectParamData = null;
            saeItemOnStepped.Location = new System.Drawing.Point(8, 275);
            saeItemOnStepped.Name = "saeItemOnStepped";
            saeItemOnStepped.PlaceholderActionName = "Stepped";
            saeItemOnStepped.RequiresActionName = false;
            saeItemOnStepped.RequiresCondition = false;
            saeItemOnStepped.RequiresDescription = false;
            saeItemOnStepped.Size = new System.Drawing.Size(293, 32);
            saeItemOnStepped.SourceDescription = "The item";
            saeItemOnStepped.TabIndex = 241;
            saeItemOnStepped.TargetDescription = "Whoever stepped on it";
            saeItemOnStepped.ThisDescription = "The item";
            saeItemOnStepped.TurnEndCriteria = HelperForms.TurnEndCriteria.CannotEndTurn;
            saeItemOnStepped.UsageCriteria = HelperForms.UsageCriteria.AnyTargetAnyTime;
            // 
            // txtItemPower
            // 
            txtItemPower.Location = new System.Drawing.Point(81, 240);
            txtItemPower.Name = "txtItemPower";
            txtItemPower.Size = new System.Drawing.Size(150, 23);
            txtItemPower.TabIndex = 239;
            txtItemPower.Enter += txtItemPower_Enter;
            txtItemPower.Leave += txtItemPower_Leave;
            // 
            // label108
            // 
            label108.AutoSize = true;
            label108.Location = new System.Drawing.Point(8, 246);
            label108.Name = "label108";
            label108.Size = new System.Drawing.Size(67, 15);
            label108.TabIndex = 238;
            label108.Text = "Item Power";
            // 
            // chkItemCanBePickedUp
            // 
            chkItemCanBePickedUp.AutoSize = true;
            chkItemCanBePickedUp.Location = new System.Drawing.Point(237, 242);
            chkItemCanBePickedUp.Name = "chkItemCanBePickedUp";
            chkItemCanBePickedUp.Size = new System.Drawing.Size(118, 19);
            chkItemCanBePickedUp.TabIndex = 237;
            chkItemCanBePickedUp.Text = "Can be picked up";
            chkItemCanBePickedUp.UseVisualStyleBackColor = true;
            // 
            // chkItemStartsVisible
            // 
            chkItemStartsVisible.AutoSize = true;
            chkItemStartsVisible.Location = new System.Drawing.Point(237, 208);
            chkItemStartsVisible.Name = "chkItemStartsVisible";
            chkItemStartsVisible.Size = new System.Drawing.Size(102, 19);
            chkItemStartsVisible.TabIndex = 236;
            chkItemStartsVisible.Text = "Spawns visible";
            chkItemStartsVisible.UseVisualStyleBackColor = true;
            // 
            // cmbItemType
            // 
            cmbItemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbItemType.FormattingEnabled = true;
            cmbItemType.Location = new System.Drawing.Point(72, 206);
            cmbItemType.Name = "cmbItemType";
            cmbItemType.Size = new System.Drawing.Size(159, 23);
            cmbItemType.TabIndex = 235;
            cmbItemType.SelectedIndexChanged += cmbItemType_SelectedIndexChanged;
            // 
            // label107
            // 
            label107.AutoSize = true;
            label107.Location = new System.Drawing.Point(8, 209);
            label107.Name = "label107";
            label107.Size = new System.Drawing.Size(58, 15);
            label107.TabIndex = 234;
            label107.Text = "Item Type";
            // 
            // label102
            // 
            label102.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label102.Location = new System.Drawing.Point(385, 17);
            label102.Name = "label102";
            label102.Size = new System.Drawing.Size(131, 52);
            label102.TabIndex = 233;
            label102.Text = "Appearance";
            label102.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fklblItemDescriptionLocale
            // 
            fklblItemDescriptionLocale.Enabled = false;
            fklblItemDescriptionLocale.FlatAppearance.BorderSize = 0;
            fklblItemDescriptionLocale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblItemDescriptionLocale.Image = (System.Drawing.Image)resources.GetObject("fklblItemDescriptionLocale.Image");
            fklblItemDescriptionLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblItemDescriptionLocale.Location = new System.Drawing.Point(8, 150);
            fklblItemDescriptionLocale.Name = "fklblItemDescriptionLocale";
            fklblItemDescriptionLocale.Size = new System.Drawing.Size(331, 42);
            fklblItemDescriptionLocale.TabIndex = 232;
            fklblItemDescriptionLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblItemDescriptionLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblItemDescriptionLocale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblItemDescriptionLocale.UseVisualStyleBackColor = true;
            fklblItemDescriptionLocale.Visible = false;
            // 
            // txtItemDescription
            // 
            txtItemDescription.Location = new System.Drawing.Point(8, 121);
            txtItemDescription.Name = "txtItemDescription";
            txtItemDescription.Size = new System.Drawing.Size(350, 23);
            txtItemDescription.TabIndex = 231;
            txtItemDescription.TextChanged += txtItemDescription_TextChanged;
            // 
            // label105
            // 
            label105.AutoSize = true;
            label105.Location = new System.Drawing.Point(8, 103);
            label105.Name = "label105";
            label105.Size = new System.Drawing.Size(67, 15);
            label105.TabIndex = 230;
            label105.Text = "Description";
            // 
            // fklblItemNameLocale
            // 
            fklblItemNameLocale.Enabled = false;
            fklblItemNameLocale.FlatAppearance.BorderSize = 0;
            fklblItemNameLocale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblItemNameLocale.Image = (System.Drawing.Image)resources.GetObject("fklblItemNameLocale.Image");
            fklblItemNameLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblItemNameLocale.Location = new System.Drawing.Point(8, 52);
            fklblItemNameLocale.Name = "fklblItemNameLocale";
            fklblItemNameLocale.Size = new System.Drawing.Size(331, 42);
            fklblItemNameLocale.TabIndex = 229;
            fklblItemNameLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblItemNameLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblItemNameLocale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblItemNameLocale.UseVisualStyleBackColor = true;
            fklblItemNameLocale.Visible = false;
            // 
            // txtItemName
            // 
            txtItemName.Location = new System.Drawing.Point(8, 23);
            txtItemName.Name = "txtItemName";
            txtItemName.Size = new System.Drawing.Size(350, 23);
            txtItemName.TabIndex = 228;
            txtItemName.TextChanged += txtItemName_TextChanged;
            // 
            // label106
            // 
            label106.AutoSize = true;
            label106.Location = new System.Drawing.Point(8, 5);
            label106.Name = "label106";
            label106.Size = new System.Drawing.Size(80, 15);
            label106.TabIndex = 227;
            label106.Text = "Default Name";
            // 
            // crsItem
            // 
            crsItem.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("crsItem.BackgroundColor");
            crsItem.Character = '\0';
            crsItem.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("crsItem.ForegroundColor");
            crsItem.Location = new System.Drawing.Point(519, 6);
            crsItem.Name = "crsItem";
            crsItem.Size = new System.Drawing.Size(211, 83);
            crsItem.TabIndex = 240;
            crsItem.PropertyChanged += crsItem_PropertyChanged;
            // 
            // ItemTab
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Controls.Add(saeItemOnDeath);
            Controls.Add(saeItemOnTurnStart);
            Controls.Add(saeItemOnAttacked);
            Controls.Add(maeItemOnAttack);
            Controls.Add(saeItemOnUse);
            Controls.Add(saeItemOnStepped);
            Controls.Add(txtItemPower);
            Controls.Add(label108);
            Controls.Add(chkItemCanBePickedUp);
            Controls.Add(chkItemStartsVisible);
            Controls.Add(cmbItemType);
            Controls.Add(label107);
            Controls.Add(label102);
            Controls.Add(fklblItemDescriptionLocale);
            Controls.Add(txtItemDescription);
            Controls.Add(label105);
            Controls.Add(fklblItemNameLocale);
            Controls.Add(txtItemName);
            Controls.Add(label106);
            Controls.Add(crsItem);
            Name = "ItemTab";
            Size = new System.Drawing.Size(733, 348);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private SingleActionEditor saeItemOnDeath;
        private SingleActionEditor saeItemOnTurnStart;
        private SingleActionEditor saeItemOnAttacked;
        private MultiActionEditor maeItemOnAttack;
        private SingleActionEditor saeItemOnUse;
        private SingleActionEditor saeItemOnStepped;
        private System.Windows.Forms.TextBox txtItemPower;
        private System.Windows.Forms.Label label108;
        private System.Windows.Forms.CheckBox chkItemCanBePickedUp;
        private System.Windows.Forms.CheckBox chkItemStartsVisible;
        private System.Windows.Forms.ComboBox cmbItemType;
        private System.Windows.Forms.Label label107;
        private System.Windows.Forms.Label label102;
        private System.Windows.Forms.Button fklblItemDescriptionLocale;
        private System.Windows.Forms.TextBox txtItemDescription;
        private System.Windows.Forms.Label label105;
        private System.Windows.Forms.Button fklblItemNameLocale;
        private System.Windows.Forms.TextBox txtItemName;
        private System.Windows.Forms.Label label106;
        private ConsoleRepresentationSelector crsItem;
    }
}
