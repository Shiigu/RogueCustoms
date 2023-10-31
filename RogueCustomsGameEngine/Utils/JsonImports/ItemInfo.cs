using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;

namespace RogueCustomsGameEngine.Utils.JsonImports
{
    [Serializable]
    public class ItemInfo : ClassInfo
    {
        public bool CanBePickedUp { get; set; }
        public bool StartsVisible { get; set; }
        public string Power { get; set; }
        public string EntityType { get; set; }
        public ActionWithEffectsInfo OnTurnStart { get; set; } = new ActionWithEffectsInfo();
        public List<ActionWithEffectsInfo> OnAttack { get; set; } = new List<ActionWithEffectsInfo>();
        public ActionWithEffectsInfo OnAttacked { get; set; } = new ActionWithEffectsInfo();
        public ActionWithEffectsInfo OnStepped { get; set; } = new ActionWithEffectsInfo();
        public ActionWithEffectsInfo OnUse { get; set; } = new ActionWithEffectsInfo();

    }
}