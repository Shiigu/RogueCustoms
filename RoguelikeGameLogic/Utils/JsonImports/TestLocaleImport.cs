using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeGameEngine.Utils.JsonImports
{
    [Serializable]
    public class TestLocaleImport
    {
        public List<LocaleInfo> Locales { get; set; }
    }
}
