using System;
using System.Collections.Generic;

namespace RoguelikeGameEngine.Utils.JsonImports
{
    [Serializable]
    public class FactionInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> AlliedWith { get; set; }
        public List<string> NeutralWith { get; set; }
        public List<string> EnemiesWith { get; set; }
    }
}
