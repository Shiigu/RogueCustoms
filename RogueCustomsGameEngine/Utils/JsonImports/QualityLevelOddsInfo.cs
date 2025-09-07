using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Utils.JsonImports
{

    [Serializable]
    public class QualityLevelOddsInfo
    {
        public string Id { get; set; }
        public int ChanceToPick { get; set; }
    }
}
