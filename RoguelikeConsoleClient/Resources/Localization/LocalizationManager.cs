using SadConsole;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeConsoleClient.Resources.Localization
{
    public class LocalizationManager
    {
        private static List<Localization> Localizations = new List<Localization>();
        private static Localization Instance => Localizations.Find(l => l.Name.Equals(Settings.Default.Language));

        public static string CurrentLocale => Instance.Locale;

        public static string GetString(string key)
        {
            return Instance[key];
        }

        public static Dictionary<string, string> GetLocalizationDisplayData()
        {
            var localizationDictionary = new Dictionary<string, string>();

            Localizations.ForEach(l => localizationDictionary[l.Locale] = l.Name);

            return localizationDictionary;
        }

        public static void Build()
        {
            foreach (var filePath in Directory.GetFiles("./Resources/Localization/Locales/", "*.txt"))
            {
                var lines = File.ReadAllLines(filePath);
                var localization = new Localization();
                foreach (var line in lines)
                {
                    var lineSplit = line.Split('=', 2, StringSplitOptions.TrimEntries);
                    if (lineSplit.Length < 2) continue;
                    var key = lineSplit[0];
                    var value = string.Join('\n', lineSplit[1].Split(new string[] { "\\r", "\\n" }, StringSplitOptions.TrimEntries));
                    if (lineSplit[0].Equals("LanguageName"))
                        localization.Name = lineSplit[1];
                    else if (lineSplit[0].Equals("LanguageLocale"))
                        localization.Locale = lineSplit[1];
                    else
                        localization.LocaleStrings.Add(new Locale { Key = key, Value = value });
                }
                if(string.IsNullOrWhiteSpace(localization.Name))
                    throw new Exception($"{Path.GetFileName(filePath)} is not a valid Localization file because it lacks a valid LanguageName");
                if(string.IsNullOrWhiteSpace(localization.Locale))
                    throw new Exception($"{Path.GetFileName(filePath)} is not a valid Localization file because it lacks a valid LanguageLocale");
                Localizations.Add(localization);
            }
        }
    }

    public class Localization
    {
        public string Name { get; set; }
        public string Locale { get; set; }

        public List<Locale> LocaleStrings { get; set; }
        public string this[string key]
        {
            get
            {
                return LocaleStrings.Find(ls => ls.Key.Equals(key)).Value;
            }
        }

        public Localization()
        {
            LocaleStrings = new List<Locale>();
        }
    }

    public class Locale
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
