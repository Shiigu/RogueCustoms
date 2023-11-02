using RogueCustomsGameEngine.Utils.JsonImports;
using System.Collections.Generic;

namespace RogueCustomsGameEngine.Utils.Helpers
{
    public static class InfoHelpers
    {
        #pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
        public static ClassInfo GetEntityInfoByName(this List<ClassInfo> entityInfos, string name)
        {
            return entityInfos.Find(ei => ei.Name.Equals(name));
        }
        #pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
    }
}
