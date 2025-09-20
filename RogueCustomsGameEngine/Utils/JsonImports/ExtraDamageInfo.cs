using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Utils.JsonImports
{
    [Serializable]
    public class ExtraDamageInfo
    {
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public string Element { get; set; }
    }
}
