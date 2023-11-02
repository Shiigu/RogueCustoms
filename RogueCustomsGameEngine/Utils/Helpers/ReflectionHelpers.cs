using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RogueCustomsGameEngine.Utils.Helpers
{
    public class ReflectionHelpers
    {
        public static List<Type> GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return
              assembly.GetTypes()
                      .Where(t => string.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
                      .ToList();
        }
    }
}
