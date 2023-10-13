using RogueCustomsGameEngine.Utils;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion
{
    public static class DungeonInfoConversionHelpers
    {
        public static DungeonInfo ConvertDungeonInfoIfNeeded(this DungeonInfo dungeon, LocaleInfo localeTemplate, List<string> mandatoryLocaleKeys)
        {
            var convertedLocales = false;
            while(!dungeon.Version.Equals(Constants.CurrentDungeonJsonVersion))
            {
                if(!convertedLocales)
                {
                    foreach (var localeInfo in dungeon.Locales)
                    {
                        localeInfo.AddMissingMandatoryLocalesIfNeeded(localeTemplate, mandatoryLocaleKeys);
                    }
                    convertedLocales = true;
                }
                switch (dungeon.Version)
                {
                    case "1.0":
                    default:
                        dungeon = dungeon.ConvertDungeonInfoToV11();
                        break;
                }
            }
            return dungeon;
        }

        #region 1.0 to 1.1
        private static DungeonInfo ConvertDungeonInfoToV11(this DungeonInfo dungeon)
        {
            dungeon.TileSetInfos = new()
            {
                DungeonInfoHelpers.CreateDefaultTileSet(),
                DungeonInfoHelpers.CreateRetroTileSet()
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

        private static void UpdateReplaceConsoleRepresentationStepsToV11(this ActionWithEffectsInfo actionWithEffects)
        {
            actionWithEffects.Effect.UpdateReplaceConsoleRepresentationParametersToV11();
        }

        private static void UpdateReplaceConsoleRepresentationParametersToV11(this EffectInfo effect)
        {
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

    }
}
