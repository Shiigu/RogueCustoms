using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V11
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.

    [Serializable]
    public class TrapInfoV11 : ClassInfoV11
    {
        public bool StartsVisible { get; set; }
        public string Power { get; set; }
        public List<ActionWithEffectsInfoV11> OnItemSteppedActions { get; set; } = new List<ActionWithEffectsInfoV11>();
    }

    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}