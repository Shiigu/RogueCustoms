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
            var distanceFactor = action.MaximumRange > 1 ? (int)GamePoint.Distance(Source.Position, Target.Position) : 0;

            return (int)(GetEffectWeight(action.Effect, map, This, Source, Target) - 5 * distanceFactor);
        }

        public int GetEffectWeight(Effect effect, Map map, Entity This, NonPlayableCharacter Source, ITargetable Target)
        {
            return new DefaultNPCAIStrategy().GetEffectWeight(effect, map, This, Source, Target);
        }
    }
}
