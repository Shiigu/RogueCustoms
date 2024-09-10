using System.Windows.Forms;

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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            var dataGridViewCellStyle1 = new DataGridViewCellStyle();
            msMenu = new MenuStrip();
            editorToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            tsButtons = new ToolStrip();
            tsbNewDungeon = new ToolStripButton();
            tsbOpenDungeon = new ToolStripButton();
            tsbSaveDungeon = new ToolStripButton();
            tsbSaveDungeonAs = new ToolStripButton();
            tssDungeonElement = new ToolStripSeparator();
            tsbAddElement = new ToolStripButton();
            tsbSaveElement = new ToolStripButton();
            tsbSaveElementAs = new ToolStripButton();
            tsbDeleteElement = new ToolStripButton();
            tssElementValidate = new ToolStripSeparator();
            tsbValidateDungeon = new ToolStripButton();
            tvDungeonInfo = new TreeView();
            tbTabs = new TabControl();
            tpBasicInfo = new TabPage();
            cmbDefaultLocale = new ComboBox();
            label8 = new Label();
            fklblEndingMessageLocale = new Button();
            fklblWelcomeMessageLocale = new Button();
            fklblAuthorLocale = new Button();
            fklblDungeonNameLocale = new Button();
            txtEndingMessage = new TextBox();
            label4 = new Label();
            txtWelcomeMessage = new TextBox();
            label3 = new Label();
            txtAuthor = new TextBox();
            label2 = new Label();
            txtDungeonName = new TextBox();
            label1 = new Label();
            tpLocales = new TabPage();
            btnDeleteLocale = new Button();
            btnAddLocale = new Button();
            btnUpdateLocale = new Button();
            fklblMissingLocale = new Button();
            txtLocaleEntryValue = new TextBox();
            label7 = new Label();
            txtLocaleEntryKey = new TextBox();
            label6 = new Label();
            dgvLocales = new DataGridView();
            cmKey = new DataGridViewTextBoxColumn();
            cmValue = new DataGridViewTextBoxColumn();
            tpTileSetInfos = new TabPage();
            label151 = new Label();
            csrEmpty = new Controls.ConsoleRepresentationSelector();
            label152 = new Label();
            csrStairs = new Controls.ConsoleRepresentationSelector();
            label153 = new Label();
            label154 = new Label();
            csrFloor = new Controls.ConsoleRepresentationSelector();
            label148 = new Label();
            csrHorizontalHallway = new Controls.ConsoleRepresentationSelector();
            label149 = new Label();
            csrBottomRightHallway = new Controls.ConsoleRepresentationSelector();
            label150 = new Label();
            csrBottomLeftHallway = new Controls.ConsoleRepresentationSelector();
            label137 = new Label();
            csrCentralHallway = new Controls.ConsoleRepresentationSelector();
            label140 = new Label();
            csrVerticalRightHallway = new Controls.ConsoleRepresentationSelector();
            label141 = new Label();
            csrVerticalLeftHallway = new Controls.ConsoleRepresentationSelector();
            label142 = new Label();
            csrHorizontalTopHallway = new Controls.ConsoleRepresentationSelector();
            label143 = new Label();
            csrHorizontalBottomHallway = new Controls.ConsoleRepresentationSelector();
            label144 = new Label();
            csrVerticalHallway = new Controls.ConsoleRepresentationSelector();
            label145 = new Label();
            csrTopRightHallway = new Controls.ConsoleRepresentationSelector();
            label146 = new Label();
            label147 = new Label();
            csrTopLeftHallway = new Controls.ConsoleRepresentationSelector();
            label138 = new Label();
            csrConnectorWall = new Controls.ConsoleRepresentationSelector();
            label134 = new Label();
            csrHorizontalWall = new Controls.ConsoleRepresentationSelector();
            label135 = new Label();
            csrBottomRightWall = new Controls.ConsoleRepresentationSelector();
            label136 = new Label();
            csrBottomLeftWall = new Controls.ConsoleRepresentationSelector();
            label133 = new Label();
            csrVerticalWall = new Controls.ConsoleRepresentationSelector();
            label132 = new Label();
            csrTopRightWall = new Controls.ConsoleRepresentationSelector();
            label131 = new Label();
            label130 = new Label();
            csrTopLeftWall = new Controls.ConsoleRepresentationSelector();
            tpFloorInfos = new TabPage();
            btnPasteAlgorithm = new Button();
            btnCopyAlgorithm = new Button();
            nudHungerLostPerTurn = new NumericUpDown();
            label31 = new Label();
            saeOnFloorStart = new Controls.SingleActionEditor();
            cmbTilesets = new ComboBox();
            label155 = new Label();
            nudRoomFusionOdds = new NumericUpDown();
            label19 = new Label();
            nudExtraRoomConnectionOdds = new NumericUpDown();
            label18 = new Label();
            nudMaxRoomConnections = new NumericUpDown();
            label5 = new Label();
            btnRemoveAlgorithm = new Button();
            btnEditAlgorithm = new Button();
            btnAddAlgorithm = new Button();
            lvFloorAlgorithms = new ListView();
            label17 = new Label();
            label16 = new Label();
            btnTrapGenerator = new Button();
            label15 = new Label();
            btnItemGenerator = new Button();
            label14 = new Label();
            btnNPCGenerator = new Button();
            label13 = new Label();
            nudHeight = new NumericUpDown();
            nudWidth = new NumericUpDown();
            label12 = new Label();
            label11 = new Label();
            fklblStairsReminder = new Button();
            chkGenerateStairsOnStart = new CheckBox();
            nudMaxFloorLevel = new NumericUpDown();
            label10 = new Label();
            nudMinFloorLevel = new NumericUpDown();
            label9 = new Label();
            tpFactionInfos = new TabPage();
            lbEnemies = new ListBox();
            label26 = new Label();
            btnEnemiesToNeutrals = new Button();
            btnEnemyToNeutral = new Button();
            btnNeutralsToEnemies = new Button();
            btnNeutralToEnemy = new Button();
            lbNeutrals = new ListBox();
            label25 = new Label();
            btnNeutralsToAllies = new Button();
            btnNeutralToAlly = new Button();
            btnAlliesToNeutrals = new Button();
            btnAllyToNeutral = new Button();
            lbAllies = new ListBox();
            label24 = new Label();
            label23 = new Label();
            fklblFactionDescriptionLocale = new Button();
            txtFactionDescription = new TextBox();
            label22 = new Label();
            fklblFactionNameLocale = new Button();
            txtFactionName = new TextBox();
            label21 = new Label();
            tpPlayerClass = new TabPage();
            ssPlayer = new Controls.StatsSheet();
            sisPlayerStartingInventory = new Controls.StartingInventorySelector();
            saePlayerOnDeath = new Controls.SingleActionEditor();
            saePlayerOnAttacked = new Controls.SingleActionEditor();
            saePlayerOnTurnStart = new Controls.SingleActionEditor();
            maePlayerOnAttack = new Controls.MultiActionEditor();
            label58 = new Label();
            cmbPlayerStartingArmor = new ComboBox();
            label57 = new Label();
            cmbPlayerStartingWeapon = new ComboBox();
            label56 = new Label();
            label54 = new Label();
            nudPlayerInventorySize = new NumericUpDown();
            label53 = new Label();
            label30 = new Label();
            chkPlayerStartsVisible = new CheckBox();
            cmbPlayerFaction = new ComboBox();
            label29 = new Label();
            chkRequirePlayerPrompt = new CheckBox();
            fklblPlayerClassDescriptionLocale = new Button();
            txtPlayerClassDescription = new TextBox();
            label28 = new Label();
            fklblPlayerClassNameLocale = new Button();
            txtPlayerClassName = new TextBox();
            label27 = new Label();
            crsPlayer = new Controls.ConsoleRepresentationSelector();
            tpNPC = new TabPage();
            lblNPCAIOddsToTargetSelfB = new Label();
            nudNPCOddsToTargetSelf = new NumericUpDown();
            cmbNPCAIType = new ComboBox();
            label20 = new Label();
            maeNPCOnInteracted = new Controls.MultiActionEditor();
            saeNPCOnSpawn = new Controls.SingleActionEditor();
            ssNPC = new Controls.StatsSheet();
            sisNPCStartingInventory = new Controls.StartingInventorySelector();
            saeNPCOnDeath = new Controls.SingleActionEditor();
            saeNPCOnAttacked = new Controls.SingleActionEditor();
            saeNPCOnTurnStart = new Controls.SingleActionEditor();
            maeNPCOnAttack = new Controls.MultiActionEditor();
            lblNPCAIOddsToTargetSelfA = new Label();
            txtNPCExperiencePayout = new TextBox();
            label103 = new Label();
            chkNPCKnowsAllCharacterPositions = new CheckBox();
            label67 = new Label();
            cmbNPCStartingArmor = new ComboBox();
            label70 = new Label();
            cmbNPCStartingWeapon = new ComboBox();
            label71 = new Label();
            label73 = new Label();
            nudNPCInventorySize = new NumericUpDown();
            label74 = new Label();
            label98 = new Label();
            chkNPCStartsVisible = new CheckBox();
            cmbNPCFaction = new ComboBox();
            label99 = new Label();
            fklblNPCDescriptionLocale = new Button();
            txtNPCDescription = new TextBox();
            label100 = new Label();
            fklblNPCNameLocale = new Button();
            txtNPCName = new TextBox();
            label101 = new Label();
            crsNPC = new Controls.ConsoleRepresentationSelector();
            tpItem = new TabPage();
            saeItemOnDeath = new Controls.SingleActionEditor();
            saeItemOnTurnStart = new Controls.SingleActionEditor();
            saeItemOnAttacked = new Controls.SingleActionEditor();
            maeItemOnAttack = new Controls.MultiActionEditor();
            saeItemOnUse = new Controls.SingleActionEditor();
            saeItemOnStepped = new Controls.SingleActionEditor();
            txtItemPower = new TextBox();
            label108 = new Label();
            chkItemCanBePickedUp = new CheckBox();
            chkItemStartsVisible = new CheckBox();
            cmbItemType = new ComboBox();
            label107 = new Label();
            label102 = new Label();
            fklblItemDescriptionLocale = new Button();
            txtItemDescription = new TextBox();
            label105 = new Label();
            fklblItemNameLocale = new Button();
            txtItemName = new TextBox();
            label106 = new Label();
            crsItem = new Controls.ConsoleRepresentationSelector();
            tpTrap = new TabPage();
            saeTrapOnStepped = new Controls.SingleActionEditor();
            txtTrapPower = new TextBox();
            label113 = new Label();
            chkTrapStartsVisible = new CheckBox();
            label116 = new Label();
            fklblTrapDescriptionLocale = new Button();
            txtTrapDescription = new TextBox();
            label117 = new Label();
            fklblTrapNameLocale = new Button();
            txtTrapName = new TextBox();
            label118 = new Label();
            crsTrap = new Controls.ConsoleRepresentationSelector();
            tpAlteredStatus = new TabPage();
            saeAlteredStatusOnAttacked = new Controls.SingleActionEditor();
            saeAlteredStatusBeforeAttack = new Controls.SingleActionEditor();
            saeAlteredStatusOnRemove = new Controls.SingleActionEditor();
            saeAlteredStatusOnTurnStart = new Controls.SingleActionEditor();
            saeAlteredStatusOnApply = new Controls.SingleActionEditor();
            chkAlteredStatusCleansedOnCleanseActions = new CheckBox();
            chkAlteredStatusCleanseOnFloorChange = new CheckBox();
            chkAlteredStatusCanOverwrite = new CheckBox();
            chkAlteredStatusCanStack = new CheckBox();
            label111 = new Label();
            fklblAlteredStatusDescriptionLocale = new Button();
            txtAlteredStatusDescription = new TextBox();
            label114 = new Label();
            fklblAlteredStatusNameLocale = new Button();
            txtAlteredStatusName = new TextBox();
            label115 = new Label();
            crsAlteredStatus = new Controls.ConsoleRepresentationSelector();
            tpValidation = new TabPage();
            tvValidationResults = new TreeView();
            ofdDungeon = new OpenFileDialog();
            sfdDungeon = new SaveFileDialog();
            msMenu.SuspendLayout();
            tsButtons.SuspendLayout();
            tbTabs.SuspendLayout();
            tpBasicInfo.SuspendLayout();
            tpLocales.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLocales).BeginInit();
            tpTileSetInfos.SuspendLayout();
            tpFloorInfos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudHungerLostPerTurn).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudRoomFusionOdds).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudExtraRoomConnectionOdds).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMaxRoomConnections).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudHeight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudWidth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMaxFloorLevel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMinFloorLevel).BeginInit();
            tpFactionInfos.SuspendLayout();
            tpPlayerClass.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudPlayerInventorySize).BeginInit();
            tpNPC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudNPCOddsToTargetSelf).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudNPCInventorySize).BeginInit();
            tpItem.SuspendLayout();
            tpTrap.SuspendLayout();
            tpAlteredStatus.SuspendLayout();
            tpValidation.SuspendLayout();
            SuspendLayout();
            // 
            // msMenu
            // 
            msMenu.Items.AddRange(new ToolStripItem[] { editorToolStripMenuItem });
            msMenu.Location = new System.Drawing.Point(0, 0);
            msMenu.Name = "msMenu";
            msMenu.Size = new System.Drawing.Size(967, 24);
            msMenu.TabIndex = 0;
            msMenu.Text = "menuStrip1";
            // 
            // editorToolStripMenuItem
            // 
            editorToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { exitToolStripMenuItem });
            editorToolStripMenuItem.Name = "editorToolStripMenuItem";
            editorToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            editorToolStripMenuItem.Text = "Editor";
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // tsButtons
            // 
            tsButtons.Items.AddRange(new ToolStripItem[] { tsbNewDungeon, tsbOpenDungeon, tsbSaveDungeon, tsbSaveDungeonAs, tssDungeonElement, tsbAddElement, tsbSaveElement, tsbSaveElementAs, tsbDeleteElement, tssElementValidate, tsbValidateDungeon });
            tsButtons.Location = new System.Drawing.Point(0, 24);
            tsButtons.Name = "tsButtons";
            tsButtons.Size = new System.Drawing.Size(967, 38);
            tsButtons.TabIndex = 1;
            tsButtons.Text = "toolStrip1";
            // 
            // tsbNewDungeon
            // 
            tsbNewDungeon.Image = (System.Drawing.Image)resources.GetObject("tsbNewDungeon.Image");
            tsbNewDungeon.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbNewDungeon.Name = "tsbNewDungeon";
            tsbNewDungeon.Size = new System.Drawing.Size(87, 35);
            tsbNewDungeon.Text = "New Dungeon";
            tsbNewDungeon.TextImageRelation = TextImageRelation.ImageAboveText;
            tsbNewDungeon.ToolTipText = "Create a new, empty Dungeon";
            tsbNewDungeon.Click += tsbNewDungeon_Click;
            // 
            // tsbOpenDungeon
            // 
            tsbOpenDungeon.Image = (System.Drawing.Image)resources.GetObject("tsbOpenDungeon.Image");
            tsbOpenDungeon.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbOpenDungeon.Name = "tsbOpenDungeon";
            tsbOpenDungeon.Size = new System.Drawing.Size(92, 35);
            tsbOpenDungeon.Text = "Open Dungeon";
            tsbOpenDungeon.TextImageRelation = TextImageRelation.ImageAboveText;
            tsbOpenDungeon.ToolTipText = "Edit an existing Dungeon";
            tsbOpenDungeon.Click += tsbOpenDungeon_Click;
            // 
            // tsbSaveDungeon
            // 
            tsbSaveDungeon.Image = (System.Drawing.Image)resources.GetObject("tsbSaveDungeon.Image");
            tsbSaveDungeon.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbSaveDungeon.Name = "tsbSaveDungeon";
            tsbSaveDungeon.Size = new System.Drawing.Size(87, 35);
            tsbSaveDungeon.Text = "Save Dungeon";
            tsbSaveDungeon.TextImageRelation = TextImageRelation.ImageAboveText;
            tsbSaveDungeon.ToolTipText = "Save Dungeon to a file";
            tsbSaveDungeon.Visible = false;
            tsbSaveDungeon.Click += tsbSaveDungeon_Click;
            // 
            // tsbSaveDungeonAs
            // 
            tsbSaveDungeonAs.Image = (System.Drawing.Image)resources.GetObject("tsbSaveDungeonAs.Image");
            tsbSaveDungeonAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbSaveDungeonAs.Name = "tsbSaveDungeonAs";
            tsbSaveDungeonAs.Size = new System.Drawing.Size(112, 35);
            tsbSaveDungeonAs.Text = "Save Dungeon As...";
            tsbSaveDungeonAs.TextImageRelation = TextImageRelation.ImageAboveText;
            tsbSaveDungeonAs.ToolTipText = "Save the Dungeon to a file of your choosing";
            tsbSaveDungeonAs.Visible = false;
            tsbSaveDungeonAs.Click += tsbSaveDungeonAs_Click;
            // 
            // tssDungeonElement
            // 
            tssDungeonElement.Name = "tssDungeonElement";
            tssDungeonElement.Size = new System.Drawing.Size(6, 38);
            tssDungeonElement.Visible = false;
            // 
            // tsbAddElement
            // 
            tsbAddElement.Image = (System.Drawing.Image)resources.GetObject("tsbAddElement.Image");
            tsbAddElement.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbAddElement.Name = "tsbAddElement";
            tsbAddElement.Size = new System.Drawing.Size(81, 35);
            tsbAddElement.Text = "New Element";
            tsbAddElement.TextImageRelation = TextImageRelation.ImageAboveText;
            tsbAddElement.ToolTipText = "Add a new element of this category";
            tsbAddElement.Visible = false;
            tsbAddElement.Click += tsbAddElement_Click;
            // 
            // tsbSaveElement
            // 
            tsbSaveElement.Image = (System.Drawing.Image)resources.GetObject("tsbSaveElement.Image");
            tsbSaveElement.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbSaveElement.Name = "tsbSaveElement";
            tsbSaveElement.Size = new System.Drawing.Size(81, 35);
            tsbSaveElement.Text = "Save Element";
            tsbSaveElement.TextImageRelation = TextImageRelation.ImageAboveText;
            tsbSaveElement.ToolTipText = "Save currently-opened Element";
            tsbSaveElement.Visible = false;
            tsbSaveElement.Click += tsbSaveElement_Click;
            // 
            // tsbSaveElementAs
            // 
            tsbSaveElementAs.Image = (System.Drawing.Image)resources.GetObject("tsbSaveElementAs.Image");
            tsbSaveElementAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbSaveElementAs.Name = "tsbSaveElementAs";
            tsbSaveElementAs.Size = new System.Drawing.Size(133, 35);
            tsbSaveElementAs.Text = "Save As New Element...";
            tsbSaveElementAs.TextImageRelation = TextImageRelation.ImageAboveText;
            tsbSaveElementAs.ToolTipText = "Save currently-opened Element with another name";
            tsbSaveElementAs.Visible = false;
            tsbSaveElementAs.Click += tsbSaveElementAs_Click;
            // 
            // tsbDeleteElement
            // 
            tsbDeleteElement.Image = (System.Drawing.Image)resources.GetObject("tsbDeleteElement.Image");
            tsbDeleteElement.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbDeleteElement.Name = "tsbDeleteElement";
            tsbDeleteElement.Size = new System.Drawing.Size(90, 35);
            tsbDeleteElement.Text = "Delete Element";
            tsbDeleteElement.TextImageRelation = TextImageRelation.ImageAboveText;
            tsbDeleteElement.ToolTipText = "Remove the currently open Element";
            tsbDeleteElement.Visible = false;
            tsbDeleteElement.Click += tsbDeleteElement_Click;
            // 
            // tssElementValidate
            // 
            tssElementValidate.Name = "tssElementValidate";
            tssElementValidate.Size = new System.Drawing.Size(6, 38);
            tssElementValidate.Visible = false;
            // 
            // tsbValidateDungeon
            // 
            tsbValidateDungeon.Image = (System.Drawing.Image)resources.GetObject("tsbValidateDungeon.Image");
            tsbValidateDungeon.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbValidateDungeon.Name = "tsbValidateDungeon";
            tsbValidateDungeon.Size = new System.Drawing.Size(104, 35);
            tsbValidateDungeon.Text = "Validate Dungeon";
            tsbValidateDungeon.TextImageRelation = TextImageRelation.ImageAboveText;
            tsbValidateDungeon.ToolTipText = "Run the Dungeon Validator to check if it won't crash Rogue Customs";
            tsbValidateDungeon.Visible = false;
            tsbValidateDungeon.Click += tsbValidateDungeon_Click;
            // 
            // tvDungeonInfo
            // 
            tvDungeonInfo.FullRowSelect = true;
            tvDungeonInfo.HideSelection = false;
            tvDungeonInfo.Location = new System.Drawing.Point(0, 65);
            tvDungeonInfo.Name = "tvDungeonInfo";
            tvDungeonInfo.Size = new System.Drawing.Size(217, 384);
            tvDungeonInfo.TabIndex = 2;
            tvDungeonInfo.AfterSelect += tvDungeonInfo_AfterSelect;
            // 
            // tbTabs
            // 
            tbTabs.Controls.Add(tpBasicInfo);
            tbTabs.Controls.Add(tpLocales);
            tbTabs.Controls.Add(tpTileSetInfos);
            tbTabs.Controls.Add(tpFloorInfos);
            tbTabs.Controls.Add(tpFactionInfos);
            tbTabs.Controls.Add(tpPlayerClass);
            tbTabs.Controls.Add(tpNPC);
            tbTabs.Controls.Add(tpItem);
            tbTabs.Controls.Add(tpTrap);
            tbTabs.Controls.Add(tpAlteredStatus);
            tbTabs.Controls.Add(tpValidation);
            tbTabs.Location = new System.Drawing.Point(219, 65);
            tbTabs.Name = "tbTabs";
            tbTabs.SelectedIndex = 0;
            tbTabs.Size = new System.Drawing.Size(748, 384);
            tbTabs.TabIndex = 3;
            tbTabs.SelectedIndexChanged += tbTabs_SelectedIndexChanged;
            // 
            // tpBasicInfo
            // 
            tpBasicInfo.Controls.Add(cmbDefaultLocale);
            tpBasicInfo.Controls.Add(label8);
            tpBasicInfo.Controls.Add(fklblEndingMessageLocale);
            tpBasicInfo.Controls.Add(fklblWelcomeMessageLocale);
            tpBasicInfo.Controls.Add(fklblAuthorLocale);
            tpBasicInfo.Controls.Add(fklblDungeonNameLocale);
            tpBasicInfo.Controls.Add(txtEndingMessage);
            tpBasicInfo.Controls.Add(label4);
            tpBasicInfo.Controls.Add(txtWelcomeMessage);
            tpBasicInfo.Controls.Add(label3);
            tpBasicInfo.Controls.Add(txtAuthor);
            tpBasicInfo.Controls.Add(label2);
            tpBasicInfo.Controls.Add(txtDungeonName);
            tpBasicInfo.Controls.Add(label1);
            tpBasicInfo.Location = new System.Drawing.Point(4, 24);
            tpBasicInfo.Name = "tpBasicInfo";
            tpBasicInfo.Padding = new Padding(3);
            tpBasicInfo.Size = new System.Drawing.Size(740, 356);
            tpBasicInfo.TabIndex = 0;
            tpBasicInfo.Text = "Basic Information";
            tpBasicInfo.UseVisualStyleBackColor = true;
            // 
            // cmbDefaultLocale
            // 
            cmbDefaultLocale.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDefaultLocale.FormattingEnabled = true;
            cmbDefaultLocale.Location = new System.Drawing.Point(188, 228);
            cmbDefaultLocale.Name = "cmbDefaultLocale";
            cmbDefaultLocale.Size = new System.Drawing.Size(81, 23);
            cmbDefaultLocale.TabIndex = 17;
            cmbDefaultLocale.SelectedIndexChanged += cmbDefaultLocale_SelectedIndexChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(11, 223);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(171, 30);
            label8.TabIndex = 16;
            label8.Text = "If the game has a language this\r\ndungeon lacks, use this locale:";
            // 
            // fklblEndingMessageLocale
            // 
            fklblEndingMessageLocale.Enabled = false;
            fklblEndingMessageLocale.FlatAppearance.BorderSize = 0;
            fklblEndingMessageLocale.FlatStyle = FlatStyle.Flat;
            fklblEndingMessageLocale.Image = (System.Drawing.Image)resources.GetObject("fklblEndingMessageLocale.Image");
            fklblEndingMessageLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblEndingMessageLocale.Location = new System.Drawing.Point(401, 304);
            fklblEndingMessageLocale.Name = "fklblEndingMessageLocale";
            fklblEndingMessageLocale.Size = new System.Drawing.Size(331, 42);
            fklblEndingMessageLocale.TabIndex = 15;
            fklblEndingMessageLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblEndingMessageLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblEndingMessageLocale.TextImageRelation = TextImageRelation.ImageBeforeText;
            fklblEndingMessageLocale.UseVisualStyleBackColor = true;
            fklblEndingMessageLocale.Visible = false;
            // 
            // fklblWelcomeMessageLocale
            // 
            fklblWelcomeMessageLocale.Enabled = false;
            fklblWelcomeMessageLocale.FlatAppearance.BorderSize = 0;
            fklblWelcomeMessageLocale.FlatStyle = FlatStyle.Flat;
            fklblWelcomeMessageLocale.Image = (System.Drawing.Image)resources.GetObject("fklblWelcomeMessageLocale.Image");
            fklblWelcomeMessageLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblWelcomeMessageLocale.Location = new System.Drawing.Point(403, 132);
            fklblWelcomeMessageLocale.Name = "fklblWelcomeMessageLocale";
            fklblWelcomeMessageLocale.Size = new System.Drawing.Size(331, 42);
            fklblWelcomeMessageLocale.TabIndex = 14;
            fklblWelcomeMessageLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblWelcomeMessageLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblWelcomeMessageLocale.TextImageRelation = TextImageRelation.ImageBeforeText;
            fklblWelcomeMessageLocale.UseVisualStyleBackColor = true;
            fklblWelcomeMessageLocale.Visible = false;
            // 
            // fklblAuthorLocale
            // 
            fklblAuthorLocale.Enabled = false;
            fklblAuthorLocale.FlatAppearance.BorderSize = 0;
            fklblAuthorLocale.FlatStyle = FlatStyle.Flat;
            fklblAuthorLocale.Image = (System.Drawing.Image)resources.GetObject("fklblAuthorLocale.Image");
            fklblAuthorLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblAuthorLocale.Location = new System.Drawing.Point(11, 168);
            fklblAuthorLocale.Name = "fklblAuthorLocale";
            fklblAuthorLocale.Size = new System.Drawing.Size(331, 42);
            fklblAuthorLocale.TabIndex = 13;
            fklblAuthorLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblAuthorLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblAuthorLocale.TextImageRelation = TextImageRelation.ImageBeforeText;
            fklblAuthorLocale.UseVisualStyleBackColor = true;
            fklblAuthorLocale.Visible = false;
            // 
            // fklblDungeonNameLocale
            // 
            fklblDungeonNameLocale.Enabled = false;
            fklblDungeonNameLocale.FlatAppearance.BorderSize = 0;
            fklblDungeonNameLocale.FlatStyle = FlatStyle.Flat;
            fklblDungeonNameLocale.Image = (System.Drawing.Image)resources.GetObject("fklblDungeonNameLocale.Image");
            fklblDungeonNameLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblDungeonNameLocale.Location = new System.Drawing.Point(11, 55);
            fklblDungeonNameLocale.Name = "fklblDungeonNameLocale";
            fklblDungeonNameLocale.Size = new System.Drawing.Size(331, 42);
            fklblDungeonNameLocale.TabIndex = 12;
            fklblDungeonNameLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblDungeonNameLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblDungeonNameLocale.TextImageRelation = TextImageRelation.ImageBeforeText;
            fklblDungeonNameLocale.UseVisualStyleBackColor = true;
            fklblDungeonNameLocale.Visible = false;
            // 
            // txtEndingMessage
            // 
            txtEndingMessage.Location = new System.Drawing.Point(403, 195);
            txtEndingMessage.Multiline = true;
            txtEndingMessage.Name = "txtEndingMessage";
            txtEndingMessage.ScrollBars = ScrollBars.Vertical;
            txtEndingMessage.Size = new System.Drawing.Size(313, 103);
            txtEndingMessage.TabIndex = 7;
            txtEndingMessage.TextChanged += txtEndingMessage_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(403, 177);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(93, 15);
            label4.TabIndex = 6;
            label4.Text = "Ending Message";
            // 
            // txtWelcomeMessage
            // 
            txtWelcomeMessage.Location = new System.Drawing.Point(403, 26);
            txtWelcomeMessage.Multiline = true;
            txtWelcomeMessage.Name = "txtWelcomeMessage";
            txtWelcomeMessage.ScrollBars = ScrollBars.Vertical;
            txtWelcomeMessage.Size = new System.Drawing.Size(313, 103);
            txtWelcomeMessage.TabIndex = 5;
            txtWelcomeMessage.TextChanged += txtWelcomeMessage_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(403, 8);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(106, 15);
            label3.TabIndex = 4;
            label3.Text = "Welcome Message";
            // 
            // txtAuthor
            // 
            txtAuthor.Location = new System.Drawing.Point(11, 134);
            txtAuthor.Name = "txtAuthor";
            txtAuthor.Size = new System.Drawing.Size(359, 23);
            txtAuthor.TabIndex = 3;
            txtAuthor.TextChanged += txtAuthor_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(11, 116);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(44, 15);
            label2.TabIndex = 2;
            label2.Text = "Author";
            // 
            // txtDungeonName
            // 
            txtDungeonName.Location = new System.Drawing.Point(11, 26);
            txtDungeonName.Name = "txtDungeonName";
            txtDungeonName.Size = new System.Drawing.Size(359, 23);
            txtDungeonName.TabIndex = 1;
            txtDungeonName.TextChanged += txtDungeonName_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(11, 8);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(39, 15);
            label1.TabIndex = 0;
            label1.Text = "Name";
            // 
            // tpLocales
            // 
            tpLocales.Controls.Add(btnDeleteLocale);
            tpLocales.Controls.Add(btnAddLocale);
            tpLocales.Controls.Add(btnUpdateLocale);
            tpLocales.Controls.Add(fklblMissingLocale);
            tpLocales.Controls.Add(txtLocaleEntryValue);
            tpLocales.Controls.Add(label7);
            tpLocales.Controls.Add(txtLocaleEntryKey);
            tpLocales.Controls.Add(label6);
            tpLocales.Controls.Add(dgvLocales);
            tpLocales.Location = new System.Drawing.Point(4, 24);
            tpLocales.Name = "tpLocales";
            tpLocales.Padding = new Padding(3);
            tpLocales.Size = new System.Drawing.Size(740, 356);
            tpLocales.TabIndex = 1;
            tpLocales.Text = "Locale Entries";
            tpLocales.UseVisualStyleBackColor = true;
            // 
            // btnDeleteLocale
            // 
            btnDeleteLocale.Enabled = false;
            btnDeleteLocale.Location = new System.Drawing.Point(401, 326);
            btnDeleteLocale.Name = "btnDeleteLocale";
            btnDeleteLocale.Size = new System.Drawing.Size(331, 23);
            btnDeleteLocale.TabIndex = 16;
            btnDeleteLocale.Text = "Delete Locale Entry";
            btnDeleteLocale.UseVisualStyleBackColor = true;
            btnDeleteLocale.Click += btnDeleteLocale_Click;
            // 
            // btnAddLocale
            // 
            btnAddLocale.Enabled = false;
            btnAddLocale.Location = new System.Drawing.Point(569, 297);
            btnAddLocale.Name = "btnAddLocale";
            btnAddLocale.Size = new System.Drawing.Size(163, 23);
            btnAddLocale.TabIndex = 15;
            btnAddLocale.Text = "Add New Locale Entry";
            btnAddLocale.UseVisualStyleBackColor = true;
            btnAddLocale.Click += btnAddLocale_Click;
            // 
            // btnUpdateLocale
            // 
            btnUpdateLocale.Enabled = false;
            btnUpdateLocale.Location = new System.Drawing.Point(401, 297);
            btnUpdateLocale.Name = "btnUpdateLocale";
            btnUpdateLocale.Size = new System.Drawing.Size(162, 23);
            btnUpdateLocale.TabIndex = 14;
            btnUpdateLocale.Text = "Update Locale Entry";
            btnUpdateLocale.UseVisualStyleBackColor = true;
            btnUpdateLocale.Click += btnUpdateLocale_Click;
            // 
            // fklblMissingLocale
            // 
            fklblMissingLocale.Enabled = false;
            fklblMissingLocale.FlatAppearance.BorderSize = 0;
            fklblMissingLocale.FlatStyle = FlatStyle.Flat;
            fklblMissingLocale.Image = (System.Drawing.Image)resources.GetObject("fklblMissingLocale.Image");
            fklblMissingLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblMissingLocale.Location = new System.Drawing.Point(403, 228);
            fklblMissingLocale.Name = "fklblMissingLocale";
            fklblMissingLocale.Size = new System.Drawing.Size(331, 42);
            fklblMissingLocale.TabIndex = 13;
            fklblMissingLocale.Text = "(Missing locale warning)";
            fklblMissingLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblMissingLocale.TextImageRelation = TextImageRelation.ImageBeforeText;
            fklblMissingLocale.UseVisualStyleBackColor = true;
            fklblMissingLocale.Visible = false;
            // 
            // txtLocaleEntryValue
            // 
            txtLocaleEntryValue.Enabled = false;
            txtLocaleEntryValue.Location = new System.Drawing.Point(401, 79);
            txtLocaleEntryValue.Multiline = true;
            txtLocaleEntryValue.Name = "txtLocaleEntryValue";
            txtLocaleEntryValue.ScrollBars = ScrollBars.Vertical;
            txtLocaleEntryValue.Size = new System.Drawing.Size(331, 143);
            txtLocaleEntryValue.TabIndex = 4;
            txtLocaleEntryValue.TextChanged += txtLocaleEntryValue_TextChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(401, 61);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(102, 15);
            label7.TabIndex = 3;
            label7.Text = "Locale Entry Value";
            // 
            // txtLocaleEntryKey
            // 
            txtLocaleEntryKey.Enabled = false;
            txtLocaleEntryKey.Location = new System.Drawing.Point(401, 25);
            txtLocaleEntryKey.Name = "txtLocaleEntryKey";
            txtLocaleEntryKey.Size = new System.Drawing.Size(331, 23);
            txtLocaleEntryKey.TabIndex = 2;
            txtLocaleEntryKey.TextChanged += txtLocaleEntryKey_TextChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(401, 7);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(93, 15);
            label6.TabIndex = 1;
            label6.Text = "Locale Entry Key";
            // 
            // dgvLocales
            // 
            dgvLocales.AllowUserToAddRows = false;
            dgvLocales.AllowUserToDeleteRows = false;
            dgvLocales.AllowUserToResizeColumns = false;
            dgvLocales.AllowUserToResizeRows = false;
            dgvLocales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLocales.ColumnHeadersVisible = false;
            dgvLocales.Columns.AddRange(new DataGridViewColumn[] { cmKey, cmValue });
            dgvLocales.Location = new System.Drawing.Point(0, 0);
            dgvLocales.MultiSelect = false;
            dgvLocales.Name = "dgvLocales";
            dgvLocales.ReadOnly = true;
            dgvLocales.RowHeadersVisible = false;
            dgvLocales.RowTemplate.Height = 25;
            dgvLocales.ScrollBars = ScrollBars.Vertical;
            dgvLocales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLocales.Size = new System.Drawing.Size(395, 356);
            dgvLocales.TabIndex = 0;
            dgvLocales.SelectionChanged += dgvLocales_SelectionChanged;
            // 
            // cmKey
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            cmKey.DefaultCellStyle = dataGridViewCellStyle1;
            cmKey.HeaderText = "Key";
            cmKey.Name = "cmKey";
            cmKey.ReadOnly = true;
            // 
            // cmValue
            // 
            cmValue.HeaderText = "Value";
            cmValue.Name = "cmValue";
            cmValue.ReadOnly = true;
            cmValue.Width = 300;
            // 
            // tpTileSetInfos
            // 
            tpTileSetInfos.AutoScroll = true;
            tpTileSetInfos.Controls.Add(label151);
            tpTileSetInfos.Controls.Add(csrEmpty);
            tpTileSetInfos.Controls.Add(label152);
            tpTileSetInfos.Controls.Add(csrStairs);
            tpTileSetInfos.Controls.Add(label153);
            tpTileSetInfos.Controls.Add(label154);
            tpTileSetInfos.Controls.Add(csrFloor);
            tpTileSetInfos.Controls.Add(label148);
            tpTileSetInfos.Controls.Add(csrHorizontalHallway);
            tpTileSetInfos.Controls.Add(label149);
            tpTileSetInfos.Controls.Add(csrBottomRightHallway);
            tpTileSetInfos.Controls.Add(label150);
            tpTileSetInfos.Controls.Add(csrBottomLeftHallway);
            tpTileSetInfos.Controls.Add(label137);
            tpTileSetInfos.Controls.Add(csrCentralHallway);
            tpTileSetInfos.Controls.Add(label140);
            tpTileSetInfos.Controls.Add(csrVerticalRightHallway);
            tpTileSetInfos.Controls.Add(label141);
            tpTileSetInfos.Controls.Add(csrVerticalLeftHallway);
            tpTileSetInfos.Controls.Add(label142);
            tpTileSetInfos.Controls.Add(csrHorizontalTopHallway);
            tpTileSetInfos.Controls.Add(label143);
            tpTileSetInfos.Controls.Add(csrHorizontalBottomHallway);
            tpTileSetInfos.Controls.Add(label144);
            tpTileSetInfos.Controls.Add(csrVerticalHallway);
            tpTileSetInfos.Controls.Add(label145);
            tpTileSetInfos.Controls.Add(csrTopRightHallway);
            tpTileSetInfos.Controls.Add(label146);
            tpTileSetInfos.Controls.Add(label147);
            tpTileSetInfos.Controls.Add(csrTopLeftHallway);
            tpTileSetInfos.Controls.Add(label138);
            tpTileSetInfos.Controls.Add(csrConnectorWall);
            tpTileSetInfos.Controls.Add(label134);
            tpTileSetInfos.Controls.Add(csrHorizontalWall);
            tpTileSetInfos.Controls.Add(label135);
            tpTileSetInfos.Controls.Add(csrBottomRightWall);
            tpTileSetInfos.Controls.Add(label136);
            tpTileSetInfos.Controls.Add(csrBottomLeftWall);
            tpTileSetInfos.Controls.Add(label133);
            tpTileSetInfos.Controls.Add(csrVerticalWall);
            tpTileSetInfos.Controls.Add(label132);
            tpTileSetInfos.Controls.Add(csrTopRightWall);
            tpTileSetInfos.Controls.Add(label131);
            tpTileSetInfos.Controls.Add(label130);
            tpTileSetInfos.Controls.Add(csrTopLeftWall);
            tpTileSetInfos.Location = new System.Drawing.Point(4, 24);
            tpTileSetInfos.Name = "tpTileSetInfos";
            tpTileSetInfos.Size = new System.Drawing.Size(740, 356);
            tpTileSetInfos.TabIndex = 10;
            tpTileSetInfos.Text = "Tileset";
            tpTileSetInfos.UseVisualStyleBackColor = true;
            // 
            // label151
            // 
            label151.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label151.Location = new System.Drawing.Point(465, 1055);
            label151.Name = "label151";
            label151.Size = new System.Drawing.Size(211, 32);
            label151.TabIndex = 169;
            label151.Text = "Empty (inaccessible)";
            label151.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrEmpty
            // 
            csrEmpty.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrEmpty.BackgroundColor");
            csrEmpty.Character = '\0';
            csrEmpty.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrEmpty.ForegroundColor");
            csrEmpty.Location = new System.Drawing.Point(465, 1090);
            csrEmpty.Name = "csrEmpty";
            csrEmpty.Size = new System.Drawing.Size(211, 83);
            csrEmpty.TabIndex = 170;
            csrEmpty.PropertyChanged += csrEmpty_PropertyChanged;
            // 
            // label152
            // 
            label152.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label152.Location = new System.Drawing.Point(248, 1055);
            label152.Name = "label152";
            label152.Size = new System.Drawing.Size(211, 32);
            label152.TabIndex = 167;
            label152.Text = "Stairs";
            label152.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrStairs
            // 
            csrStairs.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrStairs.BackgroundColor");
            csrStairs.Character = '\0';
            csrStairs.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrStairs.ForegroundColor");
            csrStairs.Location = new System.Drawing.Point(248, 1090);
            csrStairs.Name = "csrStairs";
            csrStairs.Size = new System.Drawing.Size(211, 83);
            csrStairs.TabIndex = 168;
            csrStairs.PropertyChanged += csrStairs_PropertyChanged;
            // 
            // label153
            // 
            label153.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label153.Location = new System.Drawing.Point(248, 1003);
            label153.Name = "label153";
            label153.Size = new System.Drawing.Size(211, 52);
            label153.TabIndex = 166;
            label153.Text = "OTHERS";
            label153.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label154
            // 
            label154.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label154.Location = new System.Drawing.Point(31, 1055);
            label154.Name = "label154";
            label154.Size = new System.Drawing.Size(211, 32);
            label154.TabIndex = 164;
            label154.Text = "Unoccupied Floor";
            label154.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrFloor
            // 
            csrFloor.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrFloor.BackgroundColor");
            csrFloor.Character = '\0';
            csrFloor.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrFloor.ForegroundColor");
            csrFloor.Location = new System.Drawing.Point(31, 1090);
            csrFloor.Name = "csrFloor";
            csrFloor.Size = new System.Drawing.Size(211, 83);
            csrFloor.TabIndex = 165;
            csrFloor.PropertyChanged += csrFloor_PropertyChanged;
            // 
            // label148
            // 
            label148.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label148.Location = new System.Drawing.Point(454, 611);
            label148.Name = "label148";
            label148.Size = new System.Drawing.Size(211, 32);
            label148.TabIndex = 162;
            label148.Text = "Horizontal";
            label148.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrHorizontalHallway
            // 
            csrHorizontalHallway.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrHorizontalHallway.BackgroundColor");
            csrHorizontalHallway.Character = '\0';
            csrHorizontalHallway.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrHorizontalHallway.ForegroundColor");
            csrHorizontalHallway.Location = new System.Drawing.Point(454, 646);
            csrHorizontalHallway.Name = "csrHorizontalHallway";
            csrHorizontalHallway.Size = new System.Drawing.Size(211, 83);
            csrHorizontalHallway.TabIndex = 163;
            csrHorizontalHallway.PropertyChanged += csrHorizontalHallway_PropertyChanged;
            // 
            // label149
            // 
            label149.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label149.Location = new System.Drawing.Point(237, 611);
            label149.Name = "label149";
            label149.Size = new System.Drawing.Size(211, 32);
            label149.TabIndex = 160;
            label149.Text = "Bottom Right Corner";
            label149.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrBottomRightHallway
            // 
            csrBottomRightHallway.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrBottomRightHallway.BackgroundColor");
            csrBottomRightHallway.Character = '\0';
            csrBottomRightHallway.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrBottomRightHallway.ForegroundColor");
            csrBottomRightHallway.Location = new System.Drawing.Point(237, 646);
            csrBottomRightHallway.Name = "csrBottomRightHallway";
            csrBottomRightHallway.Size = new System.Drawing.Size(211, 83);
            csrBottomRightHallway.TabIndex = 161;
            csrBottomRightHallway.PropertyChanged += csrBottomRightHallway_PropertyChanged;
            // 
            // label150
            // 
            label150.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label150.Location = new System.Drawing.Point(20, 611);
            label150.Name = "label150";
            label150.Size = new System.Drawing.Size(211, 32);
            label150.TabIndex = 158;
            label150.Text = "Bottom Left Corner";
            label150.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrBottomLeftHallway
            // 
            csrBottomLeftHallway.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrBottomLeftHallway.BackgroundColor");
            csrBottomLeftHallway.Character = '\0';
            csrBottomLeftHallway.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrBottomLeftHallway.ForegroundColor");
            csrBottomLeftHallway.Location = new System.Drawing.Point(20, 646);
            csrBottomLeftHallway.Name = "csrBottomLeftHallway";
            csrBottomLeftHallway.Size = new System.Drawing.Size(211, 83);
            csrBottomLeftHallway.TabIndex = 159;
            csrBottomLeftHallway.PropertyChanged += csrBottomLeftHallway_PropertyChanged;
            // 
            // label137
            // 
            label137.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label137.Location = new System.Drawing.Point(324, 862);
            label137.Name = "label137";
            label137.Size = new System.Drawing.Size(211, 32);
            label137.TabIndex = 156;
            label137.Text = "Central";
            label137.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrCentralHallway
            // 
            csrCentralHallway.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrCentralHallway.BackgroundColor");
            csrCentralHallway.Character = '\0';
            csrCentralHallway.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrCentralHallway.ForegroundColor");
            csrCentralHallway.Location = new System.Drawing.Point(324, 897);
            csrCentralHallway.Name = "csrCentralHallway";
            csrCentralHallway.Size = new System.Drawing.Size(211, 83);
            csrCentralHallway.TabIndex = 157;
            csrCentralHallway.PropertyChanged += csrCentralHallway_PropertyChanged;
            // 
            // label140
            // 
            label140.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label140.Location = new System.Drawing.Point(107, 862);
            label140.Name = "label140";
            label140.Size = new System.Drawing.Size(211, 32);
            label140.TabIndex = 154;
            label140.Text = "Vertical-Right";
            label140.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrVerticalRightHallway
            // 
            csrVerticalRightHallway.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrVerticalRightHallway.BackgroundColor");
            csrVerticalRightHallway.Character = '\0';
            csrVerticalRightHallway.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrVerticalRightHallway.ForegroundColor");
            csrVerticalRightHallway.Location = new System.Drawing.Point(107, 897);
            csrVerticalRightHallway.Name = "csrVerticalRightHallway";
            csrVerticalRightHallway.Size = new System.Drawing.Size(211, 83);
            csrVerticalRightHallway.TabIndex = 155;
            csrVerticalRightHallway.PropertyChanged += csrVerticalRightHallway_PropertyChanged;
            // 
            // label141
            // 
            label141.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label141.Location = new System.Drawing.Point(454, 741);
            label141.Name = "label141";
            label141.Size = new System.Drawing.Size(211, 32);
            label141.TabIndex = 152;
            label141.Text = "Vertical-Left";
            label141.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrVerticalLeftHallway
            // 
            csrVerticalLeftHallway.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrVerticalLeftHallway.BackgroundColor");
            csrVerticalLeftHallway.Character = '\0';
            csrVerticalLeftHallway.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrVerticalLeftHallway.ForegroundColor");
            csrVerticalLeftHallway.Location = new System.Drawing.Point(454, 776);
            csrVerticalLeftHallway.Name = "csrVerticalLeftHallway";
            csrVerticalLeftHallway.Size = new System.Drawing.Size(211, 83);
            csrVerticalLeftHallway.TabIndex = 153;
            csrVerticalLeftHallway.PropertyChanged += csrVerticalLeftHallway_PropertyChanged;
            // 
            // label142
            // 
            label142.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label142.Location = new System.Drawing.Point(237, 741);
            label142.Name = "label142";
            label142.Size = new System.Drawing.Size(211, 32);
            label142.TabIndex = 150;
            label142.Text = "Horizontal-Top";
            label142.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrHorizontalTopHallway
            // 
            csrHorizontalTopHallway.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrHorizontalTopHallway.BackgroundColor");
            csrHorizontalTopHallway.Character = '\0';
            csrHorizontalTopHallway.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrHorizontalTopHallway.ForegroundColor");
            csrHorizontalTopHallway.Location = new System.Drawing.Point(237, 776);
            csrHorizontalTopHallway.Name = "csrHorizontalTopHallway";
            csrHorizontalTopHallway.Size = new System.Drawing.Size(211, 83);
            csrHorizontalTopHallway.TabIndex = 151;
            csrHorizontalTopHallway.PropertyChanged += csrHorizontalTopHallway_PropertyChanged;
            // 
            // label143
            // 
            label143.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label143.Location = new System.Drawing.Point(20, 741);
            label143.Name = "label143";
            label143.Size = new System.Drawing.Size(211, 32);
            label143.TabIndex = 148;
            label143.Text = "Horizontal-Bottom";
            label143.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrHorizontalBottomHallway
            // 
            csrHorizontalBottomHallway.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrHorizontalBottomHallway.BackgroundColor");
            csrHorizontalBottomHallway.Character = '\0';
            csrHorizontalBottomHallway.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrHorizontalBottomHallway.ForegroundColor");
            csrHorizontalBottomHallway.Location = new System.Drawing.Point(20, 776);
            csrHorizontalBottomHallway.Name = "csrHorizontalBottomHallway";
            csrHorizontalBottomHallway.Size = new System.Drawing.Size(211, 83);
            csrHorizontalBottomHallway.TabIndex = 149;
            csrHorizontalBottomHallway.PropertyChanged += csrHorizontalBottomHallway_PropertyChanged;
            // 
            // label144
            // 
            label144.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label144.Location = new System.Drawing.Point(454, 490);
            label144.Name = "label144";
            label144.Size = new System.Drawing.Size(211, 32);
            label144.TabIndex = 146;
            label144.Text = "Vertical";
            label144.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrVerticalHallway
            // 
            csrVerticalHallway.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrVerticalHallway.BackgroundColor");
            csrVerticalHallway.Character = '\0';
            csrVerticalHallway.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrVerticalHallway.ForegroundColor");
            csrVerticalHallway.Location = new System.Drawing.Point(454, 525);
            csrVerticalHallway.Name = "csrVerticalHallway";
            csrVerticalHallway.Size = new System.Drawing.Size(211, 83);
            csrVerticalHallway.TabIndex = 147;
            csrVerticalHallway.PropertyChanged += csrVerticalHallway_PropertyChanged;
            // 
            // label145
            // 
            label145.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label145.Location = new System.Drawing.Point(237, 490);
            label145.Name = "label145";
            label145.Size = new System.Drawing.Size(211, 32);
            label145.TabIndex = 144;
            label145.Text = "Top Right Corner";
            label145.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrTopRightHallway
            // 
            csrTopRightHallway.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrTopRightHallway.BackgroundColor");
            csrTopRightHallway.Character = '\0';
            csrTopRightHallway.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrTopRightHallway.ForegroundColor");
            csrTopRightHallway.Location = new System.Drawing.Point(237, 525);
            csrTopRightHallway.Name = "csrTopRightHallway";
            csrTopRightHallway.Size = new System.Drawing.Size(211, 83);
            csrTopRightHallway.TabIndex = 145;
            csrTopRightHallway.PropertyChanged += csrTopRightHallway_PropertyChanged;
            // 
            // label146
            // 
            label146.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label146.Location = new System.Drawing.Point(237, 438);
            label146.Name = "label146";
            label146.Size = new System.Drawing.Size(211, 52);
            label146.TabIndex = 143;
            label146.Text = "HALLWAYS";
            label146.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label147
            // 
            label147.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label147.Location = new System.Drawing.Point(20, 490);
            label147.Name = "label147";
            label147.Size = new System.Drawing.Size(211, 32);
            label147.TabIndex = 141;
            label147.Text = "Top Left Corner";
            label147.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrTopLeftHallway
            // 
            csrTopLeftHallway.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrTopLeftHallway.BackgroundColor");
            csrTopLeftHallway.Character = '\0';
            csrTopLeftHallway.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrTopLeftHallway.ForegroundColor");
            csrTopLeftHallway.Location = new System.Drawing.Point(20, 525);
            csrTopLeftHallway.Name = "csrTopLeftHallway";
            csrTopLeftHallway.Size = new System.Drawing.Size(211, 83);
            csrTopLeftHallway.TabIndex = 142;
            csrTopLeftHallway.PropertyChanged += csrTopLeftHallway_PropertyChanged;
            // 
            // label138
            // 
            label138.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label138.Location = new System.Drawing.Point(253, 304);
            label138.Name = "label138";
            label138.Size = new System.Drawing.Size(211, 32);
            label138.TabIndex = 139;
            label138.Text = "Hallway Connector";
            label138.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrConnectorWall
            // 
            csrConnectorWall.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrConnectorWall.BackgroundColor");
            csrConnectorWall.Character = '\0';
            csrConnectorWall.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrConnectorWall.ForegroundColor");
            csrConnectorWall.Location = new System.Drawing.Point(253, 339);
            csrConnectorWall.Name = "csrConnectorWall";
            csrConnectorWall.Size = new System.Drawing.Size(211, 83);
            csrConnectorWall.TabIndex = 140;
            csrConnectorWall.PropertyChanged += csrConnectorWall_PropertyChanged;
            // 
            // label134
            // 
            label134.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label134.Location = new System.Drawing.Point(470, 183);
            label134.Name = "label134";
            label134.Size = new System.Drawing.Size(211, 32);
            label134.TabIndex = 135;
            label134.Text = "Horizontal";
            label134.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrHorizontalWall
            // 
            csrHorizontalWall.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrHorizontalWall.BackgroundColor");
            csrHorizontalWall.Character = '\0';
            csrHorizontalWall.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrHorizontalWall.ForegroundColor");
            csrHorizontalWall.Location = new System.Drawing.Point(470, 218);
            csrHorizontalWall.Name = "csrHorizontalWall";
            csrHorizontalWall.Size = new System.Drawing.Size(211, 83);
            csrHorizontalWall.TabIndex = 136;
            csrHorizontalWall.PropertyChanged += csrHorizontalWall_PropertyChanged;
            // 
            // label135
            // 
            label135.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label135.Location = new System.Drawing.Point(253, 183);
            label135.Name = "label135";
            label135.Size = new System.Drawing.Size(211, 32);
            label135.TabIndex = 133;
            label135.Text = "Bottom Right Corner";
            label135.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrBottomRightWall
            // 
            csrBottomRightWall.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrBottomRightWall.BackgroundColor");
            csrBottomRightWall.Character = '\0';
            csrBottomRightWall.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrBottomRightWall.ForegroundColor");
            csrBottomRightWall.Location = new System.Drawing.Point(253, 218);
            csrBottomRightWall.Name = "csrBottomRightWall";
            csrBottomRightWall.Size = new System.Drawing.Size(211, 83);
            csrBottomRightWall.TabIndex = 134;
            csrBottomRightWall.PropertyChanged += csrBottomRightWall_PropertyChanged;
            // 
            // label136
            // 
            label136.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label136.Location = new System.Drawing.Point(36, 183);
            label136.Name = "label136";
            label136.Size = new System.Drawing.Size(211, 32);
            label136.TabIndex = 131;
            label136.Text = "Bottom Left Corner";
            label136.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrBottomLeftWall
            // 
            csrBottomLeftWall.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrBottomLeftWall.BackgroundColor");
            csrBottomLeftWall.Character = '\0';
            csrBottomLeftWall.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrBottomLeftWall.ForegroundColor");
            csrBottomLeftWall.Location = new System.Drawing.Point(36, 218);
            csrBottomLeftWall.Name = "csrBottomLeftWall";
            csrBottomLeftWall.Size = new System.Drawing.Size(211, 83);
            csrBottomLeftWall.TabIndex = 132;
            csrBottomLeftWall.PropertyChanged += csrBottomLeftWall_PropertyChanged;
            // 
            // label133
            // 
            label133.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label133.Location = new System.Drawing.Point(470, 62);
            label133.Name = "label133";
            label133.Size = new System.Drawing.Size(211, 32);
            label133.TabIndex = 129;
            label133.Text = "Vertical";
            label133.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrVerticalWall
            // 
            csrVerticalWall.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrVerticalWall.BackgroundColor");
            csrVerticalWall.Character = '\0';
            csrVerticalWall.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrVerticalWall.ForegroundColor");
            csrVerticalWall.Location = new System.Drawing.Point(470, 97);
            csrVerticalWall.Name = "csrVerticalWall";
            csrVerticalWall.Size = new System.Drawing.Size(211, 83);
            csrVerticalWall.TabIndex = 130;
            csrVerticalWall.PropertyChanged += csrVerticalWall_PropertyChanged;
            // 
            // label132
            // 
            label132.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label132.Location = new System.Drawing.Point(253, 62);
            label132.Name = "label132";
            label132.Size = new System.Drawing.Size(211, 32);
            label132.TabIndex = 127;
            label132.Text = "Top Right Corner";
            label132.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrTopRightWall
            // 
            csrTopRightWall.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrTopRightWall.BackgroundColor");
            csrTopRightWall.Character = '\0';
            csrTopRightWall.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrTopRightWall.ForegroundColor");
            csrTopRightWall.Location = new System.Drawing.Point(253, 97);
            csrTopRightWall.Name = "csrTopRightWall";
            csrTopRightWall.Size = new System.Drawing.Size(211, 83);
            csrTopRightWall.TabIndex = 128;
            csrTopRightWall.PropertyChanged += csrTopRightWall_PropertyChanged;
            // 
            // label131
            // 
            label131.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label131.Location = new System.Drawing.Point(253, 10);
            label131.Name = "label131";
            label131.Size = new System.Drawing.Size(211, 52);
            label131.TabIndex = 126;
            label131.Text = "WALLS";
            label131.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label130
            // 
            label130.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label130.Location = new System.Drawing.Point(36, 62);
            label130.Name = "label130";
            label130.Size = new System.Drawing.Size(211, 32);
            label130.TabIndex = 124;
            label130.Text = "Top Left Corner";
            label130.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrTopLeftWall
            // 
            csrTopLeftWall.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrTopLeftWall.BackgroundColor");
            csrTopLeftWall.Character = '\0';
            csrTopLeftWall.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrTopLeftWall.ForegroundColor");
            csrTopLeftWall.Location = new System.Drawing.Point(36, 97);
            csrTopLeftWall.Name = "csrTopLeftWall";
            csrTopLeftWall.Size = new System.Drawing.Size(211, 83);
            csrTopLeftWall.TabIndex = 125;
            csrTopLeftWall.PropertyChanged += csrTopLeftWall_PropertyChanged;
            // 
            // tpFloorInfos
            // 
            tpFloorInfos.AutoScroll = true;
            tpFloorInfos.Controls.Add(btnPasteAlgorithm);
            tpFloorInfos.Controls.Add(btnCopyAlgorithm);
            tpFloorInfos.Controls.Add(nudHungerLostPerTurn);
            tpFloorInfos.Controls.Add(label31);
            tpFloorInfos.Controls.Add(saeOnFloorStart);
            tpFloorInfos.Controls.Add(cmbTilesets);
            tpFloorInfos.Controls.Add(label155);
            tpFloorInfos.Controls.Add(nudRoomFusionOdds);
            tpFloorInfos.Controls.Add(label19);
            tpFloorInfos.Controls.Add(nudExtraRoomConnectionOdds);
            tpFloorInfos.Controls.Add(label18);
            tpFloorInfos.Controls.Add(nudMaxRoomConnections);
            tpFloorInfos.Controls.Add(label5);
            tpFloorInfos.Controls.Add(btnRemoveAlgorithm);
            tpFloorInfos.Controls.Add(btnEditAlgorithm);
            tpFloorInfos.Controls.Add(btnAddAlgorithm);
            tpFloorInfos.Controls.Add(lvFloorAlgorithms);
            tpFloorInfos.Controls.Add(label17);
            tpFloorInfos.Controls.Add(label16);
            tpFloorInfos.Controls.Add(btnTrapGenerator);
            tpFloorInfos.Controls.Add(label15);
            tpFloorInfos.Controls.Add(btnItemGenerator);
            tpFloorInfos.Controls.Add(label14);
            tpFloorInfos.Controls.Add(btnNPCGenerator);
            tpFloorInfos.Controls.Add(label13);
            tpFloorInfos.Controls.Add(nudHeight);
            tpFloorInfos.Controls.Add(nudWidth);
            tpFloorInfos.Controls.Add(label12);
            tpFloorInfos.Controls.Add(label11);
            tpFloorInfos.Controls.Add(fklblStairsReminder);
            tpFloorInfos.Controls.Add(chkGenerateStairsOnStart);
            tpFloorInfos.Controls.Add(nudMaxFloorLevel);
            tpFloorInfos.Controls.Add(label10);
            tpFloorInfos.Controls.Add(nudMinFloorLevel);
            tpFloorInfos.Controls.Add(label9);
            tpFloorInfos.Location = new System.Drawing.Point(4, 24);
            tpFloorInfos.Name = "tpFloorInfos";
            tpFloorInfos.Size = new System.Drawing.Size(740, 356);
            tpFloorInfos.TabIndex = 2;
            tpFloorInfos.Text = "Floor Group";
            tpFloorInfos.UseVisualStyleBackColor = true;
            // 
            // btnPasteAlgorithm
            // 
            btnPasteAlgorithm.Enabled = false;
            btnPasteAlgorithm.Location = new System.Drawing.Point(583, 173);
            btnPasteAlgorithm.Name = "btnPasteAlgorithm";
            btnPasteAlgorithm.Size = new System.Drawing.Size(60, 23);
            btnPasteAlgorithm.TabIndex = 44;
            btnPasteAlgorithm.Text = "Paste";
            btnPasteAlgorithm.UseVisualStyleBackColor = true;
            btnPasteAlgorithm.Click += btnPasteAlgorithm_Click;
            // 
            // btnCopyAlgorithm
            // 
            btnCopyAlgorithm.Enabled = false;
            btnCopyAlgorithm.Location = new System.Drawing.Point(517, 173);
            btnCopyAlgorithm.Name = "btnCopyAlgorithm";
            btnCopyAlgorithm.Size = new System.Drawing.Size(60, 23);
            btnCopyAlgorithm.TabIndex = 43;
            btnCopyAlgorithm.Text = "Copy";
            btnCopyAlgorithm.UseVisualStyleBackColor = true;
            btnCopyAlgorithm.Click += btnCopyAlgorithm_Click;
            // 
            // nudHungerLostPerTurn
            // 
            nudHungerLostPerTurn.DecimalPlaces = 4;
            nudHungerLostPerTurn.Increment = new decimal(new int[] { 25, 0, 0, 262144 });
            nudHungerLostPerTurn.Location = new System.Drawing.Point(98, 341);
            nudHungerLostPerTurn.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            nudHungerLostPerTurn.Name = "nudHungerLostPerTurn";
            nudHungerLostPerTurn.Size = new System.Drawing.Size(54, 23);
            nudHungerLostPerTurn.TabIndex = 42;
            nudHungerLostPerTurn.Value = new decimal(new int[] { 25, 0, 0, 262144 });
            nudHungerLostPerTurn.ValueChanged += nudHungerLostPerTurn_ValueChanged;
            // 
            // label31
            // 
            label31.AutoSize = true;
            label31.Location = new System.Drawing.Point(12, 342);
            label31.Name = "label31";
            label31.Size = new System.Drawing.Size(232, 15);
            label31.TabIndex = 41;
            label31.Text = "Characters lose                    Hunger per turn";
            // 
            // saeOnFloorStart
            // 
            saeOnFloorStart.Action = null;
            saeOnFloorStart.ActionDescription = "When the floor starts...";
            saeOnFloorStart.ActionTypeText = "Floor Start";
            saeOnFloorStart.AutoSize = true;
            saeOnFloorStart.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            saeOnFloorStart.ClassId = null;
            saeOnFloorStart.Dungeon = null;
            saeOnFloorStart.EffectParamData = null;
            saeOnFloorStart.Location = new System.Drawing.Point(367, 306);
            saeOnFloorStart.Name = "saeOnFloorStart";
            saeOnFloorStart.PlaceholderActionName = "FloorStart";
            saeOnFloorStart.RequiresActionName = false;
            saeOnFloorStart.RequiresCondition = false;
            saeOnFloorStart.RequiresDescription = false;
            saeOnFloorStart.Size = new System.Drawing.Size(260, 32);
            saeOnFloorStart.SourceDescription = "The player";
            saeOnFloorStart.TabIndex = 40;
            saeOnFloorStart.TargetDescription = "The player";
            saeOnFloorStart.ThisDescription = "None (Don't use)";
            saeOnFloorStart.TurnEndCriteria = HelperForms.TurnEndCriteria.CannotEndTurn;
            saeOnFloorStart.UsageCriteria = HelperForms.UsageCriteria.AnyTargetAnyTime;
            // 
            // cmbTilesets
            // 
            cmbTilesets.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTilesets.FormattingEnabled = true;
            cmbTilesets.Location = new System.Drawing.Point(164, 76);
            cmbTilesets.Name = "cmbTilesets";
            cmbTilesets.Size = new System.Drawing.Size(149, 23);
            cmbTilesets.TabIndex = 39;
            cmbTilesets.SelectedIndexChanged += cmbTilesets_SelectedIndexChanged;
            // 
            // label155
            // 
            label155.AutoSize = true;
            label155.Location = new System.Drawing.Point(118, 79);
            label155.Name = "label155";
            label155.Size = new System.Drawing.Size(40, 15);
            label155.TabIndex = 38;
            label155.Text = "Tileset";
            // 
            // nudRoomFusionOdds
            // 
            nudRoomFusionOdds.Location = new System.Drawing.Point(515, 268);
            nudRoomFusionOdds.Name = "nudRoomFusionOdds";
            nudRoomFusionOdds.Size = new System.Drawing.Size(40, 23);
            nudRoomFusionOdds.TabIndex = 35;
            nudRoomFusionOdds.ValueChanged += nudRoomFusionOdds_ValueChanged;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new System.Drawing.Point(366, 270);
            label19.Name = "label19";
            label19.Size = new System.Drawing.Size(285, 15);
            label19.TabIndex = 34;
            label19.Text = "Two adjacent rooms have a               % chance to fuse";
            // 
            // nudExtraRoomConnectionOdds
            // 
            nudExtraRoomConnectionOdds.Location = new System.Drawing.Point(467, 238);
            nudExtraRoomConnectionOdds.Name = "nudExtraRoomConnectionOdds";
            nudExtraRoomConnectionOdds.Size = new System.Drawing.Size(40, 23);
            nudExtraRoomConnectionOdds.TabIndex = 33;
            nudExtraRoomConnectionOdds.ValueChanged += nudRoomConnectionOdds_ValueChanged;
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new System.Drawing.Point(367, 241);
            label18.Name = "label18";
            label18.Size = new System.Drawing.Size(323, 15);
            label18.TabIndex = 32;
            label18.Text = "(With a chance of               % of connecting more than once)";
            // 
            // nudMaxRoomConnections
            // 
            nudMaxRoomConnections.Location = new System.Drawing.Point(616, 210);
            nudMaxRoomConnections.Maximum = new decimal(new int[] { 9, 0, 0, 0 });
            nudMaxRoomConnections.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudMaxRoomConnections.Name = "nudMaxRoomConnections";
            nudMaxRoomConnections.Size = new System.Drawing.Size(33, 23);
            nudMaxRoomConnections.TabIndex = 31;
            nudMaxRoomConnections.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nudMaxRoomConnections.ValueChanged += nudMaxRoomConnections_ValueChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(367, 212);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(326, 15);
            label5.TabIndex = 30;
            label5.Text = "Rooms can connect between each other up to             time(s)";
            // 
            // btnRemoveAlgorithm
            // 
            btnRemoveAlgorithm.Enabled = false;
            btnRemoveAlgorithm.Location = new System.Drawing.Point(649, 173);
            btnRemoveAlgorithm.Name = "btnRemoveAlgorithm";
            btnRemoveAlgorithm.Size = new System.Drawing.Size(60, 23);
            btnRemoveAlgorithm.TabIndex = 29;
            btnRemoveAlgorithm.Text = "Remove";
            btnRemoveAlgorithm.UseVisualStyleBackColor = true;
            btnRemoveAlgorithm.Click += btnRemoveAlgorithm_Click;
            // 
            // btnEditAlgorithm
            // 
            btnEditAlgorithm.Enabled = false;
            btnEditAlgorithm.Location = new System.Drawing.Point(451, 173);
            btnEditAlgorithm.Name = "btnEditAlgorithm";
            btnEditAlgorithm.Size = new System.Drawing.Size(60, 23);
            btnEditAlgorithm.TabIndex = 28;
            btnEditAlgorithm.Text = "Edit";
            btnEditAlgorithm.UseVisualStyleBackColor = true;
            btnEditAlgorithm.Click += btnEditAlgorithm_Click;
            // 
            // btnAddAlgorithm
            // 
            btnAddAlgorithm.Location = new System.Drawing.Point(385, 173);
            btnAddAlgorithm.Name = "btnAddAlgorithm";
            btnAddAlgorithm.Size = new System.Drawing.Size(60, 23);
            btnAddAlgorithm.TabIndex = 27;
            btnAddAlgorithm.Text = "New...";
            btnAddAlgorithm.UseVisualStyleBackColor = true;
            btnAddAlgorithm.Click += btnAddAlgorithm_Click;
            // 
            // lvFloorAlgorithms
            // 
            lvFloorAlgorithms.Location = new System.Drawing.Point(367, 58);
            lvFloorAlgorithms.MultiSelect = false;
            lvFloorAlgorithms.Name = "lvFloorAlgorithms";
            lvFloorAlgorithms.Size = new System.Drawing.Size(349, 109);
            lvFloorAlgorithms.TabIndex = 26;
            lvFloorAlgorithms.UseCompatibleStateImageBehavior = false;
            lvFloorAlgorithms.SelectedIndexChanged += lvFloorAlgorithms_SelectedIndexChanged;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new System.Drawing.Point(479, 39);
            label17.Name = "label17";
            label17.Size = new System.Drawing.Size(127, 15);
            label17.TabIndex = 25;
            label17.Text = "Generation Algorithms";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label16.Location = new System.Drawing.Point(474, 9);
            label16.Name = "label16";
            label16.Size = new System.Drawing.Size(138, 21);
            label16.TabIndex = 24;
            label16.Text = "Floor Room Data";
            // 
            // btnTrapGenerator
            // 
            btnTrapGenerator.Location = new System.Drawing.Point(176, 309);
            btnTrapGenerator.Name = "btnTrapGenerator";
            btnTrapGenerator.Size = new System.Drawing.Size(151, 23);
            btnTrapGenerator.TabIndex = 23;
            btnTrapGenerator.Text = "Traps to be generated...";
            btnTrapGenerator.UseVisualStyleBackColor = true;
            btnTrapGenerator.Click += btnTrapGenerator_Click;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label15.Location = new System.Drawing.Point(187, 275);
            label15.Name = "label15";
            label15.Size = new System.Drawing.Size(126, 21);
            label15.TabIndex = 22;
            label15.Text = "Floor Trap Data";
            // 
            // btnItemGenerator
            // 
            btnItemGenerator.Location = new System.Drawing.Point(9, 309);
            btnItemGenerator.Name = "btnItemGenerator";
            btnItemGenerator.Size = new System.Drawing.Size(151, 23);
            btnItemGenerator.TabIndex = 21;
            btnItemGenerator.Text = "Items to be generated...";
            btnItemGenerator.UseVisualStyleBackColor = true;
            btnItemGenerator.Click += btnItemGenerator_Click;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label14.Location = new System.Drawing.Point(20, 275);
            label14.Name = "label14";
            label14.Size = new System.Drawing.Size(128, 21);
            label14.TabIndex = 20;
            label14.Text = "Floor Item Data";
            // 
            // btnNPCGenerator
            // 
            btnNPCGenerator.Location = new System.Drawing.Point(94, 241);
            btnNPCGenerator.Name = "btnNPCGenerator";
            btnNPCGenerator.Size = new System.Drawing.Size(151, 23);
            btnNPCGenerator.TabIndex = 19;
            btnNPCGenerator.Text = "NPCs to be generated...";
            btnNPCGenerator.UseVisualStyleBackColor = true;
            btnNPCGenerator.Click += btnNPCGenerator_Click;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label13.Location = new System.Drawing.Point(106, 207);
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(126, 21);
            label13.TabIndex = 18;
            label13.Text = "Floor NPC Data";
            // 
            // nudHeight
            // 
            nudHeight.Location = new System.Drawing.Point(51, 91);
            nudHeight.Maximum = new decimal(new int[] { 512, 0, 0, 0 });
            nudHeight.Name = "nudHeight";
            nudHeight.Size = new System.Drawing.Size(54, 23);
            nudHeight.TabIndex = 17;
            nudHeight.ValueChanged += nudHeight_ValueChanged;
            // 
            // nudWidth
            // 
            nudWidth.Location = new System.Drawing.Point(51, 58);
            nudWidth.Maximum = new decimal(new int[] { 512, 0, 0, 0 });
            nudWidth.Name = "nudWidth";
            nudWidth.Size = new System.Drawing.Size(54, 23);
            nudWidth.TabIndex = 16;
            nudWidth.ValueChanged += nudWidth_ValueChanged;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new System.Drawing.Point(6, 93);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(43, 15);
            label12.TabIndex = 15;
            label12.Text = "Height";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new System.Drawing.Point(6, 60);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(39, 15);
            label11.TabIndex = 14;
            label11.Text = "Width";
            // 
            // fklblStairsReminder
            // 
            fklblStairsReminder.Enabled = false;
            fklblStairsReminder.FlatAppearance.BorderSize = 0;
            fklblStairsReminder.FlatStyle = FlatStyle.Flat;
            fklblStairsReminder.Image = (System.Drawing.Image)resources.GetObject("fklblStairsReminder.Image");
            fklblStairsReminder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblStairsReminder.Location = new System.Drawing.Point(6, 158);
            fklblStairsReminder.Name = "fklblStairsReminder";
            fklblStairsReminder.Size = new System.Drawing.Size(287, 42);
            fklblStairsReminder.TabIndex = 13;
            fklblStairsReminder.Text = "Remember to include an element that\r\ngenerates Stairs, or it would softlock the game.";
            fklblStairsReminder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblStairsReminder.TextImageRelation = TextImageRelation.ImageBeforeText;
            fklblStairsReminder.UseVisualStyleBackColor = true;
            fklblStairsReminder.Visible = false;
            // 
            // chkGenerateStairsOnStart
            // 
            chkGenerateStairsOnStart.AutoSize = true;
            chkGenerateStairsOnStart.Location = new System.Drawing.Point(6, 133);
            chkGenerateStairsOnStart.Name = "chkGenerateStairsOnStart";
            chkGenerateStairsOnStart.Size = new System.Drawing.Size(214, 19);
            chkGenerateStairsOnStart.TabIndex = 4;
            chkGenerateStairsOnStart.Text = "Place Stairs when Floor is generated";
            chkGenerateStairsOnStart.UseVisualStyleBackColor = true;
            chkGenerateStairsOnStart.CheckedChanged += chkGenerateStairsOnStart_CheckedChanged;
            // 
            // nudMaxFloorLevel
            // 
            nudMaxFloorLevel.Location = new System.Drawing.Point(135, 9);
            nudMaxFloorLevel.Name = "nudMaxFloorLevel";
            nudMaxFloorLevel.Size = new System.Drawing.Size(33, 23);
            nudMaxFloorLevel.TabIndex = 3;
            nudMaxFloorLevel.ValueChanged += nudMaxFloorLevel_ValueChanged;
            nudMaxFloorLevel.Leave += nudMaxFloorLevel_Leave;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(111, 11);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(18, 15);
            label10.TabIndex = 2;
            label10.Text = "to";
            // 
            // nudMinFloorLevel
            // 
            nudMinFloorLevel.Location = new System.Drawing.Point(73, 9);
            nudMinFloorLevel.Name = "nudMinFloorLevel";
            nudMinFloorLevel.Size = new System.Drawing.Size(33, 23);
            nudMinFloorLevel.TabIndex = 1;
            nudMinFloorLevel.ValueChanged += nudMinFloorLevel_ValueChanged;
            nudMinFloorLevel.Leave += nudMinFloorLevel_Leave;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(6, 11);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(65, 15);
            label9.TabIndex = 0;
            label9.Text = "From Level";
            // 
            // tpFactionInfos
            // 
            tpFactionInfos.Controls.Add(lbEnemies);
            tpFactionInfos.Controls.Add(label26);
            tpFactionInfos.Controls.Add(btnEnemiesToNeutrals);
            tpFactionInfos.Controls.Add(btnEnemyToNeutral);
            tpFactionInfos.Controls.Add(btnNeutralsToEnemies);
            tpFactionInfos.Controls.Add(btnNeutralToEnemy);
            tpFactionInfos.Controls.Add(lbNeutrals);
            tpFactionInfos.Controls.Add(label25);
            tpFactionInfos.Controls.Add(btnNeutralsToAllies);
            tpFactionInfos.Controls.Add(btnNeutralToAlly);
            tpFactionInfos.Controls.Add(btnAlliesToNeutrals);
            tpFactionInfos.Controls.Add(btnAllyToNeutral);
            tpFactionInfos.Controls.Add(lbAllies);
            tpFactionInfos.Controls.Add(label24);
            tpFactionInfos.Controls.Add(label23);
            tpFactionInfos.Controls.Add(fklblFactionDescriptionLocale);
            tpFactionInfos.Controls.Add(txtFactionDescription);
            tpFactionInfos.Controls.Add(label22);
            tpFactionInfos.Controls.Add(fklblFactionNameLocale);
            tpFactionInfos.Controls.Add(txtFactionName);
            tpFactionInfos.Controls.Add(label21);
            tpFactionInfos.Location = new System.Drawing.Point(4, 24);
            tpFactionInfos.Name = "tpFactionInfos";
            tpFactionInfos.Size = new System.Drawing.Size(740, 356);
            tpFactionInfos.TabIndex = 3;
            tpFactionInfos.Text = "Faction";
            tpFactionInfos.UseVisualStyleBackColor = true;
            // 
            // lbEnemies
            // 
            lbEnemies.FormattingEnabled = true;
            lbEnemies.ItemHeight = 15;
            lbEnemies.Location = new System.Drawing.Point(450, 161);
            lbEnemies.Name = "lbEnemies";
            lbEnemies.Size = new System.Drawing.Size(111, 169);
            lbEnemies.TabIndex = 33;
            lbEnemies.SelectedIndexChanged += lbEnemies_SelectedIndexChanged;
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label26.Location = new System.Drawing.Point(461, 143);
            label26.Name = "label26";
            label26.Size = new System.Drawing.Size(92, 15);
            label26.TabIndex = 32;
            label26.Text = "Enemies With...";
            // 
            // btnEnemiesToNeutrals
            // 
            btnEnemiesToNeutrals.Enabled = false;
            btnEnemiesToNeutrals.Location = new System.Drawing.Point(407, 211);
            btnEnemiesToNeutrals.Name = "btnEnemiesToNeutrals";
            btnEnemiesToNeutrals.Size = new System.Drawing.Size(37, 23);
            btnEnemiesToNeutrals.TabIndex = 31;
            btnEnemiesToNeutrals.Text = "<<";
            btnEnemiesToNeutrals.UseVisualStyleBackColor = true;
            btnEnemiesToNeutrals.Click += btnEnemiesToNeutrals_Click;
            // 
            // btnEnemyToNeutral
            // 
            btnEnemyToNeutral.Enabled = false;
            btnEnemyToNeutral.Location = new System.Drawing.Point(407, 182);
            btnEnemyToNeutral.Name = "btnEnemyToNeutral";
            btnEnemyToNeutral.Size = new System.Drawing.Size(37, 23);
            btnEnemyToNeutral.TabIndex = 30;
            btnEnemyToNeutral.Text = "<";
            btnEnemyToNeutral.UseVisualStyleBackColor = true;
            btnEnemyToNeutral.Click += btnEnemyToNeutral_Click;
            // 
            // btnNeutralsToEnemies
            // 
            btnNeutralsToEnemies.Enabled = false;
            btnNeutralsToEnemies.Location = new System.Drawing.Point(407, 284);
            btnNeutralsToEnemies.Name = "btnNeutralsToEnemies";
            btnNeutralsToEnemies.Size = new System.Drawing.Size(37, 23);
            btnNeutralsToEnemies.TabIndex = 29;
            btnNeutralsToEnemies.Text = ">>";
            btnNeutralsToEnemies.UseVisualStyleBackColor = true;
            btnNeutralsToEnemies.Click += btnNeutralsToEnemies_Click;
            // 
            // btnNeutralToEnemy
            // 
            btnNeutralToEnemy.Enabled = false;
            btnNeutralToEnemy.Location = new System.Drawing.Point(407, 255);
            btnNeutralToEnemy.Name = "btnNeutralToEnemy";
            btnNeutralToEnemy.Size = new System.Drawing.Size(37, 23);
            btnNeutralToEnemy.TabIndex = 28;
            btnNeutralToEnemy.Text = ">";
            btnNeutralToEnemy.UseVisualStyleBackColor = true;
            btnNeutralToEnemy.Click += btnNeutralToEnemy_Click;
            // 
            // lbNeutrals
            // 
            lbNeutrals.FormattingEnabled = true;
            lbNeutrals.ItemHeight = 15;
            lbNeutrals.Location = new System.Drawing.Point(290, 161);
            lbNeutrals.Name = "lbNeutrals";
            lbNeutrals.Size = new System.Drawing.Size(111, 169);
            lbNeutrals.TabIndex = 27;
            lbNeutrals.SelectedIndexChanged += lbNeutrals_SelectedIndexChanged;
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label25.Location = new System.Drawing.Point(303, 143);
            label25.Name = "label25";
            label25.Size = new System.Drawing.Size(88, 15);
            label25.TabIndex = 26;
            label25.Text = "Neutral With...";
            // 
            // btnNeutralsToAllies
            // 
            btnNeutralsToAllies.Enabled = false;
            btnNeutralsToAllies.Location = new System.Drawing.Point(247, 211);
            btnNeutralsToAllies.Name = "btnNeutralsToAllies";
            btnNeutralsToAllies.Size = new System.Drawing.Size(37, 23);
            btnNeutralsToAllies.TabIndex = 25;
            btnNeutralsToAllies.Text = "<<";
            btnNeutralsToAllies.UseVisualStyleBackColor = true;
            btnNeutralsToAllies.Click += btnNeutralsToAllies_Click;
            // 
            // btnNeutralToAlly
            // 
            btnNeutralToAlly.Enabled = false;
            btnNeutralToAlly.Location = new System.Drawing.Point(247, 182);
            btnNeutralToAlly.Name = "btnNeutralToAlly";
            btnNeutralToAlly.Size = new System.Drawing.Size(37, 23);
            btnNeutralToAlly.TabIndex = 24;
            btnNeutralToAlly.Text = "<";
            btnNeutralToAlly.UseVisualStyleBackColor = true;
            btnNeutralToAlly.Click += btnNeutralToAlly_Click;
            // 
            // btnAlliesToNeutrals
            // 
            btnAlliesToNeutrals.Enabled = false;
            btnAlliesToNeutrals.Location = new System.Drawing.Point(247, 284);
            btnAlliesToNeutrals.Name = "btnAlliesToNeutrals";
            btnAlliesToNeutrals.Size = new System.Drawing.Size(37, 23);
            btnAlliesToNeutrals.TabIndex = 23;
            btnAlliesToNeutrals.Text = ">>";
            btnAlliesToNeutrals.UseVisualStyleBackColor = true;
            btnAlliesToNeutrals.Click += btnAlliesToNeutrals_Click;
            // 
            // btnAllyToNeutral
            // 
            btnAllyToNeutral.Enabled = false;
            btnAllyToNeutral.Location = new System.Drawing.Point(247, 255);
            btnAllyToNeutral.Name = "btnAllyToNeutral";
            btnAllyToNeutral.Size = new System.Drawing.Size(37, 23);
            btnAllyToNeutral.TabIndex = 22;
            btnAllyToNeutral.Text = ">";
            btnAllyToNeutral.UseVisualStyleBackColor = true;
            btnAllyToNeutral.Click += btnAllyToNeutral_Click;
            // 
            // lbAllies
            // 
            lbAllies.FormattingEnabled = true;
            lbAllies.ItemHeight = 15;
            lbAllies.Location = new System.Drawing.Point(130, 161);
            lbAllies.Name = "lbAllies";
            lbAllies.Size = new System.Drawing.Size(111, 169);
            lbAllies.TabIndex = 21;
            lbAllies.SelectedIndexChanged += lbAllies_SelectedIndexChanged;
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label24.Location = new System.Drawing.Point(147, 143);
            label24.Name = "label24";
            label24.Size = new System.Drawing.Size(77, 15);
            label24.TabIndex = 20;
            label24.Text = "Allied With...";
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label23.Location = new System.Drawing.Point(296, 109);
            label23.Name = "label23";
            label23.Size = new System.Drawing.Size(98, 21);
            label23.TabIndex = 19;
            label23.Text = "Allegiances";
            // 
            // fklblFactionDescriptionLocale
            // 
            fklblFactionDescriptionLocale.Enabled = false;
            fklblFactionDescriptionLocale.FlatAppearance.BorderSize = 0;
            fklblFactionDescriptionLocale.FlatStyle = FlatStyle.Flat;
            fklblFactionDescriptionLocale.Image = (System.Drawing.Image)resources.GetObject("fklblFactionDescriptionLocale.Image");
            fklblFactionDescriptionLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblFactionDescriptionLocale.Location = new System.Drawing.Point(375, 59);
            fklblFactionDescriptionLocale.Name = "fklblFactionDescriptionLocale";
            fklblFactionDescriptionLocale.Size = new System.Drawing.Size(331, 42);
            fklblFactionDescriptionLocale.TabIndex = 18;
            fklblFactionDescriptionLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblFactionDescriptionLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblFactionDescriptionLocale.TextImageRelation = TextImageRelation.ImageBeforeText;
            fklblFactionDescriptionLocale.UseVisualStyleBackColor = true;
            fklblFactionDescriptionLocale.Visible = false;
            // 
            // txtFactionDescription
            // 
            txtFactionDescription.Location = new System.Drawing.Point(375, 30);
            txtFactionDescription.Name = "txtFactionDescription";
            txtFactionDescription.Size = new System.Drawing.Size(359, 23);
            txtFactionDescription.TabIndex = 17;
            txtFactionDescription.TextChanged += txtFactionDescription_TextChanged;
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new System.Drawing.Point(375, 12);
            label22.Name = "label22";
            label22.Size = new System.Drawing.Size(67, 15);
            label22.TabIndex = 16;
            label22.Text = "Description";
            // 
            // fklblFactionNameLocale
            // 
            fklblFactionNameLocale.Enabled = false;
            fklblFactionNameLocale.FlatAppearance.BorderSize = 0;
            fklblFactionNameLocale.FlatStyle = FlatStyle.Flat;
            fklblFactionNameLocale.Image = (System.Drawing.Image)resources.GetObject("fklblFactionNameLocale.Image");
            fklblFactionNameLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblFactionNameLocale.Location = new System.Drawing.Point(13, 59);
            fklblFactionNameLocale.Name = "fklblFactionNameLocale";
            fklblFactionNameLocale.Size = new System.Drawing.Size(331, 42);
            fklblFactionNameLocale.TabIndex = 15;
            fklblFactionNameLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblFactionNameLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblFactionNameLocale.TextImageRelation = TextImageRelation.ImageBeforeText;
            fklblFactionNameLocale.UseVisualStyleBackColor = true;
            fklblFactionNameLocale.Visible = false;
            // 
            // txtFactionName
            // 
            txtFactionName.Location = new System.Drawing.Point(13, 30);
            txtFactionName.Name = "txtFactionName";
            txtFactionName.Size = new System.Drawing.Size(359, 23);
            txtFactionName.TabIndex = 14;
            txtFactionName.TextChanged += txtFactionName_TextChanged;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new System.Drawing.Point(13, 12);
            label21.Name = "label21";
            label21.Size = new System.Drawing.Size(39, 15);
            label21.TabIndex = 13;
            label21.Text = "Name";
            // 
            // tpPlayerClass
            // 
            tpPlayerClass.AutoScroll = true;
            tpPlayerClass.Controls.Add(ssPlayer);
            tpPlayerClass.Controls.Add(sisPlayerStartingInventory);
            tpPlayerClass.Controls.Add(saePlayerOnDeath);
            tpPlayerClass.Controls.Add(saePlayerOnAttacked);
            tpPlayerClass.Controls.Add(saePlayerOnTurnStart);
            tpPlayerClass.Controls.Add(maePlayerOnAttack);
            tpPlayerClass.Controls.Add(label58);
            tpPlayerClass.Controls.Add(cmbPlayerStartingArmor);
            tpPlayerClass.Controls.Add(label57);
            tpPlayerClass.Controls.Add(cmbPlayerStartingWeapon);
            tpPlayerClass.Controls.Add(label56);
            tpPlayerClass.Controls.Add(label54);
            tpPlayerClass.Controls.Add(nudPlayerInventorySize);
            tpPlayerClass.Controls.Add(label53);
            tpPlayerClass.Controls.Add(label30);
            tpPlayerClass.Controls.Add(chkPlayerStartsVisible);
            tpPlayerClass.Controls.Add(cmbPlayerFaction);
            tpPlayerClass.Controls.Add(label29);
            tpPlayerClass.Controls.Add(chkRequirePlayerPrompt);
            tpPlayerClass.Controls.Add(fklblPlayerClassDescriptionLocale);
            tpPlayerClass.Controls.Add(txtPlayerClassDescription);
            tpPlayerClass.Controls.Add(label28);
            tpPlayerClass.Controls.Add(fklblPlayerClassNameLocale);
            tpPlayerClass.Controls.Add(txtPlayerClassName);
            tpPlayerClass.Controls.Add(label27);
            tpPlayerClass.Controls.Add(crsPlayer);
            tpPlayerClass.Location = new System.Drawing.Point(4, 24);
            tpPlayerClass.Name = "tpPlayerClass";
            tpPlayerClass.Size = new System.Drawing.Size(740, 356);
            tpPlayerClass.TabIndex = 4;
            tpPlayerClass.Text = "Player Class";
            tpPlayerClass.UseVisualStyleBackColor = true;
            // 
            // ssPlayer
            // 
            ssPlayer.AttackPerLevelUp = new decimal(new int[] { 0, 0, 0, 0 });
            ssPlayer.AutoSize = true;
            ssPlayer.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ssPlayer.BaseAccuracy = 100;
            ssPlayer.BaseAttack = 0;
            ssPlayer.BaseDefense = 0;
            ssPlayer.BaseEvasion = 0;
            ssPlayer.BaseHP = 1;
            ssPlayer.BaseHPRegeneration = new decimal(new int[] { 1, 0, 0, 0 });
            ssPlayer.BaseHunger = 0;
            ssPlayer.BaseMovement = 1;
            ssPlayer.BaseMP = 0;
            ssPlayer.BaseMPRegeneration = new decimal(new int[] { 0, 0, 0, 0 });
            ssPlayer.BaseSightRangeDisplayNames = (System.Collections.Generic.Dictionary<string, string>)resources.GetObject("ssPlayer.BaseSightRangeDisplayNames");
            ssPlayer.CanGainExperience = false;
            ssPlayer.DefensePerLevelUp = new decimal(new int[] { 0, 0, 0, 0 });
            ssPlayer.ExperienceToLevelUpFormula = "";
            ssPlayer.HPPerLevelUp = new decimal(new int[] { 0, 0, 0, 0 });
            ssPlayer.HPRegenerationPerLevelUp = new decimal(new int[] { 0, 0, 0, 0 });
            ssPlayer.HungerHPDegeneration = new decimal(new int[] { 0, 0, 0, 0 });
            ssPlayer.Location = new System.Drawing.Point(385, 94);
            ssPlayer.MaxLevel = 1;
            ssPlayer.MovementPerLevelUp = new decimal(new int[] { 0, 0, 0, 0 });
            ssPlayer.MPPerLevelUp = new decimal(new int[] { 0, 0, 0, 0 });
            ssPlayer.MPRegenerationPerLevelUp = new decimal(new int[] { 0, 0, 0, 0 });
            ssPlayer.Name = "ssPlayer";
            ssPlayer.Size = new System.Drawing.Size(331, 772);
            ssPlayer.TabIndex = 129;
            ssPlayer.UsesHunger = false;
            ssPlayer.UsesMP = false;
            // 
            // sisPlayerStartingInventory
            // 
            sisPlayerStartingInventory.Inventory = (System.Collections.Generic.List<string>)resources.GetObject("sisPlayerStartingInventory.Inventory");
            sisPlayerStartingInventory.InventorySize = 0;
            sisPlayerStartingInventory.Location = new System.Drawing.Point(13, 396);
            sisPlayerStartingInventory.Name = "sisPlayerStartingInventory";
            sisPlayerStartingInventory.SelectableItems = null;
            sisPlayerStartingInventory.Size = new System.Drawing.Size(293, 79);
            sisPlayerStartingInventory.TabIndex = 128;
            // 
            // saePlayerOnDeath
            // 
            saePlayerOnDeath.Action = null;
            saePlayerOnDeath.ActionDescription = "When they die...                   ";
            saePlayerOnDeath.ActionTypeText = "On Death";
            saePlayerOnDeath.AutoSize = true;
            saePlayerOnDeath.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            saePlayerOnDeath.ClassId = null;
            saePlayerOnDeath.Dungeon = null;
            saePlayerOnDeath.EffectParamData = null;
            saePlayerOnDeath.Location = new System.Drawing.Point(13, 686);
            saePlayerOnDeath.Name = "saePlayerOnDeath";
            saePlayerOnDeath.PlaceholderActionName = "Death";
            saePlayerOnDeath.RequiresActionName = false;
            saePlayerOnDeath.RequiresCondition = false;
            saePlayerOnDeath.RequiresDescription = false;
            saePlayerOnDeath.Size = new System.Drawing.Size(283, 32);
            saePlayerOnDeath.SourceDescription = "The player";
            saePlayerOnDeath.TabIndex = 127;
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
            saePlayerOnAttacked.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            saePlayerOnAttacked.ClassId = null;
            saePlayerOnAttacked.Dungeon = null;
            saePlayerOnAttacked.EffectParamData = null;
            saePlayerOnAttacked.Location = new System.Drawing.Point(13, 648);
            saePlayerOnAttacked.Name = "saePlayerOnAttacked";
            saePlayerOnAttacked.PlaceholderActionName = "Interacted";
            saePlayerOnAttacked.RequiresActionName = false;
            saePlayerOnAttacked.RequiresCondition = false;
            saePlayerOnAttacked.RequiresDescription = false;
            saePlayerOnAttacked.Size = new System.Drawing.Size(284, 32);
            saePlayerOnAttacked.SourceDescription = "The player";
            saePlayerOnAttacked.TabIndex = 126;
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
            saePlayerOnTurnStart.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            saePlayerOnTurnStart.ClassId = null;
            saePlayerOnTurnStart.Dungeon = null;
            saePlayerOnTurnStart.EffectParamData = null;
            saePlayerOnTurnStart.Location = new System.Drawing.Point(13, 510);
            saePlayerOnTurnStart.Name = "saePlayerOnTurnStart";
            saePlayerOnTurnStart.PlaceholderActionName = "TurnStart";
            saePlayerOnTurnStart.RequiresActionName = false;
            saePlayerOnTurnStart.RequiresCondition = false;
            saePlayerOnTurnStart.RequiresDescription = false;
            saePlayerOnTurnStart.Size = new System.Drawing.Size(283, 32);
            saePlayerOnTurnStart.SourceDescription = "The player";
            saePlayerOnTurnStart.TabIndex = 125;
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
            maePlayerOnAttack.Location = new System.Drawing.Point(13, 548);
            maePlayerOnAttack.Name = "maePlayerOnAttack";
            maePlayerOnAttack.PlaceholderActionName = null;
            maePlayerOnAttack.RequiresActionName = true;
            maePlayerOnAttack.RequiresCondition = true;
            maePlayerOnAttack.RequiresDescription = true;
            maePlayerOnAttack.Size = new System.Drawing.Size(368, 94);
            maePlayerOnAttack.SourceDescription = "The player";
            maePlayerOnAttack.TabIndex = 124;
            maePlayerOnAttack.TargetDescription = "Whoever they are targeting";
            maePlayerOnAttack.ThisDescription = "The player";
            maePlayerOnAttack.TurnEndCriteria = HelperForms.TurnEndCriteria.MustEndTurn;
            maePlayerOnAttack.UsageCriteria = HelperForms.UsageCriteria.FullConditions;
            // 
            // label58
            // 
            label58.AutoSize = true;
            label58.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label58.Location = new System.Drawing.Point(141, 483);
            label58.Name = "label58";
            label58.Size = new System.Drawing.Size(67, 21);
            label58.TabIndex = 104;
            label58.Text = "Actions";
            // 
            // cmbPlayerStartingArmor
            // 
            cmbPlayerStartingArmor.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPlayerStartingArmor.FormattingEnabled = true;
            cmbPlayerStartingArmor.Location = new System.Drawing.Point(146, 331);
            cmbPlayerStartingArmor.Name = "cmbPlayerStartingArmor";
            cmbPlayerStartingArmor.Size = new System.Drawing.Size(158, 23);
            cmbPlayerStartingArmor.TabIndex = 81;
            cmbPlayerStartingArmor.SelectedIndexChanged += cmbPlayerStartingArmor_SelectedIndexChanged;
            // 
            // label57
            // 
            label57.AutoSize = true;
            label57.Location = new System.Drawing.Point(13, 334);
            label57.Name = "label57";
            label57.Size = new System.Drawing.Size(131, 15);
            label57.TabIndex = 80;
            label57.Text = "When unarmored, wear";
            // 
            // cmbPlayerStartingWeapon
            // 
            cmbPlayerStartingWeapon.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPlayerStartingWeapon.FormattingEnabled = true;
            cmbPlayerStartingWeapon.Location = new System.Drawing.Point(139, 300);
            cmbPlayerStartingWeapon.Name = "cmbPlayerStartingWeapon";
            cmbPlayerStartingWeapon.Size = new System.Drawing.Size(165, 23);
            cmbPlayerStartingWeapon.TabIndex = 79;
            cmbPlayerStartingWeapon.SelectedIndexChanged += cmbPlayerStartingWeapon_SelectedIndexChanged;
            // 
            // label56
            // 
            label56.AutoSize = true;
            label56.Location = new System.Drawing.Point(13, 303);
            label56.Name = "label56";
            label56.Size = new System.Drawing.Size(123, 15);
            label56.TabIndex = 78;
            label56.Text = "When unarmed, wield";
            // 
            // label54
            // 
            label54.AutoSize = true;
            label54.Location = new System.Drawing.Point(172, 364);
            label54.Name = "label54";
            label54.Size = new System.Drawing.Size(36, 15);
            label54.TabIndex = 72;
            label54.Text = "items";
            // 
            // nudPlayerInventorySize
            // 
            nudPlayerInventorySize.Location = new System.Drawing.Point(121, 359);
            nudPlayerInventorySize.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            nudPlayerInventorySize.Name = "nudPlayerInventorySize";
            nudPlayerInventorySize.Size = new System.Drawing.Size(45, 23);
            nudPlayerInventorySize.TabIndex = 71;
            nudPlayerInventorySize.ValueChanged += nudPlayerInventorySize_ValueChanged;
            // 
            // label53
            // 
            label53.AutoSize = true;
            label53.Location = new System.Drawing.Point(13, 362);
            label53.Name = "label53";
            label53.Size = new System.Drawing.Size(109, 15);
            label53.TabIndex = 70;
            label53.Text = "Inventory Capacity:";
            // 
            // label30
            // 
            label30.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label30.Location = new System.Drawing.Point(370, 19);
            label30.Name = "label30";
            label30.Size = new System.Drawing.Size(131, 52);
            label30.TabIndex = 26;
            label30.Text = "Appearance";
            label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkPlayerStartsVisible
            // 
            chkPlayerStartsVisible.AutoSize = true;
            chkPlayerStartsVisible.Location = new System.Drawing.Point(13, 272);
            chkPlayerStartsVisible.Name = "chkPlayerStartsVisible";
            chkPlayerStartsVisible.Size = new System.Drawing.Size(102, 19);
            chkPlayerStartsVisible.TabIndex = 25;
            chkPlayerStartsVisible.Text = "Spawns visible";
            chkPlayerStartsVisible.UseVisualStyleBackColor = true;
            chkPlayerStartsVisible.CheckedChanged += chkPlayerStartsVisible_CheckedChanged;
            // 
            // cmbPlayerFaction
            // 
            cmbPlayerFaction.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPlayerFaction.FormattingEnabled = true;
            cmbPlayerFaction.Location = new System.Drawing.Point(65, 241);
            cmbPlayerFaction.Name = "cmbPlayerFaction";
            cmbPlayerFaction.Size = new System.Drawing.Size(146, 23);
            cmbPlayerFaction.TabIndex = 24;
            cmbPlayerFaction.SelectedIndexChanged += cmbPlayerFaction_SelectedIndexChanged;
            // 
            // label29
            // 
            label29.AutoSize = true;
            label29.Location = new System.Drawing.Point(13, 244);
            label29.Name = "label29";
            label29.Size = new System.Drawing.Size(46, 15);
            label29.TabIndex = 23;
            label29.Text = "Faction";
            // 
            // chkRequirePlayerPrompt
            // 
            chkRequirePlayerPrompt.AutoSize = true;
            chkRequirePlayerPrompt.Location = new System.Drawing.Point(13, 107);
            chkRequirePlayerPrompt.Name = "chkRequirePlayerPrompt";
            chkRequirePlayerPrompt.Size = new System.Drawing.Size(287, 19);
            chkRequirePlayerPrompt.TabIndex = 22;
            chkRequirePlayerPrompt.Text = "Player will have to provide a name upon selection";
            chkRequirePlayerPrompt.UseVisualStyleBackColor = true;
            chkRequirePlayerPrompt.CheckedChanged += chkRequirePlayerPrompt_CheckedChanged;
            // 
            // fklblPlayerClassDescriptionLocale
            // 
            fklblPlayerClassDescriptionLocale.Enabled = false;
            fklblPlayerClassDescriptionLocale.FlatAppearance.BorderSize = 0;
            fklblPlayerClassDescriptionLocale.FlatStyle = FlatStyle.Flat;
            fklblPlayerClassDescriptionLocale.Image = (System.Drawing.Image)resources.GetObject("fklblPlayerClassDescriptionLocale.Image");
            fklblPlayerClassDescriptionLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblPlayerClassDescriptionLocale.Location = new System.Drawing.Point(13, 185);
            fklblPlayerClassDescriptionLocale.Name = "fklblPlayerClassDescriptionLocale";
            fklblPlayerClassDescriptionLocale.Size = new System.Drawing.Size(331, 42);
            fklblPlayerClassDescriptionLocale.TabIndex = 21;
            fklblPlayerClassDescriptionLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblPlayerClassDescriptionLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblPlayerClassDescriptionLocale.TextImageRelation = TextImageRelation.ImageBeforeText;
            fklblPlayerClassDescriptionLocale.UseVisualStyleBackColor = true;
            fklblPlayerClassDescriptionLocale.Visible = false;
            // 
            // txtPlayerClassDescription
            // 
            txtPlayerClassDescription.Location = new System.Drawing.Point(13, 156);
            txtPlayerClassDescription.Name = "txtPlayerClassDescription";
            txtPlayerClassDescription.Size = new System.Drawing.Size(350, 23);
            txtPlayerClassDescription.TabIndex = 20;
            txtPlayerClassDescription.TextChanged += txtPlayerClassDescription_TextChanged;
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.Location = new System.Drawing.Point(13, 138);
            label28.Name = "label28";
            label28.Size = new System.Drawing.Size(67, 15);
            label28.TabIndex = 19;
            label28.Text = "Description";
            // 
            // fklblPlayerClassNameLocale
            // 
            fklblPlayerClassNameLocale.Enabled = false;
            fklblPlayerClassNameLocale.FlatAppearance.BorderSize = 0;
            fklblPlayerClassNameLocale.FlatStyle = FlatStyle.Flat;
            fklblPlayerClassNameLocale.Image = (System.Drawing.Image)resources.GetObject("fklblPlayerClassNameLocale.Image");
            fklblPlayerClassNameLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblPlayerClassNameLocale.Location = new System.Drawing.Point(13, 55);
            fklblPlayerClassNameLocale.Name = "fklblPlayerClassNameLocale";
            fklblPlayerClassNameLocale.Size = new System.Drawing.Size(331, 42);
            fklblPlayerClassNameLocale.TabIndex = 18;
            fklblPlayerClassNameLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblPlayerClassNameLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblPlayerClassNameLocale.TextImageRelation = TextImageRelation.ImageBeforeText;
            fklblPlayerClassNameLocale.UseVisualStyleBackColor = true;
            fklblPlayerClassNameLocale.Visible = false;
            // 
            // txtPlayerClassName
            // 
            txtPlayerClassName.Location = new System.Drawing.Point(13, 26);
            txtPlayerClassName.Name = "txtPlayerClassName";
            txtPlayerClassName.Size = new System.Drawing.Size(350, 23);
            txtPlayerClassName.TabIndex = 17;
            txtPlayerClassName.TextChanged += txtPlayerClassName_TextChanged;
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.Location = new System.Drawing.Point(13, 8);
            label27.Name = "label27";
            label27.Size = new System.Drawing.Size(80, 15);
            label27.TabIndex = 16;
            label27.Text = "Default Name";
            // 
            // crsPlayer
            // 
            crsPlayer.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("crsPlayer.BackgroundColor");
            crsPlayer.Character = '\0';
            crsPlayer.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("crsPlayer.ForegroundColor");
            crsPlayer.Location = new System.Drawing.Point(504, 8);
            crsPlayer.Name = "crsPlayer";
            crsPlayer.Size = new System.Drawing.Size(211, 83);
            crsPlayer.TabIndex = 123;
            crsPlayer.PropertyChanged += crsPlayer_PropertyChanged;
            // 
            // tpNPC
            // 
            tpNPC.AutoScroll = true;
            tpNPC.Controls.Add(lblNPCAIOddsToTargetSelfB);
            tpNPC.Controls.Add(nudNPCOddsToTargetSelf);
            tpNPC.Controls.Add(cmbNPCAIType);
            tpNPC.Controls.Add(label20);
            tpNPC.Controls.Add(maeNPCOnInteracted);
            tpNPC.Controls.Add(saeNPCOnSpawn);
            tpNPC.Controls.Add(ssNPC);
            tpNPC.Controls.Add(sisNPCStartingInventory);
            tpNPC.Controls.Add(saeNPCOnDeath);
            tpNPC.Controls.Add(saeNPCOnAttacked);
            tpNPC.Controls.Add(saeNPCOnTurnStart);
            tpNPC.Controls.Add(maeNPCOnAttack);
            tpNPC.Controls.Add(lblNPCAIOddsToTargetSelfA);
            tpNPC.Controls.Add(txtNPCExperiencePayout);
            tpNPC.Controls.Add(label103);
            tpNPC.Controls.Add(chkNPCKnowsAllCharacterPositions);
            tpNPC.Controls.Add(label67);
            tpNPC.Controls.Add(cmbNPCStartingArmor);
            tpNPC.Controls.Add(label70);
            tpNPC.Controls.Add(cmbNPCStartingWeapon);
            tpNPC.Controls.Add(label71);
            tpNPC.Controls.Add(label73);
            tpNPC.Controls.Add(nudNPCInventorySize);
            tpNPC.Controls.Add(label74);
            tpNPC.Controls.Add(label98);
            tpNPC.Controls.Add(chkNPCStartsVisible);
            tpNPC.Controls.Add(cmbNPCFaction);
            tpNPC.Controls.Add(label99);
            tpNPC.Controls.Add(fklblNPCDescriptionLocale);
            tpNPC.Controls.Add(txtNPCDescription);
            tpNPC.Controls.Add(label100);
            tpNPC.Controls.Add(fklblNPCNameLocale);
            tpNPC.Controls.Add(txtNPCName);
            tpNPC.Controls.Add(label101);
            tpNPC.Controls.Add(crsNPC);
            tpNPC.Location = new System.Drawing.Point(4, 24);
            tpNPC.Name = "tpNPC";
            tpNPC.Size = new System.Drawing.Size(740, 356);
            tpNPC.TabIndex = 5;
            tpNPC.Text = "NPC";
            tpNPC.UseVisualStyleBackColor = true;
            // 
            // lblNPCAIOddsToTargetSelfB
            // 
            lblNPCAIOddsToTargetSelfB.AutoSize = true;
            lblNPCAIOddsToTargetSelfB.Location = new System.Drawing.Point(330, 938);
            lblNPCAIOddsToTargetSelfB.Name = "lblNPCAIOddsToTargetSelfB";
            lblNPCAIOddsToTargetSelfB.Size = new System.Drawing.Size(17, 15);
            lblNPCAIOddsToTargetSelfB.TabIndex = 217;
            lblNPCAIOddsToTargetSelfB.Text = "%";
            lblNPCAIOddsToTargetSelfB.Visible = false;
            // 
            // nudNPCOddsToTargetSelf
            // 
            nudNPCOddsToTargetSelf.Location = new System.Drawing.Point(287, 936);
            nudNPCOddsToTargetSelf.Name = "nudNPCOddsToTargetSelf";
            nudNPCOddsToTargetSelf.Size = new System.Drawing.Size(41, 23);
            nudNPCOddsToTargetSelf.TabIndex = 195;
            nudNPCOddsToTargetSelf.Visible = false;
            nudNPCOddsToTargetSelf.ValueChanged += nudNPCOddsToTargetSelf_ValueChanged;
            // 
            // cmbNPCAIType
            // 
            cmbNPCAIType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbNPCAIType.FormattingEnabled = true;
            cmbNPCAIType.Location = new System.Drawing.Point(201, 904);
            cmbNPCAIType.Name = "cmbNPCAIType";
            cmbNPCAIType.Size = new System.Drawing.Size(146, 23);
            cmbNPCAIType.TabIndex = 216;
            cmbNPCAIType.SelectedIndexChanged += cmbNPCAIType_SelectedIndexChanged;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new System.Drawing.Point(13, 907);
            label20.Name = "label20";
            label20.Size = new System.Drawing.Size(174, 15);
            label20.TabIndex = 215;
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
            maeNPCOnInteracted.Location = new System.Drawing.Point(13, 726);
            maeNPCOnInteracted.Name = "maeNPCOnInteracted";
            maeNPCOnInteracted.PlaceholderActionName = null;
            maeNPCOnInteracted.RequiresActionName = true;
            maeNPCOnInteracted.RequiresCondition = true;
            maeNPCOnInteracted.RequiresDescription = true;
            maeNPCOnInteracted.Size = new System.Drawing.Size(368, 94);
            maeNPCOnInteracted.SourceDescription = "Whoever is targeting them";
            maeNPCOnInteracted.TabIndex = 214;
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
            saeNPCOnSpawn.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            saeNPCOnSpawn.ClassId = null;
            saeNPCOnSpawn.Dungeon = null;
            saeNPCOnSpawn.EffectParamData = null;
            saeNPCOnSpawn.Location = new System.Drawing.Point(13, 550);
            saeNPCOnSpawn.Name = "saeNPCOnSpawn";
            saeNPCOnSpawn.PlaceholderActionName = "TurnStart";
            saeNPCOnSpawn.RequiresActionName = false;
            saeNPCOnSpawn.RequiresCondition = true;
            saeNPCOnSpawn.RequiresDescription = false;
            saeNPCOnSpawn.Size = new System.Drawing.Size(283, 32);
            saeNPCOnSpawn.SourceDescription = "The NPC (won't become visible)";
            saeNPCOnSpawn.TabIndex = 213;
            saeNPCOnSpawn.TargetDescription = "The NPC (won't become visible)";
            saeNPCOnSpawn.ThisDescription = "The NPC (won't become visible)";
            saeNPCOnSpawn.TurnEndCriteria = HelperForms.TurnEndCriteria.CannotEndTurn;
            saeNPCOnSpawn.UsageCriteria = HelperForms.UsageCriteria.AnyTargetAnyTime;
            // 
            // ssNPC
            // 
            ssNPC.AttackPerLevelUp = new decimal(new int[] { 0, 0, 0, 0 });
            ssNPC.AutoSize = true;
            ssNPC.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ssNPC.BaseAccuracy = 100;
            ssNPC.BaseAttack = 0;
            ssNPC.BaseDefense = 0;
            ssNPC.BaseEvasion = 0;
            ssNPC.BaseHP = 1;
            ssNPC.BaseHPRegeneration = new decimal(new int[] { 1, 0, 0, 0 });
            ssNPC.BaseHunger = 0;
            ssNPC.BaseMovement = 1;
            ssNPC.BaseMP = 0;
            ssNPC.BaseMPRegeneration = new decimal(new int[] { 0, 0, 0, 0 });
            ssNPC.BaseSightRangeDisplayNames = (System.Collections.Generic.Dictionary<string, string>)resources.GetObject("ssNPC.BaseSightRangeDisplayNames");
            ssNPC.CanGainExperience = false;
            ssNPC.DefensePerLevelUp = new decimal(new int[] { 0, 0, 0, 0 });
            ssNPC.ExperienceToLevelUpFormula = "";
            ssNPC.HPPerLevelUp = new decimal(new int[] { 0, 0, 0, 0 });
            ssNPC.HPRegenerationPerLevelUp = new decimal(new int[] { 0, 0, 0, 0 });
            ssNPC.HungerHPDegeneration = new decimal(new int[] { 0, 0, 0, 0 });
            ssNPC.Location = new System.Drawing.Point(385, 94);
            ssNPC.MaxLevel = 1;
            ssNPC.MovementPerLevelUp = new decimal(new int[] { 0, 0, 0, 0 });
            ssNPC.MPPerLevelUp = new decimal(new int[] { 0, 0, 0, 0 });
            ssNPC.MPRegenerationPerLevelUp = new decimal(new int[] { 0, 0, 0, 0 });
            ssNPC.Name = "ssNPC";
            ssNPC.Size = new System.Drawing.Size(331, 772);
            ssNPC.TabIndex = 212;
            ssNPC.UsesHunger = false;
            ssNPC.UsesMP = false;
            // 
            // sisNPCStartingInventory
            // 
            sisNPCStartingInventory.Inventory = (System.Collections.Generic.List<string>)resources.GetObject("sisNPCStartingInventory.Inventory");
            sisNPCStartingInventory.InventorySize = 0;
            sisNPCStartingInventory.Location = new System.Drawing.Point(13, 432);
            sisNPCStartingInventory.Name = "sisNPCStartingInventory";
            sisNPCStartingInventory.SelectableItems = null;
            sisNPCStartingInventory.Size = new System.Drawing.Size(293, 79);
            sisNPCStartingInventory.TabIndex = 129;
            // 
            // saeNPCOnDeath
            // 
            saeNPCOnDeath.Action = null;
            saeNPCOnDeath.ActionDescription = "When they die...                   ";
            saeNPCOnDeath.ActionTypeText = "On Death";
            saeNPCOnDeath.AutoSize = true;
            saeNPCOnDeath.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            saeNPCOnDeath.ClassId = null;
            saeNPCOnDeath.Dungeon = null;
            saeNPCOnDeath.EffectParamData = null;
            saeNPCOnDeath.Location = new System.Drawing.Point(13, 866);
            saeNPCOnDeath.Name = "saeNPCOnDeath";
            saeNPCOnDeath.PlaceholderActionName = "Death";
            saeNPCOnDeath.RequiresActionName = false;
            saeNPCOnDeath.RequiresCondition = false;
            saeNPCOnDeath.RequiresDescription = false;
            saeNPCOnDeath.Size = new System.Drawing.Size(283, 32);
            saeNPCOnDeath.SourceDescription = "The NPC";
            saeNPCOnDeath.TabIndex = 211;
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
            saeNPCOnAttacked.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            saeNPCOnAttacked.ClassId = null;
            saeNPCOnAttacked.Dungeon = null;
            saeNPCOnAttacked.EffectParamData = null;
            saeNPCOnAttacked.Location = new System.Drawing.Point(13, 826);
            saeNPCOnAttacked.Name = "saeNPCOnAttacked";
            saeNPCOnAttacked.PlaceholderActionName = "Interacted";
            saeNPCOnAttacked.RequiresActionName = false;
            saeNPCOnAttacked.RequiresCondition = false;
            saeNPCOnAttacked.RequiresDescription = false;
            saeNPCOnAttacked.Size = new System.Drawing.Size(281, 32);
            saeNPCOnAttacked.SourceDescription = "The NPC";
            saeNPCOnAttacked.TabIndex = 210;
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
            saeNPCOnTurnStart.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            saeNPCOnTurnStart.ClassId = null;
            saeNPCOnTurnStart.Dungeon = null;
            saeNPCOnTurnStart.EffectParamData = null;
            saeNPCOnTurnStart.Location = new System.Drawing.Point(13, 588);
            saeNPCOnTurnStart.Name = "saeNPCOnTurnStart";
            saeNPCOnTurnStart.PlaceholderActionName = "TurnStart";
            saeNPCOnTurnStart.RequiresActionName = false;
            saeNPCOnTurnStart.RequiresCondition = false;
            saeNPCOnTurnStart.RequiresDescription = false;
            saeNPCOnTurnStart.Size = new System.Drawing.Size(283, 32);
            saeNPCOnTurnStart.SourceDescription = "The NPC";
            saeNPCOnTurnStart.TabIndex = 209;
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
            maeNPCOnAttack.Location = new System.Drawing.Point(13, 626);
            maeNPCOnAttack.Name = "maeNPCOnAttack";
            maeNPCOnAttack.PlaceholderActionName = null;
            maeNPCOnAttack.RequiresActionName = true;
            maeNPCOnAttack.RequiresCondition = true;
            maeNPCOnAttack.RequiresDescription = false;
            maeNPCOnAttack.Size = new System.Drawing.Size(368, 94);
            maeNPCOnAttack.SourceDescription = "The NPC";
            maeNPCOnAttack.TabIndex = 208;
            maeNPCOnAttack.TargetDescription = "Whoever they are targeting";
            maeNPCOnAttack.ThisDescription = "The NPC";
            maeNPCOnAttack.TurnEndCriteria = HelperForms.TurnEndCriteria.MustEndTurn;
            maeNPCOnAttack.UsageCriteria = HelperForms.UsageCriteria.FullConditions;
            // 
            // lblNPCAIOddsToTargetSelfA
            // 
            lblNPCAIOddsToTargetSelfA.AutoSize = true;
            lblNPCAIOddsToTargetSelfA.Location = new System.Drawing.Point(13, 938);
            lblNPCAIOddsToTargetSelfA.Name = "lblNPCAIOddsToTargetSelfA";
            lblNPCAIOddsToTargetSelfA.Size = new System.Drawing.Size(276, 15);
            lblNPCAIOddsToTargetSelfA.TabIndex = 194;
            lblNPCAIOddsToTargetSelfA.Text = "Odds for NPC to target themselves with an Action: ";
            lblNPCAIOddsToTargetSelfA.Visible = false;
            // 
            // txtNPCExperiencePayout
            // 
            txtNPCExperiencePayout.Location = new System.Drawing.Point(121, 297);
            txtNPCExperiencePayout.Name = "txtNPCExperiencePayout";
            txtNPCExperiencePayout.Size = new System.Drawing.Size(242, 23);
            txtNPCExperiencePayout.TabIndex = 192;
            txtNPCExperiencePayout.Enter += txtNPCExperiencePayout_Enter;
            txtNPCExperiencePayout.Leave += txtNPCExperiencePayout_Leave;
            // 
            // label103
            // 
            label103.AutoSize = true;
            label103.Location = new System.Drawing.Point(13, 300);
            label103.Name = "label103";
            label103.Size = new System.Drawing.Size(104, 15);
            label103.TabIndex = 191;
            label103.Text = "Experience Payout";
            // 
            // chkNPCKnowsAllCharacterPositions
            // 
            chkNPCKnowsAllCharacterPositions.AutoSize = true;
            chkNPCKnowsAllCharacterPositions.Location = new System.Drawing.Point(13, 268);
            chkNPCKnowsAllCharacterPositions.Name = "chkNPCKnowsAllCharacterPositions";
            chkNPCKnowsAllCharacterPositions.Size = new System.Drawing.Size(361, 19);
            chkNPCKnowsAllCharacterPositions.TabIndex = 190;
            chkNPCKnowsAllCharacterPositions.Text = "Knows the position of all living characters (even when not seen)";
            chkNPCKnowsAllCharacterPositions.UseVisualStyleBackColor = true;
            chkNPCKnowsAllCharacterPositions.CheckedChanged += chkNPCKnowsAllCharacterPositions_CheckedChanged;
            // 
            // label67
            // 
            label67.AutoSize = true;
            label67.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label67.Location = new System.Drawing.Point(141, 514);
            label67.Name = "label67";
            label67.Size = new System.Drawing.Size(67, 21);
            label67.TabIndex = 182;
            label67.Text = "Actions";
            // 
            // cmbNPCStartingArmor
            // 
            cmbNPCStartingArmor.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbNPCStartingArmor.FormattingEnabled = true;
            cmbNPCStartingArmor.Location = new System.Drawing.Point(146, 362);
            cmbNPCStartingArmor.Name = "cmbNPCStartingArmor";
            cmbNPCStartingArmor.Size = new System.Drawing.Size(158, 23);
            cmbNPCStartingArmor.TabIndex = 175;
            cmbNPCStartingArmor.SelectedIndexChanged += cmbNPCStartingArmor_SelectedIndexChanged;
            // 
            // label70
            // 
            label70.AutoSize = true;
            label70.Location = new System.Drawing.Point(13, 365);
            label70.Name = "label70";
            label70.Size = new System.Drawing.Size(131, 15);
            label70.TabIndex = 174;
            label70.Text = "When unarmored, wear";
            // 
            // cmbNPCStartingWeapon
            // 
            cmbNPCStartingWeapon.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbNPCStartingWeapon.FormattingEnabled = true;
            cmbNPCStartingWeapon.Location = new System.Drawing.Point(139, 331);
            cmbNPCStartingWeapon.Name = "cmbNPCStartingWeapon";
            cmbNPCStartingWeapon.Size = new System.Drawing.Size(165, 23);
            cmbNPCStartingWeapon.TabIndex = 173;
            cmbNPCStartingWeapon.SelectedIndexChanged += cmbNPCStartingWeapon_SelectedIndexChanged;
            // 
            // label71
            // 
            label71.AutoSize = true;
            label71.Location = new System.Drawing.Point(13, 334);
            label71.Name = "label71";
            label71.Size = new System.Drawing.Size(123, 15);
            label71.TabIndex = 172;
            label71.Text = "When unarmed, wield";
            // 
            // label73
            // 
            label73.AutoSize = true;
            label73.Location = new System.Drawing.Point(172, 395);
            label73.Name = "label73";
            label73.Size = new System.Drawing.Size(36, 15);
            label73.TabIndex = 166;
            label73.Text = "items";
            // 
            // nudNPCInventorySize
            // 
            nudNPCInventorySize.Location = new System.Drawing.Point(121, 390);
            nudNPCInventorySize.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            nudNPCInventorySize.Name = "nudNPCInventorySize";
            nudNPCInventorySize.Size = new System.Drawing.Size(45, 23);
            nudNPCInventorySize.TabIndex = 165;
            nudNPCInventorySize.ValueChanged += nudNPCInventorySize_ValueChanged;
            // 
            // label74
            // 
            label74.AutoSize = true;
            label74.Location = new System.Drawing.Point(13, 393);
            label74.Name = "label74";
            label74.Size = new System.Drawing.Size(109, 15);
            label74.TabIndex = 164;
            label74.Text = "Inventory Capacity:";
            // 
            // label98
            // 
            label98.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label98.Location = new System.Drawing.Point(370, 19);
            label98.Name = "label98";
            label98.Size = new System.Drawing.Size(131, 52);
            label98.TabIndex = 122;
            label98.Text = "Appearance";
            label98.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkNPCStartsVisible
            // 
            chkNPCStartsVisible.AutoSize = true;
            chkNPCStartsVisible.Location = new System.Drawing.Point(13, 240);
            chkNPCStartsVisible.Name = "chkNPCStartsVisible";
            chkNPCStartsVisible.Size = new System.Drawing.Size(102, 19);
            chkNPCStartsVisible.TabIndex = 121;
            chkNPCStartsVisible.Text = "Spawns visible";
            chkNPCStartsVisible.UseVisualStyleBackColor = true;
            chkNPCStartsVisible.CheckedChanged += chkNPCStartsVisible_CheckedChanged;
            // 
            // cmbNPCFaction
            // 
            cmbNPCFaction.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbNPCFaction.FormattingEnabled = true;
            cmbNPCFaction.Location = new System.Drawing.Point(65, 209);
            cmbNPCFaction.Name = "cmbNPCFaction";
            cmbNPCFaction.Size = new System.Drawing.Size(146, 23);
            cmbNPCFaction.TabIndex = 120;
            cmbNPCFaction.SelectedIndexChanged += cmbNPCFaction_SelectedIndexChanged;
            // 
            // label99
            // 
            label99.AutoSize = true;
            label99.Location = new System.Drawing.Point(13, 212);
            label99.Name = "label99";
            label99.Size = new System.Drawing.Size(46, 15);
            label99.TabIndex = 119;
            label99.Text = "Faction";
            // 
            // fklblNPCDescriptionLocale
            // 
            fklblNPCDescriptionLocale.Enabled = false;
            fklblNPCDescriptionLocale.FlatAppearance.BorderSize = 0;
            fklblNPCDescriptionLocale.FlatStyle = FlatStyle.Flat;
            fklblNPCDescriptionLocale.Image = (System.Drawing.Image)resources.GetObject("fklblNPCDescriptionLocale.Image");
            fklblNPCDescriptionLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblNPCDescriptionLocale.Location = new System.Drawing.Point(13, 153);
            fklblNPCDescriptionLocale.Name = "fklblNPCDescriptionLocale";
            fklblNPCDescriptionLocale.Size = new System.Drawing.Size(331, 42);
            fklblNPCDescriptionLocale.TabIndex = 117;
            fklblNPCDescriptionLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblNPCDescriptionLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblNPCDescriptionLocale.TextImageRelation = TextImageRelation.ImageBeforeText;
            fklblNPCDescriptionLocale.UseVisualStyleBackColor = true;
            fklblNPCDescriptionLocale.Visible = false;
            // 
            // txtNPCDescription
            // 
            txtNPCDescription.Location = new System.Drawing.Point(13, 124);
            txtNPCDescription.Name = "txtNPCDescription";
            txtNPCDescription.Size = new System.Drawing.Size(350, 23);
            txtNPCDescription.TabIndex = 116;
            txtNPCDescription.TextChanged += txtNPCDescription_TextChanged;
            // 
            // label100
            // 
            label100.AutoSize = true;
            label100.Location = new System.Drawing.Point(13, 106);
            label100.Name = "label100";
            label100.Size = new System.Drawing.Size(67, 15);
            label100.TabIndex = 115;
            label100.Text = "Description";
            // 
            // fklblNPCNameLocale
            // 
            fklblNPCNameLocale.Enabled = false;
            fklblNPCNameLocale.FlatAppearance.BorderSize = 0;
            fklblNPCNameLocale.FlatStyle = FlatStyle.Flat;
            fklblNPCNameLocale.Image = (System.Drawing.Image)resources.GetObject("fklblNPCNameLocale.Image");
            fklblNPCNameLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblNPCNameLocale.Location = new System.Drawing.Point(13, 55);
            fklblNPCNameLocale.Name = "fklblNPCNameLocale";
            fklblNPCNameLocale.Size = new System.Drawing.Size(331, 42);
            fklblNPCNameLocale.TabIndex = 114;
            fklblNPCNameLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblNPCNameLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblNPCNameLocale.TextImageRelation = TextImageRelation.ImageBeforeText;
            fklblNPCNameLocale.UseVisualStyleBackColor = true;
            fklblNPCNameLocale.Visible = false;
            // 
            // txtNPCName
            // 
            txtNPCName.Location = new System.Drawing.Point(13, 26);
            txtNPCName.Name = "txtNPCName";
            txtNPCName.Size = new System.Drawing.Size(350, 23);
            txtNPCName.TabIndex = 113;
            txtNPCName.TextChanged += txtNPCName_TextChanged;
            // 
            // label101
            // 
            label101.AutoSize = true;
            label101.Location = new System.Drawing.Point(13, 8);
            label101.Name = "label101";
            label101.Size = new System.Drawing.Size(80, 15);
            label101.TabIndex = 112;
            label101.Text = "Default Name";
            // 
            // crsNPC
            // 
            crsNPC.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("crsNPC.BackgroundColor");
            crsNPC.Character = '\0';
            crsNPC.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("crsNPC.ForegroundColor");
            crsNPC.Location = new System.Drawing.Point(504, 8);
            crsNPC.Name = "crsNPC";
            crsNPC.Size = new System.Drawing.Size(211, 83);
            crsNPC.TabIndex = 207;
            crsNPC.PropertyChanged += crsNPC_PropertyChanged;
            // 
            // tpItem
            // 
            tpItem.Controls.Add(saeItemOnDeath);
            tpItem.Controls.Add(saeItemOnTurnStart);
            tpItem.Controls.Add(saeItemOnAttacked);
            tpItem.Controls.Add(maeItemOnAttack);
            tpItem.Controls.Add(saeItemOnUse);
            tpItem.Controls.Add(saeItemOnStepped);
            tpItem.Controls.Add(txtItemPower);
            tpItem.Controls.Add(label108);
            tpItem.Controls.Add(chkItemCanBePickedUp);
            tpItem.Controls.Add(chkItemStartsVisible);
            tpItem.Controls.Add(cmbItemType);
            tpItem.Controls.Add(label107);
            tpItem.Controls.Add(label102);
            tpItem.Controls.Add(fklblItemDescriptionLocale);
            tpItem.Controls.Add(txtItemDescription);
            tpItem.Controls.Add(label105);
            tpItem.Controls.Add(fklblItemNameLocale);
            tpItem.Controls.Add(txtItemName);
            tpItem.Controls.Add(label106);
            tpItem.Controls.Add(crsItem);
            tpItem.Location = new System.Drawing.Point(4, 24);
            tpItem.Name = "tpItem";
            tpItem.Size = new System.Drawing.Size(740, 356);
            tpItem.TabIndex = 6;
            tpItem.Text = "Item";
            tpItem.UseVisualStyleBackColor = true;
            // 
            // saeItemOnDeath
            // 
            saeItemOnDeath.Action = null;
            saeItemOnDeath.ActionDescription = "When someone carrying it dies...                ";
            saeItemOnDeath.ActionTypeText = "On Death";
            saeItemOnDeath.AutoSize = true;
            saeItemOnDeath.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            saeItemOnDeath.ClassId = null;
            saeItemOnDeath.Dungeon = null;
            saeItemOnDeath.EffectParamData = null;
            saeItemOnDeath.Location = new System.Drawing.Point(367, 316);
            saeItemOnDeath.Name = "saeItemOnDeath";
            saeItemOnDeath.PlaceholderActionName = "Death";
            saeItemOnDeath.RequiresActionName = false;
            saeItemOnDeath.RequiresCondition = false;
            saeItemOnDeath.RequiresDescription = false;
            saeItemOnDeath.Size = new System.Drawing.Size(361, 32);
            saeItemOnDeath.SourceDescription = "The item";
            saeItemOnDeath.TabIndex = 226;
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
            saeItemOnTurnStart.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            saeItemOnTurnStart.ClassId = null;
            saeItemOnTurnStart.Dungeon = null;
            saeItemOnTurnStart.EffectParamData = null;
            saeItemOnTurnStart.Location = new System.Drawing.Point(367, 244);
            saeItemOnTurnStart.Name = "saeItemOnTurnStart";
            saeItemOnTurnStart.PlaceholderActionName = "Death";
            saeItemOnTurnStart.RequiresActionName = false;
            saeItemOnTurnStart.RequiresCondition = false;
            saeItemOnTurnStart.RequiresDescription = false;
            saeItemOnTurnStart.Size = new System.Drawing.Size(362, 32);
            saeItemOnTurnStart.SourceDescription = "Whoever is equipping This";
            saeItemOnTurnStart.TabIndex = 225;
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
            saeItemOnAttacked.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            saeItemOnAttacked.ClassId = null;
            saeItemOnAttacked.Dungeon = null;
            saeItemOnAttacked.EffectParamData = null;
            saeItemOnAttacked.Location = new System.Drawing.Point(367, 206);
            saeItemOnAttacked.Name = "saeItemOnAttacked";
            saeItemOnAttacked.PlaceholderActionName = "Interacted";
            saeItemOnAttacked.RequiresActionName = false;
            saeItemOnAttacked.RequiresCondition = false;
            saeItemOnAttacked.RequiresDescription = false;
            saeItemOnAttacked.Size = new System.Drawing.Size(362, 32);
            saeItemOnAttacked.SourceDescription = "Whoever is equipping it";
            saeItemOnAttacked.TabIndex = 224;
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
            maeItemOnAttack.Location = new System.Drawing.Point(367, 106);
            maeItemOnAttack.Name = "maeItemOnAttack";
            maeItemOnAttack.PlaceholderActionName = null;
            maeItemOnAttack.RequiresActionName = true;
            maeItemOnAttack.RequiresCondition = true;
            maeItemOnAttack.RequiresDescription = true;
            maeItemOnAttack.Size = new System.Drawing.Size(368, 94);
            maeItemOnAttack.SourceDescription = null;
            maeItemOnAttack.TabIndex = 223;
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
            saeItemOnUse.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            saeItemOnUse.ClassId = null;
            saeItemOnUse.Dungeon = null;
            saeItemOnUse.EffectParamData = null;
            saeItemOnUse.Location = new System.Drawing.Point(13, 316);
            saeItemOnUse.Name = "saeItemOnUse";
            saeItemOnUse.PlaceholderActionName = "ItemUse";
            saeItemOnUse.RequiresActionName = false;
            saeItemOnUse.RequiresCondition = true;
            saeItemOnUse.RequiresDescription = false;
            saeItemOnUse.Size = new System.Drawing.Size(292, 32);
            saeItemOnUse.SourceDescription = "The item";
            saeItemOnUse.TabIndex = 222;
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
            saeItemOnStepped.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            saeItemOnStepped.ClassId = null;
            saeItemOnStepped.Dungeon = null;
            saeItemOnStepped.EffectParamData = null;
            saeItemOnStepped.Location = new System.Drawing.Point(13, 278);
            saeItemOnStepped.Name = "saeItemOnStepped";
            saeItemOnStepped.PlaceholderActionName = "Stepped";
            saeItemOnStepped.RequiresActionName = false;
            saeItemOnStepped.RequiresCondition = false;
            saeItemOnStepped.RequiresDescription = false;
            saeItemOnStepped.Size = new System.Drawing.Size(293, 32);
            saeItemOnStepped.SourceDescription = "The item";
            saeItemOnStepped.TabIndex = 221;
            saeItemOnStepped.TargetDescription = "Whoever stepped on it";
            saeItemOnStepped.ThisDescription = "The item";
            saeItemOnStepped.TurnEndCriteria = HelperForms.TurnEndCriteria.CannotEndTurn;
            saeItemOnStepped.UsageCriteria = HelperForms.UsageCriteria.AnyTargetAnyTime;
            // 
            // txtItemPower
            // 
            txtItemPower.Location = new System.Drawing.Point(86, 243);
            txtItemPower.Name = "txtItemPower";
            txtItemPower.Size = new System.Drawing.Size(150, 23);
            txtItemPower.TabIndex = 206;
            txtItemPower.Enter += txtItemPower_Enter;
            txtItemPower.Leave += txtItemPower_Leave;
            // 
            // label108
            // 
            label108.AutoSize = true;
            label108.Location = new System.Drawing.Point(13, 249);
            label108.Name = "label108";
            label108.Size = new System.Drawing.Size(67, 15);
            label108.TabIndex = 205;
            label108.Text = "Item Power";
            // 
            // chkItemCanBePickedUp
            // 
            chkItemCanBePickedUp.AutoSize = true;
            chkItemCanBePickedUp.Location = new System.Drawing.Point(242, 245);
            chkItemCanBePickedUp.Name = "chkItemCanBePickedUp";
            chkItemCanBePickedUp.Size = new System.Drawing.Size(118, 19);
            chkItemCanBePickedUp.TabIndex = 204;
            chkItemCanBePickedUp.Text = "Can be picked up";
            chkItemCanBePickedUp.UseVisualStyleBackColor = true;
            chkItemCanBePickedUp.CheckedChanged += chkItemCanBePickedUp_CheckedChanged;
            // 
            // chkItemStartsVisible
            // 
            chkItemStartsVisible.AutoSize = true;
            chkItemStartsVisible.Location = new System.Drawing.Point(242, 211);
            chkItemStartsVisible.Name = "chkItemStartsVisible";
            chkItemStartsVisible.Size = new System.Drawing.Size(102, 19);
            chkItemStartsVisible.TabIndex = 203;
            chkItemStartsVisible.Text = "Spawns visible";
            chkItemStartsVisible.UseVisualStyleBackColor = true;
            chkItemStartsVisible.CheckedChanged += chkItemStartsVisible_CheckedChanged;
            // 
            // cmbItemType
            // 
            cmbItemType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbItemType.FormattingEnabled = true;
            cmbItemType.Location = new System.Drawing.Point(77, 209);
            cmbItemType.Name = "cmbItemType";
            cmbItemType.Size = new System.Drawing.Size(159, 23);
            cmbItemType.TabIndex = 202;
            cmbItemType.SelectedIndexChanged += cmbItemType_SelectedIndexChanged;
            // 
            // label107
            // 
            label107.AutoSize = true;
            label107.Location = new System.Drawing.Point(13, 212);
            label107.Name = "label107";
            label107.Size = new System.Drawing.Size(58, 15);
            label107.TabIndex = 201;
            label107.Text = "Item Type";
            // 
            // label102
            // 
            label102.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label102.Location = new System.Drawing.Point(390, 20);
            label102.Name = "label102";
            label102.Size = new System.Drawing.Size(131, 52);
            label102.TabIndex = 196;
            label102.Text = "Appearance";
            label102.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fklblItemDescriptionLocale
            // 
            fklblItemDescriptionLocale.Enabled = false;
            fklblItemDescriptionLocale.FlatAppearance.BorderSize = 0;
            fklblItemDescriptionLocale.FlatStyle = FlatStyle.Flat;
            fklblItemDescriptionLocale.Image = (System.Drawing.Image)resources.GetObject("fklblItemDescriptionLocale.Image");
            fklblItemDescriptionLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblItemDescriptionLocale.Location = new System.Drawing.Point(13, 153);
            fklblItemDescriptionLocale.Name = "fklblItemDescriptionLocale";
            fklblItemDescriptionLocale.Size = new System.Drawing.Size(331, 42);
            fklblItemDescriptionLocale.TabIndex = 195;
            fklblItemDescriptionLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblItemDescriptionLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblItemDescriptionLocale.TextImageRelation = TextImageRelation.ImageBeforeText;
            fklblItemDescriptionLocale.UseVisualStyleBackColor = true;
            fklblItemDescriptionLocale.Visible = false;
            // 
            // txtItemDescription
            // 
            txtItemDescription.Location = new System.Drawing.Point(13, 124);
            txtItemDescription.Name = "txtItemDescription";
            txtItemDescription.Size = new System.Drawing.Size(350, 23);
            txtItemDescription.TabIndex = 194;
            txtItemDescription.TextChanged += txtItemDescription_TextChanged;
            // 
            // label105
            // 
            label105.AutoSize = true;
            label105.Location = new System.Drawing.Point(13, 106);
            label105.Name = "label105";
            label105.Size = new System.Drawing.Size(67, 15);
            label105.TabIndex = 193;
            label105.Text = "Description";
            // 
            // fklblItemNameLocale
            // 
            fklblItemNameLocale.Enabled = false;
            fklblItemNameLocale.FlatAppearance.BorderSize = 0;
            fklblItemNameLocale.FlatStyle = FlatStyle.Flat;
            fklblItemNameLocale.Image = (System.Drawing.Image)resources.GetObject("fklblItemNameLocale.Image");
            fklblItemNameLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblItemNameLocale.Location = new System.Drawing.Point(13, 55);
            fklblItemNameLocale.Name = "fklblItemNameLocale";
            fklblItemNameLocale.Size = new System.Drawing.Size(331, 42);
            fklblItemNameLocale.TabIndex = 192;
            fklblItemNameLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblItemNameLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblItemNameLocale.TextImageRelation = TextImageRelation.ImageBeforeText;
            fklblItemNameLocale.UseVisualStyleBackColor = true;
            fklblItemNameLocale.Visible = false;
            // 
            // txtItemName
            // 
            txtItemName.Location = new System.Drawing.Point(13, 26);
            txtItemName.Name = "txtItemName";
            txtItemName.Size = new System.Drawing.Size(350, 23);
            txtItemName.TabIndex = 191;
            txtItemName.TextChanged += txtItemName_TextChanged;
            // 
            // label106
            // 
            label106.AutoSize = true;
            label106.Location = new System.Drawing.Point(13, 8);
            label106.Name = "label106";
            label106.Size = new System.Drawing.Size(80, 15);
            label106.TabIndex = 190;
            label106.Text = "Default Name";
            // 
            // crsItem
            // 
            crsItem.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("crsItem.BackgroundColor");
            crsItem.Character = '\0';
            crsItem.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("crsItem.ForegroundColor");
            crsItem.Location = new System.Drawing.Point(524, 9);
            crsItem.Name = "crsItem";
            crsItem.Size = new System.Drawing.Size(211, 83);
            crsItem.TabIndex = 220;
            crsItem.PropertyChanged += crsItem_PropertyChanged;
            // 
            // tpTrap
            // 
            tpTrap.Controls.Add(saeTrapOnStepped);
            tpTrap.Controls.Add(txtTrapPower);
            tpTrap.Controls.Add(label113);
            tpTrap.Controls.Add(chkTrapStartsVisible);
            tpTrap.Controls.Add(label116);
            tpTrap.Controls.Add(fklblTrapDescriptionLocale);
            tpTrap.Controls.Add(txtTrapDescription);
            tpTrap.Controls.Add(label117);
            tpTrap.Controls.Add(fklblTrapNameLocale);
            tpTrap.Controls.Add(txtTrapName);
            tpTrap.Controls.Add(label118);
            tpTrap.Controls.Add(crsTrap);
            tpTrap.Location = new System.Drawing.Point(4, 24);
            tpTrap.Name = "tpTrap";
            tpTrap.Size = new System.Drawing.Size(740, 356);
            tpTrap.TabIndex = 7;
            tpTrap.Text = "Trap";
            tpTrap.UseVisualStyleBackColor = true;
            // 
            // saeTrapOnStepped
            // 
            saeTrapOnStepped.Action = null;
            saeTrapOnStepped.ActionDescription = "When someone steps on it...";
            saeTrapOnStepped.ActionTypeText = "Stepped";
            saeTrapOnStepped.AutoSize = true;
            saeTrapOnStepped.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            saeTrapOnStepped.ClassId = null;
            saeTrapOnStepped.Dungeon = null;
            saeTrapOnStepped.EffectParamData = null;
            saeTrapOnStepped.Location = new System.Drawing.Point(13, 272);
            saeTrapOnStepped.Name = "saeTrapOnStepped";
            saeTrapOnStepped.PlaceholderActionName = "Stepped";
            saeTrapOnStepped.RequiresActionName = false;
            saeTrapOnStepped.RequiresCondition = false;
            saeTrapOnStepped.RequiresDescription = false;
            saeTrapOnStepped.Size = new System.Drawing.Size(290, 32);
            saeTrapOnStepped.SourceDescription = "The trap";
            saeTrapOnStepped.TabIndex = 240;
            saeTrapOnStepped.TargetDescription = "Whoever steps on it";
            saeTrapOnStepped.ThisDescription = "The trap";
            saeTrapOnStepped.TurnEndCriteria = HelperForms.TurnEndCriteria.CannotEndTurn;
            saeTrapOnStepped.UsageCriteria = HelperForms.UsageCriteria.AnyTargetAnyTime;
            // 
            // txtTrapPower
            // 
            txtTrapPower.Location = new System.Drawing.Point(86, 209);
            txtTrapPower.Name = "txtTrapPower";
            txtTrapPower.Size = new System.Drawing.Size(150, 23);
            txtTrapPower.TabIndex = 236;
            txtTrapPower.Enter += txtTrapPower_Enter;
            txtTrapPower.Leave += txtTrapPower_Leave;
            // 
            // label113
            // 
            label113.AutoSize = true;
            label113.Location = new System.Drawing.Point(13, 215);
            label113.Name = "label113";
            label113.Size = new System.Drawing.Size(65, 15);
            label113.TabIndex = 235;
            label113.Text = "Trap Power";
            // 
            // chkTrapStartsVisible
            // 
            chkTrapStartsVisible.AutoSize = true;
            chkTrapStartsVisible.Location = new System.Drawing.Point(13, 247);
            chkTrapStartsVisible.Name = "chkTrapStartsVisible";
            chkTrapStartsVisible.Size = new System.Drawing.Size(102, 19);
            chkTrapStartsVisible.TabIndex = 233;
            chkTrapStartsVisible.Text = "Spawns visible";
            chkTrapStartsVisible.UseVisualStyleBackColor = true;
            chkTrapStartsVisible.CheckedChanged += chkTrapStartsVisible_CheckedChanged;
            // 
            // label116
            // 
            label116.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label116.Location = new System.Drawing.Point(390, 20);
            label116.Name = "label116";
            label116.Size = new System.Drawing.Size(131, 52);
            label116.TabIndex = 226;
            label116.Text = "Appearance";
            label116.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fklblTrapDescriptionLocale
            // 
            fklblTrapDescriptionLocale.Enabled = false;
            fklblTrapDescriptionLocale.FlatAppearance.BorderSize = 0;
            fklblTrapDescriptionLocale.FlatStyle = FlatStyle.Flat;
            fklblTrapDescriptionLocale.Image = (System.Drawing.Image)resources.GetObject("fklblTrapDescriptionLocale.Image");
            fklblTrapDescriptionLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblTrapDescriptionLocale.Location = new System.Drawing.Point(13, 153);
            fklblTrapDescriptionLocale.Name = "fklblTrapDescriptionLocale";
            fklblTrapDescriptionLocale.Size = new System.Drawing.Size(331, 42);
            fklblTrapDescriptionLocale.TabIndex = 225;
            fklblTrapDescriptionLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblTrapDescriptionLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblTrapDescriptionLocale.TextImageRelation = TextImageRelation.ImageBeforeText;
            fklblTrapDescriptionLocale.UseVisualStyleBackColor = true;
            fklblTrapDescriptionLocale.Visible = false;
            // 
            // txtTrapDescription
            // 
            txtTrapDescription.Location = new System.Drawing.Point(13, 124);
            txtTrapDescription.Name = "txtTrapDescription";
            txtTrapDescription.Size = new System.Drawing.Size(350, 23);
            txtTrapDescription.TabIndex = 224;
            txtTrapDescription.TextChanged += txtTrapDescription_TextChanged;
            // 
            // label117
            // 
            label117.AutoSize = true;
            label117.Location = new System.Drawing.Point(13, 106);
            label117.Name = "label117";
            label117.Size = new System.Drawing.Size(67, 15);
            label117.TabIndex = 223;
            label117.Text = "Description";
            // 
            // fklblTrapNameLocale
            // 
            fklblTrapNameLocale.Enabled = false;
            fklblTrapNameLocale.FlatAppearance.BorderSize = 0;
            fklblTrapNameLocale.FlatStyle = FlatStyle.Flat;
            fklblTrapNameLocale.Image = (System.Drawing.Image)resources.GetObject("fklblTrapNameLocale.Image");
            fklblTrapNameLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblTrapNameLocale.Location = new System.Drawing.Point(13, 55);
            fklblTrapNameLocale.Name = "fklblTrapNameLocale";
            fklblTrapNameLocale.Size = new System.Drawing.Size(331, 42);
            fklblTrapNameLocale.TabIndex = 222;
            fklblTrapNameLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblTrapNameLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblTrapNameLocale.TextImageRelation = TextImageRelation.ImageBeforeText;
            fklblTrapNameLocale.UseVisualStyleBackColor = true;
            fklblTrapNameLocale.Visible = false;
            // 
            // txtTrapName
            // 
            txtTrapName.Location = new System.Drawing.Point(13, 26);
            txtTrapName.Name = "txtTrapName";
            txtTrapName.Size = new System.Drawing.Size(350, 23);
            txtTrapName.TabIndex = 221;
            txtTrapName.TextChanged += txtTrapName_TextChanged;
            // 
            // label118
            // 
            label118.AutoSize = true;
            label118.Location = new System.Drawing.Point(13, 8);
            label118.Name = "label118";
            label118.Size = new System.Drawing.Size(80, 15);
            label118.TabIndex = 220;
            label118.Text = "Default Name";
            // 
            // crsTrap
            // 
            crsTrap.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("crsTrap.BackgroundColor");
            crsTrap.Character = '\0';
            crsTrap.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("crsTrap.ForegroundColor");
            crsTrap.Location = new System.Drawing.Point(524, 9);
            crsTrap.Name = "crsTrap";
            crsTrap.Size = new System.Drawing.Size(211, 83);
            crsTrap.TabIndex = 239;
            crsTrap.PropertyChanged += crsTrap_PropertyChanged;
            // 
            // tpAlteredStatus
            // 
            tpAlteredStatus.Controls.Add(saeAlteredStatusOnAttacked);
            tpAlteredStatus.Controls.Add(saeAlteredStatusBeforeAttack);
            tpAlteredStatus.Controls.Add(saeAlteredStatusOnRemove);
            tpAlteredStatus.Controls.Add(saeAlteredStatusOnTurnStart);
            tpAlteredStatus.Controls.Add(saeAlteredStatusOnApply);
            tpAlteredStatus.Controls.Add(chkAlteredStatusCleansedOnCleanseActions);
            tpAlteredStatus.Controls.Add(chkAlteredStatusCleanseOnFloorChange);
            tpAlteredStatus.Controls.Add(chkAlteredStatusCanOverwrite);
            tpAlteredStatus.Controls.Add(chkAlteredStatusCanStack);
            tpAlteredStatus.Controls.Add(label111);
            tpAlteredStatus.Controls.Add(fklblAlteredStatusDescriptionLocale);
            tpAlteredStatus.Controls.Add(txtAlteredStatusDescription);
            tpAlteredStatus.Controls.Add(label114);
            tpAlteredStatus.Controls.Add(fklblAlteredStatusNameLocale);
            tpAlteredStatus.Controls.Add(txtAlteredStatusName);
            tpAlteredStatus.Controls.Add(label115);
            tpAlteredStatus.Controls.Add(crsAlteredStatus);
            tpAlteredStatus.Location = new System.Drawing.Point(4, 24);
            tpAlteredStatus.Name = "tpAlteredStatus";
            tpAlteredStatus.Size = new System.Drawing.Size(740, 356);
            tpAlteredStatus.TabIndex = 8;
            tpAlteredStatus.Text = "Altered Status";
            tpAlteredStatus.UseVisualStyleBackColor = true;
            // 
            // saeAlteredStatusOnAttacked
            // 
            saeAlteredStatusOnAttacked.Action = null;
            saeAlteredStatusOnAttacked.ActionDescription = "When someone afflicted  \r\nby it is attacked...";
            saeAlteredStatusOnAttacked.ActionTypeText = "On Statused Attacked";
            saeAlteredStatusOnAttacked.AutoSize = true;
            saeAlteredStatusOnAttacked.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            saeAlteredStatusOnAttacked.ClassId = null;
            saeAlteredStatusOnAttacked.Dungeon = null;
            saeAlteredStatusOnAttacked.EffectParamData = null;
            saeAlteredStatusOnAttacked.Location = new System.Drawing.Point(391, 266);
            saeAlteredStatusOnAttacked.Name = "saeAlteredStatusOnAttacked";
            saeAlteredStatusOnAttacked.PlaceholderActionName = "OnAttacked";
            saeAlteredStatusOnAttacked.RequiresActionName = false;
            saeAlteredStatusOnAttacked.RequiresCondition = false;
            saeAlteredStatusOnAttacked.RequiresDescription = false;
            saeAlteredStatusOnAttacked.Size = new System.Drawing.Size(276, 32);
            saeAlteredStatusOnAttacked.SourceDescription = "Whoever it's inflicting";
            saeAlteredStatusOnAttacked.TabIndex = 265;
            saeAlteredStatusOnAttacked.TargetDescription = "Whoever attacked them";
            saeAlteredStatusOnAttacked.ThisDescription = "The Altered Status";
            saeAlteredStatusOnAttacked.TurnEndCriteria = HelperForms.TurnEndCriteria.CannotEndTurn;
            saeAlteredStatusOnAttacked.UsageCriteria = HelperForms.UsageCriteria.AnyTargetAnyTime;
            // 
            // saeAlteredStatusBeforeAttack
            // 
            saeAlteredStatusBeforeAttack.Action = null;
            saeAlteredStatusBeforeAttack.ActionDescription = "When someone afflicted\r\nby it is about to attack...   ";
            saeAlteredStatusBeforeAttack.ActionTypeText = "Before Statused Attack";
            saeAlteredStatusBeforeAttack.AutoSize = true;
            saeAlteredStatusBeforeAttack.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            saeAlteredStatusBeforeAttack.ClassId = null;
            saeAlteredStatusBeforeAttack.Dungeon = null;
            saeAlteredStatusBeforeAttack.EffectParamData = null;
            saeAlteredStatusBeforeAttack.Location = new System.Drawing.Point(391, 229);
            saeAlteredStatusBeforeAttack.Name = "saeAlteredStatusBeforeAttack";
            saeAlteredStatusBeforeAttack.PlaceholderActionName = "BeforeAttack";
            saeAlteredStatusBeforeAttack.RequiresActionName = false;
            saeAlteredStatusBeforeAttack.RequiresCondition = false;
            saeAlteredStatusBeforeAttack.RequiresDescription = false;
            saeAlteredStatusBeforeAttack.Size = new System.Drawing.Size(276, 32);
            saeAlteredStatusBeforeAttack.SourceDescription = "Whoever it's inflicting";
            saeAlteredStatusBeforeAttack.TabIndex = 264;
            saeAlteredStatusBeforeAttack.TargetDescription = "Whoever is being targeted";
            saeAlteredStatusBeforeAttack.ThisDescription = "The Altered Status";
            saeAlteredStatusBeforeAttack.TurnEndCriteria = HelperForms.TurnEndCriteria.CannotEndTurn;
            saeAlteredStatusBeforeAttack.UsageCriteria = HelperForms.UsageCriteria.AnyTargetAnyTime;
            // 
            // saeAlteredStatusOnRemove
            // 
            saeAlteredStatusOnRemove.Action = null;
            saeAlteredStatusOnRemove.ActionDescription = "When someone gets this\r\nAltered Status removed... ";
            saeAlteredStatusOnRemove.ActionTypeText = "On Status Remove";
            saeAlteredStatusOnRemove.AutoSize = true;
            saeAlteredStatusOnRemove.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            saeAlteredStatusOnRemove.ClassId = null;
            saeAlteredStatusOnRemove.Dungeon = null;
            saeAlteredStatusOnRemove.EffectParamData = null;
            saeAlteredStatusOnRemove.Location = new System.Drawing.Point(391, 153);
            saeAlteredStatusOnRemove.Name = "saeAlteredStatusOnRemove";
            saeAlteredStatusOnRemove.PlaceholderActionName = "OnRemove";
            saeAlteredStatusOnRemove.RequiresActionName = false;
            saeAlteredStatusOnRemove.RequiresCondition = false;
            saeAlteredStatusOnRemove.RequiresDescription = false;
            saeAlteredStatusOnRemove.Size = new System.Drawing.Size(276, 32);
            saeAlteredStatusOnRemove.SourceDescription = "The Altered Status";
            saeAlteredStatusOnRemove.TabIndex = 263;
            saeAlteredStatusOnRemove.TargetDescription = "Whoever it's targeting";
            saeAlteredStatusOnRemove.ThisDescription = "The Altered Status";
            saeAlteredStatusOnRemove.TurnEndCriteria = HelperForms.TurnEndCriteria.CannotEndTurn;
            saeAlteredStatusOnRemove.UsageCriteria = HelperForms.UsageCriteria.AnyTargetAnyTime;
            // 
            // saeAlteredStatusOnTurnStart
            // 
            saeAlteredStatusOnTurnStart.Action = null;
            saeAlteredStatusOnTurnStart.ActionDescription = "When someone afflicted\r\nby it begins a new turn...  ";
            saeAlteredStatusOnTurnStart.ActionTypeText = "Turn Start";
            saeAlteredStatusOnTurnStart.AutoSize = true;
            saeAlteredStatusOnTurnStart.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            saeAlteredStatusOnTurnStart.ClassId = null;
            saeAlteredStatusOnTurnStart.Dungeon = null;
            saeAlteredStatusOnTurnStart.EffectParamData = null;
            saeAlteredStatusOnTurnStart.Location = new System.Drawing.Point(391, 191);
            saeAlteredStatusOnTurnStart.Name = "saeAlteredStatusOnTurnStart";
            saeAlteredStatusOnTurnStart.PlaceholderActionName = "TurnStart";
            saeAlteredStatusOnTurnStart.RequiresActionName = false;
            saeAlteredStatusOnTurnStart.RequiresCondition = false;
            saeAlteredStatusOnTurnStart.RequiresDescription = false;
            saeAlteredStatusOnTurnStart.Size = new System.Drawing.Size(276, 32);
            saeAlteredStatusOnTurnStart.SourceDescription = "The Altered Status";
            saeAlteredStatusOnTurnStart.TabIndex = 262;
            saeAlteredStatusOnTurnStart.TargetDescription = "Whoever it's inflicting";
            saeAlteredStatusOnTurnStart.ThisDescription = "The Altered Status";
            saeAlteredStatusOnTurnStart.TurnEndCriteria = HelperForms.TurnEndCriteria.CannotEndTurn;
            saeAlteredStatusOnTurnStart.UsageCriteria = HelperForms.UsageCriteria.AnyTargetAnyTime;
            // 
            // saeAlteredStatusOnApply
            // 
            saeAlteredStatusOnApply.Action = null;
            saeAlteredStatusOnApply.ActionDescription = "When someone gets this\r\nAltered Status inflicted...  ";
            saeAlteredStatusOnApply.ActionTypeText = "On Status Apply";
            saeAlteredStatusOnApply.AutoSize = true;
            saeAlteredStatusOnApply.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            saeAlteredStatusOnApply.ClassId = null;
            saeAlteredStatusOnApply.Dungeon = null;
            saeAlteredStatusOnApply.EffectParamData = null;
            saeAlteredStatusOnApply.Location = new System.Drawing.Point(392, 115);
            saeAlteredStatusOnApply.Name = "saeAlteredStatusOnApply";
            saeAlteredStatusOnApply.PlaceholderActionName = "StatusApply";
            saeAlteredStatusOnApply.RequiresActionName = false;
            saeAlteredStatusOnApply.RequiresCondition = false;
            saeAlteredStatusOnApply.RequiresDescription = false;
            saeAlteredStatusOnApply.Size = new System.Drawing.Size(275, 32);
            saeAlteredStatusOnApply.SourceDescription = "The Altered Status";
            saeAlteredStatusOnApply.TabIndex = 261;
            saeAlteredStatusOnApply.TargetDescription = "Whoever it's targeting";
            saeAlteredStatusOnApply.ThisDescription = "The Altered Status";
            saeAlteredStatusOnApply.TurnEndCriteria = HelperForms.TurnEndCriteria.CannotEndTurn;
            saeAlteredStatusOnApply.UsageCriteria = HelperForms.UsageCriteria.AnyTargetAnyTime;
            // 
            // chkAlteredStatusCleansedOnCleanseActions
            // 
            chkAlteredStatusCleansedOnCleanseActions.AutoSize = true;
            chkAlteredStatusCleansedOnCleanseActions.Location = new System.Drawing.Point(13, 291);
            chkAlteredStatusCleansedOnCleanseActions.Name = "chkAlteredStatusCleansedOnCleanseActions";
            chkAlteredStatusCleansedOnCleanseActions.Size = new System.Drawing.Size(247, 19);
            chkAlteredStatusCleansedOnCleanseActions.TabIndex = 257;
            chkAlteredStatusCleansedOnCleanseActions.Text = "Can be removed by 'Cleanse' Action steps";
            chkAlteredStatusCleansedOnCleanseActions.UseVisualStyleBackColor = true;
            chkAlteredStatusCleansedOnCleanseActions.CheckedChanged += chkAlteredStatusCleansedOnCleanseActions_CheckedChanged;
            // 
            // chkAlteredStatusCleanseOnFloorChange
            // 
            chkAlteredStatusCleanseOnFloorChange.AutoSize = true;
            chkAlteredStatusCleanseOnFloorChange.Location = new System.Drawing.Point(13, 266);
            chkAlteredStatusCleanseOnFloorChange.Name = "chkAlteredStatusCleanseOnFloorChange";
            chkAlteredStatusCleanseOnFloorChange.Size = new System.Drawing.Size(330, 19);
            chkAlteredStatusCleanseOnFloorChange.TabIndex = 256;
            chkAlteredStatusCleanseOnFloorChange.Text = "Is removed if the afflicted Character moves to a new Floor";
            chkAlteredStatusCleanseOnFloorChange.UseVisualStyleBackColor = true;
            chkAlteredStatusCleanseOnFloorChange.CheckedChanged += chkAlteredStatusCleanseOnFloorChange_CheckedChanged;
            // 
            // chkAlteredStatusCanOverwrite
            // 
            chkAlteredStatusCanOverwrite.AutoSize = true;
            chkAlteredStatusCanOverwrite.Location = new System.Drawing.Point(13, 241);
            chkAlteredStatusCanOverwrite.Name = "chkAlteredStatusCanOverwrite";
            chkAlteredStatusCanOverwrite.Size = new System.Drawing.Size(342, 19);
            chkAlteredStatusCanOverwrite.TabIndex = 255;
            chkAlteredStatusCanOverwrite.Text = "Overwrites other Altered Statuses with the same Id if applied";
            chkAlteredStatusCanOverwrite.UseVisualStyleBackColor = true;
            chkAlteredStatusCanOverwrite.CheckedChanged += chkAlteredStatusCanOverwrite_CheckedChanged;
            // 
            // chkAlteredStatusCanStack
            // 
            chkAlteredStatusCanStack.AutoSize = true;
            chkAlteredStatusCanStack.Location = new System.Drawing.Point(13, 216);
            chkAlteredStatusCanStack.Name = "chkAlteredStatusCanStack";
            chkAlteredStatusCanStack.Size = new System.Drawing.Size(311, 19);
            chkAlteredStatusCanStack.TabIndex = 250;
            chkAlteredStatusCanStack.Text = "Can stack with other Altered Statuses with the same Id";
            chkAlteredStatusCanStack.UseVisualStyleBackColor = true;
            chkAlteredStatusCanStack.CheckedChanged += chkAlteredStatusCanStack_CheckedChanged;
            // 
            // label111
            // 
            label111.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label111.Location = new System.Drawing.Point(390, 20);
            label111.Name = "label111";
            label111.Size = new System.Drawing.Size(131, 52);
            label111.TabIndex = 245;
            label111.Text = "Appearance";
            label111.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fklblAlteredStatusDescriptionLocale
            // 
            fklblAlteredStatusDescriptionLocale.Enabled = false;
            fklblAlteredStatusDescriptionLocale.FlatAppearance.BorderSize = 0;
            fklblAlteredStatusDescriptionLocale.FlatStyle = FlatStyle.Flat;
            fklblAlteredStatusDescriptionLocale.Image = (System.Drawing.Image)resources.GetObject("fklblAlteredStatusDescriptionLocale.Image");
            fklblAlteredStatusDescriptionLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblAlteredStatusDescriptionLocale.Location = new System.Drawing.Point(13, 153);
            fklblAlteredStatusDescriptionLocale.Name = "fklblAlteredStatusDescriptionLocale";
            fklblAlteredStatusDescriptionLocale.Size = new System.Drawing.Size(331, 42);
            fklblAlteredStatusDescriptionLocale.TabIndex = 244;
            fklblAlteredStatusDescriptionLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblAlteredStatusDescriptionLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblAlteredStatusDescriptionLocale.TextImageRelation = TextImageRelation.ImageBeforeText;
            fklblAlteredStatusDescriptionLocale.UseVisualStyleBackColor = true;
            fklblAlteredStatusDescriptionLocale.Visible = false;
            // 
            // txtAlteredStatusDescription
            // 
            txtAlteredStatusDescription.Location = new System.Drawing.Point(13, 124);
            txtAlteredStatusDescription.Name = "txtAlteredStatusDescription";
            txtAlteredStatusDescription.Size = new System.Drawing.Size(350, 23);
            txtAlteredStatusDescription.TabIndex = 243;
            txtAlteredStatusDescription.TextChanged += txtAlteredStatusDescription_TextChanged;
            // 
            // label114
            // 
            label114.AutoSize = true;
            label114.Location = new System.Drawing.Point(13, 106);
            label114.Name = "label114";
            label114.Size = new System.Drawing.Size(67, 15);
            label114.TabIndex = 242;
            label114.Text = "Description";
            // 
            // fklblAlteredStatusNameLocale
            // 
            fklblAlteredStatusNameLocale.Enabled = false;
            fklblAlteredStatusNameLocale.FlatAppearance.BorderSize = 0;
            fklblAlteredStatusNameLocale.FlatStyle = FlatStyle.Flat;
            fklblAlteredStatusNameLocale.Image = (System.Drawing.Image)resources.GetObject("fklblAlteredStatusNameLocale.Image");
            fklblAlteredStatusNameLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblAlteredStatusNameLocale.Location = new System.Drawing.Point(13, 55);
            fklblAlteredStatusNameLocale.Name = "fklblAlteredStatusNameLocale";
            fklblAlteredStatusNameLocale.Size = new System.Drawing.Size(331, 42);
            fklblAlteredStatusNameLocale.TabIndex = 241;
            fklblAlteredStatusNameLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblAlteredStatusNameLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblAlteredStatusNameLocale.TextImageRelation = TextImageRelation.ImageBeforeText;
            fklblAlteredStatusNameLocale.UseVisualStyleBackColor = true;
            fklblAlteredStatusNameLocale.Visible = false;
            // 
            // txtAlteredStatusName
            // 
            txtAlteredStatusName.Location = new System.Drawing.Point(13, 26);
            txtAlteredStatusName.Name = "txtAlteredStatusName";
            txtAlteredStatusName.Size = new System.Drawing.Size(350, 23);
            txtAlteredStatusName.TabIndex = 240;
            txtAlteredStatusName.TextChanged += txtAlteredStatusName_TextChanged;
            // 
            // label115
            // 
            label115.AutoSize = true;
            label115.Location = new System.Drawing.Point(13, 8);
            label115.Name = "label115";
            label115.Size = new System.Drawing.Size(80, 15);
            label115.TabIndex = 239;
            label115.Text = "Default Name";
            // 
            // crsAlteredStatus
            // 
            crsAlteredStatus.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("crsAlteredStatus.BackgroundColor");
            crsAlteredStatus.Character = '\0';
            crsAlteredStatus.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("crsAlteredStatus.ForegroundColor");
            crsAlteredStatus.Location = new System.Drawing.Point(524, 9);
            crsAlteredStatus.Name = "crsAlteredStatus";
            crsAlteredStatus.Size = new System.Drawing.Size(211, 83);
            crsAlteredStatus.TabIndex = 260;
            crsAlteredStatus.PropertyChanged += crsAlteredStatus_PropertyChanged;
            // 
            // tpValidation
            // 
            tpValidation.Controls.Add(tvValidationResults);
            tpValidation.Location = new System.Drawing.Point(4, 24);
            tpValidation.Name = "tpValidation";
            tpValidation.Size = new System.Drawing.Size(740, 356);
            tpValidation.TabIndex = 9;
            tpValidation.Text = "Validation Results";
            tpValidation.UseVisualStyleBackColor = true;
            // 
            // tvValidationResults
            // 
            tvValidationResults.Dock = DockStyle.Fill;
            tvValidationResults.Location = new System.Drawing.Point(0, 0);
            tvValidationResults.Name = "tvValidationResults";
            tvValidationResults.Size = new System.Drawing.Size(740, 356);
            tvValidationResults.TabIndex = 0;
            // 
            // ofdDungeon
            // 
            ofdDungeon.Filter = "Dungeon JSON|*.json";
            ofdDungeon.Title = "Select a Dungeon JSON file";
            // 
            // sfdDungeon
            // 
            sfdDungeon.Filter = "Dungeon JSON|*.json";
            sfdDungeon.Title = "Set a Dungeon JSON file name to save";
            // 
            // frmMain
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(967, 450);
            Controls.Add(tbTabs);
            Controls.Add(tvDungeonInfo);
            Controls.Add(tsButtons);
            Controls.Add(msMenu);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = msMenu;
            MaximizeBox = false;
            Name = "frmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Rogue Customs Dungeon Editor";
            FormClosing += frmMain_FormClosing;
            msMenu.ResumeLayout(false);
            msMenu.PerformLayout();
            tsButtons.ResumeLayout(false);
            tsButtons.PerformLayout();
            tbTabs.ResumeLayout(false);
            tpBasicInfo.ResumeLayout(false);
            tpBasicInfo.PerformLayout();
            tpLocales.ResumeLayout(false);
            tpLocales.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLocales).EndInit();
            tpTileSetInfos.ResumeLayout(false);
            tpFloorInfos.ResumeLayout(false);
            tpFloorInfos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudHungerLostPerTurn).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudRoomFusionOdds).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudExtraRoomConnectionOdds).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMaxRoomConnections).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudHeight).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudWidth).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMaxFloorLevel).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMinFloorLevel).EndInit();
            tpFactionInfos.ResumeLayout(false);
            tpFactionInfos.PerformLayout();
            tpPlayerClass.ResumeLayout(false);
            tpPlayerClass.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudPlayerInventorySize).EndInit();
            tpNPC.ResumeLayout(false);
            tpNPC.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudNPCOddsToTargetSelf).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudNPCInventorySize).EndInit();
            tpItem.ResumeLayout(false);
            tpItem.PerformLayout();
            tpTrap.ResumeLayout(false);
            tpTrap.PerformLayout();
            tpAlteredStatus.ResumeLayout(false);
            tpAlteredStatus.PerformLayout();
            tpValidation.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
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
        private Label label30;
        private ComboBox cmbPlayerStartingArmor;
        private Label label57;
        private ComboBox cmbPlayerStartingWeapon;
        private Label label56;
        private Label label54;
        private NumericUpDown nudPlayerInventorySize;
        private Label label53;
        private Label label58;
        private global::System.Windows.Forms.TextBox txtNPCExperiencePayout;
        private global::System.Windows.Forms.Label label103;
        private global::System.Windows.Forms.CheckBox chkNPCKnowsAllCharacterPositions;
        private global::System.Windows.Forms.Label label67;
        private global::System.Windows.Forms.ComboBox cmbNPCStartingArmor;
        private global::System.Windows.Forms.Label label70;
        private global::System.Windows.Forms.ComboBox cmbNPCStartingWeapon;
        private global::System.Windows.Forms.Label label71;
        private global::System.Windows.Forms.Label label73;
        private global::System.Windows.Forms.NumericUpDown nudNPCInventorySize;
        private global::System.Windows.Forms.Label label74;
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
        private global::System.Windows.Forms.NumericUpDown nudNPCOddsToTargetSelf;
        private global::System.Windows.Forms.Label lblNPCAIOddsToTargetSelfA;
        private global::System.Windows.Forms.TextBox txtItemPower;
        private global::System.Windows.Forms.Label label108;
        private global::System.Windows.Forms.CheckBox chkItemCanBePickedUp;
        private global::System.Windows.Forms.CheckBox chkItemStartsVisible;
        private global::System.Windows.Forms.ComboBox cmbItemType;
        private global::System.Windows.Forms.Label label107;
        private global::System.Windows.Forms.Label label102;
        private global::System.Windows.Forms.Button fklblItemDescriptionLocale;
        private global::System.Windows.Forms.TextBox txtItemDescription;
        private global::System.Windows.Forms.Label label105;
        private global::System.Windows.Forms.Button fklblItemNameLocale;
        private global::System.Windows.Forms.TextBox txtItemName;
        private global::System.Windows.Forms.Label label106;
        private global::System.Windows.Forms.CheckBox chkTrapStartsVisible;
        private global::System.Windows.Forms.Label label116;
        private global::System.Windows.Forms.Button fklblTrapDescriptionLocale;
        private global::System.Windows.Forms.TextBox txtTrapDescription;
        private global::System.Windows.Forms.Label label117;
        private global::System.Windows.Forms.Button fklblTrapNameLocale;
        private global::System.Windows.Forms.TextBox txtTrapName;
        private global::System.Windows.Forms.Label label118;
        private global::System.Windows.Forms.TextBox txtTrapPower;
        private global::System.Windows.Forms.Label label113;
        private global::System.Windows.Forms.CheckBox chkAlteredStatusCanStack;
        private global::System.Windows.Forms.Label label111;
        private global::System.Windows.Forms.Button fklblAlteredStatusDescriptionLocale;
        private global::System.Windows.Forms.TextBox txtAlteredStatusDescription;
        private global::System.Windows.Forms.Label label114;
        private global::System.Windows.Forms.Button fklblAlteredStatusNameLocale;
        private global::System.Windows.Forms.TextBox txtAlteredStatusName;
        private global::System.Windows.Forms.Label label115;
        private global::System.Windows.Forms.CheckBox chkAlteredStatusCleansedOnCleanseActions;
        private global::System.Windows.Forms.CheckBox chkAlteredStatusCanOverwrite;
        private global::System.Windows.Forms.TreeView tvValidationResults;
        private global::System.Windows.Forms.SaveFileDialog sfdDungeon;
        private global::RogueCustomsDungeonEditor.Controls.ConsoleRepresentationSelector crsPlayer;
        private global::RogueCustomsDungeonEditor.Controls.ConsoleRepresentationSelector crsNPC;
        private global::RogueCustomsDungeonEditor.Controls.ConsoleRepresentationSelector crsItem;
        private global::RogueCustomsDungeonEditor.Controls.ConsoleRepresentationSelector crsTrap;
        private global::RogueCustomsDungeonEditor.Controls.ConsoleRepresentationSelector crsAlteredStatus;
        private global::System.Windows.Forms.TabPage tpTileSetInfos;
        private global::System.Windows.Forms.Label label138;
        private global::RogueCustomsDungeonEditor.Controls.ConsoleRepresentationSelector csrConnectorWall;
        private global::System.Windows.Forms.Label label134;
        private global::RogueCustomsDungeonEditor.Controls.ConsoleRepresentationSelector csrHorizontalWall;
        private global::System.Windows.Forms.Label label135;
        private global::RogueCustomsDungeonEditor.Controls.ConsoleRepresentationSelector csrBottomRightWall;
        private global::System.Windows.Forms.Label label136;
        private global::RogueCustomsDungeonEditor.Controls.ConsoleRepresentationSelector csrBottomLeftWall;
        private global::System.Windows.Forms.Label label133;
        private global::RogueCustomsDungeonEditor.Controls.ConsoleRepresentationSelector csrVerticalWall;
        private global::System.Windows.Forms.Label label132;
        private global::RogueCustomsDungeonEditor.Controls.ConsoleRepresentationSelector csrTopRightWall;
        private global::System.Windows.Forms.Label label131;
        private global::System.Windows.Forms.Label label130;
        private global::RogueCustomsDungeonEditor.Controls.ConsoleRepresentationSelector csrTopLeftWall;
        private global::System.Windows.Forms.Label label148;
        private global::RogueCustomsDungeonEditor.Controls.ConsoleRepresentationSelector csrHorizontalHallway;
        private global::System.Windows.Forms.Label label149;
        private global::RogueCustomsDungeonEditor.Controls.ConsoleRepresentationSelector csrBottomRightHallway;
        private global::System.Windows.Forms.Label label150;
        private global::RogueCustomsDungeonEditor.Controls.ConsoleRepresentationSelector csrBottomLeftHallway;
        private global::System.Windows.Forms.Label label137;
        private global::RogueCustomsDungeonEditor.Controls.ConsoleRepresentationSelector csrCentralHallway;
        private global::System.Windows.Forms.Label label140;
        private global::RogueCustomsDungeonEditor.Controls.ConsoleRepresentationSelector csrVerticalRightHallway;
        private global::System.Windows.Forms.Label label141;
        private global::RogueCustomsDungeonEditor.Controls.ConsoleRepresentationSelector csrVerticalLeftHallway;
        private global::System.Windows.Forms.Label label142;
        private global::RogueCustomsDungeonEditor.Controls.ConsoleRepresentationSelector csrHorizontalTopHallway;
        private global::System.Windows.Forms.Label label143;
        private global::RogueCustomsDungeonEditor.Controls.ConsoleRepresentationSelector csrHorizontalBottomHallway;
        private global::System.Windows.Forms.Label label144;
        private global::RogueCustomsDungeonEditor.Controls.ConsoleRepresentationSelector csrVerticalHallway;
        private global::System.Windows.Forms.Label label145;
        private global::RogueCustomsDungeonEditor.Controls.ConsoleRepresentationSelector csrTopRightHallway;
        private global::System.Windows.Forms.Label label146;
        private global::System.Windows.Forms.Label label147;
        private global::RogueCustomsDungeonEditor.Controls.ConsoleRepresentationSelector csrTopLeftHallway;
        private global::System.Windows.Forms.Label label151;
        private global::RogueCustomsDungeonEditor.Controls.ConsoleRepresentationSelector csrEmpty;
        private global::System.Windows.Forms.Label label152;
        private global::RogueCustomsDungeonEditor.Controls.ConsoleRepresentationSelector csrStairs;
        private global::System.Windows.Forms.Label label153;
        private global::System.Windows.Forms.Label label154;
        private global::RogueCustomsDungeonEditor.Controls.ConsoleRepresentationSelector csrFloor;
        private global::System.Windows.Forms.ComboBox cmbTilesets;
        private global::System.Windows.Forms.Label label155;
        private global::RogueCustomsDungeonEditor.Controls.SingleActionEditor saeOnFloorStart;
        private global::RogueCustomsDungeonEditor.Controls.MultiActionEditor maePlayerOnAttack;
        private global::RogueCustomsDungeonEditor.Controls.SingleActionEditor saePlayerOnAttacked;
        private global::RogueCustomsDungeonEditor.Controls.SingleActionEditor saePlayerOnTurnStart;
        private global::RogueCustomsDungeonEditor.Controls.SingleActionEditor saePlayerOnDeath;
        private global::RogueCustomsDungeonEditor.Controls.SingleActionEditor saeNPCOnDeath;
        private global::RogueCustomsDungeonEditor.Controls.SingleActionEditor saeNPCOnAttacked;
        private global::RogueCustomsDungeonEditor.Controls.SingleActionEditor saeNPCOnTurnStart;
        private global::RogueCustomsDungeonEditor.Controls.MultiActionEditor maeNPCOnAttack;
        private global::RogueCustomsDungeonEditor.Controls.SingleActionEditor saeItemOnUse;
        private global::RogueCustomsDungeonEditor.Controls.SingleActionEditor saeItemOnStepped;
        private global::RogueCustomsDungeonEditor.Controls.SingleActionEditor saeItemOnTurnStart;
        private global::RogueCustomsDungeonEditor.Controls.SingleActionEditor saeItemOnAttacked;
        private global::RogueCustomsDungeonEditor.Controls.MultiActionEditor maeItemOnAttack;
        private global::RogueCustomsDungeonEditor.Controls.SingleActionEditor saeTrapOnStepped;
        private global::RogueCustomsDungeonEditor.Controls.SingleActionEditor saeAlteredStatusOnTurnStart;
        private global::RogueCustomsDungeonEditor.Controls.SingleActionEditor saeAlteredStatusOnApply;
        private global::RogueCustomsDungeonEditor.Controls.StartingInventorySelector sisPlayerStartingInventory;
        private global::RogueCustomsDungeonEditor.Controls.StartingInventorySelector sisNPCStartingInventory;
        private global::RogueCustomsDungeonEditor.Controls.StatsSheet ssNPC;
        private global::RogueCustomsDungeonEditor.Controls.StatsSheet ssPlayer;
        private global::RogueCustomsDungeonEditor.Controls.MultiActionEditor maeNPCOnInteracted;
        private global::RogueCustomsDungeonEditor.Controls.SingleActionEditor saeNPCOnSpawn;
        private global::RogueCustomsDungeonEditor.Controls.SingleActionEditor saeAlteredStatusOnAttacked;
        private global::RogueCustomsDungeonEditor.Controls.SingleActionEditor saeAlteredStatusBeforeAttack;
        private global::RogueCustomsDungeonEditor.Controls.SingleActionEditor saeAlteredStatusOnRemove;
        private global::RogueCustomsDungeonEditor.Controls.SingleActionEditor saeItemOnDeath;
        private global::System.Windows.Forms.ComboBox cmbNPCAIType;
        private global::System.Windows.Forms.Label label20;
        private global::System.Windows.Forms.Label lblNPCAIOddsToTargetSelfB;
        private NumericUpDown nudHungerLostPerTurn;
        private Label label31;
        private Button btnPasteAlgorithm;
        private Button btnCopyAlgorithm;
    }
}