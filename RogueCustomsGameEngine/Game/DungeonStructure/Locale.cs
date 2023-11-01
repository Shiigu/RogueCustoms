using RogueCustomsGameEngine.Utils.JsonImports;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Game.DungeonStructure
{
    public class Locale
    {
        [MaxLength(2)]
        private string Language { get; set; }
        private List<LocaleString> LocaleStrings { get; set; }

        public bool ContainsKey(string key) => LocaleStrings.Exists(ls => ls.Key == key);

        public bool IsValueInAKey(string value) => LocaleStrings.Exists(ls => ls.Value == value);

        public string this[string key]
        {
            get {
                var localeString = LocaleStrings.Find(ls => ls.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));
                // Don't break the game just because it can't find a certain locale key. Just return the key itself for a weird display.
                if (localeString == null)
                    return key;
                return localeString.Value;
            }
        }

        public Locale() { }

        public Locale(LocaleInfo localeInfo)
        {
            Language = localeInfo.Language;
            LocaleStrings = new List<LocaleString>();
            localeInfo.LocaleStrings.ForEach(ls => LocaleStrings.Add(new LocaleString { Key = ls.Key, Value = ls.Value }));
        }
    }

    public class LocaleString
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
