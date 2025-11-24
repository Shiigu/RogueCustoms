using RogueCustomsDungeonEditor.Utils;

using RogueCustomsGameEngine.Utils.JsonImports;

using System;
using System.Linq;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
{
    public class DungeonTilesetValidator
    {
        public static DungeonValidationMessages Validate(TileSetInfo tileSet, DungeonInfo dungeonJson)
        {
            var messages = new DungeonValidationMessages();

            foreach (var tileTypeSet in tileSet.TileTypes)
            {
                var tileType = dungeonJson.TileTypeInfos.Find(tti => tti.Id.Equals(tileTypeSet.TileTypeId));

                if(tileType == null)
                {
                    messages.AddError($"{tileTypeSet.TileTypeId ?? "NULL"} is not a valid Tile Type.");
                    continue;
                }

                foreach (var errorMessage in tileTypeSet.Central.Validate($"{tileTypeSet.TileTypeId ?? "NULL"}'s Central/Default"))
                {
                    messages.AddError(errorMessage);
                }

                if (tileType.Id.Equals("hallway", StringComparison.InvariantCultureIgnoreCase))
                {
                    foreach (var errorMessage in tileTypeSet.Connector.Validate($"{tileTypeSet.TileTypeId ?? "NULL"}'s Connector"))
                    {
                        messages.AddError(errorMessage);
                    }
                }

                if (tileType.CanVisiblyConnectWithOtherTiles)
                {
                    foreach (var errorMessage in tileTypeSet.TopLeft.Validate($"{tileTypeSet.TileTypeId ?? "NULL"}'s Top Left"))
                    {
                        messages.AddError(errorMessage);
                    }
                    foreach (var errorMessage in tileTypeSet.TopRight.Validate($"{tileTypeSet.TileTypeId ?? "NULL"}'s Top Right"))
                    {
                        messages.AddError(errorMessage);
                    }
                    foreach (var errorMessage in tileTypeSet.BottomLeft.Validate($"{tileTypeSet.TileTypeId ?? "NULL"}'s Bottom Left"))
                    {
                        messages.AddError(errorMessage);
                    }
                    foreach (var errorMessage in tileTypeSet.BottomRight.Validate($"{tileTypeSet.TileTypeId ?? "NULL"}'s Bottom Right"))
                    {
                        messages.AddError(errorMessage);
                    }
                    foreach (var errorMessage in tileTypeSet.Horizontal.Validate($"{tileTypeSet.TileTypeId ?? "NULL"}'s Horizontal"))
                    {
                        messages.AddError(errorMessage);
                    }
                    foreach (var errorMessage in tileTypeSet.Vertical.Validate($"{tileTypeSet.TileTypeId ?? "NULL"}'s Vertical"))
                    {
                        messages.AddError(errorMessage);
                    }
                }

                if (tileType.CanVisiblyConnectWithOtherTiles && tileType.CanHaveMultilineConnections)
                {
                    foreach (var errorMessage in tileTypeSet.HorizontalBottom.Validate($"{tileTypeSet.TileTypeId ?? "NULL"}'s Horizontal Bottom"))
                    {
                        messages.AddError(errorMessage);
                    }
                    foreach (var errorMessage in tileTypeSet.HorizontalTop.Validate($"{tileTypeSet.TileTypeId ?? "NULL"}'s Horizontal Top"))
                    {
                        messages.AddError(errorMessage);
                    }
                    foreach (var errorMessage in tileTypeSet.VerticalLeft.Validate($"{tileTypeSet.TileTypeId ?? "NULL"}'s Vertical Left"))
                    {
                        messages.AddError(errorMessage);
                    }
                    foreach (var errorMessage in tileTypeSet.VerticalRight.Validate($"{tileTypeSet.TileTypeId ?? "NULL"}'s Vertical Right"))
                    {
                        messages.AddError(errorMessage);
                    }
                }
            }

            foreach (var tileTypeId in dungeonJson.TileTypeInfos.Select(tt => tt.Id).Except(tileSet.TileTypes.Select(tst => tst.TileTypeId)))
            {
                messages.AddError($"{tileTypeId} is not present in the Tileset.");
            }

            if(!dungeonJson.FloorInfos.Exists(fi => fi.TileSetId.Equals(tileSet.Id)))
                messages.AddWarning($"{tileSet.Id} is not set as Tileset in any Floor Group. Check if this is valid.");

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}
