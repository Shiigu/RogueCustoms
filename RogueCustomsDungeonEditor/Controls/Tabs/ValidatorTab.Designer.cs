namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    partial class ValidatorTab
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
            tvValidationResults = new System.Windows.Forms.TreeView();
            SuspendLayout();
            // 
            // tvValidationResults
            // 
            tvValidationResults.Location = new System.Drawing.Point(1, 1);
            tvValidationResults.Name = "tvValidationResults";
            tvValidationResults.Size = new System.Drawing.Size(738, 354);
            tvValidationResults.TabIndex = 1;
            // 
            // ValidatorTab
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Controls.Add(tvValidationResults);
            Name = "ValidatorTab";
            Size = new System.Drawing.Size(740, 356);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TreeView tvValidationResults;
    }
}
