using Newtonsoft.Json.Linq;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RogueCustomsDungeonValidator.Utils
{
    public static class ActionValidationHelpers
    {
        public static bool HaveAllParametersBeenParsed(this Effect effect, Entity This, Entity Source, Entity Target)
        {
            dynamic paramsObject = ActionHelpers.ParseParams(This, Source, Target, 0, effect.Params);

            var paramsObjectAsDictionary = ((IDictionary<string, object>)paramsObject);

            return paramsObjectAsDictionary.Count >= effect.Params.Length;
        }

        public static bool TestFunction(this Effect effect, Entity This, Entity Source, Entity Target)
        {
            return effect.Function(This, Source, Target, 0, out _, effect.Params);
        }
    }
}
