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

namespace RogueCustomsDungeonEditor.Controls
{
    public partial class QuestItemRewardSheet : UserControl
    {
        private DungeonInfo dungeon = null;
        private readonly List<string> staticItemIds = ["Any"];
        private List<string> validItemIds = new();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DungeonInfo Dungeon
        {
            get => dungeon;
            set
            {
                this.dungeon = value;
                validItemIds = new(staticItemIds);
                foreach (var itemType in value.ItemTypeInfos)
                {
                    validItemIds.Add(itemType.Id);
                }
                foreach (var item in value.Items)
                {
                    validItemIds.Add(item.Id);
                }
                ItemId.DataSource = validItemIds;
                QualityLevel.DataSource = value.QualityLevelInfos.ConvertAll(qli => qli.Id);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<QuestItemRewardInfo> Rewards
        {
            get
            {
                var rewards = new List<QuestItemRewardInfo>();
                foreach (DataGridViewRow row in dgvItems.Rows)
                {
                    if (row.IsNewRow) continue;
                    var itemId = row.Cells[0].Value?.ToString();
                    if (string.IsNullOrEmpty(itemId)) continue;
                    var itemLevel = 1;
                    if (row.Cells[1].Value != null)
                        int.TryParse(row.Cells[1].Value.ToString(), out itemLevel);
                    var qualityLevel = row.Cells[2].Value?.ToString() ?? "";
                    rewards.Add(new QuestItemRewardInfo()
                    {
                        ItemId = itemId,
                        ItemLevel = itemLevel,
                        QualityLevel = qualityLevel
                    });
                }
                return rewards;
            }
            set
            {
                dgvItems.EndEdit();
                dgvItems.Rows.Clear();
                foreach (var reward in value)
                {
                    dgvItems.Rows.Add(reward.ItemId, reward.ItemLevel, reward.QualityLevel);
                }
            }
        }

        public event EventHandler RewardsChanged;

        public QuestItemRewardSheet()
        {
            InitializeComponent();
        }

        public void EndEdit()
        {
            dgvItems.EndEdit();
        }

        private void dgvItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            RewardsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void dgvItems_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                if (e.FormattedValue.ToString() != "" && (!int.TryParse(e.FormattedValue.ToString(), out int level) || level < 1))
                {
                    e.Cancel = true;
                }
            }
        }

        private void dgvItems_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            RewardsChanged?.Invoke(this, EventArgs.Empty);
        }
        private void dgvItems_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            RewardsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
