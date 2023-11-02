using RogueCustomsGameEngine.Game.Entities;
using SadConsole;
using SadConsole.Input;
using SadConsole.UI;
using SadConsole.UI.Controls;
using RogueCustomsConsoleClient.UI.Consoles.Containers;
using RogueCustomsConsoleClient.UI.Consoles.Utils;
using SadRogue.Primitives;
using Console = SadConsole.Console;
using RogueCustomsConsoleClient.Resources.Localization;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using static SadConsole.Settings;
using System.ComponentModel.DataAnnotations;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsConsoleClient.Helpers;

namespace RogueCustomsConsoleClient.UI.Consoles.Error
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public class MessageConsole : Console
    {
        public string Message { get; set; }
        public string Title { get; set; }

        private readonly ScrollableMessageSubConsole MessageSubConsole;
        private readonly ControlsConsole TitleConsole;

        public MessageConsole(int width, int height) : base(width, height)
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
            this.Clear();
            var briefingMessageHeader = LocalizationManager.GetString("BriefingMessageHeader").ToAscii();
            var theEndMessageHeader = LocalizationManager.GetString("TheEndMessageHeader").ToAscii();
            var errorMessageHeader = LocalizationManager.GetString("ErrorMessageHeader").ToAscii();
            var pressEnterToContinueText = LocalizationManager.GetString("PressEnterText").ToAscii();

            var titleLabel = new Label(Title.Length)
            {
                DisplayText = Title
            };

            if(Title.Equals(briefingMessageHeader))
                titleLabel.TextColor = null;
            else if(Title.Equals(theEndMessageHeader))
                titleLabel.TextColor = new Color(0, 255, 0, 255);
            else if(Title.Equals(errorMessageHeader))
                titleLabel.TextColor = Color.Red;

            titleLabel.Position = new Point((Width / 3) + titleLabel.Width, 1).TranslateFont(Font.GetFontSize(IFont.Sizes.One), Font.GetFontSize(IFont.Sizes.Four));
            TitleConsole.Controls.Add(titleLabel);

            if(!Children.Contains(TitleConsole))
                Children.Add(TitleConsole);

            var splitMessage = Message.Split(new[] { "\r\n", "\n" }.ToArray(), StringSplitOptions.None).SplitByLengthWithWholeWords(MessageSubConsole.Width / 2).ToList();

            MessageSubConsole.PrintList(splitMessage);

            this.Print((Width / 4) - (pressEnterToContinueText.Length / 2), (Height / 2) - 2, pressEnterToContinueText);
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
