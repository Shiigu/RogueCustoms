using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
{
    public static class DungeonQuestValidator
    {
        public static async Task<DungeonValidationMessages> Validate(QuestInfo questJson, DungeonInfo dungeonJson, Dungeon sampleDungeon)
        {
            var messages = new DungeonValidationMessages();
            var questAsInstance = sampleDungeon != null ? new Quest(questJson, sampleDungeon) : null;

            if (!string.IsNullOrWhiteSpace(questJson.Id))
            {
                messages.AddRange(dungeonJson.ValidateString(questJson.Name, "Quest", "Name", true));
                messages.AddRange(dungeonJson.ValidateString(questJson.Description, "Quest", "Description", false));

                if (questJson.GuaranteedMonetaryReward < 0)
                    messages.AddError($"Quest has an invalid Guaranteed Monetary Reward. It must be a non-negative number.");

                if (questJson.GuaranteedExperienceReward < 0)
                    messages.AddError($"Quest has an invalid Guaranteed Experience Reward. It must be a non-negative number.");

                if (questJson.GuaranteedItemRewards.Count == 0 && questJson.SelectableItemRewards.Count == 0)
                {
                    if (questJson.CompensatoryMonetaryReward > 0)
                        messages.AddWarning($"Quest has set to give a Compensatory Monetary Reward, but it gives no Item Rewards. This will never be used.");

                    if (questJson.CompensatoryExperienceReward > 0)
                        messages.AddWarning($"Quest has set to give a Compensatory Experience Reward, but it gives no Item Rewards. This will never be used.");
                }
                else
                {
                    if (questJson.CompensatoryMonetaryReward < 0)
                        messages.AddError($"Quest has an invalid Compensatory Monetary Reward. It must be a non-negative number.");
                    if (questJson.CompensatoryExperienceReward < 0)
                        messages.AddError($"Quest has an invalid Compensatory Experience Reward. It must be a non-negative number.");
                }

                foreach (var condition in questJson.Conditions)
                {
                    if (condition.TargetValue <= 0)
                        messages.AddError($"Quest has a Condition with an invalid Target Value. It must be a number higher than 0.");
                    switch (condition.Type)
                    {
                        case QuestConditionType.KillNPCs:
                            if (condition.TargetId != "Any")
                            {
                                var npc = dungeonJson.NPCs.FirstOrDefault(npc => npc.Id == condition.TargetId);
                                if (npc != null) continue;
                                var faction = dungeonJson.FactionInfos.FirstOrDefault(f => f.Id == condition.TargetId);
                                if (faction != null) continue;
                                messages.AddError($"Quest has a Condition with an NPC Target of an invalid type: {condition.TargetId}.");
                            }
                            break;
                        case QuestConditionType.StatusNPCs:
                        case QuestConditionType.StatusSelf:
                            if (condition.TargetId != "Any")
                            {
                                var alteredStatus = dungeonJson.AlteredStatuses.FirstOrDefault(als => als.Id == condition.TargetId);
                                if (alteredStatus != null) continue;
                                messages.AddError($"Quest has a Condition with an Altered Status Target of an invalid type: {condition.TargetId}.");
                            }
                            break;
                        case QuestConditionType.CollectItems:
                        case QuestConditionType.UseItems:
                            if (condition.TargetId != "Any")
                            {
                                var item = dungeonJson.Items.FirstOrDefault(i => i.Id == condition.TargetId);
                                var itemType = dungeonJson.ItemTypeInfos.FirstOrDefault(it => it.Id == condition.TargetId);
                                if (item == null && itemType == null)
                                    messages.AddError($"Quest has a Condition with an Item Target of an invalid type: {condition.TargetId}.");
                                if(condition.Type == QuestConditionType.UseItems && itemType.Usability != ItemUsability.Use)
                                    messages.AddError($"Quest has a Condition to Use Items with an Item Target that cannot be used: {condition.TargetId}.");
                            }
                            break;
                        case QuestConditionType.DealDamage:
                        case QuestConditionType.HealDamage:
                        case QuestConditionType.ReachFloor:
                        case QuestConditionType.ReachLevel:
                        case QuestConditionType.ObtainCurrency:
                            if (condition.TargetId != "")
                            {
                                messages.AddWarning($"Quest a a Condition with a Target when it requires no Target. This will never be used.");
                            }
                            break;
                    }
                }

                foreach (var guaranteedItem in questJson.GuaranteedItemRewards)
                {
                    var item = dungeonJson.Items.FirstOrDefault(i => i.Id == guaranteedItem.ItemId);
                    var itemType = dungeonJson.ItemTypeInfos.FirstOrDefault(it => it.Id == guaranteedItem.ItemId);
                    if (item == null && itemType == null)
                        messages.AddError($"Quest has a Guaranteed Item Reward of an invalid type: {guaranteedItem.ItemId}.");
                    if (guaranteedItem.ItemLevel < 1)
                        messages.AddError($"Quest has a Guaranteed Item Reward with an invalid Item Level. It must be 1 or higher.");
                    var qualityLevel = dungeonJson.QualityLevelInfos.FirstOrDefault(ql => ql.Id == guaranteedItem.QualityLevel);
                    if (qualityLevel == null)
                        messages.AddError($"Quest has a Guaranteed Item Reward with an invalid Quality Level: {guaranteedItem.QualityLevel}.");
                }

                if (questJson.SelectableItemRewards.Count == 1)
                {
                    messages.AddWarning($"Quest has only one Selectable Item Reward. The amount must be 0 or higher than 1.");
                }
                else
                {
                    foreach (var selectableItem in questJson.SelectableItemRewards)
                    {
                        var item = dungeonJson.Items.FirstOrDefault(i => i.Id == selectableItem.ItemId);
                        var itemType = dungeonJson.ItemTypeInfos.FirstOrDefault(it => it.Id == selectableItem.ItemId);
                        if (item == null && itemType == null)
                            messages.AddError($"Quest has a Selectable Item Reward of an invalid type: {selectableItem.ItemId}.");
                        if (selectableItem.ItemLevel < 1)
                            messages.AddError($"Quest has a Selectable Item Reward with an invalid Item Level. It must be 1 or higher.");
                        var qualityLevel = dungeonJson.QualityLevelInfos.FirstOrDefault(ql => ql.Id == selectableItem.QualityLevel);
                        if (qualityLevel == null)
                            messages.AddError($"Quest has a Selectable Item Reward with an invalid Quality Level: {selectableItem.QualityLevel}.");
                    }
                }

                messages.AddRange(await ActionValidator.Validate(questJson.OnQuestComplete, dungeonJson));
                
                if (questAsInstance != null)
                {
                    messages.AddRange(await ActionValidator.Validate(questAsInstance.OnQuestComplete, dungeonJson, sampleDungeon));
                }
            }
            else
            {
                messages.AddError($"Quest #{dungeonJson.QuestInfos.IndexOf(questJson)} lacks an Id.");
            }

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}
