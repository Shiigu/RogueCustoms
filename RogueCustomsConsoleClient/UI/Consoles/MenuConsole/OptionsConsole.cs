using SadConsole;
using SadConsole.UI;
using SadConsole.UI.Controls;
using RogueCustomsConsoleClient.EngineHandling;
using RogueCustomsConsoleClient.UI.Consoles.Containers;
using RogueCustomsConsoleClient.Utils;
using SadRogue.Primitives;
using RogueCustomsConsoleClient.Resources.Localization;
using RogueCustomsGameEngine.Utils.Helpers;
using System.Collections.Generic;
using System;
using SadConsole.Input;
using static SFML.Graphics.Font;

namespace RogueCustomsConsoleClient.UI.Consoles.MenuConsole
{
    #pragma warning disable S4144 // Methods should not have identical implementations
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public class OptionsConsole : MenuSubConsole
    {
        private Label RadioButtonHeader, ServerAddressTextBoxHeader, LanguageTextBoxHeader;
        private RadioButton LocalRadioButton, ServerRadioButton;
        private TextBox ServerAddressTextBox;
        private Button SaveButton, ReturnButton;
        private ListBox LanguageListBox;

        private List<string> LanguageList;

        public OptionsConsole(MenuConsoleContainer parent, int width, int height) : base(parent, width, height)
        {
            Build();
        }

        public new void Build()
        {
            base.Build();
            var windowHeaderText = LocalizationManager.GetString("OptionsHeaderText").ToAscii();
            var radioButtonHeaderText = LocalizationManager.GetString("RadioButtonHeaderText").ToAscii();
            var localRadioButtonText = LocalizationManager.GetString("LocalRadioButtonText").ToAscii();
            var serverRadioButtonText = LocalizationManager.GetString("ServerRadioButtonText").ToAscii();
            var languageListHeaderText = LocalizationManager.GetString("LanguageListHeaderText").ToAscii();
            var serverAddressTextBoxHeaderText = LocalizationManager.GetString("ServerAddressTextBoxHeaderText").ToAscii();
            var saveButtonText = LocalizationManager.GetString("SaveSettingsButtonText").ToAscii();
            var returnButtonText = LocalizationManager.GetString("ReturnToMainMenuText").ToAscii();
            Font = Game.Instance.LoadFont("fonts/IBMCGA.font");
            FontSize = Font.GetFontSize(IFont.Sizes.Two);

            var windowHeaderConsole = new ControlsConsole(Width, 5)
            {
                Position = new Point(0, 0),
                FontSize = Font.GetFontSize(IFont.Sizes.Four)
            };

            var windowHeader = new Label(windowHeaderText.Length)
            {
                DisplayText = windowHeaderText
            };
            windowHeader.Position = new Point((windowHeader.Width / 2) + 1, 3);
            windowHeaderConsole.Controls.Add(windowHeader);
            Children.Add(windowHeaderConsole);

            RadioButtonHeader = new Label(radioButtonHeaderText.Length)
            {
                DisplayText = radioButtonHeaderText,
                TextColor = Color.Yellow,
                Position = new Point(3, 15)
            };
            Controls.Add(RadioButtonHeader);

            LocalRadioButton = new RadioButton(localRadioButtonText.Length + 5, 1)
            {
                Text = localRadioButtonText,
                GroupName = "ServerOptions",
                TabIndex = 0
            };
            LocalRadioButton.IsSelectedChanged += LocalOrServerRadioButton_IsSelectedChanged;
            LocalRadioButton.Position = new Point(3, 17);
            Controls.Add(LocalRadioButton);

            ServerRadioButton = new RadioButton(serverRadioButtonText.Length + 5, 1)
            {
                Text = serverRadioButtonText,
                GroupName = "ServerOptions",
                TabIndex = 1
            };
            ServerRadioButton.IsSelectedChanged += LocalOrServerRadioButton_IsSelectedChanged;
            ServerRadioButton.Position = new Point(3, 18);
            Controls.Add(ServerRadioButton);

            LanguageTextBoxHeader = new Label(languageListHeaderText.Length)
            {
                DisplayText = languageListHeaderText,
                TextColor = Color.Yellow
            };
            LanguageTextBoxHeader.Position = new Point((Width / 2) - (LanguageTextBoxHeader.Width / 2) - 13, 15);
            Controls.Add(LanguageTextBoxHeader);

            LanguageListBox = new ListBox(15, 5)
            {
                VisibleItemsMax = 5,
                IsScrollBarVisible = true,
                FocusOnClick = false,
                TabIndex = 2,
                Position = new Point((Width / 2) - (LanguageTextBoxHeader.Width / 2) - 18, 17)
            };

            LanguageList = new List<string>();
            foreach (var language in LocalizationManager.GetLocalizationDisplayData())
            {
                LanguageListBox.Items.Add(language.Value.ToAscii());
                if(LocalizationManager.CurrentLocale.Equals(language.Key))
                    LanguageListBox.SelectedItem = language.Value.ToAscii();
                LanguageList.Add(language.Value);
            }
            Controls.Add(LanguageListBox);

            ServerAddressTextBoxHeader = new Label(serverAddressTextBoxHeaderText.Length)
            {
                DisplayText = serverAddressTextBoxHeaderText,
                TextColor = Color.Yellow
            };
            ServerAddressTextBoxHeader.Position = new Point((Width / 4) - (ServerAddressTextBoxHeader.Width / 2) - 8, 26);
            Controls.Add(ServerAddressTextBoxHeader);

            ServerAddressTextBox = new TextBox(30);
            ServerAddressTextBox.EditingTextChanged += ServerAddressTextBox_EditingTextChanged;
            ServerAddressTextBox.Position = new Point(ServerAddressTextBoxHeader.Position.X, 28);
            ServerAddressTextBox.TabIndex = 3;
            Controls.Add(ServerAddressTextBox);

            SaveButton = new Button(saveButtonText.Length + 2)
            {
                Text = saveButtonText,
                TabIndex = 4
            };
            SaveButton.Position = new Point((Width / 4) - (SaveButton.Width / 2), (Height / 2) - 4);
            SaveButton.Click += PickButton_Click;
            Controls.Add(SaveButton);

            ReturnButton = new Button(returnButtonText.Length + 2)
            {
                Text = returnButtonText,
                IsEnabled = true,
                TabIndex = 5
            };
            ReturnButton.Position = new Point((Width / 4) - (ReturnButton.Width / 2), (Height / 2) - 2);
            ReturnButton.Click += ReturnButton_Click;
            Controls.Add(ReturnButton);
        }
        public override void Update(TimeSpan delta)
        {
            this.IsFocused = true;

            if (Controls.FocusedControl == LocalRadioButton || Controls.FocusedControl == ServerRadioButton)
            {
                RadioButtonHeader.TextColor = Color.White;
            }
            else
            {
                RadioButtonHeader.TextColor = Color.Yellow;
            }

            if (Controls.FocusedControl == LanguageListBox)
            {
                LanguageTextBoxHeader.TextColor = Color.White;
            }
            else
            {
                LanguageTextBoxHeader.TextColor = Color.Yellow;
            }

            if (Controls.FocusedControl == ServerAddressTextBox)
            {
                ServerAddressTextBoxHeader.TextColor = Color.White;
            }
            else
            {
                ServerAddressTextBoxHeader.TextColor = Color.Yellow;
            }

            RadioButtonHeader.IsDirty = true;
            LanguageTextBoxHeader.IsDirty = true;
            ServerAddressTextBoxHeader.IsDirty = true;

            base.Update(delta);
        }

        public override bool ProcessKeyboard(Keyboard keyboard)
        {
            bool handled = false;
            if (Controls.FocusedControl != ServerAddressTextBox)
            {
                if ((keyboard.IsKeyPressed(Keys.Up) || keyboard.IsKeyPressed(Keys.Down)) && keyboard.KeysPressed.Count == 1)
                {
                    if (Controls.FocusedControl == LocalRadioButton)
                    {
                        ServerRadioButton.IsFocused = true;
                        ServerRadioButton.Focused();
                        handled = true;
                    }
                    else if (Controls.FocusedControl == ServerRadioButton)
                    {
                        LocalRadioButton.IsFocused = true;
                        LocalRadioButton.Focused();
                        handled = true;
                    }
                }
                else if (keyboard.IsKeyPressed(Keys.Enter) && keyboard.KeysPressed.Count == 1)
                {
                    SaveButton.InvokeClick();
                    handled = true;
                }
                else if (keyboard.IsKeyPressed(Keys.Escape) && keyboard.KeysPressed.Count == 1)
                {
                    ReturnButton.InvokeClick();
                    handled = true;
                }
            }
            return handled || base.ProcessKeyboard(keyboard);
        }

        public void LoadSettingDisplayData()
        {
            LocalRadioButton.IsSelected = Settings.Default.IsLocal;
            ServerRadioButton.IsSelected = !Settings.Default.IsLocal;
            ServerAddressTextBox.Text = Settings.Default.ServerAddress;
            ServerAddressTextBoxHeader.IsVisible = ServerRadioButton.IsSelected;
            ServerAddressTextBox.IsVisible = ServerRadioButton.IsSelected;
            ServerAddressTextBox.IsEnabled = ServerRadioButton.IsSelected;
            ServerAddressTextBox.TabStop = ServerRadioButton.IsSelected;
            SaveButton.IsEnabled = LocalRadioButton.IsSelected || (ServerRadioButton.IsSelected && !string.IsNullOrWhiteSpace(ServerAddressTextBox.Text));
        }

        private void ServerAddressTextBox_EditingTextChanged(object? sender, EventArgs e)
        {
            ServerAddressTextBoxHeader.IsVisible = ServerRadioButton.IsSelected;
            ServerAddressTextBox.IsVisible = ServerRadioButton.IsSelected;
            ServerAddressTextBox.IsEnabled = ServerRadioButton.IsSelected;
            ServerAddressTextBox.TabStop = ServerRadioButton.IsSelected;
            SaveButton.IsEnabled = LocalRadioButton.IsSelected || (ServerRadioButton.IsSelected && !string.IsNullOrWhiteSpace(ServerAddressTextBox.Text));
        }

        private void LocalOrServerRadioButton_IsSelectedChanged(object? sender, EventArgs e)
        {
            ServerAddressTextBoxHeader.IsVisible = ServerRadioButton.IsSelected;
            ServerAddressTextBox.IsVisible = ServerRadioButton.IsSelected;
            ServerAddressTextBox.IsEnabled = ServerRadioButton.IsSelected;
            ServerAddressTextBox.TabStop = ServerRadioButton.IsSelected;
            SaveButton.IsEnabled = LocalRadioButton.IsSelected || (ServerRadioButton.IsSelected && !string.IsNullOrWhiteSpace(ServerAddressTextBox.Text));
        }

        private void ReturnButton_Click(object? sender, EventArgs e)
        {
            ParentContainer.MoveToConsole(MenuConsoles.Main);
        }

        private void PickButton_Click(object? sender, EventArgs args)
        {
            Settings.Default.IsLocal = LocalRadioButton.IsSelected;
            Settings.Default.ServerAddress = ServerAddressTextBox.Text;
            Settings.Default.Language = LanguageList[LanguageListBox.SelectedIndex];

            BackendHandler.Instance.ServerAddress = ServerAddressTextBox.Text;

            Settings.Default.Save();

            ParentContainer.MoveToConsole(MenuConsoles.Main);
        }
    }
    #pragma warning restore S4144 // Methods should not have identical implementations
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
