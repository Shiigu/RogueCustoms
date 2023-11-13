using D20Tek.DiceNotation.DieRoller;
using RogueCustomsGameEngine.Game.DungeonStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Utils.DiceNotation
{
    public class LCGDieRoller : RandomDieRollerBase
    {
        private readonly RngHandler Rng;
        public LCGDieRoller(RngHandler rng, IAllowRollTrackerEntry tracker = null)
            : base(tracker)
        {
            this.Rng = rng;
        }

        protected override int GetNextRandom(int sides)
        {
            return Rng.Next(sides) + 1;
        }
    }
}
