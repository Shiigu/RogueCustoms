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
    public static class DungeonCharacterValidator
    {
        public static DungeonValidationMessages Validate(CharacterInfo characterJson, bool isPlayerCharacter, DungeonInfo dungeonJson, Dungeon sampleDungeon)
        {
            Character characterAsInstance = isPlayerCharacter
                                        ? new PlayerCharacter (new EntityClass(characterJson, sampleDungeon.LocaleToUse, EntityType.Player), 1, sampleDungeon.CurrentFloor)
                                        : new NonPlayableCharacter (new EntityClass(characterJson, sampleDungeon.LocaleToUse, EntityType.NPC), 1, sampleDungeon.CurrentFloor);

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
                messages.AddRange(ActionValidator.Validate(characterAsInstance.OwnOnTurnStart, dungeonJson, sampleDungeon));
            }

            if (characterJson.BaseHP <= 0)
                messages.AddError("Base HP must be higher than 0.");
            if (characterJson.MaxHPIncreasePerLevel < 0)
                messages.AddError("HP Gained per level must be 0 or higher.");
            else if (characterJson.MaxHPIncreasePerLevel == 0)
                messages.AddWarning("HP Gained per level is 0. It won't gain any HP on level up.");
            if(characterJson.UsesMP)
            {
                if (characterJson.BaseMP <= 0)
                    messages.AddError("Character is set to use MP, but Base MP is 0 or lower.");
                if (characterJson.MaxMPIncreasePerLevel < 0)
                    messages.AddWarning("Character is set to use MP, but MP Gained per level is lower than 0. Character may run out of MP by not doing anything.");
                else if (characterJson.MaxMPIncreasePerLevel == 0)
                    messages.AddWarning("Character is set to use MP, but MP Gained per level is 0. It won't gain any MP on level up.");
            }
            if (characterJson.BaseAttack < 0)
                messages.AddError("Base Attack must be 0 or higher.");
            else if (characterJson.BaseAttack == 0)
                messages.AddWarning("Base Attack is 0. It might fail to deal any damage depending on its assigned actions.");
            if (characterJson.AttackIncreasePerLevel < 0)
                messages.AddError("Attack Gained per level must be 0 or higher.");
            else if (characterJson.AttackIncreasePerLevel == 0)
                messages.AddWarning("Attack Gained per level is 0. It won't gain any Attack on level up.");
            if (characterJson.BaseDefense < 0)
                messages.AddError("Base Defense must be 0 or higher.");
            if (characterJson.DefenseIncreasePerLevel < 0)
                messages.AddError("Defense Gained per level must be 0 or higher.");
            else if (characterJson.DefenseIncreasePerLevel == 0)
                messages.AddWarning("Defense Gained per level is 0. It won't gain any Defense on level up.");
            if (characterJson.BaseMovement < 0)
                messages.AddError("Base Movement must be 0 or higher.");
            else if (characterJson.BaseMovement == 0)
                messages.AddWarning("Base Movement is 0. Under normal circumnstances, it won't be able to move at all.");
            if (characterJson.BaseAccuracy < 0 || characterJson.BaseAccuracy > 200)
                messages.AddError("Base Accuracy must be between 0 and 200.");
            else if (characterJson.BaseAccuracy == 0)
                messages.AddWarning("Base Movement is 0. Under normal circumnstances, it won't be able to land any attacks at all.");
            if (characterJson.BaseEvasion < -100 || characterJson.BaseEvasion > 100)
                messages.AddError("Base Evasion must be between -100 and 100.");
            else if (characterJson.BaseEvasion == -100)
                messages.AddWarning("Base Evasion is -100. Under normal circumnstances, it won't be able to avoid any attacks at all.");
            if (characterJson.MovementIncreasePerLevel < 0)
                messages.AddError("Movement Gained per level must be 0 or higher.");
            else if (characterJson.MovementIncreasePerLevel == 0)
                messages.AddWarning("Movement Gained per level is 0. It won't gain any Movement on level up.");
            if (characterJson.BaseHPRegeneration < 0)
                messages.AddWarning("Base HP Regeneration is lower than 0. The Character might spontaneously die under normal circumnstances.");
            else if (characterJson.BaseHPRegeneration == 0)
                messages.AddWarning("Base HP Regeneration is 0. Under normal circumnstances, it won't be able to regenerate HP at all.");
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
            if (characterJson.HPRegenerationIncreasePerLevel < 0)
                messages.AddError("HP Regeneration Gained per level must be 0 or higher.");
            else if (characterJson.HPRegenerationIncreasePerLevel == 0)
                messages.AddWarning("HP Regeneration Gained per level is 0. It won't gain any HP Regeneration on level up.");
            if (characterJson.InventorySize < 0)
                messages.AddError("Inventory Size must be 0 or higher.");
            else if (characterJson.InventorySize == 0)
                messages.AddWarning("Inventory Size is 0. It won't be able to carry any items.");
            if (string.IsNullOrWhiteSpace(characterJson.StartingWeapon) || !dungeonJson.Items.Exists(c => c.EntityType == "Weapon" && c.Id.Equals(characterJson.StartingWeapon)))
                messages.AddError($"Starting Weapon, {characterJson.StartingWeapon}, is not valid.");
            if (string.IsNullOrWhiteSpace(characterJson.StartingArmor) || !dungeonJson.Items.Exists(c => c.EntityType == "Armor" && c.Id.Equals(characterJson.StartingArmor)))
                messages.AddError($"Starting Armor, {characterJson.StartingArmor}, is not valid.");
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

            if (characterAsInstance.OwnOnAttack.Any())
            {
                foreach (var onAttackAction in characterAsInstance.OwnOnAttack)
                {
                    messages.AddRange(ActionValidator.Validate(onAttackAction, dungeonJson, sampleDungeon));
                }
            }
            else
            {
                messages.AddWarning("Character does not have OnAttackActions. Make sure they have items, otherwise they cannot attack.");
            }

            if (characterJson.OnAttacked != null)
            {
                messages.AddRange(ActionValidator.Validate(characterAsInstance.OwnOnAttacked, dungeonJson, sampleDungeon));
            }

            if (characterJson.OnDeath != null)
            {
                messages.AddRange(ActionValidator.Validate(characterAsInstance.OwnOnDeath, dungeonJson, sampleDungeon));
            }
            else
            {
                messages.AddWarning("Character does not have OnDeath!");
            }

            return messages;
        }
    }
}
