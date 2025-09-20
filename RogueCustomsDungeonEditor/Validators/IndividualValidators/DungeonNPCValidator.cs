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

            var npcAsInstance = sampleDungeon != null ? new NonPlayableCharacter(new EntityClass(npcJson, sampleDungeon, dungeonJson.CharacterStats), 1, sampleDungeon.CurrentFloor) : null;

            if(npcJson.RegularLootTable != null)
            {
                if ((npcJson.RegularLootTable.LootTableId == "None" || string.IsNullOrWhiteSpace(npcJson.RegularLootTable.LootTableId)) && npcJson.RegularLootTable.DropPicks > 0)
                    messages.AddError("This NPC is set to drop items as loot, but has no Loot Table.");
                else if (npcJson.RegularLootTable.LootTableId != "None" && !string.IsNullOrWhiteSpace(npcJson.RegularLootTable.LootTableId) && npcJson.RegularLootTable.DropPicks <= 0)
                    messages.AddError("This NPC has a Loot Table but is not set to drop items as loot.");
            }
            else
            {
                messages.AddError("This NPC has no regular Loot Table data, not even an empty one.");
            }
            if (npcJson.LootTableWithModifiers != null)
            {
                if (npcJson.LootTableWithModifiers.LootTableId != "None" && !string.IsNullOrWhiteSpace(npcJson.LootTableWithModifiers.LootTableId) && npcJson.LootTableWithModifiers.DropPicks <= 0)
                    messages.AddError("This NPC has a Loot Table when with modifiers but is not set to drop items as loot.");
                if (npcJson.RegularLootTable != null)
                {
                    if(npcJson.LootTableWithModifiers.LootTableId == "None" && npcJson.LootTableWithModifiers.LootTableId != "None")
                        messages.AddWarning("This NPC has a Loot Table except when with Modifiers.");
                }
            }
            else
            {
                messages.AddError("This NPC has no modified Loot Table data, not even an empty one.");
            }

            if (npcJson.OnSpawn != null)
            {
                messages.AddRange(await ActionValidator.Validate(npcJson.OnSpawn, dungeonJson));
                if(npcAsInstance != null)
                    messages.AddRange(await ActionValidator.Validate(npcAsInstance.OwnOnSpawn, dungeonJson, sampleDungeon));
            }

            if (npcJson.BaseHPMultiplierIfWithModifiers < 1)
            {
                messages.AddError("NPC has a non-positive Base HP Multiplier when with Modifiers.");
            }

            if (npcJson.ExperienceYieldMultiplierIfWithModifiers < 0)
            {
                messages.AddError("NPC has a negative Experience Yield Multiplier when with Modifiers.");
            }
            else if (npcJson.ExperienceYieldMultiplierIfWithModifiers < 1)
            {
                messages.AddError("NPC has an Experience Yield Multiplier when with Modifiers that is lower than 1, meaning they will give less experience than normal.");
            }

            if (npcJson.RandomizesForecolorIfWithModifiers && !npcJson.ModifierData.Any(md => md.ModifierAmount > 0))
            {
                messages.AddWarning("This NPC is set to randomize their Foreground with Modifiers, but is not set to have any Modifiers. This property will be ignored.");
            }

            var playerFactions = dungeonJson.PlayerClasses.Select(pc => pc.Faction);
            var npcFaction = dungeonJson.FactionInfos.Find(fi => fi.Id.Equals(npcJson.Faction));

            if (npcFaction != null && npcJson.ReappearsOnTheNextFloorIfAlliedToThePlayer && npcFaction.EnemiesWith.Any(playerFactions.Contains))
            {
                messages.AddWarning("This NPC is set to tag with an Allied Player between Floors, but is set to be enemies with at least one Player. This property will be ignored unless modified through gameplay.");
            }

            if (npcAsInstance != null && npcAsInstance.OnInteracted.Any())
            {
                if (npcAsInstance.OnInteracted.HasMinimumMatches(ooa => ooa.Id.ToLower(), 2))
                {
                    messages.AddError("NPC has at least two Interacted actions with the same Id.");
                }

                foreach (var onInteractedAction in npcJson.OnInteracted)
                {
                    messages.AddRange(await ActionValidator.Validate(onInteractedAction, dungeonJson));
                }
                if (npcAsInstance != null)
                {
                    foreach (var onInteractedAction in npcAsInstance.OnInteracted)
                    {
                        foreach (var playerClass in dungeonJson.PlayerClasses)
                        {
                            if (playerClass.OnAttack.Any(oa => oa.Id.Equals(onInteractedAction.Id, StringComparison.InvariantCultureIgnoreCase)))
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
