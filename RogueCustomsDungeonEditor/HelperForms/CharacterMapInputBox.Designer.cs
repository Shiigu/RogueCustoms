using System.Drawing;
using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.HelperForms
{
    partial class CharacterMapInputBox
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
            label1 = new Label();
            panel1 = new Panel();
            tlpCharacters = new TableLayoutPanel();
            btnSave = new Button();
            btnCancel = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(181, 25);
            label1.TabIndex = 0;
            label1.Text = "Pick one character:";
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(tlpCharacters);
            panel1.Location = new Point(12, 37);
            panel1.Name = "panel1";
            panel1.Size = new Size(360, 300);
            panel1.TabIndex = 2;
            // 
            // tlpCharacters
            // 
            tlpCharacters.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tlpCharacters.AutoSize = true;
            tlpCharacters.BackColor = Color.White;
            tlpCharacters.ColumnCount = 10;
            tlpCharacters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tlpCharacters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tlpCharacters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tlpCharacters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tlpCharacters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tlpCharacters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tlpCharacters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tlpCharacters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tlpCharacters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tlpCharacters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tlpCharacters.Location = new Point(0, 0);
            tlpCharacters.Margin = new Padding(0);
            tlpCharacters.Name = "tlpCharacters";
            tlpCharacters.RowCount = 1;
            tlpCharacters.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpCharacters.Size = new Size(360, 300);
            tlpCharacters.TabIndex = 2;
            // 
            // btnSave
            // 
            btnSave.Enabled = false;
            btnSave.Location = new Point(109, 343);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 3;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(217, 343);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // CharacterMapInputBox
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(383, 374);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(panel1);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "CharacterMapInputBox";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Select a character";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Panel panel1;
        private Button btnSave;
        private Button btnCancel;
        private TableLayoutPanel tlpCharacters;
    }
}