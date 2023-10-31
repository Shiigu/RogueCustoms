using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsDungeonEditor.Utils.DungeonInfoConversion.V11
{
    [Serializable]
    public class LocaleInfoV11
    {
        public string Language { get; set; }
        public List<LocaleInfoStringV11> LocaleStrings { get; set; }
    }

    [Serializable]
    public class LocaleInfoStringV11
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
