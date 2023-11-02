using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V11
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public class ItemInfoV11 : ClassInfoV11
    {
        public bool CanBePickedUp { get; set; }
        public bool StartsVisible { get; set; }
        public string Power { get; set; }
        public string EntityType { get; set; }
        public List<ActionWithEffectsInfoV11> OnTurnStartActions { get; set; } = new List<ActionWithEffectsInfoV11>();
        public List<ActionWithEffectsInfoV11> OnAttackActions { get; set; } = new List<ActionWithEffectsInfoV11>();
        public List<ActionWithEffectsInfoV11> OnAttackedActions { get; set; } = new List<ActionWithEffectsInfoV11>();
        public List<ActionWithEffectsInfoV11> OnItemSteppedActions { get; set; } = new List<ActionWithEffectsInfoV11>();
        public List<ActionWithEffectsInfoV11> OnItemUseActions { get; set; } = new List<ActionWithEffectsInfoV11>();
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}