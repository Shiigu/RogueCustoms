using RoguelikeGameEngine.Utils.JsonImports;
using System.Collections.Generic;

namespace RoguelikeGameEngine.Utils.Helpers
{
    public static class InfoHelpers
    {
        public static ClassInfo GetEntityInfoByName(this List<ClassInfo> entityInfos, string name)
        {
            return entityInfos.Find(ei => ei.Name.Equals(name));
        }
    }
}
