using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    partial class CurrencyTab
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(CurrencyTab));
            label2 = new System.Windows.Forms.Label();
            crsCurrency = new ConsoleRepresentationSelector();
            fklblCurrencyNameLocale = new System.Windows.Forms.Button();
            txtCurrencyName = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            dgvCurrencyPileTypes = new System.Windows.Forms.DataGridView();
            Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Minimum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Maximum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            fklblCurrencyDescriptionLocale = new System.Windows.Forms.Button();
            txtCurrencyDescription = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)dgvCurrencyPileTypes).BeginInit();
            SuspendLayout();
            // 
            // label2
            // 
            label2.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            label2.Location = new System.Drawing.Point(364, 15);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(131, 52);
            label2.TabIndex = 254;
            label2.Text = "Appearance";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // crsCurrency
            // 
            crsCurrency.Location = new System.Drawing.Point(497, 3);
            crsCurrency.Name = "crsCurrency";
            crsCurrency.Size = new System.Drawing.Size(211, 83);
            crsCurrency.TabIndex = 253;
            // 
            // fklblCurrencyNameLocale
            // 
            fklblCurrencyNameLocale.Enabled = false;
            fklblCurrencyNameLocale.FlatAppearance.BorderSize = 0;
            fklblCurrencyNameLocale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblCurrencyNameLocale.Image = (System.Drawing.Image)resources.GetObject("fklblCurrencyNameLocale.Image");
            fklblCurrencyNameLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblCurrencyNameLocale.Location = new System.Drawing.Point(3, 50);
            fklblCurrencyNameLocale.Name = "fklblCurrencyNameLocale";
            fklblCurrencyNameLocale.Size = new System.Drawing.Size(343, 42);
            fklblCurrencyNameLocale.TabIndex = 252;
            fklblCurrencyNameLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblCurrencyNameLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblCurrencyNameLocale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblCurrencyNameLocale.UseVisualStyleBackColor = true;
            fklblCurrencyNameLocale.Visible = false;
            // 
            // txtCurrencyName
            // 
            txtCurrencyName.Location = new System.Drawing.Point(3, 21);
            txtCurrencyName.Name = "txtCurrencyName";
            txtCurrencyName.Size = new System.Drawing.Size(343, 23);
            txtCurrencyName.TabIndex = 251;
            txtCurrencyName.TextChanged += txtCurrencyName_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(3, 3);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(39, 15);
            label1.TabIndex = 250;
            label1.Text = "Name";
            // 
            // label3
            // 
            label3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            label3.Location = new System.Drawing.Point(364, 75);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(329, 52);
            label3.TabIndex = 255;
            label3.Text = "Currency Pile Types";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dgvCurrencyPileTypes
            // 
            dgvCurrencyPileTypes.AllowUserToResizeColumns = false;
            dgvCurrencyPileTypes.AllowUserToResizeRows = false;
            dgvCurrencyPileTypes.AllowUserToDeleteRows = true;
            dgvCurrencyPileTypes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCurrencyPileTypes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCurrencyPileTypes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { Id, Minimum, Maximum });
            dgvCurrencyPileTypes.Location = new System.Drawing.Point(352, 121);
            dgvCurrencyPileTypes.Name = "dgvCurrencyPileTypes";
            dgvCurrencyPileTypes.Size = new System.Drawing.Size(355, 178);
            dgvCurrencyPileTypes.TabIndex = 256;
            dgvCurrencyPileTypes.EditingControlShowing += dgvCurrencyPileTypes_EditingControlShowing;
            // 
            // Id
            // 
            Id.HeaderText = "Pile Type Id";
            Id.Name = "Id";
            // 
            // Minimum
            // 
            Minimum.HeaderText = "Minimum";
            Minimum.Name = "Minimum";
            // 
            // Maximum
            // 
            Maximum.HeaderText = "Maximum";
            Maximum.Name = "Maximum";
            // 
            // fklblCurrencyDescriptionLocale
            // 
            fklblCurrencyDescriptionLocale.Enabled = false;
            fklblCurrencyDescriptionLocale.FlatAppearance.BorderSize = 0;
            fklblCurrencyDescriptionLocale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblCurrencyDescriptionLocale.Image = (System.Drawing.Image)resources.GetObject("fklblCurrencyDescriptionLocale.Image");
            fklblCurrencyDescriptionLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblCurrencyDescriptionLocale.Location = new System.Drawing.Point(3, 146);
            fklblCurrencyDescriptionLocale.Name = "fklblCurrencyDescriptionLocale";
            fklblCurrencyDescriptionLocale.Size = new System.Drawing.Size(329, 42);
            fklblCurrencyDescriptionLocale.TabIndex = 259;
            fklblCurrencyDescriptionLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblCurrencyDescriptionLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblCurrencyDescriptionLocale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblCurrencyDescriptionLocale.UseVisualStyleBackColor = true;
            fklblCurrencyDescriptionLocale.Visible = false;
            // 
            // txtCurrencyDescription
            // 
            txtCurrencyDescription.Location = new System.Drawing.Point(3, 117);
            txtCurrencyDescription.Name = "txtCurrencyDescription";
            txtCurrencyDescription.Size = new System.Drawing.Size(343, 23);
            txtCurrencyDescription.TabIndex = 258;
            txtCurrencyDescription.TextChanged += txtCurrencyDescription_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(3, 99);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(67, 15);
            label4.TabIndex = 257;
            label4.Text = "Description";
            // 
            // CurrencyTab
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(fklblCurrencyDescriptionLocale);
            Controls.Add(txtCurrencyDescription);
            Controls.Add(label4);
            Controls.Add(dgvCurrencyPileTypes);
            Controls.Add(label2);
            Controls.Add(crsCurrency);
            Controls.Add(fklblCurrencyNameLocale);
            Controls.Add(txtCurrencyName);
            Controls.Add(label1);
            Controls.Add(label3);
            Name = "CurrencyTab";
            Size = new System.Drawing.Size(711, 302);
            ((System.ComponentModel.ISupportInitialize)dgvCurrencyPileTypes).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label2;
        private ConsoleRepresentationSelector crsCurrency;
        private System.Windows.Forms.Button fklblCurrencyNameLocale;
        private System.Windows.Forms.TextBox txtCurrencyName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgvCurrencyPileTypes;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Minimum;
        private System.Windows.Forms.DataGridViewTextBoxColumn Maximum;
        private System.Windows.Forms.Button fklblCurrencyDescriptionLocale;
        private System.Windows.Forms.TextBox txtCurrencyDescription;
        private System.Windows.Forms.Label label4;
    }
}
