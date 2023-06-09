﻿using System.Collections.Generic;
using System.Dynamic;

namespace RogueCustomsGameEngine.Utils.Helpers
{
    public static class ExpandoObjectHelper
    {
        public static bool HasProperty(ExpandoObject obj, string propertyName)
        {
            return obj != null && (obj as IDictionary<string, object>)?.ContainsKey(propertyName) == true;
        }
    }
}
