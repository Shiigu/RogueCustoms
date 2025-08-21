using System;
using System.Linq;
using System.Text.Json.Nodes;

using RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.DungeonInfoPatches.Interfaces;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.DungeonInfoPatches
{
    public class V15ToV16Patch : IDungeonInfoPatcher
    {
        public static void Apply(JsonObject root)
        {
            UpdateTileTypeInfos(root);
            UpdateFloorInfos(root);
            UpdateElementInfos(root);
            UpdatePlayerClasses(root);
            UpdateNPCs(root);
            UpdateItems(root);
            UpdateTraps(root);
            UpdateAlteredStatuses(root);
            UpdateScripts(root);

            root["Version"] = "1.6";
        }
        private static void UpdateTileTypeInfos(JsonObject root)
        {
            if (root["TileTypeInfos"] is not JsonArray tileTypeInfos) return;
            foreach (var tileType in tileTypeInfos.OfType<JsonObject>())
            {
                if (tileType["OnStood"] is JsonObject onStood)
                {
                    UpdateEffect(onStood);
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
                    UpdateEffect(onFloorStart);
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
                    UpdateEffect(onAfterAttack);
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
                        UpdateEffect(actionObj);
                    }
                }
                if (playerClass["OnAttacked"] is JsonObject onAttacked)
                {
                    UpdateEffect(onAttacked);
                }
                if (playerClass["OnDeath"] is JsonObject onDeath)
                {
                    RemoveGiveExperienceFromOnDeath(onDeath);
                    UpdateEffect(onDeath);
                }
                if (playerClass["OnTurnStart"] is JsonObject onTurnStart)
                {
                    UpdateEffect(onTurnStart);
                }
                if (playerClass["OnLevelUp"] is JsonObject onLevelUp)
                {
                    UpdateEffect(onLevelUp);
                }
                if (!playerClass.ContainsKey("InitialEquippedWeapon"))
                {
                    playerClass["InitialEquippedWeapon"] = null;
                }
                if (!playerClass.ContainsKey("InitialEquippedArmor"))
                {
                    playerClass["InitialEquippedArmor"] = null;
                }
            }
        }

        private static void UpdateNPCs(JsonObject root)
        {
            if (root["NPCs"] is not JsonArray npcs) return;
            foreach (var npc in npcs.OfType<JsonObject>())
            {
                foreach (var action in npc["OnAttack"]?.AsArray() ?? [])
                {
                    if (action is JsonObject actionObj)
                    {
                        UpdateEffect(actionObj);
                    }
                }
                if (npc["OnAttacked"] is JsonObject onAttacked)
                {
                    UpdateEffect(onAttacked);
                }
                if (npc["OnDeath"] is JsonObject onDeath)
                {
                    RemoveGiveExperienceFromOnDeath(onDeath);
                    UpdateEffect(onDeath);
                }
                if (npc["OnTurnStart"] is JsonObject onTurnStart)
                {
                    UpdateEffect(onTurnStart);
                }
                if (npc["OnLevelUp"] is JsonObject onLevelUp)
                {
                    UpdateEffect(onLevelUp);
                }
            }
        }

        private static void UpdateItems(JsonObject root)
        {
            if (root["Items"] is not JsonArray items) return;
            foreach (var item in items.OfType<JsonObject>())
            {
                foreach (var action in item["OnAttack"]?.AsArray() ?? [])
                {
                    if (action is JsonObject actionObj)
                    {
                        UpdateEffect(actionObj);
                    }
                }
                if (item["OnAttacked"] is JsonObject onAttacked)
                {
                    UpdateEffect(onAttacked);
                }
                if (item["OnDeath"] is JsonObject onDeath)
                {
                    UpdateEffect(onDeath);
                }
                if (item["OnTurnStart"] is JsonObject onTurnStart)
                {
                    UpdateEffect(onTurnStart);
                }
                if (item["OnUse"] is JsonObject onUse)
                {
                    UpdateEffect(onUse);
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
                    UpdateEffect(onStepped);
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
                    UpdateEffect(beforeAttack);
                }

                if (alteredStatus["OnAttacked"] is JsonObject onAttacked)
                {
                    UpdateEffect(onAttacked);
                }

                if (alteredStatus["OnApply"] is JsonObject onApply)
                {
                    UpdateEffect(onApply);
                }

                if (alteredStatus["OnTurnStart"] is JsonObject onTurnStart)
                {
                    UpdateEffect(onTurnStart);
                }

                if (alteredStatus["OnRemove"] is JsonObject onRemove)
                {
                    UpdateEffect(onRemove);
                }
            }
        }

        private static void UpdateScripts(JsonObject root)
        {
            if (root["Scripts"] is not JsonArray scripts) return;
            foreach (var script in scripts.OfType<JsonObject>())
            {
                UpdateEffect(script);
            }
        }

        public static void UpdateEffect(JsonObject actionWithEffects)
        {
            if (actionWithEffects["Effect"] is JsonObject effect)
            {
                UpdateApplyAlteredStatusStepsToV16(effect);
                UpdateApplyStatAlterationStepsToV16(effect);
                UpdateTransformTileStepsToV16(effect);
                UpdateSpawnNPCStepsToV16(effect);
            }
        }

        private static void RemoveGiveExperienceFromOnDeath(JsonObject actionWithEffects)
        {
            if (actionWithEffects["Effect"] is JsonObject effect)
            {
                RemoveGiveExperience(null, effect, actionWithEffects, "Effect");
            }
        }

        private static void RemoveGiveExperience(JsonObject? parentEffect, JsonObject effect, JsonObject parentContainer, string effectKey)
        {
            if (effect["EffectName"]?.ToString() == "GiveExperience")
            {
                if (effect[effectKey] is JsonObject childEffect)
                {
                    parentContainer[effectKey] = childEffect.DeepClone();
                    RemoveGiveExperience(parentEffect, childEffect, parentContainer, effectKey);
                }
                else
                {
                    parentContainer[effectKey] = null;
                }
                return;
            }

            if (effect["Then"] is JsonObject thenObj)
                RemoveGiveExperience(effect, thenObj, effect, "Then");
            if (effect["OnSuccess"] is JsonObject onSuccessObj)
                RemoveGiveExperience(effect, onSuccessObj, effect, "OnSuccess");
            if (effect["OnFailure"] is JsonObject onFailureObj)
                RemoveGiveExperience(effect, onFailureObj, effect, "OnFailure");
        }

        private static void UpdateApplyAlteredStatusStepsToV16(JsonObject effect)
        {
            UpdateApplyAlteredStatusParametersToV16(effect);
        }

        private static void UpdateApplyAlteredStatusParametersToV16(JsonObject effect)
        {
            if (effect["EffectName"]?.ToString() == "ApplyAlteredStatus")
            {
                var parameters = effect["Params"] as JsonArray ?? new JsonArray();

                if (!parameters.OfType<JsonObject>().Any(p => p["ParamName"]?.ToString().Equals("AnnounceStatusRefresh", StringComparison.OrdinalIgnoreCase) == true))
                {
                    parameters.Add(new JsonObject { ["ParamName"] = "AnnounceStatusRefresh", ["Value"] = "True" });
                }

                effect["Params"] = parameters;
            }

            if (effect["Then"] is JsonObject thenObj)
                UpdateApplyAlteredStatusParametersToV16(thenObj);
            if (effect["OnSuccess"] is JsonObject onSuccessObj)
                UpdateApplyAlteredStatusParametersToV16(onSuccessObj);
            if (effect["OnFailure"] is JsonObject onFailureObj)
                UpdateApplyAlteredStatusParametersToV16(onFailureObj);
        }

        private static void UpdateApplyStatAlterationStepsToV16(JsonObject effect)
        {
            UpdateApplyStatAlterationParametersToV16(effect);
        }

        private static void UpdateApplyStatAlterationParametersToV16(JsonObject effect)
        {
            if (effect["EffectName"]?.ToString() == "ApplyStatAlteration")
            {
                var parameters = effect["Params"] as JsonArray ?? new JsonArray();

                if (!parameters.OfType<JsonObject>().Any(p => p["ParamName"]?.ToString().Equals("CanBeOverwritten", StringComparison.OrdinalIgnoreCase) == true))
                {
                    parameters.Add(new JsonObject { ["ParamName"] = "CanBeOverwritten", ["Value"] = "False" });
                }

                effect["Params"] = parameters;
            }

            if (effect["Then"] is JsonObject thenObj)
                UpdateApplyStatAlterationParametersToV16(thenObj);
            if (effect["OnSuccess"] is JsonObject onSuccessObj)
                UpdateApplyStatAlterationParametersToV16(onSuccessObj);
            if (effect["OnFailure"] is JsonObject onFailureObj)
                UpdateApplyStatAlterationParametersToV16(onFailureObj);
        }
        private static void UpdateTransformTileStepsToV16(JsonObject effect)
        {
            UpdateTransformTileParametersToV16(effect);
        }

        private static void UpdateTransformTileParametersToV16(JsonObject effect)
        {
            if (effect["EffectName"]?.ToString() == "TransformTile")
            {
                var parameters = effect["Params"] as JsonArray ?? new JsonArray();

                if (!parameters.OfType<JsonObject>().Any(p => p["ParamName"]?.ToString().Equals("TurnLength", StringComparison.OrdinalIgnoreCase) == true))
                {
                    parameters.Add(new JsonObject { ["ParamName"] = "TurnLength", ["Value"] = "-1" });
                }

                effect["Params"] = parameters;
            }

            if (effect["Then"] is JsonObject thenObj)
                UpdateTransformTileParametersToV16(thenObj);
            if (effect["OnSuccess"] is JsonObject onSuccessObj)
                UpdateTransformTileParametersToV16(onSuccessObj);
            if (effect["OnFailure"] is JsonObject onFailureObj)
                UpdateTransformTileParametersToV16(onFailureObj);
        }

        private static void UpdateSpawnNPCStepsToV16(JsonObject effect)
        {
            UpdateSpawnNPCParametersToV16(effect);
        }

        private static void UpdateSpawnNPCParametersToV16(JsonObject effect)
        {
            if (effect["EffectName"]?.ToString() == "SpawnNPC")
            {
                var parameters = effect["Params"] as JsonArray ?? new JsonArray();

                if (!parameters.OfType<JsonObject>().Any(p => p["ParamName"]?.ToString().Equals("InheritsSpawnerColour", StringComparison.OrdinalIgnoreCase) == true))
                {
                    parameters.Add(new JsonObject { ["ParamName"] = "InheritsSpawnerColour", ["Value"] = "false" });
                }

                effect["Params"] = parameters;
            }

            if (effect["Then"] is JsonObject thenObj)
                UpdateSpawnNPCParametersToV16(thenObj);
            if (effect["OnSuccess"] is JsonObject onSuccessObj)
                UpdateSpawnNPCParametersToV16(onSuccessObj);
            if (effect["OnFailure"] is JsonObject onFailureObj)
                UpdateSpawnNPCParametersToV16(onFailureObj);
        }
    }
}
