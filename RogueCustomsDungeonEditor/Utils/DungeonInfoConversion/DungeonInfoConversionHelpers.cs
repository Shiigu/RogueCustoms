using RogueCustomsDungeonEditor.Controls;
using RogueCustomsDungeonEditor.EffectInfos;
using RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V11;
using RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V12;
using RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V13;
using RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V14;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils;
using RogueCustomsGameEngine.Utils.Enums;
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
#pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.

    public static class DungeonInfoConversionHelpers
    {
        public static DungeonInfo ConvertDungeonInfoIfNeeded(this DungeonInfo dungeon, string dungeonJson, LocaleInfo localeTemplate, List<string> mandatoryLocaleKeys)
        {
            var convertedLocales = false;
            while (!dungeon.Version.Equals(EngineConstants.CurrentDungeonJsonVersion))
            {
                var V10to11Dungeon = dungeon.Version.Equals("1.0") ? JsonSerializer.Deserialize<DungeonInfoV11>(dungeonJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) : null;
                var V11to12Dungeon = dungeon.Version.Equals("1.1") ? JsonSerializer.Deserialize<DungeonInfoV12>(dungeonJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) : null;
                var V12to13Dungeon = dungeon.Version.Equals("1.2") ? JsonSerializer.Deserialize<DungeonInfoV13>(dungeonJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) : null;
                var V13to14Dungeon = dungeon.Version.Equals("1.3") ? JsonSerializer.Deserialize<DungeonInfoV14>(dungeonJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) : null;
                var V14to15Dungeon = dungeon.Version.Equals("1.4") ? JsonSerializer.Deserialize<DungeonInfoV14>(dungeonJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) : null;
                switch (dungeon.Version)
                {
                    case "1.4":
                        dungeon = V14to15Dungeon.ConvertDungeonInfoToV15();
                        break;
                    case "1.3":
                        V13to14Dungeon = V12to13Dungeon.ConvertDungeonInfoToV14();
                        break;
                    case "1.2":
                        V12to13Dungeon = V11to12Dungeon.ConvertDungeonInfoToV13();
                        break;
                    case "1.1":
                        V11to12Dungeon = V10to11Dungeon.ConvertDungeonInfoToV12();
                        break;
                    case "1.0":
                        V10to11Dungeon = V10to11Dungeon.ConvertDungeonInfoToV11();
                        break;
                    default:
                        throw new ArgumentException($"There's no conversion method for Dungeon Version {dungeon.Version}");
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

        private static DungeonInfoV12 ConvertDungeonInfoToV12(this DungeonInfoV11 V11Dungeon)
        {
            var V11DungeonAsJSON = JsonSerializer.Serialize(V11Dungeon, new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
            var V12Dungeon = JsonSerializer.Deserialize<DungeonInfoV12>(V11DungeonAsJSON, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            foreach (var floorInfo in V12Dungeon.FloorInfos)
            {
                var v11FloorInfo = V11Dungeon.FloorInfos.Find(fi => fi.MinFloorLevel == floorInfo.MinFloorLevel && fi.MaxFloorLevel == floorInfo.MaxFloorLevel);
                if (v11FloorInfo == null) continue;
                floorInfo.OnFloorStartActions = new() { v11FloorInfo.OnFloorStartActions.ElementAtOrDefault(0).CloneToV12() };
                floorInfo.OnFloorStartActions.ElementAtOrDefault(0).UpdateActionParametersToV12(false);
            }

            foreach (var v12PlayerClass in V12Dungeon.PlayerClasses)
            {
                var v11PlayerClass = V11Dungeon.PlayerClasses.Find(pc => pc.Id.Equals(v12PlayerClass.Id));
                if (v11PlayerClass == null) continue;
                v12PlayerClass.OnTurnStartActions = new() { v11PlayerClass.OnTurnStartActions.ElementAtOrDefault(0).CloneToV12() };
                v12PlayerClass.OnTurnStartActions.ElementAtOrDefault(0).UpdateActionParametersToV12(false);
                v12PlayerClass.OnAttackActions = new();
                foreach (var onAttackAction in v11PlayerClass.OnAttackActions)
                {
                    var convertedAction = onAttackAction.CloneToV12();
                    convertedAction.UpdateActionParametersToV12(true);
                    v12PlayerClass.OnAttackActions.Add(convertedAction);
                }
                v12PlayerClass.OnAttackedActions = new() { v11PlayerClass.OnAttackedActions.ElementAtOrDefault(0).CloneToV12() };
                v12PlayerClass.OnAttackedActions.ElementAtOrDefault(0).UpdateActionParametersToV12(false);
                v12PlayerClass.OnDeathActions = new() { v11PlayerClass.OnDeathActions.ElementAtOrDefault(0).CloneToV12() };
                v12PlayerClass.OnDeathActions.ElementAtOrDefault(0).UpdateActionParametersToV12(false);
            }

            foreach (var v12NPC in V12Dungeon.NPCs)
            {
                var v11NPC = V11Dungeon.NPCs.Find(npc => npc.Id.Equals(v12NPC.Id));
                if (v11NPC == null) continue;
                v12NPC.OnTurnStartActions = new() { v11NPC.OnTurnStartActions.ElementAtOrDefault(0).CloneToV12() };
                v12NPC.OnTurnStartActions.ElementAtOrDefault(0).UpdateActionParametersToV12(false);
                v12NPC.OnAttackActions = new();
                foreach (var onAttackAction in v11NPC.OnAttackActions)
                {
                    var convertedAction = onAttackAction.CloneToV12();
                    convertedAction.UpdateActionParametersToV12(true);
                    v12NPC.OnAttackActions.Add(convertedAction);
                }
                v12NPC.OnAttackedActions = new() { v11NPC.OnAttackedActions.ElementAtOrDefault(0).CloneToV12() };
                v12NPC.OnAttackedActions.ElementAtOrDefault(0).UpdateActionParametersToV12(false);
                v12NPC.OnDeathActions = new() { v11NPC.OnDeathActions.ElementAtOrDefault(0).CloneToV12() };
                v12NPC.OnDeathActions.ElementAtOrDefault(0).UpdateActionParametersToV12(false);
            }

            foreach (var v12Item in V12Dungeon.Items)
            {
                var v11Item = V11Dungeon.Items.Find(i => i.Id.Equals(v12Item.Id));
                if (v11Item == null) continue;
                v12Item.OnTurnStartActions = new() { v11Item.OnTurnStartActions.ElementAtOrDefault(0).CloneToV12() };
                v12Item.OnTurnStartActions.ElementAtOrDefault(0).UpdateActionParametersToV12(false);
                v12Item.OnAttackActions = new();
                foreach (var onAttackAction in v11Item.OnAttackActions)
                {
                    var convertedAction = onAttackAction.CloneToV12();
                    convertedAction.UpdateActionParametersToV12(true);
                    v12Item.OnAttackActions.Add(convertedAction);
                }
                v12Item.OnAttackedActions = new() { v11Item.OnAttackedActions.ElementAtOrDefault(0).CloneToV12() };
                v12Item.OnAttackedActions.ElementAtOrDefault(0).UpdateActionParametersToV12(false);
                v12Item.OnItemUseActions = new() { v11Item.OnItemUseActions.ElementAtOrDefault(0).CloneToV12() };
                v12Item.OnItemUseActions.ElementAtOrDefault(0).UpdateActionParametersToV12(false);
            }

            foreach (var v12Trap in V12Dungeon.Traps)
            {
                var v11Trap = V11Dungeon.Traps.Find(t => t.Id.Equals(v12Trap.Id));
                if (v11Trap == null) continue;
                v12Trap.OnItemSteppedActions = new() { v11Trap.OnItemSteppedActions.ElementAtOrDefault(0).CloneToV12() };
                v12Trap.OnItemSteppedActions.ElementAtOrDefault(0).UpdateActionParametersToV12(false);
            }

            foreach (var v12AlteredStatus in V12Dungeon.AlteredStatuses)
            {
                var v11AlteredStatus = V11Dungeon.AlteredStatuses.Find(als => als.Id.Equals(v12AlteredStatus.Id));
                if (v11AlteredStatus == null) continue;
                v12AlteredStatus.OnStatusApplyActions = new() { v11AlteredStatus.OnStatusApplyActions.ElementAtOrDefault(0).CloneToV12() };
                v12AlteredStatus.OnStatusApplyActions.ElementAtOrDefault(0).UpdateActionParametersToV12(false);
                v12AlteredStatus.OnTurnStartActions = new() { v11AlteredStatus.OnTurnStartActions.ElementAtOrDefault(0).CloneToV12() };
                v12AlteredStatus.OnTurnStartActions.ElementAtOrDefault(0).UpdateActionParametersToV12(false);
            }

            V12Dungeon.Version = "1.2";
            return V12Dungeon;
        }

        private static ActionWithEffectsInfoV12 CloneToV12(this ActionWithEffectsInfoV11 info)
        {
            if (info == null) return null;

            var clonedAction = new ActionWithEffectsInfoV12
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
                Effect = info.Effect.CloneToV12()
            };

            return clonedAction;
        }

        public static EffectInfoV12 CloneToV12(this EffectInfoV11 info)
        {
            if (info == null) return null;

            var clonedEffect = new EffectInfoV12
            {
                EffectName = info.EffectName,
                Params = new ParameterV12[info.Params.Length]
            };

            for (int i = 0; i < clonedEffect.Params.Length; i++)
            {
                clonedEffect.Params[i] = new ParameterV12
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

        private static void UpdateActionParametersToV12(this ActionWithEffectsInfoV12? actionWithEffects, bool bypassValueToSetIfDealDamageOrBurnMP)
        {
            if (actionWithEffects == null) return;
            actionWithEffects.UpdatePrintTextStepsToV12();
            actionWithEffects.UpdateChanceAndAccuracyParametersToV12(bypassValueToSetIfDealDamageOrBurnMP);
        }

        private static void UpdateChanceAndAccuracyParametersToV12(this ActionWithEffectsInfoV12? actionWithEffects, bool bypassValueToSetIfDealDamageOrBurnMP)
        {
            actionWithEffects.Effect.UpdateChanceAndAccuracyParametersToV12(bypassValueToSetIfDealDamageOrBurnMP);
        }

        private static void UpdateChanceAndAccuracyParametersToV12(this EffectInfoV12? effect, bool bypassValueToSetIfDealDamageOrBurnMP)
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
                paramsAsList.Add(new ParameterV12
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

        private static void UpdatePrintTextStepsToV12(this ActionWithEffectsInfoV12? actionWithEffects)
        {
            actionWithEffects.Effect.UpdatePrintTextStepsToV12();
        }

        private static void UpdatePrintTextStepsToV12(this EffectInfoV12? effect)
        {
            if (effect == null) return;
            if (effect.EffectName.Equals("PrintText") && !effect.Params.Any(param => param.ParamName.Equals("BypassesVisibilityCheck", StringComparison.InvariantCultureIgnoreCase)))
            {
                var paramsAsList = effect.Params.ToList();
                paramsAsList.Add(new ParameterV12
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

        #region 1.2 to 1.3

        private static DungeonInfoV13 ConvertDungeonInfoToV13(this DungeonInfoV12 V12Dungeon)
        {
            var V12DungeonAsJSON = JsonSerializer.Serialize(V12Dungeon, new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
            var V13Dungeon = JsonSerializer.Deserialize<DungeonInfoV13>(V12DungeonAsJSON, new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
            foreach (var floorInfo in V12Dungeon.FloorInfos)
            {
                floorInfo.HungerDegeneration = 0;
            }

            foreach (var npc in V13Dungeon.NPCs)
            {
                npc.AIType = "Default";
            }

            foreach (var item in V13Dungeon.Items)
            {
                if (item.EntityType.Equals("Weapon") || item.EntityType.Equals("Armor"))
                    item.OnUse = null;
            }

            V13Dungeon.Version = "1.3";
            return V13Dungeon;
        }

        #endregion

        #region 1.3 to 1.4

        private static DungeonInfoV14 ConvertDungeonInfoToV14(this DungeonInfoV13 V13Dungeon)
        {
            var V13DungeonAsJSON = JsonSerializer.Serialize(V13Dungeon, new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
            var V14Dungeon = JsonSerializer.Deserialize<DungeonInfoV14>(V13DungeonAsJSON, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            for (var i = 0; i < V13Dungeon.FloorInfos.Count; i++)
            {
                var v13FloorInfo = V13Dungeon.FloorInfos[i];
                var v14FloorInfo = V14Dungeon.FloorInfos[i];
                v14FloorInfo.PossibleKeys = new()
                {
                    LockedRoomOdds = 0,
                    KeySpawnInEnemyInventoryOdds = 0,
                    MaxPercentageOfLockedCandidateRooms = 0,
                    KeyTypes = new()
                };
                v14FloorInfo.PossibleLayouts = new List<FloorLayoutGenerationInfo>();
                foreach (var generatorAlgorithm in v13FloorInfo.PossibleGeneratorAlgorithms)
                {
                    switch (generatorAlgorithm.Name)
                    {
                        case "Standard":
                            v14FloorInfo.PossibleLayouts.Add(ConstructFullRandom(generatorAlgorithm, v13FloorInfo.Width, v13FloorInfo.Height));
                            break;
                        case "OuterDummyRing":
                            v14FloorInfo.PossibleLayouts.Add(ConstructOuterDummyRing(generatorAlgorithm, v13FloorInfo.Width, v13FloorInfo.Height));
                            break;
                        case "InnerDummyRing":
                            v14FloorInfo.PossibleLayouts.Add(ConstructInnerDummyRing(generatorAlgorithm, v13FloorInfo.Width, v13FloorInfo.Height));
                            break;
                        case "OneBigRoom":
                            v14FloorInfo.PossibleLayouts.Add(ConstructOneBigRoom(generatorAlgorithm, v13FloorInfo.Width, v13FloorInfo.Height));
                            break;
                        default:
                            break;
                    }
                }
            }
            foreach (var playerClass in V14Dungeon.PlayerClasses)
            {
                foreach (var action in playerClass.OnAttack)
                {
                    action.UpdateDealDamageStepsToV14();
                }
                playerClass.OnAttacked?.UpdateDealDamageStepsToV14();
                playerClass.OnDeath?.UpdateDealDamageStepsToV14();
                playerClass.OnTurnStart?.UpdateDealDamageStepsToV14();
            }
            foreach (var npc in V14Dungeon.NPCs)
            {
                foreach (var action in npc.OnAttack)
                {
                    action.UpdateDealDamageStepsToV14();
                }
                foreach (var action in npc.OnInteracted)
                {
                    action.UpdateDealDamageStepsToV14();
                }
                npc.OnAttacked?.UpdateDealDamageStepsToV14();
                npc.OnDeath?.UpdateDealDamageStepsToV14();
                npc.OnTurnStart?.UpdateDealDamageStepsToV14();
                npc.OnSpawn?.UpdateDealDamageStepsToV14();
            }
            foreach (var item in V14Dungeon.Items)
            {
                foreach (var action in item.OnAttack)
                {
                    action.UpdateDealDamageStepsToV14();
                }
                item.OnAttacked?.UpdateDealDamageStepsToV14();
                item.OnDeath?.UpdateDealDamageStepsToV14();
                item.OnTurnStart?.UpdateDealDamageStepsToV14();
                item.OnUse?.UpdateDealDamageStepsToV14();
            }
            foreach (var trap in V14Dungeon.Traps)
            {
                trap.OnStepped?.UpdateDealDamageStepsToV14();
            }
            foreach (var alteredStatus in V14Dungeon.AlteredStatuses)
            {
                alteredStatus.BeforeAttack?.UpdateDealDamageStepsToV14();
                alteredStatus.OnAttacked?.UpdateDealDamageStepsToV14();
                alteredStatus.OnApply?.UpdateDealDamageStepsToV14();
                alteredStatus.OnTurnStart?.UpdateDealDamageStepsToV14();
                alteredStatus.OnRemove?.UpdateDealDamageStepsToV14();
            }

            V14Dungeon.Version = "1.4";
            return V14Dungeon;
        }

        private static FloorLayoutGenerationInfo ConstructFullRandom(GeneratorAlgorithmInfoV13 generatorToConvert, int width, int height)
        {
            var maxWidth = Math.Max(5, width / generatorToConvert.Columns);
            var maxHeight = Math.Max(5, height / generatorToConvert.Rows);
            var rows = generatorToConvert.Rows + generatorToConvert.Rows - 1;
            var columns = generatorToConvert.Columns + generatorToConvert.Columns - 1;
            var floorLayoutDispositon = new StringBuilder();
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    if (x % 2 == 0 && y % 2 == 0)
                        floorLayoutDispositon.Append(RoomDispositionType.RandomRoom.ToChar());
                    else if ((x % 2 != 0 && y % 2 == 0) || (x % 2 == 0 && y % 2 != 0))
                        floorLayoutDispositon.Append(RoomDispositionType.RandomConnection.ToChar());
                    else
                        floorLayoutDispositon.Append(RoomDispositionType.ConnectionImpossible.ToChar());
                }
            }
            var floorLayout = new FloorLayoutGenerationInfo
            {
                Name = $"{generatorToConvert.Name} - {generatorToConvert.Columns}c x {generatorToConvert.Rows}r",
                Rows = generatorToConvert.Rows,
                Columns = generatorToConvert.Columns,
                MinRoomSize = new RoomDimensionsInfo { Width = 5, Height = 5 },
                MaxRoomSize = new RoomDimensionsInfo { Width = maxWidth, Height = maxHeight },
                RoomDisposition = floorLayoutDispositon.ToString()
            };
            return floorLayout;
        }

        private static FloorLayoutGenerationInfo ConstructOuterDummyRing(GeneratorAlgorithmInfoV13 generatorToConvert, int width, int height)
        {
            var maxWidth = Math.Max(5, width / generatorToConvert.Columns);
            var maxHeight = Math.Max(5, height / generatorToConvert.Rows);
            var rows = generatorToConvert.Rows + generatorToConvert.Rows - 1;
            var columns = generatorToConvert.Columns + generatorToConvert.Columns - 1;
            var floorLayoutDispositon = new StringBuilder();
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    // Calculate which ring we are in (0-based index)
                    int ringNumber = Math.Min(Math.Min(x, columns - 1 - x), Math.Min(y, rows - 1 - y)) / 2;

                    // Determine if this ring should have Dummy rooms or Normal rooms
                    bool isDummyRing = ringNumber % 2 == 0;

                    if (y % 2 == 0) // Rows with rooms or dummy rooms
                    {
                        if (x % 2 == 0)
                        {
                            if(isDummyRing)
                                floorLayoutDispositon.Append(RoomDispositionType.GuaranteedDummyRoom.ToChar());
                            else
                                floorLayoutDispositon.Append(RoomDispositionType.GuaranteedRoom.ToChar());
                        }
                        else
                        {
                            floorLayoutDispositon.Append(RoomDispositionType.RandomConnection.ToChar());
                        }
                    }
                    else // Rows with connection/impossible spaces
                    {
                        if (x % 2 == 0)
                        {
                            floorLayoutDispositon.Append(RoomDispositionType.RandomConnection.ToChar());
                        }
                        else
                        {
                            floorLayoutDispositon.Append(RoomDispositionType.ConnectionImpossible.ToChar());
                        }
                    }
                }
            }
            var floorLayout = new FloorLayoutGenerationInfo
            {
                Name = $"{generatorToConvert.Name} - {generatorToConvert.Columns}c x {generatorToConvert.Rows}r",
                Rows = generatorToConvert.Rows,
                Columns = generatorToConvert.Columns,
                MinRoomSize = new RoomDimensionsInfo { Width = 5, Height = 5 },
                MaxRoomSize = new RoomDimensionsInfo { Width = maxWidth, Height = maxHeight },
                RoomDisposition = floorLayoutDispositon.ToString()
            };
            return floorLayout;
        }

        private static FloorLayoutGenerationInfo ConstructInnerDummyRing(GeneratorAlgorithmInfoV13 generatorToConvert, int width, int height)
        {
            var maxWidth = Math.Max(5, width / generatorToConvert.Columns);
            var maxHeight = Math.Max(5, height / generatorToConvert.Rows);
            var rows = generatorToConvert.Rows + generatorToConvert.Rows - 1;
            var columns = generatorToConvert.Columns + generatorToConvert.Columns - 1;
            var floorLayoutDispositon = new StringBuilder();
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    // Calculate which ring we are in (0-based index)
                    int ringNumber = Math.Min(Math.Min(x, columns - 1 - x), Math.Min(y, rows - 1 - y)) / 2;

                    // Determine if this ring should have Dummy rooms or Normal rooms
                    bool isDummyRing = ringNumber % 2 != 0;

                    if (y % 2 == 0) // Rows with rooms or dummy rooms
                    {
                        if (x % 2 == 0)
                        {
                            if (isDummyRing)
                                floorLayoutDispositon.Append(RoomDispositionType.GuaranteedDummyRoom.ToChar());
                            else
                                floorLayoutDispositon.Append(RoomDispositionType.GuaranteedRoom.ToChar());
                        }
                        else
                        {
                            floorLayoutDispositon.Append(RoomDispositionType.RandomConnection.ToChar());
                        }
                    }
                    else // Rows with connection/impossible spaces
                    {
                        if (x % 2 == 0)
                        {
                            floorLayoutDispositon.Append(RoomDispositionType.RandomConnection.ToChar());
                        }
                        else
                        {
                            floorLayoutDispositon.Append(RoomDispositionType.ConnectionImpossible.ToChar());
                        }
                    }
                }
            }
            var floorLayout = new FloorLayoutGenerationInfo
            {
                Name = $"{generatorToConvert.Name} - {generatorToConvert.Columns}c x {generatorToConvert.Rows}r",
                Rows = generatorToConvert.Rows,
                Columns = generatorToConvert.Columns,
                MinRoomSize = new RoomDimensionsInfo { Width = 5, Height = 5 },
                MaxRoomSize = new RoomDimensionsInfo { Width = maxWidth, Height = maxHeight },
                RoomDisposition = floorLayoutDispositon.ToString()
            };
            return floorLayout;
        }

        private static FloorLayoutGenerationInfo ConstructOneBigRoom(GeneratorAlgorithmInfoV13 generatorToConvert, int width, int height)
        {
            var maxWidth = Math.Max(5, width / generatorToConvert.Columns);
            var maxHeight = Math.Max(5, height / generatorToConvert.Rows);
            var rows = generatorToConvert.Rows + generatorToConvert.Rows - 1;
            var columns = generatorToConvert.Columns + generatorToConvert.Columns - 1;
            var floorLayoutDispositon = new StringBuilder();
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    if (x == 0 && y == 0)
                        floorLayoutDispositon.Append(RoomDispositionType.GuaranteedRoom.ToChar());
                    else if (x % 2 == 0 && y % 2 == 0)
                        floorLayoutDispositon.Append(RoomDispositionType.NoRoom.ToChar());
                    else if ((x % 2 != 0 && y % 2 == 0) || (x % 2 == 0 && y % 2 != 0))
                        floorLayoutDispositon.Append(RoomDispositionType.NoConnection.ToChar());
                    else
                        floorLayoutDispositon.Append(RoomDispositionType.ConnectionImpossible.ToChar());
                }
            }
            var floorLayout = new FloorLayoutGenerationInfo
            {
                Name = $"{generatorToConvert.Name} - {generatorToConvert.Columns}c x {generatorToConvert.Rows}r",
                Rows = generatorToConvert.Rows,
                Columns = generatorToConvert.Columns,
                MinRoomSize = new RoomDimensionsInfo { Width = 5, Height = 5 },
                MaxRoomSize = new RoomDimensionsInfo { Width = maxWidth, Height = maxHeight },
                RoomDisposition = floorLayoutDispositon.ToString()
            };
            return floorLayout;
        }

        private static void UpdateDealDamageStepsToV14(this ActionWithEffectsInfo? actionWithEffects)
        {
            if (actionWithEffects == null) return;
            actionWithEffects.Effect.UpdateDealDamageParametersToV14();
        }

        private static void UpdateDealDamageParametersToV14(this EffectInfo? effect)
        {
            if (effect == null) return;
            if (effect.EffectName.Equals("DealDamage"))
            {
                var effectParams = effect.Params.ToList();
                if (!effectParams.Any(param => param.ParamName.Equals("CriticalHitChance", StringComparison.InvariantCultureIgnoreCase)))
                    effectParams.Add(new()
                    {
                        ParamName = "CriticalHitChance",
                        Value = "0"
                    });
                if (!effectParams.Any(param => param.ParamName.Equals("CriticalHitFormula", StringComparison.InvariantCultureIgnoreCase)))
                    effectParams.Add(new()
                    {
                        ParamName = "CriticalHitFormula",
                        Value = "{CalculatedDamage}"
                    });
                if (effect.OnFailure != null && effect.OnFailure.EffectName.Equals("PrintText") && effect.OnFailure.Then == null)
                {
                    // DealDamage with a only a PrintText as OnFailure are assumed to only be telling the player it failed, so it's removed due to the refactor.
                    effect.OnFailure = null;
                }
                effect.Params = effectParams.ToArray();
            }
            effect.Then?.UpdateDealDamageParametersToV14();
            effect.OnSuccess?.UpdateDealDamageParametersToV14();
            effect.OnFailure?.UpdateDealDamageParametersToV14();
        }

        #endregion

        #region 1.4 to 1.5
        private static DungeonInfo ConvertDungeonInfoToV15(this DungeonInfoV14 V14Dungeon)
        {
            var V14DungeonAsJSON = JsonSerializer.Serialize(V14Dungeon, new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
            var V15Dungeon = JsonSerializer.Deserialize<DungeonInfo>(V14DungeonAsJSON, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            V15Dungeon.TileTypeInfos = DungeonInfoHelpers.CreateDefaultTileTypes();
            V15Dungeon.TileSetInfos = new();

            foreach (var v14TileSet in V14Dungeon.TileSetInfos)
            {
                var v15TileSet = new TileSetInfo() { Id = v14TileSet.Id, TileTypes = new() };

                v15TileSet.TileTypes.Add(new()
                {
                    TileTypeId = "Empty",
                    Central = v14TileSet.Empty
                });

                v15TileSet.TileTypes.Add(new()
                {
                    TileTypeId = "Floor",
                    Central = v14TileSet.Floor
                });

                v15TileSet.TileTypes.Add(new()
                {
                    TileTypeId = "Wall",
                    BottomLeft = v14TileSet.BottomLeftWall,
                    BottomRight = v14TileSet.BottomRightWall,
                    TopLeft = v14TileSet.TopLeftWall,
                    TopRight = v14TileSet.TopRightWall,
                    Horizontal = v14TileSet.HorizontalWall,
                    Vertical = v14TileSet.VerticalWall,
                    Connector = v14TileSet.ConnectorWall,
                    Central = v14TileSet.ConnectorWall,
                });

                v15TileSet.TileTypes.Add(new()
                {
                    TileTypeId = "Hallway",
                    BottomLeft = v14TileSet.BottomLeftHallway,
                    BottomRight = v14TileSet.BottomRightHallway,
                    TopLeft = v14TileSet.TopLeftHallway,
                    TopRight = v14TileSet.TopRightHallway,
                    Horizontal = v14TileSet.HorizontalHallway,
                    Vertical = v14TileSet.VerticalHallway,
                    HorizontalBottom = v14TileSet.HorizontalBottomHallway,
                    HorizontalTop = v14TileSet.HorizontalTopHallway,
                    VerticalLeft = v14TileSet.VerticalLeftHallway,
                    VerticalRight = v14TileSet.VerticalRightHallway,
                    Connector = v14TileSet.ConnectorWall,
                    Central = v14TileSet.CentralHallway,
                });

                v15TileSet.TileTypes.Add(new()
                {
                    TileTypeId = "Stairs",
                    Central = v14TileSet.Stairs
                });

                V15Dungeon.TileSetInfos.Add(v15TileSet);
            }

            foreach (var floorGroup in V15Dungeon.FloorInfos)
            {
                floorGroup.MonsterHouseOdds = 0;
            }

            V15Dungeon.ElementInfos = new()
            {
                new ElementInfo()
                {
                    Id = "Normal",
                    Name = "ElementNameNormal",
                    Color = new GameColor(Color.White),
                    ResistanceStatId = "",
                    ExcessResistanceCausesHealDamage = false,
                    OnAfterAttack = null
                }
            };

            V15Dungeon.CharacterStats = DungeonInfoHelpers.CreateStatsTemplate();
            
            V15Dungeon.PlayerClasses = new();

            foreach (var v14PlayerClass in V14Dungeon.PlayerClasses)
            {
                V15Dungeon.PlayerClasses.Add(v14PlayerClass.CloneToV15());
            }

            foreach (var playerClass in V15Dungeon.PlayerClasses)
            {
                foreach (var action in playerClass.OnAttack)
                {
                    action.UpdateDealDamageStepsToV15();
                }
                playerClass.OnAttacked?.UpdateDealDamageStepsToV15();
                playerClass.OnDeath?.UpdateDealDamageStepsToV15();
                playerClass.OnTurnStart?.UpdateDealDamageStepsToV15();
            }

            V15Dungeon.NPCs = new();

            foreach (var v14NPC in V14Dungeon.NPCs)
            {
                V15Dungeon.NPCs.Add(v14NPC.CloneToV15());
            }

            foreach (var npc in V15Dungeon.NPCs)
            {
                foreach (var action in npc.OnAttack)
                {
                    action.UpdateDealDamageStepsToV15();
                }
                foreach (var action in npc.OnInteracted)
                {
                    action.UpdateDealDamageStepsToV15();
                }
                npc.OnAttacked?.UpdateDealDamageStepsToV15();
                npc.OnDeath?.UpdateDealDamageStepsToV15();
                npc.OnTurnStart?.UpdateDealDamageStepsToV15();
                npc.OnSpawn?.UpdateDealDamageStepsToV15();
            }

            foreach (var item in V15Dungeon.Items)
            {
                foreach (var action in item.OnAttack)
                {
                    action.UpdateDealDamageStepsToV15();
                }
                item.OnAttacked?.UpdateDealDamageStepsToV15();
                item.OnDeath?.UpdateDealDamageStepsToV15();
                item.OnTurnStart?.UpdateDealDamageStepsToV15();
                item.OnUse?.UpdateDealDamageStepsToV15();
            }
            foreach (var trap in V15Dungeon.Traps)
            {
                trap.OnStepped?.UpdateDealDamageStepsToV15();
            }
            foreach (var alteredStatus in V15Dungeon.AlteredStatuses)
            {
                alteredStatus.BeforeAttack?.UpdateDealDamageStepsToV15();
                alteredStatus.OnAttacked?.UpdateDealDamageStepsToV15();
                alteredStatus.OnApply?.UpdateDealDamageStepsToV15();
                alteredStatus.OnTurnStart?.UpdateDealDamageStepsToV15();
                alteredStatus.OnRemove?.UpdateDealDamageStepsToV15();
            }

            V15Dungeon.Version = "1.5";
            return V15Dungeon;
        }

        private static PlayerClassInfo CloneToV15(this PlayerClassInfoV14 v14PlayerClass)
        {
            var v15PlayerClass = new PlayerClassInfo
            {
                Id = v14PlayerClass.Id,
                Name = v14PlayerClass.Name,
                Description = v14PlayerClass.Description,
                ConsoleRepresentation = v14PlayerClass.ConsoleRepresentation.Clone(),
                RequiresNamePrompt = v14PlayerClass.RequiresNamePrompt,
                Faction = v14PlayerClass.Faction,
                StartsVisible = v14PlayerClass.StartsVisible,
                Stats = new(),
                BaseSightRange = v14PlayerClass.BaseSightRange,
                InventorySize = v14PlayerClass.InventorySize,
                StartingWeapon = v14PlayerClass.StartingWeapon,
                StartingArmor = v14PlayerClass.StartingArmor,
                StartingInventory = new(v14PlayerClass.StartingInventory),
                MaxLevel = v14PlayerClass.MaxLevel,
                CanGainExperience = v14PlayerClass.CanGainExperience,
                ExperiencePayoutFormula = v14PlayerClass.ExperiencePayoutFormula,
                ExperienceToLevelUpFormula = v14PlayerClass.ExperienceToLevelUpFormula,
                OnTurnStart = v14PlayerClass.OnTurnStart,
                OnAttack = new(v14PlayerClass.OnAttack),
                OnAttacked = v14PlayerClass.OnAttacked,
                OnDeath = v14PlayerClass.OnDeath
            };

            v15PlayerClass.Stats.Add(new()
            {
                StatId = "HP",
                Base = v14PlayerClass.BaseHP,
                IncreasePerLevel = v14PlayerClass.MaxHPIncreasePerLevel
            });

            v15PlayerClass.Stats.Add(new()
            {
                StatId = "HPRegeneration",
                Base = v14PlayerClass.BaseHPRegeneration,
                IncreasePerLevel = v14PlayerClass.HPRegenerationIncreasePerLevel
            });

            if (v14PlayerClass.UsesMP)
            {
                v15PlayerClass.Stats.Add(new()
                {
                    StatId = "MP",
                    Base = v14PlayerClass.BaseMP,
                    IncreasePerLevel = v14PlayerClass.MaxMPIncreasePerLevel
                });

                v15PlayerClass.Stats.Add(new()
                {
                    StatId = "MPRegeneration",
                    Base = v14PlayerClass.BaseMPRegeneration,
                    IncreasePerLevel = v14PlayerClass.MPRegenerationIncreasePerLevel
                });
            }

            if (v14PlayerClass.UsesHunger)
            {
                v15PlayerClass.Stats.Add(new()
                {
                    StatId = "Hunger",
                    Base = v14PlayerClass.BaseHunger,
                    IncreasePerLevel = 0
                });
            }

            v15PlayerClass.Stats.Add(new()
            {
                StatId = "Attack",
                Base = v14PlayerClass.BaseAttack,
                IncreasePerLevel = v14PlayerClass.AttackIncreasePerLevel
            });

            v15PlayerClass.Stats.Add(new()
            {
                StatId = "Defense",
                Base = v14PlayerClass.BaseDefense,
                IncreasePerLevel = v14PlayerClass.DefenseIncreasePerLevel
            });

            v15PlayerClass.Stats.Add(new()
            {
                StatId = "Movement",
                Base = v14PlayerClass.BaseMovement,
                IncreasePerLevel = v14PlayerClass.MovementIncreasePerLevel
            });

            v15PlayerClass.Stats.Add(new()
            {
                StatId = "Accuracy",
                Base = v14PlayerClass.BaseAccuracy,
                IncreasePerLevel = 0
            });

            v15PlayerClass.Stats.Add(new()
            {
                StatId = "Evasion",
                Base = v14PlayerClass.BaseEvasion,
                IncreasePerLevel = 0
            });

            return v15PlayerClass;
        }
        private static NPCInfo CloneToV15(this NPCInfoV14 v14NPC)
        {
            var v15NPC = new NPCInfo
            {
                Id = v14NPC.Id,
                Name = v14NPC.Name,
                Description = v14NPC.Description,
                ConsoleRepresentation = v14NPC.ConsoleRepresentation.Clone(),
                Faction = v14NPC.Faction,
                StartsVisible = v14NPC.StartsVisible,
                Stats = new(),
                BaseSightRange = v14NPC.BaseSightRange,
                InventorySize = v14NPC.InventorySize,
                StartingWeapon = v14NPC.StartingWeapon,
                StartingArmor = v14NPC.StartingArmor,
                StartingInventory = new(v14NPC.StartingInventory),
                MaxLevel = v14NPC.MaxLevel,
                CanGainExperience = v14NPC.CanGainExperience,
                ExperiencePayoutFormula = v14NPC.ExperiencePayoutFormula,
                ExperienceToLevelUpFormula = v14NPC.ExperienceToLevelUpFormula,
                OnTurnStart = v14NPC.OnTurnStart,
                OnAttack = new(v14NPC.OnAttack),
                OnAttacked = v14NPC.OnAttacked,
                OnDeath = v14NPC.OnDeath,
                OnInteracted = new(v14NPC.OnInteracted),
                OnSpawn = v14NPC.OnSpawn,
                AIOddsToUseActionsOnSelf = v14NPC.AIOddsToUseActionsOnSelf,
                AIType = v14NPC.AIType,
                KnowsAllCharacterPositions = v14NPC.KnowsAllCharacterPositions,                
            };

            v15NPC.Stats.Add(new()
            {
                StatId = "HP",
                Base = v14NPC.BaseHP,
                IncreasePerLevel = v14NPC.MaxHPIncreasePerLevel
            });

            v15NPC.Stats.Add(new()
            {
                StatId = "HPRegeneration",
                Base = v14NPC.BaseHPRegeneration,
                IncreasePerLevel = v14NPC.HPRegenerationIncreasePerLevel
            });

            if (v14NPC.UsesMP)
            {
                v15NPC.Stats.Add(new()
                {
                    StatId = "MP",
                    Base = v14NPC.BaseMP,
                    IncreasePerLevel = v14NPC.MaxMPIncreasePerLevel
                });

                v15NPC.Stats.Add(new()
                {
                    StatId = "MPRegeneration",
                    Base = v14NPC.BaseMPRegeneration,
                    IncreasePerLevel = v14NPC.MPRegenerationIncreasePerLevel
                });
            }

            if (v14NPC.UsesHunger)
            {
                v15NPC.Stats.Add(new()
                {
                    StatId = "Hunger",
                    Base = v14NPC.BaseHunger,
                    IncreasePerLevel = 0
                });
            }

            v15NPC.Stats.Add(new()
            {
                StatId = "Attack",
                Base = v14NPC.BaseAttack,
                IncreasePerLevel = v14NPC.AttackIncreasePerLevel
            });

            v15NPC.Stats.Add(new()
            {
                StatId = "Defense",
                Base = v14NPC.BaseDefense,
                IncreasePerLevel = v14NPC.DefenseIncreasePerLevel
            });

            v15NPC.Stats.Add(new()
            {
                StatId = "Movement",
                Base = v14NPC.BaseMovement,
                IncreasePerLevel = v14NPC.MovementIncreasePerLevel
            });

            v15NPC.Stats.Add(new()
            {
                StatId = "Accuracy",
                Base = v14NPC.BaseAccuracy,
                IncreasePerLevel = 0
            });

            v15NPC.Stats.Add(new()
            {
                StatId = "Evasion",
                Base = v14NPC.BaseEvasion,
                IncreasePerLevel = 0
            });

            return v15NPC;
        }

        private static void UpdateDealDamageStepsToV15(this ActionWithEffectsInfo? actionWithEffects)
        {
            if (actionWithEffects == null) return;
            actionWithEffects.Effect.UpdateDealDamageParametersToV15();
        }

        private static void UpdateDealDamageParametersToV15(this EffectInfo? effect)
        {
            if (effect == null) return;
            if (effect.EffectName.Equals("DealDamage"))
            {
                var effectParams = effect.Params.ToList();
                if (!effectParams.Any(param => param.ParamName.Equals("Element", StringComparison.InvariantCultureIgnoreCase)))
                    effectParams.Add(new()
                    {
                        ParamName = "Element",
                        Value = "Normal"
                    });
                if (!effectParams.Any(param => param.ParamName.Equals("BypassesResistances", StringComparison.InvariantCultureIgnoreCase)))
                    effectParams.Add(new()
                    {
                        ParamName = "BypassesResistances",
                        Value = "true"
                    });
                if (!effectParams.Any(param => param.ParamName.Equals("BypassesElementEffect", StringComparison.InvariantCultureIgnoreCase)))
                    effectParams.Add(new()
                    {
                        ParamName = "BypassesElementEffect",
                        Value = "true"
                    });
                effect.Params = effectParams.ToArray();
            }
            effect.Then?.UpdateDealDamageParametersToV15();
            effect.OnSuccess?.UpdateDealDamageParametersToV15();
            effect.OnFailure?.UpdateDealDamageParametersToV15();
        }
        #endregion
    }
#pragma warning restore S2589 // Boolean expressions should not be gratuitous
#pragma warning restore S6605 // Collection-specific "Exists" method should be used instead of the "Any" extension
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
#pragma warning restore CS8604 // Posible argumento de referencia nulo
#pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}
