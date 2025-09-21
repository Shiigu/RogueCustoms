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
    public static class DungeonCharacterValidator
    {
        public static async Task<DungeonValidationMessages> Validate(CharacterInfo characterJson, bool isPlayerCharacter, DungeonInfo dungeonJson, Dungeon sampleDungeon)
        {
            Character characterAsInstance = sampleDungeon != null ? (isPlayerCharacter
                                        ? new PlayerCharacter(new EntityClass(characterJson, sampleDungeon, dungeonJson.CharacterStats), 1, sampleDungeon.CurrentFloor)
                                        : new NonPlayableCharacter(new EntityClass(characterJson, sampleDungeon, dungeonJson.CharacterStats), 1, sampleDungeon.CurrentFloor))
                                        : null;

            var messages = new DungeonValidationMessages();

            messages.AddRange(dungeonJson.ValidateString(characterJson.Name, "Character", "Name", true));
            messages.AddRange(dungeonJson.ValidateString(characterJson.Description, "Character", "Description", false));

            if (string.IsNullOrWhiteSpace(characterJson.Faction))
            {
                messages.AddError("Character does not have a faction.");
            }
            else if (!dungeonJson.FactionInfos.Exists(fi => characterJson.Faction.Equals(fi.Id)))
            {
                messages.AddError($"Faction {characterJson.Faction} could not be found.");
            }

            messages.AddRange(characterJson.ConsoleRepresentation.Validate(characterJson.Id, isPlayerCharacter, dungeonJson));

            if (characterJson.OnTurnStart != null)
            {
                messages.AddRange(await ActionValidator.Validate(characterJson.OnTurnStart, dungeonJson));
                if (characterAsInstance != null)
                    messages.AddRange(await ActionValidator.Validate(characterAsInstance.OwnOnTurnStart, dungeonJson, sampleDungeon));
            }

            var missingMandatoryStats = FormConstants.MandatoryStats.Except(FormConstants.MandatoryStats.Intersect(characterJson.Stats.Select(s => s.StatId)));

            if (missingMandatoryStats.Any())
            {
                messages.AddError($"The following mandatory stats are missing: {missingMandatoryStats.JoinAnd()}");
            }

            foreach (var stat in characterJson.Stats)
            {
                if(string.IsNullOrWhiteSpace(stat.StatId))
                    messages.AddError("At least one Stat is missing an Id.");
                var correspondingStatInfo = dungeonJson.CharacterStats.Find(cs => cs.Id.Equals(stat.StatId, StringComparison.InvariantCultureIgnoreCase));
                if (correspondingStatInfo == null)
                {
                    messages.AddError($"A Stat uses the Id {stat.StatId}, which is missing in the Stats table.");
                }
                else
                {
                    var correspondingRegenerationTargetStatInfo = dungeonJson.CharacterStats.Find(cs => cs.Id.Equals(correspondingStatInfo.RegeneratesStatId, StringComparison.InvariantCultureIgnoreCase));
                    if (correspondingRegenerationTargetStatInfo != null && !characterJson.Stats.Any(s => s.StatId.Equals(correspondingRegenerationTargetStatInfo.Id, StringComparison.InvariantCultureIgnoreCase)))
                        messages.AddError($"The Character has access to {stat.StatId} but not of {correspondingRegenerationTargetStatInfo.Id}, which it regenerates/degenerates.");
                    if (stat.Base < correspondingStatInfo.MinCap)
                        messages.AddError($"The Base value for {stat.StatId} is lower than its accepted Minimum.");
                    if (stat.Base > correspondingStatInfo.MaxCap)
                        messages.AddError($"The Base value for {stat.StatId} is higher than its accepted Maximum.");
                }
                if(stat.IncreasePerLevel < 0)
                    messages.AddError($"{stat.StatId} is set to decrease by level-up. This is not allowed.");
                else if(stat.IncreasePerLevel == 0)
                    messages.AddWarning($"{stat.StatId} is set to not change by level-up. Check if this is expected.");
            }

            if(int.TryParse(characterJson.BaseSightRange, out int sightRange))
            {
                if (sightRange < 0)
                    messages.AddError("Sight Range must be 0 or higher, fullmap, wholemap, fullroom or wholeroom (case insensitive, space between words allowed).");
                else if (sightRange == 0)
                    messages.AddError("Sight Range is 0. Under normal circumnstances, it won't be able to see anything.");
            }
            else
            {
                switch (characterJson.BaseSightRange.ToLower())
                {
                    case "full map":
                    case "fullmap":
                    case "whole map":
                    case "wholemap":
                    case "full room":
                    case "fullroom":
                    case "whole room":
                    case "wholeroom":
                        break;
                    default:
                        messages.AddError("Sight Range must be 0 or higher, fullmap, wholemap, fullroom or wholeroom (case insensitive, space between words allowed).");
                        break;
                }
            }

            if (characterJson.InventorySize < 0)
                messages.AddError("Inventory Size must be 0 or higher.");
            else if (characterJson.InventorySize == 0)
                messages.AddWarning("Inventory Size is 0. It won't be able to carry any items.");

            List<string> usedSlots = [];

            foreach (var equipmentId in characterJson.InitialEquipment)
            {
                var itemData = dungeonJson.Items.Find(c => c.Id.Equals(equipmentId));
                if (string.IsNullOrWhiteSpace(equipmentId) || itemData == null)
                {
                    messages.AddError($"One of the Starting Equipment, {equipmentId}, is not valid.");
                }
                else
                {
                    var itemType = dungeonJson.ItemTypeInfos.Find(c => c.Id.Equals(itemData.ItemType));
                    if (itemType != null)
                    {
                        if (itemType.Usability != ItemUsability.Equip)
                        {
                            messages.AddError($"One of the Starting Equipment, {equipmentId}, is not equippable.");
                        }
                        if (!string.IsNullOrWhiteSpace(itemType.Slot1) && usedSlots.Contains(itemType.Slot1))
                        {
                            messages.AddError($"One of the Starting Equipment, {equipmentId}, uses the Primary Slot {itemType.Slot1}, which is already used by another piece of equipment.");
                        }
                        else
                        {
                            usedSlots.Add(itemType.Slot1);
                        }
                        if (!string.IsNullOrWhiteSpace(itemType.Slot2) && usedSlots.Contains(itemType.Slot2))
                        {
                            messages.AddError($"One of the Starting Equipment, {equipmentId}, uses the Secondary Slot {itemType.Slot2}, which is already used by another piece of equipment.");
                        }
                        else
                        {
                            usedSlots.Add(itemType.Slot2);
                        }
                    }
                }
            }
            if (characterJson.MaxLevel < 1)
                messages.AddError("Max Level must be 1 or higher.");
            else if (characterJson.MaxLevel == 1 && characterJson.CanGainExperience)
                messages.AddError("Max Level is 1, which prevents getting experience points, but CanGainExperience is set to true. Reconcile this contradiction.");

            if(characterJson.CanGainExperience)
            {
                try
                {
                    for (int i = 1; i <= characterJson.MaxLevel; i++)
                    {
                        var payout = new Expression(characterJson.ExperienceToLevelUpFormula.Replace("level", i.ToString(), StringComparison.InvariantCultureIgnoreCase)).Eval<int>();
                        if (payout < 0)
                            messages.AddError($"Experience To Level Up formula returns a number lower than 0 at Level {i}, which is not valid.");
                    }
                }
                catch (Exception ex)
                {
                    messages.AddError($"Experience To Level Up formula is invalid: {ex.Message}.");
                }
            }

            if(characterJson.StartingInventory != null)
            {
                if(characterJson.StartingInventory.Count > characterJson.InventorySize)
                    messages.AddError("Character has more items in the Starting Inventory than what the Inventory Size allows.");
                foreach (var item in characterJson.StartingInventory)
                {
                    if(!dungeonJson.Items.Exists(i => i.Id.Equals(item)))
                        messages.AddError($"Character has invalid item {item} in Starting Inventory.");
                }
            }

            if (characterAsInstance != null && characterAsInstance.OwnOnAttack.Any())
            {
                if(characterAsInstance.OwnOnAttack.HasMinimumMatches(ooa => ooa.Id.ToLower(), 2))
                {
                    messages.AddError("Character has at least two Attack actions with the same Id.");
                }
                foreach (var onAttackAction in characterJson.OnAttack)
                {
                    messages.AddRange(await ActionValidator.Validate(onAttackAction, dungeonJson));
                }
                if (characterAsInstance != null)
                {
                    foreach (var onAttackAction in characterAsInstance.OwnOnAttack)
                    {
                        messages.AddRange(await ActionValidator.Validate(onAttackAction, dungeonJson, sampleDungeon));
                    }
                }
            }
            else
            {
                messages.AddWarning("Character does not have OnAttack Actions. Make sure they have items or a DefaultOnAttack, otherwise they cannot attack.");
            }

            if (characterJson.DefaultOnAttack != null)
            {
                messages.AddRange(await ActionValidator.Validate(characterJson.DefaultOnAttack, dungeonJson));
                if (characterAsInstance != null)
                    messages.AddRange(await ActionValidator.Validate(characterAsInstance.DefaultOnAttack, dungeonJson, sampleDungeon));
            }
            else
            {
                messages.AddWarning("Character does not have a DefaultOnAttack Action. Make sure they have items or other OnAttack Actions, otherwise they cannot attack.");
            }

            if (characterJson.OnAttacked != null)
            {
                messages.AddRange(await ActionValidator.Validate(characterJson.OnAttacked, dungeonJson));
                if(characterAsInstance != null)
                    messages.AddRange(await ActionValidator.Validate(characterAsInstance.OwnOnAttacked, dungeonJson, sampleDungeon));
            }

            if (characterJson.OnDeath != null)
            {
                messages.AddRange(await ActionValidator.Validate(characterJson.OnDeath, dungeonJson));
                if (characterAsInstance != null)
                    messages.AddRange(await ActionValidator.Validate(characterAsInstance.OwnOnDeath, dungeonJson, sampleDungeon));
            }
            else
            {
                messages.AddWarning("Character does not have OnDeath!");
            }

            if (characterJson.OnLevelUp != null)
            {
                messages.AddRange(await ActionValidator.Validate(characterJson.OnLevelUp, dungeonJson));
                if (characterAsInstance != null)
                    messages.AddRange(await ActionValidator.Validate(characterAsInstance.OnLevelUp, dungeonJson, sampleDungeon));
            }

            return messages;
        }
    }
}
