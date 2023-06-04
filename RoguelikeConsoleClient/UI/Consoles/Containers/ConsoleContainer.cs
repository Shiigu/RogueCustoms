using System.Collections.Generic;
using Console = SadConsole.Console;

namespace RoguelikeConsoleClient.UI.Consoles.Containers
{
    public abstract class ConsoleContainer : Console
    {
        public RootScreen ParentContainer;
        public List<Console> Consoles;

        protected ConsoleContainer(RootScreen parent, int width, int height) : base(width, height)
        {
            ParentContainer = parent;
        }

        public void ChangeConsoleContainerTo(ConsoleContainers console, ConsoleContainers? consoleToFollowUp = null, string title = null, string message = null)
        {
            ParentContainer.ChangeConsoleContainerTo(console, consoleToFollowUp, title, message);
        }

        public abstract void Start();
    }
}
