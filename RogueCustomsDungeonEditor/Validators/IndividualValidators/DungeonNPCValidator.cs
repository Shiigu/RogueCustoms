using org.matheval;
using RogueCustomsDungeonEditor.Utils;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.Enums;
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
    public static class DungeonNPCValidator
    {
        public static async Task<DungeonValidationMessages> Validate(NPCInfo npcJson, DungeonInfo dungeonJson, Dungeon sampleDungeon)
        {
            var messages = new DungeonValidationMessages();

            messages.AddRange(await DungeonCharacterValidator.Validate(npcJson, false, dungeonJson, sampleDungeon));

            var npcAsInstance = new NonPlayableCharacter(new EntityClass(npcJson, sampleDungeon.LocaleToUse, EntityType.NPC, dungeonJson.CharacterStats, sampleDungeon.ActionSchools), 1, sampleDungeon.CurrentFloor);

            if (npcJson.OnSpawn != null)
            {
                messages.AddRange(await ActionValidator.Validate(npcJson.OnSpawn, dungeonJson, sampleDungeon));
                messages.AddRange(await ActionValidator.Validate(npcAsInstance.OnSpawn, dungeonJson, sampleDungeon));
            }

            if (npcAsInstance.OnInteracted.Any())
            {
                if (npcAsInstance.OnInteracted.HasMinimumMatches(ooa => ooa.Id.ToLower(), 2))
                {
                    messages.AddError("NPC has at least two Interacted actions with the same Id.");
                }

                foreach (var onInteractedAction in npcJson.OnInteracted)
                {
                    messages.AddRange(await ActionValidator.Validate(onInteractedAction, dungeonJson, sampleDungeon));
                }
                foreach (var onInteractedAction in npcAsInstance.OnInteracted)
                {
                    foreach (var playerClass in dungeonJson.PlayerClasses)
                    {
                        if(playerClass.OnAttack.Any(oa => oa.Id.Equals(onInteractedAction.Id, StringComparison.InvariantCultureIgnoreCase)))
                            messages.AddError($"NPC's Interacted action, {onInteractedAction.Id}, has the same Id as one of Player Class {playerClass.Id}'s Attack actions.");
                    }
                    foreach (var item in dungeonJson.Items)
                    {
                        if (item.OnAttack.Any(oa => oa.Id.Equals(onInteractedAction.Id, StringComparison.InvariantCultureIgnoreCase)))
                            messages.AddError($"NPC's Interacted action, {onInteractedAction.Id}, has the same Id as one of Item {item.Id}'s Attack actions.");
                    }
                    messages.AddRange(await ActionValidator.Validate(onInteractedAction, dungeonJson, sampleDungeon));
                }
            }

            if (npcJson.KnowsAllCharacterPositions)
            {
                messages.AddWarning("KnowsAllCharacterPositions is set to true but Sight Range covers the entire map. KnowsAllCharacterPositions will have no effect.");
            }
            if (string.IsNullOrWhiteSpace(npcJson.AIType))
                messages.AddError("AIType must not be empty.");
            else if (!Enum.TryParse<AIType>(npcJson.AIType, true, out _))
                messages.AddError("AIType does not contain a valid value.");

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
