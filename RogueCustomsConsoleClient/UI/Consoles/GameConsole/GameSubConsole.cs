using SadConsole.UI;
using RogueCustomsConsoleClient.UI.Consoles.Containers;
using SadRogue.Primitives;
using SadConsole;

namespace RogueCustomsConsoleClient.UI.Consoles.GameConsole
{
    public abstract class GameSubConsole : ControlsConsole
    {
        public GameConsoleContainer ParentContainer { get; set; }
        public bool RefreshOnlyOnStatusUpdate { get; set; }

        protected GameSubConsole(GameConsoleContainer parent, int width, int height) : base(width, height)
        {
            ParentContainer = parent;
            Surface.DefaultBackground = Color.Black;
            Surface.DefaultForeground = Color.White;
        }

        public void Build()
        {
            Children.Clear();
            Controls.Clear();
            this.Clear();
        }
    }
}
