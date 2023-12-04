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

        private int _seed;

        public RngHandler(int seed)
        {
            Seed = seed;
        }

        public int Seed
        {
            get { return _seed; }
            set
            {
                _seed = value;
            }
        }

        public int Next()
        {
            _seed = (_seed * DefaultMultiplier + DefaultIncrement) % DefaultModulus;
            return _seed;
        }

        public int Next(int maxValue)
        {
            return Math.Abs(Next()) % maxValue;
        }

        public int Next(int minValue, int maxValue)
        {
            return Math.Abs(Next()) % (maxValue - minValue) + minValue;
        }

        public int NextInclusive(int maxValue)
        {
            return Math.Abs(Next()) % (maxValue + 1);
        }

        public int NextInclusive(int minValue, int maxValue)
        {
            return Math.Abs(Next()) % (maxValue - minValue + 1) + minValue;
        }
        public int RollProbability()
        {
            return NextInclusive(1, 100);
        }
        public override string ToString() => $"Seed: {Seed}";
    }
}
