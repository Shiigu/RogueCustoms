using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Newtonsoft.Json.Linq;

using RogueCustomsDungeonEditor.EffectInfos;
using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsDungeonEditor.Controls.Tabs
{
    public partial class QuestTab : UserControl
    {
        private DungeonInfo ActiveDungeon = null;
        private List<EffectTypeData> EffectParamData;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public QuestInfo LoadedQuest { get; private set; }
        public event EventHandler TabInfoChanged;

        private readonly List<string> staticNPCIds = ["Any"];
        private List<string> validNPCIds = new();

        private readonly List<string> staticItemIds = ["Any"];
        private List<string> validItemIds = new();

        private readonly List<string> staticConsumableIds = ["Any"];
        private List<string> validConsumableIds = new();

        private readonly List<string> staticAlteredStatusIds = ["Any"];
        private List<string> validAlteredStatusIds = new();

        private readonly Dictionary<QuestCompletionType, string> completionTypeToValidIdSourceKey = new()
        {
            { QuestCompletionType.AnyCondition, "Quest completes when any condition is fulfilled" },
            { QuestCompletionType.AllConditions, "Quest completes when all conditions are fulfilled" }
        };

        private readonly Dictionary<QuestConditionType, string> conditionTypeToValidIdSourceKey = new()
        {
            { QuestConditionType.KillNPCs, "Defeat NPCs" },
            { QuestConditionType.DealDamage, "Deal Damage" },
            { QuestConditionType.HealDamage, "Heal Damage" },
            { QuestConditionType.StatusNPCs, "Status NPCs" },
            { QuestConditionType.StatusSelf, "Status Yourself" },
            { QuestConditionType.CollectItems, "Collect Items" },
            { QuestConditionType.UseItems, "Consume Items" },
            { QuestConditionType.ReachFloor, "Reach Floor" },
            { QuestConditionType.ReachLevel, "Reach Level" },
            { QuestConditionType.ObtainCurrency, "Obtain Money" }
        };

        private readonly List<QuestConditionType> conditionsThatRequireAPick = new()
        {
            QuestConditionType.KillNPCs,
            QuestConditionType.StatusNPCs,
            QuestConditionType.CollectItems,
            QuestConditionType.UseItems
        };

        public QuestTab()
        {
            InitializeComponent();
        }

        public void LoadData(DungeonInfo activeDungeon, QuestInfo questToLoad, List<EffectTypeData> effectParamData)
        {
            ActiveDungeon = activeDungeon;
            LoadedQuest = questToLoad ?? new() { Id = "NewQuest" };
            EffectParamData = effectParamData;

            validNPCIds = new(staticNPCIds);
            validItemIds = new(staticItemIds);
            validConsumableIds = new(staticConsumableIds);
            validAlteredStatusIds = new(staticAlteredStatusIds);
            foreach (var npc in ActiveDungeon.NPCs)
            {
                validNPCIds.Add(npc.Id);
            }
            foreach (var faction in ActiveDungeon.FactionInfos)
            {
                validNPCIds.Add(faction.Id);
            }
            foreach (var item in ActiveDungeon.Items)
            {
                var itemType = ActiveDungeon.ItemTypeInfos.FirstOrDefault(iti => iti.Id == item.ItemType);
                validItemIds.Add(item.Id);
                if (itemType.Usability == ItemUsability.Use)
                {
                    validConsumableIds.Add(item.Id);
                }
            }
            foreach (var itemType in ActiveDungeon.ItemTypeInfos)
            {
                validItemIds.Add(itemType.Id);
            }
            foreach (var alteredStatus in ActiveDungeon.AlteredStatuses)
            {
                validAlteredStatusIds.Add(alteredStatus.Id);
            }

            cmConditionType.DataSource = conditionTypeToValidIdSourceKey.Values.ToList();
            cmbQuestMinimumConditions.DataSource = completionTypeToValidIdSourceKey.Values.ToList();

            txtQuestName.Text = LoadedQuest.Name;
            txtQuestDescription.Text = LoadedQuest.Description;
            chkQuestIsRepeatable.Checked = LoadedQuest.IsRepeatable;
            chkQuestIsAbandonedOnFloorChange.Checked = LoadedQuest.AbandonedOnFloorChange;
            cmbQuestMinimumConditions.SelectedItem = completionTypeToValidIdSourceKey[LoadedQuest.CompletionType];
            nudQuestMonetaryReward.Value = LoadedQuest.GuaranteedMonetaryReward;
            nudQuestExperienceReward.Value = LoadedQuest.GuaranteedExperienceReward;
            qirsGuaranteed.Dungeon = ActiveDungeon;
            qirsGuaranteed.Rewards = LoadedQuest.GuaranteedItemRewards ?? new();
            qirsSelectable.Dungeon = ActiveDungeon;
            qirsSelectable.Rewards = LoadedQuest.SelectableItemRewards ?? new();
            UpdateCompensationFields();
            nudQuestCompensationMonetaryReward.Value = LoadedQuest.CompensatoryMonetaryReward;
            nudQuestCompensationExperienceReward.Value = LoadedQuest.CompensatoryExperienceReward;
            dgvQuestConditions.Rows.Clear();
            foreach (var condition in LoadedQuest.Conditions ?? new())
            {
                var conditionTypeString = conditionTypeToValidIdSourceKey[condition.Type];

                var rowIndex = dgvQuestConditions.Rows.Add(conditionTypeString, "", condition.TargetValue);

                RefreshTargetIdColumn(rowIndex, condition.Type);

                dgvQuestConditions.Rows[rowIndex].Cells[1].Value = condition.TargetId;
            }
            saeQuestComplete.SetActionEditorParams(LoadedQuest.Id, LoadedQuest.OnQuestComplete, EffectParamData, activeDungeon, (_, _) => TabInfoChanged?.Invoke(null, EventArgs.Empty));
        }
        public List<string> SaveData(string id)
        {
            dgvQuestConditions.EndEdit();
            qirsGuaranteed.EndEdit();
            qirsSelectable.EndEdit();
            
            var validationErrors = new List<string>();

            if(string.IsNullOrWhiteSpace(txtQuestName.Text))
            {
                validationErrors.Add("The Quest must have a Name.");
            }

            if (string.IsNullOrWhiteSpace(txtQuestDescription.Text))
            {
                validationErrors.Add("The Quest must have a Description.");
            }

            var questConditions = new List<QuestConditionInfo>();

            foreach (DataGridViewRow row in dgvQuestConditions.Rows)
            {
                if (row.IsNewRow) continue;
                var isValidEntry = true;
                var conditionTypeString = Convert.ToString(row.Cells[0].Value);
                var targetId = Convert.ToString(row.Cells[1].Value);
                var targetValue = Convert.ToInt32(row.Cells[2].Value);
                var conditionType = conditionTypeToValidIdSourceKey.FirstOrDefault(kvp => kvp.Value == conditionTypeString).Key;
                if (string.IsNullOrWhiteSpace(conditionTypeString))
                {
                    isValidEntry = false;
                    validationErrors.Add($"Condition #{row.Index + 1} lacks a Type.");
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(targetId) && conditionsThatRequireAPick.Contains(conditionType))
                    {
                        isValidEntry = false;
                        validationErrors.Add($"Condition #{row.Index + 1} requires an Object.");
                    }
                }
                if (targetValue < 1)
                {
                    isValidEntry = false;
                    validationErrors.Add($"Condition #{row.Index + 1} has an invalid Value. It must be higher than 0.");
                }
                if (isValidEntry)
                {
                    questConditions.Add(new QuestConditionInfo()
                    {
                        Type = conditionType,
                        TargetId = targetId,
                        TargetValue = targetValue
                    });
                }
            }

            if(nudQuestMonetaryReward.Value == 0 && nudQuestExperienceReward.Value == 0 && nudQuestCompensationMonetaryReward.Value == 0 && nudQuestCompensationExperienceReward.Value == 0 && qirsGuaranteed.Rewards.Count == 0 && qirsSelectable.Rewards.Count == 0)
            {
                validationErrors.Add("The Quest must have at least one reward.");
            }

            if (qirsSelectable.Rewards.Count == 1)
            {
                validationErrors.Add("The Quest cannot have exactly one Selectable Reward. The amount of options must be zero or more than one.");
            }

            for (int i = 0; i < qirsGuaranteed.Rewards.Count; i++)
            {
                var guaranteedReward = qirsGuaranteed.Rewards[i];

                var itemInfo = ActiveDungeon.Items.Find(i => i.Id.Equals(guaranteedReward.ItemId));
                var itemTypeInfo = ActiveDungeon.ItemTypeInfos.Find(iti => iti.Id.Equals(guaranteedReward.ItemId));

                if (guaranteedReward.ItemId != "Any" && itemInfo == null && itemTypeInfo == null)
                    validationErrors.Add($"The Quest's Guaranteed Reward #{i} has an invalid Item Id.");

                var qualityLevel = ActiveDungeon.QualityLevelInfos.Find(qli => qli.Id.Equals(guaranteedReward.QualityLevel));
                if (qualityLevel == null)
                    validationErrors.Add($"The Quest's Guaranteed Reward #{i} has an invalid Quality Level.");
            }

            for (int i = 0; i < qirsSelectable.Rewards.Count; i++)
            {
                var selectableReward = qirsSelectable.Rewards[i];

                var itemInfo = ActiveDungeon.Items.Find(i => i.Id.Equals(selectableReward.ItemId));
                var itemTypeInfo = ActiveDungeon.ItemTypeInfos.Find(iti => iti.Id.Equals(selectableReward.ItemId));

                if (selectableReward.ItemId != "Any" && itemInfo == null && itemTypeInfo == null)
                    validationErrors.Add($"The Quest's Selectable Reward #{i} has an invalid Item Id.");

                var qualityLevel = ActiveDungeon.QualityLevelInfos.Find(qli => qli.Id.Equals(selectableReward.QualityLevel));
                if (qualityLevel == null)
                    validationErrors.Add($"The Quest's Selectable Reward #{i} has an invalid Quality Level.");
            }

            if (validationErrors.Count == 0)
            {
                LoadedQuest = new QuestInfo()
                {
                    Id = id,
                    Name = txtQuestName.Text,
                    Description = txtQuestDescription.Text,
                    IsRepeatable = chkQuestIsRepeatable.Checked,
                    AbandonedOnFloorChange = chkQuestIsAbandonedOnFloorChange.Checked,
                    CompletionType = completionTypeToValidIdSourceKey.FirstOrDefault(kvp => kvp.Value == cmbQuestMinimumConditions.SelectedItem.ToString()).Key,
                    GuaranteedMonetaryReward = (int)nudQuestMonetaryReward.Value,
                    GuaranteedExperienceReward = (int)nudQuestExperienceReward.Value,
                    GuaranteedItemRewards = qirsGuaranteed.Rewards,
                    SelectableItemRewards = qirsSelectable.Rewards,
                    CompensatoryMonetaryReward = (int)nudQuestCompensationMonetaryReward.Value,
                    CompensatoryExperienceReward = (int)nudQuestCompensationExperienceReward.Value,
                    Conditions = questConditions,
                    OnQuestComplete = saeQuestComplete.Action
                };
            }

            return validationErrors;
        }

        private void txtQuestName_TextChanged(object sender, EventArgs e)
        {
            txtQuestName.ToggleEntryInLocaleWarning(ActiveDungeon, fklblQuestNameLocale);
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void txtQuestDescription_TextChanged(object sender, EventArgs e)
        {
            txtQuestDescription.ToggleEntryInLocaleWarning(ActiveDungeon, fklblQuestDescriptionLocale);
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void chkQuestIsRepeatable_CheckedChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void chkQuestIsAbandonedOnFloorChange_CheckedChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void cmbQuestMinimumConditions_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void nudQuestMonetaryReward_ValueChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void nudQuestExperienceReward_ValueChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void nudQuestCompensationMonetaryReward_ValueChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void nudQuestCompensationExperienceReward_ValueChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
        }

        private void qirsGuaranteed_RewardsChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
            UpdateCompensationFields();
        }

        private void qirsSelectable_RewardsChanged(object sender, EventArgs e)
        {
            TabInfoChanged?.Invoke(null, EventArgs.Empty);
            UpdateCompensationFields();
        }

        private void UpdateCompensationFields()
        {
            if (qirsGuaranteed.Rewards.Count > 0 || qirsSelectable.Rewards.Count > 0)
            {
                nudQuestCompensationMonetaryReward.Enabled = true;
                nudQuestCompensationExperienceReward.Enabled = true;
            }
            else
            {
                nudQuestCompensationMonetaryReward.Enabled = false;
                nudQuestCompensationMonetaryReward.Value = 0;
                nudQuestCompensationExperienceReward.Enabled = false;
                nudQuestCompensationExperienceReward.Value = 0;
            }
        }

        private void dgvQuestConditions_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvQuestConditions.Rows[e.RowIndex].IsNewRow) return;

            TabInfoChanged?.Invoke(this, EventArgs.Empty);

            var cellValue = dgvQuestConditions.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();

            if (cellValue != null && e.ColumnIndex == 0)
            {
                var selectedConditionType = conditionTypeToValidIdSourceKey.FirstOrDefault(kvp => kvp.Value == cellValue).Key;
                var validIdSourceKey = conditionTypeToValidIdSourceKey[selectedConditionType];

                RefreshTargetIdColumn(e.RowIndex, selectedConditionType);
            }
        }

        private void RefreshTargetIdColumn(int rowIndex, QuestConditionType type)
        {
            var validIds = new List<string>();
            switch (type)
            {
                case QuestConditionType.KillNPCs:
                    validIds = validNPCIds;
                    break;
                case QuestConditionType.CollectItems:
                    validIds = validItemIds;
                    break;
                case QuestConditionType.UseItems:
                    validIds = validConsumableIds;
                    break;
                case QuestConditionType.StatusNPCs:
                case QuestConditionType.StatusSelf:
                    validIds = validAlteredStatusIds;
                    break;
                default:
                    validIds = new();
                    break;
            }
            var idCell = dgvQuestConditions.Rows[rowIndex].Cells[1] as DataGridViewComboBoxCell;
            idCell.Value = "";
            idCell.DataSource = validIds;
            if (validIds.Count == 0)
            {
                idCell.Value = null;
                idCell.ReadOnly = true;
            }
            else
            {
                idCell.ReadOnly = false;
            }
        }

        private void dgvQuestConditions_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.FormattedValue != string.Empty && (!int.TryParse(Convert.ToString(e.FormattedValue), out int amount) || amount < 1))
            {
                e.Cancel = true;
            }
        }
    }
}
