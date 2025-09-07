namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    partial class ScriptsTab
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
            lblNoScripts = new System.Windows.Forms.Label();
            tlpScriptsTab = new System.Windows.Forms.TableLayoutPanel();
            tlpHeader = new System.Windows.Forms.TableLayoutPanel();
            label3 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            pnlTable = new System.Windows.Forms.Panel();
            tlpScripts = new System.Windows.Forms.TableLayoutPanel();
            btnAddScript = new System.Windows.Forms.Button();
            btnRemoveScript = new System.Windows.Forms.Button();
            tlpScriptsTab.SuspendLayout();
            tlpHeader.SuspendLayout();
            pnlTable.SuspendLayout();
            SuspendLayout();
            // 
            // lblNoScripts
            // 
            lblNoScripts.AutoSize = true;
            lblNoScripts.Location = new System.Drawing.Point(287, 174);
            lblNoScripts.Name = "lblNoScripts";
            lblNoScripts.Size = new System.Drawing.Size(108, 15);
            lblNoScripts.TabIndex = 0;
            lblNoScripts.Text = "No scripts to show.";
            lblNoScripts.Visible = false;
            // 
            // tlpScriptsTab
            // 
            tlpScriptsTab.ColumnCount = 1;
            tlpScriptsTab.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tlpScriptsTab.Controls.Add(tlpHeader, 0, 0);
            tlpScriptsTab.Controls.Add(pnlTable, 0, 1);
            tlpScriptsTab.Location = new System.Drawing.Point(0, 0);
            tlpScriptsTab.Margin = new System.Windows.Forms.Padding(0);
            tlpScriptsTab.Name = "tlpScriptsTab";
            tlpScriptsTab.RowCount = 2;
            tlpScriptsTab.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            tlpScriptsTab.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            tlpScriptsTab.Size = new System.Drawing.Size(728, 316);
            tlpScriptsTab.TabIndex = 1;
            // 
            // tlpHeader
            // 
            tlpHeader.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            tlpHeader.ColumnCount = 2;
            tlpHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 340F));
            tlpHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 340F));
            tlpHeader.Controls.Add(label3, 1, 0);
            tlpHeader.Controls.Add(label1, 0, 0);
            tlpHeader.Location = new System.Drawing.Point(0, 0);
            tlpHeader.Margin = new System.Windows.Forms.Padding(0);
            tlpHeader.Name = "tlpHeader";
            tlpHeader.RowCount = 1;
            tlpHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tlpHeader.Size = new System.Drawing.Size(700, 31);
            tlpHeader.TabIndex = 0;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = System.Windows.Forms.DockStyle.Fill;
            label3.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            label3.Location = new System.Drawing.Point(345, 1);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(351, 29);
            label3.TabIndex = 2;
            label3.Text = "When called...";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = System.Windows.Forms.DockStyle.Fill;
            label1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            label1.Location = new System.Drawing.Point(4, 1);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(334, 29);
            label1.TabIndex = 0;
            label1.Text = "Script Id";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlTable
            // 
            pnlTable.AutoScroll = true;
            pnlTable.Controls.Add(tlpScripts);
            pnlTable.Location = new System.Drawing.Point(0, 31);
            pnlTable.Margin = new System.Windows.Forms.Padding(0);
            pnlTable.Name = "pnlTable";
            pnlTable.Size = new System.Drawing.Size(718, 285);
            pnlTable.TabIndex = 1;
            // 
            // tlpScripts
            // 
            tlpScripts.AutoSize = true;
            tlpScripts.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            tlpScripts.ColumnCount = 2;
            tlpScripts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 340F));
            tlpScripts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 357F));
            tlpScripts.Location = new System.Drawing.Point(0, 0);
            tlpScripts.Margin = new System.Windows.Forms.Padding(0);
            tlpScripts.Name = "tlpScripts";
            tlpScripts.RowCount = 1;
            tlpScripts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            tlpScripts.Size = new System.Drawing.Size(700, 34);
            tlpScripts.TabIndex = 2;
            // 
            // btnAddScript
            // 
            btnAddScript.Location = new System.Drawing.Point(224, 319);
            btnAddScript.Name = "btnAddScript";
            btnAddScript.Size = new System.Drawing.Size(115, 23);
            btnAddScript.TabIndex = 2;
            btnAddScript.Text = "Add a Script";
            btnAddScript.UseVisualStyleBackColor = true;
            btnAddScript.Click += btnAddScript_Click;
            // 
            // btnRemoveScript
            // 
            btnRemoveScript.Location = new System.Drawing.Point(345, 319);
            btnRemoveScript.Name = "btnRemoveScript";
            btnRemoveScript.Size = new System.Drawing.Size(115, 23);
            btnRemoveScript.TabIndex = 3;
            btnRemoveScript.Text = "Remove Script";
            btnRemoveScript.UseVisualStyleBackColor = true;
            btnRemoveScript.Click += btnRemoveScript_Click;
            // 
            // ScriptsTab
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(btnRemoveScript);
            Controls.Add(btnAddScript);
            Controls.Add(tlpScriptsTab);
            Controls.Add(lblNoScripts);
            Name = "ScriptsTab";
            Size = new System.Drawing.Size(728, 347);
            tlpScriptsTab.ResumeLayout(false);
            tlpHeader.ResumeLayout(false);
            tlpHeader.PerformLayout();
            pnlTable.ResumeLayout(false);
            pnlTable.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblNoScripts;
        private System.Windows.Forms.TableLayoutPanel tlpScriptsTab;
        private System.Windows.Forms.Button btnAddScript;
        private System.Windows.Forms.TableLayoutPanel tlpHeader;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRemoveScript;
        private System.Windows.Forms.Panel pnlTable;
        private System.Windows.Forms.TableLayoutPanel tlpScripts;
    }
}
