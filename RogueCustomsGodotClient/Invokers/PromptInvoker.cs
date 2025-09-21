using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Godot;

using RogueCustomsGameEngine.Game.Interaction;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsGameEngine.Utils.Representation;

using RogueCustomsGodotClient.Helpers;
using RogueCustomsGodotClient.Popups;
using RogueCustomsGodotClient.Utils;

namespace RogueCustomsGodotClient.Invokers
{
    public class PromptInvoker : IPromptInvoker
    {
        private readonly Control Parent;
        private readonly GlobalState _globalState;

        public PromptInvoker(Control parent)
        {
            Parent = parent;
            _globalState = parent.GetNode<GlobalState>("/root/GlobalState");
        }

        public async Task<bool> OpenYesNoPrompt(string title, string message, string yesButtonText, string noButtonText, GameColor borderColor)
        {
            var promptResponse = false;
            await Parent.CreateStandardPopup(title,
                                            message,
                                            new PopUpButton[]
                                            {
                                        new() { Text = yesButtonText, Callback = () => promptResponse = true, ActionPress = "ui_accept" },
                                        new() { Text = noButtonText, Callback = () => promptResponse = false, ActionPress = "ui_cancel" }
                                            }, new Color() { R8 = borderColor.R, G8 = borderColor.G, B8 = borderColor.B, A8 = borderColor.A });
            _globalState.DungeonManager.RefreshDisplay(true);
            return promptResponse;
        }

        public async Task<string> OpenSelectOption(string title, string message, SelectionItem[] choices, bool showCancelButton, GameColor borderColor)
        {
            var result = await Parent.CreateSelectOptionPopup(title, message, choices, showCancelButton, new Color() { R8 = borderColor.R, G8 = borderColor.G, B8 = borderColor.B, A8 = borderColor.A });
            _globalState.DungeonManager.RefreshDisplay(true);
            return result;
        }

        public async Task<ItemInput> OpenSelectItem(string title, InventoryDto choices, bool showCancelButton)
        {
            var result = await Parent.CreateSelectItemWindow(choices, title, showCancelButton);
            _globalState.DungeonManager.RefreshDisplay(true);
            return result;
        }

        public async Task<string> OpenSelectAction(string title, ActionListDto choices, bool showCancelButton)
        {
            var result = await Parent.CreateSelectActionWindow(choices, title, showCancelButton);
            _globalState.DungeonManager.RefreshDisplay(true);
            return result;
        }

        public async Task<int?> OpenBuyPrompt(string title, InventoryDto choices, bool showCancelButton)
        {
            var result = await Parent.CreateBuySellWindow(choices, title, showCancelButton, SelectionMode.Buy);
            _globalState.DungeonManager.RefreshDisplay(true);
            return result;
        }

        public async Task<int?> OpenSellPrompt(string title, InventoryDto choices, bool showCancelButton)
        {
            var result = await Parent.CreateBuySellWindow(choices, title, showCancelButton, SelectionMode.Sell);
            _globalState.DungeonManager.RefreshDisplay(true);
            return result;
        }
    }
}
