using RogueCustomsDungeonEditor.Utils;
using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace RogueCustomsDungeonEditor.Validators.IndividualValidators
{
    public class DungeonTileTypeValidator
    {
        public static async Task<DungeonValidationMessages> Validate(TileTypeInfo tileType, DungeonInfo dungeonJson, Dungeon sampleDungeon)
        {
            var messages = new DungeonValidationMessages();

            if (!FormConstants.DefaultTileTypes.Any(dtt => dtt.Equals(tileType.Id, StringComparison.InvariantCultureIgnoreCase)))
            {
                messages.AddRange(dungeonJson.ValidateString(tileType.Name, "Tile Type", "Name", true));
                messages.AddRange(dungeonJson.ValidateString(tileType.Description, "Tile Type", "Description", false));
            }

            if(tileType.IsWalkable && tileType.IsSolid)
                messages.AddWarning($"{tileType.Id} is set as IsWalkable and as IsSolid. The latter will be ignored.");
            if(tileType.IsWalkable && !tileType.IsVisible)
                messages.AddError($"{tileType.Id} is set as IsWalkable but not as IsVisible.");
            if(!tileType.CanVisiblyConnectWithOtherTiles && tileType.CanHaveMultilineConnections)
                messages.AddWarning($"{tileType.Id} is set to connect multiple rows of Tiles but is not set to visibly connect Tiles.");

            if (tileType.OnStood != null)
            {
                var actionInstance = ActionWithEffects.Create(tileType.OnStood);
                messages.AddRange(await ActionValidator.Validate(actionInstance, dungeonJson, sampleDungeon));
            }

            if (!messages.Any()) messages.AddSuccess("ALL OK!");

            return messages;
        }
    }
}
