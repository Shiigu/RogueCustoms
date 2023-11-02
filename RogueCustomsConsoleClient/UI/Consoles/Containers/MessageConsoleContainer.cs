using SadConsole;
using SadConsole.Input;
using RogueCustomsConsoleClient.UI.Consoles.Error;
using System;

namespace RogueCustomsConsoleClient.UI.Consoles.Containers
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public class MessageConsoleContainer : ConsoleContainer
    {
        private readonly MessageConsole MessageConsole;
        public ConsoleContainers ContainerToShiftTo { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }

        public MessageConsoleContainer(RootScreen parent) : base(parent, Game.Instance.ScreenCellsX, Game.Instance.ScreenCellsY)
        {
            MessageConsole = new MessageConsole(Width, Height);

            Children.Add(MessageConsole);

            UseKeyboard = true;
            IsFocused = true;
        }

        public override void Render(TimeSpan delta)
        {
            MessageConsole.Render(delta);

            base.Render(delta);
        }

        public override void Start()
        {
            MessageConsole.Title = Title;
            MessageConsole.Message = Message;
            MessageConsole.DisplayMessage();
        }

        public override bool ProcessKeyboard(Keyboard keyboard)
        {
            if (keyboard.IsKeyPressed(Keys.Enter) && keyboard.KeysPressed.Count == 1)
            {
                ChangeConsoleContainerTo(ContainerToShiftTo);
            }

            return true;
        }
    }
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}