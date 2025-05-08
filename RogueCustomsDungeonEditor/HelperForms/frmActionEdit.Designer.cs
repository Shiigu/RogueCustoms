using System.Drawing;
using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.HelperForms
{
    partial class frmActionEdit
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(frmActionEdit));
            lblTitle = new Label();
            tvEffectSequence = new TreeView();
            btnCancel = new Button();
            gbSelectionCriteria = new GroupBox();
            rbTile = new RadioButton();
            rbEntity = new RadioButton();
            pnlCharacterTargets = new Panel();
            chkSelf = new CheckBox();
            chkEnemies = new CheckBox();
            chkAllies = new CheckBox();
            nudMPCost = new NumericUpDown();
            label8 = new Label();
            lblNoCooldown = new Label();
            lblInfiniteUse = new Label();
            nudMaximumUses = new NumericUpDown();
            label5 = new Label();
            nudInitialCooldown = new NumericUpDown();
            label4 = new Label();
            nudCooldown = new NumericUpDown();
            label3 = new Label();
            nudMaxRange = new NumericUpDown();
            nudMinRange = new NumericUpDown();
            label2 = new Label();
            label1 = new Label();
            btnSave = new Button();
            btnEdit = new Button();
            btnNewThen = new Button();
            btnNewOnSuccessFailure = new Button();
            btnRemove = new Button();
            btnSaveAs = new Button();
            fklblActionDescriptionLocale = new Button();
            txtActionDescription = new TextBox();
            label6 = new Label();
            fklblConditionWarning = new Button();
            txtActionCondition = new TextBox();
            label7 = new Label();
            panel1 = new Panel();
            lblTarget = new Label();
            lblSource = new Label();
            lblThis = new Label();
            label12 = new Label();
            label11 = new Label();
            label10 = new Label();
            label9 = new Label();
            pictureBox1 = new PictureBox();
            chkFinishesTurn = new CheckBox();
            btnCopyStep = new Button();
            btnPasteStep = new Button();
            fklblAIConditionWarning = new Button();
            txtActionAICondition = new TextBox();
            label13 = new Label();
            fklblActionNameLocale = new Button();
            txtActionName = new TextBox();
            label14 = new Label();
            btnTestAction = new Button();
            gbSelectionCriteria.SuspendLayout();
            pnlCharacterTargets.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudMPCost).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMaximumUses).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudInitialCooldown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudCooldown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMaxRange).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMinRange).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            lblTitle.Location = new Point(4, 2);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(939, 26);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "X Action";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tvEffectSequence
            // 
            tvEffectSequence.DrawMode = TreeViewDrawMode.OwnerDrawText;
            tvEffectSequence.Font = new Font("Segoe UI", 9F);
            tvEffectSequence.FullRowSelect = true;
            tvEffectSequence.HideSelection = false;
            tvEffectSequence.Location = new Point(3, 31);
            tvEffectSequence.Name = "tvEffectSequence";
            tvEffectSequence.ShowNodeToolTips = true;
            tvEffectSequence.Size = new Size(467, 248);
            tvEffectSequence.TabIndex = 1;
            tvEffectSequence.DrawNode += tvEffectSequence_DrawNode;
            tvEffectSequence.AfterSelect += tvEffectSequence_AfterSelect;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(544, 591);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 24);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // gbSelectionCriteria
            // 
            gbSelectionCriteria.Controls.Add(rbTile);
            gbSelectionCriteria.Controls.Add(rbEntity);
            gbSelectionCriteria.Controls.Add(pnlCharacterTargets);
            gbSelectionCriteria.Controls.Add(nudMPCost);
            gbSelectionCriteria.Controls.Add(label8);
            gbSelectionCriteria.Controls.Add(lblNoCooldown);
            gbSelectionCriteria.Controls.Add(lblInfiniteUse);
            gbSelectionCriteria.Controls.Add(nudMaximumUses);
            gbSelectionCriteria.Controls.Add(label5);
            gbSelectionCriteria.Controls.Add(nudInitialCooldown);
            gbSelectionCriteria.Controls.Add(label4);
            gbSelectionCriteria.Controls.Add(nudCooldown);
            gbSelectionCriteria.Controls.Add(label3);
            gbSelectionCriteria.Controls.Add(nudMaxRange);
            gbSelectionCriteria.Controls.Add(nudMinRange);
            gbSelectionCriteria.Controls.Add(label2);
            gbSelectionCriteria.Controls.Add(label1);
            gbSelectionCriteria.Location = new Point(476, 30);
            gbSelectionCriteria.Name = "gbSelectionCriteria";
            gbSelectionCriteria.Size = new Size(467, 249);
            gbSelectionCriteria.TabIndex = 3;
            gbSelectionCriteria.TabStop = false;
            gbSelectionCriteria.Text = "Usage Criteria";
            // 
            // rbTile
            // 
            rbTile.AutoSize = true;
            rbTile.Location = new Point(375, 22);
            rbTile.Name = "rbTile";
            rbTile.Size = new Size(52, 19);
            rbTile.TabIndex = 30;
            rbTile.Text = "A tile";
            rbTile.UseVisualStyleBackColor = true;
            rbTile.CheckedChanged += rbTile_CheckedChanged;
            // 
            // rbEntity
            // 
            rbEntity.AutoSize = true;
            rbEntity.Checked = true;
            rbEntity.Location = new Point(112, 24);
            rbEntity.Name = "rbEntity";
            rbEntity.Size = new Size(73, 19);
            rbEntity.TabIndex = 29;
            rbEntity.TabStop = true;
            rbEntity.Text = "An entity";
            rbEntity.UseVisualStyleBackColor = true;
            rbEntity.CheckedChanged += rbEntity_CheckedChanged;
            // 
            // pnlCharacterTargets
            // 
            pnlCharacterTargets.AutoSize = true;
            pnlCharacterTargets.Controls.Add(chkSelf);
            pnlCharacterTargets.Controls.Add(chkEnemies);
            pnlCharacterTargets.Controls.Add(chkAllies);
            pnlCharacterTargets.Location = new Point(5, 44);
            pnlCharacterTargets.Name = "pnlCharacterTargets";
            pnlCharacterTargets.Size = new Size(462, 32);
            pnlCharacterTargets.TabIndex = 28;
            // 
            // chkSelf
            // 
            chkSelf.AutoSize = true;
            chkSelf.Location = new Point(414, 8);
            chkSelf.Name = "chkSelf";
            chkSelf.Size = new Size(45, 19);
            chkSelf.TabIndex = 6;
            chkSelf.Text = "Self";
            chkSelf.UseVisualStyleBackColor = true;
            // 
            // chkEnemies
            // 
            chkEnemies.AutoSize = true;
            chkEnemies.Location = new Point(190, 8);
            chkEnemies.Name = "chkEnemies";
            chkEnemies.Size = new Size(70, 19);
            chkEnemies.TabIndex = 5;
            chkEnemies.Text = "Enemies";
            chkEnemies.UseVisualStyleBackColor = true;
            // 
            // chkAllies
            // 
            chkAllies.AutoSize = true;
            chkAllies.Location = new Point(4, 8);
            chkAllies.Name = "chkAllies";
            chkAllies.Size = new Size(54, 19);
            chkAllies.TabIndex = 4;
            chkAllies.Text = "Allies";
            chkAllies.UseVisualStyleBackColor = true;
            // 
            // nudMPCost
            // 
            nudMPCost.Location = new Point(43, 217);
            nudMPCost.Maximum = new decimal(new int[] { 99, 0, 0, 0 });
            nudMPCost.Name = "nudMPCost";
            nudMPCost.Size = new Size(41, 23);
            nudMPCost.TabIndex = 16;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(7, 219);
            label8.Name = "label8";
            label8.Size = new Size(137, 15);
            label8.TabIndex = 15;
            label8.Text = "Costs                MP to use";
            // 
            // lblNoCooldown
            // 
            lblNoCooldown.AutoSize = true;
            lblNoCooldown.Location = new Point(352, 119);
            lblNoCooldown.Name = "lblNoCooldown";
            lblNoCooldown.Size = new Size(108, 15);
            lblNoCooldown.TabIndex = 14;
            lblNoCooldown.Text = "(Has no cooldown)";
            lblNoCooldown.TextAlign = ContentAlignment.MiddleCenter;
            lblNoCooldown.Visible = false;
            // 
            // lblInfiniteUse
            // 
            lblInfiniteUse.AutoSize = true;
            lblInfiniteUse.Location = new Point(360, 185);
            lblInfiniteUse.Name = "lblInfiniteUse";
            lblInfiniteUse.Size = new Size(100, 15);
            lblInfiniteUse.TabIndex = 13;
            lblInfiniteUse.Text = "(Has no use limit)";
            lblInfiniteUse.TextAlign = ContentAlignment.MiddleCenter;
            lblInfiniteUse.Visible = false;
            // 
            // nudMaximumUses
            // 
            nudMaximumUses.Location = new Point(109, 183);
            nudMaximumUses.Maximum = new decimal(new int[] { 99, 0, 0, 0 });
            nudMaximumUses.Name = "nudMaximumUses";
            nudMaximumUses.Size = new Size(41, 23);
            nudMaximumUses.TabIndex = 12;
            nudMaximumUses.ValueChanged += nudMaximumUses_ValueChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(5, 186);
            label5.Name = "label5";
            label5.Size = new Size(186, 15);
            label5.TabIndex = 11;
            label5.Text = "Can be used up to                 times.";
            // 
            // nudInitialCooldown
            // 
            nudInitialCooldown.Location = new Point(104, 152);
            nudInitialCooldown.Maximum = new decimal(new int[] { 99, 0, 0, 0 });
            nudInitialCooldown.Name = "nudInitialCooldown";
            nudInitialCooldown.Size = new Size(41, 23);
            nudInitialCooldown.TabIndex = 10;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(5, 155);
            label4.Name = "label4";
            label4.Size = new Size(326, 15);
            label4.TabIndex = 9;
            label4.Text = "Must wait at least                 turns to be used for the first time.";
            // 
            // nudCooldown
            // 
            nudCooldown.Location = new Point(104, 117);
            nudCooldown.Maximum = new decimal(new int[] { 99, 0, 0, 0 });
            nudCooldown.Name = "nudCooldown";
            nudCooldown.Size = new Size(41, 23);
            nudCooldown.TabIndex = 8;
            nudCooldown.ValueChanged += nudCooldown_ValueChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(5, 120);
            label3.Name = "label3";
            label3.Size = new Size(277, 15);
            label3.TabIndex = 7;
            label3.Text = "Must wait at least                 turns between each use.";
            // 
            // nudMaxRange
            // 
            nudMaxRange.Location = new Point(191, 81);
            nudMaxRange.Maximum = new decimal(new int[] { 99, 0, 0, 0 });
            nudMaxRange.Name = "nudMaxRange";
            nudMaxRange.Size = new Size(41, 23);
            nudMaxRange.TabIndex = 6;
            // 
            // nudMinRange
            // 
            nudMinRange.Location = new Point(128, 81);
            nudMinRange.Maximum = new decimal(new int[] { 99, 0, 0, 0 });
            nudMinRange.Name = "nudMinRange";
            nudMinRange.Size = new Size(41, 23);
            nudMinRange.TabIndex = 5;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(5, 83);
            label2.Name = "label2";
            label2.Size = new Size(387, 15);
            label2.TabIndex = 4;
            label2.Text = "Requires a distance of                 to                 tiles to the Target to be used.";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(5, 26);
            label1.Name = "label1";
            label1.Size = new Size(98, 15);
            label1.TabIndex = 0;
            label1.Text = "Can be used on...";
            // 
            // btnSave
            // 
            btnSave.Location = new Point(308, 591);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 24);
            btnSave.TabIndex = 4;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnEdit
            // 
            btnEdit.Enabled = false;
            btnEdit.Location = new Point(54, 285);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(113, 23);
            btnEdit.TabIndex = 5;
            btnEdit.Text = "Edit Step";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += btnEdit_Click;
            // 
            // btnNewThen
            // 
            btnNewThen.Enabled = false;
            btnNewThen.Location = new Point(173, 285);
            btnNewThen.Name = "btnNewThen";
            btnNewThen.Size = new Size(75, 23);
            btnNewThen.TabIndex = 6;
            btnNewThen.Text = "Add Then";
            btnNewThen.UseVisualStyleBackColor = true;
            btnNewThen.Click += btnNewThen_Click;
            // 
            // btnNewOnSuccessFailure
            // 
            btnNewOnSuccessFailure.Enabled = false;
            btnNewOnSuccessFailure.Location = new Point(256, 285);
            btnNewOnSuccessFailure.Name = "btnNewOnSuccessFailure";
            btnNewOnSuccessFailure.Size = new Size(165, 23);
            btnNewOnSuccessFailure.TabIndex = 7;
            btnNewOnSuccessFailure.Text = "Add On Success/On Failure";
            btnNewOnSuccessFailure.UseVisualStyleBackColor = true;
            btnNewOnSuccessFailure.Click += btnNewOnSuccessFailure_Click;
            // 
            // btnRemove
            // 
            btnRemove.Enabled = false;
            btnRemove.Location = new Point(308, 314);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(113, 23);
            btnRemove.TabIndex = 8;
            btnRemove.Text = "Remove Step";
            btnRemove.UseVisualStyleBackColor = true;
            btnRemove.Click += btnRemove_Click;
            // 
            // btnSaveAs
            // 
            btnSaveAs.Location = new Point(387, 591);
            btnSaveAs.Name = "btnSaveAs";
            btnSaveAs.Size = new Size(75, 24);
            btnSaveAs.TabIndex = 9;
            btnSaveAs.Text = "Save As...";
            btnSaveAs.UseVisualStyleBackColor = true;
            btnSaveAs.Click += btnSaveAs_Click;
            // 
            // fklblActionDescriptionLocale
            // 
            fklblActionDescriptionLocale.Enabled = false;
            fklblActionDescriptionLocale.FlatAppearance.BorderSize = 0;
            fklblActionDescriptionLocale.FlatStyle = FlatStyle.Flat;
            fklblActionDescriptionLocale.Image = (Image)resources.GetObject("fklblActionDescriptionLocale.Image");
            fklblActionDescriptionLocale.ImageAlign = ContentAlignment.TopLeft;
            fklblActionDescriptionLocale.Location = new Point(476, 420);
            fklblActionDescriptionLocale.Name = "fklblActionDescriptionLocale";
            fklblActionDescriptionLocale.Size = new Size(467, 42);
            fklblActionDescriptionLocale.TabIndex = 15;
            fklblActionDescriptionLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblActionDescriptionLocale.TextAlign = ContentAlignment.MiddleLeft;
            fklblActionDescriptionLocale.TextImageRelation = TextImageRelation.ImageBeforeText;
            fklblActionDescriptionLocale.UseVisualStyleBackColor = true;
            fklblActionDescriptionLocale.Visible = false;
            // 
            // txtActionDescription
            // 
            txtActionDescription.Location = new Point(476, 391);
            txtActionDescription.Name = "txtActionDescription";
            txtActionDescription.Size = new Size(467, 23);
            txtActionDescription.TabIndex = 14;
            txtActionDescription.TextChanged += txtActionDescription_TextChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(476, 373);
            label6.Name = "label6";
            label6.Size = new Size(67, 15);
            label6.TabIndex = 13;
            label6.Text = "Description";
            // 
            // fklblConditionWarning
            // 
            fklblConditionWarning.Enabled = false;
            fklblConditionWarning.FlatAppearance.BorderSize = 0;
            fklblConditionWarning.FlatStyle = FlatStyle.Flat;
            fklblConditionWarning.Image = (Image)resources.GetObject("fklblConditionWarning.Image");
            fklblConditionWarning.ImageAlign = ContentAlignment.TopLeft;
            fklblConditionWarning.Location = new Point(3, 523);
            fklblConditionWarning.Name = "fklblConditionWarning";
            fklblConditionWarning.Size = new Size(464, 42);
            fklblConditionWarning.TabIndex = 24;
            fklblConditionWarning.Text = "This is a valid condition, but the Editor currently can't support validating whether it will truly work as intended or not. Remember to test it in-game.";
            fklblConditionWarning.TextAlign = ContentAlignment.MiddleLeft;
            fklblConditionWarning.TextImageRelation = TextImageRelation.ImageBeforeText;
            fklblConditionWarning.UseVisualStyleBackColor = true;
            fklblConditionWarning.Visible = false;
            // 
            // txtActionCondition
            // 
            txtActionCondition.Location = new Point(3, 494);
            txtActionCondition.Name = "txtActionCondition";
            txtActionCondition.Size = new Size(464, 23);
            txtActionCondition.TabIndex = 23;
            txtActionCondition.Enter += txtActionCondition_Enter;
            txtActionCondition.Leave += txtActionCondition_Leave;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(3, 476);
            label7.Name = "label7";
            label7.Size = new Size(239, 15);
            label7.TabIndex = 22;
            label7.Text = "To be used, it must fulfill this condition first:";
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ActiveCaption;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(lblTarget);
            panel1.Controls.Add(lblSource);
            panel1.Controls.Add(lblThis);
            panel1.Controls.Add(label12);
            panel1.Controls.Add(label11);
            panel1.Controls.Add(label10);
            panel1.Controls.Add(label9);
            panel1.Controls.Add(pictureBox1);
            panel1.Location = new Point(4, 351);
            panel1.Name = "panel1";
            panel1.Size = new Size(466, 122);
            panel1.TabIndex = 25;
            // 
            // lblTarget
            // 
            lblTarget.Font = new Font("Segoe UI", 9.75F);
            lblTarget.Location = new Point(65, 90);
            lblTarget.Name = "lblTarget";
            lblTarget.Size = new Size(384, 19);
            lblTarget.TabIndex = 7;
            lblTarget.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblSource
            // 
            lblSource.Font = new Font("Segoe UI", 9.75F);
            lblSource.Location = new Point(65, 64);
            lblSource.Name = "lblSource";
            lblSource.Size = new Size(384, 19);
            lblSource.TabIndex = 6;
            lblSource.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblThis
            // 
            lblThis.Font = new Font("Segoe UI", 9.75F);
            lblThis.Location = new Point(65, 40);
            lblThis.Name = "lblThis";
            lblThis.Size = new Size(384, 19);
            lblThis.TabIndex = 5;
            lblThis.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            label12.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label12.Location = new Point(3, 91);
            label12.Name = "label12";
            label12.Size = new Size(68, 19);
            label12.TabIndex = 4;
            label12.Text = "- Target:";
            label12.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            label11.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label11.Location = new Point(3, 65);
            label11.Name = "label11";
            label11.Size = new Size(68, 19);
            label11.TabIndex = 3;
            label11.Text = "- Source:";
            label11.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            label10.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label10.Location = new Point(3, 41);
            label10.Name = "label10";
            label10.Size = new Size(68, 19);
            label10.TabIndex = 2;
            label10.Text = "- This:";
            label10.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            label9.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            label9.Location = new Point(33, 3);
            label9.Name = "label9";
            label9.Size = new Size(210, 24);
            label9.TabIndex = 1;
            label9.Text = "Who is who?";
            label9.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(3, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(24, 24);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // chkFinishesTurn
            // 
            chkFinishesTurn.AutoSize = true;
            chkFinishesTurn.Location = new Point(355, 566);
            chkFinishesTurn.Name = "chkFinishesTurn";
            chkFinishesTurn.Size = new Size(264, 19);
            chkFinishesTurn.TabIndex = 26;
            chkFinishesTurn.Text = "Executing this action finishes the current turn";
            chkFinishesTurn.UseVisualStyleBackColor = true;
            // 
            // btnCopyStep
            // 
            btnCopyStep.Enabled = false;
            btnCopyStep.Location = new Point(54, 314);
            btnCopyStep.Name = "btnCopyStep";
            btnCopyStep.Size = new Size(118, 23);
            btnCopyStep.TabIndex = 27;
            btnCopyStep.Text = "Copy Step";
            btnCopyStep.UseVisualStyleBackColor = true;
            btnCopyStep.Click += btnCopyStep_Click;
            // 
            // btnPasteStep
            // 
            btnPasteStep.Enabled = false;
            btnPasteStep.Location = new Point(178, 314);
            btnPasteStep.Name = "btnPasteStep";
            btnPasteStep.Size = new Size(124, 23);
            btnPasteStep.TabIndex = 28;
            btnPasteStep.Text = "Paste Step";
            btnPasteStep.UseVisualStyleBackColor = true;
            btnPasteStep.Click += btnPasteStep_Click;
            // 
            // fklblAIConditionWarning
            // 
            fklblAIConditionWarning.Enabled = false;
            fklblAIConditionWarning.FlatAppearance.BorderSize = 0;
            fklblAIConditionWarning.FlatStyle = FlatStyle.Flat;
            fklblAIConditionWarning.Image = (Image)resources.GetObject("fklblAIConditionWarning.Image");
            fklblAIConditionWarning.ImageAlign = ContentAlignment.TopLeft;
            fklblAIConditionWarning.Location = new Point(476, 520);
            fklblAIConditionWarning.Name = "fklblAIConditionWarning";
            fklblAIConditionWarning.Size = new Size(464, 42);
            fklblAIConditionWarning.TabIndex = 31;
            fklblAIConditionWarning.Text = "This is a valid condition, but the Editor currently can't support validating whether it will truly work as intended or not. Remember to test it in-game.";
            fklblAIConditionWarning.TextAlign = ContentAlignment.MiddleLeft;
            fklblAIConditionWarning.TextImageRelation = TextImageRelation.ImageBeforeText;
            fklblAIConditionWarning.UseVisualStyleBackColor = true;
            fklblAIConditionWarning.Visible = false;
            // 
            // txtActionAICondition
            // 
            txtActionAICondition.Location = new Point(476, 491);
            txtActionAICondition.Name = "txtActionAICondition";
            txtActionAICondition.Size = new Size(464, 23);
            txtActionAICondition.TabIndex = 30;
            txtActionAICondition.Enter += txtActionAICondition_Enter;
            txtActionAICondition.Leave += txtActionAICondition_Leave;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(476, 473);
            label13.Name = "label13";
            label13.Size = new Size(298, 15);
            label13.TabIndex = 29;
            label13.Text = "To be used by an NPC, it must fulfill this condition first:";
            // 
            // fklblActionNameLocale
            // 
            fklblActionNameLocale.Enabled = false;
            fklblActionNameLocale.FlatAppearance.BorderSize = 0;
            fklblActionNameLocale.FlatStyle = FlatStyle.Flat;
            fklblActionNameLocale.Image = (Image)resources.GetObject("fklblActionNameLocale.Image");
            fklblActionNameLocale.ImageAlign = ContentAlignment.TopLeft;
            fklblActionNameLocale.Location = new Point(476, 328);
            fklblActionNameLocale.Name = "fklblActionNameLocale";
            fklblActionNameLocale.Size = new Size(467, 42);
            fklblActionNameLocale.TabIndex = 34;
            fklblActionNameLocale.Text = "This value has been found as a Locale Entry key.\r\nIn-game, it will be replaced by the Locale Entry's value.";
            fklblActionNameLocale.TextAlign = ContentAlignment.MiddleLeft;
            fklblActionNameLocale.TextImageRelation = TextImageRelation.ImageBeforeText;
            fklblActionNameLocale.UseVisualStyleBackColor = true;
            fklblActionNameLocale.Visible = false;
            // 
            // txtActionName
            // 
            txtActionName.Location = new Point(476, 299);
            txtActionName.Name = "txtActionName";
            txtActionName.Size = new Size(467, 23);
            txtActionName.TabIndex = 33;
            txtActionName.TextChanged += txtActionName_TextChanged;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(476, 281);
            label14.Name = "label14";
            label14.Size = new Size(39, 15);
            label14.TabIndex = 32;
            label14.Text = "Name";
            // 
            // btnTestAction
            // 
            btnTestAction.Location = new Point(466, 591);
            btnTestAction.Name = "btnTestAction";
            btnTestAction.Size = new Size(75, 24);
            btnTestAction.TabIndex = 35;
            btnTestAction.Text = "Test";
            btnTestAction.UseVisualStyleBackColor = true;
            btnTestAction.Click += btnTestAction_Click;
            // 
            // frmActionEdit
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(950, 620);
            Controls.Add(btnTestAction);
            Controls.Add(fklblActionNameLocale);
            Controls.Add(txtActionName);
            Controls.Add(label14);
            Controls.Add(fklblAIConditionWarning);
            Controls.Add(txtActionAICondition);
            Controls.Add(label13);
            Controls.Add(btnPasteStep);
            Controls.Add(btnCopyStep);
            Controls.Add(chkFinishesTurn);
            Controls.Add(panel1);
            Controls.Add(fklblConditionWarning);
            Controls.Add(txtActionCondition);
            Controls.Add(label7);
            Controls.Add(gbSelectionCriteria);
            Controls.Add(fklblActionDescriptionLocale);
            Controls.Add(txtActionDescription);
            Controls.Add(label6);
            Controls.Add(btnSaveAs);
            Controls.Add(btnRemove);
            Controls.Add(btnNewOnSuccessFailure);
            Controls.Add(btnNewThen);
            Controls.Add(btnEdit);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            Controls.Add(tvEffectSequence);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmActionEdit";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Action Editor";
            FormClosing += frmActionEdit_FormClosing;
            gbSelectionCriteria.ResumeLayout(false);
            gbSelectionCriteria.PerformLayout();
            pnlCharacterTargets.ResumeLayout(false);
            pnlCharacterTargets.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudMPCost).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMaximumUses).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudInitialCooldown).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudCooldown).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMaxRange).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMinRange).EndInit();
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private TreeView tvEffectSequence;
        private Button btnCancel;
        private GroupBox gbSelectionCriteria;
        private NumericUpDown nudInitialCooldown;
        private Label label4;
        private NumericUpDown nudCooldown;
        private Label label3;
        private NumericUpDown nudMaxRange;
        private NumericUpDown nudMinRange;
        private Label label2;
        private Label label1;
        private Label lblNoCooldown;
        private Label lblInfiniteUse;
        private NumericUpDown nudMaximumUses;
        private Label label5;
        private Button btnSave;
        private Button btnEdit;
        private Button btnNewThen;
        private Button btnNewOnSuccessFailure;
        private Button btnRemove;
        private Button btnSaveAs;
        private Button fklblActionDescriptionLocale;
        private TextBox txtActionDescription;
        private Label label6;
        private Button fklblConditionWarning;
        private TextBox txtActionCondition;
        private Label label7;
        private NumericUpDown nudMPCost;
        private Label label8;
        private Panel panel1;
        private Label lblTarget;
        private Label lblSource;
        private Label lblThis;
        private Label label12;
        private Label label11;
        private Label label10;
        private Label label9;
        private PictureBox pictureBox1;
        private CheckBox chkFinishesTurn;
        private RadioButton rbTile;
        private RadioButton rbEntity;
        private Panel pnlCharacterTargets;
        private CheckBox chkSelf;
        private CheckBox chkEnemies;
        private CheckBox chkAllies;
        private Button btnCopyStep;
        private Button btnPasteStep;
        private Button fklblAIConditionWarning;
        private TextBox txtActionAICondition;
        private Label label13;
        private Button fklblActionNameLocale;
        private TextBox txtActionName;
        private Label label14;
        private Button btnTestAction;
    }
}