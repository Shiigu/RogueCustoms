using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Helpers;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using RogueCustomsGameEngine.Utils.Representation;
using System.Drawing;
using org.matheval;
using MathNet.Numerics.Statistics.Mcmc;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.Expressions;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using System.Numerics;
using RogueCustomsGameEngine.Utils.Effects.Utils;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace RogueCustomsGameEngine.Utils.Effects
{
#pragma warning disable S2259 // Null Pointers should not be dereferenced
#pragma warning disable S2589 // Boolean expressions should not be gratuitous
#pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
#pragma warning disable CS8604 // Posible argumento de referencia nulo
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.

    // Represents Actions that are not expected to be used by any type of Entity in particular. Free to use by everyone.
    public static class GenericActions
    {
        private static RngHandler Rng;
        private static Map Map;
        public static void SetActionParams(RngHandler rng, Map map)
        {
            Rng = rng;
            Map = map;
        }

        public static bool PrintText(EffectCallerParams Args)
        {
            dynamic paramsObject = ExpressionParser.ParseParams(Args);

            var ignoreVisibilityChecks = ExpandoObjectHelper.HasProperty(paramsObject, "BypassesVisibilityCheck") && paramsObject.BypassesVisibilityCheck;
            var entityTypesForVisibilityCheck = new List<EntityType> { EntityType.Player, EntityType.NPC };
            var isSourceVisible = entityTypesForVisibilityCheck.Contains(Args.Source.EntityType) && Map.Player.CanSee(Args.Source);
            var isTargetVisible = Args.Target is Character t && entityTypesForVisibilityCheck.Contains(t.EntityType) && Map.Player.CanSee(t);

            if (ignoreVisibilityChecks || isSourceVisible || isTargetVisible)
            {
                if (ExpandoObjectHelper.HasProperty(paramsObject, "Color"))
                    Map.AppendMessage(paramsObject.Text, paramsObject.Color);
                else
                    Map.AppendMessage(paramsObject.Text);
            }

            return true;
        }

        public static bool MessageBox(EffectCallerParams Args)
        {
            dynamic paramsObject = ExpressionParser.ParseParams(Args);
            Map.AddMessageBox(paramsObject.Title, paramsObject.Text, "OK", paramsObject.Color);
            return true;
        }

        public static bool HealDamage(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);
            if (paramsObject.Target is not Character t)
                // Attempted to heal Target when it's not a Character.
                return false;
            if (t.HP.Current >= t.MaxHP)
                return false;
            var healAmount = Math.Min(t.MaxHP - t.HP.Current, paramsObject.Power);
            if (paramsObject.Power > 0 && paramsObject.Power < 1)
                healAmount = 1;
            healAmount = (int)healAmount;
            t.HP.Current = Math.Min(t.MaxHP, t.HP.Current + healAmount);

            if (t == Map.Player || Map.Player.CanSee(t))
            {
                events.Add(new()
                {
                    DisplayEventType = DisplayEventType.PlaySpecialEffect,
                    Params = new() { SpecialEffect.HPUp }
                }
                );
                if (t == Map.Player)
                {
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.UpdatePlayerData,
                        Params = new() { UpdatePlayerDataType.ModifyStat, "HP", t.HP.Current }
                    });
                }
                if (t.HP.Current == t.MaxHP)
                {
                    Map.AppendMessage(Map.Locale["CharacterHealsAllHP"].Format(new { CharacterName = t.Name }), Color.DeepSkyBlue, events);
                }
                else
                {
                    Map.AppendMessage(Map.Locale["CharacterHealsSomeHP"].Format(new { CharacterName = t.Name, HealAmount = healAmount.ToString(), CharacterHPStat = Map.Locale["CharacterHPStat"] }), Color.DeepSkyBlue, events);
                }
            }
            Map.DisplayEvents.Add(($"{t.Name} recovered HP", events));
            return true;
        }

        public static async Task<bool> GiveExperience(EffectCallerParams Args)
        {
            dynamic paramsObject = ExpressionParser.ParseParams(Args);
            if (paramsObject.Target is not Character t || !t.CanGainExperience) return false;
            if (t.Level == t.MaxLevel) return false;
            if (t == Map.Player || Map.Player.CanSee(t))
                Map.AppendMessage(Map.Locale["CharacterGainsExperience"].Format(new { CharacterName = paramsObject.Target.Name, Amount = ((int)paramsObject.Amount).ToString() }), Color.DeepSkyBlue);
            await t.GainExperience((int)paramsObject.Amount);
            return true;
        }

        public static async Task<bool> ApplyAlteredStatus(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);
            if (paramsObject.Target is not Character)
                // Attempted to apply an Altered Status on Target when it's not a Character.
                return false;
            var statusToApply = Map.PossibleStatuses.Find(ps => string.Equals(ps.ClassId, paramsObject.Id, StringComparison.InvariantCultureIgnoreCase))
                                   ?? throw new ArgumentException($"Altered status {paramsObject.Id} does not exist!");
            var statusTarget = paramsObject.Target as Character;
            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Args.Source, paramsObject.Target, paramsObject);

            if (statusTarget.ExistenceStatus == EntityExistenceStatus.Alive && Rng.RollProbability() <= accuracyCheck)
            {
                var targetAlreadyHadStatus = statusTarget.AlteredStatuses.Exists(als => als.RemainingTurns != 0 && als.ClassId.Equals(paramsObject.Id));
                var statusPower = (decimal) paramsObject.Power;
                var turnlength = (int)paramsObject.TurnLength;
                var couldSeeTarget = Map.Player.CanSee(statusTarget);
                if(statusToApply.CanBeAppliedTo(statusTarget))
                {
                    if (statusTarget == Map.Player || couldSeeTarget)
                    {
                        if (!targetAlreadyHadStatus || statusToApply.CanStack || (statusToApply.CanOverwrite && paramsObject.AnnounceStatusRefresh))
                        {
                            events.Add(new()
                            {
                                DisplayEventType = DisplayEventType.PlaySpecialEffect,
                                Params = new() { SpecialEffect.Statused }
                            }
                            );
                            if (statusTarget == Map.Player)
                            {
                                events.Add(new()
                                {
                                    DisplayEventType = DisplayEventType.UpdatePlayerData,
                                    Params = new() { UpdatePlayerDataType.UpdateAlteredStatuses, statusTarget.AlteredStatuses.Select(als => new SimpleEntityDto(als)).ToList() }
                                });
                            }
                            var localeToUse = targetAlreadyHadStatus ? Map.Locale["CharacterStatusGotRefreshed"] : Map.Locale["CharacterGotStatused"];
                            Map.AppendMessage(localeToUse.Format(new { CharacterName = paramsObject.Target.Name, StatusName = statusToApply.Name }), Color.DeepSkyBlue, events);
                        }
                    }
                    Map.DisplayEvents.Add(($"{statusTarget.Name} got {statusToApply.Name} status", events));
                    return await statusToApply.ApplyTo(statusTarget, statusPower, turnlength);
                }
            }
            return false;
        }

        public static bool ApplyStatAlteration(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);
            if (paramsObject.Target is not Character)
                // Attempted to alter one of Target's stats when it's not a Character.
                return false;
            var statAlterationTarget = paramsObject.Target as Character;

            var statAlterations = paramsObject.StatAlterationList as List<StatModification>;
            if(statAlterations == null)
                // Attempted to alter one of Target's stats when it doesn't have it.
                return false;
            var statName = paramsObject.StatName.ToLowerInvariant();
            if (!statName.Equals("max") && statName.StartsWith("max"))
                statName = statName.TrimStart("max");
            var targetStat = statAlterationTarget.UsedStats.Find(s => s.Id.Equals(statName, StringComparison.InvariantCultureIgnoreCase));
            if(targetStat == null)
                // Attempted to alter one of Target's stats when it doesn't have it.
                return false;

            var statValue = targetStat.Current;
            if (statValue > targetStat.MaxCap || statValue < targetStat.MinCap)
                return false;

            var canBeStacked = paramsObject.CanBeStacked;
            var canBeOverwritten = paramsObject.CanBeOverwritten;
            var statAlterationAlreadyExists = statAlterations.Exists(sa => sa.RemainingTurns > 0 && sa.Id.Equals(paramsObject.Id));

            if (!canBeStacked && !canBeOverwritten && statAlterationAlreadyExists)
                return false;

            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Args.Source, paramsObject.Target, paramsObject);

            if (statAlterationTarget.ExistenceStatus == EntityExistenceStatus.Alive && paramsObject.Amount != 0 && Rng.RollProbability() <= accuracyCheck)
            {
                var turnLength = (int)paramsObject.TurnLength;
                var isDecimal = targetStat.StatType == StatType.Decimal || targetStat.StatType == StatType.Regeneration;
                var isPercentage = targetStat.StatType == StatType.Percentage;
                var alterationAmount = isDecimal ? paramsObject.Amount : (int)paramsObject.Amount;

                if (!canBeStacked && canBeOverwritten && statAlterationAlreadyExists)
                {
                    foreach (var statAlteration in statAlterations.Where(sa => sa.RemainingTurns > 0 && sa.Id.Equals(paramsObject.Id) && sa.Amount == alterationAmount))
                    {
                        statAlteration.RemainingTurns = turnLength;
                    }
                    return true;
                }


                if(!isDecimal)
                {
                    if (paramsObject.Amount > 0 && paramsObject.Amount < 1)
                        alterationAmount = 1;
                    else if (paramsObject.Amount < 0 && paramsObject.Amount > -1)
                        alterationAmount = -1;
                }

                if (statValue + alterationAmount > targetStat.MaxCap)
                    alterationAmount = 0;
                if (statValue + alterationAmount < targetStat.MinCap)
                    alterationAmount = 0;

                if (alterationAmount == 0)
                    // Attempted to alter one of Target's stats when it's in its cap.
                    return false;

                statAlterations.Add(new StatModification
                {
                    Id = paramsObject.Id,
                    Amount = alterationAmount,
                    RemainingTurns = turnLength
                });

                if (paramsObject.DisplayOnLog && (statAlterationTarget.EntityType == EntityType.Player
                    || (statAlterationTarget.EntityType == EntityType.NPC && Map.Player.CanSee(paramsObject.Target))))
                {
                    var amountString = targetStat.StatType == StatType.Percentage ? $"{Math.Abs(alterationAmount)}%" : Math.Abs(alterationAmount).ToString("0.#####");
                    var messageKey = alterationAmount > 0 ? "CharacterStatGotBuffed" : "CharacterStatGotNerfed";
                    var specialEffect = alterationAmount > 0 ? SpecialEffect.StatBuff : SpecialEffect.StatNerf;

                    if(statAlterationTarget == Map.Player && Stat.StatsInUI.Contains(statName))
                    {
                        if (statName.Equals("hp") || statName.Equals("mp") || statName.Equals("hunger"))
                        {
                            events.Add(new()
                            {
                                DisplayEventType = DisplayEventType.UpdatePlayerData,
                                Params = new() { UpdatePlayerDataType.ModifyMaxStat, targetStat.Id, targetStat.BaseAfterModifications }
                            });
                        }
                        else
                        {
                            events.Add(new()
                            {
                                DisplayEventType = DisplayEventType.UpdatePlayerData,
                                Params = new() { UpdatePlayerDataType.ModifyStat, targetStat.Id, targetStat.BaseAfterModifications }
                            });
                        }
                        if (statAlterationTarget == Map.Player && statName.Equals("movement"))
                        {
                            events.Add(new()
                            {
                                DisplayEventType = DisplayEventType.SetCanMove,
                                Params = new() { targetStat.BaseAfterModifications > 0 }
                            });
                        }
                    }

                    if (statAlterationTarget == Map.Player || Map.Player.CanSee(statAlterationTarget))
                    {
                        var message = Map.Locale[messageKey].Format(new
                        {
                            CharacterName = statAlterationTarget.Name,
                            StatName = Map.Locale[targetStat.Name],
                            Amount = amountString
                        });
                        events.Add(new()
                            {
                                DisplayEventType = DisplayEventType.PlaySpecialEffect,
                                Params = new() { specialEffect }
                            }
                        );
                        Map.AppendMessage(message, Color.DeepSkyBlue, events);
                    }

                }
                Map.DisplayEvents.Add(($"{statAlterationTarget.Name} got {Map.Locale[targetStat.Name]} changed", events));
                return true;
            }
            return false;
        }

        public static async Task<bool> CleanseAlteredStatus(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);
            if (paramsObject.Target is not Character c)
                // Attempted to remove an Altered Status from Target when it's not a Character.
                return false;
            var statusToRemove = Map.PossibleStatuses.Find(ps => string.Equals(ps.ClassId, paramsObject.Id, StringComparison.InvariantCultureIgnoreCase));
            if (!statusToRemove.CleansedByCleanseActions) throw new InvalidOperationException($"Attempted to remove {statusToRemove.Name} with a Cleanse action when it can't be cleansed that way.");
            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Args.Source, paramsObject.Target, paramsObject);

            if (c.AlteredStatuses.Exists(als => als.ClassId.Equals(statusToRemove.ClassId)) && Rng.RollProbability() <= accuracyCheck)
            {
                foreach (var als in c.AlteredStatuses.Where(als => als.ClassId.Equals(statusToRemove.ClassId)))
                {
                    if(als.OnRemove != null)
                        await als.OnRemove.Do(als, c, false);
                    als.FlaggedToRemove = true;
                    als.RemainingTurns = 0;
                }
                foreach (var (statName, modifications) in c.StatModifications)
                {
                    modifications.RemoveAll(a => a.Id.Equals(statusToRemove.Name));
                }
                if (c == Map.Player || Map.Player.CanSee(c))
                {
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.PlaySpecialEffect,
                        Params = new() { SpecialEffect.StatusLeaves }
                    }
                    );
                    if (c == Map.Player)
                    {
                        events.Add(new()
                        {
                            DisplayEventType = DisplayEventType.UpdatePlayerData,
                            Params = new() { UpdatePlayerDataType.UpdateAlteredStatuses, c.AlteredStatuses.Where(als => !als.ClassId.Equals(statusToRemove.ClassId)).Select(als => new SimpleEntityDto(als)).ToList() }
                        });
                    }
                    Map.AppendMessage(Map.Locale["CharacterIsNoLongerStatused"].Format(new { CharacterName = c.Name, StatusName = statusToRemove.Name }), Color.DeepSkyBlue, events);
                }
                Map.DisplayEvents.Add(($"{c.Name} lost {statusToRemove.Name} status", events));
                return true;
            }
            return false;
        }

        public static bool CleanseStatAlteration(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);
            if (paramsObject.Target is not Character t)
                // Attempted to remove of Target's Altered Statuses when it's not a Character.
                return false;
            var statAlterations = paramsObject.StatAlterationList as List<StatModification>;
            if (statAlterations == null)
                // Attempted to alter one of Target's stats when it doesn't have it.
                return false;
            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Args.Source, t, paramsObject);

            if (statAlterations?.Any() == true && Rng.RollProbability() <= accuracyCheck)
            {
                var amount = statAlterations.Sum(sa => sa.Amount);
                var statName = paramsObject.StatName.ToLowerInvariant();
                statAlterations.Clear();

                if (t.EntityType == EntityType.Player
                    || (t.EntityType == EntityType.NPC && Map.Player.CanSee(t)))
                {
                    var stat = t.UsedStats.Find(s => s.Name.ToLowerInvariant().Equals(statName));

                    var specialEffect = amount < 0 ? SpecialEffect.StatBuff : SpecialEffect.StatNerf;
                    if (t == Map.Player || Map.Player.CanSee(t))
                    {
                        events.Add(new()
                        {
                            DisplayEventType = DisplayEventType.PlaySpecialEffect,
                            Params = new() { specialEffect }
                        }
                        );
                        Map.AppendMessage(Map.Locale["CharacterStatGotNeutralized"].Format(new { CharacterName = t.Name, StatName = Map.Locale[statName] }), Color.DeepSkyBlue, events);
                    }

                    if (t == Map.Player && Stat.StatsInUI.Contains(statName))
                    {
                        if (statName.Equals("hp") || statName.Equals("mp") || statName.Equals("hunger"))
                        {
                            events.Add(new()
                            {
                                DisplayEventType = DisplayEventType.UpdatePlayerData,
                                Params = new() { UpdatePlayerDataType.ModifyMaxStat, stat.Id, stat.BaseAfterModifications }
                            });
                        }
                        else
                        {
                            events.Add(new()
                            {
                                DisplayEventType = DisplayEventType.UpdatePlayerData,
                                Params = new() { UpdatePlayerDataType.ModifyStat, stat.Id, stat.BaseAfterModifications }
                            });
                        }
                    }
                }
                Map.DisplayEvents.Add(($"{t.Name} lost {Map.Locale[statName]} stat modifications", events));
                return true;
            }

            return false;
        }

        public static bool CleanseStatAlterations(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);
            if (paramsObject.Target is not Character c)
                // Attempted to alter one of Target's stats when it's not a Character.
                return false;
            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Args.Source, paramsObject.Target, paramsObject);
            var success = false;
            if (Rng.RollProbability() <= accuracyCheck)
            {
                foreach (var (statName, modifications) in c.StatModifications)
                {
                    if (modifications.Any())
                    {
                        success = true;
                        modifications.Clear();

                        var stat = c.UsedStats.Find(c => c.Name.Equals(statName));
                        var loweredStatName = statName.ToLowerInvariant();

                        if (c == Map.Player && Stat.StatsInUI.Contains(loweredStatName))
                        {
                            if (loweredStatName.Equals("hp") || loweredStatName.Equals("mp") || loweredStatName.Equals("hunger"))
                            {
                                events.Add(new()
                                {
                                    DisplayEventType = DisplayEventType.UpdatePlayerData,
                                    Params = new() { UpdatePlayerDataType.ModifyMaxStat, stat.Id, stat.BaseAfterModifications }
                                });
                            }
                            else
                            {
                                events.Add(new()
                                {
                                    DisplayEventType = DisplayEventType.UpdatePlayerData,
                                    Params = new() { UpdatePlayerDataType.ModifyStat, stat.Id, stat.BaseAfterModifications }
                                });
                            }
                        }
                        if (c == Map.Player || Map.Player.CanSee(c))
                        {
                            events.Add(new()
                            {
                                DisplayEventType = DisplayEventType.PlaySpecialEffect,
                                Params = new() { SpecialEffect.StatBuff }
                            }
                            );
                            Map.AppendMessage(Map.Locale["CharacterStatGotNeutralized"].Format(new { CharacterName = c.Name, StatName = statName }), Color.DeepSkyBlue, events);
                        }
                    }
                }
            }

            Map.DisplayEvents.Add(($"{c.Name} lost all stat modifications", events));
            return success;
        }

        public static async Task<bool> CleanseAllAlteredStatuses(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);
            if (paramsObject.Target is not Character c)
                // Attempted to remove all of Target's Altered Statuses when it's not a Character.
                return false;
            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Args.Source, paramsObject.Target, paramsObject);
            if (Rng.RollProbability() <= accuracyCheck && c.AlteredStatuses?.Any() == true)
            {
                foreach (var als in c.AlteredStatuses)
                {
                    if (als.OnRemove != null)
                        await als.OnRemove.Do(als, c, false);
                    als.FlaggedToRemove = true;
                    als.RemainingTurns = 0;
                    if (c == Map.Player || Map.Player.CanSee(c))
                    {
                        Map.AppendMessage(Map.Locale["CharacterIsNoLongerStatused"].Format(new { CharacterName = c.Name, StatusName = als.Name }), Color.DeepSkyBlue, events);
                        if (c == Map.Player)
                        {
                            events.Add(new()
                            {
                                DisplayEventType = DisplayEventType.UpdatePlayerData,
                                Params = new() { UpdatePlayerDataType.UpdateAlteredStatuses, new List<SimpleEntityDto>() }
                            });
                        }
                    }

                    foreach (var (statName, modifications) in c.StatModifications)
                    {
                        modifications.RemoveAll(a => a.Id.Equals(als.Id));
                    }
                }

                if (c == Map.Player || Map.Player.CanSee(c))
                {
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.PlaySpecialEffect,
                        Params = new() { SpecialEffect.StatusLeaves }
                    }
                    );
                }

                Map.DisplayEvents.Add(($"{c.Name} lost all altered statuses", events));
                return true;
            }
            return false;
        }
        public static bool EndTurn(EffectCallerParams Args)
        {
            dynamic paramsObject = ExpressionParser.ParseParams(Args);
            if (paramsObject.Target is not Character c)
                // Attempted to end Target's turn when it's not a Character.
                return false;
            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Args.Source, paramsObject.Target, paramsObject);
            if (Rng.RollProbability() <= accuracyCheck && c.CanTakeAction)
            {
                c.CanTakeAction = false;
                c.TookAction = true;
                return true;
            }
            return false;
        }

        public static bool ForceSkipTurn(EffectCallerParams Args)
        {
            dynamic paramsObject = ExpressionParser.ParseParams(Args);
            if (paramsObject.Target is not Character c)
                // Attempted to skip Target's next turn when it's not a Character.
                return false;
            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Args.Source, paramsObject.Target, paramsObject);
            if (Rng.RollProbability() <= accuracyCheck && c.CanTakeAction)
            {
                c.CanTakeAction = false;
                if (c == Map.Player)
                {
                    Map.DisplayEvents.Add(($"{c.Name} is forced to skip turn", new()
                    {
                        new()
                        {
                            DisplayEventType = DisplayEventType.SetCanAct,
                            Params = new() { false }
                        }
                    }));
                }
                return true;
            }
            return false;
        }

        public static bool Teleport(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);
            if (paramsObject.Target is not Character c)
                // Attempted to teleport Target when it's not a Character.
                return false;
            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Args.Source, paramsObject.Target, paramsObject);
            if (c.ContainingTile.Type.IsWalkable && Rng.RollProbability() <= accuracyCheck)
            {
                var olderFOV = c == Map.Player ? c.FOVTiles : null;
                var initialTile = c.ContainingTile;
                var targetPosition = Map.PickEmptyPosition(true, false);
                c.Position = targetPosition;
                var targetTile = c.ContainingTile;

                if (!Map.IsDebugMode)
                {
                    if (c == Map.Player || Map.Player.FOVTiles.Any(t => t == initialTile))
                    {
                        events.Add(new()
                        {
                            DisplayEventType = DisplayEventType.UpdateTileRepresentation,
                            Params = new() { initialTile.Position, Map.GetConsoleRepresentationForCoordinates(initialTile.Position.X, initialTile.Position.Y) }
                        }
                        );
                    }

                    if (c == Map.Player || Map.Player.FOVTiles.Any(t => t == targetTile))
                    {
                        events.Add(new()
                        {
                            DisplayEventType = DisplayEventType.UpdateTileRepresentation,
                            Params = new() { targetPosition, Map.GetConsoleRepresentationForCoordinates(targetPosition.X, targetPosition.Y) }
                        }
                        );
                    }
                }


                if (c == Map.Player)
                {
                    Map.Player.UpdateVisibility();
                    if (olderFOV != null)
                    {
                        foreach (var tile in olderFOV.Where(t => !t.Visible))
                        {
                            events.Add(new()
                            {
                                DisplayEventType = DisplayEventType.UpdateTileRepresentation,
                                Params = new() { tile.Position, Map.GetConsoleRepresentationForCoordinates(tile.Position.X, tile.Position.Y) }
                            }
                            );
                        }
                    }
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.UpdatePlayerPosition,
                        Params = new() { targetPosition }
                    }
                    );

                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.SetOnStairs,
                        Params = new() { c.ContainingTile.Type == TileType.Stairs }
                    }
                    );
                }
                if (c == Map.Player || Map.Player.CanSee(c))
                {
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.PlaySpecialEffect,
                        Params = new() { SpecialEffect.Teleport }
                    }
                    ); 
                    Map.AppendMessage(Map.Locale["CharacterWasTeleported"].Format(new { CharacterName = c.Name }), Color.DeepSkyBlue, events);
                }
                Map.DisplayEvents.Add(($"{c.Name} teleported", events));
                c.ContainingTile?.StoodOn(c);
                return true;
            }
            return false;
        }

        public static bool GenerateStairs(EffectCallerParams Args)
        {
            if (!Map.StairsAreSet)
            {
                var events = new List<DisplayEventDto>();
                Map.SetStairs();
                if (!Map.IsDebugMode)
                {
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.UpdateTileRepresentation,
                        Params = new() { Map.StairsTile.Position, Map.GetConsoleRepresentationForCoordinates(Map.StairsTile.Position.X, Map.StairsTile.Position.Y) }
                    }
                    );
                }
                events.Add(new()
                {
                    DisplayEventType = DisplayEventType.PlaySpecialEffect,
                    Params = new() { SpecialEffect.StairsReveal }
                }
                );
                Map.AppendMessage(Map.Locale["StairsGotRevealed"], Color.Lime, events);
                Map.DisplayEvents.Add(($"Stairs spawned", events));
                return true;
            }
            return false;
        }

        public static bool CheckCondition(EffectCallerParams Args)
        {
            dynamic paramsObject = ExpressionParser.ParseParams(Args);
            return new Expression(paramsObject.Condition).Eval<bool>();
        }

        public static bool SetFlag(EffectCallerParams Args)
        {
            dynamic paramsObject = ExpressionParser.ParseParams(Args);
            var existingFlag = Map.Flags.Find(f => f.Key.Equals(paramsObject.Key));
            if(existingFlag != null)
                Map.SetFlagValue(paramsObject.Key, paramsObject.Value);
            else
                Map.CreateFlag(paramsObject.Key, paramsObject.Value, paramsObject.RemoveOnFloorChange);
            return true;
        }

        public static bool ReplenishMP(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);
            if (paramsObject.Target is not Character t)
                // Attempted to recover Target's MP when it's not a Character.
                return false;
            if (t.MP == null) return false;
            if (t.MP.Current >= t.MP.BaseAfterModifications)
                return false;
            var replenishAmount = Math.Min(t.MP.BaseAfterModifications - t.MP.Current, paramsObject.Power);
            if (paramsObject.Power > 0 && paramsObject.Power < 1)
                replenishAmount = 1;
            replenishAmount = (int)replenishAmount;
            t.MP.Current = Math.Min(t.MP.BaseAfterModifications, t.MP.Current + replenishAmount);

            if (t == Map.Player || Map.Player.CanSee(t))
            {
                if (t == Map.Player)
                {
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.UpdatePlayerData,
                        Params = new() { UpdatePlayerDataType.ModifyStat, "MP", t.MP.Current }
                    });
                }
                events.Add(new()
                {
                    DisplayEventType = DisplayEventType.PlaySpecialEffect,
                    Params = new() { SpecialEffect.MPUp }
                }
                );
                if (t.MP.Current == t.MaxMP)
                {
                    Map.AppendMessage(Map.Locale["CharacterRecoversAllMP"].Format(new { CharacterName = paramsObject.Target.Name, CharacterMPStat = Map.Locale["CharacterMPStat"] }), Color.DeepSkyBlue, events);
                }
                else
                {
                    Map.AppendMessage(Map.Locale["CharacterRecoversSomeMP"].Format(new { CharacterName = paramsObject.Target.Name, ReplenishAmount = replenishAmount.ToString(), CharacterMPStat = Map.Locale["CharacterMPStat"] }), Color.DeepSkyBlue, events);
                }
            }
            Map.DisplayEvents.Add(($"{t.Name} recovered MP", events));
            return true;
        }
        public static bool ReplenishHunger(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);
            if (paramsObject.Target is not Character t)
                // Attempted to recover Target's Hunger when it's not a Character.
                return false;
            if (t.Hunger == null) return false;
            if (t.Hunger.Current >= t.Hunger.Base)
                return false;
            var replenishAmount = Math.Min(t.Hunger.Base - t.Hunger.Current, paramsObject.Power);
            if (paramsObject.Power > 0 && paramsObject.Power < 1)
                replenishAmount = 1;
            replenishAmount = (int)replenishAmount;
            t.Hunger.Current = Math.Min(t.Hunger.Base, t.Hunger.Current + replenishAmount);

            if (t == Map.Player || Map.Player.CanSee(t))
            {
                if (t == Map.Player)
                {
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.UpdatePlayerData,
                        Params = new() { UpdatePlayerDataType.ModifyStat, "Hunger", t.Hunger.Current }
                    });
                }
                events.Add(new()
                {
                    DisplayEventType = DisplayEventType.PlaySpecialEffect,
                    Params = new() { SpecialEffect.HungerUp }
                }
                );
                if (t.Hunger.Current == t.MaxHunger)
                {
                    Map.AppendMessage(Map.Locale["CharacterRecoversAllHunger"].Format(new { CharacterName = t.Name, CharacterHungerStat = Map.Locale["CharacterHungerStat"] }), Color.DeepSkyBlue, events);
                }
                else
                {
                    Map.AppendMessage(Map.Locale["CharacterRecoversSomeHunger"].Format(new { CharacterName = t.Name, ReplenishAmount = replenishAmount.ToString(), CharacterHungerStat = Map.Locale["CharacterHungerStat"] }), Color.DeepSkyBlue, events);
                }
            }
            Map.DisplayEvents.Add(($"{t.Name} recovered Hunger", events));
            return true;
        }

        public static bool ToggleVisibility(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);
            if (paramsObject.Target is not Character c)
                // Attempted to toggle Target's Visibility when it's not a Character.
                return false;
            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Args.Source, paramsObject.Target, paramsObject);
            if (Rng.RollProbability() <= accuracyCheck)
            {
                c.Visible = !c.Visible;
                if (!Map.IsDebugMode)
                {
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.UpdateTileRepresentation,
                        Params = new() { c.Position, Map.GetConsoleRepresentationForCoordinates(c.Position.X, c.Position.Y) }
                    }
                    );
                }
                Map.DisplayEvents.Add(($"{c.Name} changed visibility", events));
                return true;
            }
            return false;
        }

        public static async Task<bool> GiveItem(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);

            if (paramsObject.Target is not Character t)
                // Attempted to give Target an Item when it's not a Character.
                return false;

            t = paramsObject.Target as Character;

            if (t.ItemCount == t.InventorySize)
                // Attempted to give Target an Item when their inventory is full.
                return false;

            if (paramsObject.FromInventory && paramsObject.Source is not Character s)
                // Attempted to give Target an Item from Source's inventory, when Source's not a Character.
                return false;

            s = paramsObject.Source as Character;

            var itemClass = Map.PossibleItemClasses.Find(c => c.Id.Equals(paramsObject.Id));

            if (itemClass == null)
                // Must have a valid Trap class to spawn
                return false;

            if (paramsObject.FromInventory && !s.Inventory.Exists(i => i.ClassId.Equals(itemClass.Id)))
                // Attempted to give Target an Item from Source's inventory, when Source does not have such an item.
                return false;

            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Args.Source, t, paramsObject);
            if (Rng.RollProbability() <= accuracyCheck)
            {
                var itemToGive = paramsObject.FromInventory
                    ? s.Inventory.Find(i => i.ClassId.Equals(itemClass.Id))
                    : await Map.AddEntity(paramsObject.Id, 1, new GamePoint(0, 0)) as Item;
                if (paramsObject.FromInventory)
                    s.Inventory.Remove(itemToGive);
                t.Inventory.Add(itemToGive);
                itemToGive.Owner = t;
                itemToGive.Position = null;
                if(t == Map.Player || Map.Player.CanSee(t))
                {
                    if (t == Map.Player)
                    {
                        events.Add(new()
                        {
                            DisplayEventType = DisplayEventType.UpdatePlayerData,
                            Params = new() { UpdatePlayerDataType.UpdateInventory, t.Inventory.Cast<Entity>().Union(t.KeySet.Cast<Entity>()).Select(i => new SimpleEntityDto(i)).ToList() }
                        });
                    }
                    events.Add(new()
                    {
                        DisplayEventType = DisplayEventType.PlaySpecialEffect,
                        Params = new() { SpecialEffect.ItemGet }
                    });
                    Map.AppendMessage(Map.Locale["CharacterGotAnItem"].Format(new { CharacterName = t.Name, SourceName = s.Name, ItemName = itemToGive.Name }), Color.DeepSkyBlue, events);

                }
                Map.DisplayEvents.Add(($"{t.Name} received an item", events));
                return true;
            }
            return false;
        }

        public static async Task<bool> CallScript(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);

            var script = Map.Scripts.Find(s => s.Id.Equals(paramsObject.ScriptId, StringComparison.InvariantCultureIgnoreCase))
                ?? throw new ArgumentException($"Attempted to call {paramsObject.ScriptId} when it's not a Script.");
            
            if(script == null)
                throw new ArgumentException($"Attempted to call {paramsObject.ScriptId} when it's not a valid Script.");

            var clonedScript = script.Clone();
            clonedScript.User = Args.This;
            await clonedScript.Do(Args.Source, Args.Target, false, false);

            return true;
        }

        public static bool SwitchTargetCharacter(EffectCallerParams Args)
        {
            dynamic paramsObject = ExpressionParser.ParseParams(Args);

            if (Args.Source is not Character s)
                throw new ArgumentException($"Attempted to change {Args.Source.Name}'s current Target when it's not a Character.");

            string searchArea = paramsObject.SearchArea;

            var candidateTilesForCharacters = Map.GetCandidateTilesForArea(Args.OriginalTarget.Position, searchArea);
            var candidateCharacters = new List<Character>();

            var canRepeatPick = paramsObject.CanRepeatPick;
            candidateCharacters = Map.GetCharacters().Where(c => candidateTilesForCharacters.Contains(c.ContainingTile) && c.ExistenceStatus == EntityExistenceStatus.Alive).ToList();

            var selectionCondition = paramsObject.SelectionCondition;
            var charactersToConsider = new List<Character>();

            var evalParams = new EffectCallerParams
            {
                This = Args.This,
                Source = Args.Source,
                OriginalTarget = Args.OriginalTarget
            };

            foreach (var c in candidateCharacters)
            {
                if (!paramsObject.CanRepeatPick && c.PickedForSwap)
                    continue;
                if (!paramsObject.CanPickSelf && c == s)
                    continue;
                if (!paramsObject.CanPickAllies && (c.Faction.IsAlliedWith(s.Faction) || c.Faction == s.Faction))
                    continue;
                if (!paramsObject.CanPickNeutrals && c.Faction.IsNeutralWith(s.Faction))
                    continue;
                if (!paramsObject.CanPickEnemies && c.Faction.IsEnemyWith(s.Faction))
                    continue;
                if (!paramsObject.CanPickInvisibles && !c.CanBeSeenBy(s))
                    continue;

                evalParams.Target = c;
                var condition = ExpressionParser.ParseArgForExpression(selectionCondition, evalParams);

                if (!ExpressionParser.CalculateBooleanExpression(condition))
                    continue;

                charactersToConsider.Add(c);
            }

            if (charactersToConsider.Count == 0)
                return false;

            var pickedCharacter = charactersToConsider.TakeRandomElement(Rng);
            Args.Target = pickedCharacter;
            pickedCharacter.PickedForSwap = true;

            return true;
        }

        public static bool SwitchTargetTile(EffectCallerParams Args)
        {
            dynamic paramsObject = ExpressionParser.ParseParams(Args);

            if (Args.Source is not Character c)
                throw new ArgumentException($"Attempted to change {Args.Source.Name}'s current Target when it's not a Character.");

            string searchArea = paramsObject.SearchArea;

            var candidateTiles = Map.GetCandidateTilesForArea(Args.OriginalTarget.Position, searchArea);

            var tilesToConsider = new List<Tile>();
            var canRepeatPick = paramsObject.CanRepeatPick;
            var canPickWalls = paramsObject.CanPickWalls;
            var canPickHallways = paramsObject.CanPickHallways;
            var canPickInvisibles = paramsObject.CanPickInvisibles;
            var selectionCondition = paramsObject.SelectionCondition;

            var evalParams = new EffectCallerParams
            {
                This = Args.This,
                Source = Args.Source,
                OriginalTarget = Args.OriginalTarget
            };

            foreach (var t in candidateTiles)
            {
                if (t.Type == TileType.Stairs || t.Type == TileType.Empty)
                    continue;
                if (!canRepeatPick && t.PickedForSwap)
                    continue;
                if (!canPickWalls && t.Type == TileType.Wall)
                    continue;
                if (!canPickHallways && t.Type == TileType.Hallway)
                    continue;
                if (!canPickInvisibles && c.ContainingTile != t && !c.FOVTiles.Any(t2 => t2 == t))
                    continue;

                evalParams.Target = t;
                var condition = ExpressionParser.ParseArgForExpression(selectionCondition, evalParams);

                if (!ExpressionParser.CalculateBooleanExpression(condition))
                    continue;

                tilesToConsider.Add(t);
            }

            if (tilesToConsider.Count == 0)
                return false;

            var pickedTile = tilesToConsider.TakeRandomElement(Rng);
            Args.Target = pickedTile;
            pickedTile.PickedForSwap = true;

            return true;
        }

        public static bool ResetTarget(EffectCallerParams Args)
        {
            if (Args.Source is not Character)
                throw new ArgumentException($"Attempted to change {Args.Source.Name}'s current Target when it's not a Character.");

            Args.Target = Args.OriginalTarget;

            return true;
        }
    }
    #pragma warning restore S2259 // Null Pointers should not be dereferenced
    #pragma warning restore S2589 // Boolean expressions should not be gratuitous
    #pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
    #pragma warning restore CS8604 // Posible argumento de referencia nulo
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
