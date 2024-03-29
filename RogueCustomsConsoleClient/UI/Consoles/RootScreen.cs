﻿using SadConsole;
using RogueCustomsConsoleClient.UI.Consoles.Containers;
using System;
using System.Collections.Generic;
using SadConsole.Input;

namespace RogueCustomsConsoleClient.UI.Consoles
{
    #pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
    public class RootScreen : ScreenObject
    {
        private readonly ScreenSurface _map;
        public ConsoleContainer ActiveContainer { get; set; }

        public readonly MenuConsoleContainer MenuConsoleContainer;
        public readonly GameConsoleContainer GameConsoleContainer;
        public readonly MessageConsoleContainer MessageConsoleContainer;

        public RootScreen()
        {
            _map = new ScreenSurface(Game.Instance.ScreenCellsX, Game.Instance.ScreenCellsY);

            MenuConsoleContainer = new MenuConsoleContainer(this)
            {
                IsEnabled = true,
                IsVisible = true
            };
            GameConsoleContainer = new GameConsoleContainer(this)
            {
                IsEnabled = false,
                IsVisible = false
            };
            MessageConsoleContainer = new MessageConsoleContainer(this)
            {
                IsEnabled = false,
                IsVisible = false
            };

            ActiveContainer = MenuConsoleContainer;
        }

        public override void Render(TimeSpan delta)
        {
            Children.Clear();
            Children.Add(_map);
            Children.Add(ActiveContainer);

            base.Render(delta);
        }

        public void ChangeConsoleContainerTo(ConsoleContainers console, ConsoleContainers? consoleToFollowUp = null, string title = null, string message = null)
        {
            ActiveContainer.IsEnabled = false;
            ActiveContainer.IsVisible = false;
            ActiveContainer.IsFocused = false;
            switch (console)
            {
                case ConsoleContainers.Main:
                    ActiveContainer = MenuConsoleContainer;
                    break;
                case ConsoleContainers.Game:
                    ActiveContainer = GameConsoleContainer;
                    break;
                case ConsoleContainers.Message:
                    ActiveContainer = MessageConsoleContainer;
                    MessageConsoleContainer.ContainerToShiftTo = consoleToFollowUp.GetValueOrDefault();
                    MessageConsoleContainer.Title = title;
                    MessageConsoleContainer.Message = message;
                    break;
            }
            ActiveContainer.IsEnabled = true;
            ActiveContainer.IsVisible = true;
            ActiveContainer.IsFocused = true;
            ActiveContainer.Start();
        }
    }

    public enum ConsoleContainers
    {
        Main,
        Game,
        Message
    }
    #pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
}
