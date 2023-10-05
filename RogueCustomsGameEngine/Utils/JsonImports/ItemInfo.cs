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
        public List<ActionWithEffectsInfo> OnTurnStartActions { get; set; } = new List<ActionWithEffectsInfo>();
        public List<ActionWithEffectsInfo> OnAttackActions { get; set; } = new List<ActionWithEffectsInfo>();
        public List<ActionWithEffectsInfo> OnAttackedActions { get; set; } = new List<ActionWithEffectsInfo>();
        public List<ActionWithEffectsInfo> OnItemSteppedActions { get; set; } = new List<ActionWithEffectsInfo>();
        public List<ActionWithEffectsInfo> OnItemUseActions { get; set; } = new List<ActionWithEffectsInfo>();

    }
}