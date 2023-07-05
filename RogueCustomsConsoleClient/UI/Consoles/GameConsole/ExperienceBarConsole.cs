using SadConsole;
using SadRogue.Primitives;
using SadConsole.UI.Controls;
using SadConsole.UI;
using RogueCustomsConsoleClient.UI.Consoles.Containers;
using RogueCustomsConsoleClient.Resources.Localization;
using RogueCustomsConsoleClient.Helpers;
using System;

namespace RogueCustomsConsoleClient.UI.Consoles.GameConsole
{
    public class ExperienceBarConsole : GameSubConsole
    {
        private ProgressBar ExperienceBar;

        public ExperienceBarConsole(GameConsoleContainer parent) : base(parent, GameConsoleConstants.ExperienceBarWidth, GameConsoleConstants.ExperienceBarHeight)
        {
            Build();
        }

        public void Build()
        {
            base.Build();
            DefaultBackground = Color.Black;
            Font = Game.Instance.LoadFont("fonts/IBMCGA.font");
            RefreshOnlyOnStatusUpdate = true;
            ExperienceBar = new ProgressBar(Width, Height, HorizontalAlignment.Left)
            {
                Position = new Point(0, 0),
                DisplayTextAlignment = HorizontalAlignment.Center,
                DisplayTextColor = Color.White
            };

            var themeColors = new Colors();

            themeColors.Appearance_ControlDisabled.Foreground = Color.Blue;
            themeColors.Appearance_ControlFocused.Foreground = Color.Blue;
            themeColors.Appearance_ControlMouseDown.Foreground = Color.Blue;
            themeColors.Appearance_ControlOver.Foreground = Color.Blue;
            themeColors.Appearance_ControlNormal.Foreground = Color.Blue;
            themeColors.Appearance_ControlSelected.Foreground = Color.Blue;

            ExperienceBar.SetThemeColors(themeColors);

            Controls.Add(ExperienceBar);
        }

        public override void Update(TimeSpan delta)
        {
            var dungeonStatus = ParentContainer.LatestDungeonStatus;
            if (dungeonStatus == null) return;

            var playerEntity = dungeonStatus.Entities.Find(e => e.IsPlayer);

            if (playerEntity != null)
            {
                this.Clear();

                ExperienceBar.Progress = (float)playerEntity.CurrentExperiencePercentage / 100;
                ExperienceBar.DisplayText = LocalizationManager.GetString("ExperienceBarDisplayText").Format(new
                {
                    CurrentExperience = playerEntity.Experience.ToString(),
                    ExperienceToLevelUp = playerEntity.ExperienceToLevelUp.ToString(),
                    Percentage = playerEntity.CurrentExperiencePercentage.ToString()
                }).ToAscii();
            }

            base.Update(delta);
        }
    }
}
