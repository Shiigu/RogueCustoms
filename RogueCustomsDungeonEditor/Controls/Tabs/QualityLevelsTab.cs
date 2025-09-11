using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    public partial class QualityLevelsTab : UserControl
    {
        private Dictionary<string, string> QualityLevelNameAttachmentOptions = new()
        {
            { "None", "Don't modify" },
            { "Affixes", "Attach Affixes" },
            { "QualityLevel", "Attach Quality Level" }
        };

        private DungeonInfo ActiveDungeon;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<QualityLevelInfo> LoadedQualityInfos { get; private set; }
        private Dictionary<string, GameColor> QualityLevelColors = [];
        public event EventHandler TabInfoChanged;
        public QualityLevelsTab()
        {
            InitializeComponent();
        }

        public void LoadData(DungeonInfo activeDungeon)
        {
            ActiveDungeon = activeDungeon;
            LoadedQualityInfos = activeDungeon.QualityLevelInfos;
            var attachesWhatToItemNameColumn = (DataGridViewComboBoxColumn)dgvQualityLevels.Columns["AttachesWhatToItemName"];
            attachesWhatToItemNameColumn.DataSource = QualityLevelNameAttachmentOptions.Select(qlnao => qlnao.Value).ToList();
            dgvQualityLevels.Rows.Clear();
            QualityLevelColors.Clear();
            foreach (var qualityLevel in LoadedQualityInfos)
            {
                dgvQualityLevels.Rows.Add(
                    qualityLevel.Id,
                    qualityLevel.Name,
                    qualityLevel.MinimumAffixes,
                    qualityLevel.MaximumAffixes,
                    QualityLevelNameAttachmentOptions[qualityLevel.AttachesWhatToItemName],
                    string.Empty
                );
                QualityLevelColors.Add(qualityLevel.Id, qualityLevel.ItemNameColor);
            }
        }

        public List<string> SaveData()
        {
            var validationErrors = new List<string>();
            var qualityLevels = new List<QualityLevelInfo>();
            var qualityLevelIds = new HashSet<string>();
            dgvQualityLevels.EndEdit();
            foreach (DataGridViewRow row in dgvQualityLevels.Rows)
            {
                if (row.IsNewRow) continue;

                var isValidEntry = true;
                var id = (row.Cells[0].Value ?? string.Empty).ToString().Trim();
                var name = (row.Cells[1].Value ?? string.Empty).ToString().Trim();
                var minAffixesStr = (row.Cells[2].Value ?? "0").ToString().Trim();
                var maxAffixesStr = (row.Cells[3].Value ?? "0").ToString().Trim();
                var nameAttachment = (row.Cells[4].Value ?? "None").ToString().Trim();
                var nameColor = QualityLevelColors[id];

                if (string.IsNullOrWhiteSpace(id))
                {
                    isValidEntry = false;
                    validationErrors.Add($"Row {row.Index + 1}: Enter the Quality Level Id first.");
                }
                if (string.IsNullOrWhiteSpace(name))
                {
                    isValidEntry = false;
                    validationErrors.Add($"Row {id}: Enter the Quality Level Name first.");
                }
                if (!int.TryParse(minAffixesStr, out int minAffixes) || minAffixes < 0)
                {
                    isValidEntry = false;
                    validationErrors.Add($"Row {id}: The Minimum Affixes must be a non-negative integer.");
                }
                if (!int.TryParse(maxAffixesStr, out int maxAffixes) || maxAffixes < 0)
                {
                    isValidEntry = false;
                    validationErrors.Add($"Row {id}: The Maximum Affixes must be a non-negative integer.");
                }
                if (minAffixes > maxAffixes)
                {
                    isValidEntry = false;
                    validationErrors.Add($"Row {id}: The Minimum Affixes cannot be greater than the Maximum Affixes.");
                }
                if (!QualityLevelNameAttachmentOptions.Any(qlnao => qlnao.Value.Equals(nameAttachment, StringComparison.InvariantCultureIgnoreCase)))
                {
                    isValidEntry = false;
                    validationErrors.Add($"Row {id}: The Name Attachment option is invalid.");
                }
                if (!qualityLevelIds.Add(id))
                {
                    isValidEntry = false;
                    validationErrors.Add($"Row {row.Index + 1}: The Quality Level Id '{id}' is duplicated.");
                }
                if (isValidEntry)
                {
                    var attachmentKey = "None";
                    if (QualityLevelNameAttachmentOptions.Any(qlnoa => qlnoa.Value.Equals(nameAttachment, StringComparison.InvariantCultureIgnoreCase)))
                        attachmentKey = QualityLevelNameAttachmentOptions.First(qlnoa => qlnoa.Value.Equals(nameAttachment, StringComparison.InvariantCultureIgnoreCase)).Key;

                    qualityLevels.Add(new QualityLevelInfo
                    {
                        Id = id,
                        Name = name,
                        MinimumAffixes = minAffixes,
                        MaximumAffixes = maxAffixes,
                        AttachesWhatToItemName = attachmentKey,
                        ItemNameColor = nameColor
                    });
                }
            }

            if (validationErrors.Count == 0)
                LoadedQualityInfos = qualityLevels;

            return validationErrors;
        }

        private void dgvQualityLevels_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != 5 || dgvQualityLevels.Rows[e.RowIndex].IsNewRow) return;
            var id = string.Empty;
            foreach (DataGridViewRow row in dgvQualityLevels.Rows)
            {
                if (row.Index != e.RowIndex) continue;
                id = (row.Cells[0].Value ?? string.Empty).ToString();
            }
            if (string.IsNullOrWhiteSpace(id)) return;
            var buttonCell = dgvQualityLevels.Rows[e.RowIndex].Cells[5] as DataGridViewButtonCell;
            if (buttonCell == null) return;
            var correspondingQualityLevelColor = QualityLevelColors.ContainsKey(id) ? QualityLevelColors[id] : new GameColor(Color.White);
            var initialColor = correspondingQualityLevelColor.ToColor();
            (DialogResult Result, Color pickedColor) = ColorDialogHandler.Show(initialColor);
            if (Result == DialogResult.OK)
            {
                correspondingQualityLevelColor = new GameColor(pickedColor);
                if (!QualityLevelColors.ContainsKey(id))
                    QualityLevelColors.Add(id, correspondingQualityLevelColor);
                TabInfoChanged(sender, e);
            }
        }

        private void dgvQualityLevels_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != 5 || dgvQualityLevels.Rows[e.RowIndex].IsNewRow) return;
            var id = string.Empty;
            foreach (DataGridViewRow row in dgvQualityLevels.Rows)
            {
                if (row.Index != e.RowIndex) continue;
                id = (row.Cells[0].Value ?? string.Empty).ToString();
            }
            if (string.IsNullOrWhiteSpace(id)) return;
            var buttonCell = dgvQualityLevels.Rows[e.RowIndex].Cells[5] as DataGridViewButtonCell;
            if (buttonCell == null) return;
            if (QualityLevelColors.Count <= e.RowIndex) return;
            var correspondingQualityLevelColor = QualityLevelColors.ContainsKey(id) ? QualityLevelColors[id] : new GameColor(Color.White);

            e.PaintBackground(e.CellBounds, true);
            using (var brush = new SolidBrush(correspondingQualityLevelColor.ToColor()))
            {
                e.Graphics.FillRectangle(brush, e.CellBounds);
            }

            // Draw border/text
            e.Paint(e.CellBounds, DataGridViewPaintParts.Border);
            e.Handled = true;
        }

        private void dgvQualityLevels_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvQualityLevels.Rows[e.RowIndex].IsNewRow) return;
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}
