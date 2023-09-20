using RogueCustomsGameEngine.Utils.Representation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RogueCustomsDungeonEditor.Utils
{
    public static class GameColorHelper
    {
        public static Color ToColor(this string gameColorString)
        {
            if (gameColorString == null)
            {
                throw new ArgumentNullException(nameof(gameColorString));
            }

            string[] rgbaComponents = gameColorString.Split(',');

            if (rgbaComponents.Length != 4)
            {
                throw new FormatException("Invalid RGBA format. Expected 'Red,Green,Blue,Alpha'.");
            }

            int red = int.Parse(rgbaComponents[0]);
            int green = int.Parse(rgbaComponents[1]);
            int blue = int.Parse(rgbaComponents[2]);
            int alpha = int.Parse(rgbaComponents[3]);

            return Color.FromArgb(alpha, red, green, blue);
        }

        public static Color ToColor(this GameColor gameColor)
        {
            return Color.FromArgb(gameColor.A, gameColor.R, gameColor.G, gameColor.B);
        }

        public static string ToParamString(this GameColor color)
        {
            return $"{color.R},{color.G},{color.B},{color.A}";
        }
    }
}
