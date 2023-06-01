using SadConsole;
using SadRogue.Primitives;
using SadConsole.UI.Controls;
using SadConsole.UI;
using RoguelikeConsoleClient.UI.Consoles.Containers;

namespace RoguelikeConsoleClient.UI.Consoles.GameConsole
{
    public class ExperienceBarConsole : GameSubConsole
    {
        private readonly ProgressBar experienceBar;

        public ExperienceBarConsole(GameConsoleContainer parent) : base(parent, GameConsoleConstants.ExperienceBarWidth, GameConsoleConstants.ExperienceBarHeight)
        {
            DefaultBackground = Color.Black;
            Font = Game.Instance.LoadFont("fonts/IBMCGA.font");
            RefreshOnlyOnStatusUpdate = true;
            experienceBar = new ProgressBar(Width, Height, HorizontalAlignment.Left)
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

            experienceBar.SetThemeColors(themeColors);

            Controls.Add(experienceBar);
        }

        public override void Render(TimeSpan delta)
        {
            var dungeonStatus = ParentContainer.LatestDungeonStatus;

            var playerEntity = dungeonStatus.Entities.Find(e => e.IsPlayer);

            if (playerEntity != null)
            {
                this.Clear();

                experienceBar.Progress = (float)playerEntity.CurrentExperiencePercentage / 100;
                experienceBar.DisplayText = $"Experience: {playerEntity.Experience}/{playerEntity.ExperienceToLevelUp} ({playerEntity.CurrentExperiencePercentage}% of current level)";

            }

            base.Render(delta);
        }
    }
}
