using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Game.DungeonStructure
{
    public class Flag
    {
        public string Key { get; set; }
        public int Value { get; set; }
        public bool RemoveOnFloorChange { get; set; }

        public Flag() { }

        public Flag(string key, int value, bool removeOnFloorChange) {
            Key = key;
            Value = value;
            RemoveOnFloorChange = removeOnFloorChange;
        }
    }
}
