using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;

namespace RogueCustomsGameEngine.Utils.JsonImports
{
    [Serializable]
    public class AlteredStatusInfo : ClassInfo
    {
        public List<ActionWithEffectsInfo> OnTurnStartActions { get; set; } = new List<ActionWithEffectsInfo>();
        public bool CanStack { get; set; }
        public bool CanOverwrite { get; set; }
        public bool CleanseOnFloorChange { get; set; }
        public bool CleansedByCleanseActions { get; set; }
        public List<ActionWithEffectsInfo> OnStatusApplyActions { get; set; } = new List<ActionWithEffectsInfo>();
    }
}