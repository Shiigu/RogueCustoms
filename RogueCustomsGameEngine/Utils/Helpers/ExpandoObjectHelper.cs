using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

using RogueCustomsGameEngine.Utils.Expressions;

namespace RogueCustomsGameEngine.Utils.Helpers
{
    public static class ExpandoObjectHelper
    {
        public static bool HasProperty(ExpandoObject obj, string propertyName)
        {
            if (obj is not IDictionary<string, object> objAsDictionary) return false;
            return objAsDictionary.Any(o => o.Key.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));
        }
        public static bool HasProperty(CaseInsensitiveExpandoObject obj, string propertyName)
        {
            if (obj is null) return false;
            var properties = obj.ToDictionary();
            return properties.Any(o => o.Key.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
