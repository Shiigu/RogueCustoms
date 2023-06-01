using SadConsole.UI.Controls;
using SadConsole.UI;
using SadConsole;
using SadRogue.Primitives;
using Themes = SadConsole.UI.Themes;
using Window = SadConsole.UI.Window;
using SadConsole.Input;
using RoguelikeGameEngine.Utils.InputsAndOutputs;
using RoguelikeConsoleClient.Helpers;
using System.Text.RegularExpressions;
using Keyboard = SadConsole.Input.Keyboard;
using Console = SadConsole.Console;
using RoguelikeConsoleClient.UI.Consoles.Containers;
using RoguelikeConsoleClient.EngineHandling;

namespace RoguelikeConsoleClient.UI.Consoles.GameConsole.GameWindows
{
    public class InventoryWindow : Window
    {
        private Button UseButton, DropOrSwapButton, CancelButton;
        private string TitleCaption { get; set; }
        private InventoryDto Inventory;
        private List<InventoryItemDto> CurrentlyShownInventoryItems;
        private int CurrentlyShownFirstIndex;
        private int InventorySelectedIndex;
        private DrawingArea DrawingArea;
        private bool TileIsOccupied, ItemIsEquippable;
        private GameConsoleContainer ParentConsole;

        private InventoryWindow(int width, int height) : base(width, height)
        {
        }

        public static Window Show(GameConsoleContainer parent, InventoryDto inventory)
        {
            if (inventory == null || !inventory.InventoryItems.Any()) return null;
            var width = 65;
            var height = 30;

            var useButtonText = "USE";
            var useButton = new Button(useButtonText.Length + 4, 1)
            {
                Text = useButtonText,
            };

            var dropButtonText = "DROP";
            var dropOrSwapButton = new Button(dropButtonText.Length + 2, 1)
            {
                Text = dropButtonText,
            };

            var cancelButtonText = "CANCEL";
            var cancelButton = new Button(cancelButtonText.Length + 2, 1)
            {
                Text = cancelButtonText,
            };

            var window = new InventoryWindow(width, height);

            window.UseKeyboard = true;
            window.IsFocused = true;
            window.Inventory = inventory;
            window.InventorySelectedIndex = 0;
            window.IsDirty = true;
            window.ParentConsole = parent;
            window.TitleCaption = "INVENTORY";

            var drawingArea = new DrawingArea(window.Width, window.Height);
            drawingArea.OnDraw += window.DrawWindow;

            window.DrawingArea = drawingArea;
            window.Controls.Add(drawingArea);

            useButton.Position = new Point(2, window.Height - useButton.Surface.Height);
            useButton.Click += (o, e) => {
                try
                {
                    BackendHandler.Instance.PlayerUseItemFromInventory(window.Inventory.InventoryItems[window.InventorySelectedIndex].ItemId);
                    window.ParentConsole.RequiresRefreshingDungeonState = true;
                }
                catch (Exception)
                {
                    parent.ChangeConsoleContainerTo(ConsoleContainers.Message, ConsoleContainers.Main, "ERROR", "OH NO!\nAn error has occured!\nGet ready to return to the main menu...");
                }
                finally
                {
                    window.Hide();
                }
            };
            useButton.Theme = null;

            dropOrSwapButton.Click += (o, e) => {
                try
                {
                    if (!window.TileIsOccupied)
                        BackendHandler.Instance.PlayerDropItemFromInventory(window.Inventory.InventoryItems[window.InventorySelectedIndex].ItemId);
                    else
                        BackendHandler.Instance.PlayerSwapFloorItemWithInventoryItem(window.Inventory.InventoryItems[window.InventorySelectedIndex].ItemId);
                    window.ParentConsole.RequiresRefreshingDungeonState = true;
                }
                catch (Exception)
                {
                    parent.ChangeConsoleContainerTo(ConsoleContainers.Message, ConsoleContainers.Main, "ERROR", "OH NO!\nAn error has occured!\nGet ready to return to the main menu...");
                }
                finally
                {
                    window.Hide();
                }
            };
            dropOrSwapButton.Theme = null;

            cancelButton.Position = new Point((window.Width - cancelButton.Surface.Width) - 2, window.Height - cancelButton.Surface.Height);
            cancelButton.Click += (o, e) => window.Hide();
            cancelButton.Theme = null;

            window.UseButton = useButton;
            window.Controls.Add(useButton);
            window.DropOrSwapButton = dropOrSwapButton;
            window.Controls.Add(dropOrSwapButton);
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
            ds.Surface.DrawLine(new Point(0, 0), new Point(0, Height - 3), ICellSurface.ConnectedLineThick[3], Color.Yellow);
            ds.Surface.DrawLine(new Point(0, 0), new Point(Width - 1, 0), ICellSurface.ConnectedLineThick[3], Color.Yellow);
            ds.Surface.DrawLine(new Point(32, 0), new Point(32, Height - 3), ICellSurface.ConnectedLineThick[3], Color.Yellow);
            ds.Surface.DrawLine(new Point(Width - 1, 0), new Point(Width - 1, Height - 3), ICellSurface.ConnectedLineThick[3], Color.Yellow);
            ds.Surface.DrawLine(new Point(0, window.Height - 3), new Point(Width - 1, Height - 3), ICellSurface.ConnectedLineThick[3], Color.Yellow);
            ds.Surface.ConnectLines(ICellSurface.ConnectedLineThick);
            ds.Surface.Print((Width - TitleCaption.Length - 2) / 2, 0, $" {TitleCaption} ", Color.Black, Color.Yellow);

            var item = Inventory.InventoryItems.ElementAtOrDefault(InventorySelectedIndex);

            var initialIndexToShow = CurrentlyShownInventoryItems != null && CurrentlyShownInventoryItems.Contains(item) ? CurrentlyShownFirstIndex : Math.Min(CurrentlyShownFirstIndex + 1, Inventory.InventoryItems.IndexOf(item));
            CurrentlyShownFirstIndex = initialIndexToShow;
            var inventoryItemsToShow = CurrentlyShownInventoryItems != null && CurrentlyShownInventoryItems.Contains(item) ? CurrentlyShownInventoryItems : Inventory.InventoryItems.Skip(initialIndexToShow).Take(24).ToList();
            CurrentlyShownInventoryItems = inventoryItemsToShow;

            for (int i = 0; i < inventoryItemsToShow.Count; i++)
            {
                string nameToDisplay;
                if (Inventory.InventoryItems[initialIndexToShow + i].IsInFloor)
                    nameToDisplay = $"(F) {Inventory.InventoryItems[initialIndexToShow + i].Name}";
                else if (Inventory.InventoryItems[initialIndexToShow + i].IsEquipped)
                    nameToDisplay = $"(E) {Inventory.InventoryItems[initialIndexToShow + i].Name}";
                else
                    nameToDisplay = Inventory.InventoryItems[initialIndexToShow + i].Name;
                var itemName = nameToDisplay.PadRight(29);
                if (Inventory.InventoryItems[initialIndexToShow + i] == item)
                    ds.Surface.Print(2, 2 + i, itemName, Color.Black, Color.White);
                else
                    ds.Surface.Print(2, 2 + i, itemName, Color.White, Color.Black);
            }
            TileIsOccupied = Inventory.TileIsOccupied;
            ItemIsEquippable = item.IsEquippable;
            UseButton.Text = ItemIsEquippable ? "EQUIP" : "USE";
            if(item.IsEquipped)
            {
                UseButton.IsEnabled = false;
                UseButton.IsVisible = false;
            }
            else
            {
                UseButton.IsEnabled = true;
                UseButton.IsVisible = true;
                UseButton.Text = ItemIsEquippable ? "EQUIP" : "USE";
            }
            if (item.IsInFloor || (item.IsEquipped && TileIsOccupied))
            {
                DropOrSwapButton.IsEnabled = false;
                DropOrSwapButton.IsVisible = false;
            }
            else
            {
                DropOrSwapButton.IsEnabled = true;
                DropOrSwapButton.IsVisible = true;
                DropOrSwapButton.Text = TileIsOccupied ? "SWAP" : "DROP";
            }
            DropOrSwapButton.Position = new Point(ds.Surface.Area.Center.X - DropOrSwapButton.Text.Length / 2, Height - DropOrSwapButton.Surface.Height);
            if (item != null)
            {
                UseButton.IsEnabled = item.CanBeUsed;

                ds.Surface.Print(34, 2, item.Name, Color.White, Color.Black);

                ds.Surface.SetGlyph(34, 4, item.ConsoleRepresentation.Character.ToGlyph(), item.ConsoleRepresentation.ForegroundColor.ToSadRogueColor(), item.ConsoleRepresentation.BackgroundColor.ToSadRogueColor());

                var descriptionAsString = item.Description.Wrap(30);
                var linesInDescription = item.Description.Split(
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

                var lastPrintedLine = 6;

                for (int i = 0; i < splitWrappedDescription.Count; i++)
                {
                    lastPrintedLine++;
                    ds.Surface.Print(34, lastPrintedLine, splitWrappedDescription[i].Trim(), Color.White, Color.Black);
                }

                if (item.IsInFloor)
                    ds.Surface.Print(34, lastPrintedLine + 2, "(This item is in the floor)", Color.White, Color.Black);
                else if (item.IsEquipped)
                {
                    ds.Surface.Print(34, lastPrintedLine + 2, "(This item is equipped)", Color.White, Color.Black);
                    if(TileIsOccupied)
                        ds.Surface.Print(34, lastPrintedLine + 3, "(Tile is occupied, can't drop)", Color.White, Color.Black);
                }
            }
            ds.IsDirty = true;
            ds.IsFocused = true;
        }

        public override bool ProcessKeyboard(Keyboard info)
        {
            if(info.IsKeyPressed(Keys.Up))
            {
                InventorySelectedIndex = Math.Max(0, InventorySelectedIndex - 1);
                DrawingArea.IsDirty = true;
            }
            else if (info.IsKeyPressed(Keys.Down))
            {
                InventorySelectedIndex = Math.Min(Inventory.InventoryItems.Count - 1, InventorySelectedIndex + 1);
                DrawingArea.IsDirty = true;
            }
            else if (UseButton.IsEnabled
                && (info.IsKeyPressed(Keys.Enter)
                || (info.IsKeyPressed(Keys.E) && ItemIsEquippable)
                || (info.IsKeyPressed(Keys.U) && !ItemIsEquippable)))
            {
                UseButton.InvokeClick();
            }
            else if ((info.IsKeyPressed(Keys.D) && !TileIsOccupied)
                || (info.IsKeyPressed(Keys.S) && TileIsOccupied))
            {
                DropOrSwapButton.InvokeClick();
            }
            else if (info.IsKeyPressed(Keys.C) || info.IsKeyPressed(Keys.Escape))
            {
                CancelButton.InvokeClick();
            }

            return true;
        }
    }
}
