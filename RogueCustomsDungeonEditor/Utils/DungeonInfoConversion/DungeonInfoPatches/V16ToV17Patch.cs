using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.DungeonInfoPatches.Interfaces;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.DungeonInfoPatches
{
    public class V16ToV17Patch : IDungeonInfoPatcher
    {
        public static void Apply(JsonObject root)
        {
            MoveCapsFromStatsToCharacterStats(root);
            UpdateTileTypeInfos(root);
            UpdateFloorInfos(root);
            UpdateElementInfos(root);
            UpdateItems(root);
            UpdatePlayerClasses(root);
            UpdateNPCs(root);
            UpdateTraps(root);
            UpdateAlteredStatuses(root);
            UpdateScripts(root);

            root["Version"] = "1.7";
        }

        private static void MoveCapsFromStatsToCharacterStats(JsonObject root)
        {
            if (root["CharacterStats"] is not JsonArray characterStats) return;
            foreach (var characterStat in characterStats.OfType<JsonObject>())
            {
                MoveCapsFromStatToCharacterStats(characterStat, root);
            }
        }

        private static void MoveCapsFromStatToCharacterStats(JsonObject characterStat, JsonObject root)
        {
            if (root["PlayerClasses"] is not JsonArray playerClasses) return;
            if (root["NPCs"] is not JsonArray NPCs) return;
            if (characterStat is null) return;
            if (characterStat["MinCap"] is null || characterStat["MaxCap"] is null) return;

            var minCap = characterStat["MinCap"].GetValue<decimal>();
            var maxCap = characterStat["MaxCap"].GetValue<decimal>();

            foreach (var playerClass in playerClasses.OfType<JsonObject>())
            {
                MoveCapsFromStatToClassStats(playerClass, characterStat["Id"].ToString(), minCap, maxCap);
            }

            foreach (var npc in NPCs.OfType<JsonObject>())
            {
                MoveCapsFromStatToClassStats(npc, characterStat["Id"].ToString(), minCap, maxCap);
            }

            characterStat.Remove("MinCap");
            characterStat.Remove("MaxCap");
        }

        private static void MoveCapsFromStatToClassStats(JsonObject character, string statId, decimal minCap, decimal maxCap)
        {
            if (character is null) return;
            if (character["Stats"] is not JsonArray stats) return;

            foreach (var stat in stats.OfType<JsonObject>())
            {
                if (stat["StatId"].ToString() == statId)
                {
                    stat["Minimum"] = minCap;
                    stat["Maximum"] = maxCap;
                }
            }
        }

        private static void UpdateTileTypeInfos(JsonObject root)
        {
            if (root["TileTypeInfos"] is not JsonArray tileTypeInfos) return;
            foreach (var tileType in tileTypeInfos.OfType<JsonObject>())
            {
                if (tileType["OnStood"] is JsonObject onStood)
                {
                    UpdateAction(onStood);
                }
            }
        }

        private static void UpdateFloorInfos(JsonObject root)
        {
            if (root["FloorInfos"] is not JsonArray floorInfos) return;
            foreach (var floorGroup in floorInfos.OfType<JsonObject>())
            {
                if (floorGroup["OnFloorStart"] is JsonObject onFloorStart)
                {
                    UpdateAction(onFloorStart);
                }
            }
        }

        private static void UpdateElementInfos(JsonObject root)
        {
            if (root["ElementInfos"] is not JsonArray elementInfos) return;
            foreach (var element in elementInfos.OfType<JsonObject>())
            {
                if (element["OnAfterAttack"] is JsonObject onAfterAttack)
                {
                    UpdateAction(onAfterAttack);
                }
            }
        }

        private static void UpdatePlayerClasses(JsonObject root)
        {
            if (root["PlayerClasses"] is not JsonArray playerClasses) return;
            foreach (var playerClass in playerClasses.OfType<JsonObject>())
            {
                foreach (var action in playerClass["OnAttack"]?.AsArray() ?? [])
                {
                    if (action is JsonObject actionObj)
                    {
                        UpdateAction(actionObj);
                    }
                }
                if (playerClass["OnAttacked"] is JsonObject onAttacked)
                {
                    UpdateAction(onAttacked);
                }
                if (playerClass["OnDeath"] is JsonObject onDeath)
                {
                    UpdateAction(onDeath);
                }
                if (playerClass["OnTurnStart"] is JsonObject onTurnStart)
                {
                    UpdateAction(onTurnStart);
                }
                if (playerClass["OnLevelUp"] is JsonObject onLevelUp)
                {
                    UpdateAction(onLevelUp);
                }
            }
        }
        private static void UpdateNPCs(JsonObject root)
        {
            if (root["NPCs"] is not JsonArray npcs) return;
            foreach (var npc in npcs.OfType<JsonObject>())
            {
                if (npc["OnSpawn"] is JsonObject onSpawn)
                {
                    UpdateAction(onSpawn);
                }
                foreach (var action in npc["OnAttack"]?.AsArray() ?? [])
                {
                    if (action is JsonObject actionObj)
                    {
                        UpdateAction(actionObj);
                    }
                }
                foreach (var action in npc["OnInteracted"]?.AsArray() ?? [])
                {
                    if (action is JsonObject actionObj)
                    {
                        UpdateAction(actionObj);
                    }
                }
                if (npc["OnAttacked"] is JsonObject onAttacked)
                {
                    UpdateAction(onAttacked);
                }
                if (npc["OnDeath"] is JsonObject onDeath)
                {
                    UpdateAction(onDeath);
                }
                if (npc["OnTurnStart"] is JsonObject onTurnStart)
                {
                    UpdateAction(onTurnStart);
                }
                if (npc["OnLevelUp"] is JsonObject onLevelUp)
                {
                    UpdateAction(onLevelUp);
                }
            }
        }
        private static void UpdateItems(JsonObject root)
        {
            if (root["Items"] is not JsonArray items) return;
            foreach (var item in items.OfType<JsonObject>())
            {
                if (item["ItemType"] is null)
                    item["ItemType"] = item["EntityType"]?.GetValue<string>();
                item.Remove("EntityType");
                foreach (var action in item["OnAttack"]?.AsArray() ?? [])
                {
                    if (action is JsonObject actionObj)
                    {
                        UpdateAction(actionObj);
                    }
                }
                if (item["OnAttacked"] is JsonObject onAttacked)
                {
                    UpdateAction(onAttacked);
                }
                if (item["OnDeath"] is JsonObject onDeath)
                {
                    UpdateAction(onDeath);
                }
                if (item["OnTurnStart"] is JsonObject onTurnStart)
                {
                    UpdateAction(onTurnStart);
                }
                if (item["OnUse"] is JsonObject onUse)
                {
                    UpdateAction(onUse);
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
                    UpdateAction(onStepped);
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
                    UpdateAction(beforeAttack);
                }

                if (alteredStatus["OnAttacked"] is JsonObject onAttacked)
                {
                    UpdateAction(onAttacked);
                }

                if (alteredStatus["OnApply"] is JsonObject onApply)
                {
                    UpdateAction(onApply);
                }

                if (alteredStatus["OnTurnStart"] is JsonObject onTurnStart)
                {
                    UpdateAction(onTurnStart);
                }

                if (alteredStatus["OnRemove"] is JsonObject onRemove)
                {
                    UpdateAction(onRemove);
                }
            }
        }

        private static void UpdateScripts(JsonObject root)
        {
            if (root["Scripts"] is not JsonArray scripts) return;
            foreach (var script in scripts.OfType<JsonObject>())
            {
                UpdateAction(script);
            }
        }

        public static void UpdateAction(JsonObject actionWithEffects)
        {
            UpdateActionSteps(actionWithEffects);
        }

        public static void UpdateActionSteps(JsonObject actionWithEffects)
        {
            if (actionWithEffects["Effect"] is JsonObject effect)
            {
                UpdateSpawnNPCStepsToV17(effect);
            }
        }
        private static void UpdateSpawnNPCStepsToV17(JsonObject effect)
        {
            UpdateSpawnNPCParametersToV17(effect);
        }

        private static void UpdateSpawnNPCParametersToV17(JsonObject effect)
        {
            if (effect["EffectName"]?.ToString() == "SpawnNPC")
            {
                var parameters = effect["Params"] as JsonArray ?? new JsonArray();

                if (!parameters.OfType<JsonObject>().Any(p => p["ParamName"]?.ToString().Equals("SpawnName", StringComparison.OrdinalIgnoreCase) == true))
                {
                    parameters.Add(new JsonObject { ["ParamName"] = "SpawnName", ["Value"] = "" });
                }

                effect["Params"] = parameters;
            }

            if (effect["Then"] is JsonObject thenObj)
                UpdateSpawnNPCParametersToV17(thenObj);
            if (effect["OnSuccess"] is JsonObject onSuccessObj)
                UpdateSpawnNPCParametersToV17(onSuccessObj);
            if (effect["OnFailure"] is JsonObject onFailureObj)
                UpdateSpawnNPCParametersToV17(onFailureObj);
        }
    }
}
