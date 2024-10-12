namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    partial class TilesetTab
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
            flpTileTypeSets = new System.Windows.Forms.FlowLayoutPanel();
            SuspendLayout();
            // 
            // flpTileTypeSets
            // 
            flpTileTypeSets.AutoScroll = true;
            flpTileTypeSets.AutoSize = true;
            flpTileTypeSets.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            flpTileTypeSets.Location = new System.Drawing.Point(0, 0);
            flpTileTypeSets.Margin = new System.Windows.Forms.Padding(0);
            flpTileTypeSets.Name = "flpTileTypeSets";
            flpTileTypeSets.Size = new System.Drawing.Size(676, 692);
            flpTileTypeSets.TabIndex = 79;
            flpTileTypeSets.WrapContents = false;
            // 
            // TilesetTab
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Controls.Add(flpTileTypeSets);
            Name = "TilesetTab";
            Size = new System.Drawing.Size(676, 692);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpTileTypeSets;
    }
}
