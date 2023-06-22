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
        private string MoveModeNormalControlsString, MoveModeOnStairsControlsString, MoveModeImmobilizedControlsString, MoveModeCannotActControlsString, MoveModeControlsSubString, ActionModeControlsString;

        public GameControlsConsole(GameConsoleContainer parent) : base(parent, GameConsoleConstants.ControlsWidth, GameConsoleConstants.ControlsHeight)
        {
            Build();
        }

        public void Build()
        {
            base.Build();
            MoveModeNormalControlsString = LocalizationManager.GetString("MoveModeNormalControlsText").ToAscii();
            MoveModeOnStairsControlsString = LocalizationManager.GetString("MoveModeOnStairsControlsText").ToAscii();
            MoveModeImmobilizedControlsString = LocalizationManager.GetString("MoveModeImmobilizedControlsText").ToAscii();
            MoveModeCannotActControlsString = LocalizationManager.GetString("MoveModeCannotActControlsText").ToAscii();
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
                case ControlMode.NormalMove:
                    textToRender = MoveModeNormalControlsString;
                    subtextToRender = MoveModeControlsSubString;
                    break;
                case ControlMode.OnStairs:
                    textToRender = MoveModeOnStairsControlsString;
                    subtextToRender = MoveModeControlsSubString;
                    break;
                case ControlMode.Immobilized:
                    textToRender = MoveModeImmobilizedControlsString;
                    subtextToRender = MoveModeControlsSubString;
                    break;
                case ControlMode.CannotAct:
                    textToRender = MoveModeCannotActControlsString;
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
