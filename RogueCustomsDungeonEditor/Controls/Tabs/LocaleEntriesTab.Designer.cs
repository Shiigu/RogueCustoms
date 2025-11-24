namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    partial class LocaleEntriesTab
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(LocaleEntriesTab));
            var dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            btnDeleteLocale = new System.Windows.Forms.Button();
            btnAddLocale = new System.Windows.Forms.Button();
            btnUpdateLocale = new System.Windows.Forms.Button();
            fklblMissingLocale = new System.Windows.Forms.Button();
            txtLocaleEntryValue = new System.Windows.Forms.TextBox();
            label7 = new System.Windows.Forms.Label();
            txtLocaleEntryKey = new System.Windows.Forms.TextBox();
            label6 = new System.Windows.Forms.Label();
            dgvLocales = new System.Windows.Forms.DataGridView();
            cmKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            cmValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvLocales).BeginInit();
            SuspendLayout();
            // 
            // btnDeleteLocale
            // 
            btnDeleteLocale.Enabled = false;
            btnDeleteLocale.Location = new System.Drawing.Point(398, 374);
            btnDeleteLocale.Name = "btnDeleteLocale";
            btnDeleteLocale.Size = new System.Drawing.Size(391, 23);
            btnDeleteLocale.TabIndex = 25;
            btnDeleteLocale.Text = "Delete Locale Entry";
            btnDeleteLocale.UseVisualStyleBackColor = true;
            btnDeleteLocale.Click += btnDeleteLocale_Click;
            // 
            // btnAddLocale
            // 
            btnAddLocale.Enabled = false;
            btnAddLocale.Location = new System.Drawing.Point(596, 345);
            btnAddLocale.Name = "btnAddLocale";
            btnAddLocale.Size = new System.Drawing.Size(193, 23);
            btnAddLocale.TabIndex = 24;
            btnAddLocale.Text = "Add New Locale Entry";
            btnAddLocale.UseVisualStyleBackColor = true;
            btnAddLocale.Click += btnAddLocale_Click;
            // 
            // btnUpdateLocale
            // 
            btnUpdateLocale.Enabled = false;
            btnUpdateLocale.Location = new System.Drawing.Point(398, 345);
            btnUpdateLocale.Name = "btnUpdateLocale";
            btnUpdateLocale.Size = new System.Drawing.Size(193, 23);
            btnUpdateLocale.TabIndex = 23;
            btnUpdateLocale.Text = "Update Locale Entry";
            btnUpdateLocale.UseVisualStyleBackColor = true;
            btnUpdateLocale.Click += btnUpdateLocale_Click;
            // 
            // fklblMissingLocale
            // 
            fklblMissingLocale.Enabled = false;
            fklblMissingLocale.FlatAppearance.BorderSize = 0;
            fklblMissingLocale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblMissingLocale.Image = (System.Drawing.Image)resources.GetObject("fklblMissingLocale.Image");
            fklblMissingLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblMissingLocale.Location = new System.Drawing.Point(400, 276);
            fklblMissingLocale.Name = "fklblMissingLocale";
            fklblMissingLocale.Size = new System.Drawing.Size(331, 42);
            fklblMissingLocale.TabIndex = 22;
            fklblMissingLocale.Text = "(Missing locale warning)";
            fklblMissingLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblMissingLocale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblMissingLocale.UseVisualStyleBackColor = true;
            fklblMissingLocale.Visible = false;
            // 
            // txtLocaleEntryValue
            // 
            txtLocaleEntryValue.Enabled = false;
            txtLocaleEntryValue.Location = new System.Drawing.Point(398, 77);
            txtLocaleEntryValue.Multiline = true;
            txtLocaleEntryValue.Name = "txtLocaleEntryValue";
            txtLocaleEntryValue.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtLocaleEntryValue.Size = new System.Drawing.Size(391, 193);
            txtLocaleEntryValue.TabIndex = 21;
            txtLocaleEntryValue.TextChanged += txtLocaleEntryValue_TextChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(398, 59);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(102, 15);
            label7.TabIndex = 20;
            label7.Text = "Locale Entry Value";
            // 
            // txtLocaleEntryKey
            // 
            txtLocaleEntryKey.Enabled = false;
            txtLocaleEntryKey.Location = new System.Drawing.Point(398, 23);
            txtLocaleEntryKey.Name = "txtLocaleEntryKey";
            txtLocaleEntryKey.Size = new System.Drawing.Size(391, 23);
            txtLocaleEntryKey.TabIndex = 19;
            txtLocaleEntryKey.TextChanged += txtLocaleEntryKey_TextChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(398, 5);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(93, 15);
            label6.TabIndex = 18;
            label6.Text = "Locale Entry Key";
            // 
            // dgvLocales
            // 
            dgvLocales.AllowUserToAddRows = false;
            dgvLocales.AllowUserToDeleteRows = false;
            dgvLocales.AllowUserToResizeColumns = false;
            dgvLocales.AllowUserToResizeRows = false;
            dgvLocales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLocales.ColumnHeadersVisible = false;
            dgvLocales.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { cmKey, cmValue });
            dgvLocales.Dock = System.Windows.Forms.DockStyle.Left;
            dgvLocales.Location = new System.Drawing.Point(0, 0);
            dgvLocales.MultiSelect = false;
            dgvLocales.Name = "dgvLocales";
            dgvLocales.ReadOnly = true;
            dgvLocales.RowHeadersVisible = false;
            dgvLocales.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            dgvLocales.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvLocales.Size = new System.Drawing.Size(393, 400);
            dgvLocales.TabIndex = 17;
            dgvLocales.SelectionChanged += dgvLocales_SelectionChanged;
            // 
            // cmKey
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            cmKey.DefaultCellStyle = dataGridViewCellStyle2;
            cmKey.HeaderText = "Key";
            cmKey.Name = "cmKey";
            cmKey.ReadOnly = true;
            // 
            // cmValue
            // 
            cmValue.HeaderText = "Value";
            cmValue.Name = "cmValue";
            cmValue.ReadOnly = true;
            cmValue.Width = 300;
            // 
            // LocaleEntriesTab
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Controls.Add(btnDeleteLocale);
            Controls.Add(btnAddLocale);
            Controls.Add(btnUpdateLocale);
            Controls.Add(fklblMissingLocale);
            Controls.Add(txtLocaleEntryValue);
            Controls.Add(label7);
            Controls.Add(txtLocaleEntryKey);
            Controls.Add(label6);
            Controls.Add(dgvLocales);
            Name = "LocaleEntriesTab";
            Size = new System.Drawing.Size(792, 400);
            ((System.ComponentModel.ISupportInitialize)dgvLocales).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnDeleteLocale;
        private System.Windows.Forms.Button btnAddLocale;
        private System.Windows.Forms.Button btnUpdateLocale;
        private System.Windows.Forms.Button fklblMissingLocale;
        private System.Windows.Forms.TextBox txtLocaleEntryValue;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtLocaleEntryKey;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView dgvLocales;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmValue;
    }
}
