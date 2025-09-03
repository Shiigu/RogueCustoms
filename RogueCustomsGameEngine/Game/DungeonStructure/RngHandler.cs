using RogueCustomsGameEngine.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Game.DungeonStructure
{
    [Serializable]
    public class RngHandler
    {
        private const int DefaultMultiplier = 1664525;
        private const int DefaultIncrement = 1013904223;
        private const int DefaultModulus = int.MaxValue;

        public RngHandler(int seed)
        {
            Seed = seed;
        }

        public int Seed { get; set; }

        public int Next()
        {
            Seed = (Seed * DefaultMultiplier + DefaultIncrement) % DefaultModulus;
            return Seed;
        }

        public int Next(int maxValue)
        {
            return Math.Abs(Next()) % maxValue;
        }

        public int Next(int minValue, int maxValue)
        {
            if (maxValue < minValue)
            {
                throw new ArgumentException("maxValue must be greater than or equal to minValue");
            }

            return Math.Abs(Next()) % (maxValue - minValue) + minValue;
        }

        public int NextInclusive(int maxValue)
        {
            return Math.Abs(Next()) % (maxValue + 1);
        }

        public int NextInclusive(int minValue, int maxValue)
        {
            if(maxValue < minValue)
            {
                throw new ArgumentException("maxValue must be greater than or equal to minValue");
            }

            return Math.Abs(Next()) % (maxValue - minValue + 1) + minValue;
        }
        public int RollProbability()
        {
            return NextInclusive(1, 100);
        }
        public override string ToString() => $"Seed: {Seed}";
    }
}
