using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsGameEngine.Game.DungeonStructure
{

    [Serializable]
    public class CurrencyPile
    {
        public string Id { get; set; }
        public int Minimum { get; set; }
        public int Maximum { get; set; }

        public CurrencyPile(CurrencyPileInfo info)
        {
            Id = info.Id;
            Minimum = info.Minimum;
            Maximum = info.Maximum;
        }
    }
}
