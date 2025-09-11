namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    partial class ItemSlotsTab
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
            dgvItemSlots = new System.Windows.Forms.DataGridView();
            Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ItemSlotName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvItemSlots).BeginInit();
            SuspendLayout();
            // 
            // dgvItemSlots
            // 
            dgvItemSlots.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvItemSlots.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { Id, ItemSlotName });
            dgvItemSlots.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvItemSlots.Location = new System.Drawing.Point(0, 0);
            dgvItemSlots.Name = "dgvItemSlots";
            dgvItemSlots.Size = new System.Drawing.Size(444, 335);
            dgvItemSlots.TabIndex = 1;
            // 
            // Id
            // 
            Id.HeaderText = "Id";
            Id.Name = "Id";
            Id.Width = 150;
            // 
            // ItemSlotName
            // 
            ItemSlotName.HeaderText = "Name";
            ItemSlotName.Name = "ItemSlotName";
            ItemSlotName.Width = 250;
            // 
            // ItemSlotsTab
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(dgvItemSlots);
            Name = "ItemSlotsTab";
            Size = new System.Drawing.Size(444, 335);
            ((System.ComponentModel.ISupportInitialize)dgvItemSlots).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvItemSlots;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemSlotName;
    }
}
