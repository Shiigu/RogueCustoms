using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsGameEngine.Utils.InputsAndOutputs
{
    [Serializable]
    public class QuestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CompletionTypeMessage { get; set; }
        public List<QuestConditionDto> Conditions { get; set; }
        public List<QuestRewardDto> Rewards { get; set; }
        public ConsoleRepresentation CurrencyConsoleRepresentation { get; set; }

        public int FreeSlotsRequiredForRewards { get; set; }

        public QuestDto(Quest quest)
        {
            Id = quest.Id;
            Name = quest.Name;
            Description = quest.Description;
            CurrencyConsoleRepresentation = quest.Map.CurrencyClass.ConsoleRepresentation;

            switch (quest.CompletionType)
            {
                case QuestCompletionType.AllConditions:
                    CompletionTypeMessage = quest.Map.Locale["MustFulfillAllObjectivesText"];
                    break;
                case QuestCompletionType.AnyCondition:
                    CompletionTypeMessage = quest.Map.Locale["MustFulfillAnyObjectiveText"];
                    break;
            }

            Conditions = new List<QuestConditionDto>();
            foreach (var condition in quest.Conditions)
            {
                Conditions.Add(new QuestConditionDto(condition, quest.Map));
            }

            FreeSlotsRequiredForRewards = quest.GuaranteedItemRewards.Count + (quest.SelectableItemRewards.Count > 0 ? 1 : 0);

            Rewards = new List<QuestRewardDto>();

            if(quest.GuaranteedMonetaryReward > 0)
                Rewards.Add(new(quest.GuaranteedMonetaryReward, QuestRewardType.GuaranteedMoney));

            if(quest.GuaranteedExperienceReward > 0)
                Rewards.Add(new(quest.GuaranteedExperienceReward, QuestRewardType.GuaranteedExperience));

            foreach (var reward in quest.GuaranteedItemRewards)
            {
                Rewards.Add(new QuestRewardDto(reward, QuestRewardType.GuaranteedItem, quest.Map));
            }

            foreach (var reward in quest.SelectableItemRewards)
            {
                Rewards.Add(new QuestRewardDto(reward, QuestRewardType.SelectableItem, quest.Map));
            }

            if(quest.CompensatoryMonetaryReward > 0)
                Rewards.Add(new(quest.CompensatoryMonetaryReward, QuestRewardType.CompensatoryMoney));

            if (quest.CompensatoryExperienceReward > 0)
                Rewards.Add(new(quest.CompensatoryExperienceReward, QuestRewardType.CompensatoryExperience));

            if(quest.OnQuestComplete != null)
                Rewards.Add(new(quest.OnQuestComplete));
        }
    }

    [Serializable]
    public class QuestConditionDto
    {
        public bool IsFulfilled { get; set; }
        public string Description { get; set; }
        public int CurrentValue { get; set; }
        public int TargetValue { get; set; }

        public QuestConditionDto(QuestCondition condition, Map map)
        {
            IsFulfilled = condition.CurrentValue >= condition.TargetValue;
            CurrentValue = condition.CurrentValue;
            TargetValue = condition.TargetValue;

            var targetName = string.Empty;
            switch (condition.Type)
            {
                case QuestConditionType.KillNPCs:
                    if (condition.TargetClass != null)
                        targetName = condition.TargetClass.Name;
                    else if (condition.TargetFaction != null)
                        targetName = condition.TargetFaction.Name;
                    else
                        targetName = map.Locale["AnyNPCObjectiveText"];

                    Description = map.Locale["DefeatNPCsObjectiveText"].Format(new { TargetId = targetName });
                    break;
                case QuestConditionType.DealDamage:
                    Description = map.Locale["DealDamageObjectiveText"];
                    break;
                case QuestConditionType.HealDamage:
                    Description = map.Locale["HealDamageObjectiveText"];
                    break;
                case QuestConditionType.StatusNPCs:
                    if (condition.TargetClass != null)
                        targetName = condition.TargetClass.Name;
                    else
                        targetName = map.Locale["AnyStatusObjectiveText"];

                    Description = map.Locale["StatusNPCsObjectiveText"].Format(new { TargetId = targetName });
                    break;
                case QuestConditionType.StatusSelf:
                    if (condition.TargetClass != null)
                        targetName = condition.TargetClass.Name;
                    else
                        targetName = map.Locale["AnyStatusObjectiveText"];

                    Description = map.Locale["StatusSelfObjectiveText"].Format(new { TargetId = targetName });
                    break;
                case QuestConditionType.CollectItems:
                    if (condition.TargetClass != null)
                        targetName = condition.TargetClass.Name;
                    else if (condition.TargetItemType != null)
                        targetName = condition.TargetItemType.Name;
                    else
                        targetName = map.Locale["AnyItemObjectiveText"];

                    Description = map.Locale["CollectItemsObjectiveText"].Format(new { TargetId = targetName });
                    break;
                case QuestConditionType.UseItems:
                    if (condition.TargetClass != null)
                        targetName = condition.TargetClass.Name;
                    else if (condition.TargetItemType != null)
                        targetName = condition.TargetItemType.Name;
                    else
                        targetName = map.Locale["AnyItemObjectiveText"];

                    Description = map.Locale["ConsumeItemsObjectiveText"].Format(new { TargetId = targetName });
                    break;
                case QuestConditionType.ReachFloor:
                    Description = map.Locale["ReachFloorObjectiveText"];
                    break;
                case QuestConditionType.ReachLevel:
                    Description = map.Locale["ReachLevelObjectiveText"];
                    break;
                case QuestConditionType.ObtainCurrency:
                    Description = map.Locale["ObtainMoneyObjectiveText"].Format(new { CurrencyChar = map.CurrencyClass.ConsoleRepresentation.Character });
                    break;
            }
        }
    }

    [Serializable]
    public class QuestRewardDto
    {
        public int ItemLevel { get; set; }
        public string QualityLevel { get; set; }
        public string Name { get; set; }
        public QuestRewardType Type { get; set; }
        public int Amount { get; set; }

        public QuestRewardDto(QuestItemReward reward, QuestRewardType type, Map map)
        {
            ItemLevel = reward.ItemLevel;
            QualityLevel = reward.QualityLevel.Name.Replace("{baseName}", string.Empty).Trim();
            if(reward.BaseItem != null)
            {
                Name = reward.BaseItem.Name;
            }
            else if (reward.BaseItemType != null)
            {
                Name = reward.BaseItemType.Name;
            }
            Type = type;
        }

        public QuestRewardDto(ActionWithEffects action)
        {
            Name = action.Name;
            Type = QuestRewardType.Action;
        }

        public QuestRewardDto(int amount, QuestRewardType type)
        {
            Amount = amount;
            Type = type;
        }
    }
}
