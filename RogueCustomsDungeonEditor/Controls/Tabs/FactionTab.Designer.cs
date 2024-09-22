namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    partial class FactionTab
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(FactionTab));
            lbEnemies = new System.Windows.Forms.ListBox();
            label26 = new System.Windows.Forms.Label();
            btnEnemiesToNeutrals = new System.Windows.Forms.Button();
            btnEnemyToNeutral = new System.Windows.Forms.Button();
            btnNeutralsToEnemies = new System.Windows.Forms.Button();
            btnNeutralToEnemy = new System.Windows.Forms.Button();
            lbNeutrals = new System.Windows.Forms.ListBox();
            label25 = new System.Windows.Forms.Label();
            btnNeutralsToAllies = new System.Windows.Forms.Button();
            btnNeutralToAlly = new System.Windows.Forms.Button();
            btnAlliesToNeutrals = new System.Windows.Forms.Button();
            btnAllyToNeutral = new System.Windows.Forms.Button();
            lbAllies = new System.Windows.Forms.ListBox();
            label24 = new System.Windows.Forms.Label();
            label23 = new System.Windows.Forms.Label();
            fklblFactionDescriptionLocale = new System.Windows.Forms.Button();
            txtFactionDescription = new System.Windows.Forms.TextBox();
            label22 = new System.Windows.Forms.Label();
            fklblFactionNameLocale = new System.Windows.Forms.Button();
            txtFactionName = new System.Windows.Forms.TextBox();
            label21 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // lbEnemies
            // 
            lbEnemies.FormattingEnabled = true;
            lbEnemies.ItemHeight = 15;
            lbEnemies.Location = new System.Drawing.Point(445, 156);
            lbEnemies.Name = "lbEnemies";
            lbEnemies.Size = new System.Drawing.Size(111, 169);
            lbEnemies.TabIndex = 54;
            lbEnemies.SelectedIndexChanged += lbEnemies_SelectedIndexChanged;
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label26.Location = new System.Drawing.Point(456, 138);
            label26.Name = "label26";
            label26.Size = new System.Drawing.Size(92, 15);
            label26.TabIndex = 53;
            label26.Text = "Enemies With...";
            // 
            // btnEnemiesToNeutrals
            // 
            btnEnemiesToNeutrals.Enabled = false;
            btnEnemiesToNeutrals.Location = new System.Drawing.Point(402, 206);
            btnEnemiesToNeutrals.Name = "btnEnemiesToNeutrals";
            btnEnemiesToNeutrals.Size = new System.Drawing.Size(37, 23);
            btnEnemiesToNeutrals.TabIndex = 52;
            btnEnemiesToNeutrals.Text = "<<";
            btnEnemiesToNeutrals.UseVisualStyleBackColor = true;
            btnEnemiesToNeutrals.Click += btnEnemiesToNeutrals_Click;
            // 
            // btnEnemyToNeutral
            // 
            btnEnemyToNeutral.Enabled = false;
            btnEnemyToNeutral.Location = new System.Drawing.Point(402, 177);
            btnEnemyToNeutral.Name = "btnEnemyToNeutral";
            btnEnemyToNeutral.Size = new System.Drawing.Size(37, 23);
            btnEnemyToNeutral.TabIndex = 51;
            btnEnemyToNeutral.Text = "<";
            btnEnemyToNeutral.UseVisualStyleBackColor = true;
            btnEnemyToNeutral.Click += btnEnemyToNeutral_Click;
            // 
            // btnNeutralsToEnemies
            // 
            btnNeutralsToEnemies.Enabled = false;
            btnNeutralsToEnemies.Location = new System.Drawing.Point(402, 279);
            btnNeutralsToEnemies.Name = "btnNeutralsToEnemies";
            btnNeutralsToEnemies.Size = new System.Drawing.Size(37, 23);
            btnNeutralsToEnemies.TabIndex = 50;
            btnNeutralsToEnemies.Text = ">>";
            btnNeutralsToEnemies.UseVisualStyleBackColor = true;
            btnNeutralsToEnemies.Click += btnNeutralsToEnemies_Click;
            // 
            // btnNeutralToEnemy
            // 
            btnNeutralToEnemy.Enabled = false;
            btnNeutralToEnemy.Location = new System.Drawing.Point(402, 250);
            btnNeutralToEnemy.Name = "btnNeutralToEnemy";
            btnNeutralToEnemy.Size = new System.Drawing.Size(37, 23);
            btnNeutralToEnemy.TabIndex = 49;
            btnNeutralToEnemy.Text = ">";
            btnNeutralToEnemy.UseVisualStyleBackColor = true;
            btnNeutralToEnemy.Click += btnNeutralToEnemy_Click;
            // 
            // lbNeutrals
            // 
            lbNeutrals.FormattingEnabled = true;
            lbNeutrals.ItemHeight = 15;
            lbNeutrals.Location = new System.Drawing.Point(285, 156);
            lbNeutrals.Name = "lbNeutrals";
            lbNeutrals.Size = new System.Drawing.Size(111, 169);
            lbNeutrals.TabIndex = 48;
            lbNeutrals.SelectedIndexChanged += lbNeutrals_SelectedIndexChanged;
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label25.Location = new System.Drawing.Point(298, 138);
            label25.Name = "label25";
            label25.Size = new System.Drawing.Size(88, 15);
            label25.TabIndex = 47;
            label25.Text = "Neutral With...";
            // 
            // btnNeutralsToAllies
            // 
            btnNeutralsToAllies.Enabled = false;
            btnNeutralsToAllies.Location = new System.Drawing.Point(242, 206);
            btnNeutralsToAllies.Name = "btnNeutralsToAllies";
            btnNeutralsToAllies.Size = new System.Drawing.Size(37, 23);
            btnNeutralsToAllies.TabIndex = 46;
            btnNeutralsToAllies.Text = "<<";
            btnNeutralsToAllies.UseVisualStyleBackColor = true;
            btnNeutralsToAllies.Click += btnNeutralsToAllies_Click;
            // 
            // btnNeutralToAlly
            // 
            btnNeutralToAlly.Enabled = false;
            btnNeutralToAlly.Location = new System.Drawing.Point(242, 177);
            btnNeutralToAlly.Name = "btnNeutralToAlly";
            btnNeutralToAlly.Size = new System.Drawing.Size(37, 23);
            btnNeutralToAlly.TabIndex = 45;
            btnNeutralToAlly.Text = "<";
            btnNeutralToAlly.UseVisualStyleBackColor = true;
            btnNeutralToAlly.Click += btnNeutralToAlly_Click;
            // 
            // btnAlliesToNeutrals
            // 
            btnAlliesToNeutrals.Enabled = false;
            btnAlliesToNeutrals.Location = new System.Drawing.Point(242, 279);
            btnAlliesToNeutrals.Name = "btnAlliesToNeutrals";
            btnAlliesToNeutrals.Size = new System.Drawing.Size(37, 23);
            btnAlliesToNeutrals.TabIndex = 44;
            btnAlliesToNeutrals.Text = ">>";
            btnAlliesToNeutrals.UseVisualStyleBackColor = true;
            btnAlliesToNeutrals.Click += btnAlliesToNeutrals_Click;
            // 
            // btnAllyToNeutral
            // 
            btnAllyToNeutral.Enabled = false;
            btnAllyToNeutral.Location = new System.Drawing.Point(242, 250);
            btnAllyToNeutral.Name = "btnAllyToNeutral";
            btnAllyToNeutral.Size = new System.Drawing.Size(37, 23);
            btnAllyToNeutral.TabIndex = 43;
            btnAllyToNeutral.Text = ">";
            btnAllyToNeutral.UseVisualStyleBackColor = true;
            btnAllyToNeutral.Click += btnAllyToNeutral_Click;
            // 
            // lbAllies
            // 
            lbAllies.FormattingEnabled = true;
            lbAllies.ItemHeight = 15;
            lbAllies.Location = new System.Drawing.Point(125, 156);
            lbAllies.Name = "lbAllies";
            lbAllies.Size = new System.Drawing.Size(111, 169);
            lbAllies.TabIndex = 42;
            lbAllies.SelectedIndexChanged += lbAllies_SelectedIndexChanged;
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label24.Location = new System.Drawing.Point(142, 138);
            label24.Name = "label24";
            label24.Size = new System.Drawing.Size(77, 15);
            label24.TabIndex = 41;
            label24.Text = "Allied With...";
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label23.Location = new System.Drawing.Point(291, 104);
            label23.Name = "label23";
            label23.Size = new System.Drawing.Size(98, 21);
            label23.TabIndex = 40;
            label23.Text = "Allegiances";
            // 
            // fklblFactionDescriptionLocale
            // 
            fklblFactionDescriptionLocale.Enabled = false;
            fklblFactionDescriptionLocale.FlatAppearance.BorderSize = 0;
            fklblFactionDescriptionLocale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblFactionDescriptionLocale.Image = (System.Drawing.Image)resources.GetObject("fklblFactionDescriptionLocale.Image");
            fklblFactionDescriptionLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblFactionDescriptionLocale.Location = new System.Drawing.Point(370, 54);
            fklblFactionDescriptionLocale.Name = "fklblFactionDescriptionLocale";
            fklblFactionDescriptionLocale.Size = new System.Drawing.Size(331, 42);
            fklblFactionDescriptionLocale.TabIndex = 39;
            fklblFactionDescriptionLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblFactionDescriptionLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblFactionDescriptionLocale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblFactionDescriptionLocale.UseVisualStyleBackColor = true;
            fklblFactionDescriptionLocale.Visible = false;
            // 
            // txtFactionDescription
            // 
            txtFactionDescription.Location = new System.Drawing.Point(370, 25);
            txtFactionDescription.Name = "txtFactionDescription";
            txtFactionDescription.Size = new System.Drawing.Size(359, 23);
            txtFactionDescription.TabIndex = 38;
            txtFactionDescription.TextChanged += txtFactionDescription_TextChanged;
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new System.Drawing.Point(370, 7);
            label22.Name = "label22";
            label22.Size = new System.Drawing.Size(67, 15);
            label22.TabIndex = 37;
            label22.Text = "Description";
            // 
            // fklblFactionNameLocale
            // 
            fklblFactionNameLocale.Enabled = false;
            fklblFactionNameLocale.FlatAppearance.BorderSize = 0;
            fklblFactionNameLocale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblFactionNameLocale.Image = (System.Drawing.Image)resources.GetObject("fklblFactionNameLocale.Image");
            fklblFactionNameLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblFactionNameLocale.Location = new System.Drawing.Point(8, 54);
            fklblFactionNameLocale.Name = "fklblFactionNameLocale";
            fklblFactionNameLocale.Size = new System.Drawing.Size(331, 42);
            fklblFactionNameLocale.TabIndex = 36;
            fklblFactionNameLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblFactionNameLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblFactionNameLocale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblFactionNameLocale.UseVisualStyleBackColor = true;
            fklblFactionNameLocale.Visible = false;
            // 
            // txtFactionName
            // 
            txtFactionName.Location = new System.Drawing.Point(8, 25);
            txtFactionName.Name = "txtFactionName";
            txtFactionName.Size = new System.Drawing.Size(359, 23);
            txtFactionName.TabIndex = 35;
            txtFactionName.TextChanged += txtFactionName_TextChanged;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new System.Drawing.Point(8, 7);
            label21.Name = "label21";
            label21.Size = new System.Drawing.Size(39, 15);
            label21.TabIndex = 34;
            label21.Text = "Name";
            // 
            // FactionTab
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Controls.Add(lbEnemies);
            Controls.Add(label26);
            Controls.Add(btnEnemiesToNeutrals);
            Controls.Add(btnEnemyToNeutral);
            Controls.Add(btnNeutralsToEnemies);
            Controls.Add(btnNeutralToEnemy);
            Controls.Add(lbNeutrals);
            Controls.Add(label25);
            Controls.Add(btnNeutralsToAllies);
            Controls.Add(btnNeutralToAlly);
            Controls.Add(btnAlliesToNeutrals);
            Controls.Add(btnAllyToNeutral);
            Controls.Add(lbAllies);
            Controls.Add(label24);
            Controls.Add(label23);
            Controls.Add(fklblFactionDescriptionLocale);
            Controls.Add(txtFactionDescription);
            Controls.Add(label22);
            Controls.Add(fklblFactionNameLocale);
            Controls.Add(txtFactionName);
            Controls.Add(label21);
            Name = "FactionTab";
            Size = new System.Drawing.Size(732, 328);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListBox lbEnemies;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Button btnEnemiesToNeutrals;
        private System.Windows.Forms.Button btnEnemyToNeutral;
        private System.Windows.Forms.Button btnNeutralsToEnemies;
        private System.Windows.Forms.Button btnNeutralToEnemy;
        private System.Windows.Forms.ListBox lbNeutrals;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Button btnNeutralsToAllies;
        private System.Windows.Forms.Button btnNeutralToAlly;
        private System.Windows.Forms.Button btnAlliesToNeutrals;
        private System.Windows.Forms.Button btnAllyToNeutral;
        private System.Windows.Forms.ListBox lbAllies;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button fklblFactionDescriptionLocale;
        private System.Windows.Forms.TextBox txtFactionDescription;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button fklblFactionNameLocale;
        private System.Windows.Forms.TextBox txtFactionName;
        private System.Windows.Forms.Label label21;
    }
}
