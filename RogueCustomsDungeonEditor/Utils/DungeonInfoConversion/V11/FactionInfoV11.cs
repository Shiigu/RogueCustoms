using System;
using System.Collections.Generic;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V11
{
    [Serializable]
    public class FactionInfoV11
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> AlliedWith { get; set; }
        public List<string> NeutralWith { get; set; }
        public List<string> EnemiesWith { get; set; }
    }
}
