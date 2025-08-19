using MathNet.Numerics.Statistics;
using RogueCustomsDungeonEditor.Utils;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.Exceptions;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
{
#pragma warning disable CS8604 // Posible argumento de referencia nulo
#pragma warning disable CS8601 // Posible asignación de referencia nula
    public static class ActionValidator
    {
        public static async Task<DungeonValidationMessages> Validate(ActionWithEffects action, DungeonInfo dungeonJson, Dungeon sampleDungeon)
        {
            var messages = new DungeonValidationMessages();

            var owner = action.User;
            var name = action.Name;
            var ownerName = owner != null ? owner.ClassId : "The Floor Type";

            if (string.IsNullOrWhiteSpace(action.Id))
            {
                messages.AddError($"Action {name ?? "NULL"} does not have an Id.");
            }

            if (!string.IsNullOrWhiteSpace(action.UseCondition))
            {
                if(action.UseCondition.IsBooleanExpression() && action.UseCondition.TestBooleanExpression(out _))
                    messages.AddWarning($"Action {name ?? "NULL"} has a Use Condition that seems to be a valid boolean expression, but you must check in-game whether it works as intended.");
                else
                    messages.AddError($"Action {name ?? "NULL"} has a Use Condition that does not seem to be a valid boolean expression.");
            }
            if (!string.IsNullOrWhiteSpace(action.AIUseCondition))
            {
                if (action.AIUseCondition.IsBooleanExpression() && action.AIUseCondition.TestBooleanExpression(out _))
                    messages.AddWarning($"Action {name ?? "NULL"} has an AI Use Condition that seems to be a valid boolean expression, but you must check in-game whether it works as intended.");
                else
                    messages.AddError($"Action {name ?? "NULL"} has an AI Use Condition that does not seem to be a valid boolean expression.");
            }

            var isSelectable = (owner is PlayerCharacter opc && opc.OnAttack.Contains(action))
                                || (owner is Item oi && oi.OwnOnAttack.Contains(action))
                                || (owner is NonPlayableCharacter onpc && onpc.OnInteracted.Contains(action));

            if (owner != null)
            {
                if (isSelectable)
                {
                    messages.AddRange(dungeonJson.ValidateString(action.NameLocaleKey, $"An Action of {ownerName}", "Name", true));
                    messages.AddRange(dungeonJson.ValidateString(action.DescriptionLocaleKey, $"Action {name ?? "NULL"}", "Description", false));
                }

                var duplicateTargetTypes = action.TargetTypes.GroupBy(tt => tt).Where(gtt => gtt.Count() > 1);
                foreach (var targetType in duplicateTargetTypes.Select(gtt => gtt.Key))
                {
                    messages.AddError($"Action {name ?? "NULL"} has {targetType} as a duplicate TargetType.");
                }

                if (action.MinimumRange < 0)
                    messages.AddError($"Action {name ?? "NULL"} has a MinimumRange under 0, which is not valid.");
                
                if (action.MaximumRange < 0)
                    messages.AddError($"Action {name ?? "NULL"} has a MaximumRange under 0, which is not valid.");

                if (action.MinimumRange > action.MaximumRange)
                    messages.AddError($"Action {name ?? "NULL"} has a MinimumRange higher than its MaximumRange.");

                if (!action.TargetTypes.Exists(tt => tt == TargetType.Tile))
                {
                    if (action.MinimumRange == 0 && action.TargetTypes.Any() && !action.TargetTypes.Exists(tt => tt == TargetType.Self))
                        messages.AddError($"Action {name ?? "NULL"} has a MinimumRange of 0 but does not have Self as a TargetType, making the MinimumRange useless.");
                    else if (action.MinimumRange > 0 && action.TargetTypes.Any() && action.TargetTypes.TrueForAll(tt => tt == TargetType.Self))
                        messages.AddError($"Action {name ?? "NULL"} has a MinimumRange above 0 but only has Self as a TargetType, making the action unusable.");
                    if (action.MaximumRange == 0 && action.TargetTypes.Any() && !action.TargetTypes.Exists(tt => tt == TargetType.Self))
                        messages.AddError($"Action {name ?? "NULL"} has a MaximumRange of 0 but does not have Self as a TargetType, making the MaximumRange useless.");
                    else if (action.MaximumRange > 0 && action.TargetTypes.Any() && action.TargetTypes.TrueForAll(tt => tt == TargetType.Self))
                        messages.AddError($"Action {name ?? "NULL"} has a MaximumRange above 0 but only has Self as a TargetType, making the action unusable.");
                }

                if (action.CooldownBetweenUses < 0)
                    messages.AddError($"Action {name ?? "NULL"} has a CooldownBetweenUses under 0, which is not valid.");

                if (action.StartingCooldown < 0)
                    messages.AddError($"Action {name ?? "NULL"} has a StartingCooldown under 0, which is not valid.");
                else if (action.CooldownBetweenUses < action.StartingCooldown)
                    messages.AddWarning($"Action {name ?? "NULL"} has a StartingCooldown higher than its CooldownBetweenUses.");

                if (action.MaximumUses < 0)
                    messages.AddError($"Action {name ?? "NULL"} has a MaximumUses under 0, which is not valid.");
            }
            else
            {
                if (action.MinimumRange < 0)
                    messages.AddError($"Action {name ?? "NULL"} has a MinimumRange under 0, which is not valid.");
                else if (action.MinimumRange > 0)
                    messages.AddWarning($"Action {name ?? "NULL"} has a MinimumRange above 0, which will be ignored by the game. Consider removing it.");
                if (action.MaximumRange < 0)
                    messages.AddError($"Action {name ?? "NULL"} has a MaximumRange under 0, which is not valid.");
                else if (action.MaximumRange > 0)
                    messages.AddWarning($"Action {name ?? "NULL"} has a MaximumRange above 0, which will be ignored by the game. Consider removing it.");
                if (action.CooldownBetweenUses < 0)
                    messages.AddError($"Action {name ?? "NULL"} has a CooldownBetweenUses under 0, which is not valid.");
                else if (action.CooldownBetweenUses > 0)
                    messages.AddWarning($"Action {name ?? "NULL"} has a CooldownBetweenUses above 0, which will be ignored by the game. Consider removing it.");
                if (action.StartingCooldown < 0)
                    messages.AddError($"Action {name ?? "NULL"} has a StartingCooldown under 0, which is not valid.");
                else if (action.StartingCooldown > 0)
                    messages.AddWarning($"Action {name ?? "NULL"} has a StartingCooldown above 0, which will be ignored by the game. Consider removing it.");
                if (action.MaximumUses < 0)
                    messages.AddError($"Action {name ?? "NULL"} has a MaximumUses under 0, which is not valid.");
                else if (action.MaximumUses > 0)
                    messages.AddWarning($"Action {name ?? "NULL"} has a MaximumUses above 0, which will be ignored by the game. Consider removing it.");
            }

            Entity source;
            var target = GetATestCharacter(sampleDungeon);

            target.EquippedWeapon = GetATestItem(sampleDungeon, EntityType.Weapon);
            target.EquippedArmor = GetATestItem(sampleDungeon, EntityType.Armor);

            foreach (var als in sampleDungeon.Classes.Where(ec => ec.EntityType == EntityType.AlteredStatus && (owner == null || ec.Id != owner.ClassId)))
            {
                target.AlteredStatuses.Add(new AlteredStatus(als, sampleDungeon.CurrentFloor)
                {
                    CleansedByCleanseActions = true,
                    RemainingTurns = -1,
                    Power = 1
                });
            }

            if (owner is Character c)
            {
                source = c;
                if (source.OwnOnAttack.Contains(action))
                {
                    (source as Character).Faction = sampleDungeon.Factions[0];
                    var fillerWeapon = GetASpecificItem(sampleDungeon, c.StartingWeaponId);
                    fillerWeapon.Owner = source as Character;
                    (source as Character).EquippedWeapon = fillerWeapon;
                    var fillerArmor = GetASpecificItem(sampleDungeon, c.StartingArmorId);
                    fillerArmor.Owner = source as Character;
                    (source as Character).EquippedArmor = fillerArmor;
                }
            }
            else if (owner is Item item)
            {
                source = GetATestCharacter(sampleDungeon);
                item.Owner = source as NonPlayableCharacter;
                if (item.IsEquippable && action != item.OwnOnDeath)
                {
                    if (owner.EntityType == EntityType.Weapon)
                    {
                        (source as Character).EquippedWeapon = item;
                        var fillerArmor = GetATestItem(sampleDungeon, EntityType.Armor);
                        fillerArmor.Owner = source as Character;
                        (source as Character).EquippedArmor = fillerArmor;
                    }
                    else if (owner.EntityType == EntityType.Armor)
                    {
                        var fillerWeapon = GetATestItem(sampleDungeon, EntityType.Weapon);
                        fillerWeapon.Owner = source as Character;
                        (source as Character).EquippedWeapon = fillerWeapon;
                        (source as Character).EquippedArmor = item;
                    }
                }
            }
            else if (owner is AlteredStatus alteredStatus)
            {
                source = GetATestCharacter(sampleDungeon);
                var fillerWeapon = GetATestItem(sampleDungeon, EntityType.Weapon);
                fillerWeapon.Owner = source as Character;
                (source as Character).EquippedWeapon = fillerWeapon;
                (source as Character).EquippedArmor = owner as Item;
                var fillerArmor = GetATestItem(sampleDungeon, EntityType.Armor);
                fillerArmor.Owner = source as Character;
                (source as Character).EquippedArmor = fillerArmor;
                (source as Character).AlteredStatuses.Add(alteredStatus);
            }
            else if (owner is Item i && action == i.OwnOnDeath)
            {
                source = GetATestCharacter(sampleDungeon);
                i.Owner = source as NonPlayableCharacter;
                if (owner.EntityType == EntityType.Weapon)
                {
                    (source as Character).EquippedWeapon = i;
                    var fillerArmor = GetATestItem(sampleDungeon, EntityType.Armor);
                    fillerArmor.Owner = source as Character;
                    (source as Character).EquippedArmor = fillerArmor;
                }
                else if (owner.EntityType == EntityType.Armor)
                {
                    var fillerWeapon = GetATestItem(sampleDungeon, EntityType.Weapon);
                    fillerWeapon.Owner = source as Character;
                    (source as Character).EquippedWeapon = fillerWeapon;
                    (source as Character).EquippedArmor = i;
                }
                else if (owner.EntityType == EntityType.Consumable)
                {
                    var fillerWeapon = GetATestItem(sampleDungeon, EntityType.Weapon);
                    fillerWeapon.Owner = source as Character;
                    var fillerArmor = GetATestItem(sampleDungeon, EntityType.Armor);
                    fillerArmor.Owner = source as Character;
                    (source as Character).EquippedWeapon = fillerWeapon;
                    (source as Character).EquippedArmor = fillerArmor;
                }
            }
            else if (owner == null)
            {
                source = GetATestCharacter(sampleDungeon);
            }
            else
            {
                source = owner;
            }

            target.Position = sampleDungeon.CurrentFloor.Tiles.Find(t => t.IsWalkable).Position;
            if (source is Character)
            {
                source.Position = target.Position;
                (source as Character).CanTakeAction = true;
            }
            target.CanTakeAction = true;

            var sampleWeaponClass = sampleDungeon.Classes.Find(ec => ec.EntityType == EntityType.Weapon);
            if(sampleWeaponClass != null)
            {
                target.Inventory.Add(new Item(sampleWeaponClass, sampleDungeon.CurrentFloor));
            }
            var sampleArmorClass = sampleDungeon.Classes.Find(ec => ec.EntityType == EntityType.Armor);
            if (sampleArmorClass != null)
            {
                target.Inventory.Add(new Item(sampleArmorClass, sampleDungeon.CurrentFloor));
            }
            var sampleConsumableClass = sampleDungeon.Classes.Find(ec => ec.EntityType == EntityType.Consumable);
            if (sampleConsumableClass != null)
            {
                target.Inventory.Add(new Item(sampleConsumableClass, sampleDungeon.CurrentFloor));
            }

            sampleDungeon.CurrentFloor.SetActionParams();

            if (!action.TargetTypes.Contains(TargetType.Tile))
                await TestOnACharacter(owner, source, target, sampleDungeon, action, messages);
            else
                await TestOnATile(owner, source, sampleDungeon, action, messages);

            return messages;
        }

        private static async Task TestOnACharacter(Entity owner, Entity source, Character target, Dungeon sampleDungeon, ActionWithEffects action, DungeonValidationMessages messages)
        {
            var errorOnActionChain = false;
            var pendingEffects = new List<Effect>();
            var name = action.Name;
            var currentEffect = action.Effect;
            var amountOfSuccesses = 0;
            var amountOfFailures = 0;

            if (currentEffect == null)
                messages.AddError($"Action {name ?? "NULL"} has no function chain programmed to it.");
            else
                pendingEffects.Add(currentEffect);

            while (pendingEffects.Exists(pe => pe != null) && !errorOnActionChain)
            {
                var nextEffect = pendingEffects.Find(pe => pe != null);
                if (nextEffect == null) break;
                pendingEffects.Remove(nextEffect);
                if (nextEffect.AsyncFunction != null || nextEffect.Function != null)
                {
                    var functionName = nextEffect.AsyncFunction != null
                        ? nextEffect.AsyncFunction.Method.Name
                        : nextEffect.Function.Method.Name;

                    if (functionName == "ApplyAlteredStatus")
                    {
                        // We remove the status before testing so that, if !CanStack && !CanOverwrite, the first attempt always returns Success (the rest will return Failure).
                        var statusId = nextEffect.Params.Find(p => p.ParamName.Equals("Id", StringComparison.InvariantCultureIgnoreCase)).Value;
                        target.AlteredStatuses.RemoveAll(als => als.ClassId.Equals(statusId, StringComparison.InvariantCultureIgnoreCase));
                    }

                    if (nextEffect.Then != null && nextEffect.OnSuccess != null && nextEffect.OnFailure != null)
                    {
                        errorOnActionChain = true;
                        messages.AddError($"Action {name ?? "NULL"} has both a Then and an OnSuccess/OnFailure programmed to it. Either has to be removed.");
                    }

                    try
                    {
                        if (!nextEffect.HaveAllParametersBeenParsed(owner, source, target, sampleDungeon.CurrentFloor, out bool flagsAreInvolved))
                        {
                            errorOnActionChain = true;
                            messages.AddError($"The effect {functionName} of {name ?? "NULL"} has parameters that haven't been parsed.");
                        }
                        if (flagsAreInvolved)
                            messages.AddWarning($"The effect {functionName} of {name ?? "NULL"} makes use of Flags. Due to their variability, they have been hardcoded for the Validator, and can only be properly validated in-game.");
                    }
                    catch (Exception ex)
                    {
                        errorOnActionChain = true;
                        messages.AddError($"The effect {functionName} of {name ?? "NULL"} has thrown an Exception when trying to parse its parameters: {ex.Message}.");
                    }

                    if (!errorOnActionChain)
                    {

                        try
                        {
                            target.HP.Current = target.MaxHP;
                            if(target.MP != null)
                                target.MP.Current = target.MaxMP;
                            var defenseTestModification = target.Defense.ActiveModifications.Find(dm => dm.Id == "defenseTest");
                            if (defenseTestModification == null)
                            {
                                target.Defense.ActiveModifications.Add(new StatModification
                                {
                                    Id = "defenseTest",
                                    Amount = target.Defense.Base * -1, // Defense is turned into 0
                                    RemainingTurns = -1
                                });
                            }
                            else
                            {
                                defenseTestModification.Amount = target.Defense.Base * -1; // Defense is turned into 0
                            }

                            target.AlteredStatuses.Add(new AlteredStatus(sampleDungeon.Classes.Find(ec => ec.EntityType == EntityType.AlteredStatus), sampleDungeon.CurrentFloor)
                            {
                                RemainingTurns = -1,
                                CleansedByCleanseActions = true,
                                ClassId = "test"    // So that it doesn't interfere with regular statuses
                            });
                            for (int i = 0; i < 100; i++)
                            {
                                if (await nextEffect.TestFunction(owner, source, target))
                                {
                                    amountOfSuccesses++;
                                }
                                else
                                {
                                    amountOfFailures++;
                                }
                                defenseTestModification = target.Defense.ActiveModifications.Find(dm => dm.Id == "defenseTest");
                                if (defenseTestModification != null)
                                    defenseTestModification.Amount++;
                                if (target.HP.Current <= 0)
                                    target.HP.Current = target.MaxHP;
                                else
                                    target.HP.Current--;  // Slightly damage it so that heals may work
                                if(target.MP != null)
                                {
                                    if (target.MP.Current <= 0)
                                        target.MP.Current = target.MaxMP;
                                    else
                                        target.MP.Current--;  // Slightly burn its MP so that replenishes may work
                                }
                            }
                        }
                        catch (FlagNotFoundException ex)
                        {
                            messages.AddWarning($"The effect {functionName} of {name ?? "NULL"} can cause the read of a Flag, {ex.FlagName}, that is not generated with autonomously. Make sure it generates previously, or the game will throw an error.");
                        }
                        catch (Exception ex)
                        {
                            errorOnActionChain = true;
                            messages.AddError($"The effect {functionName} of {name ?? "NULL"} has thrown an Exception when running against {target?.ClassId ?? "NULL"}: {ex.Message}.");
                        }

                        if (!errorOnActionChain)
                        {
                            if (nextEffect.OnSuccess != null && nextEffect.OnFailure != null)
                            {
                                if (amountOfSuccesses == 0)
                                    messages.AddWarning($"The effect {functionName} of {name ?? "NULL"} has OnSuccess/OnFailure but it never returned Success in 100 different attempts. Please check.");
                                else if (amountOfFailures == 0)
                                    messages.AddWarning($"The effect {functionName} of {name ?? "NULL"} has OnSuccess/OnFailure but it never returned Failure in 100 different attempts. Please check.");
                            }
                            else if (nextEffect.OnSuccess != null && nextEffect.OnFailure == null && amountOfSuccesses == 0)
                            {
                                messages.AddWarning($"The effect {functionName} of {name ?? "NULL"} only has OnSuccess but it never returned Success in 100 different attempts. Please check.");
                            }
                            else if (nextEffect.OnSuccess == null && nextEffect.OnFailure != null && amountOfFailures == 0)
                            {
                                messages.AddWarning($"The effect {functionName} of {name ?? "NULL"} only has OnFailure but it never returned Failure in 100 different attempts. Please check.");
                            }

                            if (nextEffect.Then != null)
                            {
                                pendingEffects.Add(nextEffect.Then);
                            }
                            else if (nextEffect.OnSuccess != null && nextEffect.OnFailure != null)
                            {
                                pendingEffects.Add(nextEffect.OnSuccess);
                                pendingEffects.Add(nextEffect.OnFailure);
                            }
                        }
                    }
                }
                else
                {
                    messages.AddError($"Action {name ?? "NULL"} attempts to call an undefined function.");
                }
            }
        }
        private static async Task TestOnATile(Entity owner, Entity source, Dungeon sampleDungeon, ActionWithEffects action, DungeonValidationMessages messages)
        {
            var errorOnActionChain = false;
            var pendingEffects = new List<Effect>();
            var name = action.Name;
            var currentEffect = action.Effect;
            var amountOfSuccesses = 0;
            var amountOfFailures = 0;

            if (currentEffect == null)
                messages.AddError($"Action {name ?? "NULL"} has no function chain programmed to it.");
            else
                pendingEffects.Add(currentEffect);

            while (pendingEffects.Exists(pe => pe != null) && !errorOnActionChain)
            {
                var target = sampleDungeon.CurrentFloor.Tiles.GetRandomElement();
                var nextEffect = pendingEffects.Find(pe => pe != null);
                if (nextEffect == null) break;
                pendingEffects.Remove(nextEffect);
                if (nextEffect.AsyncFunction != null || nextEffect.Function != null)
                {
                    var functionName = nextEffect.AsyncFunction != null
                        ? nextEffect.AsyncFunction.Method.Name
                        : nextEffect.Function.Method.Name;

                    if (nextEffect.Then != null && nextEffect.OnSuccess != null && nextEffect.OnFailure != null)
                    {
                        errorOnActionChain = true;
                        messages.AddError($"Action {name ?? "NULL"} has both a Then and an OnSuccess/OnFailure programmed to it. Either has to be removed.");
                    }

                    try
                    {
                        if (!nextEffect.HaveAllParametersBeenParsed(owner, source, null, sampleDungeon.CurrentFloor, out bool flagsAreInvolved))
                        {
                            errorOnActionChain = true;
                            messages.AddError($"The effect {functionName} of {name ?? "NULL"} has parameters that haven't been parsed.");
                        }
                        if (flagsAreInvolved)
                            messages.AddWarning($"The effect {functionName} of {name ?? "NULL"} makes use of Flags. Due to their variability, they have been hardcoded for the Validator, and can only be properly validated in-game.");
                    }
                    catch (Exception ex)
                    {
                        errorOnActionChain = true;
                        messages.AddError($"The effect {functionName} of {name ?? "NULL"} has thrown an Exception when trying to parse its parameters: {ex.Message}.");
                    }

                    if (!errorOnActionChain)
                    {
                        try
                        {
                            if (await nextEffect.TestFunction(owner, source, target))
                            {
                                amountOfSuccesses++;
                            }
                            else
                            {
                                amountOfFailures++;
                            }
                        }
                        catch (FlagNotFoundException ex)
                        {
                            messages.AddWarning($"The effect {functionName} of {name ?? "NULL"} can cause the read of a Flag, {ex.FlagName}, that is not generated autonomously. Make sure it generates previously, or the game will throw an error.");
                        }
                        catch (Exception ex)
                        {
                            errorOnActionChain = true;
                            messages.AddError($"The effect {functionName} of {name ?? "NULL"} has thrown an Exception when running against a certain {target?.Type} Tile: {ex.Message}.");
                        }

                        if (!errorOnActionChain)
                        {
                            if (nextEffect.OnSuccess != null && nextEffect.OnFailure != null)
                            {
                                if (amountOfSuccesses == 0)
                                    messages.AddWarning($"The effect {functionName} of {name ?? "NULL"} has OnSuccess/OnFailure but it never returned Success in 100 different attempts. Please check.");
                                else if (amountOfFailures == 0)
                                    messages.AddWarning($"The effect {functionName} of {name ?? "NULL"} has OnSuccess/OnFailure but it never returned Failure in 100 different attempts. Please check.");
                            }
                            else if (nextEffect.OnSuccess != null && nextEffect.OnFailure == null && amountOfSuccesses == 0)
                            {
                                messages.AddWarning($"The effect {functionName} of {name ?? "NULL"} only has OnSuccess but it never returned Success in 100 different attempts. Please check.");
                            }
                            else if (nextEffect.OnSuccess == null && nextEffect.OnFailure != null && amountOfFailures == 0)
                            {
                                messages.AddWarning($"The effect {functionName} of {name ?? "NULL"} only has OnFailure but it never returned Failure in 100 different attempts. Please check.");
                            }

                            if (nextEffect.Then != null)
                            {
                                pendingEffects.Add(nextEffect.Then);
                            }
                            else if (nextEffect.OnSuccess != null && nextEffect.OnFailure != null)
                            {
                                pendingEffects.Add(nextEffect.OnSuccess);
                                pendingEffects.Add(nextEffect.OnFailure);
                            }
                        }
                    }
                }
                else
                {
                    messages.AddError($"Action {name ?? "NULL"} attempts to call an undefined function.");
                }
            }
        }

        private static Character GetATestCharacter(Dungeon sampleDungeon)
        {
            var npcClasses = sampleDungeon.Classes.Where(ec => ec.EntityType == EntityType.NPC).ToList();
            var playerClasses = sampleDungeon.Classes.Where(ec => ec.EntityType == EntityType.Player).ToList();
            return sampleDungeon.Classes.Exists(ec => ec.EntityType == EntityType.NPC)
                ? new NonPlayableCharacter(npcClasses[new Random().Next(npcClasses.Count)], 1, sampleDungeon.CurrentFloor)
                : new PlayerCharacter(playerClasses[new Random().Next(playerClasses.Count)], 1, sampleDungeon.CurrentFloor);
        }

        private static Item GetASpecificItem(Dungeon sampleDungeon, string classId)
        {
            return new Item(sampleDungeon.Classes.Find(ec => ec.Id.Equals(classId)), sampleDungeon.CurrentFloor);
        }

        private static Item GetATestItem(Dungeon sampleDungeon, EntityType entityType)
        {
            return new Item(sampleDungeon.Classes.Find(ec => ec.EntityType == entityType), sampleDungeon.CurrentFloor);
        }
    }
    #pragma warning restore CS8601 // Posible asignación de referencia nula
    #pragma warning restore CS8604 // Posible argumento de referencia nulo
}
