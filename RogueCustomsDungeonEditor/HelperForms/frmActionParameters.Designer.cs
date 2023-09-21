namespace RogueCustomsDungeonEditor.HelperForms
{
    partial class frmActionParameters
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblDisplayName = new Label();
            tlpParameters = new TableLayoutPanel();
            llblWiki = new LinkLabel();
            btnSave = new Button();
            btnCancel = new Button();
            lblRequired = new Label();
            SuspendLayout();
            // 
            // lblDisplayName
            // 
            lblDisplayName.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            lblDisplayName.Location = new Point(11, 7);
            lblDisplayName.Name = "lblDisplayName";
            lblDisplayName.Size = new Size(296, 63);
            lblDisplayName.TabIndex = 0;
            lblDisplayName.Text = "DisplayName:";
            // 
            // tlpParameters
            // 
            tlpParameters.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpParameters.ColumnCount = 2;
            tlpParameters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tlpParameters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tlpParameters.Location = new Point(11, 112);
            tlpParameters.Name = "tlpParameters";
            tlpParameters.RowCount = 1;
            tlpParameters.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpParameters.Size = new Size(320, 290);
            tlpParameters.TabIndex = 1;
            tlpParameters.SizeChanged += tlpParameters_SizeChanged;
            // 
            // llblWiki
            // 
            llblWiki.AutoSize = true;
            llblWiki.Location = new Point(12, 414);
            llblWiki.Name = "llblWiki";
            llblWiki.Size = new Size(286, 15);
            llblWiki.TabIndex = 3;
            llblWiki.TabStop = true;
            llblWiki.Text = "Click here to get more information about parameters";
            llblWiki.LinkClicked += llblWiki_LinkClicked;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(69, 441);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 4;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(184, 441);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // lblRequired
            // 
            lblRequired.Location = new Point(10, 76);
            lblRequired.Name = "lblRequired";
            lblRequired.Size = new Size(315, 33);
            lblRequired.TabIndex = 6;
            lblRequired.Text = "Fields with an * require a value.\r\nFields with an ^ require a value on at least one of them.";
            // 
            // frmActionParameters
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(337, 474);
            Controls.Add(lblRequired);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(llblWiki);
            Controls.Add(tlpParameters);
            Controls.Add(lblDisplayName);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmActionParameters";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Set the Step Parameters:";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblDisplayName;
        private TableLayoutPanel tlpParameters;
        private LinkLabel llblWiki;
        private Button btnSave;
        private Button btnCancel;
        private Label lblRequired;
    }
}