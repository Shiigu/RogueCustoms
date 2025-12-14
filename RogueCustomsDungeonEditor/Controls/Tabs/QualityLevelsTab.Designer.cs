namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    partial class QualityLevelsTab
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(QualityLevelsTab));
            dgvQualityLevels = new System.Windows.Forms.DataGridView();
            Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            QualityLevelName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            MinimumAffixes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            MaximumAffixes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            AttachesWhatToItemName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            QualityColor = new System.Windows.Forms.DataGridViewButtonColumn();
            button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)dgvQualityLevels).BeginInit();
            SuspendLayout();
            // 
            // dgvQualityLevels
            // 
            dgvQualityLevels.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvQualityLevels.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { Id, QualityLevelName, MinimumAffixes, MaximumAffixes, AttachesWhatToItemName, QualityColor });
            dgvQualityLevels.Dock = System.Windows.Forms.DockStyle.Top;
            dgvQualityLevels.Location = new System.Drawing.Point(0, 0);
            dgvQualityLevels.Name = "dgvQualityLevels";
            dgvQualityLevels.Size = new System.Drawing.Size(740, 465);
            dgvQualityLevels.TabIndex = 0;
            dgvQualityLevels.CellClick += dgvQualityLevels_CellClick;
            dgvQualityLevels.CellPainting += dgvQualityLevels_CellPainting;
            dgvQualityLevels.CellValueChanged += dgvQualityLevels_CellValueChanged;
            // 
            // Id
            // 
            Id.HeaderText = "Id";
            Id.Name = "Id";
            // 
            // QualityLevelName
            // 
            QualityLevelName.HeaderText = "Name";
            QualityLevelName.Name = "QualityLevelName";
            QualityLevelName.Width = 125;
            // 
            // MinimumAffixes
            // 
            MinimumAffixes.HeaderText = "Min. Affixes";
            MinimumAffixes.Name = "MinimumAffixes";
            // 
            // MaximumAffixes
            // 
            MaximumAffixes.HeaderText = "Max. Affixes";
            MaximumAffixes.Name = "MaximumAffixes";
            // 
            // AttachesWhatToItemName
            // 
            AttachesWhatToItemName.HeaderText = "Modifies Item Name via...";
            AttachesWhatToItemName.Name = "AttachesWhatToItemName";
            AttachesWhatToItemName.Width = 170;
            // 
            // QualityColor
            // 
            QualityColor.HeaderText = "Quality Color";
            QualityColor.Name = "QualityColor";
            // 
            // button1
            // 
            button1.BackColor = System.Drawing.Color.Transparent;
            button1.Enabled = false;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            button1.Image = (System.Drawing.Image)resources.GetObject("button1.Image");
            button1.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            button1.Location = new System.Drawing.Point(0, 471);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(737, 31);
            button1.TabIndex = 265;
            button1.Text = "To include the Item's base name alongside the Quality Level, make sure the Quality Level Name includes {baseName}.";
            button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            button1.UseVisualStyleBackColor = false;
            // 
            // QualityLevelsTab
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(button1);
            Controls.Add(dgvQualityLevels);
            Name = "QualityLevelsTab";
            Size = new System.Drawing.Size(740, 515);
            ((System.ComponentModel.ISupportInitialize)dgvQualityLevels).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvQualityLevels;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn QualityLevelName;
        private System.Windows.Forms.DataGridViewTextBoxColumn MinimumAffixes;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaximumAffixes;
        private System.Windows.Forms.DataGridViewComboBoxColumn AttachesWhatToItemName;
        private System.Windows.Forms.DataGridViewButtonColumn QualityColor;
    }
}
