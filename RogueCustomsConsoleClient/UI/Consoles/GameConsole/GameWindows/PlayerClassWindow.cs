using SadConsole.UI.Controls;
using SadConsole.UI;
using SadConsole;
using SadRogue.Primitives;
using Themes = SadConsole.UI.Themes;
using Window = SadConsole.UI.Window;
using SadConsole.Input;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsConsoleClient.Helpers;
using System.Text.RegularExpressions;
using Keyboard = SadConsole.Input.Keyboard;
using Console = SadConsole.Console;
using RogueCustomsConsoleClient.UI.Consoles.Containers;
using RogueCustomsConsoleClient.EngineHandling;
using RogueCustomsConsoleClient.Resources.Localization;
using System.Collections.Generic;
using System.Linq;
using System;
using SadConsole.UI.Themes;
using SadConsole.Ansi;
using RogueCustomsConsoleClient.UI.Windows;
using RogueCustomsGameEngine.Game.Entities;

namespace RogueCustomsConsoleClient.UI.Consoles.GameConsole.GameWindows
{
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public sealed class PlayerClassWindow : Window
    {
        private readonly string SelectButtonText = LocalizationManager.GetString("SelectButtonText").ToAscii();
        private readonly string CancelButtonText = LocalizationManager.GetString("CancelButtonText").ToAscii();
        private readonly string LeftButtonText = LocalizationManager.GetString("LeftButtonText").ToAscii();
        private readonly string RightButtonText = LocalizationManager.GetString("RightButtonText").ToAscii();
        private Button SelectButton, CancelButton, LeftButton, RightButton;
        private ScrollBar ScrollBar;
        private Console TextAreaSubConsole;
        private string TitleCaption { get; set; }
        private string DetailsCaption { get; set; }
        private List<CharacterClassDto> CharacterClasses;
        private int LastShownIndex;
        private int CurrentlyShownIndex;
        private DrawingArea DrawingArea;
        private GameConsoleContainer ParentConsole;
        public string PickedPlayerClassId { get; set; }
        public string PickedPlayerName { get; set; }

        private PlayerClassWindow(int width, int height) : base(width, height)
        {
        }

        public static Window? Show(GameConsoleContainer parent, PlayerClassSelectionOutput playerClassSelectionData)
        {
            if (!playerClassSelectionData.CharacterClasses.Any()) return null;
            const int width = 65;
            const int height = 30;

            var window = new PlayerClassWindow(width, height);

            var leftButton = new Button(window.LeftButtonText.Length + 2, 3)
            {
                Text = window.LeftButtonText,
                TextAlignment = HorizontalAlignment.Center,
                Theme = new ButtonLinesTheme(),
                Position = new Point(1, 1)
            };

            var rightButton = new Button(window.RightButtonText.Length + 2, 3)
            {
                Text = window.RightButtonText,
                TextAlignment = HorizontalAlignment.Center,
                Theme = new ButtonLinesTheme(),
                Position = new Point(width - 5, 1)
            };

            var selectButton = new Button(window.SelectButtonText.Length + 4, 1)
            {
                Text = window.SelectButtonText
            };

            var cancelButton = new Button(window.CancelButtonText.Length + 2, 1)
            {
                Text = window.CancelButtonText
            };

            window.UseKeyboard = true;
            window.IsFocused = true;
            window.CharacterClasses = playerClassSelectionData.CharacterClasses;
            window.CurrentlyShownIndex = 0;
            window.IsDirty = true;
            window.ParentConsole = parent;
            window.Font = Game.Instance.LoadFont("fonts/Alloy_curses_12x12.font");
            window.TitleCaption = LocalizationManager.GetString("PlayerClassWindowTitleText");
            window.DetailsCaption = LocalizationManager.GetString("DetailsSubconsoleTitleText");
            window.LastShownIndex = -1;

            var square = new Rectangle(2, 6, window.Width - 3, window.Height - 10);
            var textAreaSubConsole = new Console(square.Width, square.Height, square.Width, 1024)
            {
                Position = new Point(square.X, square.Y),
                Font = Game.Instance.LoadFont("fonts/Alloy_curses_12x12.font")
            };
            window.TextAreaSubConsole = textAreaSubConsole;
            window.Children.Add(textAreaSubConsole);

            var drawingArea = new DrawingArea(window.Width, window.Height);
            drawingArea.OnDraw += window.DrawWindow;

            window.DrawingArea = drawingArea;
            window.Controls.Add(drawingArea);

            var scrollBar = new ScrollBar(Orientation.Vertical, square.Height + 2)
            {
                IsVisible = true,
                IsEnabled = false,
                Position = new Point(width - 2, square.Y - 1)
            };

            scrollBar.ValueChanged += (o, e) => textAreaSubConsole.View = new Rectangle(0, scrollBar.Value, textAreaSubConsole.Width, textAreaSubConsole.ViewHeight);

            window.ScrollBar = scrollBar;
            window.Controls.Add(scrollBar);

            selectButton.Position = new Point(2, window.Height - selectButton.Surface.Height);
            selectButton.Click += (o, e) => {
                var item = window.CharacterClasses.ElementAtOrDefault(window.CurrentlyShownIndex);
                if(item.RequiresNamePrompt)
                {
                    parent.ActiveWindow = InputBox.Show(new ColoredString(LocalizationManager.GetString("InputBoxPromptText").Format(new { ClassName = item.Name })), LocalizationManager.GetString("InputBoxAffirmativeButtonText"), LocalizationManager.GetString("NoButtonText"), LocalizationManager.GetString("InputBoxTitleText"), item.Name, Color.Orange,
                                                    (name) =>
                                                    {
                                                        window.SendClassSelection(new PlayerClassSelectionInput
                                                        {
                                                            ClassId = item.ClassId,
                                                            Name = name
                                                        });
                                                        window.Hide();
                                                    });
                }
                else
                {
                    window.SendClassSelection(new PlayerClassSelectionInput
                    {
                        ClassId = item.ClassId,
                        Name = item.Name
                    });
                    window.Hide();
                }
            };
            selectButton.Theme = null;

            leftButton.Click += (o, e) => {
                window.CurrentlyShownIndex = Math.Max(0, window.CurrentlyShownIndex - 1);
                window.DrawingArea.IsDirty = true;
            };
            rightButton.Click += (o, e) => {
                window.CurrentlyShownIndex = Math.Min(window.CharacterClasses.Count - 1, window.CurrentlyShownIndex + 1);
                window.DrawingArea.IsDirty = true;
            };

            cancelButton.Position = new Point(window.Width - cancelButton.Surface.Width - 2, window.Height - cancelButton.Surface.Height);
            cancelButton.Click += (o, e) => {
                parent.ActiveWindow = PromptBox.Show(new ColoredString(LocalizationManager.GetString("ExitPromptText")), LocalizationManager.GetString("YesButtonText"), LocalizationManager.GetString("NoButtonText"), LocalizationManager.GetString("PlayerClassWindowTitleText"), Color.Red,
                                                () => {
                                                    parent.ChangeConsoleContainerTo(ConsoleContainers.Main);
                                                    window.Hide();
                                                });
            };
            cancelButton.Theme = null;

            window.LeftButton = leftButton;
            window.Controls.Add(leftButton);
            window.RightButton = rightButton;
            window.Controls.Add(rightButton);
            window.SelectButton = selectButton;
            window.Controls.Add(selectButton);
            window.CancelButton = cancelButton;
            window.Controls.Add(cancelButton);

            window.Show(true);
            window.Parent = (window.Parent as RootScreen)?.ActiveContainer;
            window.Center();

            return window;
        }

        public void SendClassSelection(PlayerClassSelectionInput selectionInput)
        {
            try
            {
                BackendHandler.Instance.SetPlayerClassSelection(selectionInput);
                ParentConsole.ControlMode = ControlMode.NormalMove;
                ParentConsole.HasSetupPlayerData = true;
                ParentConsole.RequiresRefreshingDungeonState = true;
                ParentConsole.ChangeSubConsolesRenderState(true);
            }
            catch (Exception)
            {
                ParentConsole.ChangeConsoleContainerTo(ConsoleContainers.Message, ConsoleContainers.Main, LocalizationManager.GetString("ErrorMessageHeader"), LocalizationManager.GetString("ErrorText"));
            }
            finally
            {
                Hide();
            }
        }

        public void DrawWindow(DrawingArea ds, TimeSpan delta)
        {
            if (!ds.IsDirty) return;

            if (ds.Parent is not ControlHost host) return;
            if (host.ParentConsole is not Console window) return;

            TextAreaSubConsole.Clear();
            TextAreaSubConsole.Cursor.Position = new Point(0, 0);
            ds.Surface.Clear();
            ColoredGlyph appearance = ((Themes.DrawingAreaTheme)ds.Theme).Appearance;
            ds.Surface.Fill(appearance.Foreground, appearance.Background, null);
            ds.Surface.DrawLine(new Point(0, 0), new Point(0, Height - 3), ICellSurface.ConnectedLineThick[3], Color.Orange);
            ds.Surface.DrawLine(new Point(0, 0), new Point(Width - 1, 0), ICellSurface.ConnectedLineThick[3], Color.Orange);
            ds.Surface.DrawLine(new Point(0, 4), new Point(Width - 1, 4), ICellSurface.ConnectedLineThick[3], Color.Orange);
            ds.Surface.DrawLine(new Point(Width - 1, 0), new Point(Width - 1, Height - 3), ICellSurface.ConnectedLineThick[3], Color.Orange);
            ds.Surface.DrawLine(new Point(0, window.Height - 3), new Point(Width - 1, Height - 3), ICellSurface.ConnectedLineThick[3], Color.Orange);
            ds.Surface.ConnectLines(ICellSurface.ConnectedLineThick);
            ds.Surface.Print((Width - TitleCaption.Length - 2) / 2, 0, $" {TitleCaption.ToAscii()} ", Color.Black, Color.Orange);
            ds.Surface.Print((Width - DetailsCaption.Length - 2) / 2, 4, $" {DetailsCaption.ToAscii()} ", Color.Black, Color.Orange);

            LeftButton.IsVisible = CurrentlyShownIndex != 0;
            LeftButton.IsEnabled = LeftButton.IsVisible;
            RightButton.IsVisible = CurrentlyShownIndex != CharacterClasses.Count - 1;
            RightButton.IsEnabled = RightButton.IsVisible;

            var currentClass = CharacterClasses.ElementAtOrDefault(CurrentlyShownIndex);
            ds.Surface.Print((Width - currentClass.Name.Length - 2) / 2, 2, $" {currentClass.Name.ToAscii()} ", Color.White, Color.Black);

            TextAreaSubConsole.Cursor.Print(currentClass.Name.ToAscii());
            TextAreaSubConsole.Cursor.NewLine();
            TextAreaSubConsole.Cursor.NewLine();
            var representationGlyph = new ColoredGlyph(currentClass.ConsoleRepresentation.ForegroundColor.ToSadRogueColor(), currentClass.ConsoleRepresentation.BackgroundColor.ToSadRogueColor(), currentClass.ConsoleRepresentation.Character.ToGlyph());
            TextAreaSubConsole.Surface.SetGlyph(TextAreaSubConsole.Cursor.Position.X, TextAreaSubConsole.Cursor.Position.Y, representationGlyph);
            TextAreaSubConsole.Cursor.NewLine();
            var linesInDescription = currentClass.Description.Split(
                new[] { "\r\n", "\n" }, StringSplitOptions.None
                );
            foreach (var line in linesInDescription)
            {
                TextAreaSubConsole.Cursor.NewLine();
                TextAreaSubConsole.Cursor.Print(line.ToAscii());
            }
            TextAreaSubConsole.Cursor.NewLine();
            TextAreaSubConsole.Cursor.NewLine();
            TextAreaSubConsole.Cursor.Print(LocalizationManager.GetString("PlayerClassStatsHeader").ToAscii());
            foreach (var stat in currentClass.InitialStats)
            {
                PrintPlayerStatsInfo(TextAreaSubConsole, stat);
            }
            TextAreaSubConsole.Cursor.NewLine();
            TextAreaSubConsole.Cursor.NewLine();
            TextAreaSubConsole.Cursor.Print($"{currentClass.SightRangeName}: {currentClass.SightRangeStat}".ToAscii());
            PrintPlayerStartingItemInfo(TextAreaSubConsole, LocalizationManager.GetString("PlayerClassStartingWeaponHeader").ToAscii(), currentClass.StartingWeapon);
            PrintPlayerStartingItemInfo(TextAreaSubConsole, LocalizationManager.GetString("PlayerClassStartingArmorHeader").ToAscii(), currentClass.StartingArmor);
            TextAreaSubConsole.Cursor.NewLine();
            TextAreaSubConsole.Cursor.NewLine();
            TextAreaSubConsole.Cursor.Print(LocalizationManager.GetString("PlayerClassStartingInventoryHeader").ToAscii());
            if (currentClass.StartingInventory.Any())
            {
                foreach (var item in currentClass.StartingInventory)
                {
                    PrintPlayerStartingInventoryItemInfo(TextAreaSubConsole, item);
                }
            }
            else
            {
                TextAreaSubConsole.Cursor.NewLine();
                TextAreaSubConsole.Cursor.NewLine();
                TextAreaSubConsole.Cursor.Print(LocalizationManager.GetString("PlayerClassNoStartingInventoryText").ToAscii());
            }
            TextAreaSubConsole.Cursor.NewLine();
            TextAreaSubConsole.Cursor.NewLine();
            TextAreaSubConsole.Cursor.Print($"{currentClass.InventorySizeName}: {currentClass.InventorySizeStat}".ToAscii());

            if (CurrentlyShownIndex != LastShownIndex)
            {
                ScrollBar.Value = 0;
            }
            ScrollBar.Maximum = Math.Max(0, TextAreaSubConsole.Cursor.Position.Y - TextAreaSubConsole.ViewHeight + 1);
            ScrollBar.IsEnabled = TextAreaSubConsole.Cursor.Position.Y > TextAreaSubConsole.ViewHeight;
            LastShownIndex = CurrentlyShownIndex;
            ds.IsDirty = true;
            ds.IsFocused = true;
        }

        private static void PrintPlayerStatsInfo(Console subConsole, CharacterClassStatDto stat)
        {
            if (!stat.Visible) return;
            subConsole.Cursor.NewLine();
            subConsole.Cursor.NewLine();
            if (!stat.IsIntegerStat)
                subConsole.Cursor.Print($"{stat.Name}: {stat.Base:0.#####}".ToAscii());
            else
                subConsole.Cursor.Print($"{stat.Name}: {(int)stat.Base}".ToAscii());
            subConsole.Cursor.NewLine();
            subConsole.Cursor.Position = new Point(subConsole.Cursor.Position.X + 5, subConsole.Cursor.Position.Y);
            subConsole.Cursor.Print(new ColoredString(LocalizationManager.GetString("PlayerClassIncreasePerLevelText").Format(new
            {
                Increase = $"{stat.IncreasePerLevel:+0.#####;-0.#####;0}"
            }).ToAscii()));
        }

        private static void PrintPlayerStartingItemInfo(Console subConsole, string itemTypeHeader, ItemDetailDto item)
        {
            subConsole.Cursor.NewLine();
            subConsole.Cursor.NewLine();
            subConsole.Cursor.Print(itemTypeHeader.ToAscii());
            subConsole.Cursor.NewLine();
            subConsole.Cursor.NewLine();
            subConsole.Cursor.Print(item.Name.ToAscii());
            subConsole.Cursor.NewLine();
            subConsole.Cursor.NewLine();
            var itemGlyph = new ColoredGlyph(item.ConsoleRepresentation.ForegroundColor.ToSadRogueColor(), item.ConsoleRepresentation.BackgroundColor.ToSadRogueColor(), item.ConsoleRepresentation.Character.ToGlyph());
            subConsole.Surface.SetGlyph(subConsole.Cursor.Position.X, subConsole.Cursor.Position.Y, itemGlyph);
            subConsole.Cursor.NewLine();

            var linesInItemInfoDescription = item.Description.Split(
                new[] { "\r\n", "\n" }, StringSplitOptions.None
                );
            foreach (var line in linesInItemInfoDescription)
            {
                subConsole.Cursor.NewLine();
                subConsole.Cursor.Print(line.ToAscii());
            }
        }

        private static void PrintPlayerStartingInventoryItemInfo(Console subConsole, ItemDetailDto item)
        {
            subConsole.Cursor.NewLine();
            subConsole.Cursor.NewLine();
            var itemGlyph = new ColoredGlyph(item.ConsoleRepresentation.ForegroundColor.ToSadRogueColor(), item.ConsoleRepresentation.BackgroundColor.ToSadRogueColor(), item.ConsoleRepresentation.Character.ToGlyph());
            subConsole.Surface.SetGlyph(subConsole.Cursor.Position.X, subConsole.Cursor.Position.Y, itemGlyph);
            subConsole.Cursor.Position = new Point(subConsole.Cursor.Position.X + 1, subConsole.Cursor.Position.Y);
            subConsole.Cursor.Print($" - {item.Name.ToAscii()}");
        }

        public override bool ProcessKeyboard(Keyboard info)
        {
            if (info.IsKeyPressed(Keys.Left) && info.KeysPressed.Count == 1)
            {
                LeftButton.InvokeClick();
            }
            else if (info.IsKeyPressed(Keys.Right) && info.KeysPressed.Count == 1)
            {
                RightButton.InvokeClick();
            }
            else if (info.IsKeyPressed(Keys.Up) && info.KeysPressed.Count == 1)
            {
                ScrollBar.Value--;
            }
            else if (info.IsKeyPressed(Keys.Down) && info.KeysPressed.Count == 1)
            {
                ScrollBar.Value++;
            }
            else if ((info.IsKeyPressed(Keys.S) || info.IsKeyPressed(Keys.Enter)) && info.KeysPressed.Count == 1)
            {
                SelectButton.InvokeClick();
            }
            else if ((info.IsKeyPressed(Keys.C) || info.IsKeyPressed(Keys.Escape)) && info.KeysPressed.Count == 1)
            {
                CancelButton.InvokeClick();
            }

            return true;
        }
        public override bool ProcessMouse(MouseScreenObjectState state)
        {
            if (state.Mouse.ScrollWheelValueChange > 0)
                ScrollBar.Value++;
            else if (state.Mouse.ScrollWheelValueChange < 0)
                ScrollBar.Value--;
            return base.ProcessMouse(state);
        }
    }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
