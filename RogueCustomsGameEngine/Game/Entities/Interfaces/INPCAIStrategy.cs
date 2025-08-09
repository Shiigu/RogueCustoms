using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Utils.Effects.Utils;

namespace RogueCustomsGameEngine.Game.Entities.Interfaces
{
    public interface INPCAIStrategy
    {
        int GetActionWeight(ActionWithEffects action, Map map, EffectCallerParams args);
        int GetEffectWeight(Effect effect, Map map, EffectCallerParams args);
    }
}
