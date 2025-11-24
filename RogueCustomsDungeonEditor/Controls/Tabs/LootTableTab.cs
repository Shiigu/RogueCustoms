using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.JsonImports;

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

        private readonly Color ItemTypeColor = Color.Violet;
        private readonly Color ItemColor = Color.LightBlue;
        private readonly Color LootTableColor = Color.FromArgb(0, 255, 0);
        private readonly Color CurrencyColor = Color.Gold;

        public LootTableTab()
        {
            InitializeComponent();
        }

        public void LoadData(DungeonInfo activeDungeon, LootTableInfo lootTableToLoad)
        {
            ActiveDungeon = activeDungeon;
            LoadedLootTable = lootTableToLoad;
            ValidPickIds = ["No Drop", "Equippable"];
            ValidPickIds.AddRange(activeDungeon.ItemTypeInfos.Select(i => i.Id));
            ValidPickIds.AddRange(activeDungeon.LootTableInfos.Where(lt => lootTableToLoad.Id == null || !lt.Id.Equals(lootTableToLoad.Id)).Select(lt => lt.Id));
            ValidPickIds.AddRange(activeDungeon.Items.Select(i => i.Id));
            foreach (var currencyPile in activeDungeon.CurrencyInfo.CurrencyPiles)
            {
                ValidPickIds.Add($"Currency ({currencyPile.Id})");
            }
            var pickIdColumn = (DataGridViewComboBoxColumn)dgvLootTable.Columns["PickId"];
            pickIdColumn.DataSource = ValidPickIds;

            dgvLootTable.Rows.Clear();
            dgvLootTable.EditingControlShowing += dgvLootTable_EditingControlShowing;
            dgvLootTable.CellPainting += dgvLootTable_CellPainting;
            dgvLootTable.DataError += dgvLootTable_DataError;
            foreach (var entry in lootTableToLoad.Entries)
            {
                dgvLootTable.Rows.Add(entry.PickId, entry.Weight);
            }
            dgvLootTable.CellValueChanged += (sender, e) => TabInfoChanged(sender, e);

            lblItemType.BackColor = ItemTypeColor;
            lblLootTable.BackColor = LootTableColor;
            lblCurrency.BackColor = CurrencyColor;
            lblItem.BackColor = ItemColor;

            chkLootTableOverridesQualityLevelOdds.Checked = lootTableToLoad.OverridesQualityLevelOddsOfItems;

            if (lootTableToLoad.QualityLevelOdds == null)
                lootTableToLoad.QualityLevelOdds = [];

            var qualityLevelsTable = new List<QualityLevelOddsInfo>();

            foreach (var qualityLevel in activeDungeon.QualityLevelInfos)
            {
                var lootTableQualityLevelEntry = lootTableToLoad.QualityLevelOdds.Find(ql => ql.Id.Equals(qualityLevel.Id, StringComparison.InvariantCultureIgnoreCase));
                if (lootTableQualityLevelEntry != null)
                {
                    qualityLevelsTable.Add(lootTableQualityLevelEntry);
                }
                else
                {
                    qualityLevelsTable.Add(new QualityLevelOddsInfo()
                    {
                        Id = qualityLevel.Id,
                        ChanceToPick = 0
                    });
                }
            }

            qlsLootTableQualityLevelOdds.QualityLevels = qualityLevelsTable;
        }

        public List<string> SaveData(string id)
        {
            qlsLootTableQualityLevelOdds.EndEdit();
            dgvLootTable.EndEdit();
            var validationErrors = new List<string>();
            var lootTableToSave = new LootTableInfo()
            {
                Id = id,
                Entries = new(),
                OverridesQualityLevelOddsOfItems = chkLootTableOverridesQualityLevelOdds.Checked,
                QualityLevelOdds = new()
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

            if (!validationErrors.Any() && chkLootTableOverridesQualityLevelOdds.Checked)
            {
                foreach (var qualityLevel in qlsLootTableQualityLevelOdds.QualityLevels)
                {
                    if (qualityLevel.ChanceToPick < 0)
                    {
                        validationErrors.Add("At least one Quality Level Odds Entry has an invalid Weight value.\n\nIt must be an integer number equal to or higher than 0.");
                    }
                    else
                    {
                        lootTableToSave.QualityLevelOdds.Add(qualityLevel);
                    }
                }
            }

            if (!validationErrors.Any())
            {
                LoadedLootTable = lootTableToSave;
            }

            return validationErrors.Distinct().ToList();
        }

        private void chkLootTableOverridesQualityLevelOdds_CheckedChanged(object sender, EventArgs e)
        {
            qlsLootTableQualityLevelOdds.Visible = chkLootTableOverridesQualityLevelOdds.Checked;
        }

        private void dgvLootTable_DataError(object? sender, DataGridViewDataErrorEventArgs e)
        {
            // Let's leave the ComboBox empty if it holds an invalid (likely outdated) value
            if (dgvLootTable.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn)
            {
                dgvLootTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = null;

                e.ThrowException = false;
            }
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
                var itemText = cell.Value != null
                    ? cell.Value.ToString()
                    : cell.FormattedValue != null ? cell.FormattedValue.ToString() : string.Empty;
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
            var match = Regex.Match(entryText, EngineConstants.CurrencyRegexPattern);

            if (ActiveDungeon.ItemTypeInfos.Any(it => it.Id.Equals(entryText, StringComparison.InvariantCultureIgnoreCase)))
            {
                return ItemTypeColor;
            }
            else if (ActiveDungeon.LootTableInfos.Any(lt => lt.Id.Equals(entryText, StringComparison.InvariantCultureIgnoreCase)))
            {
                return LootTableColor;
            }
            else if (ActiveDungeon.Items.Any(i => i.Id.Equals(entryText, StringComparison.InvariantCultureIgnoreCase)))
            {
                return CurrencyColor;
            }
            else if (match.Success)
            {
                return CurrencyColor;
            }
            else
            {
                return Color.White;
            }
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
                if (!string.IsNullOrEmpty(LoadedLootTable.Id)) visitedLootTables.Add(LoadedLootTable.Id);
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
