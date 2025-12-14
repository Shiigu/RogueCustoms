namespace RogueCustomsDungeonEditor.Controls
{
    partial class EquipmentSheet
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
            var dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            var dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            dgvEquipment = new System.Windows.Forms.DataGridView();
            Slot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Item = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvEquipment).BeginInit();
            SuspendLayout();
            // 
            // dgvEquipment
            // 
            dgvEquipment.AllowUserToAddRows = false;
            dgvEquipment.AllowUserToDeleteRows = false;
            dgvEquipment.AllowUserToResizeColumns = false;
            dgvEquipment.AllowUserToResizeRows = false;
            dgvEquipment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEquipment.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { Slot, Item });
            dgvEquipment.Dock = System.Windows.Forms.DockStyle.Top;
            dgvEquipment.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            dgvEquipment.EnableHeadersVisualStyles = false;
            dgvEquipment.Location = new System.Drawing.Point(0, 0);
            dgvEquipment.Name = "dgvEquipment";
            dgvEquipment.RowHeadersVisible = false;
            dgvEquipment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            dgvEquipment.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            dgvEquipment.Size = new System.Drawing.Size(204, 141);
            dgvEquipment.TabIndex = 1;
            dgvEquipment.CellValueChanged += dgvEquipment_CellValueChanged;
            // 
            // Slot
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Consolas", 8.25F);
            Slot.DefaultCellStyle = dataGridViewCellStyle1;
            Slot.Frozen = true;
            Slot.HeaderText = "Slot";
            Slot.Name = "Slot";
            Slot.ReadOnly = true;
            Slot.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Item
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 8.25F);
            Item.DefaultCellStyle = dataGridViewCellStyle2;
            Item.Frozen = true;
            Item.HeaderText = "Item";
            Item.Name = "Item";
            Item.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            Item.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // EquipmentSheet
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(dgvEquipment);
            Name = "EquipmentSheet";
            Size = new System.Drawing.Size(204, 142);
            ((System.ComponentModel.ISupportInitialize)dgvEquipment).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvEquipment;
        private System.Windows.Forms.DataGridViewTextBoxColumn Slot;
        private System.Windows.Forms.DataGridViewComboBoxColumn Item;
    }
}
