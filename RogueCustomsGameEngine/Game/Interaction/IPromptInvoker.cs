using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsGameEngine.Game.Interaction
{
    public interface IPromptInvoker
    {
        Task<bool> OpenYesNoPrompt(string title, string message, string yesButtonText, string noButtonText, GameColor borderColor);
    }
}
