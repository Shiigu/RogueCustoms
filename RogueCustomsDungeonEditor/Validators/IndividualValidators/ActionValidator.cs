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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
{
    public static class ActionValidator
    {
        public static DungeonValidationMessages Validate(ActionWithEffects action, DungeonInfo dungeonJson, Dungeon sampleDungeon)
        {
            var messages = new DungeonValidationMessages();

            var owner = action.User;
            var name = action.Name;
            var description = action.Description;
            var ownerName = owner != null ? owner.ClassId : "The Floor Type";

            if(!string.IsNullOrWhiteSpace(action.UseCondition))
            {
                if(action.UseCondition.IsBooleanExpression() && action.UseCondition.TestBooleanExpression(out _))
                    messages.AddWarning($"Action {action.Name ?? "NULL"} has a Use Condition that seems to be a valid boolean expression, but you must check in-game whether it works as intended.");
                else
                    messages.AddError($"Action {action.Name ?? "NULL"} has a Use Condition that does not seem to be a valid boolean expression.");
            }

            if (owner != null)
            {
                messages.AddRange(dungeonJson.ValidateString(action.NameLocaleKey, $"An Action of {ownerName}", "Name", true));
                messages.AddRange(dungeonJson.ValidateString(action.DescriptionLocaleKey, $"Action {action.Name ?? "NULL"}", "Description", false));

                var duplicateTargetTypes = action.TargetTypes.GroupBy(tt => tt).Where(gtt => gtt.Count() > 1);
                foreach (var targetType in duplicateTargetTypes.Select(gtt => gtt.Key))
                {
                    messages.AddError($"Action {action.Name ?? "NULL"} has {targetType} as a duplicate TargetType.");
                }

                if (action.MinimumRange < 0)
                    messages.AddError($"Action {action.Name ?? "NULL"} has a MinimumRange under 0, which is not valid.");
                else if (action.MinimumRange == 0 && action.TargetTypes.Any() && !action.TargetTypes.Any(tt => tt == TargetType.Self))
                    messages.AddError($"Action {action.Name ?? "NULL"} has a MinimumRange of 0 but does not have Self as a TargetType, making the MinimumRange useless.");
                else if (action.MinimumRange > 0 && action.TargetTypes.Any() && action.TargetTypes.All(tt => tt == TargetType.Self))
                    messages.AddError($"Action {action.Name ?? "NULL"} has a MinimumRange above 0 but only has Self as a TargetType, making the action unusable.");
                if (action.MaximumRange < 0)
                    messages.AddError($"Action {action.Name ?? "NULL"} has a MaximumRange under 0, which is not valid.");
                else if (action.MaximumRange == 0 && action.TargetTypes.Any() && !action.TargetTypes.Any(tt => tt == TargetType.Self))
                    messages.AddError($"Action {action.Name ?? "NULL"} has a MaximumRange of 0 but does not have Self as a TargetType, making the MaximumRange useless.");
                else if (action.MaximumRange > 0 && action.TargetTypes.Any() && action.TargetTypes.All(tt => tt == TargetType.Self))
                    messages.AddError($"Action {action.Name ?? "NULL"} has a MaximumRange above 0 but only has Self as a TargetType, making the action unusable.");

                if (action.MinimumRange > action.MaximumRange)
                    messages.AddError($"Action {action.Name ?? "NULL"} has a MinimumRange higher than its MaximumRange.");

                if (action.CooldownBetweenUses < 0)
                    messages.AddError($"Action {action.Name ?? "NULL"} has a CooldownBetweenUses under 0, which is not valid.");

                if (action.StartingCooldown < 0)
                    messages.AddError($"Action {action.Name ?? "NULL"} has a StartingCooldown under 0, which is not valid.");
                else if (action.CooldownBetweenUses < action.StartingCooldown)
                    messages.AddWarning($"Action {action.Name ?? "NULL"} has a StartingCooldown higher than its CooldownBetweenUses.");

                if (action.MaximumUses < 0)
                    messages.AddError($"Action {action.Name ?? "NULL"} has a MaximumUses under 0, which is not valid.");
            }
            else
            {
                if (action.MinimumRange < 0)
                    messages.AddError($"Action {action.Name ?? "NULL"} has a MinimumRange under 0, which is not valid.");
                else if (action.MinimumRange > 0)
                    messages.AddWarning($"Action {action.Name ?? "NULL"} has a MinimumRange above 0, which will be ignored by the game. Consider removing it.");
                if (action.MaximumRange < 0)
                    messages.AddError($"Action {action.Name ?? "NULL"} has a MaximumRange under 0, which is not valid.");
                else if (action.MaximumRange > 0)
                    messages.AddWarning($"Action {action.Name ?? "NULL"} has a MaximumRange above 0, which will be ignored by the game. Consider removing it.");
                if (action.CooldownBetweenUses < 0)
                    messages.AddError($"Action {action.Name ?? "NULL"} has a CooldownBetweenUses under 0, which is not valid.");
                else if (action.CooldownBetweenUses > 0)
                    messages.AddWarning($"Action {action.Name ?? "NULL"} has a CooldownBetweenUses above 0, which will be ignored by the game. Consider removing it.");
                if (action.StartingCooldown < 0)
                    messages.AddError($"Action {action.Name ?? "NULL"} has a StartingCooldown under 0, which is not valid.");
                else if (action.StartingCooldown > 0)
                    messages.AddWarning($"Action {action.Name ?? "NULL"} has a StartingCooldown above 0, which will be ignored by the game. Consider removing it.");
                if (action.MaximumUses < 0)
                    messages.AddError($"Action {action.Name ?? "NULL"} has a MaximumUses under 0, which is not valid.");
                else if (action.MaximumUses > 0)
                    messages.AddWarning($"Action {action.Name ?? "NULL"} has a MaximumUses above 0, which will be ignored by the game. Consider removing it.");
            }


            Entity source;
            Character target = new NonPlayableCharacter(sampleDungeon.Classes.Find(ec => ec.EntityType == EntityType.Player || ec.EntityType == EntityType.NPC), 1, sampleDungeon.CurrentFloor);
            target.EquippedWeapon = new Item(sampleDungeon.Classes.Find(ec => ec.EntityType == EntityType.Weapon), sampleDungeon.CurrentFloor);
            target.EquippedArmor = new Item(sampleDungeon.Classes.Find(ec => ec.EntityType == EntityType.Armor), sampleDungeon.CurrentFloor);

            foreach (var als in sampleDungeon.Classes.Where(ec => ec.EntityType == EntityType.AlteredStatus && (owner == null || ec.Id != owner.ClassId)))
            {
                target.AlteredStatuses.Add(new AlteredStatus(als, sampleDungeon.CurrentFloor)
                {
                    CleansedByCleanseActions = true,
                    RemainingTurns = -1,
                    Power = 1
                });
            }

            if (owner != null && owner is Character c)
            {
                source = c;
                if(source.OwnOnAttack.Contains(action))
                {
                    (source as Character).Faction = sampleDungeon.Factions.First();
                    var fillerWeapon = new Item(sampleDungeon.Classes.Find(ec => ec.EntityType == EntityType.Weapon && ec.Id.Equals(c.StartingWeaponId)), sampleDungeon.CurrentFloor);
                    fillerWeapon.Owner = source as Character;
                    (source as Character).EquippedWeapon = fillerWeapon;
                    var fillerArmor = new Item(sampleDungeon.Classes.Find(ec => ec.EntityType == EntityType.Armor && ec.Id.Equals(c.StartingArmorId)), sampleDungeon.CurrentFloor);
                    fillerArmor.Owner = source as Character;
                    (source as Character).EquippedArmor = fillerArmor;
                }
            }
            else if (owner != null && (owner.EntityType == EntityType.Weapon || owner.EntityType == EntityType.Armor))
            {
                source = new NonPlayableCharacter(sampleDungeon.Classes.Find(ec => ec.EntityType == EntityType.Player || ec.EntityType == EntityType.NPC), 1, sampleDungeon.CurrentFloor);
                (owner as Item).Owner = source as NonPlayableCharacter;
                if (owner.EntityType == EntityType.Weapon)
                {
                    (source as Character).EquippedWeapon = (owner as Item);
                    var fillerArmor = new Item(sampleDungeon.Classes.Find(ec => ec.EntityType == EntityType.Armor), sampleDungeon.CurrentFloor);
                    fillerArmor.Owner = source as NonPlayableCharacter;
                    (source as Character).EquippedArmor = fillerArmor;
                }
                else if (owner.EntityType == EntityType.Armor)
                {
                    var fillerWeapon = new Item(sampleDungeon.Classes.Find(ec => ec.EntityType == EntityType.Weapon), sampleDungeon.CurrentFloor);
                    fillerWeapon.Owner = source as NonPlayableCharacter;
                    (source as Character).EquippedWeapon = fillerWeapon;
                    (source as Character).EquippedArmor = (owner as Item);
                }
            }
            else if (owner == null)
                source = new NonPlayableCharacter(sampleDungeon.Classes.Find(ec => ec.EntityType == EntityType.Player || ec.EntityType == EntityType.NPC), 1, sampleDungeon.CurrentFloor);
            else
                source = owner;
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

            bool errorOnActionChain = false;
            var currentEffect = action.Effect;
            var pendingEffects = new List<Effect>();

            if (currentEffect == null)
                messages.AddError($"Action {action.Name ?? "NULL"} has no function chain programmed to it.");
            else
                pendingEffects.Add(currentEffect);

            while (pendingEffects.Any(pe => pe != null) && !errorOnActionChain)
            {
                var nextEffect = pendingEffects.FirstOrDefault(pe => pe != null);
                if (nextEffect == null) break;
                pendingEffects.Remove(nextEffect);
                if(nextEffect.Function != null)
                {
                    var functionName = nextEffect.Function.Method.Name;

                    if (functionName == "ApplyAlteredStatus")
                    {
                        // We remove the status before testing so that, if !CanStack && !CanOverwrite, the first attempt always returns Success (the rest will return Failure).
                        var statusId = Array.Find(nextEffect.Params, p => p.ParamName.Equals("Id", StringComparison.InvariantCultureIgnoreCase)).Value;
                        target.AlteredStatuses.RemoveAll(als => als.ClassId.Equals(statusId, StringComparison.InvariantCultureIgnoreCase));
                    }

                    if (nextEffect.Then != null && nextEffect.OnSuccess != null && nextEffect.OnFailure != null)
                    {
                        errorOnActionChain = true;
                        messages.AddError($"Action {action.Name ?? "NULL"} has both a Then and an OnSuccess/OnFailure programmed to it. Either has to be removed.");
                    }

                    try
                    {
                        if (!nextEffect.HaveAllParametersBeenParsed(owner, source, target, sampleDungeon.CurrentFloor, out bool flagsAreInvolved))
                        {
                            errorOnActionChain = true;
                            messages.AddError($"The effect {functionName} of {action.Name ?? "NULL"} has parameters that haven't been parsed.");
                        }
                        if(flagsAreInvolved)
                            messages.AddWarning($"The effect {functionName} of {action.Name ?? "NULL"} makes use of Flags. Due to their variability, they have been hardcoded for the Validator, and can only be properly validated in-game.");
                    }
                    catch (Exception ex)
                    {
                        errorOnActionChain = true;
                        messages.AddError($"The effect {functionName} of {action.Name ?? "NULL"} has thrown an Exception when trying to parse its parameters: {ex.Message}.");
                    }

                    if (!errorOnActionChain)
                    {
                        int amountOfSuccesses = 0, amountOfFailures = 0;

                        var excessiveTargetHPWarning = false;
                        var excessiveTargetMPWarning = false;
                        var excessiveTargetAttackWarning = false;
                        var excessiveTargetDefenseWarning = false;
                        var excessiveTargetMovementWarning = false;
                        var excessiveTargetHPRegenerationWarning = false;
                        var excessiveTargetMPRegenerationWarning = false;
                        var excessiveSourceHPWarning = false;
                        var excessiveSourceMPWarning = false;
                        var excessiveSourceAttackWarning = false;
                        var excessiveSourceDefenseWarning = false;
                        var excessiveSourceMovementWarning = false;
                        var excessiveSourceHPRegenerationWarning = false;
                        var excessiveSourceMPRegenerationWarning = false;
                        var excessiveThisHPWarning = false;
                        var excessiveThisMPWarning = false;
                        var excessiveThisAttackWarning = false;
                        var excessiveThisDefenseWarning = false;
                        var excessiveThisMovementWarning = false;
                        var excessiveThisHPRegenerationWarning = false;
                        var excessiveThisMPRegenerationWarning = false;

                        try
                        {
                            target.HP = target.MaxHP;
                            target.MP = target.MaxMP;
                            var defenseTestModification = target.DefenseModifications.Find(dm => dm.Id == "defenseTest");
                            if (defenseTestModification == null)
                            {
                                target.DefenseModifications.Add(new StatModification
                                {
                                    Id = "defenseTest",
                                    Amount = target.BaseDefense * -1, // Defense is turned into 0
                                    RemainingTurns = -1
                                });
                            }
                            else
                                defenseTestModification.Amount = target.BaseDefense * -1; // Defense is turned into 0
                            target.AlteredStatuses.Add(new AlteredStatus(sampleDungeon.Classes.Find(ec => ec.EntityType == EntityType.AlteredStatus), sampleDungeon.CurrentFloor)
                            {
                                RemainingTurns = -1,
                                CleansedByCleanseActions = true,
                                ClassId = "test"    // So that it doesn't interfere with regular statuses
                            });
                            for (int i = 0; i < 100; i++)
                            {
                                if (nextEffect.TestFunction(owner, source, target))
                                {
                                    amountOfSuccesses++;
                                }
                                else
                                {
                                    amountOfFailures++;
                                }
                                defenseTestModification = target.DefenseModifications.Find(dm => dm.Id == "defenseTest");
                                if (defenseTestModification != null)
                                    defenseTestModification.Amount++;
                                if (target.HP <= 0)
                                    target.HP = target.MaxHP;
                                else
                                    target.HP--;  // Slightly damage it so that heals may work
                                if (target.MP <= 0)
                                    target.MP = target.MaxMP;
                                else
                                    target.MP--;  // Slightly burn its MP so that replenishes may work
                                if (target.TotalMaxHPIncrements > 1000000)
                                {
                                    if(!excessiveTargetHPWarning)
                                        messages.AddWarning($"The effect {functionName} of {action.Name ?? "NULL"} has sent Target's HP stat above 1000000 after {i} attempts. Check if this is expected, as it might cause an Integer overflow.");
                                    target.MaxHPModifications.Clear();
                                    excessiveTargetHPWarning = true;
                                }
                                if (target.TotalMaxMPIncrements > 1000000)
                                {
                                    if (!excessiveTargetMPWarning)
                                        messages.AddWarning($"The effect {functionName} of {action.Name ?? "NULL"} has sent Target's MP stat above 1000000 after {i} attempts. Check if this is expected, as it might cause an Integer overflow.");
                                    target.MaxMPModifications.Clear();
                                    excessiveTargetMPWarning = true;
                                }
                                if (target.TotalAttackIncrements > 1000000)
                                {
                                    if(!excessiveTargetAttackWarning)
                                        messages.AddWarning($"The effect {functionName} of {action.Name ?? "NULL"} has sent Target's Attack stat above 1000000 after {i} attempts. Check if this is expected, as it might cause an Integer overflow.");
                                    target.AttackModifications.Clear();
                                    excessiveTargetAttackWarning = true;
                                }
                                if (target.TotalDefenseIncrements > 1000000)
                                {
                                    if(!excessiveTargetDefenseWarning)
                                        messages.AddWarning($"The effect {functionName} of {action.Name ?? "NULL"} has sent Target's Defense stat above 1000000 after {i} attempts. Check if this is expected, as it might cause an Integer overflow.");
                                    target.DefenseModifications.Clear();
                                    excessiveTargetDefenseWarning = true;
                                }
                                if (target.TotalMovementIncrements > 1000000)
                                {
                                    if(!excessiveTargetMovementWarning)
                                        messages.AddWarning($"The effect {functionName} of {action.Name ?? "NULL"} has sent Target's Movement stat above 1000000 after {i} attempts. Check if this is expected, as it might cause an Integer overflow.");
                                    target.MovementModifications.Clear();
                                    excessiveTargetMovementWarning = true;
                                }
                                if (target.TotalHPRegenerationIncrements > 1000000)
                                {
                                    if(!excessiveTargetHPRegenerationWarning)
                                        messages.AddWarning($"The effect {functionName} of {action.Name ?? "NULL"} has sent Target's HP Regeneration stat above 1000000 after {i} attempts. Check if this is expected, as it might cause an Integer overflow.");
                                    target.HPRegenerationModifications.Clear();
                                    excessiveTargetHPRegenerationWarning = true;
                                }
                                if (target.TotalMPRegenerationIncrements > 1000000)
                                {
                                    if (!excessiveTargetMPRegenerationWarning)
                                        messages.AddWarning($"The effect {functionName} of {action.Name ?? "NULL"} has sent Target's MP Regeneration stat above 1000000 after {i} attempts. Check if this is expected, as it might cause an Integer overflow.");
                                    target.MPRegenerationModifications.Clear();
                                    excessiveTargetMPRegenerationWarning = true;
                                }
                                if (source is Character s)
                                {
                                    if (s.TotalMaxHPIncrements > 1000000)
                                    {
                                        if(!excessiveSourceHPWarning)
                                            messages.AddWarning($"The effect {functionName} of {action.Name ?? "NULL"} has sent Source's HP stat above 1000000 after {i} attempts. Check if this is expected, as it might cause an Integer overflow.");
                                        s.MaxHPModifications.Clear();
                                        excessiveSourceHPWarning = true;
                                    }
                                    if (s.TotalMaxMPIncrements > 1000000)
                                    {
                                        if (!excessiveSourceMPWarning)
                                            messages.AddWarning($"The effect {functionName} of {action.Name ?? "NULL"} has sent Source's MP stat above 1000000 after {i} attempts. Check if this is expected, as it might cause an Integer overflow.");
                                        s.MaxMPModifications.Clear();
                                        excessiveSourceMPWarning = true;
                                    }
                                    if (s.TotalAttackIncrements > 1000000)
                                    {
                                        if(!excessiveSourceAttackWarning)
                                            messages.AddWarning($"The effect {functionName} of {action.Name ?? "NULL"} has sent Source's Attack stat above 1000000 after {i} attempts. Check if this is expected, as it might cause an Integer overflow.");
                                        s.AttackModifications.Clear();
                                        excessiveSourceAttackWarning = true;
                                    }
                                    if (s.TotalDefenseIncrements > 1000000)
                                    {
                                        if(!excessiveSourceDefenseWarning)
                                            messages.AddWarning($"The effect {functionName} of {action.Name ?? "NULL"} has sent Source's Defense stat above 1000000 after {i} attempts. Check if this is expected, as it might cause an Integer overflow.");
                                        s.DefenseModifications.Clear();
                                        excessiveSourceDefenseWarning = true;
                                    }
                                    if (s.TotalMovementIncrements > 1000000)
                                    {
                                        if(!excessiveSourceMovementWarning)
                                            messages.AddWarning($"The effect {functionName} of {action.Name ?? "NULL"} has sent Source's Movement stat above 1000000 after {i} attempts. Check if this is expected, as it might cause an Integer overflow.");
                                        s.MovementModifications.Clear();
                                        excessiveSourceMovementWarning = true;
                                    }
                                    if (s.TotalHPRegenerationIncrements > 1000000)
                                    {
                                        if(!excessiveSourceHPRegenerationWarning)
                                            messages.AddWarning($"The effect {functionName} of {action.Name ?? "NULL"} has sent Source's HP Regeneration stat above 1000000 after {i} attempts. Check if this is expected, as it might cause an Integer overflow.");
                                        s.HPRegenerationModifications.Clear();
                                        excessiveSourceHPRegenerationWarning = true;
                                    }
                                    if (s.TotalMPRegenerationIncrements > 1000000)
                                    {
                                        if (!excessiveSourceMPRegenerationWarning)
                                            messages.AddWarning($"The effect {functionName} of {action.Name ?? "NULL"} has sent Source's MP Regeneration stat above 1000000 after {i} attempts. Check if this is expected, as it might cause an Integer overflow.");
                                        s.MPRegenerationModifications.Clear();
                                        excessiveSourceMPRegenerationWarning = true;
                                    }
                                }
                                if (owner is Character o)
                                {
                                    if (o.TotalMaxHPIncrements > 1000000)
                                    {
                                        if (!excessiveThisHPWarning)
                                            messages.AddWarning($"The effect {functionName} of {action.Name ?? "NULL"} has sent This's HP stat above 1000000 after {i} attempts. Check if this is expected, as it might cause an Integer overflow.");
                                        o.MaxHPModifications.Clear();
                                        excessiveThisHPWarning = true;
                                    }
                                    if (o.TotalMaxMPIncrements > 1000000)
                                    {
                                        if (!excessiveThisMPWarning)
                                            messages.AddWarning($"The effect {functionName} of {action.Name ?? "NULL"} has sent This's MP stat above 1000000 after {i} attempts. Check if this is expected, as it might cause an Integer overflow.");
                                        o.MaxMPModifications.Clear();
                                        excessiveThisMPWarning = true;
                                    }
                                    if (o.TotalAttackIncrements > 1000000)
                                    {
                                        if (!excessiveThisAttackWarning)
                                            messages.AddWarning($"The effect {functionName} of {action.Name ?? "NULL"} has sent This's Attack stat above 1000000 after {i} attempts. Check if this is expected, as it might cause an Integer overflow.");
                                        o.AttackModifications.Clear();
                                        excessiveThisAttackWarning = true;
                                    }
                                    if (o.TotalDefenseIncrements > 1000000)
                                    {
                                        if (!excessiveThisDefenseWarning)
                                            messages.AddWarning($"The effect {functionName} of {action.Name ?? "NULL"} has sent This's Defense stat above 1000000 after {i} attempts. Check if this is expected, as it might cause an Integer overflow.");
                                        o.DefenseModifications.Clear();
                                        excessiveThisDefenseWarning = true;
                                    }
                                    if (o.TotalMovementIncrements > 1000000)
                                    {
                                        if (!excessiveThisMovementWarning)
                                            messages.AddWarning($"The effect {functionName} of {action.Name ?? "NULL"} has sent This's Movement stat above 1000000 after {i} attempts. Check if this is expected, as it might cause an Integer overflow.");
                                        o.MovementModifications.Clear();
                                        excessiveThisMovementWarning = true;
                                    }
                                    if (o.TotalHPRegenerationIncrements > 1000000)
                                    {
                                        if (!excessiveThisHPRegenerationWarning)
                                            messages.AddWarning($"The effect {functionName} of {action.Name ?? "NULL"} has sent This's HP Regeneration stat above 1000000 after {i} attempts. Check if this is expected, as it might cause an Integer overflow.");
                                        o.HPRegenerationModifications.Clear();
                                        excessiveThisHPRegenerationWarning = true;
                                    }
                                    if (o.TotalMPRegenerationIncrements > 1000000)
                                    {
                                        if (!excessiveThisMPRegenerationWarning)
                                            messages.AddWarning($"The effect {functionName} of {action.Name ?? "NULL"} has sent This's MP Regeneration stat above 1000000 after {i} attempts. Check if this is expected, as it might cause an Integer overflow.");
                                        o.MPRegenerationModifications.Clear();
                                        excessiveThisMPRegenerationWarning = true;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            errorOnActionChain = true;
                            messages.AddError($"The effect {functionName} of {action.Name ?? "NULL"} has thrown an Exception when running: {ex.Message}.");
                        }

                        if (!errorOnActionChain)
                        {
                            if (nextEffect.OnSuccess != null && nextEffect.OnFailure != null)
                            {
                                if (amountOfSuccesses == 0)
                                    messages.AddWarning($"The effect {functionName} of {action.Name ?? "NULL"} has OnSuccess/OnFailure but it never returned Success in 100 different attempts. Please check.");
                                else if (amountOfFailures == 0)
                                    messages.AddWarning($"The effect {functionName} of {action.Name ?? "NULL"} has OnSuccess/OnFailure but it never returned Failure in 100 different attempts. Please check.");
                            }
                            else if (nextEffect.OnSuccess != null && nextEffect.OnFailure == null && amountOfSuccesses == 0)
                            {
                                messages.AddWarning($"The effect {functionName} of {action.Name ?? "NULL"} only has OnSuccess but it never returned Success in 100 different attempts. Please check.");
                            }
                            else if (nextEffect.OnSuccess == null && nextEffect.OnFailure != null && amountOfFailures == 0)
                            {
                                messages.AddWarning($"The effect {functionName} of {action.Name ?? "NULL"} only has OnFailure but it never returned Failure in 100 different attempts. Please check.");
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
                    messages.AddError($"Action {action.Name ?? "NULL"} attempts to call an undefined function.");
                }
            }

            return messages;
        }
    }
}
