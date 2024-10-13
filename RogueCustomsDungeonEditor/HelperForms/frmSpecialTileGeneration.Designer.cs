using System.Drawing;
using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.HelperForms
{
    partial class frmSpecialTileGeneration
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
            btnCancel = new Button();
            btnSave = new Button();
            dgvObjectTable = new DataGridView();
            lblFloorGroupTitle = new Label();
            TileTypeId = new DataGridViewComboBoxColumn();
            MinSpecialTileGenerations = new DataGridViewTextBoxColumn();
            MaxSpecialTileGenerations = new DataGridViewTextBoxColumn();
            GeneratorType = new DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvObjectTable).BeginInit();
            SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(260, 260);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(199, 23);
            btnCancel.TabIndex = 40;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(55, 260);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(199, 23);
            btnSave.TabIndex = 39;
            btnSave.Text = "Save Special Tile generation data";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // dgvObjectTable
            // 
            dgvObjectTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvObjectTable.Columns.AddRange(new DataGridViewColumn[] { TileTypeId, MinSpecialTileGenerations, MaxSpecialTileGenerations, GeneratorType });
            dgvObjectTable.Location = new Point(12, 45);
            dgvObjectTable.Name = "dgvObjectTable";
            dgvObjectTable.RowTemplate.Height = 25;
            dgvObjectTable.Size = new Size(499, 209);
            dgvObjectTable.TabIndex = 36;
            dgvObjectTable.Leave += dgvObjectTable_Leave;
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
            // TileTypeId
            // 
            TileTypeId.DataPropertyName = "TileTypeId";
            TileTypeId.Frozen = true;
            TileTypeId.HeaderText = "Special Tile Id";
            TileTypeId.Name = "TileTypeId";
            // 
            // MinSpecialTileGenerations
            // 
            MinSpecialTileGenerations.DataPropertyName = "MinSpecialTileGenerations";
            MinSpecialTileGenerations.Frozen = true;
            MinSpecialTileGenerations.HeaderText = "Minimum";
            MinSpecialTileGenerations.Name = "MinSpecialTileGenerations";
            // 
            // MaxSpecialTileGenerations
            // 
            MaxSpecialTileGenerations.DataPropertyName = "MaxSpecialTileGenerations";
            MaxSpecialTileGenerations.Frozen = true;
            MaxSpecialTileGenerations.HeaderText = "Maximum";
            MaxSpecialTileGenerations.Name = "MaxSpecialTileGenerations";
            MaxSpecialTileGenerations.ToolTipText = "Will not try to spawn more than this amount of this kind";
            // 
            // GeneratorType
            // 
            GeneratorType.DataPropertyName = "GeneratorType";
            GeneratorType.Frozen = true;
            GeneratorType.HeaderText = "Generation Type";
            GeneratorType.Name = "GeneratorType";
            // 
            // frmSpecialTileGeneration
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(516, 293);
            Controls.Add(lblFloorGroupTitle);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(dgvObjectTable);
            Name = "frmSpecialTileGeneration";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Special Tile Generation Data";
            ((System.ComponentModel.ISupportInitialize)dgvObjectTable).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btnCancel;
        private Button btnSave;
        private DataGridView dgvObjectTable;
        private Label lblFloorGroupTitle;
        private DataGridViewComboBoxColumn TileTypeId;
        private DataGridViewTextBoxColumn MinSpecialTileGenerations;
        private DataGridViewTextBoxColumn MaxSpecialTileGenerations;
        private DataGridViewComboBoxColumn GeneratorType;
    }
}