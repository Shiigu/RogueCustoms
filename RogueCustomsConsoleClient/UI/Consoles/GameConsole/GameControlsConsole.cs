using SadConsole;
using SadRogue.Primitives;
using RogueCustomsConsoleClient.Helpers;
using RogueCustomsConsoleClient.UI.Consoles.Containers;
using RogueCustomsConsoleClient.Resources.Localization;
using System;

namespace RogueCustomsConsoleClient.UI.Consoles.GameConsole
{
    public class GameControlsConsole : GameSubConsole
    {
        private string MoveModeControlsString, MoveModeOnStairsControlsString, ActionModeControlsString;

        public GameControlsConsole(GameConsoleContainer parent) : base(parent, GameConsoleConstants.ControlsWidth, GameConsoleConstants.ControlsHeight)
        {
            Build();
        }

        public void Build()
        {
            base.Build();
            MoveModeControlsString = LocalizationManager.GetString("MoveModeControlsText").ToAscii();
            MoveModeOnStairsControlsString = LocalizationManager.GetString("MoveModeOnStairsControlsText").ToAscii();
            ActionModeControlsString = LocalizationManager.GetString("ActionModeControlsText").ToAscii();
            DefaultBackground = Color.Black;
            Font = Game.Instance.LoadFont("fonts/IBMCGA.font");
            RefreshOnlyOnStatusUpdate = false;
        }

        public override void Render(TimeSpan delta)
        {
            this.Clear();
            string textToRender = null;
            switch (ParentContainer.ControlMode)
            {
                case ControlMode.Move:
                    if (ParentContainer.LatestDungeonStatus.IsPlayerOnStairs())
                        textToRender = MoveModeOnStairsControlsString;
                    else
                        textToRender = MoveModeControlsString;
                    break;
                case ControlMode.ActionTargeting:
                    textToRender = ActionModeControlsString;
                    break;
                case ControlMode.None:
                    textToRender = "";
                    break;
            }

            this.Print((GameConsoleConstants.ControlsWidth - textToRender.Length) / 2, 0, textToRender);
            base.Render(delta);
        }
    }
}
