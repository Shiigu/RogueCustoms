using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V11
{
    [Serializable]
    public class PlayerClassInfoV11 : CharacterInfoV11
    {
        public bool RequiresNamePrompt { get; set; }
    }
}