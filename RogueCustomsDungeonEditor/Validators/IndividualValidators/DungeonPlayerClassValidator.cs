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
    public class DungeonPlayerClassValidator
    {
        public static async Task<DungeonValidationMessages> Validate(PlayerClassInfo playerClassJson, DungeonInfo dungeonJson, Dungeon sampleDungeon)
        {
            var messages = new DungeonValidationMessages();

            messages.AddRange(await DungeonCharacterValidator.Validate(playerClassJson, true, dungeonJson, sampleDungeon));

            var foundNameInLocales = false;
            foreach (var locale in dungeonJson.Locales)
            {
                var nameLocale = locale.LocaleStrings.Find(ls => ls.Key.Equals(playerClassJson.Name));
                if (nameLocale != null)
                {
                    foundNameInLocales = true;
                    if (nameLocale.Value.Length > 13)
                    {
                        if (playerClassJson.RequiresNamePrompt)
                            messages.AddWarning($"Character is a Player whose default name in locale {locale.Language} exceeds 13 characters. Console Clients may display the name incorrectly");
                        else
                            messages.AddWarning($"Character is a Player whose name in locale {locale.Language} exceeds 13 characters. Console Clients may display the name incorrectly");
                    }
                }
                if (!foundNameInLocales && playerClassJson.Name.Length > 13)
                {
                    if (playerClassJson.RequiresNamePrompt)
                        messages.AddWarning("Character is a Player whose unlocalizable default name exceeds 13 characters. Console Clients may display the name incorrectly");
                    else
                        messages.AddWarning("Character is a Player whose unlocalizable name exceeds 13 characters. Console Clients may display the name incorrectly");
                }
            }
            var startingWeaponId = playerClassJson.StartingWeapon;
            var startingArmorId = playerClassJson.StartingArmor;
            if (!string.IsNullOrWhiteSpace(playerClassJson.InitialEquippedWeapon))
            {
                if (!dungeonJson.Items.Exists(c => c.EntityType == "Weapon" && c.Id.Equals(playerClassJson.InitialEquippedWeapon)))
                    messages.AddError($"Initial Weapon, {playerClassJson.InitialEquippedWeapon}, is not valid.");
                else if (playerClassJson.InitialEquippedWeapon.Equals(playerClassJson.StartingWeapon, StringComparison.CurrentCultureIgnoreCase))
                    messages.AddError($"Initial Weapon, {playerClassJson.InitialEquippedWeapon}, is the same as the Emergency Weapon.");
            }
            if (!string.IsNullOrWhiteSpace(playerClassJson.InitialEquippedArmor))
            {
                if (!dungeonJson.Items.Exists(c => c.EntityType == "Armor" && c.Id.Equals(playerClassJson.InitialEquippedArmor)))
                    messages.AddError($"Initial Armor, {playerClassJson.InitialEquippedArmor}, is not valid.");
                else if (playerClassJson.InitialEquippedWeapon.Equals(playerClassJson.StartingArmor, StringComparison.CurrentCultureIgnoreCase))
                    messages.AddError($"Initial Armor, {playerClassJson.InitialEquippedArmor}, is the same as the Emergency Armor.");
            }
            if (dungeonJson.FloorInfos.Exists(fi => fi.PossibleItems.Exists(pi => pi.ClassId.Equals(startingWeaponId))))
                messages.AddWarning($"Character is a Player whose Starting Weapon, {startingWeaponId}, can spawn as a pickable item in a floor. This might cause unintended behaviour.");
            if (dungeonJson.FloorInfos.Exists(fi => fi.PossibleItems.Exists(pi => pi.ClassId.Equals(startingArmorId))))
                messages.AddWarning($"Character is a Player whose Starting Armor, {startingArmorId}, can spawn as a pickable item in a floor. This might cause unintended behaviour.");
            if (!playerClassJson.CanGainExperience)
                messages.AddWarning("Character is set as a Player Class, but is not allowed to gain any experience points. Reconsider this.");
            if (playerClassJson.MaxLevel < 1)
                messages.AddError("Max Level must be 1 or higher.");
            else if (playerClassJson.MaxLevel == 1 && playerClassJson.CanGainExperience)
                messages.AddError("Max Level is 1, which prevents getting experience points, but CanGainExperience is set to true. Reconcile this contradiction.");
            else if (playerClassJson.MaxLevel == 1)
                messages.AddWarning("Character is set as a Player Class, but its Max Level is 1, so it's not allowed to gain any experience points. Reconsider this.");
            if (playerClassJson.SaleValuePercentage < 1)
                messages.AddError("Character is set as a Player Class, but the percentage of Sales is set as a non-positive number.");

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}
