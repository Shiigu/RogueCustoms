using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Utils.JsonImports;

namespace RogueCustomsGameEngine.Game.DungeonStructure
{
    [Serializable]
    public class ActionSchool
    {
        public readonly string Id;
        public readonly string Name;

        public ActionSchool(ActionSchoolInfo schoolInfo, Locale Locale)
        {
            Id = schoolInfo.Id;
            Name = Locale[schoolInfo.Name];
        }
    }
}
