using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
{
    public static class DungeonItemSlotValidator
    {
        public static DungeonValidationMessages Validate(DungeonInfo dungeonJson)
        {
            var messages = new DungeonValidationMessages();

            foreach (var itemSlotJson in dungeonJson.ItemSlotInfos)
            {
                if (!string.IsNullOrWhiteSpace(itemSlotJson.Id))
                {
                    messages.AddRange(dungeonJson.ValidateString(itemSlotJson.Name, "Item Slot", "Name", true));
                }
                else
                {
                    messages.AddError($"Item Slot #{dungeonJson.ItemSlotInfos.IndexOf(itemSlotJson)} lacks an Id.");
                }
            }

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}
