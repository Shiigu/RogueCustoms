using RoguelikeGameEngine.Utils.InputsAndOutputs;
using SadConsole;
using RoguelikeConsoleClient.EngineHandling;
using RoguelikeConsoleClient.UI.Consoles.MenuConsole;
using RoguelikeConsoleClient.Resources.Localization;
using System.Collections.Generic;
using System;

namespace RoguelikeConsoleClient.UI.Consoles.Containers
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
            PickDungeonConsole = new PickDungeonConsole(this, Width, Height);
            OptionsConsole = new OptionsConsole(this, Width, Height);

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
            ActiveConsole = MainMenuConsole;
            MainMenuConsole.Build();
            Children.Clear();
            Children.Add(ActiveConsole);
        }

        public void MoveToConsole(MenuConsoles console)
        {
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
            ActiveConsole.IsFocused = true;
            foreach (var control in ActiveConsole.Controls)
            {
                control.IsFocused = false;
            }
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