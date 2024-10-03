using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using RogueCustomsDungeonEditor.Controls;
using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsDungeonEditor.HelperForms
{
    public partial class frmFloorKeys : Form
    {
        public KeyGenerationInfo KeyGenerationInfo { get; private set; }
        public bool Saved { get; set; }
        private FloorInfo ActiveFloorGroup;
        private readonly DungeonInfo ActiveDungeon;
        public frmFloorKeys(FloorInfo floorGroupToUse, int minFloorLevel, int maxFloorLevel, DungeonInfo activeDungeon, KeyGenerationInfo baseKeyGenerator)
        {
            InitializeComponent();
            ActiveFloorGroup = floorGroupToUse;
            ActiveDungeon = activeDungeon;
            if (ActiveFloorGroup.MinFloorLevel != ActiveFloorGroup.MaxFloorLevel)
                lblFloorGroupTitle.Text = $"For Floor Levels {minFloorLevel} to {maxFloorLevel}:";
            else
                lblFloorGroupTitle.Text = $"For Floor Level {minFloorLevel}:";
            if (baseKeyGenerator != null)
                KeyGenerationInfo = baseKeyGenerator.Clone();
            else
                KeyGenerationInfo = new()
                {
                    KeySpawnInEnemyInventoryOdds = 0,
                    LockedRoomOdds = 0,
                    MaxPercentageOfLockedCandidateRooms = 0,
                    KeyTypes = new()
                };
            nudLockedDoorOdds.Value = KeyGenerationInfo.LockedRoomOdds;
            nudMaxLockedDoorsPercentage.Value = KeyGenerationInfo.MaxPercentageOfLockedCandidateRooms;
            nudKeyInEnemyInventoryOdds.Value = KeyGenerationInfo.KeySpawnInEnemyInventoryOdds;
            //flpKeySettings.Controls.Clear();
            foreach (var keyType in KeyGenerationInfo.KeyTypes)
            {
                AddKeyDoorEditor(keyType);
            }
            ToggleNumericUpDowns();
        }

        private void AddKeyDoorEditor(KeyTypeInfo keyType)
        {
            var keyDoorEditor = new KeyDoorEditor()
            {
                ActiveDungeon = ActiveDungeon,
                KeyTypeName = keyType.KeyTypeName,
                CanLockStairs = keyType.CanLockStairs,
                CanLockItems = keyType.CanLockItems,
                KeyConsoleRepresentation = keyType.KeyConsoleRepresentation,
                DoorConsoleRepresentation = keyType.DoorConsoleRepresentation,
                BorderStyle = BorderStyle.FixedSingle,
            };

            flpKeySettings.FlowDirection = FlowDirection.TopDown;
            flpKeySettings.WrapContents = false;  // Ensures controls don't wrap
            flpKeySettings.AutoScroll = true;
            keyDoorEditor.Click += KeyDoorEditor_Click;
            flpKeySettings.Controls.Add(keyDoorEditor);
        }

        private void KeyDoorEditor_Click(object? sender, EventArgs e)
        {
            var clickedControl = (sender as KeyDoorEditor);
            var selectedControl = flpKeySettings.Controls.Cast<KeyDoorEditor>().FirstOrDefault(kde => kde.IsSelected);
            selectedControl?.ToggleSelection();
            if (selectedControl != clickedControl)
                clickedControl.ToggleSelection();
            btnRemoveKeyType.Enabled = flpKeySettings.Controls.Cast<KeyDoorEditor>().Any(kde => kde.IsSelected);
        }

        private void btnAddKeyType_Click(object sender, EventArgs e)
        {
            flpKeySettings.Controls.Cast<KeyDoorEditor>().FirstOrDefault(kde => kde.IsSelected)?.ToggleSelection();
            btnRemoveKeyType.Enabled = false;
            AddKeyDoorEditor(DungeonInfoHelpers.CreateKeyTypeInfoTemplate());
        }

        private void btnRemoveKeyType_Click(object sender, EventArgs e)
        {
            var selectedRow = flpKeySettings.Controls.Cast<KeyDoorEditor>().FirstOrDefault(kde => kde.IsSelected);
            if (selectedRow != null)
                flpKeySettings.Controls.Remove(selectedRow);
            btnRemoveKeyType.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            KeyGenerationInfo = new()
            {
                KeySpawnInEnemyInventoryOdds = (int)nudKeyInEnemyInventoryOdds.Value,
                MaxPercentageOfLockedCandidateRooms = (int)nudMaxLockedDoorsPercentage.Value,
                LockedRoomOdds = (int)nudLockedDoorOdds.Value,
                KeyTypes = new()
            };
            foreach (KeyDoorEditor keyDoorEditor in flpKeySettings.Controls)
            {
                KeyGenerationInfo.KeyTypes.Add(keyDoorEditor.KeyDoorType);
            }
            var validationErrors = ValidateKeyTypes();
            if (!validationErrors.Any())
            {
                Saved = true;
                this.Close();
            }
            else
            {
                MessageBox.Show(
                    $"Cannot save Key/Door Types. Please correct the following errors:\n- {string.Join("\n- ", validationErrors.Distinct())}",
                    "Save Key/Door Types",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private List<string> ValidateKeyTypes()
        {
            var validationErrors = new List<string>();
            foreach (var keyType in KeyGenerationInfo.KeyTypes)
            {
                if (!keyType.CanLockStairs && !keyType.CanLockItems)
                    validationErrors.Add("At least one Key Type is set to not lock anything.");
                if (KeyGenerationInfo.KeyTypes.Count(kt => kt.KeyTypeName.Equals(keyType.KeyTypeName)) > 1)
                    validationErrors.Add("At least two Key Types have the same name.");
                if (KeyGenerationInfo.KeyTypes.Count(kt => kt.KeyConsoleRepresentation.Equals(keyType.KeyConsoleRepresentation)) > 1)
                    validationErrors.Add("At least two Key Types have the same Key Appearance.");
                if (KeyGenerationInfo.KeyTypes.Count(kt => kt.DoorConsoleRepresentation.Equals(keyType.DoorConsoleRepresentation))> 1)
                    validationErrors.Add("At least two Key Types have the same Door Appearance.");
            }
            if (!KeyGenerationInfo.KeyTypes.Any() && (nudMaxLockedDoorsPercentage.Value > 0 || nudLockedDoorOdds.Value > 0 || nudKeyInEnemyInventoryOdds.Value > 0))
            {
                validationErrors.Add("Odds or distribution values are above 0, but no Keys have been defined");
            }
            else if (KeyGenerationInfo.KeyTypes.Any() && nudMaxLockedDoorsPercentage.Value == 0 && nudLockedDoorOdds.Value == 0 && nudKeyInEnemyInventoryOdds.Value == 0)
            {
                validationErrors.Add("Keys have been defined, but odds and distribution values are all 0");
            }
            return validationErrors;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void flpKeySettings_ControlAdded_1(object sender, ControlEventArgs e)
        {
            ToggleNumericUpDowns();
        }

        private void flpKeySettings_ControlRemoved_1(object sender, ControlEventArgs e)
        {
            ToggleNumericUpDowns();
        }

        private void ToggleNumericUpDowns()
        {
            if (flpKeySettings.Controls.Count > 0)
            {
                nudKeyInEnemyInventoryOdds.Enabled = true;
                nudLockedDoorOdds.Enabled = true;
                nudMaxLockedDoorsPercentage.Enabled = true;
            }
            else
            {
                nudKeyInEnemyInventoryOdds.Enabled = false;
                nudKeyInEnemyInventoryOdds.Value = 0;
                nudLockedDoorOdds.Enabled = false;
                nudLockedDoorOdds.Value = 0;
                nudMaxLockedDoorsPercentage.Enabled = false;
                nudMaxLockedDoorsPercentage.Value = 0;
            }
        }
    }
}
