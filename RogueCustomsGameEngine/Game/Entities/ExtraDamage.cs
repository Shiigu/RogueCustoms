using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Game.Entities
{
    [Serializable]
    public class ExtraDamage
    {
        public int MinimumDamage { get; set; }
        public int MaximumDamage { get; set; }
        public Element Element { get; set; }
    }
}
