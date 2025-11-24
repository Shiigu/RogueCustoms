using System.Collections.Generic;

namespace RogueCustomsDungeonEditor.Utils
{
    public static class FormConstants
    {
        public const string ActionClipboardKey = "ActionWithEffects";
        public const string StepClipboardKey = "Effect";
        public const string LayoutClipboardKey = "Layout";
        public static readonly List<string> DefaultTileTypes = new() { "Empty", "Floor", "Wall", "Hallway", "Stairs", "Door" };
        public static readonly List<string> DefaultStats = new() { "HP", "HPRegeneration", "MP", "MPRegeneration", "Hunger", "Attack", "Defense", "Movement", "Accuracy", "Evasion" };
        public static readonly List<string> MandatoryStats = new () { "HP", "Attack", "Defense", "Movement", "Accuracy", "Evasion" };
    }
}
