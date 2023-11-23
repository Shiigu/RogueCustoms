using SadConsole;
using SadConsole.UI;
using SadConsole.UI.Controls;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsConsoleClient.UI.Consoles.Utils
{
    public class RogueProgressBar : ProgressBar
    {
        Color? _backgroundColor = null;

        /// <summary>
        /// The color to print the filled part of the progress bar.
        /// </summary>
        [DataMember]
        public Color? BackgroundColor
        {
            get => _backgroundColor;
            set { _backgroundColor = value; IsDirty = true; }
        }
        public RogueProgressBar(int width, int height, HorizontalAlignment horizontalAlignment) : base(width, height, horizontalAlignment)
        {
        }

        public RogueProgressBar(int width, int height, VerticalAlignment verticalAlignment) : base(width, height, verticalAlignment)
        {
        }
        /// <inheritdoc/>
        protected override void RefreshThemeStateColors(Colors colors)
        {
            base.RefreshThemeStateColors(colors);

            if (BackgroundColor != null)
                ThemeState.Normal.Foreground = BackgroundColor.Value;
        }
    }
}
