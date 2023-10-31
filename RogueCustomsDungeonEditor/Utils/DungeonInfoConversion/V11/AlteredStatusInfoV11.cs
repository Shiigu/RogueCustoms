using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V11
{
    [Serializable]
    public class AlteredStatusInfoV11 : ClassInfoV11
    {
        public List<ActionWithEffectsInfoV11> OnTurnStartActions { get; set; } = new List<ActionWithEffectsInfoV11>();
        public bool CanStack { get; set; }
        public bool CanOverwrite { get; set; }
        public bool CleanseOnFloorChange { get; set; }
        public bool CleansedByCleanseActions { get; set; }
        public List<ActionWithEffectsInfoV11> OnStatusApplyActions { get; set; } = new List<ActionWithEffectsInfoV11>();
    }
}