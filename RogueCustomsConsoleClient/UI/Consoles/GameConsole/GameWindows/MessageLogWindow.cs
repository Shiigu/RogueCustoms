using SadConsole.UI.Controls;
using SadConsole.UI;
using SadConsole;
using SadRogue.Primitives;
using Themes = SadConsole.UI.Themes;
using Window = SadConsole.UI.Window;
using SadConsole.Input;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsConsoleClient.Helpers;
using Keyboard = SadConsole.Input.Keyboard;
using Console = SadConsole.Console;
using RogueCustomsConsoleClient.UI.Consoles.Containers;
using RogueCustomsConsoleClient.Resources.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RogueCustomsConsoleClient.UI.Consoles.GameConsole.GameWindows
{
    public class MessageLogWindow : Window
    {
        private Button CloseButton;
        private string TitleCaption { get; set; }
        private readonly string CloseButtonText = LocalizationManager.GetString("CloseButtonText").ToAscii();

        private ScrollBar ScrollBar;
        private Console TextAreaSubConsole;

        private MessageLogWindow(int width, int height) : base(width, height)
        {
        }

        public static Window Show(GameConsoleContainer parent, List<MessageDto> messages)
        {
            if (messages?.Any() == false) return null;
            var width = 65;
            var height = 30;

            var window = new MessageLogWindow(width, height);

            var closeButton = new Button(window.CloseButtonText.Length + 2, 1)
            {
                Text = window.CloseButtonText
            };

            window.UseKeyboard = true;
            window.IsDirty = true;
            window.UseMouse = true;
            window.Font = Game.Instance.LoadFont("fonts/Alloy_curses_12x12.font");
            window.TitleCaption = LocalizationManager.GetString("MessageWindowTitleText").ToAscii();

            var drawingArea = new DrawingArea(window.Width, window.Height);
            drawingArea.OnDraw += window.DrawWindow;

            window.Controls.Add(drawingArea);

            var square = new Rectangle(2, 2, window.Width - 3, window.Height - 5);

            var textAreaSubConsole = new Console(window.Width - 5, 24, window.Width - 5, 1024)
            {
                Position = new Point(square.X, square.Y),
                Font = Game.Instance.LoadFont("fonts/Alloy_curses_12x12.font")
            };

            foreach (var logMessage in messages)
            {
                textAreaSubConsole.Cursor.Print(new ColoredString(logMessage.Message.ToAscii(), logMessage.ForegroundColor.ToSadRogueColor(), logMessage.BackgroundColor.ToSadRogueColor()));
                if(textAreaSubConsole.Cursor.Position.X > 0)
                    textAreaSubConsole.Cursor.NewLine();
            }

            window.TextAreaSubConsole = textAreaSubConsole;
            window.Children.Add(textAreaSubConsole);

            var scrollBar = new ScrollBar(Orientation.Vertical, square.Height + 1)
            {
                IsVisible = true,
                IsEnabled = false,
                Position = new Point(square.X + square.Width - 1, square.Y - 1)
            };

            scrollBar.ValueChanged += (o, e) =>
            {
                textAreaSubConsole.View = new Rectangle(0, scrollBar.Value, textAreaSubConsole.Width, textAreaSubConsole.ViewHeight);
            };

            window.ScrollBar = scrollBar;
            window.Controls.Add(scrollBar);

            closeButton.Position = new Point((window.Width - closeButton.Width) / 2, window.Height - closeButton.Surface.Height);
            closeButton.Click += (o, e) => window.Hide();
            closeButton.Theme = null;

            window.CloseButton = closeButton;
            window.Controls.Add(closeButton);

            window.Show(true);
            window.Parent = (window.Parent as RootScreen).ActiveContainer;
            window.Center();

            return window;
        }

        public void DrawWindow(DrawingArea ds, TimeSpan delta)
        {
            if (!ds.IsDirty) return;

            var window = (ds.Parent as ControlHost).ParentConsole as Console;

            ScrollBar.Maximum = Math.Max(0, TextAreaSubConsole.Cursor.Position.Y - TextAreaSubConsole.ViewHeight);
            ScrollBar.IsEnabled = TextAreaSubConsole.Cursor.Position.Y > TextAreaSubConsole.ViewHeight;
            ScrollBar.Value = ScrollBar.IsEnabled ? ScrollBar.Maximum : 0;

            ds.Surface.Clear();
            ColoredGlyph appearance = ((Themes.DrawingAreaTheme)ds.Theme).Appearance;
            ds.Surface.Fill(appearance.Foreground, appearance.Background, null);
            ds.Surface.DrawLine(new Point(0, 0), new Point(0, Height - 3), ICellSurface.ConnectedLineThick[3], Color.WhiteSmoke);
            ds.Surface.DrawLine(new Point(0, 0), new Point(Width - 1, 0), ICellSurface.ConnectedLineThick[3], Color.WhiteSmoke);
            ds.Surface.DrawLine(new Point(Width - 1, 0), new Point(Width - 1, Height - 3), ICellSurface.ConnectedLineThick[3], Color.WhiteSmoke);
            ds.Surface.DrawLine(new Point(0, window.Height - 3), new Point(Width - 1, Height - 3), ICellSurface.ConnectedLineThick[3], Color.WhiteSmoke);
            ds.Surface.ConnectLines(ICellSurface.ConnectedLineThick);
            ds.Surface.Print((Width - TitleCaption.Length - 2) / 2, 0, $" {TitleCaption.ToAscii()} ", Color.Black, Color.WhiteSmoke);

            ds.IsDirty = true;
            ds.IsFocused = true;
        }

        public override bool ProcessKeyboard(Keyboard info)
        {
            if (info.IsKeyPressed(Keys.Up) && info.KeysPressed.Count == 1)
            {
                if(ScrollBar.IsEnabled)
                    ScrollBar.Value--;
            }
            else if (info.IsKeyPressed(Keys.Down) && info.KeysPressed.Count == 1)
            {
                if (ScrollBar.IsEnabled)
                    ScrollBar.Value++;
            }
            else if ((info.IsKeyPressed(Keys.C) || info.IsKeyPressed(Keys.Escape)) && info.KeysPressed.Count == 1)
            {
                CloseButton.InvokeClick();
            }

            return true;
        }

        public override bool ProcessMouse(MouseScreenObjectState state)
        {
            if (ScrollBar.IsEnabled)
            {
                if (state.Mouse.ScrollWheelValueChange > 0)
                    ScrollBar.Value++;
                else if (state.Mouse.ScrollWheelValueChange < 0)
                    ScrollBar.Value--;
            }
            return base.ProcessMouse(state);
        }
    }
}
