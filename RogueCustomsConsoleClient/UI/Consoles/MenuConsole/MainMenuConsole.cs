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
using MathNet.Numerics;
using RogueCustomsConsoleClient.Utils;
using RogueCustomsConsoleClient.Helpers;

namespace RogueCustomsConsoleClient.UI.Consoles.MenuConsole
{
    #pragma warning disable IDE0037 // Usar nombre de miembro inferido
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    #pragma warning disable CS8622 // La nulabilidad de los tipos de referencia del tipo de parámetro no coincide con el delegado de destino (posiblemente debido a los atributos de nulabilidad).
    public class MainMenuConsole : MenuSubConsole
    {
        private Button ExitButton;
        private List<Button> Buttons;
        private int CurrentFocusedIndex;
        private string GameVersionString;

        public MainMenuConsole(MenuConsoleContainer parent, int width, int height) : base(parent, width, height)
        {
            Build();
        }

        public new void Build()
        {
            base.Build();
            var gameNameText = LocalizationManager.GetString("GameTitle").ToUpperInvariant().ToAscii();
            var selectFileText = LocalizationManager.GetString("SelectFileText").ToAscii();
            var optionsText = LocalizationManager.GetString("OptionsText").ToAscii();
            var exitText = LocalizationManager.GetString("ExitButtonText").ToAscii();
            Font = Game.Instance.LoadFont("fonts/IBMCGA.font");
            FontSize = Font.GetFontSize(IFont.Sizes.Two);
            GameVersionString = LocalizationManager.GetString("GameVersionText").Format(new { GameVersion = GlobalConstants.GameVersion }).ToAscii();

            var logoConsole = new ControlsConsole(Width, 8)
            {
                Position = new Point(0, 0),
                FontSize = Font.GetFontSize(IFont.Sizes.Four),
                FocusedMode = FocusBehavior.None,
                UseMouse = false,
                UseKeyboard = false,
                IsFocused = false
            };
            logoConsole.Controls.DisableControlFocusing = true;

            var gameName = new Label(gameNameText.Length)
            {
                DisplayText = gameNameText,
                IsEnabled = false
            };
            gameName.Position = new Point((gameName.Width / 2) + 2, 4);

            logoConsole.Controls.Add(gameName);

            gameName.IsFocused = false;

            var selectFileButton = new Button(selectFileText.Length + 2)
            {
                Text = selectFileText
            };
            selectFileButton.Position = new Point((Width / 4) - (selectFileButton.Width / 2), 20);
            selectFileButton.MouseMove += (_, _) => ChangeFocusTo(0);
            selectFileButton.Click += SelectFileButton_Click;
            selectFileButton.IsFocused = true;

            var optionsButton = new Button(optionsText.Length + 2)
            {
                Text = optionsText
            };
            optionsButton.Position = new Point((Width / 4) - (optionsButton.Width / 2), 25);
            optionsButton.MouseEnter += (_, _) => ChangeFocusTo(1);
            optionsButton.Click += OptionsButton_Click;

            ExitButton = new Button(exitText.Length + 2)
            {
                Text = exitText
            };
            ExitButton.Position = new Point((Width / 4) - (ExitButton.Width / 2), 30);
            ExitButton.MouseEnter += (_, _) => ChangeFocusTo(2);
            ExitButton.Click += ExitButton_Click;

            Children.Add(logoConsole);
            Controls.Add(selectFileButton);
            Controls.Add(optionsButton);
            Controls.Add(ExitButton);

            Buttons = new List<Button> { selectFileButton, optionsButton, ExitButton };

            CurrentFocusedIndex = 0;
            selectFileButton.IsFocused = true;
            this.IsFocused = true;
        }

        public override void Update(TimeSpan delta)
        {
            this.IsFocused = true;
            this.Print((Width / 2) - GameVersionString.Length, (Height / 2) - 1, GameVersionString);
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
                ParentContainer.PossibleDungeonsInfo = BackendHandler.Instance.GetPickableDungeonList(LocalizationManager.CurrentLocale);
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
    #pragma warning restore IDE0037 // Usar nombre de miembro inferido
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    #pragma warning restore CS8622 // La nulabilidad de los tipos de referencia del tipo de parámetro no coincide con el delegado de destino (posiblemente debido a los atributos de nulabilidad).
}
