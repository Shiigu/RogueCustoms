using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.Effects.Utils;

namespace RogueCustomsGameEngine.Game.Entities.NPCAIStrategies
{
    public class NullAIStrategy : INPCAIStrategy
    {
        // This AI does absolutely nothing. Meant to be used by static NPCs like treasure chests, shopkeepers or quest givers.

        public int GetActionWeight(ActionWithEffects action, Map map, EffectCallerParams args) => 0;
        public int GetEffectWeight(Effect effect, Map map, EffectCallerParams args) => 0;
    }
}
