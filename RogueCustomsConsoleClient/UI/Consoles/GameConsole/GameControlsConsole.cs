using SadConsole;
using SadRogue.Primitives;
using RogueCustomsConsoleClient.Helpers;
using RogueCustomsConsoleClient.UI.Consoles.Containers;
using RogueCustomsConsoleClient.Resources.Localization;
using System;

namespace RogueCustomsConsoleClient.UI.Consoles.GameConsole
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public class GameControlsConsole : GameSubConsole
    {
        private string MoveModeNormalControlsString, MoveModeOnStairsControlsString, MoveModeImmobilizedControlsString, MoveModeCannotActControlsString, MoveModeControlsSubString, ActionModeControlsString, ViewModeControlsString;

        public GameControlsConsole(GameConsoleContainer parent) : base(parent, GameConsoleConstants.ControlsWidth, GameConsoleConstants.ControlsHeight)
        {
            Build();
        }

        public new void Build()
        {
            base.Build();
            MoveModeNormalControlsString = LocalizationManager.GetString("MoveModeNormalControlsText").ToAscii();
            MoveModeOnStairsControlsString = LocalizationManager.GetString("MoveModeOnStairsControlsText").ToAscii();
            MoveModeImmobilizedControlsString = LocalizationManager.GetString("MoveModeImmobilizedControlsText").ToAscii();
            MoveModeCannotActControlsString = LocalizationManager.GetString("MoveModeCannotActControlsText").ToAscii();
            MoveModeControlsSubString = LocalizationManager.GetString("MoveModeControlsSubText").ToAscii();
            ActionModeControlsString = LocalizationManager.GetString("ActionModeControlsText").ToAscii();
            ViewModeControlsString = LocalizationManager.GetString("ViewModeControlsText").ToAscii();
            DefaultBackground = Color.Black;
            Font = Game.Instance.LoadFont("fonts/IBMCGA.font");
            RefreshOnlyOnStatusUpdate = false;
        }

        public override void Update(TimeSpan delta)
        {
            this.Clear();
            var textToRender = string.Empty;
            var subtextToRender = string.Empty;
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
                case ControlMode.ViewTargeting:
                    textToRender = ViewModeControlsString;
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
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
