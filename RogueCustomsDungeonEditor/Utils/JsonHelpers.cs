using System;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace RogueCustomsDungeonEditor.Utils
{
    public static class JsonHelpers
    {
        public static void Let(this JsonObject? obj, Action<JsonObject> action)
        {
            if (obj != null) action(obj);
        }
        public static JsonArray AsArray(this JsonNode? node) => node as JsonArray ?? new JsonArray();
        public static JsonObject AsObject(this JsonNode? node) => node as JsonObject;
        public static JsonObject ToJsonObject<T>(this T obj)
        {
            if (obj == null) return new JsonObject();
            var json = JsonSerializer.Serialize(obj, obj.GetType());
            return JsonNode.Parse(json)?.AsObject() ?? new JsonObject();
        }
        public static T? GetValue<T>(this JsonNode? node)
        {
            if (node is JsonValue value && value.TryGetValue<T>(out var result))
                return result;
            return default;
        }
    }
}
