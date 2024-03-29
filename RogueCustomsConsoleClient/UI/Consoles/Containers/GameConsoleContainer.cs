﻿using SadConsole;
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
using SadConsole.Effects;
using RogueCustomsGameEngine.Game.Entities;
using MathNet.Numerics.Distributions;

namespace RogueCustomsConsoleClient.UI.Consoles.Containers
{
    public class GameConsoleContainer : ConsoleContainer
    {
        new private readonly List<GameSubConsole> Consoles;

        public bool RequiresRefreshingDungeonState { get; set; }
        private readonly BicolorBlink DamageFlash, StatusChangeFlash, HealFlash, MPBurnFlash, MPReplenishFlash;

        public bool HasSetupPlayerData { get; set; }
        public bool Flashing { get; set; }
        public DungeonDto? LatestDungeonStatus { get; set; }
        public ControlMode ControlMode { get; set; }

        public (int X, int Y) CursorLocation => DungeonConsole.CursorLocation;

        private readonly DungeonConsole DungeonConsole;
        private readonly MessageLogConsole MessageLogConsole;
        private readonly PlayerInfoConsole PlayerInfoConsole;
        private readonly ButtonsConsole ButtonsConsole;
        private readonly ExperienceBarConsole ExperienceBarConsole;
        private readonly GameControlsConsole GameControlsConsole;

        private readonly TimeSpan FlashDuration = TimeSpan.FromSeconds(1 / 24d);
        public Window? ActiveWindow { get; set; }

        public GameConsoleContainer(RootScreen parent) : base(parent, Game.Instance.ScreenCellsX, Game.Instance.ScreenCellsY)
        {
            RequiresRefreshingDungeonState = false;
            DamageFlash = new BicolorBlink() { BlinkCount = 1, BlinkSpeed = FlashDuration, BlinkOutForegroundColor = Color.Red, BlinkOutBackgroundColor = Color.Red, RemoveOnFinished = true };
            StatusChangeFlash = new BicolorBlink() { BlinkCount = 1, BlinkSpeed = FlashDuration, BlinkOutForegroundColor = Color.Violet, BlinkOutBackgroundColor = Color.Violet, RemoveOnFinished = true };
            HealFlash = new BicolorBlink() { BlinkCount = 1, BlinkSpeed = FlashDuration, BlinkOutForegroundColor = Color.Green, BlinkOutBackgroundColor = Color.Green, RemoveOnFinished = true };
            MPBurnFlash = new BicolorBlink() { BlinkCount = 1, BlinkSpeed = FlashDuration, BlinkOutForegroundColor = Color.DarkBlue, BlinkOutBackgroundColor = Color.DarkViolet, RemoveOnFinished = true };
            MPReplenishFlash = new BicolorBlink() { BlinkCount = 1, BlinkSpeed = FlashDuration, BlinkOutForegroundColor = Color.Blue, BlinkOutBackgroundColor = Color.BlueViolet, RemoveOnFinished = true };

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
            DungeonConsole.IsVisible = false;
            DungeonConsole.IsEnabled = false;
            MessageLogConsole.Build();
            MessageLogConsole.IsVisible = false;
            MessageLogConsole.IsEnabled = false;
            PlayerInfoConsole.Build();
            PlayerInfoConsole.IsVisible = false;
            PlayerInfoConsole.IsEnabled = false;
            ButtonsConsole.Build();
            ButtonsConsole.IsVisible = false;
            ButtonsConsole.IsEnabled = false;
            ExperienceBarConsole.Build();
            ExperienceBarConsole.IsVisible = false;
            ExperienceBarConsole.IsEnabled = false;
            GameControlsConsole.Build();
            GameControlsConsole.IsVisible = false;
            GameControlsConsole.IsEnabled = false;
            ControlMode = ControlMode.NormalMove;
            RequiresRefreshingDungeonState = true;
            if (BackendHandler.Instance.IsLoadedSaveGame)
            {
                HasSetupPlayerData = true;
                ChangeSubConsolesRenderState(true);
            }
            else
            {
                HasSetupPlayerData = false;
            }
            ActiveWindow = null;
        }

        public override void Update(TimeSpan delta)
        {
            IsFocused = !Children.Any(c => c is Window);

            if (HasSetupPlayerData)
            {
                InGameUpdate(delta);
                if (ActiveWindow is PlayerClassWindow)
                {
                    ActiveWindow.Hide();
                    ActiveWindow = null;
                }
            }
            else if (ActiveWindow?.IsVisible != true)
            {
                var classSelectionList = BackendHandler.Instance.GetPlayerClassSelection();
                if (classSelectionList.CharacterClasses.Count == 1)
                {
                    if (classSelectionList.CharacterClasses[0].RequiresNamePrompt)
                    {
                        ActiveWindow = InputBox.Show(new ColoredString(LocalizationManager.GetString("InputBoxSingleClassPromptText").Format(new { ClassName = classSelectionList.CharacterClasses[0].Name })), LocalizationManager.GetString("InputBoxAffirmativeButtonText"), LocalizationManager.GetString("NoButtonText"), LocalizationManager.GetString("InputBoxTitleText"), classSelectionList.CharacterClasses[0].Name, Color.Orange,
                                                    (name) =>
                                                    {
                                                        SendClassSelection(new PlayerClassSelectionInput
                                                        {
                                                            ClassId = classSelectionList.CharacterClasses[0].ClassId,
                                                            Name = name
                                                        });
                                                    }, () => ChangeConsoleContainerTo(ConsoleContainers.Main));
                    }
                    else
                    {
                        SendClassSelection(new PlayerClassSelectionInput
                        {
                            ClassId = classSelectionList.CharacterClasses[0].ClassId,
                            Name = classSelectionList.CharacterClasses[0].Name
                        });
                    }
                }
                else
                {
                    ActiveWindow = PlayerClassWindow.Show(this, classSelectionList);
                }
            }
            else
            {
                base.Update(delta);
            }
        }

        private void InGameUpdate(TimeSpan delta)
        {
            if (ControlMode == ControlMode.NormalMove && DungeonConsole.WithCursor)
                DungeonConsole.RemoveCursor();

            if (!this.IsEnabled) return;

            try
            {
                if (RequiresRefreshingDungeonState)
                {
                    LatestDungeonStatus = BackendHandler.Instance.GetDungeonStatus();
                    ShowMessagesIfNeeded();
                }

                ButtonsConsole.SaveButton.IsEnabled = LatestDungeonStatus.DungeonStatus == DungeonStatus.Running;

                if (LatestDungeonStatus.DungeonStatus == DungeonStatus.GameOver)
                    ControlMode = ControlMode.None;

                if (ControlMode != ControlMode.ActionTargeting && ControlMode != ControlMode.ViewTargeting && ControlMode != ControlMode.None)
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
                if (!console.IsVisible) return;
                if (LatestDungeonStatus != null && (RequiresRefreshingDungeonState || !console.RefreshOnlyOnStatusUpdate))
                    console.Update(delta);
            });

            Children.ToList().ForEach(c =>
            {
                if (!c.IsVisible) return;
                if (!Consoles.Contains(c))
                    c.Update(delta);
            });

            if (!Flashing && LatestDungeonStatus.PlayerTookDamage)
            {
                ApplyFlashEffect(DamageFlash);
            }
            if (!Flashing && LatestDungeonStatus.PlayerGotStatusChange)
            {
                ApplyFlashEffect(StatusChangeFlash);
            }
            if (!Flashing && LatestDungeonStatus.PlayerGotHealed)
            {
                ApplyFlashEffect(HealFlash);
            }
            if (!Flashing && LatestDungeonStatus.PlayerGotMPBurned)
            {
                ApplyFlashEffect(MPBurnFlash);
            }
            if (!Flashing && LatestDungeonStatus.PlayerGotMPReplenished)
            {
                ApplyFlashEffect(MPReplenishFlash);
            }

            if (Flashing)
            {
                if (DamageFlash.IsFinished && LatestDungeonStatus.PlayerTookDamage)
                {
                    RemoveFlashEffect();
                }
                else if (StatusChangeFlash.IsFinished && LatestDungeonStatus.PlayerGotStatusChange)
                {
                    RemoveFlashEffect();
                }
                else if (HealFlash.IsFinished && LatestDungeonStatus.PlayerGotHealed)
                {
                    RemoveFlashEffect();
                }
                else if (MPBurnFlash.IsFinished && LatestDungeonStatus.PlayerGotMPBurned)
                {
                    RemoveFlashEffect();
                }
                else if (MPReplenishFlash.IsFinished && LatestDungeonStatus.PlayerGotMPReplenished)
                {
                    RemoveFlashEffect();
                }
                if (!LatestDungeonStatus.PlayerTookDamage && !LatestDungeonStatus.PlayerGotHealed
                 && !LatestDungeonStatus.PlayerGotMPBurned && !LatestDungeonStatus.PlayerGotMPReplenished
                  && !LatestDungeonStatus.PlayerGotStatusChange)
                {
                    Flashing = false;
                    ChangeSubConsolesRenderState(true);
                }
            }

            Surface.Effects.UpdateEffects(delta);
            RequiresRefreshingDungeonState = false;
        }

        public void ChangeSubConsolesRenderState(bool state)
        {
            RequiresRefreshingDungeonState = state;
            DungeonConsole.IsVisible = state;
            DungeonConsole.IsEnabled = state;
            MessageLogConsole.IsVisible = state;
            MessageLogConsole.IsEnabled = state;
            PlayerInfoConsole.IsVisible = state;
            PlayerInfoConsole.IsEnabled = state;
            ButtonsConsole.IsVisible = state;
            ButtonsConsole.IsEnabled = state;
            ExperienceBarConsole.IsVisible = state;
            ExperienceBarConsole.IsEnabled = state;
            GameControlsConsole.IsVisible = state;
            GameControlsConsole.IsEnabled = state;
        }

        private void RemoveFlashEffect()
        {
            Surface.Effects.RemoveAll();
            LatestDungeonStatus.PlayerTookDamage = false;
            LatestDungeonStatus.PlayerGotHealed = false;
            LatestDungeonStatus.PlayerGotMPBurned = false;
            LatestDungeonStatus.PlayerGotMPReplenished = false;
            LatestDungeonStatus.PlayerGotStatusChange = false;
        }

        private void ApplyFlashEffect(ICellEffect effect)
        {
            Flashing = true;
            ChangeSubConsolesRenderState(false);
            this.SetEffect(this.GetCells(new Rectangle(0, 0, Width, Height)), effect);
            effect.Restart();
        }

        private void SendClassSelection(PlayerClassSelectionInput selectionInput)
        {
            try
            {
                BackendHandler.Instance.SetPlayerClassSelection(selectionInput);
                ControlMode = ControlMode.NormalMove;
                HasSetupPlayerData = true;
                ChangeSubConsolesRenderState(true);
            }
            catch (Exception)
            {
                ChangeConsoleContainerTo(ConsoleContainers.Message, ConsoleContainers.Main, LocalizationManager.GetString("ErrorMessageHeader"), LocalizationManager.GetString("ErrorText"));
            }
        }

        private void ShowMessagesIfNeeded(int index = 0)
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
                if(this != (Parent as RootScreen)?.ActiveContainer)
                    return (Parent as RootScreen)?.ActiveContainer.ProcessKeyboard(keyboard) ?? false;
                if (LatestDungeonStatus == null) return true;
                if (ControlMode == ControlMode.NormalMove || ControlMode == ControlMode.OnStairs || ControlMode == ControlMode.Immobilized || ControlMode == ControlMode.CannotAct)
                    return ProcessMoveModeKeyboard(keyboard);
                if (ControlMode == ControlMode.ActionTargeting || ControlMode == ControlMode.ViewTargeting)
                    return ProcessTargetingModeKeyboard(keyboard);
                if (ControlMode == ControlMode.None)
                    return ProcessNoneModeKeyboard(keyboard);
            }
            catch
            {
                LatestDungeonStatus = null;
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
                if (keyboard.IsKeyPressed(Keys.Up) && !keyboard.IsKeyPressed(Keys.Down) && keyboard.KeysPressed.Count <= 2)
                {
                    input.Y = -1;
                    handled = true;
                }
                else if (keyboard.IsKeyPressed(Keys.Down) && !keyboard.IsKeyPressed(Keys.Up) && keyboard.KeysPressed.Count <= 2)
                {
                    input.Y = 1;
                    handled = true;
                }

                if (keyboard.IsKeyPressed(Keys.Left) && !keyboard.IsKeyPressed(Keys.Right) && keyboard.KeysPressed.Count <= 2)
                {
                    input.X = -1;
                    handled = true;
                }
                else if (keyboard.IsKeyPressed(Keys.Right) && !keyboard.IsKeyPressed(Keys.Left) && keyboard.KeysPressed.Count <= 2)
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

                if (keyboard.IsKeyPressed(Keys.U) && ControlMode == ControlMode.OnStairs && keyboard.KeysPressed.Count == 1)
                {
                    ActiveWindow = PromptBox.Show(new ColoredString(LocalizationManager.GetString("StairsPromptText")), LocalizationManager.GetString("YesButtonText"), LocalizationManager.GetString("NoButtonText"), LatestDungeonStatus.DungeonName, Color.Green,
                                                    () =>
                                                    {
                                                        BackendHandler.Instance.PlayerTakeStairs();
                                                        RequiresRefreshingDungeonState = true;
                                                    });
                    handled = true;
                }
            }

            if(ControlMode != ControlMode.CannotAct)
            {
                if (keyboard.IsKeyPressed(Keys.A) && keyboard.KeysPressed.Count == 1)
                {
                    ControlMode = ControlMode.ActionTargeting;
                    DungeonConsole.AddCursor();
                    handled = true;
                }

                if (keyboard.IsKeyPressed(Keys.V) && keyboard.KeysPressed.Count == 1)
                {
                    ControlMode = ControlMode.ViewTargeting;
                    DungeonConsole.AddCursor();
                    handled = true;
                }
            }

            if (keyboard.IsKeyPressed(Keys.S) && keyboard.KeysPressed.Count == 1)
            {
                BackendHandler.Instance.PlayerSkipTurn();
                RequiresRefreshingDungeonState = true;
                handled = true;
            }

            if (keyboard.IsKeyPressed(Keys.I) && keyboard.KeysPressed.Count == 1)
            {
                var inventory = BackendHandler.Instance.GetPlayerInventory();
                if (inventory?.InventoryItems.Any() == true)
                    ActiveWindow = InventoryWindow.Show(this, inventory, !LatestDungeonStatus.PlayerEntity.CanTakeAction);
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.M) && keyboard.KeysPressed.Count == 1)
            {
                MessageLogConsole.MessageLogWindowButton.InvokeClick();
                handled = true;
            }

            if (keyboard.IsKeyPressed(Keys.D) && keyboard.KeysPressed.Count == 1)
            {
                PlayerInfoConsole.DetailsButton.InvokeClick();
                handled = true;
            }

            if (keyboard.IsKeyPressed(Keys.F5) && keyboard.KeysPressed.Count == 1)
            {
                ButtonsConsole.SaveButton.InvokeClick();
                handled = true;
            }

            if (keyboard.IsKeyPressed(Keys.Escape) && keyboard.KeysPressed.Count == 1)
            {
                ButtonsConsole.ExitButton.InvokeClick();
                handled = true;
            }

            return handled;
        }
        private bool ProcessNoneModeKeyboard(Keyboard keyboard)
        {
            bool handled = false;

            if (keyboard.IsKeyPressed(Keys.M) && keyboard.KeysPressed.Count == 1)
            {
                MessageLogConsole.MessageLogWindowButton.InvokeClick();
                handled = true;
            }

            if (keyboard.IsKeyPressed(Keys.D) && keyboard.KeysPressed.Count == 1)
            {
                PlayerInfoConsole.DetailsButton.InvokeClick();
                handled = true;
            }

            if (keyboard.IsKeyPressed(Keys.Escape) && keyboard.KeysPressed.Count == 1)
            {
                ButtonsConsole.ExitButton.InvokeClick();
                handled = true;
            }

            return handled;
        }

        private bool ProcessTargetingModeKeyboard(Keyboard keyboard)
        {
            bool handled = false;
            var input = new CoordinateInput
            {
                X = 0,
                Y = 0
            };

            if (keyboard.IsKeyPressed(Keys.Up) && !keyboard.IsKeyPressed(Keys.Down) && keyboard.KeysPressed.Count <= 2)
            {
                input.Y = -1;
                handled = true;
            }
            else if (keyboard.IsKeyPressed(Keys.Down) && !keyboard.IsKeyPressed(Keys.Up) && keyboard.KeysPressed.Count <= 2)
            {
                input.Y = 1;
                handled = true;
            }

            if (keyboard.IsKeyPressed(Keys.Left) && !keyboard.IsKeyPressed(Keys.Right) && keyboard.KeysPressed.Count <= 2)
            {
                input.X = -1;
                handled = true;
            }
            else if (keyboard.IsKeyPressed(Keys.Right) && !keyboard.IsKeyPressed(Keys.Left) && keyboard.KeysPressed.Count <= 2)
            {
                input.X = 1;
                handled = true;
            }

            if (input.X != 0 || input.Y != 0)
            {
                DungeonConsole.MoveCursor(input);
                return handled;
            }

            if(ControlMode == ControlMode.ActionTargeting)
            {
                if ((keyboard.IsKeyPressed(Keys.A) || keyboard.IsKeyPressed(Keys.Enter)) && keyboard.KeysPressed.Count == 1)
                {
                    var actionList = BackendHandler.Instance.GetPlayerAttackActions(CursorLocation.X, CursorLocation.Y);
                    if (actionList.Actions.Any())
                        ActiveWindow = ActionWindow.Show(this, actionList);
                    return handled;
                }
            }
            else if (ControlMode == ControlMode.ViewTargeting && (keyboard.IsKeyPressed(Keys.V) || keyboard.IsKeyPressed(Keys.Enter)) && keyboard.KeysPressed.Count == 1)
            {
                var entityDetails = BackendHandler.Instance.GetDetailsOfEntity(CursorLocation.X, CursorLocation.Y);
                if (entityDetails != null)
                    ActiveWindow = EntityDetailWindow.Show(entityDetails);
                return handled;
            }

            if (keyboard.IsKeyPressed(Keys.Escape) && keyboard.KeysPressed.Count == 1)
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
        ViewTargeting,
        None
    }
}
