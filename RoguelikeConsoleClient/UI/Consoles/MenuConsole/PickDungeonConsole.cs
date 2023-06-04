using SadConsole;
using SadConsole.UI;
using SadConsole.UI.Controls;
using RoguelikeConsoleClient.EngineHandling;
using RoguelikeConsoleClient.UI.Consoles.Containers;
using RoguelikeConsoleClient.Utils;
using SadRogue.Primitives;
using RoguelikeConsoleClient.Resources.Localization;
using RoguelikeConsoleClient.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoguelikeConsoleClient.UI.Consoles.MenuConsole
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
            var oldFontSize = Font.GetFontSize(IFont.Sizes.One);
            var newFontSize = Font.GetFontSize(IFont.Sizes.Two);
            FontSize = newFontSize;
            WindowHeaderConsole = new ControlsConsole(Width, 1)
            {
                Position = new Point(0, 1),
                FontSize = Font.GetFontSize(IFont.Sizes.Four)
            };

            WindowHeader = new Label(WindowHeaderText.Length)
            {
                DisplayText = WindowHeaderText
            };
            WindowHeader.Position = new Point((Width - WindowHeader.Width) / 5 + 4, 1).TranslateFont(oldFontSize, Font.GetFontSize(IFont.Sizes.Four));
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
                Position = new Point(Width / 2 - 3 - PickButtonText.Length, Height - 6).TranslateFont(oldFontSize, newFontSize),
                Text = PickButtonText,
                IsEnabled = false
            };
            PickButton.Click += PickButton_Click;
            Controls.Add(PickButton);

            ReturnButton = new Button(ReturnButtonText.Length + 2)
            {
                Position = new Point(Width / 2 - 3 - ReturnButtonText.Length, Height - 2).TranslateFont(oldFontSize, newFontSize),
                Text = ReturnButtonText,
                IsEnabled = true
            };
            ReturnButton.Click += ReturnButton_Click;
            Controls.Add(ReturnButton);
        }

        public void FillList()
        {
            var RepeatedNameCount = new Dictionary<string, int>();
            DungeonListBox.Items.Clear();
            if(ParentContainer.PossibleDungeons.Any())
            {
                DungeonListBox.IsVisible = true;
                this.Clear();
                foreach (var dungeon in ParentContainer.PossibleDungeons)
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
                var selectedItem = ParentContainer.PossibleDungeons[DungeonListBox.SelectedIndex];

                BackendHandler.Instance.CreateDungeon(selectedItem.InternalName, LocalizationManager.CurrentLocale);

                var message = BackendHandler.Instance.GetDungeonWelcomeMessage();

                ParentContainer.ChangeConsoleContainerTo(ConsoleContainers.Message, ConsoleContainers.Game, LocalizationManager.GetString("BriefingMessageHeader"), message);
            }
            catch (Exception)
            {
                ParentContainer.ChangeConsoleContainerTo(ConsoleContainers.Message, ConsoleContainers.Main, LocalizationManager.GetString("ErrorMessageHeader"), LocalizationManager.GetString("ErrorText"));
            }
        }
    }
}
