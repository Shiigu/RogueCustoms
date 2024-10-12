namespace RogueCustomsDungeonEditor.Controls
{
    partial class TileTypeSetEditor
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(TileTypeSetEditor));
            lblHorizontalHeader = new System.Windows.Forms.Label();
            csrHorizontal = new ConsoleRepresentationSelector();
            lblBottomRightHeader = new System.Windows.Forms.Label();
            csrBottomRight = new ConsoleRepresentationSelector();
            lblBottomLeftHeader = new System.Windows.Forms.Label();
            csrBottomLeft = new ConsoleRepresentationSelector();
            lblCentralHeader = new System.Windows.Forms.Label();
            csrCentral = new ConsoleRepresentationSelector();
            lblVerticalRightHeader = new System.Windows.Forms.Label();
            csrVerticalRight = new ConsoleRepresentationSelector();
            lblVerticalLeftHeader = new System.Windows.Forms.Label();
            csrVerticalLeft = new ConsoleRepresentationSelector();
            lblHorizontalTopHeader = new System.Windows.Forms.Label();
            csrHorizontalTop = new ConsoleRepresentationSelector();
            lblHorizontalBottomHeader = new System.Windows.Forms.Label();
            csrHorizontalBottom = new ConsoleRepresentationSelector();
            lblVerticalHeader = new System.Windows.Forms.Label();
            csrVertical = new ConsoleRepresentationSelector();
            lblTopRightHeader = new System.Windows.Forms.Label();
            csrTopRight = new ConsoleRepresentationSelector();
            lblName = new System.Windows.Forms.Label();
            lblTopLeftHeader = new System.Windows.Forms.Label();
            csrTopLeft = new ConsoleRepresentationSelector();
            lblConnectorHeader = new System.Windows.Forms.Label();
            csrConnector = new ConsoleRepresentationSelector();
            SuspendLayout();
            // 
            // lblHorizontalHeader
            // 
            lblHorizontalHeader.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblHorizontalHeader.Location = new System.Drawing.Point(436, 303);
            lblHorizontalHeader.Name = "lblHorizontalHeader";
            lblHorizontalHeader.Size = new System.Drawing.Size(211, 32);
            lblHorizontalHeader.TabIndex = 230;
            lblHorizontalHeader.Text = "Horizontal";
            lblHorizontalHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrHorizontal
            // 
            csrHorizontal.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrHorizontal.BackgroundColor");
            csrHorizontal.Character = '\0';
            csrHorizontal.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrHorizontal.ForegroundColor");
            csrHorizontal.Location = new System.Drawing.Point(436, 338);
            csrHorizontal.Name = "csrHorizontal";
            csrHorizontal.Size = new System.Drawing.Size(211, 83);
            csrHorizontal.TabIndex = 231;
            csrHorizontal.PropertyChanged += csrHorizontal_PropertyChanged;
            // 
            // lblBottomRightHeader
            // 
            lblBottomRightHeader.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblBottomRightHeader.Location = new System.Drawing.Point(219, 303);
            lblBottomRightHeader.Name = "lblBottomRightHeader";
            lblBottomRightHeader.Size = new System.Drawing.Size(211, 32);
            lblBottomRightHeader.TabIndex = 228;
            lblBottomRightHeader.Text = "Bottom Right Corner";
            lblBottomRightHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrBottomRight
            // 
            csrBottomRight.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrBottomRight.BackgroundColor");
            csrBottomRight.Character = '\0';
            csrBottomRight.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrBottomRight.ForegroundColor");
            csrBottomRight.Location = new System.Drawing.Point(219, 338);
            csrBottomRight.Name = "csrBottomRight";
            csrBottomRight.Size = new System.Drawing.Size(211, 83);
            csrBottomRight.TabIndex = 229;
            csrBottomRight.PropertyChanged += csrBottomRight_PropertyChanged;
            // 
            // lblBottomLeftHeader
            // 
            lblBottomLeftHeader.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblBottomLeftHeader.Location = new System.Drawing.Point(2, 303);
            lblBottomLeftHeader.Name = "lblBottomLeftHeader";
            lblBottomLeftHeader.Size = new System.Drawing.Size(211, 32);
            lblBottomLeftHeader.TabIndex = 226;
            lblBottomLeftHeader.Text = "Bottom Left Corner";
            lblBottomLeftHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrBottomLeft
            // 
            csrBottomLeft.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrBottomLeft.BackgroundColor");
            csrBottomLeft.Character = '\0';
            csrBottomLeft.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrBottomLeft.ForegroundColor");
            csrBottomLeft.Location = new System.Drawing.Point(2, 338);
            csrBottomLeft.Name = "csrBottomLeft";
            csrBottomLeft.Size = new System.Drawing.Size(211, 83);
            csrBottomLeft.TabIndex = 227;
            csrBottomLeft.PropertyChanged += csrBottomLeft_PropertyChanged;
            // 
            // lblCentralHeader
            // 
            lblCentralHeader.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblCentralHeader.Location = new System.Drawing.Point(129, 54);
            lblCentralHeader.Name = "lblCentralHeader";
            lblCentralHeader.Size = new System.Drawing.Size(211, 32);
            lblCentralHeader.TabIndex = 224;
            lblCentralHeader.Text = "Central/Default";
            lblCentralHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrCentral
            // 
            csrCentral.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrCentral.BackgroundColor");
            csrCentral.Character = '\0';
            csrCentral.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrCentral.ForegroundColor");
            csrCentral.Location = new System.Drawing.Point(129, 89);
            csrCentral.Name = "csrCentral";
            csrCentral.Size = new System.Drawing.Size(211, 83);
            csrCentral.TabIndex = 225;
            csrCentral.PropertyChanged += csrCentral_PropertyChanged;
            // 
            // lblVerticalRightHeader
            // 
            lblVerticalRightHeader.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblVerticalRightHeader.Location = new System.Drawing.Point(129, 556);
            lblVerticalRightHeader.Name = "lblVerticalRightHeader";
            lblVerticalRightHeader.Size = new System.Drawing.Size(211, 32);
            lblVerticalRightHeader.TabIndex = 222;
            lblVerticalRightHeader.Text = "Vertical-Right";
            lblVerticalRightHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrVerticalRight
            // 
            csrVerticalRight.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrVerticalRight.BackgroundColor");
            csrVerticalRight.Character = '\0';
            csrVerticalRight.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrVerticalRight.ForegroundColor");
            csrVerticalRight.Location = new System.Drawing.Point(129, 591);
            csrVerticalRight.Name = "csrVerticalRight";
            csrVerticalRight.Size = new System.Drawing.Size(211, 83);
            csrVerticalRight.TabIndex = 223;
            csrVerticalRight.PropertyChanged += csrVerticalRight_PropertyChanged;
            // 
            // lblVerticalLeftHeader
            // 
            lblVerticalLeftHeader.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblVerticalLeftHeader.Location = new System.Drawing.Point(346, 556);
            lblVerticalLeftHeader.Name = "lblVerticalLeftHeader";
            lblVerticalLeftHeader.Size = new System.Drawing.Size(211, 32);
            lblVerticalLeftHeader.TabIndex = 220;
            lblVerticalLeftHeader.Text = "Vertical-Left";
            lblVerticalLeftHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrVerticalLeft
            // 
            csrVerticalLeft.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrVerticalLeft.BackgroundColor");
            csrVerticalLeft.Character = '\0';
            csrVerticalLeft.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrVerticalLeft.ForegroundColor");
            csrVerticalLeft.Location = new System.Drawing.Point(346, 591);
            csrVerticalLeft.Name = "csrVerticalLeft";
            csrVerticalLeft.Size = new System.Drawing.Size(211, 83);
            csrVerticalLeft.TabIndex = 221;
            csrVerticalLeft.PropertyChanged += csrVerticalLeft_PropertyChanged;
            // 
            // lblHorizontalTopHeader
            // 
            lblHorizontalTopHeader.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblHorizontalTopHeader.Location = new System.Drawing.Point(346, 435);
            lblHorizontalTopHeader.Name = "lblHorizontalTopHeader";
            lblHorizontalTopHeader.Size = new System.Drawing.Size(211, 32);
            lblHorizontalTopHeader.TabIndex = 218;
            lblHorizontalTopHeader.Text = "Horizontal-Top";
            lblHorizontalTopHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrHorizontalTop
            // 
            csrHorizontalTop.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrHorizontalTop.BackgroundColor");
            csrHorizontalTop.Character = '\0';
            csrHorizontalTop.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrHorizontalTop.ForegroundColor");
            csrHorizontalTop.Location = new System.Drawing.Point(346, 470);
            csrHorizontalTop.Name = "csrHorizontalTop";
            csrHorizontalTop.Size = new System.Drawing.Size(211, 83);
            csrHorizontalTop.TabIndex = 219;
            csrHorizontalTop.PropertyChanged += csrHorizontalTop_PropertyChanged;
            // 
            // lblHorizontalBottomHeader
            // 
            lblHorizontalBottomHeader.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblHorizontalBottomHeader.Location = new System.Drawing.Point(129, 435);
            lblHorizontalBottomHeader.Name = "lblHorizontalBottomHeader";
            lblHorizontalBottomHeader.Size = new System.Drawing.Size(211, 32);
            lblHorizontalBottomHeader.TabIndex = 216;
            lblHorizontalBottomHeader.Text = "Horizontal-Bottom";
            lblHorizontalBottomHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrHorizontalBottom
            // 
            csrHorizontalBottom.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrHorizontalBottom.BackgroundColor");
            csrHorizontalBottom.Character = '\0';
            csrHorizontalBottom.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrHorizontalBottom.ForegroundColor");
            csrHorizontalBottom.Location = new System.Drawing.Point(129, 470);
            csrHorizontalBottom.Name = "csrHorizontalBottom";
            csrHorizontalBottom.Size = new System.Drawing.Size(211, 83);
            csrHorizontalBottom.TabIndex = 217;
            csrHorizontalBottom.PropertyChanged += csrHorizontalBottom_PropertyChanged;
            // 
            // lblVerticalHeader
            // 
            lblVerticalHeader.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblVerticalHeader.Location = new System.Drawing.Point(436, 182);
            lblVerticalHeader.Name = "lblVerticalHeader";
            lblVerticalHeader.Size = new System.Drawing.Size(211, 32);
            lblVerticalHeader.TabIndex = 214;
            lblVerticalHeader.Text = "Vertical";
            lblVerticalHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrVertical
            // 
            csrVertical.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrVertical.BackgroundColor");
            csrVertical.Character = '\0';
            csrVertical.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrVertical.ForegroundColor");
            csrVertical.Location = new System.Drawing.Point(436, 217);
            csrVertical.Name = "csrVertical";
            csrVertical.Size = new System.Drawing.Size(211, 83);
            csrVertical.TabIndex = 215;
            csrVertical.PropertyChanged += csrVertical_PropertyChanged;
            // 
            // lblTopRightHeader
            // 
            lblTopRightHeader.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblTopRightHeader.Location = new System.Drawing.Point(219, 182);
            lblTopRightHeader.Name = "lblTopRightHeader";
            lblTopRightHeader.Size = new System.Drawing.Size(211, 32);
            lblTopRightHeader.TabIndex = 212;
            lblTopRightHeader.Text = "Top Right Corner";
            lblTopRightHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrTopRight
            // 
            csrTopRight.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrTopRight.BackgroundColor");
            csrTopRight.Character = '\0';
            csrTopRight.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrTopRight.ForegroundColor");
            csrTopRight.Location = new System.Drawing.Point(219, 217);
            csrTopRight.Name = "csrTopRight";
            csrTopRight.Size = new System.Drawing.Size(211, 83);
            csrTopRight.TabIndex = 213;
            csrTopRight.PropertyChanged += csrTopRight_PropertyChanged;
            // 
            // lblName
            // 
            lblName.Font = new System.Drawing.Font("Segoe UI", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblName.Location = new System.Drawing.Point(2, 2);
            lblName.Name = "lblName";
            lblName.Size = new System.Drawing.Size(645, 52);
            lblName.TabIndex = 211;
            lblName.Text = "NAME";
            lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTopLeftHeader
            // 
            lblTopLeftHeader.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblTopLeftHeader.Location = new System.Drawing.Point(2, 182);
            lblTopLeftHeader.Name = "lblTopLeftHeader";
            lblTopLeftHeader.Size = new System.Drawing.Size(211, 32);
            lblTopLeftHeader.TabIndex = 209;
            lblTopLeftHeader.Text = "Top Left Corner";
            lblTopLeftHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrTopLeft
            // 
            csrTopLeft.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrTopLeft.BackgroundColor");
            csrTopLeft.Character = '\0';
            csrTopLeft.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrTopLeft.ForegroundColor");
            csrTopLeft.Location = new System.Drawing.Point(2, 217);
            csrTopLeft.Name = "csrTopLeft";
            csrTopLeft.Size = new System.Drawing.Size(211, 83);
            csrTopLeft.TabIndex = 210;
            csrTopLeft.PropertyChanged += csrTopLeft_PropertyChanged;
            // 
            // lblConnectorHeader
            // 
            lblConnectorHeader.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblConnectorHeader.Location = new System.Drawing.Point(346, 54);
            lblConnectorHeader.Name = "lblConnectorHeader";
            lblConnectorHeader.Size = new System.Drawing.Size(211, 32);
            lblConnectorHeader.TabIndex = 232;
            lblConnectorHeader.Text = "Connector";
            lblConnectorHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrConnector
            // 
            csrConnector.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrConnector.BackgroundColor");
            csrConnector.Character = '\0';
            csrConnector.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrConnector.ForegroundColor");
            csrConnector.Location = new System.Drawing.Point(346, 89);
            csrConnector.Name = "csrConnector";
            csrConnector.Size = new System.Drawing.Size(211, 83);
            csrConnector.TabIndex = 233;
            csrConnector.PropertyChanged += csrConnector_PropertyChanged;
            // 
            // TileTypeSetEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Controls.Add(lblConnectorHeader);
            Controls.Add(csrConnector);
            Controls.Add(lblHorizontalHeader);
            Controls.Add(csrHorizontal);
            Controls.Add(lblBottomRightHeader);
            Controls.Add(csrBottomRight);
            Controls.Add(lblBottomLeftHeader);
            Controls.Add(csrBottomLeft);
            Controls.Add(lblCentralHeader);
            Controls.Add(csrCentral);
            Controls.Add(lblVerticalRightHeader);
            Controls.Add(csrVerticalRight);
            Controls.Add(lblVerticalLeftHeader);
            Controls.Add(csrVerticalLeft);
            Controls.Add(lblHorizontalTopHeader);
            Controls.Add(csrHorizontalTop);
            Controls.Add(lblHorizontalBottomHeader);
            Controls.Add(csrHorizontalBottom);
            Controls.Add(lblVerticalHeader);
            Controls.Add(csrVertical);
            Controls.Add(lblTopRightHeader);
            Controls.Add(csrTopRight);
            Controls.Add(lblName);
            Controls.Add(lblTopLeftHeader);
            Controls.Add(csrTopLeft);
            Name = "TileTypeSetEditor";
            Size = new System.Drawing.Size(650, 677);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblHorizontalHeader;
        private ConsoleRepresentationSelector csrHorizontal;
        private System.Windows.Forms.Label lblBottomRightHeader;
        private ConsoleRepresentationSelector csrBottomRight;
        private System.Windows.Forms.Label lblBottomLeftHeader;
        private ConsoleRepresentationSelector csrBottomLeft;
        private System.Windows.Forms.Label lblCentralHeader;
        private ConsoleRepresentationSelector csrCentral;
        private System.Windows.Forms.Label lblVerticalRightHeader;
        private ConsoleRepresentationSelector csrVerticalRight;
        private System.Windows.Forms.Label lblVerticalLeftHeader;
        private ConsoleRepresentationSelector csrVerticalLeft;
        private System.Windows.Forms.Label lblHorizontalTopHeader;
        private ConsoleRepresentationSelector csrHorizontalTop;
        private System.Windows.Forms.Label lblHorizontalBottomHeader;
        private ConsoleRepresentationSelector csrHorizontalBottom;
        private System.Windows.Forms.Label lblVerticalHeader;
        private ConsoleRepresentationSelector csrVertical;
        private System.Windows.Forms.Label lblTopRightHeader;
        private ConsoleRepresentationSelector csrTopRight;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblTopLeftHeader;
        private ConsoleRepresentationSelector csrTopLeft;
        private System.Windows.Forms.Label lblConnectorHeader;
        private ConsoleRepresentationSelector csrConnector;
    }
}
