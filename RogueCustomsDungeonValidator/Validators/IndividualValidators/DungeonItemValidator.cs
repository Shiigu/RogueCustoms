using org.matheval;
using RogueCustomsDungeonValidator.Utils;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsDungeonValidator.Validators.IndividualValidators
{
    public class DungeonItemValidator
    {
        public static DungeonValidationMessages Validate(ClassInfo itemJson, DungeonInfo dungeonJson, Dungeon sampleDungeon)
        {
            var itemAsInstance = new Item(new EntityClass(itemJson, sampleDungeon.LocaleToUse, null), sampleDungeon.CurrentFloor);
            var messages = new DungeonValidationMessages();

            messages.AddRange(dungeonJson.ValidateString(itemJson.Name, "Item", "Name", true));
            messages.AddRange(dungeonJson.ValidateString(itemJson.Description, "Item", "Description", true));

            messages.AddRange(itemJson.ConsoleRepresentation.Validate(itemJson.Id, false, dungeonJson));

            if(string.IsNullOrWhiteSpace(itemJson.Power))
                messages.AddWarning("Item does not have a set Power. Remember to hardcode Action Power parameters if needed, or the game may crash.");

            if (itemJson.OnTurnStartActions.Any())
            {
                if (itemJson.EntityType == "Weapon" || itemJson.EntityType == "Armor")
                {
                    foreach (var onTurnStartAction in itemJson.OnTurnStartActions.ConvertAll(otsa => new ActionWithEffects(otsa)))
                    {
                        messages.AddRange(ActionValidator.Validate(onTurnStartAction, dungeonJson, sampleDungeon));
                    }
                }
                else
                {
                    messages.AddWarning("Item is not equippable but has OnTurnStartActions. This will be ignored by the game. Consider removing it.");
                }
            }

            if (itemJson.OnAttackActions.Any())
            {
                if (itemJson.EntityType == "Weapon" || itemJson.EntityType == "Consumable")
                {
                    foreach (var onAttackAction in itemAsInstance.OwnOnAttackActions)
                    {
                        messages.AddRange(ActionValidator.Validate(onAttackAction, dungeonJson, sampleDungeon));
                    }
                }
                else if (itemJson.EntityType == "Armor")
                {
                    messages.AddWarning("Armor has OnAttackActions. Are you sure about that?");
                }
            }
            else if (itemJson.EntityType == "Weapon")
            {
                messages.AddError("Weapon does not have OnAttackActions.");
            }

            if (itemJson.OnAttackedActions.Any())
            {
                if (itemJson.EntityType == "Armor")
                {
                    foreach (var onAttackedAction in itemAsInstance.OwnOnAttackedActions)
                    {
                        messages.AddRange(ActionValidator.Validate(onAttackedAction, dungeonJson, sampleDungeon));
                    }
                }
                else if (itemJson.EntityType == "Weapon")
                {
                    messages.AddWarning("Weapon has OnAttackedActions. Are you sure about that?");
                }
                else if (itemJson.EntityType == "Consumable")
                {
                    messages.AddWarning("Consumable has OnAttackedActions, which will be ignored by the game. Consider removing it.");
                }
            }

            foreach (var onItemSteppedAction in itemAsInstance.OnItemSteppedActions)
            {
                messages.AddRange(ActionValidator.Validate(onItemSteppedAction, dungeonJson, sampleDungeon));
            }


            if (itemJson.OnItemUseActions.Any())
            {
                foreach (var onItemUseAction in itemAsInstance.OnItemUseActions)
                {
                    messages.AddRange(ActionValidator.Validate(onItemUseAction, dungeonJson, sampleDungeon));
                }
            }
            else if (itemJson.EntityType == "Weapon" || itemJson.EntityType == "Armor")
            {
                messages.AddWarning("Equippable Item doesn't have any OnItemUseActions. If it's not set as a StartingWeapon or StartingArmor of a Character, the item will be unusable.");
            }
            else if (!itemJson.OnAttackActions.Any())
            {
                messages.AddError("Consumable Item doesn't have any OnItemUseActions or OnAttackActions.");
            }

            if (itemJson.OnDeathActions.Any())
                messages.AddWarning("Item has OnDeathActions, which will be ignored by the game. Consider removing it.");
            if (itemJson.OnStatusApplyActions.Any())
                messages.AddWarning("Item has OnStatusApplyActions, which will be ignored by the game. Consider removing it.");

            if (!dungeonJson.FloorInfos.Any(fi => fi.PossibleItems.Any(pm => pm.ClassId.Equals(itemJson.Id))))
            {
                if(itemJson.EntityType == "Consumable" || !dungeonJson.NPCs.Any(c => c.StartingWeapon.Equals(itemJson.Id) || c.StartingArmor.Equals(itemJson.Id)))
                {
                    messages.AddWarning("Item does not show up in any list of PossibleItems. It will never be spawned. Consider adding it to a PossibleItems list.");
                }
            }

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}
