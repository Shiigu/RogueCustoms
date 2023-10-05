using System.Drawing;
using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.HelperForms
{
    partial class frmNPCGeneration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNPCGeneration));
            lblFloorGroupTitle = new Label();
            dgvNPCTable = new DataGridView();
            ClassId = new DataGridViewComboBoxColumn();
            MinLevel = new DataGridViewTextBoxColumn();
            MaxLevel = new DataGridViewTextBoxColumn();
            SimultaneousMaxForKindInFloor = new DataGridViewTextBoxColumn();
            OverallMaxForKindInFloor = new DataGridViewTextBoxColumn();
            ChanceToPick = new DataGridViewTextBoxColumn();
            CanSpawnOnFirstTurn = new DataGridViewCheckBoxColumn();
            CanSpawnAfterFirstTurn = new DataGridViewCheckBoxColumn();
            nudTurnsPerNPCGeneration = new NumericUpDown();
            label16 = new Label();
            nudSimultaneousMaxNPCs = new NumericUpDown();
            label15 = new Label();
            nudMinNPCSpawnsAtStart = new NumericUpDown();
            label14 = new Label();
            btnSave = new Button();
            btnCancel = new Button();
            fklblNoNPCSpawnsWarning = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvNPCTable).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudTurnsPerNPCGeneration).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudSimultaneousMaxNPCs).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMinNPCSpawnsAtStart).BeginInit();
            SuspendLayout();
            // 
            // lblFloorGroupTitle
            // 
            lblFloorGroupTitle.AutoSize = true;
            lblFloorGroupTitle.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            lblFloorGroupTitle.Location = new Point(3, 9);
            lblFloorGroupTitle.Name = "lblFloorGroupTitle";
            lblFloorGroupTitle.Size = new Size(243, 25);
            lblFloorGroupTitle.TabIndex = 0;
            lblFloorGroupTitle.Text = "For Floor Level(s) X (to Y):";
            // 
            // dgvNPCTable
            // 
            dgvNPCTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvNPCTable.Columns.AddRange(new DataGridViewColumn[] { ClassId, MinLevel, MaxLevel, SimultaneousMaxForKindInFloor, OverallMaxForKindInFloor, ChanceToPick, CanSpawnOnFirstTurn, CanSpawnAfterFirstTurn });
            dgvNPCTable.Location = new Point(3, 46);
            dgvNPCTable.Name = "dgvNPCTable";
            dgvNPCTable.RowTemplate.Height = 25;
            dgvNPCTable.Size = new Size(664, 209);
            dgvNPCTable.TabIndex = 1;
            dgvNPCTable.Leave += dgvNPCTable_Leave;
            // 
            // ClassId
            // 
            ClassId.DataPropertyName = "ClassId";
            ClassId.HeaderText = "NPC Class";
            ClassId.Name = "ClassId";
            // 
            // MinLevel
            // 
            MinLevel.DataPropertyName = "MinLevel";
            MinLevel.HeaderText = "Minimum Level";
            MinLevel.Name = "MinLevel";
            // 
            // MaxLevel
            // 
            MaxLevel.DataPropertyName = "MaxLevel";
            MaxLevel.HeaderText = "Maximum Level";
            MaxLevel.Name = "MaxLevel";
            // 
            // SimultaneousMaxForKindInFloor
            // 
            SimultaneousMaxForKindInFloor.DataPropertyName = "SimultaneousMaxForKindInFloor";
            SimultaneousMaxForKindInFloor.HeaderText = "Simultaneous Limit";
            SimultaneousMaxForKindInFloor.Name = "SimultaneousMaxForKindInFloor";
            SimultaneousMaxForKindInFloor.ToolTipText = "Won't spawn this NPC if there are at least this amount alive";
            // 
            // OverallMaxForKindInFloor
            // 
            OverallMaxForKindInFloor.DataPropertyName = "OverallMaxForKindInFloor";
            OverallMaxForKindInFloor.HeaderText = "Overall Limit";
            OverallMaxForKindInFloor.Name = "OverallMaxForKindInFloor";
            OverallMaxForKindInFloor.ToolTipText = "Won't spawn this NPC if at least this amount had been spawned already";
            // 
            // ChanceToPick
            // 
            ChanceToPick.DataPropertyName = "ChanceToPick";
            ChanceToPick.HeaderText = "Chance to Pick";
            ChanceToPick.Name = "ChanceToPick";
            ChanceToPick.ToolTipText = "Must be a number between 1 and 100";
            // 
            // CanSpawnOnFirstTurn
            // 
            CanSpawnOnFirstTurn.DataPropertyName = "CanSpawnOnFirstTurn";
            CanSpawnOnFirstTurn.HeaderText = "Spawns on First Turn?";
            CanSpawnOnFirstTurn.Name = "CanSpawnOnFirstTurn";
            // 
            // CanSpawnAfterFirstTurn
            // 
            CanSpawnAfterFirstTurn.DataPropertyName = "CanSpawnAfterFirstTurn";
            CanSpawnAfterFirstTurn.HeaderText = "Spawns after First Turn?";
            CanSpawnAfterFirstTurn.Name = "CanSpawnAfterFirstTurn";
            // 
            // nudTurnsPerNPCGeneration
            // 
            nudTurnsPerNPCGeneration.Location = new Point(149, 316);
            nudTurnsPerNPCGeneration.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            nudTurnsPerNPCGeneration.Name = "nudTurnsPerNPCGeneration";
            nudTurnsPerNPCGeneration.Size = new Size(58, 23);
            nudTurnsPerNPCGeneration.TabIndex = 31;
            nudTurnsPerNPCGeneration.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(3, 318);
            label16.Name = "label16";
            label16.Size = new Size(248, 15);
            label16.TabIndex = 30;
            label16.Text = "Try to spawn an NPC every                      turn(s)";
            // 
            // nudSimultaneousMaxNPCs
            // 
            nudSimultaneousMaxNPCs.Location = new Point(210, 290);
            nudSimultaneousMaxNPCs.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            nudSimultaneousMaxNPCs.Name = "nudSimultaneousMaxNPCs";
            nudSimultaneousMaxNPCs.Size = new Size(58, 23);
            nudSimultaneousMaxNPCs.TabIndex = 29;
            nudSimultaneousMaxNPCs.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nudSimultaneousMaxNPCs.ValueChanged += nudSimultaneousMaxNPCs_ValueChanged;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(3, 292);
            label15.Name = "label15";
            label15.Size = new Size(204, 15);
            label15.TabIndex = 28;
            label15.Text = "Don't spawn NPCs if there are at least";
            // 
            // nudMinNPCSpawnsAtStart
            // 
            nudMinNPCSpawnsAtStart.Location = new Point(237, 261);
            nudMinNPCSpawnsAtStart.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            nudMinNPCSpawnsAtStart.Name = "nudMinNPCSpawnsAtStart";
            nudMinNPCSpawnsAtStart.Size = new Size(60, 23);
            nudMinNPCSpawnsAtStart.TabIndex = 27;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(3, 264);
            label14.Name = "label14";
            label14.Size = new Size(338, 15);
            label14.TabIndex = 26;
            label14.Text = "When the Floor is generated, spawn at least                      NPC(s)";
            // 
            // btnSave
            // 
            btnSave.Location = new Point(194, 351);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(161, 23);
            btnSave.TabIndex = 32;
            btnSave.Text = "Save NPC generation data";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(361, 351);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 33;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // fklblNoNPCSpawnsWarning
            // 
            fklblNoNPCSpawnsWarning.Enabled = false;
            fklblNoNPCSpawnsWarning.FlatAppearance.BorderSize = 0;
            fklblNoNPCSpawnsWarning.FlatStyle = FlatStyle.Flat;
            fklblNoNPCSpawnsWarning.Image = (Image)resources.GetObject("fklblNoNPCSpawnsWarning.Image");
            fklblNoNPCSpawnsWarning.ImageAlign = ContentAlignment.TopLeft;
            fklblNoNPCSpawnsWarning.Location = new Point(297, 285);
            fklblNoNPCSpawnsWarning.Name = "fklblNoNPCSpawnsWarning";
            fklblNoNPCSpawnsWarning.Size = new Size(384, 32);
            fklblNoNPCSpawnsWarning.TabIndex = 34;
            fklblNoNPCSpawnsWarning.Text = "Warning: This will prevent any NPCs in the table from spawning!";
            fklblNoNPCSpawnsWarning.TextAlign = ContentAlignment.MiddleLeft;
            fklblNoNPCSpawnsWarning.TextImageRelation = TextImageRelation.ImageBeforeText;
            fklblNoNPCSpawnsWarning.UseVisualStyleBackColor = true;
            fklblNoNPCSpawnsWarning.Visible = false;
            // 
            // frmNPCGeneration
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(670, 386);
            Controls.Add(fklblNoNPCSpawnsWarning);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(nudTurnsPerNPCGeneration);
            Controls.Add(label16);
            Controls.Add(nudSimultaneousMaxNPCs);
            Controls.Add(label15);
            Controls.Add(nudMinNPCSpawnsAtStart);
            Controls.Add(label14);
            Controls.Add(dgvNPCTable);
            Controls.Add(lblFloorGroupTitle);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmNPCGeneration";
            StartPosition = FormStartPosition.CenterParent;
            Text = "NPC Generation for Floor Group";
            ((System.ComponentModel.ISupportInitialize)dgvNPCTable).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudTurnsPerNPCGeneration).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudSimultaneousMaxNPCs).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMinNPCSpawnsAtStart).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblFloorGroupTitle;
        private DataGridView dgvNPCTable;
        private NumericUpDown nudTurnsPerNPCGeneration;
        private Label label16;
        private NumericUpDown nudSimultaneousMaxNPCs;
        private Label label15;
        private NumericUpDown nudMinNPCSpawnsAtStart;
        private Label label14;
        private Button btnSave;
        private Button btnCancel;
        private DataGridViewComboBoxColumn ClassId;
        private DataGridViewTextBoxColumn MinLevel;
        private DataGridViewTextBoxColumn MaxLevel;
        private DataGridViewTextBoxColumn SimultaneousMaxForKindInFloor;
        private DataGridViewTextBoxColumn OverallMaxForKindInFloor;
        private DataGridViewTextBoxColumn ChanceToPick;
        private DataGridViewCheckBoxColumn CanSpawnOnFirstTurn;
        private DataGridViewCheckBoxColumn CanSpawnAfterFirstTurn;
        private Button fklblNoNPCSpawnsWarning;
    }
}