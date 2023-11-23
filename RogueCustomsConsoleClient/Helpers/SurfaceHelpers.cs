using SadConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsConsoleClient.Helpers
{
    public static class SurfaceHelpers
    {
        public static void Print(this ISurface surface, int x, int y, string text, bool toAscii)
        {
            if(toAscii)
                surface.Print(x, y, text.ToAscii());
            else
                surface.Print(x, y, text);
        }
        public static void SetGlyph(this ISurface surface, int x, int y, ColoredGlyph coloredGlyph)
        {
            surface.SetGlyph(x, y, coloredGlyph.Glyph, coloredGlyph.Foreground, coloredGlyph.Background);
        }
    }
}
