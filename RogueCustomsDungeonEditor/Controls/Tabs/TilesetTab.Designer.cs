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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(TilesetTab));
            label151 = new System.Windows.Forms.Label();
            csrEmpty = new ConsoleRepresentationSelector();
            label152 = new System.Windows.Forms.Label();
            csrStairs = new ConsoleRepresentationSelector();
            label153 = new System.Windows.Forms.Label();
            label154 = new System.Windows.Forms.Label();
            csrFloor = new ConsoleRepresentationSelector();
            label148 = new System.Windows.Forms.Label();
            csrHorizontalHallway = new ConsoleRepresentationSelector();
            label149 = new System.Windows.Forms.Label();
            csrBottomRightHallway = new ConsoleRepresentationSelector();
            label150 = new System.Windows.Forms.Label();
            csrBottomLeftHallway = new ConsoleRepresentationSelector();
            label137 = new System.Windows.Forms.Label();
            csrCentralHallway = new ConsoleRepresentationSelector();
            label140 = new System.Windows.Forms.Label();
            csrVerticalRightHallway = new ConsoleRepresentationSelector();
            label141 = new System.Windows.Forms.Label();
            csrVerticalLeftHallway = new ConsoleRepresentationSelector();
            label142 = new System.Windows.Forms.Label();
            csrHorizontalTopHallway = new ConsoleRepresentationSelector();
            label143 = new System.Windows.Forms.Label();
            csrHorizontalBottomHallway = new ConsoleRepresentationSelector();
            label144 = new System.Windows.Forms.Label();
            csrVerticalHallway = new ConsoleRepresentationSelector();
            label145 = new System.Windows.Forms.Label();
            csrTopRightHallway = new ConsoleRepresentationSelector();
            label146 = new System.Windows.Forms.Label();
            label147 = new System.Windows.Forms.Label();
            csrTopLeftHallway = new ConsoleRepresentationSelector();
            label138 = new System.Windows.Forms.Label();
            csrConnectorWall = new ConsoleRepresentationSelector();
            label134 = new System.Windows.Forms.Label();
            csrHorizontalWall = new ConsoleRepresentationSelector();
            label135 = new System.Windows.Forms.Label();
            csrBottomRightWall = new ConsoleRepresentationSelector();
            label136 = new System.Windows.Forms.Label();
            csrBottomLeftWall = new ConsoleRepresentationSelector();
            label133 = new System.Windows.Forms.Label();
            csrVerticalWall = new ConsoleRepresentationSelector();
            label132 = new System.Windows.Forms.Label();
            csrTopRightWall = new ConsoleRepresentationSelector();
            label131 = new System.Windows.Forms.Label();
            label130 = new System.Windows.Forms.Label();
            csrTopLeftWall = new ConsoleRepresentationSelector();
            SuspendLayout();
            // 
            // label151
            // 
            label151.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label151.Location = new System.Drawing.Point(457, 1052);
            label151.Name = "label151";
            label151.Size = new System.Drawing.Size(211, 32);
            label151.TabIndex = 214;
            label151.Text = "Empty (inaccessible)";
            label151.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrEmpty
            // 
            csrEmpty.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrEmpty.BackgroundColor");
            csrEmpty.Character = '\0';
            csrEmpty.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrEmpty.ForegroundColor");
            csrEmpty.Location = new System.Drawing.Point(457, 1087);
            csrEmpty.Name = "csrEmpty";
            csrEmpty.Size = new System.Drawing.Size(211, 83);
            csrEmpty.TabIndex = 215;
            csrEmpty.PropertyChanged += csrEmpty_PropertyChanged;
            // 
            // label152
            // 
            label152.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label152.Location = new System.Drawing.Point(240, 1052);
            label152.Name = "label152";
            label152.Size = new System.Drawing.Size(211, 32);
            label152.TabIndex = 212;
            label152.Text = "Stairs";
            label152.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrStairs
            // 
            csrStairs.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrStairs.BackgroundColor");
            csrStairs.Character = '\0';
            csrStairs.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrStairs.ForegroundColor");
            csrStairs.Location = new System.Drawing.Point(240, 1087);
            csrStairs.Name = "csrStairs";
            csrStairs.Size = new System.Drawing.Size(211, 83);
            csrStairs.TabIndex = 213;
            csrStairs.PropertyChanged += csrStairs_PropertyChanged;
            // 
            // label153
            // 
            label153.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label153.Location = new System.Drawing.Point(240, 1000);
            label153.Name = "label153";
            label153.Size = new System.Drawing.Size(211, 52);
            label153.TabIndex = 211;
            label153.Text = "OTHERS";
            label153.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label154
            // 
            label154.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label154.Location = new System.Drawing.Point(23, 1052);
            label154.Name = "label154";
            label154.Size = new System.Drawing.Size(211, 32);
            label154.TabIndex = 209;
            label154.Text = "Unoccupied Floor";
            label154.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrFloor
            // 
            csrFloor.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrFloor.BackgroundColor");
            csrFloor.Character = '\0';
            csrFloor.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrFloor.ForegroundColor");
            csrFloor.Location = new System.Drawing.Point(23, 1087);
            csrFloor.Name = "csrFloor";
            csrFloor.Size = new System.Drawing.Size(211, 83);
            csrFloor.TabIndex = 210;
            csrFloor.PropertyChanged += csrFloor_PropertyChanged;
            // 
            // label148
            // 
            label148.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label148.Location = new System.Drawing.Point(446, 608);
            label148.Name = "label148";
            label148.Size = new System.Drawing.Size(211, 32);
            label148.TabIndex = 207;
            label148.Text = "Horizontal";
            label148.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrHorizontalHallway
            // 
            csrHorizontalHallway.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrHorizontalHallway.BackgroundColor");
            csrHorizontalHallway.Character = '\0';
            csrHorizontalHallway.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrHorizontalHallway.ForegroundColor");
            csrHorizontalHallway.Location = new System.Drawing.Point(446, 643);
            csrHorizontalHallway.Name = "csrHorizontalHallway";
            csrHorizontalHallway.Size = new System.Drawing.Size(211, 83);
            csrHorizontalHallway.TabIndex = 208;
            csrHorizontalHallway.PropertyChanged += csrHorizontalHallway_PropertyChanged;
            // 
            // label149
            // 
            label149.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label149.Location = new System.Drawing.Point(229, 608);
            label149.Name = "label149";
            label149.Size = new System.Drawing.Size(211, 32);
            label149.TabIndex = 205;
            label149.Text = "Bottom Right Corner";
            label149.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrBottomRightHallway
            // 
            csrBottomRightHallway.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrBottomRightHallway.BackgroundColor");
            csrBottomRightHallway.Character = '\0';
            csrBottomRightHallway.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrBottomRightHallway.ForegroundColor");
            csrBottomRightHallway.Location = new System.Drawing.Point(229, 643);
            csrBottomRightHallway.Name = "csrBottomRightHallway";
            csrBottomRightHallway.Size = new System.Drawing.Size(211, 83);
            csrBottomRightHallway.TabIndex = 206;
            csrBottomRightHallway.PropertyChanged += csrBottomRightHallway_PropertyChanged;
            // 
            // label150
            // 
            label150.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label150.Location = new System.Drawing.Point(12, 608);
            label150.Name = "label150";
            label150.Size = new System.Drawing.Size(211, 32);
            label150.TabIndex = 203;
            label150.Text = "Bottom Left Corner";
            label150.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrBottomLeftHallway
            // 
            csrBottomLeftHallway.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrBottomLeftHallway.BackgroundColor");
            csrBottomLeftHallway.Character = '\0';
            csrBottomLeftHallway.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrBottomLeftHallway.ForegroundColor");
            csrBottomLeftHallway.Location = new System.Drawing.Point(12, 643);
            csrBottomLeftHallway.Name = "csrBottomLeftHallway";
            csrBottomLeftHallway.Size = new System.Drawing.Size(211, 83);
            csrBottomLeftHallway.TabIndex = 204;
            csrBottomLeftHallway.PropertyChanged += csrBottomLeftHallway_PropertyChanged;
            // 
            // label137
            // 
            label137.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label137.Location = new System.Drawing.Point(316, 859);
            label137.Name = "label137";
            label137.Size = new System.Drawing.Size(211, 32);
            label137.TabIndex = 201;
            label137.Text = "Central";
            label137.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrCentralHallway
            // 
            csrCentralHallway.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrCentralHallway.BackgroundColor");
            csrCentralHallway.Character = '\0';
            csrCentralHallway.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrCentralHallway.ForegroundColor");
            csrCentralHallway.Location = new System.Drawing.Point(316, 894);
            csrCentralHallway.Name = "csrCentralHallway";
            csrCentralHallway.Size = new System.Drawing.Size(211, 83);
            csrCentralHallway.TabIndex = 202;
            csrCentralHallway.PropertyChanged += csrCentralHallway_PropertyChanged;
            // 
            // label140
            // 
            label140.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label140.Location = new System.Drawing.Point(99, 859);
            label140.Name = "label140";
            label140.Size = new System.Drawing.Size(211, 32);
            label140.TabIndex = 199;
            label140.Text = "Vertical-Right";
            label140.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrVerticalRightHallway
            // 
            csrVerticalRightHallway.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrVerticalRightHallway.BackgroundColor");
            csrVerticalRightHallway.Character = '\0';
            csrVerticalRightHallway.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrVerticalRightHallway.ForegroundColor");
            csrVerticalRightHallway.Location = new System.Drawing.Point(99, 894);
            csrVerticalRightHallway.Name = "csrVerticalRightHallway";
            csrVerticalRightHallway.Size = new System.Drawing.Size(211, 83);
            csrVerticalRightHallway.TabIndex = 200;
            csrVerticalRightHallway.PropertyChanged += csrVerticalRightHallway_PropertyChanged;
            // 
            // label141
            // 
            label141.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label141.Location = new System.Drawing.Point(446, 738);
            label141.Name = "label141";
            label141.Size = new System.Drawing.Size(211, 32);
            label141.TabIndex = 197;
            label141.Text = "Vertical-Left";
            label141.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrVerticalLeftHallway
            // 
            csrVerticalLeftHallway.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrVerticalLeftHallway.BackgroundColor");
            csrVerticalLeftHallway.Character = '\0';
            csrVerticalLeftHallway.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrVerticalLeftHallway.ForegroundColor");
            csrVerticalLeftHallway.Location = new System.Drawing.Point(446, 773);
            csrVerticalLeftHallway.Name = "csrVerticalLeftHallway";
            csrVerticalLeftHallway.Size = new System.Drawing.Size(211, 83);
            csrVerticalLeftHallway.TabIndex = 198;
            csrVerticalLeftHallway.PropertyChanged += csrVerticalLeftHallway_PropertyChanged;
            // 
            // label142
            // 
            label142.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label142.Location = new System.Drawing.Point(229, 738);
            label142.Name = "label142";
            label142.Size = new System.Drawing.Size(211, 32);
            label142.TabIndex = 195;
            label142.Text = "Horizontal-Top";
            label142.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrHorizontalTopHallway
            // 
            csrHorizontalTopHallway.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrHorizontalTopHallway.BackgroundColor");
            csrHorizontalTopHallway.Character = '\0';
            csrHorizontalTopHallway.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrHorizontalTopHallway.ForegroundColor");
            csrHorizontalTopHallway.Location = new System.Drawing.Point(229, 773);
            csrHorizontalTopHallway.Name = "csrHorizontalTopHallway";
            csrHorizontalTopHallway.Size = new System.Drawing.Size(211, 83);
            csrHorizontalTopHallway.TabIndex = 196;
            csrHorizontalTopHallway.PropertyChanged += csrHorizontalTopHallway_PropertyChanged;
            // 
            // label143
            // 
            label143.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label143.Location = new System.Drawing.Point(12, 738);
            label143.Name = "label143";
            label143.Size = new System.Drawing.Size(211, 32);
            label143.TabIndex = 193;
            label143.Text = "Horizontal-Bottom";
            label143.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrHorizontalBottomHallway
            // 
            csrHorizontalBottomHallway.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrHorizontalBottomHallway.BackgroundColor");
            csrHorizontalBottomHallway.Character = '\0';
            csrHorizontalBottomHallway.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrHorizontalBottomHallway.ForegroundColor");
            csrHorizontalBottomHallway.Location = new System.Drawing.Point(12, 773);
            csrHorizontalBottomHallway.Name = "csrHorizontalBottomHallway";
            csrHorizontalBottomHallway.Size = new System.Drawing.Size(211, 83);
            csrHorizontalBottomHallway.TabIndex = 194;
            csrHorizontalBottomHallway.PropertyChanged += csrHorizontalBottomHallway_PropertyChanged;
            // 
            // label144
            // 
            label144.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label144.Location = new System.Drawing.Point(446, 487);
            label144.Name = "label144";
            label144.Size = new System.Drawing.Size(211, 32);
            label144.TabIndex = 191;
            label144.Text = "Vertical";
            label144.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrVerticalHallway
            // 
            csrVerticalHallway.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrVerticalHallway.BackgroundColor");
            csrVerticalHallway.Character = '\0';
            csrVerticalHallway.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrVerticalHallway.ForegroundColor");
            csrVerticalHallway.Location = new System.Drawing.Point(446, 522);
            csrVerticalHallway.Name = "csrVerticalHallway";
            csrVerticalHallway.Size = new System.Drawing.Size(211, 83);
            csrVerticalHallway.TabIndex = 192;
            csrVerticalHallway.PropertyChanged += csrVerticalHallway_PropertyChanged;
            // 
            // label145
            // 
            label145.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label145.Location = new System.Drawing.Point(229, 487);
            label145.Name = "label145";
            label145.Size = new System.Drawing.Size(211, 32);
            label145.TabIndex = 189;
            label145.Text = "Top Right Corner";
            label145.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrTopRightHallway
            // 
            csrTopRightHallway.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrTopRightHallway.BackgroundColor");
            csrTopRightHallway.Character = '\0';
            csrTopRightHallway.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrTopRightHallway.ForegroundColor");
            csrTopRightHallway.Location = new System.Drawing.Point(229, 522);
            csrTopRightHallway.Name = "csrTopRightHallway";
            csrTopRightHallway.Size = new System.Drawing.Size(211, 83);
            csrTopRightHallway.TabIndex = 190;
            csrTopRightHallway.PropertyChanged += csrTopRightHallway_PropertyChanged;
            // 
            // label146
            // 
            label146.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label146.Location = new System.Drawing.Point(229, 435);
            label146.Name = "label146";
            label146.Size = new System.Drawing.Size(211, 52);
            label146.TabIndex = 188;
            label146.Text = "HALLWAYS";
            label146.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label147
            // 
            label147.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label147.Location = new System.Drawing.Point(12, 487);
            label147.Name = "label147";
            label147.Size = new System.Drawing.Size(211, 32);
            label147.TabIndex = 186;
            label147.Text = "Top Left Corner";
            label147.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrTopLeftHallway
            // 
            csrTopLeftHallway.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrTopLeftHallway.BackgroundColor");
            csrTopLeftHallway.Character = '\0';
            csrTopLeftHallway.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrTopLeftHallway.ForegroundColor");
            csrTopLeftHallway.Location = new System.Drawing.Point(12, 522);
            csrTopLeftHallway.Name = "csrTopLeftHallway";
            csrTopLeftHallway.Size = new System.Drawing.Size(211, 83);
            csrTopLeftHallway.TabIndex = 187;
            csrTopLeftHallway.PropertyChanged += csrTopLeftHallway_PropertyChanged;
            // 
            // label138
            // 
            label138.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label138.Location = new System.Drawing.Point(245, 301);
            label138.Name = "label138";
            label138.Size = new System.Drawing.Size(211, 32);
            label138.TabIndex = 184;
            label138.Text = "Hallway Connector";
            label138.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrConnectorWall
            // 
            csrConnectorWall.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrConnectorWall.BackgroundColor");
            csrConnectorWall.Character = '\0';
            csrConnectorWall.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrConnectorWall.ForegroundColor");
            csrConnectorWall.Location = new System.Drawing.Point(245, 336);
            csrConnectorWall.Name = "csrConnectorWall";
            csrConnectorWall.Size = new System.Drawing.Size(211, 83);
            csrConnectorWall.TabIndex = 185;
            csrConnectorWall.PropertyChanged += csrConnectorWall_PropertyChanged;
            // 
            // label134
            // 
            label134.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label134.Location = new System.Drawing.Point(462, 180);
            label134.Name = "label134";
            label134.Size = new System.Drawing.Size(211, 32);
            label134.TabIndex = 182;
            label134.Text = "Horizontal";
            label134.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrHorizontalWall
            // 
            csrHorizontalWall.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrHorizontalWall.BackgroundColor");
            csrHorizontalWall.Character = '\0';
            csrHorizontalWall.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrHorizontalWall.ForegroundColor");
            csrHorizontalWall.Location = new System.Drawing.Point(462, 215);
            csrHorizontalWall.Name = "csrHorizontalWall";
            csrHorizontalWall.Size = new System.Drawing.Size(211, 83);
            csrHorizontalWall.TabIndex = 183;
            csrHorizontalWall.PropertyChanged += csrHorizontalWall_PropertyChanged;
            // 
            // label135
            // 
            label135.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label135.Location = new System.Drawing.Point(245, 180);
            label135.Name = "label135";
            label135.Size = new System.Drawing.Size(211, 32);
            label135.TabIndex = 180;
            label135.Text = "Bottom Right Corner";
            label135.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrBottomRightWall
            // 
            csrBottomRightWall.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrBottomRightWall.BackgroundColor");
            csrBottomRightWall.Character = '\0';
            csrBottomRightWall.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrBottomRightWall.ForegroundColor");
            csrBottomRightWall.Location = new System.Drawing.Point(245, 215);
            csrBottomRightWall.Name = "csrBottomRightWall";
            csrBottomRightWall.Size = new System.Drawing.Size(211, 83);
            csrBottomRightWall.TabIndex = 181;
            csrBottomRightWall.PropertyChanged += csrBottomRightWall_PropertyChanged;
            // 
            // label136
            // 
            label136.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label136.Location = new System.Drawing.Point(28, 180);
            label136.Name = "label136";
            label136.Size = new System.Drawing.Size(211, 32);
            label136.TabIndex = 178;
            label136.Text = "Bottom Left Corner";
            label136.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrBottomLeftWall
            // 
            csrBottomLeftWall.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrBottomLeftWall.BackgroundColor");
            csrBottomLeftWall.Character = '\0';
            csrBottomLeftWall.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrBottomLeftWall.ForegroundColor");
            csrBottomLeftWall.Location = new System.Drawing.Point(28, 215);
            csrBottomLeftWall.Name = "csrBottomLeftWall";
            csrBottomLeftWall.Size = new System.Drawing.Size(211, 83);
            csrBottomLeftWall.TabIndex = 179;
            csrBottomLeftWall.PropertyChanged += csrBottomLeftWall_PropertyChanged;
            // 
            // label133
            // 
            label133.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label133.Location = new System.Drawing.Point(462, 59);
            label133.Name = "label133";
            label133.Size = new System.Drawing.Size(211, 32);
            label133.TabIndex = 176;
            label133.Text = "Vertical";
            label133.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrVerticalWall
            // 
            csrVerticalWall.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrVerticalWall.BackgroundColor");
            csrVerticalWall.Character = '\0';
            csrVerticalWall.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrVerticalWall.ForegroundColor");
            csrVerticalWall.Location = new System.Drawing.Point(462, 94);
            csrVerticalWall.Name = "csrVerticalWall";
            csrVerticalWall.Size = new System.Drawing.Size(211, 83);
            csrVerticalWall.TabIndex = 177;
            csrVerticalWall.PropertyChanged += csrVerticalWall_PropertyChanged;
            // 
            // label132
            // 
            label132.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label132.Location = new System.Drawing.Point(245, 59);
            label132.Name = "label132";
            label132.Size = new System.Drawing.Size(211, 32);
            label132.TabIndex = 174;
            label132.Text = "Top Right Corner";
            label132.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrTopRightWall
            // 
            csrTopRightWall.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrTopRightWall.BackgroundColor");
            csrTopRightWall.Character = '\0';
            csrTopRightWall.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrTopRightWall.ForegroundColor");
            csrTopRightWall.Location = new System.Drawing.Point(245, 94);
            csrTopRightWall.Name = "csrTopRightWall";
            csrTopRightWall.Size = new System.Drawing.Size(211, 83);
            csrTopRightWall.TabIndex = 175;
            csrTopRightWall.PropertyChanged += csrTopRightWall_PropertyChanged;
            // 
            // label131
            // 
            label131.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label131.Location = new System.Drawing.Point(245, 7);
            label131.Name = "label131";
            label131.Size = new System.Drawing.Size(211, 52);
            label131.TabIndex = 173;
            label131.Text = "WALLS";
            label131.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label130
            // 
            label130.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label130.Location = new System.Drawing.Point(28, 59);
            label130.Name = "label130";
            label130.Size = new System.Drawing.Size(211, 32);
            label130.TabIndex = 171;
            label130.Text = "Top Left Corner";
            label130.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // csrTopLeftWall
            // 
            csrTopLeftWall.BackgroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrTopLeftWall.BackgroundColor");
            csrTopLeftWall.Character = '\0';
            csrTopLeftWall.ForegroundColor = (RogueCustomsGameEngine.Utils.Representation.GameColor)resources.GetObject("csrTopLeftWall.ForegroundColor");
            csrTopLeftWall.Location = new System.Drawing.Point(28, 94);
            csrTopLeftWall.Name = "csrTopLeftWall";
            csrTopLeftWall.Size = new System.Drawing.Size(211, 83);
            csrTopLeftWall.TabIndex = 172;
            csrTopLeftWall.PropertyChanged += csrTopLeftWall_PropertyChanged;
            // 
            // TilesetTab
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Controls.Add(label151);
            Controls.Add(csrEmpty);
            Controls.Add(label152);
            Controls.Add(csrStairs);
            Controls.Add(label153);
            Controls.Add(label154);
            Controls.Add(csrFloor);
            Controls.Add(label148);
            Controls.Add(csrHorizontalHallway);
            Controls.Add(label149);
            Controls.Add(csrBottomRightHallway);
            Controls.Add(label150);
            Controls.Add(csrBottomLeftHallway);
            Controls.Add(label137);
            Controls.Add(csrCentralHallway);
            Controls.Add(label140);
            Controls.Add(csrVerticalRightHallway);
            Controls.Add(label141);
            Controls.Add(csrVerticalLeftHallway);
            Controls.Add(label142);
            Controls.Add(csrHorizontalTopHallway);
            Controls.Add(label143);
            Controls.Add(csrHorizontalBottomHallway);
            Controls.Add(label144);
            Controls.Add(csrVerticalHallway);
            Controls.Add(label145);
            Controls.Add(csrTopRightHallway);
            Controls.Add(label146);
            Controls.Add(label147);
            Controls.Add(csrTopLeftHallway);
            Controls.Add(label138);
            Controls.Add(csrConnectorWall);
            Controls.Add(label134);
            Controls.Add(csrHorizontalWall);
            Controls.Add(label135);
            Controls.Add(csrBottomRightWall);
            Controls.Add(label136);
            Controls.Add(csrBottomLeftWall);
            Controls.Add(label133);
            Controls.Add(csrVerticalWall);
            Controls.Add(label132);
            Controls.Add(csrTopRightWall);
            Controls.Add(label131);
            Controls.Add(label130);
            Controls.Add(csrTopLeftWall);
            Name = "TilesetTab";
            Size = new System.Drawing.Size(676, 1173);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label label151;
        private ConsoleRepresentationSelector csrEmpty;
        private System.Windows.Forms.Label label152;
        private ConsoleRepresentationSelector csrStairs;
        private System.Windows.Forms.Label label153;
        private System.Windows.Forms.Label label154;
        private ConsoleRepresentationSelector csrFloor;
        private System.Windows.Forms.Label label148;
        private ConsoleRepresentationSelector csrHorizontalHallway;
        private System.Windows.Forms.Label label149;
        private ConsoleRepresentationSelector csrBottomRightHallway;
        private System.Windows.Forms.Label label150;
        private ConsoleRepresentationSelector csrBottomLeftHallway;
        private System.Windows.Forms.Label label137;
        private ConsoleRepresentationSelector csrCentralHallway;
        private System.Windows.Forms.Label label140;
        private ConsoleRepresentationSelector csrVerticalRightHallway;
        private System.Windows.Forms.Label label141;
        private ConsoleRepresentationSelector csrVerticalLeftHallway;
        private System.Windows.Forms.Label label142;
        private ConsoleRepresentationSelector csrHorizontalTopHallway;
        private System.Windows.Forms.Label label143;
        private ConsoleRepresentationSelector csrHorizontalBottomHallway;
        private System.Windows.Forms.Label label144;
        private ConsoleRepresentationSelector csrVerticalHallway;
        private System.Windows.Forms.Label label145;
        private ConsoleRepresentationSelector csrTopRightHallway;
        private System.Windows.Forms.Label label146;
        private System.Windows.Forms.Label label147;
        private ConsoleRepresentationSelector csrTopLeftHallway;
        private System.Windows.Forms.Label label138;
        private ConsoleRepresentationSelector csrConnectorWall;
        private System.Windows.Forms.Label label134;
        private ConsoleRepresentationSelector csrHorizontalWall;
        private System.Windows.Forms.Label label135;
        private ConsoleRepresentationSelector csrBottomRightWall;
        private System.Windows.Forms.Label label136;
        private ConsoleRepresentationSelector csrBottomLeftWall;
        private System.Windows.Forms.Label label133;
        private ConsoleRepresentationSelector csrVerticalWall;
        private System.Windows.Forms.Label label132;
        private ConsoleRepresentationSelector csrTopRightWall;
        private System.Windows.Forms.Label label131;
        private System.Windows.Forms.Label label130;
        private ConsoleRepresentationSelector csrTopLeftWall;
    }
}
