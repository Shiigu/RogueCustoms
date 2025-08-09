using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.DungeonInfoPatches.Interfaces;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.DungeonInfoPatches
{
    public class V15ToV16Patch : IDungeonInfoPatcher
    {
        public static void Apply(JsonObject root)
        {
            UpdatePlayerClasses(root);
            UpdateNPCs(root);

            root["Version"] = "1.6";
        }

        private static void UpdatePlayerClasses(JsonObject root)
        {
            if (root["PlayerClasses"] is not JsonArray playerClasses) return;
            foreach (var playerClass in playerClasses.OfType<JsonObject>())
            {
                if (playerClass["OnDeath"] is JsonObject onDeath)
                {
                    RemoveGiveExperienceFromOnDeath(onDeath);
                }
            }
        }

        private static void UpdateNPCs(JsonObject root)
        {
            if (root["NPCs"] is not JsonArray npcs) return;
            foreach (var npc in npcs.OfType<JsonObject>())
            {
                if (npc["OnDeath"] is JsonObject onDeath)
                {
                    RemoveGiveExperienceFromOnDeath(onDeath);
                }
            }
        }

        private static void RemoveGiveExperienceFromOnDeath(JsonObject actionWithEffects)
        {
            if (actionWithEffects["Effect"] is JsonObject effect)
            {
                RemoveGiveExperienceRecursive(null, effect, actionWithEffects, "Effect");
            }
        }

        private static void RemoveGiveExperienceRecursive(JsonObject? parentEffect, JsonObject effect, JsonObject parentContainer, string effectKey)
        {
            if (effect["EffectName"]?.ToString() == "GiveExperience")
            {
                if (effect[effectKey] is JsonObject childEffect)
                {
                    parentContainer[effectKey] = childEffect.DeepClone();
                    RemoveGiveExperienceRecursive(parentEffect, childEffect, parentContainer, effectKey);
                }
                else
                {
                    parentContainer[effectKey] = null;
                }
                return;
            }

            if (effect["Then"] is JsonObject thenObj)
                RemoveGiveExperienceRecursive(effect, thenObj, effect, "Then");
            if (effect["OnSuccess"] is JsonObject onSuccessObj)
                RemoveGiveExperienceRecursive(effect, onSuccessObj, effect, "OnSuccess");
            if (effect["OnFailure"] is JsonObject onFailureObj)
                RemoveGiveExperienceRecursive(effect, onFailureObj, effect, "OnFailure");
        }
    }
}
