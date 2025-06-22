using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Godot;

using RogueCustomsGameEngine.Game.Interaction;
using RogueCustomsGameEngine.Utils.Representation;

using RogueCustomsGodotClient.Helpers;
using RogueCustomsGodotClient.Popups;

namespace RogueCustomsGodotClient.Invokers
{
    public class PromptInvoker : IPromptInvoker
    {
        private readonly Control Parent;

        public PromptInvoker(Control parent)
        {
            Parent = parent;
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
            return promptResponse;
        }
    }
}
