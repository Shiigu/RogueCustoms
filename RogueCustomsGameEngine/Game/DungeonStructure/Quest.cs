using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsGameEngine.Game.DungeonStructure
{
    [Serializable]
    public class Quest
    {
        public int Id { get; set; }
        public string QuestId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsRepeatable { get; set; }
        public bool AbandonedOnFloorChange { get; set; }
        public List<QuestCondition> Conditions { get; set; }
        public QuestCompletionType CompletionType { get; set; }
        public int GuaranteedMonetaryReward { get; set; }
        public int GuaranteedExperienceReward { get; set; }
        public List<QuestItemReward> GuaranteedItemRewards { get; set; }
        public List<QuestItemReward> SelectableItemRewards { get; set; }
        public int CompensatoryMonetaryReward { get; set; }
        public int CompensatoryExperienceReward { get; set; }
        public ActionWithEffects OnQuestComplete { get; set; }

        public Map Map { get; set; }
        public QuestStatus Status { get; set; }
        public int ItemRewardSeed { get; set; }

        public override string ToString() => $"#{Id} => Quest: {Name} (Status: {Status})";

        public Quest(QuestInfo info, Dungeon dungeon)
        {
            QuestId = info.Id;
            Name = dungeon.LocaleToUse[info.Name];
            Description = dungeon.LocaleToUse[info.Description];
            IsRepeatable = info.IsRepeatable;
            AbandonedOnFloorChange = info.AbandonedOnFloorChange;
            OnQuestComplete = ActionWithEffects.Create(info.OnQuestComplete, dungeon.ActionSchools);
            Conditions = new List<QuestCondition>();
            foreach (var conditionInfo in info.Conditions)
            {
                var condition = new QuestCondition
                {
                    Type = conditionInfo.Type,
                    TargetValue = conditionInfo.TargetValue,
                    CurrentValue = 0,
                    TargetId = conditionInfo.TargetId
                };

                switch (condition.Type)
                {
                    case QuestConditionType.KillNPCs:
                        if (conditionInfo.TargetId != "Any")
                        {
                            var existingNPCClass = dungeon.NPCClasses.Find(x => x.Id == conditionInfo.TargetId);
                            if (existingNPCClass != null)
                            {
                                condition.TargetClass = existingNPCClass;
                            }
                            else
                            {
                                var existingFaction = dungeon.Factions.Find(x => x.Id == conditionInfo.TargetId);
                                if (existingFaction != null)
                                {
                                    condition.TargetFaction = existingFaction;
                                }
                                else
                                {
                                    throw new MissingFieldException($"Quest {QuestId} has an NPC Target Condition of an invalid type: {conditionInfo.TargetId}");
                                }
                            }
                        }
                        break;
                    case QuestConditionType.StatusNPCs:
                    case QuestConditionType.StatusSelf:
                        if (conditionInfo.TargetId != "Any")
                        {
                            var existingAlteredStatusClass = dungeon.AlteredStatusClasses.Find(x => x.Id == conditionInfo.TargetId);
                            if (existingAlteredStatusClass != null)
                            {
                                condition.TargetClass = existingAlteredStatusClass;
                            }
                            else
                            {
                                throw new MissingFieldException($"Quest {QuestId} has an Altered Status Target Condition of an invalid type: {conditionInfo.TargetId}");
                            }
                        }
                        break;
                    case QuestConditionType.CollectItems:
                    case QuestConditionType.UseItems:
                        if (conditionInfo.TargetId != "Any")
                        {
                            var existingItemClass = dungeon.ItemClasses.Find(x => x.Id == conditionInfo.TargetId);
                            if (existingItemClass != null)
                            {
                                condition.TargetClass = existingItemClass;
                            }
                            else
                            {
                                var existingItemType = dungeon.ItemTypes.Find(x => x.Id == conditionInfo.TargetId);
                                if (existingItemType != null)
                                {
                                    if(condition.Type == QuestConditionType.UseItems && existingItemType.Usability != ItemUsability.Use)
                                        throw new InvalidOperationException($"Quest {QuestId} has an Item Use Target Condition that cannot be used: {conditionInfo.TargetId}");
                                    condition.TargetItemType = existingItemType;
                                }
                                else
                                {
                                    throw new MissingFieldException($"Quest {QuestId} has an Item Target Condition of an invalid type: {conditionInfo.TargetId}");
                                }
                            }
                        }
                        break;
                }

                Conditions.Add(condition);
            }
            CompletionType = info.CompletionType;
            GuaranteedMonetaryReward = info.GuaranteedMonetaryReward;
            GuaranteedExperienceReward = info.GuaranteedExperienceReward;
            GuaranteedItemRewards = new List<QuestItemReward>();
            foreach (var itemRewardInfo in info.GuaranteedItemRewards)
            {
                var itemReward = new QuestItemReward
                {
                    ItemLevel = itemRewardInfo.ItemLevel
                };
                itemReward.QualityLevel = dungeon.QualityLevels.Find(x => x.Id == itemRewardInfo.QualityLevel);
                if (itemRewardInfo.ItemId != "Any")
                {
                    var existingItemClass = dungeon.ItemClasses.Find(x => x.Id == itemRewardInfo.ItemId);
                    if (existingItemClass != null)
                    {
                        itemReward.BaseItem = existingItemClass;
                    }
                    else
                    {
                        var existingItemType = dungeon.ItemTypes.Find(x => x.Id == itemRewardInfo.ItemId);
                        if (existingItemType != null)
                        {
                            itemReward.BaseItemType = existingItemType;
                        }
                        else
                        {
                            throw new MissingFieldException($"Quest {QuestId} has an Item Reward of an invalid type: {itemRewardInfo.ItemId}");
                        }
                    }
                }
                GuaranteedItemRewards.Add(itemReward);
            }
            SelectableItemRewards = new List<QuestItemReward>();
            foreach (var itemRewardInfo in info.SelectableItemRewards)
            {
                var itemReward = new QuestItemReward
                {
                    ItemLevel = itemRewardInfo.ItemLevel
                };
                itemReward.QualityLevel = dungeon.QualityLevels.Find(x => x.Id == itemRewardInfo.QualityLevel);
                if (itemRewardInfo.ItemId != "Any")
                {
                    var existingItemClass = dungeon.ItemClasses.Find(x => x.Id == itemRewardInfo.ItemId);
                    if (existingItemClass != null)
                    {
                        itemReward.BaseItem = existingItemClass;
                    }
                    else
                    {
                        var existingItemType = dungeon.ItemTypes.Find(x => x.Id == itemRewardInfo.ItemId);
                        if (existingItemType != null)
                        {
                            itemReward.BaseItemType = existingItemType;
                        }
                        else
                        {
                            throw new MissingFieldException($"Quest {QuestId} has an Item Reward of an invalid type: {itemRewardInfo.ItemId}");
                        }
                    }
                }
                SelectableItemRewards.Add(itemReward);
            }
            CompensatoryMonetaryReward = info.CompensatoryMonetaryReward;
            CompensatoryExperienceReward = info.CompensatoryExperienceReward;
        }

        private Quest() { }

        public async Task<Quest> Instantiate(Map map)
        {
            var instantiatedQuest = new Quest
            {
                QuestId = QuestId,
                Name = Name,
                Description = Description,
                IsRepeatable = IsRepeatable,
                AbandonedOnFloorChange = AbandonedOnFloorChange,
                Conditions = Conditions.ConvertAll(c => new QuestCondition
                {
                    Type = c.Type,
                    TargetClass = c.TargetClass,
                    TargetItemType = c.TargetItemType,
                    TargetFaction = c.TargetFaction,
                    TargetValue = c.TargetValue,
                    CurrentValue = 0,
                    TargetId = c.TargetId
                }),
                CompletionType = CompletionType,
                GuaranteedMonetaryReward = GuaranteedMonetaryReward,
                GuaranteedExperienceReward = GuaranteedExperienceReward,
                GuaranteedItemRewards = GuaranteedItemRewards.ConvertAll(r => new QuestItemReward
                {
                    BaseItem = r.BaseItem,
                    BaseItemType = r.BaseItemType,
                    ItemLevel = r.ItemLevel,
                    QualityLevel = r.QualityLevel
                }),
                SelectableItemRewards = SelectableItemRewards.ConvertAll(r => new QuestItemReward
                {
                    BaseItem = r.BaseItem,
                    BaseItemType = r.BaseItemType,
                    ItemLevel = r.ItemLevel,
                    QualityLevel = r.QualityLevel
                }),
                CompensatoryMonetaryReward = CompensatoryMonetaryReward,
                CompensatoryExperienceReward = CompensatoryExperienceReward,
                OnQuestComplete = OnQuestComplete?.Clone(),
                Status = QuestStatus.InProgress,
                Map = map,
                ItemRewardSeed = map.Rng.Next(),
                Id = map.GenerateEntityId()
            };

            await instantiatedQuest.SetUpInitialConditions();

            return instantiatedQuest;
        }

        private async Task SetUpInitialConditions()
        {
            // Setting conditions related to current-state
            foreach (var condition in Conditions)
            {
                switch (condition.Type)
                {
                    case QuestConditionType.CollectItems:
                        if (condition.TargetClass != null)
                            condition.CurrentValue = Map.Player.Inventory.Count(i => i.ClassId.Equals(condition.TargetClass.Id));
                        else if (condition.TargetItemType != null)
                            condition.CurrentValue = Map.Player.Inventory.Count(i => i.ItemType.Id.Equals(condition.TargetItemType.Id));
                        else
                            condition.CurrentValue = Map.Player.Inventory.Count;
                        break;
                    case QuestConditionType.ReachFloor:
                        condition.CurrentValue = Map.FloorLevel;
                        break;
                    case QuestConditionType.ReachLevel:
                        condition.CurrentValue = Map.Player.Level;
                        break;
                    case QuestConditionType.ObtainCurrency:
                        condition.CurrentValue = Map.Player.CurrencyCarried;
                        break;
                }
            }

            if ((CompletionType == QuestCompletionType.AnyCondition && Conditions.Any(c => c.CurrentValue >= c.TargetValue))
                || (CompletionType == QuestCompletionType.AllConditions && Conditions.All(c => c.CurrentValue >= c.TargetValue)))
            {
                await Complete();
            }
        }

        public async Task UpdateConditions(QuestConditionType type, string targetId, int amount)
        {
            foreach (var condition in Conditions)
            {
                if (condition.Type == type)
                {
                    switch (type)
                    {
                        case QuestConditionType.KillNPCs:
                            if (condition.TargetClass != null && condition.TargetClass.Id == targetId)
                                condition.CurrentValue++;
                            else if (condition.TargetFaction != null && condition.TargetFaction.Id == targetId)
                                condition.CurrentValue++;
                            else if (condition.TargetClass == null && condition.TargetFaction == null && !Map.Dungeon.Factions.Any(f => f.Id.Equals(targetId))) // Ignoring calls from Faction prevents "Any" kills from being counted twice
                                condition.CurrentValue++;
                            break;
                        case QuestConditionType.DealDamage:
                        case QuestConditionType.HealDamage:
                            condition.CurrentValue += amount;
                            break;
                        case QuestConditionType.StatusNPCs:
                        case QuestConditionType.StatusSelf:
                            if (condition.TargetClass != null && condition.TargetClass.Id == targetId)
                                condition.CurrentValue += amount;
                            else if (condition.TargetClass == null)
                                condition.CurrentValue += amount;
                            break;
                        case QuestConditionType.CollectItems:
                        case QuestConditionType.UseItems:
                            if (condition.TargetClass != null && condition.TargetClass.Id == targetId)
                                condition.CurrentValue += amount;
                            else if (condition.TargetItemType != null && condition.TargetItemType.Id == targetId)
                                condition.CurrentValue += amount;
                            else if (condition.TargetClass == null && condition.TargetItemType == null)
                                condition.CurrentValue += amount;
                            break;
                        case QuestConditionType.ReachFloor:
                        case QuestConditionType.ReachLevel:
                        case QuestConditionType.ObtainCurrency:
                            condition.CurrentValue = amount;
                            break;
                    }
                }
            }
            if ((CompletionType == QuestCompletionType.AnyCondition && Conditions.Any(c => c.CurrentValue >= c.TargetValue))
                || (CompletionType == QuestCompletionType.AllConditions && Conditions.All(c => c.CurrentValue >= c.TargetValue)))
            {
                await Complete();
            }
        }

        private async Task Complete()
        {
            Map.AwaitingQuestInput = true;
            Status = QuestStatus.Completed;

            var events = new List<DisplayEventDto>();

            var monetaryReward = GuaranteedMonetaryReward;
            var experienceReward = GuaranteedExperienceReward;

            var generatorRng = new RngHandler(ItemRewardSeed);

            events.Add(new()
            {
                DisplayEventType = DisplayEventType.PlaySpecialEffect,
                Params = new() { SpecialEffect.QuestComplete }
            });

            Map.AppendMessage(Map.Locale["QuestCompleteText"].Format(new { CharacterName = Map.Player.Name, QuestName = Name }), new GameColor(Color.FromArgb(0, 255, 0)), events);
            Map.AddMessageBox(Map.Locale["QuestCompleteHeaderText"], Map.Locale["QuestCompleteMessageText"].Format(new { QuestName = Name }), "OK", new GameColor(Color.FromArgb(0, 255, 0)), events);

            Map.DisplayEvents.Add(($"{Map.Player.Name} completed Quest {Name}", events));

            events = new();

            if (GuaranteedItemRewards.Count + (SelectableItemRewards.Count > 0 ? 1 : 0) + Map.Player.Inventory.Count > Map.Player.InventorySize)
            {
                monetaryReward += CompensatoryMonetaryReward;
                experienceReward += CompensatoryExperienceReward;
                Map.AppendMessage(Map.Locale["InventoryIsFullForQuestText"].Format(new { CharacterName = Map.Player.Name }), Color.DarkOrange, events);
            }
            else
            {
                var guaranteedItems = new List<Item>();

                foreach (var reward in GuaranteedItemRewards)
                {
                    Item item = null;
                    if (reward.BaseItem != null)
                    {
                        item = new Item(reward.BaseItem, reward.ItemLevel, Map, generatorRng);
                    }
                    else if (reward.BaseItemType != null)
                    {
                        var possibleItemClasses = Map.PossibleItemClasses.Where(ic => ic.CanDrop && ic.ItemType.Id.Equals(reward.BaseItemType.Id));
                        var pickedClass = possibleItemClasses.TakeRandomElement(generatorRng);
                        item = new Item(pickedClass, reward.ItemLevel, Map, generatorRng);
                    }
                    else
                    {
                        var pickedClass = Map.PossibleItemClasses.Where(ic => ic.CanDrop).TakeRandomElement(generatorRng);
                        item = new Item(pickedClass, reward.ItemLevel, Map, generatorRng);
                    }
                    item.Id = Map.GenerateEntityId();
                    item.SetQualityLevel(reward.QualityLevel, generatorRng);
                    guaranteedItems.Add(item);
                }

                foreach (var item in guaranteedItems)
                {
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.PlaySpecialEffect,
                        Params = new() { SpecialEffect.ItemGet }
                    });

                    Map.Player.Inventory.Add(item);
                    item.Position = null;

                    Map.AppendMessage(Map.Locale["CharacterObtainedAnItem"].Format(new { CharacterName = Map.Player.Name, ItemName = item.Name }), Color.DeepSkyBlue, events);
                    Map.Player.InformRefreshedPlayerData(events);
                }

                var selectableItems = new List<Item>();

                foreach (var reward in SelectableItemRewards)
                {
                    Item item = null;
                    if (reward.BaseItem != null)
                    {
                        item = new Item(reward.BaseItem, reward.ItemLevel, Map, generatorRng);
                    }
                    else if (reward.BaseItemType != null)
                    {
                        var possibleItemClasses = Map.PossibleItemClasses.Where(ic => ic.CanDrop && ic.ItemType.Id.Equals(reward.BaseItemType.Id));
                        var pickedClass = possibleItemClasses.TakeRandomElement(generatorRng);
                        item = new Item(pickedClass, reward.ItemLevel, Map, generatorRng);
                    }
                    else
                    {
                        var pickedClass = Map.PossibleItemClasses.Where(ic => ic.CanDrop).TakeRandomElement(generatorRng);
                        item = new Item(pickedClass, reward.ItemLevel, Map, generatorRng);
                    }
                    item.Id = Map.GenerateEntityId();
                    item.SetQualityLevel(reward.QualityLevel, generatorRng);
                    selectableItems.Add(item);
                }

                if(selectableItems.Count > 0)
                {
                    var optionDtos = new InventoryDto()
                    {
                        CurrencyConsoleRepresentation = Map.CurrencyClass.ConsoleRepresentation.Clone()
                    };
                    foreach (var item in selectableItems)
                    {
                        optionDtos.InventoryItems.Add(new InventoryItemDto(item, Map.Player, Map, false)
                        {
                            CanBeUsed = true
                        });
                    }
                    var triggerPromptEvent = new DisplayEventDto()
                    {
                        DisplayEventType = DisplayEventType.TriggerPrompt,
                        Params = [false]
                    };
                    events.Add(triggerPromptEvent);
                    Map.DisplayEvents.Add(($"Open Select Reward For Quest {Name} Prompt", events));

                    while (!(bool)triggerPromptEvent.Params[0]) await Task.Delay(10);

                    var chosenOption = await Map.OpenSelectItem(Map.Locale["SelectQuestRewardText"], optionDtos, false);

                    var chosenItem = selectableItems.Find(i => i.Id == chosenOption.Id);

                    if(chosenItem != null)
                    {
                        events.Add(new()
                        {
                            DisplayEventType = DisplayEventType.PlaySpecialEffect,
                            Params = new() { SpecialEffect.ItemGet }
                        });

                        Map.Player.Inventory.Add(chosenItem);
                        chosenItem.Position = null;

                        Map.AppendMessage(Map.Locale["CharacterObtainedAnItem"].Format(new { CharacterName = Map.Player.Name, ItemName = chosenItem.Name }), Color.DeepSkyBlue, events);
                        Map.Player.InformRefreshedPlayerData(events);
                    }
                }
            }

            if (monetaryReward > 0)
            {
                var currencyPile = Map.CreateCurrency(monetaryReward);
                Map.Player.CurrencyCarried += monetaryReward;
                events.Add(new()
                {
                    DisplayEventType = DisplayEventType.PlaySpecialEffect,
                    Params = new() { SpecialEffect.Currency }
                });
                Map.AppendMessage(Map.Locale["CharacterPicksCurrency"].Format(new { CharacterName = Map.Player.Name, CurrencyName = currencyPile.Name }), events);
            }

            if (experienceReward > 0)
            {
                Map.AppendMessage(Map.Locale["CharacterGainsExperience"].Format(new { CharacterName = Map.Player.Name, Amount = experienceReward.ToString() }), Color.DeepSkyBlue, events);
                await Map.Player.GainExperience(experienceReward);
            }

            Map.Player.InformRefreshedPlayerData(events);
                        
            Map.DisplayEvents.Add(($"{Map.Player.Name} gets Rewards from Quest {Name}", events));

            OnQuestComplete?.Do(Map.Player, Map.Player, false);

            Map.AwaitingQuestInput = false;
        }
    }

    [Serializable]
    public class QuestCondition
    {
        public QuestConditionType Type { get; set; }
        public EntityClass TargetClass { get; set; }
        public ItemType TargetItemType { get; set; }
        public Faction TargetFaction { get; set; }
        public int TargetValue { get; set; }
        public int CurrentValue { get; set; }
        public string TargetId { get; set; }
    }

    [Serializable]
    public class QuestItemReward
    {
        public EntityClass BaseItem { get; set; }
        public ItemType BaseItemType { get; set; }
        public int ItemLevel { get; set; }
        public QualityLevel QualityLevel { get; set; }
    }
}
