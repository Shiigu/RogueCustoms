namespace RogueCustomsDungeonEditor.Controls
{
    partial class QuestItemRewardSheet
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
            dgvItems = new System.Windows.Forms.DataGridView();
            ItemId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ItemLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            QualityLevel = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
            SuspendLayout();
            // 
            // dgvItems
            // 
            dgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { ItemId, ItemLevel, QualityLevel });
            dgvItems.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvItems.Location = new System.Drawing.Point(0, 0);
            dgvItems.Name = "dgvItems";
            dgvItems.Size = new System.Drawing.Size(345, 178);
            dgvItems.TabIndex = 155;
            dgvItems.CellValidating += dgvItems_CellValidating;
            dgvItems.CellValueChanged += dgvItems_CellValueChanged;
            dgvItems.RowsAdded += dgvItems_RowsAdded;
            dgvItems.RowsRemoved += dgvItems_RowsRemoved;
            // 
            // ItemId
            // 
            ItemId.HeaderText = "Item";
            ItemId.Name = "ItemId";
            // 
            // ItemLevel
            // 
            ItemLevel.HeaderText = "Item Level";
            ItemLevel.Name = "ItemLevel";
            // 
            // QualityLevel
            // 
            QualityLevel.HeaderText = "Quality Level";
            QualityLevel.Name = "QualityLevel";
            // 
            // QuestItemRewardSheet
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(dgvItems);
            Name = "QuestItemRewardSheet";
            Size = new System.Drawing.Size(345, 178);
            ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.DataGridViewComboBoxColumn ItemId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemLevel;
        private System.Windows.Forms.DataGridViewComboBoxColumn QualityLevel;
    }
}
