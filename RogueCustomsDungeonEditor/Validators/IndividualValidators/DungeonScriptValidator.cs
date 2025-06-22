using org.matheval;
using RogueCustomsDungeonEditor.Utils;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
{
    public class DungeonScriptValidator
    {
        public static async Task<DungeonValidationMessages> Validate(ActionWithEffectsInfo script, DungeonInfo dungeonJson, Dungeon sampleDungeon)
        {
            var messages = new DungeonValidationMessages();

            if (string.IsNullOrWhiteSpace(script.Id))
                messages.AddError("A Script does not have an Id.");

            messages.AddRange(dungeonJson.ValidateString(script.Name, "Script", "Name", true));

            var scriptInstance = ActionWithEffects.Create(script);

            var floorGroupValidationMessages = await ActionValidator.Validate(scriptInstance.Clone(), dungeonJson, sampleDungeon);
            if (floorGroupValidationMessages.ValidationMessages.Any(m => m.Type == DungeonValidationMessageType.Error))
                messages.AddWarning("The Script, when called by a Floor Group, throws an error. Please check this.");

            var samplePlayerClass = dungeonJson.PlayerClasses.FirstOrDefault();

            if(samplePlayerClass != null)
            {
                var playerClassAsInstance = new PlayerCharacter(new EntityClass(samplePlayerClass, sampleDungeon.LocaleToUse, EntityType.Player, dungeonJson.CharacterStats), 1, sampleDungeon.CurrentFloor);
                var clonedScript = scriptInstance.Clone();
                clonedScript.User = playerClassAsInstance;
                playerClassAsInstance.OwnOnAttack.Add(clonedScript);
                var validationMessages = await ActionValidator.Validate(clonedScript, dungeonJson, sampleDungeon);
                if(validationMessages.ValidationMessages.Any(m => m.Type == DungeonValidationMessageType.Error))
                    messages.AddWarning("The Script, when called by a Player Class, throws an error. Please check this.");
            }
            else
            {
                messages.AddWarning("The Script has no Player Class to be tested on.");
            }

            var sampleNPC = dungeonJson.NPCs.FirstOrDefault();

            if (sampleNPC != null)
            {
                var NPCAsInstance = new NonPlayableCharacter(new EntityClass(sampleNPC, sampleDungeon.LocaleToUse, EntityType.NPC, dungeonJson.CharacterStats), 1, sampleDungeon.CurrentFloor);
                var clonedScript = scriptInstance.Clone();
                clonedScript.User = NPCAsInstance;
                NPCAsInstance.OwnOnAttack.Add(clonedScript);
                var validationMessages = await ActionValidator.Validate(clonedScript, dungeonJson, sampleDungeon);
                if (validationMessages.ValidationMessages.Any(m => m.Type == DungeonValidationMessageType.Error))
                    messages.AddWarning("The Script, when called by an NPC, throws an error. Please check this.");
            }
            else
            {
                messages.AddWarning("The Script has no NPC to be tested on.");
            }

            var sampleItem = dungeonJson.Items.FirstOrDefault();
            if (sampleItem != null)
            {
                var itemAsInstance = new Item(new EntityClass(sampleItem, sampleDungeon.LocaleToUse, EntityType.Weapon, null), sampleDungeon.CurrentFloor);
                var clonedScript = scriptInstance.Clone();
                clonedScript.User = itemAsInstance;
                itemAsInstance.OwnOnAttack.Add(clonedScript);
                var validationMessages = await ActionValidator.Validate(clonedScript, dungeonJson, sampleDungeon);
                if (validationMessages.ValidationMessages.Any(m => m.Type == DungeonValidationMessageType.Error))
                    messages.AddWarning("The Script, when called by an Item, throws an error. Please check this.");
            }
            else
            {
                messages.AddWarning("The Script has no Item to be tested on.");
            }

            var sampleTrap = dungeonJson.Traps.FirstOrDefault();
            if (sampleTrap != null)
            {
                var trapAsInstance = new Trap(new EntityClass(sampleTrap, sampleDungeon.LocaleToUse, EntityType.Trap, null), sampleDungeon.CurrentFloor);
                var clonedScript = scriptInstance.Clone();
                clonedScript.User = trapAsInstance;
                trapAsInstance.OnStepped = clonedScript;
                var validationMessages = await ActionValidator.Validate(clonedScript, dungeonJson, sampleDungeon);
                if (validationMessages.ValidationMessages.Any(m => m.Type == DungeonValidationMessageType.Error))
                    messages.AddWarning("The Script, when called by a Trap, throws an error. Please check this.");
            }
            else
            {
                messages.AddWarning("The Script has no Trap to be tested on.");
            }

            var sampleAlteredStatus = dungeonJson.AlteredStatuses.FirstOrDefault();
            if (sampleAlteredStatus != null)
            {
                var alteredStatusAsInstance = new AlteredStatus(new EntityClass(sampleAlteredStatus, sampleDungeon.LocaleToUse, EntityType.AlteredStatus, null), sampleDungeon.CurrentFloor);
                var clonedScript = scriptInstance.Clone();
                clonedScript.User = alteredStatusAsInstance;
                alteredStatusAsInstance.OnApply = clonedScript;
                var validationMessages = await ActionValidator.Validate(clonedScript, dungeonJson, sampleDungeon);
                if (validationMessages.ValidationMessages.Any(m => m.Type == DungeonValidationMessageType.Error))
                    messages.AddWarning("The Script, when called by an Altered Status, throws an error. Please check this.");
            }
            else
            {
                messages.AddWarning("The Script has no Altered Status to be tested on.");
            }

            var sampleElement = dungeonJson.ElementInfos.FirstOrDefault();
            if (sampleElement != null)
            {
                var elementAsInstance = new Element(sampleElement, sampleDungeon.LocaleToUse);
                var clonedScript = scriptInstance.Clone();
                elementAsInstance.OnAfterAttack = clonedScript;
                var validationMessages = await ActionValidator.Validate(clonedScript, dungeonJson, sampleDungeon);
                if (validationMessages.ValidationMessages.Any(m => m.Type == DungeonValidationMessageType.Error))
                    messages.AddWarning("The Script, when called by an Element, throws an error. Please check this.");
            }
            else
            {
                messages.AddWarning("The Script has no Element to be tested on.");
            }


            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}
