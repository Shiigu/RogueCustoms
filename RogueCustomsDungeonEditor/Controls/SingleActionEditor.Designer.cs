namespace RogueCustomsDungeonEditor.Controls
{
    partial class SingleActionEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SingleActionEditor));
            tlpAction = new System.Windows.Forms.TableLayoutPanel();
            btnDelete = new System.Windows.Forms.Button();
            btnPaste = new System.Windows.Forms.Button();
            btnCopy = new System.Windows.Forms.Button();
            lblDescription = new System.Windows.Forms.Label();
            btnEdit = new System.Windows.Forms.Button();
            tlpAction.SuspendLayout();
            SuspendLayout();
            // 
            // tlpAction
            // 
            tlpAction.AutoSize = true;
            tlpAction.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tlpAction.ColumnCount = 5;
            tlpAction.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tlpAction.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            tlpAction.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            tlpAction.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            tlpAction.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            tlpAction.Controls.Add(btnDelete, 4, 0);
            tlpAction.Controls.Add(btnPaste, 3, 0);
            tlpAction.Controls.Add(btnCopy, 2, 0);
            tlpAction.Controls.Add(lblDescription, 0, 0);
            tlpAction.Controls.Add(btnEdit, 1, 0);
            tlpAction.Dock = System.Windows.Forms.DockStyle.Fill;
            tlpAction.Location = new System.Drawing.Point(0, 0);
            tlpAction.Name = "tlpAction";
            tlpAction.RowCount = 1;
            tlpAction.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tlpAction.Size = new System.Drawing.Size(214, 32);
            tlpAction.TabIndex = 0;
            // 
            // btnDelete
            // 
            btnDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            btnDelete.Enabled = false;
            btnDelete.Image = (System.Drawing.Image)resources.GetObject("btnDelete.Image");
            btnDelete.Location = new System.Drawing.Point(185, 3);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(26, 26);
            btnDelete.TabIndex = 4;
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnPaste
            // 
            btnPaste.Dock = System.Windows.Forms.DockStyle.Fill;
            btnPaste.Enabled = false;
            btnPaste.Image = (System.Drawing.Image)resources.GetObject("btnPaste.Image");
            btnPaste.Location = new System.Drawing.Point(153, 3);
            btnPaste.Name = "btnPaste";
            btnPaste.Size = new System.Drawing.Size(26, 26);
            btnPaste.TabIndex = 3;
            btnPaste.UseVisualStyleBackColor = true;
            btnPaste.Click += btnPaste_Click;
            // 
            // btnCopy
            // 
            btnCopy.Dock = System.Windows.Forms.DockStyle.Fill;
            btnCopy.Enabled = false;
            btnCopy.Image = (System.Drawing.Image)resources.GetObject("btnCopy.Image");
            btnCopy.Location = new System.Drawing.Point(121, 3);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new System.Drawing.Size(26, 26);
            btnCopy.TabIndex = 2;
            btnCopy.UseVisualStyleBackColor = true;
            btnCopy.Click += btnCopy_Click;
            // 
            // lblDescription
            // 
            lblDescription.AutoSize = true;
            lblDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            lblDescription.Location = new System.Drawing.Point(3, 0);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new System.Drawing.Size(80, 32);
            lblDescription.TabIndex = 0;
            lblDescription.Text = "lblDescription";
            lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnEdit
            // 
            btnEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            btnEdit.Image = (System.Drawing.Image)resources.GetObject("btnEdit.Image");
            btnEdit.Location = new System.Drawing.Point(89, 3);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new System.Drawing.Size(26, 26);
            btnEdit.TabIndex = 1;
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += btnEdit_Click;
            // 
            // SingleActionEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Controls.Add(tlpAction);
            Name = "SingleActionEditor";
            Size = new System.Drawing.Size(214, 32);
            tlpAction.ResumeLayout(false);
            tlpAction.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpAction;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.Button btnCopy;
    }
}
