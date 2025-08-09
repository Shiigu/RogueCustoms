using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.Effects.Utils;
using RogueCustomsGameEngine.Utils.Helpers;
using RogueCustomsGameEngine.Utils.Representation;

namespace RogueCustomsGameEngine.Game.Entities.NPCAIStrategies
{
    public class AllOutNPCAIStrategy : INPCAIStrategy
    {
        public int GetActionWeight(ActionWithEffects action, Map map, EffectCallerParams args)
        {
            var distanceFactor = action.MaximumRange > 1 ? (int)GamePoint.Distance(args.Source.Position, args.Target.Position) : 0;

            return (int)(GetEffectWeight(action.Effect, map, args) - 5 * distanceFactor);
        }

        public int GetEffectWeight(Effect effect, Map map, EffectCallerParams args)
        {
            return new DefaultNPCAIStrategy().GetEffectWeight(effect, map, args);
        }
    }
}
