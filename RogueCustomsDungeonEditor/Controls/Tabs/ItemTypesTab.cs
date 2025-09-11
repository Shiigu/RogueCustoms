using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public event EventHandler TabInfoChanged;
        public ItemTypesTab()
        {
            InitializeComponent();
        }

        public void LoadData(DungeonInfo activeDungeon)
        {
            ActiveDungeon = activeDungeon;
            LoadedItemTypeInfos = activeDungeon.ItemTypeInfos;
            ItemSlots = activeDungeon.ItemSlotInfos.ConvertAll(x => x.Id);
            var usabilityColumn = (DataGridViewComboBoxColumn)dgvItemTypes.Columns["Usability"];
            usabilityColumn.DataSource = ItemUsabilityOptions.Select(qlnao => qlnao.Value).ToList();
            var powerTypeColumn = (DataGridViewComboBoxColumn)dgvItemTypes.Columns["PowerType"];
            powerTypeColumn.DataSource = ItemPowerTypeOptions.Select(qlnao => qlnao.Value).ToList();
            var slot1Column = (DataGridViewComboBoxColumn)dgvItemTypes.Columns["Slot1"];
            slot1Column.DataSource = ItemSlots;
            var slot2Column = (DataGridViewComboBoxColumn)dgvItemTypes.Columns["Slot2"];
            slot2Column.DataSource = ItemSlots.Union([""]).ToList();
            dgvItemTypes.Rows.Clear();
            foreach (var itemType in LoadedItemTypeInfos)
            {
                dgvItemTypes.Rows.Add(itemType.Id, itemType.Name, ItemUsabilityOptions[itemType.Usability.ToString()], ItemPowerTypeOptions[itemType.PowerType.ToString()], itemType.Slot1, itemType.Slot2);
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
                var slot2 = (row.Cells[4].Value ?? string.Empty).ToString().Trim();

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
                if (string.IsNullOrWhiteSpace(slot1))
                {
                    isValidEntry = false;
                    validationErrors.Add($"Row {id}: Enter the Primary Slot first.");
                }
                if (!string.IsNullOrWhiteSpace(slot1) && !string.IsNullOrWhiteSpace(slot2) && slot1.Equals(slot2,StringComparison.CurrentCultureIgnoreCase))
                {
                    isValidEntry = false;
                    validationErrors.Add($"Row {id}: Primary and Secondary Slots are Identical. If you want the Item Type to occupy only one slot, leave the Secondary Slot blank.");
                }
                if (!itemTypeIds.Add(id))
                {
                    isValidEntry = false;
                    validationErrors.Add($"Row {row.Index + 1}: The Item Type Id '{id}' is duplicated.");
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
                        Slot2 = slot2
                    });
                }
            }

            if (validationErrors.Count == 0)
                LoadedItemTypeInfos = itemTypes;

            return validationErrors;
        }
    }
}
