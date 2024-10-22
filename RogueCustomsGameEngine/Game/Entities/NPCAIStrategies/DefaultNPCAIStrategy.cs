using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.Expressions;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsGameEngine.Game.Entities.NPCAIStrategies
{
    public class DefaultNPCAIStrategy : INPCAIStrategy
    {
        public int GetActionWeight(ActionWithEffects action, Map map, Entity This, NonPlayableCharacter Source, ITargetable Target)
        {
            var distanceFactor = action.MaximumRange > 1 ? (int)GamePoint.Distance(Source.Position, Target.Position) : 0;

            var mpUseFactor = Source.MP != null ? (double) (action.MPCost / Source.MaxMP) : 0;
            return (int)(GetEffectWeight(action.Effect, map, This, Source, Target) * (1 - mpUseFactor) - 5 * distanceFactor);
        }

        public int GetEffectWeight(Effect effect, Map map, Entity This, NonPlayableCharacter Source, ITargetable Target)
        {
            var weight = 0;

            var targetAsCharacter = Target is Character c ? c : null;
            var targetAsTile = Target is Tile t ? t : null;
            dynamic paramsObject = ExpressionParser.ParseParams(This, Source, Target, effect.Params);
            var accuracyFactor = effect.Params.Any(p => p.ParamName == "Accuracy")
                 ? Math.Min(1, ExpressionParser.CalculateAdjustedAccuracy(Source, targetAsCharacter, paramsObject) / 100f * 2)
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
                    if (damageDealt > targetAsCharacter.HP.Current)
                        weight = 1000;
                    else
                        weight = 5 + damageDealt * 5;

                    weight = (int)(weight * accuracyFactor);
                    break;

                case "BurnMP":
                    if (targetAsCharacter == null || targetAsCharacter.ExistenceStatus != EntityExistenceStatus.Alive || targetAsCharacter.MP == null)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    var burnAmount = paramsObject.Power;
                    if (paramsObject.Power > 0 && paramsObject.Power < 1)
                        burnAmount = 1;
                    weight = (int)((2 + burnAmount * 5) * accuracyFactor);
                    break;

                case "RemoveHunger":
                    if (targetAsCharacter == null || targetAsCharacter.ExistenceStatus != EntityExistenceStatus.Alive || targetAsCharacter.Hunger == null)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    var hungerRemoveAmount = paramsObject.Power;
                    if (paramsObject.Power > 0 && paramsObject.Power < 1)
                        hungerRemoveAmount = 1;
                    weight = (int)((1 + hungerRemoveAmount * 5) * accuracyFactor);
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
                        weight = (int)(1 * accuracyFactor);
                    }
                    break;

                case "HealDamage":
                    if (targetAsCharacter == null || targetAsCharacter.ExistenceStatus != EntityExistenceStatus.Alive || targetAsCharacter.HP.Current >= targetAsCharacter.MaxHP)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    var healAmount = Math.Min(targetAsCharacter.MaxHP - targetAsCharacter.HP.Current, paramsObject.Power);
                    if (paramsObject.Power > 0 && paramsObject.Power < 1)
                        healAmount = 1;
                    weight = (int)((3 + healAmount * 5) * accuracyFactor);
                    break;

                case "ReplenishMP":
                    if (targetAsCharacter == null || targetAsCharacter.ExistenceStatus != EntityExistenceStatus.Alive || targetAsCharacter.MP == null || targetAsCharacter.MP.Current >= targetAsCharacter.MaxMP)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    var replenishAmount = Math.Min(targetAsCharacter.MaxMP - targetAsCharacter.MP.Current, paramsObject.Power);
                    if (paramsObject.Power > 0 && paramsObject.Power < 1)
                        replenishAmount = 1;
                    weight = (int)((2 + replenishAmount * 5) * accuracyFactor);
                    break;

                case "ReplenishHunger":
                    if (targetAsCharacter == null || targetAsCharacter.ExistenceStatus != EntityExistenceStatus.Alive || targetAsCharacter.Hunger == null || targetAsCharacter.Hunger.Current >= targetAsCharacter.MaxHunger)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    replenishAmount = Math.Min(targetAsCharacter.MaxHunger - targetAsCharacter.Hunger.Current, paramsObject.Power);
                    if (paramsObject.Power > 0 && paramsObject.Power < 1)
                        replenishAmount = 1;
                    weight = (int)((1 + replenishAmount * 5) * accuracyFactor);
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
                    weight = (int)(4 + (powerWeight * 5 + turnLengthWeight) * accuracyFactor);
                    break;

                case "ApplyStatAlteration":
                    if (targetAsCharacter == null || targetAsCharacter.ExistenceStatus != EntityExistenceStatus.Alive)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }

                    var targetStat = targetAsCharacter.UsedStats.Find(s => s.Id.Equals(paramsObject.StatName, StringComparison.InvariantCultureIgnoreCase));
                    if (targetStat == null)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }

                    var isStatWithMax = targetStat.HasMax;
                    var isRegeneration = targetStat.StatType == StatType.Regeneration;

                    var statAlterations = paramsObject.StatAlterationList as List<StatModification>;
                    var alterationAmount = isRegeneration ? paramsObject.Amount : (int)paramsObject.Amount;

                    if (!isRegeneration)
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
                    if (isStatWithMax)
                        alterationWeight /= 10;
                    else if (isRegeneration)
                        alterationWeight *= 10;
                    weight = (int)(4 + ((int)alterationWeight * 5 + turnLengthWeight) * accuracyFactor);
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
                    weight = (int)(2 * accuracyFactor);
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
                    weight = (int)(2 * accuracyFactor);
                    break;

                case "CleanseAllAlteredStatuses":
                    if (targetAsCharacter == null || targetAsCharacter.ExistenceStatus != EntityExistenceStatus.Alive)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    weight = (int)(3 * targetAsCharacter.AlteredStatuses.Count * accuracyFactor);
                    break;

                case "CleanseStatAlterations":
                    if (targetAsCharacter == null || targetAsCharacter.ExistenceStatus != EntityExistenceStatus.Alive)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    statAlterations = paramsObject.StatAlterationList as List<StatModification>;
                    weight = (int)(3 * statAlterations.Count * accuracyFactor);
                    break;

                case "ForceSkipTurn":
                    if (targetAsCharacter == null || targetAsCharacter.ExistenceStatus != EntityExistenceStatus.Alive || !targetAsCharacter.CanTakeAction)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    weight = (int)(3 * accuracyFactor);
                    break;

                case "Teleport":
                case "ToggleVisibility":
                    if (targetAsCharacter == null || targetAsCharacter.ExistenceStatus != EntityExistenceStatus.Alive)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    weight = (int)(2 * accuracyFactor);
                    break;

                case "GiveItem":
                    if (targetAsCharacter == null || targetAsCharacter.ExistenceStatus != EntityExistenceStatus.Alive || targetAsCharacter.ItemCount >= targetAsCharacter.InventorySize)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    weight = (int)(2 * accuracyFactor);
                    break;

                case "PlaceTrap":
                    if (targetAsTile == null)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    var distanceToUse = Source.Position != targetAsTile.Position ? (int)GamePoint.Distance(Source.Position, targetAsTile.Position) : 0.5;
                    weight = (int)(2 * accuracyFactor / distanceToUse);
                    break;

                case "SpawnNPC":
                    if (targetAsTile == null || targetAsTile.LivingCharacter != null)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    distanceToUse = Source.Position != targetAsTile.Position ? (int)GamePoint.Distance(Source.Position, targetAsTile.Position) : 0.5;
                    weight = (int)(2 * accuracyFactor / distanceToUse);
                    break;
                case "ReviveNPC":
                    if (targetAsTile == null || targetAsTile.GetDeadCharacters().Any(c => c.Faction == Source.Faction || c.Faction.AlliedWith.Contains(Source.Faction)) != null)
                    {
                        weight = (weight == 0) ? -500 : weight - 100;
                        break;
                    }
                    distanceToUse = Source.Position != targetAsTile.Position ? (int)GamePoint.Distance(Source.Position, targetAsTile.Position) : 0.5;
                    weight = (int)(2 * accuracyFactor / distanceToUse);
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
