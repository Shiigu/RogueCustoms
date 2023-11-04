using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.Controls
{
    public partial class StartingInventorySelector : UserControl
    {
        public List<string> Inventory
        {
            get
            {
                var items = new List<string>();

                foreach (string inventoryItem in lbStartingInventory.Items)
                {
                    items.Add(inventoryItem);
                }

                return items;
            }
            set
            {
                lbStartingInventory.Items.Clear();

                foreach (var inventoryItem in value)
                {
                    lbStartingInventory.Items.Add(inventoryItem);
                }

                btnAdd.Enabled = lbStartingInventory.Items.Count < InventorySize;
            }
        }

        public int ItemCount => lbStartingInventory.Items.Count;

        private List<string> selectableItems;

        public List<string> SelectableItems
        {
            get
            {
                return selectableItems;
            }
            set
            {
                selectableItems = value;
                cmbItemChoices.Items.Clear();
                if (value == null) return;
                foreach (var item in value)
                    cmbItemChoices.Items.Add(item);
            }
        }

        public int InventorySize { get; set; }

        public event EventHandler InventoryContentsChanged;

        public StartingInventorySelector()
        {
            InitializeComponent();
        }

        private void cmbItemChoices_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnAdd.Enabled = !string.IsNullOrWhiteSpace(cmbItemChoices.Text) && ItemCount < InventorySize;
        }

        private void lbStartingInventory_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRemove.Enabled = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            lbStartingInventory.Items.Add(cmbItemChoices.Text);
            cmbItemChoices.SelectedItem = null;
            InventoryContentsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            lbStartingInventory.Items.Remove(lbStartingInventory.SelectedItem);
            btnAdd.Enabled = !string.IsNullOrWhiteSpace(cmbItemChoices.Text) && ItemCount < InventorySize;
            InventoryContentsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
