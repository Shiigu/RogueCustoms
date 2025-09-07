using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    partial class LootTableTab
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
            dgvLootTable = new DataGridView();
            btnTestLootTable = new Button();
            label1 = new Label();
            lblCategory = new Label();
            lblLootTable = new Label();
            lblCurrency = new Label();
            lblItem = new Label();
            chkLootTableOverridesQualityLevelOdds = new CheckBox();
            qlsLootTableQualityLevelOdds = new QualityLevelSheet();
            PickId = new DataGridViewComboBoxColumn();
            Weight = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvLootTable).BeginInit();
            SuspendLayout();
            // 
            // dgvLootTable
            // 
            dgvLootTable.AllowUserToResizeColumns = false;
            dgvLootTable.AllowUserToResizeRows = false;
            dgvLootTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLootTable.Columns.AddRange(new DataGridViewColumn[] { PickId, Weight });
            dgvLootTable.Location = new System.Drawing.Point(0, 0);
            dgvLootTable.MultiSelect = false;
            dgvLootTable.Name = "dgvLootTable";
            dgvLootTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLootTable.Size = new System.Drawing.Size(451, 315);
            dgvLootTable.TabIndex = 0;
            // 
            // btnTestLootTable
            // 
            btnTestLootTable.Location = new System.Drawing.Point(172, 321);
            btnTestLootTable.Name = "btnTestLootTable";
            btnTestLootTable.Size = new System.Drawing.Size(136, 28);
            btnTestLootTable.TabIndex = 1;
            btnTestLootTable.Text = "Test Loot Table";
            btnTestLootTable.UseVisualStyleBackColor = true;
            btnTestLootTable.Click += btnTestLootTable_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(457, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(69, 15);
            label1.TabIndex = 2;
            label1.Text = "Color labels";
            // 
            // lblCategory
            // 
            lblCategory.BorderStyle = BorderStyle.FixedSingle;
            lblCategory.Location = new System.Drawing.Point(457, 28);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new System.Drawing.Size(163, 17);
            lblCategory.TabIndex = 3;
            lblCategory.Text = "Category";
            // 
            // lblLootTable
            // 
            lblLootTable.BorderStyle = BorderStyle.FixedSingle;
            lblLootTable.Location = new System.Drawing.Point(457, 44);
            lblLootTable.Name = "lblLootTable";
            lblLootTable.Size = new System.Drawing.Size(163, 17);
            lblLootTable.TabIndex = 4;
            lblLootTable.Text = "Loot Table";
            lblLootTable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCurrency
            // 
            lblCurrency.BorderStyle = BorderStyle.FixedSingle;
            lblCurrency.Location = new System.Drawing.Point(457, 60);
            lblCurrency.Name = "lblCurrency";
            lblCurrency.Size = new System.Drawing.Size(163, 17);
            lblCurrency.TabIndex = 5;
            lblCurrency.Text = "Currency";
            // 
            // lblItem
            // 
            lblItem.BorderStyle = BorderStyle.FixedSingle;
            lblItem.Location = new System.Drawing.Point(457, 76);
            lblItem.Name = "lblItem";
            lblItem.Size = new System.Drawing.Size(163, 17);
            lblItem.TabIndex = 6;
            lblItem.Text = "Specific Item";
            // 
            // chkLootTableOverridesQualityLevelOdds
            // 
            chkLootTableOverridesQualityLevelOdds.AutoSize = true;
            chkLootTableOverridesQualityLevelOdds.Location = new System.Drawing.Point(457, 109);
            chkLootTableOverridesQualityLevelOdds.Name = "chkLootTableOverridesQualityLevelOdds";
            chkLootTableOverridesQualityLevelOdds.Size = new System.Drawing.Size(257, 19);
            chkLootTableOverridesQualityLevelOdds.TabIndex = 7;
            chkLootTableOverridesQualityLevelOdds.Text = "All drops have the same Quality Level Odds:";
            chkLootTableOverridesQualityLevelOdds.UseVisualStyleBackColor = true;
            chkLootTableOverridesQualityLevelOdds.CheckedChanged += chkLootTableOverridesQualityLevelOdds_CheckedChanged;
            // 
            // qlsLootTableQualityLevelOdds
            // 
            qlsLootTableQualityLevelOdds.Location = new System.Drawing.Point(478, 140);
            qlsLootTableQualityLevelOdds.Name = "qlsLootTableQualityLevelOdds";
            qlsLootTableQualityLevelOdds.Size = new System.Drawing.Size(203, 175);
            qlsLootTableQualityLevelOdds.TabIndex = 8;
            qlsLootTableQualityLevelOdds.Visible = false;
            // 
            // PickId
            // 
            PickId.FillWeight = 250F;
            PickId.HeaderText = "Pick Id";
            PickId.Name = "PickId";
            PickId.Width = 202;
            // 
            // Weight
            // 
            Weight.FillWeight = 250F;
            Weight.HeaderText = "Chance to Pick";
            Weight.Name = "Weight";
            Weight.Width = 202;
            // 
            // LootTableTab
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(qlsLootTableQualityLevelOdds);
            Controls.Add(chkLootTableOverridesQualityLevelOdds);
            Controls.Add(lblItem);
            Controls.Add(lblCurrency);
            Controls.Add(lblLootTable);
            Controls.Add(lblCategory);
            Controls.Add(label1);
            Controls.Add(btnTestLootTable);
            Controls.Add(dgvLootTable);
            Name = "LootTableTab";
            Size = new System.Drawing.Size(712, 352);
            ((System.ComponentModel.ISupportInitialize)dgvLootTable).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvLootTable;
        private System.Windows.Forms.Button btnTestLootTable;
        private Label label1;
        private Label lblCategory;
        private Label lblLootTable;
        private Label lblCurrency;
        private Label lblItem;
        private CheckBox chkLootTableOverridesQualityLevelOdds;
        private QualityLevelSheet qlsLootTableQualityLevelOdds;
        private DataGridViewComboBoxColumn PickId;
        private DataGridViewTextBoxColumn Weight;
    }
}
