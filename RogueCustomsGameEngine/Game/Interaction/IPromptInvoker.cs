using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsGameEngine.Game.Interaction
{
    public interface IPromptInvoker
    {
        Task<bool> OpenYesNoPrompt(string title, string message, string yesButtonText, string noButtonText, GameColor borderColor);
        Task<string> OpenSelectOption(string title, string message, SelectionItem[] choices, bool showCancelButton, GameColor borderColor);
        Task<string> OpenSelectItem(string title, InventoryDto choices, bool showCancelButton);
        Task<string> OpenSelectAction(string title, ActionListDto choices, bool showCancelButton);
    }
}
