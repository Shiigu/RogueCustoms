namespace RogueCustomsDungeonEditor.Controls
{
    partial class QualityLevelSheet
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
            dgvQualityLevels = new System.Windows.Forms.DataGridView();
            btnCalculateOdds = new System.Windows.Forms.Button();
            QualityLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Odds = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvQualityLevels).BeginInit();
            SuspendLayout();
            // 
            // dgvQualityLevels
            // 
            dgvQualityLevels.AllowUserToAddRows = false;
            dgvQualityLevels.AllowUserToDeleteRows = false;
            dgvQualityLevels.AllowUserToResizeColumns = false;
            dgvQualityLevels.AllowUserToResizeRows = false;
            dgvQualityLevels.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvQualityLevels.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { QualityLevel, Odds });
            dgvQualityLevels.Dock = System.Windows.Forms.DockStyle.Top;
            dgvQualityLevels.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            dgvQualityLevels.EnableHeadersVisualStyles = false;
            dgvQualityLevels.Location = new System.Drawing.Point(0, 0);
            dgvQualityLevels.Name = "dgvQualityLevels";
            dgvQualityLevels.RowHeadersVisible = false;
            dgvQualityLevels.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            dgvQualityLevels.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            dgvQualityLevels.Size = new System.Drawing.Size(203, 141);
            dgvQualityLevels.TabIndex = 0;
            dgvQualityLevels.CellBeginEdit += dgvQualityLevels_CellBeginEdit;
            dgvQualityLevels.CellEndEdit += dgvQualityLevels_CellEndEdit;
            dgvQualityLevels.CellValidating += dgvQualityLevels_CellValidating;
            // 
            // btnCalculateOdds
            // 
            btnCalculateOdds.Location = new System.Drawing.Point(13, 147);
            btnCalculateOdds.Name = "btnCalculateOdds";
            btnCalculateOdds.Size = new System.Drawing.Size(179, 23);
            btnCalculateOdds.TabIndex = 1;
            btnCalculateOdds.Text = "Calculate Odds from Weights";
            btnCalculateOdds.UseVisualStyleBackColor = true;
            btnCalculateOdds.Click += btnCalculateOdds_Click;
            // 
            // QualityLevel
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Consolas", 8.25F);
            QualityLevel.DefaultCellStyle = dataGridViewCellStyle1;
            QualityLevel.Frozen = true;
            QualityLevel.HeaderText = "Quality Level";
            QualityLevel.Name = "QualityLevel";
            QualityLevel.ReadOnly = true;
            QualityLevel.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Odds
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 8.25F);
            Odds.DefaultCellStyle = dataGridViewCellStyle2;
            Odds.Frozen = true;
            Odds.HeaderText = "Weight";
            Odds.Name = "Odds";
            Odds.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // QualityLevelSheet
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(dgvQualityLevels);
            Controls.Add(btnCalculateOdds);
            Name = "QualityLevelSheet";
            Size = new System.Drawing.Size(203, 175);
            ((System.ComponentModel.ISupportInitialize)dgvQualityLevels).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvQualityLevels;
        private System.Windows.Forms.Button btnCalculateOdds;
        private System.Windows.Forms.DataGridViewTextBoxColumn QualityLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Odds;
    }
}
