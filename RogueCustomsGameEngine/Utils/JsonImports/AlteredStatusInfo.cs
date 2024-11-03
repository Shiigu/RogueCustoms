using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;

namespace RogueCustomsGameEngine.Utils.JsonImports
{
    [Serializable]
    public class AlteredStatusInfo : ClassInfo
    {
        public ActionWithEffectsInfo OnTurnStart { get; set; }
        public ActionWithEffectsInfo OnAttacked { get; set; }
        public bool CanStack { get; set; }
        public bool CanOverwrite { get; set; }
        public bool CleanseOnFloorChange { get; set; }
        public bool CleansedByCleanseActions { get; set; }
        public ActionWithEffectsInfo OnApply { get; set; }
        public ActionWithEffectsInfo OnRemove { get; set; }
        public ActionWithEffectsInfo BeforeAttack { get; set; }
    }
}