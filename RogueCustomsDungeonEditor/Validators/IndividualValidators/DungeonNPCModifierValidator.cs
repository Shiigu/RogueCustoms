using System;
using System.Linq;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
{
    public static class DungeonNPCModifierValidator
    {
        public static async Task<DungeonValidationMessages> Validate(NPCModifierInfo npcModifier, DungeonInfo dungeonJson, Dungeon sampleDungeon)
        {
            var messages = new DungeonValidationMessages();
            var validStatIds = dungeonJson.CharacterStats.ConvertAll(s => s.Id.ToLowerInvariant());
            if (!string.IsNullOrWhiteSpace(npcModifier.Id))
            {
                if (dungeonJson.NPCModifierInfos.Any(a => a != npcModifier && a.Id.Equals(npcModifier.Id, StringComparison.InvariantCultureIgnoreCase)))
                {
                    messages.AddError($"NPC Modifier {npcModifier.Id} has a duplicate Id.");
                }
                var npcModifierAsInstance = sampleDungeon != null ? new NPCModifier(npcModifier, sampleDungeon.LocaleToUse, sampleDungeon.Elements, sampleDungeon.ActionSchools) : null;
                messages.AddRange(dungeonJson.ValidateString(npcModifier.Name, "NPC Modifier", "Name", true));
                if (npcModifier.StatModifiers != null && npcModifier.StatModifiers.Any())
                {
                    foreach (var stat in npcModifier.StatModifiers)
                    {
                        if (!validStatIds.Contains(stat.Id.ToLowerInvariant()))
                        {
                            messages.AddError($"NPC Modifier {npcModifier.Id}'s modification is invalid as there is no stat with Id {stat.Id}.");
                            continue;
                        }
                        if (stat.Amount == 0)
                            messages.AddWarning($"NPC Modifier {npcModifier.Id}'s modification on {stat.Id} is set to 0. This won't have any effect.");
                    }
                }
                if (npcModifier.ExtraDamage != null)
                {
                    if (npcModifier.ExtraDamage.MinDamage < 0)
                        messages.AddError($"NPC Modifier {npcModifier.Id}'s Extra Damage has a negative Min Damage of {npcModifier.ExtraDamage.MinDamage}.");
                    if (npcModifier.ExtraDamage.MaxDamage < 0)
                        messages.AddError($"NPC Modifier {npcModifier.Id}'s Extra Damage has a negative Max Damage of {npcModifier.ExtraDamage.MaxDamage}.");
                    if (npcModifier.ExtraDamage.MinDamage > npcModifier.ExtraDamage.MaxDamage)
                        messages.AddError($"NPC Modifier {npcModifier.Id}'s Extra Damage has a Min Damage of {npcModifier.ExtraDamage.MinDamage} greater than its Max Damage of {npcModifier.ExtraDamage.MaxDamage}.");
                    if (!string.IsNullOrWhiteSpace(npcModifier.ExtraDamage.Element) && !sampleDungeon.Elements.Any(e => e.Id.Equals(npcModifier.ExtraDamage.Element, StringComparison.InvariantCultureIgnoreCase)))
                        messages.AddError($"NPC Modifier {npcModifier.Id}'s Extra Damage has an invalid Element of {npcModifier.ExtraDamage.Element}.");
                }
                messages.AddRange(await ActionValidator.Validate(npcModifier.OnSpawn, dungeonJson));
                if (npcModifierAsInstance != null)
                    messages.AddRange(await ActionValidator.Validate(npcModifierAsInstance.OwnOnSpawn, dungeonJson, sampleDungeon));
                messages.AddRange(await ActionValidator.Validate(npcModifier.OnTurnStart, dungeonJson));
                if (npcModifierAsInstance != null)
                    messages.AddRange(await ActionValidator.Validate(npcModifierAsInstance.OwnOnTurnStart, dungeonJson, sampleDungeon));
                messages.AddRange(await ActionValidator.Validate(npcModifier.OnAttack, dungeonJson));
                if (npcModifierAsInstance != null)
                    messages.AddRange(await ActionValidator.Validate(npcModifierAsInstance.OwnOnAttack, dungeonJson, sampleDungeon));
                messages.AddRange(await ActionValidator.Validate(npcModifier.OnAttacked, dungeonJson));
                if (npcModifierAsInstance != null)
                    messages.AddRange(await ActionValidator.Validate(npcModifierAsInstance.OwnOnAttacked, dungeonJson, sampleDungeon));
                messages.AddRange(await ActionValidator.Validate(npcModifier.OnDeath, dungeonJson));
                if (npcModifierAsInstance != null)
                    messages.AddRange(await ActionValidator.Validate(npcModifierAsInstance.OwnOnDeath, dungeonJson, sampleDungeon));
            }
            else
            {
                messages.AddError($"NPC Modifier #{dungeonJson.NPCModifierInfos.IndexOf(npcModifier)} lacks an Id.");
            }

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}
