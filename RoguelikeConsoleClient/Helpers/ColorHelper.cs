using RoguelikeGameEngine.Utils.Representation;
using SadRogue.Primitives;

namespace RoguelikeConsoleClient.Helpers
{
    public static class ColorHelper
    {
        public static Color ToSadRogueColor(this GameColor color)
        {
            return Color.FromNonPremultiplied(color.R, color.G, color.B, color.A);
        }
    }
}
