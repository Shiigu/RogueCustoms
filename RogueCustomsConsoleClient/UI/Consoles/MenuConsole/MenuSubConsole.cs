using SadConsole.UI;
using RogueCustomsConsoleClient.UI.Consoles.Containers;
using SadRogue.Primitives;
using SadConsole;

namespace RogueCustomsConsoleClient.UI.Consoles.MenuConsole
{
    public abstract class MenuSubConsole : ControlsConsole
    {
        protected MenuConsoleContainer ParentContainer;

        protected MenuSubConsole(MenuConsoleContainer parent, int width, int height) : base(width, height)
        {
            ParentContainer = parent;
        }

        public void Build()
        {
            Children.Clear();
            Controls.Clear();
            this.Clear();
        }
    }
}
