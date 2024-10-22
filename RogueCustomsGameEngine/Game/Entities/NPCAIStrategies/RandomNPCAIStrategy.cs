using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsGameEngine.Game.Entities.NPCAIStrategies
{
    public class RandomNPCAIStrategy : INPCAIStrategy
    {
        public int GetActionWeight(ActionWithEffects action, Map map, Entity This, NonPlayableCharacter Source, ITargetable Target)
        {
            var distanceFactor = action.MaximumRange > 1 ? (int) GamePoint.Distance(Source.Position, Target.Position) : 0;
            return Target != Source ? 1000 - 10 * distanceFactor : 1;
        }

        public int GetEffectWeight(Effect effect, Map map, Entity This, NonPlayableCharacter Source, ITargetable Target) => 0;
    }
}
