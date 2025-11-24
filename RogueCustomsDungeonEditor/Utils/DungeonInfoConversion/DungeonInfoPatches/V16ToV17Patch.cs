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
    }
}
