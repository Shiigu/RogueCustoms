using System.Collections.Generic;
using Console = SadConsole.Console;

namespace RogueCustomsConsoleClient.UI.Consoles.Containers
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    #pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
    public abstract class ConsoleContainer : Console
    {
        public RootScreen ParentContainer { get; set; }
        public List<Console> Consoles { get; set; }

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
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    #pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}
