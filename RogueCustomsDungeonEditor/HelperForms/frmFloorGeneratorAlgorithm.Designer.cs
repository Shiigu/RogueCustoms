namespace RogueCustomsDungeonEditor.HelperForms
{
    partial class frmFloorGeneratorAlgorithm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFloorGeneratorAlgorithm));
            lblFloorGroupTitle = new Label();
            lvFloorAlgorithms = new ListView();
            label1 = new Label();
            nudAlgorithmRows = new NumericUpDown();
            label2 = new Label();
            nudAlgorithmColumns = new NumericUpDown();
            label3 = new Label();
            label4 = new Label();
            btnSave = new Button();
            btnCancel = new Button();
            fklblRedundantAlgorithm = new Button();
            ((System.ComponentModel.ISupportInitialize)nudAlgorithmRows).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudAlgorithmColumns).BeginInit();
            SuspendLayout();
            // 
            // lblFloorGroupTitle
            // 
            lblFloorGroupTitle.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            lblFloorGroupTitle.Location = new Point(12, 9);
            lblFloorGroupTitle.Name = "lblFloorGroupTitle";
            lblFloorGroupTitle.Size = new Size(401, 25);
            lblFloorGroupTitle.TabIndex = 36;
            lblFloorGroupTitle.Text = "For Floor Level(s) X (to Y):";
            lblFloorGroupTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lvFloorAlgorithms
            // 
            lvFloorAlgorithms.Location = new Point(12, 37);
            lvFloorAlgorithms.MultiSelect = false;
            lvFloorAlgorithms.Name = "lvFloorAlgorithms";
            lvFloorAlgorithms.ShowItemToolTips = true;
            lvFloorAlgorithms.Size = new Size(401, 159);
            lvFloorAlgorithms.TabIndex = 37;
            lvFloorAlgorithms.UseCompatibleStateImageBehavior = false;
            lvFloorAlgorithms.SelectedIndexChanged += lvFloorAlgorithms_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(79, 204);
            label1.Name = "label1";
            label1.Size = new Size(108, 15);
            label1.TabIndex = 38;
            label1.Text = "Amount of Rooms:";
            // 
            // nudAlgorithmRows
            // 
            nudAlgorithmRows.Enabled = false;
            nudAlgorithmRows.Location = new Point(257, 202);
            nudAlgorithmRows.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudAlgorithmRows.Name = "nudAlgorithmRows";
            nudAlgorithmRows.Size = new Size(39, 23);
            nudAlgorithmRows.TabIndex = 39;
            nudAlgorithmRows.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nudAlgorithmRows.ValueChanged += nudAlgorithmRows_ValueChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(238, 204);
            label2.Name = "label2";
            label2.Size = new Size(13, 15);
            label2.TabIndex = 40;
            label2.Text = "x";
            // 
            // nudAlgorithmColumns
            // 
            nudAlgorithmColumns.Enabled = false;
            nudAlgorithmColumns.Location = new Point(193, 202);
            nudAlgorithmColumns.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudAlgorithmColumns.Name = "nudAlgorithmColumns";
            nudAlgorithmColumns.Size = new Size(39, 23);
            nudAlgorithmColumns.TabIndex = 41;
            nudAlgorithmColumns.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nudAlgorithmColumns.ValueChanged += nudAlgorithmColumns_ValueChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(185, 228);
            label3.Name = "label3";
            label3.Size = new Size(55, 15);
            label3.TabIndex = 42;
            label3.Text = "Columns";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(258, 228);
            label4.Name = "label4";
            label4.Size = new Size(35, 15);
            label4.TabIndex = 43;
            label4.Text = "Rows";
            // 
            // btnSave
            // 
            btnSave.Enabled = false;
            btnSave.Location = new Point(130, 294);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 44;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(211, 294);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 45;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // fklblRedundantAlgorithm
            // 
            fklblRedundantAlgorithm.Enabled = false;
            fklblRedundantAlgorithm.FlatAppearance.BorderSize = 0;
            fklblRedundantAlgorithm.FlatStyle = FlatStyle.Flat;
            fklblRedundantAlgorithm.Image = (Image)resources.GetObject("fklblRedundantAlgorithm.Image");
            fklblRedundantAlgorithm.ImageAlign = ContentAlignment.TopLeft;
            fklblRedundantAlgorithm.Location = new Point(51, 246);
            fklblRedundantAlgorithm.Name = "fklblRedundantAlgorithm";
            fklblRedundantAlgorithm.Size = new Size(327, 42);
            fklblRedundantAlgorithm.TabIndex = 46;
            fklblRedundantAlgorithm.Text = "This Layout is already present in the Floor Group.\r\nThis will reduce the odds to select other Layouts.";
            fklblRedundantAlgorithm.TextAlign = ContentAlignment.MiddleLeft;
            fklblRedundantAlgorithm.TextImageRelation = TextImageRelation.ImageBeforeText;
            fklblRedundantAlgorithm.UseVisualStyleBackColor = true;
            fklblRedundantAlgorithm.Visible = false;
            // 
            // frmFloorGeneratorAlgorithm
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(417, 327);
            Controls.Add(fklblRedundantAlgorithm);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(nudAlgorithmColumns);
            Controls.Add(label2);
            Controls.Add(nudAlgorithmRows);
            Controls.Add(label1);
            Controls.Add(lvFloorAlgorithms);
            Controls.Add(lblFloorGroupTitle);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmFloorGeneratorAlgorithm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Floor Generator Algorithm";
            ((System.ComponentModel.ISupportInitialize)nudAlgorithmRows).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudAlgorithmColumns).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblFloorGroupTitle;
        private ListView lvFloorAlgorithms;
        private Label label1;
        private NumericUpDown nudAlgorithmRows;
        private Label label2;
        private NumericUpDown nudAlgorithmColumns;
        private Label label3;
        private Label label4;
        private Button btnSave;
        private Button btnCancel;
        private Button fklblRedundantAlgorithm;
    }
}