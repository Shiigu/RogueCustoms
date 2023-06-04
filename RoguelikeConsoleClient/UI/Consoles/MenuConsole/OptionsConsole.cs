using SadConsole;
using SadConsole.UI;
using SadConsole.UI.Controls;
using RoguelikeConsoleClient.EngineHandling;
using RoguelikeConsoleClient.UI.Consoles.Containers;
using RoguelikeConsoleClient.Utils;
using SadRogue.Primitives;
using RoguelikeConsoleClient.Resources.Localization;
using RoguelikeGameEngine.Utils.Helpers;
using System.Collections.Generic;
using System;

namespace RoguelikeConsoleClient.UI.Consoles.MenuConsole
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
            var oldFontSize = Font.GetFontSize(IFont.Sizes.One);
            var newFontSize = Font.GetFontSize(IFont.Sizes.Two);
            FontSize = newFontSize;

            WindowHeaderConsole = new ControlsConsole(Width, 5)
            {
                Position = new Point(0, 0),
                FontSize = Font.GetFontSize(IFont.Sizes.Four)
            };

            WindowHeader = new Label(WindowHeaderText.Length)
            {
                DisplayText = WindowHeaderText
            };
            WindowHeader.Position = new Point((Width - WindowHeader.Width) / 5 + 4, 15).TranslateFont(oldFontSize, Font.GetFontSize(IFont.Sizes.Four));
            WindowHeaderConsole.Controls.Add(WindowHeader);
            Children.Add(WindowHeaderConsole);

            ServerRadioButton = new RadioButton(ServerRadioButtonText.Length + 5, 1)
            {
                Text = ServerRadioButtonText,
                GroupName = "ServerOptions"
            };
            ServerRadioButton.IsSelectedChanged += LocalOrServerRadioButton_IsSelectedChanged;
            ServerRadioButton.Position = new Point(Width / 4 - ServerRadioButton.Width, 36).TranslateFont(oldFontSize, newFontSize);
            Controls.Add(ServerRadioButton);

            LocalRadioButton = new RadioButton(LocalRadioButtonText.Length + 5, 1)
            {
                Text = LocalRadioButtonText,
                GroupName = "ServerOptions"
            };
            LocalRadioButton.IsSelectedChanged += LocalOrServerRadioButton_IsSelectedChanged;
            LocalRadioButton.Position = new Point(Width / 4 - ServerRadioButton.Width, 34).TranslateFont(oldFontSize, newFontSize);
            Controls.Add(LocalRadioButton);

            RadioButtonHeader = new Label(RadioButtonHeaderText.Length)
            {
                DisplayText = RadioButtonHeaderText
            };
            RadioButtonHeader.Position = new Point(Width / 4 - ServerRadioButton.Width, 30).TranslateFont(oldFontSize, newFontSize);
            Controls.Add(RadioButtonHeader);

            LanguageTextBoxHeader = new Label(LanguageListHeaderText.Length)
            {
                DisplayText = LanguageListHeaderText
            };
            LanguageTextBoxHeader.Position = new Point(Width - LanguageTextBoxHeader.Width * 3 - 5, 30).TranslateFont(oldFontSize, newFontSize);
            Controls.Add(LanguageTextBoxHeader);

            LanguageListBox = new ListBox(15, 5)
            {
                VisibleItemsMax = 5,
                IsScrollBarVisible = true,
                FocusOnClick = false
            };
            LanguageListBox.Position = new Point(Width - LanguageListBox.Width * 2, 34).TranslateFont(oldFontSize, newFontSize);

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
                DisplayText = ServerAddressTextBoxHeaderText
            };
            ServerAddressTextBoxHeader.Position = new Point(Width / 3 - ServerAddressTextBoxHeader.Width, 44).TranslateFont(oldFontSize, newFontSize);
            Controls.Add(ServerAddressTextBoxHeader);

            ServerAddressTextBox = new TextBox(25);
            ServerAddressTextBox.EditingTextChanged += ServerAddressTextBox_EditingTextChanged;
            ServerAddressTextBox.Position = new Point(Width / 3 - ServerAddressTextBoxHeader.Width, 46).TranslateFont(oldFontSize, newFontSize);
            Controls.Add(ServerAddressTextBox);

            SaveButton = new Button(SaveButtonText.Length + 2)
            {
                Position = new Point(Width / 2 - 15, Height - 6).TranslateFont(oldFontSize, newFontSize),
                Text = SaveButtonText
            };
            SaveButton.Click += PickButton_Click;
            Controls.Add(SaveButton);

            ReturnButton = new Button(ReturnButtonText.Length + 2)
            {
                Position = new Point(Width / 2 - 22, Height - 2).TranslateFont(oldFontSize, newFontSize),
                Text = ReturnButtonText,
                IsEnabled = true
            };
            ReturnButton.Click += ReturnButton_Click;
            Controls.Add(ReturnButton);
        }

        public void LoadSettingDisplayData()
        {
            LocalRadioButton.IsSelected = Settings.Default.IsLocal;
            ServerRadioButton.IsSelected = !Settings.Default.IsLocal;
            ServerAddressTextBox.Text = Settings.Default.ServerAddress;
            ServerAddressTextBoxHeader.IsVisible = ServerRadioButton.IsSelected;
            ServerAddressTextBox.IsVisible = ServerRadioButton.IsSelected;
            ServerAddressTextBox.IsEnabled = ServerRadioButton.IsSelected;
            SaveButton.IsEnabled = LocalRadioButton.IsSelected || (ServerRadioButton.IsSelected && !string.IsNullOrWhiteSpace(ServerAddressTextBox.Text));
        }

        private void ServerAddressTextBox_EditingTextChanged(object? sender, EventArgs e)
        {
            ServerAddressTextBoxHeader.IsVisible = ServerRadioButton.IsSelected;
            ServerAddressTextBox.IsVisible = ServerRadioButton.IsSelected;
            ServerAddressTextBox.IsEnabled = ServerRadioButton.IsSelected;
            SaveButton.IsEnabled = LocalRadioButton.IsSelected || (ServerRadioButton.IsSelected && !string.IsNullOrWhiteSpace(ServerAddressTextBox.Text));
        }

        private void LocalOrServerRadioButton_IsSelectedChanged(object? sender, EventArgs e)
        {
            ServerAddressTextBoxHeader.IsVisible = ServerRadioButton.IsSelected;
            ServerAddressTextBox.IsVisible = ServerRadioButton.IsSelected;
            ServerAddressTextBox.IsEnabled = ServerRadioButton.IsSelected;
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
