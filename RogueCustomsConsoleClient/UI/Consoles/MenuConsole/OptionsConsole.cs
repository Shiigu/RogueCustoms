﻿using SadConsole;
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
    public class OptionsConsole : MenuSubConsole
    {
        private string WindowHeaderText, RadioButtonHeaderText, LocalRadioButtonText, ServerRadioButtonText, LanguageListHeaderText, ServerAddressTextBoxHeaderText, SaveButtonText, ReturnButtonText;

        private Label WindowHeader, RadioButtonHeader, ServerAddressTextBoxHeader, LanguageTextBoxHeader;
        private RadioButton LocalRadioButton, ServerRadioButton;
        private TextBox ServerAddressTextBox;
        private Button SaveButton, ReturnButton;
        private ControlsConsole WindowHeaderConsole;
        private ListBox LanguageListBox;

        private List<string> LanguageList;

        public OptionsConsole(MenuConsoleContainer parent, int width, int height) : base(parent, width, height)
        {
            Build();
        }

        public void Build()
        {
            base.Build();
            WindowHeaderText = LocalizationManager.GetString("OptionsHeaderText").ToAscii();
            RadioButtonHeaderText = LocalizationManager.GetString("RadioButtonHeaderText").ToAscii();
            LocalRadioButtonText = LocalizationManager.GetString("LocalRadioButtonText").ToAscii();
            ServerRadioButtonText = LocalizationManager.GetString("ServerRadioButtonText").ToAscii();
            LanguageListHeaderText = LocalizationManager.GetString("LanguageListHeaderText").ToAscii();
            ServerAddressTextBoxHeaderText = LocalizationManager.GetString("ServerAddressTextBoxHeaderText").ToAscii();
            SaveButtonText = LocalizationManager.GetString("SaveButtonText").ToAscii();
            ReturnButtonText = LocalizationManager.GetString("ReturnToMainMenuText").ToAscii();
            Font = Game.Instance.LoadFont("fonts/IBMCGA.font");
            FontSize = Font.GetFontSize(IFont.Sizes.Two);

            WindowHeaderConsole = new ControlsConsole(Width, 5)
            {
                Position = new Point(0, 0),
                FontSize = Font.GetFontSize(IFont.Sizes.Four)
            };

            WindowHeader = new Label(WindowHeaderText.Length)
            {
                DisplayText = WindowHeaderText
            };
            WindowHeader.Position = new Point(WindowHeader.Width / 2 + 1, 3);
            WindowHeaderConsole.Controls.Add(WindowHeader);
            Children.Add(WindowHeaderConsole);

            RadioButtonHeader = new Label(RadioButtonHeaderText.Length)
            {
                DisplayText = RadioButtonHeaderText,
                TextColor = Color.Yellow
            };
            RadioButtonHeader.Position = new Point(3, 15);
            Controls.Add(RadioButtonHeader);

            LocalRadioButton = new RadioButton(LocalRadioButtonText.Length + 5, 1)
            {
                Text = LocalRadioButtonText,
                GroupName = "ServerOptions",
                TabIndex = 0
            };
            LocalRadioButton.IsSelectedChanged += LocalOrServerRadioButton_IsSelectedChanged;
            LocalRadioButton.Position = new Point(3, 17);
            Controls.Add(LocalRadioButton);

            ServerRadioButton = new RadioButton(ServerRadioButtonText.Length + 5, 1)
            {
                Text = ServerRadioButtonText,
                GroupName = "ServerOptions",
                TabIndex = 1
            };
            ServerRadioButton.IsSelectedChanged += LocalOrServerRadioButton_IsSelectedChanged;
            ServerRadioButton.Position = new Point(3, 18);
            Controls.Add(ServerRadioButton);

            LanguageTextBoxHeader = new Label(LanguageListHeaderText.Length)
            {
                DisplayText = LanguageListHeaderText,
                TextColor = Color.Yellow
            };
            LanguageTextBoxHeader.Position = new Point(Width / 2 - LanguageTextBoxHeader.Width / 2 - 13, 15);
            Controls.Add(LanguageTextBoxHeader);

            LanguageListBox = new ListBox(15, 5)
            {
                VisibleItemsMax = 5,
                IsScrollBarVisible = true,
                FocusOnClick = false,
                TabIndex = 2
            };
            LanguageListBox.Position = new Point(Width / 2 - LanguageTextBoxHeader.Width / 2 - 18, 17);

            LanguageList = new List<string>();
            foreach (var language in LocalizationManager.GetLocalizationDisplayData())
            {
                LanguageListBox.Items.Add(language.Value.ToAscii());
                if(LocalizationManager.CurrentLocale.Equals(language.Key))
                    LanguageListBox.SelectedItem = language.Value.ToAscii();
                LanguageList.Add(language.Value);
            }
            Controls.Add(LanguageListBox);

            ServerAddressTextBoxHeader = new Label(ServerAddressTextBoxHeaderText.Length)
            {
                DisplayText = ServerAddressTextBoxHeaderText,
                TextColor = Color.Yellow
            };
            ServerAddressTextBoxHeader.Position = new Point(Width / 4 - ServerAddressTextBoxHeader.Width / 2 - 8, 26);
            Controls.Add(ServerAddressTextBoxHeader);

            ServerAddressTextBox = new TextBox(30);
            ServerAddressTextBox.EditingTextChanged += ServerAddressTextBox_EditingTextChanged;
            ServerAddressTextBox.Position = new Point(ServerAddressTextBoxHeader.Position.X, 28);
            ServerAddressTextBox.TabIndex = 3;
            Controls.Add(ServerAddressTextBox);

            SaveButton = new Button(SaveButtonText.Length + 2)
            {
                Text = SaveButtonText,
                TabIndex = 4
            };
            SaveButton.Position = new Point(Width / 4 - SaveButton.Width / 2, Height / 2 - 4);
            SaveButton.Click += PickButton_Click;
            Controls.Add(SaveButton);

            ReturnButton = new Button(ReturnButtonText.Length + 2)
            {
                Text = ReturnButtonText,
                IsEnabled = true,
                TabIndex = 5
            };
            ReturnButton.Position = new Point(Width / 4 - ReturnButton.Width / 2, Height / 2 - 2);
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
}
