﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.DungeonStructure;
using RogueCustomsGameEngine.Game.Entities.Interfaces;

namespace RogueCustomsGameEngine.Game.Entities.NPCAIStrategies
{
    public class RandomNPCAIStrategy : INPCAIStrategy
    {
        public int GetActionWeight(ActionWithEffects action, Map map, Entity This, NonPlayableCharacter Source, ITargetable Target)
        {
            // Very heavily discourage NPCs from using actions that cannot be applied on the target in the current turn.
            if (!action.CanBeUsedOn(Target, Source))
                return int.MinValue;

            return Target != Source || map.Rng.RollProbability() <= Source.AIOddsToUseActionsOnSelf ? 1000 : 1;
        }

        public int GetEffectWeight(Effect effect, Map map, Entity This, NonPlayableCharacter Source, ITargetable Target) => 0;
    }
}
