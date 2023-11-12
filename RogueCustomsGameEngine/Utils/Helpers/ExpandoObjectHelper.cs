using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace RogueCustomsGameEngine.Utils.Helpers
{
    public static class ExpandoObjectHelper
    {
        public static bool HasProperty(ExpandoObject obj, string propertyName)
        {
            if (obj is not IDictionary<string, object> objAsDictionary) return false;
            return objAsDictionary.Any(o => o.Key.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
