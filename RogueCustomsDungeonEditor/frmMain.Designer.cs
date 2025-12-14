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
            msMenu = new MenuStrip();
            editorToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            getHelpHereToolStripMenuItem = new ToolStripMenuItem();
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
            BasicInformationTab = new Controls.Tabs.BasicInformationTab();
            tpLocales = new TabPage();
            LocaleEntriesTab = new Controls.Tabs.LocaleEntriesTab();
            tpTileTypeInfo = new TabPage();
            TileTypeTab = new Controls.Tabs.TileTypeTab();
            tpTileSetInfos = new TabPage();
            TilesetTab = new Controls.Tabs.TilesetTab();
            tpFloorInfos = new TabPage();
            FloorGroupTab = new Controls.Tabs.FloorGroupTab();
            tbActionSchoolInfos = new TabPage();
            ActionSchoolsTab = new Controls.Tabs.ActionSchoolsTab();
            tbElementInfos = new TabPage();
            ElementTab = new Controls.Tabs.ElementTab();
            tpFactionInfos = new TabPage();
            FactionTab = new Controls.Tabs.FactionTab();
            tpLootTableInfos = new TabPage();
            LootTableTab = new Controls.Tabs.LootTableTab();
            tpCurrencyInfo = new TabPage();
            CurrencyTab = new Controls.Tabs.CurrencyTab();
            tpItemSlotInfos = new TabPage();
            ItemSlotsTab = new Controls.Tabs.ItemSlotsTab();
            tpItemTypeInfos = new TabPage();
            ItemTypesTab = new Controls.Tabs.ItemTypesTab();
            tbStatInfos = new TabPage();
            StatTab = new Controls.Tabs.StatTab();
            tpNPCModifiers = new TabPage();
            NPCModifierTab = new Controls.Tabs.NPCModifierTab();
            tpAffixes = new TabPage();
            AffixTab = new Controls.Tabs.AffixTab();
            tpQualityLevels = new TabPage();
            QualityLevelsTab = new Controls.Tabs.QualityLevelsTab();
            tpLearnsets = new TabPage();
            LearnsetTab = new Controls.Tabs.LearnsetTab();
            tpPlayerClass = new TabPage();
            PlayerClassTab = new Controls.Tabs.PlayerClassTab();
            tpNPC = new TabPage();
            NPCTab = new Controls.Tabs.NPCTab();
            tpItem = new TabPage();
            ItemTab = new Controls.Tabs.ItemTab();
            tpTrap = new TabPage();
            TrapTab = new Controls.Tabs.TrapTab();
            tpAlteredStatus = new TabPage();
            AlteredStatusTab = new Controls.Tabs.AlteredStatusTab();
            tpScripts = new TabPage();
            ScriptsTab = new Controls.Tabs.ScriptsTab();
            tpValidation = new TabPage();
            ValidatorTab = new Controls.Tabs.ValidatorTab();
            ofdDungeon = new OpenFileDialog();
            sfdDungeon = new SaveFileDialog();
            tpQuests = new TabPage();
            QuestTab = new Controls.Tabs.QuestTab();
            msMenu.SuspendLayout();
            tsButtons.SuspendLayout();
            tbTabs.SuspendLayout();
            tpBasicInfo.SuspendLayout();
            tpLocales.SuspendLayout();
            tpTileTypeInfo.SuspendLayout();
            tpTileSetInfos.SuspendLayout();
            tpFloorInfos.SuspendLayout();
            tbActionSchoolInfos.SuspendLayout();
            tbElementInfos.SuspendLayout();
            tpFactionInfos.SuspendLayout();
            tpLootTableInfos.SuspendLayout();
            tpCurrencyInfo.SuspendLayout();
            tpItemSlotInfos.SuspendLayout();
            tpItemTypeInfos.SuspendLayout();
            tbStatInfos.SuspendLayout();
            tpNPCModifiers.SuspendLayout();
            tpAffixes.SuspendLayout();
            tpQualityLevels.SuspendLayout();
            tpLearnsets.SuspendLayout();
            tpPlayerClass.SuspendLayout();
            tpNPC.SuspendLayout();
            tpItem.SuspendLayout();
            tpTrap.SuspendLayout();
            tpAlteredStatus.SuspendLayout();
            tpScripts.SuspendLayout();
            tpValidation.SuspendLayout();
            tpQuests.SuspendLayout();
            SuspendLayout();
            // 
            // msMenu
            // 
            msMenu.Items.AddRange(new ToolStripItem[] { editorToolStripMenuItem, getHelpHereToolStripMenuItem });
            msMenu.Location = new System.Drawing.Point(0, 0);
            msMenu.Name = "msMenu";
            msMenu.Size = new System.Drawing.Size(1084, 24);
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
            exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // getHelpHereToolStripMenuItem
            // 
            getHelpHereToolStripMenuItem.Name = "getHelpHereToolStripMenuItem";
            getHelpHereToolStripMenuItem.Size = new System.Drawing.Size(92, 20);
            getHelpHereToolStripMenuItem.Text = "Get help here!";
            getHelpHereToolStripMenuItem.Click += getHelpHereToolStripMenuItem_Click;
            // 
            // tsButtons
            // 
            tsButtons.Items.AddRange(new ToolStripItem[] { tsbNewDungeon, tsbOpenDungeon, tsbSaveDungeon, tsbSaveDungeonAs, tssDungeonElement, tsbAddElement, tsbSaveElement, tsbSaveElementAs, tsbDeleteElement, tssElementValidate, tsbValidateDungeon });
            tsButtons.Location = new System.Drawing.Point(0, 24);
            tsButtons.Name = "tsButtons";
            tsButtons.Size = new System.Drawing.Size(1084, 38);
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
            tvDungeonInfo.Dock = DockStyle.Left;
            tvDungeonInfo.FullRowSelect = true;
            tvDungeonInfo.HideSelection = false;
            tvDungeonInfo.Location = new System.Drawing.Point(0, 62);
            tvDungeonInfo.Name = "tvDungeonInfo";
            tvDungeonInfo.Size = new System.Drawing.Size(217, 549);
            tvDungeonInfo.TabIndex = 2;
            tvDungeonInfo.AfterSelect += tvDungeonInfo_AfterSelect;
            // 
            // tbTabs
            // 
            tbTabs.Controls.Add(tpBasicInfo);
            tbTabs.Controls.Add(tpLocales);
            tbTabs.Controls.Add(tpTileTypeInfo);
            tbTabs.Controls.Add(tpTileSetInfos);
            tbTabs.Controls.Add(tpFloorInfos);
            tbTabs.Controls.Add(tbActionSchoolInfos);
            tbTabs.Controls.Add(tbElementInfos);
            tbTabs.Controls.Add(tpFactionInfos);
            tbTabs.Controls.Add(tpLootTableInfos);
            tbTabs.Controls.Add(tpCurrencyInfo);
            tbTabs.Controls.Add(tpItemSlotInfos);
            tbTabs.Controls.Add(tpItemTypeInfos);
            tbTabs.Controls.Add(tbStatInfos);
            tbTabs.Controls.Add(tpNPCModifiers);
            tbTabs.Controls.Add(tpAffixes);
            tbTabs.Controls.Add(tpQualityLevels);
            tbTabs.Controls.Add(tpLearnsets);
            tbTabs.Controls.Add(tpPlayerClass);
            tbTabs.Controls.Add(tpNPC);
            tbTabs.Controls.Add(tpItem);
            tbTabs.Controls.Add(tpTrap);
            tbTabs.Controls.Add(tpAlteredStatus);
            tbTabs.Controls.Add(tpQuests);
            tbTabs.Controls.Add(tpScripts);
            tbTabs.Controls.Add(tpValidation);
            tbTabs.Dock = DockStyle.Fill;
            tbTabs.Location = new System.Drawing.Point(217, 62);
            tbTabs.Name = "tbTabs";
            tbTabs.SelectedIndex = 0;
            tbTabs.Size = new System.Drawing.Size(867, 549);
            tbTabs.TabIndex = 3;
            tbTabs.SelectedIndexChanged += tbTabs_SelectedIndexChanged;
            // 
            // tpBasicInfo
            // 
            tpBasicInfo.Controls.Add(BasicInformationTab);
            tpBasicInfo.Location = new System.Drawing.Point(4, 24);
            tpBasicInfo.Name = "tpBasicInfo";
            tpBasicInfo.Padding = new Padding(3);
            tpBasicInfo.Size = new System.Drawing.Size(859, 521);
            tpBasicInfo.TabIndex = 0;
            tpBasicInfo.Text = "Basic Information";
            tpBasicInfo.UseVisualStyleBackColor = true;
            // 
            // BasicInformationTab
            // 
            BasicInformationTab.AutoSize = true;
            BasicInformationTab.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BasicInformationTab.Dock = DockStyle.Fill;
            BasicInformationTab.Location = new System.Drawing.Point(3, 3);
            BasicInformationTab.Name = "BasicInformationTab";
            BasicInformationTab.Size = new System.Drawing.Size(853, 515);
            BasicInformationTab.TabIndex = 0;
            // 
            // tpLocales
            // 
            tpLocales.Controls.Add(LocaleEntriesTab);
            tpLocales.Location = new System.Drawing.Point(4, 24);
            tpLocales.Name = "tpLocales";
            tpLocales.Padding = new Padding(3);
            tpLocales.Size = new System.Drawing.Size(859, 521);
            tpLocales.TabIndex = 1;
            tpLocales.Text = "Locale Entries";
            tpLocales.UseVisualStyleBackColor = true;
            // 
            // LocaleEntriesTab
            // 
            LocaleEntriesTab.AutoSize = true;
            LocaleEntriesTab.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            LocaleEntriesTab.Dock = DockStyle.Fill;
            LocaleEntriesTab.Location = new System.Drawing.Point(3, 3);
            LocaleEntriesTab.Name = "LocaleEntriesTab";
            LocaleEntriesTab.Size = new System.Drawing.Size(853, 515);
            LocaleEntriesTab.TabIndex = 0;
            // 
            // tpTileTypeInfo
            // 
            tpTileTypeInfo.Controls.Add(TileTypeTab);
            tpTileTypeInfo.Location = new System.Drawing.Point(4, 24);
            tpTileTypeInfo.Name = "tpTileTypeInfo";
            tpTileTypeInfo.Size = new System.Drawing.Size(859, 521);
            tpTileTypeInfo.TabIndex = 11;
            tpTileTypeInfo.Text = "Tile Type";
            tpTileTypeInfo.UseVisualStyleBackColor = true;
            // 
            // TileTypeTab
            // 
            TileTypeTab.Dock = DockStyle.Fill;
            TileTypeTab.Location = new System.Drawing.Point(0, 0);
            TileTypeTab.Name = "TileTypeTab";
            TileTypeTab.Size = new System.Drawing.Size(859, 521);
            TileTypeTab.TabIndex = 0;
            // 
            // tpTileSetInfos
            // 
            tpTileSetInfos.AutoScroll = true;
            tpTileSetInfos.Controls.Add(TilesetTab);
            tpTileSetInfos.Location = new System.Drawing.Point(4, 24);
            tpTileSetInfos.Name = "tpTileSetInfos";
            tpTileSetInfos.Size = new System.Drawing.Size(859, 521);
            tpTileSetInfos.TabIndex = 10;
            tpTileSetInfos.Text = "Tileset";
            tpTileSetInfos.UseVisualStyleBackColor = true;
            // 
            // TilesetTab
            // 
            TilesetTab.AutoSize = true;
            TilesetTab.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            TilesetTab.Location = new System.Drawing.Point(19, 0);
            TilesetTab.Name = "TilesetTab";
            TilesetTab.Size = new System.Drawing.Size(676, 692);
            TilesetTab.TabIndex = 0;
            // 
            // tpFloorInfos
            // 
            tpFloorInfos.AutoScroll = true;
            tpFloorInfos.Controls.Add(FloorGroupTab);
            tpFloorInfos.Location = new System.Drawing.Point(4, 24);
            tpFloorInfos.Name = "tpFloorInfos";
            tpFloorInfos.Size = new System.Drawing.Size(859, 521);
            tpFloorInfos.TabIndex = 2;
            tpFloorInfos.Text = "Floor Group";
            tpFloorInfos.UseVisualStyleBackColor = true;
            // 
            // FloorGroupTab
            // 
            FloorGroupTab.AutoSize = true;
            FloorGroupTab.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            FloorGroupTab.Location = new System.Drawing.Point(0, 0);
            FloorGroupTab.Name = "FloorGroupTab";
            FloorGroupTab.Size = new System.Drawing.Size(783, 494);
            FloorGroupTab.TabIndex = 0;
            // 
            // tbActionSchoolInfos
            // 
            tbActionSchoolInfos.Controls.Add(ActionSchoolsTab);
            tbActionSchoolInfos.Location = new System.Drawing.Point(4, 24);
            tbActionSchoolInfos.Name = "tbActionSchoolInfos";
            tbActionSchoolInfos.Padding = new Padding(3);
            tbActionSchoolInfos.Size = new System.Drawing.Size(859, 521);
            tbActionSchoolInfos.TabIndex = 15;
            tbActionSchoolInfos.Text = "Action Schools";
            tbActionSchoolInfos.UseVisualStyleBackColor = true;
            // 
            // ActionSchoolsTab
            // 
            ActionSchoolsTab.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ActionSchoolsTab.Dock = DockStyle.Fill;
            ActionSchoolsTab.Location = new System.Drawing.Point(3, 3);
            ActionSchoolsTab.Name = "ActionSchoolsTab";
            ActionSchoolsTab.Size = new System.Drawing.Size(853, 515);
            ActionSchoolsTab.TabIndex = 0;
            // 
            // tbElementInfos
            // 
            tbElementInfos.Controls.Add(ElementTab);
            tbElementInfos.Location = new System.Drawing.Point(4, 24);
            tbElementInfos.Name = "tbElementInfos";
            tbElementInfos.Size = new System.Drawing.Size(859, 521);
            tbElementInfos.TabIndex = 13;
            tbElementInfos.Text = "Attack Element";
            tbElementInfos.UseVisualStyleBackColor = true;
            // 
            // ElementTab
            // 
            ElementTab.AutoSize = true;
            ElementTab.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ElementTab.Dock = DockStyle.Fill;
            ElementTab.Location = new System.Drawing.Point(0, 0);
            ElementTab.Name = "ElementTab";
            ElementTab.Size = new System.Drawing.Size(859, 521);
            ElementTab.TabIndex = 0;
            // 
            // tpFactionInfos
            // 
            tpFactionInfos.Controls.Add(FactionTab);
            tpFactionInfos.Location = new System.Drawing.Point(4, 24);
            tpFactionInfos.Name = "tpFactionInfos";
            tpFactionInfos.Size = new System.Drawing.Size(859, 521);
            tpFactionInfos.TabIndex = 3;
            tpFactionInfos.Text = "Faction";
            tpFactionInfos.UseVisualStyleBackColor = true;
            // 
            // FactionTab
            // 
            FactionTab.AutoSize = true;
            FactionTab.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            FactionTab.Dock = DockStyle.Fill;
            FactionTab.Location = new System.Drawing.Point(0, 0);
            FactionTab.Name = "FactionTab";
            FactionTab.Size = new System.Drawing.Size(859, 521);
            FactionTab.TabIndex = 0;
            // 
            // tpLootTableInfos
            // 
            tpLootTableInfos.Controls.Add(LootTableTab);
            tpLootTableInfos.Location = new System.Drawing.Point(4, 24);
            tpLootTableInfos.Name = "tpLootTableInfos";
            tpLootTableInfos.Size = new System.Drawing.Size(859, 521);
            tpLootTableInfos.TabIndex = 16;
            tpLootTableInfos.Text = "Loot Tables";
            tpLootTableInfos.UseVisualStyleBackColor = true;
            // 
            // LootTableTab
            // 
            LootTableTab.Dock = DockStyle.Fill;
            LootTableTab.Location = new System.Drawing.Point(0, 0);
            LootTableTab.Name = "LootTableTab";
            LootTableTab.Size = new System.Drawing.Size(859, 521);
            LootTableTab.TabIndex = 0;
            // 
            // tpCurrencyInfo
            // 
            tpCurrencyInfo.Controls.Add(CurrencyTab);
            tpCurrencyInfo.Location = new System.Drawing.Point(4, 24);
            tpCurrencyInfo.Name = "tpCurrencyInfo";
            tpCurrencyInfo.Size = new System.Drawing.Size(859, 521);
            tpCurrencyInfo.TabIndex = 17;
            tpCurrencyInfo.Text = "Currency";
            tpCurrencyInfo.UseVisualStyleBackColor = true;
            // 
            // CurrencyTab
            // 
            CurrencyTab.Dock = DockStyle.Fill;
            CurrencyTab.Location = new System.Drawing.Point(0, 0);
            CurrencyTab.Name = "CurrencyTab";
            CurrencyTab.Size = new System.Drawing.Size(859, 521);
            CurrencyTab.TabIndex = 0;
            // 
            // tpItemSlotInfos
            // 
            tpItemSlotInfos.Controls.Add(ItemSlotsTab);
            tpItemSlotInfos.Location = new System.Drawing.Point(4, 24);
            tpItemSlotInfos.Name = "tpItemSlotInfos";
            tpItemSlotInfos.Padding = new Padding(3);
            tpItemSlotInfos.Size = new System.Drawing.Size(859, 521);
            tpItemSlotInfos.TabIndex = 20;
            tpItemSlotInfos.Text = "Item Slots";
            tpItemSlotInfos.UseVisualStyleBackColor = true;
            // 
            // ItemSlotsTab
            // 
            ItemSlotsTab.Dock = DockStyle.Fill;
            ItemSlotsTab.Location = new System.Drawing.Point(3, 3);
            ItemSlotsTab.Name = "ItemSlotsTab";
            ItemSlotsTab.Size = new System.Drawing.Size(853, 515);
            ItemSlotsTab.TabIndex = 0;
            // 
            // tpItemTypeInfos
            // 
            tpItemTypeInfos.Controls.Add(ItemTypesTab);
            tpItemTypeInfos.Location = new System.Drawing.Point(4, 24);
            tpItemTypeInfos.Name = "tpItemTypeInfos";
            tpItemTypeInfos.Padding = new Padding(3);
            tpItemTypeInfos.Size = new System.Drawing.Size(859, 521);
            tpItemTypeInfos.TabIndex = 21;
            tpItemTypeInfos.Text = "Item Types";
            tpItemTypeInfos.UseVisualStyleBackColor = true;
            // 
            // ItemTypesTab
            // 
            ItemTypesTab.Dock = DockStyle.Fill;
            ItemTypesTab.Location = new System.Drawing.Point(3, 3);
            ItemTypesTab.Name = "ItemTypesTab";
            ItemTypesTab.Size = new System.Drawing.Size(853, 515);
            ItemTypesTab.TabIndex = 0;
            // 
            // tbStatInfos
            // 
            tbStatInfos.Controls.Add(StatTab);
            tbStatInfos.Location = new System.Drawing.Point(4, 24);
            tbStatInfos.Name = "tbStatInfos";
            tbStatInfos.Padding = new Padding(3);
            tbStatInfos.Size = new System.Drawing.Size(859, 521);
            tbStatInfos.TabIndex = 12;
            tbStatInfos.Text = "Stat";
            tbStatInfos.UseVisualStyleBackColor = true;
            // 
            // StatTab
            // 
            StatTab.AutoSize = true;
            StatTab.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            StatTab.Dock = DockStyle.Fill;
            StatTab.Location = new System.Drawing.Point(3, 3);
            StatTab.Name = "StatTab";
            StatTab.Size = new System.Drawing.Size(853, 515);
            StatTab.TabIndex = 0;
            // 
            // tpNPCModifiers
            // 
            tpNPCModifiers.Controls.Add(NPCModifierTab);
            tpNPCModifiers.Location = new System.Drawing.Point(4, 24);
            tpNPCModifiers.Name = "tpNPCModifiers";
            tpNPCModifiers.Padding = new Padding(3);
            tpNPCModifiers.Size = new System.Drawing.Size(859, 521);
            tpNPCModifiers.TabIndex = 22;
            tpNPCModifiers.Text = "NPC Modifiers";
            tpNPCModifiers.UseVisualStyleBackColor = true;
            // 
            // NPCModifierTab
            // 
            NPCModifierTab.Dock = DockStyle.Fill;
            NPCModifierTab.Location = new System.Drawing.Point(3, 3);
            NPCModifierTab.Name = "NPCModifierTab";
            NPCModifierTab.Size = new System.Drawing.Size(853, 515);
            NPCModifierTab.TabIndex = 0;
            // 
            // tpAffixes
            // 
            tpAffixes.Controls.Add(AffixTab);
            tpAffixes.Location = new System.Drawing.Point(4, 24);
            tpAffixes.Name = "tpAffixes";
            tpAffixes.Size = new System.Drawing.Size(859, 521);
            tpAffixes.TabIndex = 18;
            tpAffixes.Text = "Affixes";
            tpAffixes.UseVisualStyleBackColor = true;
            // 
            // AffixTab
            // 
            AffixTab.Dock = DockStyle.Fill;
            AffixTab.Location = new System.Drawing.Point(0, 0);
            AffixTab.Name = "AffixTab";
            AffixTab.Size = new System.Drawing.Size(859, 521);
            AffixTab.TabIndex = 0;
            // 
            // tpQualityLevels
            // 
            tpQualityLevels.Controls.Add(QualityLevelsTab);
            tpQualityLevels.Location = new System.Drawing.Point(4, 24);
            tpQualityLevels.Name = "tpQualityLevels";
            tpQualityLevels.Size = new System.Drawing.Size(859, 521);
            tpQualityLevels.TabIndex = 19;
            tpQualityLevels.Text = "Quality Levels";
            tpQualityLevels.UseVisualStyleBackColor = true;
            // 
            // QualityLevelsTab
            // 
            QualityLevelsTab.Dock = DockStyle.Fill;
            QualityLevelsTab.Location = new System.Drawing.Point(0, 0);
            QualityLevelsTab.Name = "QualityLevelsTab";
            QualityLevelsTab.Size = new System.Drawing.Size(859, 521);
            QualityLevelsTab.TabIndex = 0;
            // 
            // tpLearnsets
            // 
            tpLearnsets.Controls.Add(LearnsetTab);
            tpLearnsets.Location = new System.Drawing.Point(4, 24);
            tpLearnsets.Name = "tpLearnsets";
            tpLearnsets.Size = new System.Drawing.Size(859, 521);
            tpLearnsets.TabIndex = 23;
            tpLearnsets.Text = "Learnset";
            tpLearnsets.UseVisualStyleBackColor = true;
            // 
            // LearnsetTab
            // 
            LearnsetTab.Dock = DockStyle.Fill;
            LearnsetTab.Location = new System.Drawing.Point(0, 0);
            LearnsetTab.Name = "LearnsetTab";
            LearnsetTab.Size = new System.Drawing.Size(859, 521);
            LearnsetTab.TabIndex = 0;
            // 
            // tpPlayerClass
            // 
            tpPlayerClass.AutoScroll = true;
            tpPlayerClass.Controls.Add(PlayerClassTab);
            tpPlayerClass.Location = new System.Drawing.Point(4, 24);
            tpPlayerClass.Name = "tpPlayerClass";
            tpPlayerClass.Size = new System.Drawing.Size(859, 521);
            tpPlayerClass.TabIndex = 4;
            tpPlayerClass.Text = "Player Class";
            tpPlayerClass.UseVisualStyleBackColor = true;
            // 
            // PlayerClassTab
            // 
            PlayerClassTab.AutoSize = true;
            PlayerClassTab.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            PlayerClassTab.Dock = DockStyle.Top;
            PlayerClassTab.Location = new System.Drawing.Point(0, 0);
            PlayerClassTab.Name = "PlayerClassTab";
            PlayerClassTab.Size = new System.Drawing.Size(842, 885);
            PlayerClassTab.TabIndex = 0;
            // 
            // tpNPC
            // 
            tpNPC.AutoScroll = true;
            tpNPC.Controls.Add(NPCTab);
            tpNPC.Location = new System.Drawing.Point(4, 24);
            tpNPC.Name = "tpNPC";
            tpNPC.Size = new System.Drawing.Size(859, 521);
            tpNPC.TabIndex = 5;
            tpNPC.Text = "NPC";
            tpNPC.UseVisualStyleBackColor = true;
            // 
            // NPCTab
            // 
            NPCTab.AutoSize = true;
            NPCTab.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            NPCTab.Dock = DockStyle.Top;
            NPCTab.Location = new System.Drawing.Point(0, 0);
            NPCTab.Name = "NPCTab";
            NPCTab.Size = new System.Drawing.Size(842, 1416);
            NPCTab.TabIndex = 0;
            // 
            // tpItem
            // 
            tpItem.AutoScroll = true;
            tpItem.Controls.Add(ItemTab);
            tpItem.Location = new System.Drawing.Point(4, 24);
            tpItem.Name = "tpItem";
            tpItem.Size = new System.Drawing.Size(859, 521);
            tpItem.TabIndex = 6;
            tpItem.Text = "Item";
            tpItem.UseVisualStyleBackColor = true;
            // 
            // ItemTab
            // 
            ItemTab.AutoSize = true;
            ItemTab.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ItemTab.Dock = DockStyle.Top;
            ItemTab.Location = new System.Drawing.Point(0, 0);
            ItemTab.Name = "ItemTab";
            ItemTab.Size = new System.Drawing.Size(842, 642);
            ItemTab.TabIndex = 0;
            // 
            // tpTrap
            // 
            tpTrap.Controls.Add(TrapTab);
            tpTrap.Location = new System.Drawing.Point(4, 24);
            tpTrap.Name = "tpTrap";
            tpTrap.Size = new System.Drawing.Size(859, 521);
            tpTrap.TabIndex = 7;
            tpTrap.Text = "Trap";
            tpTrap.UseVisualStyleBackColor = true;
            // 
            // TrapTab
            // 
            TrapTab.AutoSize = true;
            TrapTab.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            TrapTab.Dock = DockStyle.Top;
            TrapTab.Location = new System.Drawing.Point(0, 0);
            TrapTab.Name = "TrapTab";
            TrapTab.Size = new System.Drawing.Size(859, 304);
            TrapTab.TabIndex = 0;
            // 
            // tpAlteredStatus
            // 
            tpAlteredStatus.Controls.Add(AlteredStatusTab);
            tpAlteredStatus.Location = new System.Drawing.Point(4, 24);
            tpAlteredStatus.Name = "tpAlteredStatus";
            tpAlteredStatus.Size = new System.Drawing.Size(859, 521);
            tpAlteredStatus.TabIndex = 8;
            tpAlteredStatus.Text = "Altered Status";
            tpAlteredStatus.UseVisualStyleBackColor = true;
            // 
            // AlteredStatusTab
            // 
            AlteredStatusTab.AutoSize = true;
            AlteredStatusTab.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            AlteredStatusTab.Dock = DockStyle.Top;
            AlteredStatusTab.Location = new System.Drawing.Point(0, 0);
            AlteredStatusTab.Name = "AlteredStatusTab";
            AlteredStatusTab.Size = new System.Drawing.Size(859, 310);
            AlteredStatusTab.TabIndex = 0;
            // 
            // tpScripts
            // 
            tpScripts.AutoScroll = true;
            tpScripts.Controls.Add(ScriptsTab);
            tpScripts.Location = new System.Drawing.Point(4, 24);
            tpScripts.Name = "tpScripts";
            tpScripts.Size = new System.Drawing.Size(859, 521);
            tpScripts.TabIndex = 14;
            tpScripts.Text = "Scripts";
            tpScripts.UseVisualStyleBackColor = true;
            // 
            // ScriptsTab
            // 
            ScriptsTab.Dock = DockStyle.Fill;
            ScriptsTab.Location = new System.Drawing.Point(0, 0);
            ScriptsTab.Name = "ScriptsTab";
            ScriptsTab.Size = new System.Drawing.Size(859, 521);
            ScriptsTab.TabIndex = 0;
            // 
            // tpValidation
            // 
            tpValidation.Controls.Add(ValidatorTab);
            tpValidation.Location = new System.Drawing.Point(4, 24);
            tpValidation.Name = "tpValidation";
            tpValidation.Size = new System.Drawing.Size(859, 521);
            tpValidation.TabIndex = 9;
            tpValidation.Text = "Validation Results";
            tpValidation.UseVisualStyleBackColor = true;
            // 
            // ValidatorTab
            // 
            ValidatorTab.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ValidatorTab.Dock = DockStyle.Fill;
            ValidatorTab.Location = new System.Drawing.Point(0, 0);
            ValidatorTab.Name = "ValidatorTab";
            ValidatorTab.Size = new System.Drawing.Size(859, 521);
            ValidatorTab.TabIndex = 0;
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
            // tpQuests
            // 
            tpQuests.Controls.Add(QuestTab);
            tpQuests.Location = new System.Drawing.Point(4, 24);
            tpQuests.Name = "tpQuests";
            tpQuests.Size = new System.Drawing.Size(859, 521);
            tpQuests.TabIndex = 24;
            tpQuests.Text = "Quest";
            tpQuests.UseVisualStyleBackColor = true;
            // 
            // QuestTab
            // 
            QuestTab.AutoScroll = true;
            QuestTab.Dock = DockStyle.Fill;
            QuestTab.Location = new System.Drawing.Point(0, 0);
            QuestTab.Name = "QuestTab";
            QuestTab.Size = new System.Drawing.Size(859, 521);
            QuestTab.TabIndex = 0;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1084, 611);
            Controls.Add(tbTabs);
            Controls.Add(tvDungeonInfo);
            Controls.Add(tsButtons);
            Controls.Add(msMenu);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = msMenu;
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
            tpTileTypeInfo.ResumeLayout(false);
            tpTileSetInfos.ResumeLayout(false);
            tpTileSetInfos.PerformLayout();
            tpFloorInfos.ResumeLayout(false);
            tpFloorInfos.PerformLayout();
            tbActionSchoolInfos.ResumeLayout(false);
            tbElementInfos.ResumeLayout(false);
            tbElementInfos.PerformLayout();
            tpFactionInfos.ResumeLayout(false);
            tpFactionInfos.PerformLayout();
            tpLootTableInfos.ResumeLayout(false);
            tpCurrencyInfo.ResumeLayout(false);
            tpItemSlotInfos.ResumeLayout(false);
            tpItemTypeInfos.ResumeLayout(false);
            tbStatInfos.ResumeLayout(false);
            tbStatInfos.PerformLayout();
            tpNPCModifiers.ResumeLayout(false);
            tpAffixes.ResumeLayout(false);
            tpQualityLevels.ResumeLayout(false);
            tpLearnsets.ResumeLayout(false);
            tpPlayerClass.ResumeLayout(false);
            tpPlayerClass.PerformLayout();
            tpNPC.ResumeLayout(false);
            tpNPC.PerformLayout();
            tpItem.ResumeLayout(false);
            tpItem.PerformLayout();
            tpTrap.ResumeLayout(false);
            tpTrap.PerformLayout();
            tpAlteredStatus.ResumeLayout(false);
            tpAlteredStatus.PerformLayout();
            tpScripts.ResumeLayout(false);
            tpValidation.ResumeLayout(false);
            tpQuests.ResumeLayout(false);
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
        private TabPage tpNPC;
        private TabPage tpItem;
        private TabPage tpTrap;
        private TabPage tpAlteredStatus;
        private TabPage tpValidation;
        private OpenFileDialog ofdDungeon;
        private ToolStripButton tsbAddElement;
        private ToolStripButton tsbSaveElement;
        private ToolStripButton tsbSaveElementAs;
        private ToolStripButton tsbDeleteElement;
        private ToolStripSeparator tssElementValidate;
        private global::System.Windows.Forms.SaveFileDialog sfdDungeon;
        private global::System.Windows.Forms.TabPage tpTileSetInfos;
        private Controls.Tabs.BasicInformationTab BasicInformationTab;
        private Controls.Tabs.LocaleEntriesTab LocaleEntriesTab;
        private Controls.Tabs.TilesetTab TilesetTab;
        private Controls.Tabs.FloorGroupTab FloorGroupTab;
        private Controls.Tabs.FactionTab FactionTab;
        private TabPage tpPlayerClass;
        private Controls.Tabs.PlayerClassTab PlayerClassTab;
        private Controls.Tabs.NPCTab NPCTab;
        private Controls.Tabs.ItemTab ItemTab;
        private Controls.Tabs.TrapTab TrapTab;
        private Controls.Tabs.AlteredStatusTab AlteredStatusTab;
        private Controls.Tabs.ValidatorTab ValidatorTab;
        private TabPage tpTileTypeInfo;
        private Controls.Tabs.TileTypeTab TileTypeTab;
        private TabPage tbStatInfos;
        private Controls.Tabs.StatTab StatTab;
        private TabPage tbElementInfos;
        private Controls.Tabs.ElementTab ElementTab;
        private TabPage tpScripts;
        private Controls.Tabs.ScriptsTab ScriptsTab;
        private TabPage tbActionSchoolInfos;
        private Controls.Tabs.ActionSchoolsTab ActionSchoolsTab;
        private TabPage tpLootTableInfos;
        private Controls.Tabs.LootTableTab LootTableTab;
        private TabPage tpCurrencyInfo;
        private Controls.Tabs.CurrencyTab CurrencyTab;
        private TabPage tpAffixes;
        private TabPage tpQualityLevels;
        private Controls.Tabs.AffixTab AffixTab;
        private Controls.Tabs.QualityLevelsTab QualityLevelsTab;
        private TabPage tpItemSlotInfos;
        private TabPage tpItemTypeInfos;
        private Controls.Tabs.ItemSlotsTab ItemSlotsTab;
        private Controls.Tabs.ItemTypesTab ItemTypesTab;
        private TabPage tpNPCModifiers;
        private Controls.Tabs.NPCModifierTab NPCModifierTab;
        private ToolStripMenuItem getHelpHereToolStripMenuItem;
        private TabPage tpLearnsets;
        private Controls.Tabs.LearnsetTab LearnsetTab;
        private TabPage tpQuests;
        private Controls.Tabs.QuestTab QuestTab;
    }
}