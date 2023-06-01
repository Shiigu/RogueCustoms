using SadConsole.UI.Controls;
using SadConsole.UI;
using SadConsole;
using SadRogue.Primitives;
using Themes = SadConsole.UI.Themes;
using Window = SadConsole.UI.Window;
using SadConsole.Input;
using RoguelikeGameEngine.Utils.InputsAndOutputs;
using RoguelikeConsoleClient.Helpers;
using Keyboard = SadConsole.Input.Keyboard;
using Console = SadConsole.Console;
using RoguelikeConsoleClient.UI.Consoles.Containers;

namespace RoguelikeConsoleClient.UI.Consoles.GameConsole.GameWindows
{
    public class PlayerCharacterDetailWindow : Window
    {
        private Button CloseButton;
        private string TitleCaption { get; set; }

        private ScrollBar ScrollBar;
        private Console TextAreaSubConsole;

        private PlayerCharacterDetailWindow(int width, int height) : base(width, height)
        {
        }

        public static Window Show(GameConsoleContainer parent, PlayerInfoDto playerInfo)
        {
            if (playerInfo == null) return null;
            var width = 65;
            var height = 30;

            var closeButtonText = "CLOSE";
            var closeButton = new Button(closeButtonText.Length + 2, 1)
            {
                Text = closeButtonText
            };

            var window = new PlayerCharacterDetailWindow(width, height);

            window.UseKeyboard = true;
            window.IsDirty = true;
            window.UseMouse = true;
            window.TitleCaption = $"{playerInfo.Name.ToUpperInvariant()} INFO";

            var drawingArea = new DrawingArea(window.Width, window.Height);
            drawingArea.OnDraw += window.DrawWindow;

            window.Controls.Add(drawingArea);

            var square = new Rectangle(2, 2, window.Width - 3, window.Height - 5);

            var textAreaSubConsole = new Console(window.Width - 5, 24, window.Width - 5, 1024)
            {
                Position = new Point(square.X, square.Y)
            };

            PrintPlayerLevelInfo(textAreaSubConsole, playerInfo.Level, playerInfo.IsAtMaxLevel, playerInfo.CurrentExperience, playerInfo.ExperienceToNextLevel);
            textAreaSubConsole.Cursor.Print("STATS:");
            PrintPlayerStatsInfo(textAreaSubConsole, "HP", playerInfo.CurrentHP, playerInfo.MaxHP, playerInfo.BaseMaxHP, playerInfo.MaxHPModifications);
            PrintPlayerStatsInfo(textAreaSubConsole, "Attack", playerInfo.CurrentAttack, 0, playerInfo.BaseAttack, playerInfo.AttackModifications);
            PrintPlayerStatsInfo(textAreaSubConsole, "Defense", playerInfo.CurrentDefense, 0, playerInfo.CurrentDefense, playerInfo.DefenseModifications);
            PrintPlayerStatsInfo(textAreaSubConsole, "Movement", playerInfo.CurrentMovement, 0, playerInfo.CurrentMovement, playerInfo.MovementModifications);
            PrintPlayerStatsInfo(textAreaSubConsole, "HP Regeneration", playerInfo.CurrentHPRegeneration, 0, playerInfo.BaseHPRegeneration, playerInfo.HPRegenerationModifications);
            PrintPlayerAlteredStatusesInfo(textAreaSubConsole, playerInfo.AlteredStatuses);
            PrintPlayerEquippedItemInfo(textAreaSubConsole, "WEAPON", playerInfo.WeaponInfo);
            PrintPlayerEquippedItemInfo(textAreaSubConsole, "ARMOR", playerInfo.ArmorInfo);
            textAreaSubConsole.Cursor.NewLine();

            window.TextAreaSubConsole = textAreaSubConsole;
            window.Children.Add(textAreaSubConsole);

            var scrollBar = new ScrollBar(Orientation.Vertical, square.Height + 1)
            {
                IsVisible = true,
                IsEnabled = false,
                Position = new Point(square.X + square.Width - 1, square.Y - 1)
            };

            scrollBar.ValueChanged += (o, e) =>
            {
                textAreaSubConsole.View = new Rectangle(0, scrollBar.Value, textAreaSubConsole.Width, textAreaSubConsole.ViewHeight);
            };

            window.ScrollBar = scrollBar;
            window.Controls.Add(scrollBar);

            closeButton.Position = new Point((window.Width - closeButton.Width) / 2, window.Height - closeButton.Surface.Height);
            closeButton.Click += (o, e) => window.Hide();
            closeButton.Theme = null;

            window.CloseButton = closeButton;
            window.Controls.Add(closeButton);

            window.Show(true);
            window.Parent = (window.Parent as RootScreen).ActiveContainer;
            window.Center();

            return window;
        }

        public void DrawWindow(DrawingArea ds, TimeSpan delta)
        {
            if (!ds.IsDirty) return;

            var window = (ds.Parent as ControlHost).ParentConsole as Console;

            ScrollBar.Maximum = Math.Max(0, TextAreaSubConsole.Cursor.Position.Y - TextAreaSubConsole.ViewHeight);
            ScrollBar.IsEnabled = TextAreaSubConsole.Cursor.Position.Y > TextAreaSubConsole.ViewHeight;

            ds.Surface.Clear();
            ColoredGlyph appearance = ((Themes.DrawingAreaTheme)ds.Theme).Appearance;
            ds.Surface.Fill(appearance.Foreground, appearance.Background, null);
            ds.Surface.DrawLine(new Point(0, 0), new Point(0, Height - 3), ICellSurface.ConnectedLineThick[3], Color.WhiteSmoke);
            ds.Surface.DrawLine(new Point(0, 0), new Point(Width - 1, 0), ICellSurface.ConnectedLineThick[3], Color.WhiteSmoke);
            ds.Surface.DrawLine(new Point(Width - 1, 0), new Point(Width - 1, Height - 3), ICellSurface.ConnectedLineThick[3], Color.WhiteSmoke);
            ds.Surface.DrawLine(new Point(0, window.Height - 3), new Point(Width - 1, Height - 3), ICellSurface.ConnectedLineThick[3], Color.WhiteSmoke);
            ds.Surface.ConnectLines(ICellSurface.ConnectedLineThick);
            ds.Surface.Print((Width - TitleCaption.Length - 2) / 2, 0, $" {TitleCaption} ", Color.Black, Color.WhiteSmoke);

            ds.IsDirty = true;
            ds.IsFocused = true;
        }

        private static void PrintPlayerLevelInfo(Console subConsole, int currentLevel, bool isAtMaxLevel, int currentExperience, int experienceToNextLevel)
        {
            subConsole.Cursor.Position = new Point(0, 0);
            subConsole.Cursor.Print($"LEVEL {currentLevel}");
            subConsole.Cursor.NewLine();
            subConsole.Cursor.NewLine();
            subConsole.Cursor.Print($"Current Experience: {currentExperience}");
            subConsole.Cursor.NewLine();
            if (!isAtMaxLevel)
                subConsole.Cursor.Print($"Experience to Level {currentLevel + 1}: {experienceToNextLevel}");
            else
                subConsole.Cursor.Print("AT MAX LEVEL!");
            subConsole.Cursor.NewLine();
            subConsole.Cursor.NewLine();
        }

        private static void PrintPlayerStatsInfo(Console subConsole, string statName, decimal currentStat, decimal maxStat, decimal baseStat, List<StatModificationDto> modifications)
        {
            subConsole.Cursor.NewLine();
            subConsole.Cursor.NewLine();
            switch (statName)
            {
                case "HP":
                        subConsole.Cursor.Print($"{statName}: {(int)currentStat}/{(int)maxStat}");
                        break;
                case "HP Regeneration":
                        subConsole.Cursor.Print($"{statName}: {currentStat}");
                        break;
                default:
                        subConsole.Cursor.Print($"{statName}: {(int)currentStat}");
                        break;
            }
            subConsole.Cursor.NewLine();
            subConsole.Cursor.Position = new Point(subConsole.Cursor.Position.X + 5, subConsole.Cursor.Position.Y);
            switch (statName)
            {
                case "HP":
                    subConsole.Cursor.Print($"Max Base: {baseStat}");
                    break;
                case "HP Regeneration":
                    subConsole.Cursor.Print($"Base: {baseStat}");
                    break;
                default:
                    subConsole.Cursor.Print($"Base: {(int)baseStat}");
                    break;
            }
            modifications.ForEach(mhm =>
            {
                subConsole.Cursor.NewLine();
                subConsole.Cursor.Position = new Point(subConsole.Cursor.Position.X + 5, subConsole.Cursor.Position.Y);
                if (statName == "HP Regeneration")
                {
                    if (mhm.Amount > 0)
                        subConsole.Cursor.Print(new ColoredString($"{mhm.Amount:+0.###;-0.###;0} from {mhm.Source}", Color.AnsiGreenBright, Color.Black));
                    else if (mhm.Amount < 0)
                        subConsole.Cursor.Print(new ColoredString($"{mhm.Amount:+0.###;-0.###;0} from {mhm.Source}", Color.Red, Color.Black));
                }
                else
                {
                    if (mhm.Amount > 0)
                        subConsole.Cursor.Print(new ColoredString($"{mhm.Amount.ToString("+0;-0;0")} from {mhm.Source}", Color.AnsiGreenBright, Color.Black));
                    else if (mhm.Amount < 0)
                        subConsole.Cursor.Print(new ColoredString($"{mhm.Amount.ToString("+0;-0;0")} from {mhm.Source}", Color.Red, Color.Black));
                }
            });
        }

        private static void PrintPlayerAlteredStatusesInfo(Console subConsole, List<AlteredStatusDetailDto> alteredStatuses)
        {
            subConsole.Cursor.NewLine();
            subConsole.Cursor.NewLine();
            subConsole.Cursor.Print("ALTERED STATUSES:");
            subConsole.Cursor.NewLine();
            if (!alteredStatuses.Any())
            {
                subConsole.Cursor.NewLine();
                subConsole.Cursor.Print("NONE!");
            }
            else
            {
                alteredStatuses.ForEach(als =>
                {
                    subConsole.Cursor.NewLine();
                    var statusGlyph = new ColoredGlyph(als.ConsoleRepresentation.ForegroundColor.ToSadRogueColor(), als.ConsoleRepresentation.BackgroundColor.ToSadRogueColor(), als.ConsoleRepresentation.Character.ToGlyph());
                    subConsole.Surface.SetGlyph(subConsole.Cursor.Position.X, subConsole.Cursor.Position.Y, statusGlyph);
                    subConsole.Cursor.Position = new Point(subConsole.Cursor.Position.X + 1, subConsole.Cursor.Position.Y);
                    if (als.RemainingTurns > 0)
                        subConsole.Cursor.Print(new ColoredString($" - {als.Name}: {als.Description} [{als.RemainingTurns} TURNS LEFT]", als.ConsoleRepresentation.ForegroundColor.ToSadRogueColor(), als.ConsoleRepresentation.BackgroundColor.ToSadRogueColor()));
                    else
                        subConsole.Cursor.Print(new ColoredString($" - {als.Name}: {als.Description}", als.ConsoleRepresentation.ForegroundColor.ToSadRogueColor(), als.ConsoleRepresentation.BackgroundColor.ToSadRogueColor()));
                });
            }
        }

        private static void PrintPlayerEquippedItemInfo(Console subConsole, string itemTypeName, EquippedItemDetailDto item)
        {
            subConsole.Cursor.NewLine();
            subConsole.Cursor.NewLine();
            subConsole.Cursor.Print($"CURRENT {itemTypeName}:");
            subConsole.Cursor.NewLine();
            subConsole.Cursor.NewLine();
            subConsole.Cursor.Print(item.Name);
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
                subConsole.Cursor.Print(line);
            }
        }

        public override bool ProcessKeyboard(Keyboard info)
        {
            if (info.IsKeyPressed(Keys.C) || info.IsKeyPressed(Keys.Escape))
            {
                CloseButton.InvokeClick();
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
}
