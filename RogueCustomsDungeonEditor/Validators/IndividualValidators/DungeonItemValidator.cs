using org.matheval;
using RogueCustomsDungeonEditor.Utils;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
{
    public class DungeonItemValidator
    {
        public static DungeonValidationMessages Validate(ItemInfo itemJson, DungeonInfo dungeonJson, Dungeon sampleDungeon)
        {
            var itemAsInstance = new Item(new EntityClass(itemJson, sampleDungeon.LocaleToUse, null), sampleDungeon.CurrentFloor);
            var messages = new DungeonValidationMessages();

            messages.AddRange(dungeonJson.ValidateString(itemJson.Name, "Item", "Name", true));
            messages.AddRange(dungeonJson.ValidateString(itemJson.Description, "Item", "Description", true));

            messages.AddRange(itemJson.ConsoleRepresentation.Validate(itemJson.Id, false, dungeonJson));

            if(string.IsNullOrWhiteSpace(itemJson.Power))
                messages.AddWarning("Item does not have a set Power. Remember to hardcode Action Power parameters if needed, or the game may crash.");

            if (itemJson.OnTurnStart != null)
            {
                if (itemJson.EntityType == "Weapon" || itemJson.EntityType == "Armor")
                {
                    messages.AddRange(ActionValidator.Validate(itemAsInstance.OwnOnTurnStart, dungeonJson, sampleDungeon));
                }
                else
                {
                    messages.AddWarning("Item is not equippable but has OnTurnStartActions. This will be ignored by the game. Consider removing it.");
                }
            }

            if (itemJson.OnAttack.Any())
            {
                if (itemJson.EntityType == "Weapon" || itemJson.EntityType == "Consumable")
                {
                    foreach (var onAttackAction in itemAsInstance.OwnOnAttack)
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
                messages.AddWarning("Weapon does not have OnAttackActions. Weapon cannot do anything in this current state.");
            }

            if (itemJson.OnAttacked != null)
            {
                if (itemJson.EntityType == "Armor")
                {
                    messages.AddRange(ActionValidator.Validate(itemAsInstance.OwnOnAttacked, dungeonJson, sampleDungeon));
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

            if (itemJson.OnStepped != null)
            {
                messages.AddRange(ActionValidator.Validate(itemAsInstance.OnStepped, dungeonJson, sampleDungeon));
            }


            if (itemJson.OnUse != null)
            {
                messages.AddRange(ActionValidator.Validate(itemAsInstance.OnUse, dungeonJson, sampleDungeon));
            }
            else if (itemJson.EntityType == "Weapon" || itemJson.EntityType == "Armor")
            {
                messages.AddWarning("Equippable Item doesn't have any OnItemUseActions. If it's not set as a StartingWeapon or StartingArmor of a Character, the item will be unusable.");
            }
            else if (!itemJson.OnAttack.Any())
            {
                messages.AddWarning("Consumable Item doesn't have any OnItemUseActions or OnAttackActions. Item cannot do anything in this current state.");
            }

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
