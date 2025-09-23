using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsDungeonEditor.Controls
{
    public partial class EquipmentSheet : UserControl
    {
        private DungeonInfo dungeon = null;
        private List<string> availableSlots = new();
        private Dictionary<string, List<string>> validItemsForSlots = new();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DungeonInfo Dungeon
        {
            get => dungeon;
            set
            {
                foreach (var slot in value.ItemSlotInfos)
                {
                    var validPicks = new List<string>();
                    foreach (var item in value.Items)
                    {
                        var correspondingItemType = value.ItemTypeInfos.Find(x => x.Id == item.ItemType);
                        if (correspondingItemType == null) continue;
                        if (correspondingItemType.Slot1.Equals(slot.Id, StringComparison.InvariantCultureIgnoreCase) || correspondingItemType.Slot2.Equals(slot.Id, StringComparison.InvariantCultureIgnoreCase))
                            validPicks.Add(item.Id);
                    }
                    validItemsForSlots[slot.Id] = validPicks;
                }
                dgvEquipment.Rows.Clear();
                foreach (var slot in value.ItemSlotInfos)
                {
                    dgvEquipment.Rows.Add(slot.Id, "");
                    var dropdown = (DataGridViewComboBoxCell)dgvEquipment.Rows[dgvEquipment.Rows.Count - 1].Cells[1];
                    dropdown.Items.Clear();
                    dropdown.Items.Add("");
                    dropdown.Items.AddRange(validItemsForSlots[slot.Id].ToArray());
                }
                dungeon = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<string> AvailableSlots
        {
            get => availableSlots;
            set
            {
                availableSlots = value;
                foreach (DataGridViewRow row in dgvEquipment.Rows)
                {
                    var slotId = row.Cells[0].Value.ToString();
                    row.Visible = availableSlots.Contains(slotId);
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<string> Equipment
        {
            set
            {
                if (value == null || value.Count == 0) return;
                foreach (var classId in value)
                {
                    foreach (DataGridViewRow row in dgvEquipment.Rows)
                    {
                        var slotId = row.Cells[0].Value.ToString();
                        if (validItemsForSlots[slotId].Contains(classId))
                        {
                            row.Cells[1].Value = classId;
                            break;
                        }
                    }
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Dictionary<string, string> EquipmentList
        {
            get
            {
                var equipment = new Dictionary<string, string>();
                foreach (DataGridViewRow row in dgvEquipment.Rows)
                {
                    if (row.IsNewRow || !row.Visible || row.Cells[1].Value == null || string.IsNullOrWhiteSpace(row.Cells[1].Value.ToString())) continue;
                    equipment[row.Cells[0].Value.ToString()] = row.Cells[1].Value.ToString();
                }
                return equipment;
            }
        }
        public EquipmentSheet()
        {
            InitializeComponent();
        }

        public void EndEdit()
        {
            dgvEquipment.EndEdit();
        }
    }
}
