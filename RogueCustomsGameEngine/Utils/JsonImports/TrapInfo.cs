using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;

namespace RogueCustomsGameEngine.Utils.JsonImports
{
    [Serializable]
    public class TrapInfo : ClassInfo
    {
        public bool StartsVisible { get; set; }
        public string Power { get; set; }
        public ActionWithEffectsInfo OnStepped { get; set; } = new ActionWithEffectsInfo();
    }
}