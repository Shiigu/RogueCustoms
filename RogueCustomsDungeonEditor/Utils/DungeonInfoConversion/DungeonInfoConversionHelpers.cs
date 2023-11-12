using RogueCustomsDungeonEditor.EffectInfos;
using RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V11;
using RogueCustomsGameEngine.Utils;
using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion
{
    #pragma warning disable S2589 // Boolean expressions should not be gratuitous
    #pragma warning disable S6605 // Collection-specific "Exists" method should be used instead of the "Any" extension
    #pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
    #pragma warning disable CS8604 // Posible argumento de referencia nulo
    public static class DungeonInfoConversionHelpers
    {
        public static DungeonInfo ConvertDungeonInfoIfNeeded(this DungeonInfo dungeon, string dungeonJson, LocaleInfo localeTemplate, List<string> mandatoryLocaleKeys)
        {
            var convertedLocales = false;
            var V10to11Dungeon = JsonSerializer.Deserialize<DungeonInfoV11>(dungeonJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            while (!dungeon.Version.Equals(Constants.CurrentDungeonJsonVersion))
            {
                switch (dungeon.Version)
                {
                    case "1.1":
                        dungeon = V10to11Dungeon.ConvertDungeonInfoToV12();
                        break;
                    default:
                        V10to11Dungeon = V10to11Dungeon.ConvertDungeonInfoToV11();
                        break;
                }
                if (!convertedLocales)
                {
                    foreach (var localeInfo in dungeon.Locales)
                    {
                        localeInfo.AddMissingMandatoryLocalesIfNeeded(localeTemplate, mandatoryLocaleKeys);
                    }
                    convertedLocales = true;
                }
            }

            return dungeon;
        }

        #region 1.0 to 1.1
        private static DungeonInfoV11 ConvertDungeonInfoToV11(this DungeonInfoV11 dungeon)
        {
            dungeon.TileSetInfos = new()
            {
                CreateDefaultTileSetV11(),
                CreateRetroTileSetV11()
            };
            foreach (var floorGroup in dungeon.FloorInfos)
            {
                floorGroup.TileSetId = "Default";
                foreach (var action in floorGroup.OnFloorStartActions)
                {
                    action.UpdateReplaceConsoleRepresentationStepsToV11();
                }
            }
            foreach (var playerClass in dungeon.PlayerClasses)
            {
                foreach(var action in playerClass.OnTurnStartActions)
                {
                    action.UpdateReplaceConsoleRepresentationStepsToV11();
                }
                foreach (var action in playerClass.OnAttackActions)
                {
                    action.UpdateReplaceConsoleRepresentationStepsToV11();
                }
                foreach (var action in playerClass.OnAttackedActions)
                {
                    action.UpdateReplaceConsoleRepresentationStepsToV11();
                }
                foreach (var action in playerClass.OnDeathActions)
                {
                    action.UpdateReplaceConsoleRepresentationStepsToV11();
                }
            }
            foreach (var npc in dungeon.NPCs)
            {
                foreach (var action in npc.OnTurnStartActions)
                {
                    action.UpdateReplaceConsoleRepresentationStepsToV11();
                }
                foreach (var action in npc.OnAttackActions)
                {
                    action.UpdateReplaceConsoleRepresentationStepsToV11();
                }
                foreach (var action in npc.OnAttackedActions)
                {
                    action.UpdateReplaceConsoleRepresentationStepsToV11();
                }
                foreach (var action in npc.OnDeathActions)
                {
                    action.UpdateReplaceConsoleRepresentationStepsToV11();
                }
            }
            foreach (var item in dungeon.Items)
            {
                foreach (var action in item.OnTurnStartActions)
                {
                    action.UpdateReplaceConsoleRepresentationStepsToV11();
                }
                foreach (var action in item.OnAttackActions)
                {
                    action.UpdateReplaceConsoleRepresentationStepsToV11();
                }
                foreach (var action in item.OnAttackedActions)
                {
                    action.UpdateReplaceConsoleRepresentationStepsToV11();
                }
                foreach (var action in item.OnItemUseActions)
                {
                    action.UpdateReplaceConsoleRepresentationStepsToV11();
                }
                foreach (var action in item.OnItemSteppedActions)
                {
                    action.UpdateReplaceConsoleRepresentationStepsToV11();
                }
            }
            foreach (var trap in dungeon.Traps)
            {
                foreach (var action in trap.OnItemSteppedActions)
                {
                    action.UpdateReplaceConsoleRepresentationStepsToV11();
                }
            }
            foreach (var alteredStatus in dungeon.AlteredStatuses)
            {
                foreach (var action in alteredStatus.OnTurnStartActions)
                {
                    action.UpdateReplaceConsoleRepresentationStepsToV11();
                }
                foreach (var action in alteredStatus.OnStatusApplyActions)
                {
                    action.UpdateReplaceConsoleRepresentationStepsToV11();
                }
            }
            dungeon.Version = "1.1";
            return dungeon;
        }

        private static TileSetInfoV11 CreateDefaultTileSetV11()
        {
            return new TileSetInfoV11
            {
                Id = "Default",
                TopLeftWall = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '█'
                },
                TopRightWall = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '█'
                },
                BottomLeftWall = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '█'
                },
                BottomRightWall = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '█'
                },
                HorizontalWall = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '█'
                },
                VerticalWall = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '█'
                },
                ConnectorWall = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '▒'
                },
                TopLeftHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '▒'
                },
                TopRightHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '▒'
                },
                BottomLeftHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '▒'
                },
                BottomRightHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '▒'
                },
                HorizontalHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '▒'
                },
                VerticalHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '▒'
                },
                HorizontalTopHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '▒'
                },
                HorizontalBottomHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '▒'
                },
                VerticalRightHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '▒'
                },
                VerticalLeftHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '▒'
                },
                CentralHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Blue),
                    Character = '▒'
                },
                Floor = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.DarkGray),
                    Character = '.'
                },
                Stairs = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Yellow),
                    ForegroundColor = new GameColor(Color.DarkGreen),
                    Character = '>'
                },
                Empty = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Black),
                    Character = ' '
                }
            };
        }

        private static TileSetInfoV11 CreateRetroTileSetV11()
        {
            return new TileSetInfoV11
            {
                Id = "Retro",
                TopLeftWall = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 84, 0)),
                    Character = '╔'
                },
                TopRightWall = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 84, 0)),
                    Character = '╗'
                },
                BottomLeftWall = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 84, 0)),
                    Character = '╚'
                },
                BottomRightWall = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 84, 0)),
                    Character = '╝'
                },
                HorizontalWall = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 84, 0)),
                    Character = '═'
                },
                VerticalWall = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 84, 0)),
                    Character = '║'
                },
                ConnectorWall = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 84, 0)),
                    Character = '╬'
                },
                TopLeftHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                    Character = '▒'
                },
                TopRightHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                    Character = '▒'
                },
                BottomLeftHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                    Character = '▒'
                },
                BottomRightHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                    Character = '▒'
                },
                HorizontalHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                    Character = '▒'
                },
                VerticalHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                    Character = '▒'
                },
                HorizontalTopHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                    Character = '▒'
                },
                HorizontalBottomHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                    Character = '▒'
                },
                VerticalRightHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                    Character = '▒'
                },
                VerticalLeftHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                    Character = '▒'
                },
                CentralHallway = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 168, 168, 168)),
                    Character = '▒'
                },
                Floor = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 84, 252, 84)),
                    Character = '.'
                },
                Stairs = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.FromArgb(255, 84, 252, 84)),
                    Character = '╫'
                },
                Empty = new ConsoleRepresentation
                {
                    BackgroundColor = new GameColor(Color.Black),
                    ForegroundColor = new GameColor(Color.Black),
                    Character = ' '
                }
            };
        }

        private static void UpdateReplaceConsoleRepresentationStepsToV11(this ActionWithEffectsInfoV11? actionWithEffects)
        {
            if (actionWithEffects == null) return;
            actionWithEffects.Effect.UpdateReplaceConsoleRepresentationParametersToV11();
        }

        private static void UpdateReplaceConsoleRepresentationParametersToV11(this EffectInfoV11? effect)
        {
            if (effect == null) return;
            if(effect.EffectName.Equals("ReplaceConsoleRepresentation"))
            {
                foreach (var param in effect.Params.Where(param => param.ParamName.Equals("Color", StringComparison.InvariantCultureIgnoreCase)))
                {
                    param.ParamName = "ForeColor";
                }
            }
            effect.Then?.UpdateReplaceConsoleRepresentationParametersToV11();
            effect.OnSuccess?.UpdateReplaceConsoleRepresentationParametersToV11();
            effect.OnFailure?.UpdateReplaceConsoleRepresentationParametersToV11();
        }

        #endregion

        #region 1.1 to 1.2

        private static DungeonInfo ConvertDungeonInfoToV12(this DungeonInfoV11 V11Dungeon)
        {
            var V11DungeonAsJSON = JsonSerializer.Serialize(V11Dungeon, new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
            var V12Dungeon = JsonSerializer.Deserialize<DungeonInfo>(V11DungeonAsJSON, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            foreach (var floorInfo in V12Dungeon.FloorInfos)
            {
                var v11FloorInfo = V11Dungeon.FloorInfos.Find(fi => fi.MinFloorLevel == floorInfo.MinFloorLevel && fi.MaxFloorLevel == floorInfo.MaxFloorLevel);
                if (v11FloorInfo == null) continue;
                floorInfo.OnFloorStart = v11FloorInfo.OnFloorStartActions.ElementAtOrDefault(0).CloneToV12();
                floorInfo.OnFloorStart.UpdateActionParametersToV12(false);
            }

            foreach (var v12PlayerClass in V12Dungeon.PlayerClasses)
            {
                var v11PlayerClass = V11Dungeon.PlayerClasses.Find(pc => pc.Id.Equals(v12PlayerClass.Id));
                if (v11PlayerClass == null) continue;
                v12PlayerClass.OnTurnStart = v11PlayerClass.OnTurnStartActions.ElementAtOrDefault(0).CloneToV12();
                v12PlayerClass.OnTurnStart.UpdateActionParametersToV12(false);
                v12PlayerClass.OnAttack = new();
                foreach (var onAttackAction in v11PlayerClass.OnAttackActions)
                {
                    var convertedAction = onAttackAction.CloneToV12();
                    convertedAction.UpdateActionParametersToV12(true);
                    v12PlayerClass.OnAttack.Add(convertedAction);
                }
                v12PlayerClass.OnAttacked = v11PlayerClass.OnAttackedActions.ElementAtOrDefault(0).CloneToV12();
                v12PlayerClass.OnAttacked.UpdateActionParametersToV12(false);
                v12PlayerClass.OnDeath = v11PlayerClass.OnDeathActions.ElementAtOrDefault(0).CloneToV12();
                v12PlayerClass.OnDeath.UpdateActionParametersToV12(false);
                v12PlayerClass.BaseAccuracy = 100;
                v12PlayerClass.BaseEvasion = 0;
            }

            foreach (var v12NPC in V12Dungeon.NPCs)
            {
                var v11NPC = V11Dungeon.NPCs.Find(npc => npc.Id.Equals(v12NPC.Id));
                if (v11NPC == null) continue;
                v12NPC.OnTurnStart = v11NPC.OnTurnStartActions.ElementAtOrDefault(0).CloneToV12();
                v12NPC.OnTurnStart.UpdateActionParametersToV12(false);
                v12NPC.OnAttack = new();
                foreach (var onAttackAction in v11NPC.OnAttackActions)
                {
                    var convertedAction = onAttackAction.CloneToV12();
                    convertedAction.UpdateActionParametersToV12(true);
                    v12NPC.OnAttack.Add(convertedAction);
                }
                v12NPC.OnAttacked = v11NPC.OnAttackedActions.ElementAtOrDefault(0).CloneToV12();
                v12NPC.OnAttacked.UpdateActionParametersToV12(false);
                v12NPC.OnDeath = v11NPC.OnDeathActions.ElementAtOrDefault(0).CloneToV12();
                v12NPC.OnDeath.UpdateActionParametersToV12(false);
                v12NPC.BaseAccuracy = 100;
                v12NPC.BaseEvasion = 0;
            }

            foreach (var v12Item in V12Dungeon.Items)
            {
                var v11Item = V11Dungeon.Items.Find(i => i.Id.Equals(v12Item.Id));
                if (v11Item == null) continue;
                v12Item.OnTurnStart = v11Item.OnTurnStartActions.ElementAtOrDefault(0).CloneToV12();
                v12Item.OnTurnStart.UpdateActionParametersToV12(false);
                v12Item.OnAttack = new();
                foreach (var onAttackAction in v11Item.OnAttackActions)
                {
                    var convertedAction = onAttackAction.CloneToV12();
                    convertedAction.UpdateActionParametersToV12(true);
                    v12Item.OnAttack.Add(convertedAction);
                }
                v12Item.OnAttacked = v11Item.OnAttackedActions.ElementAtOrDefault(0).CloneToV12();
                v12Item.OnAttacked.UpdateActionParametersToV12(false);
                v12Item.OnUse = v11Item.OnItemUseActions.ElementAtOrDefault(0).CloneToV12();
                v12Item.OnUse.UpdateActionParametersToV12(false);
                v12Item.OnStepped = v11Item.OnItemSteppedActions.ElementAtOrDefault(0).CloneToV12();
                v12Item.OnStepped.UpdateActionParametersToV12(false);
            }

            foreach (var v12Trap in V12Dungeon.Traps)
            {
                var v11Trap = V11Dungeon.Traps.Find(t => t.Id.Equals(v12Trap.Id));
                if (v11Trap == null) continue;
                v12Trap.OnStepped = v11Trap.OnItemSteppedActions.ElementAtOrDefault(0).CloneToV12();
                v12Trap.OnStepped.UpdateActionParametersToV12(false);
            }

            foreach (var v12AlteredStatus in V12Dungeon.AlteredStatuses)
            {
                var v11AlteredStatus = V11Dungeon.AlteredStatuses.Find(als => als.Id.Equals(v12AlteredStatus.Id));
                if (v11AlteredStatus == null) continue;
                v12AlteredStatus.OnApply = v11AlteredStatus.OnStatusApplyActions.ElementAtOrDefault(0).CloneToV12();
                v12AlteredStatus.OnApply.UpdateActionParametersToV12(false);
                v12AlteredStatus.OnTurnStart = v11AlteredStatus.OnTurnStartActions.ElementAtOrDefault(0).CloneToV12();
                v12AlteredStatus.OnTurnStart.UpdateActionParametersToV12(false);
            }

            V12Dungeon.Version = "1.2";
            return V12Dungeon;
        }

        private static ActionWithEffectsInfo CloneToV12(this ActionWithEffectsInfoV11 info)
        {
            if (info == null) return null;

            var clonedAction = new ActionWithEffectsInfo
            {
                Name = info.Name,
                Description = info.Description,
                CooldownBetweenUses = info.CooldownBetweenUses,
                StartingCooldown = info.StartingCooldown,
                MinimumRange = info.MinimumRange,
                MaximumRange = info.MaximumRange,
                MaximumUses = info.MaximumUses,
                MPCost = info.MPCost,
                UseCondition = info.UseCondition,
                TargetTypes = new List<string>(info.TargetTypes ?? new List<string>()),
                Effect = info.Effect.CloneToV12(),
                FinishesTurnWhenUsed = true
            };

            return clonedAction;
        }

        public static EffectInfo CloneToV12(this EffectInfoV11 info)
        {
            if (info == null) return null;

            var clonedEffect = new EffectInfo
            {
                EffectName = info.EffectName,
                Params = new Parameter[info.Params.Length]
            };

            for (int i = 0; i < clonedEffect.Params.Length; i++)
            {
                clonedEffect.Params[i] = new Parameter
                {
                    ParamName = info.Params[i].ParamName,
                    Value = info.Params[i].Value
                };
            }

            if (!string.IsNullOrWhiteSpace(info?.Then?.EffectName))
                clonedEffect.Then = info.Then.CloneToV12();
            if (!string.IsNullOrWhiteSpace(info?.OnSuccess?.EffectName))
                clonedEffect.OnSuccess = info.OnSuccess.CloneToV12();
            if (!string.IsNullOrWhiteSpace(info?.OnFailure?.EffectName))
                clonedEffect.OnFailure = info.OnFailure.CloneToV12();

            return clonedEffect;
        }

        private static void UpdateActionParametersToV12(this ActionWithEffectsInfo? actionWithEffects, bool bypassValueToSetIfDealDamageOrBurnMP)
        {
            if (actionWithEffects == null) return;
            actionWithEffects.UpdatePrintTextStepsToV12();
            actionWithEffects.UpdateChanceAndAccuracyParametersToV12(bypassValueToSetIfDealDamageOrBurnMP);
        }

        private static void UpdateChanceAndAccuracyParametersToV12(this ActionWithEffectsInfo? actionWithEffects, bool bypassValueToSetIfDealDamageOrBurnMP)
        {
            actionWithEffects.Effect.UpdateChanceAndAccuracyParametersToV12(bypassValueToSetIfDealDamageOrBurnMP);
        }

        private static void UpdateChanceAndAccuracyParametersToV12(this EffectInfo? effect, bool bypassValueToSetIfDealDamageOrBurnMP)
        {
            if (effect == null) return;
            var hasAccuracyParameter = false;
            foreach (var param in effect.Params.Where(param => param.ParamName.Equals("Chance", StringComparison.InvariantCultureIgnoreCase)))
            {
                param.ParamName = "Accuracy";
                hasAccuracyParameter = true;
            }
            if (hasAccuracyParameter || (effect.Params.Any(param => param.ParamName.Equals("Accuracy", StringComparison.InvariantCultureIgnoreCase))
                                         && !effect.Params.Any(param => param.ParamName.Equals("BypassesAccuracyCheck", StringComparison.InvariantCultureIgnoreCase))))
            {
                var paramsAsList = effect.Params.ToList();
                paramsAsList.Add(new Parameter
                {
                    ParamName = "BypassesAccuracyCheck",
                    Value = (!effect.EffectName.Equals("DealDamage") && !effect.EffectName.Equals("BurnMP")) ? bypassValueToSetIfDealDamageOrBurnMP.ToString() : false.ToString()
                });
                effect.Params = paramsAsList.ToArray();
            }
            effect.Then.UpdateChanceAndAccuracyParametersToV12(bypassValueToSetIfDealDamageOrBurnMP);
            effect.OnSuccess.UpdateChanceAndAccuracyParametersToV12(bypassValueToSetIfDealDamageOrBurnMP);
            effect.OnFailure.UpdateChanceAndAccuracyParametersToV12(bypassValueToSetIfDealDamageOrBurnMP);
        }

        private static void UpdatePrintTextStepsToV12(this ActionWithEffectsInfo? actionWithEffects)
        {
            actionWithEffects.Effect.UpdatePrintTextStepsToV12();
        }

        private static void UpdatePrintTextStepsToV12(this EffectInfo? effect)
        {
            if (effect == null) return;
            if (effect.EffectName.Equals("PrintText") && !effect.Params.Any(param => param.ParamName.Equals("BypassesVisibilityCheck", StringComparison.InvariantCultureIgnoreCase)))
            {
                var paramsAsList = effect.Params.ToList();
                paramsAsList.Add(new Parameter
                {
                    ParamName = "BypassesVisibilityCheck",
                    Value = false.ToString()
                });
                effect.Params = paramsAsList.ToArray();
            }
            effect.Then?.UpdatePrintTextStepsToV12();
            effect.OnSuccess?.UpdatePrintTextStepsToV12();
            effect.OnFailure?.UpdatePrintTextStepsToV12();
        }

        #endregion
    }
    #pragma warning restore S2589 // Boolean expressions should not be gratuitous
    #pragma warning restore S6605 // Collection-specific "Exists" method should be used instead of the "Any" extension
    #pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
    #pragma warning restore CS8604 // Posible argumento de referencia nulo
}
