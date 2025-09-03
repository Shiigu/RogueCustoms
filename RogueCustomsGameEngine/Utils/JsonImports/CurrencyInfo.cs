using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsGameEngine.Utils.JsonImports
{
    [Serializable]
    public class CurrencyInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ConsoleRepresentation ConsoleRepresentation { get; set; }
        public List<CurrencyPileInfo> CurrencyPiles { get; set; }
    }

    [Serializable]
    public class CurrencyPileInfo
    {
        public string Id { get; set; }
        public int Minimum { get; set; }
        public int Maximum { get; set; }
    }
}
