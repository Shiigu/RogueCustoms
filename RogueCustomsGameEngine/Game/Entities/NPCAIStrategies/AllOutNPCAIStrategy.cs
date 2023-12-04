using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsGameEngine.Game.Entities.NPCAIStrategies
{
    public class AllOutNPCAIStrategy : INPCAIStrategy
    {
        public int GetActionWeight(ActionWithEffects action, Map map, Entity This, NonPlayableCharacter Source, ITargetable Target)
        {
            // Very heavily discourage NPCs from using actions that cannot be applied on the target in the current turn.
            if (!action.CanBeUsedOn(Target, Source))
                return int.MinValue;

            var randomFactor = map.Rng.NextInclusive(50, 150) / 100f;
            return (int)(GetEffectWeight(action.Effect, map, This, Source, Target) * randomFactor);
        }

        public int GetEffectWeight(Effect effect, Map map, Entity This, NonPlayableCharacter Source, ITargetable Target)
        {
            return new DefaultNPCAIStrategy().GetEffectWeight(effect, map, This, Source, Target);
        }
    }
}
