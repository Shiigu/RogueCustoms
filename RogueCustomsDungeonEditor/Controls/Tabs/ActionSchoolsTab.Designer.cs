namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    partial class ActionSchoolsTab
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
            dgvSchools = new System.Windows.Forms.DataGridView();
            cmId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            cmName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvSchools).BeginInit();
            SuspendLayout();
            // 
            // dgvSchools
            // 
            dgvSchools.AllowUserToResizeRows = false;
            dgvSchools.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSchools.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { cmId, cmName });
            dgvSchools.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvSchools.Location = new System.Drawing.Point(0, 0);
            dgvSchools.MultiSelect = false;
            dgvSchools.Name = "dgvSchools";
            dgvSchools.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            dgvSchools.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            dgvSchools.Size = new System.Drawing.Size(465, 350);
            dgvSchools.TabIndex = 17;
            // 
            // cmId
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            cmId.DefaultCellStyle = dataGridViewCellStyle1;
            cmId.HeaderText = "Id";
            cmId.Name = "cmId";
            // 
            // cmName
            // 
            cmName.HeaderText = "Name";
            cmName.Name = "cmName";
            cmName.Width = 300;
            // 
            // ActionSchoolsTab
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Controls.Add(dgvSchools);
            Name = "ActionSchoolsTab";
            Size = new System.Drawing.Size(465, 350);
            ((System.ComponentModel.ISupportInitialize)dgvSchools).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.DataGridView dgvSchools;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmId;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmName;
    }
}
