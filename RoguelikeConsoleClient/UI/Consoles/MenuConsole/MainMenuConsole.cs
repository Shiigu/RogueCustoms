using SadConsole;
using SadConsole.UI;
using SadConsole.UI.Controls;
using RoguelikeConsoleClient.EngineHandling;
using RoguelikeConsoleClient.UI.Consoles.Containers;
using RoguelikeConsoleClient.UI.Consoles.GameConsole.GameWindows;
using SadRogue.Primitives;
using Console = SadConsole.Console;
using RoguelikeConsoleClient.Resources.Localization;
using System;

namespace RoguelikeConsoleClient.UI.Consoles.MenuConsole
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
            var oldFontSize = Font.GetFontSize(IFont.Sizes.One);
            var newFontSize = Font.GetFontSize(IFont.Sizes.Two);
            FontSize = newFontSize;

            LogoConsole = new ControlsConsole(Width, 8)
            {
                Position = new Point(0, 0),
                FontSize = Font.GetFontSize(IFont.Sizes.Four)
            };

            GameName = new Label(GameNameText.Length)
            {
                DisplayText = GameNameText
            };
            GameName.Position = new Point((Width - GameName.Width) / 3 - 4, 15).TranslateFont(oldFontSize, Font.GetFontSize(IFont.Sizes.Four));

            LogoConsole.Controls.Add(GameName);

            SelectFileButton = new Button(SelectFileText.Length + 2)
            {
                Text = SelectFileText
            };
            SelectFileButton.Position = new Point(Width / 2 - SelectFileButton.Width, 35).TranslateFont(oldFontSize, newFontSize);
            SelectFileButton.Click += SelectFileButton_Click;

            OptionsButton = new Button(OptionsText.Length + 2)
            {
                Text = OptionsText
            };
            OptionsButton.Position = new Point(Width / 2 - OptionsButton.Width, 45).TranslateFont(oldFontSize, newFontSize);
            OptionsButton.Click += OptionsButton_Click;

            ExitButton = new Button(ExitText.Length + 2)
            {
                Text = ExitText
            };
            ExitButton.Position = new Point(Width / 2 - ExitButton.Width, 55).TranslateFont(oldFontSize, newFontSize);
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
