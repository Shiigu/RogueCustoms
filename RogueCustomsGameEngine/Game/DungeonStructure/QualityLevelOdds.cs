using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsGameEngine.Game.DungeonStructure
{
    [Serializable]
    public class QualityLevelOdds
    {
        public string Id { get; set; }
        public QualityLevel QualityLevel { get; set; }
        public int ChanceToPick { get; set; }

        public QualityLevelOdds(QualityLevelOddsInfo info, Dungeon dungeon)
        {
            Id = info.Id;
            ChanceToPick = info.ChanceToPick;
            QualityLevel = dungeon.QualityLevels.FirstOrDefault(ql => ql.Id.Equals(info.Id));
        }
    }
}
