using RogueCustomsGameEngine.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsGameEngine.Game.DungeonStructure
{
    public class RngHandler
    {
        private readonly Random Rng;
        public int Seed { get; set; }

        private int _rngCalls;
        public int RngCalls {
            get { return _rngCalls; }
            set {
                if (_rngCalls < value)
                {
                    for (int i = 0; i < (value - RngCalls); i++)
                    {
                        Next(1);    // Move the RNG as many times as needed
                    }
                }
                _rngCalls = value;
            }
        }

        public RngHandler() { }

        public RngHandler(int seed)
        {
            Rng = new Random(seed);
            _rngCalls = 0;
        }

        public virtual int Next()
        {
            var randValue = Rng.Next();
            _rngCalls++;
            return randValue;
        }

        public virtual int Next(int maxValue)
        {
            var randValue = Rng.Next(maxValue);
            _rngCalls++;
            return randValue;
        }

        public virtual int Next(int minValue, int maxValue)
        {
            var randValue = Rng.Next(minValue, maxValue);
            _rngCalls++;
            return randValue;
        }

        public virtual int NextInclusive(int maxValue)
        {
            var randValue = Rng.NextInclusive(maxValue);
            _rngCalls++;
            return randValue;
        }

        public virtual int NextInclusive(int minValue, int maxValue)
        {
            var randValue = Rng.NextInclusive(minValue, maxValue);
            _rngCalls++;
            return randValue;
        }

        public virtual long NextInt64()
        {
            var randValue = Rng.NextInt64();
            _rngCalls++;
            return randValue;
        }

        public virtual long NextInt64(int maxValue)
        {
            var randValue = Rng.NextInt64(maxValue);
            _rngCalls++;
            return randValue;
        }

        public virtual long NextInt64(int minValue, int maxValue)
        {
            var randValue = Rng.NextInt64(minValue, maxValue);
            _rngCalls++;
            return randValue;
        }

        public virtual float NextSingle()
        {
            var randValue = Rng.NextSingle();
            _rngCalls++;
            return randValue;
        }

        public virtual double NextDouble()
        {
            var randValue = Rng.NextDouble();
            _rngCalls++;
            return randValue;
        }
    }
}
