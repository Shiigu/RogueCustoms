using RogueCustomsGameEngine.Utils.Representation;
using SadRogue.Primitives;

namespace RogueCustomsConsoleClient.Helpers
{
    public static class ColorHelper
    {
        public static Color ToSadRogueColor(this GameColor color)
        {
            return Color.FromNonPremultiplied(color.R, color.G, color.B, color.A);
        }
    }
}
