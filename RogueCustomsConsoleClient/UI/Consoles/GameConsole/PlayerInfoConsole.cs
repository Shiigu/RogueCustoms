using SadConsole;
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
using RogueCustomsConsoleClient.UI.Consoles.Utils;

namespace RogueCustomsConsoleClient.UI.Consoles.GameConsole
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public class PlayerInfoConsole : GameSubConsole
    {
        private ProgressBar HPBar;
        private ProgressBar MPBar;
        private ProgressBar HungerBar;
        public Button DetailsButton { get; set; }

        public PlayerInfoConsole(GameConsoleContainer parent) : base(parent, GameConsoleConstants.PlayerInfoCellWidth, GameConsoleConstants.PlayerInfoCellHeight)
        {
            Build();
        }

        public new void Build()
        {
            base.Build();
            RefreshOnlyOnStatusUpdate = true;
            var detailsButtonText = $" {LocalizationManager.GetString("DetailsButtonText")} ".ToAscii();
            DetailsButton = new Button(detailsButtonText.Length, 1)
            {
                Position = new Point((Width - detailsButtonText.Length) / 2, 42),
                Text = detailsButtonText
            };
            DetailsButton.Click += DetailsButton_Click;

            HPBar = new RogueProgressBar(Width - 4, 1, HorizontalAlignment.Left)
            {
                Position = new Point(2, 10),
                DisplayTextAlignment = HorizontalAlignment.Center,
                DisplayTextColor = Color.White,
                BarColor = Color.Red,
                BackgroundColor = Color.Red,
                BackgroundGlyph = '░'.ToGlyph()
            };

            MPBar = new RogueProgressBar(Width - 4, 1, HorizontalAlignment.Left)
            {
                Position = new Point(2, 11),
                DisplayTextAlignment = HorizontalAlignment.Center,
                DisplayTextColor = Color.White,
                BarColor = Color.Blue,
                BackgroundColor = Color.Blue,
                BackgroundGlyph = '░'.ToGlyph()
            };

            HungerBar = new RogueProgressBar(Width - 4, 1, HorizontalAlignment.Left)
            {
                Position = new Point(2, 38),
                DisplayTextAlignment = HorizontalAlignment.Center,
                DisplayTextColor = Color.White,
                BarColor = Color.DarkRed,
                BackgroundColor = Color.DarkRed,
                BackgroundGlyph = '▒'.ToGlyph()
            };

            Controls.Add(HPBar);
            Controls.Add(MPBar);
            Controls.Add(HungerBar);
            Controls.Add(DetailsButton);
        }

        private void DetailsButton_Click(object? sender, EventArgs e)
        {
            try
            {
                var playerInfo = BackendHandler.Instance.GetPlayerDetailInfo();
                ParentContainer.ActiveWindow = PlayerCharacterDetailWindow.Show(playerInfo);
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
                        if (statusBaseRowIndex + (index / statusesPerRow) == DetailsButton.Position.Y - 2)
                        {
                            this.Print(2, statusBaseRowIndex + (index / statusesPerRow), LocalizationManager.GetString("PlayerInfoTooManyStatusesText"), true);
                            break;
                        }
                        this.Print(statusBaseColumnIndex + (index % statusesPerRow), statusBaseRowIndex + (index / statusesPerRow), als.ConsoleRepresentation.Character.ToString().ToAscii(), als.ConsoleRepresentation.ForegroundColor.ToSadRogueColor(), als.ConsoleRepresentation.BackgroundColor.ToSadRogueColor());
                    }
                }
                else
                {
                    this.Print(4, 30, LocalizationManager.GetString("PlayerNoStatusesText"), true);
                }

                HungerBar.IsVisible = playerEntity.UsesHunger;
                if (playerEntity.UsesHunger)
                {
                    HungerBar.DisplayText = $"{playerEntity.Hunger}/{playerEntity.MaxHunger}";
                    HungerBar.Progress = (float)playerEntity.Hunger / playerEntity.MaxHunger;
                    this.Print((square.Width - playerEntity.HungerStatName.Length) / 2, 37, playerEntity.HungerStatName, true);
                }
            }
            base.Update(delta);
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
