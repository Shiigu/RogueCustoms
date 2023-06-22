using SadConsole.UI.Controls;
using SadConsole.UI;
using SadConsole;
using SadRogue.Primitives;
using Themes = SadConsole.UI.Themes;
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

namespace RogueCustomsConsoleClient.UI.Consoles.GameConsole.GameWindows
{
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

        public static Window Show(GameConsoleContainer parent, List<ActionItemDto> items)
        {
            if (!items.Any()) return null;
            var width = 65;
            var height = 30;

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
            window.ActionItems = items;
            window.ActionSelectedIndex = 0;
            window.IsDirty = true;
            window.ParentConsole = parent;
            window.Font = Game.Instance.LoadFont("fonts/Cheepicus12.font");
            window.TitleCaption = LocalizationManager.GetString("ActionWindowTitleText");

            var drawingArea = new DrawingArea(window.Width, window.Height);
            drawingArea.OnDraw += window.DrawWindow;

            window.DrawingArea = drawingArea;
            window.Controls.Add(drawingArea);

            doButton.Position = new Point(2, window.Height - doButton.Surface.Height);
            doButton.Click += (o, e) => {
                var item = window.ActionItems.ElementAtOrDefault(window.ActionSelectedIndex);
                var attackInput = new AttackInput
                {
                    Name = item.Name,
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
            doButton.Theme = null;

            cancelButton.Position = new Point((window.Width - cancelButton.Surface.Width) - 2, window.Height - cancelButton.Surface.Height);
            cancelButton.Click += (o, e) => { parent.ControlMode = ControlMode.NormalMove; window.Hide(); };
            cancelButton.Theme = null;

            window.DoButton = doButton;
            window.Controls.Add(doButton);
            window.CancelButton = cancelButton;
            window.Controls.Add(cancelButton);

            window.Show(true);
            window.Parent = (window.Parent as RootScreen).ActiveContainer;
            window.Center();

            return window;
        }

        public void DrawWindow(DrawingArea ds, TimeSpan delta)
        {
            if (!ds.IsDirty) return;

            var window = (ds.Parent as ControlHost).ParentConsole as Console;

            var square = new Rectangle(0, 0, Width, Height);

            ds.Surface.Clear();
            ColoredGlyph appearance = ((Themes.DrawingAreaTheme)ds.Theme).Appearance;
            ds.Surface.Fill(appearance.Foreground, appearance.Background, null);
            ds.Surface.DrawLine(new Point(0, 0), new Point(0, Height - 3), ICellSurface.ConnectedLineThick[3], Color.DarkRed);
            ds.Surface.DrawLine(new Point(0, 0), new Point(Width - 1, 0), ICellSurface.ConnectedLineThick[3], Color.DarkRed);
            ds.Surface.DrawLine(new Point(32, 0), new Point(32, Height - 3), ICellSurface.ConnectedLineThick[3], Color.DarkRed);
            ds.Surface.DrawLine(new Point(Width - 1, 0), new Point(Width - 1, Height - 3), ICellSurface.ConnectedLineThick[3], Color.DarkRed);
            ds.Surface.DrawLine(new Point(0, window.Height - 3), new Point(Width - 1, Height - 3), ICellSurface.ConnectedLineThick[3], Color.DarkRed);
            ds.Surface.ConnectLines(ICellSurface.ConnectedLineThick);
            ds.Surface.Print((Width - TitleCaption.Length - 2) / 2, 0, $" {TitleCaption.ToAscii()} ", Color.White, Color.DarkRed);

            var action = ActionItems.ElementAtOrDefault(ActionSelectedIndex);
            var initialIndexToShow = CurrentlyShownActionItems != null && CurrentlyShownActionItems.Contains(action) ? CurrentlyShownFirstIndex : Math.Min(CurrentlyShownFirstIndex + 1, ActionItems.IndexOf(action));
            CurrentlyShownFirstIndex = initialIndexToShow;
            var actionItemsToShow = CurrentlyShownActionItems != null && CurrentlyShownActionItems.Contains(action) ? CurrentlyShownActionItems : ActionItems.Skip(initialIndexToShow).Take(24).ToList();
            CurrentlyShownActionItems = actionItemsToShow;

            for (int i = 0; i < actionItemsToShow.Count; i++)
            {
                var actionName = ActionItems[initialIndexToShow + i].Name.PadRight(29);
                if (ActionItems[initialIndexToShow + i] == action)
                    ds.Surface.Print(2, 2 + i, actionName.ToAscii(), Color.Black, Color.White);
                else
                    ds.Surface.Print(2, 2 + i, actionName.ToAscii(), Color.White, Color.Black);
            }

            DoButton.IsEnabled = action.CanBeUsed;

            if (action != null)
            {
                DoButton.IsEnabled = action.CanBeUsed;

                ds.Surface.Print(34, 2, action.Name.ToAscii(), Color.White, Color.Black);

                var descriptionAsString = action.Description.Wrap(30);
                var linesInDescription = action.Description.Split(
                    new[] { "\r\n", "\n" }, StringSplitOptions.None
                    );
                var splitWrappedDescription = new List<string>();
                foreach (var line in linesInDescription)
                {
                    if (line.Trim().Length < 20)
                        splitWrappedDescription.Add(line);
                    else
                        splitWrappedDescription.AddRange((from Match m in Regex.Matches(line, @"[(]?\b(.{1,29}\s*\b)[.]?[)]?") select m.Value).ToList());
                }

                for (int i = 0; i < splitWrappedDescription.Count; i++)
                {
                    ds.Surface.Print(34, 4 + i, splitWrappedDescription[i].Trim().ToAscii(), Color.White, Color.Black);
                }
            }
            ds.IsDirty = true;
            ds.IsFocused = true;
        }

        public override bool ProcessKeyboard(Keyboard info)
        {
            var action = ActionItems.ElementAtOrDefault(ActionSelectedIndex);

            if (info.IsKeyPressed(Keys.Up))
            {
                ActionSelectedIndex = Math.Max(0, ActionSelectedIndex - 1);
                DrawingArea.IsDirty = true;
            }
            else if (info.IsKeyPressed(Keys.Down))
            {
                ActionSelectedIndex = Math.Min(ActionItems.Count - 1, ActionSelectedIndex + 1);
                DrawingArea.IsDirty = true;
            }
            else if ((info.IsKeyPressed(Keys.A) || info.IsKeyPressed(Keys.D) || info.IsKeyPressed(Keys.Enter)) && action.CanBeUsed)
            {
                DoButton.InvokeClick();
            }
            else if (info.IsKeyPressed(Keys.C) || info.IsKeyPressed(Keys.Escape))
            {
                CancelButton.InvokeClick();
            }

            return true;
        }
    }
}
