using SadConsole;
using SadConsole.UI;
using SadConsole.UI.Controls;
using RoguelikeConsoleClient.EngineHandling;
using RoguelikeConsoleClient.UI.Consoles.Containers;
using RoguelikeConsoleClient.Utils;
using SadRogue.Primitives;

namespace RoguelikeConsoleClient.UI.Consoles.MenuConsole
{
    public class PickDungeonConsole : MenuSubConsole
    {
        private const string WindowHeaderText = "PICK A DUNGEON";
        private const string PickButtonText = "PLAY SELECTED DUNGEON";
        private const string ReturnButtonText = "RETURN TO MAIN MENU";

        private readonly Label WindowHeader;
        private readonly ListBox DungeonListBox;
        private readonly Button PickButton, ReturnButton;
        private readonly ControlsConsole WindowHeaderConsole;

        public PickDungeonConsole(MenuConsoleContainer parent, int width, int height) : base(parent, width, height)
        {
            var oldFontSize = FontSize;
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
                Position = new Point(Width / 2 - 24, Height - 6).TranslateFont(oldFontSize, newFontSize),
                Text = PickButtonText,
                IsEnabled = false
            };
            PickButton.Click += PickButton_Click;
            Controls.Add(PickButton);

            ReturnButton = new Button(ReturnButtonText.Length + 2)
            {
                Position = new Point(Width / 2 - 22, Height - 2).TranslateFont(oldFontSize, newFontSize),
                Text = ReturnButtonText,
                IsEnabled = true
            };
            ReturnButton.Click += ReturnButton_Click;
            Controls.Add(ReturnButton);
        }

        public void FillList()
        {
            Dictionary<string, int> RepeatedNameCount = new Dictionary<string, int>();
            DungeonListBox.Items.Clear();
            if(ParentContainer.PossibleDungeons.Any())
            {
                DungeonListBox.IsVisible = true;
                this.Clear();
                foreach (var dungeon in ParentContainer.PossibleDungeons)
                {
                    if (!RepeatedNameCount.ContainsKey(dungeon.ToString()))
                    {
                        DungeonListBox.Items.Add(dungeon.ToString());
                        RepeatedNameCount[dungeon.ToString()] = 1;
                    }
                    else
                    {
                        DungeonListBox.Items.Add($"{dungeon} ({RepeatedNameCount[dungeon.ToString()]})");
                        RepeatedNameCount[dungeon.ToString()]++;
                    }
                }
                DungeonListBox.ScrollBar.IsVisible = true;
                DungeonListBox.SelectedIndex = 0;
            }
            else
            {
                PickButton.IsEnabled = false;
                DungeonListBox.IsVisible = false;
                this.Print(1, 6, "There are no dungeons to show.");
                if (BackendHandler.Instance.IsLocal)
                    this.Print(1, 7, "Put dungeons in the JSON folder first.");
                else
                    this.Print(1, 7, "Server has no dungeons installed.");
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

                BackendHandler.Instance.CreateDungeon(selectedItem.InternalName);

                var message = BackendHandler.Instance.GetDungeonWelcomeMessage();

                ParentContainer.ChangeConsoleContainerTo(ConsoleContainers.Message, ConsoleContainers.Game, "BRIEFING", message);
            }
            catch (Exception)
            {
                ParentContainer.ChangeConsoleContainerTo(ConsoleContainers.Message, ConsoleContainers.Main, "ERROR", "OH NO!\nAn error has occured!\nGet ready to return to the main menu...");
            }
        }
    }
}
