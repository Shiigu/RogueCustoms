using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;

namespace RogueCustomsGameEngine.Utils.JsonImports
{
    [Serializable]
    public class NPCInfo : CharacterInfo
    {
        public bool KnowsAllCharacterPositions { get; set; }
        public int AIOddsToUseActionsOnSelf { get; set; }
    }
}