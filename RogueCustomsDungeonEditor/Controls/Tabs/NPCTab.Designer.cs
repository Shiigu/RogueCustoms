namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    partial class NPCTab
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(NPCTab));
            cmbNPCAIType = new System.Windows.Forms.ComboBox();
            label20 = new System.Windows.Forms.Label();
            maeNPCOnInteracted = new MultiActionEditor();
            saeNPCOnSpawn = new SingleActionEditor();
            ssNPC = new StatsSheet();
            sisNPCStartingInventory = new StartingInventorySelector();
            saeNPCOnDeath = new SingleActionEditor();
            saeNPCOnAttacked = new SingleActionEditor();
            saeNPCOnTurnStart = new SingleActionEditor();
            maeNPCOnAttack = new MultiActionEditor();
            txtNPCExperiencePayout = new System.Windows.Forms.TextBox();
            label103 = new System.Windows.Forms.Label();
            chkNPCKnowsAllCharacterPositions = new System.Windows.Forms.CheckBox();
            label67 = new System.Windows.Forms.Label();
            cmbNPCStartingArmor = new System.Windows.Forms.ComboBox();
            label70 = new System.Windows.Forms.Label();
            cmbNPCStartingWeapon = new System.Windows.Forms.ComboBox();
            label71 = new System.Windows.Forms.Label();
            label73 = new System.Windows.Forms.Label();
            nudNPCInventorySize = new System.Windows.Forms.NumericUpDown();
            label74 = new System.Windows.Forms.Label();
            label98 = new System.Windows.Forms.Label();
            chkNPCStartsVisible = new System.Windows.Forms.CheckBox();
            cmbNPCFaction = new System.Windows.Forms.ComboBox();
            label99 = new System.Windows.Forms.Label();
            fklblNPCDescriptionLocale = new System.Windows.Forms.Button();
            txtNPCDescription = new System.Windows.Forms.TextBox();
            label100 = new System.Windows.Forms.Label();
            fklblNPCNameLocale = new System.Windows.Forms.Button();
            txtNPCName = new System.Windows.Forms.TextBox();
            label101 = new System.Windows.Forms.Label();
            crsNPC = new ConsoleRepresentationSelector();
            label1 = new System.Windows.Forms.Label();
            chkNPCPursuesOutOfSightCharacters = new System.Windows.Forms.CheckBox();
            chkNPCWandersIfWithoutTarget = new System.Windows.Forms.CheckBox();
            label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)nudNPCInventorySize).BeginInit();
            SuspendLayout();
            // 
            // cmbNPCAIType
            // 
            cmbNPCAIType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbNPCAIType.FormattingEnabled = true;
            cmbNPCAIType.Location = new System.Drawing.Point(193, 328);
            cmbNPCAIType.Name = "cmbNPCAIType";
            cmbNPCAIType.Size = new System.Drawing.Size(146, 23);
            cmbNPCAIType.TabIndex = 251;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new System.Drawing.Point(5, 331);
            label20.Name = "label20";
            label20.Size = new System.Drawing.Size(174, 15);
            label20.TabIndex = 250;
            label20.Text = "NPC decision-making strategy: ";
            // 
            // maeNPCOnInteracted
            // 
            maeNPCOnInteracted.ActionDescription = "Someone can interact with\r\nthem with the following:";
            maeNPCOnInteracted.Actions = (System.Collections.Generic.List<RogueCustomsGameEngine.Utils.JsonImports.ActionWithEffectsInfo>)resources.GetObject("maeNPCOnInteracted.Actions");
            maeNPCOnInteracted.ActionTypeText = "Interact";
            maeNPCOnInteracted.ClassId = null;
            maeNPCOnInteracted.Dungeon = null;
            maeNPCOnInteracted.EffectParamData = null;
            maeNPCOnInteracted.Location = new System.Drawing.Point(6, 644);
            maeNPCOnInteracted.Name = "maeNPCOnInteracted";
            maeNPCOnInteracted.PlaceholderActionName = null;
            maeNPCOnInteracted.RequiresActionId = true;
            maeNPCOnInteracted.RequiresActionName = true;
            maeNPCOnInteracted.RequiresCondition = true;
            maeNPCOnInteracted.RequiresDescription = true;
            maeNPCOnInteracted.Size = new System.Drawing.Size(368, 94);
            maeNPCOnInteracted.SourceDescription = "Whoever is targeting them";
            maeNPCOnInteracted.TabIndex = 249;
            maeNPCOnInteracted.TargetDescription = "The NPC";
            maeNPCOnInteracted.ThisDescription = "The NPC";
            maeNPCOnInteracted.TurnEndCriteria = HelperForms.TurnEndCriteria.MayNotEndTurn;
            maeNPCOnInteracted.UsageCriteria = HelperForms.UsageCriteria.FullConditions;
            // 
            // saeNPCOnSpawn
            // 
            saeNPCOnSpawn.Action = null;
            saeNPCOnSpawn.ActionDescription = "When spawning...                ";
            saeNPCOnSpawn.ActionTypeText = "Turn Start";
            saeNPCOnSpawn.AutoSize = true;
            saeNPCOnSpawn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saeNPCOnSpawn.ClassId = null;
            saeNPCOnSpawn.Dungeon = null;
            saeNPCOnSpawn.EffectParamData = null;
            saeNPCOnSpawn.Location = new System.Drawing.Point(6, 468);
            saeNPCOnSpawn.Name = "saeNPCOnSpawn";
            saeNPCOnSpawn.PlaceholderActionId = "TurnStart";
            saeNPCOnSpawn.RequiresCondition = true;
            saeNPCOnSpawn.Size = new System.Drawing.Size(283, 32);
            saeNPCOnSpawn.SourceDescription = "The NPC (won't become visible)";
            saeNPCOnSpawn.TabIndex = 248;
            saeNPCOnSpawn.TargetDescription = "The NPC (won't become visible)";
            saeNPCOnSpawn.ThisDescription = "The NPC (won't become visible)";
            saeNPCOnSpawn.TurnEndCriteria = HelperForms.TurnEndCriteria.CannotEndTurn;
            saeNPCOnSpawn.UsageCriteria = HelperForms.UsageCriteria.AnyTargetAnyTime;
            // 
            // ssNPC
            // 
            ssNPC.AutoSize = true;
            ssNPC.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ssNPC.BaseSightRangeDisplayNames = (System.Collections.Generic.Dictionary<string, string>)resources.GetObject("ssNPC.BaseSightRangeDisplayNames");
            ssNPC.CanGainExperience = false;
            ssNPC.ExperienceToLevelUpFormula = "";
            ssNPC.Location = new System.Drawing.Point(380, 92);
            ssNPC.MaxLevel = 1;
            ssNPC.Name = "ssNPC";
            ssNPC.Size = new System.Drawing.Size(303, 408);
            ssNPC.StatInfos = null;
            ssNPC.TabIndex = 247;
            // 
            // sisNPCStartingInventory
            // 
            sisNPCStartingInventory.Inventory = (System.Collections.Generic.List<string>)resources.GetObject("sisNPCStartingInventory.Inventory");
            sisNPCStartingInventory.InventorySize = 0;
            sisNPCStartingInventory.Location = new System.Drawing.Point(387, 587);
            sisNPCStartingInventory.Name = "sisNPCStartingInventory";
            sisNPCStartingInventory.SelectableItems = null;
            sisNPCStartingInventory.Size = new System.Drawing.Size(293, 79);
            sisNPCStartingInventory.TabIndex = 228;
            // 
            // saeNPCOnDeath
            // 
            saeNPCOnDeath.Action = null;
            saeNPCOnDeath.ActionDescription = "When they die...                   ";
            saeNPCOnDeath.ActionTypeText = "On Death";
            saeNPCOnDeath.AutoSize = true;
            saeNPCOnDeath.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saeNPCOnDeath.ClassId = null;
            saeNPCOnDeath.Dungeon = null;
            saeNPCOnDeath.EffectParamData = null;
            saeNPCOnDeath.Location = new System.Drawing.Point(6, 784);
            saeNPCOnDeath.Name = "saeNPCOnDeath";
            saeNPCOnDeath.PlaceholderActionId = "Death";
            saeNPCOnDeath.RequiresCondition = false;
            saeNPCOnDeath.Size = new System.Drawing.Size(283, 32);
            saeNPCOnDeath.SourceDescription = "The NPC";
            saeNPCOnDeath.TabIndex = 246;
            saeNPCOnDeath.TargetDescription = "Whoever killed them (if any)";
            saeNPCOnDeath.ThisDescription = "The NPC";
            saeNPCOnDeath.TurnEndCriteria = HelperForms.TurnEndCriteria.CannotEndTurn;
            saeNPCOnDeath.UsageCriteria = HelperForms.UsageCriteria.AnyTargetAnyTime;
            // 
            // saeNPCOnAttacked
            // 
            saeNPCOnAttacked.Action = null;
            saeNPCOnAttacked.ActionDescription = "When they get attacked...  ";
            saeNPCOnAttacked.ActionTypeText = "Attacked";
            saeNPCOnAttacked.AutoSize = true;
            saeNPCOnAttacked.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saeNPCOnAttacked.ClassId = null;
            saeNPCOnAttacked.Dungeon = null;
            saeNPCOnAttacked.EffectParamData = null;
            saeNPCOnAttacked.Location = new System.Drawing.Point(6, 744);
            saeNPCOnAttacked.Name = "saeNPCOnAttacked";
            saeNPCOnAttacked.PlaceholderActionId = "Interacted";
            saeNPCOnAttacked.RequiresCondition = false;
            saeNPCOnAttacked.Size = new System.Drawing.Size(281, 32);
            saeNPCOnAttacked.SourceDescription = "The NPC";
            saeNPCOnAttacked.TabIndex = 245;
            saeNPCOnAttacked.TargetDescription = "Whoever interacted with them";
            saeNPCOnAttacked.ThisDescription = "The NPC";
            saeNPCOnAttacked.TurnEndCriteria = HelperForms.TurnEndCriteria.CannotEndTurn;
            saeNPCOnAttacked.UsageCriteria = HelperForms.UsageCriteria.AnyTargetAnyTime;
            // 
            // saeNPCOnTurnStart
            // 
            saeNPCOnTurnStart.Action = null;
            saeNPCOnTurnStart.ActionDescription = "When the next turn starts...";
            saeNPCOnTurnStart.ActionTypeText = "Turn Start";
            saeNPCOnTurnStart.AutoSize = true;
            saeNPCOnTurnStart.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saeNPCOnTurnStart.ClassId = null;
            saeNPCOnTurnStart.Dungeon = null;
            saeNPCOnTurnStart.EffectParamData = null;
            saeNPCOnTurnStart.Location = new System.Drawing.Point(6, 506);
            saeNPCOnTurnStart.Name = "saeNPCOnTurnStart";
            saeNPCOnTurnStart.PlaceholderActionId = "TurnStart";
            saeNPCOnTurnStart.RequiresCondition = false;
            saeNPCOnTurnStart.Size = new System.Drawing.Size(283, 32);
            saeNPCOnTurnStart.SourceDescription = "The NPC";
            saeNPCOnTurnStart.TabIndex = 244;
            saeNPCOnTurnStart.TargetDescription = "The NPC";
            saeNPCOnTurnStart.ThisDescription = "The NPC";
            saeNPCOnTurnStart.TurnEndCriteria = HelperForms.TurnEndCriteria.CannotEndTurn;
            saeNPCOnTurnStart.UsageCriteria = HelperForms.UsageCriteria.AnyTargetAnyTime;
            // 
            // maeNPCOnAttack
            // 
            maeNPCOnAttack.ActionDescription = "Can do the following to\r\ninteract with someone:";
            maeNPCOnAttack.Actions = (System.Collections.Generic.List<RogueCustomsGameEngine.Utils.JsonImports.ActionWithEffectsInfo>)resources.GetObject("maeNPCOnAttack.Actions");
            maeNPCOnAttack.ActionTypeText = "Interact";
            maeNPCOnAttack.ClassId = null;
            maeNPCOnAttack.Dungeon = null;
            maeNPCOnAttack.EffectParamData = null;
            maeNPCOnAttack.Location = new System.Drawing.Point(6, 544);
            maeNPCOnAttack.Name = "maeNPCOnAttack";
            maeNPCOnAttack.PlaceholderActionName = null;
            maeNPCOnAttack.RequiresActionId = true;
            maeNPCOnAttack.RequiresActionName = true;
            maeNPCOnAttack.RequiresCondition = true;
            maeNPCOnAttack.RequiresDescription = false;
            maeNPCOnAttack.Size = new System.Drawing.Size(368, 94);
            maeNPCOnAttack.SourceDescription = "The NPC";
            maeNPCOnAttack.TabIndex = 243;
            maeNPCOnAttack.TargetDescription = "Whoever they are targeting";
            maeNPCOnAttack.ThisDescription = "The NPC";
            maeNPCOnAttack.TurnEndCriteria = HelperForms.TurnEndCriteria.MustEndTurn;
            maeNPCOnAttack.UsageCriteria = HelperForms.UsageCriteria.FullConditions;
            // 
            // txtNPCExperiencePayout
            // 
            txtNPCExperiencePayout.Location = new System.Drawing.Point(114, 267);
            txtNPCExperiencePayout.Name = "txtNPCExperiencePayout";
            txtNPCExperiencePayout.Size = new System.Drawing.Size(242, 23);
            txtNPCExperiencePayout.TabIndex = 239;
            txtNPCExperiencePayout.Enter += txtNPCExperiencePayout_Enter;
            txtNPCExperiencePayout.Leave += txtNPCExperiencePayout_Leave;
            // 
            // label103
            // 
            label103.AutoSize = true;
            label103.Location = new System.Drawing.Point(6, 270);
            label103.Name = "label103";
            label103.Size = new System.Drawing.Size(104, 15);
            label103.TabIndex = 238;
            label103.Text = "Experience Payout";
            // 
            // chkNPCKnowsAllCharacterPositions
            // 
            chkNPCKnowsAllCharacterPositions.AutoSize = true;
            chkNPCKnowsAllCharacterPositions.Location = new System.Drawing.Point(6, 362);
            chkNPCKnowsAllCharacterPositions.Name = "chkNPCKnowsAllCharacterPositions";
            chkNPCKnowsAllCharacterPositions.Size = new System.Drawing.Size(361, 19);
            chkNPCKnowsAllCharacterPositions.TabIndex = 237;
            chkNPCKnowsAllCharacterPositions.Text = "Knows the position of all living characters (even when not seen)";
            chkNPCKnowsAllCharacterPositions.UseVisualStyleBackColor = true;
            chkNPCKnowsAllCharacterPositions.CheckedChanged += chkNPCKnowsAllCharacterPositions_CheckedChanged;
            // 
            // label67
            // 
            label67.AutoSize = true;
            label67.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label67.Location = new System.Drawing.Point(129, 444);
            label67.Name = "label67";
            label67.Size = new System.Drawing.Size(67, 21);
            label67.TabIndex = 236;
            label67.Text = "Actions";
            // 
            // cmbNPCStartingArmor
            // 
            cmbNPCStartingArmor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbNPCStartingArmor.FormattingEnabled = true;
            cmbNPCStartingArmor.Location = new System.Drawing.Point(520, 708);
            cmbNPCStartingArmor.Name = "cmbNPCStartingArmor";
            cmbNPCStartingArmor.Size = new System.Drawing.Size(158, 23);
            cmbNPCStartingArmor.TabIndex = 235;
            cmbNPCStartingArmor.SelectedIndexChanged += cmbNPCStartingArmor_SelectedIndexChanged;
            // 
            // label70
            // 
            label70.AutoSize = true;
            label70.Location = new System.Drawing.Point(387, 711);
            label70.Name = "label70";
            label70.Size = new System.Drawing.Size(131, 15);
            label70.TabIndex = 234;
            label70.Text = "When unarmored, wear";
            // 
            // cmbNPCStartingWeapon
            // 
            cmbNPCStartingWeapon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbNPCStartingWeapon.FormattingEnabled = true;
            cmbNPCStartingWeapon.Location = new System.Drawing.Point(513, 677);
            cmbNPCStartingWeapon.Name = "cmbNPCStartingWeapon";
            cmbNPCStartingWeapon.Size = new System.Drawing.Size(165, 23);
            cmbNPCStartingWeapon.TabIndex = 233;
            cmbNPCStartingWeapon.SelectedIndexChanged += cmbNPCStartingWeapon_SelectedIndexChanged;
            // 
            // label71
            // 
            label71.AutoSize = true;
            label71.Location = new System.Drawing.Point(387, 680);
            label71.Name = "label71";
            label71.Size = new System.Drawing.Size(123, 15);
            label71.TabIndex = 232;
            label71.Text = "When unarmed, wield";
            // 
            // label73
            // 
            label73.AutoSize = true;
            label73.Location = new System.Drawing.Point(546, 550);
            label73.Name = "label73";
            label73.Size = new System.Drawing.Size(36, 15);
            label73.TabIndex = 231;
            label73.Text = "items";
            // 
            // nudNPCInventorySize
            // 
            nudNPCInventorySize.Location = new System.Drawing.Point(495, 545);
            nudNPCInventorySize.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            nudNPCInventorySize.Name = "nudNPCInventorySize";
            nudNPCInventorySize.Size = new System.Drawing.Size(45, 23);
            nudNPCInventorySize.TabIndex = 230;
            nudNPCInventorySize.ValueChanged += nudNPCInventorySize_ValueChanged;
            // 
            // label74
            // 
            label74.AutoSize = true;
            label74.Location = new System.Drawing.Point(387, 548);
            label74.Name = "label74";
            label74.Size = new System.Drawing.Size(109, 15);
            label74.TabIndex = 229;
            label74.Text = "Inventory Capacity:";
            // 
            // label98
            // 
            label98.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label98.Location = new System.Drawing.Point(365, 17);
            label98.Name = "label98";
            label98.Size = new System.Drawing.Size(131, 52);
            label98.TabIndex = 227;
            label98.Text = "Appearance";
            label98.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkNPCStartsVisible
            // 
            chkNPCStartsVisible.AutoSize = true;
            chkNPCStartsVisible.Location = new System.Drawing.Point(8, 238);
            chkNPCStartsVisible.Name = "chkNPCStartsVisible";
            chkNPCStartsVisible.Size = new System.Drawing.Size(102, 19);
            chkNPCStartsVisible.TabIndex = 226;
            chkNPCStartsVisible.Text = "Spawns visible";
            chkNPCStartsVisible.UseVisualStyleBackColor = true;
            chkNPCStartsVisible.CheckedChanged += chkNPCStartsVisible_CheckedChanged;
            // 
            // cmbNPCFaction
            // 
            cmbNPCFaction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbNPCFaction.FormattingEnabled = true;
            cmbNPCFaction.Location = new System.Drawing.Point(60, 207);
            cmbNPCFaction.Name = "cmbNPCFaction";
            cmbNPCFaction.Size = new System.Drawing.Size(146, 23);
            cmbNPCFaction.TabIndex = 225;
            cmbNPCFaction.SelectedIndexChanged += cmbNPCFaction_SelectedIndexChanged;
            // 
            // label99
            // 
            label99.AutoSize = true;
            label99.Location = new System.Drawing.Point(8, 210);
            label99.Name = "label99";
            label99.Size = new System.Drawing.Size(46, 15);
            label99.TabIndex = 224;
            label99.Text = "Faction";
            // 
            // fklblNPCDescriptionLocale
            // 
            fklblNPCDescriptionLocale.Enabled = false;
            fklblNPCDescriptionLocale.FlatAppearance.BorderSize = 0;
            fklblNPCDescriptionLocale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblNPCDescriptionLocale.Image = (System.Drawing.Image)resources.GetObject("fklblNPCDescriptionLocale.Image");
            fklblNPCDescriptionLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblNPCDescriptionLocale.Location = new System.Drawing.Point(8, 151);
            fklblNPCDescriptionLocale.Name = "fklblNPCDescriptionLocale";
            fklblNPCDescriptionLocale.Size = new System.Drawing.Size(331, 42);
            fklblNPCDescriptionLocale.TabIndex = 223;
            fklblNPCDescriptionLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblNPCDescriptionLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblNPCDescriptionLocale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblNPCDescriptionLocale.UseVisualStyleBackColor = true;
            fklblNPCDescriptionLocale.Visible = false;
            // 
            // txtNPCDescription
            // 
            txtNPCDescription.Location = new System.Drawing.Point(8, 122);
            txtNPCDescription.Name = "txtNPCDescription";
            txtNPCDescription.Size = new System.Drawing.Size(350, 23);
            txtNPCDescription.TabIndex = 222;
            txtNPCDescription.TextChanged += txtNPCDescription_TextChanged;
            // 
            // label100
            // 
            label100.AutoSize = true;
            label100.Location = new System.Drawing.Point(8, 104);
            label100.Name = "label100";
            label100.Size = new System.Drawing.Size(67, 15);
            label100.TabIndex = 221;
            label100.Text = "Description";
            // 
            // fklblNPCNameLocale
            // 
            fklblNPCNameLocale.Enabled = false;
            fklblNPCNameLocale.FlatAppearance.BorderSize = 0;
            fklblNPCNameLocale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblNPCNameLocale.Image = (System.Drawing.Image)resources.GetObject("fklblNPCNameLocale.Image");
            fklblNPCNameLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblNPCNameLocale.Location = new System.Drawing.Point(8, 53);
            fklblNPCNameLocale.Name = "fklblNPCNameLocale";
            fklblNPCNameLocale.Size = new System.Drawing.Size(331, 42);
            fklblNPCNameLocale.TabIndex = 220;
            fklblNPCNameLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblNPCNameLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblNPCNameLocale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblNPCNameLocale.UseVisualStyleBackColor = true;
            fklblNPCNameLocale.Visible = false;
            // 
            // txtNPCName
            // 
            txtNPCName.Location = new System.Drawing.Point(8, 24);
            txtNPCName.Name = "txtNPCName";
            txtNPCName.Size = new System.Drawing.Size(350, 23);
            txtNPCName.TabIndex = 219;
            txtNPCName.TextChanged += txtNPCName_TextChanged;
            // 
            // label101
            // 
            label101.AutoSize = true;
            label101.Location = new System.Drawing.Point(8, 6);
            label101.Name = "label101";
            label101.Size = new System.Drawing.Size(39, 15);
            label101.TabIndex = 218;
            label101.Text = "Name";
            // 
            // crsNPC
            // 
            crsNPC.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("crsNPC.BackgroundColor");
            crsNPC.Character = '\0';
            crsNPC.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("crsNPC.ForegroundColor");
            crsNPC.Location = new System.Drawing.Point(500, 5);
            crsNPC.Name = "crsNPC";
            crsNPC.Size = new System.Drawing.Size(211, 83);
            crsNPC.TabIndex = 242;
            crsNPC.PropertyChanged += crsNPC_PropertyChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label1.Location = new System.Drawing.Point(485, 510);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(85, 21);
            label1.TabIndex = 252;
            label1.Text = "Inventory";
            // 
            // chkNPCPursuesOutOfSightCharacters
            // 
            chkNPCPursuesOutOfSightCharacters.AutoSize = true;
            chkNPCPursuesOutOfSightCharacters.Location = new System.Drawing.Point(6, 387);
            chkNPCPursuesOutOfSightCharacters.Name = "chkNPCPursuesOutOfSightCharacters";
            chkNPCPursuesOutOfSightCharacters.Size = new System.Drawing.Size(292, 19);
            chkNPCPursuesOutOfSightCharacters.TabIndex = 253;
            chkNPCPursuesOutOfSightCharacters.Text = "Will pursue a target Character (even if out of sight)";
            chkNPCPursuesOutOfSightCharacters.UseVisualStyleBackColor = true;
            chkNPCPursuesOutOfSightCharacters.CheckedChanged += chkNPCPursuesOutOfSightCharacters_CheckedChanged;
            // 
            // chkWandersIfWithoutTarget
            // 
            chkNPCWandersIfWithoutTarget.AutoSize = true;
            chkNPCWandersIfWithoutTarget.Location = new System.Drawing.Point(6, 412);
            chkNPCWandersIfWithoutTarget.Name = "chkWandersIfWithoutTarget";
            chkNPCWandersIfWithoutTarget.Size = new System.Drawing.Size(282, 19);
            chkNPCWandersIfWithoutTarget.TabIndex = 254;
            chkNPCWandersIfWithoutTarget.Text = "Will wander around if there's no target Character";
            chkNPCWandersIfWithoutTarget.UseVisualStyleBackColor = true;
            chkNPCWandersIfWithoutTarget.CheckedChanged += chkNPCWandersIfWithoutTarget_CheckedChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label2.Location = new System.Drawing.Point(170, 304);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(26, 21);
            label2.TabIndex = 255;
            label2.Text = "AI";
            // 
            // NPCTab
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Controls.Add(label2);
            Controls.Add(chkNPCWandersIfWithoutTarget);
            Controls.Add(chkNPCPursuesOutOfSightCharacters);
            Controls.Add(label1);
            Controls.Add(cmbNPCAIType);
            Controls.Add(label20);
            Controls.Add(maeNPCOnInteracted);
            Controls.Add(saeNPCOnSpawn);
            Controls.Add(ssNPC);
            Controls.Add(sisNPCStartingInventory);
            Controls.Add(saeNPCOnDeath);
            Controls.Add(saeNPCOnAttacked);
            Controls.Add(saeNPCOnTurnStart);
            Controls.Add(maeNPCOnAttack);
            Controls.Add(txtNPCExperiencePayout);
            Controls.Add(label103);
            Controls.Add(chkNPCKnowsAllCharacterPositions);
            Controls.Add(label67);
            Controls.Add(cmbNPCStartingArmor);
            Controls.Add(label70);
            Controls.Add(cmbNPCStartingWeapon);
            Controls.Add(label71);
            Controls.Add(label73);
            Controls.Add(nudNPCInventorySize);
            Controls.Add(label74);
            Controls.Add(label98);
            Controls.Add(chkNPCStartsVisible);
            Controls.Add(cmbNPCFaction);
            Controls.Add(label99);
            Controls.Add(fklblNPCDescriptionLocale);
            Controls.Add(txtNPCDescription);
            Controls.Add(label100);
            Controls.Add(fklblNPCNameLocale);
            Controls.Add(txtNPCName);
            Controls.Add(label101);
            Controls.Add(crsNPC);
            Name = "NPCTab";
            Size = new System.Drawing.Size(714, 821);
            ((System.ComponentModel.ISupportInitialize)nudNPCInventorySize).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.ComboBox cmbNPCAIType;
        private System.Windows.Forms.Label label20;
        private MultiActionEditor maeNPCOnInteracted;
        private SingleActionEditor saeNPCOnSpawn;
        private StatsSheet ssNPC;
        private StartingInventorySelector sisNPCStartingInventory;
        private SingleActionEditor saeNPCOnDeath;
        private SingleActionEditor saeNPCOnAttacked;
        private SingleActionEditor saeNPCOnTurnStart;
        private MultiActionEditor maeNPCOnAttack;
        private System.Windows.Forms.TextBox txtNPCExperiencePayout;
        private System.Windows.Forms.Label label103;
        private System.Windows.Forms.CheckBox chkNPCKnowsAllCharacterPositions;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.ComboBox cmbNPCStartingArmor;
        private System.Windows.Forms.Label label70;
        private System.Windows.Forms.ComboBox cmbNPCStartingWeapon;
        private System.Windows.Forms.Label label71;
        private System.Windows.Forms.Label label73;
        private System.Windows.Forms.NumericUpDown nudNPCInventorySize;
        private System.Windows.Forms.Label label74;
        private System.Windows.Forms.Label label98;
        private System.Windows.Forms.CheckBox chkNPCStartsVisible;
        private System.Windows.Forms.ComboBox cmbNPCFaction;
        private System.Windows.Forms.Label label99;
        private System.Windows.Forms.Button fklblNPCDescriptionLocale;
        private System.Windows.Forms.TextBox txtNPCDescription;
        private System.Windows.Forms.Label label100;
        private System.Windows.Forms.Button fklblNPCNameLocale;
        private System.Windows.Forms.TextBox txtNPCName;
        private System.Windows.Forms.Label label101;
        private ConsoleRepresentationSelector crsNPC;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkNPCPursuesOutOfSightCharacters;
        private System.Windows.Forms.CheckBox chkNPCWandersIfWithoutTarget;
        private System.Windows.Forms.Label label2;
    }
}
