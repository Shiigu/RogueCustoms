using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using SadConsole;
using RogueCustomsConsoleClient.EngineHandling;
using RogueCustomsConsoleClient.UI.Consoles.MenuConsole;
using RogueCustomsConsoleClient.Resources.Localization;
using System.Collections.Generic;
using System;
using SadConsole.Input;

namespace RogueCustomsConsoleClient.UI.Consoles.Containers
{
    public class MenuConsoleContainer : ConsoleContainer
    {
        private MenuSubConsole ActiveConsole;

        private readonly MainMenuConsole MainMenuConsole;
        private readonly PickDungeonConsole PickDungeonConsole;
        private readonly OptionsConsole OptionsConsole;

        public List<DungeonListDto> PossibleDungeons;

        public MenuConsoleContainer(RootScreen parent) : base(parent, Game.Instance.ScreenCellsX, Game.Instance.ScreenCellsY)
        {
            MainMenuConsole = new MainMenuConsole(this, Width, Height);
            MainMenuConsole.IsVisible = false;
            MainMenuConsole.IsEnabled = false;
            PickDungeonConsole = new PickDungeonConsole(this, Width, Height);
            PickDungeonConsole.IsVisible = false;
            PickDungeonConsole.IsEnabled = false;
            OptionsConsole = new OptionsConsole(this, Width, Height);
            OptionsConsole.IsVisible = false;
            OptionsConsole.IsEnabled = false;

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
}