using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.DungeonInfoPatches.Interfaces;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.DungeonInfoPatches
{
    public class V11ToV12Patch : IDungeonInfoPatcher
    {
        public static void Apply(JsonObject root)
        {
            UpdateEffectTrees(root);
            root["Version"] = "1.2";
        }

        private static void UpdateEffectTrees(JsonObject root)
        {
            foreach (var entityListName in new[]
            {
                "FloorInfos", "PlayerClasses", "NPCs", "Items", "Traps", "AlteredStatuses"
            })
            {
                if (root[entityListName] is not JsonArray entities) continue;

                foreach (var entity in entities.OfType<JsonObject>())
                {
                    foreach (var actionList in entity.Where(kvp => kvp.Value is JsonArray).ToList())
                    {
                        var array = actionList.Value as JsonArray;
                        foreach (var action in array.OfType<JsonObject>())
                        {
                            UpdateEffectTree(action["Effect"] as JsonObject);
                        }
                    }
                }
            }
        }

        private static void UpdateEffectTree(JsonObject effect)
        {
            if (effect == null) return;

            UpdateChanceToAccuracy(effect);
            UpdatePrintText(effect);

            UpdateEffectTree(effect["Then"] as JsonObject);
            UpdateEffectTree(effect["OnSuccess"] as JsonObject);
            UpdateEffectTree(effect["OnFailure"] as JsonObject);
        }

        private static void UpdateChanceToAccuracy(JsonObject effect)
        {
            if (effect["Params"] is not JsonArray parameters) return;

            bool renamed = false;
            bool hasAccuracy = false;
            bool hasBypassesCheck = false;
            string effectName = effect["EffectName"]?.ToString() ?? "";

            foreach (var param in parameters.OfType<JsonObject>())
            {
                var name = param["ParamName"]?.ToString();
                if (name?.Equals("Chance", StringComparison.OrdinalIgnoreCase) == true)
                {
                    param["ParamName"] = "Accuracy";
                    renamed = true;
                }

                if (name?.Equals("Accuracy", StringComparison.OrdinalIgnoreCase) == true)
                    hasAccuracy = true;

                if (name?.Equals("BypassesAccuracyCheck", StringComparison.OrdinalIgnoreCase) == true)
                    hasBypassesCheck = true;
            }

            if ((renamed || hasAccuracy) && !hasBypassesCheck)
            {
                var shouldBypass = effectName != "DealDamage" && effectName != "BurnMP";

                parameters.Add(new JsonObject
                {
                    ["ParamName"] = "BypassesAccuracyCheck",
                    ["Value"] = shouldBypass.ToString().ToLower()
                });
            }
        }

        private static void UpdatePrintText(JsonObject effect)
        {
            if (effect["EffectName"]?.ToString() != "PrintText") return;
            if (effect["Params"] is not JsonArray parameters) return;

            var hasBypassesVisibility = parameters
                .OfType<JsonObject>()
                .Any(p => p["ParamName"]?.ToString().Equals("BypassesVisibilityCheck", StringComparison.OrdinalIgnoreCase) == true);

            if (!hasBypassesVisibility)
            {
                parameters.Add(new JsonObject
                {
                    ["ParamName"] = "BypassesVisibilityCheck",
                    ["Value"] = "false"
                });
            }
        }
    }
}
