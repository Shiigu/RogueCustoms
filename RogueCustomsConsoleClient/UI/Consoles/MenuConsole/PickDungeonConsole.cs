using SadConsole;
using SadConsole.UI;
using SadConsole.UI.Controls;
using RogueCustomsConsoleClient.EngineHandling;
using RogueCustomsConsoleClient.UI.Consoles.Containers;
using RogueCustomsConsoleClient.Utils;
using SadRogue.Primitives;
using RogueCustomsConsoleClient.Resources.Localization;
using RogueCustomsConsoleClient.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using SadConsole.Input;
using static SFML.Graphics.Font;
using RogueCustomsConsoleClient.UI.Consoles.GameConsole.GameWindows;
using RogueCustomsConsoleClient.UI.Windows;

namespace RogueCustomsConsoleClient.UI.Consoles.MenuConsole
{
    public class PickDungeonConsole : MenuSubConsole
    {
        private string WindowHeaderText, PickButtonText, ReturnButtonText;

        private Label WindowHeader;
        private ListBox DungeonListBox;
        private Button PickButton, ReturnButton;
        private ControlsConsole WindowHeaderConsole;

        public PickDungeonConsole(MenuConsoleContainer parent, int width, int height) : base(parent, width, height)
        {
            Build();
        }

        public void Build()
        {
            base.Build();
            WindowHeaderText = LocalizationManager.GetString("PickDungeonHeaderText").ToAscii();
            PickButtonText = LocalizationManager.GetString("PickButtonText").ToAscii();
            ReturnButtonText = LocalizationManager.GetString("ReturnToMainMenuText").ToAscii();
            Font = Game.Instance.LoadFont("fonts/IBMCGA.font");
            FontSize = Font.GetFontSize(IFont.Sizes.Two);
            WindowHeaderConsole = new ControlsConsole(Width, 1)
            {
                Position = new Point(0, 1),
                FontSize = Font.GetFontSize(IFont.Sizes.Four)
            };

            WindowHeader = new Label(WindowHeaderText.Length)
            {
                DisplayText = WindowHeaderText
            };
            WindowHeader.Position = new Point(WindowHeader.Width / 2, 0);
            WindowHeaderConsole.Controls.Add(WindowHeader);
            Children.Add(WindowHeaderConsole);

            DungeonListBox = new ListBox(Width / 2 - 2, 20)
            {
                Position = new Point(1, 6),
                VisibleItemsMax = 20,
                IsScrollBarVisible = true,
                FocusOnClick = false
            };
            DungeonListBox.SelectedItemChanged += DungeonListBox_SelectedItemChanged;
            Controls.Add(DungeonListBox);

            PickButton = new Button(PickButtonText.Length + 2)
            {
                Text = PickButtonText,
                IsEnabled = false
            };
            PickButton.Position = new Point(Width / 4 - PickButton.Width / 2, Height / 2 - 4);
            PickButton.Click += PickButton_Click;
            Controls.Add(PickButton);

            ReturnButton = new Button(ReturnButtonText.Length + 2)
            {
                Text = ReturnButtonText,
                IsEnabled = true
            };
            ReturnButton.Position = new Point(Width / 4 - ReturnButton.Width / 2, Height / 2 - 2);
            ReturnButton.Click += ReturnButton_Click;
            Controls.Add(ReturnButton);
        }
        public override void Update(TimeSpan delta)
        {
            if (ParentContainer.ActiveWindow?.IsVisible != true)
                this.IsFocused = true;
            base.Update(delta);
        }

        public override bool ProcessKeyboard(Keyboard keyboard)
        {
            bool handled = false;
            if (keyboard.IsKeyPressed(Keys.Up) && keyboard.KeysPressed.Count == 1)
            {
                if (DungeonListBox.SelectedIndex == 0)
                    DungeonListBox.SelectedIndex = DungeonListBox.Items.Count - 1;
                else
                    DungeonListBox.SelectedIndex--;
                handled = true;
            }
            else if (keyboard.IsKeyPressed(Keys.Down) && keyboard.KeysPressed.Count == 1)
            {
                if (DungeonListBox.SelectedIndex == DungeonListBox.Items.Count - 1)
                    DungeonListBox.SelectedIndex = 0;
                else
                    DungeonListBox.SelectedIndex++;
                handled = true;
            }
            else if (keyboard.IsKeyPressed(Keys.Enter) && keyboard.KeysPressed.Count == 1)
            {
                PickButton.InvokeClick();
                handled = true;
            }
            else if (keyboard.IsKeyPressed(Keys.Escape) && keyboard.KeysPressed.Count == 1)
            {
                ReturnButton.InvokeClick();
                handled = true;
            }
            return handled;
        }

        public void FillList()
        {
            var RepeatedNameCount = new Dictionary<string, int>();
            DungeonListBox.Items.Clear();
            if(ParentContainer.PossibleDungeonsInfo.Dungeons.Any())
            {
                DungeonListBox.IsVisible = true;
                this.Clear();
                foreach (var dungeon in ParentContainer.PossibleDungeonsInfo.Dungeons)
                {
                    var dungeonDisplayName = LocalizationManager.GetString("DungeonDisplayNameText").Format(new
                    {
                        DungeonName = dungeon.Name,
                        Author = dungeon.Author
                    }).ToAscii();
                    if (!RepeatedNameCount.ContainsKey(dungeonDisplayName))
                    {
                        DungeonListBox.Items.Add(dungeonDisplayName);
                        RepeatedNameCount[dungeonDisplayName] = 1;
                    }
                    else
                    {
                        DungeonListBox.Items.Add($"{dungeonDisplayName} ({RepeatedNameCount[dungeonDisplayName]})");
                        RepeatedNameCount[dungeonDisplayName]++;
                    }
                }
                DungeonListBox.ScrollBar.IsVisible = true;
                DungeonListBox.SelectedIndex = 0;
            }
            else
            {
                PickButton.IsEnabled = false;
                DungeonListBox.IsVisible = false;
                this.Print(1, 6, LocalizationManager.GetString("NoDungeonsText"));
                if (BackendHandler.Instance.IsLocal)
                    this.Print(1, 7, LocalizationManager.GetString("NoLocalDungeonsSubtext"));
                else
                    this.Print(1, 7, LocalizationManager.GetString("NoServerDungeonsSubtext"));
            }
        }

        private void DungeonListBox_SelectedItemChanged(object? sender, ListBox.SelectedItemEventArgs e)
        {
            PickButton.IsEnabled = true;
        }

        private void ReturnButton_Click(object? sender, EventArgs e)
        {
            ParentContainer.MoveToConsole(MenuConsoles.Main);
        }

        private void PickButton_Click(object? sender, EventArgs args)
        {
            try
            {
                var selectedItem = ParentContainer.PossibleDungeonsInfo.Dungeons[DungeonListBox.SelectedIndex];

                if (selectedItem.IsAtCurrentVersion)
                {
                    BackendHandler.Instance.CreateDungeon(selectedItem.InternalName, LocalizationManager.CurrentLocale);
                    var message = BackendHandler.Instance.GetDungeonWelcomeMessage();

                    ParentContainer.ChangeConsoleContainerTo(ConsoleContainers.Message, ConsoleContainers.Game, LocalizationManager.GetString("BriefingMessageHeader"), message);
                }
                else
                {
                    ParentContainer.ActiveWindow = MessageBox.Show(new ColoredString(LocalizationManager.GetString("IncompatibleDungeonMessageBoxText").Format(new { DungeonJsonVersion = selectedItem.Version, RequiredDungeonJsonVersion = ParentContainer.PossibleDungeonsInfo.CurrentVersion })), LocalizationManager.GetString("OKButtonText"), LocalizationManager.GetString("IncompatibleDungeonMessageBoxHeader"), Color.Red);
                }
            }
            catch (Exception)
            {
                ParentContainer.ChangeConsoleContainerTo(ConsoleContainers.Message, ConsoleContainers.Main, LocalizationManager.GetString("ErrorMessageHeader"), LocalizationManager.GetString("ErrorText"));
            }
        }
    }
}
