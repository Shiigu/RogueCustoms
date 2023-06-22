using SadConsole;
using SadConsole.Input;
using SadConsole.UI;
using RogueCustomsConsoleClient.Helpers;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsConsoleClient.UI.Windows;
using SadRogue.Primitives;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsConsoleClient.UI.Consoles.GameConsole;
using RogueCustomsConsoleClient.UI.Consoles.GameConsole.GameWindows;
using RogueCustomsConsoleClient.EngineHandling;
using RogueCustomsConsoleClient.Resources.Localization;
using System.Collections.Generic;
using System;
using System.Linq;

namespace RogueCustomsConsoleClient.UI.Consoles.Containers
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

            float adjustmentForDungeonConsoleFontWidth = (float)DungeonConsole.FontSize.X / FontSize.X;
            float adjustmentForDungeonConsoleFontHeight = (float)DungeonConsole.FontSize.Y / FontSize.Y;

            MessageLogConsole = new MessageLogConsole(this)
            {
                Position = new Point(1, (int) (DungeonConsole.Height * adjustmentForDungeonConsoleFontHeight) + 2)
            };
            MessageLogConsole.Clear();

            PlayerInfoConsole = new PlayerInfoConsole(this)
            {
                Position = new Point((int)(DungeonConsole.Width * adjustmentForDungeonConsoleFontWidth) + 2, 1)
            };
            PlayerInfoConsole.Clear();

            ButtonsConsole = new ButtonsConsole(this)
            {
                Position = new Point((int) (DungeonConsole.Width * adjustmentForDungeonConsoleFontWidth) + 2, PlayerInfoConsole.Height + 2)
            };
            ButtonsConsole.Clear();

            ExperienceBarConsole = new ExperienceBarConsole(this)
            {
                Position = new Point(1, (int)(DungeonConsole.Height * adjustmentForDungeonConsoleFontHeight) + MessageLogConsole.Height + 3)
            };
            ExperienceBarConsole.Clear();

            GameControlsConsole = new GameControlsConsole(this)
            {
                Position = new Point(1, (int)(DungeonConsole.Height * adjustmentForDungeonConsoleFontHeight) + MessageLogConsole.Height + 5)
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
            ControlMode = ControlMode.NormalMove;
            LastTurnCount = -1;
            RequiresRefreshingDungeonState = true;
        }

        public override void Update(TimeSpan delta)
        {
            IsFocused = !Children.Any(c => c is Window);

            if (ControlMode == ControlMode.NormalMove)
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

                if (LatestDungeonStatus.DungeonStatus == DungeonStatus.GameOver)
                    ControlMode = ControlMode.None;

                if(ControlMode != ControlMode.ActionTargeting && ControlMode != ControlMode.None)
                {
                    if (!LatestDungeonStatus.PlayerEntity.CanTakeAction)
                        ControlMode = ControlMode.CannotAct;
                    else if (LatestDungeonStatus.PlayerEntity.Movement == 0 && LatestDungeonStatus.PlayerEntity.CanTakeAction)
                        ControlMode = ControlMode.Immobilized;
                    else if (LatestDungeonStatus.IsPlayerOnStairs())
                        ControlMode = ControlMode.OnStairs;
                    else
                        ControlMode = ControlMode.NormalMove;
                }

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
                    console.Update(delta);
            });

            Children.ToList().ForEach(c =>
            {
                if (!Consoles.Contains(c))
                    c.Update(delta);
            });

            RequiresRefreshingDungeonState = false;
            base.Update(delta);
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
                if (LatestDungeonStatus == null) return true;
                if (ControlMode == ControlMode.NormalMove || ControlMode == ControlMode.OnStairs || ControlMode == ControlMode.Immobilized || ControlMode == ControlMode.CannotAct)
                    return ProcessMoveModeKeyboard(keyboard);
                if (ControlMode == ControlMode.ActionTargeting)
                    return ProcessActionTargetingModeKeyboard(keyboard);
                if (ControlMode == ControlMode.None)
                    return ProcessNoneModeKeyboard(keyboard);
            }
            catch (Exception)
            {
                ChangeConsoleContainerTo(ConsoleContainers.Message, ConsoleContainers.Main, LocalizationManager.GetString("ErrorMessageHeader"), LocalizationManager.GetString("ErrorText"));
            }
            return false;
        }

        private bool ProcessMoveModeKeyboard(Keyboard keyboard)
        {
            bool handled = false;
            var input = new CoordinateInput
            {
                X = 0,
                Y = 0
            };

            if(ControlMode == ControlMode.NormalMove || ControlMode == ControlMode.OnStairs)
            {
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

                if (keyboard.IsKeyPressed(Keys.U) && ControlMode == ControlMode.OnStairs)
                {
                    ActiveWindow = PromptBox.Show(new ColoredString(LocalizationManager.GetString("StairsPromptText")), LocalizationManager.GetString("YesButtonText"), LocalizationManager.GetString("NoButtonText"), LatestDungeonStatus.DungeonName, Color.Green,
                                                    () =>
                                                    {
                                                        BackendHandler.Instance.PlayerTakeStairs();
                                                        RequiresRefreshingDungeonState = true;
                                                    });
                    handled = true;
                }

                if (keyboard.IsKeyPressed(Keys.A))
                {
                    ControlMode = ControlMode.ActionTargeting;
                    DungeonConsole.AddCursor();
                    handled = true;
                }
            }


            if (keyboard.IsKeyPressed(Keys.S))
            {
                BackendHandler.Instance.PlayerSkipTurn();
                RequiresRefreshingDungeonState = true;
                handled = true;
            }

            if (keyboard.IsKeyPressed(Keys.I))
            {
                var inventory = BackendHandler.Instance.GetPlayerInventory();
                if (inventory != null && inventory.InventoryItems.Any())
                    ActiveWindow = InventoryWindow.Show(this, inventory, !LatestDungeonStatus.PlayerEntity.CanTakeAction);
                handled = true;
                return handled;
            }

            if (keyboard.IsKeyPressed(Keys.M))
            {
                MessageLogConsole.MessageLogWindowButton.InvokeClick();
                handled = true;
            }

            if (keyboard.IsKeyPressed(Keys.D))
            {
                PlayerInfoConsole.DetailsButton.InvokeClick();
                handled = true;
            }

            if (keyboard.IsKeyPressed(Keys.Escape))
            {
                ButtonsConsole.ExitButton.InvokeClick();
                handled = true;
            }

            return handled;
        }
        private bool ProcessNoneModeKeyboard(Keyboard keyboard)
        {
            bool handled = false;

            if (keyboard.IsKeyPressed(Keys.M))
            {
                MessageLogConsole.MessageLogWindowButton.InvokeClick();
                handled = true;
            }

            if (keyboard.IsKeyPressed(Keys.D))
            {
                PlayerInfoConsole.DetailsButton.InvokeClick();
                handled = true;
            }

            if (keyboard.IsKeyPressed(Keys.Escape))
            {
                ButtonsConsole.ExitButton.InvokeClick();
                handled = true;
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
                ControlMode = ControlMode.NormalMove;
                return handled;
            }

            return handled;

        }
    }

    public enum ControlMode
    {
        NormalMove,
        Immobilized,
        CannotAct,
        OnStairs,
        ActionTargeting,
        None
    }
}
