using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.Helpers;

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

        public QuestDto(Quest quest)
        {
            Id = quest.Id;
            Name = quest.Name;
            Description = quest.Description;

            switch(quest.CompletionType)
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
}
