using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsGodotClient.Entities
{
    public class SaveGame
    {
        public string DungeonVersion { get; set; }
        public string DungeonName { get; set; }
        public string FloorName { get; set; }
        public string PlayerName { get; set; }
        public ConsoleRepresentation PlayerRepresentation { get; set; }
        public int PlayerLevel { get; set; }
        public bool IsPlayerDead { get; set; }
        public DateTime SaveDate { get; set; }
        public string DungeonData { get; set; }
    }
}
