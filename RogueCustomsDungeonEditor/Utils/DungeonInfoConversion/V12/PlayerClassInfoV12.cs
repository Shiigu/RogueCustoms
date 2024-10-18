using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V12
{
    [Serializable]
    public class PlayerClassInfoV12 : CharacterInfoV12
    {
        public bool RequiresNamePrompt { get; set; }
    }
}