using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Utils.InputsAndOutputs
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public class MessageDto
    {
        public string Message { get; set; }
        public GameColor BackgroundColor { get; set; } = new GameColor(Color.Black);
        public GameColor ForegroundColor { get; set; } = new GameColor(Color.Transparent);
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
