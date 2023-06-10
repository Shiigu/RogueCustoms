using SadConsole.UI.Controls;
using SadConsole.UI;
using SadConsole;
using SadRogue.Primitives;
using Themes = SadConsole.UI.Themes;
using Window = SadConsole.UI.Window;
using SadConsole.Input;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsConsoleClient.Helpers;
using Keyboard = SadConsole.Input.Keyboard;
using Console = SadConsole.Console;
using RogueCustomsConsoleClient.UI.Consoles.Containers;
using RogueCustomsConsoleClient.Resources.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RogueCustomsConsoleClient.UI.Consoles.GameConsole.GameWindows
{
    public class PlayerCharacterDetailWindow : Window
    {
        private Button CloseButton;
        private string TitleCaption { get; set; }
        private readonly string CloseButtonText = LocalizationManager.GetString("CloseButtonText").ToAscii();

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

            var window = new PlayerCharacterDetailWindow(width, height);

            var closeButton = new Button(window.CloseButtonText.Length + 2, 1)
            {
                Text = window.CloseButtonText
            };

            window.UseKeyboard = true;
            window.IsDirty = true;
            window.UseMouse = true;
            window.Font = Game.Instance.LoadFont("fonts/Cheepicus12.font");
            window.TitleCaption = LocalizationManager.GetString("PlayerCharacterDetailWindowTitleText").ToAscii().Format(new { PlayerName = playerInfo.Name.ToUpperInvariant() });

            var drawingArea = new DrawingArea(window.Width, window.Height);
            drawingArea.OnDraw += window.DrawWindow;

            window.Controls.Add(drawingArea);

            var square = new Rectangle(2, 2, window.Width - 3, window.Height - 5);

            var textAreaSubConsole = new Console(window.Width - 5, 24, window.Width - 5, 1024)
            {
                Position = new Point(square.X, square.Y),
                Font = Game.Instance.LoadFont("fonts/Cheepicus12.font")
            };

            PrintPlayerLevelInfo(textAreaSubConsole, playerInfo.Level, playerInfo.IsAtMaxLevel, playerInfo.CurrentExperience, playerInfo.ExperienceToNextLevel);
            textAreaSubConsole.Cursor.Print(LocalizationManager.GetString("PlayerCharacterDetailStatsHeader").ToAscii());
            playerInfo.Stats.ForEach(stat => PrintPlayerStatsInfo(textAreaSubConsole, stat));
            PrintPlayerAlteredStatusesInfo(textAreaSubConsole, playerInfo.AlteredStatuses);
            PrintPlayerEquippedItemInfo(textAreaSubConsole, LocalizationManager.GetString("PlayerCharacterDetailEquippedWeaponHeader").ToAscii(), playerInfo.WeaponInfo);
            PrintPlayerEquippedItemInfo(textAreaSubConsole, LocalizationManager.GetString("PlayerCharacterDetailEquippedArmorHeader").ToAscii(), playerInfo.ArmorInfo);
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
            ds.Surface.Print((Width - TitleCaption.Length - 2) / 2, 0, $" {TitleCaption.ToAscii()} ", Color.Black, Color.WhiteSmoke);

            ds.IsDirty = true;
            ds.IsFocused = true;
        }

        private static void PrintPlayerLevelInfo(Console subConsole, int currentLevel, bool isAtMaxLevel, int currentExperience, int experienceToNextLevel)
        {
            subConsole.Cursor.Position = new Point(0, 0);
            subConsole.Cursor.Print(LocalizationManager.GetString("PlayerLevelText").Format(new { CurrentLevel = currentLevel.ToString() }).ToAscii());
            subConsole.Cursor.NewLine();
            subConsole.Cursor.NewLine();
            subConsole.Cursor.Print(LocalizationManager.GetString("PlayerCharacterDetailCurrentExperienceText").Format(new { CurrentExperience = currentExperience.ToString() }).ToAscii());
            subConsole.Cursor.NewLine();
            if (!isAtMaxLevel)
                subConsole.Cursor.Print(LocalizationManager.GetString("PlayerCharacterDetailExperienceToLevelUpText").Format(new {
                    NextLevel = currentLevel + 1,
                    RequiredExperience = experienceToNextLevel.ToString()
                }).ToAscii());
            else
                subConsole.Cursor.Print(LocalizationManager.GetString("PlayerCharacterDetailAtMaxLevelText").ToAscii());
            subConsole.Cursor.NewLine();
            subConsole.Cursor.NewLine();
        }

        private static void PrintPlayerStatsInfo(Console subConsole, StatDto stat)
        {
            subConsole.Cursor.NewLine();
            subConsole.Cursor.NewLine();
            if (stat.HasMaxStat && stat.Max != null && stat.IsIntegerStat)
                subConsole.Cursor.Print($"{stat.Name}: {(int) stat.Current}/{(int) stat.Max}".ToAscii());
            else if (stat.HasMaxStat && stat.Max != null && !stat.IsIntegerStat)
                subConsole.Cursor.Print($"{stat.Name}: {stat.Current:0.000}/{stat.Max:0.000}".ToAscii());
            else if (!stat.HasMaxStat && !stat.IsIntegerStat)
                subConsole.Cursor.Print($"{stat.Name}: {stat.Current:0.000}".ToAscii());
            else if (!stat.HasMaxStat && stat.IsIntegerStat)
                subConsole.Cursor.Print($"{stat.Name}: {(int) stat.Current}".ToAscii());
            subConsole.Cursor.NewLine();
            subConsole.Cursor.Position = new Point(subConsole.Cursor.Position.X + 5, subConsole.Cursor.Position.Y);
            if (stat.HasMaxStat && stat.IsIntegerStat)
                subConsole.Cursor.Print($"{LocalizationManager.GetString("PlayerCharacterDetailMaxBaseText")}: {(int)stat.Base}".ToAscii());
            else if (stat.HasMaxStat && !stat.IsIntegerStat)
                subConsole.Cursor.Print($"{LocalizationManager.GetString("PlayerCharacterDetailMaxBaseText")}: {stat.Base:0.000}".ToAscii());
            else if (!stat.HasMaxStat && !stat.IsIntegerStat)
                subConsole.Cursor.Print($"{LocalizationManager.GetString("PlayerCharacterDetailBaseText")}: {stat.Base:0.000}".ToAscii());
            else if (!stat.HasMaxStat && stat.IsIntegerStat)
                subConsole.Cursor.Print($"{LocalizationManager.GetString("PlayerCharacterDetailBaseText")}: {(int)stat.Base}".ToAscii());
            stat.Modifications.ForEach(mhm =>
            {
                subConsole.Cursor.NewLine();
                subConsole.Cursor.Position = new Point(subConsole.Cursor.Position.X + 5, subConsole.Cursor.Position.Y);
                string modificationAmountText, sourceDisplayText;
                Color modificationDisplayForegroundColor = Color.White;
                sourceDisplayText = mhm.Source;

                if(stat.IsIntegerStat)
                    modificationAmountText = $"{mhm.Amount:+0;-0;0}";
                else
                    modificationAmountText = $"{mhm.Amount:+0.000;-0.000;0}";

                if (mhm.Amount > 0)
                    modificationDisplayForegroundColor = Color.AnsiGreenBright;
                else if (mhm.Amount < 0)
                    modificationDisplayForegroundColor = Color.Red;

                subConsole.Cursor.Print(new ColoredString(LocalizationManager.GetString("PlayerCharacterDetailStatAlterationText").Format(new
                {
                    Alteration = modificationAmountText,
                    Source = sourceDisplayText
                }).ToAscii(), modificationDisplayForegroundColor, Color.Black));
            });
        }

        private static void PrintPlayerAlteredStatusesInfo(Console subConsole, List<AlteredStatusDetailDto> alteredStatuses)
        {
            subConsole.Cursor.NewLine();
            subConsole.Cursor.NewLine();
            subConsole.Cursor.Print(LocalizationManager.GetString("PlayerCharacterDetailAlteredStatusesHeaderText").ToAscii());
            subConsole.Cursor.NewLine();
            if (!alteredStatuses.Any())
            {
                subConsole.Cursor.NewLine();
                subConsole.Cursor.Print(LocalizationManager.GetString("PlayerNoStatusesText").ToAscii());
            }
            else
            {
                alteredStatuses.ForEach(als =>
                {
                    subConsole.Cursor.NewLine();
                    var statusGlyph = new ColoredGlyph(als.ConsoleRepresentation.ForegroundColor.ToSadRogueColor(), als.ConsoleRepresentation.BackgroundColor.ToSadRogueColor(), als.ConsoleRepresentation.Character.ToGlyph());
                    subConsole.Surface.SetGlyph(subConsole.Cursor.Position.X, subConsole.Cursor.Position.Y, statusGlyph);
                    subConsole.Cursor.Position = new Point(subConsole.Cursor.Position.X + 1, subConsole.Cursor.Position.Y);
                    string statusDescriptionText = als.RemainingTurns > 0
                        ? LocalizationManager.GetString("PlayerCharacterDetailAlteredStatusDescriptionTextWithTurns").Format(new
                        {
                            StatusName = als.Name,
                            StatusDescription = als.Description,
                            RemainingTurns = als.RemainingTurns
                        })
                        : LocalizationManager.GetString("PlayerCharacterDetailAlteredStatusDescriptionText").Format(new
                        {
                            StatusName = als.Name,
                            StatusDescription = als.Description
                        });
                    subConsole.Cursor.Print(new ColoredString($" {statusDescriptionText}".ToAscii(), als.ConsoleRepresentation.ForegroundColor.ToSadRogueColor(), als.ConsoleRepresentation.BackgroundColor.ToSadRogueColor()));
                });
            }
        }

        private static void PrintPlayerEquippedItemInfo(Console subConsole, string itemTypeHeader, EquippedItemDetailDto item)
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
