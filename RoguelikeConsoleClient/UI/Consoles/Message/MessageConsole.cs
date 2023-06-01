using RoguelikeGameEngine.Game.Entities;
using SadConsole;
using SadConsole.Input;
using SadConsole.UI;
using SadConsole.UI.Controls;
using RoguelikeConsoleClient.UI.Consoles.Containers;
using RoguelikeConsoleClient.UI.Consoles.Utils;
using SadRogue.Primitives;
using Console = SadConsole.Console;

namespace RoguelikeConsoleClient.UI.Consoles.Error
{
    public class MessageConsole : Console
    {
        public string Message { get; set; }
        public string Title { get; set; }
        private const string PressEnterToContinue = "Press Enter to Continue...";
        private readonly ScrollableMessageSubConsole MessageSubConsole;
        private readonly ControlsConsole TitleConsole;

        public MessageConsole(MessageConsoleContainer parent, int width, int height) : base(width, height)
        {
            FontSize = Font.GetFontSize(IFont.Sizes.Two);
            TitleConsole = new ControlsConsole(Width, 1)
            {
                Position = new Point(0, 1),
                FontSize = Font.GetFontSize(IFont.Sizes.Four)
            };

            MessageSubConsole = new ScrollableMessageSubConsole(Width - 4, Height - 14, 1024)
            {
                Position = new Point(1, 5),
                FontSize = Font.GetFontSize(IFont.Sizes.Two)
            };
            Children.Add(MessageSubConsole);
        }

        public void DisplayMessage()
        {
            TitleConsole.Controls.Clear();
            TitleConsole.Clear();

            var titleLabel = new Label(Title.Length)
            {
                DisplayText = Title
            };
            
            switch(Title)
            {
                case "BRIEFING":
                    titleLabel.TextColor = null;
                    break;
                case "THE END":
                    titleLabel.TextColor = new Color(0, 255, 0, 255);
                    break;
                case "ERROR":
                    titleLabel.TextColor = Color.Red;
                    break;
            }

            titleLabel.Position = new Point(((Width / 2) - (titleLabel.Width / 2)) - Title.Length - 1, 1).TranslateFont(Font.GetFontSize(IFont.Sizes.One), Font.GetFontSize(IFont.Sizes.Four));
            TitleConsole.Controls.Add(titleLabel);

            if(!Children.Contains(TitleConsole))
                Children.Add(TitleConsole);

            MessageSubConsole.PrintList(Message.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).ToList());

            this.Print((Width / 2) - (PressEnterToContinue.Length / 2) - 21, (Height / 2) - 1, PressEnterToContinue);
        }
    }
}
