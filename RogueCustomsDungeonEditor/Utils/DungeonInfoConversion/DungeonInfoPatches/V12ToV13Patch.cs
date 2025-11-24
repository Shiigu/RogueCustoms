using System.Linq;
using System.Text.Json.Nodes;

using RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.DungeonInfoPatches.Interfaces;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.DungeonInfoPatches
{
    public class V12ToV13Patch : IDungeonInfoPatcher
    {
        public static void Apply(JsonObject root)
        {
            AddHungerDegenerationToFloorInfos(root);
            AddAITypeToNPCs(root);
            NullifyOnUseForWeaponsAndArmor(root);
            root["Version"] = "1.3";
        }

        private static void AddHungerDegenerationToFloorInfos(JsonObject root)
        {
            if (root["FloorInfos"] is not JsonArray floorInfos) return;

            foreach (var floorInfo in floorInfos.OfType<JsonObject>())
            {
                floorInfo["HungerDegeneration"] = 0;
            }
        }

        private static void AddAITypeToNPCs(JsonObject root)
        {
            if (root["NPCs"] is not JsonArray npcs) return;

            foreach (var npc in npcs.OfType<JsonObject>())
            {
                npc["AIType"] = "Default";
            }
        }

        private static void NullifyOnUseForWeaponsAndArmor(JsonObject root)
        {
            if (root["Items"] is not JsonArray items) return;

            foreach (var item in items.OfType<JsonObject>())
            {
                var entityType = item["EntityType"]?.ToString();
                if (entityType == "Weapon" || entityType == "Armor")
                {
                    item["OnUse"] = null;
                }
            }
        }
    }
}
