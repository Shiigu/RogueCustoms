namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    partial class ItemTypesTab
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
            dgvItemTypes = new System.Windows.Forms.DataGridView();
            UnidentifiedActionDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            UnidentifiedActionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            UnidentifiedItemDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            UnidentifiedItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            MinimumQualityLevelForUnidentified = new System.Windows.Forms.DataGridViewComboBoxColumn();
            Slot2 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            Slot1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            PowerType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            Usability = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ItemSlotName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvItemTypes).BeginInit();
            SuspendLayout();
            // 
            // dgvItemTypes
            // 
            dgvItemTypes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvItemTypes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { Id, ItemSlotName, Usability, PowerType, Slot1, Slot2, MinimumQualityLevelForUnidentified, UnidentifiedItemName, UnidentifiedItemDescription, UnidentifiedActionName, UnidentifiedActionDescription });
            dgvItemTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvItemTypes.Location = new System.Drawing.Point(0, 0);
            dgvItemTypes.Name = "dgvItemTypes";
            dgvItemTypes.Size = new System.Drawing.Size(1143, 241);
            dgvItemTypes.TabIndex = 2;
            dgvItemTypes.DataError += dgvItemTypes_DataError;
            // 
            // UnidentifiedActionDescription
            // 
            UnidentifiedActionDescription.HeaderText = "Action Description when Unidentified";
            UnidentifiedActionDescription.Name = "UnidentifiedActionDescription";
            // 
            // UnidentifiedActionName
            // 
            UnidentifiedActionName.HeaderText = "Action Name when Unidentified";
            UnidentifiedActionName.Name = "UnidentifiedActionName";
            // 
            // UnidentifiedItemDescription
            // 
            UnidentifiedItemDescription.HeaderText = "Description when Unidentified";
            UnidentifiedItemDescription.Name = "UnidentifiedItemDescription";
            // 
            // UnidentifiedItemName
            // 
            UnidentifiedItemName.HeaderText = "Name when Unidentified";
            UnidentifiedItemName.Name = "UnidentifiedItemName";
            // 
            // MinimumQualityLevelForUnidentified
            // 
            MinimumQualityLevelForUnidentified.HeaderText = "Is Unidentified Starting at...";
            MinimumQualityLevelForUnidentified.Name = "MinimumQualityLevelForUnidentified";
            // 
            // Slot2
            // 
            Slot2.HeaderText = "Secondary Slot";
            Slot2.Name = "Slot2";
            // 
            // Slot1
            // 
            Slot1.HeaderText = "Primary Slot";
            Slot1.Name = "Slot1";
            // 
            // PowerType
            // 
            PowerType.HeaderText = "Power Is...";
            PowerType.Name = "PowerType";
            // 
            // Usability
            // 
            Usability.HeaderText = "Used Via...";
            Usability.Name = "Usability";
            // 
            // ItemSlotName
            // 
            ItemSlotName.HeaderText = "Name";
            ItemSlotName.Name = "ItemSlotName";
            // 
            // Id
            // 
            Id.HeaderText = "Id";
            Id.Name = "Id";
            Id.Width = 75;
            // 
            // ItemTypesTab
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(dgvItemTypes);
            Name = "ItemTypesTab";
            Size = new System.Drawing.Size(1143, 241);
            ((System.ComponentModel.ISupportInitialize)dgvItemTypes).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvItemTypes;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemSlotName;
        private System.Windows.Forms.DataGridViewComboBoxColumn Usability;
        private System.Windows.Forms.DataGridViewComboBoxColumn PowerType;
        private System.Windows.Forms.DataGridViewComboBoxColumn Slot1;
        private System.Windows.Forms.DataGridViewComboBoxColumn Slot2;
        private System.Windows.Forms.DataGridViewComboBoxColumn MinimumQualityLevelForUnidentified;
        private System.Windows.Forms.DataGridViewTextBoxColumn UnidentifiedItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn UnidentifiedItemDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn UnidentifiedActionName;
        private System.Windows.Forms.DataGridViewTextBoxColumn UnidentifiedActionDescription;
    }
}
