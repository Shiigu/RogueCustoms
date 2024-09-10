using System.Drawing;
using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.HelperForms
{
    partial class frmFloorLayout
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
            lblFloorGroupTitle = new Label();
            label1 = new Label();
            nudMinHeight = new NumericUpDown();
            label2 = new Label();
            nudMinWidth = new NumericUpDown();
            label3 = new Label();
            label4 = new Label();
            btnSave = new Button();
            btnCancel = new Button();
            label5 = new Label();
            label6 = new Label();
            nudMaxWidth = new NumericUpDown();
            label7 = new Label();
            nudMaxHeight = new NumericUpDown();
            label8 = new Label();
            tlpRoomDisposition = new TableLayoutPanel();
            lblFloorSize = new Label();
            label9 = new Label();
            cmbFloorTemplate = new ComboBox();
            label10 = new Label();
            btnSetToMinimumSize = new Button();
            btnSetToMaximumSize = new Button();
            ((System.ComponentModel.ISupportInitialize)nudMinHeight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMinWidth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMaxWidth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMaxHeight).BeginInit();
            SuspendLayout();
            // 
            // lblFloorGroupTitle
            // 
            lblFloorGroupTitle.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            lblFloorGroupTitle.Location = new Point(12, 6);
            lblFloorGroupTitle.Name = "lblFloorGroupTitle";
            lblFloorGroupTitle.Size = new Size(393, 25);
            lblFloorGroupTitle.TabIndex = 36;
            lblFloorGroupTitle.Text = "For Floor Level(s) X (to Y):";
            lblFloorGroupTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(38, 311);
            label1.Name = "label1";
            label1.Size = new Size(121, 15);
            label1.TabIndex = 38;
            label1.Text = "Minimum Room Size:";
            // 
            // nudMinHeight
            // 
            nudMinHeight.Location = new Point(229, 306);
            nudMinHeight.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            nudMinHeight.Name = "nudMinHeight";
            nudMinHeight.Size = new Size(39, 23);
            nudMinHeight.TabIndex = 39;
            nudMinHeight.Value = new decimal(new int[] { 5, 0, 0, 0 });
            nudMinHeight.Validating += nudMinHeight_Validating;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(210, 308);
            label2.Name = "label2";
            label2.Size = new Size(13, 15);
            label2.TabIndex = 40;
            label2.Text = "x";
            // 
            // nudMinWidth
            // 
            nudMinWidth.Location = new Point(165, 306);
            nudMinWidth.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            nudMinWidth.Name = "nudMinWidth";
            nudMinWidth.Size = new Size(39, 23);
            nudMinWidth.TabIndex = 41;
            nudMinWidth.Value = new decimal(new int[] { 5, 0, 0, 0 });
            nudMinWidth.Validating += nudMinWidth_Validating;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(173, 332);
            label3.Name = "label3";
            label3.Size = new Size(18, 15);
            label3.TabIndex = 42;
            label3.Text = "W";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(238, 332);
            label4.Name = "label4";
            label4.Size = new Size(16, 15);
            label4.TabIndex = 43;
            label4.Text = "H";
            // 
            // btnSave
            // 
            btnSave.Enabled = false;
            btnSave.Location = new Point(148, 409);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 44;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(229, 409);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 45;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(238, 384);
            label5.Name = "label5";
            label5.Size = new Size(16, 15);
            label5.TabIndex = 52;
            label5.Text = "H";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(173, 384);
            label6.Name = "label6";
            label6.Size = new Size(18, 15);
            label6.TabIndex = 51;
            label6.Text = "W";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // nudMaxWidth
            // 
            nudMaxWidth.Location = new Point(165, 358);
            nudMaxWidth.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            nudMaxWidth.Name = "nudMaxWidth";
            nudMaxWidth.Size = new Size(39, 23);
            nudMaxWidth.TabIndex = 50;
            nudMaxWidth.Value = new decimal(new int[] { 5, 0, 0, 0 });
            nudMaxWidth.Validating += nudMaxWidth_Validating;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(210, 360);
            label7.Name = "label7";
            label7.Size = new Size(13, 15);
            label7.TabIndex = 49;
            label7.Text = "x";
            // 
            // nudMaxHeight
            // 
            nudMaxHeight.Location = new Point(229, 358);
            nudMaxHeight.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            nudMaxHeight.Name = "nudMaxHeight";
            nudMaxHeight.Size = new Size(39, 23);
            nudMaxHeight.TabIndex = 48;
            nudMaxHeight.Value = new decimal(new int[] { 5, 0, 0, 0 });
            nudMaxHeight.Validating += nudMaxHeight_Validating;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(38, 363);
            label8.Name = "label8";
            label8.Size = new Size(123, 15);
            label8.TabIndex = 47;
            label8.Text = "Maximum Room Size:";
            // 
            // tlpRoomDisposition
            // 
            tlpRoomDisposition.BackColor = SystemColors.Control;
            tlpRoomDisposition.ColumnCount = 7;
            tlpRoomDisposition.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 24F));
            tlpRoomDisposition.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 24F));
            tlpRoomDisposition.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 24F));
            tlpRoomDisposition.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 24F));
            tlpRoomDisposition.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 24F));
            tlpRoomDisposition.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 24F));
            tlpRoomDisposition.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 24F));
            tlpRoomDisposition.Location = new Point(134, 37);
            tlpRoomDisposition.Name = "tlpRoomDisposition";
            tlpRoomDisposition.RowCount = 7;
            tlpRoomDisposition.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tlpRoomDisposition.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tlpRoomDisposition.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tlpRoomDisposition.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tlpRoomDisposition.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tlpRoomDisposition.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tlpRoomDisposition.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tlpRoomDisposition.Size = new Size(168, 168);
            tlpRoomDisposition.TabIndex = 53;
            // 
            // lblFloorSize
            // 
            lblFloorSize.Location = new Point(79, 273);
            lblFloorSize.Name = "lblFloorSize";
            lblFloorSize.Size = new Size(288, 17);
            lblFloorSize.TabIndex = 54;
            lblFloorSize.Text = "Floor Size (Rows x Columns): 4x4";
            lblFloorSize.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(73, 245);
            label9.Name = "label9";
            label9.Size = new Size(118, 15);
            label9.TabIndex = 55;
            label9.Text = "Use a Floor template:";
            // 
            // cmbFloorTemplate
            // 
            cmbFloorTemplate.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFloorTemplate.FormattingEnabled = true;
            cmbFloorTemplate.Location = new Point(195, 242);
            cmbFloorTemplate.Name = "cmbFloorTemplate";
            cmbFloorTemplate.Size = new Size(177, 23);
            cmbFloorTemplate.TabIndex = 56;
            cmbFloorTemplate.SelectedIndexChanged += cmbFloorTemplate_SelectedIndexChanged;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(89, 214);
            label10.Name = "label10";
            label10.Size = new Size(260, 15);
            label10.TabIndex = 57;
            label10.Text = "Left or Right click on a square to change its type";
            // 
            // btnSetToMinimumSize
            // 
            btnSetToMinimumSize.Location = new Point(274, 306);
            btnSetToMinimumSize.Name = "btnSetToMinimumSize";
            btnSetToMinimumSize.Size = new Size(131, 23);
            btnSetToMinimumSize.TabIndex = 58;
            btnSetToMinimumSize.Text = "Set to Minimum Size";
            btnSetToMinimumSize.UseVisualStyleBackColor = true;
            btnSetToMinimumSize.Click += btnSetToMinimumSize_Click;
            // 
            // btnSetToMaximumSize
            // 
            btnSetToMaximumSize.Location = new Point(274, 358);
            btnSetToMaximumSize.Name = "btnSetToMaximumSize";
            btnSetToMaximumSize.Size = new Size(131, 23);
            btnSetToMaximumSize.TabIndex = 59;
            btnSetToMaximumSize.Text = "Set to Maximum Size";
            btnSetToMaximumSize.UseVisualStyleBackColor = true;
            btnSetToMaximumSize.Click += btnSetToMaximumSize_Click;
            // 
            // frmFloorLayout
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(417, 438);
            Controls.Add(btnSetToMaximumSize);
            Controls.Add(btnSetToMinimumSize);
            Controls.Add(label10);
            Controls.Add(cmbFloorTemplate);
            Controls.Add(label9);
            Controls.Add(lblFloorSize);
            Controls.Add(label5);
            Controls.Add(label6);
            Controls.Add(nudMaxWidth);
            Controls.Add(label7);
            Controls.Add(nudMaxHeight);
            Controls.Add(label8);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(nudMinWidth);
            Controls.Add(label2);
            Controls.Add(nudMinHeight);
            Controls.Add(label1);
            Controls.Add(lblFloorGroupTitle);
            Controls.Add(tlpRoomDisposition);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmFloorLayout";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Floor Generator Algorithm";
            ((System.ComponentModel.ISupportInitialize)nudMinHeight).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMinWidth).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMaxWidth).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMaxHeight).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblFloorGroupTitle;
        private Label label1;
        private NumericUpDown nudMinHeight;
        private Label label2;
        private NumericUpDown nudMinWidth;
        private Label label3;
        private Label label4;
        private Button btnSave;
        private Button btnCancel;
        private Label label5;
        private Label label6;
        private NumericUpDown nudMaxWidth;
        private Label label7;
        private NumericUpDown nudMaxHeight;
        private Label label8;
        private TableLayoutPanel tlpRoomDisposition;
        private Label lblFloorSize;
        private Label label9;
        private ComboBox cmbFloorTemplate;
        private Label label10;
        private Button btnSetToMinimumSize;
        private Button btnSetToMaximumSize;
    }
}