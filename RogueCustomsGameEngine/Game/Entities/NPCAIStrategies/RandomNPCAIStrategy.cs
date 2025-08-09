using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.Effects.Utils;
using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsGameEngine.Game.Entities.NPCAIStrategies
{
    public class RandomNPCAIStrategy : INPCAIStrategy
    {
        public int GetActionWeight(ActionWithEffects action, Map map, EffectCallerParams args)
        {
            var distanceFactor = action.MaximumRange > 1 ? (int) GamePoint.Distance(args.Source.Position, args.Target.Position) : 0;
            return args.Target != args.Source ? 1000 - 10 * distanceFactor : 500;
        }

        public int GetEffectWeight(Effect effect, Map map, EffectCallerParams args) => 0;
    }
}
