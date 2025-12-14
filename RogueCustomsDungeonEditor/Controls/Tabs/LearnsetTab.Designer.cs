namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    partial class LearnsetTab
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
            dgvLearnset = new System.Windows.Forms.DataGridView();
            cmLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            cmLearns = new System.Windows.Forms.DataGridViewComboBoxColumn();
            cmForgets = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvLearnset).BeginInit();
            SuspendLayout();
            // 
            // dgvLearnset
            // 
            dgvLearnset.AllowUserToResizeRows = false;
            dgvLearnset.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLearnset.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { cmLevel, cmLearns, cmForgets });
            dgvLearnset.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvLearnset.Location = new System.Drawing.Point(0, 0);
            dgvLearnset.MultiSelect = false;
            dgvLearnset.Name = "dgvLearnset";
            dgvLearnset.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            dgvLearnset.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            dgvLearnset.Size = new System.Drawing.Size(465, 350);
            dgvLearnset.TabIndex = 18;
            dgvLearnset.CellValidating += dgvLearnset_CellValidating;
            dgvLearnset.CellValueChanged += dgvLearnset_CellValueChanged;
            // 
            // cmLevel
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            cmLevel.DefaultCellStyle = dataGridViewCellStyle1;
            cmLevel.HeaderText = "Level";
            cmLevel.Name = "cmLevel";
            cmLevel.Width = 50;
            // 
            // cmLearns
            // 
            cmLearns.HeaderText = "Learns this Script";
            cmLearns.Name = "cmLearns";
            cmLearns.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            cmLearns.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            cmLearns.Width = 175;
            // 
            // cmForgets
            // 
            cmForgets.HeaderText = "... but only by forgetting";
            cmForgets.Name = "cmForgets";
            cmForgets.Width = 175;
            // 
            // LearnsetTab
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(dgvLearnset);
            Name = "LearnsetTab";
            Size = new System.Drawing.Size(465, 350);
            ((System.ComponentModel.ISupportInitialize)dgvLearnset).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvLearnset;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmLevel;
        private System.Windows.Forms.DataGridViewComboBoxColumn cmLearns;
        private System.Windows.Forms.DataGridViewComboBoxColumn cmForgets;
    }
}
