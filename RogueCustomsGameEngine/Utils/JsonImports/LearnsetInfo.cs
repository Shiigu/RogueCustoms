using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsGameEngine.Utils.JsonImports
{
    [Serializable]
    public class LearnsetInfo
    {
        public string Id { get; set; }
        public List<LearnsetEntryInfo> Entries { get; set; }
    }

    [Serializable]
    public class LearnsetEntryInfo
    {
        public int Level { get; set; }
        public string LearnedScriptId { get; set; }
        public string ForgotScriptId { get; set; }
    }
}
