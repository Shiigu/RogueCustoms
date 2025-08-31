namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    partial class TileTypeTab
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(TileTypeTab));
            fklblTileTypeDescriptionLocale = new System.Windows.Forms.Button();
            txtTileTypeDescription = new System.Windows.Forms.TextBox();
            label100 = new System.Windows.Forms.Label();
            fklblTileTypeNameLocale = new System.Windows.Forms.Button();
            txtTileTypeName = new System.Windows.Forms.TextBox();
            label101 = new System.Windows.Forms.Label();
            chkTileTypeIsWalkable = new System.Windows.Forms.CheckBox();
            chkTileTypeIsSolid = new System.Windows.Forms.CheckBox();
            chkTileTypeIsVisible = new System.Windows.Forms.CheckBox();
            chkTileTypeCanBeTransformed = new System.Windows.Forms.CheckBox();
            chkTileTypeAcceptsItems = new System.Windows.Forms.CheckBox();
            chkTileTypeCanVisiblyConnectWithOtherTiles = new System.Windows.Forms.CheckBox();
            chkTileTypeCanHaveMultilineConnections = new System.Windows.Forms.CheckBox();
            label98 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            saeOnStood = new SingleActionEditor();
            chkTileTypeCausesPartialInvisibility = new System.Windows.Forms.CheckBox();
            SuspendLayout();
            // 
            // fklblTileTypeDescriptionLocale
            // 
            fklblTileTypeDescriptionLocale.Enabled = false;
            fklblTileTypeDescriptionLocale.FlatAppearance.BorderSize = 0;
            fklblTileTypeDescriptionLocale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblTileTypeDescriptionLocale.Image = (System.Drawing.Image)resources.GetObject("fklblTileTypeDescriptionLocale.Image");
            fklblTileTypeDescriptionLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblTileTypeDescriptionLocale.Location = new System.Drawing.Point(3, 150);
            fklblTileTypeDescriptionLocale.Name = "fklblTileTypeDescriptionLocale";
            fklblTileTypeDescriptionLocale.Size = new System.Drawing.Size(331, 42);
            fklblTileTypeDescriptionLocale.TabIndex = 229;
            fklblTileTypeDescriptionLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblTileTypeDescriptionLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblTileTypeDescriptionLocale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblTileTypeDescriptionLocale.UseVisualStyleBackColor = true;
            fklblTileTypeDescriptionLocale.Visible = false;
            // 
            // txtTileTypeDescription
            // 
            txtTileTypeDescription.Location = new System.Drawing.Point(3, 121);
            txtTileTypeDescription.Name = "txtTileTypeDescription";
            txtTileTypeDescription.Size = new System.Drawing.Size(350, 23);
            txtTileTypeDescription.TabIndex = 228;
            txtTileTypeDescription.TextChanged += txtTileTypeDescription_TextChanged;
            // 
            // label100
            // 
            label100.AutoSize = true;
            label100.Location = new System.Drawing.Point(3, 103);
            label100.Name = "label100";
            label100.Size = new System.Drawing.Size(67, 15);
            label100.TabIndex = 227;
            label100.Text = "Description";
            // 
            // fklblTileTypeNameLocale
            // 
            fklblTileTypeNameLocale.Enabled = false;
            fklblTileTypeNameLocale.FlatAppearance.BorderSize = 0;
            fklblTileTypeNameLocale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblTileTypeNameLocale.Image = (System.Drawing.Image)resources.GetObject("fklblTileTypeNameLocale.Image");
            fklblTileTypeNameLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblTileTypeNameLocale.Location = new System.Drawing.Point(3, 52);
            fklblTileTypeNameLocale.Name = "fklblTileTypeNameLocale";
            fklblTileTypeNameLocale.Size = new System.Drawing.Size(331, 42);
            fklblTileTypeNameLocale.TabIndex = 226;
            fklblTileTypeNameLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblTileTypeNameLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblTileTypeNameLocale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblTileTypeNameLocale.UseVisualStyleBackColor = true;
            fklblTileTypeNameLocale.Visible = false;
            // 
            // txtTileTypeName
            // 
            txtTileTypeName.Location = new System.Drawing.Point(3, 23);
            txtTileTypeName.Name = "txtTileTypeName";
            txtTileTypeName.Size = new System.Drawing.Size(350, 23);
            txtTileTypeName.TabIndex = 225;
            txtTileTypeName.TextChanged += txtTileTypeName_TextChanged;
            // 
            // label101
            // 
            label101.AutoSize = true;
            label101.Location = new System.Drawing.Point(3, 5);
            label101.Name = "label101";
            label101.Size = new System.Drawing.Size(39, 15);
            label101.TabIndex = 224;
            label101.Text = "Name";
            // 
            // chkTileTypeIsWalkable
            // 
            chkTileTypeIsWalkable.AutoSize = true;
            chkTileTypeIsWalkable.Location = new System.Drawing.Point(372, 65);
            chkTileTypeIsWalkable.Name = "chkTileTypeIsWalkable";
            chkTileTypeIsWalkable.Size = new System.Drawing.Size(85, 19);
            chkTileTypeIsWalkable.TabIndex = 230;
            chkTileTypeIsWalkable.Text = "Is Walkable";
            chkTileTypeIsWalkable.UseVisualStyleBackColor = true;
            chkTileTypeIsWalkable.CheckedChanged += chkTileTypeIsWalkable_CheckedChanged;
            // 
            // chkTileTypeIsSolid
            // 
            chkTileTypeIsSolid.AutoSize = true;
            chkTileTypeIsSolid.Location = new System.Drawing.Point(502, 65);
            chkTileTypeIsSolid.Name = "chkTileTypeIsSolid";
            chkTileTypeIsSolid.Size = new System.Drawing.Size(63, 19);
            chkTileTypeIsSolid.TabIndex = 231;
            chkTileTypeIsSolid.Text = "Is Solid";
            chkTileTypeIsSolid.UseVisualStyleBackColor = true;
            chkTileTypeIsSolid.CheckedChanged += chkTileTypeIsSolid_CheckedChanged;
            // 
            // chkTileTypeIsVisible
            // 
            chkTileTypeIsVisible.AutoSize = true;
            chkTileTypeIsVisible.Location = new System.Drawing.Point(608, 65);
            chkTileTypeIsVisible.Name = "chkTileTypeIsVisible";
            chkTileTypeIsVisible.Size = new System.Drawing.Size(90, 19);
            chkTileTypeIsVisible.TabIndex = 232;
            chkTileTypeIsVisible.Text = "Can be seen";
            chkTileTypeIsVisible.UseVisualStyleBackColor = true;
            chkTileTypeIsVisible.CheckedChanged += chkTileTypeIsVisible_CheckedChanged;
            // 
            // chkTileTypeCanBeTransformed
            // 
            chkTileTypeCanBeTransformed.AutoSize = true;
            chkTileTypeCanBeTransformed.Location = new System.Drawing.Point(535, 90);
            chkTileTypeCanBeTransformed.Name = "chkTileTypeCanBeTransformed";
            chkTileTypeCanBeTransformed.Size = new System.Drawing.Size(179, 19);
            chkTileTypeCanBeTransformed.TabIndex = 234;
            chkTileTypeCanBeTransformed.Text = "Is a valid TransformTile target";
            chkTileTypeCanBeTransformed.UseVisualStyleBackColor = true;
            chkTileTypeCanBeTransformed.CheckedChanged += chkTileTypeCanBeTransformed_CheckedChanged;
            // 
            // chkTileTypeAcceptsItems
            // 
            chkTileTypeAcceptsItems.AutoSize = true;
            chkTileTypeAcceptsItems.Location = new System.Drawing.Point(372, 90);
            chkTileTypeAcceptsItems.Name = "chkTileTypeAcceptsItems";
            chkTileTypeAcceptsItems.Size = new System.Drawing.Size(157, 19);
            chkTileTypeAcceptsItems.TabIndex = 235;
            chkTileTypeAcceptsItems.Text = "Items can be placed here";
            chkTileTypeAcceptsItems.UseVisualStyleBackColor = true;
            chkTileTypeAcceptsItems.CheckedChanged += chkTileTypeAcceptsItems_CheckedChanged;
            // 
            // chkTileTypeCanVisiblyConnectWithOtherTiles
            // 
            chkTileTypeCanVisiblyConnectWithOtherTiles.AutoSize = true;
            chkTileTypeCanVisiblyConnectWithOtherTiles.Location = new System.Drawing.Point(14, 250);
            chkTileTypeCanVisiblyConnectWithOtherTiles.Name = "chkTileTypeCanVisiblyConnectWithOtherTiles";
            chkTileTypeCanVisiblyConnectWithOtherTiles.Size = new System.Drawing.Size(304, 19);
            chkTileTypeCanVisiblyConnectWithOtherTiles.TabIndex = 236;
            chkTileTypeCanVisiblyConnectWithOtherTiles.Text = "Can visibly connect with other Tiles of the same Type";
            chkTileTypeCanVisiblyConnectWithOtherTiles.UseVisualStyleBackColor = true;
            chkTileTypeCanVisiblyConnectWithOtherTiles.CheckedChanged += chkTileTypeCanVisiblyConnectWithOtherTiles_CheckedChanged;
            // 
            // chkTileTypeCanHaveMultilineConnections
            // 
            chkTileTypeCanHaveMultilineConnections.AutoSize = true;
            chkTileTypeCanHaveMultilineConnections.Location = new System.Drawing.Point(14, 275);
            chkTileTypeCanHaveMultilineConnections.Name = "chkTileTypeCanHaveMultilineConnections";
            chkTileTypeCanHaveMultilineConnections.Size = new System.Drawing.Size(354, 19);
            chkTileTypeCanHaveMultilineConnections.TabIndex = 237;
            chkTileTypeCanHaveMultilineConnections.Text = "Can visibly connect with another row of Tiles of the same Type";
            chkTileTypeCanHaveMultilineConnections.UseVisualStyleBackColor = true;
            chkTileTypeCanHaveMultilineConnections.CheckedChanged += chkTileTypeCanHaveMultilineConnections_CheckedChanged;
            // 
            // label98
            // 
            label98.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label98.Location = new System.Drawing.Point(475, 5);
            label98.Name = "label98";
            label98.Size = new System.Drawing.Size(131, 52);
            label98.TabIndex = 238;
            label98.Text = "Behaviour";
            label98.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label1.Location = new System.Drawing.Point(101, 195);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(131, 52);
            label1.TabIndex = 239;
            label1.Text = "Appearance";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // saeOnStood
            // 
            saeOnStood.Action = null;
            saeOnStood.ActionDescription = "When a character starts a turn on this\r\nor steps into this...";
            saeOnStood.ActionTypeText = "Standing on Tile";
            saeOnStood.AutoSize = true;
            saeOnStood.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saeOnStood.ClassId = null;
            saeOnStood.Dungeon = null;
            saeOnStood.EffectParamData = null;
            saeOnStood.Location = new System.Drawing.Point(372, 156);
            saeOnStood.Name = "saeOnStood";
            saeOnStood.PlaceholderActionId = "OnTileStand";
            saeOnStood.RequiresActionId = false;
            saeOnStood.RequiresCondition = true;
            saeOnStood.RequiresDescription = false;
            saeOnStood.RequiresName = false;
            saeOnStood.Size = new System.Drawing.Size(337, 32);
            saeOnStood.SourceDescription = "The player (Don't use)";
            saeOnStood.TabIndex = 240;
            saeOnStood.TargetDescription = "The player";
            saeOnStood.ThisDescription = "None (Don't use)";
            saeOnStood.TurnEndCriteria = HelperForms.TurnEndCriteria.CannotEndTurn;
            saeOnStood.UsageCriteria = HelperForms.UsageCriteria.AnyTargetAnyTime;
            saeOnStood.ActionContentsChanged += saeOnStood_ActionContentsChanged;
            // 
            // chkTileTypeCausesPartialInvisibility
            // 
            chkTileTypeCausesPartialInvisibility.AutoSize = true;
            chkTileTypeCausesPartialInvisibility.Location = new System.Drawing.Point(372, 116);
            chkTileTypeCausesPartialInvisibility.Name = "chkTileTypeCausesPartialInvisibility";
            chkTileTypeCausesPartialInvisibility.Size = new System.Drawing.Size(269, 34);
            chkTileTypeCausesPartialInvisibility.TabIndex = 241;
            chkTileTypeCausesPartialInvisibility.Text = "Characters standing here are invisible to\r\nCharacters standing on tiles of a different type";
            chkTileTypeCausesPartialInvisibility.UseVisualStyleBackColor = true;
            chkTileTypeCausesPartialInvisibility.CheckedChanged += chkTileTypeCausesPartialInvisibility_CheckedChanged;
            // 
            // TileTypeTab
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(chkTileTypeCausesPartialInvisibility);
            Controls.Add(saeOnStood);
            Controls.Add(label1);
            Controls.Add(label98);
            Controls.Add(chkTileTypeCanHaveMultilineConnections);
            Controls.Add(chkTileTypeCanVisiblyConnectWithOtherTiles);
            Controls.Add(chkTileTypeAcceptsItems);
            Controls.Add(chkTileTypeCanBeTransformed);
            Controls.Add(chkTileTypeIsVisible);
            Controls.Add(chkTileTypeIsSolid);
            Controls.Add(chkTileTypeIsWalkable);
            Controls.Add(fklblTileTypeDescriptionLocale);
            Controls.Add(txtTileTypeDescription);
            Controls.Add(label100);
            Controls.Add(fklblTileTypeNameLocale);
            Controls.Add(txtTileTypeName);
            Controls.Add(label101);
            Name = "TileTypeTab";
            Size = new System.Drawing.Size(718, 307);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button fklblTileTypeDescriptionLocale;
        private System.Windows.Forms.TextBox txtTileTypeDescription;
        private System.Windows.Forms.Label label100;
        private System.Windows.Forms.Button fklblTileTypeNameLocale;
        private System.Windows.Forms.TextBox txtTileTypeName;
        private System.Windows.Forms.Label label101;
        private System.Windows.Forms.CheckBox chkTileTypeIsWalkable;
        private System.Windows.Forms.CheckBox chkTileTypeIsSolid;
        private System.Windows.Forms.CheckBox chkTileTypeIsVisible;
        private System.Windows.Forms.CheckBox chkTileTypeCanBeTransformed;
        private System.Windows.Forms.CheckBox chkTileTypeAcceptsItems;
        private System.Windows.Forms.CheckBox chkTileTypeCanVisiblyConnectWithOtherTiles;
        private System.Windows.Forms.CheckBox chkTileTypeCanHaveMultilineConnections;
        private System.Windows.Forms.Label label98;
        private System.Windows.Forms.Label label1;
        private SingleActionEditor saeOnStood;
        private System.Windows.Forms.CheckBox chkTileTypeCausesPartialInvisibility;
    }
}
