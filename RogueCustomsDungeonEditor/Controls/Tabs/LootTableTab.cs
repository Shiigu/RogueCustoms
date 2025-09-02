using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.JsonImports;

using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

#pragma warning disable CA1416 // Validar la compatibilidad de la plataforma
namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    public partial class LootTableTab : UserControl
    {
        private List<string> ValidPickIds;
        private DungeonInfo ActiveDungeon;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LootTableInfo LoadedLootTable { get; private set; }
        public event EventHandler TabInfoChanged;

        private Color CategoryColor = Color.Violet;
        private Color ItemColor = Color.LightBlue;
        private Color LootTableColor = Color.Gold;

        public LootTableTab()
        {
            InitializeComponent();
        }

        public void LoadData(DungeonInfo activeDungeon, LootTableInfo lootTableToLoad)
        {
            ActiveDungeon = activeDungeon;
            LoadedLootTable = lootTableToLoad;
            ValidPickIds = ["No Drop", "Weapon", "Armor", "Equippable", "Consumable"];
            ValidPickIds.AddRange(activeDungeon.LootTableInfos.Where(lt => lootTableToLoad.Id == null || !lt.Id.Equals(lootTableToLoad.Id)).Select(lt => lt.Id));
            ValidPickIds.AddRange(activeDungeon.Items.Select(i => i.Id));
            var pickIdColumn = (DataGridViewComboBoxColumn)dgvLootTable.Columns["PickId"];
            pickIdColumn.DataSource = ValidPickIds;
            dgvLootTable.Rows.Clear();
            foreach (var entry in lootTableToLoad.Entries)
            {
                dgvLootTable.Rows.Add(entry.PickId, entry.Weight);
            }
            dgvLootTable.CellValueChanged += (sender, e) => TabInfoChanged(sender, e);

            dgvLootTable.EditingControlShowing += dgvLootTable_EditingControlShowing;
            dgvLootTable.CellPainting += dgvLootTable_CellPainting;
        }

        private void dgvLootTable_EditingControlShowing(object? sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox comboBox)
            {
                // This is to avoid redrawing
                comboBox.DrawItem -= ComboBox_DrawItem;

                comboBox.DrawMode = DrawMode.OwnerDrawFixed;
                comboBox.DrawItem += ComboBox_DrawItem;
            }

        }

        private void ComboBox_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                e.DrawBackground();

                bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

                if (e.Index >= 0 && e.Index < comboBox.Items.Count)
                {
                    var itemText = comboBox.Items[e.Index].ToString() ?? string.Empty;
                    if (!isSelected)
                    {
                        Color? backColor = GetColorForEntry(itemText);

                        if (backColor != null)
                        {
                            using Brush backBrush = new SolidBrush(backColor.Value);
                            e.Graphics.FillRectangle(backBrush, e.Bounds);
                        }
                    }
                    using Brush textBrush = new SolidBrush(Color.Black);
                    e.Graphics.DrawString(itemText, e.Font, textBrush, e.Bounds);
                }

                e.DrawFocusRectangle();
            }
        }

        private void dgvLootTable_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && !dgvLootTable.Rows[e.RowIndex].IsNewRow && dgvLootTable.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewComboBoxCell)
            {
                var cell = dgvLootTable.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewComboBoxCell;
                var itemText = cell.Value.ToString();
                Color? backColor = GetColorForEntry(itemText);

                e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
                e.Paint(e.ClipBounds, DataGridViewPaintParts.ContentBackground);

                using (Brush forebrush = new SolidBrush(Color.Black))
                using (Brush backbrush = new SolidBrush(backColor ?? cell.Style.BackColor))
                using (StringFormat format = new StringFormat())
                {
                    Rectangle rect = new Rectangle(e.CellBounds.X + 1, e.CellBounds.Y + 1, e.CellBounds.Width - 19, e.CellBounds.Height - 3);
                    format.LineAlignment = StringAlignment.Center;

                    e.Graphics.FillRectangle(backbrush, rect);
                    e.Graphics.DrawString(cell.FormattedValue.ToString(), e.CellStyle.Font, forebrush, rect, format);
                }

                e.Paint(e.ClipBounds, DataGridViewPaintParts.ErrorIcon);
                e.Paint(e.ClipBounds, DataGridViewPaintParts.Focus);
                e.Handled = true;
            }
        }

        private Color? GetColorForEntry(string entryText)
        {
            if (entryText.Equals("No Drop", StringComparison.InvariantCultureIgnoreCase) ||
                entryText.Equals("Weapon", StringComparison.InvariantCultureIgnoreCase) ||
                entryText.Equals("Armor", StringComparison.InvariantCultureIgnoreCase) ||
                entryText.Equals("Equippable", StringComparison.InvariantCultureIgnoreCase) ||
                entryText.Equals("Consumable", StringComparison.InvariantCultureIgnoreCase))
            {
                return CategoryColor;
            }
            else if (ActiveDungeon.LootTableInfos.Any(lt => lt.Id.Equals(entryText, StringComparison.InvariantCultureIgnoreCase)))
            {
                return LootTableColor;
            }
            else
            {
                return ItemColor;
            }
        }

        public List<string> SaveData(string id)
        {
            var validationErrors = new List<string>();
            var lootTableToSave = new LootTableInfo()
            {
                Id = id,
                Entries = new()
            };

            var alreadyPickedIds = new List<string>();

            foreach (DataGridViewRow row in dgvLootTable.Rows)
            {
                if (row.IsNewRow) continue;
                try
                {
                    var isValidEntry = true;
                    var pickId = row.Cells[0].Value.ToString();
                    if (string.IsNullOrWhiteSpace(pickId))
                    {
                        isValidEntry = false;
                        validationErrors.Add("At least one Loot Table Entry lacks an Id.");
                    }
                    else if (alreadyPickedIds.Contains(pickId, StringComparer.InvariantCultureIgnoreCase))
                    {
                        isValidEntry = false;
                        validationErrors.Add("At least one Loot Table Entry is a duplicate.");
                    }
                    else
                    {
                        alreadyPickedIds.Add(pickId);
                    }
                    if (!int.TryParse(row.Cells[1].Value.ToString(), out int weight) || weight <= 0)
                    {
                        isValidEntry = false;
                        validationErrors.Add("At least one Loot Table Entry has an invalid Weight value.\n\nIt must be an integer number higher than 0.");
                    }
                    if (isValidEntry)
                    {
                        lootTableToSave.Entries.Add(new()
                        {
                            PickId = pickId,
                            Weight = weight
                        });
                    }
                }
                catch
                {
                    validationErrors.Add("At least one Loot Table Entry is invalid.");
                }
            }

            if (!validationErrors.Any())
            {
                LoadedLootTable = lootTableToSave;
            }

            return validationErrors.Distinct().ToList();
        }

        private void btnTestLootTable_Click(object sender, EventArgs e)
        {
            var validationErrors = SaveData(LoadedLootTable.Id);
            if (validationErrors.Count > 0)
            {
                MessageBox.Show(
                    $"Cannot test the Loot Table. Please correct the following errors:\n- {string.Join("\n- ", validationErrors)}",
                    "Test Loot Table",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
            var lootTableIds = ActiveDungeon.LootTableInfos.ConvertAll(lt => lt.Id);
            var pickedRolls = new Dictionary<string, int>();
            var sampleRng = new RngHandler(Environment.TickCount);
            for (int i = 0; i < 100; i++)
            {
                var pickedId = string.Empty;
                var visitedLootTables = new List<string>();
                var currentLootTable = LoadedLootTable;
                if(!string.IsNullOrEmpty(LoadedLootTable.Id)) visitedLootTables.Add(LoadedLootTable.Id);
                while (pickedId?.Length == 0 || lootTableIds.Contains(pickedId))
                {
                    if (currentLootTable.Entries.Any(e => visitedLootTables.Contains(e.PickId)))
                    {
                        visitedLootTables.AddRange(currentLootTable.Entries.Where(e => visitedLootTables.Contains(e.PickId)).Select(e => e.PickId));
                        MessageBox.Show(
                            $"ALERT! POSSIBLE INFINITE LOOP DETECTED!\n\n{string.Join(" => ", visitedLootTables)}\n\nPlease correct this to avoid infinite loops.",
                            "Test Loot Table",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                        return;
                    }
                    else
                    {
                        pickedId = currentLootTable.Entries.TakeRandomElementWithWeights(e => e.Weight, sampleRng).PickId;
                        if (lootTableIds.Contains(pickedId) && !visitedLootTables.Contains(pickedId))
                        {
                            visitedLootTables.Add(pickedId);
                            currentLootTable = ActiveDungeon.LootTableInfos.Find(lt => lt.Id == pickedId);
                        }
                        else
                        {
                            if (!pickedRolls.ContainsKey(pickedId))
                                pickedRolls.Add(pickedId, 0);
                            pickedRolls[pickedId]++;
                        }
                    }
                }
            }
            var resultLines = new StringBuilder();
            foreach (var pickedRoll in pickedRolls)
            {
                resultLines.AppendLine($"- {pickedRoll.Key}: {pickedRoll.Value} time(s)");
            }
            MessageBox.Show(
                $"Running the Loot Table 100 times resulted in the following distribution:\n\n{resultLines.ToString()}\nPlease correct the chosen Weights if you find the results unsatisfactory.",
                "Test Loot Table",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
    }
}
#pragma warning restore CA1416 // Validar la compatibilidad de la plataforma
