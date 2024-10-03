namespace RogueCustomsDungeonEditor.Controls
{
    partial class ItemStatsSheet
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
            dgvStatsModifiers = new System.Windows.Forms.DataGridView();
            Stat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Modifier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvStatsModifiers).BeginInit();
            SuspendLayout();
            // 
            // dgvStatsModifiers
            // 
            dgvStatsModifiers.AllowUserToAddRows = false;
            dgvStatsModifiers.AllowUserToDeleteRows = false;
            dgvStatsModifiers.AllowUserToResizeColumns = false;
            dgvStatsModifiers.AllowUserToResizeRows = false;
            dgvStatsModifiers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvStatsModifiers.ColumnHeadersVisible = false;
            dgvStatsModifiers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { Stat, Modifier });
            dgvStatsModifiers.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvStatsModifiers.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            dgvStatsModifiers.EnableHeadersVisualStyles = false;
            dgvStatsModifiers.Location = new System.Drawing.Point(0, 0);
            dgvStatsModifiers.Name = "dgvStatsModifiers";
            dgvStatsModifiers.RowHeadersVisible = false;
            dgvStatsModifiers.RowTemplate.Height = 25;
            dgvStatsModifiers.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            dgvStatsModifiers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            dgvStatsModifiers.Size = new System.Drawing.Size(203, 141);
            dgvStatsModifiers.TabIndex = 0;
            dgvStatsModifiers.CellBeginEdit += dgvStatsModifiers_CellBeginEdit;
            dgvStatsModifiers.CellEndEdit += dgvStatsModifiers_CellEndEdit;
            dgvStatsModifiers.CellValidating += dgvStatsModifiers_CellValidating;
            // 
            // Stat
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Stat.DefaultCellStyle = dataGridViewCellStyle1;
            Stat.Frozen = true;
            Stat.HeaderText = "Stat";
            Stat.Name = "Stat";
            Stat.ReadOnly = true;
            Stat.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            Stat.Width = 120;
            // 
            // Modifier
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Modifier.DefaultCellStyle = dataGridViewCellStyle2;
            Modifier.Frozen = true;
            Modifier.HeaderText = "Modifier";
            Modifier.Name = "Modifier";
            Modifier.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            Modifier.Width = 80;
            // 
            // ItemStatsSheet
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(dgvStatsModifiers);
            Name = "ItemStatsSheet";
            Size = new System.Drawing.Size(203, 141);
            ((System.ComponentModel.ISupportInitialize)dgvStatsModifiers).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvStatsModifiers;
        private System.Windows.Forms.DataGridViewTextBoxColumn Stat;
        private System.Windows.Forms.DataGridViewTextBoxColumn Modifier;
    }
}
