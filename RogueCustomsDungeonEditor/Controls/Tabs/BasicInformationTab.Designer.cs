namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    partial class BasicInformationTab
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(BasicInformationTab));
            cmbDefaultLocale = new System.Windows.Forms.ComboBox();
            label8 = new System.Windows.Forms.Label();
            fklblEndingMessageLocale = new System.Windows.Forms.Button();
            fklblWelcomeMessageLocale = new System.Windows.Forms.Button();
            fklblAuthorLocale = new System.Windows.Forms.Button();
            fklblDungeonNameLocale = new System.Windows.Forms.Button();
            txtEndingMessage = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            txtWelcomeMessage = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            txtAuthor = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            txtDungeonName = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // cmbDefaultLocale
            // 
            cmbDefaultLocale.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbDefaultLocale.FormattingEnabled = true;
            cmbDefaultLocale.Location = new System.Drawing.Point(189, 231);
            cmbDefaultLocale.Name = "cmbDefaultLocale";
            cmbDefaultLocale.Size = new System.Drawing.Size(81, 23);
            cmbDefaultLocale.TabIndex = 31;
            cmbDefaultLocale.SelectedIndexChanged += cmbDefaultLocale_SelectedIndexChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(12, 226);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(171, 30);
            label8.TabIndex = 30;
            label8.Text = "If the game has a language this\r\ndungeon lacks, use this locale:";
            // 
            // fklblEndingMessageLocale
            // 
            fklblEndingMessageLocale.Enabled = false;
            fklblEndingMessageLocale.FlatAppearance.BorderSize = 0;
            fklblEndingMessageLocale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblEndingMessageLocale.Image = (System.Drawing.Image)resources.GetObject("fklblEndingMessageLocale.Image");
            fklblEndingMessageLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblEndingMessageLocale.Location = new System.Drawing.Point(402, 307);
            fklblEndingMessageLocale.Name = "fklblEndingMessageLocale";
            fklblEndingMessageLocale.Size = new System.Drawing.Size(331, 42);
            fklblEndingMessageLocale.TabIndex = 29;
            fklblEndingMessageLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblEndingMessageLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblEndingMessageLocale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblEndingMessageLocale.UseVisualStyleBackColor = true;
            fklblEndingMessageLocale.Visible = false;
            // 
            // fklblWelcomeMessageLocale
            // 
            fklblWelcomeMessageLocale.Enabled = false;
            fklblWelcomeMessageLocale.FlatAppearance.BorderSize = 0;
            fklblWelcomeMessageLocale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblWelcomeMessageLocale.Image = (System.Drawing.Image)resources.GetObject("fklblWelcomeMessageLocale.Image");
            fklblWelcomeMessageLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblWelcomeMessageLocale.Location = new System.Drawing.Point(404, 135);
            fklblWelcomeMessageLocale.Name = "fklblWelcomeMessageLocale";
            fklblWelcomeMessageLocale.Size = new System.Drawing.Size(331, 42);
            fklblWelcomeMessageLocale.TabIndex = 28;
            fklblWelcomeMessageLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblWelcomeMessageLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblWelcomeMessageLocale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblWelcomeMessageLocale.UseVisualStyleBackColor = true;
            fklblWelcomeMessageLocale.Visible = false;
            // 
            // fklblAuthorLocale
            // 
            fklblAuthorLocale.Enabled = false;
            fklblAuthorLocale.FlatAppearance.BorderSize = 0;
            fklblAuthorLocale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblAuthorLocale.Image = (System.Drawing.Image)resources.GetObject("fklblAuthorLocale.Image");
            fklblAuthorLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblAuthorLocale.Location = new System.Drawing.Point(12, 171);
            fklblAuthorLocale.Name = "fklblAuthorLocale";
            fklblAuthorLocale.Size = new System.Drawing.Size(331, 42);
            fklblAuthorLocale.TabIndex = 27;
            fklblAuthorLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblAuthorLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblAuthorLocale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblAuthorLocale.UseVisualStyleBackColor = true;
            fklblAuthorLocale.Visible = false;
            // 
            // fklblDungeonNameLocale
            // 
            fklblDungeonNameLocale.Enabled = false;
            fklblDungeonNameLocale.FlatAppearance.BorderSize = 0;
            fklblDungeonNameLocale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblDungeonNameLocale.Image = (System.Drawing.Image)resources.GetObject("fklblDungeonNameLocale.Image");
            fklblDungeonNameLocale.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            fklblDungeonNameLocale.Location = new System.Drawing.Point(12, 58);
            fklblDungeonNameLocale.Name = "fklblDungeonNameLocale";
            fklblDungeonNameLocale.Size = new System.Drawing.Size(331, 42);
            fklblDungeonNameLocale.TabIndex = 26;
            fklblDungeonNameLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblDungeonNameLocale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblDungeonNameLocale.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblDungeonNameLocale.UseVisualStyleBackColor = true;
            fklblDungeonNameLocale.Visible = false;
            // 
            // txtEndingMessage
            // 
            txtEndingMessage.Location = new System.Drawing.Point(404, 198);
            txtEndingMessage.Multiline = true;
            txtEndingMessage.Name = "txtEndingMessage";
            txtEndingMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtEndingMessage.Size = new System.Drawing.Size(313, 103);
            txtEndingMessage.TabIndex = 25;
            txtEndingMessage.TextChanged += txtEndingMessage_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(404, 180);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(93, 15);
            label4.TabIndex = 24;
            label4.Text = "Ending Message";
            // 
            // txtWelcomeMessage
            // 
            txtWelcomeMessage.Location = new System.Drawing.Point(404, 29);
            txtWelcomeMessage.Multiline = true;
            txtWelcomeMessage.Name = "txtWelcomeMessage";
            txtWelcomeMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtWelcomeMessage.Size = new System.Drawing.Size(313, 103);
            txtWelcomeMessage.TabIndex = 23;
            txtWelcomeMessage.TextChanged += txtWelcomeMessage_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(404, 11);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(106, 15);
            label3.TabIndex = 22;
            label3.Text = "Welcome Message";
            // 
            // txtAuthor
            // 
            txtAuthor.Location = new System.Drawing.Point(12, 137);
            txtAuthor.Name = "txtAuthor";
            txtAuthor.Size = new System.Drawing.Size(359, 23);
            txtAuthor.TabIndex = 21;
            txtAuthor.TextChanged += txtAuthor_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 119);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(44, 15);
            label2.TabIndex = 20;
            label2.Text = "Author";
            // 
            // txtDungeonName
            // 
            txtDungeonName.Location = new System.Drawing.Point(12, 29);
            txtDungeonName.Name = "txtDungeonName";
            txtDungeonName.Size = new System.Drawing.Size(359, 23);
            txtDungeonName.TabIndex = 19;
            txtDungeonName.TextChanged += txtDungeonName_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 11);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(39, 15);
            label1.TabIndex = 18;
            label1.Text = "Name";
            // 
            // BasicInformationTab
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Controls.Add(cmbDefaultLocale);
            Controls.Add(label8);
            Controls.Add(fklblEndingMessageLocale);
            Controls.Add(fklblWelcomeMessageLocale);
            Controls.Add(fklblAuthorLocale);
            Controls.Add(fklblDungeonNameLocale);
            Controls.Add(txtEndingMessage);
            Controls.Add(label4);
            Controls.Add(txtWelcomeMessage);
            Controls.Add(label3);
            Controls.Add(txtAuthor);
            Controls.Add(label2);
            Controls.Add(txtDungeonName);
            Controls.Add(label1);
            Name = "BasicInformationTab";
            Size = new System.Drawing.Size(738, 352);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ComboBox cmbDefaultLocale;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button fklblEndingMessageLocale;
        private System.Windows.Forms.Button fklblWelcomeMessageLocale;
        private System.Windows.Forms.Button fklblAuthorLocale;
        private System.Windows.Forms.Button fklblDungeonNameLocale;
        private System.Windows.Forms.TextBox txtEndingMessage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtWelcomeMessage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAuthor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDungeonName;
        private System.Windows.Forms.Label label1;
    }
}
