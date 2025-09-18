namespace RogueCustomsDungeonEditor.HelperForms
{
    partial class frmStaticFloorEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStaticFloorEditor));
            lblFloorGroupTitle = new System.Windows.Forms.Label();
            lblToolbox = new System.Windows.Forms.Label();
            tlpToolbox = new System.Windows.Forms.TableLayoutPanel();
            pnlToolbox = new System.Windows.Forms.Panel();
            fklblToolbox = new System.Windows.Forms.Button();
            flpEditor = new System.Windows.Forms.FlowLayoutPanel();
            pnlToolboxSection = new System.Windows.Forms.Panel();
            pnlGridSection = new System.Windows.Forms.Panel();
            pnlGrid = new System.Windows.Forms.Panel();
            tcEntitySpawns = new System.Windows.Forms.TabControl();
            tcCharacters = new System.Windows.Forms.TabPage();
            dgvCharacters = new System.Windows.Forms.DataGridView();
            CharacterType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            CharacterX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            CharacterY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            CharacterLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            tcItems = new System.Windows.Forms.TabPage();
            dgvItems = new System.Windows.Forms.DataGridView();
            ItemType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ItemX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ItemY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ItemLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ItemQualityLevel = new System.Windows.Forms.DataGridViewComboBoxColumn();
            tlpEditor = new System.Windows.Forms.TableLayoutPanel();
            tlpButtons = new System.Windows.Forms.TableLayoutPanel();
            flpButtons = new System.Windows.Forms.FlowLayoutPanel();
            btnHighlightRooms = new System.Windows.Forms.Button();
            btnHighlightIslands = new System.Windows.Forms.Button();
            btnSave = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            tlpCheckboxes = new System.Windows.Forms.TableLayoutPanel();
            flpCheckboxes = new System.Windows.Forms.FlowLayoutPanel();
            chkUseNPCGenerator = new System.Windows.Forms.CheckBox();
            chkUseItemGenerator = new System.Windows.Forms.CheckBox();
            chkUseTrapGenerator = new System.Windows.Forms.CheckBox();
            pnlToolbox.SuspendLayout();
            flpEditor.SuspendLayout();
            pnlToolboxSection.SuspendLayout();
            pnlGridSection.SuspendLayout();
            tcEntitySpawns.SuspendLayout();
            tcCharacters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCharacters).BeginInit();
            tcItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
            tlpEditor.SuspendLayout();
            tlpButtons.SuspendLayout();
            flpButtons.SuspendLayout();
            tlpCheckboxes.SuspendLayout();
            flpCheckboxes.SuspendLayout();
            SuspendLayout();
            // 
            // lblFloorGroupTitle
            // 
            lblFloorGroupTitle.Dock = System.Windows.Forms.DockStyle.Top;
            lblFloorGroupTitle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            lblFloorGroupTitle.Location = new System.Drawing.Point(0, 0);
            lblFloorGroupTitle.Name = "lblFloorGroupTitle";
            lblFloorGroupTitle.Size = new System.Drawing.Size(1329, 25);
            lblFloorGroupTitle.TabIndex = 37;
            lblFloorGroupTitle.Text = "For Floor Level(s) X (to Y):";
            lblFloorGroupTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblToolbox
            // 
            lblToolbox.Dock = System.Windows.Forms.DockStyle.Top;
            lblToolbox.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblToolbox.Location = new System.Drawing.Point(0, 0);
            lblToolbox.Name = "lblToolbox";
            lblToolbox.Size = new System.Drawing.Size(120, 21);
            lblToolbox.TabIndex = 55;
            lblToolbox.Text = "Toolbox";
            lblToolbox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tlpToolbox
            // 
            tlpToolbox.AutoSize = true;
            tlpToolbox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tlpToolbox.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            tlpToolbox.ColumnCount = 2;
            tlpToolbox.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            tlpToolbox.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            tlpToolbox.Dock = System.Windows.Forms.DockStyle.Top;
            tlpToolbox.Location = new System.Drawing.Point(0, 0);
            tlpToolbox.Name = "tlpToolbox";
            tlpToolbox.RowCount = 1;
            tlpToolbox.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            tlpToolbox.Size = new System.Drawing.Size(117, 14);
            tlpToolbox.TabIndex = 56;
            // 
            // pnlToolbox
            // 
            pnlToolbox.AutoScroll = true;
            pnlToolbox.Controls.Add(tlpToolbox);
            pnlToolbox.Location = new System.Drawing.Point(0, 21);
            pnlToolbox.Name = "pnlToolbox";
            pnlToolbox.Size = new System.Drawing.Size(117, 365);
            pnlToolbox.TabIndex = 61;
            // 
            // fklblToolbox
            // 
            fklblToolbox.Dock = System.Windows.Forms.DockStyle.Fill;
            fklblToolbox.Enabled = false;
            fklblToolbox.FlatAppearance.BorderSize = 0;
            fklblToolbox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblToolbox.Image = (System.Drawing.Image)resources.GetObject("fklblToolbox.Image");
            fklblToolbox.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblToolbox.Location = new System.Drawing.Point(3, 3);
            fklblToolbox.Name = "fklblToolbox";
            fklblToolbox.Size = new System.Drawing.Size(1315, 30);
            fklblToolbox.TabIndex = 234;
            fklblToolbox.Text = "All elements in the Toolbox, sans Empty, Wall, Hallway and Stairs, come with a Floor tile underneath.";
            fklblToolbox.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblToolbox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblToolbox.UseVisualStyleBackColor = true;
            // 
            // flpEditor
            // 
            flpEditor.AutoSize = true;
            flpEditor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            flpEditor.Controls.Add(pnlToolboxSection);
            flpEditor.Controls.Add(pnlGridSection);
            flpEditor.Controls.Add(tcEntitySpawns);
            flpEditor.Location = new System.Drawing.Point(3, 39);
            flpEditor.Name = "flpEditor";
            flpEditor.Size = new System.Drawing.Size(1315, 395);
            flpEditor.TabIndex = 235;
            // 
            // pnlToolboxSection
            // 
            pnlToolboxSection.AutoSize = true;
            pnlToolboxSection.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            pnlToolboxSection.Controls.Add(pnlToolbox);
            pnlToolboxSection.Controls.Add(lblToolbox);
            pnlToolboxSection.Dock = System.Windows.Forms.DockStyle.Top;
            pnlToolboxSection.Location = new System.Drawing.Point(3, 3);
            pnlToolboxSection.Name = "pnlToolboxSection";
            pnlToolboxSection.Size = new System.Drawing.Size(120, 389);
            pnlToolboxSection.TabIndex = 0;
            // 
            // pnlGridSection
            // 
            pnlGridSection.AutoScroll = true;
            pnlGridSection.Controls.Add(pnlGrid);
            pnlGridSection.Location = new System.Drawing.Point(129, 3);
            pnlGridSection.Name = "pnlGridSection";
            pnlGridSection.Size = new System.Drawing.Size(768, 384);
            pnlGridSection.TabIndex = 3;
            // 
            // pnlGrid
            // 
            pnlGrid.Location = new System.Drawing.Point(0, 0);
            pnlGrid.Margin = new System.Windows.Forms.Padding(0);
            pnlGrid.Name = "pnlGrid";
            pnlGrid.Size = new System.Drawing.Size(768, 384);
            pnlGrid.TabIndex = 2;
            // 
            // tcEntitySpawns
            // 
            tcEntitySpawns.Controls.Add(tcCharacters);
            tcEntitySpawns.Controls.Add(tcItems);
            tcEntitySpawns.Location = new System.Drawing.Point(903, 3);
            tcEntitySpawns.Name = "tcEntitySpawns";
            tcEntitySpawns.SelectedIndex = 0;
            tcEntitySpawns.Size = new System.Drawing.Size(409, 384);
            tcEntitySpawns.TabIndex = 238;
            // 
            // tcCharacters
            // 
            tcCharacters.Controls.Add(dgvCharacters);
            tcCharacters.Location = new System.Drawing.Point(4, 24);
            tcCharacters.Name = "tcCharacters";
            tcCharacters.Padding = new System.Windows.Forms.Padding(3);
            tcCharacters.Size = new System.Drawing.Size(401, 356);
            tcCharacters.TabIndex = 0;
            tcCharacters.Text = "Characters";
            tcCharacters.UseVisualStyleBackColor = true;
            // 
            // dgvCharacters
            // 
            dgvCharacters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCharacters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { CharacterType, CharacterX, CharacterY, CharacterLevel });
            dgvCharacters.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvCharacters.Location = new System.Drawing.Point(3, 3);
            dgvCharacters.Name = "dgvCharacters";
            dgvCharacters.Size = new System.Drawing.Size(395, 350);
            dgvCharacters.TabIndex = 0;
            dgvCharacters.CellBeginEdit += dgvCharacters_CellBeginEdit;
            dgvCharacters.CellEndEdit += dgvCharacters_CellEndEdit;
            // 
            // CharacterType
            // 
            CharacterType.HeaderText = "Type";
            CharacterType.Name = "CharacterType";
            CharacterType.ReadOnly = true;
            // 
            // CharacterX
            // 
            CharacterX.HeaderText = "X";
            CharacterX.Name = "CharacterX";
            CharacterX.ReadOnly = true;
            CharacterX.Width = 50;
            // 
            // CharacterY
            // 
            CharacterY.HeaderText = "Y";
            CharacterY.Name = "CharacterY";
            CharacterY.ReadOnly = true;
            CharacterY.Width = 50;
            // 
            // CharacterLevel
            // 
            CharacterLevel.HeaderText = "Level";
            CharacterLevel.Name = "CharacterLevel";
            CharacterLevel.Width = 50;
            // 
            // tcItems
            // 
            tcItems.Controls.Add(dgvItems);
            tcItems.Location = new System.Drawing.Point(4, 24);
            tcItems.Name = "tcItems";
            tcItems.Padding = new System.Windows.Forms.Padding(3);
            tcItems.Size = new System.Drawing.Size(401, 356);
            tcItems.TabIndex = 1;
            tcItems.Text = "Items";
            tcItems.UseVisualStyleBackColor = true;
            // 
            // dgvItems
            // 
            dgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { ItemType, ItemX, ItemY, ItemLevel, ItemQualityLevel });
            dgvItems.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvItems.Location = new System.Drawing.Point(3, 3);
            dgvItems.Name = "dgvItems";
            dgvItems.Size = new System.Drawing.Size(395, 350);
            dgvItems.TabIndex = 1;
            dgvItems.CellBeginEdit += dgvItems_CellBeginEdit;
            dgvItems.CellEndEdit += dgvItems_CellEndEdit;
            // 
            // ItemType
            // 
            ItemType.HeaderText = "Type";
            ItemType.Name = "ItemType";
            ItemType.ReadOnly = true;
            // 
            // ItemX
            // 
            ItemX.HeaderText = "X";
            ItemX.Name = "ItemX";
            ItemX.ReadOnly = true;
            ItemX.Width = 50;
            // 
            // ItemY
            // 
            ItemY.HeaderText = "Y";
            ItemY.Name = "ItemY";
            ItemY.ReadOnly = true;
            ItemY.Width = 50;
            // 
            // ItemLevel
            // 
            ItemLevel.HeaderText = "Level";
            ItemLevel.Name = "ItemLevel";
            ItemLevel.Width = 50;
            // 
            // ItemQualityLevel
            // 
            ItemQualityLevel.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            ItemQualityLevel.HeaderText = "Quality";
            ItemQualityLevel.Name = "ItemQualityLevel";
            // 
            // tlpEditor
            // 
            tlpEditor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tlpEditor.ColumnCount = 1;
            tlpEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tlpEditor.Controls.Add(tlpButtons, 0, 3);
            tlpEditor.Controls.Add(fklblToolbox, 0, 0);
            tlpEditor.Controls.Add(flpEditor, 0, 1);
            tlpEditor.Controls.Add(tlpCheckboxes, 0, 2);
            tlpEditor.Location = new System.Drawing.Point(4, 28);
            tlpEditor.Name = "tlpEditor";
            tlpEditor.RowCount = 4;
            tlpEditor.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tlpEditor.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tlpEditor.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tlpEditor.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tlpEditor.Size = new System.Drawing.Size(1321, 550);
            tlpEditor.TabIndex = 237;
            // 
            // tlpButtons
            // 
            tlpButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tlpButtons.ColumnCount = 3;
            tlpButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tlpButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tlpButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tlpButtons.Controls.Add(flpButtons, 1, 0);
            tlpButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            tlpButtons.Location = new System.Drawing.Point(3, 507);
            tlpButtons.Name = "tlpButtons";
            tlpButtons.RowCount = 1;
            tlpButtons.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tlpButtons.Size = new System.Drawing.Size(1315, 40);
            tlpButtons.TabIndex = 237;
            // 
            // flpButtons
            // 
            flpButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            flpButtons.Controls.Add(btnHighlightRooms);
            flpButtons.Controls.Add(btnHighlightIslands);
            flpButtons.Controls.Add(btnSave);
            flpButtons.Controls.Add(btnCancel);
            flpButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            flpButtons.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            flpButtons.Location = new System.Drawing.Point(395, 3);
            flpButtons.Name = "flpButtons";
            flpButtons.Size = new System.Drawing.Size(525, 34);
            flpButtons.TabIndex = 0;
            // 
            // btnHighlightRooms
            // 
            btnHighlightRooms.Enabled = false;
            btnHighlightRooms.Location = new System.Drawing.Point(3, 3);
            btnHighlightRooms.Name = "btnHighlightRooms";
            btnHighlightRooms.Size = new System.Drawing.Size(125, 23);
            btnHighlightRooms.TabIndex = 240;
            btnHighlightRooms.Text = "Highlight Rooms";
            btnHighlightRooms.UseVisualStyleBackColor = true;
            btnHighlightRooms.Click += btnHighlightRooms_Click;
            // 
            // btnHighlightIslands
            // 
            btnHighlightIslands.Enabled = false;
            btnHighlightIslands.Location = new System.Drawing.Point(134, 3);
            btnHighlightIslands.Name = "btnHighlightIslands";
            btnHighlightIslands.Size = new System.Drawing.Size(125, 23);
            btnHighlightIslands.TabIndex = 241;
            btnHighlightIslands.Text = "Highlight Islands";
            btnHighlightIslands.UseVisualStyleBackColor = true;
            btnHighlightIslands.Click += btnHighlightIslands_Click;
            // 
            // btnSave
            // 
            btnSave.Enabled = false;
            btnSave.Location = new System.Drawing.Point(265, 3);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(125, 23);
            btnSave.TabIndex = 238;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(396, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(125, 23);
            btnCancel.TabIndex = 239;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // tlpCheckboxes
            // 
            tlpCheckboxes.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tlpCheckboxes.ColumnCount = 3;
            tlpCheckboxes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tlpCheckboxes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tlpCheckboxes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tlpCheckboxes.Controls.Add(flpCheckboxes, 1, 0);
            tlpCheckboxes.Dock = System.Windows.Forms.DockStyle.Fill;
            tlpCheckboxes.Location = new System.Drawing.Point(3, 440);
            tlpCheckboxes.Name = "tlpCheckboxes";
            tlpCheckboxes.RowCount = 1;
            tlpCheckboxes.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tlpCheckboxes.Size = new System.Drawing.Size(1315, 61);
            tlpCheckboxes.TabIndex = 236;
            // 
            // flpCheckboxes
            // 
            flpCheckboxes.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            flpCheckboxes.Controls.Add(chkUseNPCGenerator);
            flpCheckboxes.Controls.Add(chkUseItemGenerator);
            flpCheckboxes.Controls.Add(chkUseTrapGenerator);
            flpCheckboxes.Dock = System.Windows.Forms.DockStyle.Fill;
            flpCheckboxes.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            flpCheckboxes.Location = new System.Drawing.Point(432, 3);
            flpCheckboxes.Name = "flpCheckboxes";
            flpCheckboxes.Size = new System.Drawing.Size(450, 55);
            flpCheckboxes.TabIndex = 0;
            // 
            // chkUseNPCGenerator
            // 
            chkUseNPCGenerator.AutoSize = true;
            chkUseNPCGenerator.Location = new System.Drawing.Point(3, 3);
            chkUseNPCGenerator.Name = "chkUseNPCGenerator";
            chkUseNPCGenerator.Size = new System.Drawing.Size(200, 19);
            chkUseNPCGenerator.TabIndex = 0;
            chkUseNPCGenerator.Text = "Generate NPCs after the first turn";
            chkUseNPCGenerator.UseVisualStyleBackColor = true;
            // 
            // chkUseItemGenerator
            // 
            chkUseItemGenerator.AutoSize = true;
            chkUseItemGenerator.Location = new System.Drawing.Point(209, 3);
            chkUseItemGenerator.Name = "chkUseItemGenerator";
            chkUseItemGenerator.Size = new System.Drawing.Size(190, 19);
            chkUseItemGenerator.TabIndex = 1;
            chkUseItemGenerator.Text = "Generate Items on the first turn";
            chkUseItemGenerator.UseVisualStyleBackColor = true;
            // 
            // chkUseTrapGenerator
            // 
            chkUseTrapGenerator.AutoSize = true;
            chkUseTrapGenerator.Location = new System.Drawing.Point(3, 28);
            chkUseTrapGenerator.Name = "chkUseTrapGenerator";
            chkUseTrapGenerator.Size = new System.Drawing.Size(189, 19);
            chkUseTrapGenerator.TabIndex = 2;
            chkUseTrapGenerator.Text = "Generate Traps on the first turn";
            chkUseTrapGenerator.UseVisualStyleBackColor = true;
            // 
            // frmStaticFloorEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new System.Drawing.Size(1329, 586);
            Controls.Add(tlpEditor);
            Controls.Add(lblFloorGroupTitle);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmStaticFloorEditor";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Floor Geometry";
            pnlToolbox.ResumeLayout(false);
            pnlToolbox.PerformLayout();
            flpEditor.ResumeLayout(false);
            flpEditor.PerformLayout();
            pnlToolboxSection.ResumeLayout(false);
            pnlGridSection.ResumeLayout(false);
            tcEntitySpawns.ResumeLayout(false);
            tcCharacters.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCharacters).EndInit();
            tcItems.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
            tlpEditor.ResumeLayout(false);
            tlpEditor.PerformLayout();
            tlpButtons.ResumeLayout(false);
            flpButtons.ResumeLayout(false);
            tlpCheckboxes.ResumeLayout(false);
            flpCheckboxes.ResumeLayout(false);
            flpCheckboxes.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblFloorGroupTitle;
        private System.Windows.Forms.Label lblToolbox;
        private System.Windows.Forms.TableLayoutPanel tlpToolbox;
        private System.Windows.Forms.Panel pnlToolbox;
        private System.Windows.Forms.Button fklblToolbox;
        private System.Windows.Forms.FlowLayoutPanel flpEditor;
        private System.Windows.Forms.Panel pnlToolboxSection;
        private System.Windows.Forms.TableLayoutPanel tlpEditor;
        private System.Windows.Forms.Panel pnlGridSection;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.TableLayoutPanel tlpCheckboxes;
        private System.Windows.Forms.FlowLayoutPanel flpCheckboxes;
        private System.Windows.Forms.TableLayoutPanel tlpButtons;
        private System.Windows.Forms.FlowLayoutPanel flpButtons;
        private System.Windows.Forms.Button btnHighlightRooms;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkUseNPCGenerator;
        private System.Windows.Forms.CheckBox chkUseItemGenerator;
        private System.Windows.Forms.CheckBox chkUseTrapGenerator;
        private System.Windows.Forms.Button btnHighlightIslands;
        private System.Windows.Forms.TabControl tcEntitySpawns;
        private System.Windows.Forms.TabPage tcCharacters;
        private System.Windows.Forms.DataGridView dgvCharacters;
        private System.Windows.Forms.TabPage tcItems;
        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemX;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemY;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemLevel;
        private System.Windows.Forms.DataGridViewComboBoxColumn ItemQualityLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn CharacterType;
        private System.Windows.Forms.DataGridViewTextBoxColumn CharacterX;
        private System.Windows.Forms.DataGridViewTextBoxColumn CharacterY;
        private System.Windows.Forms.DataGridViewTextBoxColumn CharacterLevel;
    }
}