namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    partial class AffixTab
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
            btnRemoveAffix = new System.Windows.Forms.Button();
            btnAddAffix = new System.Windows.Forms.Button();
            panel1 = new System.Windows.Forms.Panel();
            tlpAffixes = new System.Windows.Forms.TableLayoutPanel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // btnRemoveAffix
            // 
            btnRemoveAffix.Location = new System.Drawing.Point(361, 330);
            btnRemoveAffix.Name = "btnRemoveAffix";
            btnRemoveAffix.Size = new System.Drawing.Size(115, 23);
            btnRemoveAffix.TabIndex = 5;
            btnRemoveAffix.Text = "Remove Affix";
            btnRemoveAffix.UseVisualStyleBackColor = true;
            btnRemoveAffix.Click += btnRemoveAffix_Click;
            // 
            // btnAddAffix
            // 
            btnAddAffix.Location = new System.Drawing.Point(240, 330);
            btnAddAffix.Name = "btnAddAffix";
            btnAddAffix.Size = new System.Drawing.Size(115, 23);
            btnAddAffix.TabIndex = 4;
            btnAddAffix.Text = "Add an Affix";
            btnAddAffix.UseVisualStyleBackColor = true;
            btnAddAffix.Click += btnAddAffix_Click;
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(tlpAffixes);
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(732, 324);
            panel1.TabIndex = 6;
            // 
            // tlpAffixes
            // 
            tlpAffixes.AutoScroll = true;
            tlpAffixes.AutoSize = true;
            tlpAffixes.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tlpAffixes.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            tlpAffixes.ColumnCount = 1;
            tlpAffixes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tlpAffixes.Dock = System.Windows.Forms.DockStyle.Top;
            tlpAffixes.Location = new System.Drawing.Point(0, 0);
            tlpAffixes.Name = "tlpAffixes";
            tlpAffixes.RowCount = 1;
            tlpAffixes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tlpAffixes.Size = new System.Drawing.Size(732, 2);
            tlpAffixes.TabIndex = 1;
            // 
            // AffixTab
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(panel1);
            Controls.Add(btnRemoveAffix);
            Controls.Add(btnAddAffix);
            Name = "AffixTab";
            Size = new System.Drawing.Size(732, 363);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Button btnRemoveAffix;
        private System.Windows.Forms.Button btnAddAffix;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tlpAffixes;
    }
}
