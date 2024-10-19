using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V12
{
    [Serializable]
    public class AlteredStatusInfoV12 : ClassInfoV12
    {
        public List<ActionWithEffectsInfoV12> OnTurnStartActions { get; set; } = new List<ActionWithEffectsInfoV12>();
        public bool CanStack { get; set; }
        public bool CanOverwrite { get; set; }
        public bool CleanseOnFloorChange { get; set; }
        public bool CleansedByCleanseActions { get; set; }
        public List<ActionWithEffectsInfoV12> OnStatusApplyActions { get; set; } = new List<ActionWithEffectsInfoV12>();
    }
}