using SadConsole.UI;
using RoguelikeConsoleClient.UI.Consoles.Containers;
using SadRogue.Primitives;

namespace RoguelikeConsoleClient.UI.Consoles.GameConsole
{
    public abstract class GameSubConsole : ControlsConsole
    {
        public GameConsoleContainer ParentContainer;

        protected GameSubConsole(GameConsoleContainer parent, int width, int height) : base(width, height)
        {
            ParentContainer = parent;
            DefaultBackground = Color.Black;
            DefaultForeground = Color.White;
        }

        public bool RefreshOnlyOnStatusUpdate;
    }
}
