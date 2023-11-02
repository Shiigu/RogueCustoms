using SadConsole.UI.Controls;
using SadConsole.UI;
using SadConsole;
using SadRogue.Primitives;
using Themes = SadConsole.UI.Themes;
using Window = SadConsole.UI.Window;
using SadConsole.Input;
using Console = SadConsole.Console;
using RogueCustomsConsoleClient.UI.Consoles;
using System;
using System.Linq;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsConsoleClient.Resources.Localization;
using RogueCustomsGameEngine.Utils.Representation;
using RogueCustomsConsoleClient.Helpers;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsConsoleClient.Utils;
using RogueCustomsConsoleClient.UI.Consoles.GameConsole;

namespace RogueCustomsConsoleClient.UI.Windows
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public class EntityDetailWindow : Window
    {
        private Button CloseButton { get; set; }
        private string TitleCaption { get; set; }
        private string[] DescriptionLines { get; set; }
        private EntityDetailDto Details { get; set; }
        private Color WindowColor { get; set; }

        private EntityDetailWindow(int width, int height) : base(width, height) { }

        public override bool ProcessKeyboard(Keyboard info)
        {
            if((info.IsKeyPressed(Keys.Escape) || info.IsKeyPressed(Keys.Enter)) && info.KeysPressed.Count == 1)
                CloseButton.InvokeClick();

            return true;
        }

        public static Window? Show(EntityDetailDto entityDetail)
        {
            if (entityDetail == null) return null;

            var linesInDescription = entityDetail.Description.Split(
                new[] { "\r\n", "\n" }, StringSplitOptions.None
                );
            var splitWrappedDescription = linesInDescription.SplitByLengthWithWholeWords(GameConsoleConstants.EntityDetailWindowMaxLength).ToList();

            var titleText = LocalizationManager.GetString("EntityDetailTitleText").ToAscii();
            var okButtonText = LocalizationManager.GetString("OKButtonText").ToAscii();

            var longestLineLength = Math.Max(splitWrappedDescription.Max(l => l.Length), titleText.Length);
            int width = longestLineLength + 4;
            int buttonWidth = okButtonText.Length + 2;

            if (buttonWidth < 9)
                buttonWidth = 9;

            if (width < buttonWidth + 4)
                width = buttonWidth + 4;

            var okButton = new Button(buttonWidth, 1)
            {
                Text = okButtonText.ToAscii(),
            };

            var window = new EntityDetailWindow(Math.Max(width, titleText.Length), 7 + splitWrappedDescription.Count + okButton.Surface.Height)
            {
                DescriptionLines = splitWrappedDescription.ToArray(),
                TitleCaption = titleText.ToAscii(),
                CloseButton = okButton,
                WindowColor = Color.Green,
                Details = entityDetail,
                Font = Game.Instance.LoadFont("fonts/Alloy_curses_12x12.font")
            };

            var printArea = new DrawingArea(window.Width, window.Height);
            printArea.OnDraw += window.DrawWindow;

            window.Controls.Add(printArea);

            okButton.Position = new SadRogue.Primitives.Point((window.Width - okButton.Surface.Width) / 2, window.Height - okButton.Surface.Height);
            okButton.Click += (o, e) => {
                window.DialogResult = true;
                window.Hide();
            };
            okButton.Theme = null;

            window.Controls.Add(okButton);
            okButton.IsFocused = true;
            window.Show(true);
            window.Parent = (window.Parent as RootScreen)?.ActiveContainer;
            window.Center();

            return window;
        }

        public void DrawWindow(DrawingArea ds, TimeSpan delta)
        {
            if (!ds.IsDirty) return;

            var window = ((ds.Parent as ControlHost)?.ParentConsole) as Console;

            var square = new Rectangle(0, 0, window.Width, window.Height);

            ColoredGlyph appearance = ((Themes.DrawingAreaTheme)ds.Theme).Appearance;
            ds.Surface.Fill(appearance.Foreground, appearance.Background, null);
            ds.Surface.DrawBox(square, ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThick, new ColoredGlyph(WindowColor, Color.Black)));
            ds.Surface.Print((square.Width - TitleCaption.Length - 2) / 2, 0, $" {TitleCaption} ", Color.Black, WindowColor);

            ds.Surface.Print(2, 2, Details.Name.ToAscii());

            var representationGlyph = new ColoredGlyph(Details.ConsoleRepresentation.ForegroundColor.ToSadRogueColor(), Details.ConsoleRepresentation.BackgroundColor.ToSadRogueColor(), Details.ConsoleRepresentation.Character.ToGlyph());
            ds.Surface.SetGlyph(2, 4, representationGlyph);

            for (int i = 0; i < DescriptionLines.Length; i++)
            {
                ds.Surface.Print(2, 6 + i, DescriptionLines[i].Replace("\r\n", "").Replace("\n", "").ToAscii());
            }
            ds.IsDirty = true;
            ds.IsFocused = true;
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
