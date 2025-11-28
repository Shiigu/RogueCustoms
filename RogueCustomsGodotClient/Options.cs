using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Godot;

using RogueCustomsGodotClient.Utils;

namespace RogueCustomsGodotClient
{
    public class Options
    {
        public string Localization => TranslationServer.Translate("LanguageLocale");
        public SortActionMode SortActionMode { get; set; }
        public FlashEffectMode FlashEffectMode { get; set; }
        public bool HighlightPlayerOnFloorStart { get; set; }
        public InactiveControlShowMode InactiveControlShowMode { get; set; }
    }
}
