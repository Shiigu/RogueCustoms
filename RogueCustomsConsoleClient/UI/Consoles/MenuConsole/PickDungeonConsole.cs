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
using RogueCustomsConsoleClient.UI.Consoles.GameConsole.GameWindows;
using RogueCustomsConsoleClient.UI.Windows;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using static SadConsole.UI.Controls.Table.Cell.Options;

namespace RogueCustomsConsoleClient.UI.Consoles.MenuConsole
{
    #pragma warning disable IDE0037 // Usar nombre de miembro inferido
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public class PickDungeonConsole : MenuSubConsole
    {
        private Table DungeonTable;
        private Button PickButton, ReturnButton;
        private DungeonPickDto SelectedItem;
        private const int MaxSimultaneousRows = 24;

        public PickDungeonConsole(MenuConsoleContainer parent, int width, int height) : base(parent, width, height)
        {
            Build();
        }

        public new void Build()
        {
            base.Build();
            var windowHeaderText = LocalizationManager.GetString("PickDungeonHeaderText").ToAscii();
            var pickButtonText = LocalizationManager.GetString("PickButtonText").ToAscii();
            var returnButtonText = LocalizationManager.GetString("ReturnToMainMenuText").ToAscii();
            FontSize = Font.GetFontSize(IFont.Sizes.Two);
            var windowHeaderConsole = new ControlsConsole(Width, 1)
            {
                Position = new Point(0, 1),
                FontSize = Font.GetFontSize(IFont.Sizes.Four)
            };

            var windowHeader = new Label(windowHeaderText.Length)
            {
                DisplayText = windowHeaderText
            };
            windowHeader.Position = new Point(windowHeader.Width / 2, 0);
            windowHeaderConsole.Controls.Add(windowHeader);
            Children.Add(windowHeaderConsole);

            DungeonTable = new Table((Width / 2) - 1, MaxSimultaneousRows + 1)
            {
                Position = new Point(1, 5),
                AutoScrollOnCellSelection = true,
                FocusOnMouseClick = false,
                DefaultSelectionMode = Table.TableCells.Layout.Mode.EntireRow,
                DefaultHoverMode = Table.TableCells.Layout.Mode.None,
                DefaultBackground = Color.Black,
                DefaultForeground = Color.White,
                DrawFakeCells = true
            };
            DungeonTable.Cells.HeaderRow = true;
            DungeonTable.SelectedCellChanged += DungeonTable_SelectedCellChanged;
            DungeonTable.SetupScrollBar(Orientation.Vertical, MaxSimultaneousRows, new Point(DungeonTable.Position.X + DungeonTable.Width - 1, DungeonTable.Position.Y));
            DungeonTable.VerticalScrollBar.Maximum = MaxSimultaneousRows - 1;
            DungeonTable.VerticalScrollBar.IsVisible = true;
            DungeonTable.ThemeState.Selected.Foreground = Color.AnsiBlack;
            DungeonTable.ThemeState.Selected.Background = Color.AnsiWhiteBright;

            var colors = DungeonTable.FindThemeColors().Clone();
            colors.ControlForegroundSelected.SetColor(Color.Black);
            colors.ControlBackgroundSelected.SetColor(Color.White);
            colors.RebuildAppearances();
            DungeonTable.SetThemeColors(colors);

            Controls.Add(DungeonTable);

            PickButton = new Button(pickButtonText.Length + 2)
            {
                Text = pickButtonText,
                IsEnabled = false
            };
            PickButton.Position = new Point((Width / 4) - (PickButton.Width / 2), (Height / 2) - 4);
            PickButton.Click += PickButton_Click;
            Controls.Add(PickButton);

            ReturnButton = new Button(returnButtonText.Length + 2)
            {
                Text = returnButtonText,
                IsEnabled = true
            };
            ReturnButton.Position = new Point((Width / 4) - (ReturnButton.Width / 2), (Height / 2) - 2);
            ReturnButton.Click += ReturnButton_Click;
            Controls.Add(ReturnButton);
        }

        private void DungeonTable_SelectedCellChanged(object? sender, Table.CellChangedEventArgs e)
        {
            if (DungeonTable.SelectedCell == null)
            {
                PickButton.IsEnabled = false;
                return;
            }
            var rowIndex = DungeonTable.SelectedCell.Row;
            if (rowIndex == 0)
            {
                for (int i = DungeonTable.Cells.MaxRow; i > 0; i--)
                {
                    if (!string.IsNullOrWhiteSpace(DungeonTable.Cells[i, 0].Value.ToString()))
                    {
                        DungeonTable.Cells[i, 0].Select();
                        return;
                    }
                }
                DungeonTable.Cells[1, 0].Select();
                return;
            }
            else if (string.IsNullOrWhiteSpace(DungeonTable.Cells[rowIndex, 0].Value.ToString()))
            {
                DungeonTable.Cells[1, 0].Select();
                return;
            }
            SelectedItem = ParentContainer.PossibleDungeonsInfo.Dungeons.Find(d => d.Name.Equals(DungeonTable.Cells[rowIndex, 0].Value.ToString()));
            PickButton.IsEnabled = SelectedItem != null;
        }

        public override void Update(TimeSpan delta)
        {
            if (ParentContainer.ActiveWindow?.IsVisible != true)
                this.IsFocused = true;
            base.Update(delta);
        }

        public override bool ProcessKeyboard(Keyboard keyboard)
        {
            if (keyboard.IsKeyPressed(Keys.Up) && keyboard.KeysPressed.Count == 1)
            {
                if(DungeonTable.SelectedCell != null)
                {
                    DungeonTable.Cells[DungeonTable.SelectedCell.Row - 1, 0].Select();
                }
                return true;
            }
            else if (keyboard.IsKeyPressed(Keys.Down) && keyboard.KeysPressed.Count == 1)
            {
                if (DungeonTable.SelectedCell != null)
                {
                    DungeonTable.Cells[DungeonTable.SelectedCell.Row + 1, 0].Select();
                }
                return true;
            }
            else if (keyboard.IsKeyPressed(Keys.Enter) && keyboard.KeysPressed.Count == 1)
            {
                PickButton.InvokeClick();
                return true;
            }
            else if (keyboard.IsKeyPressed(Keys.Escape) && keyboard.KeysPressed.Count == 1)
            {
                ReturnButton.InvokeClick();
                return true;
            }
            return false;
        }

        public void FillList()
        {
            var RepeatedNameCount = new Dictionary<string, int>();
            DungeonTable.Cells.Clear();
            if(ParentContainer.PossibleDungeonsInfo.Dungeons.Any())
            {
                DungeonTable.IsVisible = true;
                this.Clear();
                var dungeonNameHeaderText = LocalizationManager.GetString("DungeonNameHeaderText").ToAscii();
                var authorHeaderText = LocalizationManager.GetString("AuthorHeaderText").ToAscii();
                var versionHeaderText = LocalizationManager.GetString("VersionHeaderText").ToAscii();

                DungeonTable.Cells[0, 0].Foreground = Color.White;
                DungeonTable.Cells[0, 0].Background = Color.Blue;
                DungeonTable.Cells[0, 0].Resize(1, (int)(DungeonTable.Width * 0.4));
                DungeonTable.Cells[0, 0].Settings.HorizontalAlignment = HorizontalAlign.Center;
                DungeonTable.Cells[0, 0].Value = dungeonNameHeaderText;
                DungeonTable.Cells[0, 1].Foreground = Color.White;
                DungeonTable.Cells[0, 1].Background = Color.Blue;
                DungeonTable.Cells[0, 1].Resize(1, (int)(DungeonTable.Width * 0.4));
                DungeonTable.Cells[0, 1].Settings.HorizontalAlignment = HorizontalAlign.Center;
                DungeonTable.Cells[0, 1].Value = authorHeaderText;
                DungeonTable.Cells[0, 2].Foreground = Color.White;
                DungeonTable.Cells[0, 2].Background = Color.Blue;
                DungeonTable.Cells[0, 2].Settings.HorizontalAlignment = HorizontalAlign.Center;
                DungeonTable.Cells[0, 2].Resize(1, (int)(DungeonTable.Width * 0.25));
                DungeonTable.Cells[0, 2].Value = versionHeaderText;

                var rowIndex = 1;
                foreach (var dungeon in ParentContainer.PossibleDungeonsInfo.Dungeons)
                {
                    var colorToUse = rowIndex % 2 == 0 ? Color.Black : new Color(33, 33, 33);
                    DungeonTable.Cells[rowIndex, 0].Foreground = Color.White;
                    DungeonTable.Cells[rowIndex, 0].Background = colorToUse;
                    DungeonTable.Cells[rowIndex, 0].Resize(1, (int)(DungeonTable.Width * 0.4));
                    DungeonTable.Cells[rowIndex, 0].Settings.HorizontalAlignment = HorizontalAlign.Left;
                    if (!RepeatedNameCount.ContainsKey(dungeon.Name))
                    {
                        DungeonTable.Cells[rowIndex, 0].Value = dungeon.Name;
                        RepeatedNameCount[dungeon.Name] = 1;
                    }
                    else
                    {
                        DungeonTable.Cells[rowIndex, 0].Value = $"{dungeon.Name} ({RepeatedNameCount[dungeon.Name]})";
                        RepeatedNameCount[dungeon.Name]++;
                    }
                    DungeonTable.Cells[rowIndex, 1].Foreground = Color.White;
                    DungeonTable.Cells[rowIndex, 1].Background = colorToUse;
                    DungeonTable.Cells[rowIndex, 1].Resize(1, (int)(DungeonTable.Width * 0.4));
                    DungeonTable.Cells[rowIndex, 1].Settings.HorizontalAlignment = HorizontalAlign.Center;
                    DungeonTable.Cells[rowIndex, 1].Value = dungeon.Author;
                    DungeonTable.Cells[rowIndex, 2].Foreground = Color.White;
                    DungeonTable.Cells[rowIndex, 2].Background = colorToUse;
                    DungeonTable.Cells[rowIndex, 2].Settings.HorizontalAlignment = HorizontalAlign.Center;
                    DungeonTable.Cells[rowIndex, 2].Resize(1, (int)(DungeonTable.Width * 0.25));
                    DungeonTable.Cells[rowIndex, 2].Value = dungeon.Version;
                    rowIndex++;
                }
                DungeonTable.Cells[1, 0].Select();
            }
            else
            {
                PickButton.IsEnabled = false;
                DungeonTable.IsVisible = false;
                this.Print(1, 6, LocalizationManager.GetString("NoDungeonsText"));
                if (BackendHandler.Instance.IsLocal)
                    this.Print(1, 7, LocalizationManager.GetString("NoLocalDungeonsSubtext"));
                else
                    this.Print(1, 7, LocalizationManager.GetString("NoServerDungeonsSubtext"));
            }
        }

        private void ReturnButton_Click(object? sender, EventArgs e)
        {
            ParentContainer.MoveToConsole(MenuConsoles.Main);
        }

        private void PickButton_Click(object? sender, EventArgs args)
        {
            try
            {
                if (SelectedItem.IsAtCurrentVersion)
                {
                    BackendHandler.Instance.CreateDungeon(SelectedItem.InternalName, LocalizationManager.CurrentLocale);
                    var message = BackendHandler.Instance.GetDungeonWelcomeMessage();

                    ParentContainer.ChangeConsoleContainerTo(ConsoleContainers.Message, ConsoleContainers.Game, LocalizationManager.GetString("BriefingMessageHeader"), message);
                }
                else
                {
                    ParentContainer.ActiveWindow = MessageBox.Show(new ColoredString(LocalizationManager.GetString("IncompatibleDungeonMessageBoxText").Format(new { DungeonJsonVersion = SelectedItem.Version, RequiredDungeonJsonVersion = ParentContainer.PossibleDungeonsInfo.CurrentVersion })), LocalizationManager.GetString("OKButtonText"), LocalizationManager.GetString("IncompatibleDungeonMessageBoxHeader"), Color.Red);
                }
            }
            catch (Exception)
            {
                ParentContainer.ChangeConsoleContainerTo(ConsoleContainers.Message, ConsoleContainers.Main, LocalizationManager.GetString("ErrorMessageHeader"), LocalizationManager.GetString("ErrorText"));
            }
        }
    }

    #pragma warning restore IDE0037 // Usar nombre de miembro inferido
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
