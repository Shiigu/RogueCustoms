namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    partial class FloorGroupTab
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(FloorGroupTab));
            btnPasteAlgorithm = new System.Windows.Forms.Button();
            btnCopyAlgorithm = new System.Windows.Forms.Button();
            nudHungerLostPerTurn = new System.Windows.Forms.NumericUpDown();
            label31 = new System.Windows.Forms.Label();
            saeOnFloorStart = new SingleActionEditor();
            cmbTilesets = new System.Windows.Forms.ComboBox();
            label155 = new System.Windows.Forms.Label();
            nudRoomFusionOdds = new System.Windows.Forms.NumericUpDown();
            label19 = new System.Windows.Forms.Label();
            nudExtraRoomConnectionOdds = new System.Windows.Forms.NumericUpDown();
            label18 = new System.Windows.Forms.Label();
            nudMaxRoomConnections = new System.Windows.Forms.NumericUpDown();
            label5 = new System.Windows.Forms.Label();
            btnRemoveAlgorithm = new System.Windows.Forms.Button();
            btnEditAlgorithm = new System.Windows.Forms.Button();
            btnAddProceduralLayout = new System.Windows.Forms.Button();
            lvFloorAlgorithms = new System.Windows.Forms.ListView();
            label17 = new System.Windows.Forms.Label();
            label16 = new System.Windows.Forms.Label();
            btnTrapGenerator = new System.Windows.Forms.Button();
            label15 = new System.Windows.Forms.Label();
            btnItemGenerator = new System.Windows.Forms.Button();
            label14 = new System.Windows.Forms.Label();
            btnNPCGenerator = new System.Windows.Forms.Button();
            label13 = new System.Windows.Forms.Label();
            nudHeight = new System.Windows.Forms.NumericUpDown();
            nudWidth = new System.Windows.Forms.NumericUpDown();
            label12 = new System.Windows.Forms.Label();
            label11 = new System.Windows.Forms.Label();
            fklblStairsReminder = new System.Windows.Forms.Button();
            chkGenerateStairsOnStart = new System.Windows.Forms.CheckBox();
            nudMaxFloorLevel = new System.Windows.Forms.NumericUpDown();
            label10 = new System.Windows.Forms.Label();
            nudMinFloorLevel = new System.Windows.Forms.NumericUpDown();
            label9 = new System.Windows.Forms.Label();
            btnFloorKeys = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            btnSpecialTileGenerator = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            nudFloorGroupMonsterHouseOdds = new System.Windows.Forms.NumericUpDown();
            label3 = new System.Windows.Forms.Label();
            btnAddStaticLayout = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)nudHungerLostPerTurn).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudRoomFusionOdds).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudExtraRoomConnectionOdds).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMaxRoomConnections).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudHeight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudWidth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMaxFloorLevel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMinFloorLevel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudFloorGroupMonsterHouseOdds).BeginInit();
            SuspendLayout();
            // 
            // btnPasteAlgorithm
            // 
            btnPasteAlgorithm.Enabled = false;
            btnPasteAlgorithm.Location = new System.Drawing.Point(534, 196);
            btnPasteAlgorithm.Name = "btnPasteAlgorithm";
            btnPasteAlgorithm.Size = new System.Drawing.Size(60, 23);
            btnPasteAlgorithm.TabIndex = 79;
            btnPasteAlgorithm.Text = "Paste";
            btnPasteAlgorithm.UseVisualStyleBackColor = true;
            btnPasteAlgorithm.Click += btnPasteAlgorithm_Click;
            // 
            // btnCopyAlgorithm
            // 
            btnCopyAlgorithm.Enabled = false;
            btnCopyAlgorithm.Location = new System.Drawing.Point(468, 196);
            btnCopyAlgorithm.Name = "btnCopyAlgorithm";
            btnCopyAlgorithm.Size = new System.Drawing.Size(60, 23);
            btnCopyAlgorithm.TabIndex = 78;
            btnCopyAlgorithm.Text = "Copy";
            btnCopyAlgorithm.UseVisualStyleBackColor = true;
            btnCopyAlgorithm.Click += btnCopyAlgorithm_Click;
            // 
            // nudHungerLostPerTurn
            // 
            nudHungerLostPerTurn.DecimalPlaces = 4;
            nudHungerLostPerTurn.Increment = new decimal(new int[] { 25, 0, 0, 262144 });
            nudHungerLostPerTurn.Location = new System.Drawing.Point(93, 171);
            nudHungerLostPerTurn.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            nudHungerLostPerTurn.Name = "nudHungerLostPerTurn";
            nudHungerLostPerTurn.Size = new System.Drawing.Size(54, 23);
            nudHungerLostPerTurn.TabIndex = 77;
            nudHungerLostPerTurn.Value = new decimal(new int[] { 25, 0, 0, 262144 });
            nudHungerLostPerTurn.ValueChanged += nudHungerLostPerTurn_ValueChanged;
            // 
            // label31
            // 
            label31.AutoSize = true;
            label31.Location = new System.Drawing.Point(7, 172);
            label31.Name = "label31";
            label31.Size = new System.Drawing.Size(232, 15);
            label31.TabIndex = 76;
            label31.Text = "Characters lose                    Hunger per turn";
            // 
            // saeOnFloorStart
            // 
            saeOnFloorStart.ActionDescription = "When the floor starts...";
            saeOnFloorStart.ActionTypeText = "Floor Start";
            saeOnFloorStart.AutoSize = true;
            saeOnFloorStart.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            saeOnFloorStart.ClassId = null;
            saeOnFloorStart.Location = new System.Drawing.Point(356, 365);
            saeOnFloorStart.Name = "saeOnFloorStart";
            saeOnFloorStart.PlaceholderActionId = "FloorStart";
            saeOnFloorStart.Size = new System.Drawing.Size(260, 32);
            saeOnFloorStart.SourceDescription = "The player (Don't use)";
            saeOnFloorStart.TabIndex = 75;
            saeOnFloorStart.TargetDescription = "The player";
            saeOnFloorStart.ThisDescription = "None (Don't use)";
            saeOnFloorStart.ActionContentsChanged += saeOnFloorStart_ActionContentsChanged;
            // 
            // cmbTilesets
            // 
            cmbTilesets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbTilesets.FormattingEnabled = true;
            cmbTilesets.Location = new System.Drawing.Point(162, 54);
            cmbTilesets.Name = "cmbTilesets";
            cmbTilesets.Size = new System.Drawing.Size(149, 23);
            cmbTilesets.TabIndex = 74;
            cmbTilesets.SelectedIndexChanged += cmbTilesets_SelectedIndexChanged;
            // 
            // label155
            // 
            label155.AutoSize = true;
            label155.Location = new System.Drawing.Point(116, 57);
            label155.Name = "label155";
            label155.Size = new System.Drawing.Size(41, 15);
            label155.TabIndex = 73;
            label155.Text = "Tileset";
            // 
            // nudRoomFusionOdds
            // 
            nudRoomFusionOdds.Location = new System.Drawing.Point(505, 284);
            nudRoomFusionOdds.Name = "nudRoomFusionOdds";
            nudRoomFusionOdds.Size = new System.Drawing.Size(40, 23);
            nudRoomFusionOdds.TabIndex = 72;
            nudRoomFusionOdds.ValueChanged += nudRoomFusionOdds_ValueChanged;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new System.Drawing.Point(356, 286);
            label19.Name = "label19";
            label19.Size = new System.Drawing.Size(286, 15);
            label19.TabIndex = 71;
            label19.Text = "Two adjacent rooms have a               % chance to fuse";
            // 
            // nudExtraRoomConnectionOdds
            // 
            nudExtraRoomConnectionOdds.Location = new System.Drawing.Point(457, 254);
            nudExtraRoomConnectionOdds.Name = "nudExtraRoomConnectionOdds";
            nudExtraRoomConnectionOdds.Size = new System.Drawing.Size(40, 23);
            nudExtraRoomConnectionOdds.TabIndex = 70;
            nudExtraRoomConnectionOdds.ValueChanged += nudExtraRoomConnectionOdds_ValueChanged;
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new System.Drawing.Point(357, 257);
            label18.Name = "label18";
            label18.Size = new System.Drawing.Size(323, 15);
            label18.TabIndex = 69;
            label18.Text = "(With a chance of               % of connecting more than once)";
            // 
            // nudMaxRoomConnections
            // 
            nudMaxRoomConnections.Location = new System.Drawing.Point(606, 226);
            nudMaxRoomConnections.Maximum = new decimal(new int[] { 9, 0, 0, 0 });
            nudMaxRoomConnections.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudMaxRoomConnections.Name = "nudMaxRoomConnections";
            nudMaxRoomConnections.Size = new System.Drawing.Size(33, 23);
            nudMaxRoomConnections.TabIndex = 68;
            nudMaxRoomConnections.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nudMaxRoomConnections.ValueChanged += nudMaxRoomConnections_ValueChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(357, 228);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(326, 15);
            label5.TabIndex = 67;
            label5.Text = "Rooms can connect between each other up to             time(s)";
            // 
            // btnRemoveAlgorithm
            // 
            btnRemoveAlgorithm.Enabled = false;
            btnRemoveAlgorithm.Location = new System.Drawing.Point(600, 196);
            btnRemoveAlgorithm.Name = "btnRemoveAlgorithm";
            btnRemoveAlgorithm.Size = new System.Drawing.Size(60, 23);
            btnRemoveAlgorithm.TabIndex = 66;
            btnRemoveAlgorithm.Text = "Remove";
            btnRemoveAlgorithm.UseVisualStyleBackColor = true;
            btnRemoveAlgorithm.Click += btnRemoveAlgorithm_Click;
            // 
            // btnEditAlgorithm
            // 
            btnEditAlgorithm.Enabled = false;
            btnEditAlgorithm.Location = new System.Drawing.Point(403, 196);
            btnEditAlgorithm.Name = "btnEditAlgorithm";
            btnEditAlgorithm.Size = new System.Drawing.Size(60, 23);
            btnEditAlgorithm.TabIndex = 65;
            btnEditAlgorithm.Text = "Edit";
            btnEditAlgorithm.UseVisualStyleBackColor = true;
            btnEditAlgorithm.Click += btnEditAlgorithm_Click;
            // 
            // btnAddProceduralLayout
            // 
            btnAddProceduralLayout.Location = new System.Drawing.Point(374, 167);
            btnAddProceduralLayout.Name = "btnAddProceduralLayout";
            btnAddProceduralLayout.Size = new System.Drawing.Size(151, 23);
            btnAddProceduralLayout.TabIndex = 64;
            btnAddProceduralLayout.Text = "New Procedural Layout...";
            btnAddProceduralLayout.UseVisualStyleBackColor = true;
            btnAddProceduralLayout.Click += btnAddProceduralLayout_Click;
            // 
            // lvFloorAlgorithms
            // 
            lvFloorAlgorithms.Location = new System.Drawing.Point(356, 52);
            lvFloorAlgorithms.MultiSelect = false;
            lvFloorAlgorithms.Name = "lvFloorAlgorithms";
            lvFloorAlgorithms.Size = new System.Drawing.Size(349, 109);
            lvFloorAlgorithms.TabIndex = 63;
            lvFloorAlgorithms.UseCompatibleStateImageBehavior = false;
            lvFloorAlgorithms.SelectedIndexChanged += lvFloorAlgorithms_SelectedIndexChanged;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new System.Drawing.Point(468, 33);
            label17.Name = "label17";
            label17.Size = new System.Drawing.Size(127, 15);
            label17.TabIndex = 62;
            label17.Text = "Generation Algorithms";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            label16.Location = new System.Drawing.Point(463, 3);
            label16.Name = "label16";
            label16.Size = new System.Drawing.Size(138, 21);
            label16.TabIndex = 61;
            label16.Text = "Floor Room Data";
            // 
            // btnTrapGenerator
            // 
            btnTrapGenerator.Location = new System.Drawing.Point(175, 305);
            btnTrapGenerator.Name = "btnTrapGenerator";
            btnTrapGenerator.Size = new System.Drawing.Size(151, 23);
            btnTrapGenerator.TabIndex = 60;
            btnTrapGenerator.Text = "Traps to be generated...";
            btnTrapGenerator.UseVisualStyleBackColor = true;
            btnTrapGenerator.Click += btnTrapGenerator_Click;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            label15.Location = new System.Drawing.Point(186, 271);
            label15.Name = "label15";
            label15.Size = new System.Drawing.Size(126, 21);
            label15.TabIndex = 59;
            label15.Text = "Floor Trap Data";
            // 
            // btnItemGenerator
            // 
            btnItemGenerator.Location = new System.Drawing.Point(8, 305);
            btnItemGenerator.Name = "btnItemGenerator";
            btnItemGenerator.Size = new System.Drawing.Size(151, 23);
            btnItemGenerator.TabIndex = 58;
            btnItemGenerator.Text = "Items to be generated...";
            btnItemGenerator.UseVisualStyleBackColor = true;
            btnItemGenerator.Click += btnItemGenerator_Click;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            label14.Location = new System.Drawing.Point(19, 271);
            label14.Name = "label14";
            label14.Size = new System.Drawing.Size(128, 21);
            label14.TabIndex = 57;
            label14.Text = "Floor Item Data";
            // 
            // btnNPCGenerator
            // 
            btnNPCGenerator.Location = new System.Drawing.Point(9, 240);
            btnNPCGenerator.Name = "btnNPCGenerator";
            btnNPCGenerator.Size = new System.Drawing.Size(151, 23);
            btnNPCGenerator.TabIndex = 56;
            btnNPCGenerator.Text = "NPCs to be generated...";
            btnNPCGenerator.UseVisualStyleBackColor = true;
            btnNPCGenerator.Click += btnNPCGenerator_Click;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            label13.Location = new System.Drawing.Point(21, 206);
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(126, 21);
            label13.TabIndex = 55;
            label13.Text = "Floor NPC Data";
            // 
            // nudHeight
            // 
            nudHeight.Location = new System.Drawing.Point(49, 69);
            nudHeight.Maximum = new decimal(new int[] { 512, 0, 0, 0 });
            nudHeight.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            nudHeight.Name = "nudHeight";
            nudHeight.Size = new System.Drawing.Size(54, 23);
            nudHeight.TabIndex = 54;
            nudHeight.Value = new decimal(new int[] { 5, 0, 0, 0 });
            nudHeight.ValueChanged += nudHeight_ValueChanged;
            // 
            // nudWidth
            // 
            nudWidth.Location = new System.Drawing.Point(49, 36);
            nudWidth.Maximum = new decimal(new int[] { 512, 0, 0, 0 });
            nudWidth.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            nudWidth.Name = "nudWidth";
            nudWidth.Size = new System.Drawing.Size(54, 23);
            nudWidth.TabIndex = 53;
            nudWidth.Value = new decimal(new int[] { 5, 0, 0, 0 });
            nudWidth.ValueChanged += nudWidth_ValueChanged;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new System.Drawing.Point(4, 71);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(43, 15);
            label12.TabIndex = 52;
            label12.Text = "Height";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new System.Drawing.Point(4, 38);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(39, 15);
            label11.TabIndex = 51;
            label11.Text = "Width";
            // 
            // fklblStairsReminder
            // 
            fklblStairsReminder.Enabled = false;
            fklblStairsReminder.FlatAppearance.BorderSize = 0;
            fklblStairsReminder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fklblStairsReminder.Image = (System.Drawing.Image)resources.GetObject("fklblStairsReminder.Image");
            fklblStairsReminder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblStairsReminder.Location = new System.Drawing.Point(4, 123);
            fklblStairsReminder.Name = "fklblStairsReminder";
            fklblStairsReminder.Size = new System.Drawing.Size(287, 42);
            fklblStairsReminder.TabIndex = 50;
            fklblStairsReminder.Text = "Remember to include an element that\r\ngenerates Stairs, or it would softlock the game.";
            fklblStairsReminder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            fklblStairsReminder.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            fklblStairsReminder.UseVisualStyleBackColor = true;
            fklblStairsReminder.Visible = false;
            // 
            // chkGenerateStairsOnStart
            // 
            chkGenerateStairsOnStart.AutoSize = true;
            chkGenerateStairsOnStart.Location = new System.Drawing.Point(4, 98);
            chkGenerateStairsOnStart.Name = "chkGenerateStairsOnStart";
            chkGenerateStairsOnStart.Size = new System.Drawing.Size(214, 19);
            chkGenerateStairsOnStart.TabIndex = 49;
            chkGenerateStairsOnStart.Text = "Place Stairs when Floor is generated";
            chkGenerateStairsOnStart.UseVisualStyleBackColor = true;
            chkGenerateStairsOnStart.CheckedChanged += chkGenerateStairsOnStart_CheckedChanged;
            // 
            // nudMaxFloorLevel
            // 
            nudMaxFloorLevel.Location = new System.Drawing.Point(133, 6);
            nudMaxFloorLevel.Name = "nudMaxFloorLevel";
            nudMaxFloorLevel.Size = new System.Drawing.Size(33, 23);
            nudMaxFloorLevel.TabIndex = 48;
            nudMaxFloorLevel.ValueChanged += nudMaxFloorLevel_ValueChanged;
            nudMaxFloorLevel.Leave += nudMaxFloorLevel_Leave;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(109, 8);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(18, 15);
            label10.TabIndex = 47;
            label10.Text = "to";
            // 
            // nudMinFloorLevel
            // 
            nudMinFloorLevel.Location = new System.Drawing.Point(71, 6);
            nudMinFloorLevel.Name = "nudMinFloorLevel";
            nudMinFloorLevel.Size = new System.Drawing.Size(33, 23);
            nudMinFloorLevel.TabIndex = 46;
            nudMinFloorLevel.ValueChanged += nudMinFloorLevel_ValueChanged;
            nudMinFloorLevel.Leave += nudMinFloorLevel_Leave;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(4, 8);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(65, 15);
            label9.TabIndex = 45;
            label9.Text = "From Level";
            // 
            // btnFloorKeys
            // 
            btnFloorKeys.Location = new System.Drawing.Point(175, 240);
            btnFloorKeys.Name = "btnFloorKeys";
            btnFloorKeys.Size = new System.Drawing.Size(151, 23);
            btnFloorKeys.TabIndex = 81;
            btnFloorKeys.Text = "Keys to be generated...";
            btnFloorKeys.UseVisualStyleBackColor = true;
            btnFloorKeys.Click += btnFloorKeys_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            label1.Location = new System.Drawing.Point(187, 206);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(121, 21);
            label1.TabIndex = 80;
            label1.Text = "Floor Key Data";
            // 
            // btnSpecialTileGenerator
            // 
            btnSpecialTileGenerator.Location = new System.Drawing.Point(71, 373);
            btnSpecialTileGenerator.Name = "btnSpecialTileGenerator";
            btnSpecialTileGenerator.Size = new System.Drawing.Size(206, 23);
            btnSpecialTileGenerator.TabIndex = 83;
            btnSpecialTileGenerator.Text = "Special Tiles to be generated...";
            btnSpecialTileGenerator.UseVisualStyleBackColor = true;
            btnSpecialTileGenerator.Click += btnSpecialTileGenerator_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            label2.Location = new System.Drawing.Point(103, 343);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(137, 21);
            label2.TabIndex = 82;
            label2.Text = "Special Tile Data";
            // 
            // nudFloorGroupMonsterHouseOdds
            // 
            nudFloorGroupMonsterHouseOdds.Location = new System.Drawing.Point(408, 325);
            nudFloorGroupMonsterHouseOdds.Name = "nudFloorGroupMonsterHouseOdds";
            nudFloorGroupMonsterHouseOdds.Size = new System.Drawing.Size(40, 23);
            nudFloorGroupMonsterHouseOdds.TabIndex = 85;
            nudFloorGroupMonsterHouseOdds.ValueChanged += nudFloorGroupMonsterHouseOdds_ValueChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(356, 327);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(341, 15);
            label3.TabIndex = 84;
            label3.Text = "There's a               % chance of a Room to host a Monster House";
            // 
            // btnAddStaticLayout
            // 
            btnAddStaticLayout.Location = new System.Drawing.Point(531, 167);
            btnAddStaticLayout.Name = "btnAddStaticLayout";
            btnAddStaticLayout.Size = new System.Drawing.Size(151, 23);
            btnAddStaticLayout.TabIndex = 86;
            btnAddStaticLayout.Text = "New Static Layout...";
            btnAddStaticLayout.UseVisualStyleBackColor = true;
            btnAddStaticLayout.Click += btnAddStaticLayout_Click;
            // 
            // FloorGroupTab
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Controls.Add(btnAddStaticLayout);
            Controls.Add(nudFloorGroupMonsterHouseOdds);
            Controls.Add(label3);
            Controls.Add(btnSpecialTileGenerator);
            Controls.Add(label2);
            Controls.Add(btnFloorKeys);
            Controls.Add(label1);
            Controls.Add(btnPasteAlgorithm);
            Controls.Add(btnCopyAlgorithm);
            Controls.Add(nudHungerLostPerTurn);
            Controls.Add(label31);
            Controls.Add(saeOnFloorStart);
            Controls.Add(cmbTilesets);
            Controls.Add(label155);
            Controls.Add(nudRoomFusionOdds);
            Controls.Add(label19);
            Controls.Add(nudExtraRoomConnectionOdds);
            Controls.Add(label18);
            Controls.Add(nudMaxRoomConnections);
            Controls.Add(label5);
            Controls.Add(btnRemoveAlgorithm);
            Controls.Add(btnEditAlgorithm);
            Controls.Add(btnAddProceduralLayout);
            Controls.Add(lvFloorAlgorithms);
            Controls.Add(label17);
            Controls.Add(label16);
            Controls.Add(btnTrapGenerator);
            Controls.Add(label15);
            Controls.Add(btnItemGenerator);
            Controls.Add(label14);
            Controls.Add(btnNPCGenerator);
            Controls.Add(label13);
            Controls.Add(nudHeight);
            Controls.Add(nudWidth);
            Controls.Add(label12);
            Controls.Add(label11);
            Controls.Add(fklblStairsReminder);
            Controls.Add(chkGenerateStairsOnStart);
            Controls.Add(nudMaxFloorLevel);
            Controls.Add(label10);
            Controls.Add(nudMinFloorLevel);
            Controls.Add(label9);
            Name = "FloorGroupTab";
            Size = new System.Drawing.Size(717, 404);
            ((System.ComponentModel.ISupportInitialize)nudHungerLostPerTurn).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudRoomFusionOdds).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudExtraRoomConnectionOdds).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMaxRoomConnections).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudHeight).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudWidth).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMaxFloorLevel).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMinFloorLevel).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudFloorGroupMonsterHouseOdds).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnPasteAlgorithm;
        private System.Windows.Forms.Button btnCopyAlgorithm;
        private System.Windows.Forms.NumericUpDown nudHungerLostPerTurn;
        private System.Windows.Forms.Label label31;
        private SingleActionEditor saeOnFloorStart;
        private System.Windows.Forms.ComboBox cmbTilesets;
        private System.Windows.Forms.Label label155;
        private System.Windows.Forms.NumericUpDown nudRoomFusionOdds;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.NumericUpDown nudExtraRoomConnectionOdds;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.NumericUpDown nudMaxRoomConnections;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnRemoveAlgorithm;
        private System.Windows.Forms.Button btnEditAlgorithm;
        private System.Windows.Forms.Button btnAddProceduralLayout;
        private System.Windows.Forms.ListView lvFloorAlgorithms;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button btnTrapGenerator;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnItemGenerator;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnNPCGenerator;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown nudHeight;
        private System.Windows.Forms.NumericUpDown nudWidth;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button fklblStairsReminder;
        private System.Windows.Forms.CheckBox chkGenerateStairsOnStart;
        private System.Windows.Forms.NumericUpDown nudMaxFloorLevel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown nudMinFloorLevel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnFloorKeys;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSpecialTileGenerator;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudFloorGroupMonsterHouseOdds;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnAddStaticLayout;
    }
}
