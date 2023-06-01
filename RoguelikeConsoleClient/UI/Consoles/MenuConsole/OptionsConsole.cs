using SadConsole;
using SadConsole.UI;
using SadConsole.UI.Controls;
using RoguelikeConsoleClient.EngineHandling;
using RoguelikeConsoleClient.UI.Consoles.Containers;
using RoguelikeConsoleClient.Utils;
using SadRogue.Primitives;

namespace RoguelikeConsoleClient.UI.Consoles.MenuConsole
{
    public class OptionsConsole : MenuSubConsole
    {
        private const string WindowHeaderText = "SELECT OPTIONS";
        private const string RadioButtonHeaderText = "GAME HOST LOCATION";
        private const string LocalRadioButtonText = "LOCAL";
        private const string ServerRadioButtonText = "IN A SERVER";
        private const string ServerAddressTextBoxHeaderText = "SERVER ADDRESS";
        private const string SaveButtonText = "SAVE SETTINGS";
        private const string ReturnButtonText = "RETURN TO MAIN MENU";

        private readonly Label WindowHeader, RadioButtonHeader, ServerAddressTextBoxHeader;
        private readonly RadioButton LocalRadioButton, ServerRadioButton;
        private readonly TextBox ServerAddressTextBox;
        private readonly Button SaveButton, ReturnButton;
        private readonly ControlsConsole WindowHeaderConsole;

        public OptionsConsole(MenuConsoleContainer parent, int width, int height) : base(parent, width, height)
        {
            var oldFontSize = FontSize;
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

            RadioButtonHeader = new Label(GlobalConstants.ScreenCellWidth / 2)
            {
                DisplayText = RadioButtonHeaderText
            };
            RadioButtonHeader.Position = new Point(Width / 2 - RadioButtonHeader.Width / 2 + 4, 30).TranslateFont(oldFontSize, newFontSize);
            Controls.Add(RadioButtonHeader);

            ServerRadioButton = new RadioButton(ServerRadioButtonText.Length + 5, 1)
            {
                Text = ServerRadioButtonText,
                GroupName = "ServerOptions"
            };
            ServerRadioButton.IsSelectedChanged += LocalOrServerRadioButton_IsSelectedChanged;
            ServerRadioButton.Position = new Point(Width / 2 - ServerRadioButton.Width, 36).TranslateFont(oldFontSize, newFontSize);
            Controls.Add(ServerRadioButton);

            LocalRadioButton = new RadioButton(LocalRadioButtonText.Length + 5, 1)
            {
                Text = LocalRadioButtonText,
                GroupName = "ServerOptions"
            };
            LocalRadioButton.IsSelectedChanged += LocalOrServerRadioButton_IsSelectedChanged;
            LocalRadioButton.Position = new Point(Width / 2 - ServerRadioButton.Width, 34).TranslateFont(oldFontSize, newFontSize);
            Controls.Add(LocalRadioButton);

            ServerAddressTextBoxHeader = new Label(GlobalConstants.ScreenCellWidth / 2)
            {
                DisplayText = ServerAddressTextBoxHeaderText
            };
            ServerAddressTextBoxHeader.Position = new Point(Width / 2 - ServerAddressTextBoxHeader.Width / 3, 44).TranslateFont(oldFontSize, newFontSize);
            Controls.Add(ServerAddressTextBoxHeader);

            ServerAddressTextBox = new TextBox(27);
            ServerAddressTextBox.EditingTextChanged += ServerAddressTextBox_EditingTextChanged;
            ServerAddressTextBox.Position = new Point(Width / 2 - ServerAddressTextBox.Width, 46).TranslateFont(oldFontSize, newFontSize);
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

            BackendHandler.Instance.ServerAddress = ServerAddressTextBox.Text;

            Settings.Default.Save();

            ParentContainer.MoveToConsole(MenuConsoles.Main);
        }
    }
}
