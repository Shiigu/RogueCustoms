using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;

namespace RogueCustomsGameEngine.Utils.JsonImports
{
    [Serializable]
    public class AlteredStatusInfo : ClassInfo
    {
        public ActionWithEffectsInfo OnTurnStart { get; set; } = new ActionWithEffectsInfo();
        public ActionWithEffectsInfo OnAttacked { get; set; } = new ActionWithEffectsInfo();
        public bool CanStack { get; set; }
        public bool CanOverwrite { get; set; }
        public bool CleanseOnFloorChange { get; set; }
        public bool CleansedByCleanseActions { get; set; }
        public ActionWithEffectsInfo OnApply { get; set; } = new ActionWithEffectsInfo();
        public ActionWithEffectsInfo OnRemove { get; set; } = new ActionWithEffectsInfo();
        public ActionWithEffectsInfo BeforeAttack { get; set; } = new ActionWithEffectsInfo();
    }
}