namespace RogueCustomsDungeonEditor.Controls
{
    partial class MultiActionEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultiActionEditor));
            btnAdd = new System.Windows.Forms.Button();
            lbActions = new System.Windows.Forms.ListBox();
            lblDescription = new System.Windows.Forms.Label();
            btnEdit = new System.Windows.Forms.Button();
            btnCopy = new System.Windows.Forms.Button();
            btnPaste = new System.Windows.Forms.Button();
            btnDelete = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // btnAdd
            // 
            btnAdd.Image = (System.Drawing.Image)resources.GetObject("btnAdd.Image");
            btnAdd.Location = new System.Drawing.Point(258, 12);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new System.Drawing.Size(32, 32);
            btnAdd.TabIndex = 94;
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // lbActions
            // 
            lbActions.FormattingEnabled = true;
            lbActions.ItemHeight = 15;
            lbActions.Location = new System.Drawing.Point(155, 0);
            lbActions.Name = "lbActions";
            lbActions.Size = new System.Drawing.Size(97, 94);
            lbActions.TabIndex = 93;
            lbActions.SelectedIndexChanged += lbActions_SelectedIndexChanged;
            // 
            // lblDescription
            // 
            lblDescription.Location = new System.Drawing.Point(0, 0);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new System.Drawing.Size(149, 94);
            lblDescription.TabIndex = 92;
            lblDescription.Text = "Can do the following to\r\ninteract with someone:";
            lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnEdit
            // 
            btnEdit.Enabled = false;
            btnEdit.Image = (System.Drawing.Image)resources.GetObject("btnEdit.Image");
            btnEdit.Location = new System.Drawing.Point(258, 50);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new System.Drawing.Size(32, 32);
            btnEdit.TabIndex = 95;
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += btnEdit_Click;
            // 
            // btnCopy
            // 
            btnCopy.Enabled = false;
            btnCopy.Image = (System.Drawing.Image)resources.GetObject("btnCopy.Image");
            btnCopy.Location = new System.Drawing.Point(296, 12);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new System.Drawing.Size(32, 32);
            btnCopy.TabIndex = 96;
            btnCopy.UseVisualStyleBackColor = true;
            btnCopy.Click += btnCopy_Click;
            // 
            // btnPaste
            // 
            btnPaste.Enabled = false;
            btnPaste.Image = (System.Drawing.Image)resources.GetObject("btnPaste.Image");
            btnPaste.Location = new System.Drawing.Point(296, 50);
            btnPaste.Name = "btnPaste";
            btnPaste.Size = new System.Drawing.Size(32, 32);
            btnPaste.TabIndex = 97;
            btnPaste.UseVisualStyleBackColor = true;
            btnPaste.Click += btnPaste_Click;
            // 
            // btnDelete
            // 
            btnDelete.Enabled = false;
            btnDelete.Image = (System.Drawing.Image)resources.GetObject("btnDelete.Image");
            btnDelete.Location = new System.Drawing.Point(334, 31);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(32, 32);
            btnDelete.TabIndex = 98;
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // MultiActionEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(btnDelete);
            Controls.Add(btnPaste);
            Controls.Add(btnCopy);
            Controls.Add(btnEdit);
            Controls.Add(btnAdd);
            Controls.Add(lbActions);
            Controls.Add(lblDescription);
            Name = "MultiActionEditor";
            Size = new System.Drawing.Size(368, 94);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ListBox lbActions;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.Button btnDelete;
    }
}
