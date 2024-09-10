namespace RogueCustomsDungeonEditor.Controls
{
    partial class RoomDispositionTile
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
            components = new System.ComponentModel.Container();
            pcbTile = new System.Windows.Forms.PictureBox();
            cmsRoomTypes = new System.Windows.Forms.ContextMenuStrip(components);
            ttTile = new System.Windows.Forms.ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)pcbTile).BeginInit();
            SuspendLayout();
            // 
            // pcbTile
            // 
            pcbTile.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pcbTile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pcbTile.Location = new System.Drawing.Point(0, 0);
            pcbTile.Margin = new System.Windows.Forms.Padding(0);
            pcbTile.MaximumSize = new System.Drawing.Size(24, 24);
            pcbTile.MinimumSize = new System.Drawing.Size(24, 24);
            pcbTile.Name = "pcbTile";
            pcbTile.Size = new System.Drawing.Size(24, 24);
            pcbTile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pcbTile.TabIndex = 0;
            pcbTile.TabStop = false;
            pcbTile.Click += pcbTile_Click;
            pcbTile.MouseHover += pcbTile_MouseHover;
            // 
            // cmsRoomTypes
            // 
            cmsRoomTypes.Name = "cmsRoomTypes";
            cmsRoomTypes.Size = new System.Drawing.Size(61, 4);
            // 
            // RoomDispositionTile
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(pcbTile);
            Margin = new System.Windows.Forms.Padding(0);
            MaximumSize = new System.Drawing.Size(24, 24);
            MinimumSize = new System.Drawing.Size(24, 24);
            Name = "RoomDispositionTile";
            Size = new System.Drawing.Size(24, 24);
            ((System.ComponentModel.ISupportInitialize)pcbTile).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.PictureBox pcbTile;
        private System.Windows.Forms.ContextMenuStrip cmsRoomTypes;
        private System.Windows.Forms.ToolTip ttTile;
    }
}
