namespace RogueCustomsDungeonEditor.HelperForms
{
    partial class InputBox
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
            txtPromptText = new TextBox();
            btnOk = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // lblPrompt
            // 
            lblPrompt.Location = new Point(12, 9);
            lblPrompt.Name = "lblPrompt";
            lblPrompt.Size = new Size(267, 62);
            lblPrompt.TabIndex = 0;
            lblPrompt.Text = "InputBoxPrompt";
            // 
            // txtPromptText
            // 
            txtPromptText.Location = new Point(11, 81);
            txtPromptText.Name = "txtPromptText";
            txtPromptText.Size = new Size(347, 23);
            txtPromptText.TabIndex = 1;
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
            // InputBox
            // 
            AcceptButton = btnOk;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(370, 112);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            Controls.Add(txtPromptText);
            Controls.Add(lblPrompt);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "InputBox";
            StartPosition = FormStartPosition.CenterParent;
            Text = "InputBox";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblPrompt;
        private TextBox txtPromptText;
        private Button btnOk;
        private Button btnCancel;
    }
}