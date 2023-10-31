using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V11
{
    [Serializable]
    public class NPCInfoV11 : CharacterInfoV11
    {
        public bool KnowsAllCharacterPositions { get; set; }
        public int AIOddsToUseActionsOnSelf { get; set; }
    }
}