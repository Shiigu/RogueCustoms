namespace RogueCustomsDungeonValidator
{
    partial class frmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            label1 = new System.Windows.Forms.Label();
            txtDungeonPath = new System.Windows.Forms.TextBox();
            btnBrowse = new System.Windows.Forms.Button();
            btnValidateDungeon = new System.Windows.Forms.Button();
            btnExit = new System.Windows.Forms.Button();
            ofdDungeon = new System.Windows.Forms.OpenFileDialog();
            lblNoAnalysis = new System.Windows.Forms.Label();
            tvResults = new System.Windows.Forms.TreeView();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(117, 15);
            label1.TabIndex = 0;
            label1.Text = "Dungeon to validate:";
            // 
            // txtDungeonPath
            // 
            txtDungeonPath.Location = new System.Drawing.Point(12, 27);
            txtDungeonPath.Name = "txtDungeonPath";
            txtDungeonPath.ReadOnly = true;
            txtDungeonPath.Size = new System.Drawing.Size(489, 23);
            txtDungeonPath.TabIndex = 1;
            txtDungeonPath.TextChanged += txtDungeonPath_TextChanged;
            // 
            // btnBrowse
            // 
            btnBrowse.Location = new System.Drawing.Point(507, 26);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new System.Drawing.Size(75, 23);
            btnBrowse.TabIndex = 2;
            btnBrowse.Text = "Browse...";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // btnValidateDungeon
            // 
            btnValidateDungeon.Enabled = false;
            btnValidateDungeon.Location = new System.Drawing.Point(270, 61);
            btnValidateDungeon.Name = "btnValidateDungeon";
            btnValidateDungeon.Size = new System.Drawing.Size(75, 23);
            btnValidateDungeon.TabIndex = 3;
            btnValidateDungeon.Text = "Validate";
            btnValidateDungeon.UseVisualStyleBackColor = true;
            btnValidateDungeon.Click += btnValidateDungeon_Click;
            // 
            // btnExit
            // 
            btnExit.Location = new System.Drawing.Point(270, 492);
            btnExit.Name = "btnExit";
            btnExit.Size = new System.Drawing.Size(75, 23);
            btnExit.TabIndex = 5;
            btnExit.Text = "Exit";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // ofdDungeon
            // 
            ofdDungeon.Filter = "Dungeon JSON|*.json";
            ofdDungeon.Title = "Select a Dungeon JSON file";
            // 
            // lblNoAnalysis
            // 
            lblNoAnalysis.AutoSize = true;
            lblNoAnalysis.Location = new System.Drawing.Point(12, 87);
            lblNoAnalysis.Name = "lblNoAnalysis";
            lblNoAnalysis.Size = new System.Drawing.Size(364, 15);
            lblNoAnalysis.TabIndex = 6;
            lblNoAnalysis.Text = "Select a Dungeon.JSON file and click on Validate to have it analyzed.";
            // 
            // tvResults
            // 
            tvResults.Location = new System.Drawing.Point(12, 87);
            tvResults.Name = "tvResults";
            tvResults.Size = new System.Drawing.Size(568, 399);
            tvResults.TabIndex = 7;
            tvResults.Visible = false;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(592, 520);
            Controls.Add(tvResults);
            Controls.Add(lblNoAnalysis);
            Controls.Add(btnExit);
            Controls.Add(btnValidateDungeon);
            Controls.Add(btnBrowse);
            Controls.Add(txtDungeonPath);
            Controls.Add(label1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "frmMain";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Dungeon Validator";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDungeonPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnValidateDungeon;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.OpenFileDialog ofdDungeon;
        private System.Windows.Forms.Label lblNoAnalysis;
        private System.Windows.Forms.TreeView tvResults;
    }
}
