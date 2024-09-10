using RogueCustomsDungeonEditor.Controls;
using RogueCustomsDungeonEditor.Utils;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
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

            if(!dungeonJson.TileSetInfos.Exists(tsi => tsi.Id.Equals(floorJson.TileSetId)))
                messages.AddError($"{floorJson.TileSetId} is not a recognized TileSet.");

            if (floorJson.PossibleMonsters.Any())
            {
                var totalChanceForPossibleMonsters = 0;
                foreach (var possibleMonster in floorJson.PossibleMonsters)
                {
                    if(!dungeonJson.NPCs.Exists(c => c.Id.Equals(possibleMonster.ClassId)))
                        messages.AddError($"{possibleMonster.ClassId} is in the PossibleMonsters list but it's not an NPC.");
                    if (floorJson.PossibleMonsters.Count(pm => pm.ClassId.Equals(possibleMonster.ClassId) && pm.MinLevel == possibleMonster.MinLevel
                                                        && pm.MaxLevel == possibleMonster.MaxLevel && pm.CanSpawnOnFirstTurn == possibleMonster.CanSpawnOnFirstTurn
                                                        && pm.CanSpawnAfterFirstTurn == possibleMonster.CanSpawnAfterFirstTurn) > 1)
                    {
                        messages.AddError($"{possibleMonster.ClassId} shows up as a duplicate PossibleMonster in the current Floor Type.");
                    }

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
                    if (!dungeonJson.Items.Exists(c => (c.EntityType == "Weapon" || c.EntityType == "Armor" || c.EntityType == "Consumable") && c.Id.Equals(possibleItem.ClassId)))
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
                    if (!dungeonJson.Traps.Exists(c => c.Id.Equals(possibleTrap.ClassId)))
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

            if (!floorJson.RoomFusionOdds.Between(0, 100))
                messages.AddError("RoomFusionOdds must be an integer number between 0 and 100.");

            var roomTypesToNotCount = new List<RoomDispositionType> { RoomDispositionType.NoRoom, RoomDispositionType.NoConnection, RoomDispositionType.ConnectionImpossible };

            foreach (var floorLayoutGenerator in floorJson.PossibleLayouts)
            {
                if (floorLayoutGenerator.Rows <= 0)
                    messages.AddError($"{floorLayoutGenerator.Name}'s Rows must be an integer number higher than 0.");
                if (floorLayoutGenerator.Columns <= 0)
                    messages.AddError($"{floorLayoutGenerator.Name}'s Columns must be an integer number higher than 0.");
                if (floorLayoutGenerator.Rows > 1 || floorLayoutGenerator.Columns > 1)
                {
                    if (5 * floorLayoutGenerator.Rows > floorJson.Height)
                        messages.AddError($"{floorLayoutGenerator.Name}: With a Floor Height of {floorJson.Height}, it's not possible to create {floorLayoutGenerator.Rows} non-Dummy rooms with the minimum 5 Height. Change Height or Rows.");
                    if (5 * floorLayoutGenerator.Columns > floorJson.Width)
                        messages.AddError($"{floorLayoutGenerator.Name}: With a Floor Width of {floorJson.Width}, it's not possible to create {floorLayoutGenerator.Columns} non-Dummy rooms with the minimum 5 Width. Change Width or Columns.");
                }
                else
                {
                    if (floorJson.Height < 5)
                        messages.AddError($"{floorLayoutGenerator.Name}: With a Floor Height of {floorJson.Height}, it's not possible to create a {floorLayoutGenerator.Name} floor. Height must be at least 5.");
                    if (floorJson.Width < 5)
                        messages.AddError($"{floorLayoutGenerator.Name}: With a Floor Width of {floorJson.Width}, it's not possible to create a {floorLayoutGenerator.Name} floor. Width must be at least 5.");
                }
                var expandedColumns = floorLayoutGenerator.Columns * 2 - 1;
                var expandedRows = floorLayoutGenerator.Rows * 2 - 1;
                var roomDispositionMatrix = new RoomDispositionType[expandedRows, expandedColumns];
                for (int i = 0; i < floorLayoutGenerator.RoomDisposition.Length; i++)
                {
                    var tile = floorLayoutGenerator.RoomDisposition[i];
                    (int X, int Y) = (i / expandedColumns, i % expandedColumns);
                    var isHallwayTile = (X % 2 != 0 && Y % 2 == 0) || (X % 2 == 0 && Y % 2 != 0);
                    roomDispositionMatrix[X, Y] = tile.ToRoomDispositionIndicator(isHallwayTile);
                }

                var roomTilesToCount = roomDispositionMatrix.Where(rdt => !roomTypesToNotCount.Contains(rdt));
                var guaranteedFuseTiles = roomDispositionMatrix.Where(rdt => rdt == RoomDispositionType.GuaranteedFusion);
                var normalRoomTiles = roomDispositionMatrix.Where(rdt => rdt == RoomDispositionType.GuaranteedRoom || rdt == RoomDispositionType.RandomRoom);
                var guaranteedRoomTiles = roomDispositionMatrix.Where(rdt => rdt == RoomDispositionType.GuaranteedRoom);

                var validationErrors = new List<string>();

                if (!guaranteedRoomTiles.Any() && normalRoomTiles.Count == 1)
                    messages.AddError($"{floorLayoutGenerator.Name}: When making a Single-Room layout, the room must be guaranteed.");
                if ((normalRoomTiles.Count - guaranteedFuseTiles.Count) <= 1 && roomTilesToCount.Count > 1)
                    messages.AddError($"{floorLayoutGenerator.Name}: When making a layout that is not Single-Room layout, at least two normal, non-fused rooms must be possible.");
                if (normalRoomTiles.Count < 1)
                    messages.AddError($"{floorLayoutGenerator.Name}: At least one non-Guaranteed Dummy Room is required.");
                if (!RoomsHaveNoMoreThanOneFusion(roomDispositionMatrix))
                    messages.AddError($"{floorLayoutGenerator.Name}: At least one Room is guaranteed more than one Fusion, which is not allowed.");
                if (!ConnectionsHaveBothEndsCovered(roomDispositionMatrix))
                    messages.AddError($"{floorLayoutGenerator.Name}: At least one possible Connection is missing one of its ends.");
            }

            var floorAsInstance = new Map(sampleDungeon, floorJson.MinFloorLevel, new());
            floorAsInstance.GenerateDebugMap();

            if (floorJson.OnFloorStart != null)
            {
                messages.AddRange(ActionValidator.Validate(floorAsInstance.FloorConfigurationToUse.OnFloorStart, dungeonJson, sampleDungeon));
            }

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
        private static bool RoomsHaveNoMoreThanOneFusion(RoomDispositionType[,] roomDispositionMatrix)
        {
            var validRoomTileTypes = new List<RoomDispositionType>() { RoomDispositionType.GuaranteedRoom, RoomDispositionType.GuaranteedDummyRoom, RoomDispositionType.RandomRoom };
            for (int i = 0; i < roomDispositionMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < roomDispositionMatrix.GetLength(1); j++)
                {
                    var tile = roomDispositionMatrix[i, j];
                    if (!validRoomTileTypes.Contains(tile)) continue;
                    RoomDispositionType? upHallway = null, downHallway = null, leftHallway = null, rightHallway = null;
                    try
                    {
                        upHallway = roomDispositionMatrix[i, j - 1];
                    }
                    catch { }
                    try
                    {
                        downHallway = roomDispositionMatrix[i, j + 1];
                    }
                    catch { }
                    try
                    {
                        leftHallway = roomDispositionMatrix[i - 1, j];
                    }
                    catch { }
                    try
                    {
                        rightHallway = roomDispositionMatrix[i + 1, j];
                    }
                    catch { }
                    var hallwayList = new List<RoomDispositionType?> { upHallway, downHallway, leftHallway, rightHallway };
                    if (hallwayList.Count(rdt => rdt != null && rdt == RoomDispositionType.GuaranteedFusion) > 1)
                        return false;
                }
            }
            return true;
        }

        private static bool ConnectionsHaveBothEndsCovered(RoomDispositionType[,] roomDispositionMatrix)
        {
            var validHallwayTileTypes = new List<RoomDispositionType>() { RoomDispositionType.GuaranteedFusion, RoomDispositionType.GuaranteedHallway, RoomDispositionType.RandomConnection };
            var invalidEndTileTypes = new List<RoomDispositionType>() { RoomDispositionType.NoRoom, RoomDispositionType.ConnectionImpossible };
            for (int i = 0; i < roomDispositionMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < roomDispositionMatrix.GetLength(1); j++)
                {
                    var tile = roomDispositionMatrix[i, j];
                    if (!validHallwayTileTypes.Contains(tile)) continue;
                    RoomDispositionType? upRoom = null, downRoom = null, leftRoom = null, rightRoom = null;
                    try
                    {
                        upRoom = roomDispositionMatrix[i, j - 1];
                    }
                    catch { }
                    try
                    {
                        downRoom = roomDispositionMatrix[i, j + 1];
                    }
                    catch { }
                    try
                    {
                        leftRoom = roomDispositionMatrix[i - 1, j];
                    }
                    catch { }
                    try
                    {
                        rightRoom = roomDispositionMatrix[i + 1, j];
                    }
                    catch { }
                    var isVerticalConnection = j % 2 != 0 && i % 2 == 0;
                    var isHorizontalConnection = j % 2 == 0 && i % 2 != 0;
                    if (isVerticalConnection && ((upRoom == null || downRoom == null) || (invalidEndTileTypes.Contains(upRoom.Value) || invalidEndTileTypes.Contains(downRoom.Value))))
                        return false;
                    else if (isHorizontalConnection && ((leftRoom == null || rightRoom == null) || (invalidEndTileTypes.Contains(leftRoom.Value) || invalidEndTileTypes.Contains(rightRoom.Value))))
                        return false;
                }
            }
            return true;
        }
    }
}
