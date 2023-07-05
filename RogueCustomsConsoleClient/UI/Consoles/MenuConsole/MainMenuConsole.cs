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
using System.Collections.Generic;
using SadConsole.Input;
using static SFML.Graphics.Font;

namespace RogueCustomsConsoleClient.UI.Consoles.MenuConsole
{
    public class MainMenuConsole : MenuSubConsole
    {
        private string GameNameText, SelectFileText, OptionsText, ExitText;

        private Label GameName;
        private Button SelectFileButton, OptionsButton, ExitButton;
        private List<Button> Buttons;
        private int CurrentFocusedIndex;
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
                FontSize = Font.GetFontSize(IFont.Sizes.Four),
                FocusedMode = FocusBehavior.None,
                UseMouse = false,
                UseKeyboard = false
            };
            LogoConsole.IsFocused = false;
            LogoConsole.Controls.DisableControlFocusing = true;

            GameName = new Label(GameNameText.Length)
            {
                DisplayText = GameNameText,
                IsEnabled = false
            };
            GameName.Position = new Point(GameName.Width / 2 + 2, 4);

            LogoConsole.Controls.Add(GameName);

            GameName.IsFocused = false;

            SelectFileButton = new Button(SelectFileText.Length + 2)
            {
                Text = SelectFileText
            };
            SelectFileButton.Position = new Point(Width / 4 - SelectFileButton.Width / 2, 20);
            SelectFileButton.MouseMove += (_, _) => ChangeFocusTo(0);
            SelectFileButton.Click += SelectFileButton_Click;
            SelectFileButton.IsFocused = true;

            OptionsButton = new Button(OptionsText.Length + 2)
            {
                Text = OptionsText
            };
            OptionsButton.Position = new Point(Width / 4 - OptionsButton.Width / 2, 25);
            OptionsButton.MouseEnter += (_, _) => ChangeFocusTo(1);
            OptionsButton.Click += OptionsButton_Click;

            ExitButton = new Button(ExitText.Length + 2)
            {
                Text = ExitText
            };
            ExitButton.Position = new Point(Width / 4 - ExitButton.Width / 2, 30);
            ExitButton.MouseEnter += (_, _) => ChangeFocusTo(2);
            ExitButton.Click += ExitButton_Click;

            Children.Add(LogoConsole);
            Controls.Add(SelectFileButton);
            Controls.Add(OptionsButton);
            Controls.Add(ExitButton);

            Buttons = new List<Button> { SelectFileButton, OptionsButton, ExitButton };

            CurrentFocusedIndex = 0;
            SelectFileButton.IsFocused = true;
            this.IsFocused = true;
        }

        public override void Update(TimeSpan delta)
        {
            this.IsFocused = true;
            base.Update(delta);
        }

        public override bool ProcessKeyboard(Keyboard keyboard)
        {
            bool handled = false;
            var changeFocus = false;
            int index = CurrentFocusedIndex;
            if(keyboard.IsKeyPressed(Keys.Up) && keyboard.KeysPressed.Count == 1)
            {
                Buttons[CurrentFocusedIndex].FocusLost();
                Buttons[CurrentFocusedIndex].IsFocused = false;
                if (index == 0)
                    index = 2;
                else
                    index--;
                changeFocus = true;
                handled = true;
            }
            else if (keyboard.IsKeyPressed(Keys.Down) && keyboard.KeysPressed.Count == 1)
            {
                Buttons[CurrentFocusedIndex].FocusLost();
                Buttons[CurrentFocusedIndex].IsFocused = false;
                if (index == 2)
                    index = 0;
                else
                    index++;
                changeFocus = true;
                handled = true;
            }
            else if (keyboard.IsKeyPressed(Keys.Enter) && keyboard.KeysPressed.Count == 1)
            {
                Buttons[CurrentFocusedIndex].InvokeClick();
                handled = true;
            }
            else if (keyboard.IsKeyPressed(Keys.Escape) && keyboard.KeysPressed.Count == 1)
            {
                ExitButton.InvokeClick();
                handled = true;
            }

            if (changeFocus)
            {
                ChangeFocusTo(index);
            }

            return handled;
        }

        private void ChangeFocusTo(int index)
        {
            if (CurrentFocusedIndex == index) return;
            Buttons[CurrentFocusedIndex].IsFocused = false;
            Buttons[CurrentFocusedIndex].FocusLost();
            CurrentFocusedIndex = index;
            Buttons[CurrentFocusedIndex].IsFocused = true;
            Buttons[CurrentFocusedIndex].Focused();
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
