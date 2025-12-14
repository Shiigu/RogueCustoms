using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Utils.Enums;

namespace RogueCustomsGameEngine.Utils.JsonImports
{
    [Serializable]
    public class QuestInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsRepeatable { get; set; }
        public bool AbandonedOnFloorChange { get; set; }
        public List<QuestConditionInfo> Conditions { get; set; }
        public QuestCompletionType CompletionType { get; set; }
        public int GuaranteedMonetaryReward { get; set; }
        public int GuaranteedExperienceReward { get; set; }
        public List<QuestItemRewardInfo> GuaranteedItemRewards { get; set; }
        public List<QuestItemRewardInfo> SelectableItemRewards { get; set; }
        public int CompensatoryMonetaryReward { get; set; }
        public int CompensatoryExperienceReward { get; set; }
        public ActionWithEffectsInfo OnQuestComplete { get; set; }
    }

    [Serializable]
    public class QuestConditionInfo
    {
        public QuestConditionType Type { get; set; }
        public string TargetId { get; set; }
        public int TargetValue { get; set; }
    }

    [Serializable]
    public class QuestItemRewardInfo
    {
        public string ItemId { get; set; }
        public int ItemLevel { get; set; }
        public string QualityLevel { get; set; }
    }
}
