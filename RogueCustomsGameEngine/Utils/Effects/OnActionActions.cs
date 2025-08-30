using Newtonsoft.Json.Linq;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.Effects.Utils;
using RogueCustomsGameEngine.Utils.Enums;
using RogueCustomsGameEngine.Utils.Exceptions;
using RogueCustomsGameEngine.Utils.Expressions;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.InputsAndOutputs;
using RogueCustomsGameEngine.Utils.Representation;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Utils.Effects
{
#pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
#pragma warning disable CS8604 // Posible argumento de referencia nulo
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public static class OnActionActions
    {
        private static RngHandler Rng;
        private static Map Map;

        public static void SetActionParams(RngHandler rng, Map map)
        {
            Rng = rng;
            Map = map;
        }

        public static bool ChangeCooldown(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);

            if (paramsObject.Target is not Character t)
                // Target must be a Character
                return false;

            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Args.Source, null, paramsObject);

            if (Rng.RollProbability() > accuracyCheck)
                return false;

            var success = false;

            string actionSchool = paramsObject.ActionSchool;
            int cooldownModifier = (int) paramsObject.Amount;

            if (cooldownModifier == 0)
                // Cooldown Modifier must not be 0
                return false;

            List<ActionWithEffects> correspondingActions = [];

            if (!actionSchool.Equals("<<CUSTOM>>", StringComparison.InvariantCultureIgnoreCase))
            {
                correspondingActions.AddRange(t.OnAttack.Where(oa => actionSchool.Equals("All", StringComparison.InvariantCultureIgnoreCase) || oa.School.Id.Equals(actionSchool, StringComparison.InvariantCultureIgnoreCase)));
            }
            else
            {
                string actionId = paramsObject.CustomId;

                var correspondingAction = t.OnAttack.Find(oa => oa.SelectionId.Equals(actionId, StringComparison.InvariantCultureIgnoreCase));

                if(correspondingAction != null)
                    correspondingActions.Add(correspondingAction);
            }

            bool informThePlayer = paramsObject.InformThePlayer;

            foreach (var correspondingAction in correspondingActions)
            {
                var priorCooldown = correspondingAction.CurrentCooldown;
                correspondingAction.CurrentCooldown += cooldownModifier;

                if (correspondingAction.CurrentCooldown < 0)
                    correspondingAction.CurrentCooldown = 0;

                var actualCooldownChange = correspondingAction.CurrentCooldown - priorCooldown;

                if (informThePlayer && (t == Map.Player || Map.Player.CanSee(t)))
                {
                    var message = string.Empty;

                    if (correspondingAction.CurrentCooldown == 0)
                        message = Map.Locale["CharacterActionCooldownRemoved"].Format(new { CharacterName = t.Name, ActionName = correspondingAction.Name });
                    else if (actualCooldownChange > 0)
                        message = Map.Locale["CharacterActionCooldownIncreased"].Format(new { CharacterName = t.Name, ActionName = correspondingAction.Name, Amount = actualCooldownChange.ToString() });
                    else if (actualCooldownChange < 0)
                        message = Map.Locale["CharacterActionCooldownDecreased"].Format(new { CharacterName = t.Name, ActionName = correspondingAction.Name, Amount = actualCooldownChange.ToString() });

                    Map.AppendMessage(message, Color.DeepSkyBlue, events);
                }

                Map.DisplayEvents.Add(($"{t.Name} got {correspondingAction.Name}'s Cooldown modified", events));

                success = success || actualCooldownChange > 0;
            }

            return success;
        }
        public static bool ChangeUses(EffectCallerParams Args)
        {
            var events = new List<DisplayEventDto>();
            dynamic paramsObject = ExpressionParser.ParseParams(Args);

            if (paramsObject.Target is not Character t)
                // Target must be a Character
                return false;

            var accuracyCheck = ExpressionParser.CalculateAdjustedAccuracy(Args.Source, null, paramsObject);

            if (Rng.RollProbability() > accuracyCheck)
                return false;

            var success = false;

            string actionSchool = paramsObject.ActionSchool;
            int usesModifier = (int)paramsObject.Amount;

            if (usesModifier == 0)
                // Uses Modifier must not be 0
                return false;

            List<ActionWithEffects> correspondingActions = [];

            if (!actionSchool.Equals("<<CUSTOM>>", StringComparison.InvariantCultureIgnoreCase))
            {
                correspondingActions.AddRange(t.OnAttack.Where(oa => oa.MaximumUses > 0 && (actionSchool.Equals("All", StringComparison.InvariantCultureIgnoreCase) || oa.School.Id.Equals(actionSchool, StringComparison.InvariantCultureIgnoreCase))));
            }
            else
            {
                string actionId = paramsObject.CustomId;

                var correspondingAction = t.OnAttack.Find(oa => oa.MaximumUses > 0 && oa.SelectionId.Equals(actionId, StringComparison.InvariantCultureIgnoreCase));

                if (correspondingAction != null)
                    correspondingActions.Add(correspondingAction);
            }

            bool informThePlayer = paramsObject.InformThePlayer;

            foreach (var correspondingAction in correspondingActions)
            {
                var priorUses = correspondingAction.CurrentUses;
                correspondingAction.CurrentCooldown += usesModifier;

                if (correspondingAction.CurrentUses < 0)
                    correspondingAction.CurrentUses = 0;
                if (correspondingAction.CurrentUses > correspondingAction.MaximumUses)
                    correspondingAction.CurrentUses = correspondingAction.MaximumUses;

                var actualUsesChange = correspondingAction.CurrentUses - priorUses;

                if (informThePlayer && (t == Map.Player || Map.Player.CanSee(t)))
                {
                    var message = string.Empty;

                    if (correspondingAction.CurrentUses == correspondingAction.MaximumUses)
                        message = Map.Locale["CharacterActionUsesMaximized"].Format(new { CharacterName = t.Name, ActionName = correspondingAction.Name, Amount = actualUsesChange.ToString() });
                    else if (actualUsesChange > 0)
                        message = Map.Locale["CharacterActionUsesIncreased"].Format(new { CharacterName = t.Name, ActionName = correspondingAction.Name, Amount = actualUsesChange.ToString() });
                    else if (actualUsesChange < 0)
                        message = Map.Locale["CharacterActionUsesDecreased"].Format(new { CharacterName = t.Name, ActionName = correspondingAction.Name, Amount = actualUsesChange.ToString() });

                    Map.AppendMessage(message, Color.DeepSkyBlue, events);
                }

                Map.DisplayEvents.Add(($"{t.Name} got {correspondingAction.Name}'s Uses modified", events));

                success = success || actualUsesChange > 0;
            }

            return success;
        }
    }
#pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
#pragma warning restore CS8604 // Posible argumento de referencia nulo
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}
