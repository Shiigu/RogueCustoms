using SadConsole;
using SadRogue.Primitives;
using RogueCustomsConsoleClient.UI.Consoles.Containers;
using RogueCustomsConsoleClient.UI.Consoles.Utils;
using RogueCustomsConsoleClient.Resources.Localization;
using System;
using SadConsole.UI.Controls;
using RogueCustomsConsoleClient.EngineHandling;
using RogueCustomsConsoleClient.UI.Consoles.GameConsole.GameWindows;

namespace RogueCustomsConsoleClient.UI.Consoles.GameConsole
{
    public class MessageLogConsole : GameSubConsole
    {
        private ScrollableMessageSubConsole ScrollableMessageLogSubConsole;
        private string TitleCaption, MessageLogWindowButtonText;
        public Button MessageLogWindowButton;
        public MessageLogConsole(GameConsoleContainer parent) : base(parent, GameConsoleConstants.MessageLogCellWidth, GameConsoleConstants.MessageLogCellHeight)
        {
            Build();
        }

        public void Build()
        {
            base.Build();
            TitleCaption = LocalizationManager.GetString("MessageHeaderText").ToAscii();
            DefaultBackground = Color.Black;
            Font = Game.Instance.LoadFont("fonts/IBMCGA.font");
            RefreshOnlyOnStatusUpdate = true;
            ScrollableMessageLogSubConsole = new ScrollableMessageSubConsole(Width - 3, Height - 2, 200)
            {
                Position = new Point(1, 1)
            };
            Children.Add(ScrollableMessageLogSubConsole);
            MessageLogWindowButtonText = $" {LocalizationManager.GetString("MessageLogButtonText")} ".ToAscii();
            MessageLogWindowButton = new Button(MessageLogWindowButtonText.Length, 1)
            {
                Position = new Point((Width - MessageLogWindowButtonText.Length) / 2, Height - 1),
                Text = MessageLogWindowButtonText
            };
            MessageLogWindowButton.Click += MessageLogWindowButton_Click;
            Controls.Add(MessageLogWindowButton);
        }

        private void MessageLogWindowButton_Click(object? sender, EventArgs e)
        {
            try
            {
                ParentContainer.ActiveWindow = MessageLogWindow.Show(ParentContainer, ParentContainer.LatestDungeonStatus.LogMessages);
            }
            catch (Exception)
            {
                ParentContainer.ChangeConsoleContainerTo(ConsoleContainers.Message, ConsoleContainers.Main, LocalizationManager.GetString("ErrorMessageHeader"), LocalizationManager.GetString("ErrorText"));
            }
        }

        public override void Update(TimeSpan delta)
        {
            this.Clear();

            var dungeonStatus = ParentContainer.LatestDungeonStatus;

            var square = new Rectangle(0, 0, Width, Height);
            var title = $" {TitleCaption} ";

            this.DrawBox(square, ShapeParameters.CreateBorder(new ColoredGlyph(Color.Violet, Color.Black, 178)));
            this.Print((square.Width - title.Length) / 2, 0, title);

            if (ParentContainer.RequiresRefreshingDungeonState)
            {
                ScrollableMessageLogSubConsole.PrintList(dungeonStatus.LogMessages);
            }

            base.Update(delta);
        }
    }
}
