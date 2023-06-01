using SadConsole;
using SadRogue.Primitives;
using RoguelikeConsoleClient.Helpers;
using RoguelikeConsoleClient.UI.Consoles.Containers;

namespace RoguelikeConsoleClient.UI.Consoles.GameConsole
{
    public class GameControlsConsole : GameSubConsole
    {
        private readonly string MoveModeControlsString = "<ARROWS> Move <A> Action Menu <I> Inventory Menu <S> Skip Turn";
        private readonly string MoveModeOnStairsControlsString = "<ARROWS> Move <A> Action Menu <I> Inventory Menu <U> Take Stairs <S> Skip Turn";
        private readonly string ActionModeControlsString = "<ARROWS> Move <A> Open Action Window <ESC> Cancel";

        public GameControlsConsole(GameConsoleContainer parent) : base(parent, GameConsoleConstants.ControlsWidth, GameConsoleConstants.ControlsWidth)
        {
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
