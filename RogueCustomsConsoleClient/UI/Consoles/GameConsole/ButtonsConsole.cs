using SadConsole;
using SadRogue.Primitives;
using SadConsole.UI.Controls;
using RogueCustomsConsoleClient.UI.Consoles.Containers;
using RogueCustomsConsoleClient.Resources.Localization;
using System;
using RogueCustomsConsoleClient.UI.Windows;

namespace RogueCustomsConsoleClient.UI.Consoles.GameConsole
{
    public class ButtonsConsole : GameSubConsole
    {
        private string ExitButtonText;
        public Button ExitButton;

        public ButtonsConsole(GameConsoleContainer parent) : base(parent, GameConsoleConstants.ButtonsCellWidth, GameConsoleConstants.ButtonsCellHeight)
        {
            Build();
        }

        public void Build()
        {
            base.Build();
            DefaultBackground = Color.Black;
            Font = Game.Instance.LoadFont("fonts/IBMCGA.font");
            UseMouse = true;
            RefreshOnlyOnStatusUpdate = false;
            ExitButtonText = LocalizationManager.GetString("ExitButtonText").ToAscii();
            ExitButton = new Button(ExitButtonText.Length + 2)
            {
                Text = ExitButtonText
            };
            ExitButton.Click += ExitButton_Click;
            ExitButton.Position = new Point((GameConsoleConstants.ButtonsCellWidth - ExitButton.Surface.Width) / 2, 10);

            Controls.Add(ExitButton);
        }

        public override void Update(TimeSpan delta)
        {
            this.Clear();

            var square = new Rectangle(0, 0, Width, Height);

            this.DrawBox(square, ShapeParameters.CreateBorder(new ColoredGlyph(Color.Red, Color.Black, 178)));

            base.Update(delta);
        }

        private void ExitButton_Click(object? sender, EventArgs args)
        {
            ParentContainer.ActiveWindow = PromptBox.Show(new ColoredString(LocalizationManager.GetString("ExitPromptText")), LocalizationManager.GetString("YesButtonText"), LocalizationManager.GetString("NoButtonText"), ParentContainer.LatestDungeonStatus.DungeonName, Color.Red,
                                                () => ParentContainer.ChangeConsoleContainerTo(ConsoleContainers.Main));
        }
    }
}
