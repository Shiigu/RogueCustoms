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
            UpdateAffixInfos(root);
            UpdateLearnsets(root);
            UpdatePlayerClasses(root);
            UpdateNPCs(root);
            UpdateNPCModifierInfos(root);
            UpdateTraps(root);
            UpdateAlteredStatuses(root);
            UpdateQuests(root);
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

        private static void UpdateLearnsets(JsonObject root)
        {
            if (root["LearnsetInfos"] is JsonArray) return;
            root["LearnsetInfos"] = new JsonArray()
                {
                    new JsonObject
                    {
                        ["Id"] = "Default",
                        ["Entries"] = new JsonArray()
                    }
                };
        }

        private static void UpdatePlayerClasses(JsonObject root)
        {
            if (root["PlayerClasses"] is not JsonArray playerClasses) return;
            foreach (var playerClass in playerClasses.OfType<JsonObject>())
            {
                if (playerClass["Learnset"] is null)
                    playerClass["Learnset"] = "Default";
                if (playerClass["DefaultOnAttack"] is JsonObject defaultOnAttack)
                {
                    UpdateAction(defaultOnAttack);
                }
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
                if (npc["Learnset"] is null)
                    npc["Learnset"] = "Default";
                if (npc["BeforeProcessAI"] is JsonObject beforeProcessAI)
                {
                    UpdateAction(beforeProcessAI);
                }
                if (npc["OnSpawn"] is JsonObject onSpawn)
                {
                    UpdateAction(onSpawn);
                }
                if (npc["DefaultOnAttack"] is JsonObject defaultOnAttack)
                {
                    UpdateAction(defaultOnAttack);
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
                if (item["RequiredPlayerLevel"] is null)
                    item["RequiredPlayerLevel"] = 1;
                if (item["CanBeUnequipped"] is null)
                    item["CanBeUnequipped"] = true;
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

        private static void UpdateAffixInfos(JsonObject root)
        {
            if (root["AffixInfos"] is not JsonArray affixes) return;
            foreach (var affix in affixes.OfType<JsonObject>())
            {
                if (affix["RequiredPlayerLevel"] is null)
                    affix["RequiredPlayerLevel"] = 1;
                if (affix["OnTurnStart"] is JsonObject onTurnStart)
                {
                    UpdateAction(onTurnStart);
                }
                if (affix["OnAttacked"] is JsonObject onAttacked)
                {
                    UpdateAction(onAttacked);
                }
                if (affix["OnAttack"] is JsonObject onAttack)
                {
                    UpdateAction(onAttack);
                }
            }
        }

        private static void UpdateNPCModifierInfos(JsonObject root)
        {
            if (root["NPCModifierInfo"] is not JsonArray npcModifiers) return;
            foreach (var npcModifier in npcModifiers.OfType<JsonObject>())
            {
                if (npcModifier["OnSpawn"] is JsonObject onSpawn)
                {
                    UpdateAction(onSpawn);
                }
                if (npcModifier["OnTurnStart"] is JsonObject onTurnStart)
                {
                    UpdateAction(onTurnStart);
                }
                if (npcModifier["OnAttacked"] is JsonObject onAttacked)
                {
                    UpdateAction(onAttacked);
                }
                if (npcModifier["OnAttack"] is JsonObject onAttack)
                {
                    UpdateAction(onAttack);
                }
                if (npcModifier["OnDeath"] is JsonObject onDeath)
                {
                    UpdateAction(onDeath);
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

        private static void UpdateQuests(JsonObject root)
        {
            if (root["QuestInfos"] is JsonArray) return;
            root["QuestInfos"] = new JsonArray();
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
                UpdateDealDamageStepsToV17(effect);
                UpdateApplyStatAlterationStepsToV17(effect);
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

        private static void UpdateDealDamageStepsToV17(JsonObject effect)
        {
            UpdateDealDamageParametersToV17(effect);
        }

        private static void UpdateDealDamageParametersToV17(JsonObject effect)
        {
            if (effect["EffectName"]?.ToString() == "DealDamage")
            {
                var parameters = effect["Params"] as JsonArray ?? new JsonArray();

                if (!parameters.OfType<JsonObject>().Any(p => p["ParamName"]?.ToString().Equals("InformOfFailure", StringComparison.OrdinalIgnoreCase) == true))
                {
                    parameters.Add(new JsonObject { ["ParamName"] = "InformOfFailure", ["Value"] = "true" });
                }

                effect["Params"] = parameters;
            }

            if (effect["Then"] is JsonObject thenObj)
                UpdateDealDamageParametersToV17(thenObj);
            if (effect["OnSuccess"] is JsonObject onSuccessObj)
                UpdateDealDamageParametersToV17(onSuccessObj);
            if (effect["OnFailure"] is JsonObject onFailureObj)
                UpdateDealDamageParametersToV17(onFailureObj);
        }

        private static void UpdateApplyStatAlterationStepsToV17(JsonObject effect)
        {
            UpdateApplyStatAlterationParametersToV17(effect);
        }

        private static void UpdateApplyStatAlterationParametersToV17(JsonObject effect)
        {
            if (effect["EffectName"]?.ToString() == "ApplyStatAlteration")
            {
                var parameters = effect["Params"] as JsonArray ?? new JsonArray();
                var idParam = parameters.OfType<JsonObject>().FirstOrDefault(p => p["ParamName"]?.ToString().Equals("Id", StringComparison.OrdinalIgnoreCase) == true);
                if (idParam != null)
                {
                    var id = idParam["Value"].GetValue<string>();

                    if (!parameters.OfType<JsonObject>().Any(p => p["ParamName"]?.ToString().Equals("Name", StringComparison.OrdinalIgnoreCase) == true))
                    {
                        parameters.Add(new JsonObject { ["ParamName"] = "Name", ["Value"] = id });
                    }

                    effect["Params"] = parameters;
                }
            }

            if (effect["Then"] is JsonObject thenObj)
                UpdateApplyStatAlterationParametersToV17(thenObj);
            if (effect["OnSuccess"] is JsonObject onSuccessObj)
                UpdateApplyStatAlterationParametersToV17(onSuccessObj);
            if (effect["OnFailure"] is JsonObject onFailureObj)
                UpdateApplyStatAlterationParametersToV17(onFailureObj);
        }
    }
}
