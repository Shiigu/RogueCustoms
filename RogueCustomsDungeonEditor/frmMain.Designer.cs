namespace RogueCustomsDungeonEditor
{
    partial class frmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            global::System.ComponentModel.ComponentResourceManager resources = new global::System.ComponentModel.ComponentResourceManager(typeof(global::RogueCustomsDungeonEditor.frmMain));
            global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new global::System.Windows.Forms.DataGridViewCellStyle();
            this.msMenu = (new global::System.Windows.Forms.MenuStrip());
            this.editorToolStripMenuItem = (new global::System.Windows.Forms.ToolStripMenuItem());
            this.exitToolStripMenuItem = (new global::System.Windows.Forms.ToolStripMenuItem());
            this.tsButtons = (new global::System.Windows.Forms.ToolStrip());
            this.tsbNewDungeon = (new global::System.Windows.Forms.ToolStripButton());
            this.tsbOpenDungeon = (new global::System.Windows.Forms.ToolStripButton());
            this.tsbSaveDungeon = (new global::System.Windows.Forms.ToolStripButton());
            this.tsbSaveDungeonAs = (new global::System.Windows.Forms.ToolStripButton());
            this.tssDungeonElement = (new global::System.Windows.Forms.ToolStripSeparator());
            this.tsbAddElement = (new global::System.Windows.Forms.ToolStripButton());
            this.tsbSaveElement = (new global::System.Windows.Forms.ToolStripButton());
            this.tsbSaveElementAs = (new global::System.Windows.Forms.ToolStripButton());
            this.tsbDeleteElement = (new global::System.Windows.Forms.ToolStripButton());
            this.tssElementValidate = (new global::System.Windows.Forms.ToolStripSeparator());
            this.tsbValidateDungeon = (new global::System.Windows.Forms.ToolStripButton());
            this.tvDungeonInfo = (new global::System.Windows.Forms.TreeView());
            this.tbTabs = (new global::System.Windows.Forms.TabControl());
            this.tpBasicInfo = (new global::System.Windows.Forms.TabPage());
            this.cmbDefaultLocale = (new global::System.Windows.Forms.ComboBox());
            this.label8 = (new global::System.Windows.Forms.Label());
            this.fklblEndingMessageLocale = (new global::System.Windows.Forms.Button());
            this.fklblWelcomeMessageLocale = (new global::System.Windows.Forms.Button());
            this.fklblAuthorLocale = (new global::System.Windows.Forms.Button());
            this.fklblDungeonNameLocale = (new global::System.Windows.Forms.Button());
            this.txtEndingMessage = (new global::System.Windows.Forms.TextBox());
            this.label4 = (new global::System.Windows.Forms.Label());
            this.txtWelcomeMessage = (new global::System.Windows.Forms.TextBox());
            this.label3 = (new global::System.Windows.Forms.Label());
            this.txtAuthor = (new global::System.Windows.Forms.TextBox());
            this.label2 = (new global::System.Windows.Forms.Label());
            this.txtDungeonName = (new global::System.Windows.Forms.TextBox());
            this.label1 = (new global::System.Windows.Forms.Label());
            this.tpLocales = (new global::System.Windows.Forms.TabPage());
            this.btnDeleteLocale = (new global::System.Windows.Forms.Button());
            this.btnAddLocale = (new global::System.Windows.Forms.Button());
            this.btnUpdateLocale = (new global::System.Windows.Forms.Button());
            this.fklblMissingLocale = (new global::System.Windows.Forms.Button());
            this.txtLocaleEntryValue = (new global::System.Windows.Forms.TextBox());
            this.label7 = (new global::System.Windows.Forms.Label());
            this.txtLocaleEntryKey = (new global::System.Windows.Forms.TextBox());
            this.label6 = (new global::System.Windows.Forms.Label());
            this.dgvLocales = (new global::System.Windows.Forms.DataGridView());
            this.cmKey = (new global::System.Windows.Forms.DataGridViewTextBoxColumn());
            this.cmValue = (new global::System.Windows.Forms.DataGridViewTextBoxColumn());
            this.tpFloorInfos = (new global::System.Windows.Forms.TabPage());
            this.btnOnFloorStartAction = (new global::System.Windows.Forms.Button());
            this.label20 = (new global::System.Windows.Forms.Label());
            this.nudRoomFusionOdds = (new global::System.Windows.Forms.NumericUpDown());
            this.label19 = (new global::System.Windows.Forms.Label());
            this.nudExtraRoomConnectionOdds = (new global::System.Windows.Forms.NumericUpDown());
            this.label18 = (new global::System.Windows.Forms.Label());
            this.nudMaxRoomConnections = (new global::System.Windows.Forms.NumericUpDown());
            this.label5 = (new global::System.Windows.Forms.Label());
            this.btnRemoveAlgorithm = (new global::System.Windows.Forms.Button());
            this.btnEditAlgorithm = (new global::System.Windows.Forms.Button());
            this.btnAddAlgorithm = (new global::System.Windows.Forms.Button());
            this.lvFloorAlgorithms = (new global::System.Windows.Forms.ListView());
            this.label17 = (new global::System.Windows.Forms.Label());
            this.label16 = (new global::System.Windows.Forms.Label());
            this.btnTrapGenerator = (new global::System.Windows.Forms.Button());
            this.label15 = (new global::System.Windows.Forms.Label());
            this.btnItemGenerator = (new global::System.Windows.Forms.Button());
            this.label14 = (new global::System.Windows.Forms.Label());
            this.btnNPCGenerator = (new global::System.Windows.Forms.Button());
            this.label13 = (new global::System.Windows.Forms.Label());
            this.nudHeight = (new global::System.Windows.Forms.NumericUpDown());
            this.nudWidth = (new global::System.Windows.Forms.NumericUpDown());
            this.label12 = (new global::System.Windows.Forms.Label());
            this.label11 = (new global::System.Windows.Forms.Label());
            this.fklblStairsReminder = (new global::System.Windows.Forms.Button());
            this.chkGenerateStairsOnStart = (new global::System.Windows.Forms.CheckBox());
            this.nudMaxFloorLevel = (new global::System.Windows.Forms.NumericUpDown());
            this.label10 = (new global::System.Windows.Forms.Label());
            this.nudMinFloorLevel = (new global::System.Windows.Forms.NumericUpDown());
            this.label9 = (new global::System.Windows.Forms.Label());
            this.tpFactionInfos = (new global::System.Windows.Forms.TabPage());
            this.lbEnemies = (new global::System.Windows.Forms.ListBox());
            this.label26 = (new global::System.Windows.Forms.Label());
            this.btnEnemiesToNeutrals = (new global::System.Windows.Forms.Button());
            this.btnEnemyToNeutral = (new global::System.Windows.Forms.Button());
            this.btnNeutralsToEnemies = (new global::System.Windows.Forms.Button());
            this.btnNeutralToEnemy = (new global::System.Windows.Forms.Button());
            this.lbNeutrals = (new global::System.Windows.Forms.ListBox());
            this.label25 = (new global::System.Windows.Forms.Label());
            this.btnNeutralsToAllies = (new global::System.Windows.Forms.Button());
            this.btnNeutralToAlly = (new global::System.Windows.Forms.Button());
            this.btnAlliesToNeutrals = (new global::System.Windows.Forms.Button());
            this.btnAllyToNeutral = (new global::System.Windows.Forms.Button());
            this.lbAllies = (new global::System.Windows.Forms.ListBox());
            this.label24 = (new global::System.Windows.Forms.Label());
            this.label23 = (new global::System.Windows.Forms.Label());
            this.fklblFactionDescriptionLocale = (new global::System.Windows.Forms.Button());
            this.txtFactionDescription = (new global::System.Windows.Forms.TextBox());
            this.label22 = (new global::System.Windows.Forms.Label());
            this.fklblFactionNameLocale = (new global::System.Windows.Forms.Button());
            this.txtFactionName = (new global::System.Windows.Forms.TextBox());
            this.label21 = (new global::System.Windows.Forms.Label());
            this.tpPlayerClass = (new global::System.Windows.Forms.TabPage());
            this.btnChangePlayerConsoleCharacterBackColor = (new global::System.Windows.Forms.Button());
            this.btnPlayerOnDeathAction = (new global::System.Windows.Forms.Button());
            this.label63 = (new global::System.Windows.Forms.Label());
            this.btnPlayerOnAttackedAction = (new global::System.Windows.Forms.Button());
            this.label61 = (new global::System.Windows.Forms.Label());
            this.btnPlayerOnTurnStartAction = (new global::System.Windows.Forms.Button());
            this.label60 = (new global::System.Windows.Forms.Label());
            this.label58 = (new global::System.Windows.Forms.Label());
            this.label62 = (new global::System.Windows.Forms.Label());
            this.btnRemovePlayerOnAttackAction = (new global::System.Windows.Forms.Button());
            this.btnEditPlayerOnAttackAction = (new global::System.Windows.Forms.Button());
            this.btnAddPlayerOnAttackAction = (new global::System.Windows.Forms.Button());
            this.lbPlayerOnAttackActions = (new global::System.Windows.Forms.ListBox());
            this.label59 = (new global::System.Windows.Forms.Label());
            this.cmbPlayerStartingArmor = (new global::System.Windows.Forms.ComboBox());
            this.label57 = (new global::System.Windows.Forms.Label());
            this.cmbPlayerStartingWeapon = (new global::System.Windows.Forms.ComboBox());
            this.label56 = (new global::System.Windows.Forms.Label());
            this.lbPlayerStartingInventory = (new global::System.Windows.Forms.ListBox());
            this.btnPlayerRemoveItem = (new global::System.Windows.Forms.Button());
            this.btnPlayerAddItem = (new global::System.Windows.Forms.Button());
            this.cmbPlayerInventoryItemChoices = (new global::System.Windows.Forms.ComboBox());
            this.label55 = (new global::System.Windows.Forms.Label());
            this.label54 = (new global::System.Windows.Forms.Label());
            this.nudPlayerInventorySize = (new global::System.Windows.Forms.NumericUpDown());
            this.label53 = (new global::System.Windows.Forms.Label());
            this.label52 = (new global::System.Windows.Forms.Label());
            this.label47 = (new global::System.Windows.Forms.Label());
            this.label51 = (new global::System.Windows.Forms.Label());
            this.chkPlayerCanGainExperience = (new global::System.Windows.Forms.CheckBox());
            this.nudPlayerMaxLevel = (new global::System.Windows.Forms.NumericUpDown());
            this.label50 = (new global::System.Windows.Forms.Label());
            this.txtPlayerLevelUpFormula = (new global::System.Windows.Forms.TextBox());
            this.label49 = (new global::System.Windows.Forms.Label());
            this.label48 = (new global::System.Windows.Forms.Label());
            this.nudPlayerFlatSightRange = (new global::System.Windows.Forms.NumericUpDown());
            this.cmbPlayerSightRange = (new global::System.Windows.Forms.ComboBox());
            this.label43 = (new global::System.Windows.Forms.Label());
            this.label44 = (new global::System.Windows.Forms.Label());
            this.label45 = (new global::System.Windows.Forms.Label());
            this.label46 = (new global::System.Windows.Forms.Label());
            this.nudPlayerHPRegenerationPerLevelUp = (new global::System.Windows.Forms.NumericUpDown());
            this.nudPlayerBaseHPRegeneration = (new global::System.Windows.Forms.NumericUpDown());
            this.label42 = (new global::System.Windows.Forms.Label());
            this.label41 = (new global::System.Windows.Forms.Label());
            this.label40 = (new global::System.Windows.Forms.Label());
            this.label39 = (new global::System.Windows.Forms.Label());
            this.nudPlayerMovementPerLevelUp = (new global::System.Windows.Forms.NumericUpDown());
            this.nudPlayerBaseMovement = (new global::System.Windows.Forms.NumericUpDown());
            this.label37 = (new global::System.Windows.Forms.Label());
            this.nudPlayerDefensePerLevelUp = (new global::System.Windows.Forms.NumericUpDown());
            this.label38 = (new global::System.Windows.Forms.Label());
            this.nudPlayerBaseDefense = (new global::System.Windows.Forms.NumericUpDown());
            this.label36 = (new global::System.Windows.Forms.Label());
            this.label34 = (new global::System.Windows.Forms.Label());
            this.nudPlayerAttackPerLevelUp = (new global::System.Windows.Forms.NumericUpDown());
            this.label35 = (new global::System.Windows.Forms.Label());
            this.nudPlayerBaseAttack = (new global::System.Windows.Forms.NumericUpDown());
            this.label33 = (new global::System.Windows.Forms.Label());
            this.nudPlayerHPPerLevelUp = (new global::System.Windows.Forms.NumericUpDown());
            this.label32 = (new global::System.Windows.Forms.Label());
            this.nudPlayerBaseHP = (new global::System.Windows.Forms.NumericUpDown());
            this.label31 = (new global::System.Windows.Forms.Label());
            this.btnChangePlayerConsoleCharacterForeColor = (new global::System.Windows.Forms.Button());
            this.btnChangePlayerConsoleCharacter = (new global::System.Windows.Forms.Button());
            this.lblPlayerConsoleRepresentation = (new global::System.Windows.Forms.Label());
            this.label30 = (new global::System.Windows.Forms.Label());
            this.chkPlayerStartsVisible = (new global::System.Windows.Forms.CheckBox());
            this.cmbPlayerFaction = (new global::System.Windows.Forms.ComboBox());
            this.label29 = (new global::System.Windows.Forms.Label());
            this.chkRequirePlayerPrompt = (new global::System.Windows.Forms.CheckBox());
            this.fklblPlayerClassDescriptionLocale = (new global::System.Windows.Forms.Button());
            this.txtPlayerClassDescription = (new global::System.Windows.Forms.TextBox());
            this.label28 = (new global::System.Windows.Forms.Label());
            this.fklblPlayerClassNameLocale = (new global::System.Windows.Forms.Button());
            this.txtPlayerClassName = (new global::System.Windows.Forms.TextBox());
            this.label27 = (new global::System.Windows.Forms.Label());
            this.lblPlayerSightRangeText = (new global::System.Windows.Forms.Label());
            this.tpNPC = (new global::System.Windows.Forms.TabPage());
            this.nudNPCOddsToTargetSelf = (new global::System.Windows.Forms.NumericUpDown());
            this.label104 = (new global::System.Windows.Forms.Label());
            this.txtNPCExperiencePayout = (new global::System.Windows.Forms.TextBox());
            this.label103 = (new global::System.Windows.Forms.Label());
            this.chkNPCKnowsAllCharacterPositions = (new global::System.Windows.Forms.CheckBox());
            this.btnChangeNPCConsoleCharacterBackColor = (new global::System.Windows.Forms.Button());
            this.btnNPCOnDeathAction = (new global::System.Windows.Forms.Button());
            this.label64 = (new global::System.Windows.Forms.Label());
            this.btnNPCOnAttackedAction = (new global::System.Windows.Forms.Button());
            this.label65 = (new global::System.Windows.Forms.Label());
            this.btnNPCOnTurnStartAction = (new global::System.Windows.Forms.Button());
            this.label66 = (new global::System.Windows.Forms.Label());
            this.label67 = (new global::System.Windows.Forms.Label());
            this.label68 = (new global::System.Windows.Forms.Label());
            this.btnRemoveNPCOnAttackAction = (new global::System.Windows.Forms.Button());
            this.btnEditNPCOnAttackAction = (new global::System.Windows.Forms.Button());
            this.btnAddNPCOnAttackAction = (new global::System.Windows.Forms.Button());
            this.lbNPCOnAttackActions = (new global::System.Windows.Forms.ListBox());
            this.label69 = (new global::System.Windows.Forms.Label());
            this.cmbNPCStartingArmor = (new global::System.Windows.Forms.ComboBox());
            this.label70 = (new global::System.Windows.Forms.Label());
            this.cmbNPCStartingWeapon = (new global::System.Windows.Forms.ComboBox());
            this.label71 = (new global::System.Windows.Forms.Label());
            this.lbNPCStartingInventory = (new global::System.Windows.Forms.ListBox());
            this.btnNPCRemoveItem = (new global::System.Windows.Forms.Button());
            this.btnNPCAddItem = (new global::System.Windows.Forms.Button());
            this.cmbNPCInventoryItemChoices = (new global::System.Windows.Forms.ComboBox());
            this.label72 = (new global::System.Windows.Forms.Label());
            this.label73 = (new global::System.Windows.Forms.Label());
            this.nudNPCInventorySize = (new global::System.Windows.Forms.NumericUpDown());
            this.label74 = (new global::System.Windows.Forms.Label());
            this.label75 = (new global::System.Windows.Forms.Label());
            this.label76 = (new global::System.Windows.Forms.Label());
            this.label77 = (new global::System.Windows.Forms.Label());
            this.chkNPCCanGainExperience = (new global::System.Windows.Forms.CheckBox());
            this.nudNPCMaxLevel = (new global::System.Windows.Forms.NumericUpDown());
            this.label78 = (new global::System.Windows.Forms.Label());
            this.txtNPCLevelUpFormula = (new global::System.Windows.Forms.TextBox());
            this.label79 = (new global::System.Windows.Forms.Label());
            this.label80 = (new global::System.Windows.Forms.Label());
            this.nudNPCFlatSightRange = (new global::System.Windows.Forms.NumericUpDown());
            this.cmbNPCSightRange = (new global::System.Windows.Forms.ComboBox());
            this.label81 = (new global::System.Windows.Forms.Label());
            this.label82 = (new global::System.Windows.Forms.Label());
            this.label83 = (new global::System.Windows.Forms.Label());
            this.label84 = (new global::System.Windows.Forms.Label());
            this.nudNPCHPRegenerationPerLevelUp = (new global::System.Windows.Forms.NumericUpDown());
            this.nudNPCBaseHPRegeneration = (new global::System.Windows.Forms.NumericUpDown());
            this.label85 = (new global::System.Windows.Forms.Label());
            this.label86 = (new global::System.Windows.Forms.Label());
            this.label87 = (new global::System.Windows.Forms.Label());
            this.label88 = (new global::System.Windows.Forms.Label());
            this.nudNPCMovementPerLevelUp = (new global::System.Windows.Forms.NumericUpDown());
            this.nudNPCBaseMovement = (new global::System.Windows.Forms.NumericUpDown());
            this.label89 = (new global::System.Windows.Forms.Label());
            this.nudNPCDefensePerLevelUp = (new global::System.Windows.Forms.NumericUpDown());
            this.label90 = (new global::System.Windows.Forms.Label());
            this.nudNPCBaseDefense = (new global::System.Windows.Forms.NumericUpDown());
            this.label91 = (new global::System.Windows.Forms.Label());
            this.label92 = (new global::System.Windows.Forms.Label());
            this.nudNPCAttackPerLevelUp = (new global::System.Windows.Forms.NumericUpDown());
            this.label93 = (new global::System.Windows.Forms.Label());
            this.nudNPCBaseAttack = (new global::System.Windows.Forms.NumericUpDown());
            this.label94 = (new global::System.Windows.Forms.Label());
            this.nudNPCHPPerLevelUp = (new global::System.Windows.Forms.NumericUpDown());
            this.label95 = (new global::System.Windows.Forms.Label());
            this.nudNPCBaseHP = (new global::System.Windows.Forms.NumericUpDown());
            this.label96 = (new global::System.Windows.Forms.Label());
            this.btnChangeNPCConsoleCharacterForeColor = (new global::System.Windows.Forms.Button());
            this.btnChangeNPCConsoleCharacter = (new global::System.Windows.Forms.Button());
            this.lblNPCConsoleRepresentation = (new global::System.Windows.Forms.Label());
            this.label98 = (new global::System.Windows.Forms.Label());
            this.chkNPCStartsVisible = (new global::System.Windows.Forms.CheckBox());
            this.cmbNPCFaction = (new global::System.Windows.Forms.ComboBox());
            this.label99 = (new global::System.Windows.Forms.Label());
            this.fklblNPCDescriptionLocale = (new global::System.Windows.Forms.Button());
            this.txtNPCDescription = (new global::System.Windows.Forms.TextBox());
            this.label100 = (new global::System.Windows.Forms.Label());
            this.fklblNPCNameLocale = (new global::System.Windows.Forms.Button());
            this.txtNPCName = (new global::System.Windows.Forms.TextBox());
            this.label101 = (new global::System.Windows.Forms.Label());
            this.lblNPCSightRangeText = (new global::System.Windows.Forms.Label());
            this.tpItem = (new global::System.Windows.Forms.TabPage());
            this.btnItemOnTurnStartAction = (new global::System.Windows.Forms.Button());
            this.lblItemOnTurnStartAction = (new global::System.Windows.Forms.Label());
            this.btnItemOnAttackedAction = (new global::System.Windows.Forms.Button());
            this.lblItemOnAttackedAction = (new global::System.Windows.Forms.Label());
            this.btnRemoveItemOnAttackAction = (new global::System.Windows.Forms.Button());
            this.btnEditItemOnAttackAction = (new global::System.Windows.Forms.Button());
            this.btnAddItemOnAttackAction = (new global::System.Windows.Forms.Button());
            this.lbItemOnAttackActions = (new global::System.Windows.Forms.ListBox());
            this.lblItemOnAttackActions = (new global::System.Windows.Forms.Label());
            this.btnItemOnUseAction = (new global::System.Windows.Forms.Button());
            this.lblItemOnUseAction = (new global::System.Windows.Forms.Label());
            this.btnItemOnSteppedAction = (new global::System.Windows.Forms.Button());
            this.lblItemOnSteppedAction = (new global::System.Windows.Forms.Label());
            this.txtItemPower = (new global::System.Windows.Forms.TextBox());
            this.label108 = (new global::System.Windows.Forms.Label());
            this.chkItemCanBePickedUp = (new global::System.Windows.Forms.CheckBox());
            this.chkItemStartsVisible = (new global::System.Windows.Forms.CheckBox());
            this.cmbItemType = (new global::System.Windows.Forms.ComboBox());
            this.label107 = (new global::System.Windows.Forms.Label());
            this.btnChangeItemConsoleCharacterBackColor = (new global::System.Windows.Forms.Button());
            this.btnChangeItemConsoleCharacterForeColor = (new global::System.Windows.Forms.Button());
            this.btnChangeItemConsoleCharacter = (new global::System.Windows.Forms.Button());
            this.lblItemConsoleRepresentation = (new global::System.Windows.Forms.Label());
            this.label102 = (new global::System.Windows.Forms.Label());
            this.fklblItemDescriptionLocale = (new global::System.Windows.Forms.Button());
            this.txtItemDescription = (new global::System.Windows.Forms.TextBox());
            this.label105 = (new global::System.Windows.Forms.Label());
            this.fklblItemNameLocale = (new global::System.Windows.Forms.Button());
            this.txtItemName = (new global::System.Windows.Forms.TextBox());
            this.label106 = (new global::System.Windows.Forms.Label());
            this.tpTrap = (new global::System.Windows.Forms.TabPage());
            this.btnTrapOnSteppedAction = (new global::System.Windows.Forms.Button());
            this.label112 = (new global::System.Windows.Forms.Label());
            this.txtTrapPower = (new global::System.Windows.Forms.TextBox());
            this.label113 = (new global::System.Windows.Forms.Label());
            this.chkTrapStartsVisible = (new global::System.Windows.Forms.CheckBox());
            this.btnChangeTrapConsoleCharacterBackColor = (new global::System.Windows.Forms.Button());
            this.btnChangeTrapConsoleCharacterForeColor = (new global::System.Windows.Forms.Button());
            this.btnChangeTrapConsoleCharacter = (new global::System.Windows.Forms.Button());
            this.lblTrapConsoleRepresentation = (new global::System.Windows.Forms.Label());
            this.label116 = (new global::System.Windows.Forms.Label());
            this.fklblTrapDescriptionLocale = (new global::System.Windows.Forms.Button());
            this.txtTrapDescription = (new global::System.Windows.Forms.TextBox());
            this.label117 = (new global::System.Windows.Forms.Label());
            this.fklblTrapNameLocale = (new global::System.Windows.Forms.Button());
            this.txtTrapName = (new global::System.Windows.Forms.TextBox());
            this.label118 = (new global::System.Windows.Forms.Label());
            this.tpAlteredStatus = (new global::System.Windows.Forms.TabPage());
            this.btnAlteredStatusOnTurnStartAction = (new global::System.Windows.Forms.Button());
            this.label109 = (new global::System.Windows.Forms.Label());
            this.chkAlteredStatusCleansedOnCleanseActions = (new global::System.Windows.Forms.CheckBox());
            this.chkAlteredStatusCleanseOnFloorChange = (new global::System.Windows.Forms.CheckBox());
            this.chkAlteredStatusCanOverwrite = (new global::System.Windows.Forms.CheckBox());
            this.btnAlteredStatusOnApplyAction = (new global::System.Windows.Forms.Button());
            this.label97 = (new global::System.Windows.Forms.Label());
            this.chkAlteredStatusCanStack = (new global::System.Windows.Forms.CheckBox());
            this.btnChangeAlteredStatusConsoleCharacterBackColor = (new global::System.Windows.Forms.Button());
            this.btnChangeAlteredStatusConsoleCharacterForeColor = (new global::System.Windows.Forms.Button());
            this.btnChangeAlteredStatusConsoleCharacter = (new global::System.Windows.Forms.Button());
            this.lblAlteredStatusConsoleRepresentation = (new global::System.Windows.Forms.Label());
            this.label111 = (new global::System.Windows.Forms.Label());
            this.fklblAlteredStatusDescriptionLocale = (new global::System.Windows.Forms.Button());
            this.txtAlteredStatusDescription = (new global::System.Windows.Forms.TextBox());
            this.label114 = (new global::System.Windows.Forms.Label());
            this.fklblAlteredStatusNameLocale = (new global::System.Windows.Forms.Button());
            this.txtAlteredStatusName = (new global::System.Windows.Forms.TextBox());
            this.label115 = (new global::System.Windows.Forms.Label());
            this.tpValidation = (new global::System.Windows.Forms.TabPage());
            this.tvValidationResults = (new global::System.Windows.Forms.TreeView());
            this.ofdDungeon = (new global::System.Windows.Forms.OpenFileDialog());
            this.sfdDungeon = (new global::System.Windows.Forms.SaveFileDialog());
            this.msMenu.SuspendLayout();
            this.tsButtons.SuspendLayout();
            this.tbTabs.SuspendLayout();
            this.tpBasicInfo.SuspendLayout();
            this.tpLocales.SuspendLayout();
            ((global::System.ComponentModel.ISupportInitialize)(this.dgvLocales)).BeginInit();
            this.tpFloorInfos.SuspendLayout();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudRoomFusionOdds)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudExtraRoomConnectionOdds)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudMaxRoomConnections)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudMaxFloorLevel)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudMinFloorLevel)).BeginInit();
            this.tpFactionInfos.SuspendLayout();
            this.tpPlayerClass.SuspendLayout();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudPlayerInventorySize)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudPlayerMaxLevel)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudPlayerFlatSightRange)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudPlayerHPRegenerationPerLevelUp)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudPlayerBaseHPRegeneration)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudPlayerMovementPerLevelUp)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudPlayerBaseMovement)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudPlayerDefensePerLevelUp)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudPlayerBaseDefense)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudPlayerAttackPerLevelUp)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudPlayerBaseAttack)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudPlayerHPPerLevelUp)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudPlayerBaseHP)).BeginInit();
            this.tpNPC.SuspendLayout();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudNPCOddsToTargetSelf)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudNPCInventorySize)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudNPCMaxLevel)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudNPCFlatSightRange)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudNPCHPRegenerationPerLevelUp)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudNPCBaseHPRegeneration)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudNPCMovementPerLevelUp)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudNPCBaseMovement)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudNPCDefensePerLevelUp)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudNPCBaseDefense)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudNPCAttackPerLevelUp)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudNPCBaseAttack)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudNPCHPPerLevelUp)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudNPCBaseHP)).BeginInit();
            this.tpItem.SuspendLayout();
            this.tpTrap.SuspendLayout();
            this.tpAlteredStatus.SuspendLayout();
            this.tpValidation.SuspendLayout();
            this.SuspendLayout();
            // 
            // msMenu
            // 
            this.msMenu.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.editorToolStripMenuItem });
            this.msMenu.Location = (new global::System.Drawing.Point(0, 0));
            this.msMenu.Name = ("msMenu");
            this.msMenu.Size = (new global::System.Drawing.Size(967, 24));
            this.msMenu.TabIndex = (0);
            this.msMenu.Text = ("menuStrip1");
            // 
            // editorToolStripMenuItem
            // 
            this.editorToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.exitToolStripMenuItem });
            this.editorToolStripMenuItem.Name = ("editorToolStripMenuItem");
            this.editorToolStripMenuItem.Size = (new global::System.Drawing.Size(50, 20));
            this.editorToolStripMenuItem.Text = ("Editor");
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = ("exitToolStripMenuItem");
            this.exitToolStripMenuItem.Size = (new global::System.Drawing.Size(93, 22));
            this.exitToolStripMenuItem.Text = ("Exit");
            this.exitToolStripMenuItem.Click += (this.exitToolStripMenuItem_Click);
            // 
            // tsButtons
            // 
            this.tsButtons.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.tsbNewDungeon, this.tsbOpenDungeon, this.tsbSaveDungeon, this.tsbSaveDungeonAs, this.tssDungeonElement, this.tsbAddElement, this.tsbSaveElement, this.tsbSaveElementAs, this.tsbDeleteElement, this.tssElementValidate, this.tsbValidateDungeon });
            this.tsButtons.Location = (new global::System.Drawing.Point(0, 24));
            this.tsButtons.Name = ("tsButtons");
            this.tsButtons.Size = (new global::System.Drawing.Size(967, 38));
            this.tsButtons.TabIndex = (1);
            this.tsButtons.Text = ("toolStrip1");
            // 
            // tsbNewDungeon
            // 
            this.tsbNewDungeon.Image = ((global::System.Drawing.Image)(resources.GetObject("tsbNewDungeon.Image")));
            this.tsbNewDungeon.ImageTransparentColor = (global::System.Drawing.Color.Magenta);
            this.tsbNewDungeon.Name = ("tsbNewDungeon");
            this.tsbNewDungeon.Size = (new global::System.Drawing.Size(87, 35));
            this.tsbNewDungeon.Text = ("New Dungeon");
            this.tsbNewDungeon.TextImageRelation = (global::System.Windows.Forms.TextImageRelation.ImageAboveText);
            this.tsbNewDungeon.ToolTipText = ("Create a new, empty Dungeon");
            this.tsbNewDungeon.Click += (this.tsbNewDungeon_Click);
            // 
            // tsbOpenDungeon
            // 
            this.tsbOpenDungeon.Image = ((global::System.Drawing.Image)(resources.GetObject("tsbOpenDungeon.Image")));
            this.tsbOpenDungeon.ImageTransparentColor = (global::System.Drawing.Color.Magenta);
            this.tsbOpenDungeon.Name = ("tsbOpenDungeon");
            this.tsbOpenDungeon.Size = (new global::System.Drawing.Size(92, 35));
            this.tsbOpenDungeon.Text = ("Open Dungeon");
            this.tsbOpenDungeon.TextImageRelation = (global::System.Windows.Forms.TextImageRelation.ImageAboveText);
            this.tsbOpenDungeon.ToolTipText = ("Edit an existing Dungeon");
            this.tsbOpenDungeon.Click += (this.tsbOpenDungeon_Click);
            // 
            // tsbSaveDungeon
            // 
            this.tsbSaveDungeon.Image = ((global::System.Drawing.Image)(resources.GetObject("tsbSaveDungeon.Image")));
            this.tsbSaveDungeon.ImageTransparentColor = (global::System.Drawing.Color.Magenta);
            this.tsbSaveDungeon.Name = ("tsbSaveDungeon");
            this.tsbSaveDungeon.Size = (new global::System.Drawing.Size(87, 35));
            this.tsbSaveDungeon.Text = ("Save Dungeon");
            this.tsbSaveDungeon.TextImageRelation = (global::System.Windows.Forms.TextImageRelation.ImageAboveText);
            this.tsbSaveDungeon.ToolTipText = ("Save Dungeon to a file");
            this.tsbSaveDungeon.Visible = (false);
            this.tsbSaveDungeon.Click += (this.tsbSaveDungeon_Click);
            // 
            // tsbSaveDungeonAs
            // 
            this.tsbSaveDungeonAs.Image = ((global::System.Drawing.Image)(resources.GetObject("tsbSaveDungeonAs.Image")));
            this.tsbSaveDungeonAs.ImageTransparentColor = (global::System.Drawing.Color.Magenta);
            this.tsbSaveDungeonAs.Name = ("tsbSaveDungeonAs");
            this.tsbSaveDungeonAs.Size = (new global::System.Drawing.Size(112, 35));
            this.tsbSaveDungeonAs.Text = ("Save Dungeon As...");
            this.tsbSaveDungeonAs.TextImageRelation = (global::System.Windows.Forms.TextImageRelation.ImageAboveText);
            this.tsbSaveDungeonAs.ToolTipText = ("Save the Dungeon to a file of your choosing");
            this.tsbSaveDungeonAs.Visible = (false);
            this.tsbSaveDungeonAs.Click += (this.tsbSaveDungeonAs_Click);
            // 
            // tssDungeonElement
            // 
            this.tssDungeonElement.Name = ("tssDungeonElement");
            this.tssDungeonElement.Size = (new global::System.Drawing.Size(6, 38));
            this.tssDungeonElement.Visible = (false);
            // 
            // tsbAddElement
            // 
            this.tsbAddElement.Image = ((global::System.Drawing.Image)(resources.GetObject("tsbAddElement.Image")));
            this.tsbAddElement.ImageTransparentColor = (global::System.Drawing.Color.Magenta);
            this.tsbAddElement.Name = ("tsbAddElement");
            this.tsbAddElement.Size = (new global::System.Drawing.Size(81, 35));
            this.tsbAddElement.Text = ("New Element");
            this.tsbAddElement.TextImageRelation = (global::System.Windows.Forms.TextImageRelation.ImageAboveText);
            this.tsbAddElement.ToolTipText = ("Add a new element of this category");
            this.tsbAddElement.Visible = (false);
            this.tsbAddElement.Click += (this.tsbAddElement_Click);
            // 
            // tsbSaveElement
            // 
            this.tsbSaveElement.Image = ((global::System.Drawing.Image)(resources.GetObject("tsbSaveElement.Image")));
            this.tsbSaveElement.ImageTransparentColor = (global::System.Drawing.Color.Magenta);
            this.tsbSaveElement.Name = ("tsbSaveElement");
            this.tsbSaveElement.Size = (new global::System.Drawing.Size(81, 35));
            this.tsbSaveElement.Text = ("Save Element");
            this.tsbSaveElement.TextImageRelation = (global::System.Windows.Forms.TextImageRelation.ImageAboveText);
            this.tsbSaveElement.ToolTipText = ("Save currently-opened Element");
            this.tsbSaveElement.Visible = (false);
            this.tsbSaveElement.Click += (this.tsbSaveElement_Click);
            // 
            // tsbSaveElementAs
            // 
            this.tsbSaveElementAs.Image = ((global::System.Drawing.Image)(resources.GetObject("tsbSaveElementAs.Image")));
            this.tsbSaveElementAs.ImageTransparentColor = (global::System.Drawing.Color.Magenta);
            this.tsbSaveElementAs.Name = ("tsbSaveElementAs");
            this.tsbSaveElementAs.Size = (new global::System.Drawing.Size(133, 35));
            this.tsbSaveElementAs.Text = ("Save As New Element...");
            this.tsbSaveElementAs.TextImageRelation = (global::System.Windows.Forms.TextImageRelation.ImageAboveText);
            this.tsbSaveElementAs.ToolTipText = ("Save currently-opened Element with another name");
            this.tsbSaveElementAs.Visible = (false);
            this.tsbSaveElementAs.Click += (this.tsbSaveElementAs_Click);
            // 
            // tsbDeleteElement
            // 
            this.tsbDeleteElement.Image = ((global::System.Drawing.Image)(resources.GetObject("tsbDeleteElement.Image")));
            this.tsbDeleteElement.ImageTransparentColor = (global::System.Drawing.Color.Magenta);
            this.tsbDeleteElement.Name = ("tsbDeleteElement");
            this.tsbDeleteElement.Size = (new global::System.Drawing.Size(90, 35));
            this.tsbDeleteElement.Text = ("Delete Element");
            this.tsbDeleteElement.TextImageRelation = (global::System.Windows.Forms.TextImageRelation.ImageAboveText);
            this.tsbDeleteElement.ToolTipText = ("Remove the currently open Element");
            this.tsbDeleteElement.Visible = (false);
            this.tsbDeleteElement.Click += (this.tsbDeleteElement_Click);
            // 
            // tssElementValidate
            // 
            this.tssElementValidate.Name = ("tssElementValidate");
            this.tssElementValidate.Size = (new global::System.Drawing.Size(6, 38));
            this.tssElementValidate.Visible = (false);
            // 
            // tsbValidateDungeon
            // 
            this.tsbValidateDungeon.Image = ((global::System.Drawing.Image)(resources.GetObject("tsbValidateDungeon.Image")));
            this.tsbValidateDungeon.ImageTransparentColor = (global::System.Drawing.Color.Magenta);
            this.tsbValidateDungeon.Name = ("tsbValidateDungeon");
            this.tsbValidateDungeon.Size = (new global::System.Drawing.Size(104, 35));
            this.tsbValidateDungeon.Text = ("Validate Dungeon");
            this.tsbValidateDungeon.TextImageRelation = (global::System.Windows.Forms.TextImageRelation.ImageAboveText);
            this.tsbValidateDungeon.ToolTipText = ("Run the Dungeon Validator to check if it won't crash Rogue Customs");
            this.tsbValidateDungeon.Visible = (false);
            this.tsbValidateDungeon.Click += (this.tsbValidateDungeon_Click);
            // 
            // tvDungeonInfo
            // 
            this.tvDungeonInfo.Location = (new global::System.Drawing.Point(0, 65));
            this.tvDungeonInfo.Name = ("tvDungeonInfo");
            this.tvDungeonInfo.Size = (new global::System.Drawing.Size(217, 384));
            this.tvDungeonInfo.TabIndex = (2);
            this.tvDungeonInfo.NodeMouseClick += (this.tvDungeonInfo_NodeMouseClick);
            // 
            // tbTabs
            // 
            this.tbTabs.Controls.Add(this.tpBasicInfo);
            this.tbTabs.Controls.Add(this.tpLocales);
            this.tbTabs.Controls.Add(this.tpFloorInfos);
            this.tbTabs.Controls.Add(this.tpFactionInfos);
            this.tbTabs.Controls.Add(this.tpPlayerClass);
            this.tbTabs.Controls.Add(this.tpNPC);
            this.tbTabs.Controls.Add(this.tpItem);
            this.tbTabs.Controls.Add(this.tpTrap);
            this.tbTabs.Controls.Add(this.tpAlteredStatus);
            this.tbTabs.Controls.Add(this.tpValidation);
            this.tbTabs.Location = (new global::System.Drawing.Point(219, 65));
            this.tbTabs.Name = ("tbTabs");
            this.tbTabs.SelectedIndex = (0);
            this.tbTabs.Size = (new global::System.Drawing.Size(748, 384));
            this.tbTabs.TabIndex = (3);
            this.tbTabs.SelectedIndexChanged += (this.tbTabs_SelectedIndexChanged);
            // 
            // tpBasicInfo
            // 
            this.tpBasicInfo.Controls.Add(this.cmbDefaultLocale);
            this.tpBasicInfo.Controls.Add(this.label8);
            this.tpBasicInfo.Controls.Add(this.fklblEndingMessageLocale);
            this.tpBasicInfo.Controls.Add(this.fklblWelcomeMessageLocale);
            this.tpBasicInfo.Controls.Add(this.fklblAuthorLocale);
            this.tpBasicInfo.Controls.Add(this.fklblDungeonNameLocale);
            this.tpBasicInfo.Controls.Add(this.txtEndingMessage);
            this.tpBasicInfo.Controls.Add(this.label4);
            this.tpBasicInfo.Controls.Add(this.txtWelcomeMessage);
            this.tpBasicInfo.Controls.Add(this.label3);
            this.tpBasicInfo.Controls.Add(this.txtAuthor);
            this.tpBasicInfo.Controls.Add(this.label2);
            this.tpBasicInfo.Controls.Add(this.txtDungeonName);
            this.tpBasicInfo.Controls.Add(this.label1);
            this.tpBasicInfo.Location = (new global::System.Drawing.Point(4, 24));
            this.tpBasicInfo.Name = ("tpBasicInfo");
            this.tpBasicInfo.Padding = (new global::System.Windows.Forms.Padding(3));
            this.tpBasicInfo.Size = (new global::System.Drawing.Size(740, 356));
            this.tpBasicInfo.TabIndex = (0);
            this.tpBasicInfo.Text = ("Basic Information");
            this.tpBasicInfo.UseVisualStyleBackColor = (true);
            // 
            // cmbDefaultLocale
            // 
            this.cmbDefaultLocale.DropDownStyle = (global::System.Windows.Forms.ComboBoxStyle.DropDownList);
            this.cmbDefaultLocale.FormattingEnabled = (true);
            this.cmbDefaultLocale.Location = (new global::System.Drawing.Point(188, 228));
            this.cmbDefaultLocale.Name = ("cmbDefaultLocale");
            this.cmbDefaultLocale.Size = (new global::System.Drawing.Size(81, 23));
            this.cmbDefaultLocale.TabIndex = (17);
            this.cmbDefaultLocale.SelectedIndexChanged += (this.cmbDefaultLocale_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = (true);
            this.label8.Location = (new global::System.Drawing.Point(11, 223));
            this.label8.Name = ("label8");
            this.label8.Size = (new global::System.Drawing.Size(171, 30));
            this.label8.TabIndex = (16);
            this.label8.Text = ("If the game has a language this\r\ndungeon lacks, use this locale:");
            // 
            // fklblEndingMessageLocale
            // 
            this.fklblEndingMessageLocale.Enabled = (false);
            this.fklblEndingMessageLocale.FlatAppearance.BorderSize = (0);
            this.fklblEndingMessageLocale.FlatStyle = (global::System.Windows.Forms.FlatStyle.Flat);
            this.fklblEndingMessageLocale.Image = ((global::System.Drawing.Image)(resources.GetObject("fklblEndingMessageLocale.Image")));
            this.fklblEndingMessageLocale.ImageAlign = (global::System.Drawing.ContentAlignment.TopLeft);
            this.fklblEndingMessageLocale.Location = (new global::System.Drawing.Point(401, 304));
            this.fklblEndingMessageLocale.Name = ("fklblEndingMessageLocale");
            this.fklblEndingMessageLocale.Size = (new global::System.Drawing.Size(331, 42));
            this.fklblEndingMessageLocale.TabIndex = (15);
            this.fklblEndingMessageLocale.Text = ("This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.");
            this.fklblEndingMessageLocale.TextAlign = (global::System.Drawing.ContentAlignment.MiddleLeft);
            this.fklblEndingMessageLocale.TextImageRelation = (global::System.Windows.Forms.TextImageRelation.ImageBeforeText);
            this.fklblEndingMessageLocale.UseVisualStyleBackColor = (true);
            this.fklblEndingMessageLocale.Visible = (false);
            // 
            // fklblWelcomeMessageLocale
            // 
            this.fklblWelcomeMessageLocale.Enabled = (false);
            this.fklblWelcomeMessageLocale.FlatAppearance.BorderSize = (0);
            this.fklblWelcomeMessageLocale.FlatStyle = (global::System.Windows.Forms.FlatStyle.Flat);
            this.fklblWelcomeMessageLocale.Image = ((global::System.Drawing.Image)(resources.GetObject("fklblWelcomeMessageLocale.Image")));
            this.fklblWelcomeMessageLocale.ImageAlign = (global::System.Drawing.ContentAlignment.TopLeft);
            this.fklblWelcomeMessageLocale.Location = (new global::System.Drawing.Point(403, 132));
            this.fklblWelcomeMessageLocale.Name = ("fklblWelcomeMessageLocale");
            this.fklblWelcomeMessageLocale.Size = (new global::System.Drawing.Size(331, 42));
            this.fklblWelcomeMessageLocale.TabIndex = (14);
            this.fklblWelcomeMessageLocale.Text = ("This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.");
            this.fklblWelcomeMessageLocale.TextAlign = (global::System.Drawing.ContentAlignment.MiddleLeft);
            this.fklblWelcomeMessageLocale.TextImageRelation = (global::System.Windows.Forms.TextImageRelation.ImageBeforeText);
            this.fklblWelcomeMessageLocale.UseVisualStyleBackColor = (true);
            this.fklblWelcomeMessageLocale.Visible = (false);
            // 
            // fklblAuthorLocale
            // 
            this.fklblAuthorLocale.Enabled = (false);
            this.fklblAuthorLocale.FlatAppearance.BorderSize = (0);
            this.fklblAuthorLocale.FlatStyle = (global::System.Windows.Forms.FlatStyle.Flat);
            this.fklblAuthorLocale.Image = ((global::System.Drawing.Image)(resources.GetObject("fklblAuthorLocale.Image")));
            this.fklblAuthorLocale.ImageAlign = (global::System.Drawing.ContentAlignment.TopLeft);
            this.fklblAuthorLocale.Location = (new global::System.Drawing.Point(11, 168));
            this.fklblAuthorLocale.Name = ("fklblAuthorLocale");
            this.fklblAuthorLocale.Size = (new global::System.Drawing.Size(331, 42));
            this.fklblAuthorLocale.TabIndex = (13);
            this.fklblAuthorLocale.Text = ("This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.");
            this.fklblAuthorLocale.TextAlign = (global::System.Drawing.ContentAlignment.MiddleLeft);
            this.fklblAuthorLocale.TextImageRelation = (global::System.Windows.Forms.TextImageRelation.ImageBeforeText);
            this.fklblAuthorLocale.UseVisualStyleBackColor = (true);
            this.fklblAuthorLocale.Visible = (false);
            // 
            // fklblDungeonNameLocale
            // 
            this.fklblDungeonNameLocale.Enabled = (false);
            this.fklblDungeonNameLocale.FlatAppearance.BorderSize = (0);
            this.fklblDungeonNameLocale.FlatStyle = (global::System.Windows.Forms.FlatStyle.Flat);
            this.fklblDungeonNameLocale.Image = ((global::System.Drawing.Image)(resources.GetObject("fklblDungeonNameLocale.Image")));
            this.fklblDungeonNameLocale.ImageAlign = (global::System.Drawing.ContentAlignment.TopLeft);
            this.fklblDungeonNameLocale.Location = (new global::System.Drawing.Point(11, 55));
            this.fklblDungeonNameLocale.Name = ("fklblDungeonNameLocale");
            this.fklblDungeonNameLocale.Size = (new global::System.Drawing.Size(331, 42));
            this.fklblDungeonNameLocale.TabIndex = (12);
            this.fklblDungeonNameLocale.Text = ("This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.");
            this.fklblDungeonNameLocale.TextAlign = (global::System.Drawing.ContentAlignment.MiddleLeft);
            this.fklblDungeonNameLocale.TextImageRelation = (global::System.Windows.Forms.TextImageRelation.ImageBeforeText);
            this.fklblDungeonNameLocale.UseVisualStyleBackColor = (true);
            this.fklblDungeonNameLocale.Visible = (false);
            // 
            // txtEndingMessage
            // 
            this.txtEndingMessage.Location = (new global::System.Drawing.Point(403, 195));
            this.txtEndingMessage.Multiline = (true);
            this.txtEndingMessage.Name = ("txtEndingMessage");
            this.txtEndingMessage.ScrollBars = (global::System.Windows.Forms.ScrollBars.Vertical);
            this.txtEndingMessage.Size = (new global::System.Drawing.Size(313, 103));
            this.txtEndingMessage.TabIndex = (7);
            this.txtEndingMessage.TextChanged += (this.txtEndingMessage_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = (true);
            this.label4.Location = (new global::System.Drawing.Point(403, 177));
            this.label4.Name = ("label4");
            this.label4.Size = (new global::System.Drawing.Size(93, 15));
            this.label4.TabIndex = (6);
            this.label4.Text = ("Ending Message");
            // 
            // txtWelcomeMessage
            // 
            this.txtWelcomeMessage.Location = (new global::System.Drawing.Point(403, 26));
            this.txtWelcomeMessage.Multiline = (true);
            this.txtWelcomeMessage.Name = ("txtWelcomeMessage");
            this.txtWelcomeMessage.ScrollBars = (global::System.Windows.Forms.ScrollBars.Vertical);
            this.txtWelcomeMessage.Size = (new global::System.Drawing.Size(313, 103));
            this.txtWelcomeMessage.TabIndex = (5);
            this.txtWelcomeMessage.TextChanged += (this.txtWelcomeMessage_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = (true);
            this.label3.Location = (new global::System.Drawing.Point(403, 8));
            this.label3.Name = ("label3");
            this.label3.Size = (new global::System.Drawing.Size(106, 15));
            this.label3.TabIndex = (4);
            this.label3.Text = ("Welcome Message");
            // 
            // txtAuthor
            // 
            this.txtAuthor.Location = (new global::System.Drawing.Point(11, 134));
            this.txtAuthor.Name = ("txtAuthor");
            this.txtAuthor.Size = (new global::System.Drawing.Size(359, 23));
            this.txtAuthor.TabIndex = (3);
            this.txtAuthor.TextChanged += (this.txtAuthor_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = (true);
            this.label2.Location = (new global::System.Drawing.Point(11, 116));
            this.label2.Name = ("label2");
            this.label2.Size = (new global::System.Drawing.Size(44, 15));
            this.label2.TabIndex = (2);
            this.label2.Text = ("Author");
            // 
            // txtDungeonName
            // 
            this.txtDungeonName.Location = (new global::System.Drawing.Point(11, 26));
            this.txtDungeonName.Name = ("txtDungeonName");
            this.txtDungeonName.Size = (new global::System.Drawing.Size(359, 23));
            this.txtDungeonName.TabIndex = (1);
            this.txtDungeonName.TextChanged += (this.txtDungeonName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = (true);
            this.label1.Location = (new global::System.Drawing.Point(11, 8));
            this.label1.Name = ("label1");
            this.label1.Size = (new global::System.Drawing.Size(39, 15));
            this.label1.TabIndex = (0);
            this.label1.Text = ("Name");
            // 
            // tpLocales
            // 
            this.tpLocales.Controls.Add(this.btnDeleteLocale);
            this.tpLocales.Controls.Add(this.btnAddLocale);
            this.tpLocales.Controls.Add(this.btnUpdateLocale);
            this.tpLocales.Controls.Add(this.fklblMissingLocale);
            this.tpLocales.Controls.Add(this.txtLocaleEntryValue);
            this.tpLocales.Controls.Add(this.label7);
            this.tpLocales.Controls.Add(this.txtLocaleEntryKey);
            this.tpLocales.Controls.Add(this.label6);
            this.tpLocales.Controls.Add(this.dgvLocales);
            this.tpLocales.Location = (new global::System.Drawing.Point(4, 24));
            this.tpLocales.Name = ("tpLocales");
            this.tpLocales.Padding = (new global::System.Windows.Forms.Padding(3));
            this.tpLocales.Size = (new global::System.Drawing.Size(740, 356));
            this.tpLocales.TabIndex = (1);
            this.tpLocales.Text = ("Locale Entries");
            this.tpLocales.UseVisualStyleBackColor = (true);
            // 
            // btnDeleteLocale
            // 
            this.btnDeleteLocale.Enabled = (false);
            this.btnDeleteLocale.Location = (new global::System.Drawing.Point(401, 326));
            this.btnDeleteLocale.Name = ("btnDeleteLocale");
            this.btnDeleteLocale.Size = (new global::System.Drawing.Size(331, 23));
            this.btnDeleteLocale.TabIndex = (16);
            this.btnDeleteLocale.Text = ("Delete Locale Entry");
            this.btnDeleteLocale.UseVisualStyleBackColor = (true);
            this.btnDeleteLocale.Click += (this.btnDeleteLocale_Click);
            // 
            // btnAddLocale
            // 
            this.btnAddLocale.Enabled = (false);
            this.btnAddLocale.Location = (new global::System.Drawing.Point(569, 297));
            this.btnAddLocale.Name = ("btnAddLocale");
            this.btnAddLocale.Size = (new global::System.Drawing.Size(163, 23));
            this.btnAddLocale.TabIndex = (15);
            this.btnAddLocale.Text = ("Add New Locale Entry");
            this.btnAddLocale.UseVisualStyleBackColor = (true);
            this.btnAddLocale.Click += (this.btnAddLocale_Click);
            // 
            // btnUpdateLocale
            // 
            this.btnUpdateLocale.Enabled = (false);
            this.btnUpdateLocale.Location = (new global::System.Drawing.Point(401, 297));
            this.btnUpdateLocale.Name = ("btnUpdateLocale");
            this.btnUpdateLocale.Size = (new global::System.Drawing.Size(162, 23));
            this.btnUpdateLocale.TabIndex = (14);
            this.btnUpdateLocale.Text = ("Update Locale Entry");
            this.btnUpdateLocale.UseVisualStyleBackColor = (true);
            this.btnUpdateLocale.Click += (this.btnUpdateLocale_Click);
            // 
            // fklblMissingLocale
            // 
            this.fklblMissingLocale.Enabled = (false);
            this.fklblMissingLocale.FlatAppearance.BorderSize = (0);
            this.fklblMissingLocale.FlatStyle = (global::System.Windows.Forms.FlatStyle.Flat);
            this.fklblMissingLocale.Image = ((global::System.Drawing.Image)(resources.GetObject("fklblMissingLocale.Image")));
            this.fklblMissingLocale.ImageAlign = (global::System.Drawing.ContentAlignment.TopLeft);
            this.fklblMissingLocale.Location = (new global::System.Drawing.Point(403, 228));
            this.fklblMissingLocale.Name = ("fklblMissingLocale");
            this.fklblMissingLocale.Size = (new global::System.Drawing.Size(331, 42));
            this.fklblMissingLocale.TabIndex = (13);
            this.fklblMissingLocale.Text = ("(Missing locale warning)");
            this.fklblMissingLocale.TextAlign = (global::System.Drawing.ContentAlignment.MiddleLeft);
            this.fklblMissingLocale.TextImageRelation = (global::System.Windows.Forms.TextImageRelation.ImageBeforeText);
            this.fklblMissingLocale.UseVisualStyleBackColor = (true);
            this.fklblMissingLocale.Visible = (false);
            // 
            // txtLocaleEntryValue
            // 
            this.txtLocaleEntryValue.Enabled = (false);
            this.txtLocaleEntryValue.Location = (new global::System.Drawing.Point(401, 79));
            this.txtLocaleEntryValue.Multiline = (true);
            this.txtLocaleEntryValue.Name = ("txtLocaleEntryValue");
            this.txtLocaleEntryValue.Size = (new global::System.Drawing.Size(331, 143));
            this.txtLocaleEntryValue.TabIndex = (4);
            this.txtLocaleEntryValue.TextChanged += (this.txtLocaleEntryValue_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = (true);
            this.label7.Location = (new global::System.Drawing.Point(401, 61));
            this.label7.Name = ("label7");
            this.label7.Size = (new global::System.Drawing.Size(102, 15));
            this.label7.TabIndex = (3);
            this.label7.Text = ("Locale Entry Value");
            // 
            // txtLocaleEntryKey
            // 
            this.txtLocaleEntryKey.Enabled = (false);
            this.txtLocaleEntryKey.Location = (new global::System.Drawing.Point(401, 25));
            this.txtLocaleEntryKey.Name = ("txtLocaleEntryKey");
            this.txtLocaleEntryKey.Size = (new global::System.Drawing.Size(331, 23));
            this.txtLocaleEntryKey.TabIndex = (2);
            this.txtLocaleEntryKey.TextChanged += (this.txtLocaleEntryKey_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = (true);
            this.label6.Location = (new global::System.Drawing.Point(401, 7));
            this.label6.Name = ("label6");
            this.label6.Size = (new global::System.Drawing.Size(93, 15));
            this.label6.TabIndex = (1);
            this.label6.Text = ("Locale Entry Key");
            // 
            // dgvLocales
            // 
            this.dgvLocales.AllowUserToAddRows = (false);
            this.dgvLocales.AllowUserToDeleteRows = (false);
            this.dgvLocales.AllowUserToResizeColumns = (false);
            this.dgvLocales.AllowUserToResizeRows = (false);
            this.dgvLocales.ColumnHeadersHeightSizeMode = (global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize);
            this.dgvLocales.ColumnHeadersVisible = (false);
            this.dgvLocales.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.cmKey, this.cmValue });
            this.dgvLocales.Location = (new global::System.Drawing.Point(0, 0));
            this.dgvLocales.MultiSelect = (false);
            this.dgvLocales.Name = ("dgvLocales");
            this.dgvLocales.ReadOnly = (true);
            this.dgvLocales.RowHeadersVisible = (false);
            this.dgvLocales.RowTemplate.Height = (25);
            this.dgvLocales.ScrollBars = (global::System.Windows.Forms.ScrollBars.Vertical);
            this.dgvLocales.SelectionMode = (global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect);
            this.dgvLocales.Size = (new global::System.Drawing.Size(395, 356));
            this.dgvLocales.TabIndex = (0);
            this.dgvLocales.SelectionChanged += (this.dgvLocales_SelectionChanged);
            // 
            // cmKey
            // 
            dataGridViewCellStyle2.Font = (new global::System.Drawing.Font("Microsoft Sans Serif", 8.25F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point));
            this.cmKey.DefaultCellStyle = (dataGridViewCellStyle2);
            this.cmKey.HeaderText = ("Key");
            this.cmKey.Name = ("cmKey");
            this.cmKey.ReadOnly = (true);
            // 
            // cmValue
            // 
            this.cmValue.HeaderText = ("Value");
            this.cmValue.Name = ("cmValue");
            this.cmValue.ReadOnly = (true);
            this.cmValue.Width = (300);
            // 
            // tpFloorInfos
            // 
            this.tpFloorInfos.AutoScroll = (true);
            this.tpFloorInfos.Controls.Add(this.btnOnFloorStartAction);
            this.tpFloorInfos.Controls.Add(this.label20);
            this.tpFloorInfos.Controls.Add(this.nudRoomFusionOdds);
            this.tpFloorInfos.Controls.Add(this.label19);
            this.tpFloorInfos.Controls.Add(this.nudExtraRoomConnectionOdds);
            this.tpFloorInfos.Controls.Add(this.label18);
            this.tpFloorInfos.Controls.Add(this.nudMaxRoomConnections);
            this.tpFloorInfos.Controls.Add(this.label5);
            this.tpFloorInfos.Controls.Add(this.btnRemoveAlgorithm);
            this.tpFloorInfos.Controls.Add(this.btnEditAlgorithm);
            this.tpFloorInfos.Controls.Add(this.btnAddAlgorithm);
            this.tpFloorInfos.Controls.Add(this.lvFloorAlgorithms);
            this.tpFloorInfos.Controls.Add(this.label17);
            this.tpFloorInfos.Controls.Add(this.label16);
            this.tpFloorInfos.Controls.Add(this.btnTrapGenerator);
            this.tpFloorInfos.Controls.Add(this.label15);
            this.tpFloorInfos.Controls.Add(this.btnItemGenerator);
            this.tpFloorInfos.Controls.Add(this.label14);
            this.tpFloorInfos.Controls.Add(this.btnNPCGenerator);
            this.tpFloorInfos.Controls.Add(this.label13);
            this.tpFloorInfos.Controls.Add(this.nudHeight);
            this.tpFloorInfos.Controls.Add(this.nudWidth);
            this.tpFloorInfos.Controls.Add(this.label12);
            this.tpFloorInfos.Controls.Add(this.label11);
            this.tpFloorInfos.Controls.Add(this.fklblStairsReminder);
            this.tpFloorInfos.Controls.Add(this.chkGenerateStairsOnStart);
            this.tpFloorInfos.Controls.Add(this.nudMaxFloorLevel);
            this.tpFloorInfos.Controls.Add(this.label10);
            this.tpFloorInfos.Controls.Add(this.nudMinFloorLevel);
            this.tpFloorInfos.Controls.Add(this.label9);
            this.tpFloorInfos.Location = (new global::System.Drawing.Point(4, 24));
            this.tpFloorInfos.Name = ("tpFloorInfos");
            this.tpFloorInfos.Size = (new global::System.Drawing.Size(740, 356));
            this.tpFloorInfos.TabIndex = (2);
            this.tpFloorInfos.Text = ("Floor Group");
            this.tpFloorInfos.UseVisualStyleBackColor = (true);
            // 
            // btnOnFloorStartAction
            // 
            this.btnOnFloorStartAction.Location = (new global::System.Drawing.Point(528, 310));
            this.btnOnFloorStartAction.Name = ("btnOnFloorStartAction");
            this.btnOnFloorStartAction.Size = (new global::System.Drawing.Size(75, 23));
            this.btnOnFloorStartAction.TabIndex = (37);
            this.btnOnFloorStartAction.Text = ("... do this!");
            this.btnOnFloorStartAction.UseVisualStyleBackColor = (true);
            this.btnOnFloorStartAction.Click += (this.btnOnFloorStartAction_Click);
            // 
            // label20
            // 
            this.label20.AutoSize = (true);
            this.label20.Font = (new global::System.Drawing.Font("Segoe UI", 9F, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point));
            this.label20.Location = (new global::System.Drawing.Point(369, 314));
            this.label20.Name = ("label20");
            this.label20.Size = (new global::System.Drawing.Size(153, 15));
            this.label20.TabIndex = (36);
            this.label20.Text = ("When a Floor is generated...");
            // 
            // nudRoomFusionOdds
            // 
            this.nudRoomFusionOdds.Location = (new global::System.Drawing.Point(518, 269));
            this.nudRoomFusionOdds.Name = ("nudRoomFusionOdds");
            this.nudRoomFusionOdds.Size = (new global::System.Drawing.Size(40, 23));
            this.nudRoomFusionOdds.TabIndex = (35);
            this.nudRoomFusionOdds.ValueChanged += (this.nudRoomFusionOdds_ValueChanged);
            // 
            // label19
            // 
            this.label19.AutoSize = (true);
            this.label19.Location = (new global::System.Drawing.Point(369, 271));
            this.label19.Name = ("label19");
            this.label19.Size = (new global::System.Drawing.Size(285, 15));
            this.label19.TabIndex = (34);
            this.label19.Text = ("Two adjacent rooms have a               % chance to fuse");
            // 
            // nudExtraRoomConnectionOdds
            // 
            this.nudExtraRoomConnectionOdds.Location = (new global::System.Drawing.Point(470, 239));
            this.nudExtraRoomConnectionOdds.Name = ("nudExtraRoomConnectionOdds");
            this.nudExtraRoomConnectionOdds.Size = (new global::System.Drawing.Size(40, 23));
            this.nudExtraRoomConnectionOdds.TabIndex = (33);
            this.nudExtraRoomConnectionOdds.ValueChanged += (this.nudRoomConnectionOdds_ValueChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = (true);
            this.label18.Location = (new global::System.Drawing.Point(370, 242));
            this.label18.Name = ("label18");
            this.label18.Size = (new global::System.Drawing.Size(323, 15));
            this.label18.TabIndex = (32);
            this.label18.Text = ("(With a chance of               % of connecting more than once)");
            // 
            // nudMaxRoomConnections
            // 
            this.nudMaxRoomConnections.Location = (new global::System.Drawing.Point(619, 211));
            this.nudMaxRoomConnections.Maximum = (new global::System.Decimal(new global::System.Int32[] { 9, 0, 0, 0 }));
            this.nudMaxRoomConnections.Minimum = (new global::System.Decimal(new global::System.Int32[] { 1, 0, 0, 0 }));
            this.nudMaxRoomConnections.Name = ("nudMaxRoomConnections");
            this.nudMaxRoomConnections.Size = (new global::System.Drawing.Size(33, 23));
            this.nudMaxRoomConnections.TabIndex = (31);
            this.nudMaxRoomConnections.Value = (new global::System.Decimal(new global::System.Int32[] { 1, 0, 0, 0 }));
            this.nudMaxRoomConnections.ValueChanged += (this.nudMaxRoomConnections_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = (true);
            this.label5.Location = (new global::System.Drawing.Point(370, 213));
            this.label5.Name = ("label5");
            this.label5.Size = (new global::System.Drawing.Size(326, 15));
            this.label5.TabIndex = (30);
            this.label5.Text = ("Rooms can connect between each other up to             time(s)");
            // 
            // btnRemoveAlgorithm
            // 
            this.btnRemoveAlgorithm.Enabled = (false);
            this.btnRemoveAlgorithm.Location = (new global::System.Drawing.Point(586, 174));
            this.btnRemoveAlgorithm.Name = ("btnRemoveAlgorithm");
            this.btnRemoveAlgorithm.Size = (new global::System.Drawing.Size(75, 23));
            this.btnRemoveAlgorithm.TabIndex = (29);
            this.btnRemoveAlgorithm.Text = ("Remove");
            this.btnRemoveAlgorithm.UseVisualStyleBackColor = (true);
            this.btnRemoveAlgorithm.Click += (this.btnRemoveAlgorithm_Click);
            // 
            // btnEditAlgorithm
            // 
            this.btnEditAlgorithm.Enabled = (false);
            this.btnEditAlgorithm.Location = (new global::System.Drawing.Point(505, 174));
            this.btnEditAlgorithm.Name = ("btnEditAlgorithm");
            this.btnEditAlgorithm.Size = (new global::System.Drawing.Size(75, 23));
            this.btnEditAlgorithm.TabIndex = (28);
            this.btnEditAlgorithm.Text = ("Edit");
            this.btnEditAlgorithm.UseVisualStyleBackColor = (true);
            this.btnEditAlgorithm.Click += (this.btnEditAlgorithm_Click);
            // 
            // btnAddAlgorithm
            // 
            this.btnAddAlgorithm.Location = (new global::System.Drawing.Point(424, 174));
            this.btnAddAlgorithm.Name = ("btnAddAlgorithm");
            this.btnAddAlgorithm.Size = (new global::System.Drawing.Size(75, 23));
            this.btnAddAlgorithm.TabIndex = (27);
            this.btnAddAlgorithm.Text = ("New...");
            this.btnAddAlgorithm.UseVisualStyleBackColor = (true);
            this.btnAddAlgorithm.Click += (this.btnAddAlgorithm_Click);
            // 
            // lvFloorAlgorithms
            // 
            this.lvFloorAlgorithms.Location = (new global::System.Drawing.Point(370, 59));
            this.lvFloorAlgorithms.MultiSelect = (false);
            this.lvFloorAlgorithms.Name = ("lvFloorAlgorithms");
            this.lvFloorAlgorithms.Size = (new global::System.Drawing.Size(349, 109));
            this.lvFloorAlgorithms.TabIndex = (26);
            this.lvFloorAlgorithms.UseCompatibleStateImageBehavior = (false);
            this.lvFloorAlgorithms.SelectedIndexChanged += (this.lvFloorAlgorithms_SelectedIndexChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = (true);
            this.label17.Location = (new global::System.Drawing.Point(482, 40));
            this.label17.Name = ("label17");
            this.label17.Size = (new global::System.Drawing.Size(127, 15));
            this.label17.TabIndex = (25);
            this.label17.Text = ("Generation Algorithms");
            // 
            // label16
            // 
            this.label16.AutoSize = (true);
            this.label16.Font = (new global::System.Drawing.Font("Segoe UI", 12F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point));
            this.label16.Location = (new global::System.Drawing.Point(477, 10));
            this.label16.Name = ("label16");
            this.label16.Size = (new global::System.Drawing.Size(138, 21));
            this.label16.TabIndex = (24);
            this.label16.Text = ("Floor Room Data");
            // 
            // btnTrapGenerator
            // 
            this.btnTrapGenerator.Location = (new global::System.Drawing.Point(179, 310));
            this.btnTrapGenerator.Name = ("btnTrapGenerator");
            this.btnTrapGenerator.Size = (new global::System.Drawing.Size(151, 23));
            this.btnTrapGenerator.TabIndex = (23);
            this.btnTrapGenerator.Text = ("Traps to be generated...");
            this.btnTrapGenerator.UseVisualStyleBackColor = (true);
            this.btnTrapGenerator.Click += (this.btnTrapGenerator_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = (true);
            this.label15.Font = (new global::System.Drawing.Font("Segoe UI", 12F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point));
            this.label15.Location = (new global::System.Drawing.Point(190, 276));
            this.label15.Name = ("label15");
            this.label15.Size = (new global::System.Drawing.Size(126, 21));
            this.label15.TabIndex = (22);
            this.label15.Text = ("Floor Trap Data");
            // 
            // btnItemGenerator
            // 
            this.btnItemGenerator.Location = (new global::System.Drawing.Point(12, 310));
            this.btnItemGenerator.Name = ("btnItemGenerator");
            this.btnItemGenerator.Size = (new global::System.Drawing.Size(151, 23));
            this.btnItemGenerator.TabIndex = (21);
            this.btnItemGenerator.Text = ("Items to be generated...");
            this.btnItemGenerator.UseVisualStyleBackColor = (true);
            this.btnItemGenerator.Click += (this.btnItemGenerator_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = (true);
            this.label14.Font = (new global::System.Drawing.Font("Segoe UI", 12F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point));
            this.label14.Location = (new global::System.Drawing.Point(23, 276));
            this.label14.Name = ("label14");
            this.label14.Size = (new global::System.Drawing.Size(128, 21));
            this.label14.TabIndex = (20);
            this.label14.Text = ("Floor Item Data");
            // 
            // btnNPCGenerator
            // 
            this.btnNPCGenerator.Location = (new global::System.Drawing.Point(97, 242));
            this.btnNPCGenerator.Name = ("btnNPCGenerator");
            this.btnNPCGenerator.Size = (new global::System.Drawing.Size(151, 23));
            this.btnNPCGenerator.TabIndex = (19);
            this.btnNPCGenerator.Text = ("NPCs to be generated...");
            this.btnNPCGenerator.UseVisualStyleBackColor = (true);
            this.btnNPCGenerator.Click += (this.btnNPCGenerator_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = (true);
            this.label13.Font = (new global::System.Drawing.Font("Segoe UI", 12F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point));
            this.label13.Location = (new global::System.Drawing.Point(109, 208));
            this.label13.Name = ("label13");
            this.label13.Size = (new global::System.Drawing.Size(126, 21));
            this.label13.TabIndex = (18);
            this.label13.Text = ("Floor NPC Data");
            // 
            // nudHeight
            // 
            this.nudHeight.Location = (new global::System.Drawing.Point(54, 92));
            this.nudHeight.Name = ("nudHeight");
            this.nudHeight.Size = (new global::System.Drawing.Size(54, 23));
            this.nudHeight.TabIndex = (17);
            this.nudHeight.ValueChanged += (this.nudHeight_ValueChanged);
            // 
            // nudWidth
            // 
            this.nudWidth.Location = (new global::System.Drawing.Point(54, 59));
            this.nudWidth.Name = ("nudWidth");
            this.nudWidth.Size = (new global::System.Drawing.Size(54, 23));
            this.nudWidth.TabIndex = (16);
            this.nudWidth.ValueChanged += (this.nudWidth_ValueChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = (true);
            this.label12.Location = (new global::System.Drawing.Point(9, 94));
            this.label12.Name = ("label12");
            this.label12.Size = (new global::System.Drawing.Size(43, 15));
            this.label12.TabIndex = (15);
            this.label12.Text = ("Height");
            // 
            // label11
            // 
            this.label11.AutoSize = (true);
            this.label11.Location = (new global::System.Drawing.Point(9, 61));
            this.label11.Name = ("label11");
            this.label11.Size = (new global::System.Drawing.Size(39, 15));
            this.label11.TabIndex = (14);
            this.label11.Text = ("Width");
            // 
            // fklblStairsReminder
            // 
            this.fklblStairsReminder.Enabled = (false);
            this.fklblStairsReminder.FlatAppearance.BorderSize = (0);
            this.fklblStairsReminder.FlatStyle = (global::System.Windows.Forms.FlatStyle.Flat);
            this.fklblStairsReminder.Image = ((global::System.Drawing.Image)(resources.GetObject("fklblStairsReminder.Image")));
            this.fklblStairsReminder.ImageAlign = (global::System.Drawing.ContentAlignment.MiddleLeft);
            this.fklblStairsReminder.Location = (new global::System.Drawing.Point(9, 159));
            this.fklblStairsReminder.Name = ("fklblStairsReminder");
            this.fklblStairsReminder.Size = (new global::System.Drawing.Size(287, 42));
            this.fklblStairsReminder.TabIndex = (13);
            this.fklblStairsReminder.Text = ("Remember to include an element that\r\ngenerates Stairs, or it would softlock the game.");
            this.fklblStairsReminder.TextAlign = (global::System.Drawing.ContentAlignment.MiddleLeft);
            this.fklblStairsReminder.TextImageRelation = (global::System.Windows.Forms.TextImageRelation.ImageBeforeText);
            this.fklblStairsReminder.UseVisualStyleBackColor = (true);
            this.fklblStairsReminder.Visible = (false);
            // 
            // chkGenerateStairsOnStart
            // 
            this.chkGenerateStairsOnStart.AutoSize = (true);
            this.chkGenerateStairsOnStart.Location = (new global::System.Drawing.Point(9, 134));
            this.chkGenerateStairsOnStart.Name = ("chkGenerateStairsOnStart");
            this.chkGenerateStairsOnStart.Size = (new global::System.Drawing.Size(214, 19));
            this.chkGenerateStairsOnStart.TabIndex = (4);
            this.chkGenerateStairsOnStart.Text = ("Place Stairs when Floor is generated");
            this.chkGenerateStairsOnStart.UseVisualStyleBackColor = (true);
            this.chkGenerateStairsOnStart.CheckedChanged += (this.chkGenerateStairsOnStart_CheckedChanged);
            // 
            // nudMaxFloorLevel
            // 
            this.nudMaxFloorLevel.Location = (new global::System.Drawing.Point(138, 10));
            this.nudMaxFloorLevel.Name = ("nudMaxFloorLevel");
            this.nudMaxFloorLevel.Size = (new global::System.Drawing.Size(33, 23));
            this.nudMaxFloorLevel.TabIndex = (3);
            this.nudMaxFloorLevel.ValueChanged += (this.nudMaxFloorLevel_ValueChanged);
            this.nudMaxFloorLevel.Leave += (this.nudMaxFloorLevel_Leave);
            // 
            // label10
            // 
            this.label10.AutoSize = (true);
            this.label10.Location = (new global::System.Drawing.Point(114, 12));
            this.label10.Name = ("label10");
            this.label10.Size = (new global::System.Drawing.Size(18, 15));
            this.label10.TabIndex = (2);
            this.label10.Text = ("to");
            // 
            // nudMinFloorLevel
            // 
            this.nudMinFloorLevel.Location = (new global::System.Drawing.Point(76, 10));
            this.nudMinFloorLevel.Name = ("nudMinFloorLevel");
            this.nudMinFloorLevel.Size = (new global::System.Drawing.Size(33, 23));
            this.nudMinFloorLevel.TabIndex = (1);
            this.nudMinFloorLevel.ValueChanged += (this.nudMinFloorLevel_ValueChanged);
            this.nudMinFloorLevel.Leave += (this.nudMinFloorLevel_Leave);
            // 
            // label9
            // 
            this.label9.AutoSize = (true);
            this.label9.Location = (new global::System.Drawing.Point(9, 12));
            this.label9.Name = ("label9");
            this.label9.Size = (new global::System.Drawing.Size(65, 15));
            this.label9.TabIndex = (0);
            this.label9.Text = ("From Level");
            // 
            // tpFactionInfos
            // 
            this.tpFactionInfos.Controls.Add(this.lbEnemies);
            this.tpFactionInfos.Controls.Add(this.label26);
            this.tpFactionInfos.Controls.Add(this.btnEnemiesToNeutrals);
            this.tpFactionInfos.Controls.Add(this.btnEnemyToNeutral);
            this.tpFactionInfos.Controls.Add(this.btnNeutralsToEnemies);
            this.tpFactionInfos.Controls.Add(this.btnNeutralToEnemy);
            this.tpFactionInfos.Controls.Add(this.lbNeutrals);
            this.tpFactionInfos.Controls.Add(this.label25);
            this.tpFactionInfos.Controls.Add(this.btnNeutralsToAllies);
            this.tpFactionInfos.Controls.Add(this.btnNeutralToAlly);
            this.tpFactionInfos.Controls.Add(this.btnAlliesToNeutrals);
            this.tpFactionInfos.Controls.Add(this.btnAllyToNeutral);
            this.tpFactionInfos.Controls.Add(this.lbAllies);
            this.tpFactionInfos.Controls.Add(this.label24);
            this.tpFactionInfos.Controls.Add(this.label23);
            this.tpFactionInfos.Controls.Add(this.fklblFactionDescriptionLocale);
            this.tpFactionInfos.Controls.Add(this.txtFactionDescription);
            this.tpFactionInfos.Controls.Add(this.label22);
            this.tpFactionInfos.Controls.Add(this.fklblFactionNameLocale);
            this.tpFactionInfos.Controls.Add(this.txtFactionName);
            this.tpFactionInfos.Controls.Add(this.label21);
            this.tpFactionInfos.Location = (new global::System.Drawing.Point(4, 24));
            this.tpFactionInfos.Name = ("tpFactionInfos");
            this.tpFactionInfos.Size = (new global::System.Drawing.Size(740, 356));
            this.tpFactionInfos.TabIndex = (3);
            this.tpFactionInfos.Text = ("Faction");
            this.tpFactionInfos.UseVisualStyleBackColor = (true);
            // 
            // lbEnemies
            // 
            this.lbEnemies.FormattingEnabled = (true);
            this.lbEnemies.ItemHeight = (15);
            this.lbEnemies.Location = (new global::System.Drawing.Point(450, 161));
            this.lbEnemies.Name = ("lbEnemies");
            this.lbEnemies.Size = (new global::System.Drawing.Size(111, 169));
            this.lbEnemies.TabIndex = (33);
            this.lbEnemies.SelectedIndexChanged += (this.lbEnemies_SelectedIndexChanged);
            // 
            // label26
            // 
            this.label26.AutoSize = (true);
            this.label26.Font = (new global::System.Drawing.Font("Segoe UI", 9F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point));
            this.label26.Location = (new global::System.Drawing.Point(461, 143));
            this.label26.Name = ("label26");
            this.label26.Size = (new global::System.Drawing.Size(92, 15));
            this.label26.TabIndex = (32);
            this.label26.Text = ("Enemies With...");
            // 
            // btnEnemiesToNeutrals
            // 
            this.btnEnemiesToNeutrals.Enabled = (false);
            this.btnEnemiesToNeutrals.Location = (new global::System.Drawing.Point(407, 211));
            this.btnEnemiesToNeutrals.Name = ("btnEnemiesToNeutrals");
            this.btnEnemiesToNeutrals.Size = (new global::System.Drawing.Size(37, 23));
            this.btnEnemiesToNeutrals.TabIndex = (31);
            this.btnEnemiesToNeutrals.Text = ("<<");
            this.btnEnemiesToNeutrals.UseVisualStyleBackColor = (true);
            this.btnEnemiesToNeutrals.Click += (this.btnEnemiesToNeutrals_Click);
            // 
            // btnEnemyToNeutral
            // 
            this.btnEnemyToNeutral.Enabled = (false);
            this.btnEnemyToNeutral.Location = (new global::System.Drawing.Point(407, 182));
            this.btnEnemyToNeutral.Name = ("btnEnemyToNeutral");
            this.btnEnemyToNeutral.Size = (new global::System.Drawing.Size(37, 23));
            this.btnEnemyToNeutral.TabIndex = (30);
            this.btnEnemyToNeutral.Text = ("<");
            this.btnEnemyToNeutral.UseVisualStyleBackColor = (true);
            this.btnEnemyToNeutral.Click += (this.btnEnemyToNeutral_Click);
            // 
            // btnNeutralsToEnemies
            // 
            this.btnNeutralsToEnemies.Enabled = (false);
            this.btnNeutralsToEnemies.Location = (new global::System.Drawing.Point(407, 284));
            this.btnNeutralsToEnemies.Name = ("btnNeutralsToEnemies");
            this.btnNeutralsToEnemies.Size = (new global::System.Drawing.Size(37, 23));
            this.btnNeutralsToEnemies.TabIndex = (29);
            this.btnNeutralsToEnemies.Text = (">>");
            this.btnNeutralsToEnemies.UseVisualStyleBackColor = (true);
            this.btnNeutralsToEnemies.Click += (this.btnNeutralsToEnemies_Click);
            // 
            // btnNeutralToEnemy
            // 
            this.btnNeutralToEnemy.Enabled = (false);
            this.btnNeutralToEnemy.Location = (new global::System.Drawing.Point(407, 255));
            this.btnNeutralToEnemy.Name = ("btnNeutralToEnemy");
            this.btnNeutralToEnemy.Size = (new global::System.Drawing.Size(37, 23));
            this.btnNeutralToEnemy.TabIndex = (28);
            this.btnNeutralToEnemy.Text = (">");
            this.btnNeutralToEnemy.UseVisualStyleBackColor = (true);
            this.btnNeutralToEnemy.Click += (this.btnNeutralToEnemy_Click);
            // 
            // lbNeutrals
            // 
            this.lbNeutrals.FormattingEnabled = (true);
            this.lbNeutrals.ItemHeight = (15);
            this.lbNeutrals.Location = (new global::System.Drawing.Point(290, 161));
            this.lbNeutrals.Name = ("lbNeutrals");
            this.lbNeutrals.Size = (new global::System.Drawing.Size(111, 169));
            this.lbNeutrals.TabIndex = (27);
            this.lbNeutrals.SelectedIndexChanged += (this.lbNeutrals_SelectedIndexChanged);
            // 
            // label25
            // 
            this.label25.AutoSize = (true);
            this.label25.Font = (new global::System.Drawing.Font("Segoe UI", 9F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point));
            this.label25.Location = (new global::System.Drawing.Point(303, 143));
            this.label25.Name = ("label25");
            this.label25.Size = (new global::System.Drawing.Size(88, 15));
            this.label25.TabIndex = (26);
            this.label25.Text = ("Neutral With...");
            // 
            // btnNeutralsToAllies
            // 
            this.btnNeutralsToAllies.Enabled = (false);
            this.btnNeutralsToAllies.Location = (new global::System.Drawing.Point(247, 211));
            this.btnNeutralsToAllies.Name = ("btnNeutralsToAllies");
            this.btnNeutralsToAllies.Size = (new global::System.Drawing.Size(37, 23));
            this.btnNeutralsToAllies.TabIndex = (25);
            this.btnNeutralsToAllies.Text = ("<<");
            this.btnNeutralsToAllies.UseVisualStyleBackColor = (true);
            this.btnNeutralsToAllies.Click += (this.btnNeutralsToAllies_Click);
            // 
            // btnNeutralToAlly
            // 
            this.btnNeutralToAlly.Enabled = (false);
            this.btnNeutralToAlly.Location = (new global::System.Drawing.Point(247, 182));
            this.btnNeutralToAlly.Name = ("btnNeutralToAlly");
            this.btnNeutralToAlly.Size = (new global::System.Drawing.Size(37, 23));
            this.btnNeutralToAlly.TabIndex = (24);
            this.btnNeutralToAlly.Text = ("<");
            this.btnNeutralToAlly.UseVisualStyleBackColor = (true);
            this.btnNeutralToAlly.Click += (this.btnNeutralToAlly_Click);
            // 
            // btnAlliesToNeutrals
            // 
            this.btnAlliesToNeutrals.Enabled = (false);
            this.btnAlliesToNeutrals.Location = (new global::System.Drawing.Point(247, 284));
            this.btnAlliesToNeutrals.Name = ("btnAlliesToNeutrals");
            this.btnAlliesToNeutrals.Size = (new global::System.Drawing.Size(37, 23));
            this.btnAlliesToNeutrals.TabIndex = (23);
            this.btnAlliesToNeutrals.Text = (">>");
            this.btnAlliesToNeutrals.UseVisualStyleBackColor = (true);
            this.btnAlliesToNeutrals.Click += (this.btnAlliesToNeutrals_Click);
            // 
            // btnAllyToNeutral
            // 
            this.btnAllyToNeutral.Enabled = (false);
            this.btnAllyToNeutral.Location = (new global::System.Drawing.Point(247, 255));
            this.btnAllyToNeutral.Name = ("btnAllyToNeutral");
            this.btnAllyToNeutral.Size = (new global::System.Drawing.Size(37, 23));
            this.btnAllyToNeutral.TabIndex = (22);
            this.btnAllyToNeutral.Text = (">");
            this.btnAllyToNeutral.UseVisualStyleBackColor = (true);
            this.btnAllyToNeutral.Click += (this.btnAllyToNeutral_Click);
            // 
            // lbAllies
            // 
            this.lbAllies.FormattingEnabled = (true);
            this.lbAllies.ItemHeight = (15);
            this.lbAllies.Location = (new global::System.Drawing.Point(130, 161));
            this.lbAllies.Name = ("lbAllies");
            this.lbAllies.Size = (new global::System.Drawing.Size(111, 169));
            this.lbAllies.TabIndex = (21);
            this.lbAllies.SelectedIndexChanged += (this.lbAllies_SelectedIndexChanged);
            // 
            // label24
            // 
            this.label24.AutoSize = (true);
            this.label24.Font = (new global::System.Drawing.Font("Segoe UI", 9F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point));
            this.label24.Location = (new global::System.Drawing.Point(147, 143));
            this.label24.Name = ("label24");
            this.label24.Size = (new global::System.Drawing.Size(77, 15));
            this.label24.TabIndex = (20);
            this.label24.Text = ("Allied With...");
            // 
            // label23
            // 
            this.label23.AutoSize = (true);
            this.label23.Font = (new global::System.Drawing.Font("Segoe UI", 12F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point));
            this.label23.Location = (new global::System.Drawing.Point(296, 109));
            this.label23.Name = ("label23");
            this.label23.Size = (new global::System.Drawing.Size(98, 21));
            this.label23.TabIndex = (19);
            this.label23.Text = ("Allegiances");
            // 
            // fklblFactionDescriptionLocale
            // 
            this.fklblFactionDescriptionLocale.Enabled = (false);
            this.fklblFactionDescriptionLocale.FlatAppearance.BorderSize = (0);
            this.fklblFactionDescriptionLocale.FlatStyle = (global::System.Windows.Forms.FlatStyle.Flat);
            this.fklblFactionDescriptionLocale.Image = ((global::System.Drawing.Image)(resources.GetObject("fklblFactionDescriptionLocale.Image")));
            this.fklblFactionDescriptionLocale.ImageAlign = (global::System.Drawing.ContentAlignment.TopLeft);
            this.fklblFactionDescriptionLocale.Location = (new global::System.Drawing.Point(375, 59));
            this.fklblFactionDescriptionLocale.Name = ("fklblFactionDescriptionLocale");
            this.fklblFactionDescriptionLocale.Size = (new global::System.Drawing.Size(331, 42));
            this.fklblFactionDescriptionLocale.TabIndex = (18);
            this.fklblFactionDescriptionLocale.Text = ("This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.");
            this.fklblFactionDescriptionLocale.TextAlign = (global::System.Drawing.ContentAlignment.MiddleLeft);
            this.fklblFactionDescriptionLocale.TextImageRelation = (global::System.Windows.Forms.TextImageRelation.ImageBeforeText);
            this.fklblFactionDescriptionLocale.UseVisualStyleBackColor = (true);
            this.fklblFactionDescriptionLocale.Visible = (false);
            // 
            // txtFactionDescription
            // 
            this.txtFactionDescription.Location = (new global::System.Drawing.Point(375, 30));
            this.txtFactionDescription.Name = ("txtFactionDescription");
            this.txtFactionDescription.Size = (new global::System.Drawing.Size(359, 23));
            this.txtFactionDescription.TabIndex = (17);
            this.txtFactionDescription.TextChanged += (this.txtFactionDescription_TextChanged);
            // 
            // label22
            // 
            this.label22.AutoSize = (true);
            this.label22.Location = (new global::System.Drawing.Point(375, 12));
            this.label22.Name = ("label22");
            this.label22.Size = (new global::System.Drawing.Size(67, 15));
            this.label22.TabIndex = (16);
            this.label22.Text = ("Description");
            // 
            // fklblFactionNameLocale
            // 
            this.fklblFactionNameLocale.Enabled = (false);
            this.fklblFactionNameLocale.FlatAppearance.BorderSize = (0);
            this.fklblFactionNameLocale.FlatStyle = (global::System.Windows.Forms.FlatStyle.Flat);
            this.fklblFactionNameLocale.Image = ((global::System.Drawing.Image)(resources.GetObject("fklblFactionNameLocale.Image")));
            this.fklblFactionNameLocale.ImageAlign = (global::System.Drawing.ContentAlignment.TopLeft);
            this.fklblFactionNameLocale.Location = (new global::System.Drawing.Point(13, 59));
            this.fklblFactionNameLocale.Name = ("fklblFactionNameLocale");
            this.fklblFactionNameLocale.Size = (new global::System.Drawing.Size(331, 42));
            this.fklblFactionNameLocale.TabIndex = (15);
            this.fklblFactionNameLocale.Text = ("This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.");
            this.fklblFactionNameLocale.TextAlign = (global::System.Drawing.ContentAlignment.MiddleLeft);
            this.fklblFactionNameLocale.TextImageRelation = (global::System.Windows.Forms.TextImageRelation.ImageBeforeText);
            this.fklblFactionNameLocale.UseVisualStyleBackColor = (true);
            this.fklblFactionNameLocale.Visible = (false);
            // 
            // txtFactionName
            // 
            this.txtFactionName.Location = (new global::System.Drawing.Point(13, 30));
            this.txtFactionName.Name = ("txtFactionName");
            this.txtFactionName.Size = (new global::System.Drawing.Size(359, 23));
            this.txtFactionName.TabIndex = (14);
            this.txtFactionName.TextChanged += (this.txtFactionName_TextChanged);
            // 
            // label21
            // 
            this.label21.AutoSize = (true);
            this.label21.Location = (new global::System.Drawing.Point(13, 12));
            this.label21.Name = ("label21");
            this.label21.Size = (new global::System.Drawing.Size(39, 15));
            this.label21.TabIndex = (13);
            this.label21.Text = ("Name");
            // 
            // tpPlayerClass
            // 
            this.tpPlayerClass.AutoScroll = (true);
            this.tpPlayerClass.Controls.Add(this.btnChangePlayerConsoleCharacterBackColor);
            this.tpPlayerClass.Controls.Add(this.btnPlayerOnDeathAction);
            this.tpPlayerClass.Controls.Add(this.label63);
            this.tpPlayerClass.Controls.Add(this.btnPlayerOnAttackedAction);
            this.tpPlayerClass.Controls.Add(this.label61);
            this.tpPlayerClass.Controls.Add(this.btnPlayerOnTurnStartAction);
            this.tpPlayerClass.Controls.Add(this.label60);
            this.tpPlayerClass.Controls.Add(this.label58);
            this.tpPlayerClass.Controls.Add(this.label62);
            this.tpPlayerClass.Controls.Add(this.btnRemovePlayerOnAttackAction);
            this.tpPlayerClass.Controls.Add(this.btnEditPlayerOnAttackAction);
            this.tpPlayerClass.Controls.Add(this.btnAddPlayerOnAttackAction);
            this.tpPlayerClass.Controls.Add(this.lbPlayerOnAttackActions);
            this.tpPlayerClass.Controls.Add(this.label59);
            this.tpPlayerClass.Controls.Add(this.cmbPlayerStartingArmor);
            this.tpPlayerClass.Controls.Add(this.label57);
            this.tpPlayerClass.Controls.Add(this.cmbPlayerStartingWeapon);
            this.tpPlayerClass.Controls.Add(this.label56);
            this.tpPlayerClass.Controls.Add(this.lbPlayerStartingInventory);
            this.tpPlayerClass.Controls.Add(this.btnPlayerRemoveItem);
            this.tpPlayerClass.Controls.Add(this.btnPlayerAddItem);
            this.tpPlayerClass.Controls.Add(this.cmbPlayerInventoryItemChoices);
            this.tpPlayerClass.Controls.Add(this.label55);
            this.tpPlayerClass.Controls.Add(this.label54);
            this.tpPlayerClass.Controls.Add(this.nudPlayerInventorySize);
            this.tpPlayerClass.Controls.Add(this.label53);
            this.tpPlayerClass.Controls.Add(this.label52);
            this.tpPlayerClass.Controls.Add(this.label47);
            this.tpPlayerClass.Controls.Add(this.label51);
            this.tpPlayerClass.Controls.Add(this.chkPlayerCanGainExperience);
            this.tpPlayerClass.Controls.Add(this.nudPlayerMaxLevel);
            this.tpPlayerClass.Controls.Add(this.label50);
            this.tpPlayerClass.Controls.Add(this.txtPlayerLevelUpFormula);
            this.tpPlayerClass.Controls.Add(this.label49);
            this.tpPlayerClass.Controls.Add(this.label48);
            this.tpPlayerClass.Controls.Add(this.nudPlayerFlatSightRange);
            this.tpPlayerClass.Controls.Add(this.cmbPlayerSightRange);
            this.tpPlayerClass.Controls.Add(this.label43);
            this.tpPlayerClass.Controls.Add(this.label44);
            this.tpPlayerClass.Controls.Add(this.label45);
            this.tpPlayerClass.Controls.Add(this.label46);
            this.tpPlayerClass.Controls.Add(this.nudPlayerHPRegenerationPerLevelUp);
            this.tpPlayerClass.Controls.Add(this.nudPlayerBaseHPRegeneration);
            this.tpPlayerClass.Controls.Add(this.label42);
            this.tpPlayerClass.Controls.Add(this.label41);
            this.tpPlayerClass.Controls.Add(this.label40);
            this.tpPlayerClass.Controls.Add(this.label39);
            this.tpPlayerClass.Controls.Add(this.nudPlayerMovementPerLevelUp);
            this.tpPlayerClass.Controls.Add(this.nudPlayerBaseMovement);
            this.tpPlayerClass.Controls.Add(this.label37);
            this.tpPlayerClass.Controls.Add(this.nudPlayerDefensePerLevelUp);
            this.tpPlayerClass.Controls.Add(this.label38);
            this.tpPlayerClass.Controls.Add(this.nudPlayerBaseDefense);
            this.tpPlayerClass.Controls.Add(this.label36);
            this.tpPlayerClass.Controls.Add(this.label34);
            this.tpPlayerClass.Controls.Add(this.nudPlayerAttackPerLevelUp);
            this.tpPlayerClass.Controls.Add(this.label35);
            this.tpPlayerClass.Controls.Add(this.nudPlayerBaseAttack);
            this.tpPlayerClass.Controls.Add(this.label33);
            this.tpPlayerClass.Controls.Add(this.nudPlayerHPPerLevelUp);
            this.tpPlayerClass.Controls.Add(this.label32);
            this.tpPlayerClass.Controls.Add(this.nudPlayerBaseHP);
            this.tpPlayerClass.Controls.Add(this.label31);
            this.tpPlayerClass.Controls.Add(this.btnChangePlayerConsoleCharacterForeColor);
            this.tpPlayerClass.Controls.Add(this.btnChangePlayerConsoleCharacter);
            this.tpPlayerClass.Controls.Add(this.lblPlayerConsoleRepresentation);
            this.tpPlayerClass.Controls.Add(this.label30);
            this.tpPlayerClass.Controls.Add(this.chkPlayerStartsVisible);
            this.tpPlayerClass.Controls.Add(this.cmbPlayerFaction);
            this.tpPlayerClass.Controls.Add(this.label29);
            this.tpPlayerClass.Controls.Add(this.chkRequirePlayerPrompt);
            this.tpPlayerClass.Controls.Add(this.fklblPlayerClassDescriptionLocale);
            this.tpPlayerClass.Controls.Add(this.txtPlayerClassDescription);
            this.tpPlayerClass.Controls.Add(this.label28);
            this.tpPlayerClass.Controls.Add(this.fklblPlayerClassNameLocale);
            this.tpPlayerClass.Controls.Add(this.txtPlayerClassName);
            this.tpPlayerClass.Controls.Add(this.label27);
            this.tpPlayerClass.Controls.Add(this.lblPlayerSightRangeText);
            this.tpPlayerClass.Location = (new global::System.Drawing.Point(4, 24));
            this.tpPlayerClass.Name = ("tpPlayerClass");
            this.tpPlayerClass.Size = (new global::System.Drawing.Size(740, 356));
            this.tpPlayerClass.TabIndex = (4);
            this.tpPlayerClass.Text = ("Player Class");
            this.tpPlayerClass.UseVisualStyleBackColor = (true);
            // 
            // btnChangePlayerConsoleCharacterBackColor
            // 
            this.btnChangePlayerConsoleCharacterBackColor.Location = (new global::System.Drawing.Point(577, 66));
            this.btnChangePlayerConsoleCharacterBackColor.Name = ("btnChangePlayerConsoleCharacterBackColor");
            this.btnChangePlayerConsoleCharacterBackColor.Size = (new global::System.Drawing.Size(135, 23));
            this.btnChangePlayerConsoleCharacterBackColor.TabIndex = (111);
            this.btnChangePlayerConsoleCharacterBackColor.Text = ("Change Background...");
            this.btnChangePlayerConsoleCharacterBackColor.UseVisualStyleBackColor = (true);
            this.btnChangePlayerConsoleCharacterBackColor.Click += (this.btnChangePlayerConsoleCharacterBackColor_Click);
            // 
            // btnPlayerOnDeathAction
            // 
            this.btnPlayerOnDeathAction.Location = (new global::System.Drawing.Point(168, 679));
            this.btnPlayerOnDeathAction.Name = ("btnPlayerOnDeathAction");
            this.btnPlayerOnDeathAction.Size = (new global::System.Drawing.Size(75, 23));
            this.btnPlayerOnDeathAction.TabIndex = (110);
            this.btnPlayerOnDeathAction.Text = ("... do this!");
            this.btnPlayerOnDeathAction.UseVisualStyleBackColor = (true);
            this.btnPlayerOnDeathAction.Click += (this.btnPlayerOnDeathAction_Click);
            // 
            // label63
            // 
            this.label63.AutoSize = (true);
            this.label63.Font = (new global::System.Drawing.Font("Segoe UI", 9F, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point));
            this.label63.Location = (new global::System.Drawing.Point(13, 683));
            this.label63.Name = ("label63");
            this.label63.Size = (new global::System.Drawing.Size(92, 15));
            this.label63.TabIndex = (109);
            this.label63.Text = ("When they die...");
            // 
            // btnPlayerOnAttackedAction
            // 
            this.btnPlayerOnAttackedAction.Location = (new global::System.Drawing.Point(168, 648));
            this.btnPlayerOnAttackedAction.Name = ("btnPlayerOnAttackedAction");
            this.btnPlayerOnAttackedAction.Size = (new global::System.Drawing.Size(75, 23));
            this.btnPlayerOnAttackedAction.TabIndex = (108);
            this.btnPlayerOnAttackedAction.Text = ("... do this!");
            this.btnPlayerOnAttackedAction.UseVisualStyleBackColor = (true);
            this.btnPlayerOnAttackedAction.Click += (this.btnPlayerOnAttackedAction_Click);
            // 
            // label61
            // 
            this.label61.AutoSize = (true);
            this.label61.Font = (new global::System.Drawing.Font("Segoe UI", 9F, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point));
            this.label61.Location = (new global::System.Drawing.Point(13, 652));
            this.label61.Name = ("label61");
            this.label61.Size = (new global::System.Drawing.Size(149, 15));
            this.label61.TabIndex = (107);
            this.label61.Text = ("When they get interacted...");
            // 
            // btnPlayerOnTurnStartAction
            // 
            this.btnPlayerOnTurnStartAction.Location = (new global::System.Drawing.Point(168, 514));
            this.btnPlayerOnTurnStartAction.Name = ("btnPlayerOnTurnStartAction");
            this.btnPlayerOnTurnStartAction.Size = (new global::System.Drawing.Size(75, 23));
            this.btnPlayerOnTurnStartAction.TabIndex = (106);
            this.btnPlayerOnTurnStartAction.Text = ("... do this!");
            this.btnPlayerOnTurnStartAction.UseVisualStyleBackColor = (true);
            this.btnPlayerOnTurnStartAction.Click += (this.btnPlayerOnTurnStartAction_Click);
            // 
            // label60
            // 
            this.label60.AutoSize = (true);
            this.label60.Font = (new global::System.Drawing.Font("Segoe UI", 9F, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point));
            this.label60.Location = (new global::System.Drawing.Point(13, 518));
            this.label60.Name = ("label60");
            this.label60.Size = (new global::System.Drawing.Size(149, 15));
            this.label60.TabIndex = (105);
            this.label60.Text = ("When the next turn starts...");
            // 
            // label58
            // 
            this.label58.AutoSize = (true);
            this.label58.Font = (new global::System.Drawing.Font("Segoe UI", 12F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point));
            this.label58.Location = (new global::System.Drawing.Point(141, 483));
            this.label58.Name = ("label58");
            this.label58.Size = (new global::System.Drawing.Size(67, 21));
            this.label58.TabIndex = (104);
            this.label58.Text = ("Actions");
            // 
            // label62
            // 
            this.label62.AutoSize = (true);
            this.label62.Location = (new global::System.Drawing.Point(402, 289));
            this.label62.Name = ("label62");
            this.label62.Size = (new global::System.Drawing.Size(291, 30));
            this.label62.TabIndex = (102);
            this.label62.Text = ("NOTE: When not The Whole Map, Sight is significantly\r\nreduced in hallways");
            this.label62.TextAlign = (global::System.Drawing.ContentAlignment.MiddleLeft);
            // 
            // btnRemovePlayerOnAttackAction
            // 
            this.btnRemovePlayerOnAttackAction.Location = (new global::System.Drawing.Point(271, 607));
            this.btnRemovePlayerOnAttackAction.Name = ("btnRemovePlayerOnAttackAction");
            this.btnRemovePlayerOnAttackAction.Size = (new global::System.Drawing.Size(68, 23));
            this.btnRemovePlayerOnAttackAction.TabIndex = (91);
            this.btnRemovePlayerOnAttackAction.Text = ("Remove");
            this.btnRemovePlayerOnAttackAction.UseVisualStyleBackColor = (true);
            this.btnRemovePlayerOnAttackAction.Click += (this.btnRemovePlayerOnAttackAction_Click);
            // 
            // btnEditPlayerOnAttackAction
            // 
            this.btnEditPlayerOnAttackAction.Location = (new global::System.Drawing.Point(271, 578));
            this.btnEditPlayerOnAttackAction.Name = ("btnEditPlayerOnAttackAction");
            this.btnEditPlayerOnAttackAction.Size = (new global::System.Drawing.Size(68, 23));
            this.btnEditPlayerOnAttackAction.TabIndex = (90);
            this.btnEditPlayerOnAttackAction.Text = ("Edit");
            this.btnEditPlayerOnAttackAction.UseVisualStyleBackColor = (true);
            this.btnEditPlayerOnAttackAction.Click += (this.btnEditPlayerOnAttackAction_Click);
            // 
            // btnAddPlayerOnAttackAction
            // 
            this.btnAddPlayerOnAttackAction.Location = (new global::System.Drawing.Point(271, 549));
            this.btnAddPlayerOnAttackAction.Name = ("btnAddPlayerOnAttackAction");
            this.btnAddPlayerOnAttackAction.Size = (new global::System.Drawing.Size(68, 23));
            this.btnAddPlayerOnAttackAction.TabIndex = (89);
            this.btnAddPlayerOnAttackAction.Text = ("Add");
            this.btnAddPlayerOnAttackAction.UseVisualStyleBackColor = (true);
            this.btnAddPlayerOnAttackAction.Click += (this.btnAddPlayerOnAttackAction_Click);
            // 
            // lbPlayerOnAttackActions
            // 
            this.lbPlayerOnAttackActions.FormattingEnabled = (true);
            this.lbPlayerOnAttackActions.ItemHeight = (15);
            this.lbPlayerOnAttackActions.Location = (new global::System.Drawing.Point(168, 543));
            this.lbPlayerOnAttackActions.Name = ("lbPlayerOnAttackActions");
            this.lbPlayerOnAttackActions.Size = (new global::System.Drawing.Size(97, 94));
            this.lbPlayerOnAttackActions.TabIndex = (88);
            this.lbPlayerOnAttackActions.SelectedIndexChanged += (this.lbPlayerOnAttackActions_SelectedIndexChanged);
            // 
            // label59
            // 
            this.label59.AutoSize = (true);
            this.label59.Location = (new global::System.Drawing.Point(13, 568));
            this.label59.Name = ("label59");
            this.label59.Size = (new global::System.Drawing.Size(132, 30));
            this.label59.TabIndex = (87);
            this.label59.Text = ("Can do the following to\r\ninteract with someone:");
            // 
            // cmbPlayerStartingArmor
            // 
            this.cmbPlayerStartingArmor.DropDownStyle = (global::System.Windows.Forms.ComboBoxStyle.DropDownList);
            this.cmbPlayerStartingArmor.FormattingEnabled = (true);
            this.cmbPlayerStartingArmor.Location = (new global::System.Drawing.Point(146, 331));
            this.cmbPlayerStartingArmor.Name = ("cmbPlayerStartingArmor");
            this.cmbPlayerStartingArmor.Size = (new global::System.Drawing.Size(158, 23));
            this.cmbPlayerStartingArmor.TabIndex = (81);
            this.cmbPlayerStartingArmor.SelectedIndexChanged += (this.cmbPlayerStartingArmor_SelectedIndexChanged);
            // 
            // label57
            // 
            this.label57.AutoSize = (true);
            this.label57.Location = (new global::System.Drawing.Point(13, 334));
            this.label57.Name = ("label57");
            this.label57.Size = (new global::System.Drawing.Size(131, 15));
            this.label57.TabIndex = (80);
            this.label57.Text = ("When unarmored, wear");
            // 
            // cmbPlayerStartingWeapon
            // 
            this.cmbPlayerStartingWeapon.DropDownStyle = (global::System.Windows.Forms.ComboBoxStyle.DropDownList);
            this.cmbPlayerStartingWeapon.FormattingEnabled = (true);
            this.cmbPlayerStartingWeapon.Location = (new global::System.Drawing.Point(139, 300));
            this.cmbPlayerStartingWeapon.Name = ("cmbPlayerStartingWeapon");
            this.cmbPlayerStartingWeapon.Size = (new global::System.Drawing.Size(165, 23));
            this.cmbPlayerStartingWeapon.TabIndex = (79);
            this.cmbPlayerStartingWeapon.SelectedIndexChanged += (this.cmbPlayerStartingWeapon_SelectedIndexChanged);
            // 
            // label56
            // 
            this.label56.AutoSize = (true);
            this.label56.Location = (new global::System.Drawing.Point(13, 303));
            this.label56.Name = ("label56");
            this.label56.Size = (new global::System.Drawing.Size(123, 15));
            this.label56.TabIndex = (78);
            this.label56.Text = ("When unarmed, wield");
            // 
            // lbPlayerStartingInventory
            // 
            this.lbPlayerStartingInventory.FormattingEnabled = (true);
            this.lbPlayerStartingInventory.ItemHeight = (15);
            this.lbPlayerStartingInventory.Location = (new global::System.Drawing.Point(156, 392));
            this.lbPlayerStartingInventory.Name = ("lbPlayerStartingInventory");
            this.lbPlayerStartingInventory.Size = (new global::System.Drawing.Size(148, 79));
            this.lbPlayerStartingInventory.TabIndex = (77);
            this.lbPlayerStartingInventory.SelectedIndexChanged += (this.lbPlayerStartingInventory_SelectedIndexChanged);
            // 
            // btnPlayerRemoveItem
            // 
            this.btnPlayerRemoveItem.Enabled = (false);
            this.btnPlayerRemoveItem.Location = (new global::System.Drawing.Point(75, 444));
            this.btnPlayerRemoveItem.Name = ("btnPlayerRemoveItem");
            this.btnPlayerRemoveItem.Size = (new global::System.Drawing.Size(59, 23));
            this.btnPlayerRemoveItem.TabIndex = (76);
            this.btnPlayerRemoveItem.Text = ("Remove");
            this.btnPlayerRemoveItem.UseVisualStyleBackColor = (true);
            this.btnPlayerRemoveItem.Click += (this.btnPlayerRemoveItem_Click);
            // 
            // btnPlayerAddItem
            // 
            this.btnPlayerAddItem.Enabled = (false);
            this.btnPlayerAddItem.Location = (new global::System.Drawing.Point(13, 444));
            this.btnPlayerAddItem.Name = ("btnPlayerAddItem");
            this.btnPlayerAddItem.Size = (new global::System.Drawing.Size(56, 23));
            this.btnPlayerAddItem.TabIndex = (75);
            this.btnPlayerAddItem.Text = ("Add");
            this.btnPlayerAddItem.UseVisualStyleBackColor = (true);
            this.btnPlayerAddItem.Click += (this.btnPlayerAddItem_Click);
            // 
            // cmbPlayerInventoryItemChoices
            // 
            this.cmbPlayerInventoryItemChoices.DropDownStyle = (global::System.Windows.Forms.ComboBoxStyle.DropDownList);
            this.cmbPlayerInventoryItemChoices.FormattingEnabled = (true);
            this.cmbPlayerInventoryItemChoices.Location = (new global::System.Drawing.Point(13, 415));
            this.cmbPlayerInventoryItemChoices.Name = ("cmbPlayerInventoryItemChoices");
            this.cmbPlayerInventoryItemChoices.Size = (new global::System.Drawing.Size(121, 23));
            this.cmbPlayerInventoryItemChoices.TabIndex = (74);
            this.cmbPlayerInventoryItemChoices.SelectedIndexChanged += (this.cmbPlayerInventoryItemChoices_SelectedIndexChanged);
            // 
            // label55
            // 
            this.label55.AutoSize = (true);
            this.label55.Location = (new global::System.Drawing.Point(13, 392));
            this.label55.Name = ("label55");
            this.label55.Size = (new global::System.Drawing.Size(92, 15));
            this.label55.TabIndex = (73);
            this.label55.Text = ("Initial Inventory:");
            // 
            // label54
            // 
            this.label54.AutoSize = (true);
            this.label54.Location = (new global::System.Drawing.Point(172, 364));
            this.label54.Name = ("label54");
            this.label54.Size = (new global::System.Drawing.Size(36, 15));
            this.label54.TabIndex = (72);
            this.label54.Text = ("items");
            // 
            // nudPlayerInventorySize
            // 
            this.nudPlayerInventorySize.Location = (new global::System.Drawing.Point(121, 359));
            this.nudPlayerInventorySize.Maximum = (new global::System.Decimal(new global::System.Int32[] { 999, 0, 0, 0 }));
            this.nudPlayerInventorySize.Name = ("nudPlayerInventorySize");
            this.nudPlayerInventorySize.Size = (new global::System.Drawing.Size(45, 23));
            this.nudPlayerInventorySize.TabIndex = (71);
            this.nudPlayerInventorySize.ValueChanged += (this.nudPlayerInventorySize_ValueChanged);
            // 
            // label53
            // 
            this.label53.AutoSize = (true);
            this.label53.Location = (new global::System.Drawing.Point(13, 362));
            this.label53.Name = ("label53");
            this.label53.Size = (new global::System.Drawing.Size(109, 15));
            this.label53.TabIndex = (70);
            this.label53.Text = ("Inventory Capacity:");
            // 
            // label52
            // 
            this.label52.AutoSize = (true);
            this.label52.Location = (new global::System.Drawing.Point(407, 641));
            this.label52.Name = ("label52");
            this.label52.Size = (new global::System.Drawing.Size(54, 15));
            this.label52.TabIndex = (69);
            this.label52.Text = ("Recovers");
            // 
            // label47
            // 
            this.label47.AutoSize = (true);
            this.label47.Location = (new global::System.Drawing.Point(395, 483));
            this.label47.Name = ("label47");
            this.label47.Size = (new global::System.Drawing.Size(325, 30));
            this.label47.TabIndex = (68);
            this.label47.Text = ("NOTE: HP, Attack, Defense, Movement and HP Regeneration\r\nare internal names. Display Names depend on Locales.");
            // 
            // label51
            // 
            this.label51.AutoSize = (true);
            this.label51.Font = (new global::System.Drawing.Font("Segoe UI", 9F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point));
            this.label51.Location = (new global::System.Drawing.Point(408, 454));
            this.label51.Name = ("label51");
            this.label51.Size = (new global::System.Drawing.Size(115, 15));
            this.label51.TabIndex = (67);
            this.label51.Text = ("After Leveling Up...");
            // 
            // chkPlayerCanGainExperience
            // 
            this.chkPlayerCanGainExperience.AutoSize = (true);
            this.chkPlayerCanGainExperience.Location = (new global::System.Drawing.Point(407, 362));
            this.chkPlayerCanGainExperience.Name = ("chkPlayerCanGainExperience");
            this.chkPlayerCanGainExperience.Size = (new global::System.Drawing.Size(169, 19));
            this.chkPlayerCanGainExperience.TabIndex = (66);
            this.chkPlayerCanGainExperience.Text = ("Can gain Experience Points");
            this.chkPlayerCanGainExperience.UseVisualStyleBackColor = (true);
            this.chkPlayerCanGainExperience.CheckedChanged += (this.chkPlayerCanGainExperience_CheckedChanged);
            // 
            // nudPlayerMaxLevel
            // 
            this.nudPlayerMaxLevel.Location = (new global::System.Drawing.Point(513, 414));
            this.nudPlayerMaxLevel.Name = ("nudPlayerMaxLevel");
            this.nudPlayerMaxLevel.Size = (new global::System.Drawing.Size(44, 23));
            this.nudPlayerMaxLevel.TabIndex = (65);
            this.nudPlayerMaxLevel.Value = (new global::System.Decimal(new global::System.Int32[] { 1, 0, 0, 0 }));
            this.nudPlayerMaxLevel.ValueChanged += (this.nudPlayerMaxLevel_ValueChanged);
            // 
            // label50
            // 
            this.label50.AutoSize = (true);
            this.label50.Location = (new global::System.Drawing.Point(404, 417));
            this.label50.Name = ("label50");
            this.label50.Size = (new global::System.Drawing.Size(103, 15));
            this.label50.TabIndex = (64);
            this.label50.Text = ("Maximum Level is");
            // 
            // txtPlayerLevelUpFormula
            // 
            this.txtPlayerLevelUpFormula.Location = (new global::System.Drawing.Point(534, 386));
            this.txtPlayerLevelUpFormula.Name = ("txtPlayerLevelUpFormula");
            this.txtPlayerLevelUpFormula.Size = (new global::System.Drawing.Size(180, 23));
            this.txtPlayerLevelUpFormula.TabIndex = (63);
            this.txtPlayerLevelUpFormula.Enter += (this.txtPlayerLevelUpFormula_Enter);
            this.txtPlayerLevelUpFormula.Leave += (this.txtPlayerLevelUpFormula_Leave);
            // 
            // label49
            // 
            this.label49.AutoSize = (true);
            this.label49.Location = (new global::System.Drawing.Point(404, 389));
            this.label49.Name = ("label49");
            this.label49.Size = (new global::System.Drawing.Size(126, 15));
            this.label49.TabIndex = (62);
            this.label49.Text = ("Experience to Level Up");
            // 
            // label48
            // 
            this.label48.AutoSize = (true);
            this.label48.Font = (new global::System.Drawing.Font("Segoe UI", 12F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point));
            this.label48.Location = (new global::System.Drawing.Point(502, 332));
            this.label48.Name = ("label48");
            this.label48.Size = (new global::System.Drawing.Size(101, 21));
            this.label48.TabIndex = (61);
            this.label48.Text = ("Leveling Up");
            // 
            // nudPlayerFlatSightRange
            // 
            this.nudPlayerFlatSightRange.Location = (new global::System.Drawing.Point(588, 255));
            this.nudPlayerFlatSightRange.Maximum = (new global::System.Decimal(new global::System.Int32[] { 999, 0, 0, 0 }));
            this.nudPlayerFlatSightRange.Name = ("nudPlayerFlatSightRange");
            this.nudPlayerFlatSightRange.Size = (new global::System.Drawing.Size(54, 23));
            this.nudPlayerFlatSightRange.TabIndex = (59);
            this.nudPlayerFlatSightRange.Visible = (false);
            this.nudPlayerFlatSightRange.ValueChanged += (this.nudPlayerFlatSightRange_ValueChanged);
            // 
            // cmbPlayerSightRange
            // 
            this.cmbPlayerSightRange.DropDownStyle = (global::System.Windows.Forms.ComboBoxStyle.DropDownList);
            this.cmbPlayerSightRange.FormattingEnabled = (true);
            this.cmbPlayerSightRange.Location = (new global::System.Drawing.Point(452, 255));
            this.cmbPlayerSightRange.Name = ("cmbPlayerSightRange");
            this.cmbPlayerSightRange.Size = (new global::System.Drawing.Size(121, 23));
            this.cmbPlayerSightRange.TabIndex = (58);
            this.cmbPlayerSightRange.SelectedIndexChanged += (this.cmbPlayerSightRange_SelectedIndexChanged);
            // 
            // label43
            // 
            this.label43.AutoSize = (true);
            this.label43.Location = (new global::System.Drawing.Point(402, 258));
            this.label43.Name = ("label43");
            this.label43.Size = (new global::System.Drawing.Size(48, 15));
            this.label43.TabIndex = (57);
            this.label43.Text = ("Can see");
            // 
            // label44
            // 
            this.label44.AutoSize = (true);
            this.label44.Location = (new global::System.Drawing.Point(637, 202));
            this.label44.Name = ("label44");
            this.label44.Size = (new global::System.Drawing.Size(68, 15));
            this.label44.TabIndex = (56);
            this.label44.Text = ("HP per turn");
            // 
            // label45
            // 
            this.label45.AutoSize = (true);
            this.label45.Location = (new global::System.Drawing.Point(522, 200));
            this.label45.Name = ("label45");
            this.label45.Size = (new global::System.Drawing.Size(54, 15));
            this.label45.TabIndex = (55);
            this.label45.Text = ("Recovers");
            // 
            // label46
            // 
            this.label46.AutoSize = (true);
            this.label46.Location = (new global::System.Drawing.Point(521, 643));
            this.label46.Name = ("label46");
            this.label46.Size = (new global::System.Drawing.Size(167, 15));
            this.label46.TabIndex = (54);
            this.label46.Text = ("more HP per turn per Level Up");
            // 
            // nudPlayerHPRegenerationPerLevelUp
            // 
            this.nudPlayerHPRegenerationPerLevelUp.DecimalPlaces = (4);
            this.nudPlayerHPRegenerationPerLevelUp.Location = (new global::System.Drawing.Point(464, 639));
            this.nudPlayerHPRegenerationPerLevelUp.Maximum = (new global::System.Decimal(new global::System.Int32[] { 999, 0, 0, 0 }));
            this.nudPlayerHPRegenerationPerLevelUp.Name = ("nudPlayerHPRegenerationPerLevelUp");
            this.nudPlayerHPRegenerationPerLevelUp.Size = (new global::System.Drawing.Size(54, 23));
            this.nudPlayerHPRegenerationPerLevelUp.TabIndex = (53);
            this.nudPlayerHPRegenerationPerLevelUp.ValueChanged += (this.nudPlayerHPRegenerationPerLevelUp_ValueChanged);
            // 
            // nudPlayerBaseHPRegeneration
            // 
            this.nudPlayerBaseHPRegeneration.DecimalPlaces = (4);
            this.nudPlayerBaseHPRegeneration.Location = (new global::System.Drawing.Point(577, 198));
            this.nudPlayerBaseHPRegeneration.Maximum = (new global::System.Decimal(new global::System.Int32[] { 999, 0, 0, 0 }));
            this.nudPlayerBaseHPRegeneration.Name = ("nudPlayerBaseHPRegeneration");
            this.nudPlayerBaseHPRegeneration.Size = (new global::System.Drawing.Size(55, 23));
            this.nudPlayerBaseHPRegeneration.TabIndex = (52);
            this.nudPlayerBaseHPRegeneration.Value = (new global::System.Decimal(new global::System.Int32[] { 1, 0, 0, 0 }));
            this.nudPlayerBaseHPRegeneration.ValueChanged += (this.nudPlayerBaseHPRegeneration_ValueChanged);
            // 
            // label42
            // 
            this.label42.AutoSize = (true);
            this.label42.Location = (new global::System.Drawing.Point(407, 614));
            this.label42.Name = ("label42");
            this.label42.Size = (new global::System.Drawing.Size(61, 15));
            this.label42.TabIndex = (51);
            this.label42.Text = ("Can move");
            // 
            // label41
            // 
            this.label41.AutoSize = (true);
            this.label41.Location = (new global::System.Drawing.Point(643, 170));
            this.label41.Name = ("label41");
            this.label41.Size = (new global::System.Drawing.Size(73, 15));
            this.label41.TabIndex = (50);
            this.label41.Text = ("tiles per turn");
            // 
            // label40
            // 
            this.label40.AutoSize = (true);
            this.label40.Location = (new global::System.Drawing.Point(522, 168));
            this.label40.Name = ("label40");
            this.label40.Size = (new global::System.Drawing.Size(61, 15));
            this.label40.TabIndex = (49);
            this.label40.Text = ("Can move");
            // 
            // label39
            // 
            this.label39.AutoSize = (true);
            this.label39.Location = (new global::System.Drawing.Point(525, 615));
            this.label39.Name = ("label39");
            this.label39.Size = (new global::System.Drawing.Size(174, 15));
            this.label39.TabIndex = (48);
            this.label39.Text = ("more Tiles per turn per Level Up");
            // 
            // nudPlayerMovementPerLevelUp
            // 
            this.nudPlayerMovementPerLevelUp.DecimalPlaces = (4);
            this.nudPlayerMovementPerLevelUp.Location = (new global::System.Drawing.Point(468, 612));
            this.nudPlayerMovementPerLevelUp.Maximum = (new global::System.Decimal(new global::System.Int32[] { 999, 0, 0, 0 }));
            this.nudPlayerMovementPerLevelUp.Name = ("nudPlayerMovementPerLevelUp");
            this.nudPlayerMovementPerLevelUp.Size = (new global::System.Drawing.Size(54, 23));
            this.nudPlayerMovementPerLevelUp.TabIndex = (47);
            this.nudPlayerMovementPerLevelUp.ValueChanged += (this.nudPlayerMovementPerLevelUp_ValueChanged);
            // 
            // nudPlayerBaseMovement
            // 
            this.nudPlayerBaseMovement.Location = (new global::System.Drawing.Point(583, 166));
            this.nudPlayerBaseMovement.Maximum = (new global::System.Decimal(new global::System.Int32[] { 999, 0, 0, 0 }));
            this.nudPlayerBaseMovement.Minimum = (new global::System.Decimal(new global::System.Int32[] { 1, 0, 0, 0 }));
            this.nudPlayerBaseMovement.Name = ("nudPlayerBaseMovement");
            this.nudPlayerBaseMovement.Size = (new global::System.Drawing.Size(55, 23));
            this.nudPlayerBaseMovement.TabIndex = (45);
            this.nudPlayerBaseMovement.Value = (new global::System.Decimal(new global::System.Int32[] { 1, 0, 0, 0 }));
            this.nudPlayerBaseMovement.ValueChanged += (this.nudPlayerBaseMovement_ValueChanged);
            // 
            // label37
            // 
            this.label37.AutoSize = (true);
            this.label37.Location = (new global::System.Drawing.Point(463, 585));
            this.label37.Name = ("label37");
            this.label37.Size = (new global::System.Drawing.Size(148, 15));
            this.label37.TabIndex = (44);
            this.label37.Text = ("more Defense per Level Up");
            // 
            // nudPlayerDefensePerLevelUp
            // 
            this.nudPlayerDefensePerLevelUp.DecimalPlaces = (4);
            this.nudPlayerDefensePerLevelUp.Location = (new global::System.Drawing.Point(407, 582));
            this.nudPlayerDefensePerLevelUp.Maximum = (new global::System.Decimal(new global::System.Int32[] { 999, 0, 0, 0 }));
            this.nudPlayerDefensePerLevelUp.Name = ("nudPlayerDefensePerLevelUp");
            this.nudPlayerDefensePerLevelUp.Size = (new global::System.Drawing.Size(54, 23));
            this.nudPlayerDefensePerLevelUp.TabIndex = (43);
            this.nudPlayerDefensePerLevelUp.ValueChanged += (this.nudPlayerDefensePerLevelUp_ValueChanged);
            // 
            // label38
            // 
            this.label38.AutoSize = (true);
            this.label38.Location = (new global::System.Drawing.Point(459, 229));
            this.label38.Name = ("label38");
            this.label38.Size = (new global::System.Drawing.Size(49, 15));
            this.label38.TabIndex = (42);
            this.label38.Text = ("Defense");
            // 
            // nudPlayerBaseDefense
            // 
            this.nudPlayerBaseDefense.Location = (new global::System.Drawing.Point(402, 225));
            this.nudPlayerBaseDefense.Maximum = (new global::System.Decimal(new global::System.Int32[] { 999, 0, 0, 0 }));
            this.nudPlayerBaseDefense.Name = ("nudPlayerBaseDefense");
            this.nudPlayerBaseDefense.Size = (new global::System.Drawing.Size(55, 23));
            this.nudPlayerBaseDefense.TabIndex = (41);
            this.nudPlayerBaseDefense.ValueChanged += (this.nudPlayerBaseDefense_ValueChanged);
            // 
            // label36
            // 
            this.label36.AutoSize = (true);
            this.label36.Location = (new global::System.Drawing.Point(394, 124));
            this.label36.Name = ("label36");
            this.label36.Size = (new global::System.Drawing.Size(325, 30));
            this.label36.TabIndex = (40);
            this.label36.Text = ("NOTE: HP, Attack, Defense, Movement and HP Regeneration\r\nare internal names. Display Names depend on Locales.");
            // 
            // label34
            // 
            this.label34.AutoSize = (true);
            this.label34.Location = (new global::System.Drawing.Point(463, 558));
            this.label34.Name = ("label34");
            this.label34.Size = (new global::System.Drawing.Size(140, 15));
            this.label34.TabIndex = (39);
            this.label34.Text = ("more Attack per Level Up");
            // 
            // nudPlayerAttackPerLevelUp
            // 
            this.nudPlayerAttackPerLevelUp.DecimalPlaces = (4);
            this.nudPlayerAttackPerLevelUp.Location = (new global::System.Drawing.Point(407, 555));
            this.nudPlayerAttackPerLevelUp.Maximum = (new global::System.Decimal(new global::System.Int32[] { 999, 0, 0, 0 }));
            this.nudPlayerAttackPerLevelUp.Name = ("nudPlayerAttackPerLevelUp");
            this.nudPlayerAttackPerLevelUp.Size = (new global::System.Drawing.Size(54, 23));
            this.nudPlayerAttackPerLevelUp.TabIndex = (38);
            this.nudPlayerAttackPerLevelUp.ValueChanged += (this.nudPlayerAttackPerLevelUp_ValueChanged);
            // 
            // label35
            // 
            this.label35.AutoSize = (true);
            this.label35.Location = (new global::System.Drawing.Point(459, 200));
            this.label35.Name = ("label35");
            this.label35.Size = (new global::System.Drawing.Size(41, 15));
            this.label35.TabIndex = (37);
            this.label35.Text = ("Attack");
            // 
            // nudPlayerBaseAttack
            // 
            this.nudPlayerBaseAttack.Location = (new global::System.Drawing.Point(402, 196));
            this.nudPlayerBaseAttack.Maximum = (new global::System.Decimal(new global::System.Int32[] { 999, 0, 0, 0 }));
            this.nudPlayerBaseAttack.Name = ("nudPlayerBaseAttack");
            this.nudPlayerBaseAttack.Size = (new global::System.Drawing.Size(55, 23));
            this.nudPlayerBaseAttack.TabIndex = (36);
            this.nudPlayerBaseAttack.ValueChanged += (this.nudPlayerBaseAttack_ValueChanged);
            // 
            // label33
            // 
            this.label33.AutoSize = (true);
            this.label33.Location = (new global::System.Drawing.Point(463, 531));
            this.label33.Name = ("label33");
            this.label33.Size = (new global::System.Drawing.Size(122, 15));
            this.label33.TabIndex = (35);
            this.label33.Text = ("more HP per Level Up");
            // 
            // nudPlayerHPPerLevelUp
            // 
            this.nudPlayerHPPerLevelUp.DecimalPlaces = (4);
            this.nudPlayerHPPerLevelUp.Location = (new global::System.Drawing.Point(407, 528));
            this.nudPlayerHPPerLevelUp.Maximum = (new global::System.Decimal(new global::System.Int32[] { 999, 0, 0, 0 }));
            this.nudPlayerHPPerLevelUp.Name = ("nudPlayerHPPerLevelUp");
            this.nudPlayerHPPerLevelUp.Size = (new global::System.Drawing.Size(54, 23));
            this.nudPlayerHPPerLevelUp.TabIndex = (34);
            this.nudPlayerHPPerLevelUp.ValueChanged += (this.nudPlayerHPPerLevelUp_ValueChanged);
            // 
            // label32
            // 
            this.label32.AutoSize = (true);
            this.label32.Location = (new global::System.Drawing.Point(459, 170));
            this.label32.Name = ("label32");
            this.label32.Size = (new global::System.Drawing.Size(23, 15));
            this.label32.TabIndex = (33);
            this.label32.Text = ("HP");
            // 
            // nudPlayerBaseHP
            // 
            this.nudPlayerBaseHP.Location = (new global::System.Drawing.Point(402, 166));
            this.nudPlayerBaseHP.Maximum = (new global::System.Decimal(new global::System.Int32[] { 999, 0, 0, 0 }));
            this.nudPlayerBaseHP.Minimum = (new global::System.Decimal(new global::System.Int32[] { 1, 0, 0, 0 }));
            this.nudPlayerBaseHP.Name = ("nudPlayerBaseHP");
            this.nudPlayerBaseHP.Size = (new global::System.Drawing.Size(55, 23));
            this.nudPlayerBaseHP.TabIndex = (32);
            this.nudPlayerBaseHP.Value = (new global::System.Decimal(new global::System.Int32[] { 1, 0, 0, 0 }));
            this.nudPlayerBaseHP.ValueChanged += (this.nudPlayerBaseHP_ValueChanged);
            // 
            // label31
            // 
            this.label31.AutoSize = (true);
            this.label31.Font = (new global::System.Drawing.Font("Segoe UI", 12F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point));
            this.label31.Location = (new global::System.Drawing.Point(477, 100));
            this.label31.Name = ("label31");
            this.label31.Size = (new global::System.Drawing.Size(155, 21));
            this.label31.TabIndex = (31);
            this.label31.Text = ("Base Stats (Level 1)");
            // 
            // btnChangePlayerConsoleCharacterForeColor
            // 
            this.btnChangePlayerConsoleCharacterForeColor.Location = (new global::System.Drawing.Point(577, 37));
            this.btnChangePlayerConsoleCharacterForeColor.Name = ("btnChangePlayerConsoleCharacterForeColor");
            this.btnChangePlayerConsoleCharacterForeColor.Size = (new global::System.Drawing.Size(135, 23));
            this.btnChangePlayerConsoleCharacterForeColor.TabIndex = (29);
            this.btnChangePlayerConsoleCharacterForeColor.Text = ("Change Foreground...");
            this.btnChangePlayerConsoleCharacterForeColor.UseVisualStyleBackColor = (true);
            this.btnChangePlayerConsoleCharacterForeColor.Click += (this.btnChangePlayerConsoleCharacterForeColor_Click);
            // 
            // btnChangePlayerConsoleCharacter
            // 
            this.btnChangePlayerConsoleCharacter.Location = (new global::System.Drawing.Point(577, 8));
            this.btnChangePlayerConsoleCharacter.Name = ("btnChangePlayerConsoleCharacter");
            this.btnChangePlayerConsoleCharacter.Size = (new global::System.Drawing.Size(135, 23));
            this.btnChangePlayerConsoleCharacter.TabIndex = (28);
            this.btnChangePlayerConsoleCharacter.Text = ("Change Character...");
            this.btnChangePlayerConsoleCharacter.UseVisualStyleBackColor = (true);
            this.btnChangePlayerConsoleCharacter.Click += (this.btnChangePlayerConsoleCharacter_Click);
            // 
            // lblPlayerConsoleRepresentation
            // 
            this.lblPlayerConsoleRepresentation.Font = (new global::System.Drawing.Font("Courier New", 36F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point));
            this.lblPlayerConsoleRepresentation.Location = (new global::System.Drawing.Point(504, 17));
            this.lblPlayerConsoleRepresentation.Name = ("lblPlayerConsoleRepresentation");
            this.lblPlayerConsoleRepresentation.Size = (new global::System.Drawing.Size(64, 64));
            this.lblPlayerConsoleRepresentation.TabIndex = (27);
            this.lblPlayerConsoleRepresentation.TextAlign = (global::System.Drawing.ContentAlignment.MiddleCenter);
            // 
            // label30
            // 
            this.label30.Font = (new global::System.Drawing.Font("Segoe UI", 14.25F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point));
            this.label30.Location = (new global::System.Drawing.Point(370, 19));
            this.label30.Name = ("label30");
            this.label30.Size = (new global::System.Drawing.Size(131, 52));
            this.label30.TabIndex = (26);
            this.label30.Text = ("Appearance -");
            this.label30.TextAlign = (global::System.Drawing.ContentAlignment.MiddleCenter);
            // 
            // chkPlayerStartsVisible
            // 
            this.chkPlayerStartsVisible.AutoSize = (true);
            this.chkPlayerStartsVisible.Location = (new global::System.Drawing.Point(13, 272));
            this.chkPlayerStartsVisible.Name = ("chkPlayerStartsVisible");
            this.chkPlayerStartsVisible.Size = (new global::System.Drawing.Size(102, 19));
            this.chkPlayerStartsVisible.TabIndex = (25);
            this.chkPlayerStartsVisible.Text = ("Spawns visible");
            this.chkPlayerStartsVisible.UseVisualStyleBackColor = (true);
            this.chkPlayerStartsVisible.CheckedChanged += (this.chkPlayerStartsVisible_CheckedChanged);
            // 
            // cmbPlayerFaction
            // 
            this.cmbPlayerFaction.DropDownStyle = (global::System.Windows.Forms.ComboBoxStyle.DropDownList);
            this.cmbPlayerFaction.FormattingEnabled = (true);
            this.cmbPlayerFaction.Location = (new global::System.Drawing.Point(65, 241));
            this.cmbPlayerFaction.Name = ("cmbPlayerFaction");
            this.cmbPlayerFaction.Size = (new global::System.Drawing.Size(146, 23));
            this.cmbPlayerFaction.TabIndex = (24);
            this.cmbPlayerFaction.SelectedIndexChanged += (this.cmbPlayerFaction_SelectedIndexChanged);
            // 
            // label29
            // 
            this.label29.AutoSize = (true);
            this.label29.Location = (new global::System.Drawing.Point(13, 244));
            this.label29.Name = ("label29");
            this.label29.Size = (new global::System.Drawing.Size(46, 15));
            this.label29.TabIndex = (23);
            this.label29.Text = ("Faction");
            // 
            // chkRequirePlayerPrompt
            // 
            this.chkRequirePlayerPrompt.AutoSize = (true);
            this.chkRequirePlayerPrompt.Location = (new global::System.Drawing.Point(13, 107));
            this.chkRequirePlayerPrompt.Name = ("chkRequirePlayerPrompt");
            this.chkRequirePlayerPrompt.Size = (new global::System.Drawing.Size(287, 19));
            this.chkRequirePlayerPrompt.TabIndex = (22);
            this.chkRequirePlayerPrompt.Text = ("Player will have to provide a name upon selection");
            this.chkRequirePlayerPrompt.UseVisualStyleBackColor = (true);
            this.chkRequirePlayerPrompt.CheckedChanged += (this.chkRequirePlayerPrompt_CheckedChanged);
            // 
            // fklblPlayerClassDescriptionLocale
            // 
            this.fklblPlayerClassDescriptionLocale.Enabled = (false);
            this.fklblPlayerClassDescriptionLocale.FlatAppearance.BorderSize = (0);
            this.fklblPlayerClassDescriptionLocale.FlatStyle = (global::System.Windows.Forms.FlatStyle.Flat);
            this.fklblPlayerClassDescriptionLocale.Image = ((global::System.Drawing.Image)(resources.GetObject("fklblPlayerClassDescriptionLocale.Image")));
            this.fklblPlayerClassDescriptionLocale.ImageAlign = (global::System.Drawing.ContentAlignment.TopLeft);
            this.fklblPlayerClassDescriptionLocale.Location = (new global::System.Drawing.Point(13, 185));
            this.fklblPlayerClassDescriptionLocale.Name = ("fklblPlayerClassDescriptionLocale");
            this.fklblPlayerClassDescriptionLocale.Size = (new global::System.Drawing.Size(331, 42));
            this.fklblPlayerClassDescriptionLocale.TabIndex = (21);
            this.fklblPlayerClassDescriptionLocale.Text = ("This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.");
            this.fklblPlayerClassDescriptionLocale.TextAlign = (global::System.Drawing.ContentAlignment.MiddleLeft);
            this.fklblPlayerClassDescriptionLocale.TextImageRelation = (global::System.Windows.Forms.TextImageRelation.ImageBeforeText);
            this.fklblPlayerClassDescriptionLocale.UseVisualStyleBackColor = (true);
            this.fklblPlayerClassDescriptionLocale.Visible = (false);
            // 
            // txtPlayerClassDescription
            // 
            this.txtPlayerClassDescription.Location = (new global::System.Drawing.Point(13, 156));
            this.txtPlayerClassDescription.Name = ("txtPlayerClassDescription");
            this.txtPlayerClassDescription.Size = (new global::System.Drawing.Size(350, 23));
            this.txtPlayerClassDescription.TabIndex = (20);
            this.txtPlayerClassDescription.TextChanged += (this.txtPlayerClassDescription_TextChanged);
            // 
            // label28
            // 
            this.label28.AutoSize = (true);
            this.label28.Location = (new global::System.Drawing.Point(13, 138));
            this.label28.Name = ("label28");
            this.label28.Size = (new global::System.Drawing.Size(67, 15));
            this.label28.TabIndex = (19);
            this.label28.Text = ("Description");
            // 
            // fklblPlayerClassNameLocale
            // 
            this.fklblPlayerClassNameLocale.Enabled = (false);
            this.fklblPlayerClassNameLocale.FlatAppearance.BorderSize = (0);
            this.fklblPlayerClassNameLocale.FlatStyle = (global::System.Windows.Forms.FlatStyle.Flat);
            this.fklblPlayerClassNameLocale.Image = ((global::System.Drawing.Image)(resources.GetObject("fklblPlayerClassNameLocale.Image")));
            this.fklblPlayerClassNameLocale.ImageAlign = (global::System.Drawing.ContentAlignment.TopLeft);
            this.fklblPlayerClassNameLocale.Location = (new global::System.Drawing.Point(13, 55));
            this.fklblPlayerClassNameLocale.Name = ("fklblPlayerClassNameLocale");
            this.fklblPlayerClassNameLocale.Size = (new global::System.Drawing.Size(331, 42));
            this.fklblPlayerClassNameLocale.TabIndex = (18);
            this.fklblPlayerClassNameLocale.Text = ("This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.");
            this.fklblPlayerClassNameLocale.TextAlign = (global::System.Drawing.ContentAlignment.MiddleLeft);
            this.fklblPlayerClassNameLocale.TextImageRelation = (global::System.Windows.Forms.TextImageRelation.ImageBeforeText);
            this.fklblPlayerClassNameLocale.UseVisualStyleBackColor = (true);
            this.fklblPlayerClassNameLocale.Visible = (false);
            // 
            // txtPlayerClassName
            // 
            this.txtPlayerClassName.Location = (new global::System.Drawing.Point(13, 26));
            this.txtPlayerClassName.Name = ("txtPlayerClassName");
            this.txtPlayerClassName.Size = (new global::System.Drawing.Size(350, 23));
            this.txtPlayerClassName.TabIndex = (17);
            this.txtPlayerClassName.TextChanged += (this.txtPlayerClassName_TextChanged);
            // 
            // label27
            // 
            this.label27.AutoSize = (true);
            this.label27.Location = (new global::System.Drawing.Point(13, 8));
            this.label27.Name = ("label27");
            this.label27.Size = (new global::System.Drawing.Size(80, 15));
            this.label27.TabIndex = (16);
            this.label27.Text = ("Default Name");
            // 
            // lblPlayerSightRangeText
            // 
            this.lblPlayerSightRangeText.AutoSize = (true);
            this.lblPlayerSightRangeText.Location = (new global::System.Drawing.Point(648, 258));
            this.lblPlayerSightRangeText.Name = ("lblPlayerSightRangeText");
            this.lblPlayerSightRangeText.Size = (new global::System.Drawing.Size(28, 15));
            this.lblPlayerSightRangeText.TabIndex = (60);
            this.lblPlayerSightRangeText.Text = ("tiles");
            this.lblPlayerSightRangeText.Visible = (false);
            // 
            // tpNPC
            // 
            this.tpNPC.AutoScroll = (true);
            this.tpNPC.Controls.Add(this.nudNPCOddsToTargetSelf);
            this.tpNPC.Controls.Add(this.label104);
            this.tpNPC.Controls.Add(this.txtNPCExperiencePayout);
            this.tpNPC.Controls.Add(this.label103);
            this.tpNPC.Controls.Add(this.chkNPCKnowsAllCharacterPositions);
            this.tpNPC.Controls.Add(this.btnChangeNPCConsoleCharacterBackColor);
            this.tpNPC.Controls.Add(this.btnNPCOnDeathAction);
            this.tpNPC.Controls.Add(this.label64);
            this.tpNPC.Controls.Add(this.btnNPCOnAttackedAction);
            this.tpNPC.Controls.Add(this.label65);
            this.tpNPC.Controls.Add(this.btnNPCOnTurnStartAction);
            this.tpNPC.Controls.Add(this.label66);
            this.tpNPC.Controls.Add(this.label67);
            this.tpNPC.Controls.Add(this.label68);
            this.tpNPC.Controls.Add(this.btnRemoveNPCOnAttackAction);
            this.tpNPC.Controls.Add(this.btnEditNPCOnAttackAction);
            this.tpNPC.Controls.Add(this.btnAddNPCOnAttackAction);
            this.tpNPC.Controls.Add(this.lbNPCOnAttackActions);
            this.tpNPC.Controls.Add(this.label69);
            this.tpNPC.Controls.Add(this.cmbNPCStartingArmor);
            this.tpNPC.Controls.Add(this.label70);
            this.tpNPC.Controls.Add(this.cmbNPCStartingWeapon);
            this.tpNPC.Controls.Add(this.label71);
            this.tpNPC.Controls.Add(this.lbNPCStartingInventory);
            this.tpNPC.Controls.Add(this.btnNPCRemoveItem);
            this.tpNPC.Controls.Add(this.btnNPCAddItem);
            this.tpNPC.Controls.Add(this.cmbNPCInventoryItemChoices);
            this.tpNPC.Controls.Add(this.label72);
            this.tpNPC.Controls.Add(this.label73);
            this.tpNPC.Controls.Add(this.nudNPCInventorySize);
            this.tpNPC.Controls.Add(this.label74);
            this.tpNPC.Controls.Add(this.label75);
            this.tpNPC.Controls.Add(this.label76);
            this.tpNPC.Controls.Add(this.label77);
            this.tpNPC.Controls.Add(this.chkNPCCanGainExperience);
            this.tpNPC.Controls.Add(this.nudNPCMaxLevel);
            this.tpNPC.Controls.Add(this.label78);
            this.tpNPC.Controls.Add(this.txtNPCLevelUpFormula);
            this.tpNPC.Controls.Add(this.label79);
            this.tpNPC.Controls.Add(this.label80);
            this.tpNPC.Controls.Add(this.nudNPCFlatSightRange);
            this.tpNPC.Controls.Add(this.cmbNPCSightRange);
            this.tpNPC.Controls.Add(this.label81);
            this.tpNPC.Controls.Add(this.label82);
            this.tpNPC.Controls.Add(this.label83);
            this.tpNPC.Controls.Add(this.label84);
            this.tpNPC.Controls.Add(this.nudNPCHPRegenerationPerLevelUp);
            this.tpNPC.Controls.Add(this.nudNPCBaseHPRegeneration);
            this.tpNPC.Controls.Add(this.label85);
            this.tpNPC.Controls.Add(this.label86);
            this.tpNPC.Controls.Add(this.label87);
            this.tpNPC.Controls.Add(this.label88);
            this.tpNPC.Controls.Add(this.nudNPCMovementPerLevelUp);
            this.tpNPC.Controls.Add(this.nudNPCBaseMovement);
            this.tpNPC.Controls.Add(this.label89);
            this.tpNPC.Controls.Add(this.nudNPCDefensePerLevelUp);
            this.tpNPC.Controls.Add(this.label90);
            this.tpNPC.Controls.Add(this.nudNPCBaseDefense);
            this.tpNPC.Controls.Add(this.label91);
            this.tpNPC.Controls.Add(this.label92);
            this.tpNPC.Controls.Add(this.nudNPCAttackPerLevelUp);
            this.tpNPC.Controls.Add(this.label93);
            this.tpNPC.Controls.Add(this.nudNPCBaseAttack);
            this.tpNPC.Controls.Add(this.label94);
            this.tpNPC.Controls.Add(this.nudNPCHPPerLevelUp);
            this.tpNPC.Controls.Add(this.label95);
            this.tpNPC.Controls.Add(this.nudNPCBaseHP);
            this.tpNPC.Controls.Add(this.label96);
            this.tpNPC.Controls.Add(this.btnChangeNPCConsoleCharacterForeColor);
            this.tpNPC.Controls.Add(this.btnChangeNPCConsoleCharacter);
            this.tpNPC.Controls.Add(this.lblNPCConsoleRepresentation);
            this.tpNPC.Controls.Add(this.label98);
            this.tpNPC.Controls.Add(this.chkNPCStartsVisible);
            this.tpNPC.Controls.Add(this.cmbNPCFaction);
            this.tpNPC.Controls.Add(this.label99);
            this.tpNPC.Controls.Add(this.fklblNPCDescriptionLocale);
            this.tpNPC.Controls.Add(this.txtNPCDescription);
            this.tpNPC.Controls.Add(this.label100);
            this.tpNPC.Controls.Add(this.fklblNPCNameLocale);
            this.tpNPC.Controls.Add(this.txtNPCName);
            this.tpNPC.Controls.Add(this.label101);
            this.tpNPC.Controls.Add(this.lblNPCSightRangeText);
            this.tpNPC.Location = (new global::System.Drawing.Point(4, 24));
            this.tpNPC.Name = ("tpNPC");
            this.tpNPC.Size = (new global::System.Drawing.Size(740, 356));
            this.tpNPC.TabIndex = (5);
            this.tpNPC.Text = ("NPC");
            this.tpNPC.UseVisualStyleBackColor = (true);
            // 
            // nudNPCOddsToTargetSelf
            // 
            this.nudNPCOddsToTargetSelf.Location = (new global::System.Drawing.Point(287, 744));
            this.nudNPCOddsToTargetSelf.Name = ("nudNPCOddsToTargetSelf");
            this.nudNPCOddsToTargetSelf.Size = (new global::System.Drawing.Size(41, 23));
            this.nudNPCOddsToTargetSelf.TabIndex = (195);
            this.nudNPCOddsToTargetSelf.ValueChanged += (this.nudNPCOddsToTargetSelf_ValueChanged);
            // 
            // label104
            // 
            this.label104.AutoSize = (true);
            this.label104.Location = (new global::System.Drawing.Point(13, 746));
            this.label104.Name = ("label104");
            this.label104.Size = (new global::System.Drawing.Size(334, 15));
            this.label104.TabIndex = (194);
            this.label104.Text = ("Odds for NPC to target themselves with an Action:                 %");
            // 
            // txtNPCExperiencePayout
            // 
            this.txtNPCExperiencePayout.Location = (new global::System.Drawing.Point(121, 297));
            this.txtNPCExperiencePayout.Name = ("txtNPCExperiencePayout");
            this.txtNPCExperiencePayout.Size = (new global::System.Drawing.Size(242, 23));
            this.txtNPCExperiencePayout.TabIndex = (192);
            this.txtNPCExperiencePayout.Enter += (this.txtNPCExperiencePayout_Enter);
            this.txtNPCExperiencePayout.Leave += (this.txtNPCExperiencePayout_Leave);
            // 
            // label103
            // 
            this.label103.AutoSize = (true);
            this.label103.Location = (new global::System.Drawing.Point(13, 300));
            this.label103.Name = ("label103");
            this.label103.Size = (new global::System.Drawing.Size(104, 15));
            this.label103.TabIndex = (191);
            this.label103.Text = ("Experience Payout");
            // 
            // chkNPCKnowsAllCharacterPositions
            // 
            this.chkNPCKnowsAllCharacterPositions.AutoSize = (true);
            this.chkNPCKnowsAllCharacterPositions.Location = (new global::System.Drawing.Point(13, 268));
            this.chkNPCKnowsAllCharacterPositions.Name = ("chkNPCKnowsAllCharacterPositions");
            this.chkNPCKnowsAllCharacterPositions.Size = (new global::System.Drawing.Size(361, 19));
            this.chkNPCKnowsAllCharacterPositions.TabIndex = (190);
            this.chkNPCKnowsAllCharacterPositions.Text = ("Knows the position of all living characters (even when not seen)");
            this.chkNPCKnowsAllCharacterPositions.UseVisualStyleBackColor = (true);
            this.chkNPCKnowsAllCharacterPositions.CheckedChanged += (this.chkNPCKnowsAllCharacterPositions_CheckedChanged);
            // 
            // btnChangeNPCConsoleCharacterBackColor
            // 
            this.btnChangeNPCConsoleCharacterBackColor.Location = (new global::System.Drawing.Point(577, 66));
            this.btnChangeNPCConsoleCharacterBackColor.Name = ("btnChangeNPCConsoleCharacterBackColor");
            this.btnChangeNPCConsoleCharacterBackColor.Size = (new global::System.Drawing.Size(135, 23));
            this.btnChangeNPCConsoleCharacterBackColor.TabIndex = (189);
            this.btnChangeNPCConsoleCharacterBackColor.Text = ("Change Background...");
            this.btnChangeNPCConsoleCharacterBackColor.UseVisualStyleBackColor = (true);
            this.btnChangeNPCConsoleCharacterBackColor.Click += (this.btnChangeNPCConsoleCharacterBackColor_Click);
            // 
            // btnNPCOnDeathAction
            // 
            this.btnNPCOnDeathAction.Location = (new global::System.Drawing.Point(168, 710));
            this.btnNPCOnDeathAction.Name = ("btnNPCOnDeathAction");
            this.btnNPCOnDeathAction.Size = (new global::System.Drawing.Size(75, 23));
            this.btnNPCOnDeathAction.TabIndex = (188);
            this.btnNPCOnDeathAction.Text = ("... do this!");
            this.btnNPCOnDeathAction.UseVisualStyleBackColor = (true);
            this.btnNPCOnDeathAction.Click += (this.btnNPCOnDeathAction_Click);
            // 
            // label64
            // 
            this.label64.AutoSize = (true);
            this.label64.Font = (new global::System.Drawing.Font("Segoe UI", 9F, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point));
            this.label64.Location = (new global::System.Drawing.Point(13, 714));
            this.label64.Name = ("label64");
            this.label64.Size = (new global::System.Drawing.Size(92, 15));
            this.label64.TabIndex = (187);
            this.label64.Text = ("When they die...");
            // 
            // btnNPCOnAttackedAction
            // 
            this.btnNPCOnAttackedAction.Location = (new global::System.Drawing.Point(168, 679));
            this.btnNPCOnAttackedAction.Name = ("btnNPCOnAttackedAction");
            this.btnNPCOnAttackedAction.Size = (new global::System.Drawing.Size(75, 23));
            this.btnNPCOnAttackedAction.TabIndex = (186);
            this.btnNPCOnAttackedAction.Text = ("... do this!");
            this.btnNPCOnAttackedAction.UseVisualStyleBackColor = (true);
            this.btnNPCOnAttackedAction.Click += (this.btnNPCOnAttackedAction_Click);
            // 
            // label65
            // 
            this.label65.AutoSize = (true);
            this.label65.Font = (new global::System.Drawing.Font("Segoe UI", 9F, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point));
            this.label65.Location = (new global::System.Drawing.Point(13, 683));
            this.label65.Name = ("label65");
            this.label65.Size = (new global::System.Drawing.Size(149, 15));
            this.label65.TabIndex = (185);
            this.label65.Text = ("When they get interacted...");
            // 
            // btnNPCOnTurnStartAction
            // 
            this.btnNPCOnTurnStartAction.Location = (new global::System.Drawing.Point(168, 545));
            this.btnNPCOnTurnStartAction.Name = ("btnNPCOnTurnStartAction");
            this.btnNPCOnTurnStartAction.Size = (new global::System.Drawing.Size(75, 23));
            this.btnNPCOnTurnStartAction.TabIndex = (184);
            this.btnNPCOnTurnStartAction.Text = ("... do this!");
            this.btnNPCOnTurnStartAction.UseVisualStyleBackColor = (true);
            this.btnNPCOnTurnStartAction.Click += (this.btnNPCOnTurnStartAction_Click);
            // 
            // label66
            // 
            this.label66.AutoSize = (true);
            this.label66.Font = (new global::System.Drawing.Font("Segoe UI", 9F, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point));
            this.label66.Location = (new global::System.Drawing.Point(13, 549));
            this.label66.Name = ("label66");
            this.label66.Size = (new global::System.Drawing.Size(149, 15));
            this.label66.TabIndex = (183);
            this.label66.Text = ("When the next turn starts...");
            // 
            // label67
            // 
            this.label67.AutoSize = (true);
            this.label67.Font = (new global::System.Drawing.Font("Segoe UI", 12F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point));
            this.label67.Location = (new global::System.Drawing.Point(141, 514));
            this.label67.Name = ("label67");
            this.label67.Size = (new global::System.Drawing.Size(67, 21));
            this.label67.TabIndex = (182);
            this.label67.Text = ("Actions");
            // 
            // label68
            // 
            this.label68.AutoSize = (true);
            this.label68.Location = (new global::System.Drawing.Point(402, 289));
            this.label68.Name = ("label68");
            this.label68.Size = (new global::System.Drawing.Size(291, 30));
            this.label68.TabIndex = (181);
            this.label68.Text = ("NOTE: When not The Whole Map, Sight is significantly\r\nreduced in hallways");
            this.label68.TextAlign = (global::System.Drawing.ContentAlignment.MiddleLeft);
            // 
            // btnRemoveNPCOnAttackAction
            // 
            this.btnRemoveNPCOnAttackAction.Location = (new global::System.Drawing.Point(271, 638));
            this.btnRemoveNPCOnAttackAction.Name = ("btnRemoveNPCOnAttackAction");
            this.btnRemoveNPCOnAttackAction.Size = (new global::System.Drawing.Size(68, 23));
            this.btnRemoveNPCOnAttackAction.TabIndex = (180);
            this.btnRemoveNPCOnAttackAction.Text = ("Remove");
            this.btnRemoveNPCOnAttackAction.UseVisualStyleBackColor = (true);
            this.btnRemoveNPCOnAttackAction.Click += (this.btnRemoveNPCOnAttackAction_Click);
            // 
            // btnEditNPCOnAttackAction
            // 
            this.btnEditNPCOnAttackAction.Location = (new global::System.Drawing.Point(271, 609));
            this.btnEditNPCOnAttackAction.Name = ("btnEditNPCOnAttackAction");
            this.btnEditNPCOnAttackAction.Size = (new global::System.Drawing.Size(68, 23));
            this.btnEditNPCOnAttackAction.TabIndex = (179);
            this.btnEditNPCOnAttackAction.Text = ("Edit");
            this.btnEditNPCOnAttackAction.UseVisualStyleBackColor = (true);
            this.btnEditNPCOnAttackAction.Click += (this.btnEditNPCOnAttackAction_Click);
            // 
            // btnAddNPCOnAttackAction
            // 
            this.btnAddNPCOnAttackAction.Location = (new global::System.Drawing.Point(271, 580));
            this.btnAddNPCOnAttackAction.Name = ("btnAddNPCOnAttackAction");
            this.btnAddNPCOnAttackAction.Size = (new global::System.Drawing.Size(68, 23));
            this.btnAddNPCOnAttackAction.TabIndex = (178);
            this.btnAddNPCOnAttackAction.Text = ("Add");
            this.btnAddNPCOnAttackAction.UseVisualStyleBackColor = (true);
            this.btnAddNPCOnAttackAction.Click += (this.btnAddNPCOnAttackAction_Click);
            // 
            // lbNPCOnAttackActions
            // 
            this.lbNPCOnAttackActions.FormattingEnabled = (true);
            this.lbNPCOnAttackActions.ItemHeight = (15);
            this.lbNPCOnAttackActions.Location = (new global::System.Drawing.Point(168, 574));
            this.lbNPCOnAttackActions.Name = ("lbNPCOnAttackActions");
            this.lbNPCOnAttackActions.Size = (new global::System.Drawing.Size(97, 94));
            this.lbNPCOnAttackActions.TabIndex = (177);
            this.lbNPCOnAttackActions.SelectedIndexChanged += (this.lbNPCOnAttackActions_SelectedIndexChanged);
            // 
            // label69
            // 
            this.label69.AutoSize = (true);
            this.label69.Location = (new global::System.Drawing.Point(13, 599));
            this.label69.Name = ("label69");
            this.label69.Size = (new global::System.Drawing.Size(132, 30));
            this.label69.TabIndex = (176);
            this.label69.Text = ("Can do the following to\r\ninteract with someone:");
            // 
            // cmbNPCStartingArmor
            // 
            this.cmbNPCStartingArmor.DropDownStyle = (global::System.Windows.Forms.ComboBoxStyle.DropDownList);
            this.cmbNPCStartingArmor.FormattingEnabled = (true);
            this.cmbNPCStartingArmor.Location = (new global::System.Drawing.Point(146, 362));
            this.cmbNPCStartingArmor.Name = ("cmbNPCStartingArmor");
            this.cmbNPCStartingArmor.Size = (new global::System.Drawing.Size(158, 23));
            this.cmbNPCStartingArmor.TabIndex = (175);
            this.cmbNPCStartingArmor.SelectedIndexChanged += (this.cmbNPCStartingArmor_SelectedIndexChanged);
            // 
            // label70
            // 
            this.label70.AutoSize = (true);
            this.label70.Location = (new global::System.Drawing.Point(13, 365));
            this.label70.Name = ("label70");
            this.label70.Size = (new global::System.Drawing.Size(131, 15));
            this.label70.TabIndex = (174);
            this.label70.Text = ("When unarmored, wear");
            // 
            // cmbNPCStartingWeapon
            // 
            this.cmbNPCStartingWeapon.DropDownStyle = (global::System.Windows.Forms.ComboBoxStyle.DropDownList);
            this.cmbNPCStartingWeapon.FormattingEnabled = (true);
            this.cmbNPCStartingWeapon.Location = (new global::System.Drawing.Point(139, 331));
            this.cmbNPCStartingWeapon.Name = ("cmbNPCStartingWeapon");
            this.cmbNPCStartingWeapon.Size = (new global::System.Drawing.Size(165, 23));
            this.cmbNPCStartingWeapon.TabIndex = (173);
            this.cmbNPCStartingWeapon.SelectedIndexChanged += (this.cmbNPCStartingWeapon_SelectedIndexChanged);
            // 
            // label71
            // 
            this.label71.AutoSize = (true);
            this.label71.Location = (new global::System.Drawing.Point(13, 334));
            this.label71.Name = ("label71");
            this.label71.Size = (new global::System.Drawing.Size(123, 15));
            this.label71.TabIndex = (172);
            this.label71.Text = ("When unarmed, wield");
            // 
            // lbNPCStartingInventory
            // 
            this.lbNPCStartingInventory.FormattingEnabled = (true);
            this.lbNPCStartingInventory.ItemHeight = (15);
            this.lbNPCStartingInventory.Location = (new global::System.Drawing.Point(156, 423));
            this.lbNPCStartingInventory.Name = ("lbNPCStartingInventory");
            this.lbNPCStartingInventory.Size = (new global::System.Drawing.Size(148, 79));
            this.lbNPCStartingInventory.TabIndex = (171);
            this.lbNPCStartingInventory.SelectedIndexChanged += (this.lbNPCStartingInventory_SelectedIndexChanged);
            // 
            // btnNPCRemoveItem
            // 
            this.btnNPCRemoveItem.Enabled = (false);
            this.btnNPCRemoveItem.Location = (new global::System.Drawing.Point(75, 475));
            this.btnNPCRemoveItem.Name = ("btnNPCRemoveItem");
            this.btnNPCRemoveItem.Size = (new global::System.Drawing.Size(59, 23));
            this.btnNPCRemoveItem.TabIndex = (170);
            this.btnNPCRemoveItem.Text = ("Remove");
            this.btnNPCRemoveItem.UseVisualStyleBackColor = (true);
            this.btnNPCRemoveItem.Click += (this.btnNPCRemoveItem_Click);
            // 
            // btnNPCAddItem
            // 
            this.btnNPCAddItem.Enabled = (false);
            this.btnNPCAddItem.Location = (new global::System.Drawing.Point(13, 475));
            this.btnNPCAddItem.Name = ("btnNPCAddItem");
            this.btnNPCAddItem.Size = (new global::System.Drawing.Size(56, 23));
            this.btnNPCAddItem.TabIndex = (169);
            this.btnNPCAddItem.Text = ("Add");
            this.btnNPCAddItem.UseVisualStyleBackColor = (true);
            this.btnNPCAddItem.Click += (this.btnNPCAddItem_Click);
            // 
            // cmbNPCInventoryItemChoices
            // 
            this.cmbNPCInventoryItemChoices.DropDownStyle = (global::System.Windows.Forms.ComboBoxStyle.DropDownList);
            this.cmbNPCInventoryItemChoices.FormattingEnabled = (true);
            this.cmbNPCInventoryItemChoices.Location = (new global::System.Drawing.Point(13, 446));
            this.cmbNPCInventoryItemChoices.Name = ("cmbNPCInventoryItemChoices");
            this.cmbNPCInventoryItemChoices.Size = (new global::System.Drawing.Size(121, 23));
            this.cmbNPCInventoryItemChoices.TabIndex = (168);
            this.cmbNPCInventoryItemChoices.SelectedIndexChanged += (this.cmbNPCInventoryItemChoices_SelectedIndexChanged);
            // 
            // label72
            // 
            this.label72.AutoSize = (true);
            this.label72.Location = (new global::System.Drawing.Point(13, 423));
            this.label72.Name = ("label72");
            this.label72.Size = (new global::System.Drawing.Size(92, 15));
            this.label72.TabIndex = (167);
            this.label72.Text = ("Initial Inventory:");
            // 
            // label73
            // 
            this.label73.AutoSize = (true);
            this.label73.Location = (new global::System.Drawing.Point(172, 395));
            this.label73.Name = ("label73");
            this.label73.Size = (new global::System.Drawing.Size(36, 15));
            this.label73.TabIndex = (166);
            this.label73.Text = ("items");
            // 
            // nudNPCInventorySize
            // 
            this.nudNPCInventorySize.Location = (new global::System.Drawing.Point(121, 390));
            this.nudNPCInventorySize.Maximum = (new global::System.Decimal(new global::System.Int32[] { 999, 0, 0, 0 }));
            this.nudNPCInventorySize.Name = ("nudNPCInventorySize");
            this.nudNPCInventorySize.Size = (new global::System.Drawing.Size(45, 23));
            this.nudNPCInventorySize.TabIndex = (165);
            this.nudNPCInventorySize.ValueChanged += (this.nudNPCInventorySize_ValueChanged);
            // 
            // label74
            // 
            this.label74.AutoSize = (true);
            this.label74.Location = (new global::System.Drawing.Point(13, 393));
            this.label74.Name = ("label74");
            this.label74.Size = (new global::System.Drawing.Size(109, 15));
            this.label74.TabIndex = (164);
            this.label74.Text = ("Inventory Capacity:");
            // 
            // label75
            // 
            this.label75.AutoSize = (true);
            this.label75.Location = (new global::System.Drawing.Point(407, 641));
            this.label75.Name = ("label75");
            this.label75.Size = (new global::System.Drawing.Size(54, 15));
            this.label75.TabIndex = (163);
            this.label75.Text = ("Recovers");
            // 
            // label76
            // 
            this.label76.AutoSize = (true);
            this.label76.Location = (new global::System.Drawing.Point(395, 483));
            this.label76.Name = ("label76");
            this.label76.Size = (new global::System.Drawing.Size(325, 30));
            this.label76.TabIndex = (162);
            this.label76.Text = ("NOTE: HP, Attack, Defense, Movement and HP Regeneration\r\nare internal names. Display Names depend on Locales.");
            // 
            // label77
            // 
            this.label77.AutoSize = (true);
            this.label77.Font = (new global::System.Drawing.Font("Segoe UI", 9F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point));
            this.label77.Location = (new global::System.Drawing.Point(408, 454));
            this.label77.Name = ("label77");
            this.label77.Size = (new global::System.Drawing.Size(115, 15));
            this.label77.TabIndex = (161);
            this.label77.Text = ("After Leveling Up...");
            // 
            // chkNPCCanGainExperience
            // 
            this.chkNPCCanGainExperience.AutoSize = (true);
            this.chkNPCCanGainExperience.Location = (new global::System.Drawing.Point(407, 362));
            this.chkNPCCanGainExperience.Name = ("chkNPCCanGainExperience");
            this.chkNPCCanGainExperience.Size = (new global::System.Drawing.Size(169, 19));
            this.chkNPCCanGainExperience.TabIndex = (160);
            this.chkNPCCanGainExperience.Text = ("Can gain Experience Points");
            this.chkNPCCanGainExperience.UseVisualStyleBackColor = (true);
            this.chkNPCCanGainExperience.CheckedChanged += (this.chkNPCCanGainExperience_CheckedChanged);
            // 
            // nudNPCMaxLevel
            // 
            this.nudNPCMaxLevel.Location = (new global::System.Drawing.Point(513, 414));
            this.nudNPCMaxLevel.Name = ("nudNPCMaxLevel");
            this.nudNPCMaxLevel.Size = (new global::System.Drawing.Size(44, 23));
            this.nudNPCMaxLevel.TabIndex = (159);
            this.nudNPCMaxLevel.Value = (new global::System.Decimal(new global::System.Int32[] { 1, 0, 0, 0 }));
            this.nudNPCMaxLevel.ValueChanged += (this.nudNPCMaxLevel_ValueChanged);
            // 
            // label78
            // 
            this.label78.AutoSize = (true);
            this.label78.Location = (new global::System.Drawing.Point(404, 417));
            this.label78.Name = ("label78");
            this.label78.Size = (new global::System.Drawing.Size(103, 15));
            this.label78.TabIndex = (158);
            this.label78.Text = ("Maximum Level is");
            // 
            // txtNPCLevelUpFormula
            // 
            this.txtNPCLevelUpFormula.Location = (new global::System.Drawing.Point(534, 386));
            this.txtNPCLevelUpFormula.Name = ("txtNPCLevelUpFormula");
            this.txtNPCLevelUpFormula.Size = (new global::System.Drawing.Size(180, 23));
            this.txtNPCLevelUpFormula.TabIndex = (157);
            this.txtNPCLevelUpFormula.Enter += (this.txtNPCLevelUpFormula_Enter);
            this.txtNPCLevelUpFormula.Leave += (this.txtNPCLevelUpFormula_Leave);
            // 
            // label79
            // 
            this.label79.AutoSize = (true);
            this.label79.Location = (new global::System.Drawing.Point(404, 389));
            this.label79.Name = ("label79");
            this.label79.Size = (new global::System.Drawing.Size(126, 15));
            this.label79.TabIndex = (156);
            this.label79.Text = ("Experience to Level Up");
            // 
            // label80
            // 
            this.label80.AutoSize = (true);
            this.label80.Font = (new global::System.Drawing.Font("Segoe UI", 12F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point));
            this.label80.Location = (new global::System.Drawing.Point(502, 332));
            this.label80.Name = ("label80");
            this.label80.Size = (new global::System.Drawing.Size(101, 21));
            this.label80.TabIndex = (155);
            this.label80.Text = ("Leveling Up");
            // 
            // nudNPCFlatSightRange
            // 
            this.nudNPCFlatSightRange.Location = (new global::System.Drawing.Point(588, 255));
            this.nudNPCFlatSightRange.Maximum = (new global::System.Decimal(new global::System.Int32[] { 999, 0, 0, 0 }));
            this.nudNPCFlatSightRange.Name = ("nudNPCFlatSightRange");
            this.nudNPCFlatSightRange.Size = (new global::System.Drawing.Size(54, 23));
            this.nudNPCFlatSightRange.TabIndex = (153);
            this.nudNPCFlatSightRange.Visible = (false);
            this.nudNPCFlatSightRange.ValueChanged += (this.nudNPCFlatSightRange_ValueChanged);
            // 
            // cmbNPCSightRange
            // 
            this.cmbNPCSightRange.DropDownStyle = (global::System.Windows.Forms.ComboBoxStyle.DropDownList);
            this.cmbNPCSightRange.FormattingEnabled = (true);
            this.cmbNPCSightRange.Location = (new global::System.Drawing.Point(452, 255));
            this.cmbNPCSightRange.Name = ("cmbNPCSightRange");
            this.cmbNPCSightRange.Size = (new global::System.Drawing.Size(121, 23));
            this.cmbNPCSightRange.TabIndex = (152);
            this.cmbNPCSightRange.SelectedIndexChanged += (this.cmbNPCSightRange_SelectedIndexChanged);
            // 
            // label81
            // 
            this.label81.AutoSize = (true);
            this.label81.Location = (new global::System.Drawing.Point(402, 258));
            this.label81.Name = ("label81");
            this.label81.Size = (new global::System.Drawing.Size(48, 15));
            this.label81.TabIndex = (151);
            this.label81.Text = ("Can see");
            // 
            // label82
            // 
            this.label82.AutoSize = (true);
            this.label82.Location = (new global::System.Drawing.Point(637, 202));
            this.label82.Name = ("label82");
            this.label82.Size = (new global::System.Drawing.Size(68, 15));
            this.label82.TabIndex = (150);
            this.label82.Text = ("HP per turn");
            // 
            // label83
            // 
            this.label83.AutoSize = (true);
            this.label83.Location = (new global::System.Drawing.Point(522, 200));
            this.label83.Name = ("label83");
            this.label83.Size = (new global::System.Drawing.Size(54, 15));
            this.label83.TabIndex = (149);
            this.label83.Text = ("Recovers");
            // 
            // label84
            // 
            this.label84.AutoSize = (true);
            this.label84.Location = (new global::System.Drawing.Point(521, 643));
            this.label84.Name = ("label84");
            this.label84.Size = (new global::System.Drawing.Size(167, 15));
            this.label84.TabIndex = (148);
            this.label84.Text = ("more HP per turn per Level Up");
            // 
            // nudNPCHPRegenerationPerLevelUp
            // 
            this.nudNPCHPRegenerationPerLevelUp.DecimalPlaces = (4);
            this.nudNPCHPRegenerationPerLevelUp.Location = (new global::System.Drawing.Point(464, 639));
            this.nudNPCHPRegenerationPerLevelUp.Maximum = (new global::System.Decimal(new global::System.Int32[] { 999, 0, 0, 0 }));
            this.nudNPCHPRegenerationPerLevelUp.Name = ("nudNPCHPRegenerationPerLevelUp");
            this.nudNPCHPRegenerationPerLevelUp.Size = (new global::System.Drawing.Size(54, 23));
            this.nudNPCHPRegenerationPerLevelUp.TabIndex = (147);
            // 
            // nudNPCBaseHPRegeneration
            // 
            this.nudNPCBaseHPRegeneration.DecimalPlaces = (4);
            this.nudNPCBaseHPRegeneration.Location = (new global::System.Drawing.Point(577, 198));
            this.nudNPCBaseHPRegeneration.Maximum = (new global::System.Decimal(new global::System.Int32[] { 999, 0, 0, 0 }));
            this.nudNPCBaseHPRegeneration.Name = ("nudNPCBaseHPRegeneration");
            this.nudNPCBaseHPRegeneration.Size = (new global::System.Drawing.Size(55, 23));
            this.nudNPCBaseHPRegeneration.TabIndex = (146);
            this.nudNPCBaseHPRegeneration.Value = (new global::System.Decimal(new global::System.Int32[] { 1, 0, 0, 0 }));
            this.nudNPCBaseHPRegeneration.ValueChanged += (this.nudNPCBaseHPRegeneration_ValueChanged);
            // 
            // label85
            // 
            this.label85.AutoSize = (true);
            this.label85.Location = (new global::System.Drawing.Point(407, 614));
            this.label85.Name = ("label85");
            this.label85.Size = (new global::System.Drawing.Size(61, 15));
            this.label85.TabIndex = (145);
            this.label85.Text = ("Can move");
            // 
            // label86
            // 
            this.label86.AutoSize = (true);
            this.label86.Location = (new global::System.Drawing.Point(643, 170));
            this.label86.Name = ("label86");
            this.label86.Size = (new global::System.Drawing.Size(73, 15));
            this.label86.TabIndex = (144);
            this.label86.Text = ("tiles per turn");
            // 
            // label87
            // 
            this.label87.AutoSize = (true);
            this.label87.Location = (new global::System.Drawing.Point(522, 168));
            this.label87.Name = ("label87");
            this.label87.Size = (new global::System.Drawing.Size(61, 15));
            this.label87.TabIndex = (143);
            this.label87.Text = ("Can move");
            // 
            // label88
            // 
            this.label88.AutoSize = (true);
            this.label88.Location = (new global::System.Drawing.Point(525, 615));
            this.label88.Name = ("label88");
            this.label88.Size = (new global::System.Drawing.Size(174, 15));
            this.label88.TabIndex = (142);
            this.label88.Text = ("more Tiles per turn per Level Up");
            // 
            // nudNPCMovementPerLevelUp
            // 
            this.nudNPCMovementPerLevelUp.DecimalPlaces = (4);
            this.nudNPCMovementPerLevelUp.Location = (new global::System.Drawing.Point(468, 612));
            this.nudNPCMovementPerLevelUp.Maximum = (new global::System.Decimal(new global::System.Int32[] { 999, 0, 0, 0 }));
            this.nudNPCMovementPerLevelUp.Name = ("nudNPCMovementPerLevelUp");
            this.nudNPCMovementPerLevelUp.Size = (new global::System.Drawing.Size(54, 23));
            this.nudNPCMovementPerLevelUp.TabIndex = (141);
            this.nudNPCMovementPerLevelUp.ValueChanged += (this.nudNPCMovementPerLevelUp_ValueChanged);
            // 
            // nudNPCBaseMovement
            // 
            this.nudNPCBaseMovement.Location = (new global::System.Drawing.Point(583, 166));
            this.nudNPCBaseMovement.Maximum = (new global::System.Decimal(new global::System.Int32[] { 999, 0, 0, 0 }));
            this.nudNPCBaseMovement.Minimum = (new global::System.Decimal(new global::System.Int32[] { 1, 0, 0, 0 }));
            this.nudNPCBaseMovement.Name = ("nudNPCBaseMovement");
            this.nudNPCBaseMovement.Size = (new global::System.Drawing.Size(55, 23));
            this.nudNPCBaseMovement.TabIndex = (140);
            this.nudNPCBaseMovement.Value = (new global::System.Decimal(new global::System.Int32[] { 1, 0, 0, 0 }));
            this.nudNPCBaseMovement.ValueChanged += (this.nudNPCBaseMovement_ValueChanged);
            // 
            // label89
            // 
            this.label89.AutoSize = (true);
            this.label89.Location = (new global::System.Drawing.Point(463, 585));
            this.label89.Name = ("label89");
            this.label89.Size = (new global::System.Drawing.Size(148, 15));
            this.label89.TabIndex = (139);
            this.label89.Text = ("more Defense per Level Up");
            // 
            // nudNPCDefensePerLevelUp
            // 
            this.nudNPCDefensePerLevelUp.DecimalPlaces = (4);
            this.nudNPCDefensePerLevelUp.Location = (new global::System.Drawing.Point(407, 582));
            this.nudNPCDefensePerLevelUp.Maximum = (new global::System.Decimal(new global::System.Int32[] { 999, 0, 0, 0 }));
            this.nudNPCDefensePerLevelUp.Name = ("nudNPCDefensePerLevelUp");
            this.nudNPCDefensePerLevelUp.Size = (new global::System.Drawing.Size(54, 23));
            this.nudNPCDefensePerLevelUp.TabIndex = (138);
            this.nudNPCDefensePerLevelUp.ValueChanged += (this.nudNPCDefensePerLevelUp_ValueChanged);
            // 
            // label90
            // 
            this.label90.AutoSize = (true);
            this.label90.Location = (new global::System.Drawing.Point(459, 229));
            this.label90.Name = ("label90");
            this.label90.Size = (new global::System.Drawing.Size(49, 15));
            this.label90.TabIndex = (137);
            this.label90.Text = ("Defense");
            // 
            // nudNPCBaseDefense
            // 
            this.nudNPCBaseDefense.Location = (new global::System.Drawing.Point(402, 225));
            this.nudNPCBaseDefense.Maximum = (new global::System.Decimal(new global::System.Int32[] { 999, 0, 0, 0 }));
            this.nudNPCBaseDefense.Name = ("nudNPCBaseDefense");
            this.nudNPCBaseDefense.Size = (new global::System.Drawing.Size(55, 23));
            this.nudNPCBaseDefense.TabIndex = (136);
            this.nudNPCBaseDefense.ValueChanged += (this.nudNPCBaseDefense_ValueChanged);
            // 
            // label91
            // 
            this.label91.AutoSize = (true);
            this.label91.Location = (new global::System.Drawing.Point(394, 124));
            this.label91.Name = ("label91");
            this.label91.Size = (new global::System.Drawing.Size(325, 30));
            this.label91.TabIndex = (135);
            this.label91.Text = ("NOTE: HP, Attack, Defense, Movement and HP Regeneration\r\nare internal names. Display Names depend on Locales.");
            // 
            // label92
            // 
            this.label92.AutoSize = (true);
            this.label92.Location = (new global::System.Drawing.Point(463, 558));
            this.label92.Name = ("label92");
            this.label92.Size = (new global::System.Drawing.Size(140, 15));
            this.label92.TabIndex = (134);
            this.label92.Text = ("more Attack per Level Up");
            // 
            // nudNPCAttackPerLevelUp
            // 
            this.nudNPCAttackPerLevelUp.DecimalPlaces = (4);
            this.nudNPCAttackPerLevelUp.Location = (new global::System.Drawing.Point(407, 555));
            this.nudNPCAttackPerLevelUp.Maximum = (new global::System.Decimal(new global::System.Int32[] { 999, 0, 0, 0 }));
            this.nudNPCAttackPerLevelUp.Name = ("nudNPCAttackPerLevelUp");
            this.nudNPCAttackPerLevelUp.Size = (new global::System.Drawing.Size(54, 23));
            this.nudNPCAttackPerLevelUp.TabIndex = (133);
            this.nudNPCAttackPerLevelUp.ValueChanged += (this.nudNPCAttackPerLevelUp_ValueChanged);
            // 
            // label93
            // 
            this.label93.AutoSize = (true);
            this.label93.Location = (new global::System.Drawing.Point(459, 200));
            this.label93.Name = ("label93");
            this.label93.Size = (new global::System.Drawing.Size(41, 15));
            this.label93.TabIndex = (132);
            this.label93.Text = ("Attack");
            // 
            // nudNPCBaseAttack
            // 
            this.nudNPCBaseAttack.Location = (new global::System.Drawing.Point(402, 196));
            this.nudNPCBaseAttack.Maximum = (new global::System.Decimal(new global::System.Int32[] { 999, 0, 0, 0 }));
            this.nudNPCBaseAttack.Name = ("nudNPCBaseAttack");
            this.nudNPCBaseAttack.Size = (new global::System.Drawing.Size(55, 23));
            this.nudNPCBaseAttack.TabIndex = (131);
            this.nudNPCBaseAttack.ValueChanged += (this.nudNPCBaseAttack_ValueChanged);
            // 
            // label94
            // 
            this.label94.AutoSize = (true);
            this.label94.Location = (new global::System.Drawing.Point(463, 531));
            this.label94.Name = ("label94");
            this.label94.Size = (new global::System.Drawing.Size(122, 15));
            this.label94.TabIndex = (130);
            this.label94.Text = ("more HP per Level Up");
            // 
            // nudNPCHPPerLevelUp
            // 
            this.nudNPCHPPerLevelUp.DecimalPlaces = (4);
            this.nudNPCHPPerLevelUp.Location = (new global::System.Drawing.Point(407, 528));
            this.nudNPCHPPerLevelUp.Maximum = (new global::System.Decimal(new global::System.Int32[] { 999, 0, 0, 0 }));
            this.nudNPCHPPerLevelUp.Name = ("nudNPCHPPerLevelUp");
            this.nudNPCHPPerLevelUp.Size = (new global::System.Drawing.Size(54, 23));
            this.nudNPCHPPerLevelUp.TabIndex = (129);
            this.nudNPCHPPerLevelUp.ValueChanged += (this.nudNPCHPPerLevelUp_ValueChanged);
            // 
            // label95
            // 
            this.label95.AutoSize = (true);
            this.label95.Location = (new global::System.Drawing.Point(459, 170));
            this.label95.Name = ("label95");
            this.label95.Size = (new global::System.Drawing.Size(23, 15));
            this.label95.TabIndex = (128);
            this.label95.Text = ("HP");
            // 
            // nudNPCBaseHP
            // 
            this.nudNPCBaseHP.Location = (new global::System.Drawing.Point(402, 166));
            this.nudNPCBaseHP.Maximum = (new global::System.Decimal(new global::System.Int32[] { 999, 0, 0, 0 }));
            this.nudNPCBaseHP.Minimum = (new global::System.Decimal(new global::System.Int32[] { 1, 0, 0, 0 }));
            this.nudNPCBaseHP.Name = ("nudNPCBaseHP");
            this.nudNPCBaseHP.Size = (new global::System.Drawing.Size(55, 23));
            this.nudNPCBaseHP.TabIndex = (127);
            this.nudNPCBaseHP.Value = (new global::System.Decimal(new global::System.Int32[] { 1, 0, 0, 0 }));
            this.nudNPCBaseHP.ValueChanged += (this.nudNPCBaseHP_ValueChanged);
            // 
            // label96
            // 
            this.label96.AutoSize = (true);
            this.label96.Font = (new global::System.Drawing.Font("Segoe UI", 12F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point));
            this.label96.Location = (new global::System.Drawing.Point(477, 100));
            this.label96.Name = ("label96");
            this.label96.Size = (new global::System.Drawing.Size(155, 21));
            this.label96.TabIndex = (126);
            this.label96.Text = ("Base Stats (Level 1)");
            // 
            // btnChangeNPCConsoleCharacterForeColor
            // 
            this.btnChangeNPCConsoleCharacterForeColor.Location = (new global::System.Drawing.Point(577, 37));
            this.btnChangeNPCConsoleCharacterForeColor.Name = ("btnChangeNPCConsoleCharacterForeColor");
            this.btnChangeNPCConsoleCharacterForeColor.Size = (new global::System.Drawing.Size(135, 23));
            this.btnChangeNPCConsoleCharacterForeColor.TabIndex = (125);
            this.btnChangeNPCConsoleCharacterForeColor.Text = ("Change Foreground...");
            this.btnChangeNPCConsoleCharacterForeColor.UseVisualStyleBackColor = (true);
            this.btnChangeNPCConsoleCharacterForeColor.Click += (this.btnChangeNPCConsoleCharacterForeColor_Click);
            // 
            // btnChangeNPCConsoleCharacter
            // 
            this.btnChangeNPCConsoleCharacter.Location = (new global::System.Drawing.Point(577, 8));
            this.btnChangeNPCConsoleCharacter.Name = ("btnChangeNPCConsoleCharacter");
            this.btnChangeNPCConsoleCharacter.Size = (new global::System.Drawing.Size(135, 23));
            this.btnChangeNPCConsoleCharacter.TabIndex = (124);
            this.btnChangeNPCConsoleCharacter.Text = ("Change Character...");
            this.btnChangeNPCConsoleCharacter.UseVisualStyleBackColor = (true);
            this.btnChangeNPCConsoleCharacter.Click += (this.btnChangeNPCConsoleCharacter_Click);
            // 
            // lblNPCConsoleRepresentation
            // 
            this.lblNPCConsoleRepresentation.Font = (new global::System.Drawing.Font("Courier New", 36F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point));
            this.lblNPCConsoleRepresentation.Location = (new global::System.Drawing.Point(504, 17));
            this.lblNPCConsoleRepresentation.Name = ("lblNPCConsoleRepresentation");
            this.lblNPCConsoleRepresentation.Size = (new global::System.Drawing.Size(64, 64));
            this.lblNPCConsoleRepresentation.TabIndex = (123);
            this.lblNPCConsoleRepresentation.TextAlign = (global::System.Drawing.ContentAlignment.MiddleCenter);
            // 
            // label98
            // 
            this.label98.Font = (new global::System.Drawing.Font("Segoe UI", 14.25F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point));
            this.label98.Location = (new global::System.Drawing.Point(370, 19));
            this.label98.Name = ("label98");
            this.label98.Size = (new global::System.Drawing.Size(131, 52));
            this.label98.TabIndex = (122);
            this.label98.Text = ("Appearance -");
            this.label98.TextAlign = (global::System.Drawing.ContentAlignment.MiddleCenter);
            // 
            // chkNPCStartsVisible
            // 
            this.chkNPCStartsVisible.AutoSize = (true);
            this.chkNPCStartsVisible.Location = (new global::System.Drawing.Point(13, 240));
            this.chkNPCStartsVisible.Name = ("chkNPCStartsVisible");
            this.chkNPCStartsVisible.Size = (new global::System.Drawing.Size(102, 19));
            this.chkNPCStartsVisible.TabIndex = (121);
            this.chkNPCStartsVisible.Text = ("Spawns visible");
            this.chkNPCStartsVisible.UseVisualStyleBackColor = (true);
            this.chkNPCStartsVisible.CheckedChanged += (this.chkNPCStartsVisible_CheckedChanged);
            // 
            // cmbNPCFaction
            // 
            this.cmbNPCFaction.DropDownStyle = (global::System.Windows.Forms.ComboBoxStyle.DropDownList);
            this.cmbNPCFaction.FormattingEnabled = (true);
            this.cmbNPCFaction.Location = (new global::System.Drawing.Point(65, 209));
            this.cmbNPCFaction.Name = ("cmbNPCFaction");
            this.cmbNPCFaction.Size = (new global::System.Drawing.Size(146, 23));
            this.cmbNPCFaction.TabIndex = (120);
            this.cmbNPCFaction.SelectedIndexChanged += (this.cmbNPCFaction_SelectedIndexChanged);
            // 
            // label99
            // 
            this.label99.AutoSize = (true);
            this.label99.Location = (new global::System.Drawing.Point(13, 212));
            this.label99.Name = ("label99");
            this.label99.Size = (new global::System.Drawing.Size(46, 15));
            this.label99.TabIndex = (119);
            this.label99.Text = ("Faction");
            // 
            // fklblNPCDescriptionLocale
            // 
            this.fklblNPCDescriptionLocale.Enabled = (false);
            this.fklblNPCDescriptionLocale.FlatAppearance.BorderSize = (0);
            this.fklblNPCDescriptionLocale.FlatStyle = (global::System.Windows.Forms.FlatStyle.Flat);
            this.fklblNPCDescriptionLocale.Image = ((global::System.Drawing.Image)(resources.GetObject("fklblNPCDescriptionLocale.Image")));
            this.fklblNPCDescriptionLocale.ImageAlign = (global::System.Drawing.ContentAlignment.TopLeft);
            this.fklblNPCDescriptionLocale.Location = (new global::System.Drawing.Point(13, 153));
            this.fklblNPCDescriptionLocale.Name = ("fklblNPCDescriptionLocale");
            this.fklblNPCDescriptionLocale.Size = (new global::System.Drawing.Size(331, 42));
            this.fklblNPCDescriptionLocale.TabIndex = (117);
            this.fklblNPCDescriptionLocale.Text = ("This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.");
            this.fklblNPCDescriptionLocale.TextAlign = (global::System.Drawing.ContentAlignment.MiddleLeft);
            this.fklblNPCDescriptionLocale.TextImageRelation = (global::System.Windows.Forms.TextImageRelation.ImageBeforeText);
            this.fklblNPCDescriptionLocale.UseVisualStyleBackColor = (true);
            this.fklblNPCDescriptionLocale.Visible = (false);
            // 
            // txtNPCDescription
            // 
            this.txtNPCDescription.Location = (new global::System.Drawing.Point(13, 124));
            this.txtNPCDescription.Name = ("txtNPCDescription");
            this.txtNPCDescription.Size = (new global::System.Drawing.Size(350, 23));
            this.txtNPCDescription.TabIndex = (116);
            this.txtNPCDescription.TextChanged += (this.txtNPCDescription_TextChanged);
            // 
            // label100
            // 
            this.label100.AutoSize = (true);
            this.label100.Location = (new global::System.Drawing.Point(13, 106));
            this.label100.Name = ("label100");
            this.label100.Size = (new global::System.Drawing.Size(67, 15));
            this.label100.TabIndex = (115);
            this.label100.Text = ("Description");
            // 
            // fklblNPCNameLocale
            // 
            this.fklblNPCNameLocale.Enabled = (false);
            this.fklblNPCNameLocale.FlatAppearance.BorderSize = (0);
            this.fklblNPCNameLocale.FlatStyle = (global::System.Windows.Forms.FlatStyle.Flat);
            this.fklblNPCNameLocale.Image = ((global::System.Drawing.Image)(resources.GetObject("fklblNPCNameLocale.Image")));
            this.fklblNPCNameLocale.ImageAlign = (global::System.Drawing.ContentAlignment.TopLeft);
            this.fklblNPCNameLocale.Location = (new global::System.Drawing.Point(13, 55));
            this.fklblNPCNameLocale.Name = ("fklblNPCNameLocale");
            this.fklblNPCNameLocale.Size = (new global::System.Drawing.Size(331, 42));
            this.fklblNPCNameLocale.TabIndex = (114);
            this.fklblNPCNameLocale.Text = ("This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.");
            this.fklblNPCNameLocale.TextAlign = (global::System.Drawing.ContentAlignment.MiddleLeft);
            this.fklblNPCNameLocale.TextImageRelation = (global::System.Windows.Forms.TextImageRelation.ImageBeforeText);
            this.fklblNPCNameLocale.UseVisualStyleBackColor = (true);
            this.fklblNPCNameLocale.Visible = (false);
            // 
            // txtNPCName
            // 
            this.txtNPCName.Location = (new global::System.Drawing.Point(13, 26));
            this.txtNPCName.Name = ("txtNPCName");
            this.txtNPCName.Size = (new global::System.Drawing.Size(350, 23));
            this.txtNPCName.TabIndex = (113);
            this.txtNPCName.TextChanged += (this.txtNPCName_TextChanged);
            // 
            // label101
            // 
            this.label101.AutoSize = (true);
            this.label101.Location = (new global::System.Drawing.Point(13, 8));
            this.label101.Name = ("label101");
            this.label101.Size = (new global::System.Drawing.Size(80, 15));
            this.label101.TabIndex = (112);
            this.label101.Text = ("Default Name");
            // 
            // lblNPCSightRangeText
            // 
            this.lblNPCSightRangeText.AutoSize = (true);
            this.lblNPCSightRangeText.Location = (new global::System.Drawing.Point(648, 258));
            this.lblNPCSightRangeText.Name = ("lblNPCSightRangeText");
            this.lblNPCSightRangeText.Size = (new global::System.Drawing.Size(28, 15));
            this.lblNPCSightRangeText.TabIndex = (154);
            this.lblNPCSightRangeText.Text = ("tiles");
            this.lblNPCSightRangeText.Visible = (false);
            // 
            // tpItem
            // 
            this.tpItem.Controls.Add(this.btnItemOnTurnStartAction);
            this.tpItem.Controls.Add(this.lblItemOnTurnStartAction);
            this.tpItem.Controls.Add(this.btnItemOnAttackedAction);
            this.tpItem.Controls.Add(this.lblItemOnAttackedAction);
            this.tpItem.Controls.Add(this.btnRemoveItemOnAttackAction);
            this.tpItem.Controls.Add(this.btnEditItemOnAttackAction);
            this.tpItem.Controls.Add(this.btnAddItemOnAttackAction);
            this.tpItem.Controls.Add(this.lbItemOnAttackActions);
            this.tpItem.Controls.Add(this.lblItemOnAttackActions);
            this.tpItem.Controls.Add(this.btnItemOnUseAction);
            this.tpItem.Controls.Add(this.lblItemOnUseAction);
            this.tpItem.Controls.Add(this.btnItemOnSteppedAction);
            this.tpItem.Controls.Add(this.lblItemOnSteppedAction);
            this.tpItem.Controls.Add(this.txtItemPower);
            this.tpItem.Controls.Add(this.label108);
            this.tpItem.Controls.Add(this.chkItemCanBePickedUp);
            this.tpItem.Controls.Add(this.chkItemStartsVisible);
            this.tpItem.Controls.Add(this.cmbItemType);
            this.tpItem.Controls.Add(this.label107);
            this.tpItem.Controls.Add(this.btnChangeItemConsoleCharacterBackColor);
            this.tpItem.Controls.Add(this.btnChangeItemConsoleCharacterForeColor);
            this.tpItem.Controls.Add(this.btnChangeItemConsoleCharacter);
            this.tpItem.Controls.Add(this.lblItemConsoleRepresentation);
            this.tpItem.Controls.Add(this.label102);
            this.tpItem.Controls.Add(this.fklblItemDescriptionLocale);
            this.tpItem.Controls.Add(this.txtItemDescription);
            this.tpItem.Controls.Add(this.label105);
            this.tpItem.Controls.Add(this.fklblItemNameLocale);
            this.tpItem.Controls.Add(this.txtItemName);
            this.tpItem.Controls.Add(this.label106);
            this.tpItem.Location = (new global::System.Drawing.Point(4, 24));
            this.tpItem.Name = ("tpItem");
            this.tpItem.Size = (new global::System.Drawing.Size(740, 356));
            this.tpItem.TabIndex = (6);
            this.tpItem.Text = ("Item");
            this.tpItem.UseVisualStyleBackColor = (true);
            // 
            // btnItemOnTurnStartAction
            // 
            this.btnItemOnTurnStartAction.Location = (new global::System.Drawing.Point(661, 243));
            this.btnItemOnTurnStartAction.Name = ("btnItemOnTurnStartAction");
            this.btnItemOnTurnStartAction.Size = (new global::System.Drawing.Size(68, 23));
            this.btnItemOnTurnStartAction.TabIndex = (219);
            this.btnItemOnTurnStartAction.Text = ("... do this!");
            this.btnItemOnTurnStartAction.UseVisualStyleBackColor = (true);
            this.btnItemOnTurnStartAction.Click += (this.btnItemOnTurnStartAction_Click);
            // 
            // lblItemOnTurnStartAction
            // 
            this.lblItemOnTurnStartAction.AutoSize = (true);
            this.lblItemOnTurnStartAction.Font = (new global::System.Drawing.Font("Segoe UI", 9F, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point));
            this.lblItemOnTurnStartAction.Location = (new global::System.Drawing.Point(390, 247));
            this.lblItemOnTurnStartAction.Name = ("lblItemOnTurnStartAction");
            this.lblItemOnTurnStartAction.Size = (new global::System.Drawing.Size(256, 15));
            this.lblItemOnTurnStartAction.TabIndex = (218);
            this.lblItemOnTurnStartAction.Text = ("When someone equipping it starts a new turn...");
            // 
            // btnItemOnAttackedAction
            // 
            this.btnItemOnAttackedAction.Location = (new global::System.Drawing.Point(661, 281));
            this.btnItemOnAttackedAction.Name = ("btnItemOnAttackedAction");
            this.btnItemOnAttackedAction.Size = (new global::System.Drawing.Size(68, 23));
            this.btnItemOnAttackedAction.TabIndex = (217);
            this.btnItemOnAttackedAction.Text = ("... do this!");
            this.btnItemOnAttackedAction.UseVisualStyleBackColor = (true);
            this.btnItemOnAttackedAction.Click += (this.btnItemOnAttackedAction_Click);
            // 
            // lblItemOnAttackedAction
            // 
            this.lblItemOnAttackedAction.AutoSize = (true);
            this.lblItemOnAttackedAction.Font = (new global::System.Drawing.Font("Segoe UI", 9F, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point));
            this.lblItemOnAttackedAction.Location = (new global::System.Drawing.Point(390, 285));
            this.lblItemOnAttackedAction.Name = ("lblItemOnAttackedAction");
            this.lblItemOnAttackedAction.Size = (new global::System.Drawing.Size(273, 15));
            this.lblItemOnAttackedAction.TabIndex = (216);
            this.lblItemOnAttackedAction.Text = ("When someone equipping it gets interacted with...");
            // 
            // btnRemoveItemOnAttackAction
            // 
            this.btnRemoveItemOnAttackAction.Location = (new global::System.Drawing.Point(661, 204));
            this.btnRemoveItemOnAttackAction.Name = ("btnRemoveItemOnAttackAction");
            this.btnRemoveItemOnAttackAction.Size = (new global::System.Drawing.Size(68, 23));
            this.btnRemoveItemOnAttackAction.TabIndex = (215);
            this.btnRemoveItemOnAttackAction.Text = ("Remove");
            this.btnRemoveItemOnAttackAction.UseVisualStyleBackColor = (true);
            this.btnRemoveItemOnAttackAction.Click += (this.btnRemoveItemOnAttackAction_Click);
            // 
            // btnEditItemOnAttackAction
            // 
            this.btnEditItemOnAttackAction.Location = (new global::System.Drawing.Point(661, 175));
            this.btnEditItemOnAttackAction.Name = ("btnEditItemOnAttackAction");
            this.btnEditItemOnAttackAction.Size = (new global::System.Drawing.Size(68, 23));
            this.btnEditItemOnAttackAction.TabIndex = (214);
            this.btnEditItemOnAttackAction.Text = ("Edit");
            this.btnEditItemOnAttackAction.UseVisualStyleBackColor = (true);
            this.btnEditItemOnAttackAction.Click += (this.btnEditItemOnAttackAction_Click);
            // 
            // btnAddItemOnAttackAction
            // 
            this.btnAddItemOnAttackAction.Location = (new global::System.Drawing.Point(661, 146));
            this.btnAddItemOnAttackAction.Name = ("btnAddItemOnAttackAction");
            this.btnAddItemOnAttackAction.Size = (new global::System.Drawing.Size(68, 23));
            this.btnAddItemOnAttackAction.TabIndex = (213);
            this.btnAddItemOnAttackAction.Text = ("Add");
            this.btnAddItemOnAttackAction.UseVisualStyleBackColor = (true);
            this.btnAddItemOnAttackAction.Click += (this.btnAddItemOnAttackAction_Click);
            // 
            // lbItemOnAttackActions
            // 
            this.lbItemOnAttackActions.FormattingEnabled = (true);
            this.lbItemOnAttackActions.ItemHeight = (15);
            this.lbItemOnAttackActions.Location = (new global::System.Drawing.Point(558, 140));
            this.lbItemOnAttackActions.Name = ("lbItemOnAttackActions");
            this.lbItemOnAttackActions.Size = (new global::System.Drawing.Size(97, 94));
            this.lbItemOnAttackActions.TabIndex = (212);
            this.lbItemOnAttackActions.SelectedIndexChanged += (this.lbItemOnAttackActions_SelectedIndexChanged);
            // 
            // lblItemOnAttackActions
            // 
            this.lblItemOnAttackActions.AutoSize = (true);
            this.lblItemOnAttackActions.Location = (new global::System.Drawing.Point(390, 165));
            this.lblItemOnAttackActions.Name = ("lblItemOnAttackActions");
            this.lblItemOnAttackActions.Size = (new global::System.Drawing.Size(132, 30));
            this.lblItemOnAttackActions.TabIndex = (211);
            this.lblItemOnAttackActions.Text = ("Can do the following to\r\ninteract with someone:");
            // 
            // btnItemOnUseAction
            // 
            this.btnItemOnUseAction.Location = (new global::System.Drawing.Point(235, 316));
            this.btnItemOnUseAction.Name = ("btnItemOnUseAction");
            this.btnItemOnUseAction.Size = (new global::System.Drawing.Size(75, 23));
            this.btnItemOnUseAction.TabIndex = (210);
            this.btnItemOnUseAction.Text = ("... do this!");
            this.btnItemOnUseAction.UseVisualStyleBackColor = (true);
            this.btnItemOnUseAction.Click += (this.btnItemOnUseAction_Click);
            // 
            // lblItemOnUseAction
            // 
            this.lblItemOnUseAction.AutoSize = (true);
            this.lblItemOnUseAction.Font = (new global::System.Drawing.Font("Segoe UI", 9F, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point));
            this.lblItemOnUseAction.Location = (new global::System.Drawing.Point(15, 320));
            this.lblItemOnUseAction.Name = ("lblItemOnUseAction");
            this.lblItemOnUseAction.Size = (new global::System.Drawing.Size(214, 15));
            this.lblItemOnUseAction.TabIndex = (209);
            this.lblItemOnUseAction.Text = ("When someone uses it on themselves...");
            // 
            // btnItemOnSteppedAction
            // 
            this.btnItemOnSteppedAction.Location = (new global::System.Drawing.Point(235, 285));
            this.btnItemOnSteppedAction.Name = ("btnItemOnSteppedAction");
            this.btnItemOnSteppedAction.Size = (new global::System.Drawing.Size(75, 23));
            this.btnItemOnSteppedAction.TabIndex = (208);
            this.btnItemOnSteppedAction.Text = ("... do this!");
            this.btnItemOnSteppedAction.UseVisualStyleBackColor = (true);
            this.btnItemOnSteppedAction.Click += (this.btnItemOnSteppedAction_Click);
            // 
            // lblItemOnSteppedAction
            // 
            this.lblItemOnSteppedAction.AutoSize = (true);
            this.lblItemOnSteppedAction.Font = (new global::System.Drawing.Font("Segoe UI", 9F, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point));
            this.lblItemOnSteppedAction.Location = (new global::System.Drawing.Point(15, 289));
            this.lblItemOnSteppedAction.Name = ("lblItemOnSteppedAction");
            this.lblItemOnSteppedAction.Size = (new global::System.Drawing.Size(156, 15));
            this.lblItemOnSteppedAction.TabIndex = (207);
            this.lblItemOnSteppedAction.Text = ("When someone steps on it...");
            // 
            // txtItemPower
            // 
            this.txtItemPower.Location = (new global::System.Drawing.Point(86, 243));
            this.txtItemPower.Name = ("txtItemPower");
            this.txtItemPower.Size = (new global::System.Drawing.Size(150, 23));
            this.txtItemPower.TabIndex = (206);
            this.txtItemPower.Enter += (this.txtItemPower_Enter);
            this.txtItemPower.Leave += (this.txtItemPower_Leave);
            // 
            // label108
            // 
            this.label108.AutoSize = (true);
            this.label108.Location = (new global::System.Drawing.Point(13, 249));
            this.label108.Name = ("label108");
            this.label108.Size = (new global::System.Drawing.Size(67, 15));
            this.label108.TabIndex = (205);
            this.label108.Text = ("Item Power");
            // 
            // chkItemCanBePickedUp
            // 
            this.chkItemCanBePickedUp.AutoSize = (true);
            this.chkItemCanBePickedUp.Location = (new global::System.Drawing.Point(242, 245));
            this.chkItemCanBePickedUp.Name = ("chkItemCanBePickedUp");
            this.chkItemCanBePickedUp.Size = (new global::System.Drawing.Size(118, 19));
            this.chkItemCanBePickedUp.TabIndex = (204);
            this.chkItemCanBePickedUp.Text = ("Can be picked up");
            this.chkItemCanBePickedUp.UseVisualStyleBackColor = (true);
            this.chkItemCanBePickedUp.CheckedChanged += (this.chkItemCanBePickedUp_CheckedChanged);
            // 
            // chkItemStartsVisible
            // 
            this.chkItemStartsVisible.AutoSize = (true);
            this.chkItemStartsVisible.Location = (new global::System.Drawing.Point(242, 211));
            this.chkItemStartsVisible.Name = ("chkItemStartsVisible");
            this.chkItemStartsVisible.Size = (new global::System.Drawing.Size(102, 19));
            this.chkItemStartsVisible.TabIndex = (203);
            this.chkItemStartsVisible.Text = ("Spawns visible");
            this.chkItemStartsVisible.UseVisualStyleBackColor = (true);
            this.chkItemStartsVisible.CheckedChanged += (this.chkItemStartsVisible_CheckedChanged);
            // 
            // cmbItemType
            // 
            this.cmbItemType.DropDownStyle = (global::System.Windows.Forms.ComboBoxStyle.DropDownList);
            this.cmbItemType.FormattingEnabled = (true);
            this.cmbItemType.Items.AddRange(new global::System.Object[] { "Weapon", "Armor", "Consumable" });
            this.cmbItemType.Location = (new global::System.Drawing.Point(77, 209));
            this.cmbItemType.Name = ("cmbItemType");
            this.cmbItemType.Size = (new global::System.Drawing.Size(159, 23));
            this.cmbItemType.TabIndex = (202);
            this.cmbItemType.SelectedIndexChanged += (this.cmbItemType_SelectedIndexChanged);
            // 
            // label107
            // 
            this.label107.AutoSize = (true);
            this.label107.Location = (new global::System.Drawing.Point(13, 212));
            this.label107.Name = ("label107");
            this.label107.Size = (new global::System.Drawing.Size(58, 15));
            this.label107.TabIndex = (201);
            this.label107.Text = ("Item Type");
            // 
            // btnChangeItemConsoleCharacterBackColor
            // 
            this.btnChangeItemConsoleCharacterBackColor.Location = (new global::System.Drawing.Point(597, 67));
            this.btnChangeItemConsoleCharacterBackColor.Name = ("btnChangeItemConsoleCharacterBackColor");
            this.btnChangeItemConsoleCharacterBackColor.Size = (new global::System.Drawing.Size(135, 23));
            this.btnChangeItemConsoleCharacterBackColor.TabIndex = (200);
            this.btnChangeItemConsoleCharacterBackColor.Text = ("Change Background...");
            this.btnChangeItemConsoleCharacterBackColor.UseVisualStyleBackColor = (true);
            this.btnChangeItemConsoleCharacterBackColor.Click += (this.btnChangeItemConsoleCharacterBackColor_Click);
            // 
            // btnChangeItemConsoleCharacterForeColor
            // 
            this.btnChangeItemConsoleCharacterForeColor.Location = (new global::System.Drawing.Point(597, 38));
            this.btnChangeItemConsoleCharacterForeColor.Name = ("btnChangeItemConsoleCharacterForeColor");
            this.btnChangeItemConsoleCharacterForeColor.Size = (new global::System.Drawing.Size(135, 23));
            this.btnChangeItemConsoleCharacterForeColor.TabIndex = (199);
            this.btnChangeItemConsoleCharacterForeColor.Text = ("Change Foreground...");
            this.btnChangeItemConsoleCharacterForeColor.UseVisualStyleBackColor = (true);
            this.btnChangeItemConsoleCharacterForeColor.Click += (this.btnChangeItemConsoleCharacterForeColor_Click);
            // 
            // btnChangeItemConsoleCharacter
            // 
            this.btnChangeItemConsoleCharacter.Location = (new global::System.Drawing.Point(597, 9));
            this.btnChangeItemConsoleCharacter.Name = ("btnChangeItemConsoleCharacter");
            this.btnChangeItemConsoleCharacter.Size = (new global::System.Drawing.Size(135, 23));
            this.btnChangeItemConsoleCharacter.TabIndex = (198);
            this.btnChangeItemConsoleCharacter.Text = ("Change Character...");
            this.btnChangeItemConsoleCharacter.UseVisualStyleBackColor = (true);
            this.btnChangeItemConsoleCharacter.Click += (this.btnChangeItemConsoleCharacter_Click);
            // 
            // lblItemConsoleRepresentation
            // 
            this.lblItemConsoleRepresentation.Font = (new global::System.Drawing.Font("Courier New", 36F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point));
            this.lblItemConsoleRepresentation.Location = (new global::System.Drawing.Point(524, 18));
            this.lblItemConsoleRepresentation.Name = ("lblItemConsoleRepresentation");
            this.lblItemConsoleRepresentation.Size = (new global::System.Drawing.Size(64, 64));
            this.lblItemConsoleRepresentation.TabIndex = (197);
            this.lblItemConsoleRepresentation.TextAlign = (global::System.Drawing.ContentAlignment.MiddleCenter);
            // 
            // label102
            // 
            this.label102.Font = (new global::System.Drawing.Font("Segoe UI", 14.25F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point));
            this.label102.Location = (new global::System.Drawing.Point(390, 20));
            this.label102.Name = ("label102");
            this.label102.Size = (new global::System.Drawing.Size(131, 52));
            this.label102.TabIndex = (196);
            this.label102.Text = ("Appearance -");
            this.label102.TextAlign = (global::System.Drawing.ContentAlignment.MiddleCenter);
            // 
            // fklblItemDescriptionLocale
            // 
            this.fklblItemDescriptionLocale.Enabled = (false);
            this.fklblItemDescriptionLocale.FlatAppearance.BorderSize = (0);
            this.fklblItemDescriptionLocale.FlatStyle = (global::System.Windows.Forms.FlatStyle.Flat);
            this.fklblItemDescriptionLocale.Image = ((global::System.Drawing.Image)(resources.GetObject("fklblItemDescriptionLocale.Image")));
            this.fklblItemDescriptionLocale.ImageAlign = (global::System.Drawing.ContentAlignment.TopLeft);
            this.fklblItemDescriptionLocale.Location = (new global::System.Drawing.Point(13, 153));
            this.fklblItemDescriptionLocale.Name = ("fklblItemDescriptionLocale");
            this.fklblItemDescriptionLocale.Size = (new global::System.Drawing.Size(331, 42));
            this.fklblItemDescriptionLocale.TabIndex = (195);
            this.fklblItemDescriptionLocale.Text = ("This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.");
            this.fklblItemDescriptionLocale.TextAlign = (global::System.Drawing.ContentAlignment.MiddleLeft);
            this.fklblItemDescriptionLocale.TextImageRelation = (global::System.Windows.Forms.TextImageRelation.ImageBeforeText);
            this.fklblItemDescriptionLocale.UseVisualStyleBackColor = (true);
            this.fklblItemDescriptionLocale.Visible = (false);
            // 
            // txtItemDescription
            // 
            this.txtItemDescription.Location = (new global::System.Drawing.Point(13, 124));
            this.txtItemDescription.Name = ("txtItemDescription");
            this.txtItemDescription.Size = (new global::System.Drawing.Size(350, 23));
            this.txtItemDescription.TabIndex = (194);
            this.txtItemDescription.TextChanged += (this.txtItemDescription_TextChanged);
            // 
            // label105
            // 
            this.label105.AutoSize = (true);
            this.label105.Location = (new global::System.Drawing.Point(13, 106));
            this.label105.Name = ("label105");
            this.label105.Size = (new global::System.Drawing.Size(67, 15));
            this.label105.TabIndex = (193);
            this.label105.Text = ("Description");
            // 
            // fklblItemNameLocale
            // 
            this.fklblItemNameLocale.Enabled = (false);
            this.fklblItemNameLocale.FlatAppearance.BorderSize = (0);
            this.fklblItemNameLocale.FlatStyle = (global::System.Windows.Forms.FlatStyle.Flat);
            this.fklblItemNameLocale.Image = ((global::System.Drawing.Image)(resources.GetObject("fklblItemNameLocale.Image")));
            this.fklblItemNameLocale.ImageAlign = (global::System.Drawing.ContentAlignment.TopLeft);
            this.fklblItemNameLocale.Location = (new global::System.Drawing.Point(13, 55));
            this.fklblItemNameLocale.Name = ("fklblItemNameLocale");
            this.fklblItemNameLocale.Size = (new global::System.Drawing.Size(331, 42));
            this.fklblItemNameLocale.TabIndex = (192);
            this.fklblItemNameLocale.Text = ("This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.");
            this.fklblItemNameLocale.TextAlign = (global::System.Drawing.ContentAlignment.MiddleLeft);
            this.fklblItemNameLocale.TextImageRelation = (global::System.Windows.Forms.TextImageRelation.ImageBeforeText);
            this.fklblItemNameLocale.UseVisualStyleBackColor = (true);
            this.fklblItemNameLocale.Visible = (false);
            // 
            // txtItemName
            // 
            this.txtItemName.Location = (new global::System.Drawing.Point(13, 26));
            this.txtItemName.Name = ("txtItemName");
            this.txtItemName.Size = (new global::System.Drawing.Size(350, 23));
            this.txtItemName.TabIndex = (191);
            this.txtItemName.TextChanged += (this.txtItemName_TextChanged);
            // 
            // label106
            // 
            this.label106.AutoSize = (true);
            this.label106.Location = (new global::System.Drawing.Point(13, 8));
            this.label106.Name = ("label106");
            this.label106.Size = (new global::System.Drawing.Size(80, 15));
            this.label106.TabIndex = (190);
            this.label106.Text = ("Default Name");
            // 
            // tpTrap
            // 
            this.tpTrap.Controls.Add(this.btnTrapOnSteppedAction);
            this.tpTrap.Controls.Add(this.label112);
            this.tpTrap.Controls.Add(this.txtTrapPower);
            this.tpTrap.Controls.Add(this.label113);
            this.tpTrap.Controls.Add(this.chkTrapStartsVisible);
            this.tpTrap.Controls.Add(this.btnChangeTrapConsoleCharacterBackColor);
            this.tpTrap.Controls.Add(this.btnChangeTrapConsoleCharacterForeColor);
            this.tpTrap.Controls.Add(this.btnChangeTrapConsoleCharacter);
            this.tpTrap.Controls.Add(this.lblTrapConsoleRepresentation);
            this.tpTrap.Controls.Add(this.label116);
            this.tpTrap.Controls.Add(this.fklblTrapDescriptionLocale);
            this.tpTrap.Controls.Add(this.txtTrapDescription);
            this.tpTrap.Controls.Add(this.label117);
            this.tpTrap.Controls.Add(this.fklblTrapNameLocale);
            this.tpTrap.Controls.Add(this.txtTrapName);
            this.tpTrap.Controls.Add(this.label118);
            this.tpTrap.Location = (new global::System.Drawing.Point(4, 24));
            this.tpTrap.Name = ("tpTrap");
            this.tpTrap.Size = (new global::System.Drawing.Size(740, 356));
            this.tpTrap.TabIndex = (7);
            this.tpTrap.Text = ("Trap");
            this.tpTrap.UseVisualStyleBackColor = (true);
            // 
            // btnTrapOnSteppedAction
            // 
            this.btnTrapOnSteppedAction.Location = (new global::System.Drawing.Point(175, 276));
            this.btnTrapOnSteppedAction.Name = ("btnTrapOnSteppedAction");
            this.btnTrapOnSteppedAction.Size = (new global::System.Drawing.Size(75, 23));
            this.btnTrapOnSteppedAction.TabIndex = (238);
            this.btnTrapOnSteppedAction.Text = ("... do this!");
            this.btnTrapOnSteppedAction.UseVisualStyleBackColor = (true);
            this.btnTrapOnSteppedAction.Click += (this.btnTrapOnSteppedAction_Click);
            // 
            // label112
            // 
            this.label112.AutoSize = (true);
            this.label112.Font = (new global::System.Drawing.Font("Segoe UI", 9F, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point));
            this.label112.Location = (new global::System.Drawing.Point(13, 280));
            this.label112.Name = ("label112");
            this.label112.Size = (new global::System.Drawing.Size(156, 15));
            this.label112.TabIndex = (237);
            this.label112.Text = ("When someone steps on it...");
            // 
            // txtTrapPower
            // 
            this.txtTrapPower.Location = (new global::System.Drawing.Point(86, 209));
            this.txtTrapPower.Name = ("txtTrapPower");
            this.txtTrapPower.Size = (new global::System.Drawing.Size(150, 23));
            this.txtTrapPower.TabIndex = (236);
            this.txtTrapPower.Enter += (this.txtTrapPower_Enter);
            this.txtTrapPower.Leave += (this.txtTrapPower_Leave);
            // 
            // label113
            // 
            this.label113.AutoSize = (true);
            this.label113.Location = (new global::System.Drawing.Point(13, 215));
            this.label113.Name = ("label113");
            this.label113.Size = (new global::System.Drawing.Size(65, 15));
            this.label113.TabIndex = (235);
            this.label113.Text = ("Trap Power");
            // 
            // chkTrapStartsVisible
            // 
            this.chkTrapStartsVisible.AutoSize = (true);
            this.chkTrapStartsVisible.Location = (new global::System.Drawing.Point(13, 247));
            this.chkTrapStartsVisible.Name = ("chkTrapStartsVisible");
            this.chkTrapStartsVisible.Size = (new global::System.Drawing.Size(102, 19));
            this.chkTrapStartsVisible.TabIndex = (233);
            this.chkTrapStartsVisible.Text = ("Spawns visible");
            this.chkTrapStartsVisible.UseVisualStyleBackColor = (true);
            this.chkTrapStartsVisible.CheckedChanged += (this.chkTrapStartsVisible_CheckedChanged);
            // 
            // btnChangeTrapConsoleCharacterBackColor
            // 
            this.btnChangeTrapConsoleCharacterBackColor.Location = (new global::System.Drawing.Point(597, 67));
            this.btnChangeTrapConsoleCharacterBackColor.Name = ("btnChangeTrapConsoleCharacterBackColor");
            this.btnChangeTrapConsoleCharacterBackColor.Size = (new global::System.Drawing.Size(135, 23));
            this.btnChangeTrapConsoleCharacterBackColor.TabIndex = (230);
            this.btnChangeTrapConsoleCharacterBackColor.Text = ("Change Background...");
            this.btnChangeTrapConsoleCharacterBackColor.UseVisualStyleBackColor = (true);
            this.btnChangeTrapConsoleCharacterBackColor.Click += (this.btnChangeTrapConsoleCharacterBackColor_Click);
            // 
            // btnChangeTrapConsoleCharacterForeColor
            // 
            this.btnChangeTrapConsoleCharacterForeColor.Location = (new global::System.Drawing.Point(597, 38));
            this.btnChangeTrapConsoleCharacterForeColor.Name = ("btnChangeTrapConsoleCharacterForeColor");
            this.btnChangeTrapConsoleCharacterForeColor.Size = (new global::System.Drawing.Size(135, 23));
            this.btnChangeTrapConsoleCharacterForeColor.TabIndex = (229);
            this.btnChangeTrapConsoleCharacterForeColor.Text = ("Change Foreground...");
            this.btnChangeTrapConsoleCharacterForeColor.UseVisualStyleBackColor = (true);
            this.btnChangeTrapConsoleCharacterForeColor.Click += (this.btnChangeTrapConsoleCharacterForeColor_Click);
            // 
            // btnChangeTrapConsoleCharacter
            // 
            this.btnChangeTrapConsoleCharacter.Location = (new global::System.Drawing.Point(597, 9));
            this.btnChangeTrapConsoleCharacter.Name = ("btnChangeTrapConsoleCharacter");
            this.btnChangeTrapConsoleCharacter.Size = (new global::System.Drawing.Size(135, 23));
            this.btnChangeTrapConsoleCharacter.TabIndex = (228);
            this.btnChangeTrapConsoleCharacter.Text = ("Change Character...");
            this.btnChangeTrapConsoleCharacter.UseVisualStyleBackColor = (true);
            this.btnChangeTrapConsoleCharacter.Click += (this.btnChangeTrapConsoleCharacter_Click);
            // 
            // lblTrapConsoleRepresentation
            // 
            this.lblTrapConsoleRepresentation.Font = (new global::System.Drawing.Font("Courier New", 36F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point));
            this.lblTrapConsoleRepresentation.Location = (new global::System.Drawing.Point(524, 18));
            this.lblTrapConsoleRepresentation.Name = ("lblTrapConsoleRepresentation");
            this.lblTrapConsoleRepresentation.Size = (new global::System.Drawing.Size(64, 64));
            this.lblTrapConsoleRepresentation.TabIndex = (227);
            this.lblTrapConsoleRepresentation.TextAlign = (global::System.Drawing.ContentAlignment.MiddleCenter);
            // 
            // label116
            // 
            this.label116.Font = (new global::System.Drawing.Font("Segoe UI", 14.25F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point));
            this.label116.Location = (new global::System.Drawing.Point(390, 20));
            this.label116.Name = ("label116");
            this.label116.Size = (new global::System.Drawing.Size(131, 52));
            this.label116.TabIndex = (226);
            this.label116.Text = ("Appearance -");
            this.label116.TextAlign = (global::System.Drawing.ContentAlignment.MiddleCenter);
            // 
            // fklblTrapDescriptionLocale
            // 
            this.fklblTrapDescriptionLocale.Enabled = (false);
            this.fklblTrapDescriptionLocale.FlatAppearance.BorderSize = (0);
            this.fklblTrapDescriptionLocale.FlatStyle = (global::System.Windows.Forms.FlatStyle.Flat);
            this.fklblTrapDescriptionLocale.Image = ((global::System.Drawing.Image)(resources.GetObject("fklblTrapDescriptionLocale.Image")));
            this.fklblTrapDescriptionLocale.ImageAlign = (global::System.Drawing.ContentAlignment.TopLeft);
            this.fklblTrapDescriptionLocale.Location = (new global::System.Drawing.Point(13, 153));
            this.fklblTrapDescriptionLocale.Name = ("fklblTrapDescriptionLocale");
            this.fklblTrapDescriptionLocale.Size = (new global::System.Drawing.Size(331, 42));
            this.fklblTrapDescriptionLocale.TabIndex = (225);
            this.fklblTrapDescriptionLocale.Text = ("This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.");
            this.fklblTrapDescriptionLocale.TextAlign = (global::System.Drawing.ContentAlignment.MiddleLeft);
            this.fklblTrapDescriptionLocale.TextImageRelation = (global::System.Windows.Forms.TextImageRelation.ImageBeforeText);
            this.fklblTrapDescriptionLocale.UseVisualStyleBackColor = (true);
            this.fklblTrapDescriptionLocale.Visible = (false);
            // 
            // txtTrapDescription
            // 
            this.txtTrapDescription.Location = (new global::System.Drawing.Point(13, 124));
            this.txtTrapDescription.Name = ("txtTrapDescription");
            this.txtTrapDescription.Size = (new global::System.Drawing.Size(350, 23));
            this.txtTrapDescription.TabIndex = (224);
            this.txtTrapDescription.TextChanged += (this.txtTrapDescription_TextChanged);
            // 
            // label117
            // 
            this.label117.AutoSize = (true);
            this.label117.Location = (new global::System.Drawing.Point(13, 106));
            this.label117.Name = ("label117");
            this.label117.Size = (new global::System.Drawing.Size(67, 15));
            this.label117.TabIndex = (223);
            this.label117.Text = ("Description");
            // 
            // fklblTrapNameLocale
            // 
            this.fklblTrapNameLocale.Enabled = (false);
            this.fklblTrapNameLocale.FlatAppearance.BorderSize = (0);
            this.fklblTrapNameLocale.FlatStyle = (global::System.Windows.Forms.FlatStyle.Flat);
            this.fklblTrapNameLocale.Image = ((global::System.Drawing.Image)(resources.GetObject("fklblTrapNameLocale.Image")));
            this.fklblTrapNameLocale.ImageAlign = (global::System.Drawing.ContentAlignment.TopLeft);
            this.fklblTrapNameLocale.Location = (new global::System.Drawing.Point(13, 55));
            this.fklblTrapNameLocale.Name = ("fklblTrapNameLocale");
            this.fklblTrapNameLocale.Size = (new global::System.Drawing.Size(331, 42));
            this.fklblTrapNameLocale.TabIndex = (222);
            this.fklblTrapNameLocale.Text = ("This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.");
            this.fklblTrapNameLocale.TextAlign = (global::System.Drawing.ContentAlignment.MiddleLeft);
            this.fklblTrapNameLocale.TextImageRelation = (global::System.Windows.Forms.TextImageRelation.ImageBeforeText);
            this.fklblTrapNameLocale.UseVisualStyleBackColor = (true);
            this.fklblTrapNameLocale.Visible = (false);
            // 
            // txtTrapName
            // 
            this.txtTrapName.Location = (new global::System.Drawing.Point(13, 26));
            this.txtTrapName.Name = ("txtTrapName");
            this.txtTrapName.Size = (new global::System.Drawing.Size(350, 23));
            this.txtTrapName.TabIndex = (221);
            this.txtTrapName.TextChanged += (this.txtTrapName_TextChanged);
            // 
            // label118
            // 
            this.label118.AutoSize = (true);
            this.label118.Location = (new global::System.Drawing.Point(13, 8));
            this.label118.Name = ("label118");
            this.label118.Size = (new global::System.Drawing.Size(80, 15));
            this.label118.TabIndex = (220);
            this.label118.Text = ("Default Name");
            // 
            // tpAlteredStatus
            // 
            this.tpAlteredStatus.Controls.Add(this.btnAlteredStatusOnTurnStartAction);
            this.tpAlteredStatus.Controls.Add(this.label109);
            this.tpAlteredStatus.Controls.Add(this.chkAlteredStatusCleansedOnCleanseActions);
            this.tpAlteredStatus.Controls.Add(this.chkAlteredStatusCleanseOnFloorChange);
            this.tpAlteredStatus.Controls.Add(this.chkAlteredStatusCanOverwrite);
            this.tpAlteredStatus.Controls.Add(this.btnAlteredStatusOnApplyAction);
            this.tpAlteredStatus.Controls.Add(this.label97);
            this.tpAlteredStatus.Controls.Add(this.chkAlteredStatusCanStack);
            this.tpAlteredStatus.Controls.Add(this.btnChangeAlteredStatusConsoleCharacterBackColor);
            this.tpAlteredStatus.Controls.Add(this.btnChangeAlteredStatusConsoleCharacterForeColor);
            this.tpAlteredStatus.Controls.Add(this.btnChangeAlteredStatusConsoleCharacter);
            this.tpAlteredStatus.Controls.Add(this.lblAlteredStatusConsoleRepresentation);
            this.tpAlteredStatus.Controls.Add(this.label111);
            this.tpAlteredStatus.Controls.Add(this.fklblAlteredStatusDescriptionLocale);
            this.tpAlteredStatus.Controls.Add(this.txtAlteredStatusDescription);
            this.tpAlteredStatus.Controls.Add(this.label114);
            this.tpAlteredStatus.Controls.Add(this.fklblAlteredStatusNameLocale);
            this.tpAlteredStatus.Controls.Add(this.txtAlteredStatusName);
            this.tpAlteredStatus.Controls.Add(this.label115);
            this.tpAlteredStatus.Location = (new global::System.Drawing.Point(4, 24));
            this.tpAlteredStatus.Name = ("tpAlteredStatus");
            this.tpAlteredStatus.Size = (new global::System.Drawing.Size(740, 356));
            this.tpAlteredStatus.TabIndex = (8);
            this.tpAlteredStatus.Text = ("Altered Status");
            this.tpAlteredStatus.UseVisualStyleBackColor = (true);
            // 
            // btnAlteredStatusOnTurnStartAction
            // 
            this.btnAlteredStatusOnTurnStartAction.Location = (new global::System.Drawing.Point(657, 153));
            this.btnAlteredStatusOnTurnStartAction.Name = ("btnAlteredStatusOnTurnStartAction");
            this.btnAlteredStatusOnTurnStartAction.Size = (new global::System.Drawing.Size(75, 23));
            this.btnAlteredStatusOnTurnStartAction.TabIndex = (259);
            this.btnAlteredStatusOnTurnStartAction.Text = ("... do this!");
            this.btnAlteredStatusOnTurnStartAction.UseVisualStyleBackColor = (true);
            this.btnAlteredStatusOnTurnStartAction.Click += (this.btnAlteredStatusOnTurnStartAction_Click);
            // 
            // label109
            // 
            this.label109.AutoSize = (true);
            this.label109.Font = (new global::System.Drawing.Font("Segoe UI", 9F, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point));
            this.label109.Location = (new global::System.Drawing.Point(389, 157));
            this.label109.Name = ("label109");
            this.label109.Size = (new global::System.Drawing.Size(268, 15));
            this.label109.TabIndex = (258);
            this.label109.Text = ("When someone afflicted by it begins a new turn...");
            // 
            // chkAlteredStatusCleansedOnCleanseActions
            // 
            this.chkAlteredStatusCleansedOnCleanseActions.AutoSize = (true);
            this.chkAlteredStatusCleansedOnCleanseActions.Location = (new global::System.Drawing.Point(13, 291));
            this.chkAlteredStatusCleansedOnCleanseActions.Name = ("chkAlteredStatusCleansedOnCleanseActions");
            this.chkAlteredStatusCleansedOnCleanseActions.Size = (new global::System.Drawing.Size(247, 19));
            this.chkAlteredStatusCleansedOnCleanseActions.TabIndex = (257);
            this.chkAlteredStatusCleansedOnCleanseActions.Text = ("Can be removed by 'Cleanse' Action steps");
            this.chkAlteredStatusCleansedOnCleanseActions.UseVisualStyleBackColor = (true);
            this.chkAlteredStatusCleansedOnCleanseActions.CheckedChanged += (this.chkAlteredStatusCleansedOnCleanseActions_CheckedChanged);
            // 
            // chkAlteredStatusCleanseOnFloorChange
            // 
            this.chkAlteredStatusCleanseOnFloorChange.AutoSize = (true);
            this.chkAlteredStatusCleanseOnFloorChange.Location = (new global::System.Drawing.Point(13, 266));
            this.chkAlteredStatusCleanseOnFloorChange.Name = ("chkAlteredStatusCleanseOnFloorChange");
            this.chkAlteredStatusCleanseOnFloorChange.Size = (new global::System.Drawing.Size(330, 19));
            this.chkAlteredStatusCleanseOnFloorChange.TabIndex = (256);
            this.chkAlteredStatusCleanseOnFloorChange.Text = ("Is removed if the afflicted Character moves to a new Floor");
            this.chkAlteredStatusCleanseOnFloorChange.UseVisualStyleBackColor = (true);
            this.chkAlteredStatusCleanseOnFloorChange.CheckedChanged += (this.chkAlteredStatusCleanseOnFloorChange_CheckedChanged);
            // 
            // chkAlteredStatusCanOverwrite
            // 
            this.chkAlteredStatusCanOverwrite.AutoSize = (true);
            this.chkAlteredStatusCanOverwrite.Location = (new global::System.Drawing.Point(13, 241));
            this.chkAlteredStatusCanOverwrite.Name = ("chkAlteredStatusCanOverwrite");
            this.chkAlteredStatusCanOverwrite.Size = (new global::System.Drawing.Size(342, 19));
            this.chkAlteredStatusCanOverwrite.TabIndex = (255);
            this.chkAlteredStatusCanOverwrite.Text = ("Overwrites other Altered Statuses with the same Id if applied");
            this.chkAlteredStatusCanOverwrite.UseVisualStyleBackColor = (true);
            this.chkAlteredStatusCanOverwrite.CheckedChanged += (this.chkAlteredStatusCanOverwrite_CheckedChanged);
            // 
            // btnAlteredStatusOnApplyAction
            // 
            this.btnAlteredStatusOnApplyAction.Location = (new global::System.Drawing.Point(657, 124));
            this.btnAlteredStatusOnApplyAction.Name = ("btnAlteredStatusOnApplyAction");
            this.btnAlteredStatusOnApplyAction.Size = (new global::System.Drawing.Size(75, 23));
            this.btnAlteredStatusOnApplyAction.TabIndex = (254);
            this.btnAlteredStatusOnApplyAction.Text = ("... do this!");
            this.btnAlteredStatusOnApplyAction.UseVisualStyleBackColor = (true);
            this.btnAlteredStatusOnApplyAction.Click += (this.btnAlteredStatusOnApplyAction_Click);
            // 
            // label97
            // 
            this.label97.AutoSize = (true);
            this.label97.Font = (new global::System.Drawing.Font("Segoe UI", 9F, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point));
            this.label97.Location = (new global::System.Drawing.Point(389, 128));
            this.label97.Name = ("label97");
            this.label97.Size = (new global::System.Drawing.Size(268, 15));
            this.label97.TabIndex = (253);
            this.label97.Text = ("When someone gets this Altered Status inflicted...");
            // 
            // chkAlteredStatusCanStack
            // 
            this.chkAlteredStatusCanStack.AutoSize = (true);
            this.chkAlteredStatusCanStack.Location = (new global::System.Drawing.Point(13, 216));
            this.chkAlteredStatusCanStack.Name = ("chkAlteredStatusCanStack");
            this.chkAlteredStatusCanStack.Size = (new global::System.Drawing.Size(311, 19));
            this.chkAlteredStatusCanStack.TabIndex = (250);
            this.chkAlteredStatusCanStack.Text = ("Can stack with other Altered Statuses with the same Id");
            this.chkAlteredStatusCanStack.UseVisualStyleBackColor = (true);
            this.chkAlteredStatusCanStack.CheckedChanged += (this.chkAlteredStatusCanStack_CheckedChanged);
            // 
            // btnChangeAlteredStatusConsoleCharacterBackColor
            // 
            this.btnChangeAlteredStatusConsoleCharacterBackColor.Location = (new global::System.Drawing.Point(597, 67));
            this.btnChangeAlteredStatusConsoleCharacterBackColor.Name = ("btnChangeAlteredStatusConsoleCharacterBackColor");
            this.btnChangeAlteredStatusConsoleCharacterBackColor.Size = (new global::System.Drawing.Size(135, 23));
            this.btnChangeAlteredStatusConsoleCharacterBackColor.TabIndex = (249);
            this.btnChangeAlteredStatusConsoleCharacterBackColor.Text = ("Change Background...");
            this.btnChangeAlteredStatusConsoleCharacterBackColor.UseVisualStyleBackColor = (true);
            this.btnChangeAlteredStatusConsoleCharacterBackColor.Click += (this.btnChangeAlteredStatusConsoleCharacterBackColor_Click);
            // 
            // btnChangeAlteredStatusConsoleCharacterForeColor
            // 
            this.btnChangeAlteredStatusConsoleCharacterForeColor.Location = (new global::System.Drawing.Point(597, 38));
            this.btnChangeAlteredStatusConsoleCharacterForeColor.Name = ("btnChangeAlteredStatusConsoleCharacterForeColor");
            this.btnChangeAlteredStatusConsoleCharacterForeColor.Size = (new global::System.Drawing.Size(135, 23));
            this.btnChangeAlteredStatusConsoleCharacterForeColor.TabIndex = (248);
            this.btnChangeAlteredStatusConsoleCharacterForeColor.Text = ("Change Foreground...");
            this.btnChangeAlteredStatusConsoleCharacterForeColor.UseVisualStyleBackColor = (true);
            this.btnChangeAlteredStatusConsoleCharacterForeColor.Click += (this.btnChangeAlteredStatusConsoleCharacterForeColor_Click);
            // 
            // btnChangeAlteredStatusConsoleCharacter
            // 
            this.btnChangeAlteredStatusConsoleCharacter.Location = (new global::System.Drawing.Point(597, 9));
            this.btnChangeAlteredStatusConsoleCharacter.Name = ("btnChangeAlteredStatusConsoleCharacter");
            this.btnChangeAlteredStatusConsoleCharacter.Size = (new global::System.Drawing.Size(135, 23));
            this.btnChangeAlteredStatusConsoleCharacter.TabIndex = (247);
            this.btnChangeAlteredStatusConsoleCharacter.Text = ("Change Character...");
            this.btnChangeAlteredStatusConsoleCharacter.UseVisualStyleBackColor = (true);
            this.btnChangeAlteredStatusConsoleCharacter.Click += (this.btnChangeAlteredStatusConsoleCharacter_Click);
            // 
            // lblAlteredStatusConsoleRepresentation
            // 
            this.lblAlteredStatusConsoleRepresentation.Font = (new global::System.Drawing.Font("Courier New", 36F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point));
            this.lblAlteredStatusConsoleRepresentation.Location = (new global::System.Drawing.Point(524, 18));
            this.lblAlteredStatusConsoleRepresentation.Name = ("lblAlteredStatusConsoleRepresentation");
            this.lblAlteredStatusConsoleRepresentation.Size = (new global::System.Drawing.Size(64, 64));
            this.lblAlteredStatusConsoleRepresentation.TabIndex = (246);
            this.lblAlteredStatusConsoleRepresentation.TextAlign = (global::System.Drawing.ContentAlignment.MiddleCenter);
            // 
            // label111
            // 
            this.label111.Font = (new global::System.Drawing.Font("Segoe UI", 14.25F, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point));
            this.label111.Location = (new global::System.Drawing.Point(390, 20));
            this.label111.Name = ("label111");
            this.label111.Size = (new global::System.Drawing.Size(131, 52));
            this.label111.TabIndex = (245);
            this.label111.Text = ("Appearance -");
            this.label111.TextAlign = (global::System.Drawing.ContentAlignment.MiddleCenter);
            // 
            // fklblAlteredStatusDescriptionLocale
            // 
            this.fklblAlteredStatusDescriptionLocale.Enabled = (false);
            this.fklblAlteredStatusDescriptionLocale.FlatAppearance.BorderSize = (0);
            this.fklblAlteredStatusDescriptionLocale.FlatStyle = (global::System.Windows.Forms.FlatStyle.Flat);
            this.fklblAlteredStatusDescriptionLocale.Image = ((global::System.Drawing.Image)(resources.GetObject("fklblAlteredStatusDescriptionLocale.Image")));
            this.fklblAlteredStatusDescriptionLocale.ImageAlign = (global::System.Drawing.ContentAlignment.TopLeft);
            this.fklblAlteredStatusDescriptionLocale.Location = (new global::System.Drawing.Point(13, 153));
            this.fklblAlteredStatusDescriptionLocale.Name = ("fklblAlteredStatusDescriptionLocale");
            this.fklblAlteredStatusDescriptionLocale.Size = (new global::System.Drawing.Size(331, 42));
            this.fklblAlteredStatusDescriptionLocale.TabIndex = (244);
            this.fklblAlteredStatusDescriptionLocale.Text = ("This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.");
            this.fklblAlteredStatusDescriptionLocale.TextAlign = (global::System.Drawing.ContentAlignment.MiddleLeft);
            this.fklblAlteredStatusDescriptionLocale.TextImageRelation = (global::System.Windows.Forms.TextImageRelation.ImageBeforeText);
            this.fklblAlteredStatusDescriptionLocale.UseVisualStyleBackColor = (true);
            this.fklblAlteredStatusDescriptionLocale.Visible = (false);
            // 
            // txtAlteredStatusDescription
            // 
            this.txtAlteredStatusDescription.Location = (new global::System.Drawing.Point(13, 124));
            this.txtAlteredStatusDescription.Name = ("txtAlteredStatusDescription");
            this.txtAlteredStatusDescription.Size = (new global::System.Drawing.Size(350, 23));
            this.txtAlteredStatusDescription.TabIndex = (243);
            this.txtAlteredStatusDescription.TextChanged += (this.txtAlteredStatusDescription_TextChanged);
            // 
            // label114
            // 
            this.label114.AutoSize = (true);
            this.label114.Location = (new global::System.Drawing.Point(13, 106));
            this.label114.Name = ("label114");
            this.label114.Size = (new global::System.Drawing.Size(67, 15));
            this.label114.TabIndex = (242);
            this.label114.Text = ("Description");
            // 
            // fklblAlteredStatusNameLocale
            // 
            this.fklblAlteredStatusNameLocale.Enabled = (false);
            this.fklblAlteredStatusNameLocale.FlatAppearance.BorderSize = (0);
            this.fklblAlteredStatusNameLocale.FlatStyle = (global::System.Windows.Forms.FlatStyle.Flat);
            this.fklblAlteredStatusNameLocale.Image = ((global::System.Drawing.Image)(resources.GetObject("fklblAlteredStatusNameLocale.Image")));
            this.fklblAlteredStatusNameLocale.ImageAlign = (global::System.Drawing.ContentAlignment.TopLeft);
            this.fklblAlteredStatusNameLocale.Location = (new global::System.Drawing.Point(13, 55));
            this.fklblAlteredStatusNameLocale.Name = ("fklblAlteredStatusNameLocale");
            this.fklblAlteredStatusNameLocale.Size = (new global::System.Drawing.Size(331, 42));
            this.fklblAlteredStatusNameLocale.TabIndex = (241);
            this.fklblAlteredStatusNameLocale.Text = ("This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.");
            this.fklblAlteredStatusNameLocale.TextAlign = (global::System.Drawing.ContentAlignment.MiddleLeft);
            this.fklblAlteredStatusNameLocale.TextImageRelation = (global::System.Windows.Forms.TextImageRelation.ImageBeforeText);
            this.fklblAlteredStatusNameLocale.UseVisualStyleBackColor = (true);
            this.fklblAlteredStatusNameLocale.Visible = (false);
            // 
            // txtAlteredStatusName
            // 
            this.txtAlteredStatusName.Location = (new global::System.Drawing.Point(13, 26));
            this.txtAlteredStatusName.Name = ("txtAlteredStatusName");
            this.txtAlteredStatusName.Size = (new global::System.Drawing.Size(350, 23));
            this.txtAlteredStatusName.TabIndex = (240);
            this.txtAlteredStatusName.TextChanged += (this.txtAlteredStatusName_TextChanged);
            // 
            // label115
            // 
            this.label115.AutoSize = (true);
            this.label115.Location = (new global::System.Drawing.Point(13, 8));
            this.label115.Name = ("label115");
            this.label115.Size = (new global::System.Drawing.Size(80, 15));
            this.label115.TabIndex = (239);
            this.label115.Text = ("Default Name");
            // 
            // tpValidation
            // 
            this.tpValidation.Controls.Add(this.tvValidationResults);
            this.tpValidation.Location = (new global::System.Drawing.Point(4, 24));
            this.tpValidation.Name = ("tpValidation");
            this.tpValidation.Size = (new global::System.Drawing.Size(740, 356));
            this.tpValidation.TabIndex = (9);
            this.tpValidation.Text = ("Validation Results");
            this.tpValidation.UseVisualStyleBackColor = (true);
            // 
            // tvValidationResults
            // 
            this.tvValidationResults.Dock = (global::System.Windows.Forms.DockStyle.Fill);
            this.tvValidationResults.Location = (new global::System.Drawing.Point(0, 0));
            this.tvValidationResults.Name = ("tvValidationResults");
            this.tvValidationResults.Size = (new global::System.Drawing.Size(740, 356));
            this.tvValidationResults.TabIndex = (0);
            // 
            // ofdDungeon
            // 
            this.ofdDungeon.Filter = ("Dungeon JSON|*.json");
            this.ofdDungeon.Title = ("Select a Dungeon JSON file");
            // 
            // sfdDungeon
            // 
            this.sfdDungeon.Filter = ("Dungeon JSON|*.json");
            this.sfdDungeon.Title = ("Set a Dungeon JSON file name to save");
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = (new global::System.Drawing.SizeF(7F, 15F));
            this.AutoScaleMode = (global::System.Windows.Forms.AutoScaleMode.Font);
            this.ClientSize = (new global::System.Drawing.Size(967, 450));
            this.Controls.Add(this.tbTabs);
            this.Controls.Add(this.tvDungeonInfo);
            this.Controls.Add(this.tsButtons);
            this.Controls.Add(this.msMenu);
            this.Icon = ((global::System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = (this.msMenu);
            this.MaximizeBox = (false);
            this.Name = ("frmMain");
            this.StartPosition = (global::System.Windows.Forms.FormStartPosition.CenterScreen);
            this.Text = ("Rogue Customs Dungeon Editor");
            this.msMenu.ResumeLayout(false);
            this.msMenu.PerformLayout();
            this.tsButtons.ResumeLayout(false);
            this.tsButtons.PerformLayout();
            this.tbTabs.ResumeLayout(false);
            this.tpBasicInfo.ResumeLayout(false);
            this.tpBasicInfo.PerformLayout();
            this.tpLocales.ResumeLayout(false);
            this.tpLocales.PerformLayout();
            ((global::System.ComponentModel.ISupportInitialize)(this.dgvLocales)).EndInit();
            this.tpFloorInfos.ResumeLayout(false);
            this.tpFloorInfos.PerformLayout();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudRoomFusionOdds)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudExtraRoomConnectionOdds)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudMaxRoomConnections)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudMaxFloorLevel)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudMinFloorLevel)).EndInit();
            this.tpFactionInfos.ResumeLayout(false);
            this.tpFactionInfos.PerformLayout();
            this.tpPlayerClass.ResumeLayout(false);
            this.tpPlayerClass.PerformLayout();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudPlayerInventorySize)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudPlayerMaxLevel)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudPlayerFlatSightRange)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudPlayerHPRegenerationPerLevelUp)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudPlayerBaseHPRegeneration)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudPlayerMovementPerLevelUp)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudPlayerBaseMovement)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudPlayerDefensePerLevelUp)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudPlayerBaseDefense)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudPlayerAttackPerLevelUp)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudPlayerBaseAttack)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudPlayerHPPerLevelUp)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudPlayerBaseHP)).EndInit();
            this.tpNPC.ResumeLayout(false);
            this.tpNPC.PerformLayout();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudNPCOddsToTargetSelf)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudNPCInventorySize)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudNPCMaxLevel)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudNPCFlatSightRange)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudNPCHPRegenerationPerLevelUp)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudNPCBaseHPRegeneration)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudNPCMovementPerLevelUp)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudNPCBaseMovement)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudNPCDefensePerLevelUp)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudNPCBaseDefense)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudNPCAttackPerLevelUp)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudNPCBaseAttack)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudNPCHPPerLevelUp)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.nudNPCBaseHP)).EndInit();
            this.tpItem.ResumeLayout(false);
            this.tpItem.PerformLayout();
            this.tpTrap.ResumeLayout(false);
            this.tpTrap.PerformLayout();
            this.tpAlteredStatus.ResumeLayout(false);
            this.tpAlteredStatus.PerformLayout();
            this.tpValidation.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private MenuStrip msMenu;
        private ToolStripMenuItem editorToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStrip tsButtons;
        private ToolStripButton tsbNewDungeon;
        private ToolStripButton tsbOpenDungeon;
        private ToolStripButton tsbSaveDungeon;
        private ToolStripButton tsbSaveDungeonAs;
        private ToolStripSeparator tssDungeonElement;
        private ToolStripButton tsbValidateDungeon;
        private TreeView tvDungeonInfo;
        private TabControl tbTabs;
        private TabPage tpBasicInfo;
        private TabPage tpLocales;
        private TabPage tpFloorInfos;
        private TabPage tpFactionInfos;
        private TabPage tpPlayerClass;
        private TabPage tpNPC;
        private TabPage tpItem;
        private TabPage tpTrap;
        private TabPage tpAlteredStatus;
        private TabPage tpValidation;
        private OpenFileDialog ofdDungeon;
        private TextBox txtEndingMessage;
        private Label label4;
        private TextBox txtWelcomeMessage;
        private Label label3;
        private TextBox txtAuthor;
        private Label label2;
        private TextBox txtDungeonName;
        private Label label1;
        private Button fklblEndingMessageLocale;
        private Button fklblWelcomeMessageLocale;
        private Button fklblAuthorLocale;
        private Button fklblDungeonNameLocale;
        private Button btnDeleteLocale;
        private Button btnAddLocale;
        private Button btnUpdateLocale;
        private Button fklblMissingLocale;
        private TextBox txtLocaleEntryValue;
        private Label label7;
        private TextBox txtLocaleEntryKey;
        private Label label6;
        private DataGridView dgvLocales;
        private DataGridViewTextBoxColumn cmKey;
        private DataGridViewTextBoxColumn cmValue;
        private ToolStripButton tsbAddElement;
        private ToolStripButton tsbSaveElement;
        private ToolStripButton tsbSaveElementAs;
        private ToolStripButton tsbDeleteElement;
        private ToolStripSeparator tssElementValidate;
        private ComboBox cmbDefaultLocale;
        private Label label8;
        private NumericUpDown nudMaxFloorLevel;
        private Label label10;
        private NumericUpDown nudMinFloorLevel;
        private Label label9;
        private Button fklblStairsReminder;
        private CheckBox chkGenerateStairsOnStart;
        private Button btnNPCGenerator;
        private Label label13;
        private NumericUpDown nudHeight;
        private NumericUpDown nudWidth;
        private Label label12;
        private Label label11;
        private Button btnItemGenerator;
        private Label label14;
        private Button btnTrapGenerator;
        private Label label15;
        private ListView lvFloorAlgorithms;
        private Label label17;
        private Label label16;
        private Button btnRemoveAlgorithm;
        private Button btnEditAlgorithm;
        private Button btnAddAlgorithm;
        private Button btnOnFloorStartAction;
        private Label label20;
        private NumericUpDown nudRoomFusionOdds;
        private Label label19;
        private NumericUpDown nudExtraRoomConnectionOdds;
        private Label label18;
        private NumericUpDown nudMaxRoomConnections;
        private Label label5;
        private ListBox lbEnemies;
        private Label label26;
        private Button btnEnemiesToNeutrals;
        private Button btnEnemyToNeutral;
        private Button btnNeutralsToEnemies;
        private Button btnNeutralToEnemy;
        private ListBox lbNeutrals;
        private Label label25;
        private Button btnNeutralsToAllies;
        private Button btnNeutralToAlly;
        private Button btnAlliesToNeutrals;
        private Button btnAllyToNeutral;
        private ListBox lbAllies;
        private Label label24;
        private Label label23;
        private Button fklblFactionDescriptionLocale;
        private TextBox txtFactionDescription;
        private Label label22;
        private Button fklblFactionNameLocale;
        private TextBox txtFactionName;
        private Label label21;
        private Button fklblPlayerClassDescriptionLocale;
        private TextBox txtPlayerClassDescription;
        private Label label28;
        private Button fklblPlayerClassNameLocale;
        private TextBox txtPlayerClassName;
        private Label label27;
        private ComboBox cmbPlayerFaction;
        private Label label29;
        private CheckBox chkRequirePlayerPrompt;
        private CheckBox chkPlayerStartsVisible;
        private Button btnChangePlayerConsoleCharacterForeColor;
        private Button btnChangePlayerConsoleCharacter;
        private Label lblPlayerConsoleRepresentation;
        private Label label30;
        private Button btnRemovePlayerOnAttackAction;
        private Button btnEditPlayerOnAttackAction;
        private Button btnAddPlayerOnAttackAction;
        private ListBox lbPlayerOnAttackActions;
        private Label label59;
        private ComboBox cmbPlayerStartingArmor;
        private Label label57;
        private ComboBox cmbPlayerStartingWeapon;
        private Label label56;
        private ListBox lbPlayerStartingInventory;
        private Button btnPlayerRemoveItem;
        private Button btnPlayerAddItem;
        private ComboBox cmbPlayerInventoryItemChoices;
        private Label label55;
        private Label label54;
        private NumericUpDown nudPlayerInventorySize;
        private Label label53;
        private Label label52;
        private Label label47;
        private Label label51;
        private CheckBox chkPlayerCanGainExperience;
        private NumericUpDown nudPlayerMaxLevel;
        private Label label50;
        private TextBox txtPlayerLevelUpFormula;
        private Label label49;
        private Label label48;
        private NumericUpDown nudPlayerFlatSightRange;
        private ComboBox cmbPlayerSightRange;
        private Label label43;
        private Label label44;
        private Label label45;
        private Label label46;
        private NumericUpDown nudPlayerHPRegenerationPerLevelUp;
        private NumericUpDown nudPlayerBaseHPRegeneration;
        private Label label42;
        private Label label41;
        private Label label40;
        private Label label39;
        private NumericUpDown nudPlayerMovementPerLevelUp;
        private NumericUpDown nudPlayerBaseMovement;
        private Label label37;
        private NumericUpDown nudPlayerDefensePerLevelUp;
        private Label label38;
        private NumericUpDown nudPlayerBaseDefense;
        private Label label36;
        private Label label34;
        private NumericUpDown nudPlayerAttackPerLevelUp;
        private Label label35;
        private NumericUpDown nudPlayerBaseAttack;
        private Label label33;
        private NumericUpDown nudPlayerHPPerLevelUp;
        private Label label32;
        private NumericUpDown nudPlayerBaseHP;
        private Label label31;
        private Label lblPlayerSightRangeText;
        private Label label62;
        private Button btnPlayerOnDeathAction;
        private Label label63;
        private Button btnPlayerOnAttackedAction;
        private Label label61;
        private Button btnPlayerOnTurnStartAction;
        private Label label60;
        private Label label58;
        private Button btnChangePlayerConsoleCharacterBackColor;
        private global::System.Windows.Forms.TextBox txtNPCExperiencePayout;
        private global::System.Windows.Forms.Label label103;
        private global::System.Windows.Forms.CheckBox chkNPCKnowsAllCharacterPositions;
        private global::System.Windows.Forms.Button btnChangeNPCConsoleCharacterBackColor;
        private global::System.Windows.Forms.Button btnNPCOnDeathAction;
        private global::System.Windows.Forms.Label label64;
        private global::System.Windows.Forms.Button btnNPCOnAttackedAction;
        private global::System.Windows.Forms.Label label65;
        private global::System.Windows.Forms.Button btnNPCOnTurnStartAction;
        private global::System.Windows.Forms.Label label66;
        private global::System.Windows.Forms.Label label67;
        private global::System.Windows.Forms.Label label68;
        private global::System.Windows.Forms.Button btnRemoveNPCOnAttackAction;
        private global::System.Windows.Forms.Button btnEditNPCOnAttackAction;
        private global::System.Windows.Forms.Button btnAddNPCOnAttackAction;
        private global::System.Windows.Forms.ListBox lbNPCOnAttackActions;
        private global::System.Windows.Forms.Label label69;
        private global::System.Windows.Forms.ComboBox cmbNPCStartingArmor;
        private global::System.Windows.Forms.Label label70;
        private global::System.Windows.Forms.ComboBox cmbNPCStartingWeapon;
        private global::System.Windows.Forms.Label label71;
        private global::System.Windows.Forms.ListBox lbNPCStartingInventory;
        private global::System.Windows.Forms.Button btnNPCRemoveItem;
        private global::System.Windows.Forms.Button btnNPCAddItem;
        private global::System.Windows.Forms.ComboBox cmbNPCInventoryItemChoices;
        private global::System.Windows.Forms.Label label72;
        private global::System.Windows.Forms.Label label73;
        private global::System.Windows.Forms.NumericUpDown nudNPCInventorySize;
        private global::System.Windows.Forms.Label label74;
        private global::System.Windows.Forms.Label label75;
        private global::System.Windows.Forms.Label label76;
        private global::System.Windows.Forms.Label label77;
        private global::System.Windows.Forms.CheckBox chkNPCCanGainExperience;
        private global::System.Windows.Forms.NumericUpDown nudNPCMaxLevel;
        private global::System.Windows.Forms.Label label78;
        private global::System.Windows.Forms.TextBox txtNPCLevelUpFormula;
        private global::System.Windows.Forms.Label label79;
        private global::System.Windows.Forms.Label label80;
        private global::System.Windows.Forms.NumericUpDown nudNPCFlatSightRange;
        private global::System.Windows.Forms.ComboBox cmbNPCSightRange;
        private global::System.Windows.Forms.Label label81;
        private global::System.Windows.Forms.Label label82;
        private global::System.Windows.Forms.Label label83;
        private global::System.Windows.Forms.Label label84;
        private global::System.Windows.Forms.NumericUpDown nudNPCHPRegenerationPerLevelUp;
        private global::System.Windows.Forms.NumericUpDown nudNPCBaseHPRegeneration;
        private global::System.Windows.Forms.Label label85;
        private global::System.Windows.Forms.Label label86;
        private global::System.Windows.Forms.Label label87;
        private global::System.Windows.Forms.Label label88;
        private global::System.Windows.Forms.NumericUpDown nudNPCMovementPerLevelUp;
        private global::System.Windows.Forms.NumericUpDown nudNPCBaseMovement;
        private global::System.Windows.Forms.Label label89;
        private global::System.Windows.Forms.NumericUpDown nudNPCDefensePerLevelUp;
        private global::System.Windows.Forms.Label label90;
        private global::System.Windows.Forms.NumericUpDown nudNPCBaseDefense;
        private global::System.Windows.Forms.Label label91;
        private global::System.Windows.Forms.Label label92;
        private global::System.Windows.Forms.NumericUpDown nudNPCAttackPerLevelUp;
        private global::System.Windows.Forms.Label label93;
        private global::System.Windows.Forms.NumericUpDown nudNPCBaseAttack;
        private global::System.Windows.Forms.Label label94;
        private global::System.Windows.Forms.NumericUpDown nudNPCHPPerLevelUp;
        private global::System.Windows.Forms.Label label95;
        private global::System.Windows.Forms.NumericUpDown nudNPCBaseHP;
        private global::System.Windows.Forms.Label label96;
        private global::System.Windows.Forms.Button btnChangeNPCConsoleCharacterForeColor;
        private global::System.Windows.Forms.Button btnChangeNPCConsoleCharacter;
        private global::System.Windows.Forms.Label lblNPCConsoleRepresentation;
        private global::System.Windows.Forms.Label label98;
        private global::System.Windows.Forms.CheckBox chkNPCStartsVisible;
        private global::System.Windows.Forms.ComboBox cmbNPCFaction;
        private global::System.Windows.Forms.Label label99;
        private global::System.Windows.Forms.CheckBox chkAlteredStatusCleanseOnFloorChange;
        private global::System.Windows.Forms.Button fklblNPCNameLocale;
        private global::System.Windows.Forms.TextBox txtNPCDescription;
        private global::System.Windows.Forms.Label label100;
        private global::System.Windows.Forms.Button fklblNPCDescriptionLocale;
        private global::System.Windows.Forms.TextBox txtNPCName;
        private global::System.Windows.Forms.Label label101;
        private global::System.Windows.Forms.Label lblNPCSightRangeText;
        private global::System.Windows.Forms.NumericUpDown nudNPCOddsToTargetSelf;
        private global::System.Windows.Forms.Label label104;
        private global::System.Windows.Forms.TextBox txtItemPower;
        private global::System.Windows.Forms.Label label108;
        private global::System.Windows.Forms.CheckBox chkItemCanBePickedUp;
        private global::System.Windows.Forms.CheckBox chkItemStartsVisible;
        private global::System.Windows.Forms.ComboBox cmbItemType;
        private global::System.Windows.Forms.Label label107;
        private global::System.Windows.Forms.Button btnChangeItemConsoleCharacterBackColor;
        private global::System.Windows.Forms.Button btnChangeItemConsoleCharacterForeColor;
        private global::System.Windows.Forms.Button btnChangeItemConsoleCharacter;
        private global::System.Windows.Forms.Label lblItemConsoleRepresentation;
        private global::System.Windows.Forms.Label label102;
        private global::System.Windows.Forms.Button fklblItemDescriptionLocale;
        private global::System.Windows.Forms.TextBox txtItemDescription;
        private global::System.Windows.Forms.Label label105;
        private global::System.Windows.Forms.Button fklblItemNameLocale;
        private global::System.Windows.Forms.TextBox txtItemName;
        private global::System.Windows.Forms.Label label106;
        private global::System.Windows.Forms.Button btnItemOnAttackedAction;
        private global::System.Windows.Forms.Label lblItemOnAttackedAction;
        private global::System.Windows.Forms.Button btnRemoveItemOnAttackAction;
        private global::System.Windows.Forms.Button btnEditItemOnAttackAction;
        private global::System.Windows.Forms.Button btnAddItemOnAttackAction;
        private global::System.Windows.Forms.ListBox lbItemOnAttackActions;
        private global::System.Windows.Forms.Label lblItemOnAttackActions;
        private global::System.Windows.Forms.Button btnItemOnUseAction;
        private global::System.Windows.Forms.Label lblItemOnUseAction;
        private global::System.Windows.Forms.Button btnItemOnSteppedAction;
        private global::System.Windows.Forms.Label lblItemOnSteppedAction;
        private global::System.Windows.Forms.Button btnItemOnTurnStartAction;
        private global::System.Windows.Forms.Label lblItemOnTurnStartAction;
        private global::System.Windows.Forms.Button btnTrapOnSteppedAction;
        private global::System.Windows.Forms.Label label112;
        private global::System.Windows.Forms.CheckBox chkTrapStartsVisible;
        private global::System.Windows.Forms.Button btnChangeTrapConsoleCharacterBackColor;
        private global::System.Windows.Forms.Button btnChangeTrapConsoleCharacterForeColor;
        private global::System.Windows.Forms.Button btnChangeTrapConsoleCharacter;
        private global::System.Windows.Forms.Label lblTrapConsoleRepresentation;
        private global::System.Windows.Forms.Label label116;
        private global::System.Windows.Forms.Button fklblTrapDescriptionLocale;
        private global::System.Windows.Forms.TextBox txtTrapDescription;
        private global::System.Windows.Forms.Label label117;
        private global::System.Windows.Forms.Button fklblTrapNameLocale;
        private global::System.Windows.Forms.TextBox txtTrapName;
        private global::System.Windows.Forms.Label label118;
        private global::System.Windows.Forms.TextBox txtTrapPower;
        private global::System.Windows.Forms.Label label113;
        private global::System.Windows.Forms.Button btnAlteredStatusOnApplyAction;
        private global::System.Windows.Forms.Label label97;
        private global::System.Windows.Forms.TextBox textBox1;
        private global::System.Windows.Forms.CheckBox chkAlteredStatusCanStack;
        private global::System.Windows.Forms.Button btnChangeAlteredStatusConsoleCharacterBackColor;
        private global::System.Windows.Forms.Button btnChangeAlteredStatusConsoleCharacterForeColor;
        private global::System.Windows.Forms.Button btnChangeAlteredStatusConsoleCharacter;
        private global::System.Windows.Forms.Label lblAlteredStatusConsoleRepresentation;
        private global::System.Windows.Forms.Label label111;
        private global::System.Windows.Forms.Button fklblAlteredStatusDescriptionLocale;
        private global::System.Windows.Forms.TextBox txtAlteredStatusDescription;
        private global::System.Windows.Forms.Label label114;
        private global::System.Windows.Forms.Button fklblAlteredStatusNameLocale;
        private global::System.Windows.Forms.TextBox txtAlteredStatusName;
        private global::System.Windows.Forms.Label label115;
        private global::System.Windows.Forms.CheckBox chkAlteredStatusCleansedOnCleanseActions;
        private global::System.Windows.Forms.CheckBox chkAlteredStatusCanOverwrite;
        private global::System.Windows.Forms.Button btnAlteredStatusOnTurnStartAction;
        private global::System.Windows.Forms.Label label109;
        private global::System.Windows.Forms.TreeView tvValidationResults;
        private global::System.Windows.Forms.SaveFileDialog sfdDungeon;
    }
}