using org.matheval;
using RogueCustomsDungeonEditor.Utils;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
{
    public class DungeonNPCValidator
    {
        public static DungeonValidationMessages Validate(NPCInfo npcJson, DungeonInfo dungeonJson, Dungeon sampleDungeon)
        {
            var messages = new DungeonValidationMessages();

            messages.AddRange(DungeonCharacterValidator.Validate(npcJson, false, dungeonJson, sampleDungeon));
            if(npcJson.KnowsAllCharacterPositions)
            {
                messages.AddWarning("KnowsAllCharacterPositions is set to true but Sight Range covers the entire map. KnowsAllCharacterPositions will have no effect.");
            }
            if (npcJson.AIOddsToUseActionsOnSelf < 0)
                messages.AddError("AIOddsToUseActionsOnSelf must be 0 or higher.");
            else if (npcJson.AIOddsToUseActionsOnSelf == 0 && npcJson.InventorySize > 0)
                messages.AddWarning("AIOddsToUseActionsOnSelf is 0 but Inventory Size is above 0. It won't be able to use any items it carries.");
            else if (npcJson.AIOddsToUseActionsOnSelf > 0 && npcJson.InventorySize == 0)
                messages.AddWarning("AIOddsToUseActionsOnSelf is above 0 but Inventory Size is 0. Unable to carry any items, AIOddsToUseActionsOnSelf won't have any effect.");

            try
            {
                for (int i = 1; i <= npcJson.MaxLevel; i++)
                {
                    var payout = new Expression(npcJson.ExperiencePayoutFormula.Replace("level", i.ToString(), StringComparison.InvariantCultureIgnoreCase)).Eval<int>();
                    if (payout < 0)
                        messages.AddError($"Experience Payout formula returns a number lower than 0 at Level {i}, which is not valid.");
                }
            }
            catch (Exception ex)
            {
                messages.AddError($"Experience Payout formula is invalid: {ex.Message}.");
            }

            if(!dungeonJson.FloorInfos.Exists(fi => fi.PossibleMonsters.Exists(pm => pm.ClassId.Equals(npcJson.Id))))
                messages.AddWarning("Character is an NPC but does not show up in any list of PossibleMonsters. It will never be spawned. Consider adding it to a PossibleMonsters list.");

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}
