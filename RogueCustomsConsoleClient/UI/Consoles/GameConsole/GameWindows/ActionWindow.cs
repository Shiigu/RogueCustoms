using SadConsole.UI.Controls;
using SadConsole.UI;
using SadConsole;
using SadRogue.Primitives;
using Window = SadConsole.UI.Window;
using SadConsole.Input;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsConsoleClient.Helpers;
using System.Text.RegularExpressions;
using Keyboard = SadConsole.Input.Keyboard;
using Console = SadConsole.Console;
using RogueCustomsConsoleClient.UI.Consoles.Containers;
using RogueCustomsConsoleClient.EngineHandling;
using RogueCustomsConsoleClient.Resources.Localization;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Immutable;

namespace RogueCustomsConsoleClient.UI.Consoles.GameConsole.GameWindows
{
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning disable CS8604 // Posible argumento de referencia nulo
    public class ActionWindow : Window
    {
        private readonly string DoButtonText = LocalizationManager.GetString("DoButtonText").ToAscii();
        private readonly string CancelButtonText = LocalizationManager.GetString("CancelButtonText").ToAscii();
        private Button DoButton, CancelButton;
        private string TitleCaption { get; set; }
        private List<ActionItemDto> ActionItems;
        private List<ActionItemDto> CurrentlyShownActionItems;
        private int CurrentlyShownFirstIndex;
        private int ActionSelectedIndex;
        private DrawingArea DrawingArea;
        private GameConsoleContainer ParentConsole;

        private ActionWindow(int width, int height) : base(width, height)
        {
        }

        public static Window? Show(GameConsoleContainer parent, ActionListDto actionListData)
        {
            if (!actionListData.Actions.Any()) return null;
            const int width = GameConsoleConstants.SelectionWindowWidth;
            const int height = GameConsoleConstants.SelectionWindowHeight;

            var window = new ActionWindow(width, height);

            var doButton = new Button(window.DoButtonText.Length + 4, 1)
            {
                Text = window.DoButtonText,
            };

            var cancelButton = new Button(window.CancelButtonText.Length + 2, 1)
            {
                Text = window.CancelButtonText,
            };

            window.UseKeyboard = true;
            window.IsFocused = true;
            window.ActionItems = actionListData.Actions;
            window.ActionSelectedIndex = 0;
            window.IsDirty = true;
            window.ParentConsole = parent;
            window.Font = Game.Instance.LoadFont("fonts/Alloy_curses_12x12.font");
            window.TitleCaption = LocalizationManager.GetString("ActionWindowTitleText").Format(new { TargetName = actionListData.TargetName });

            var drawingArea = new DrawingArea(window.Width, window.Height);
            drawingArea.OnDraw += window.DrawWindow;

            window.DrawingArea = drawingArea;
            window.Controls.Add(drawingArea);

            doButton.Position = new Point(2, window.Height - doButton.Surface.Height);
            doButton.Click += (o, e) => {
                var item = window.ActionItems.ElementAtOrDefault(window.ActionSelectedIndex);
                var attackInput = new AttackInput
                {
                    SelectionId = item.SelectionId,
                    X = window.ParentConsole.CursorLocation.X,
                    Y = window.ParentConsole.CursorLocation.Y
                };
                try
                {
                    BackendHandler.Instance.PlayerAttackTargetWith(attackInput);
                    parent.ControlMode = ControlMode.NormalMove;
                    window.ParentConsole.RequiresRefreshingDungeonState = true;
                }
                catch (Exception)
                {
                    parent.ChangeConsoleContainerTo(ConsoleContainers.Message, ConsoleContainers.Main, LocalizationManager.GetString("ErrorMessageHeader"), LocalizationManager.GetString("ErrorText"));
                }
                finally
                {
                    window.Hide();
                }
            };

            cancelButton.Position = new Point(window.Width - cancelButton.Surface.Width - 2, window.Height - cancelButton.Surface.Height);
            cancelButton.Click += (o, e) => { parent.ControlMode = ControlMode.NormalMove; window.Hide(); };

            window.DoButton = doButton;
            window.Controls.Add(doButton);
            window.CancelButton = cancelButton;
            window.Controls.Add(cancelButton);

            window.Show(true);
            window.Parent = (window.Parent as RootScreen)?.ActiveContainer;
            window.Center();

            return window;
        }

        public void DrawWindow(DrawingArea ds, TimeSpan delta)
        {
            if (!ds.IsDirty) return;

            var window = (ds.Parent as ControlHost)?.ParentConsole as Console;

            ds.Surface.Clear();
            var appearance = ds.ThemeState.Normal;
            ds.Surface.Fill(appearance.Foreground, appearance.Background, null);
            ds.Surface.DrawLine(new Point(0, 0), new Point(0, Height - 3), ICellSurface.ConnectedLineThick[3], Color.DarkRed);
            ds.Surface.DrawLine(new Point(0, 0), new Point(Width - 1, 0), ICellSurface.ConnectedLineThick[3], Color.DarkRed);
            ds.Surface.DrawLine(new Point(39, 0), new Point(39, Height - 3), ICellSurface.ConnectedLineThick[3], Color.DarkRed);
            ds.Surface.DrawLine(new Point(Width - 1, 0), new Point(Width - 1, Height - 3), ICellSurface.ConnectedLineThick[3], Color.DarkRed);
            ds.Surface.DrawLine(new Point(0, window.Height - 3), new Point(Width - 1, Height - 3), ICellSurface.ConnectedLineThick[3], Color.DarkRed);
            ds.Surface.ConnectLines(ICellSurface.ConnectedLineThick);
            ds.Surface.Print((Width - TitleCaption.Length - 2) / 2, 0, $" {TitleCaption.ToAscii()} ", Color.White, Color.DarkRed);

            var action = ActionItems.ElementAtOrDefault(ActionSelectedIndex);
            var initialIndexToShow = CurrentlyShownActionItems?.Contains(action) == true ? CurrentlyShownFirstIndex : Math.Min(CurrentlyShownFirstIndex + 1, ActionItems.IndexOf(action));
            CurrentlyShownFirstIndex = initialIndexToShow;
            var actionItemsToShow = CurrentlyShownActionItems?.Contains(action) == true ? CurrentlyShownActionItems : ActionItems.Skip(initialIndexToShow).Take(30).ToList();
            CurrentlyShownActionItems = actionItemsToShow;

            for (int i = 0; i < actionItemsToShow.Count; i++)
            {
                var actionName = ActionItems[initialIndexToShow + i].Name.PadRight(36);
                if (ActionItems[initialIndexToShow + i] == action)
                    ds.Surface.Print(2, 2 + i, actionName.ToAscii(), Color.Black, Color.White);
                else
                    ds.Surface.Print(2, 2 + i, actionName.ToAscii(), Color.White, Color.Black);
            }

            DoButton.IsEnabled = action.CanBeUsed;

            DoButton.IsEnabled = action.CanBeUsed;

            ds.Surface.Print(41, 2, action.Name.ToAscii(), Color.White, Color.Black);

            var linesInDescription = action.Description.Split(
                new[] { "\r\n", "\n" }, StringSplitOptions.None
                );
            var splitWrappedDescription = linesInDescription.SplitByLengthWithWholeWords(GameConsoleConstants.DescriptionWindowMaxLength).ToList();

            for (int i = 0; i < splitWrappedDescription.Count; i++)
            {
                ds.Surface.Print(41, 4 + i, splitWrappedDescription[i].Trim().ToAscii(), Color.White, Color.Black);
            }
            ds.IsDirty = true;
            ds.IsFocused = true;
        }

        public override bool ProcessKeyboard(Keyboard info)
        {
            var action = ActionItems.ElementAtOrDefault(ActionSelectedIndex);

            if (info.IsKeyPressed(Keys.Up) && info.KeysPressed.Count == 1)
            {
                ActionSelectedIndex = Math.Max(0, ActionSelectedIndex - 1);
                DrawingArea.IsDirty = true;
            }
            else if (info.IsKeyPressed(Keys.Down) && info.KeysPressed.Count == 1)
            {
                ActionSelectedIndex = Math.Min(ActionItems.Count - 1, ActionSelectedIndex + 1);
                DrawingArea.IsDirty = true;
            }
            else if ((info.IsKeyPressed(Keys.A) || info.IsKeyPressed(Keys.D) || info.IsKeyPressed(Keys.Enter)) && action.CanBeUsed && info.KeysPressed.Count == 1)
            {
                DoButton.InvokeClick();
            }
            else if ((info.IsKeyPressed(Keys.C) || info.IsKeyPressed(Keys.Escape)) && info.KeysPressed.Count == 1)
            {
                CancelButton.InvokeClick();
            }

            return true;
        }
    }
#pragma warning restore CS8604 // Posible argumento de referencia nulo
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
