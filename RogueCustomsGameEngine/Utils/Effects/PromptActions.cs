using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Utils.Effects.Utils;
using RogueCustomsGameEngine.Utils.Expressions;
using RogueCustomsGameEngine.Utils.Helpers;

namespace RogueCustomsGameEngine.Utils.Effects
{
#pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
#pragma warning disable CS8604 // Posible argumento de referencia nulo
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    
    public static class PromptActions
    {
        private static RngHandler Rng;
        private static Map Map;

        public static void SetActionParams(RngHandler rng, Map map)
        {
            Rng = rng;
            Map = map;
        }

        public static async Task<bool> SelectYesNo(EffectCallerParams Args)
        {
            dynamic paramsObject = ExpressionParser.ParseParams(Args);

            if (Map.IsDebugMode)
            {
                var rolledYes = Rng.RollProbability() > 50;
                var response = rolledYes ? paramsObject.YesButtonText : paramsObject.NoButtonText;
                Map.AppendMessage($"PROMPT => {paramsObject.Title}: {paramsObject.Message}\n\nRESPONSE: {response}", paramsObject.Color);
                return rolledYes;
            }
            else if(Args.Source is NonPlayableCharacter)
            {
                return true; // NPCs will only try to do this if they want to, so we assume they say "yes" to the prompt.
            }

            return await Map.OpenYesNoPrompt(paramsObject.Title, paramsObject.Message, paramsObject.YesButtonText, paramsObject.NoButtonText, paramsObject.Color);
        }
    }

#pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
#pragma warning restore CS8604 // Posible argumento de referencia nulo
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
