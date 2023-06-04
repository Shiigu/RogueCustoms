using SadConsole;
using SadConsole.Input;
using RoguelikeConsoleClient.UI.Consoles.Error;
using System;

namespace RoguelikeConsoleClient.UI.Consoles.Containers
{
    public class MessageConsoleContainer : ConsoleContainer
    {
        private readonly MessageConsole MessageConsole;
        public ConsoleContainers ContainerToShiftTo;
        public string Message;
        public string Title;

        public MessageConsoleContainer(RootScreen parent) : base(parent, Game.Instance.ScreenCellsX, Game.Instance.ScreenCellsY)
        {
            MessageConsole = new MessageConsole(this, Width, Height);

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

        public override bool ProcessKeyboard(Keyboard info)
        {
            if (info.IsKeyPressed(Keys.Enter))
            {
                ChangeConsoleContainerTo(ContainerToShiftTo);
            }

            return true;
        }
    }
}