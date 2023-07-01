using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Utils.InputsAndOutputs
{
    public class MessageDto
    {
        public string Message { get; set; }
        public GameColor BackgroundColor { get; set; } = new GameColor(Color.Black);
        public GameColor ForegroundColor { get; set; } = new GameColor(Color.Transparent);
    }
}
