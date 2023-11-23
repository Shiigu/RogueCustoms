using SadConsole.UI.Controls;
using SadConsole.UI;
using SadConsole;
using SadRogue.Primitives;
using Window = SadConsole.UI.Window;
using SadConsole.Input;
using Console = SadConsole.Console;
using RogueCustomsConsoleClient.UI.Consoles;
using System;
using System.Linq;

namespace RogueCustomsConsoleClient.UI.Windows
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    #pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
    public sealed class PromptBox : Window
    {
        private Button AffirmativeButton { get; set; }
        private Button NegativeButton { get; set; }
        private string TitleCaption { get; set; }
        private string[] MessageLines { get; set; }
        private Color WindowColor { get; set; }

        private PromptBox(int width, int height) : base(width, height) { }

        public override bool ProcessKeyboard(Keyboard info)
        {
            if((info.IsKeyPressed(Keys.Enter) || info.IsKeyPressed(Keys.Y) || info.IsKeyPressed(Keys.S)) && info.KeysPressed.Count == 1)
                AffirmativeButton.InvokeClick();
            if((info.IsKeyPressed(Keys.Escape) || info.IsKeyPressed(Keys.N)) && info.KeysPressed.Count == 1)
                NegativeButton.InvokeClick();

            return true;
        }

        public static Window? Show(ColoredString message, string affirmativeButtonText, string negativeButtonText, string titleText, Color windowColor, Action affirmativeCallback = null, Action negativeCallback = null)
        {
            if (string.IsNullOrWhiteSpace(message.ToString())) return null;
            var messageAsString = message.ToString();
            string[] linesInMessage;
            if (messageAsString.Contains("\r\n"))
                linesInMessage = messageAsString.Split("\r\n");
            else if (messageAsString.Contains('\n'))
                linesInMessage = messageAsString.Split("\n");
            else
                linesInMessage = new string[1] { messageAsString };
            var longestLineLength = Math.Max(linesInMessage.Max(l => l.Length), titleText.Length);
            int width = longestLineLength + 4;
            int buttonWidth = affirmativeButtonText.Length + 2;

            if (buttonWidth < 9)
                buttonWidth = 9;

            if (width < buttonWidth + 4)
                width = buttonWidth + 4;

            var affirmativeButton = new Button(buttonWidth, 1)
            {
                Text = affirmativeButtonText.ToAscii(),
            };

            var negativeButton = new Button(buttonWidth, 1)
            {
                Text = negativeButtonText.ToAscii(),
            };

            var window = new PromptBox(Math.Max(width, titleText.Length), 3 + linesInMessage.Length + affirmativeButton.Surface.Height)
            {
                MessageLines = linesInMessage,
                TitleCaption = titleText.ToAscii(),
                AffirmativeButton = affirmativeButton,
                NegativeButton = negativeButton,
                WindowColor = windowColor,
                Font = Game.Instance.LoadFont("fonts/Alloy_curses_12x12.font")
            };

            message.IgnoreBackground = true;

            var printArea = new DrawingArea(window.Width, window.Height);
            printArea.OnDraw += window.DrawWindow;

            window.Controls.Add(printArea);

            affirmativeButton.Position = new Point(2, window.Height - affirmativeButton.Surface.Height);
            affirmativeButton.Click += (o, e) => {
                window.DialogResult = true;
                window.Hide();
                affirmativeCallback?.Invoke();
            };

            window.Controls.Add(affirmativeButton);

            negativeButton.Position = new Point(window.Width - negativeButton.Width - 2, window.Height - negativeButton.Surface.Height);
            negativeButton.Click += (o, e) => {
                window.DialogResult = false;
                window.Hide();
                negativeCallback?.Invoke();
            };

            window.Controls.Add(negativeButton);
            affirmativeButton.IsFocused = true;
            window.Show(true);
            window.Parent = (window.Parent as RootScreen)?.ActiveContainer;
            window.Center();

            return window;
        }

        public void DrawWindow(DrawingArea ds, TimeSpan delta)
        {
            if (!ds.IsDirty) return;

            if (ds.Parent is not ControlHost host) return;
            if (host.ParentConsole is not Console window) return;

            var square = new Rectangle(0, 0, window.Width, window.Height);

            var appearance = ds.ThemeState.Normal;
            ds.Surface.Fill(appearance.Foreground, appearance.Background, null);
            ds.Surface.DrawBox(square, ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThick, new ColoredGlyph(WindowColor, Color.Black)));
            ds.Surface.Print((square.Width - TitleCaption.Length - 2) / 2, 0, $" {TitleCaption} ", Color.Black, WindowColor);
            for (int i = 0; i < MessageLines.Length; i++)
            {
                ds.Surface.Print(2, 2 + i, MessageLines[i].Replace("\r\n", "").Replace("\n", "").ToAscii());
            }
            ds.IsDirty = true;
            ds.IsFocused = true;
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    #pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}
