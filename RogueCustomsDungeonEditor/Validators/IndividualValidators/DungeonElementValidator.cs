using RogueCustomsDungeonEditor.Utils;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
{
    public static class DungeonElementValidator
    {
        public static async Task<DungeonValidationMessages> Validate(ElementInfo element, DungeonInfo dungeonJson, Dungeon sampleDungeon)
        {
            var elementAsInstance = new Element(element, sampleDungeon.LocaleToUse);
            var messages = new DungeonValidationMessages();

            messages.AddRange(dungeonJson.ValidateString(element.Name, $"Element {element.Id}", "Name", true));
            if(!string.IsNullOrWhiteSpace(element.ResistanceStatId))
            {
                var resistanceStat = dungeonJson.CharacterStats.Find(s => s.Id.Equals(element.ResistanceStatId, StringComparison.InvariantCultureIgnoreCase));

                if (resistanceStat == null)
                    messages.AddError($"The Element's Resistance Stat, {element.ResistanceStatId}, does not exist.");
                else if (!resistanceStat.StatType.Equals("Percentage", StringComparison.InvariantCultureIgnoreCase))
                    messages.AddError($"The Element's Resistance Stat, {element.ResistanceStatId}, is not of the Percentage type.");
            }
            else if(element.ExcessResistanceCausesHealDamage)
                messages.AddWarning($"The Element does not have a Resistance Stat, but ExcessResistanceCausesHealDamage is set to true. It will be ignored.");

            if (element.OnAfterAttack != null)
                messages.AddRange(await ActionValidator.Validate(elementAsInstance.OnAfterAttack, dungeonJson, sampleDungeon));

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}