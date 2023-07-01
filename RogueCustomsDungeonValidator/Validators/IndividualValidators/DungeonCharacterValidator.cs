using org.matheval;
using RogueCustomsDungeonValidator.Utils;
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

namespace RogueCustomsDungeonValidator.Validators.IndividualValidators
{
    public class DungeonCharacterValidator
    {
        public static DungeonValidationMessages Validate(ClassInfo characterJson, DungeonInfo dungeonJson, Dungeon sampleDungeon)
        {
            Character characterAsInstance = characterJson.EntityType == "Player"
                                        ? new PlayerCharacter (new EntityClass(characterJson, sampleDungeon.LocaleToUse), 1, sampleDungeon.CurrentFloor)
                                        : new NonPlayableCharacter (new EntityClass(characterJson, sampleDungeon.LocaleToUse), 1, sampleDungeon.CurrentFloor);

            var messages = new DungeonValidationMessages();

            messages.AddRange(dungeonJson.ValidateString(characterJson.Name, "Character", "Name", true));
            messages.AddRange(dungeonJson.ValidateString(characterJson.Description, "Character", "Description", false));

            if (string.IsNullOrWhiteSpace(characterJson.Faction))
            {
                messages.AddError("Character does not have a faction.");
            }
            else if (!dungeonJson.FactionInfos.Any(fi => characterJson.Faction.Equals(fi.Id)))
            {
                messages.AddError($"Faction {characterJson.Faction} could not be found.");
            }

            messages.AddRange(characterJson.ConsoleRepresentation.Validate(characterJson.Id, dungeonJson));

            foreach (var onTurnStartAction in characterJson.OnTurnStartActions.ConvertAll(otsa => new ActionWithEffects(otsa)))
            {
                messages.AddRange(ActionValidator.Validate(onTurnStartAction, dungeonJson, sampleDungeon));
            }

            if (characterJson.BaseHP <= 0)
                messages.AddError("Base HP must be higher than 0.");
            if (characterJson.MaxHPIncreasePerLevel < 0)
                messages.AddError("HP Gained per level must be 0 or higher.");
            else if (characterJson.MaxHPIncreasePerLevel == 0)
                messages.AddWarning("HP Gained per level is 0. It won't gain any HP on level up.");
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
            if (characterJson.MovementIncreasePerLevel < 0)
                messages.AddError("Movement Gained per level must be 0 or higher.");
            else if (characterJson.MovementIncreasePerLevel == 0)
                messages.AddWarning("Movement Gained per level is 0. It won't gain any Movement on level up.");
            if (characterJson.BaseHPRegeneration < 0)
                messages.AddError("Base HP Regeneration must be 0 or higher.");
            else if (characterJson.BaseHPRegeneration == 0)
                messages.AddWarning("Base HP Regeneration is 0. Under normal circumnstances, it won't be able to regenerate HP at all.");
            var seesWholeMap = false;
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
                        seesWholeMap = true;
                        break;
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
            if(characterJson.KnowsAllCharacterPositions)
            {
                if(seesWholeMap)
                    messages.AddWarning("KnowsAllCharacterPositions is set to true but Sight Range covers the entire map. KnowsAllCharacterPositions will have no effect.");
                else if (characterJson.EntityType == "Player")
                    messages.AddWarning("KnowsAllCharacterPositions is set to true but Character is a Player. KnowsAllCharacterPositions will have no effect.");
            }
            if (characterJson.HPRegenerationIncreasePerLevel < 0)
                messages.AddError("HP Regeneration Gained per level must be 0 or higher.");
            else if (characterJson.HPRegenerationIncreasePerLevel == 0)
                messages.AddWarning("HP Regeneration Gained per level is 0. It won't gain any HP Regeneration on level up.");
            if (characterJson.InventorySize < 0)
                messages.AddError("Inventory Size must be 0 or higher.");
            else if (characterJson.InventorySize == 0)
                messages.AddWarning("Inventory Size is 0. It won't be able to carry any items.");
            if (string.IsNullOrWhiteSpace(characterJson.StartingWeapon) || !dungeonJson.Items.Any(c => c.EntityType == "Weapon" && c.Id.Equals(characterJson.StartingWeapon)))
                messages.AddError($"Starting Weapon, {characterJson.StartingWeapon}, is not valid.");
            if (string.IsNullOrWhiteSpace(characterJson.StartingArmor) || !dungeonJson.Items.Any(c => c.EntityType == "Armor" && c.Id.Equals(characterJson.StartingArmor)))
                messages.AddError($"Starting Armor, {characterJson.StartingArmor}, is not valid.");
            if (!characterJson.CanGainExperience && characterJson.EntityType == "Player")
                messages.AddWarning("Character is set as a Player Class, but is not allowed to gain any experience points. Reconsider this.");
            if (characterJson.MaxLevel < 1)
                messages.AddError("Max Level must be 1 or higher.");
            else if (characterJson.MaxLevel == 1 && characterJson.CanGainExperience)
                messages.AddError("Max Level is 1, which prevents getting experience points, but CanGainExperience is set to true. Reconcile this contradiction.");
            else if (characterJson.MaxLevel == 1 && characterJson.EntityType == "Player")
                messages.AddWarning("Character is set as a Player Class, but its Max Level is 1, so it's not allowed to gain any experience points. Reconsider this.");

            try
            {
                for (int i = 1; i <= characterJson.MaxLevel; i++)
                {
                    var payout = new Expression(characterJson.ExperiencePayoutFormula.Replace("level", i.ToString(), StringComparison.InvariantCultureIgnoreCase)).Eval<int>();
                    if (payout < 0)
                        messages.AddError($"Experience Payout formula returns a number lower than 0 at Level {i}, which is not valid.");
                }
            }
            catch (Exception ex)
            {
                messages.AddError($"Experience Payout formula is invalid: {ex.Message}.");
            }

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

            if(characterAsInstance.OwnOnAttackActions.Any())
            {
                foreach (var onAttackAction in characterAsInstance.OwnOnAttackActions)
                {
                    messages.AddRange(ActionValidator.Validate(onAttackAction, dungeonJson, sampleDungeon));
                }
            }
            else
            {
                messages.AddWarning("Character does not have OnAttackActions. Make sure they have items, otherwise they cannot attack.");
            }

            foreach (var onAttackedAction in characterAsInstance.OwnOnAttackedActions)
            {
                messages.AddRange(ActionValidator.Validate(onAttackedAction, dungeonJson, sampleDungeon));
            }

            if (characterJson.OnDeathActions.Any())
            {
                foreach (var onDeathAction in characterAsInstance.OwnOnDeathActions)
                {
                    messages.AddRange(ActionValidator.Validate(onDeathAction, dungeonJson, sampleDungeon));
                }
            }
            else
            {
                messages.AddWarning("Character does not have OnDeathActions!");
            }

            if (characterJson.OnItemSteppedActions.Any())
                messages.AddWarning("Character has OnItemSteppedActions, which will be ignored by the game. Consider removing it.");
            if (characterJson.OnItemUseActions.Any())
                messages.AddWarning("Character has OnItemUseActions, which will be ignored by the game. Consider removing it.");
            if (characterJson.OnStatusApplyActions.Any())
                messages.AddWarning("Character has OnStatusApplyActions, which will be ignored by the game. Consider removing it.");

            if(characterJson.EntityType == "NPC" && !dungeonJson.FloorInfos.Any(fi => fi.PossibleMonsters.Any(pm => pm.ClassId.Equals(characterJson.Id))))
                messages.AddWarning("Character is an NPC but does not show up in any list of PossibleMonsters. It will never be spawned. Consider adding it to a PossibleMonsters list.");

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}
