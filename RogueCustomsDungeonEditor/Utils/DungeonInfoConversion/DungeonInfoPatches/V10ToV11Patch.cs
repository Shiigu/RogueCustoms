using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.DungeonInfoPatches.Interfaces;

using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.DungeonInfoPatches
{
    public class V10ToV11Patch : IDungeonInfoPatcher
    {
        public static void Apply(JsonObject root)
        {
            AddTileSetInfos(root);
            AddTileSetIdToFloorInfos(root);
            RenameColorParamInReplaceConsoleRepresentation(root);
            root["Version"] = "1.1";
        }

        private static void AddTileSetInfos(JsonObject root)
        {
            root["TileSetInfos"] = JsonSerializer.SerializeToNode(new[]
            {
                new {
                    Id = "Default",
                    TopLeftWall = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(Color.FromName("Blue")), Character = '█' },
                    TopRightWall = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(Color.FromName("Blue")), Character = '█' },
                    BottomLeftWall = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(Color.FromName("Blue")), Character = '█' },
                    BottomRightWall = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(Color.FromName("Blue")), Character = '█' },
                    HorizontalWall = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(Color.FromName("Blue")), Character = '█' },
                    VerticalWall = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(Color.FromName("Blue")), Character = '█' },
                    ConnectorWall = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(Color.FromName("Blue")), Character = '▒' },
                    TopLeftHallway = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(Color.FromName("Blue")), Character = '▒' },
                    TopRightHallway = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(Color.FromName("Blue")), Character = '▒' },
                    BottomLeftHallway = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(Color.FromName("Blue")), Character = '▒' },
                    BottomRightHallway = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(Color.FromName("Blue")), Character = '▒' },
                    HorizontalHallway = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(Color.FromName("Blue")), Character = '▒' },
                    VerticalHallway = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(Color.FromName("Blue")), Character = '▒' },
                    HorizontalTopHallway = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(Color.FromName("Blue")), Character = '▒' },
                    HorizontalBottomHallway = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(Color.FromName("Blue")), Character = '▒' },
                    VerticalLeftHallway = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(Color.FromName("Blue")), Character = '▒' },
                    VerticalRightHallway = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(Color.FromName("Blue")), Character = '▒' },
                    CentralHallway = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(Color.FromName("Blue")), Character = '▒' },
                    Floor = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(Color.FromName("DarkGray")), Character = '.' },
                    Stairs = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Yellow")), ForegroundColor = new GameColor(Color.FromName("DarkGreen")), Character = '>' },
                    Empty = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(Color.FromName("Black")), Character = ' ' }
                },
                new {
                    Id = "Retro",
                    TopLeftWall = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(ColorTranslator.FromHtml("#A85400")), Character = '╔' },
                    TopRightWall = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(ColorTranslator.FromHtml("#A85400")), Character = '╗' },
                    BottomLeftWall = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(ColorTranslator.FromHtml("#A85400")), Character = '╚' },
                    BottomRightWall = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(ColorTranslator.FromHtml("#A85400")), Character = '╝' },
                    HorizontalWall = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(ColorTranslator.FromHtml("#A85400")), Character = '═' },
                    VerticalWall = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(ColorTranslator.FromHtml("#A85400")), Character = '║' },
                    ConnectorWall = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(ColorTranslator.FromHtml("#A85400")), Character = '╬' },
                    TopLeftHallway = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(ColorTranslator.FromHtml("#A8A8A8")), Character = '▒' },
                    TopRightHallway = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(ColorTranslator.FromHtml("#A8A8A8")), Character = '▒' },
                    BottomLeftHallway = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(ColorTranslator.FromHtml("#A8A8A8")), Character = '▒' },
                    BottomRightHallway = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(ColorTranslator.FromHtml("#A8A8A8")), Character = '▒' },
                    HorizontalHallway = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(ColorTranslator.FromHtml("#A8A8A8")), Character = '▒' },
                    VerticalHallway = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(ColorTranslator.FromHtml("#A8A8A8")), Character = '▒' },
                    HorizontalTopHallway = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(ColorTranslator.FromHtml("#A8A8A8")), Character = '▒' },
                    HorizontalBottomHallway = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(ColorTranslator.FromHtml("#A8A8A8")), Character = '▒' },
                    VerticalLeftHallway = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(ColorTranslator.FromHtml("#A8A8A8")), Character = '▒' },
                    VerticalRightHallway = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(ColorTranslator.FromHtml("#A8A8A8")), Character = '▒' },
                    CentralHallway = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(ColorTranslator.FromHtml("#A8A8A8")), Character = '▒' },
                    Floor = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(ColorTranslator.FromHtml("#54FC54")), Character = '.' },
                    Stairs = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(ColorTranslator.FromHtml("#54FC54")), Character = '╫' },
                    Empty = new ConsoleRepresentation { BackgroundColor = new GameColor(Color.FromName("Black")), ForegroundColor = new GameColor(Color.FromName("Black")), Character = ' ' }
                }
            });
        }

        private static void AddTileSetIdToFloorInfos(JsonObject root)
        {
            if (root["FloorInfos"] is not JsonArray floorInfos) return;

            foreach (var floorNode in floorInfos)
            {
                if (floorNode is not JsonObject floorObj) continue;
                floorObj["TileSetId"] = "Default";
            }
        }

        private static void RenameColorParamInReplaceConsoleRepresentation(JsonObject root)
        {
            foreach (var section in new[] { "FloorInfos", "PlayerClasses", "NPCs", "Items", "Traps", "AlteredStatuses" })
            {
                if (root[section] is not JsonArray sectionArray) continue;

                foreach (var entity in sectionArray.OfType<JsonObject>())
                {
                    foreach (var prop in entity.Where(p => p.Value is JsonArray).ToArray())
                    {
                        if (prop.Value is not JsonArray actionArray) continue;

                        foreach (var action in actionArray.OfType<JsonObject>())
                        {
                            RenameColorParamInEffect(action["Effect"] as JsonObject);
                        }
                    }
                }
            }
        }

        private static void RenameColorParamInEffect(JsonObject effect)
        {
            if (effect == null) return;

            if (effect.TryGetPropertyValue("EffectName", out var effectNameNode)
                && effectNameNode?.ToString() == "ReplaceConsoleRepresentation")
            {
                if (effect["Params"] is JsonArray paramArray)
                {
                    foreach (var param in paramArray.OfType<JsonObject>())
                    {
                        if (param["ParamName"]?.ToString().Equals("Color", StringComparison.OrdinalIgnoreCase) == true)
                        {
                            param["ParamName"] = "ForeColor";
                        }
                    }
                }
            }

            RenameColorParamInEffect(effect["Then"] as JsonObject);
            RenameColorParamInEffect(effect["OnSuccess"] as JsonObject);
            RenameColorParamInEffect(effect["OnFailure"] as JsonObject);
        }

        private static object Make(string bg, string fg, string ch)
        {
            return new
            {
                BackgroundColor = bg,
                ForegroundColor = fg,
                Character = ch
            };
        }
    }
}
