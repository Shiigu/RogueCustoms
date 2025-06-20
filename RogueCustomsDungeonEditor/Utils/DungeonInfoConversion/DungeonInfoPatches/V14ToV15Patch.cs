using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.DungeonInfoPatches.Interfaces;

using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.DungeonInfoPatches
{
    public class V14ToV15Patch : IDungeonInfoPatcher
    {
        public static void Apply(JsonObject root)
        {
            UpdateTileTypeInfos(root);
            UpdateTileSetInfos(root);
            UpdateFloorInfos(root);
            UpdateElementInfos(root);
            UpdateCharacterStats(root);
            UpdatePlayerClasses(root);
            UpdateNPCs(root);
            UpdateItems(root);
            UpdateTraps(root);
            UpdateAlteredStatuses(root);

            root["Scripts"] = new JsonArray();
            root["Version"] = "1.5";
        }

        private static void UpdateTileTypeInfos(JsonObject root)
        {
            var tileTypeInfos = DungeonInfoHelpers.CreateDefaultTileTypes();
            var tileTypeInfosArray = new JsonArray();
            foreach (var tileType in tileTypeInfos)
            {
                tileTypeInfosArray.Add(tileType.ToJsonObject());
            }
            root["TileTypeInfos"] = tileTypeInfosArray;
        }

        private static void UpdateTileSetInfos(JsonObject root)
        {
            if (root["TileSetInfos"] is not JsonArray v14TileSets) return;
            var v15TileSets = new JsonArray();
            foreach (var tileSetNode in v14TileSets.OfType<JsonObject>())
            {
                var v15TileSet = new JsonObject
                {
                    ["Id"] = tileSetNode["Id"]?.DeepClone(),
                    ["TileTypes"] = new JsonArray
                    {
                        new JsonObject
                        {
                            ["TileTypeId"] = "Empty",
                            ["Central"] = tileSetNode["Empty"]?.DeepClone()
                        },
                        new JsonObject
                        {
                            ["TileTypeId"] = "Floor",
                            ["Central"] = tileSetNode["Floor"]?.DeepClone()
                        },
                        new JsonObject
                        {
                            ["TileTypeId"] = "Wall",
                            ["BottomLeft"] = tileSetNode["BottomLeftWall"]?.DeepClone(),
                            ["BottomRight"] = tileSetNode["BottomRightWall"]?.DeepClone(),
                            ["TopLeft"] = tileSetNode["TopLeftWall"]?.DeepClone(),
                            ["TopRight"] = tileSetNode["TopRightWall"]?.DeepClone(),
                            ["Horizontal"] = tileSetNode["HorizontalWall"]?.DeepClone(),
                            ["Vertical"] = tileSetNode["VerticalWall"]?.DeepClone(),
                            ["Connector"] = tileSetNode["ConnectorWall"]?.DeepClone(),
                            ["Central"] = tileSetNode["ConnectorWall"]?.DeepClone()
                        },
                        new JsonObject
                        {
                            ["TileTypeId"] = "Hallway",
                            ["BottomLeft"] = tileSetNode["BottomLeftHallway"]?.DeepClone(),
                            ["BottomRight"] = tileSetNode["BottomRightHallway"]?.DeepClone(),
                            ["TopLeft"] = tileSetNode["TopLeftHallway"]?.DeepClone(),
                            ["TopRight"] = tileSetNode["TopRightHallway"]?.DeepClone(),
                            ["Horizontal"] = tileSetNode["HorizontalHallway"]?.DeepClone(),
                            ["Vertical"] = tileSetNode["VerticalHallway"]?.DeepClone(),
                            ["HorizontalBottom"] = tileSetNode["HorizontalBottomHallway"]?.DeepClone(),
                            ["HorizontalTop"] = tileSetNode["HorizontalTopHallway"]?.DeepClone(),
                            ["VerticalLeft"] = tileSetNode["VerticalLeftHallway"]?.DeepClone(),
                            ["VerticalRight"] = tileSetNode["VerticalRightHallway"]?.DeepClone(),
                            ["Connector"] = tileSetNode["ConnectorWall"]?.DeepClone(),
                            ["Central"] = tileSetNode["CentralHallway"]?.DeepClone()
                        },
                        new JsonObject
                        {
                            ["TileTypeId"] = "Stairs",
                            ["Central"] = tileSetNode["Stairs"]?.DeepClone()
                        }
                    }
                };
                v15TileSets.Add(v15TileSet);
            }
            root["TileSetInfos"] = v15TileSets;
        }

        private static void UpdateFloorInfos(JsonObject root)
        {
            if (root["FloorInfos"] is not JsonArray floorInfos) return;
            foreach (var floorGroup in floorInfos.OfType<JsonObject>())
            {
                floorGroup["MonsterHouseOdds"] = 0;
                foreach (var npc in floorGroup["PossibleMonsters"]?.AsArray() ?? new JsonArray())
                {
                    if (npc is JsonObject npcObj)
                    {
                        npcObj["MinimumInFirstTurn"] = 0;
                        npcObj["SpawnCondition"] = "";
                    }
                }
                foreach (var item in floorGroup["PossibleItems"]?.AsArray() ?? new JsonArray())
                {
                    if (item is JsonObject itemObj)
                    {
                        itemObj["MinimumInFirstTurn"] = 0;
                        itemObj["SpawnCondition"] = "";
                    }
                }
                foreach (var trap in floorGroup["PossibleTraps"]?.AsArray() ?? new JsonArray())
                {
                    if (trap is JsonObject trapObj)
                    {
                        trapObj["MinimumInFirstTurn"] = 0;
                        trapObj["SpawnCondition"] = "";
                    }
                }
                if (floorGroup["OnFloorStart"] is JsonObject onFloorStart)
                    onFloorStart["Id"] = onFloorStart["Name"]?.DeepClone();
            }
        }

        private static void UpdateElementInfos(JsonObject root)
        {
            var elementInfos = new JsonArray
            {
                new JsonObject
                {
                    ["Id"] = "Normal",
                    ["Name"] = "ElementNameNormal",
                    ["Color"] = new GameColor(Color.White).ToJsonObject(),
                    ["ResistanceStatId"] = "",
                    ["ExcessResistanceCausesHealDamage"] = false,
                    ["OnAfterAttack"] = null
                }
            };
            root["ElementInfos"] = elementInfos;
        }

        private static void UpdateCharacterStats(JsonObject root)
        {
            var stats = DungeonInfoHelpers.CreateStatsTemplate();
            var statsArray = new JsonArray();
            foreach (var stat in stats)
            {
                statsArray.Add(stat.ToJsonObject());
            }
            root["CharacterStats"] = statsArray;
        }

        private static void UpdatePlayerClasses(JsonObject root)
        {
            if (root["PlayerClasses"] is not JsonArray playerClasses) return;
            foreach (var playerClass in playerClasses.OfType<JsonObject>())
            {
                var stats = new JsonArray
                {
                    CreateStat("HP", playerClass, "BaseHP", "MaxHPIncreasePerLevel"),
                    CreateStat("HPRegeneration", playerClass, "BaseHPRegeneration", "HPRegenerationIncreasePerLevel")
                };

                if (playerClass["UsesMP"]?.GetValue<bool>() == true)
                {
                    stats.Add(CreateStat("MP", playerClass, "BaseMP", "MaxMPIncreasePerLevel"));
                    stats.Add(CreateStat("MPRegeneration", playerClass, "BaseMPRegeneration", "MPRegenerationIncreasePerLevel"));
                }
                if (playerClass["UsesHunger"]?.GetValue<bool>() == true)
                {
                    stats.Add(CreateStat("Hunger", playerClass, "BaseHunger", null, 0));
                }
                stats.Add(CreateStat("Attack", playerClass, "BaseAttack", "AttackIncreasePerLevel"));
                stats.Add(CreateStat("Defense", playerClass, "BaseDefense", "DefenseIncreasePerLevel"));
                stats.Add(CreateStat("Movement", playerClass, "BaseMovement", "MovementIncreasePerLevel"));
                stats.Add(CreateStat("Accuracy", playerClass, "BaseAccuracy", null, 0));
                stats.Add(CreateStat("Evasion", playerClass, "BaseEvasion", null, 0));
                playerClass["Stats"] = stats;

                foreach (var action in playerClass["OnAttack"]?.AsArray() ?? new JsonArray())
                {
                    if (action is JsonObject actionObj)
                    {
                        UpdateDealDamageStepsToV15(actionObj);
                        actionObj["Id"] = actionObj["Name"]?.DeepClone();
                    }
                }
                if (playerClass["OnAttacked"] is JsonObject onAttacked)
                {
                    UpdateDealDamageStepsToV15(onAttacked);
                    onAttacked["Id"] = onAttacked["Name"]?.DeepClone();
                }
                if (playerClass["OnDeath"] is JsonObject onDeath)
                {
                    UpdateDealDamageStepsToV15(onDeath);
                    onDeath["Id"] = onDeath["Name"]?.DeepClone();
                }
                if (playerClass["OnTurnStart"] is JsonObject onTurnStart)
                {
                    UpdateDealDamageStepsToV15(onTurnStart);
                    onTurnStart["Id"] = onTurnStart["Name"]?.DeepClone();
                }
                playerClass["OnLevelUp"] = null;
            }
        }

        private static void UpdateNPCs(JsonObject root)
        {
            if (root["NPCs"] is not JsonArray npcs) return;
            foreach (var npc in npcs.OfType<JsonObject>())
            {
                var stats = new JsonArray
                {
                    CreateStat("HP", npc, "BaseHP", "MaxHPIncreasePerLevel"),
                    CreateStat("HPRegeneration", npc, "BaseHPRegeneration", "HPRegenerationIncreasePerLevel")
                };

                if (npc["UsesMP"]?.GetValue<bool>() == true)
                {
                    stats.Add(CreateStat("MP", npc, "BaseMP", "MaxMPIncreasePerLevel"));
                    stats.Add(CreateStat("MPRegeneration", npc, "BaseMPRegeneration", "MPRegenerationIncreasePerLevel"));
                }
                if (npc["UsesHunger"]?.GetValue<bool>() == true)
                {
                    stats.Add(CreateStat("Hunger", npc, "BaseHunger", null, 0));
                }
                stats.Add(CreateStat("Attack", npc, "BaseAttack", "AttackIncreasePerLevel"));
                stats.Add(CreateStat("Defense", npc, "BaseDefense", "DefenseIncreasePerLevel"));
                stats.Add(CreateStat("Movement", npc, "BaseMovement", "MovementIncreasePerLevel"));
                stats.Add(CreateStat("Accuracy", npc, "BaseAccuracy", null, 0));
                stats.Add(CreateStat("Evasion", npc, "BaseEvasion", null, 0));
                npc["Stats"] = stats;

                foreach (var action in npc["OnAttack"]?.AsArray() ?? new JsonArray())
                {
                    if (action is JsonObject actionObj)
                    {
                        UpdateDealDamageStepsToV15(actionObj);
                        actionObj["Id"] = actionObj["Name"]?.DeepClone();
                    }
                }
                foreach (var action in npc["OnInteracted"]?.AsArray() ?? new JsonArray())
                {
                    if (action is JsonObject actionObj)
                    {
                        UpdateDealDamageStepsToV15(actionObj);
                        actionObj["Id"] = actionObj["Name"]?.DeepClone();
                    }
                }
                if (npc["OnAttacked"] is JsonObject onAttacked)
                {
                    UpdateDealDamageStepsToV15(onAttacked);
                    onAttacked["Id"] = onAttacked["Name"]?.DeepClone();
                }
                if (npc["OnDeath"] is JsonObject onDeath)
                {
                    UpdateDealDamageStepsToV15(onDeath);
                    onDeath["Id"] = onDeath["Name"]?.DeepClone();
                }
                if (npc["OnTurnStart"] is JsonObject onTurnStart)
                {
                    UpdateDealDamageStepsToV15(onTurnStart);
                    onTurnStart["Id"] = onTurnStart["Name"]?.DeepClone();
                }
                if (npc["OnSpawn"] is JsonObject onSpawn)
                {
                    UpdateDealDamageStepsToV15(onSpawn);
                    onSpawn["Id"] = onSpawn["Name"]?.DeepClone();
                }
                npc["OnLevelUp"] = null;
            }
        }

        private static void UpdateItems(JsonObject root)
        {
            if (root["Items"] is not JsonArray items) return;
            foreach (var item in items.OfType<JsonObject>())
            {
                foreach (var action in item["OnAttack"]?.AsArray() ?? new JsonArray())
                {
                    if (action is JsonObject actionObj)
                    {
                        UpdateDealDamageStepsToV15(actionObj);
                        actionObj["Id"] = actionObj["Name"]?.DeepClone();
                    }
                }
                if (item["OnAttacked"] is JsonObject onAttacked)
                {
                    UpdateDealDamageStepsToV15(onAttacked);
                    onAttacked["Id"] = onAttacked["Name"]?.DeepClone();
                }
                if (item["OnDeath"] is JsonObject onDeath)
                {
                    UpdateDealDamageStepsToV15(onDeath);
                    onDeath["Id"] = onDeath["Name"]?.DeepClone();
                }
                if (item["OnTurnStart"] is JsonObject onTurnStart)
                {
                    UpdateDealDamageStepsToV15(onTurnStart);
                    onTurnStart["Id"] = onTurnStart["Name"]?.DeepClone();
                }
                if (item["OnUse"] is JsonObject onUse)
                {
                    UpdateDealDamageStepsToV15(onUse);
                    onUse["Id"] = onUse["Name"]?.DeepClone();
                }
            }
        }

        private static void UpdateTraps(JsonObject root)
        {
            if (root["Traps"] is not JsonArray traps) return;
            foreach (var trap in traps.OfType<JsonObject>())
            {
                if (trap["OnStepped"] is JsonObject onStepped)
                {
                    UpdateDealDamageStepsToV15(onStepped);
                    onStepped["Id"] = onStepped["Name"]?.DeepClone();
                }
            }
        }

        private static void UpdateAlteredStatuses(JsonObject root)
        {
            if (root["AlteredStatuses"] is not JsonArray alteredStatuses) return;
            foreach (var alteredStatus in alteredStatuses.OfType<JsonObject>())
            {
                if (alteredStatus["BeforeAttack"] is JsonObject beforeAttack)
                {
                    UpdateDealDamageStepsToV15(beforeAttack);
                    beforeAttack["Id"] = beforeAttack["Name"]?.DeepClone();
                }
                else
                {
                    alteredStatus["BeforeAttack"] = null;
                }
                if (alteredStatus["OnAttacked"] is JsonObject onAttacked)
                {
                    UpdateDealDamageStepsToV15(onAttacked);
                    onAttacked["Id"] = onAttacked["Name"]?.DeepClone();
                }
                else
                {
                    alteredStatus["OnAttacked"] = null;
                }
                if (alteredStatus["OnApply"] is JsonObject onApply)
                {
                    UpdateDealDamageStepsToV15(onApply);
                    onApply["Id"] = onApply["Name"]?.DeepClone();
                }
                if (alteredStatus["OnTurnStart"] is JsonObject onTurnStart)
                {
                    UpdateDealDamageStepsToV15(onTurnStart);
                    onTurnStart["Id"] = onTurnStart["Name"]?.DeepClone();
                }
                if (alteredStatus["OnRemove"] is JsonObject onRemove)
                {
                    UpdateDealDamageStepsToV15(onRemove);
                    onRemove["Id"] = onRemove["Name"]?.DeepClone();
                }
                else
                {
                    alteredStatus["OnRemove"] = null;
                }
            }
        }

        private static void UpdateDealDamageStepsToV15(JsonObject actionWithEffects)
        {
            if (actionWithEffects["Effect"] is JsonObject effect)
            {
                UpdateDealDamageParametersToV15(effect);
            }
        }

        private static void UpdateDealDamageParametersToV15(JsonObject effect)
        {
            if (effect["EffectName"]?.ToString() == "DealDamage")
            {
                var parameters = effect["Params"] as JsonArray ?? new JsonArray();

                if (!parameters.OfType<JsonObject>().Any(p => p["ParamName"]?.ToString().Equals("Element", StringComparison.OrdinalIgnoreCase) == true))
                {
                    parameters.Add(new JsonObject { ["ParamName"] = "Element", ["Value"] = "Normal" });
                }
                if (!parameters.OfType<JsonObject>().Any(p => p["ParamName"]?.ToString().Equals("BypassesResistances", StringComparison.OrdinalIgnoreCase) == true))
                {
                    parameters.Add(new JsonObject { ["ParamName"] = "BypassesResistances", ["Value"] = "true" });
                }
                if (!parameters.OfType<JsonObject>().Any(p => p["ParamName"]?.ToString().Equals("BypassesElementEffect", StringComparison.OrdinalIgnoreCase) == true))
                {
                    parameters.Add(new JsonObject { ["ParamName"] = "BypassesElementEffect", ["Value"] = "true" });
                }
                effect["Params"] = parameters;
            }
            effect["Then"]?.AsObject()?.Let(UpdateDealDamageParametersToV15);
            effect["OnSuccess"]?.AsObject()?.Let(UpdateDealDamageParametersToV15);
            effect["OnFailure"]?.AsObject()?.Let(UpdateDealDamageParametersToV15);
        }

        private static JsonObject CreateStat(string statId, JsonObject source, string baseField, string? incField, int? incDefault = null)
        {
            var stat = new JsonObject
            {
                ["StatId"] = statId,
                ["Base"] = source[baseField]?.DeepClone() ?? 0
            };
            if (incField != null)
                stat["IncreasePerLevel"] = source[incField]?.DeepClone() ?? 0;
            else if (incDefault.HasValue)
                stat["IncreasePerLevel"] = incDefault.Value;
            else
                stat["IncreasePerLevel"] = 0;
            return stat;
        }
    }
}
