namespace RogueCustomsDungeonEditor.HelperForms
{
    partial class frmFloorKeys
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFloorKeys));
            lblFloorGroupTitle = new System.Windows.Forms.Label();
            btnAddKeyType = new System.Windows.Forms.Button();
            btnRemoveKeyType = new System.Windows.Forms.Button();
            panel1 = new System.Windows.Forms.Panel();
            label12 = new System.Windows.Forms.Label();
            label11 = new System.Windows.Forms.Label();
            label10 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            btnCancel = new System.Windows.Forms.Button();
            btnSave = new System.Windows.Forms.Button();
            nudMaxLockedDoorsPercentage = new System.Windows.Forms.NumericUpDown();
            label18 = new System.Windows.Forms.Label();
            nudLockedDoorOdds = new System.Windows.Forms.NumericUpDown();
            label5 = new System.Windows.Forms.Label();
            nudKeyInEnemyInventoryOdds = new System.Windows.Forms.NumericUpDown();
            label1 = new System.Windows.Forms.Label();
            flpKeySettings = new System.Windows.Forms.FlowLayoutPanel();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMaxLockedDoorsPercentage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudLockedDoorOdds).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudKeyInEnemyInventoryOdds).BeginInit();
            SuspendLayout();
            // 
            // lblFloorGroupTitle
            // 
            lblFloorGroupTitle.AutoSize = true;
            lblFloorGroupTitle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblFloorGroupTitle.Location = new System.Drawing.Point(12, 9);
            lblFloorGroupTitle.Name = "lblFloorGroupTitle";
            lblFloorGroupTitle.Size = new System.Drawing.Size(243, 25);
            lblFloorGroupTitle.TabIndex = 1;
            lblFloorGroupTitle.Text = "For Floor Level(s) X (to Y):";
            // 
            // btnAddKeyType
            // 
            btnAddKeyType.Location = new System.Drawing.Point(261, 244);
            btnAddKeyType.Name = "btnAddKeyType";
            btnAddKeyType.Size = new System.Drawing.Size(134, 23);
            btnAddKeyType.TabIndex = 3;
            btnAddKeyType.Text = "Add a new Key Type";
            btnAddKeyType.UseVisualStyleBackColor = true;
            btnAddKeyType.Click += btnAddKeyType_Click;
            // 
            // btnRemoveKeyType
            // 
            btnRemoveKeyType.Enabled = false;
            btnRemoveKeyType.Location = new System.Drawing.Point(438, 244);
            btnRemoveKeyType.Name = "btnRemoveKeyType";
            btnRemoveKeyType.Size = new System.Drawing.Size(134, 23);
            btnRemoveKeyType.TabIndex = 4;
            btnRemoveKeyType.Text = "Remove Key Type";
            btnRemoveKeyType.UseVisualStyleBackColor = true;
            btnRemoveKeyType.Click += btnRemoveKeyType_Click;
            // 
            // panel1
            // 
            panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel1.Controls.Add(label12);
            panel1.Controls.Add(label11);
            panel1.Controls.Add(label10);
            panel1.Controls.Add(label9);
            panel1.Controls.Add(pictureBox1);
            panel1.Location = new System.Drawing.Point(13, 362);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(814, 157);
            panel1.TabIndex = 26;
            // 
            // label12
            // 
            label12.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label12.Location = new System.Drawing.Point(3, 105);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(780, 45);
            label12.TabIndex = 4;
            label12.Text = "The order of the Key Types does not indicate priority. They will be chosen at random where applicable.\r\n\r\nAll of a Room's exits will be locked with the same Door Type.";
            label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            label11.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label11.Location = new System.Drawing.Point(3, 49);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(310, 54);
            label11.TabIndex = 3;
            label11.Text = "- It is the Room with the Stairs.\r\n- It is a Room that has Items within.\r\n- It is connected to a Room that has a Locked Door.";
            label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            label10.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label10.Location = new System.Drawing.Point(3, 30);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(267, 19);
            label10.TabIndex = 2;
            label10.Text = "It must fulfill one of the following conditions:";
            label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            label9.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label9.Location = new System.Drawing.Point(32, 3);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(461, 24);
            label9.TabIndex = 1;
            label9.Text = "What makes a Room candidate for a Locked Door?";
            label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (System.Drawing.Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new System.Drawing.Point(3, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(24, 24);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(437, 525);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(134, 23);
            btnCancel.TabIndex = 35;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new System.Drawing.Point(260, 525);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(134, 23);
            btnSave.TabIndex = 34;
            btnSave.Text = "Save Key/Door data";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // nudMaxLockedDoorsPercentage
            // 
            nudMaxLockedDoorsPercentage.Location = new System.Drawing.Point(57, 333);
            nudMaxLockedDoorsPercentage.Name = "nudMaxLockedDoorsPercentage";
            nudMaxLockedDoorsPercentage.Size = new System.Drawing.Size(40, 23);
            nudMaxLockedDoorsPercentage.TabIndex = 74;
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new System.Drawing.Point(22, 335);
            label18.Name = "label18";
            label18.Size = new System.Drawing.Size(488, 15);
            label18.TabIndex = 73;
            label18.Text = "Up to               % of the candidate Rooms in the Floor (rounded up) can have a Locked Door.";
            // 
            // nudLockedDoorOdds
            // 
            nudLockedDoorOdds.Location = new System.Drawing.Point(157, 275);
            nudLockedDoorOdds.Name = "nudLockedDoorOdds";
            nudLockedDoorOdds.Size = new System.Drawing.Size(40, 23);
            nudLockedDoorOdds.TabIndex = 72;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(23, 277);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(362, 15);
            label5.TabIndex = 71;
            label5.Text = "A candidate Room has a               % chance of having Locked Doors.";
            // 
            // nudKeyInEnemyInventoryOdds
            // 
            nudKeyInEnemyInventoryOdds.Location = new System.Drawing.Point(92, 304);
            nudKeyInEnemyInventoryOdds.Name = "nudKeyInEnemyInventoryOdds";
            nudKeyInEnemyInventoryOdds.Size = new System.Drawing.Size(40, 23);
            nudKeyInEnemyInventoryOdds.TabIndex = 76;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(23, 306);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(795, 15);
            label1.TabIndex = 75;
            label1.Text = "Keys have a                % chance of spawning in the Inventory of a visible Enemy NPC that spawned on the first turn (keys don't occupy inventory slots).";
            // 
            // flpKeySettings
            // 
            flpKeySettings.AutoScroll = true;
            flpKeySettings.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            flpKeySettings.Location = new System.Drawing.Point(13, 47);
            flpKeySettings.Margin = new System.Windows.Forms.Padding(0);
            flpKeySettings.Name = "flpKeySettings";
            flpKeySettings.Size = new System.Drawing.Size(816, 194);
            flpKeySettings.TabIndex = 78;
            flpKeySettings.WrapContents = false;
            flpKeySettings.ControlAdded += flpKeySettings_ControlAdded_1;
            flpKeySettings.ControlRemoved += flpKeySettings_ControlRemoved_1;
            // 
            // frmFloorKeys
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(842, 560);
            Controls.Add(flpKeySettings);
            Controls.Add(nudKeyInEnemyInventoryOdds);
            Controls.Add(label1);
            Controls.Add(nudMaxLockedDoorsPercentage);
            Controls.Add(label18);
            Controls.Add(nudLockedDoorOdds);
            Controls.Add(label5);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(panel1);
            Controls.Add(btnRemoveKeyType);
            Controls.Add(btnAddKeyType);
            Controls.Add(lblFloorGroupTitle);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmFloorKeys";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Key and Door Generator for Floor Group";
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMaxLockedDoorsPercentage).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudLockedDoorOdds).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudKeyInEnemyInventoryOdds).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblFloorGroupTitle;
        private System.Windows.Forms.Button btnAddKeyType;
        private System.Windows.Forms.Button btnRemoveKeyType;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.NumericUpDown nudMaxLockedDoorsPercentage;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.NumericUpDown nudLockedDoorOdds;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudKeyInEnemyInventoryOdds;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel flpKeySettings;
    }
}