using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using SadConsole;
using RogueCustomsConsoleClient.EngineHandling;
using RogueCustomsConsoleClient.UI.Consoles.MenuConsole;
using RogueCustomsConsoleClient.Resources.Localization;
using System.Collections.Generic;
using System;
using SadConsole.Input;
using SadConsole.UI;

namespace RogueCustomsConsoleClient.UI.Consoles.Containers
{
    #pragma warning disable S2259 // Null pointers should not be dereferenced
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    #pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
    public class MenuConsoleContainer : ConsoleContainer
    {
        private MenuSubConsole ActiveConsole;

        private readonly MainMenuConsole MainMenuConsole;
        private readonly PickDungeonConsole PickDungeonConsole;
        private readonly OptionsConsole OptionsConsole;
        public Window ActiveWindow { get; set; }

        public DungeonListDto PossibleDungeonsInfo { get; set; }

        public MenuConsoleContainer(RootScreen parent) : base(parent, Game.Instance.ScreenCellsX, Game.Instance.ScreenCellsY)
        {
            MainMenuConsole = new MainMenuConsole(this, Width, Height)
            {
                IsVisible = false,
                IsEnabled = false
            };
            PickDungeonConsole = new PickDungeonConsole(this, Width, Height)
            {
                IsVisible = false,
                IsEnabled = false
            };
            OptionsConsole = new OptionsConsole(this, Width, Height)
            {
                IsVisible = false,
                IsEnabled = false
            };

            UseKeyboard = false;
            UseMouse = false;
            IsFocused = false;
            Start();
        }

        public override void Render(TimeSpan delta)
        {
            base.Render(delta);

            if (ActiveConsole != Children[0])
            {
                Children.Clear();
                Children.Add(ActiveConsole);
            }
            ActiveConsole.Render(delta);

            if (ActiveWindow?.IsVisible == true)
                ActiveWindow.Render(delta);
        }

        public override void Start()
        {
            MoveToConsole(MenuConsoles.Main);
            Children.Clear();
            Children.Add(ActiveConsole);
        }

        public void MoveToConsole(MenuConsoles console)
        {
            if(ActiveConsole != null)
            {
                ActiveConsole.IsVisible = false;
                ActiveConsole.IsEnabled = false;
            }
            ActiveWindow = null;
            switch (console)
            {
                case MenuConsoles.Main:
                    ActiveConsole = MainMenuConsole;
                    MainMenuConsole.Build();
                    break;
                case MenuConsoles.PickDungeon:
                    ActiveConsole = PickDungeonConsole;
                    PickDungeonConsole.Build();
                    break;
                case MenuConsoles.Options:
                    ActiveConsole = OptionsConsole;
                    OptionsConsole.Build();
                    break;
            }
            ActiveConsole.IsVisible = true;
            ActiveConsole.IsEnabled = true;
            switch (console)
            {
                case MenuConsoles.Main:
                    BackendHandler.Instance.IsLocal = Settings.Default.IsLocal;
                    BackendHandler.Instance.ServerAddress = Settings.Default.ServerAddress;
                    break;
                case MenuConsoles.PickDungeon:
                    PickDungeonConsole.FillList();
                    break;
                case MenuConsoles.Options:
                    OptionsConsole.LoadSettingDisplayData();
                    break;
            }
        }
    }

    public enum MenuConsoles
    {
        Main,
        PickDungeon,
        Options
    }
    #pragma warning restore S2259 // Null pointers should not be dereferenced
    #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    #pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}