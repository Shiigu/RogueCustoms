namespace RogueCustomsDungeonEditor.Controls
{
    partial class StartingInventorySelector
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
            lbStartingInventory = new System.Windows.Forms.ListBox();
            btnRemove = new System.Windows.Forms.Button();
            btnAdd = new System.Windows.Forms.Button();
            cmbItemChoices = new System.Windows.Forms.ComboBox();
            label55 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // lbStartingInventory
            // 
            lbStartingInventory.FormattingEnabled = true;
            lbStartingInventory.ItemHeight = 15;
            lbStartingInventory.Location = new System.Drawing.Point(143, 0);
            lbStartingInventory.Name = "lbStartingInventory";
            lbStartingInventory.Size = new System.Drawing.Size(150, 79);
            lbStartingInventory.TabIndex = 82;
            lbStartingInventory.SelectedIndexChanged += lbStartingInventory_SelectedIndexChanged;
            // 
            // btnRemove
            // 
            btnRemove.Enabled = false;
            btnRemove.Location = new System.Drawing.Point(62, 52);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new System.Drawing.Size(59, 23);
            btnRemove.TabIndex = 81;
            btnRemove.Text = "Remove";
            btnRemove.UseVisualStyleBackColor = true;
            btnRemove.Click += btnRemove_Click;
            // 
            // btnAdd
            // 
            btnAdd.Enabled = false;
            btnAdd.Location = new System.Drawing.Point(0, 52);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new System.Drawing.Size(56, 23);
            btnAdd.TabIndex = 80;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // cmbItemChoices
            // 
            cmbItemChoices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbItemChoices.FormattingEnabled = true;
            cmbItemChoices.Location = new System.Drawing.Point(0, 23);
            cmbItemChoices.Name = "cmbItemChoices";
            cmbItemChoices.Size = new System.Drawing.Size(121, 23);
            cmbItemChoices.TabIndex = 79;
            cmbItemChoices.SelectedIndexChanged += cmbItemChoices_SelectedIndexChanged;
            // 
            // label55
            // 
            label55.AutoSize = true;
            label55.Location = new System.Drawing.Point(0, 0);
            label55.Name = "label55";
            label55.Size = new System.Drawing.Size(92, 15);
            label55.TabIndex = 78;
            label55.Text = "Initial Inventory:";
            // 
            // StartingInventorySelector
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(lbStartingInventory);
            Controls.Add(btnRemove);
            Controls.Add(btnAdd);
            Controls.Add(cmbItemChoices);
            Controls.Add(label55);
            Name = "StartingInventorySelector";
            Size = new System.Drawing.Size(293, 79);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListBox lbStartingInventory;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ComboBox cmbItemChoices;
        private System.Windows.Forms.Label label55;
    }
}
