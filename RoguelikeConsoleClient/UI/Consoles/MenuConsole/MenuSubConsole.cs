using SadConsole.UI;
using RoguelikeConsoleClient.UI.Consoles.Containers;
using SadRogue.Primitives;
using SadConsole;

namespace RoguelikeConsoleClient.UI.Consoles.MenuConsole
{
    public abstract class MenuSubConsole : ControlsConsole
    {
        protected MenuConsoleContainer ParentContainer;

        protected MenuSubConsole(MenuConsoleContainer parent, int width, int height) : base(width, height)
        {
            ParentContainer = parent;
            DefaultBackground = Color.Black;
            DefaultForeground = Color.White;
        }

        public void Build()
        {
            Children.Clear();
            Controls.Clear();
            this.Clear();
        }
    }
}
