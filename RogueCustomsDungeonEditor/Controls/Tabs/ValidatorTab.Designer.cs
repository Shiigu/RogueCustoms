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
            ssValidatorStatus = new System.Windows.Forms.StatusStrip();
            tsslValidationProgress = new System.Windows.Forms.ToolStripStatusLabel();
            tspbValidationProgress = new System.Windows.Forms.ToolStripProgressBar();
            ssValidatorStatus.SuspendLayout();
            SuspendLayout();
            // 
            // tvValidationResults
            // 
            tvValidationResults.Location = new System.Drawing.Point(1, 1);
            tvValidationResults.Name = "tvValidationResults";
            tvValidationResults.Size = new System.Drawing.Size(738, 332);
            tvValidationResults.TabIndex = 1;
            // 
            // ssValidatorStatus
            // 
            ssValidatorStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsslValidationProgress, tspbValidationProgress });
            ssValidatorStatus.Location = new System.Drawing.Point(0, 334);
            ssValidatorStatus.Name = "ssValidatorStatus";
            ssValidatorStatus.Size = new System.Drawing.Size(740, 22);
            ssValidatorStatus.SizingGrip = false;
            ssValidatorStatus.TabIndex = 2;
            // 
            // tsslValidationProgress
            // 
            tsslValidationProgress.Name = "tsslValidationProgress";
            tsslValidationProgress.Size = new System.Drawing.Size(16, 17);
            tsslValidationProgress.Text = "...";
            tsslValidationProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tspbValidationProgress
            // 
            tspbValidationProgress.Name = "tspbValidationProgress";
            tspbValidationProgress.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            tspbValidationProgress.Size = new System.Drawing.Size(720, 16);
            tspbValidationProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // ValidatorTab
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Controls.Add(ssValidatorStatus);
            Controls.Add(tvValidationResults);
            Name = "ValidatorTab";
            Size = new System.Drawing.Size(740, 356);
            ssValidatorStatus.ResumeLayout(false);
            ssValidatorStatus.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TreeView tvValidationResults;
        private System.Windows.Forms.StatusStrip ssValidatorStatus;
        private System.Windows.Forms.ToolStripStatusLabel tsslValidationProgress;
        private System.Windows.Forms.ToolStripProgressBar tspbValidationProgress;
    }
}
