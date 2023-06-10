using SadConsole;
using SadConsole.UI;
using SadConsole.UI.Controls;
using RogueCustomsConsoleClient.EngineHandling;
using RogueCustomsConsoleClient.UI.Consoles.Containers;
using RogueCustomsConsoleClient.UI.Consoles.GameConsole.GameWindows;
using SadRogue.Primitives;
using Console = SadConsole.Console;
using RogueCustomsConsoleClient.Resources.Localization;
using System;

namespace RogueCustomsConsoleClient.UI.Consoles.MenuConsole
{
    public class MainMenuConsole : MenuSubConsole
    {
        private string GameNameText, SelectFileText, OptionsText, ExitText;

        private Label GameName;
        private Button SelectFileButton, OptionsButton, ExitButton;
        private ControlsConsole LogoConsole;

        public MainMenuConsole(MenuConsoleContainer parent, int width, int height) : base(parent, width, height)
        {
            Build();
        }

        public void Build()
        {
            base.Build();
            GameNameText = LocalizationManager.GetString("GameTitle").ToUpperInvariant().ToAscii();
            SelectFileText = LocalizationManager.GetString("SelectFileText").ToAscii();
            OptionsText = LocalizationManager.GetString("OptionsText").ToAscii();
            ExitText = LocalizationManager.GetString("ExitButtonText").ToAscii();
            Font = Game.Instance.LoadFont("fonts/IBMCGA.font");
            FontSize = Font.GetFontSize(IFont.Sizes.Two);

            LogoConsole = new ControlsConsole(Width, 8)
            {
                Position = new Point(0, 0),
                FontSize = Font.GetFontSize(IFont.Sizes.Four)
            };

            GameName = new Label(GameNameText.Length)
            {
                DisplayText = GameNameText
            };
            GameName.Position = new Point(GameName.Width / 2 + 2, 4);

            LogoConsole.Controls.Add(GameName);

            SelectFileButton = new Button(SelectFileText.Length + 2)
            {
                Text = SelectFileText
            };
            SelectFileButton.Position = new Point(Width / 4 - SelectFileButton.Width / 2, 20);
            SelectFileButton.Click += SelectFileButton_Click;

            OptionsButton = new Button(OptionsText.Length + 2)
            {
                Text = OptionsText
            };
            OptionsButton.Position = new Point(Width / 4 - OptionsButton.Width / 2, 25);
            OptionsButton.Click += OptionsButton_Click;

            ExitButton = new Button(ExitText.Length + 2)
            {
                Text = ExitText
            };
            ExitButton.Position = new Point(Width / 4 - ExitButton.Width / 2, 30);
            ExitButton.Click += ExitButton_Click;

            Children.Add(LogoConsole);
            Controls.Add(SelectFileButton);
            Controls.Add(OptionsButton);
            Controls.Add(ExitButton);
        }

        private void SelectFileButton_Click(object sender, EventArgs args)
        {
            try
            {
                ParentContainer.PossibleDungeons = BackendHandler.Instance.GetPickableDungeonList(LocalizationManager.CurrentLocale);
                ParentContainer.MoveToConsole(MenuConsoles.PickDungeon);
            }
            catch (Exception)
            {
                ParentContainer.ChangeConsoleContainerTo(ConsoleContainers.Message, ConsoleContainers.Main, LocalizationManager.GetString("ErrorMessageHeader"), LocalizationManager.GetString("ErrorText"));
            }
        }
        private void OptionsButton_Click(object sender, EventArgs args)
        {
            ParentContainer.MoveToConsole(MenuConsoles.Options);
        }

        private void ExitButton_Click(object sender, EventArgs args)
        {
            Environment.Exit(0);
        }
    }
}
