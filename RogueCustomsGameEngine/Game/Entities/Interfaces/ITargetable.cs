using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Game.Entities.Interfaces
{
    public interface ITargetable
    {
        public GamePoint Position { get; set; }
    }
}
