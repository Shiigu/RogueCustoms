using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Game.DungeonStructure
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    [Serializable]
    public class Flag
    {
        public string Key { get; set; }
        public object Value { get; set; }
        public bool RemoveOnFloorChange { get; set; }

        public Flag() { }

        public Flag(string key, object value, bool removeOnFloorChange) {
            Key = key;
            Value = value;
            RemoveOnFloorChange = removeOnFloorChange;
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
