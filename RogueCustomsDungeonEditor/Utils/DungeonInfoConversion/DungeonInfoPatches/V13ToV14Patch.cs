using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.DungeonInfoPatches.Interfaces;

using RogueCustomsGameEngine.Utils.Enums;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.DungeonInfoPatches
{
    public class V13ToV14Patch : IDungeonInfoPatcher
    {
        public static void Apply(JsonObject root)
        {
            UpdateFloorInfos(root);
            UpdateNPCs(root);

            foreach (var entityListName in new[]
            {
                "PlayerClasses", "NPCs", "Items", "Traps", "AlteredStatuses"
            })
            {
                if (root[entityListName] is not JsonArray entities) continue;

                foreach (var entity in entities.OfType<JsonObject>())
                {
                    RenameActionsProperties(entity);
                    foreach (var kvp in entity.ToList())
                    {
                        if (kvp.Value is JsonArray actionArray)
                        {
                            foreach (var action in actionArray.OfType<JsonObject>())
                            {
                                UpdateDealDamageStepsToV14(action);
                            }
                        }
                        else if (kvp.Value is JsonObject singleAction)
                        {
                            UpdateDealDamageStepsToV14(singleAction);
                        }
                    }
                }
            }

            root["Version"] = "1.4";
        }

        private static void RenameActionsProperties(JsonObject obj)
        {
            var keysToRename = obj
                .Where(kvp => kvp.Key.EndsWith("Actions"))
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var oldKey in keysToRename)
            {
                var value = obj[oldKey];
                var newKey = oldKey[..^"Actions".Length];

                if (newKey == "OnStatusApply")
                    newKey = "OnApply";

                obj.Remove(oldKey);
                var arr = value as JsonArray;

                if(newKey == "OnItemStepped" && arr != null)
                {
                    if (obj["EntityType"] == null)
                    {
                        var firstItemJson = arr[0].ToJsonString();
                        obj["OnStepped"] = JsonNode.Parse(firstItemJson);
                    }
                }
                else if (newKey != "OnAttack" && newKey != "OnInteracted" && arr != null)
                {
                    if (arr.Count > 0)
                    {
                        var firstItemJson = arr[0].ToJsonString();
                        obj[newKey] = JsonNode.Parse(firstItemJson);
                    }
                    else
                    {
                        obj[newKey] = null;
                    }
                }
                else
                {
                    obj[newKey] = value?.DeepClone();
                }
            }

            foreach (var kvp in obj)
            {
                if (kvp.Value is JsonObject childObj)
                    RenameActionsProperties(childObj);
                else if (kvp.Value is JsonArray arr)
                {
                    foreach (var item in arr.OfType<JsonObject>())
                        RenameActionsProperties(item);
                }
            }
        }

        private static void UpdateFloorInfos(JsonObject root)
        {
            if (root["FloorInfos"] is not JsonArray floorInfos) return;

            foreach (var floorInfoNode in floorInfos.OfType<JsonObject>())
            {
                floorInfoNode["PossibleKeys"] = new JsonObject
                {
                    ["LockedRoomOdds"] = 0,
                    ["KeySpawnInEnemyInventoryOdds"] = 0,
                    ["MaxPercentageOfLockedCandidateRooms"] = 0,
                    ["KeyTypes"] = new JsonArray()
                };

                floorInfoNode["PossibleLayouts"] = new JsonArray();

                if (floorInfoNode["PossibleGeneratorAlgorithms"] is not JsonArray genAlgorithms) continue;

                int width = floorInfoNode["Width"]?.GetValue<int>() ?? 0;
                int height = floorInfoNode["Height"]?.GetValue<int>() ?? 0;

                foreach (var genAlgoNode in genAlgorithms.OfType<JsonObject>())
                {
                    string name = genAlgoNode["Name"]?.ToString() ?? "";

                    JsonObject layout = name switch
                    {
                        "Standard" => ConstructFullRandom(genAlgoNode, width, height),
                        "OuterDummyRing" => ConstructOuterDummyRing(genAlgoNode, width, height),
                        "InnerDummyRing" => ConstructInnerDummyRing(genAlgoNode, width, height),
                        "OneBigRoom" => ConstructOneBigRoom(genAlgoNode, width, height),
                        _ => null
                    };

                    if (layout != null)
                        ((JsonArray)floorInfoNode["PossibleLayouts"]).Add(layout);
                }
            }
        }

        private static JsonObject ConstructFullRandom(JsonObject generatorToConvert, int width, int height)
        {
            int cols = generatorToConvert["Columns"]?.GetValue<int>() ?? 1;
            int rows = generatorToConvert["Rows"]?.GetValue<int>() ?? 1;

            int maxWidth = Math.Max(5, width / cols);
            int maxHeight = Math.Max(5, height / rows);
            int layoutRows = rows + rows - 1;
            int layoutCols = cols + cols - 1;

            var sb = new StringBuilder();

            for (int y = 0; y < layoutRows; y++)
            {
                for (int x = 0; x < layoutCols; x++)
                {
                    if (x % 2 == 0 && y % 2 == 0)
                        sb.Append(RoomDispositionTypeToChar("RandomRoom"));
                    else if ((x % 2 != 0 && y % 2 == 0) || (x % 2 == 0 && y % 2 != 0))
                        sb.Append(RoomDispositionTypeToChar("RandomConnection"));
                    else
                        sb.Append(RoomDispositionTypeToChar("ConnectionImpossible"));
                }
            }

            return new JsonObject
            {
                ["Name"] = $"{generatorToConvert["Name"]} - {cols}c x {rows}r",
                ["Rows"] = rows,
                ["Columns"] = cols,
                ["MinRoomSize"] = new JsonObject
                {
                    ["Width"] = 5,
                    ["Height"] = 5
                },
                ["MaxRoomSize"] = new JsonObject
                {
                    ["Width"] = maxWidth,
                    ["Height"] = maxHeight
                },
                ["RoomDisposition"] = sb.ToString()
            };
        }

        private static JsonObject ConstructOuterDummyRing(JsonObject generatorToConvert, int width, int height)
        {
            int cols = generatorToConvert["Columns"]?.GetValue<int>() ?? 1;
            int rows = generatorToConvert["Rows"]?.GetValue<int>() ?? 1;

            int maxWidth = Math.Max(5, width / cols);
            int maxHeight = Math.Max(5, height / rows);
            int layoutRows = rows + rows - 1;
            int layoutCols = cols + cols - 1;

            var sb = new StringBuilder();

            for (int y = 0; y < layoutRows; y++)
            {
                for (int x = 0; x < layoutCols; x++)
                {
                    int ringNumber = Math.Min(Math.Min(x, layoutCols - 1 - x), Math.Min(y, layoutRows - 1 - y)) / 2;
                    bool isDummyRing = ringNumber % 2 == 0;

                    if (y % 2 == 0)
                    {
                        if (x % 2 == 0)
                        {
                            sb.Append(RoomDispositionTypeToChar(isDummyRing ? "GuaranteedDummyRoom" : "GuaranteedRoom"));
                        }
                        else
                        {
                            sb.Append(RoomDispositionTypeToChar("RandomConnection"));
                        }
                    }
                    else
                    {
                        if (x % 2 == 0)
                            sb.Append(RoomDispositionTypeToChar("RandomConnection"));
                        else
                            sb.Append(RoomDispositionTypeToChar("ConnectionImpossible"));
                    }
                }
            }

            return new JsonObject
            {
                ["Name"] = $"{generatorToConvert["Name"]} - {cols}c x {rows}r",
                ["Rows"] = rows,
                ["Columns"] = cols,
                ["MinRoomSize"] = new JsonObject
                {
                    ["Width"] = 5,
                    ["Height"] = 5
                },
                ["MaxRoomSize"] = new JsonObject
                {
                    ["Width"] = maxWidth,
                    ["Height"] = maxHeight
                },
                ["RoomDisposition"] = sb.ToString()
            };
        }

        private static JsonObject ConstructInnerDummyRing(JsonObject generatorToConvert, int width, int height)
        {
            int cols = generatorToConvert["Columns"]?.GetValue<int>() ?? 1;
            int rows = generatorToConvert["Rows"]?.GetValue<int>() ?? 1;

            int maxWidth = Math.Max(5, width / cols);
            int maxHeight = Math.Max(5, height / rows);
            int layoutRows = rows + rows - 1;
            int layoutCols = cols + cols - 1;

            var sb = new StringBuilder();

            for (int y = 0; y < layoutRows; y++)
            {
                for (int x = 0; x < layoutCols; x++)
                {
                    int ringNumber = Math.Min(Math.Min(x, layoutCols - 1 - x), Math.Min(y, layoutRows - 1 - y)) / 2;
                    bool isDummyRing = ringNumber % 2 != 0;

                    if (y % 2 == 0)
                    {
                        if (x % 2 == 0)
                        {
                            sb.Append(RoomDispositionTypeToChar(isDummyRing ? "GuaranteedDummyRoom" : "GuaranteedRoom"));
                        }
                        else
                        {
                            sb.Append(RoomDispositionTypeToChar("RandomConnection"));
                        }
                    }
                    else
                    {
                        if (x % 2 == 0)
                            sb.Append(RoomDispositionTypeToChar("RandomConnection"));
                        else
                            sb.Append(RoomDispositionTypeToChar("ConnectionImpossible"));
                    }
                }
            }

            return new JsonObject
            {
                ["Name"] = $"{generatorToConvert["Name"]} - {cols}c x {rows}r",
                ["Rows"] = rows,
                ["Columns"] = cols,
                ["MinRoomSize"] = new JsonObject
                {
                    ["Width"] = 5,
                    ["Height"] = 5
                },
                ["MaxRoomSize"] = new JsonObject
                {
                    ["Width"] = maxWidth,
                    ["Height"] = maxHeight
                },
                ["RoomDisposition"] = sb.ToString()
            };
        }

        private static JsonObject ConstructOneBigRoom(JsonObject generatorToConvert, int width, int height)
        {
            int cols = generatorToConvert["Columns"]?.GetValue<int>() ?? 1;
            int rows = generatorToConvert["Rows"]?.GetValue<int>() ?? 1;

            int maxWidth = Math.Max(5, width / cols);
            int maxHeight = Math.Max(5, height / rows);
            int layoutRows = rows + rows - 1;
            int layoutCols = cols + cols - 1;

            var sb = new StringBuilder();

            for (int y = 0; y < layoutRows; y++)
            {
                for (int x = 0; x < layoutCols; x++)
                {
                    if (x == 0 && y == 0)
                        sb.Append(RoomDispositionTypeToChar("GuaranteedRoom"));
                    else if (x % 2 == 0 && y % 2 == 0)
                        sb.Append(RoomDispositionTypeToChar("NoRoom"));
                    else if ((x % 2 != 0 && y % 2 == 0) || (x % 2 == 0 && y % 2 != 0))
                        sb.Append(RoomDispositionTypeToChar("NoConnection"));
                    else
                        sb.Append(RoomDispositionTypeToChar("ConnectionImpossible"));
                }
            }

            return new JsonObject
            {
                ["Name"] = $"{generatorToConvert["Name"]} - {cols}c x {rows}r",
                ["Rows"] = rows,
                ["Columns"] = cols,
                ["MinRoomSize"] = new JsonObject
                {
                    ["Width"] = 5,
                    ["Height"] = 5
                },
                ["MaxRoomSize"] = new JsonObject
                {
                    ["Width"] = maxWidth,
                    ["Height"] = maxHeight
                },
                ["RoomDisposition"] = sb.ToString()
            };
        }

        private static char RoomDispositionTypeToChar(string disposition)
        {
            return Enum.Parse<RoomDispositionType>(disposition).ToChar();
        }

        private static void UpdateNPCs(JsonObject root)
        {
            if (root["NPCs"] is not JsonArray npcs) return;

            foreach (var npc in npcs.OfType<JsonObject>())
            {
                npc["OnInteracted"] = new JsonArray();
                npc["OnSpawn"] = null;
            }
        }

        private static void UpdateDealDamageStepsToV14(JsonObject? actionWithEffects)
        {
            if (actionWithEffects == null) return;

            var effect = actionWithEffects["Effect"] as JsonObject;
            UpdateDealDamageParametersToV14(effect);
        }

        private static void UpdateDealDamageParametersToV14(JsonObject? effect)
        {
            if (effect == null) return;

            if (string.Equals(effect["EffectName"]?.ToString(), "DealDamage", StringComparison.OrdinalIgnoreCase))
            {
                if (effect["Params"] is JsonArray paramArray)
                {
                    var hasCriticalHitChance = paramArray.OfType<JsonObject>()
                        .Any(p => string.Equals(p["ParamName"]?.ToString(), "CriticalHitChance", StringComparison.OrdinalIgnoreCase));
                    var hasCriticalHitFormula = paramArray.OfType<JsonObject>()
                        .Any(p => string.Equals(p["ParamName"]?.ToString(), "CriticalHitFormula", StringComparison.OrdinalIgnoreCase));

                    if (!hasCriticalHitChance)
                    {
                        paramArray.Add(new JsonObject
                        {
                            ["ParamName"] = "CriticalHitChance",
                            ["Value"] = "0"
                        });
                    }

                    if (!hasCriticalHitFormula)
                    {
                        paramArray.Add(new JsonObject
                        {
                            ["ParamName"] = "CriticalHitFormula",
                            ["Value"] = "{CalculatedDamage}"
                        });
                    }
                }

                if (effect["OnFailure"] is JsonObject onFailureEffect)
                {
                    if (string.Equals(onFailureEffect["EffectName"]?.ToString(), "PrintText", StringComparison.OrdinalIgnoreCase)
                        && onFailureEffect["Then"] == null)
                    {
                        effect["OnFailure"] = null;
                    }
                }
            }

            UpdateDealDamageParametersToV14(effect["Then"] as JsonObject);
            UpdateDealDamageParametersToV14(effect["OnSuccess"] as JsonObject);
            UpdateDealDamageParametersToV14(effect["OnFailure"] as JsonObject);
        }
    }
}
