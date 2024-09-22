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
            tpTileSetInfos = new TabPage();
            TilesetTab = new Controls.Tabs.TilesetTab();
            tpFloorInfos = new TabPage();
            FloorGroupTab = new Controls.Tabs.FloorGroupTab();
            tpFactionInfos = new TabPage();
            FactionTab = new Controls.Tabs.FactionTab();
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
            tpValidation = new TabPage();
            ofdDungeon = new OpenFileDialog();
            sfdDungeon = new SaveFileDialog();
            ValidatorTab = new Controls.Tabs.ValidatorTab();
            msMenu.SuspendLayout();
            tsButtons.SuspendLayout();
            tbTabs.SuspendLayout();
            tpBasicInfo.SuspendLayout();
            tpLocales.SuspendLayout();
            tpTileSetInfos.SuspendLayout();
            tpFloorInfos.SuspendLayout();
            tpFactionInfos.SuspendLayout();
            tpPlayerClass.SuspendLayout();
            tpNPC.SuspendLayout();
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
            tpBasicInfo.Controls.Add(BasicInformationTab);
            tpBasicInfo.Location = new System.Drawing.Point(4, 24);
            tpBasicInfo.Name = "tpBasicInfo";
            tpBasicInfo.Padding = new Padding(3);
            tpBasicInfo.Size = new System.Drawing.Size(740, 356);
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
            BasicInformationTab.Size = new System.Drawing.Size(734, 350);
            BasicInformationTab.TabIndex = 0;
            // 
            // tpLocales
            // 
            tpLocales.Controls.Add(LocaleEntriesTab);
            tpLocales.Location = new System.Drawing.Point(4, 24);
            tpLocales.Name = "tpLocales";
            tpLocales.Padding = new Padding(3);
            tpLocales.Size = new System.Drawing.Size(740, 356);
            tpLocales.TabIndex = 1;
            tpLocales.Text = "Locale Entries";
            tpLocales.UseVisualStyleBackColor = true;
            // 
            // LocaleEntriesTab
            // 
            LocaleEntriesTab.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            LocaleEntriesTab.AutoSize = true;
            LocaleEntriesTab.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            LocaleEntriesTab.Location = new System.Drawing.Point(0, 0);
            LocaleEntriesTab.Name = "LocaleEntriesTab";
            LocaleEntriesTab.Size = new System.Drawing.Size(732, 352);
            LocaleEntriesTab.TabIndex = 0;
            // 
            // tpTileSetInfos
            // 
            tpTileSetInfos.AutoScroll = true;
            tpTileSetInfos.Controls.Add(TilesetTab);
            tpTileSetInfos.Location = new System.Drawing.Point(4, 24);
            tpTileSetInfos.Name = "tpTileSetInfos";
            tpTileSetInfos.Size = new System.Drawing.Size(740, 356);
            tpTileSetInfos.TabIndex = 10;
            tpTileSetInfos.Text = "Tileset";
            tpTileSetInfos.UseVisualStyleBackColor = true;
            // 
            // TilesetTab
            // 
            TilesetTab.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TilesetTab.AutoSize = true;
            TilesetTab.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            TilesetTab.Location = new System.Drawing.Point(20, 0);
            TilesetTab.Name = "TilesetTab";
            TilesetTab.Size = new System.Drawing.Size(676, 1173);
            TilesetTab.TabIndex = 0;
            // 
            // tpFloorInfos
            // 
            tpFloorInfos.AutoScroll = true;
            tpFloorInfos.Controls.Add(FloorGroupTab);
            tpFloorInfos.Location = new System.Drawing.Point(4, 24);
            tpFloorInfos.Name = "tpFloorInfos";
            tpFloorInfos.Size = new System.Drawing.Size(740, 356);
            tpFloorInfos.TabIndex = 2;
            tpFloorInfos.Text = "Floor Group";
            tpFloorInfos.UseVisualStyleBackColor = true;
            // 
            // FloorGroupTab
            // 
            FloorGroupTab.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            FloorGroupTab.AutoSize = true;
            FloorGroupTab.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            FloorGroupTab.Location = new System.Drawing.Point(0, 0);
            FloorGroupTab.Name = "FloorGroupTab";
            FloorGroupTab.Size = new System.Drawing.Size(708, 346);
            FloorGroupTab.TabIndex = 0;
            // 
            // tpFactionInfos
            // 
            tpFactionInfos.Controls.Add(FactionTab);
            tpFactionInfos.Location = new System.Drawing.Point(4, 24);
            tpFactionInfos.Name = "tpFactionInfos";
            tpFactionInfos.Size = new System.Drawing.Size(740, 356);
            tpFactionInfos.TabIndex = 3;
            tpFactionInfos.Text = "Faction";
            tpFactionInfos.UseVisualStyleBackColor = true;
            // 
            // FactionTab
            // 
            FactionTab.AutoSize = true;
            FactionTab.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            FactionTab.Location = new System.Drawing.Point(4, 12);
            FactionTab.Name = "FactionTab";
            FactionTab.Size = new System.Drawing.Size(732, 328);
            FactionTab.TabIndex = 0;
            // 
            // tpPlayerClass
            // 
            tpPlayerClass.AutoScroll = true;
            tpPlayerClass.Controls.Add(PlayerClassTab);
            tpPlayerClass.Location = new System.Drawing.Point(4, 24);
            tpPlayerClass.Name = "tpPlayerClass";
            tpPlayerClass.Size = new System.Drawing.Size(740, 356);
            tpPlayerClass.TabIndex = 4;
            tpPlayerClass.Text = "Player Class";
            tpPlayerClass.UseVisualStyleBackColor = true;
            // 
            // PlayerClassTab
            // 
            PlayerClassTab.AutoSize = true;
            PlayerClassTab.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            PlayerClassTab.Location = new System.Drawing.Point(5, 0);
            PlayerClassTab.Name = "PlayerClassTab";
            PlayerClassTab.Size = new System.Drawing.Size(714, 866);
            PlayerClassTab.TabIndex = 0;
            // 
            // tpNPC
            // 
            tpNPC.AutoScroll = true;
            tpNPC.Controls.Add(NPCTab);
            tpNPC.Location = new System.Drawing.Point(4, 24);
            tpNPC.Name = "tpNPC";
            tpNPC.Size = new System.Drawing.Size(740, 356);
            tpNPC.TabIndex = 5;
            tpNPC.Text = "NPC";
            tpNPC.UseVisualStyleBackColor = true;
            // 
            // NPCTab
            // 
            NPCTab.AutoSize = true;
            NPCTab.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            NPCTab.Location = new System.Drawing.Point(4, 3);
            NPCTab.Name = "NPCTab";
            NPCTab.Size = new System.Drawing.Size(714, 928);
            NPCTab.TabIndex = 0;
            // 
            // tpItem
            // 
            tpItem.Controls.Add(ItemTab);
            tpItem.Location = new System.Drawing.Point(4, 24);
            tpItem.Name = "tpItem";
            tpItem.Size = new System.Drawing.Size(740, 356);
            tpItem.TabIndex = 6;
            tpItem.Text = "Item";
            tpItem.UseVisualStyleBackColor = true;
            // 
            // ItemTab
            // 
            ItemTab.AutoSize = true;
            ItemTab.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ItemTab.Location = new System.Drawing.Point(3, 4);
            ItemTab.Name = "ItemTab";
            ItemTab.Size = new System.Drawing.Size(733, 348);
            ItemTab.TabIndex = 0;
            // 
            // tpTrap
            // 
            tpTrap.Controls.Add(TrapTab);
            tpTrap.Location = new System.Drawing.Point(4, 24);
            tpTrap.Name = "tpTrap";
            tpTrap.Size = new System.Drawing.Size(740, 356);
            tpTrap.TabIndex = 7;
            tpTrap.Text = "Trap";
            tpTrap.UseVisualStyleBackColor = true;
            // 
            // TrapTab
            // 
            TrapTab.AutoSize = true;
            TrapTab.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            TrapTab.Location = new System.Drawing.Point(4, 3);
            TrapTab.Name = "TrapTab";
            TrapTab.Size = new System.Drawing.Size(733, 304);
            TrapTab.TabIndex = 0;
            // 
            // tpAlteredStatus
            // 
            tpAlteredStatus.Controls.Add(AlteredStatusTab);
            tpAlteredStatus.Location = new System.Drawing.Point(4, 24);
            tpAlteredStatus.Name = "tpAlteredStatus";
            tpAlteredStatus.Size = new System.Drawing.Size(740, 356);
            tpAlteredStatus.TabIndex = 8;
            tpAlteredStatus.Text = "Altered Status";
            tpAlteredStatus.UseVisualStyleBackColor = true;
            // 
            // AlteredStatusTab
            // 
            AlteredStatusTab.AutoSize = true;
            AlteredStatusTab.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            AlteredStatusTab.Location = new System.Drawing.Point(0, 0);
            AlteredStatusTab.Name = "AlteredStatusTab";
            AlteredStatusTab.Size = new System.Drawing.Size(733, 310);
            AlteredStatusTab.TabIndex = 0;
            // 
            // tpValidation
            // 
            tpValidation.Controls.Add(ValidatorTab);
            tpValidation.Location = new System.Drawing.Point(4, 24);
            tpValidation.Name = "tpValidation";
            tpValidation.Size = new System.Drawing.Size(740, 356);
            tpValidation.TabIndex = 9;
            tpValidation.Text = "Validation Results";
            tpValidation.UseVisualStyleBackColor = true;
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
            // ValidatorTab
            // 
            ValidatorTab.AutoSize = true;
            ValidatorTab.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ValidatorTab.Location = new System.Drawing.Point(0, 0);
            ValidatorTab.Name = "ValidatorTab";
            ValidatorTab.Size = new System.Drawing.Size(743, 359);
            ValidatorTab.TabIndex = 0;
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
            tpTileSetInfos.ResumeLayout(false);
            tpTileSetInfos.PerformLayout();
            tpFloorInfos.ResumeLayout(false);
            tpFloorInfos.PerformLayout();
            tpFactionInfos.ResumeLayout(false);
            tpFactionInfos.PerformLayout();
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
            tpValidation.ResumeLayout(false);
            tpValidation.PerformLayout();
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
    }
}