﻿using SadConsole;
using SadConsole.UI.Controls;
using RogueCustomsConsoleClient.EngineHandling;
using RogueCustomsConsoleClient.Helpers;
using RogueCustomsConsoleClient.UI.Consoles.Containers;
using RogueCustomsConsoleClient.UI.Consoles.GameConsole.GameWindows;
using SadRogue.Primitives;
using RogueCustomsConsoleClient.Resources.Localization;
using System;
using System.Linq;
using SadConsole.UI;

namespace RogueCustomsConsoleClient.UI.Consoles.GameConsole
{
    public class PlayerInfoConsole : GameSubConsole
    {
        private string DetailsButtonText;
        private ProgressBar HPBar;
        private ProgressBar MPBar;
        public Button DetailsButton;

        public PlayerInfoConsole(GameConsoleContainer parent) : base(parent, GameConsoleConstants.PlayerInfoCellWidth, GameConsoleConstants.PlayerInfoCellHeight)
        {
            Build();
        }

        public void Build()
        {
            base.Build();
            Font = Game.Instance.LoadFont("fonts/IBMCGA.font");
            RefreshOnlyOnStatusUpdate = true;
            DetailsButtonText = $" {LocalizationManager.GetString("DetailsButtonText")} ".ToAscii();
            DetailsButton = new Button(DetailsButtonText.Length, 1)
            {
                Position = new Point((Width - DetailsButtonText.Length) / 2, 39),
                Text = DetailsButtonText
            };
            DetailsButton.Click += DetailsButton_Click;

            HPBar = new ProgressBar(Width - 4, 1, HorizontalAlignment.Left)
            {
                Position = new Point(2, 10),
                DisplayTextAlignment = HorizontalAlignment.Center,
                DisplayTextColor = Color.White
            };

            var hpBarThemeColors = new Colors();

            hpBarThemeColors.Appearance_ControlDisabled.Foreground = Color.Red;
            hpBarThemeColors.Appearance_ControlFocused.Foreground = Color.Red;
            hpBarThemeColors.Appearance_ControlMouseDown.Foreground = Color.Red;
            hpBarThemeColors.Appearance_ControlOver.Foreground = Color.Red;
            hpBarThemeColors.Appearance_ControlNormal.Foreground = Color.Red;
            hpBarThemeColors.Appearance_ControlSelected.Foreground = Color.Red;

            HPBar.SetThemeColors(hpBarThemeColors);

            MPBar = new ProgressBar(Width - 4, 1, HorizontalAlignment.Left)
            {
                Position = new Point(2, 11),
                DisplayTextAlignment = HorizontalAlignment.Center,
                DisplayTextColor = Color.White
            };

            var mpBarThemeColors = new Colors();

            mpBarThemeColors.Appearance_ControlDisabled.Foreground = Color.Blue;
            mpBarThemeColors.Appearance_ControlFocused.Foreground = Color.Blue;
            mpBarThemeColors.Appearance_ControlMouseDown.Foreground = Color.Blue;
            mpBarThemeColors.Appearance_ControlOver.Foreground = Color.Blue;
            mpBarThemeColors.Appearance_ControlNormal.Foreground = Color.Blue;
            mpBarThemeColors.Appearance_ControlSelected.Foreground = Color.Blue;

            MPBar.SetThemeColors(mpBarThemeColors);

            Controls.Add(HPBar);
            Controls.Add(MPBar);
            Controls.Add(DetailsButton);
        }

        private void DetailsButton_Click(object? sender, EventArgs e)
        {
            try
            {
                var playerInfo = BackendHandler.Instance.GetPlayerDetailInfo();
                ParentContainer.ActiveWindow = PlayerCharacterDetailWindow.Show(ParentContainer, playerInfo);
            }
            catch (Exception)
            {
                ParentContainer.ChangeConsoleContainerTo(ConsoleContainers.Message, ConsoleContainers.Main, LocalizationManager.GetString("ErrorMessageHeader"), LocalizationManager.GetString("ErrorText"));
            }
        }

        public override void Update(TimeSpan delta)
        {
            var dungeonStatus = ParentContainer.LatestDungeonStatus;
            if (dungeonStatus == null) return;

            var playerEntity = dungeonStatus.Entities.Find(e => e.IsPlayer);

            if (playerEntity != null)
            {
                this.Clear();

                var square = new Rectangle(0, 0, Width, Height);
                var title = $" {LocalizationManager.GetString("PlayerInfoConsoleTitle")} ";

                this.DrawBox(square, ShapeParameters.CreateBorder(new ColoredGlyph(Color.Violet, Color.Black, 178)));
                this.Print((square.Width - title.Length) / 2, 0, title, true);

                this.Print((square.Width - playerEntity.Name.Length) / 2, 2, playerEntity.Name.ToAscii());

                var levelString = LocalizationManager.GetString("PlayerLevelText").Format(new { CurrentLevel = playerEntity.Level });

                this.Print((square.Width - levelString.Length) / 2, 4, levelString, true);
                this.SetGlyph((square.Width - 1) / 2, 6, new ColoredGlyph(playerEntity.ConsoleRepresentation.ForegroundColor.ToSadRogueColor(), playerEntity.ConsoleRepresentation.BackgroundColor.ToSadRogueColor(), playerEntity.ConsoleRepresentation.Character.ToGlyph()));

                this.Print((square.Width - playerEntity.HPStatName.Length) / 2, 9, playerEntity.HPStatName, true);
                HPBar.DisplayText = $"{playerEntity.HP}/{playerEntity.MaxHP}";
                HPBar.Progress = (float)playerEntity.HP / playerEntity.MaxHP;

                MPBar.IsVisible = playerEntity.UsesMP;
                if(playerEntity.UsesMP)
                {
                    MPBar.DisplayText = $"{playerEntity.MP}/{playerEntity.MaxMP}";
                    MPBar.Progress = (float)playerEntity.MP / playerEntity.MaxMP;
                    this.Print((square.Width - playerEntity.MPStatName.Length) / 2, 12, playerEntity.MPStatName, true);
                }

                this.Print(2, 14, LocalizationManager.GetString("PlayerInfoWeaponHeader"), true);
                this.SetGlyph(2, 15, new ColoredGlyph(playerEntity.Weapon.ConsoleRepresentation.ForegroundColor.ToSadRogueColor(), playerEntity.Weapon.ConsoleRepresentation.BackgroundColor.ToSadRogueColor(), playerEntity.Weapon.ConsoleRepresentation.Character.ToGlyph()));
                this.Print(3, 15, $" - {playerEntity.Weapon.Name}", true);

                this.Print(2, 17, $"{playerEntity.DamageStatName}:", true);
                this.Print(2, 18, playerEntity.Damage, true);

                this.Print(2, 21, LocalizationManager.GetString("PlayerInfoArmorHeader"), true);
                this.SetGlyph(2, 22, new ColoredGlyph(playerEntity.Armor.ConsoleRepresentation.ForegroundColor.ToSadRogueColor(), playerEntity.Armor.ConsoleRepresentation.BackgroundColor.ToSadRogueColor(), playerEntity.Armor.ConsoleRepresentation.Character.ToGlyph()));
                this.Print(3, 22, $" - {playerEntity.Armor.Name}", true);

                this.Print(2, 24, $"{playerEntity.MitigationStatName}:", true);
                this.Print(2, 25, playerEntity.Mitigation, true);

                this.Print(2, 27, $"{playerEntity.MovementStatName}: {playerEntity.Movement}", true);

                this.Print(2, 29, LocalizationManager.GetString("PlayerInfoStatusesHeader"), true);
                if(dungeonStatus.AlteredStatuses.Any())
                {
                    const int statusBaseColumnIndex = 2;
                    const int statusBaseRowIndex = 31;
                    const int statusesPerRow = 13;
                    foreach (var als in dungeonStatus.AlteredStatuses)
                    {
                        var index = dungeonStatus.AlteredStatuses.IndexOf(als);
                        if (statusBaseRowIndex + (int)(index / statusesPerRow) == DetailsButton.Position.Y - 2)
                        {
                            this.Print(2, statusBaseRowIndex + (int)(index / statusesPerRow), LocalizationManager.GetString("PlayerInfoTooManyStatusesText"), true);
                            break;
                        }
                        this.Print(statusBaseColumnIndex + (index % statusesPerRow), statusBaseRowIndex + (int)(index / statusesPerRow), als.ConsoleRepresentation.Character.ToString().ToAscii(), als.ConsoleRepresentation.ForegroundColor.ToSadRogueColor(), als.ConsoleRepresentation.BackgroundColor.ToSadRogueColor());
                    }
                }
                else
                {
                    this.Print(4, 30, LocalizationManager.GetString("PlayerNoStatusesText"), true);
                }
            }
            base.Update(delta);
        }
    }
}
