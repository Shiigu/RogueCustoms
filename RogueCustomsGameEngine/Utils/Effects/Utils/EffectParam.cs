using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.Entities.Interfaces;

#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
namespace RogueCustomsGameEngine.Utils.Effects.Utils
{
    public class EffectCallerParams
    {
        public Entity This { get; set; }
        public Entity Source { get; set; }
        public ITargetable Target { get; set; }
        public List<EffectParam> Params { get; set; }

        public ITargetable OriginalTarget { get; set; }

        public string this[string key]
        {
            get
            {
                var foundParam = Params.Find(ls => ls.ParamName.Equals(key, StringComparison.InvariantCultureIgnoreCase));

                return foundParam == null ? throw new MissingFieldException($"The Parameter {key} was not found.") : foundParam.Value;
            }
        }
    }

    public class EffectParam
    {
        public string ParamName { get; set; }
        public string Value { get; set; }
    }
}
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
