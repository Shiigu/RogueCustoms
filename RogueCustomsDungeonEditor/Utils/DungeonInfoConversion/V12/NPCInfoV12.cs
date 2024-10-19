using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V12
{
    [Serializable]
    public class NPCInfoV12 : CharacterInfoV12
    {
        public bool KnowsAllCharacterPositions { get; set; }
        public int AIOddsToUseActionsOnSelf { get; set; }
    }
}