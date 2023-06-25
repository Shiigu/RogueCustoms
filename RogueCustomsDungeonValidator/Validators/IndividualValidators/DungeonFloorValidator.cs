using RogueCustomsDungeonValidator.Utils;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RogueCustomsDungeonValidator.Validators.IndividualValidators
{
    public class DungeonFloorValidator
    {
        public static DungeonValidationMessages ValidateGeneralFloorPlan(DungeonInfo dungeonJson)
        {
            var messages = new DungeonValidationMessages();

            if (dungeonJson.AmountOfFloors <= 0)
                messages.AddError("Dungeon's Floor Amount is invalid. Must be an integer number higher than 0.");

            for (int i = 1; i <= dungeonJson.AmountOfFloors; i++)
            {
                var typesForFloorLevel = dungeonJson.FloorInfos.Where(fi => fi.MinFloorLevel <= fi.MaxFloorLevel && i.Between(fi.MinFloorLevel, fi.MaxFloorLevel));
                if (!typesForFloorLevel.Any())
                    messages.AddError($"Dungeon does not have a FloorInfo that matches Floor Level {i}.");
                else if (typesForFloorLevel.Count() > 1)
                    messages.AddError($"Dungeon has more than one FloorInfo that matches Floor Level {i}: {typesForFloorLevel.Select(ft => $"[{ft.MinFloorLevel}-{ft.MaxFloorLevel}]").JoinAnd()}");
            }

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }

        public static DungeonValidationMessages ValidateFloorType(FloorInfo floorJson, DungeonInfo dungeonJson, Dungeon sampleDungeon)
        {
            var messages = new DungeonValidationMessages();

            if (floorJson.MinFloorLevel <= 0)
                messages.AddError("MinFloorLevel must be an integer number higher than 0.");
            if (floorJson.MaxFloorLevel <= 0)
                messages.AddError("MaxFloorLevel must be an integer number higher than 0.");
            if (floorJson.MinFloorLevel > floorJson.MaxFloorLevel)
                messages.AddError("MaxFloorLevel cannot be lower than MinFloorLevel.");
            if (floorJson.Width <= 0)
                messages.AddError("Width must be an integer number higher than 0.");
            else if (floorJson.Width > 64)
                messages.AddWarning($"With a Floor Width of {floorJson.Width}, only a portion of the floor will be visible in the Console Client at the same time. If you want the entire Width to be visible, set it between 5 and 64.");
            if (floorJson.Height <= 0)
                messages.AddError("Height must be an integer number higher than 0.");
            else if (floorJson.Height > 32)
                messages.AddWarning($"With a Floor Height of {floorJson.Height}, only a portion of the floor will be visible in the Console Client at the same time. If you want the entire Height to be visible, set it between 5 and 32.");
            if (!floorJson.GenerateStairsOnStart)
                messages.AddWarning("GenerateStairsOnStart is false. Make sure at least one Character/Item/Trap calls GenerateStairs, or the Dungeon won't be able to be completed.");

            if (floorJson.PossibleMonsters.Any())
            {
                var totalChanceForPossibleMonsters = 0;
                foreach (var possibleMonster in floorJson.PossibleMonsters)
                {
                    if(!dungeonJson.Characters.Any(c => c.EntityType == "NPC" && c.Id.Equals(possibleMonster.ClassId)))
                        messages.AddError($"{possibleMonster.ClassId} is in the PossibleMonsters list but it's not an NPC.");
                    if (floorJson.PossibleMonsters.Count(pm => pm.ClassId.Equals(possibleMonster.ClassId)) > 1)
                        messages.AddError($"{possibleMonster.ClassId} shows up as a duplicate PossibleMonster in the current Floor Type.");
                    if (!possibleMonster.ChanceToPick.Between(1, 100))
                        messages.AddError($"{possibleMonster.ClassId}'s ChanceToPick must be an integer number between 1 and 100.");
                    else
                        totalChanceForPossibleMonsters += possibleMonster.ChanceToPick;
                    if (possibleMonster.MinLevel <= 0)
                        messages.AddError($"{possibleMonster.ClassId}'s MinLevel must be an integer number higher than 0.");
                    if (possibleMonster.MaxLevel <= 0)
                        messages.AddError($"{possibleMonster.ClassId}'s MaxLevel must be an integer number higher than 0.");
                    if (possibleMonster.MaxLevel < possibleMonster.MinLevel)
                        messages.AddError($"{possibleMonster.ClassId}'s MaxLevel cannot be lower than its MinLevel.");
                    if (possibleMonster.OverallMaxForKindInFloor <= 0)
                        messages.AddError($"{possibleMonster.ClassId}'s OverallMaxForKindInFloor must be an integer number higher than 0.");
                    if (possibleMonster.SimultaneousMaxForKindInFloor <= 0)
                        messages.AddError($"{possibleMonster.ClassId}'s SimultaneousMaxForKindInFloor must be an integer number higher than 0.");
                    if (possibleMonster.OverallMaxForKindInFloor < possibleMonster.SimultaneousMaxForKindInFloor)
                        messages.AddError($"{possibleMonster.ClassId}'s OverallMaxForKindIsFloor cannot be lower than its SimultaneousMaxForKindInFloor.");
                    if (!possibleMonster.CanSpawnOnFirstTurn && !possibleMonster.CanSpawnAfterFirstTurn)
                        messages.AddError($"{possibleMonster.ClassId}'s CanSpawnOnFirstTurn and CanSpawnAfterFirstTurn are both disabled.");
                }
                if (totalChanceForPossibleMonsters != 100)
                    messages.AddError("Total chance for PossibleMonsters does not equal 100.");

                if (floorJson.SimultaneousMinMonstersAtStart < 0)
                    messages.AddError("SimultaneousMinMonstersAtStart must be an integer number equal to or higher than 0.");
                else if (floorJson.SimultaneousMinMonstersAtStart == 0 && floorJson.PossibleMonsters.Any())
                    messages.AddWarning("SimultaneousMinMonstersAtStart is 0. No monsters will be present at start.");
                if (floorJson.SimultaneousMaxMonstersInFloor < 0)
                    messages.AddError("SimultaneousMaxMonstersInFloor must be an integer number equal to or higher than 0.");
                else if (floorJson.SimultaneousMinMonstersAtStart == 0 && floorJson.PossibleMonsters.Any())
                    messages.AddError("SimultaneousMaxMonstersInFloor is 0 but PossibleMonsters is not empty.");
                if (floorJson.SimultaneousMinMonstersAtStart > floorJson.SimultaneousMaxMonstersInFloor)
                    messages.AddError("SimultaneousMinMonstersAtStart is higher than SimultaneousMaxMonstersInFloor.");
                if (floorJson.TurnsPerMonsterGeneration <= 0)
                    messages.AddError("TurnsPerMonsterGeneration must be an integer number higher than 0.");
            }

            if (floorJson.PossibleItems.Any())
            {
                var totalChanceForPossibleItems = 0;
                foreach (var possibleItem in floorJson.PossibleItems)
                {
                    if (!dungeonJson.Items.Any(c => (c.EntityType == "Weapon" || c.EntityType == "Armor" || c.EntityType == "Consumable") && c.Id.Equals(possibleItem.ClassId)))
                        messages.AddError($"{possibleItem.ClassId} is in the PossibleItems list but it's not an Item.");
                    if (floorJson.PossibleItems.Count(pm => pm.ClassId.Equals(possibleItem.ClassId)) > 1)
                        messages.AddError($"{possibleItem.ClassId} shows up as a duplicate PossibleItem in the current Floor Type.");
                    if (!possibleItem.ChanceToPick.Between(1, 100))
                        messages.AddError($"{possibleItem.ClassId}'s ChanceToPick must be an integer number between 1 and 100.");
                    else
                        totalChanceForPossibleItems += possibleItem.ChanceToPick;
                    if (possibleItem.SimultaneousMaxForKindInFloor <= 0)
                        messages.AddError($"{possibleItem.ClassId}'s SimultaneousMaxForKindInFloor must be an integer number higher than 0.");
                }
                if (totalChanceForPossibleItems != 100)
                    messages.AddError("Total chance for PossibleItems does not equal 100.");
                if (floorJson.MinItemsInFloor < 0)
                    messages.AddError("MinItemsInFloor must be an integer number equal to or higher than 0.");
                if (floorJson.MaxItemsInFloor < 0)
                    messages.AddError("MaxItemsInFloor must be an integer number equal to or higher than 0.");
                else if (floorJson.MaxItemsInFloor == 0)
                    messages.AddError("MaxItemsInFloor is 0 but PossibleItems is not empty.");
            }

            if (floorJson.MaxItemsInFloor > 0 && !floorJson.PossibleItems.Any())
                messages.AddError("MaxItemsInFloor is higher than 0 but PossibleItems is empty.");
            if (floorJson.MaxItemsInFloor < floorJson.MinItemsInFloor)
                messages.AddError("MaxItemsInFloor cannot be than its MinItemsInFloor.");

            if (floorJson.PossibleTraps.Any())
            {
                var totalChanceForPossibleTraps = 0;
                foreach (var possibleTrap in floorJson.PossibleTraps)
                {
                    if (!dungeonJson.Traps.Any(c => c.Id.Equals(possibleTrap.ClassId)))
                        messages.AddError($"{possibleTrap.ClassId} is in the PossibleTraps list but it's not a Trap.");
                    if (floorJson.PossibleTraps.Count(pm => pm.ClassId.Equals(possibleTrap.ClassId)) > 1)
                        messages.AddError($"{possibleTrap.ClassId} shows up as a duplicate PossibleTrap in the current Floor Type.");
                    if (!possibleTrap.ChanceToPick.Between(1, 100))
                        messages.AddError($"{possibleTrap.ClassId}'s ChanceToPick must be an integer number between 1 and 100.");
                    else
                        totalChanceForPossibleTraps += possibleTrap.ChanceToPick;
                    if (possibleTrap.SimultaneousMaxForKindInFloor <= 0)
                        messages.AddError($"{possibleTrap.ClassId}'s SimultaneousMaxForKindInFloor must be an integer number higher than 0.");
                }
                if (totalChanceForPossibleTraps != 100)
                    messages.AddError("Total chance for PossibleTraps does not equal 100.");
                if (floorJson.MinTrapsInFloor < 0)
                    messages.AddError("MinTrapsInFloor must be an integer number equal to or higher than 0.");
                if (floorJson.MaxTrapsInFloor < 0)
                    messages.AddError("MaxTrapsInFloor must be an integer number equal to or higher than 0.");
                else if (floorJson.MaxTrapsInFloor == 0)
                    messages.AddError("MaxTrapsInFloor is 0 but PossibleTraps is not empty.");
            }

            if (floorJson.MaxTrapsInFloor > 0 && !floorJson.PossibleTraps.Any())
                messages.AddError("MaxTrapsInFloor is higher than 0 but PossibleTraps is empty.");
            if (floorJson.MaxTrapsInFloor < floorJson.MinTrapsInFloor)
                messages.AddError("MaxTrapsInFloor cannot be than its MinTrapsInFloor.");

            if (floorJson.MaxConnectionsBetweenRooms < 0)
                messages.AddError("MaxConnectionsBetweenRooms must be an integer number equal to or higher than 0.");
            if (!floorJson.OddsForExtraConnections.Between(0, 100))
                messages.AddError("OddsForExtraConnections must be an integer number between 0 and 100.");
            if (floorJson.MaxConnectionsBetweenRooms > 1 && floorJson.OddsForExtraConnections == 0)
                messages.AddWarning("MaxConnectionsBetweenRooms is higher than 1 but OddsForExtraConnections is 0. No extra connections will be generated.");
            else if (floorJson.MaxConnectionsBetweenRooms == 1 && floorJson.OddsForExtraConnections > 0)
                messages.AddWarning("MaxConnectionsBetweenRooms is 1 but OddsForExtraConnections is higher than 0. No extra connections will be generated.");
            else if (floorJson.MaxConnectionsBetweenRooms > 0 && floorJson.PossibleGeneratorAlgorithms.Any(pga => pga.Name == "OneBigRoom"))
                messages.AddWarning("MaxConnectionsBetweenRooms is higher than 0 but OneBigRoom is a possible generator. No extra connections will be generated for OneBigRoom.");
            else if (floorJson.OddsForExtraConnections > 0 && floorJson.PossibleGeneratorAlgorithms.Any(pga => pga.Name == "OneBigRoom"))
                messages.AddWarning("OddsForExtraConnections is higher than 0 but OneBigRoom is a possible generator. No extra connections will be generated for OneBigRoom.");

            if (!floorJson.RoomFusionOdds.Between(0, 100))
                messages.AddError("RoomFusionOdds must be an integer number between 0 and 100.");
            else if (floorJson.RoomFusionOdds.Between(1, 100) && floorJson.PossibleGeneratorAlgorithms.Any(pga => pga.Name == "OneBigRoom"))
                messages.AddWarning("RoomFusionOdds is higher than 0 but OneBigRoom is a possible generator. No rooms will be fused for OneBigRoom.");

            foreach (var possibleGeneratorAlgorithm in floorJson.PossibleGeneratorAlgorithms)
            {
                if (possibleGeneratorAlgorithm.Rows <= 0)
                    messages.AddError($"{possibleGeneratorAlgorithm.Name}'s Rows must be an integer number higher than 0.");
                else if (possibleGeneratorAlgorithm.Rows > 1 && possibleGeneratorAlgorithm.Name == "OneBigRoom")
                    messages.AddWarning($"{possibleGeneratorAlgorithm.Name}'s Rows is higher than 1. It will be ignored.");
                if (possibleGeneratorAlgorithm.Columns <= 0)
                    messages.AddError($"{possibleGeneratorAlgorithm.Name}'s Columns must be an integer number higher than 0.");
                else if (possibleGeneratorAlgorithm.Columns > 1 && possibleGeneratorAlgorithm.Name == "OneBigRoom")
                    messages.AddWarning($"{possibleGeneratorAlgorithm.Name}'s Columns is higher than 1. It will be ignored.");
                if (possibleGeneratorAlgorithm.Name != "OneBigRoom")
                {
                    if (5 * possibleGeneratorAlgorithm.Rows > floorJson.Height)
                        messages.AddError($"With a Floor Height of {floorJson.Height}, it's not possible to create {possibleGeneratorAlgorithm.Rows} non-Dummy rooms with the minimum 5 Height. Change Height or Rows.");
                    if (5 * possibleGeneratorAlgorithm.Columns > floorJson.Width)
                        messages.AddError($"With a Floor Width of {floorJson.Width}, it's not possible to create {possibleGeneratorAlgorithm.Columns} non-Dummy rooms with the minimum 5 Width. Change Width or Columns.");
                }
                else
                {
                    if (floorJson.Height < 5)
                        messages.AddError($"With a Floor Height of {floorJson.Height}, it's not possible to create a {possibleGeneratorAlgorithm.Name} floor. Height must be at least 5.");
                    if (floorJson.Width < 5)
                        messages.AddError($"With a Floor Width of {floorJson.Width}, it's not possible to create a {possibleGeneratorAlgorithm.Name} floor. Width must be at least 5.");
                }
            }

            foreach (var onFloorStartAction in floorJson.OnFloorStartActions.ConvertAll(ofsa => new ActionWithEffects(ofsa)))
            {
                messages.AddRange(ActionValidator.Validate(onFloorStartAction, dungeonJson, sampleDungeon));
            }

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}
