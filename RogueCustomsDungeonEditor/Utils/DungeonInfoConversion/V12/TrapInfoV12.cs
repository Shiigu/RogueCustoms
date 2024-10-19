﻿using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V12
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.

    [Serializable]
    public class TrapInfoV12 : ClassInfoV12
    {
        public bool StartsVisible { get; set; }
        public string Power { get; set; }
        public List<ActionWithEffectsInfoV12> OnItemSteppedActions { get; set; } = new List<ActionWithEffectsInfoV12>();
    }

    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}