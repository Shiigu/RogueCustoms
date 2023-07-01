using RogueCustomsConsoleClient.Helpers;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using SadConsole;
using SadConsole.UI.Controls;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using Console = SadConsole.Console;

namespace RogueCustomsConsoleClient.UI.Consoles.Utils
{
    public class ScrollableMessageSubConsole : Console
    {
        private readonly Console ControlsContainer;
        public readonly ScrollBar ScrollBar;

        public ScrollableMessageSubConsole(int width, int height, int bufferHeight) : base(width - 1, height, width - 1, bufferHeight)
        {
            ControlsContainer = new Console(1, height);

            ScrollBar = new ScrollBar(Orientation.Vertical, height);
            ScrollBar.IsVisible = true;
            ScrollBar.IsEnabled = false;
            ScrollBar.ValueChanged += ScrollBar_ValueChanged;

            var controlHost = new SadConsole.UI.ControlHost();
            controlHost.Add(ScrollBar);
            ControlsContainer.SadComponents.Add(controlHost);
            ControlsContainer.Position = new Point(Position.X + width, Position.Y);
            ControlsContainer.IsVisible = true;
            ControlsContainer.FocusOnMouseClick = false;

            UseMouse = true;

            Children.Add(ControlsContainer);
        }

        private void ScrollBar_ValueChanged(object? sender, EventArgs e)
        {
            //Display viewable content based on our scroll offset.
            View = new Rectangle(0, ScrollBar.Value, Width, ViewHeight);
        }

        public void PrintList(List<string> textList)
        {
            this.Clear();
            Cursor.Position = new Point(0, 0);
            
            for (int i = 0; i < textList.Count; i++)
            {
                Cursor.Print(textList[i].ToAscii());
                if (i < textList.Count - 1)
                    Cursor.NewLine();
            }
            ScrollBar.Maximum = Math.Max(0, Cursor.Position.Y + 1 - ViewHeight);
            ScrollBar.IsEnabled = Cursor.Position.Y > ViewHeight;
            ScrollBar.Value = ScrollBar.IsEnabled ? ScrollBar.Maximum : 0;
        }
        public void PrintColoredList(List<MessageDto> messageList)
        {
            this.Clear();
            Cursor.Position = new Point(0, 0);

            for (int i = 0; i < messageList.Count; i++)
            {
                var logMessage = messageList[i];
                Cursor.Print(new ColoredString(logMessage.Message.ToAscii(), logMessage.ForegroundColor.ToSadRogueColor(), logMessage.BackgroundColor.ToSadRogueColor()));
                if (i < messageList.Count - 1)
                    Cursor.NewLine();
            }
            ScrollBar.Maximum = Math.Max(0, Cursor.Position.Y + 1 - ViewHeight);
            ScrollBar.IsEnabled = Cursor.Position.Y > ViewHeight;
            ScrollBar.Value = ScrollBar.IsEnabled ? ScrollBar.Maximum : 0;
        }
    }
}
