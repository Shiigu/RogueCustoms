using org.matheval;
using RogueCustomsDungeonEditor.Utils;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
{
    public class DungeonItemValidator
    {
        public static async Task<DungeonValidationMessages> Validate(ItemInfo itemJson, DungeonInfo dungeonJson, Dungeon sampleDungeon)
        {
            var itemAsInstance = sampleDungeon != null ? new Item(new EntityClass(itemJson, null, sampleDungeon, dungeonJson.CharacterStats), 1, sampleDungeon.CurrentFloor) : null;
            var messages = new DungeonValidationMessages();

            messages.AddRange(dungeonJson.ValidateString(itemJson.Name, "Item", "Name", true));
            messages.AddRange(dungeonJson.ValidateString(itemJson.Description, "Item", "Description", true));

            messages.AddRange(itemJson.ConsoleRepresentation.Validate(itemJson.Id, false, dungeonJson));

            if(!string.IsNullOrWhiteSpace(itemJson.EntityType))
            {
                if (string.IsNullOrWhiteSpace(itemJson.Power))
                {
                    var powerFieldName = itemJson.EntityType switch
                    {
                        "Weapon" => "Weapon Damage",
                        "Armor" => "Armor Mitigation",
                        "Consumable" => "Consumable Power",
                        _ => "Item Power"
                    };
                    messages.AddError($"Item does not have a set {powerFieldName}. If it's not meant to be used, place a 0.");
                }

                if(itemJson.BaseValue < 0)
                {
                    messages.AddError($"Item has a Base Sale Value lower than 0.");
                }
                else if (itemJson.BaseValue == 0)
                {
                    messages.AddWarning($"Item has a Base Sale Value that is 0. It won't be able to be sold or purchased.");
                }

                if (itemJson.OnTurnStart != null)
                {
                    if (itemJson.EntityType == "Weapon" || itemJson.EntityType == "Armor")
                    {
                        messages.AddRange(await ActionValidator.Validate(itemJson.OnTurnStart, dungeonJson));
                        if(itemAsInstance != null)
                            messages.AddRange(await ActionValidator.Validate(itemAsInstance.OwnOnTurnStart, dungeonJson, sampleDungeon));
                    }
                    else
                    {
                        messages.AddWarning("Item is not equippable but has OnTurnStartActions. This will be ignored by the game. Consider removing it.");
                    }
                }

                var validStatIds = dungeonJson.CharacterStats.ConvertAll(s => s.Id);

                if (itemJson.StatModifiers != null && itemJson.StatModifiers.Any())
                {
                    foreach (var stat in itemJson.StatModifiers)
                    {
                        if(!validStatIds.Contains(stat.Id.ToLowerInvariant()))
                        {
                            messages.AddWarning($"Item's modification is invalid as there is no stat with Id {stat.Id}.");
                            continue;
                        }
                        if(stat.Amount == 0)
                            messages.AddWarning($"Item's modification on {stat.Id} is set to 0. This won't have any effect.");
                    }
                }

                if (itemJson.OnAttack.Any())
                {
                    if (itemJson.EntityType == "Weapon" || itemJson.EntityType == "Consumable")
                    {
                        foreach (var onAttackAction in itemJson.OnAttack)
                        {
                            messages.AddRange(await ActionValidator.Validate(onAttackAction, dungeonJson));
                        }
                        if (itemAsInstance != null)
                        {
                            if (itemAsInstance.OwnOnAttack.HasMinimumMatches(ooa => ooa.Id.ToLower(), 2))
                            {
                                messages.AddError("Item has at least two Attack actions with the same Id.");
                            }
                            foreach (var onAttackAction in itemAsInstance.OwnOnAttack)
                            {
                                foreach (var playerClass in dungeonJson.PlayerClasses)
                                {
                                    if (playerClass.OnAttack.Any(oa => oa.Id.Equals(onAttackAction.Id, StringComparison.InvariantCultureIgnoreCase)))
                                        messages.AddError($"Item's Attack action, {onAttackAction.Id}, has the same Id as one of Player Class {playerClass.Id}'s Attack actions.");
                                }
                                foreach (var npc in dungeonJson.NPCs)
                                {
                                    if (npc.OnAttack.Any(oa => oa.Id.Equals(onAttackAction.Id, StringComparison.InvariantCultureIgnoreCase)))
                                        messages.AddError($"Item's Attack action, {onAttackAction.Id}, has the NPC Id as one of Item {npc.Id}'s Attack actions.");
                                    if (npc.OnInteracted.Any(oa => oa.Id.Equals(onAttackAction.Id, StringComparison.InvariantCultureIgnoreCase)))
                                        messages.AddError($"Item's Attack action, {onAttackAction.Id}, has the NPC Id as one of Item {npc.Id}'s Interacted actions.");
                                }
                                messages.AddRange(await ActionValidator.Validate(onAttackAction, dungeonJson, sampleDungeon));
                            }
                        }
                    }
                    else if (itemJson.EntityType == "Armor")
                    {
                        messages.AddWarning("Armor has OnAttackActions. Are you sure about that?");
                    }
                }
                else if (itemJson.EntityType == "Weapon")
                {
                    messages.AddWarning("Weapon does not have OnAttackActions. Weapon cannot do anything in this current state.");
                }

                var maximumQualityLevel = dungeonJson.QualityLevelInfos.Find(qli => qli.Id.Equals(itemJson.MaximumQualityLevel));
                if(maximumQualityLevel == null)
                {
                    messages.AddError($"Item has an invalid Maximum Quality Level.");
                    foreach (var odd in itemJson.QualityLevelOdds)
                    {
                        var correspondingQualityLevel = dungeonJson.QualityLevelInfos.Find(qli => qli.Id.Equals(odd.Id));
                        if (correspondingQualityLevel == null)
                        {
                            messages.AddError("Item has odds for an Maximum Quality Level.");
                        }
                        else
                        {
                            if (odd.ChanceToPick < 0)
                                messages.AddError($"Item has an invalid Weight for Quality Level {odd.Id}. It must be a non-negative integer.");
                            if (odd.ChanceToPick > 0 && correspondingQualityLevel.MaximumAffixes > maximumQualityLevel.MaximumAffixes)
                                messages.AddError($"Item has odds for Quality Level {odd.Id}, which has more affixes than the item's Maximum Quality Level and is thus superior.");
                        }
                    }
                }

                if (itemJson.OnAttacked != null)
                {
                    if (itemJson.EntityType == "Armor")
                    {
                        messages.AddRange(await ActionValidator.Validate(itemJson.OnAttacked, dungeonJson));
                        if (itemAsInstance != null)
                            messages.AddRange(await ActionValidator.Validate(itemAsInstance.OwnOnAttacked, dungeonJson, sampleDungeon));
                    }
                    else if (itemJson.EntityType == "Weapon")
                    {
                        messages.AddWarning("Weapon has OnAttacked. Are you sure about that?");
                    }
                    else if (itemJson.EntityType == "Consumable")
                    {
                        messages.AddWarning("Consumable has OnAttacked, which will be ignored by the game. Consider removing it.");
                    }
                }

                if (itemJson.OnDeath != null)
                {
                    messages.AddRange(await ActionValidator.Validate(itemJson.OnDeath, dungeonJson));
                    if (itemAsInstance != null)
                        messages.AddRange(await ActionValidator.Validate(itemAsInstance.OwnOnDeath, dungeonJson, sampleDungeon));
                }

                if (itemJson.OnUse != null)
                {
                    messages.AddRange(await ActionValidator.Validate(itemJson.OnUse, dungeonJson));
                    if (itemAsInstance != null)
                        messages.AddRange(await ActionValidator.Validate(itemAsInstance.OnUse, dungeonJson, sampleDungeon));
                }
                else if (itemJson.EntityType == "Consumable" && !itemJson.OnAttack.Any())
                {
                    messages.AddWarning("Item doesn't have any OnItemUseActions or OnAttackActions. Item lacks selectable Actions in this current state.");
                }

                if (!dungeonJson.FloorInfos.Exists(fi => fi.PossibleItems.Exists(pm => pm.ClassId.Equals(itemJson.Id))))
                {
                    if (itemJson.EntityType == "Consumable" || !dungeonJson.NPCs.Exists(c => c.StartingWeapon.Equals(itemJson.Id) || c.StartingArmor.Equals(itemJson.Id)))
                    {
                        messages.AddWarning("Item does not show up in any list of PossibleItems. It will never be spawned. Consider adding it to a PossibleItems list.");
                    }
                }
            }
            else
            {
                messages.AddError("Item doesn't have an EntityType.");
            }


            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}
