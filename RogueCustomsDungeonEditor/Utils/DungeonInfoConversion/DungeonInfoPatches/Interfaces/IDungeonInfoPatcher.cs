using System.Text.Json.Nodes;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.DungeonInfoPatches.Interfaces
{
    public interface IDungeonInfoPatcher
    {
        static abstract void Apply(JsonObject root);
    }
}
