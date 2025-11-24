using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
{
    public static class DungeonAffixValidator
    {
        private static readonly List<string> ValidAffixTypes = ["Prefix", "Suffix"];

        public static async Task<DungeonValidationMessages> Validate(AffixInfo affix, DungeonInfo dungeonJson, Dungeon sampleDungeon)
        {
            var messages = new DungeonValidationMessages();
            var validStatIds = dungeonJson.CharacterStats.ConvertAll(s => s.Id.ToLowerInvariant());
            if (!string.IsNullOrWhiteSpace(affix.Id))
            {
                if (dungeonJson.AffixInfos.Any(a => a != affix && a.Id.Equals(affix.Id, StringComparison.InvariantCultureIgnoreCase)))
                {
                    messages.AddError($"Affix {affix.Id} has a duplicate Id.");
                }
                var affixAsInstance = sampleDungeon != null ? new Affix(affix, sampleDungeon.LocaleToUse, sampleDungeon.ItemTypes, sampleDungeon.Elements, sampleDungeon.ActionSchools) : null;
                messages.AddRange(dungeonJson.ValidateString(affix.Name, "Affix", "Name", true));
                if (!ValidAffixTypes.Any(vit => vit.Equals(affix.AffixType, StringComparison.InvariantCultureIgnoreCase)))
                {
                    messages.AddError($"Affix {affix.Id} has an invalid Affix Type.");
                }
                if (affix.AffectedItemTypes.Any(ait => !dungeonJson.ItemTypeInfos.Any(it => it.Id.Equals(ait, StringComparison.InvariantCultureIgnoreCase))))
                {
                    messages.AddError($"Affix {affix.Id} has an invalid Affected Item Type.");
                }
                else if (affix.AffectedItemTypes.Count == 0)
                {
                    messages.AddError($"Affix {affix.Id} has no Affected Item Types.");
                }
                if (affix.MinimumItemLevel < 1)
                {
                    messages.AddError($"Affix {affix.Id} has an invalid Minimum Item Level of {affix.MinimumItemLevel}.");
                }
                if (affix.ItemValueModifierPercentage < 0 || affix.ItemValueModifierPercentage > 100)
                {
                    messages.AddError($"Affix {affix.Id} has an invalid Item Value Modifier Percentage of {affix.ItemValueModifierPercentage}%.");
                }
                if (affix.StatModifiers != null && affix.StatModifiers.Any())
                {
                    foreach (var stat in affix.StatModifiers)
                    {
                        if (!validStatIds.Contains(stat.Id.ToLowerInvariant()))
                        {
                            messages.AddError($"Affix {affix.Id}'s modification is invalid as there is no stat with Id {stat.Id}.");
                            continue;
                        }
                        if (stat.Amount == 0)
                            messages.AddWarning($"Affix {affix.Id}'s modification on {stat.Id} is set to 0. This won't have any effect.");
                    }
                }
                if (affix.ExtraDamage != null)
                {
                    if (affix.ExtraDamage.MinDamage < 0)
                        messages.AddError($"Affix {affix.Id}'s Extra Damage has a negative Min Damage of {affix.ExtraDamage.MinDamage}.");
                    if (affix.ExtraDamage.MaxDamage < 0)
                        messages.AddError($"Affix {affix.Id}'s Extra Damage has a negative Max Damage of {affix.ExtraDamage.MaxDamage}.");
                    if (affix.ExtraDamage.MinDamage > affix.ExtraDamage.MaxDamage)
                        messages.AddError($"Affix {affix.Id}'s Extra Damage has a Min Damage of {affix.ExtraDamage.MinDamage} greater than its Max Damage of {affix.ExtraDamage.MaxDamage}.");
                    if (!string.IsNullOrWhiteSpace(affix.ExtraDamage.Element) && !sampleDungeon.Elements.Any(e => e.Id.Equals(affix.ExtraDamage.Element, StringComparison.InvariantCultureIgnoreCase)))
                        messages.AddError($"Affix {affix.Id}'s Extra Damage has an invalid Element of {affix.ExtraDamage.Element}.");
                }
                messages.AddRange(await ActionValidator.Validate(affix.OnTurnStart, dungeonJson));
                if (affixAsInstance != null)
                    messages.AddRange(await ActionValidator.Validate(affixAsInstance.OwnOnTurnStart, dungeonJson, sampleDungeon));
                messages.AddRange(await ActionValidator.Validate(affix.OnAttack, dungeonJson));
                if (affixAsInstance != null)
                    messages.AddRange(await ActionValidator.Validate(affixAsInstance.OwnOnAttack, dungeonJson, sampleDungeon));
                messages.AddRange(await ActionValidator.Validate(affix.OnAttacked, dungeonJson));
                if (affixAsInstance != null)
                    messages.AddRange(await ActionValidator.Validate(affixAsInstance.OwnOnAttacked, dungeonJson, sampleDungeon));
            }
            else
            {
                messages.AddError($"Affix #{dungeonJson.AffixInfos.IndexOf(affix)} lacks an Id.");
            }

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}
