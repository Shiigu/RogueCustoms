namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    partial class NPCModifierTab
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
            btnRemoveNPCModifier = new System.Windows.Forms.Button();
            btnAddNPCModifier = new System.Windows.Forms.Button();
            panel1 = new System.Windows.Forms.Panel();
            tlpNPCModifiers = new System.Windows.Forms.TableLayoutPanel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // btnRemoveNPCModifier
            // 
            btnRemoveNPCModifier.Location = new System.Drawing.Point(361, 330);
            btnRemoveNPCModifier.Name = "btnRemoveNPCModifier";
            btnRemoveNPCModifier.Size = new System.Drawing.Size(139, 23);
            btnRemoveNPCModifier.TabIndex = 5;
            btnRemoveNPCModifier.Text = "Remove NPC Modifier";
            btnRemoveNPCModifier.UseVisualStyleBackColor = true;
            btnRemoveNPCModifier.Click += btnRemoveNPCModifier_Click;
            // 
            // btnAddNPCModifier
            // 
            btnAddNPCModifier.Location = new System.Drawing.Point(216, 330);
            btnAddNPCModifier.Name = "btnAddNPCModifier";
            btnAddNPCModifier.Size = new System.Drawing.Size(139, 23);
            btnAddNPCModifier.TabIndex = 4;
            btnAddNPCModifier.Text = "Add an NPC Modifier";
            btnAddNPCModifier.UseVisualStyleBackColor = true;
            btnAddNPCModifier.Click += btnAddNPCModifier_Click;
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(tlpNPCModifiers);
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(732, 324);
            panel1.TabIndex = 6;
            // 
            // tlpNPCModifiers
            // 
            tlpNPCModifiers.AutoScroll = true;
            tlpNPCModifiers.AutoSize = true;
            tlpNPCModifiers.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tlpNPCModifiers.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            tlpNPCModifiers.ColumnCount = 1;
            tlpNPCModifiers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tlpNPCModifiers.Dock = System.Windows.Forms.DockStyle.Top;
            tlpNPCModifiers.Location = new System.Drawing.Point(0, 0);
            tlpNPCModifiers.Name = "tlpNPCModifiers";
            tlpNPCModifiers.RowCount = 1;
            tlpNPCModifiers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tlpNPCModifiers.Size = new System.Drawing.Size(732, 2);
            tlpNPCModifiers.TabIndex = 1;
            // 
            // NPCModifierTab
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(panel1);
            Controls.Add(btnRemoveNPCModifier);
            Controls.Add(btnAddNPCModifier);
            Name = "NPCModifierTab";
            Size = new System.Drawing.Size(732, 363);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Button btnRemoveNPCModifier;
        private System.Windows.Forms.Button btnAddNPCModifier;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tlpNPCModifiers;
    }
}
