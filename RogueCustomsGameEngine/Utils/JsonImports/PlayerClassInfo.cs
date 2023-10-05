using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;

namespace RogueCustomsGameEngine.Utils.JsonImports
{
    [Serializable]
    public class PlayerClassInfo : CharacterInfo
    {
        public bool RequiresNamePrompt { get; set; }
    }
}