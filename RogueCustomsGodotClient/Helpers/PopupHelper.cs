using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Godot;

using RogueCustomsGameEngine.Utils.InputsAndOutputs;

using RogueCustomsGodotClient.Popups;
using RogueCustomsGodotClient.Utils;

namespace RogueCustomsGodotClient.Helpers
{
    public static class PopupHelper
    {
        public static bool IsPopUp(this Node n)
        {
            return n is InputBox || n is PopUp || n is ScrollablePopUp || n is PlayerSelectItem || n is SelectClass || n is SelectSaveGame;
        }

        public static async Task CreateStandardPopup(this Control control, string titleText, string innerText, PopUpButton[] buttons, Color borderColor)
        {
            var overlay = new ColorRect
            {
                Color = new Color() { R8 = 0, G8 = 0, B8 = 0, A = 0.75f },
                Size = control.GetViewportRect().Size
            };
            control.AddChild(overlay);

            var popup = (PopUp)GD.Load<PackedScene>("res://Pop-ups/PopUp.tscn").Instantiate();
            control.AddChild(popup);

            var popupClosedSignal = popup.ToSignal(popup, "PopupClosed");
            popup.Show(titleText, innerText, buttons, borderColor, () => overlay.QueueFree());
            
            await popupClosedSignal;
        }

        public static void CreateScrollablePopup(this Control control, string titleText, string innerText, Color borderColor, bool scrollToEnd)
        {
            var overlay = new ColorRect
            {
                Color = new Color() { R8 = 0, G8 = 0, B8 = 0, A = 0.75f },
                Size = control.GetViewportRect().Size
            };
            control.AddChild(overlay);

            var scrollablePopup = (ScrollablePopUp)GD.Load<PackedScene>("res://Pop-ups/ScrollablePopUp.tscn").Instantiate();
            control.AddChild(scrollablePopup);
            scrollablePopup.Show(titleText, innerText, borderColor, () => overlay.QueueFree(), scrollToEnd);
        }

        public static void CreateInventoryWindow(this Control control, InventoryDto itemInfo)
        {
            var overlay = new ColorRect
            {
                Color = new Color() { R8 = 0, G8 = 0, B8 = 0, A = 0.75f },
                Size = control.GetViewportRect().Size
            };
            control.AddChild(overlay);

            var inventoryPopup = (PlayerSelectItem)GD.Load<PackedScene>("res://Pop-ups/PlayerSelectItem.tscn").Instantiate();
            control.AddChild(inventoryPopup);
            inventoryPopup.Show(itemInfo, SelectionMode.Inventory, () => overlay.QueueFree());
        }

        public static void CreateActionSelectWindow(this Control control, ActionListDto actionInfo, Vector2I targetCoords)
        {
            var overlay = new ColorRect
            {
                Color = new Color() { R8 = 0, G8 = 0, B8 = 0, A = 0.75f },
                Size = control.GetViewportRect().Size
            };
            control.AddChild(overlay);

            var actionSelectPopup = (PlayerSelectItem)GD.Load<PackedScene>("res://Pop-ups/PlayerSelectItem.tscn").Instantiate();
            control.AddChild(actionSelectPopup);
            actionSelectPopup.Show(actionInfo, SelectionMode.Interact, targetCoords, () => overlay.QueueFree());
        }

        public static void CreateInputBox(this Control control, string titleText, string promptText, string placeholderText, Color borderColor, Action<string> okCallback, Action cancelCallback)
        {
            var overlay = new ColorRect
            {
                Color = new Color() { R8 = 0, G8 = 0, B8 = 0, A = 0.75f },
                Size = control.GetViewportRect().Size
            };
            control.AddChild(overlay);

            var inputBox = (InputBox)GD.Load<PackedScene>("res://Pop-ups/InputBox.tscn").Instantiate();
            control.AddChild(inputBox);
            inputBox.Show(titleText, promptText, placeholderText, borderColor, (inputText) => { overlay.QueueFree(); okCallback?.Invoke(inputText); }, () => { overlay.QueueFree(); cancelCallback?.Invoke(); });
        }

        public static void CreateSelectClassWindow(this Control control, Action<string> selectCallback, Action cancelCallback)
        {
            var selectClassWindow = (SelectClass)GD.Load<PackedScene>("res://Pop-ups/SelectClass.tscn").Instantiate();
            control.AddChild(selectClassWindow);
            selectClassWindow.Show((classId) => { selectCallback?.Invoke(classId); }, () => cancelCallback?.Invoke());
        }

        public static void CreateLoadSaveGamePopup(this Control control)
        {
            var overlay = new ColorRect
            {
                Color = new Color() { R8 = 0, G8 = 0, B8 = 0, A = 0.75f },
                Size = control.GetViewportRect().Size
            };
            control.AddChild(overlay);

            var selectSaveGamePopup = (SelectSaveGame)GD.Load<PackedScene>("res://Pop-ups/SelectSaveGame.tscn").Instantiate();
            control.AddChild(selectSaveGamePopup);
            selectSaveGamePopup.Show(() => { overlay.QueueFree(); });
        }

        public static async Task<string> CreateSelectPopup(this Control control, string titleText, string innerText, SelectionItem[] choices, bool showCancelButton, Color borderColor)
        {
            var overlay = new ColorRect
            {
                Color = new Color() { R8 = 0, G8 = 0, B8 = 0, A = 0.75f },
                Size = control.GetViewportRect().Size
            };
            control.AddChild(overlay);

            var popup = (SelectPopUp)GD.Load<PackedScene>("res://Pop-ups/SelectPopUp.tscn").Instantiate();
            control.AddChild(popup);

            var popupClosedSignal = popup.ToSignal(popup, "PopupClosed");
            popup.Show(titleText, innerText, choices, showCancelButton, borderColor);

            var result = await popupClosedSignal;

            overlay.QueueFree();

            return result.Length > 0 ? (string) result[0] : null;
        }
    }
}
