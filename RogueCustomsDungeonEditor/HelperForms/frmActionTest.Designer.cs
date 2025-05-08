namespace RogueCustomsDungeonEditor.HelperForms
{
    partial class frmActionTest
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(frmActionTest));
            lblTitle = new System.Windows.Forms.Label();
            lblSourceTitle = new System.Windows.Forms.Label();
            crsSource = new Controls.ConsoleRepresentationSelector();
            crsTarget = new Controls.ConsoleRepresentationSelector();
            lblTargetTitle = new System.Windows.Forms.Label();
            issSource = new Controls.ItemStatsSheet();
            issTarget = new Controls.ItemStatsSheet();
            lblSourceStatuses = new System.Windows.Forms.Label();
            clbSourceStatuses = new System.Windows.Forms.CheckedListBox();
            clbTargetStatuses = new System.Windows.Forms.CheckedListBox();
            lblTargetStatuses = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            btnTestAction = new System.Windows.Forms.Button();
            lblStatsInfo = new System.Windows.Forms.Label();
            btnClearLog = new System.Windows.Forms.Button();
            btnClose = new System.Windows.Forms.Button();
            txtMessageLog = new System.Windows.Forms.RichTextBox();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            lblTitle.Location = new System.Drawing.Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(1076, 26);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Test X Action";
            lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSourceTitle
            // 
            lblSourceTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblSourceTitle.Location = new System.Drawing.Point(12, 49);
            lblSourceTitle.Name = "lblSourceTitle";
            lblSourceTitle.Size = new System.Drawing.Size(210, 26);
            lblSourceTitle.TabIndex = 2;
            lblSourceTitle.Text = "Source";
            lblSourceTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // crsSource
            // 
            crsSource.Location = new System.Drawing.Point(12, 78);
            crsSource.Name = "crsSource";
            crsSource.Size = new System.Drawing.Size(210, 82);
            crsSource.TabIndex = 3;
            // 
            // crsTarget
            // 
            crsTarget.Location = new System.Drawing.Point(339, 78);
            crsTarget.Name = "crsTarget";
            crsTarget.Size = new System.Drawing.Size(210, 82);
            crsTarget.TabIndex = 5;
            // 
            // lblTargetTitle
            // 
            lblTargetTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblTargetTitle.Location = new System.Drawing.Point(339, 49);
            lblTargetTitle.Name = "lblTargetTitle";
            lblTargetTitle.Size = new System.Drawing.Size(210, 26);
            lblTargetTitle.TabIndex = 4;
            lblTargetTitle.Text = "Target";
            lblTargetTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // issSource
            // 
            issSource.Location = new System.Drawing.Point(19, 169);
            issSource.Name = "issSource";
            issSource.Size = new System.Drawing.Size(203, 141);
            issSource.TabIndex = 8;
            // 
            // issTarget
            // 
            issTarget.Location = new System.Drawing.Point(346, 169);
            issTarget.Name = "issTarget";
            issTarget.Size = new System.Drawing.Size(203, 141);
            issTarget.TabIndex = 11;
            // 
            // lblSourceStatuses
            // 
            lblSourceStatuses.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            lblSourceStatuses.Location = new System.Drawing.Point(19, 362);
            lblSourceStatuses.Name = "lblSourceStatuses";
            lblSourceStatuses.Size = new System.Drawing.Size(210, 26);
            lblSourceStatuses.TabIndex = 13;
            lblSourceStatuses.Text = "Altered Statuses";
            lblSourceStatuses.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // clbSourceStatuses
            // 
            clbSourceStatuses.FormattingEnabled = true;
            clbSourceStatuses.Location = new System.Drawing.Point(63, 391);
            clbSourceStatuses.Name = "clbSourceStatuses";
            clbSourceStatuses.Size = new System.Drawing.Size(120, 94);
            clbSourceStatuses.TabIndex = 14;
            // 
            // clbTargetStatuses
            // 
            clbTargetStatuses.FormattingEnabled = true;
            clbTargetStatuses.Location = new System.Drawing.Point(390, 391);
            clbTargetStatuses.Name = "clbTargetStatuses";
            clbTargetStatuses.Size = new System.Drawing.Size(120, 94);
            clbTargetStatuses.TabIndex = 16;
            // 
            // lblTargetStatuses
            // 
            lblTargetStatuses.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            lblTargetStatuses.Location = new System.Drawing.Point(346, 362);
            lblTargetStatuses.Name = "lblTargetStatuses";
            lblTargetStatuses.Size = new System.Drawing.Size(210, 26);
            lblTargetStatuses.TabIndex = 15;
            lblTargetStatuses.Text = "Altered Statuses";
            lblTargetStatuses.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            label4.Location = new System.Drawing.Point(594, 49);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(470, 26);
            label4.TabIndex = 17;
            label4.Text = "Message Log";
            label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnTestAction
            // 
            btnTestAction.Location = new System.Drawing.Point(654, 462);
            btnTestAction.Name = "btnTestAction";
            btnTestAction.Size = new System.Drawing.Size(123, 23);
            btnTestAction.TabIndex = 19;
            btnTestAction.Text = "Test Action";
            btnTestAction.UseVisualStyleBackColor = true;
            btnTestAction.Click += btnTestAction_Click;
            // 
            // lblStatsInfo
            // 
            lblStatsInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblStatsInfo.Location = new System.Drawing.Point(19, 313);
            lblStatsInfo.Name = "lblStatsInfo";
            lblStatsInfo.Size = new System.Drawing.Size(537, 49);
            lblStatsInfo.TabIndex = 20;
            lblStatsInfo.Text = resources.GetString("lblStatsInfo.Text");
            lblStatsInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClearLog
            // 
            btnClearLog.Location = new System.Drawing.Point(912, 462);
            btnClearLog.Name = "btnClearLog";
            btnClearLog.Size = new System.Drawing.Size(123, 23);
            btnClearLog.TabIndex = 21;
            btnClearLog.Text = "Clear Message Log";
            btnClearLog.UseVisualStyleBackColor = true;
            btnClearLog.Click += btnClearLog_Click;
            // 
            // btnClose
            // 
            btnClose.Location = new System.Drawing.Point(783, 462);
            btnClose.Name = "btnClose";
            btnClose.Size = new System.Drawing.Size(123, 23);
            btnClose.TabIndex = 22;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // txtMessageLog
            // 
            txtMessageLog.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            txtMessageLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtMessageLog.Location = new System.Drawing.Point(618, 78);
            txtMessageLog.Name = "txtMessageLog";
            txtMessageLog.ReadOnly = true;
            txtMessageLog.Size = new System.Drawing.Size(446, 378);
            txtMessageLog.TabIndex = 23;
            txtMessageLog.Text = "";
            // 
            // frmActionTest
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1076, 494);
            Controls.Add(txtMessageLog);
            Controls.Add(btnClose);
            Controls.Add(btnClearLog);
            Controls.Add(lblStatsInfo);
            Controls.Add(btnTestAction);
            Controls.Add(label4);
            Controls.Add(clbTargetStatuses);
            Controls.Add(lblTargetStatuses);
            Controls.Add(clbSourceStatuses);
            Controls.Add(lblSourceStatuses);
            Controls.Add(issTarget);
            Controls.Add(issSource);
            Controls.Add(crsTarget);
            Controls.Add(lblTargetTitle);
            Controls.Add(crsSource);
            Controls.Add(lblSourceTitle);
            Controls.Add(lblTitle);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Name = "frmActionTest";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Action Tester";
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSourceTitle;
        private Controls.ConsoleRepresentationSelector crsSource;
        private Controls.ConsoleRepresentationSelector crsTarget;
        private System.Windows.Forms.Label lblTargetTitle;
        private Controls.ItemStatsSheet issSource;
        private Controls.ItemStatsSheet issTarget;
        private System.Windows.Forms.Label lblSourceStatuses;
        private System.Windows.Forms.CheckedListBox clbSourceStatuses;
        private System.Windows.Forms.CheckedListBox clbTargetStatuses;
        private System.Windows.Forms.Label lblTargetStatuses;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnTestAction;
        private System.Windows.Forms.Label lblStatsInfo;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.RichTextBox txtMessageLog;
    }
}