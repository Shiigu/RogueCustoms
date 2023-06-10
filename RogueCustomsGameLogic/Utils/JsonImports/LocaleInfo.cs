using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Utils.JsonImports
{
    [Serializable]
    public class LocaleInfo
    {
        public string Language { get; set; }
        public List<LocaleInfoString> LocaleStrings { get; set; }
    }

    [Serializable]
    public class LocaleInfoString
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
