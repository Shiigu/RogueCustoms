using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V11
{
    [Serializable]
    public class TrapInfoV11 : ClassInfoV11
    {
        public bool StartsVisible { get; set; }
        public string Power { get; set; }
        public List<ActionWithEffectsInfoV11> OnItemSteppedActions { get; set; } = new List<ActionWithEffectsInfoV11>();
    }
}