using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsGameEngine.Game.Entities.NPCAIStrategies
{
    public class DefaultNPCAIStrategy : INPCAIStrategy
    {
        public int GetActionWeight(ActionWithEffects action, Map map, Entity This, NonPlayableCharacter Source, ITargetable Target)
        {
            // Very heavily discourage NPCs from using actions that cannot be applied on the target in the current turn.
            if (!action.CanBeUsedOn(Target, Source))
                return int.MinValue;

            var randomFactor = map.Rng.NextInclusive(50, 150) / 100f;
            var mpUseFactor = Source.UsesMP ? ((double) action.MPCost / Source.MaxMP) : 0;
            return (int)(GetEffectWeight(action.Effect, map, This, Source, Target) * (randomFactor - mpUseFactor));
        }

        public int GetEffectWeight(Effect effect, Map map, Entity This, NonPlayableCharacter Source, ITargetable Target)
        {
            var weight = 0;

            var targetAsCharacter = Target is Character c ? c : null;
            var targetAsTile = Target is Tile t ? t : null;
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, 0, effect.Params);
            var accuracyFactor = effect.Params.Any(p => p.ParamName == "Accuracy")
                 ? Math.Min(1, ActionHelpers.CalculateAdjustedAccuracy(Source, targetAsCharacter, paramsObject) / 100f * 2)
                 : 1;

            switch (effect.Function.Method.Name)
            {
                case "DealDamage":
                    if (targetAsCharacter == null || targetAsCharacter.ExistenceStatus != EntityExistenceStatus.Alive)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    var damageDealt = Math.Max(0, paramsObject.Damage - paramsObject.Mitigation);
                    if (damageDealt > 0 && damageDealt < 1)
                        damageDealt = 1;
                    damageDealt = (int)damageDealt;
                    if (damageDealt > targetAsCharacter.HP)
                        weight = 5000000;
                    else
                        weight = 250 + damageDealt * 50;

                    weight = (int)(weight * accuracyFactor);
                    break;

                case "BurnMP":
                    if (targetAsCharacter == null || targetAsCharacter.ExistenceStatus != EntityExistenceStatus.Alive || !targetAsCharacter.UsesMP)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    var burnAmount = paramsObject.Power;
                    if (paramsObject.Power > 0 && paramsObject.Power < 1)
                        burnAmount = 1;
                    weight = (int)((250 + burnAmount * 50) * accuracyFactor);
                    break;

                case "RemoveHunger":
                    if (targetAsCharacter == null || targetAsCharacter.ExistenceStatus != EntityExistenceStatus.Alive || !targetAsCharacter.UsesHunger)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    var hungerRemoveAmount = paramsObject.Power;
                    if (paramsObject.Power > 0 && paramsObject.Power < 1)
                        hungerRemoveAmount = 1;
                    weight = (int)((100 + hungerRemoveAmount * 50) * accuracyFactor);
                    break;

                case "StealItem":
                    if (targetAsCharacter == null || targetAsCharacter.ExistenceStatus != EntityExistenceStatus.Alive || Source.ItemCount >= Source.InventorySize)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    var stealableItems = new List<Item>();
                    if (paramsObject.CanStealEquippables)
                        stealableItems.AddRange(targetAsCharacter.Inventory.Where(i => i.IsEquippable));
                    if (paramsObject.CanStealConsumables)
                        stealableItems.AddRange(targetAsCharacter.Inventory.Where(i => i.EntityType == EntityType.Consumable));
                    if (stealableItems.Any())
                    {
                        weight = (int)(500 * accuracyFactor);
                    }
                    break;

                case "HealDamage":
                    if (targetAsCharacter == null || targetAsCharacter.ExistenceStatus != EntityExistenceStatus.Alive || targetAsCharacter.HP >= targetAsCharacter.MaxHP)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    var healAmount = Math.Min(targetAsCharacter.MaxHP - targetAsCharacter.HP, paramsObject.Power);
                    if (paramsObject.Power > 0 && paramsObject.Power < 1)
                        healAmount = 1;
                    weight = (int)((250 + healAmount * 50) * accuracyFactor);
                    break;

                case "ReplenishMP":
                    if (targetAsCharacter == null || targetAsCharacter.ExistenceStatus != EntityExistenceStatus.Alive || !targetAsCharacter.UsesMP || targetAsCharacter.MP >= targetAsCharacter.MaxMP)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    var replenishAmount = Math.Min(targetAsCharacter.MaxMP - targetAsCharacter.MP, paramsObject.Power);
                    if (paramsObject.Power > 0 && paramsObject.Power < 1)
                        replenishAmount = 1;
                    weight = (int)((250 + replenishAmount * 50) * accuracyFactor);
                    break;

                case "ReplenishHunger":
                    if (targetAsCharacter == null || targetAsCharacter.ExistenceStatus != EntityExistenceStatus.Alive || !targetAsCharacter.UsesHunger || targetAsCharacter.Hunger >= targetAsCharacter.MaxHunger)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    replenishAmount = Math.Min(targetAsCharacter.MaxMP - targetAsCharacter.MP, paramsObject.Power);
                    if (paramsObject.Power > 0 && paramsObject.Power < 1)
                        replenishAmount = 1;
                    weight = (int)((50 + replenishAmount * 50) * accuracyFactor);
                    break;

                case "ApplyAlteredStatus":
                    if (targetAsCharacter == null || targetAsCharacter.ExistenceStatus != EntityExistenceStatus.Alive)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    var alteredStatus = map.PossibleStatuses.Find(als => als.ClassId.Equals(paramsObject.Id));
                    if (alteredStatus == null)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    if (!alteredStatus.CanOverwrite && !alteredStatus.CanStack && targetAsCharacter.AlteredStatuses.Exists(als => als.ClassId.Equals(paramsObject.Id)))
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    var turnLengthWeight = paramsObject.TurnLength > 0 ? (int)Math.Min(1, Math.Max(10, paramsObject.TurnLength / 5)) : 10;
                    var powerWeight = paramsObject.Power > 0 ? (int)paramsObject.Power : 1;
                    weight = (int)(250 + (powerWeight * 5 + turnLengthWeight) * accuracyFactor);
                    break;

                case "ApplyStatAlteration":
                    if (targetAsCharacter == null || targetAsCharacter.ExistenceStatus != EntityExistenceStatus.Alive)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }

                    var isHPOrMP = string.Equals(paramsObject.StatName, "hp", StringComparison.InvariantCultureIgnoreCase)
                                    || string.Equals(paramsObject.StatName, "maxhp", StringComparison.InvariantCultureIgnoreCase)
                                    || string.Equals(paramsObject.StatName, "mp", StringComparison.InvariantCultureIgnoreCase)
                                    || string.Equals(paramsObject.StatName, "maxmp", StringComparison.InvariantCultureIgnoreCase);
                    var isHPOrMPRegeneration = string.Equals(paramsObject.StatName, "hpregeneration", StringComparison.InvariantCultureIgnoreCase)
                                    || string.Equals(paramsObject.StatName, "mpregeneration", StringComparison.InvariantCultureIgnoreCase);

                    var statAlterations = paramsObject.StatAlterationList as List<StatModification>;
                    var alterationAmount = isHPOrMPRegeneration ? paramsObject.Amount : (int)paramsObject.Amount;

                    if (!isHPOrMPRegeneration)
                    {
                        if (paramsObject.Amount > 0 && paramsObject.Amount < 1)
                            alterationAmount = 1;
                        else if (paramsObject.Amount < 0 && paramsObject.Amount > -1)
                            alterationAmount = -1;
                    }
                    if (paramsObject.Amount != 0 && (!paramsObject.CanBeStacked && statAlterations.Exists(sa => sa.RemainingTurns > 0 && sa.Id.Equals(paramsObject.Id))))
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }

                    turnLengthWeight = paramsObject.TurnLength > 0 ? (int)paramsObject.TurnLength : 50;
                    var alterationWeight = Math.Abs(alterationAmount);
                    if (isHPOrMP)
                        alterationWeight /= 10;
                    else if (isHPOrMPRegeneration)
                        alterationWeight *= 10;
                    weight = (int)(250 + ((int)alterationWeight * 5 + turnLengthWeight) * accuracyFactor);
                    break;
                case "CleanseAlteredStatus":
                    if (targetAsCharacter == null || targetAsCharacter.ExistenceStatus != EntityExistenceStatus.Alive)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    if (!targetAsCharacter.AlteredStatuses.Exists(als => als.ClassId.Equals(paramsObject.Id)))
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    weight = (int)(50 * accuracyFactor);
                    break;
                case "CleanseStatAlteration":
                    if (targetAsCharacter == null || targetAsCharacter.ExistenceStatus != EntityExistenceStatus.Alive)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    statAlterations = paramsObject.StatAlterationList as List<StatModification>;
                    if (!statAlterations.Exists(sa => sa.RemainingTurns > 0 && sa.Id.Equals(paramsObject.Id)))
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    weight = (int)(50 * accuracyFactor);
                    break;

                case "CleanseAllAlteredStatuses":
                    if (targetAsCharacter == null || targetAsCharacter.ExistenceStatus != EntityExistenceStatus.Alive)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    weight = (int)(50 * targetAsCharacter.AlteredStatuses.Count * accuracyFactor);
                    break;

                case "CleanseStatAlterations":
                    if (targetAsCharacter == null || targetAsCharacter.ExistenceStatus != EntityExistenceStatus.Alive)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    statAlterations = paramsObject.StatAlterationList as List<StatModification>;
                    weight = (int)(50 * statAlterations.Count * accuracyFactor);
                    break;

                case "ForceSkipTurn":
                    if (targetAsCharacter == null || targetAsCharacter.ExistenceStatus != EntityExistenceStatus.Alive || !targetAsCharacter.CanTakeAction)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    weight = (int)(500 * accuracyFactor);
                    break;

                case "Teleport":
                case "ToggleVisibility":
                    if (targetAsCharacter == null || targetAsCharacter.ExistenceStatus != EntityExistenceStatus.Alive)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    weight = (int)(50 * accuracyFactor);
                    break;

                case "GiveItem":
                    if (targetAsCharacter == null || targetAsCharacter.ExistenceStatus != EntityExistenceStatus.Alive || targetAsCharacter.ItemCount >= targetAsCharacter.InventorySize)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    weight = (int)(100 * accuracyFactor);
                    break;

                case "PlaceTrap":
                    if (targetAsTile == null)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    var distanceToUse = Source.Position != targetAsTile.Position ? (int)GamePoint.Distance(Source.Position, targetAsTile.Position) : 0.5;
                    weight = (int)(150 * accuracyFactor / distanceToUse);
                    break;

                case "SpawnNPC":
                case "ReviveNPC":
                    if (targetAsTile == null || targetAsTile.LivingCharacter != null)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    distanceToUse = Source.Position != targetAsTile.Position ? (int)GamePoint.Distance(Source.Position, targetAsTile.Position) : 0.5;
                    weight = (int)(150 * accuracyFactor / distanceToUse);
                    break;

                case "Remove":
                case "ReplaceConsoleRepresentation":
                case "PrintText":
                case "MessageBox":
                case "GiveExperience":
                case "GenerateStairs":
                case "CheckCondition":
                case "SetFlag":
                    // These functions are irrelevant for NPC decision making
                    break;

                default:
                    throw new NotImplementedException($"Weight calculation not implemented for function: {effect.Function.Method.Name}");
            }

            if (effect.Then != null)
            {
                weight += GetEffectWeight(effect.Then, map, This, Source, Target);
            }
            else
            {
                if (effect.OnSuccess != null)
                    weight += GetEffectWeight(effect.OnSuccess, map, This, Source, Target);
                if (effect.OnFailure != null)
                    weight += GetEffectWeight(effect.OnFailure, map, This, Source, Target);
            }

            return weight;
        }
    }
}
