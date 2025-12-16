using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.JsonImports;
using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsGameEngine.Game.DungeonStructure
{
    [Serializable]
    public class QualityLevel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int MinimumAffixes { get; set; }
        public int MaximumAffixes { get; set; }
        public QualityLevelNameAttachment AttachesWhatToItemName { get; set; }
        public GameColor ItemNameColor { get; set; }

        public QualityLevel(QualityLevelInfo info, Locale locale)
        {
            Id = info.Id;
            Name = locale[info.Name];
            MinimumAffixes = info.MinimumAffixes;
            MaximumAffixes = info.MaximumAffixes;
            AttachesWhatToItemName = Enum.Parse<QualityLevelNameAttachment>(info.AttachesWhatToItemName);
            ItemNameColor = info.ItemNameColor;
        }

        public bool IsBetween(QualityLevel min, QualityLevel max)
        {
            return this.MaximumAffixes >= min.MaximumAffixes && this.MaximumAffixes <= max.MaximumAffixes;
        }
    }
}
