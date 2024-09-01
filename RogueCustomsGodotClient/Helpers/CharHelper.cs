using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsGodotClient.Helpers
{
    public static class CharHelper
    {
        public static string ToBbCodeRepresentation(this ConsoleRepresentation consoleRepresentation)
        {
            return $"[bgcolor=#{consoleRepresentation.BackgroundColor.R:X2}{consoleRepresentation.BackgroundColor.G:X2}{consoleRepresentation.BackgroundColor.B:X2}{consoleRepresentation.BackgroundColor.A:X2}][color=#{consoleRepresentation.ForegroundColor.R:X2}{consoleRepresentation.ForegroundColor.G:X2}{consoleRepresentation.ForegroundColor.B:X2}{consoleRepresentation.ForegroundColor.A:X2}]{consoleRepresentation.Character}[/color][/bgcolor]";
        }
    }
}
