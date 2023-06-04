using SadConsole;
using SadConsole.Input;
using SadConsole.UI;
using RoguelikeConsoleClient.Helpers;
using RoguelikeGameEngine.Utils.InputsAndOutputs;
using RoguelikeConsoleClient.UI.Windows;
using SadRogue.Primitives;
using RoguelikeGameEngine.Utils.Enums;
using RoguelikeConsoleClient.UI.Consoles.GameConsole;
using RoguelikeConsoleClient.UI.Consoles.GameConsole.GameWindows;
using RoguelikeConsoleClient.EngineHandling;
using RoguelikeConsoleClient.Resources.Localization;
using System.Collections.Generic;
using System;
using System.Linq;

namespace RoguelikeConsoleClient.UI.Consoles.Containers
{
    public class GameConsoleContainer : ConsoleContainer
    {
        new private readonly List<GameSubConsole> Consoles;

        public bool RequiresRefreshingDungeonState;

        private int LastTurnCount;
        public DungeonDto? LatestDungeonStatus;
        public ControlMode ControlMode { get; set; }

        public (int X, int Y) CursorLocation => DungeonConsole.CursorLocation;

        private readonly DungeonConsole DungeonConsole;
        private readonly MessageLogConsole MessageLogConsole;
        private readonly PlayerInfoConsole PlayerInfoConsole;
        private readonly ButtonsConsole ButtonsConsole;
        private readonly ExperienceBarConsole ExperienceBarConsole;
        private readonly GameControlsConsole GameControlsConsole;
        public Window ActiveWindow;

        public GameConsoleContainer(RootScreen parent) : base(parent, Game.Instance.ScreenCellsX, Game.Instance.ScreenCellsY)
        {
            RequiresRefreshingDungeonState = false;

            DungeonConsole = new DungeonConsole(this)
            {
                Position = new Point(1, 1)
            };
            DungeonConsole.Clear();

            MessageLogConsole = new MessageLogConsole(this)
            {
                Position = new Point(1, DungeonConsole.Height + 2)
            };
            MessageLogConsole.Clear();

            PlayerInfoConsole = new PlayerInfoConsole(this)
            {
                Position = new Point(DungeonConsole.Width + 2, 1)
            };
            PlayerInfoConsole.Clear();

            ButtonsConsole = new ButtonsConsole(this)
            {
                Position = new Point(DungeonConsole.Width + 2, PlayerInfoConsole.Height + 2)
            };
            ButtonsConsole.Clear();

            ExperienceBarConsole = new ExperienceBarConsole(this)
            {
                Position = new Point(1, DungeonConsole.Height + MessageLogConsole.Height + 3)
            };
            ExperienceBarConsole.Clear();

            GameControlsConsole = new GameControlsConsole(this)
            {
                Position = new Point(1, DungeonConsole.Height + MessageLogConsole.Height + 5)
            };
            GameControlsConsole.Clear();

            Children.Add(DungeonConsole);
            Children.Add(MessageLogConsole);
            Children.Add(PlayerInfoConsole);
            Children.Add(ButtonsConsole);
            Children.Add(ExperienceBarConsole);
            Children.Add(GameControlsConsole);

            Consoles = new List<GameSubConsole>() { DungeonConsole,
                                                MessageLogConsole,
                                                PlayerInfoConsole,
                                                ButtonsConsole,
                                                ExperienceBarConsole,
                                                GameControlsConsole};
        }

        public override void Start()
        {
            DungeonConsole.Build();
            MessageLogConsole.Build();
            PlayerInfoConsole.Build();
            ButtonsConsole.Build();
            ExperienceBarConsole.Build();
            GameControlsConsole.Build();
            ControlMode = ControlMode.Move;
            LastTurnCount = -1;
            RequiresRefreshingDungeonState = true;
        }

        public override void Render(TimeSpan delta)
        {
            IsFocused = !Children.Any(c => c is Window);

            if (ControlMode == ControlMode.Move)
                DungeonConsole.RemoveCursor();

            try
            {

                if (RequiresRefreshingDungeonState)
                {
                    LatestDungeonStatus = BackendHandler.Instance.GetDungeonStatus();
                    if (LastTurnCount != LatestDungeonStatus.TurnCount)
                        ShowMessagesIfNeeded(0);
                    LastTurnCount = LatestDungeonStatus.TurnCount;
                }

                if (LatestDungeonStatus.DungeonStatus != DungeonStatus.Running)
                    ControlMode = ControlMode.None;

                if (LatestDungeonStatus.DungeonStatus == DungeonStatus.Completed)
                {
                    var message = BackendHandler.Instance.GetDungeonEndingMessage();
                    LatestDungeonStatus = null;
                    ChangeConsoleContainerTo(ConsoleContainers.Message, ConsoleContainers.Main, LocalizationManager.GetString("TheEndMessageHeader"), message);
                    base.Render(delta);
                    return;
                }
            }
            catch (Exception)
            {
                ChangeConsoleContainerTo(ConsoleContainers.Message, ConsoleContainers.Main, LocalizationManager.GetString("ErrorMessageHeader"), LocalizationManager.GetString("ErrorText"));
            }

            Consoles.ForEach(console =>
            {
                if (RequiresRefreshingDungeonState || !console.RefreshOnlyOnStatusUpdate)
                    console.Render(delta);
            });

            Children.ToList().ForEach(c =>
            {
                if (!Consoles.Contains(c))
                    c.Render(delta);
            });

            RequiresRefreshingDungeonState = false;
            base.Render(delta);
        }

        private void ShowMessagesIfNeeded(int index)
        {
            var messageBox = LatestDungeonStatus.MessageBoxes.ElementAtOrDefault(index);
            if (messageBox != null)
            {
                ActiveWindow = MessageBox.Show(new ColoredString(messageBox.Message), messageBox.ButtonCaption, messageBox.Title, messageBox.WindowColor.ToSadRogueColor(), () => ShowMessagesIfNeeded(index + 1));
            }
        }

        public override bool ProcessKeyboard(Keyboard keyboard)
        {
            try
            {
                if (!LatestDungeonStatus.IsAlive) return true;
                if (ControlMode == ControlMode.Move)
                    return ProcessMoveModeKeyboard(keyboard);
                if (ControlMode == ControlMode.ActionTargeting)
                    return ProcessActionTargetingModeKeyboard(keyboard);
            }
            catch (Exception)
            {
                ChangeConsoleContainerTo(ConsoleContainers.Message, ConsoleContainers.Main, LocalizationManager.GetString("ErrorMessageHeader"), LocalizationManager.GetString("ErrorText"));
            }
            return true;
        }

        private bool ProcessMoveModeKeyboard(Keyboard keyboard)
        {
            bool handled = false;
            var input = new CoordinateInput
            {
                X = 0,
                Y = 0
            };

            if (keyboard.IsKeyPressed(Keys.Up) && !keyboard.IsKeyPressed(Keys.Down))
            {
                input.Y = -1;
                handled = true;
            }
            else if (keyboard.IsKeyPressed(Keys.Down) && !keyboard.IsKeyPressed(Keys.Up))
            {
                input.Y = 1;
                handled = true;
            }

            if (keyboard.IsKeyPressed(Keys.Left) && !keyboard.IsKeyPressed(Keys.Right))
            {

                input.X = -1;
                handled = true;
            }
            else if (keyboard.IsKeyPressed(Keys.Right) && !keyboard.IsKeyPressed(Keys.Left))
            {
                input.X = 1;
                handled = true;
            }

            if (input.X != 0 || input.Y != 0)
            {
                BackendHandler.Instance.MovePlayer(input);
                RequiresRefreshingDungeonState = true;
                return handled;
            }

            if (keyboard.IsKeyPressed(Keys.U) && LatestDungeonStatus.IsPlayerOnStairs())
            {
                ActiveWindow = PromptBox.Show(new ColoredString(LocalizationManager.GetString("StairsPromptText")), LocalizationManager.GetString("YesButtonText"), LocalizationManager.GetString("NoButtonText"), LatestDungeonStatus.Name, Color.Green,
                                                () =>
                                                {
                                                    BackendHandler.Instance.PlayerTakeStairs();
                                                    RequiresRefreshingDungeonState = true;
                                                });
                return handled;
            }

            if (keyboard.IsKeyPressed(Keys.A))
            {
                ControlMode = ControlMode.ActionTargeting;
                DungeonConsole.AddCursor();
                return handled;
            }

            if (keyboard.IsKeyPressed(Keys.S))
            {
                BackendHandler.Instance.PlayerSkipTurn();
                RequiresRefreshingDungeonState = true;
                return handled;
            }

            if (keyboard.IsKeyPressed(Keys.I))
            {
                var inventory = BackendHandler.Instance.GetPlayerInventory();
                if (inventory != null && inventory.InventoryItems.Any())
                    ActiveWindow = InventoryWindow.Show(this, inventory);
                return handled;
            }

            return handled;
        }

        private bool ProcessActionTargetingModeKeyboard(Keyboard keyboard)
        {
            bool handled = false;
            var input = new CoordinateInput
            {
                X = 0,
                Y = 0
            };

            if (keyboard.IsKeyPressed(Keys.Up) && !keyboard.IsKeyPressed(Keys.Down))
            {
                input.Y = -1;
                handled = true;
            }
            else if (keyboard.IsKeyPressed(Keys.Down) && !keyboard.IsKeyPressed(Keys.Up))
            {
                input.Y = 1;
                handled = true;
            }

            if (keyboard.IsKeyPressed(Keys.Left) && !keyboard.IsKeyPressed(Keys.Right))
            {
                input.X = -1;
                handled = true;
            }
            else if (keyboard.IsKeyPressed(Keys.Right) && !keyboard.IsKeyPressed(Keys.Left))
            {
                input.X = 1;
                handled = true;
            }

            if (input.X != 0 || input.Y != 0)
            {
                DungeonConsole.MoveCursor(input);
                return handled;
            }

            if (keyboard.IsKeyPressed(Keys.A) || keyboard.IsKeyPressed(Keys.Enter))
            {
                var actionList = BackendHandler.Instance.GetPlayerAttackActions(CursorLocation.X, CursorLocation.Y);
                if (actionList.Any())
                    ActiveWindow = ActionWindow.Show(this, actionList);
                return handled;
            }

            if (keyboard.IsKeyPressed(Keys.Escape))
            {
                ControlMode = ControlMode.Move;
                return handled;
            }

            return handled;

        }
    }

    public enum ControlMode
    {
        Move,
        ActionTargeting,
        None
    }
}
