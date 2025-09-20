using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using MathNet.Numerics;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
{
    public static class DungeonAffixValidator
    {
        private static readonly List<string> ValidAffixTypes = ["Prefix", "Suffix"];

        public static async Task<DungeonValidationMessages> Validate(DungeonInfo dungeonJson, Dungeon sampleDungeon)
        {
            var messages = new DungeonValidationMessages();
            var validStatIds = dungeonJson.CharacterStats.ConvertAll(s => s.Id.ToLowerInvariant());

            foreach (var affixJson in dungeonJson.AffixInfos)
            {
                if (!string.IsNullOrWhiteSpace(affixJson.Id))
                {
                    if(dungeonJson.AffixInfos.Any(a => a != affixJson && a.Id.Equals(affixJson.Id, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        messages.AddError($"Affix {affixJson.Id} has a duplicate Id.");
                    }
                    var affixAsInstance = sampleDungeon != null ? new Affix(affixJson, sampleDungeon.LocaleToUse, sampleDungeon.ItemTypes, sampleDungeon.Elements, sampleDungeon.ActionSchools) : null;
                    messages.AddRange(dungeonJson.ValidateString(affixJson.Name, "Affix", "Name", true));
                    if (!ValidAffixTypes.Any(vit => vit.Equals(affixJson.AffixType, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        messages.AddError($"Affix {affixJson.Id} has an invalid Affix Type.");
                    }
                    if (affixJson.AffectedItemTypes.Any(ait => !dungeonJson.ItemTypeInfos.Any(it => it.Id.Equals(ait, StringComparison.InvariantCultureIgnoreCase))))
                    {
                        messages.AddError($"Affix {affixJson.Id} has an invalid Affected Item Type.");
                    }
                    else if (affixJson.AffectedItemTypes.Count == 0)
                    {
                        messages.AddError($"Affix {affixJson.Id} has no Affected Item Types.");
                    }
                    if(affixJson.MinimumItemLevel < 1)
                    {
                        messages.AddError($"Affix {affixJson.Id} has an invalid Minimum Item Level of {affixJson.MinimumItemLevel}.");
                    }
                    if (affixJson.ItemValueModifierPercentage < 0 || affixJson.ItemValueModifierPercentage > 100)
                    {
                        messages.AddError($"Affix {affixJson.Id} has an invalid Item Value Modifier Percentage of {affixJson.ItemValueModifierPercentage}%.");
                    }
                    if (affixJson.StatModifiers != null && affixJson.StatModifiers.Any())
                    {
                        foreach (var stat in affixJson.StatModifiers)
                        {
                            if (!validStatIds.Contains(stat.Id.ToLowerInvariant()))
                            {
                                messages.AddError($"Affix {affixJson.Id}'s modification is invalid as there is no stat with Id {stat.Id}.");
                                continue;
                            }
                            if (stat.Amount == 0)
                                messages.AddWarning($"Affix {affixJson.Id}'s modification on {stat.Id} is set to 0. This won't have any effect.");
                        }
                    }
                    if (affixJson.ExtraDamage != null)
                    {
                        if (affixJson.ExtraDamage.MinDamage < 0)
                            messages.AddError($"Affix {affixJson.Id}'s Extra Damage has a negative Min Damage of {affixJson.ExtraDamage.MinDamage}.");
                        if (affixJson.ExtraDamage.MaxDamage < 0)
                            messages.AddError($"Affix {affixJson.Id}'s Extra Damage has a negative Max Damage of {affixJson.ExtraDamage.MaxDamage}.");
                        if (affixJson.ExtraDamage.MinDamage > affixJson.ExtraDamage.MaxDamage)
                            messages.AddError($"Affix {affixJson.Id}'s Extra Damage has a Min Damage of {affixJson.ExtraDamage.MinDamage} greater than its Max Damage of {affixJson.ExtraDamage.MaxDamage}.");
                        if (!string.IsNullOrWhiteSpace(affixJson.ExtraDamage.Element) && !sampleDungeon.Elements.Any(e => e.Id.Equals(affixJson.ExtraDamage.Element, StringComparison.InvariantCultureIgnoreCase)))
                            messages.AddError($"Affix {affixJson.Id}'s Extra Damage has an invalid Element of {affixJson.ExtraDamage.Element}.");
                    }
                    messages.AddRange(await ActionValidator.Validate(affixJson.OnTurnStart, dungeonJson));
                    if (affixAsInstance != null)
                        messages.AddRange(await ActionValidator.Validate(affixAsInstance.OwnOnTurnStart, dungeonJson, sampleDungeon));
                    messages.AddRange(await ActionValidator.Validate(affixJson.OnAttack, dungeonJson));
                    if (affixAsInstance != null)
                        messages.AddRange(await ActionValidator.Validate(affixAsInstance.OwnOnAttack, dungeonJson, sampleDungeon));
                    messages.AddRange(await ActionValidator.Validate(affixJson.OnAttacked, dungeonJson));
                    if (affixAsInstance != null)
                        messages.AddRange(await ActionValidator.Validate(affixAsInstance.OwnOnAttacked, dungeonJson, sampleDungeon));
                }
                else
                {
                    messages.AddError($"Affix #{dungeonJson.AffixInfos.IndexOf(affixJson)} lacks an Id.");
                }
            }

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}
