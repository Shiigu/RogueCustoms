using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RogueCustomsGameEngine.Game.Entities.Interfaces;
using RogueCustomsGameEngine.Utils.Enums;

namespace RogueCustomsGameEngine.Game.Entities.NPCAIStrategies
{
    public class NPCAIStrategyFactory
    {
        private static Dictionary<AIType, INPCAIStrategy> NPCAIStrategyDictionary => new Dictionary<AIType, INPCAIStrategy>
        {
            { AIType.Default, new DefaultNPCAIStrategy() },
            { AIType.Random, new RandomNPCAIStrategy() },
            { AIType.CostEfficient, new CostEfficientNPCAIStrategy() },
            { AIType.AllOut, new AllOutNPCAIStrategy() }
        };

        public static INPCAIStrategy GetNPCAIStrategy(AIType type)
        {
            return NPCAIStrategyDictionary.GetValueOrDefault(type) ?? new DefaultNPCAIStrategy();
        }
    }
}
