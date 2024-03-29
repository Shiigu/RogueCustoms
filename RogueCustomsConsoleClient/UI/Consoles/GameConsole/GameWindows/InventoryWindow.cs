﻿using SadConsole.UI.Controls;
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
using System.Text;
using System.Collections.Generic;
using System;
using System.Linq;
using RogueCustomsGameEngine.Utils.Helpers;

namespace RogueCustomsConsoleClient.UI.Consoles.GameConsole.GameWindows
{
    #pragma warning disable CS8604 // Posible argumento de referencia nulo
    #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public sealed class InventoryWindow : Window
    {
        private readonly string UseButtonText = LocalizationManager.GetString("UseButtonText").ToAscii();
        private readonly string EquipButtonText = LocalizationManager.GetString("EquipButtonText").ToAscii();
        private readonly string DropButtonText = LocalizationManager.GetString("DropButtonText").ToAscii();
        private readonly string SwapButtonText = LocalizationManager.GetString("SwapButtonText").ToAscii();
        private readonly string CancelButtonText = LocalizationManager.GetString("CancelButtonText").ToAscii();

        private Button UseOrEquipButton, DropOrSwapButton, CancelButton;
        private bool IsReadOnly;
        private string TitleCaption;
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

        public static Window? Show(GameConsoleContainer parent, InventoryDto inventory, bool readOnly)
        {
            if (inventory?.InventoryItems.Any() != true) return null;
            const int width = GameConsoleConstants.SelectionWindowWidth;
            const int height = GameConsoleConstants.SelectionWindowHeight;

            var window = new InventoryWindow(width, height);

            var useButton = new Button(Math.Max(window.UseButtonText.Length, window.EquipButtonText.Length) + 2, 1)
            {
                Text = window.UseButtonText,
            };

            var dropOrSwapButton = new Button(Math.Max(window.DropButtonText.Length, window.SwapButtonText.Length) + 2, 1)
            {
                Text = window.DropButtonText,
            };

            var cancelButton = new Button(window.CancelButtonText.Length + 2, 1)
            {
                Text = window.CancelButtonText,
            };

            window.UseKeyboard = true;
            window.IsFocused = true;
            window.IsReadOnly = readOnly;
            window.Inventory = inventory;
            window.InventorySelectedIndex = 0;
            window.IsDirty = true;
            window.ParentConsole = parent;
            window.Font = Game.Instance.LoadFont("fonts/Alloy_curses_12x12.font");
            window.TitleCaption = LocalizationManager.GetString("InventoryWindowTitleText").ToAscii();

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
                    parent.ChangeConsoleContainerTo(ConsoleContainers.Message, ConsoleContainers.Main, LocalizationManager.GetString("ErrorMessageHeader"), LocalizationManager.GetString("ErrorText"));
                }
                finally
                {
                    window.Hide();
                }
            };

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
                    parent.ChangeConsoleContainerTo(ConsoleContainers.Message, ConsoleContainers.Main, LocalizationManager.GetString("ErrorMessageHeader"), LocalizationManager.GetString("ErrorText"));
                }
                finally
                {
                    window.Hide();
                }
            };

            cancelButton.Position = new Point(window.Width - cancelButton.Surface.Width - 2, window.Height - cancelButton.Surface.Height);
            cancelButton.Click += (o, e) => window.Hide();

            window.UseOrEquipButton = useButton;
            window.Controls.Add(useButton);
            window.DropOrSwapButton = dropOrSwapButton;
            window.Controls.Add(dropOrSwapButton);
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

            if (ds.Parent is not ControlHost host) return;
            if (host.ParentConsole is not Console window) return;

            ds.Surface.Clear();
            var appearance = ds.ThemeState.Normal;

            ds.Surface.Fill(appearance.Foreground, appearance.Background, null);
            ds.Surface.DrawLine(new Point(0, 0), new Point(0, Height - 3), ICellSurface.ConnectedLineThick[3], Color.Yellow);
            ds.Surface.DrawLine(new Point(0, 0), new Point(Width - 1, 0), ICellSurface.ConnectedLineThick[3], Color.Yellow);
            ds.Surface.DrawLine(new Point(39, 0), new Point(39, Height - 3), ICellSurface.ConnectedLineThick[3], Color.Yellow);
            ds.Surface.DrawLine(new Point(Width - 1, 0), new Point(Width - 1, Height - 3), ICellSurface.ConnectedLineThick[3], Color.Yellow);
            ds.Surface.DrawLine(new Point(0, window.Height - 3), new Point(Width - 1, Height - 3), ICellSurface.ConnectedLineThick[3], Color.Yellow);
            ds.Surface.ConnectLines(ICellSurface.ConnectedLineThick);
            ds.Surface.Print((Width - TitleCaption.Length - 2) / 2, 0, $" {TitleCaption.ToAscii()} ", Color.Black, Color.Yellow);

            var item = Inventory.InventoryItems.ElementAtOrDefault(InventorySelectedIndex);

            var initialIndexToShow = CurrentlyShownInventoryItems?.Contains(item) == true ? CurrentlyShownFirstIndex : Math.Min(CurrentlyShownFirstIndex + 1, Inventory.InventoryItems.IndexOf(item));
            CurrentlyShownFirstIndex = initialIndexToShow;
            var inventoryItemsToShow = CurrentlyShownInventoryItems?.Contains(item) == true ? CurrentlyShownInventoryItems : Inventory.InventoryItems.Skip(initialIndexToShow).Take(30).ToList();
            CurrentlyShownInventoryItems = inventoryItemsToShow;

            for (int i = 0; i < inventoryItemsToShow.Count; i++)
            {
                string nameToDisplay;
                if (Inventory.InventoryItems[initialIndexToShow + i].IsInFloor)
                    nameToDisplay = $"{LocalizationManager.GetString("FloorItemNamePrefix").ToAscii()} {Inventory.InventoryItems[initialIndexToShow + i].Name.ToAscii()}";
                else if (Inventory.InventoryItems[initialIndexToShow + i].IsEquipped)
                    nameToDisplay = $"{LocalizationManager.GetString("EquippedItemNamePrefix").ToAscii()} {Inventory.InventoryItems[initialIndexToShow + i].Name.ToAscii()}";
                else
                    nameToDisplay = Inventory.InventoryItems[initialIndexToShow + i].Name.ToAscii();
                var itemName = nameToDisplay.PadRight(36);
                if (Inventory.InventoryItems[initialIndexToShow + i] == item)
                    ds.Surface.Print(2, 2 + i, itemName, Color.Black, Color.White);
                else
                    ds.Surface.Print(2, 2 + i, itemName, Color.White, Color.Black);
            }
            TileIsOccupied = Inventory.TileIsOccupied;
            ItemIsEquippable = item.IsEquippable;

            if(item.IsEquipped)
            {
                UseOrEquipButton.IsEnabled = false;
                UseOrEquipButton.IsVisible = false;
            }
            else
            {
                UseOrEquipButton.IsEnabled = !IsReadOnly;
                UseOrEquipButton.IsVisible = true;
                UseOrEquipButton.Text = ItemIsEquippable ? EquipButtonText : UseButtonText;
            }
            if (item.IsInFloor || (item.IsEquipped && TileIsOccupied))
            {
                DropOrSwapButton.IsEnabled = false;
                DropOrSwapButton.IsVisible = false;
            }
            else
            {
                DropOrSwapButton.IsEnabled = !IsReadOnly;
                DropOrSwapButton.IsVisible = true;
                DropOrSwapButton.Text = TileIsOccupied ? SwapButtonText : DropButtonText;
            }
            DropOrSwapButton.Position = new Point(ds.Surface.Area.Center.X - (DropOrSwapButton.Text.Length / 2), Height - DropOrSwapButton.Surface.Height);
            if (item != null)
            {
                UseOrEquipButton.IsEnabled = !item.IsEquipped && item.CanBeUsed;

                ds.Surface.Print(41, 2, item.Name.ToAscii(), Color.White, Color.Black);

                ds.Surface.SetGlyph(41, 4, item.ConsoleRepresentation.Character.ToGlyph(), item.ConsoleRepresentation.ForegroundColor.ToSadRogueColor(), item.ConsoleRepresentation.BackgroundColor.ToSadRogueColor());

                var descriptionToDisplay = new StringBuilder(item.Description);

                if (item.IsInFloor)
                {
                    descriptionToDisplay.Append("\n\n").Append(LocalizationManager.GetString("FloorItemDescriptionText"));
                }
                else if (item.IsEquipped)
                {
                    descriptionToDisplay.Append("\n\n").Append(LocalizationManager.GetString("EquippedItemDescriptionText"));
                    if (TileIsOccupied)
                        descriptionToDisplay.Append(LocalizationManager.GetString("OccupiedTileDescriptionText"));
                }

                var linesInDescription = descriptionToDisplay.ToString().Split(
                    new[] { "\r\n", "\n" }, StringSplitOptions.None
                    );
                var splitWrappedDescription = linesInDescription.SplitByLengthWithWholeWords(GameConsoleConstants.DescriptionWindowMaxLength).ToList();

                var lastPrintedLine = 6;

                for (int i = 0; i < splitWrappedDescription.Count; i++)
                {
                    lastPrintedLine++;
                    ds.Surface.Print(41, lastPrintedLine, splitWrappedDescription[i].Trim().ToAscii(), Color.White, Color.Black);
                }
            }
            ds.IsDirty = true;
            ds.IsFocused = true;
        }

        public override bool ProcessKeyboard(Keyboard info)
        {
            if(info.IsKeyPressed(Keys.Up) && info.KeysPressed.Count == 1)
            {
                InventorySelectedIndex = Math.Max(0, InventorySelectedIndex - 1);
                DrawingArea.IsDirty = true;
            }
            else if (info.IsKeyPressed(Keys.Down) && info.KeysPressed.Count == 1)
            {
                InventorySelectedIndex = Math.Min(Inventory.InventoryItems.Count - 1, InventorySelectedIndex + 1);
                DrawingArea.IsDirty = true;
            }
            else if (UseOrEquipButton.IsEnabled
                && (info.IsKeyPressed(Keys.Enter)
                || (info.IsKeyPressed(Keys.E) && ItemIsEquippable)
                || (info.IsKeyPressed(Keys.U) && !ItemIsEquippable)) && info.KeysPressed.Count == 1)
            {
                UseOrEquipButton.InvokeClick();
            }
            else if (DropOrSwapButton.IsEnabled
                && ((info.IsKeyPressed(Keys.D) && !TileIsOccupied)
                || (info.IsKeyPressed(Keys.S) && TileIsOccupied)) && info.KeysPressed.Count == 1)
            {
                DropOrSwapButton.InvokeClick();
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
