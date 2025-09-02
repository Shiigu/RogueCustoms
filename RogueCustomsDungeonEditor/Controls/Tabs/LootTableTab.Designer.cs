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
            dgvLootTable = new System.Windows.Forms.DataGridView();
            btnTestLootTable = new System.Windows.Forms.Button();
            PickId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            Weight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvLootTable).BeginInit();
            SuspendLayout();
            // 
            // dgvLootTable
            // 
            dgvLootTable.AllowUserToResizeColumns = false;
            dgvLootTable.AllowUserToResizeRows = false;
            dgvLootTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLootTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { PickId, Weight });
            dgvLootTable.Dock = System.Windows.Forms.DockStyle.Top;
            dgvLootTable.Location = new System.Drawing.Point(0, 0);
            dgvLootTable.MultiSelect = false;
            dgvLootTable.Name = "dgvLootTable";
            dgvLootTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            dgvLootTable.Size = new System.Drawing.Size(543, 315);
            dgvLootTable.TabIndex = 0;
            // 
            // btnTestLootTable
            // 
            btnTestLootTable.Location = new System.Drawing.Point(210, 321);
            btnTestLootTable.Name = "btnTestLootTable";
            btnTestLootTable.Size = new System.Drawing.Size(136, 28);
            btnTestLootTable.TabIndex = 1;
            btnTestLootTable.Text = "Test Loot Table";
            btnTestLootTable.UseVisualStyleBackColor = true;
            btnTestLootTable.Click += btnTestLootTable_Click;
            // 
            // PickId
            // 
            PickId.FillWeight = 250F;
            PickId.HeaderText = "Pick Id";
            PickId.Name = "PickId";
            PickId.Width = 250;
            // 
            // Weight
            // 
            Weight.FillWeight = 250F;
            Weight.HeaderText = "Chance to Pick";
            Weight.Name = "Weight";
            Weight.Width = 250;
            // 
            // LootTableTab
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(btnTestLootTable);
            Controls.Add(dgvLootTable);
            Name = "LootTableTab";
            Size = new System.Drawing.Size(543, 352);
            ((System.ComponentModel.ISupportInitialize)dgvLootTable).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvLootTable;
        private System.Windows.Forms.Button btnTestLootTable;
        private System.Windows.Forms.DataGridViewComboBoxColumn PickId;
        private System.Windows.Forms.DataGridViewTextBoxColumn Weight;
    }
}
