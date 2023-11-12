using SadConsole;
using SadRogue.Primitives;
using SadConsole.UI.Controls;
using RogueCustomsConsoleClient.UI.Consoles.Containers;
using RogueCustomsConsoleClient.Resources.Localization;
using System;
using RogueCustomsConsoleClient.UI.Windows;
using RogueCustomsConsoleClient.EngineHandling;

namespace RogueCustomsConsoleClient.UI.Consoles.GameConsole
{
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public class ButtonsConsole : GameSubConsole
    {
        public Button ExitButton { get; private set; }
        public Button SaveButton { get; private set; }

        public ButtonsConsole(GameConsoleContainer parent) : base(parent, GameConsoleConstants.ButtonsCellWidth, GameConsoleConstants.ButtonsCellHeight)
        {
            Build();
        }

        public new void Build()
        {
            base.Build();
            DefaultBackground = Color.Black;
            Font = Game.Instance.LoadFont("fonts/IBMCGA.font");
            UseMouse = true;
            RefreshOnlyOnStatusUpdate = false;
            var saveButtonText = LocalizationManager.GetString("SaveDungeonButtonText").ToAscii();
            SaveButton = new Button(saveButtonText.Length + 2)
            {
                Text = saveButtonText
            };
            SaveButton.Click += SaveButton_Click;
            SaveButton.Position = new Point((GameConsoleConstants.ButtonsCellWidth - SaveButton.Surface.Width) / 2, 5);
            var exitButtonText = LocalizationManager.GetString("ExitButtonText").ToAscii();
            ExitButton = new Button(exitButtonText.Length + 2)
            {
                Text = exitButtonText
            };
            ExitButton.Click += ExitButton_Click;
            ExitButton.Position = new Point((GameConsoleConstants.ButtonsCellWidth - ExitButton.Surface.Width) / 2, 15);

            Controls.Add(SaveButton);
            Controls.Add(ExitButton);
        }

        public override void Update(TimeSpan delta)
        {
            this.Clear();

            if(!IsEnabled || !IsVisible) return;

            var square = new Rectangle(0, 0, Width, Height);

            this.DrawBox(square, ShapeParameters.CreateBorder(new ColoredGlyph(Color.Red, Color.Black, 178)));

            base.Update(delta);
        }

        private void SaveButton_Click(object? sender, EventArgs e)
        {
            try
            {
                BackendHandler.Instance.SaveDungeon();
                ParentContainer.ActiveWindow = PromptBox.Show(new ColoredString(LocalizationManager.GetString("SuccessfulSavePromptText")), LocalizationManager.GetString("YesButtonText"), LocalizationManager.GetString("NoButtonText"), ParentContainer.LatestDungeonStatus.DungeonName, new Color(0, 255, 0),
                                    () => ParentContainer.ChangeConsoleContainerTo(ConsoleContainers.Main));
            }
            catch
            {
                ParentContainer.ActiveWindow = MessageBox.Show(new ColoredString(LocalizationManager.GetString("FailedSaveText")), LocalizationManager.GetString("OKButtonText"), ParentContainer.LatestDungeonStatus.DungeonName, Color.Red);
            }
        }

        private void ExitButton_Click(object? sender, EventArgs args)
        {
            ParentContainer.ActiveWindow = PromptBox.Show(new ColoredString(LocalizationManager.GetString("ExitPromptText")), LocalizationManager.GetString("YesButtonText"), LocalizationManager.GetString("NoButtonText"), ParentContainer.LatestDungeonStatus.DungeonName, Color.Red,
                                                () => ParentContainer.ChangeConsoleContainerTo(ConsoleContainers.Main));
        }
    }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
