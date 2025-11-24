using System;
using System.Linq;

using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
{
    public static class DungeonItemTypeValidator
    {
        public static DungeonValidationMessages Validate(DungeonInfo dungeonJson)
        {
            var messages = new DungeonValidationMessages();

            foreach (var itemTypeJson in dungeonJson.ItemTypeInfos)
            {
                if (!string.IsNullOrWhiteSpace(itemTypeJson.Id))
                {
                    if (dungeonJson.ItemTypeInfos.Any(a => a != itemTypeJson && a.Id.Equals(itemTypeJson.Id, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        messages.AddError($"Item Type {itemTypeJson.Id} has a duplicate Id.");
                    }
                    messages.AddRange(dungeonJson.ValidateString(itemTypeJson.Name, "Item Type", "Name", true));

                    if (itemTypeJson.Usability == ItemUsability.Equip)
                    {
                        if (!string.IsNullOrWhiteSpace(itemTypeJson.Slot1))
                        {
                            var slot1 = dungeonJson.ItemSlotInfos.Find(qli => qli.Id.Equals(itemTypeJson.Slot1));
                            if (slot1 == null)
                                messages.AddError($"Item Type {itemTypeJson.Id} has an invalid Primary Item Slot.");
                        }
                        else
                        {
                            messages.AddError($"Item Type {itemTypeJson.Id} has no Primary Item Slot set.");
                        }
                        if (!string.IsNullOrWhiteSpace(itemTypeJson.Slot2))
                        {
                            if (itemTypeJson.Slot1.Equals(itemTypeJson.Slot2, StringComparison.InvariantCultureIgnoreCase))
                            {
                                messages.AddError($"Item Type {itemTypeJson.Id} has an invalid Secondary Item Slot. It's identical to its Primary Item Slot.");
                            }
                            else
                            {

                                var slot2 = dungeonJson.ItemSlotInfos.Find(qli => qli.Id.Equals(itemTypeJson.Slot2));
                                if (slot2 == null)
                                    messages.AddError($"Item Type {itemTypeJson.Id} has an invalid Secondary Item Slot.");
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(itemTypeJson.Slot1))
                        {
                            messages.AddError($"Item Type {itemTypeJson.Id} has a Primary Item Slot even though it's not Equippable.");
                        }
                        if (!string.IsNullOrWhiteSpace(itemTypeJson.Slot2))
                        {
                            messages.AddError($"Item Type {itemTypeJson.Id} has a Primary Item Slot even though it's not Equippable.");
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(itemTypeJson.MinimumQualityLevelForUnidentified))
                    {
                        var minimumQualityLevelForUnidentified = dungeonJson.QualityLevelInfos.Find(qli => qli.Id.Equals(itemTypeJson.MinimumQualityLevelForUnidentified));
                        if (minimumQualityLevelForUnidentified == null)
                        {
                            messages.AddError($"Item Type {itemTypeJson.Id} has an invalid Minimum Quality Level to set an Item as Unidentified.");
                        }
                        else
                        {
                            messages.AddRange(dungeonJson.ValidateString(itemTypeJson.UnidentifiedItemName, "Item Type", "Unidentified Item Name", true));
                            messages.AddRange(dungeonJson.ValidateString(itemTypeJson.UnidentifiedItemDescription, "Item Type", "Unidentified Item Description", true));
                            messages.AddRange(dungeonJson.ValidateString(itemTypeJson.UnidentifiedItemActionName, "Item Type", "Unidentified Item Action Name", true));
                            messages.AddRange(dungeonJson.ValidateString(itemTypeJson.UnidentifiedItemActionDescription, "Item Type", "Unidentified Item Action Description", true));
                        }

                    }
                }
                else
                {
                    messages.AddError($"Item Type #{dungeonJson.ItemTypeInfos.IndexOf(itemTypeJson)} lacks an Id.");
                }
            }

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}
