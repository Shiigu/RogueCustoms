using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsGameEngine.Utils.JsonImports
{
    [Serializable]
    public class QualityLevelInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int MinimumAffixes { get; set; }
        public int MaximumAffixes { get; set; }
        public string AttachesWhatToItemName { get; set; }
        public GameColor ItemNameColor { get; set; }
    }
}
