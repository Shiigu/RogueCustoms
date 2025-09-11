using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    public partial class ItemSlotsTab : UserControl
    {
        private DungeonInfo ActiveDungeon;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<ItemSlotInfo> LoadedItemSlotInfos { get; private set; }
        public event EventHandler TabInfoChanged;
        public ItemSlotsTab()
        {
            InitializeComponent();
        }

        public void LoadData(DungeonInfo activeDungeon)
        {
            ActiveDungeon = activeDungeon;
            LoadedItemSlotInfos = activeDungeon.ItemSlotInfos;
            dgvItemSlots.Rows.Clear();
            foreach (var itemSlot in LoadedItemSlotInfos)
            {
                dgvItemSlots.Rows.Add(itemSlot.Id, itemSlot.Name);
            }
            dgvItemSlots.CellValueChanged += (sender, e) => TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        public List<string> SaveData()
        {
            var validationErrors = new List<string>();
            var itemSlotIds = new HashSet<string>();
            var itemSlots = new List<ItemSlotInfo>();
            dgvItemSlots.EndEdit();
            foreach (DataGridViewRow row in dgvItemSlots.Rows)
            {
                if (row.IsNewRow) continue;

                var isValidEntry = true;
                var id = (row.Cells[0].Value ?? string.Empty).ToString().Trim();
                var name = (row.Cells[1].Value ?? string.Empty).ToString().Trim();

                if (string.IsNullOrWhiteSpace(id))
                {
                    isValidEntry = false;
                    validationErrors.Add($"Row {row.Index + 1}: Enter the Item Slot Id first.");
                }
                if (string.IsNullOrWhiteSpace(name))
                {
                    isValidEntry = false;
                    validationErrors.Add($"Row {id}: Enter the Item Slot Name first.");
                }
                if (!itemSlotIds.Add(id))
                {
                    isValidEntry = false;
                    validationErrors.Add($"Row {row.Index + 1}: The Item Slot Id '{id}' is duplicated.");
                }
                if (isValidEntry)
                {
                    itemSlots.Add(new ItemSlotInfo
                    {
                        Id = id,
                        Name = name
                    });
                }
            }

            if (validationErrors.Count == 0)
                LoadedItemSlotInfos = itemSlots;

            return validationErrors;
        }
    }
}
