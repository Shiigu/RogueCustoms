using System.Drawing;
using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.HelperForms
{
    partial class ComboInputBox
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
            lblPrompt = new Label();
            btnOk = new Button();
            btnCancel = new Button();
            cmbPrompt = new ComboBox();
            SuspendLayout();
            // 
            // lblPrompt
            // 
            lblPrompt.Location = new Point(12, 9);
            lblPrompt.Name = "lblPrompt";
            lblPrompt.Size = new Size(267, 62);
            lblPrompt.TabIndex = 0;
            lblPrompt.Text = "ComboInputBoxPrompt";
            // 
            // btnOk
            // 
            btnOk.Location = new Point(283, 9);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(75, 23);
            btnOk.TabIndex = 2;
            btnOk.Text = "OK";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(283, 38);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // cmbPrompt
            // 
            cmbPrompt.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPrompt.FormattingEnabled = true;
            cmbPrompt.Location = new Point(12, 76);
            cmbPrompt.Name = "cmbPrompt";
            cmbPrompt.Size = new Size(265, 23);
            cmbPrompt.TabIndex = 4;
            // 
            // ComboInputBox
            // 
            AcceptButton = btnOk;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(370, 111);
            Controls.Add(cmbPrompt);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            Controls.Add(lblPrompt);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ComboInputBox";
            StartPosition = FormStartPosition.CenterParent;
            Text = "ComboInputBox";
            ResumeLayout(false);
        }

        #endregion

        private Label lblPrompt;
        private Button btnOk;
        private Button btnCancel;
        private ComboBox cmbPrompt;
    }
}