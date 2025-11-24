using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    public partial class ItemTypesTab : UserControl
    {
        private Dictionary<string, string> ItemUsabilityOptions = new()
        {
            { "Nothing", "Can't be used" },
            { "Equip", "Equipping" },
            { "Use", "Using" }
        };
        private Dictionary<string, string> ItemPowerTypeOptions = new()
        {
            { "Damage", "Damage" },
            { "Mitigation", "Mitigation" },
            { "UsePower", "Use Power" }
        };
        private DungeonInfo ActiveDungeon;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<ItemTypeInfo> LoadedItemTypeInfos { get; private set; }
        private List<string> ItemSlots;
        private List<string> QualityLevels;
        public event EventHandler TabInfoChanged;
        public ItemTypesTab()
        {
            InitializeComponent();
        }

        public void LoadData(DungeonInfo activeDungeon)
        {
            ActiveDungeon = activeDungeon;
            LoadedItemTypeInfos = activeDungeon.ItemTypeInfos;
            QualityLevels = activeDungeon.QualityLevelInfos.ConvertAll(x => x.Id);
            ItemSlots = activeDungeon.ItemSlotInfos.ConvertAll(x => x.Id);
            var usabilityColumn = (DataGridViewComboBoxColumn)dgvItemTypes.Columns["Usability"];
            usabilityColumn.DataSource = ItemUsabilityOptions.Select(qlnao => qlnao.Value).ToList();
            var powerTypeColumn = (DataGridViewComboBoxColumn)dgvItemTypes.Columns["PowerType"];
            powerTypeColumn.DataSource = ItemPowerTypeOptions.Select(qlnao => qlnao.Value).ToList();
            var slot1Column = (DataGridViewComboBoxColumn)dgvItemTypes.Columns["Slot1"];
            slot1Column.DataSource = ItemSlots;
            var slot2Column = (DataGridViewComboBoxColumn)dgvItemTypes.Columns["Slot2"];
            slot2Column.DataSource = ItemSlots.Union([""]).ToList();
            var unidentifiedQualityLevelColumn = (DataGridViewComboBoxColumn)dgvItemTypes.Columns["MinimumQualityLevelForUnidentified"];
            unidentifiedQualityLevelColumn.DataSource = QualityLevels.Union([""]).ToList();
            dgvItemTypes.Rows.Clear();
            foreach (var itemType in LoadedItemTypeInfos)
            {
                dgvItemTypes.Rows.Add(itemType.Id, itemType.Name, ItemUsabilityOptions[itemType.Usability.ToString()], ItemPowerTypeOptions[itemType.PowerType.ToString()], itemType.Slot1, itemType.Slot2, itemType.MinimumQualityLevelForUnidentified, itemType.UnidentifiedItemName, itemType.UnidentifiedItemDescription, itemType.UnidentifiedItemActionName, itemType.UnidentifiedItemActionDescription);
            }
            dgvItemTypes.CellValueChanged += (sender, e) => TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }
        public List<string> SaveData()
        {
            var validationErrors = new List<string>();
            var itemTypes = new List<ItemTypeInfo>();
            var itemTypeIds = new HashSet<string>();
            dgvItemTypes.EndEdit();
            foreach (DataGridViewRow row in dgvItemTypes.Rows)
            {
                if (row.IsNewRow) continue;

                var isValidEntry = true;
                var id = (row.Cells[0].Value ?? string.Empty).ToString().Trim();
                var name = (row.Cells[1].Value ?? string.Empty).ToString().Trim();
                var usability = (row.Cells[2].Value ?? string.Empty).ToString().Trim();
                var powerType = (row.Cells[3].Value ?? string.Empty).ToString().Trim();
                var slot1 = (row.Cells[4].Value ?? string.Empty).ToString().Trim();
                var slot2 = (row.Cells[5].Value ?? string.Empty).ToString().Trim();
                var unidentifiedQualityLevel = (row.Cells[6].Value ?? string.Empty).ToString().Trim();
                var unidentifiedName = (row.Cells[7].Value ?? string.Empty).ToString().Trim();
                var unidentifiedDescription = (row.Cells[8].Value ?? string.Empty).ToString().Trim();
                var unidentifiedActionName = (row.Cells[9].Value ?? string.Empty).ToString().Trim();
                var unidentifiedActionDescription = (row.Cells[10].Value ?? string.Empty).ToString().Trim();

                if (string.IsNullOrWhiteSpace(id))
                {
                    isValidEntry = false;
                    validationErrors.Add($"Row {row.Index + 1}: Enter the Item Type Id first.");
                }
                if (string.IsNullOrWhiteSpace(name))
                {
                    isValidEntry = false;
                    validationErrors.Add($"Row {id}: Enter the Item Type Name first.");
                }
                if (!ItemUsabilityOptions.Any(iuo => iuo.Value.Equals(usability, StringComparison.InvariantCultureIgnoreCase)))
                {
                    isValidEntry = false;
                    validationErrors.Add($"Row {id}: The Usability option is invalid.");
                }
                if (!ItemPowerTypeOptions.Any(ipto => ipto.Value.Equals(powerType, StringComparison.InvariantCultureIgnoreCase)))
                {
                    isValidEntry = false;
                    validationErrors.Add($"Row {id}: The Power Type option is invalid.");
                }
                if ((!string.IsNullOrWhiteSpace(slot1) || !string.IsNullOrWhiteSpace(slot1)) && usability != "Equipping")
                {
                    isValidEntry = false;
                    validationErrors.Add($"Row {id}: The item is set to not be Equippable but it has assigned Slots.");
                }
                if (string.IsNullOrWhiteSpace(slot1) && usability == "Equipping")
                {
                    isValidEntry = false;
                    validationErrors.Add($"Row {id}: Enter the Primary Slot first.");
                }
                if (!string.IsNullOrWhiteSpace(slot1) && !string.IsNullOrWhiteSpace(slot2) && slot1.Equals(slot2, StringComparison.CurrentCultureIgnoreCase))
                {
                    isValidEntry = false;
                    validationErrors.Add($"Row {id}: Primary and Secondary Slots are Identical. If you want the Item Type to occupy only one slot, leave the Secondary Slot blank.");
                }
                if (!itemTypeIds.Add(id))
                {
                    isValidEntry = false;
                    validationErrors.Add($"Row {row.Index + 1}: The Item Type Id '{id}' is duplicated.");
                }
                if (!string.IsNullOrWhiteSpace(unidentifiedQualityLevel))
                {
                    if (string.IsNullOrWhiteSpace(unidentifiedName))
                    {
                        isValidEntry = false;
                        validationErrors.Add($"Row {id}: Enter the Unidentified Item Name first.");
                    }
                    if (string.IsNullOrWhiteSpace(unidentifiedDescription))
                    {
                        isValidEntry = false;
                        validationErrors.Add($"Row {id}: Enter the Unidentified Item Description first.");
                    }
                    if (string.IsNullOrWhiteSpace(unidentifiedActionName))
                    {
                        isValidEntry = false;
                        validationErrors.Add($"Row {id}: Enter the Unidentified Item Action Name first.");
                    }
                    if (string.IsNullOrWhiteSpace(unidentifiedActionDescription))
                    {
                        isValidEntry = false;
                        validationErrors.Add($"Row {id}: Enter the Unidentified Item Action Name first.");
                    }
                }
                if (isValidEntry)
                {
                    var usabilityKey = ItemUsabilityOptions.First(iuo => iuo.Value.Equals(usability, StringComparison.InvariantCultureIgnoreCase)).Key;
                    var powerTypeKey = ItemPowerTypeOptions.First(ipto => ipto.Value.Equals(powerType, StringComparison.InvariantCultureIgnoreCase)).Key;

                    itemTypes.Add(new ItemTypeInfo
                    {
                        Id = id,
                        Name = name,
                        Usability = Enum.Parse<ItemUsability>(usabilityKey),
                        PowerType = Enum.Parse<ItemPowerType>(powerTypeKey),
                        Slot1 = slot1,
                        Slot2 = slot2,
                        MinimumQualityLevelForUnidentified = unidentifiedQualityLevel,
                        UnidentifiedItemName = unidentifiedName,
                        UnidentifiedItemDescription = unidentifiedDescription,
                        UnidentifiedItemActionName = unidentifiedActionName,
                        UnidentifiedItemActionDescription = unidentifiedActionDescription
                    });
                }
            }

            if (validationErrors.Count == 0)
                LoadedItemTypeInfos = itemTypes;

            return validationErrors;
        }

        private void dgvItemTypes_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Let's leave the ComboBox empty if it holds an invalid (likely outdated) value
            if (dgvItemTypes.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn)
            {
                dgvItemTypes.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = null;

                e.ThrowException = false;
            }
        }
    }
}
