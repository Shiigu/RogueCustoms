using System.Drawing;
using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.HelperForms
{
    partial class frmObjectGeneration
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
            nudMaxInFloor = new NumericUpDown();
            btnCancel = new Button();
            btnSave = new Button();
            nudMinInFloor = new NumericUpDown();
            label14 = new Label();
            dgvObjectTable = new DataGridView();
            ClassId = new DataGridViewComboBoxColumn();
            SimultaneousMaxForKindInFloor = new DataGridViewTextBoxColumn();
            ChanceToPick = new DataGridViewTextBoxColumn();
            lblFloorGroupTitle = new Label();
            ((System.ComponentModel.ISupportInitialize)nudMaxInFloor).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMinInFloor).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvObjectTable).BeginInit();
            SuspendLayout();
            // 
            // nudMaxInFloor
            // 
            nudMaxInFloor.Location = new Point(350, 260);
            nudMaxInFloor.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            nudMaxInFloor.Name = "nudMaxInFloor";
            nudMaxInFloor.Size = new Size(60, 23);
            nudMaxInFloor.TabIndex = 41;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(236, 285);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(174, 23);
            btnCancel.TabIndex = 40;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(56, 285);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(174, 23);
            btnSave.TabIndex = 39;
            btnSave.Text = "Save X generation data";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // nudMinInFloor
            // 
            nudMinInFloor.Location = new Point(257, 260);
            nudMinInFloor.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            nudMinInFloor.Name = "nudMinInFloor";
            nudMinInFloor.Size = new Size(60, 23);
            nudMinInFloor.TabIndex = 38;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(12, 263);
            label14.Name = "label14";
            label14.Size = new Size(335, 15);
            label14.TabIndex = 37;
            label14.Text = "When the Floor is generated, spawn between                        and";
            // 
            // dgvObjectTable
            // 
            dgvObjectTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvObjectTable.Columns.AddRange(new DataGridViewColumn[] { ClassId, SimultaneousMaxForKindInFloor, ChanceToPick });
            dgvObjectTable.Location = new Point(12, 45);
            dgvObjectTable.Name = "dgvObjectTable";
            dgvObjectTable.RowTemplate.Height = 25;
            dgvObjectTable.Size = new Size(435, 209);
            dgvObjectTable.TabIndex = 36;
            dgvObjectTable.Leave += dgvObjectTable_Leave;
            // 
            // ClassId
            // 
            ClassId.DataPropertyName = "ClassId";
            ClassId.HeaderText = "X Class";
            ClassId.Name = "ClassId";
            // 
            // SimultaneousMaxForKindInFloor
            // 
            SimultaneousMaxForKindInFloor.DataPropertyName = "SimultaneousMaxForKindInFloor";
            SimultaneousMaxForKindInFloor.HeaderText = "Maximum";
            SimultaneousMaxForKindInFloor.Name = "SimultaneousMaxForKindInFloor";
            SimultaneousMaxForKindInFloor.ToolTipText = "Will not try to spawn more than this amount of this kind";
            // 
            // ChanceToPick
            // 
            ChanceToPick.DataPropertyName = "ChanceToPick";
            ChanceToPick.HeaderText = "Chance to Pick";
            ChanceToPick.Name = "ChanceToPick";
            ChanceToPick.ToolTipText = "Must be a number between 1 and 100";
            // 
            // lblFloorGroupTitle
            // 
            lblFloorGroupTitle.AutoSize = true;
            lblFloorGroupTitle.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            lblFloorGroupTitle.Location = new Point(12, 8);
            lblFloorGroupTitle.Name = "lblFloorGroupTitle";
            lblFloorGroupTitle.Size = new Size(243, 25);
            lblFloorGroupTitle.TabIndex = 35;
            lblFloorGroupTitle.Text = "For Floor Level(s) X (to Y):";
            // 
            // frmObjectGeneration
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(458, 318);
            Controls.Add(lblFloorGroupTitle);
            Controls.Add(nudMaxInFloor);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(nudMinInFloor);
            Controls.Add(label14);
            Controls.Add(dgvObjectTable);
            Name = "frmObjectGeneration";
            StartPosition = FormStartPosition.CenterParent;
            Text = "frmObjectGeneration";
            ((System.ComponentModel.ISupportInitialize)nudMaxInFloor).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMinInFloor).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvObjectTable).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private NumericUpDown nudMaxInFloor;
        private Button btnCancel;
        private Button btnSave;
        private NumericUpDown nudMinInFloor;
        private Label label14;
        private DataGridView dgvObjectTable;
        private Label lblFloorGroupTitle;
        private DataGridViewComboBoxColumn ClassId;
        private DataGridViewTextBoxColumn SimultaneousMaxForKindInFloor;
        private DataGridViewTextBoxColumn ChanceToPick;
    }
}