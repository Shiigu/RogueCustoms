namespace RogueCustomsDungeonEditor.Controls
{
    partial class KeyDoorEditor
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(KeyDoorEditor));
            chkCanLockStairs = new System.Windows.Forms.CheckBox();
            fklblKeyDoorNameLocale = new System.Windows.Forms.Button();
            txtKeyTypeName = new System.Windows.Forms.TextBox();
            label27 = new System.Windows.Forms.Label();
            chkCanLockItems = new System.Windows.Forms.CheckBox();
            label30 = new System.Windows.Forms.Label();
            crsKey = new ConsoleRepresentationSelector();
            label1 = new System.Windows.Forms.Label();
            crsDoor = new ConsoleRepresentationSelector();
            SuspendLayout();
            // 
            // chkCanLockStairs
            // 
            chkCanLockStairs.AutoSize = true;
            chkCanLockStairs.BackColor = System.Drawing.Color.Transparent;
            chkCanLockStairs.Location = new System.Drawing.Point(59, 113);
            chkCanLockStairs.Name = "chkCanLockStairs";
            chkCanLockStairs.Size = new System.Drawing.Size(173, 19);
            chkCanLockStairs.TabIndex = 140;
            chkCanLockStairs.Text = "Can lock a Room with Stairs";
            chkCanLockStairs.UseVisualStyleBackColor = false;
            // 
            // fklblKeyDoorNameLocale
            // 
            fklblKeyDoorNameLocale.BackColor = System.Drawing.Color.Transparent;
            fklblKeyDoorNameLocale.Enabled = false;
            fklblKeyDoorNameLocale.FlatAppearance.BorderSize = 0;
            fklblKeyDoorNameLocale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblKeyDoorNameLocale.Image = (System.Drawing.Image)resources.GetObject("fklblKeyDoorNameLocale.Image");
            fklblKeyDoorNameLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblKeyDoorNameLocale.Location = new System.Drawing.Point(3, 52);
            fklblKeyDoorNameLocale.Name = "fklblKeyDoorNameLocale";
            fklblKeyDoorNameLocale.Size = new System.Drawing.Size(315, 55);
            fklblKeyDoorNameLocale.TabIndex = 139;
            fklblKeyDoorNameLocale.Text = "These values have been found as Locale Entry keys.\r\nIn-game, they will be replaced by the Locale Entries' values.";
            fklblKeyDoorNameLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblKeyDoorNameLocale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblKeyDoorNameLocale.UseVisualStyleBackColor = false;
            fklblKeyDoorNameLocale.Visible = false;
            // 
            // txtKeyTypeName
            // 
            txtKeyTypeName.Location = new System.Drawing.Point(3, 23);
            txtKeyTypeName.Name = "txtKeyTypeName";
            txtKeyTypeName.Size = new System.Drawing.Size(287, 23);
            txtKeyTypeName.TabIndex = 138;
            txtKeyTypeName.TextChanged += txtKeyTypeName_TextChanged;
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.BackColor = System.Drawing.Color.Transparent;
            label27.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            label27.Location = new System.Drawing.Point(28, 5);
            label27.Name = "label27";
            label27.Size = new System.Drawing.Size(239, 15);
            label27.TabIndex = 137;
            label27.Text = "Key type (Names: KeyTypeX / DoorTypeX)";
            // 
            // chkCanLockItems
            // 
            chkCanLockItems.AutoSize = true;
            chkCanLockItems.BackColor = System.Drawing.Color.Transparent;
            chkCanLockItems.Location = new System.Drawing.Point(59, 138);
            chkCanLockItems.Name = "chkCanLockItems";
            chkCanLockItems.Size = new System.Drawing.Size(174, 19);
            chkCanLockItems.TabIndex = 142;
            chkCanLockItems.Text = "Can lock a Room with Items";
            chkCanLockItems.UseVisualStyleBackColor = false;
            // 
            // label30
            // 
            label30.BackColor = System.Drawing.Color.Transparent;
            label30.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            label30.Location = new System.Drawing.Point(324, 9);
            label30.Name = "label30";
            label30.Size = new System.Drawing.Size(211, 52);
            label30.TabIndex = 149;
            label30.Text = "Key Appearance";
            label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // crsKey
            // 
            crsKey.Location = new System.Drawing.Point(324, 70);
            crsKey.Name = "crsKey";
            crsKey.Size = new System.Drawing.Size(211, 83);
            crsKey.TabIndex = 150;
            // 
            // label1
            // 
            label1.BackColor = System.Drawing.Color.Transparent;
            label1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            label1.Location = new System.Drawing.Point(576, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(211, 52);
            label1.TabIndex = 151;
            label1.Text = "Door Appearance";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // crsDoor
            // 
            crsDoor.Location = new System.Drawing.Point(576, 70);
            crsDoor.Name = "crsDoor";
            crsDoor.Size = new System.Drawing.Size(211, 83);
            crsDoor.TabIndex = 152;
            // 
            // KeyDoorEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(label1);
            Controls.Add(crsDoor);
            Controls.Add(label30);
            Controls.Add(crsKey);
            Controls.Add(chkCanLockItems);
            Controls.Add(chkCanLockStairs);
            Controls.Add(fklblKeyDoorNameLocale);
            Controls.Add(txtKeyTypeName);
            Controls.Add(label27);
            Margin = new System.Windows.Forms.Padding(0);
            Name = "KeyDoorEditor";
            Size = new System.Drawing.Size(797, 162);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.CheckBox chkCanLockStairs;
        private System.Windows.Forms.Button fklblKeyDoorNameLocale;
        private System.Windows.Forms.TextBox txtKeyTypeName;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.CheckBox chkCanLockItems;
        private System.Windows.Forms.Label label30;
        private ConsoleRepresentationSelector crsKey;
        private System.Windows.Forms.Label label1;
        private ConsoleRepresentationSelector crsDoor;
    }
}
