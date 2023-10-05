using RogueCustomsDungeonEditor.Utils;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
{
    public class DungeonTilesetValidator
    {

        public static DungeonValidationMessages Validate(TileSetInfo tileSet, DungeonInfo dungeonJson)
        {
            var messages = new DungeonValidationMessages();

            foreach (var errorMessage in tileSet.TopLeftWall.Validate($"{tileSet.Id ?? "NULL"}'s Top Left Wall"))
            {
                messages.AddError(errorMessage);
            }
            foreach (var errorMessage in tileSet.TopRightWall.Validate($"{tileSet.Id ?? "NULL"}'s Top Right Wall"))
            {
                messages.AddError(errorMessage);
            }
            foreach (var errorMessage in tileSet.BottomLeftWall.Validate($"{tileSet.Id ?? "NULL"}'s Bottom Left Wall"))
            {
                messages.AddError(errorMessage);
            }
            foreach (var errorMessage in tileSet.BottomRightWall.Validate($"{tileSet.Id ?? "NULL"}'s Bottom Right Wall"))
            {
                messages.AddError(errorMessage);
            }
            foreach (var errorMessage in tileSet.HorizontalWall.Validate($"{tileSet.Id ?? "NULL"}'s Horizontal Wall"))
            {
                messages.AddError(errorMessage);
            }
            foreach (var errorMessage in tileSet.ConnectorWall.Validate($"{tileSet.Id ?? "NULL"}'s Connector Wall"))
            {
                messages.AddError(errorMessage);
            }
            foreach (var errorMessage in tileSet.VerticalWall.Validate($"{tileSet.Id ?? "NULL"}'s Vertical Wall"))
            {
                messages.AddError(errorMessage);
            }
            foreach (var errorMessage in tileSet.TopLeftHallway.Validate($"{tileSet.Id ?? "NULL"}'s Top Left Hallway"))
            {
                messages.AddError(errorMessage);
            }
            foreach (var errorMessage in tileSet.TopRightHallway.Validate($"{tileSet.Id ?? "NULL"}'s Top Right Hallway"))
            {
                messages.AddError(errorMessage);
            }
            foreach (var errorMessage in tileSet.BottomLeftHallway.Validate($"{tileSet.Id ?? "NULL"}'s Bottom Left Hallway"))
            {
                messages.AddError(errorMessage);
            }
            foreach (var errorMessage in tileSet.BottomRightHallway.Validate($"{tileSet.Id ?? "NULL"}'s Bottom Right Hallway"))
            {
                messages.AddError(errorMessage);
            }
            foreach (var errorMessage in tileSet.HorizontalHallway.Validate($"{tileSet.Id ?? "NULL"}'s Horizontal Hallway"))
            {
                messages.AddError(errorMessage);
            }
            foreach (var errorMessage in tileSet.HorizontalBottomHallway.Validate($"{tileSet.Id ?? "NULL"}'s Horizontal Bottom Hallway"))
            {
                messages.AddError(errorMessage);
            }
            foreach (var errorMessage in tileSet.HorizontalTopHallway.Validate($"{tileSet.Id ?? "NULL"}'s Horizontal Top Hallway"))
            {
                messages.AddError(errorMessage);
            }
            foreach (var errorMessage in tileSet.VerticalHallway.Validate($"{tileSet.Id ?? "NULL"}'s Vertical Hallway"))
            {
                messages.AddError(errorMessage);
            }
            foreach (var errorMessage in tileSet.VerticalLeftHallway.Validate($"{tileSet.Id ?? "NULL"}'s Vertical Left Hallway"))
            {
                messages.AddError(errorMessage);
            }
            foreach (var errorMessage in tileSet.VerticalRightHallway.Validate($"{tileSet.Id ?? "NULL"}'s Vertical Right Hallway"))
            {
                messages.AddError(errorMessage);
            }
            foreach (var errorMessage in tileSet.CentralHallway.Validate($"{tileSet.Id ?? "NULL"}'s Central Hallway"))
            {
                messages.AddError(errorMessage);
            }
            foreach (var errorMessage in tileSet.Floor.Validate($"{tileSet.Id ?? "NULL"}'s Floor"))
            {
                messages.AddError(errorMessage);
            }
            foreach (var errorMessage in tileSet.Stairs.Validate($"{tileSet.Id ?? "NULL"}'s Stairs"))
            {
                messages.AddError(errorMessage);
            }
            foreach (var errorMessage in tileSet.Empty.Validate($"{tileSet.Id ?? "NULL"}'s Empty (inaccessible)"))
            {
                messages.AddError(errorMessage);
            }

            if(!dungeonJson.FloorInfos.Any(fi => fi.TileSetId.Equals(tileSet.Id)))
                messages.AddWarning($"{tileSet.Id} is not set as Tileset in any Floor Group. Check if this is valid.");

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}
