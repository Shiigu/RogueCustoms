using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsDungeonEditor.Utils
{
    public static class FormConstants
    {
        public const string ActionClipboardKey = "ActionWithEffects";
        public const string StepClipboardKey = "Effect";
        public const string LayoutClipboardKey = "Layout";
        public static readonly List<string> DefaultTileTypes = new() { "Empty", "Floor", "Wall", "Hallway", "Stairs", "Door" };
    }
}
