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
        private string MoveModeControlsString, MoveModeOnStairsControlsString, MoveModeControlsSubString, ActionModeControlsString;

        public GameControlsConsole(GameConsoleContainer parent) : base(parent, GameConsoleConstants.ControlsWidth, GameConsoleConstants.ControlsHeight)
        {
            Build();
        }

        public void Build()
        {
            base.Build();
            MoveModeControlsString = LocalizationManager.GetString("MoveModeControlsText").ToAscii();
            MoveModeOnStairsControlsString = LocalizationManager.GetString("MoveModeOnStairsControlsText").ToAscii();
            MoveModeControlsSubString = LocalizationManager.GetString("MoveModeControlsSubText").ToAscii();
            ActionModeControlsString = LocalizationManager.GetString("ActionModeControlsText").ToAscii();
            DefaultBackground = Color.Black;
            Font = Game.Instance.LoadFont("fonts/IBMCGA.font");
            RefreshOnlyOnStatusUpdate = false;
        }

        public override void Update(TimeSpan delta)
        {
            this.Clear();
            string textToRender = null;
            string subtextToRender = null;
            switch (ParentContainer.ControlMode)
            {
                case ControlMode.Move:
                    if (ParentContainer.LatestDungeonStatus.IsPlayerOnStairs())
                        textToRender = MoveModeOnStairsControlsString;
                    else
                        textToRender = MoveModeControlsString;
                    subtextToRender = MoveModeControlsSubString;
                    break;
                case ControlMode.ActionTargeting:
                    textToRender = ActionModeControlsString;
                    subtextToRender = "";
                    break;
                case ControlMode.None:
                    textToRender = "";
                    subtextToRender = MoveModeControlsSubString;
                    break;
            }

            this.Print((GameConsoleConstants.ControlsWidth - textToRender.Length) / 2, 0, textToRender);
            this.Print((GameConsoleConstants.ControlsWidth - subtextToRender.Length) / 2, 2, subtextToRender);
            base.Update(delta);
        }
    }
}
